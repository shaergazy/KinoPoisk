using AutoMapper;
using BLL.DTO;
using BLL.DTO.Person;
using BLL.Services.Interfaces;
using DAL.Models;
using KinopoiskWeb.DataTables;
using KinopoiskWeb.ViewModels.Person;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace KinopoiskWeb.Pages.People
{
    //[Authorize(Roles = "Admin")]
    public class IndexModel : PageModel
    {
        private readonly IPersonService _service;
        private readonly IMapper _mapper;
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(IPersonService service, IMapper mapper, ILogger<IndexModel> logger)
        {
            _service = service;
            _mapper = mapper;
            _logger = logger;
        }

        [BindProperty]
        public List<IndexPersonVM> People { get; set; }

        [BindProperty]
        public PersonVM Person { get; set; }

        [BindProperty]
        public int PersonId { get; set; }

        [BindProperty]
        public DataTablesRequest DataTablesRequest { get; set; }

        public async Task OnGetAsync()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                var response = _mapper.Map<DataTablesResponseVM<Person>>(await _service.SearchAsync(_mapper
                                      .Map<DataTablesRequestDto>(DataTablesRequest)));
                return new JsonResult(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while processing the search request.");
                return new JsonResult(new { success = false, message = "An error occurred while processing the search request." });
            }
        }

        public async Task<JsonResult> OnGetById(int id)
        {
            try
            {
                var person = await _service.GetByIdAsync(id);
                if (person == null)
                {
                    return new JsonResult(NotFound());
                }
                return new JsonResult(_mapper.Map<IndexPersonVM>(person));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while fetching the person with ID {id}.");
                return new JsonResult(new { success = false, message = $"An error occurred while fetching the person with ID {id}." });
            }
        }

        public async Task<IActionResult> OnPostHandleCreateOrUpdateAsync(PersonVM person)
        {
            try
            {
                if (person == null)
                    throw new ArgumentNullException(nameof(person));

                if (person.Id == Guid.Empty)
                    await _service.CreateAsync(_mapper.Map<AddPersonDto>(person));
                else
                    await _service.UpdateAsync(_mapper.Map<EditPersonDto>(person));

                return RedirectToPage();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating or updating the person.");
                TempData["ErrorMessage"] = "An error occurred while creating or updating the person.";
                return Page();
            }
        }

        public async Task<IActionResult> OnPostDeleteAsync()
        {
            try
            {
                await _service.DeleteAsync(PersonId);
                return RedirectToPage();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while deleting the person with ID {PersonId}.");
                TempData["ErrorMessage"] = $"An error occurred while deleting the person.";
                return RedirectToPage();
            }
        }

        public JsonResult OnGetPeople(string searchTerm)
        {
            try
            {
                People = _mapper.Map<List<IndexPersonVM>>(_service.GetAll());
                if (!string.IsNullOrWhiteSpace(searchTerm))
                {
                    People = People.Where(m => (m.Translations.Any(t => t.Value.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)))).ToList();
                }
                return new JsonResult(People);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching the people list.");
                return new JsonResult(new { success = false, message = "An error occurred while fetching the people list." });
            }
        }

        public JsonResult OnGetDirectors(string searchTerm)
        {
            try
            {
                People = _mapper.Map<List<IndexPersonVM>>(_service.GetDirectors());
                if (!string.IsNullOrWhiteSpace(searchTerm))
                {
                    People = People.Where(m => (m.Translations.Any(t => t.Value.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)))).ToList();
                }
                return new JsonResult(People);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching the directors list.");
                return new JsonResult(new { success = false, message = "An error occurred while fetching the directors list." });
            }
        }

        public JsonResult OnGetActors(string searchTerm)
        {
            try
            {
                People = _mapper.Map<List<IndexPersonVM>>(_service.GetActors());
                if (!string.IsNullOrWhiteSpace(searchTerm))
                {
                    People = People.Where(m => (m.Translations.Any(t => t.Value.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)))).ToList();
                }
                return new JsonResult(People);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching the actors list.");
                return new JsonResult(new { success = false, message = "An error occurred while fetching the actors list." });
            }
        }
    }
}
