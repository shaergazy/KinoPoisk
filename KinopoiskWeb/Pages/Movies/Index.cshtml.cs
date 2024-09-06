//using AutoMapper;
//using BLL.DTO;
//using BLL.Services.Interfaces;
//using DAL.Models;
//using KinopoiskWeb.DataTables;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Mvc.RazorPages;

//namespace KinopoiskWeb.Pages.Movies
//{
//    public class IndexModel : PageModel
//    {
//        private readonly IMovieService _service;
//        private readonly IMapper _mapper;
//        private readonly ILogger<IndexModel> _logger;

//        public IndexModel(IMovieService service, IMapper mapper, ILogger<IndexModel> logger)
//        {
//            _mapper = mapper;
//            _service = service;
//            _logger = logger;
//        }

//        [BindProperty]
//        public MovieDataTablesRequest DataTablesRequest { get; set; }

//        [BindProperty]
//        public Guid MovieId { get; set; }

//        public void OnGet()
//        {
//            DataTablesRequest = new MovieDataTablesRequest();
//        }

//        public async Task<IActionResult> OnPostAsync()
//        {
//            try
//            {
//                var dto = _mapper.Map<MovieDataTablesRequestDto>(DataTablesRequest);
//                var response = await _service.SearchAsync(dto);
//                var viewModel = _mapper.Map<DataTablesResponseVM<Movie>>(response);
//                return new JsonResult(viewModel);
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "An error occurred while searching for movies.");
//                TempData["ErrorMessage"] = "An error occurred while searching for movies.";
//                return new JsonResult(new { success = false, message = "An error occurred while searching for movies." });
//            }
//        }

//        public async Task<IActionResult> OnPostGeneratePdfAsync()
//        {
//            try
//            {
//                var dto = _mapper.Map<MovieDataTablesRequestDto>(DataTablesRequest);
//                var pdfData = await _service.GeneratePdfAsync(dto);
//                return File(pdfData, "application/pdf");
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "An error occurred while generating PDF.");
//                TempData["ErrorMessage"] = "An error occurred while generating PDF.";
//                return RedirectToPage();
//            }
//        }

//        public async Task<IActionResult> OnPostGenerateExcelAsync()
//        {
//            try
//            {
//                var dto = _mapper.Map<MovieDataTablesRequestDto>(DataTablesRequest);
//                var excelData = await _service.GenerateExcelAsync(dto);
//                return File(excelData, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "An error occurred while generating Excel.");
//                TempData["ErrorMessage"] = "An error occurred while generating Excel.";
//                return RedirectToPage();
//            }
//        }

//        public async Task<IActionResult> OnPostDeleteAsync()
//        {
//            try
//            {
//                var d = MovieId;
//                await _service.DeleteAsync(MovieId);
//                return new JsonResult(new { success = true });
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "An error occurred while deleting the movie.");
//                return new JsonResult(new { success = false, message = "An error occurred while deleting the movie." });
//            }
//        }

//    }
//}
