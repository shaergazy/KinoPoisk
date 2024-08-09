using AutoMapper;
using BLL.DTO.SubscriptionPlan;
using BLL.Services.Interfaces;
using DAL.Models;
using Data.Repositories.RepositoryInterfaces;
using Microsoft.Extensions.Logging;

namespace BLL.Services.Implementation
{
    public class SubscriptionPlanService : GenericService<ListSubscriptionPlanDto, AddSubscriptionPlanDto, EditSubscriptionPlanDto, GetSubscriptionPlanDto, SubscriptionPlan, int> , ISubscriptionPlanService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork<SubscriptionPlan, int> _uow;
        private readonly ILogger<SubscriptionPlanService> _logger;

        public SubscriptionPlanService(IMapper mapper, IUnitOfWork<SubscriptionPlan, int> unitOfWork, ILogger<SubscriptionPlanService> logger) : base(mapper, unitOfWork, logger)
        {
            _mapper = mapper;
            _uow = unitOfWork;
            _logger = logger;
        }

        public override async Task<SubscriptionPlan> BuildEntityForCreate(AddSubscriptionPlanDto dto)
        {
            if (dto.Cost == null || dto.Name == null)
            {
                _logger.LogWarning("Invalid AddSubscriptionPlanDto: Cost or Name is null");
                throw new ArgumentNullException("You have to complete all properties");
            }

            if (_uow.Repository.Any(x => x.Name == dto.Name))
            {
                _logger.LogWarning("Attempted to create an already existing SubscriptionPlan: {SubscriptionPlanName}", dto.Name);
                throw new Exception("SubscriptionPlan already exist");
            }

            var SubscriptionPlan = _mapper.Map<SubscriptionPlan>(dto);

            _logger.LogInformation("Created new SubscriptionPlan: {SubscriptionPlanName}", SubscriptionPlan.Name);

            return SubscriptionPlan;
        }

        public override async Task<SubscriptionPlan> BuildEntityForUpdate(EditSubscriptionPlanDto dto)
        {
            if (dto.Cost == null || dto.Name == null)
            {
                _logger.LogWarning("Invalid EditSubscriptionPlanDto: Cost or Name is null");
                throw new ArgumentNullException("You have to complete all properties");
            }

            if (_uow.Repository.Any(x => x.Name == dto.Name && x.Id != dto.Id))
            {
                _logger.LogWarning("Attempted to create an already existing SubscriptionPlan: {SubscriptionPlanName}", dto.Name);
                throw new Exception("SubscriptionPlan already exist");
            }

            var SubscriptionPlan = _mapper.Map<SubscriptionPlan>(dto);

            _logger.LogInformation("Updated new SubscriptionPlan: {SubscriptionPlanName}", SubscriptionPlan.Name);

            return SubscriptionPlan;
        }
    }
}
