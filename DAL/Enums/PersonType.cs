using System.ComponentModel;

namespace DAL.Enums
{
    public enum PersonType
    {
        [Description("Director")]
        Director = 1,
        [Description("Actor")]
        Actor = 2,
    }
}
