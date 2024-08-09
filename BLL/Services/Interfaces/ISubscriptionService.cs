using BLL.DTO;
using DAL.Models;

namespace BLL.Services.Interfaces
{
    public interface ISubscriptionService
    {
        Task<string> CreateSubscriptionAsync(PaymentDetailsDto dto);
        Task CancelSubscriptionAsync(string subscriptionId);
        Task<Subscription> GetSubscriptionByUserIdAsync(string userId);
        Task UpdateSubscriptionStatusAsync(string subscriptionId, string newStatus);
        bool IsSubscriptionActive(string userId);
    }

}
