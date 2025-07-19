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
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        public UserRepository(AppDbContext dbContext) : base(dbContext)
        {
        }
    }
}
