using AutoMapper;
using BLL.Services.Interfaces;
using KinopoiskWeb.ViewModels.Movie;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace KinopoiskWeb.Pages.Movies
{
    public class DetaixlsModel : PageModel
    {
        private readonly IMovieService _movieService;
        private readonly IMapper _mapper;

        public DetaixlsModel(IMovieService movieService, IMapper mapper)
        {
            _movieService = movieService;
            _mapper = mapper;
        }

        public DetailsMovieVM Movie { get; private set; }
        public async Task<IActionResult> OnGetAsync(Guid id)
        {
            Movie = _mapper.Map<DetailsMovieVM>(await _movieService.GetByIdAsync(id));

            if (Movie == null)
            {
                return NotFound();
            }

            return Page();
        }
    }
}
