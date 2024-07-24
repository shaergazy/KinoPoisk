using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;
using BLL.Services;
using BLL.DTO.Movie;
using BLL.Services.Interfaces;

namespace KinopoiskWeb.Pages.Movies
{
    [IgnoreAntiforgeryToken]
    public class ImportModel : PageModel
    {
        private readonly OMDBService _omdbService;
        private readonly IMovieService _movieService;

        public ImportModel(OMDBService omdbService, IMovieService movieService)
        {
            _omdbService = omdbService;
            _movieService = movieService;
        }

        public async Task<IActionResult> OnGetSearchAsync(string title, int? year, string plot)
        {
            if (string.IsNullOrEmpty(title))
            {
                return new JsonResult(new { success = false, error = "Title is required" });
            }

            var searchResults = _omdbService.GetSearchList(year, title);

            if (searchResults != null)
            {
                return new JsonResult(new { success = true, movies = searchResults });
            }
            return new JsonResult(new { success = false, error = "No movies found" });
        }

        public async Task<IActionResult> OnGetSearchByIdAsync(string imdbId, int? year, string plot)
        {
            if (string.IsNullOrEmpty(imdbId))
            {
                return new JsonResult(new { success = false, error = "IMDb ID is required" });
            }

            var item = _omdbService.GetItemById(imdbId, plot == "full");

            if (item != null)
            {
                //var searchResults = new SearchList { SearchResults = new List<Item> { item } };
                return new JsonResult(new { success = true, movies = item });
            }
            return new JsonResult(new { success = false, error = "No movies found" });
        }

        public async Task<IActionResult> OnPostImportAsync( string imdbId)
        {
            if (string.IsNullOrEmpty(imdbId))
            {
                return new JsonResult(new { success = false, error = "Invalid movie data" });
            }

            var item = _omdbService.GetItemById(imdbId);
            if (item != null)
            {
                //var movieDto = new AddMovieDto
                //{
                //    Title = item.Title,
                //    ReleasedDate = item.Year,
                //    Director = item.Director,
                //    Actors = item.Actors,
                //    Description = item.Plot,
                //    Poster = item.Poster,
                //    ImdbId = item.
                //};

                //var result = await _movieService.AddMovieAsync(movieDto);

                //if (result.Success)
                //{
                //    return new JsonResult(new { success = true });
                //}

                //return new JsonResult(new { success = false, error = result.ErrorMessage });
            }
            return new JsonResult(new { success = false, error = "Movie not found" });
        }
    }
}
