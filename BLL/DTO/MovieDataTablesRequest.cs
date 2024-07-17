namespace BLL.DTO
{
    public class MovieDataTablesRequest : DataTablesRequestDto
    {
        public string Title { get; set; }
        public int? Year { get; set; }
        public int? CountryId { get; set; }
        public string Actor { get; set; }
        public string Director { get; set; }
    }
}
