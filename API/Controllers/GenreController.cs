using API.Infrastructure;
using BLL.Services.Interfaces;
using Common.DTO;
using Common.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
        public async Task<int> Create([FromBody] GenreDto.Base dto)
        {
            if (dto == null) throw new ArgumentNullException(nameof(dto));
            return await _service.Create(dto);
        }

        /// <summary>
        /// GET all Genres
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        [ProducesResponseType(typeof(List<GenreDto.IdHasBase>), 200)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<List<GenreDto.IdHasBase>> GetAll()
        {
            return await _service.GetAll();
        }

        /// <summary>
        /// Edit Genre
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPut]
        [AuthorizeRoles(RoleType.Admin)]
        [ProducesResponseType(204)]
        public async Task Edit(GenreDto.IdHasBase dto)
        {
            await _service.EditById(dto);
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
            await _service.DeleteById(id);
        }
    }
}
