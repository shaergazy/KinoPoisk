using AutoMapper;
using BLL.DTO;
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
        private readonly IMapper _mapper;

        public SubscribeModel(ISubscriptionService subscriptionService, IMapper mapper)
        {
            _subscriptionService = subscriptionService;
            _mapper = mapper;
        }

        [BindProperty]
        public PaymentDetailsVM Details { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            Details.UserId = userId;

            var subscriptionId = await _subscriptionService.CreateSubscriptionAsync(_mapper.Map<PaymentDetailsDto>(Details));

            return RedirectToPage("Index");
        }
    }
}
