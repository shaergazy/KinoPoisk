using AutoMapper;
using BLL.DTO;
using BLL.DTO.Genre;
using BLL.Services.Interfaces;
using DAL.Models;
using KinopoiskWeb.DataTables;
using KinopoiskWeb.ViewModels.Genre;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace KinopoiskWeb.Pages.Genres
{
    public class IndexModel : PageModel
    {
        private readonly IGenreService _service;
        private readonly IMapper _mapper;
        private readonly ILogger<IndexModel> _logger;
        private readonly IStringLocalizer<IndexModel> _localizer;

        public IndexModel(IGenreService service, IMapper mapper, ILogger<IndexModel> logger, IStringLocalizer<IndexModel> localizer)
        {
            _service = service;
            _mapper = mapper;
            _logger = logger;
            _localizer = localizer;
        }

        [BindProperty]
        public IList<GenreVM> Genres { get; set; }

        [BindProperty]
        public GenreVM Genre { get; set; }

        [BindProperty]
        public int GenreId { get; set; }

        [BindProperty]
        public DataTablesRequest DataTablesRequest { get; set; }

        public async Task OnGetAsync()
        {
            Genres = _mapper.Map<List<GenreVM>>(_service.GetAll());
        }

        public async Task<IActionResult> OnPostAsync()
        {
            _logger.LogInformation("Handling data table request.");
            var response = _mapper.Map<DataTablesResponseVM<Genre>>(await _service.SearchAsync(_mapper
                                  .Map<DataTablesRequestDto>(DataTablesRequest)));
            return new JsonResult(response);
        }

        public async Task<JsonResult> OnGetById(int id)
        {
            _logger.LogInformation("Fetching genre with ID {GenreId}.", id);
            var genre = await _service.GetByIdAsync(id);
            if (genre == null)
            {
                _logger.LogWarning("Genre with ID {GenreId} not found.", id);
                return new JsonResult(NotFound());
            }
            var s  = _mapper.Map<GenreVM>(genre);
            return new JsonResult(_mapper.Map<GenreVM>(genre));
        }

        public async Task<IActionResult> OnPostHandleUpdateOrCreateAsync()
        {
            if (Genre == null)
            {
                _logger.LogError("Genre parameter is null.");
                ModelState.AddModelError(string.Empty, _localizer["GenreNullError"].Value);
                return Page();
            }
            try
            {
                if (Genre.Id == 0)
                {
                    _logger.LogInformation("Creating new genre.");
                    await _service.CreateAsync(_mapper.Map<AddGenreDto>(Genre));
                    TempData["SuccessMessage"] = _localizer["GenreCreated"].Value;
                }
                else
                {
                    _logger.LogInformation("Updating genre with ID {GenreId}.", Genre.Id);
                    await _service.UpdateAsync(_mapper.Map<EditGenreDto>(Genre));
                    TempData["SuccessMessage"] = _localizer["GenreUpdated"].Value;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while saving genre with ID {GenreId}.", Genre.Id);
                TempData["ErrorMessage"] = _localizer["SaveGenreError"].Value;
            }

            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostDeleteAsync()
        {
            try
            {
                _logger.LogInformation("Deleting genre with ID {GenreId}.", GenreId);
                await _service.DeleteAsync(GenreId);
                TempData["SuccessMessage"] = _localizer["GenreDeleted"].Value;
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Error occurred while deleting genre with ID {GenreId}.", GenreId);
                TempData["ErrorMessage"] = _localizer["DeleteGenreError"].Value;
            }

            return RedirectToPage();
        }

        public async Task<JsonResult> OnGetGenres(string searchTerm)
        {
            _logger.LogInformation("Fetching genres with search term: {SearchTerm}.", searchTerm);
            var genres =  _service.GetAll();

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                genres = genres.Where(g =>
                    g.Translations.Any(t => t.Value.Contains(searchTerm, StringComparison.OrdinalIgnoreCase))).ToList();
            }

            var result = _mapper.Map<List<GenreVM>>(genres);
            return new JsonResult(result);
        }
    }
}
