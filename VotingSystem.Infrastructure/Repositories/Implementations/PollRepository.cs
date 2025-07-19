using Microsoft.EntityFrameworkCore;
using VotingSystem.Domain.Entities;
using VotingSystem.Infrastructure.Data;
using VotingSystem.Infrastructure.Repositories.Interfaces;

namespace VotingSystem.Infrastructure.Repositories.Implementations
{
    public class PollRepository : BaseRepository<Poll>, IPollRepository
    {
        public PollRepository(AppDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<List<Poll>> GetAllPolls()
        {
            return await _dbContext.Polls
                .Include(p => p.Options)
                .ToListAsync();
        }

        public async Task<Poll> GetPollById(int pollId)
        {
            return await _dbContext.Polls
                .Include(p => p.Options)
                .FirstOrDefaultAsync(x => x.PollId == pollId);
        }
    }
}
