using Data.Models;

namespace DAL.Entities
{
    public class Movie
    {
        public Guid Id { get; set; }
        public string Poster { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime RealesedDate {  get; set; }
        public int CountryId { get; set; }
        public Country Country { get; set; }
        public ICollection<Genre> Genres { get; set; }
        public ICollection<Comment> Comments { get; set; }
        public ICollection<MoviePerson> People { get; set; }
        public int Duration { get; set; }
        public float IMDBRating { get; set; }
        public ICollection<MovieRating> Ratings { get; set;}
    }
}
