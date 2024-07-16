namespace KinopoiskWeb.ViewModels.Movie
{
    public class DetailsMovieVM
    {
            public Guid Id { get; set; }
            public string Poster { get; set; }
            public string Title { get; set; }
            public string Description { get; set; }
            public DateTime ReleasedDate { get; set; }
            public string Country { get; set; }
            public string[] Genres { get; set; }
            public string[] Comments { get; set; }
            public string[] Actors { get; set; }
            public string Director {  get; set; }
            public uint? Duration { get; set; }
            public float? IMDBRating { get; set; }
            public float Rating { get; set; }
    }
}
