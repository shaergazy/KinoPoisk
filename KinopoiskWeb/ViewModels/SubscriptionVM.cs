﻿namespace KinopoiskWeb.ViewModels
{
    public class SubscriptionVM
    {
        public string PlanName { get; set; }
        public decimal Amount { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool IsActive { get; set; }
        public DateTime? NextBillingDate { get; set; }
    }
}
