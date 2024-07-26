using BLL.DTO.Movie;
using BLL.Services;
using BLL.Services.Interfaces;
using DAL.Models;
using Data.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace KinopoiskWeb.Pages.Movies
{
    //[IgnoreAntiforgeryToken]
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

        public async Task<IActionResult> OnPostImportAsync([FromBody] ImdbIdRequest request)
        {
            try
            {
                var item = _omdbService.GetItemById(request.ImdbId);
                if (item == null)  return new JsonResult(new { success = false });
                await _movieService.ImportMovieAsync(item);
                return new JsonResult(new { success = true, movie = item });
            }
            catch (Exception)
            {

                throw;
            }
        }
        public class ImdbIdRequest
        {
            public string ImdbId { get; set; }
        }


    }
}
