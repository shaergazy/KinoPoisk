using AutoMapper;
using BLL.DTO.Movie;
using BLL.Services.Interfaces;
using KinopoiskWeb.ViewModels.Movie;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace KinopoiskWeb.Pages.Movies
{
    [Authorize]
    public class CreateModel : PageModel
    {
        private readonly IMovieService _movieService;
        private readonly IMapper _mapper;

        [BindProperty]
        public CreateMovieVM Movie { get; set; }

        public CreateModel(IMovieService movieService, IMapper mapper)
        {
            _movieService = movieService;
            _mapper = mapper;
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
                return RedirectToPage("Create");
            }
            catch (Exception)
            {
                TempData["ErrorMessage"] = "Failed to create the movie.";
                return Page();
            }
        }
    }
}
