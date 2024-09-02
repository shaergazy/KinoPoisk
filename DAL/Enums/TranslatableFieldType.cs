using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Enums
{
    public enum TranslatableFieldType
    {
        [Description("Name")]
        Name = 0,
        [Description("Description")]
        Description = 1,
    }
}
