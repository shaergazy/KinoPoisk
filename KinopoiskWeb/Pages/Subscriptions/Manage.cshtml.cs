using AutoMapper;
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
        private readonly IMapper _mapper;

        public SubscriptionVM Subscription { get; set; }

        public ManageModel(ISubscriptionService subscriptionService, IMapper mapper)
        {
            _subscriptionService = subscriptionService;
            _mapper = mapper;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            Subscription = _mapper.Map<SubscriptionVM>(await _subscriptionService.GetSubscriptionByUserIdAsync(userId));

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
