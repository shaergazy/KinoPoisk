namespace BLL.DTO
{
    public class DataTableModel
    {
        public string draw {  get; set; }
        public string start { get; set; }
        public string length { get; set; }
        public string sortColumn { get; set; }
        public string sortColumnDirection { get; set; }
        public string searchValue { get; set; }
        public int pageSize { get; set; }
        public int skip { get; set; }
        public int recordsTotal = 0;
    }
}
