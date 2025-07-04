using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace tod.Models
{
    public class User
    {
        [Key]
        public int userId { get; set; }

        [Required]
        public string userName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string userMail { get; set; } = string.Empty;

        [Required]
        public string userPassword { get; set; } = string.Empty;

        [Required]
        public string userRole { get; set; } = string.Empty; // "üye" veya "lider"

        public int? teamId { get; set; } // Nullable FK
        [ForeignKey("teamId")]
        public Team? Team { get; set; }
    }
} 