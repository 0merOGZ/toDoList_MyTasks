using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.EntityFrameworkCore;

namespace tod.Models {

    public class TodoContext : DbContext {
 
         public TodoContext(DbContextOptions<TodoContext> options) : base(options) {}

         public DbSet<Todo> Todos { get; set; } = null!;
         public DbSet<Category> Categories { get; set; } = null!;
         public DbSet<Status> Statuses { get; set; } = null!;
         public DbSet<Team> Teams { get; set; } = null!;
         public DbSet<User> Users { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            modelBuilder.Entity<Category>().HasData(
                new Category { categoryId = "personal", categoryName = "Personal" },
                new Category { categoryId = "work", categoryName = "Work" },
                new Category { categoryId = "school", categoryName = "School" },
                new Category { categoryId = "relationship", categoryName = "Relationship" },
                new Category { categoryId = "health", categoryName = "Health" },
                new Category { categoryId = "finance", categoryName = "Finance" },
                new Category { categoryId = "other", categoryName = "Other" }
            );

            modelBuilder.Entity<Status>().HasData(
                new Status { statusId = "pending", statusName = "Pending" },
                new Status { statusId = "completed", statusName = "Completed" }
            );

            // Team-Leader (User) ilişkisi
            modelBuilder.Entity<Team>()
                .HasOne(t => t.Leader)
                .WithMany()
                .HasForeignKey(t => t.teamLeader)
                .OnDelete(DeleteBehavior.Restrict);

            // Team-Members ilişkisi (bir takımın birden fazla üyesi olabilir)
            modelBuilder.Entity<User>()
                .HasOne(u => u.Team)
                .WithMany(t => t.Members)
                .HasForeignKey(u => u.teamId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}