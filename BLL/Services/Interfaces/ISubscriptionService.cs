using DAL.Models;

namespace BLL.Services.Interfaces
{
    public interface ISubscriptionService
    {
        Task<string> CreateSubscriptionAsync(string userId, decimal amount, string cardNumber, string expirationDate, string cardCode, short billingInterval, string billingUnit);
        Task CancelSubscriptionAsync(string subscriptionId);
        Task<Subscription> GetSubscriptionByUserIdAsync(string userId);
        Task UpdateSubscriptionStatusAsync(string subscriptionId, string newStatus);
        bool IsSubscriptionActive(string userId);
    }

}
