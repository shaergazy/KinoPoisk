using BLL.Services.Interfaces;
using BLL.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace KinopoiskWeb.Pages.Genres
{
    public class IndexModel : PageModel
    {
        private readonly IGenreService _service;

        public IndexModel(IGenreService service)
        {
            _service = service;
        }


        [BindProperty]
        public IList<GenreDto.IdHasBase> Genres { get; set; }

        [BindProperty]
        public GenreDto.IdHasBase NewGenre { get; set; }

        [BindProperty]
        public GenreDto.IdHasBase EditedGenre { get; set; }

        [BindProperty]
        public GenreDto.IdHasBase GenreToDelete { get; set; }

        public async Task OnGetAsync()
        {
            Genres = await _service.GetAll();
        }

        public async Task<IActionResult> OnPostCreateAsync()
        {
            await _service.CreateAsync(NewGenre);

            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostEditAsync()
        {
            await _service.UpdateAsync(EditedGenre);

            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostDeleteAsync()
        {
            await _service.DeleteById(GenreToDelete.Id);
            return RedirectToPage();
        }
    }


}
