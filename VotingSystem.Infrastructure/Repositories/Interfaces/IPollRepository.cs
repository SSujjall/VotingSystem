using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VotingSystem.Domain.Entities;

namespace VotingSystem.Infrastructure.Repositories.Interfaces
{
    public interface IPollRepository : IBaseRepository<Poll>
    {
        Task<List<Poll>> GetAllPolls();
        Task<Poll> GetPollById(int pollId);
    }
}
