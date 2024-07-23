using AutoMapper;
using BLL.DTO;
using BLL.DTO.Genre;
using BLL.Services.Interfaces;
using DAL.Models;
using KinopoiskWeb.DataTables;
using KinopoiskWeb.ViewModels.Genre;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace KinopoiskWeb.Pages.Genres
{
    [AllowAnonymous]
    [Authorize(Roles = "Admin")]
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
            var response = _mapper.Map<DataTablesResponseVM<Genre>>(await _service.SearchAsync(_mapper
                                  .Map<DataTablesRequestDto>(DataTablesRequest)));
            return new JsonResult(response);
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
            try
            {
                await _service.DeleteAsync(GenreId);
                TempData["SuccessMessage"] = "Deleted";
                return RedirectToPage();
            }
            catch (DbUpdateException)
            {
                TempData["ErrorMessage"] = "Some movie has contain this genre, thatswhy you are not able to remove";
                return RedirectToPage();
            }
        }


        public JsonResult OnGetGenres(string searchTerm)
        {
            var genres = _service.GetAll();
            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                genres = genres.Where(s =>
                    s.Name.ToUpper().Contains(searchTerm.ToUpper()));
            }
            return new JsonResult(genres);
        }
    }


}
