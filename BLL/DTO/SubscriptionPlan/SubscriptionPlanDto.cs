using Common.Interfaces;
using DAL.Enums;
using System.ComponentModel.DataAnnotations;

namespace BLL.DTO.SubscriptionPlan
{
    public class Base
    {
        [Required]
        public string Name { get; set; }
        public decimal Cost { get; set; }
        public IntervalType IntervalType { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
    public class IdHasBase : Base, IIdHas<int>
    {
        public int Id { get; set; }
    }
    public class AddSubscriptionPlanDto : Base { }
    public class EditSubscriptionPlanDto : IdHasBase { }
    public class DeleteSubscriptionPlanDto : IdHasBase { }
    public class GetSubscriptionPlanDto : IdHasBase { }
    public class ListSubscriptionPlanDto : IdHasBase { }
}
