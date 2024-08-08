using BLL.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace KinopoiskWeb.Pages
{
    public class PaymentModel : PageModel
    {
        private readonly IAuthorizeNetService _authorizeNetService;

        public PaymentModel(IAuthorizeNetService authorizeNetService)
        {
            _authorizeNetService = authorizeNetService;
        }

        [BindProperty]
        public decimal Amount { get; set; }

        [BindProperty]
        public string CardNumber { get; set; }

        [BindProperty]
        public string ExpirationDate { get; set; }

        [BindProperty]
        public string CardCode { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                var transactionId = await _authorizeNetService.ProcessPaymentAsync(Amount, CardNumber, ExpirationDate, CardCode);
                ViewData["Message"] = "Payment successful! Transaction ID: " + transactionId;
            }
            catch (Exception ex)
            {
                ViewData["Message"] = "Payment failed: " + ex.Message;
            }

            return Page();
        }
    }
}
