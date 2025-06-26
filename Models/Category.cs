using System.ComponentModel.DataAnnotations;

namespace tod.Models
{
    public class Category
    {
        [Key]
        public string categoryId{ get; set; } = string.Empty;
        public string categoryName { get; set; } = string.Empty;
    }
}