namespace KinopoiskWeb.ViewModels.Movie
{
    public class CreateMovieVM
    {
        public IFormFile Poster { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime ReleasedDate { get; set; } = DateTime.Now;
        public Guid? CountryId { get; set; }
        public List<Guid>? GenreIds { get; set; }
        public uint? Duration { get; set; }
        public float? IMDBRating { get; set; }
        public Guid DirectorId { get; set; }
        public List<ActorVM>? Actors { get; set; }
    }

    public class ActorVM
    {
        public Guid PersonId { get; set; }
        public uint Order { get; set; }
    }
}
