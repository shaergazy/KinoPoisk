using AutoMapper;
using BLL.DTO.GenreDTOs;
using BLL.Services.Interfaces;
using KinopoiskWeb.ViewModels.GenreVM;
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
        public CreateGenreVM NewGenre { get; set; }

        [BindProperty]
        public EditGenreVM EditedGenre { get; set; }

        [BindProperty]
        public int GenreId { get; set; }

        public async Task OnGetAsync()
        {
            Genres = _mapper.Map<List<IndexGenreVM>>(await _service.GetAll());
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
