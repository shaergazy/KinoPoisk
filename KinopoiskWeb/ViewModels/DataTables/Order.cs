using System.ComponentModel;

namespace KinopoiskWeb.DataTables
{
    public class Order
    {
        [DefaultValue(0)]
        public int Column { get; set; }

        [DefaultValue("asc")]
        public string Dir { get; set; }
    }
}
