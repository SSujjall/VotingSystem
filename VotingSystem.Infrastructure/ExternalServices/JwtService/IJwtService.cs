using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using VotingSystem.Domain.Entities;

namespace VotingSystem.Infrastructure.ExternalServices.JwtService
{
    public interface IJwtService
    {
        Task<string> GenerateJwtToken(User user);
    }
}
