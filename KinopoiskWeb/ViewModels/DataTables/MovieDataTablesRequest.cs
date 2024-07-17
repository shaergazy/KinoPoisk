namespace KinopoiskWeb.DataTables
{
    public class MovieDataTablesRequest : DataTablesRequest
    {
        public string Title { get; set; }
        public DateTime? ReleasedDate { get; set; }
        public int? CountryId { get; set; }
        public string Actor { get; set; }
        public string Director { get; set; }
    }
}
