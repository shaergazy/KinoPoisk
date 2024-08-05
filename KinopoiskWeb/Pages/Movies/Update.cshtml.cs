using AutoMapper;
using BLL.Services.Interfaces;
using DAL.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace KinopoiskWeb.Pages.Movies
{
    public class UpdateModel : PageModel
    {
        private readonly IMovieService _movieService;
        private readonly IMapper _mapper;

        [BindProperty]
        public Movie Movie { get; set; }

        public UpdateModel(IMovieService movieService, IMapper mapper)
        {
            _movieService = movieService;
            _mapper = mapper;
        }

        public async Task<IActionResult> OnGetAsync(Guid id)
        {
            var movieDto = await _movieService.GetByIdAsync(id);
            if (movieDto == null)
            {
                return NotFound();
            }

            Movie = _mapper.Map<Movie>(movieDto);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            //await _movieService.UpdateAsync(Movie);
            return RedirectToPage("/Movies/Index");
        }
    }
}
