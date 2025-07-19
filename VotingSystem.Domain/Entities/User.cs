
using Microsoft.AspNetCore.Identity;

namespace VotingSystem.Domain.Entities
{
    public class User : IdentityUser
    {
        public string FullName { get; set; }

        // Navigation Properties
        public virtual ICollection<Poll> CreatedPolls { get; set; } = new List<Poll>();
        public virtual ICollection<Vote> Votes { get; set; } = new List<Vote>();
    }
}
