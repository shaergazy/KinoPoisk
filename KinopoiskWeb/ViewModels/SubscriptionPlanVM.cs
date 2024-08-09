using DAL.Enums;

namespace KinopoiskWeb.ViewModels
{
    public class SubscriptionPlanVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Cost { get; set; }
        public IntervalType IntervalType { get; set; }
    } 
}
