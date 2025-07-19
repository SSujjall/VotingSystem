using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VotingSystem.Domain.Entities
{
    public class PollOption
    {
        [Key]
        public int PollOptionId { get; set; }

        [ForeignKey(nameof(Poll))]
        public int PollId { get; set; }
        public string OptionText { get; set; } = string.Empty;
        public int VoteCount { get; set; } = 0;
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // Navigation properties
        public virtual Poll Poll { get; set; } = null!;
        public virtual ICollection<Vote> Votes { get; set; } = new List<Vote>();
    }
}
