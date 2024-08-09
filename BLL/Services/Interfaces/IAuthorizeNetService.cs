using BLL.DTO;
using DAL.Models.Users;

namespace BLL.Services.Interfaces
{
    public interface IAuthorizeNetService
    {
        Task<string> CreateSubscriptionAsync(PaymentDetailsDto dto);
        Task CancelSubscriptionAsync(string subscriptionId);
    }
}
