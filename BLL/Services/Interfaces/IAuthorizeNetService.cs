using BLL.DTO;
using DAL.Models;
using DAL.Models.Users;

namespace BLL.Services.Interfaces
{
    public interface IAuthorizeNetService
    {
        Task<string> CreateSubscriptionAsync(PaymentDetailsDto dto, SubscriptionPlan plan, User user);
        Task CancelSubscriptionAsync(string subscriptionId);
    }
}
