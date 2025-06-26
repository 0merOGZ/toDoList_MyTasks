using System.ComponentModel.DataAnnotations;

namespace tod.Models
{
    public class Status
    {
        [Key]
        public string statusId{ get; set; } = string.Empty;
        public string statusName { get; set; } = string.Empty;
    }
}