using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VotingSystem.Domain.Entities
{
    public class Vote
    {
        [Key]
        public int VoteId { get; set; }

        [ForeignKey(nameof(User))]
        public string UserId { get; set; } = string.Empty;

        [ForeignKey(nameof(Poll))]
        public int PollId { get; set; }

        [ForeignKey(nameof(PollOption))]
        public int PollOptionId { get; set; }
        public DateTime VotedAt { get; set; } = DateTime.Now;

        // Navigation properties
        public virtual User User { get; set; } = null!;
        public virtual Poll Poll { get; set; } = null!;
        public virtual PollOption PollOption { get; set; } = null!;
    }
}
