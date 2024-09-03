using API.Infrastructure;
using BLL.Services.Interfaces;
using Common.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using BLL.DTO.Genre;

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
    public class GenreController : ControllerBase
    {
        private readonly IGenreService _service;

        public GenreController(IGenreService service)
        {
            _service = service;
        }

        /// <summary>
        /// Add Genre
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        [HttpPost]
        [AuthorizeRoles(RoleType.Admin)]
        [ProducesResponseType(typeof(int), 200)]
        public async Task<int> Create([FromBody] AddGenreDto dto)
        {
            //if (dto == null) throw new ArgumentNullException(nameof(dto));
            //var genre = await _service.CreateAsync(dto);
            //return genre.Id;
            return 1;
        }

        /// <summary>
        /// GET all Genres
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        [ProducesResponseType(typeof(List<ListGenreDto>), 200)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<List<ListGenreDto>> GetAll()
        {
            return  _service.GetAll().ToList();
        }

        /// <summary>
        /// Edit Genre
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPut]
        [AuthorizeRoles(RoleType.Admin)]
        [ProducesResponseType(204)]
        public async Task Edit(EditGenreDto dto)
        {
            await _service.UpdateAsync(dto);
        }

        /// <summary>
        /// Delete Genre
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        [AuthorizeRoles(RoleType.Admin)]
        [ProducesResponseType(204)]
        public async Task DeleteById(int id)
        {
            await _service.DeleteAsync(id);
        }
    }
}
