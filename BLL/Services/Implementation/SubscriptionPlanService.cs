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
    }
}
