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

        public async Task<IActionResult> OnPostGeneratePdfAsync()
        {
            var dto = _mapper.Map<MovieDataTablesRequestDto>(DataTablesRequest);
            var pdfData = await _service.GeneratePdfAsync(dto);
            return File(pdfData, "application/pdf");
        }

        public async Task<IActionResult> OnPostGenerateExcelAsync()
        {
            var dto = _mapper.Map<MovieDataTablesRequestDto>(DataTablesRequest);
            var excelData = await _service.GenerateExcelAsync(dto);
            return File(excelData, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
        }
    }
}


