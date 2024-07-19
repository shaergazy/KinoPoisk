using AutoMapper;
using BLL.Services.Interfaces;
using KinopoiskWeb.ViewModels.Movie;
using Microsoft.AspNetCore.Mvc.RazorPages;

public class IndexModel : PageModel
{
    private readonly IMovieService _movieService;
    private readonly IMapper _mapper;

    public IndexModel(IMovieService movieService, IMapper mapper)
    {
        _movieService = movieService;
        _mapper = mapper;
    }

    public List<IndexMovieVM> NewestMovies { get; set; }
    public List<IndexMovieVM> HighRatedMovies { get; set; }


    public async Task OnGetAsync()
    {
        NewestMovies = new List<IndexMovieVM>();
        HighRatedMovies = new List<IndexMovieVM> { new IndexMovieVM() };
        NewestMovies = _mapper.Map<List<IndexMovieVM>>((await _movieService.GetNewestMoviesAsync(10)).ToList()); 
        HighRatedMovies = _mapper.Map<List<IndexMovieVM>>((await _movieService.GetTopRatedMoviesAsync(10)).ToList());
    }
}
