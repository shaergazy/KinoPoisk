using AutoMapper;
using BLL.DTO;
using BLL.Services.Interfaces;
using DAL.Models;
using KinopoiskWeb.DataTables;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace KinopoiskWeb.Pages.Movies
{
    public class IndexModel : PageModel
    {
        private readonly IMovieService _service;
        private readonly IMapper _mapper;

        public IndexModel(IMovieService service, IMapper mapper)
        {
            _mapper = mapper;
            _service = service;
        }

        [BindProperty]
        public MovieDataTablesRequest DataTablesRequest { get; set; }

        public void OnGet()
        {
            DataTablesRequest = new MovieDataTablesRequest();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var dto = _mapper.Map<MovieDataTablesRequestDto>(DataTablesRequest);
            var response = await _service.SearchAsync(dto);
            var viewModel = _mapper.Map<DataTablesResponseVM<Movie>>(response);
            return new JsonResult(viewModel);
        }
    }
}
