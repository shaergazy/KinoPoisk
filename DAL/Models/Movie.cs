using Data.Models;

namespace DAL.Models
{
    public class Movie : TranslatableEntity
    {
        public string Poster { get; set; }
        public DateTime ReleasedDate {  get; set; }
        public Guid? CountryId { get; set; }
        public Country Country { get; set; }
        public ICollection<MovieGenre> Genres { get; set; }
        public ICollection<Comment> Comments { get; set; }
        public ICollection<MoviePerson> People { get; set; }
        public uint? Duration { get; set; }
        public float? IMDBRating { get; set; }
        public ICollection<MovieRating> Ratings { get; set;}
        public float Rating { get; set; }  
    }
}