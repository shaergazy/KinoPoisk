using BLL.Services.Interfaces;
using Common.Helpers;
using DAL.Models.Users;
using DTO;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;

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

        public async Task SendWelcomeEmailAsync(User user)
        {
            var subject = ResourceHelper.GetString("WelcomeEmailSubject");
            var body = string.Format(ResourceHelper.GetString("WelcomeEmailBody"), user.UserName, "Kinopoiskweb");

            await SendEmailAsync(user.Email, subject, body);
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
                    _logger.LogInformation("Attempting to send email with subject '{Subject}' to '{ToEmail}'.", subject, toEmail);

                    await client.ConnectAsync(_emailSettings.SmtpServer, _emailSettings.Port, SecureSocketOptions.StartTls);
                    await client.AuthenticateAsync(_emailSettings.UserName, _emailSettings.Password);
                    await client.SendAsync(emailMessage);

                    _logger.LogInformation("Email successfully sent to {Email} with subject '{Subject}'.", toEmail, subject);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error sending email to {Email} with subject '{Subject}'.", toEmail, subject);
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
