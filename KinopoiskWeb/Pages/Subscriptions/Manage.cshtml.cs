using BLL.Services.Interfaces;
using KinopoiskWeb.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;

namespace KinopoiskWeb.Pages.Subscriptions
{
    public class ManageModel : PageModel
    {
        private readonly ISubscriptionService _subscriptionService;

        public SubscriptionVM Subscription { get; set; }

        public ManageModel(ISubscriptionService subscriptionService)
        {
            _subscriptionService = subscriptionService;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var subscription = await _subscriptionService.GetSubscriptionByUserIdAsync(userId);

            if (subscription != null)
            {
                Subscription = new SubscriptionVM
                {
                    PlanName = subscription.Plan.Name,
                    Amount = subscription.Amount,
                    StartDate = subscription.StartDate,
                    EndDate = subscription.EndDate,
                    IsActive = subscription.IsActive,
                    NextBillingDate = subscription.NextBillingDate
                };
            }

            return Page();
        }

        public async Task<IActionResult> OnPostCancelAsync()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var subscription = await _subscriptionService.GetSubscriptionByUserIdAsync(userId);

            if (subscription != null)
            {
                await _subscriptionService.CancelSubscriptionAsync(subscription.SubscriptionId);
                return RedirectToPage();
            }

            return Page();
        }
    }
}
