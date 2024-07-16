using AutoMapper;
using BLL.DTO;
using BLL.DTO.Country;
using BLL.Services.Interfaces;
using DAL.Models;
using KinopoiskWeb.DataTables;
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
            Countries = _mapper.Map<List<IndexCountryVM>>(_service.GetAll());
        }

        [BindProperty]
        public DataTablesRequest DataTablesRequest { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            var response = _mapper.Map<DataTablesResponseVM<Country>>( await _service.SearchAsync(_mapper
                                  .Map<DataTablesRequestDto>(DataTablesRequest)));
            return new JsonResult(response);
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

        public JsonResult OnGetCountries(string searchTerm)
        {
            var entities = _service.GetAll();
            if (!string.IsNullOrWhiteSpace(searchTerm) && searchTerm.Length >= 3)
            {
                entities = entities.Where(s => s.Name.ToUpper()
                                   .Contains(searchTerm.ToUpper()));
            }
            return new JsonResult(entities);
        }
    }

}
