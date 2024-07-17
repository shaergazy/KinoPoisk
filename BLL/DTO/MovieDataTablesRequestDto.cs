namespace BLL.DTO
{
    public class MovieDataTablesRequestDto : DataTablesRequestDto
    {
        public string Title { get; set; }
        public int? Year { get; set; }
        public int? CountryId { get; set; }
        public string Actor { get; set; }
        public string Director { get; set; }
    }
}
