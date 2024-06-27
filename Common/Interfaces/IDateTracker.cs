using System;

namespace Common.Interfaces
{
    public interface IDateTracker
    {
        DateTime CreatedDateTime { get; set; }

        DateTime? EditedDateTime { get; set; }
    }
}
