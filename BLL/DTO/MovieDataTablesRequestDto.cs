namespace BLL.DTO
{
    public class MovieDataTablesRequestDto : DataTablesRequestDto
    {
        public string Title { get; set; }
        public int? Year { get; set; }
        public int? Country { get; set; }
        public int? Actor { get; set; }
        public int? Director { get; set; }
    }
}
