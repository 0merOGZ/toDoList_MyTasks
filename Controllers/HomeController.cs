using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using tod.Models;
using Microsoft.AspNetCore.Http;

namespace tod.Controllers;

public class HomeController : Controller
{
    private TodoContext context;

    public HomeController(TodoContext ctx) => context = ctx;

    public IActionResult Index(string id, string sort, string dueIn)
    {
        // Kullanıcı oturum bilgisi oku
        var userName = HttpContext.Session.GetString("userName");
        var userRole = HttpContext.Session.GetString("userRole");
        var teamName = HttpContext.Session.GetString("teamName");
        var teamInvitationCode = HttpContext.Session.GetString("teamInvitationCode");
        ViewBag.UserName = userName;
        ViewBag.UserRole = userRole;
        ViewBag.TeamName = teamName;
        ViewBag.TeamInvitationCode = teamInvitationCode;
        ViewBag.IsLoggedIn = !string.IsNullOrEmpty(userName);

        var filters = new Filters(id);
        ViewBag.Filters = filters;
        ViewBag.Categories = context.Categories.ToList();
        ViewBag.Statuses = context.Statuses.ToList();
        ViewBag.DueFilters = Filters.dueFilterValues;
        ViewBag.Sort = sort;

        IQueryable<Todo> query = context.Todos.
        Include(t => t.Category).
        Include(t => t.Status)
        .Include(t => t.User)
        .Where(t => !t.IsArchived);

        // Kullanıcıya göre filtreleme
        if (userRole == "Team Member")
        {
            // Sadece kendi görevleri
            var userIdStr = HttpContext.Session.GetString("userId");
            if (!string.IsNullOrEmpty(userIdStr))
            {
                int userId = int.Parse(userIdStr);
                query = query.Where(t => t.userId == userId);
            }
        }
        else if (userRole == "Team Leader")
        {
            // Kendi takımındaki üyelerin sadece 'work' kategorisindeki görevleri
            var teamIdStr = HttpContext.Session.GetString("teamId");
            if (!string.IsNullOrEmpty(teamIdStr))
            {
                int teamId = int.Parse(teamIdStr);
                query = query.Where(t => t.User != null && t.User.teamId == teamId && t.categoryId == "work");
            }
        }

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
        string dueInValue = filters.dueIn ?? "all";
        if (!string.IsNullOrEmpty(dueInValue) && dueInValue != "all")
        {
            if (int.TryParse(dueInValue, out int days))
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
    public IActionResult Archive()
    {
        var userRole = HttpContext.Session.GetString("userRole");
        var userName = HttpContext.Session.GetString("userName");
        var teamName = HttpContext.Session.GetString("teamName");
        ViewBag.UserName = userName;
        ViewBag.UserRole = userRole;
        ViewBag.TeamName = teamName;
        ViewBag.IsLoggedIn = !string.IsNullOrEmpty(userName);

        IQueryable<Todo> query = context.Todos
            .Include(t => t.Category)
            .Include(t => t.Status)
            .Include(t => t.User)
            .Where(t => t.IsArchived);

        if (userRole == "Team Member")
        {
            var userIdStr = HttpContext.Session.GetString("userId");
            if (!string.IsNullOrEmpty(userIdStr))
            {
                int userId = int.Parse(userIdStr);
                query = query.Where(t => t.userId == userId);
            }
        }
        else if (userRole == "Team Leader")
        {
            var teamIdStr = HttpContext.Session.GetString("teamId");
            if (!string.IsNullOrEmpty(teamIdStr))
            {
                int teamId = int.Parse(teamIdStr);
                query = query.Where(t => t.User != null && t.User.teamId == teamId);
            }
        }

        var archivedTasks = query.ToList();
        return View(archivedTasks);
    }

    [HttpGet]
    public IActionResult Add()
    {
        ViewBag.Categories = context.Categories.ToList();
        ViewBag.Statuses = context.Statuses.ToList();
        var task = new Todo { statusId = "pending"};
        // Eğer kullanıcı liderse takım üyelerini gönder
        var userRole = HttpContext.Session.GetString("userRole");
        var teamIdStr = HttpContext.Session.GetString("teamId");
        if (userRole == "Team Leader" && !string.IsNullOrEmpty(teamIdStr))
        {
            int teamId = int.Parse(teamIdStr);
            var members = context.Users.Where(u => u.teamId == teamId && u.userRole == "Team Member").ToList();
            ViewBag.TeamMembers = members;
        }
        else
        {
            ViewBag.TeamMembers = null;
        }
        return View(task); 
    }

    [HttpPost]
    public IActionResult Add(Todo task, int? assignedUserId) 
    {
        if (ModelState.IsValid)
        {
            var userRole = HttpContext.Session.GetString("userRole");
            if (userRole == "Team Leader" && assignedUserId.HasValue)
            {
                task.userId = assignedUserId.Value;
                task.categoryId = "work"; // Otomatik work kategorisi
            }
            else
            {
                // Kendi görevini ekleyen üye ise
                var userIdStr = HttpContext.Session.GetString("userId");
                if (!string.IsNullOrEmpty(userIdStr))
                {
                    task.userId = int.Parse(userIdStr);
                }
                // categoryId member'ın seçimine bırakılıyor
            }
            context.Todos.Add(task);
            context.SaveChanges();
            return RedirectToAction("Index");
        }
        else
        {
            ViewBag.Categories = context.Categories.ToList();
            ViewBag.Statuses = context.Statuses.ToList();
            // TeamMembers tekrar gönder
            var userRole = HttpContext.Session.GetString("userRole");
            var teamIdStr = HttpContext.Session.GetString("teamId");
            if (userRole == "Team Leader" && !string.IsNullOrEmpty(teamIdStr))
            {
                int teamId = int.Parse(teamIdStr);
                var members = context.Users.Where(u => u.teamId == teamId && u.userRole == "Team Member").ToList();
                ViewBag.TeamMembers = members;
            }
            else
            {
                ViewBag.TeamMembers = null;
            }
            return View(task);
        }
    }

    [HttpPost]
    public IActionResult Filter(string[] filter, string sort)
    {
        // filter dizisi artık 4 elemanlı olmalı
        while (filter.Length < 4) {
            filter = filter.Append("all").ToArray();
        }
        string id = string.Join("-", filter);
        return RedirectToAction("Index", new { id = id, sort = sort });
    }

    [HttpPost]
    public IActionResult DeleteTask(int todId, string id)
    {
        var toDelete = context.Todos.FirstOrDefault(t => t.todId == todId);
        if (toDelete != null)
        {
            context.Todos.Remove(toDelete);
            context.SaveChanges();
        }
        return RedirectToAction("Index", new { id = id });
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
    public IActionResult Unmark(int todId, string id)
    {
        var task = context.Todos.FirstOrDefault(task => task.todId == todId);
        if (task != null)
        {
            task.statusId = "pending";
            context.Entry(task).Reference(t => t.Status).IsModified = true;
            context.SaveChanges();
            context.Entry(task).Reload();
        }
        return RedirectToAction("Index", new { id = id });
    }

    [HttpPost]
    public IActionResult DeleteComplete(string id)
    {
        var toArchive = context.Todos.Where(t => t.statusId == "completed" && !t.IsArchived).ToList();
        foreach (var task in toArchive) 
        {
            task.IsArchived = true;
            task.ArchivedDate = DateTime.Now;
        }
        context.SaveChanges(); 
        return RedirectToAction("Index", new { ID=id });
    }

    [HttpPost]
    public IActionResult SignIn(string userName, string mail, string userRole, string password, string? invitationCode)
    {
        string dbRole = userRole == "erkek" ? "Team Leader" : "Team Member";
        if (dbRole == "Team Leader")
        {
            // Önce kullanıcıyı oluştur ve kaydet
            var user = new User
            {
                userName = userName,
                userMail = mail,
                userPassword = password,
                userRole = dbRole
                // teamId daha sonra atanacak
            };
            context.Users.Add(user);
            context.SaveChanges();

            // Sonra takımı oluştur, lider olarak bu kullanıcıyı ata
            var team = new Team
            {
                teamName = userName + "'s Team",
                teamInvitationCode = new Random().Next(10000, 99999).ToString(),
                teamLeader = user.userId
            };
            context.Teams.Add(team);
            context.SaveChanges();

            // Kullanıcıya takımId'yi ata ve kaydet
            user.teamId = team.teamId;
            context.SaveChanges();
        }
        else if (dbRole == "Team Member")
        {
            // Sadece kullanıcıyı ekle, takıma katılım JoinTeam ile olacak
            var user = new User
            {
                userName = userName,
                userMail = mail,
                userPassword = password,
                userRole = dbRole
                // teamId yok
            };
            context.Users.Add(user);
            context.SaveChanges();
        }
        return RedirectToAction("Index");
    }

    public IActionResult SignIn()
    {
        return View();
    }

    [HttpPost]
    public IActionResult LogIn(string mail, string password)
    {
        var user = context.Users.Include(u => u.Team).FirstOrDefault(u => u.userMail == mail && u.userPassword == password);
        if (user != null)
        {
            HttpContext.Session.SetString("userId", user.userId.ToString());
            HttpContext.Session.SetString("userName", user.userName);
            HttpContext.Session.SetString("userRole", user.userRole);
            HttpContext.Session.SetString("teamId", user.teamId?.ToString() ?? "");
            HttpContext.Session.SetString("teamName", user.Team?.teamName ?? "");
            HttpContext.Session.SetString("teamInvitationCode", user.Team?.teamInvitationCode ?? "");
        }
        return RedirectToAction("Index");
    }

    public IActionResult LogOut()
    {
        HttpContext.Session.Clear();
        return RedirectToAction("Index");
    }

    [HttpPost]
    public IActionResult JoinTeam(string invitationCode)
    {
        var userIdStr = HttpContext.Session.GetString("userId");
        if (string.IsNullOrEmpty(userIdStr))
        {
            return RedirectToAction("Index");
        }
        int userId = int.Parse(userIdStr);
        var user = context.Users.FirstOrDefault(u => u.userId == userId);
        if (user == null || user.userRole != "Team Member")
        {
            return RedirectToAction("Index");
        }
        var team = context.Teams.FirstOrDefault(t => t.teamInvitationCode == invitationCode);
        if (team == null)
        {
            TempData["JoinTeamError"] = "Invitation code is invalid.";
            return RedirectToAction("Index");
        }
        user.teamId = team.teamId;
        context.SaveChanges();
        // Session güncelle
        HttpContext.Session.SetString("teamId", team.teamId.ToString());
        HttpContext.Session.SetString("teamName", team.teamName);
        HttpContext.Session.SetString("teamInvitationCode", team.teamInvitationCode);
        return RedirectToAction("Index");
    }
}
