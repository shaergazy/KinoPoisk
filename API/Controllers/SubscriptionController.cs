using API.Infrastructure;
using BLL.Services.Interfaces;
using Common.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using BLL.DTO;

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
    public class SubscriptionController : ControllerBase
    {
        private readonly ISubscriptionService _service;

        public SubscriptionController(ISubscriptionService service)
        {
            _service = service;
        }

        /// <summary>
        /// Add Subscription
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        [HttpPost]
        [AuthorizeRoles(RoleType.Admin)]
        [ProducesResponseType(typeof(int), 200)]
        public async Task<string> Create([FromBody] PaymentDetailsDto dto)
        {
            if (dto == null) throw new ArgumentNullException(nameof(dto));
            var subscription = await _service.CreateSubscriptionAsync(dto);
            return subscription;
        }

        /// <summary>
        /// Get Subscription by UserId
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        [ProducesResponseType(typeof(List<GetSubscriptionDto>), 200)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<GetSubscriptionDto> GetSubscriptionByUserId(string userId)
        {
            return await _service.GetSubscriptionByUserIdAsync(userId);
        }

        /// <summary>
        /// Cancel Subscription
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPatch]
        [AuthorizeRoles(RoleType.Admin)]
        [ProducesResponseType(204)]
        public async Task Edit(string subscriptionId)
        {
            await _service.CancelSubscriptionAsync(subscriptionId);
        }

    }
}
