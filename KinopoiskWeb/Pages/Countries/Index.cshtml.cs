using BLL.DTO;
using BLL.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace KinopoiskWeb.Pages.Countries
{
    public class IndexModel : PageModel
    {
        private readonly ICountryService _service;

        public IndexModel(ICountryService service)
        {
            _service = service;
        }

        [BindProperty]
        public List<CountryDto.Get> Countries { get; set; }

        [BindProperty]
        public CountryDto.Add NewCountry { get; set; }

        [BindProperty]
        public CountryDto.Edit EditedCountry { get; set; }

        [BindProperty]
        public CountryDto.Delete CountryToDelete { get; set; }

        public async Task OnGetAsync()
        {
            Countries = await _service.GetAll();
        }

        public async Task<IActionResult> OnPostCreateAsync()
        {
            //if (!ModelState.IsValid)
            //{
            //    return RedirectToPage("/Error");
            //}

            await _service.CreateAsync(NewCountry);

            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostEditAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var country = _service.GetById(EditedCountry.Id);
            if (country == null)
            {
                return NotFound();
            }

            await _service.UpdateAsync(EditedCountry);

            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostDeleteAsync()
        {
            var country = _service.GetById(CountryToDelete.Id);
            if (country == null)
            {
                return NotFound();
            }

            await _service.DeleteById(CountryToDelete.Id);

            return RedirectToPage();
        }
    }

}
