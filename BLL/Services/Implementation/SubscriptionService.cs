using BLL.Services.Interfaces;
using DAL.Models;
using Data.Repositories.RepositoryInterfaces;

namespace BLL.Services.Implementation
{
    public class SubscriptionService : ISubscriptionService
    {
        private readonly IUnitOfWork<Subscription, Guid> _uow;
        private readonly IAuthorizeNetService _authorizeNetService;

        public SubscriptionService(IUnitOfWork<Subscription, Guid> uow, IAuthorizeNetService authorizeNetService)
        {
            _uow = uow;
            _authorizeNetService = authorizeNetService;
        }

        public async Task<string> CreateSubscriptionAsync(string userId, decimal amount, string cardNumber, string expirationDate, string cardCode, short billingInterval, string billingUnit)
        {
            var user = await _uow.Users.FirstOrDefaultAsync(x => x.Id == userId);
            var subscriptionId = await _authorizeNetService.CreateSubscriptionAsync(amount, cardNumber, expirationDate, cardCode, billingInterval, billingUnit, user);

            var subscription = new Subscription
            {
                UserId = userId,
                SubscriptionId = subscriptionId,
                StartDate = DateTime.UtcNow,
                Amount = amount,
                IsActive = true,
                Plan = DAL.Enums.SubscriptionPlan.Monthly,
                NextBillingDate = DateTime.UtcNow.AddMonths(billingInterval)
            };

            await _uow.Subscriptions.AddAsync(subscription);
            await _uow.SaveChangesAsync();

            return subscriptionId;
        }

        public async Task CancelSubscriptionAsync(string subscriptionId)
        {
            await _authorizeNetService.CancelSubscriptionAsync(subscriptionId);

            var subscription = await _uow.Subscriptions.FirstOrDefaultAsync(s => s.SubscriptionId == subscriptionId);
            if (subscription != null)
            {
                subscription.IsActive = false;
                subscription.EndDate = DateTime.UtcNow;
                await _uow.SaveChangesAsync();
            }
        }

        public async Task<Subscription> GetSubscriptionByUserIdAsync(string userId)
        {
            return await _uow.Subscriptions
                .FirstOrDefaultAsync(s => s.UserId == userId && s.IsActive);
        }

        public async Task UpdateSubscriptionStatusAsync(string subscriptionId, string newStatus)
        {
            //var subscription = await _uow.Subscriptions.FirstOrDefaultAsync(s => s.SubscriptionId == subscriptionId);
            //if (subscription != null)
            //{
            //    subscription.IsActive = newStatus;
            //    if (newStatus == "Cancelled")
            //    {
            //        subscription.EndDate = DateTime.UtcNow;
            //    }

            //    await _uow.SaveChangesAsync();
            //}
        }

        public bool IsSubscriptionActive(string userId)
        {
            var subscription = _uow.Subscriptions
                .Where(s => s.UserId == userId && s.IsActive)
                .FirstOrDefault();

            return subscription != null;
        }
    }

}
