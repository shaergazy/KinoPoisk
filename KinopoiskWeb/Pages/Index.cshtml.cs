using AutoMapper;
using BLL.Services.Interfaces;
using KinopoiskWeb.ViewModels.Movie;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Localization;
using System.Globalization;

namespace KinopoiskWeb.Pages;
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
        var culture = CultureInfo.CurrentCulture.TwoLetterISOLanguageName;

        // Ключи кеша с учетом культуры
        var newestMoviesCacheKey = $"{NewestMoviesCacheKey}_{culture}";
        var highRatedMoviesCacheKey = $"{HighRatedMoviesCacheKey}_{culture}";

        // Получение кешированных данных для NewestMovies
        if (!_cache.TryGetValue(newestMoviesCacheKey, out List<IndexMovieVM> cachedNewestMovies))
        {
            var newestMoviesDto = await _movieService.GetNewestMoviesAsync(10);
            cachedNewestMovies = _mapper.Map<List<IndexMovieVM>>(newestMoviesDto.ToList());

            var cacheEntryOptions = new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(1),
                SlidingExpiration = TimeSpan.FromMinutes(1)
            };

            _cache.Set(newestMoviesCacheKey, cachedNewestMovies, cacheEntryOptions);
        }
        NewestMovies = cachedNewestMovies;

        // Получение кешированных данных для HighRatedMovies
        if (!_cache.TryGetValue(highRatedMoviesCacheKey, out List<IndexMovieVM> cachedHighRatedMovies))
        {
            var highRatedMoviesDto = await _movieService.GetTopRatedMoviesAsync(10);
            cachedHighRatedMovies = _mapper.Map<List<IndexMovieVM>>(highRatedMoviesDto.ToList());

            var cacheEntryOptions = new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(1),
                SlidingExpiration = TimeSpan.FromMinutes(1)
            };

            _cache.Set(highRatedMoviesCacheKey, cachedHighRatedMovies, cacheEntryOptions);
        }
        HighRatedMovies = cachedHighRatedMovies;
    }

}
