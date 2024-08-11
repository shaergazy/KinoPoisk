using DAL.Enums;
using DAL.Models.Users;

namespace DAL.Models
{
    public class Subscription
    {
        public Guid Id { get; set; }
        public string UserId { get; set; }
        public User User { get; set; }
        public string SubscriptionId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool IsActive { get; set; }
        public int SubscriptionPlanId { get; set; }
        public SubscriptionPlan Plan { get; set; }
    }
}
