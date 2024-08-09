using BLL.Services.Interfaces;
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

        public async Task<IActionResult> OnPostAsync(string cardNumber, string expirationDate, string cardCode)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            decimal amount = 9.99m;
            short billingInterval = 1;
            string billingUnit = "months";

            var subscriptionId = await _subscriptionService.CreateSubscriptionAsync(userId, amount, cardNumber, expirationDate, cardCode, billingInterval, billingUnit);

            return RedirectToPage("Index");
        }
    }
}
