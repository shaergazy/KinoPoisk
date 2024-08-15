using API.Infrastructure;
using BLL.Services.Interfaces;
using Common.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using BLL.DTO.Movie;

namespace API.Controllers
{
    ///<summary>
    /// Applications controller
    ///</summary>
    /// <response code="400">Error in model data</response>
    /// <response code="401">Unauthorized</response>
    /// <response code="500">Uncatched, unknown error</response>
    /// <response code="403">Access denied</response>
    [Route("api/[controller]")]
    [ApiController]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public class MovieController : ControllerBase
    {
        private readonly IMovieService _service;

        public MovieController(IMovieService service)
        {
            _service = service;
        }

        /// <summary>
        /// Add Movie
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        [HttpPost]
        [AuthorizeRoles(RoleType.Admin)]
        [ProducesResponseType(typeof(int), 200)]
        public async Task<Guid> Create([FromBody] AddMovieDto dto)
        {
            if (dto == null) throw new ArgumentNullException(nameof(dto));
            var movie = await _service.CreateAsync(dto);
            return movie.Id;
        }

        /// <summary>
        /// GET all Movies
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        [ProducesResponseType(typeof(List<ListMovieDto>), 200)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<List<ListMovieDto>> GetAll()
        {
            return _service.GetAll().ToList();
        }

        /// <summary>
        /// Edit Movie
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPut]
        [AuthorizeRoles(RoleType.Admin)]
        [ProducesResponseType(204)]
        public async Task Edit(EditMovieDto dto)
        {
            await _service.UpdateAsync(dto);
        }

        /// <summary>
        /// Delete Movie
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        [AuthorizeRoles(RoleType.Admin)]
        [ProducesResponseType(204)]
        public async Task DeleteById(Guid id)
        {
            await _service.DeleteAsync(id);
        }
    }
}
