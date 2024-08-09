namespace BLL.DTO
{
    public class PaymentDetailsDto
    {
        public string CardNumber { get; set; }
        public string ExpirationDate { get; set; }
        public string CardCode { get; set; }
        public string UserId { get; set; } 
        public int PlanId { get; set; }
    }
}
