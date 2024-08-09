using AutoMapper;
using BLL.DTO.SubscriptionPlan;
using BLL.Services.Interfaces;
using KinopoiskWeb.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace KinopoiskWeb.Pages.Subscriptions
{
    public class SubscriptionPlanModel : PageModel
    {
        private readonly ISubscriptionPlanService _subscriptionPlanService;
        private readonly IMapper _mapper;

        public SubscriptionPlanModel(ISubscriptionPlanService subscriptionPlanService, IMapper mapper)
        {
            _subscriptionPlanService = subscriptionPlanService;
            _mapper = mapper;
        }

        public IList<SubscriptionPlanVM> SubscriptionPlans { get; set; }

        [BindProperty]
        public SubscriptionPlanVM NewPlan { get; set; }

        [BindProperty]
        public SubscriptionPlanVM EditPlan { get; set; }

        public async Task OnGetAsync()
        {
            var plans =  _subscriptionPlanService.GetAll();
            SubscriptionPlans = _mapper.Map<IList<SubscriptionPlanVM>>(plans);
        }

        public async Task<IActionResult> OnPostCreateOrUpdateAsync(int id)
        {
            if(id == 0)
            {
                await _subscriptionPlanService.CreateAsync(_mapper.Map<AddSubscriptionPlanDto>(NewPlan));
                return RedirectToPage();
            }
            else
            {
                EditPlan.Id = id;
                await _subscriptionPlanService.UpdateAsync(_mapper.Map<EditSubscriptionPlanDto>(EditPlan));

                return RedirectToPage();
            }
        }

        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            await _subscriptionPlanService.DeleteAsync(id);
            return RedirectToPage();
        }
    }
}
