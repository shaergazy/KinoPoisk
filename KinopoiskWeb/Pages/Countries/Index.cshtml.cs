using AutoMapper;
using BLL.DTO.Country;
using BLL.Services.Interfaces;
using KinopoiskWeb.ViewModels.Country;
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
        public CountryVM Country { get; set; }

        [BindProperty]
        public int CountryId { get; set; }

        public async Task OnGetAsync()
        {
            Countries = _mapper.Map<List<IndexCountryVM>>(await _service.GetAllAsync());
        }

        public async Task<IActionResult> OnPostHandleCreateOrUpdateAsync(CountryVM country)
        {
            if (country == null)
                throw new ArgumentNullException(nameof(country));

            if (country.Id == 0)
                await _service.CreateAsync(_mapper.Map<AddCountryDto>(country));
            else
                await _service.UpdateAsync(_mapper.Map<EditCountryDto>(country));

            return RedirectToPage();
        }

        public async Task<JsonResult> OnGetById(int id)
        {
            var country = await _service.GetByIdAsync(id);
            if (country == null)
            {
                return new JsonResult(NotFound());
            }

            return new JsonResult(_mapper.Map<IndexCountryVM>(country));
        }

        public async Task<IActionResult> OnPostDeleteAsync()
        {
            await _service.DeleteAsync(CountryId);

            return RedirectToPage();
        }
    }

}
