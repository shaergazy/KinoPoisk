//using API.Infrastructure;
//using BLL.Services.Interfaces;
//using Common.Enums;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc;
//using BLL.DTO.SubscriptionPlan;

//namespace API.Controllers
//{
//    ///<summary>
//    /// Applications controller
//    ///</summary>
//    /// <response code="400">Error in model data</response>
//    /// <response code="401">Unauthorized</response>
//    /// <response code="500">Uncatched, unknown error</response>
//    /// <response code="403">Access denied</response>
//    [Route("api/[controller]")]
//    [ApiController]
//    [ProducesResponseType(StatusCodes.Status400BadRequest)]
//    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
//    public class SubscriptionPlanController : ControllerBase
//    {
//        private readonly ISubscriptionPlanService _service;

//        public SubscriptionPlanController(ISubscriptionPlanService service)
//        {
//            _service = service;
//        }

//        /// <summary>
//        /// Add SubscriptionPlan
//        /// </summary>
//        /// <param name="dto"></param>
//        /// <returns></returns>
//        /// <exception cref="ArgumentNullException"></exception>
//        [HttpPost]
//        [AuthorizeRoles(RoleType.Admin)]
//        [ProducesResponseType(typeof(int), 200)]
//        public async Task<int> Create([FromBody] AddSubscriptionPlanDto dto)
//        {
//            if (dto == null) throw new ArgumentNullException(nameof(dto));
//            var subscriptionplan = await _service.CreateAsync(dto);
//            return subscriptionplan.Id;
//        }

//        /// <summary>
//        /// GET all SubscriptionPlans
//        /// </summary>
//        /// <returns></returns>
//        [HttpGet]
//        [AllowAnonymous]
//        [ProducesResponseType(typeof(List<ListSubscriptionPlanDto>), 200)]
//        [ProducesResponseType(StatusCodes.Status404NotFound)]
//        public async Task<List<ListSubscriptionPlanDto>> GetAll()
//        {
//            return _service.GetAll().ToList();
//        }

//        /// <summary>
//        /// Edit SubscriptionPlan
//        /// </summary>
//        /// <param name="dto"></param>
//        /// <returns></returns>
//        [HttpPut]
//        [AuthorizeRoles(RoleType.Admin)]
//        [ProducesResponseType(204)]
//        public async Task Edit(EditSubscriptionPlanDto dto)
//        {
//            await _service.UpdateAsync(dto);
//        }

//        /// <summary>
//        /// Delete SubscriptionPlan
//        /// </summary>
//        /// <param name="id"></param>
//        /// <returns></returns>
//        [HttpDelete]
//        [AuthorizeRoles(RoleType.Admin)]
//        [ProducesResponseType(204)]
//        public async Task DeleteById(int id)
//        {
//            await _service.DeleteAsync(id);
//        }
//    }
//}
