using AutoMapper;
using BLL.DTO;
using BLL.DTO.Movie;
using BLL.Services.Interfaces;
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
        private readonly ILogger<DetailsModel> _logger;

        public DetailsModel(IMovieService movieService, IMapper mapper, ILogger<DetailsModel> logger)
        {
            _movieService = movieService;
            _mapper = mapper;
            _logger = logger;
        }

        public DetailsMovieVM Movie { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid id)
        {
            try
            {

                Movie = await GetMovieDetailsAsync(id);

                _logger.LogInformation("Movie details loaded for id: {Id}", id);
                return Page();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while loading movie details for id: {Id}", id);
                return StatusCode(500, "Internal server error");
            }
        }

        public async Task<DetailsMovieVM> GetMovieDetailsAsync(Guid id)
        {
            var movieDto = await _movieService.GetByIdAsync(id);

            if (movieDto == null)
            {
                _logger.LogWarning("Movie not found with id: {Id}", id);
                throw new Exception("Movie not found");
            }
            return _mapper.Map<DetailsMovieVM>(movieDto);
        }

        public async Task<IActionResult> OnPostRateAsync([FromBody] RateMovieVM model)
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                if (string.IsNullOrEmpty(userId) || !Guid.TryParse(userId, out var parsedUserId))
                {
                    _logger.LogWarning("User not logged in or invalid UserId while trying to rate a movie.");
                    return new JsonResult(new { success = false, redirect = Url.Page("/Account/Register") });
                }

                model.UserId = parsedUserId;
                await _movieService.AddRatingAsync(_mapper.Map<AddMovieRating>(model));

                _logger.LogInformation("User {UserId} rated movie {MovieId} with rating {Rating}", userId, model.MovieId, model.StarCount);
                return new JsonResult(new { success = true });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while rating the movie.");
                return StatusCode(500, "Internal server error");
            }
        }

        public async Task<JsonResult> OnPostLoadCommentsAsync(Guid id, DataTablesRequest request)
        {
            try
            {
                Movie = await GetMovieDetailsAsync(id);
                var response = await _movieService.GetCommentsAsync(id, _mapper.Map<DataTablesRequestDto>(request));
                var viewModel = _mapper.Map<DataTablesResponseVM<GetCommentDto>>(response);

                _logger.LogInformation("Loaded comments for movie id: {Id}", id);
                return new JsonResult(viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while loading comments for movie id: {Id}", id);
                return new JsonResult(new { success = false, error = "Internal server error" });
            }
        }

        [Authorize]
        public async Task<IActionResult> OnPostAddCommentAsync([FromBody] AddCommentVM model)
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                if (string.IsNullOrEmpty(userId))
                {
                    _logger.LogWarning("User not logged in while trying to add a comment.");
                    return new JsonResult(new { success = false, redirect = Url.Page("/Account/Register") });
                }

                model.UserId = userId;
                await _movieService.AddCommentAsync(_mapper.Map<AddCommentDto>(model));

                _logger.LogInformation("User {UserId} added a comment to movie {MovieId}", userId, model.MovieId);
                return new JsonResult(new { success = true });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while adding a comment.");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
