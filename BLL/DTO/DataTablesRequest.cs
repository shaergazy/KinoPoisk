namespace BLL.DTO
{
    public class DataTablesRequestDto
    {
        public int Draw { get; set; }

        public string SortColumn { get; set; }

        public string SortDirection { get; set; }

        public int Start { get; set; }

        public int Length { get; set; }

        public string SearchTerm { get; set; }
    }
}
