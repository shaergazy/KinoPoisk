using AutoMapper;
using BLL;
using BLL.DTO;
using BLL.Services.Interfaces;
using DAL.Models;
using KinopoiskWeb.DataTables;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OfficeOpenXml;
using QuestPDF.Fluent;

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
            var movies = await _service.SearchAsync(dto); // Use all data directly from the request

            var document = new ListMoviePdfDocument(movies.Data.ToList());
            using (var ms = new MemoryStream())
            {
                document.GeneratePdf(ms);
                return File(ms.ToArray(), "application/pdf", "MoviesReport.pdf");
            }
        }

        public async Task<IActionResult> OnPostGenerateExcelAsync()
        {
            var dto = _mapper.Map<MovieDataTablesRequestDto>(DataTablesRequest);
            var response = await _service.SearchAsync(dto);
            var movies = response.Data;

            using (var ms = new MemoryStream())
            {
                using (var package = new ExcelPackage(ms))
                {
                    var worksheet = package.Workbook.Worksheets.Add("Movies");

                    // Headers
                    worksheet.Cells[1, 1].Value = "Title";
                    worksheet.Cells[1, 2].Value = "Description";
                    worksheet.Cells[1, 3].Value = "Released Date";
                    worksheet.Cells[1, 4].Value = "Duration";
                    worksheet.Cells[1, 5].Value = "IMDB Rating";
                    worksheet.Cells[1, 6].Value = "Rating";

                    // Content
                    for (int i = 0; i < movies.Count; i++)
                    {
                        var movie = movies[i];
                        worksheet.Cells[i + 2, 1].Value = movie.Title;
                        worksheet.Cells[i + 2, 2].Value = movie.Description;
                        worksheet.Cells[i + 2, 3].Value = movie.ReleasedDate.ToString("dd-MM-yyyy");
                        worksheet.Cells[i + 2, 4].Value = movie.Duration;
                        worksheet.Cells[i + 2, 5].Value = movie.IMDBRating;
                        worksheet.Cells[i + 2, 6].Value = movie.Rating;
                    }
                    package.Save();
                }
                return File(ms.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
            }
        }

    }

}
