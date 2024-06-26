namespace DAL.Entities
{
    public class Movie
    {
        public int Id { get; set; }
        public string Poster { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime RealesedYear {  get; set; }
        public int CountryId { get; set; }
        public Country Country { get; set; }
        public ICollection<Genre> Genres { get; set; }
        public int DirecrorId { get; set; }
        public Director Director { get; set; }
        public ICollection<Actor> Actors { get; set; }
        public int Duration { get; set; }
        public float IMDBRating { get; set; }
        public float Rating { get; set;}
    }
}
