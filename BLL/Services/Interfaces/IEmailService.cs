using BLL.DTO;
using DAL.Models.Users;

namespace BLL.Services.Interfaces
{
    public interface IEmailService
    {
        Task SendEmailAsync(string toEmail, string subject, string body);
        Task SendWelcomeEmailAsync(User user);
    }
}
