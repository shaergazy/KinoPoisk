using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DAL.Entities;
using Data.Repositories.RepositoryInterfaces;

namespace KinopoiskWeb.Pages.Genres
{
    public class IndexModel : PageModel
    {
        private readonly IUnitOfWork _uow;

        public IndexModel(IUnitOfWork uow)
        {
            _uow = uow;
        }


        [BindProperty]
        public IList<Genre> Genres { get; set; }

        [BindProperty]
        public Genre NewGenre { get; set; }

        [BindProperty]
        public Genre EditedGenre { get; set; }

        [BindProperty]
        public Genre GenreToDelete { get; set; }

        public async Task OnGetAsync()
        {
            Genres = _uow.Genres.GetAll().ToList();
        }

        public async Task<IActionResult> OnPostCreateAsync()
        {
            if (_uow.Genres.Any(x => x.Name == NewGenre.Name))
            {
                return Page();
            }

            await _uow.Genres.AddAsync(NewGenre, true);

            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostEditAsync()
        {
            if (_uow.Genres.Any(x => x.Name == EditedGenre.Name))
            {
                return Page();
            }

            await _uow.Genres.UpdateAsync(EditedGenre);

            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostDeleteAsync()
        {
            var genre = _uow.Genres.GetAll().FirstOrDefault(x => x.Id == GenreToDelete.Id);

            if (genre != null)
            {
                await _uow.Genres.DeleteAsync(genre.Id);
                await _uow.SaveChangesAsync();
            }

            return RedirectToPage();
        }
    }


}
