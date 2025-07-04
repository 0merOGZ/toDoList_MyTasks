using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace tod.Models
{
    public class Team
    {
        [Key]
        public int teamId { get; set; }

        [Required]
        public string teamName { get; set; } = string.Empty;

        [Required]
        public string teamInvitationCode { get; set; } = string.Empty;

        [Required]
        public int teamLeader { get; set; } // userId FK
        [ForeignKey("teamLeader")]
        public User? Leader { get; set; }

        public ICollection<User>? Members { get; set; }
    }
} 