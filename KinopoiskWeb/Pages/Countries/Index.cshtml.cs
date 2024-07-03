using AutoMapper;
using BLL.DTO.CountryDTOs;
using BLL.Services.Interfaces;
using KinopoiskWeb.ViewModels.CountryVM;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace KinopoiskWeb.Pages.Countries
{
    public class IndexModel : PageModel
    {
        private readonly ICountryService _service;
        private readonly IMapper _mapper;

        public IndexModel(ICountryService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        [BindProperty]
        public List<IndexCountryVM> Countries { get; set; }

        [BindProperty]
        public CreateCountryVM NewCountry { get; set; }

        [BindProperty]
        public EditCountryVM EditedCountry { get; set; }

        [BindProperty]
        public int CountryId { get; set; }

        public async Task OnGetAsync()
        {
            Countries = _mapper.Map<List<IndexCountryVM>>(await _service.GetAll());
        }

        public async Task<IActionResult> OnPostCreateAsync()
        {
            //if (!ModelState.IsValid)
            //{
            //    return RedirectToPage("/Error");
            //}

            await _service.CreateAsync(_mapper.Map<AddCountryDto>(NewCountry));

            return RedirectToPage();
        }

        public async Task<JsonResult> OnGetGetCountry(int id)
        {
            var country = await _service.GetById(id);
            if (country == null)
            {
                return new JsonResult(NotFound());
            }

            return new JsonResult(country);
        }

        public async Task<IActionResult> OnPostEditAsync()
        {
            //if (!ModelState.IsValid)
            //{
            //    return Page();
            //}

            var country = _service.GetById(EditedCountry.Id);
            if (country == null)
            {
                return NotFound();
            }

            await _service.UpdateAsync(_mapper.Map<EditCountryDto>(EditedCountry));

            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostDeleteAsync()
        {
            await _service.DeleteById(CountryId);

            return RedirectToPage();
        }
    }

}
