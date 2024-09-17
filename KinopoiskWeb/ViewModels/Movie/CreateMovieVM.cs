namespace KinopoiskWeb.ViewModels.Movie
{
    public class CreateMovieVM
    {
        public IFormFile Poster { get; set; }
        public ICollection<TranslationVM> Translations { get; set; }
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
