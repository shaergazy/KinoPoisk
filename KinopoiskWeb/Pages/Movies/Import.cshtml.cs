using BLL.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using BLL.Services;

namespace KinopoiskWeb.Pages.Movies
{
    public class ImportModel : PageModel
    {
        private readonly IMovieService _movieService;

        public ImportModel(IMovieService movieService)
        {
            _movieService = movieService;
        }

        public async Task<IActionResult> OnGetSearchAsync(string title)
        {
            if (string.IsNullOrEmpty(title))
            {
                return new JsonResult(new { success = false, error = "Title is required" });
            }

            string apiKey = "fc8e73bf";
            OMDBService omdb = new OMDBService(apiKey);

            var a = omdb.GetItemByTitle(title);

            if(a != null) 
                return new JsonResult(new { success = true, movies = a } );
            return new JsonResult(new { success = false });

            //string apiUrl = $"http://www.omdbapi.com/?apikey={apiKey}&s={title}";

            //using (HttpClient client = new HttpClient())
            //{
            //    HttpResponseMessage response = await client.GetAsync(apiUrl);
            //    if (response.IsSuccessStatusCode)
            //    {
            //        string jsonResponse = await response.Content.ReadAsStringAsync();
            //        var movieSearchResult = JsonConvert.DeserializeObject<MovieSearchResult>(jsonResponse);

            //        if (movieSearchResult.Response == "True")
            //        {
            //            return new JsonResult(new { success = true, movies = movieSearchResult.Search });
            //        }
            //        else
            //        {
            //            return new JsonResult(new { success = false, error = "No movies found" });
            //        }
            //    }
            //    else
            //    {
            //        return new JsonResult(new { success = false, error = "Error fetching data from OMDB API" });
            //    }
        }

        public async Task<IActionResult> OnPostImportAsync([FromBody] OMDBMovie movie)
        {
            if (movie == null)
            {
                return new JsonResult(new { success = false, error = "Invalid movie data" });
            }

            //var movieDto = new AddMovieDto
            //{
            //    Title = movie.Title,
            //    Year = movie.Year,
            //    Genre = movie.Genre,
            //    Director = movie.Director,
            //    Actors = movie.Actors,
            //    Plot = movie.Plot,
            //    Poster = movie.Poster,
            //    IMDBRating = movie.imdbRating
            //};

            //await _movieService.AddMovieAsync(movieDto);

            return new JsonResult(new { success = true });
        }

        public class MovieSearchResult
        {
            public string Response { get; set; }
            public List<OMDBMovie> Search { get; set; }
        }

        public class OMDBMovie
        {
            public string Title { get; set; }
            public string Year { get; set; }
            public string Genre { get; set; }
            public string Director { get; set; }
            public string Actors { get; set; }
            public string Plot { get; set; }
            public string Poster { get; set; }
            public string imdbRating { get; set; }
        }
    }
}