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

        public void OnGet()
        {
        }

        [BindProperty]
        public DataTablesRequest DataTablesRequest { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            var response = _mapper.Map<DataTablesResponseVM<Movie>>(await _service.SearchAsync(_mapper
                                  .Map<DataTablesRequestDto>(DataTablesRequest)));
            return new JsonResult(response);
        }
    }
}
