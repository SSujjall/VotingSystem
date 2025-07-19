using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VotingSystem.Domain.Entities;
using VotingSystem.Infrastructure.Repositories.Interfaces;

namespace VotingSystem.Infrastructure.Repositories.Implementations
{
    public class AuthRepository : IAuthRepository
    {
        private readonly UserManager<User> _userManager;

        public AuthRepository(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public async Task<bool> CreateNewUser(User user, string password)
        {
            var result = await _userManager.CreateAsync(user, password);
            return result.Succeeded ? true : false;
        }

        public async Task<User> FindByUsername(string username)
        {
            var userExists = await _userManager.FindByNameAsync(username);
            return userExists;
        }

        public async Task<string> GetUserRole(User user)
        {
            var userRoles = await _userManager.GetRolesAsync(user);
            return userRoles.FirstOrDefault();
        }

        public async Task<bool> UsernameExists(string username)
        {
            var userExists = await _userManager.FindByNameAsync(username);
            return userExists != null ? true : false;
        }

        public async Task<bool> EmailExists(string email)
        {
            var emailExists = await _userManager.FindByEmailAsync(email);
            return emailExists != null ? true : false;
        }
    }
}
