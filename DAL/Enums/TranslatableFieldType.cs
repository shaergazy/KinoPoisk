using System.ComponentModel;

namespace DAL.Enums
{
    public enum TranslatableFieldType
    {
        [Description("Name")]
        Name = 0,
        [Description("Description")]
        Description = 1,
        [Description("Title")]
        Title = 2,
        [Description("FirstName")]
        FirstName = 3,
        [Description("Description")]
        LastName = 4,
    }
}
