using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VotingSystem.Infrastructure.ExternalServices.EmailService.Models;

namespace VotingSystem.Infrastructure.ExternalServices.EmailService
{
    public interface IEmailService
    {
        Task SendEmailAsync(EmailMessage message);
    }
}
