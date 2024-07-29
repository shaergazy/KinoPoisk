using AutoMapper;
using BLL.Services.Interfaces;
using KinopoiskWeb.ViewModels.Movie;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Caching.Memory;

public class IndexModel : PageModel
{
    private readonly IMovieService _movieService;
    private readonly IMapper _mapper;
    private readonly IMemoryCache _cache; 

    private const string NewestMoviesCacheKey = "NewestMovies";
    private const string HighRatedMoviesCacheKey = "HighRatedMovies";

    public IndexModel(IMovieService movieService, IMapper mapper, IMemoryCache cache)
    {
        _movieService = movieService;
        _mapper = mapper;
        _cache = cache;
    }

    public List<IndexMovieVM> NewestMovies { get; set; }
    public List<IndexMovieVM> HighRatedMovies { get; set; }

    public async Task OnGetAsync()
    {
        if (!_cache.TryGetValue(NewestMoviesCacheKey, out List<IndexMovieVM> cachedNewestMovies))
        {
            var newestMoviesDto = await _movieService.GetNewestMoviesAsync(10);
            cachedNewestMovies = _mapper.Map<List<IndexMovieVM>>(newestMoviesDto.ToList());

            var cacheEntryOptions = new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10),
                SlidingExpiration = TimeSpan.FromMinutes(1) 
            };

            _cache.Set(NewestMoviesCacheKey, cachedNewestMovies, cacheEntryOptions);
        }

        NewestMovies = cachedNewestMovies;

        if (!_cache.TryGetValue(HighRatedMoviesCacheKey, out List<IndexMovieVM> cachedHighRatedMovies))
        {
            var highRatedMoviesDto = await _movieService.GetTopRatedMoviesAsync(10);
            cachedHighRatedMovies = _mapper.Map<List<IndexMovieVM>>(highRatedMoviesDto.ToList());

            var cacheEntryOptions = new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10),
                SlidingExpiration = TimeSpan.FromMinutes(1)
            };

            _cache.Set(HighRatedMoviesCacheKey, cachedHighRatedMovies, cacheEntryOptions);
        }

        HighRatedMovies = cachedHighRatedMovies;
    }
}
