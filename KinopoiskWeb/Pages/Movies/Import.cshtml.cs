using BLL.DTO.Movie;
using BLL.Services;
using BLL.Services.Interfaces;
using DAL.Models;
using Data.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace KinopoiskWeb.Pages.Movies
{
    public class ImportModel : PageModel
    {
        private readonly OMDBService _omdbService;
        private readonly IMovieService _movieService;
        private readonly ILogger<ImportModel> _logger;

        public ImportModel(OMDBService omdbService, IMovieService movieService, ILogger<ImportModel> logger)
        {
            _omdbService = omdbService;
            _movieService = movieService;
            _logger = logger;
        }

        public async Task<IActionResult> OnGetSearchAsync(string title, int? year, string plot)
        {
            if (string.IsNullOrEmpty(title))
            {
                _logger.LogWarning("Search attempted without a title.");
                return new JsonResult(new { success = false, error = "Title is required" });
            }

            _logger.LogInformation("Searching for movies with title: {Title}, year: {Year}, plot: {Plot}", title, year, plot);

            var searchResults = _omdbService.GetSearchList(year, title);

            if (searchResults != null)
            {
                _logger.LogInformation("Movies found for title: {Title}", title);
                return new JsonResult(new { success = true, movies = searchResults });
            }

            _logger.LogWarning("No movies found for title: {Title}", title);
            return new JsonResult(new { success = false, error = "No movies found" });
        }

        public async Task<IActionResult> OnGetSearchByIdAsync(string imdbId, int? year, string plot)
        {
            if (string.IsNullOrEmpty(imdbId))
            {
                _logger.LogWarning("Search by IMDb ID attempted without an IMDb ID.");
                return new JsonResult(new { success = false, error = "IMDb ID is required" });
            }

            _logger.LogInformation("Searching for movie with IMDb ID: {ImdbId}, year: {Year}, plot: {Plot}", imdbId, year, plot);

            var item = _omdbService.GetItemById(imdbId, plot == "full");

            if (item != null)
            {
                _logger.LogInformation("Movie found with IMDb ID: {ImdbId}", imdbId);
                return new JsonResult(new { success = true, movies = item });
            }

            _logger.LogWarning($"No movie found with IMDb ID: {imdbId}");
            return new JsonResult(new { success = false, error = "No movies found" });
        }

        public async Task<IActionResult> OnPostImportAsync([FromBody] ImdbIdRequest request)
        {
            try
            {
                if (string.IsNullOrEmpty(request.ImdbId))
                {
                    _logger.LogWarning("Import attempted without an IMDb ID.");
                    return new JsonResult(new { success = false });
                }

                _logger.LogInformation("Importing movie with IMDb ID: {ImdbId}", request.ImdbId);

                var item = _omdbService.GetItemById(request.ImdbId);
                if (item == null)
                {
                    _logger.LogWarning("No movie found to import with IMDb ID: {ImdbId}", request.ImdbId);
                    return new JsonResult(new { success = false });
                }

                await _movieService.ImportMovieAsync(item);

                _logger.LogInformation("Successfully imported movie with IMDb ID: {ImdbId}", request.ImdbId);
                return new JsonResult(new { success = true, movie = item });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while importing movie with IMDb ID: {ImdbId}", request.ImdbId);
                return new JsonResult(new { success = false, error = "An error occurred while importing the movie." });
            }
        }

        public class ImdbIdRequest
        {
            public string ImdbId { get; set; }
        }
    }
}
