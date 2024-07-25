using AutoMapper;
using BLL.DTO;
using BLL.DTO.Movie;
using BLL.Services.Interfaces;
using DAL.Models;
using KinopoiskWeb.DataTables;
using KinopoiskWeb.ViewModels.Movie;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;

namespace KinopoiskWeb.Pages.Movies
{
    [IgnoreAntiforgeryToken]
    public class DetailsModel : PageModel
    {
        private readonly IMovieService _movieService;
        private readonly IMapper _mapper;

        public DetailsModel(IMovieService movieService, IMapper mapper)
        {
            _movieService = movieService;
            _mapper = mapper;
        }

        public DetailsMovieVM Movie { get; private set; }
        public async Task<IActionResult> OnGetAsync(Guid id)
        {
            Movie = _mapper.Map<DetailsMovieVM>(await _movieService.GetByIdAsync(id));

            if (Movie == null)
            {
                return NotFound();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostRateAsync([FromBody] RateMovieVM model)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                if (userId == null)
                {
                    return new JsonResult(new { success = false, redirect = Url.Page("/Account/Register") });
                }
            }

            model.UserId = Guid.Parse(userId);

            await _movieService.AddRatingAsync(_mapper.Map<AddMovieRating>(model));
            return new JsonResult(new { success = true });
        }

        public async Task<JsonResult> OnPostLoadCommentsAsync(Guid id, DataTablesRequest request)
        {
            var comments = await _movieService.GetCommentsAsync(id, request.Start, request.Length);

            var response = new DataTablesResponse<GetCommentDto>
            {
                Draw = request.Draw,
                Data = comments.ToList(),
            };

            return new JsonResult(response);
        }

        [Authorize]
        public async Task<IActionResult> OnPostAddCommentAsync([FromBody] AddCommentVM model)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return new JsonResult(new { success = false, redirect = Url.Page("/Account/Register") });
            }

            model.UserId = Guid.Parse(userId);
            await _movieService.AddCommentAsync(_mapper.Map<AddCommentDo>(model));

            return new JsonResult(new { success = true });
        }
    }
}
