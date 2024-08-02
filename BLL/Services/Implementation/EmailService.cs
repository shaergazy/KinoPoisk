using BLL.Services.Interfaces;
using DTO;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;
using System.Net.Security;

namespace BLL.Services.Implementation
{
    public class EmailService : IEmailService
    {
        private readonly SettingsDto.Mail _emailSettings;
        private readonly ILogger<EmailService> _logger;

        public EmailService(IOptions<SettingsDto.Mail> emailSettings, ILogger<EmailService> logger)
        {
            _emailSettings = emailSettings.Value;
            _logger = logger;
        }

        public async Task SendEmailAsync(string toEmail, string subject, string body)
        {
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress(_emailSettings.DisplayName, _emailSettings.UserName));
            emailMessage.To.Add(MailboxAddress.Parse(toEmail));
            emailMessage.Subject = subject;

            var bodyBuilder = new BodyBuilder
            {
                HtmlBody = body
            };

            emailMessage.Body = bodyBuilder.ToMessageBody();

            using (var client = new SmtpClient())
            {
                try
                {
                    await client.ConnectAsync(_emailSettings.SmtpServer, _emailSettings.Port, SecureSocketOptions.StartTls);
                    await client.AuthenticateAsync(_emailSettings.UserName, _emailSettings.Password);
                    await client.SendAsync(emailMessage);
                    _logger.LogInformation("Email sent to {Email}", toEmail);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error sending email to {Email}", toEmail);
                    throw;
                }
                finally
                {
                    await client.DisconnectAsync(true);
                }
            }
        }
    }
}
