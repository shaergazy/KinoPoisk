using DAL.Models.Users;

namespace BLL.Services.Interfaces
{
    public interface IAuthorizeNetService
    {
        Task<string> CreateSubscriptionAsync(decimal amount, string cardNumber, string expirationDate, string cardCode, short billingInterval, string billingUnit, User user);
        Task CancelSubscriptionAsync(string subscriptionId);
    }
}
