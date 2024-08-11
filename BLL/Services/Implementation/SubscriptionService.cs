using AutoMapper;
using BLL.DTO;
using BLL.Services.Interfaces;
using DAL.Models;
using Data.Repositories.RepositoryInterfaces;
using Microsoft.EntityFrameworkCore;

namespace BLL.Services.Implementation
{
    public class SubscriptionService : ISubscriptionService
    {
        private readonly IUnitOfWork<Subscription, Guid> _uow;
        private readonly IAuthorizeNetService _authorizeNetService;
        private readonly IMapper _mapper;

        public SubscriptionService(IUnitOfWork<Subscription, Guid> uow, IAuthorizeNetService authorizeNetService, IMapper mapper)
        {
            _uow = uow;
            _authorizeNetService = authorizeNetService;
            _mapper = mapper;
        }

        public async Task<string> CreateSubscriptionAsync(PaymentDetailsDto dto)
        {
            var user = await _uow.Users.FirstOrDefaultAsync(x => x.Id == dto.UserId);
            var plan = await _uow.SubscriptionPlans.FirstOrDefaultAsync(x => x.Id == dto.PlanId);
            var subscriptionId = await _authorizeNetService.CreateSubscriptionAsync(dto, plan, user);

            var subscription = new Subscription
            {
                UserId = dto.UserId,
                SubscriptionId = subscriptionId,
                StartDate = DateTime.UtcNow,
                IsActive = true,
                SubscriptionPlanId = dto.PlanId,
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

        public async Task<GetSubscriptionDto> GetSubscriptionByUserIdAsync(string userId)
        {
            var subscription = await _uow.Subscriptions
                .Include(x => x.Plan)
                .FirstOrDefaultAsync(x => x.UserId == userId);
            return _mapper.Map<GetSubscriptionDto>(subscription);
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
