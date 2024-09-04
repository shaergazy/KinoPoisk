using DAL.Enums;

namespace BLL.DTO
{
    public class DataTablesRequestDto
    {
        public int Draw { get; set; }

        public string SortColumn { get; set; } = "Id";

        public string SortDirection { get; set; } = "desc";

        public int Start { get; set; }

        public int Length { get; set; }

        public string SearchTerm { get; set; }

        public LanguageCode LanguageCode { get; set; }
    }
}
