using BLL.DTO;
using DAL.Models;

namespace BLL.Services.Interfaces
{
    public interface ISubscriptionService
    {
        Task<string> CreateSubscriptionAsync(PaymentDetailsDto dto);
        Task CancelSubscriptionAsync(string subscriptionId);
        Task<GetSubscriptionDto> GetSubscriptionByUserIdAsync(string userId);
        bool IsSubscriptionActive(string userId);
    }

}
