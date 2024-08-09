using System.ComponentModel;

namespace DAL.Enums
{
    public enum SubscriptionPlan
    {
        [Description("Monthly")]
        Monthly = 1,
        [Description("Yearly")]
        Yearly = 2,
    }
}
