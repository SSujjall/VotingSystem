using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VotingSystem.Domain.Entities;

namespace VotingSystem.Infrastructure.Repositories.Interfaces
{
    public interface IVoteRepository : IBaseRepository<Vote>
    {
        Task<bool> HasUserVoted(string userId, int pollId);
        Task<List<Vote>> GetUserVotes(string userId);
        Task<Vote?> GetVoteByUserAndPoll(string userId, int pollId);
    }
}
