using AutoMapper;
using BLL.DTO.Movie;
using BLL.Services.Interfaces;
using DAL.Models;
using KinopoiskWeb.ViewModels.Movie;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace KinopoiskWeb.Pages.Movies
{
    public class CreateModel : PageModel
    {
        private readonly IMovieService _movieService;
        private readonly IMapper _mapper;

        [BindProperty]
        public CreateMovieVM Movie { get; set; }

        [BindProperty]
        public ActorVM Actor { get; set; }

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
            if (!ModelState.IsValid)
            {
                return Page();
            }
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
    

        public async Task<JsonResult> OnGetCountries()
        {
            var countries = _movieService.GetCountries().ToList();
            return new JsonResult(countries);
        }

        public async Task<JsonResult> OnGetGenres()
        {
            var genres = _movieService.GetGenres().ToList();
            return new JsonResult(genres);
        }

        public async Task<JsonResult> OnGetPeople()
        {
            var people = _movieService.GetPeople().ToList();
            return new JsonResult(people);
        }

        public async Task<IActionResult> OnPostAddActor()
        {
            if (Movie == null)
            {
                Movie = new CreateMovieVM();
            }

            if (Movie.Actors == null)
            {
                Movie.Actors = new List<ActorVM>();
            }

            if (Actor != null && Actor.PersonId != 0)
            {
                var person = await _movieService.GetPeople().FirstOrDefaultAsync(p => p.Id == Actor.PersonId);
                if (person != null)
                {
                    Movie.Actors.Add(new ActorVM
                    {
                        PersonId = Actor.PersonId,
                        Order = Actor.Order,
                        PersonName = $"{person.FirstName} {person.LastName}"
                    });
                }
            }

            // ���������� JSON ��������� ��� AJAX ������
            return Page();/*new JsonResult(new { success = true, personName = Actor.PersonName });*/
        }
    }
}
