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
    //[Authorize(Roles = "Admin")]
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
        public IList<IndexGenreVM> Genres { get; set; }

        [BindProperty]
        public GenreVM Genre { get; set; }

        [BindProperty]
        public int GenreId { get; set; }

        public async Task OnGetAsync()
        {
        }

        [BindProperty]
        public DataTablesRequest DataTablesRequest { get; set; }

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

            return new JsonResult(_mapper.Map<IndexGenreVM>(genre));
        }

        public async Task<IActionResult> OnPostHandleUpdateOrCreateAsync(GenreVM genre)
        {
            if (genre == null)
            {
                _logger.LogError("Genre parameter is null.");
                throw new ArgumentNullException(nameof(genre));
            }

            if (genre.Id == 0)
            {
                _logger.LogInformation("Creating new genre.");
                await _service.CreateAsync(_mapper.Map<AddGenreDto>(genre));
            }
            else
            {
                _logger.LogInformation("Updating genre with ID {GenreId}.", genre.Id);
                await _service.UpdateAsync(_mapper.Map<EditGenreDto>(genre));
            }

            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostDeleteAsync()
        {
            try
            {
                _logger.LogInformation("Deleting genre with ID {GenreId}.", GenreId);
                await _service.DeleteAsync(GenreId);
                TempData["SuccessMessage"] = _localizer["Deleted"];
                return RedirectToPage();
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Error occurred while deleting genre with ID {GenreId}.", GenreId);
                TempData["ErrorMessage"] = _localizer["DeleteGenreError"];
                return RedirectToPage();
            }
        }

        public JsonResult OnGetGenres(string searchTerm)
        {
            _logger.LogInformation("Fetching genres with search term: {SearchTerm}.", searchTerm);
            var genres = _service.GetAll();
            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                //genres = genres.Where(s =>
                //    s.Name.ToUpper().Contains(searchTerm.ToUpper()));
            }
            return new JsonResult(genres);
        }
    }
}
