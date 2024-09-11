namespace BLL.DTO
{
    public class MovieDataTablesRequestDto : DataTablesRequestDto
    {
        public string Title { get; set; }
        public int? Year { get; set; }
        public Guid? Country { get; set; }
        public Guid? Actor { get; set; }
        public Guid? Director { get; set; }
    }
}
