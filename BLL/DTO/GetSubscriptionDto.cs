namespace BLL.DTO
{
    public class GetSubscriptionDto
    {
        public Guid Id { get; set; }
        public string SubscriptionId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool IsActive { get; set; }
        public string SubscriptionPlanName { get; set; }
        public decimal Cost { get; set; }
    }
}
