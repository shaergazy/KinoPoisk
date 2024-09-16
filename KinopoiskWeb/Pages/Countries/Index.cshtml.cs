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
    //[Authorize(Roles = "Admin")]
    public class IndexModel : PageModel
    {
        private readonly ICountryService _service;
        private readonly IMapper _mapper;
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ICountryService service, IMapper mapper, ILogger<IndexModel> logger)
        {
            _service = service;
            _mapper = mapper;
            _logger = logger;
        }

        [BindProperty]
        public List<CountryVM> Countries { get; set; }

        [BindProperty]
        public CountryVM Country { get; set; }

        [BindProperty]
        public Guid CountryId { get; set; }

        public async Task OnGetAsync()
        {
        }

        [BindProperty]
        public DataTablesRequest DataTablesRequest { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            _logger.LogInformation("Handling data table request.");
            var response = _mapper.Map<DataTablesResponseVM<Country>>(await _service.SearchAsync(_mapper
                                  .Map<DataTablesRequestDto>(DataTablesRequest)));
            return new JsonResult(response);
        }

        public async Task<IActionResult> OnPostHandleCreateOrUpdateAsync(CountryVM country)
        {
             if (country == null)
            {
                _logger.LogError("Country parameter is null.");
                throw new ArgumentNullException(nameof(country));
            }

            if (country.Id == Guid.Empty)
            {
                _logger.LogInformation("Creating new country.");
                await _service.CreateAsync(_mapper.Map<AddCountryDto>(country));
            }
            else
            {
                _logger.LogInformation("Updating country with ID {CountryId}.", country.Id);
                await _service.UpdateAsync(_mapper.Map<EditCountryDto>(country));
            }

            return RedirectToPage();
        }

        public async Task<JsonResult> OnGetById(Guid id)
        {
            _logger.LogInformation("Fetching country with ID {CountryId}.", id);
            var country = await _service.GetByIdAsync(id);
            if (country == null)
            {
                _logger.LogWarning("Country with ID {CountryId} not found.", id);
                return new JsonResult(NotFound());
            }

            return new JsonResult(_mapper.Map<CountryVM>(country));
        }

        public async Task<IActionResult> OnPostDeleteAsync()
        {
            _logger.LogInformation("Deleting country with ID {CountryId}.", CountryId);
            await _service.DeleteAsync(CountryId);
            return RedirectToPage();
        }

        public JsonResult OnGetCountries(string searchTerm)
        {
            _logger.LogInformation("Fetching countries with search term: {SearchTerm}.", searchTerm);
            var entities = _service.GetAllWithTranslations();
            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                entities = entities.Where(c => c.Translations.Any(t => t.Value.ToUpper().Contains(searchTerm.ToUpper())));
            }
            return new JsonResult(entities);
        }
    }
}
