using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations.Schema;

namespace tod.Models {

    public class Todo {

        [Key]
        public int todId{ get; set; } 
        [Required(ErrorMessage = "Name is required")]
        public string todName { get; set; } = string.Empty;
        [Required(ErrorMessage = "Description is required")]
        public string todDescription { get; set; } = string.Empty;
        [Required(ErrorMessage = "Date is required")]
        public DateTime? dueDate { get; set; } 

        [Required(ErrorMessage = "Urgency is required")]
        public string urgency { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "Category is required")]
        public string categoryId { get; set; } = string.Empty;
        [ValidateNever] 
        public Category Category { get; set; } = null!;

        [Required(ErrorMessage = "Status is required")]
        public string statusId { get; set; } = string.Empty;   
        [ValidateNever]
        public Status Status { get; set; } = null!;

        public bool Overdue =>
            Status != null &&
            Status.statusId != null &&
            Status.statusId.ToLower() == "pending" &&
            dueDate.HasValue &&
            dueDate.Value < DateTime.Now;

        public bool IsArchived { get; set; } = false;
        public DateTime? ArchivedDate { get; set; }

        public int? userId { get; set; } // Nullable FK
        [ForeignKey("userId")]
        public User? User { get; set; }
    }
}