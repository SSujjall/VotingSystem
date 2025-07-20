using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VotingSystem.Domain.Entities;
using VotingSystem.Infrastructure.Data;
using VotingSystem.Infrastructure.Repositories.Interfaces;

namespace VotingSystem.Infrastructure.Repositories.Implementations
{
    public class VoteRepository : BaseRepository<Vote>, IVoteRepository
    {
        public VoteRepository(AppDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<List<Vote>> GetUserVotes(string userId)
        {
            return await _dbContext.Votes
                .Include(v => v.Poll)
                .Include(v => v.PollOption)
                .Where(v => v.UserId == userId)
                .ToListAsync();
        }

        public async Task<Vote?> GetVoteByUserAndPoll(string userId, int pollId)
        {
            return await _dbContext.Votes
                .Include(v => v.PollOption)
                .FirstOrDefaultAsync(v => v.UserId == userId && v.PollId == pollId);
        }

        public async Task<bool> HasUserVoted(string userId, int pollId)
        {
            return await _dbContext.Votes
                .AnyAsync(v => v.UserId == userId && v.PollId == pollId);
        }
    }
}
