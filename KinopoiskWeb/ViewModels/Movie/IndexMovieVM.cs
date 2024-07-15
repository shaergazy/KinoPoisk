namespace KinopoiskWeb.ViewModels.Movie
{
    public class IndexMovieVM
    {
        public string Poster { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string? Director { get; set; }
        public uint Duration { get; set; }
        public string[] Actors { get; set; }
    }
}
