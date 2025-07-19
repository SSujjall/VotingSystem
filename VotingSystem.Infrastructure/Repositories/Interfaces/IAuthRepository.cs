using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VotingSystem.Domain.Entities;

namespace VotingSystem.Infrastructure.Repositories.Interfaces
{
    public interface IAuthRepository
    {
        public Task<User> FindByUsername(string username);
        public Task<bool> CreateNewUser(User user, string password);
        public Task<string> GetUserRole(User user);
        Task<bool> UsernameExists(string username);
        Task<bool> EmailExists(string email);
    }
}
