using AutoMapper;
using BLL.DTO.Genre;
using BLL.DTO;
using BLL.Services.Interfaces;
using DAL.Models;
using KinopoiskWeb.ViewModels.Genre;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BLL.DataTables;

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
            //Genres = _mapper.Map<List<IndexGenreVM>>(_service.GetAll());
        }

        [BindProperty]
        public DataTablesRequest DataTablesRequest { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            return await _service.GetSortedAsync(DataTablesRequest);
        }

        public async Task<JsonResult> OnGetById(int id)
        {
            var genre = await _service.GetByIdAsync(id);
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
            await _service.DeleteAsync(GenreId);
            return RedirectToPage();
        }
    }


}
