﻿using MimeKit;
using VotingSystem.Infrastructure.ExternalServices.EmailService.Config;
using VotingSystem.Infrastructure.ExternalServices.EmailService.Models;

namespace VotingSystem.Infrastructure.ExternalServices.EmailService
{
    public class EmailService : IEmailService
    {
        private readonly EmailConfig _emailConfig;
        public EmailService(EmailConfig emailConfig) => _emailConfig = emailConfig;

        public async Task SendEmailAsync(EmailMessage message)
        {
            var emailMessage = CreateEmailMessage(message);
            await Send(emailMessage);
        }

        private MimeMessage CreateEmailMessage(EmailMessage message)
        {
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress("VotingSystem", _emailConfig.From));
            emailMessage.To.AddRange(message.To);
            emailMessage.Subject = message.Subject;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Text) { Text = message.Content };

            return emailMessage;
        }

        private async Task Send(MimeMessage mailMessage)
        {
            var client = new MailKit.Net.Smtp.SmtpClient();

            try
            {
                await client.ConnectAsync(_emailConfig.SmtpServer, _emailConfig.Port, true);
                client.AuthenticationMechanisms.Remove("XOAUTH2");
                await client.AuthenticateAsync(_emailConfig.Username, _emailConfig.Password);
                await client.SendAsync(mailMessage);
            }
            catch
            {
                throw new InvalidOperationException();
            }
            finally
            {
                await client.DisconnectAsync(true);
                client.Dispose();
            }
        }
    }
}
