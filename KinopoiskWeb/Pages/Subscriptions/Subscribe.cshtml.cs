using BLL.Services.Interfaces;
using KinopoiskWeb.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;

namespace KinopoiskWeb.Pages.Subscriptions
{
    public class SubscribeModel : PageModel
    {
        private readonly ISubscriptionService _subscriptionService;

        public SubscribeModel(ISubscriptionService subscriptionService)
        {
            _subscriptionService = subscriptionService;
        }

        [BindProperty]
        public PaymentDetailsVM Details { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            Details.UserId = userId;

            var subscriptionId = await _subscriptionService.CreateSubscriptionAsync(userId, amount, cardNumber, expirationDate, cardCode, billingInterval, billingUnit);

            return RedirectToPage("Index");
        }
    }
}
