using AutoMapper;
using BLL.DTO.Genre;
using BLL.Services.Interfaces;
using KinopoiskWeb.ViewModels.Genre;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace KinopoiskWeb.Pages.Genres
{
    public class IndexModel : PageModel
    {
        private readonly IGenreService _service;
        private readonly IMapper _mapper;
        public IndexModel(IGenreService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }


        [BindProperty]
        public IList<IndexGenreVM> Genres { get; set; }

        [BindProperty]
        public GenreVM Genre { get; set; }

        [BindProperty]
        public int GenreId { get; set; }

        public async Task OnGetAsync()
        {
            Genres = _mapper.Map<List<IndexGenreVM>>(await _service.GetAll());
        }

        public async Task<JsonResult> OnGetById(int id)
        {
            var genre = await _service.GetById(id);
            if (genre == null)
            {
                return new JsonResult(NotFound());
            }

            return new JsonResult(_mapper.Map<IndexGenreVM>(genre));
        }

        public async Task<IActionResult> OnPostHandleUpdateOrCreateAsync(GenreVM genre)
        {
            if (genre == null)
                throw new ArgumentNullException(nameof(genre));

            if (genre.Id == 0)
                await _service.CreateAsync(_mapper.Map<AddGenreDto>(genre));
            else
                await _service.UpdateAsync(_mapper.Map<EditGenreDto>(genre));

            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostDeleteAsync()
        {
            await _service.DeleteById(GenreId);
            return RedirectToPage();
        }
    }


}
