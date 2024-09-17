using AutoMapper;
using BLL.DTO.Movie;
using BLL.Services.Interfaces;
using DAL.Enums;
using KinopoiskWeb.ViewModels.Movie;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
namespace KinopoiskWeb.Pages.Movies
{
    public class CreateModel : PageModel
    {
        private readonly IMovieService _movieService;
        private readonly IMapper _mapper;
        private readonly ILogger<CreateModel> _logger;

        [BindProperty]
        public CreateMovieVM Movie { get; set; }

        public CreateModel(IMovieService movieService, IMapper mapper, ILogger<CreateModel> logger)
        {
            _movieService = movieService;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            Movie = new CreateMovieVM
            {
                Actors = new List<ActorVM>()
            };
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                await _movieService.CreateAsync(_mapper.Map<AddMovieDto>(Movie));
                TempData["SuccessMessage"] = "Movie created successfully!";
                _logger.LogInformation("Movie '{Title}' created successfully by user {UserId}", 
                                        Movie.Translations.FirstOrDefault(x => x.FieldType == TranslatableFieldType.Title), 
                                        User.Identity.Name);
                return RedirectToPage("Create");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Failed to create the movie.";
                _logger.LogError(ex, "Failed to create movie '{Title}' by user {UserId}",
                                        Movie.Translations.FirstOrDefault(x => x.FieldType == TranslatableFieldType.Title),
                                        User.Identity.Name);
                return Page();
            }
        }
    }
}
