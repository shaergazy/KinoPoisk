using AutoMapper;
using BLL.DTO.GenreDTOs;
using BLL.Services.Implementation;
using BLL.Services.Interfaces;
using Data.Repositories.RepositoryInterfaces;
using KinopoiskWeb.ViewModels.GenreVM;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace KinopoiskWeb.Pages.Genres
{
    public class IndexModel : PageModel
    {
        private readonly IGenreService _service;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _uow;

        public IndexModel(IGenreService service, IMapper mapper, IUnitOfWork uow)
        {
            _service = service;
            _mapper = mapper;
            _uow = uow;
        }


        [BindProperty]
        public IList<IndexGenreVM> Genres { get; set; }

        [BindProperty]
        public CreateGenreVM NewGenre { get; set; }

        [BindProperty]
        public EditGenreVM EditedGenre { get; set; }

        [BindProperty]
        public int GenreId { get; set; }

        public async Task OnGetAsync()
        {
            Genres = _mapper.Map<List<IndexGenreVM>>(await _service.GetAll());
        }

        public async Task<JsonResult> OnGetGetGenre(int id)
        {
            var genre = await _service.GetById(id);
            if (genre == null)
            {
                return new JsonResult(NotFound());
            }

            return new JsonResult(genre);
        }

        public async Task<IActionResult> OnPostCreateAsync()
        {
            await _service.CreateAsync(_mapper.Map<AddGenreDto>(NewGenre));

            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostEditAsync()
        {
            await _service.UpdateAsync(_mapper.Map<EditGenreDto>(EditedGenre));

            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostDeleteAsync()
        {
            await _service.DeleteById(GenreId);
            return RedirectToPage();
        }
    }


}
