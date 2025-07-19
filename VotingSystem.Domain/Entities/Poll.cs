using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VotingSystem.Domain.Entities
{
    public class Poll
    {
        [Key]
        public int PollId { get; set; }

        [ForeignKey(nameof(User))]
        public string CreatedBy { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? EndsAt { get; set; }

        // Navigation properties
        public virtual User User { get; set; } = null!;
        public virtual ICollection<PollOption> Options { get; set; } = new List<PollOption>();
        public virtual ICollection<Vote> Votes { get; set; } = new List<Vote>();
    }
}
