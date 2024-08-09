using BLL.DTO.SubscriptionPlan;
using DAL.Models;

namespace BLL.Services.Interfaces
{
    public interface ISubscriptionPlanService : IGenericService<ListSubscriptionPlanDto, AddSubscriptionPlanDto, EditSubscriptionPlanDto, GetSubscriptionPlanDto,SubscriptionPlan, int>
    {
    }
}
