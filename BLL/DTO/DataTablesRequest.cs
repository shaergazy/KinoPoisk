namespace BLL.DTO
{
    public class DataTablesRequestDto
    {
        public int Draw { get; set; }

        public string Column { get; set; }

        public string Order { get; set; }

        public int Start { get; set; }

        public int Length { get; set; }

        public string SearchTerm { get; set; }
    }
}
