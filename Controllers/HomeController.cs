using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using tod.Models;

namespace tod.Controllers;

public class HomeController : Controller
{
    private TodoContext context;

    public HomeController(TodoContext ctx) => context = ctx;

    public IActionResult Index(string id, string sort, string dueIn)
    {
        var filters = new Filters(id);
        ViewBag.Filters = filters;
        ViewBag.Categories = context.Categories.ToList();
        ViewBag.Statuses = context.Statuses.ToList();
        ViewBag.DueFilters = Filters.dueFilterValues;
        ViewBag.Sort = sort;

        IQueryable<Todo> query = context.Todos.
        Include(t => t.Category).
        Include(t => t.Status);

        if (filters.HasCategory)
        {
            query = query.Where(t => t.categoryId == filters.categoryID); 
        }
        if (filters.HasStatus)
        {
            query = query.Where(t => t.statusId == filters.statusID); 
        }
        if (filters.HasDueDate)
        {
            var today = DateTime.Today;
            if(filters.IsPast)
            {
                query = query.Where(t => t.dueDate < today);
            }
            else if(filters.IsFuture)
            {
                query = query.Where(t => t.dueDate > today);
            }
            else if(filters.IsToday)
            {
                query = query.Where(t => t.dueDate == today);
            }
        }

        // Sıralama
        switch (sort)
        {
            case "date_asc":
                query = query.OrderBy(t => t.dueDate);
                break;
            case "date_desc":
                query = query.OrderByDescending(t => t.dueDate);
                break;
            case "name_asc":
                query = query.OrderBy(t => t.todName);
                break;
            case "name_desc":
                query = query.OrderByDescending(t => t.todName);
                break;
            case "category_asc":
                query = query.OrderBy(t => t.Category.categoryName);
                break;
            case "category_desc":
                query = query.OrderByDescending(t => t.Category.categoryName);
                break;
            default:
                query = query.OrderBy(t => t.dueDate);
                break;
        }

        var tasks = query.ToList();

        // Due In filtresi (bellekte uygula)
        if (!string.IsNullOrEmpty(dueIn) && dueIn != "all")
        {
            if (int.TryParse(dueIn, out int days))
            {
                var today = DateTime.Today;
                tasks = tasks
                    .Where(t => t.dueDate.HasValue && (t.dueDate.Value.Date - today).TotalDays <= days && (t.dueDate.Value.Date - today).TotalDays >= 0)
                    .ToList();
            }
        }

        // Tamamlanan ve toplam görev sayısı
        ViewBag.CompletedCount = tasks.Count(t => t.statusId == "completed");
        ViewBag.TotalCount = tasks.Count;

        return View(tasks);
    }

    [HttpGet]
    public IActionResult Add()
    {
        ViewBag.Categories = context.Categories.ToList();
        ViewBag.Statuses = context.Statuses.ToList();
        var task = new Todo { statusId = "pending"};
        return View(task); 
    }

    [HttpPost]
    public IActionResult Add(Todo task) 
    {
        if (ModelState.IsValid)
        {
            context.Todos.Add(task);
            context.SaveChanges();
            return RedirectToAction("Index");
        }
        else
        {
            ViewBag.Categories = context.Categories.ToList();
            ViewBag.Statuses = context.Statuses.ToList();
            return View(task);
        }
    }

    [HttpPost]
    public IActionResult Filter(string[] filter, string sort)
    {
        string id = string.Join("-", filter);
        return RedirectToAction("Index", new { ID = id, sort = sort });
    }

    [HttpPost]
    public IActionResult MarkComplete([FromRoute] string id, Todo selected)
    {
        selected = context.Todos.Find(selected.todId)!;

        if (selected != null)
        {
            selected.statusId = "completed";
            context.SaveChanges();
        }
        return RedirectToAction("Index", new { ID=id });
    }

    [HttpPost]
    public IActionResult MarkPending(int todId, string id)
    {
        var task = context.Todos.FirstOrDefault(task => task.todId == todId);
        if (task != null)
        {
            task.statusId = "pending";
            context.SaveChanges();
        }
        return RedirectToAction("Index", new { id = id });
    }


    [HttpPost]
    public IActionResult DeleteComplete(string id)
    {
        var toDelete = context.Todos.Where(t => t.statusId == "completed").ToList();
        foreach (var task in toDelete) 
        {
            context.Todos.Remove(task);
        }
        context.SaveChanges(); 
        return RedirectToAction("Index", new { ID=id });
    }

    public IActionResult SignIn()
    {
        return View();
    }
    public IActionResult LogIn()
    {
        return View();
    }
}
