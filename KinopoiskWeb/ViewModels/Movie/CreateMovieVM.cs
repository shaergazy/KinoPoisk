namespace KinopoiskWeb.ViewModels.Movie
{
    public class CreateMovieVM
    {
        public IFormFile Poster { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime ReleasedDate { get; set; }
        public int? CountryId { get; set; }
        public List<int>? GenreIds { get; set; }
        public uint? Duration { get; set; }
        public float? IMDBRating { get; set; }
        public int DirectorId { get; set; }
        public List<ActorVM>? Actors { get; set; }
        public string ActorsJson { get; set; }
    }

    public class ActorVM
    {
        public int PersonId { get; set; }
        public uint Order { get; set; }
    }
}
