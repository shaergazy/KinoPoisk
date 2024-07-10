using DAL.Models;
using Data.Models;
using System.ComponentModel.DataAnnotations;

namespace DAL.Models
{
    public class Movie
    {
        [Key]
        public Guid Id { get; set; }
        public string Poster { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime ReleasedDate {  get; set; }
        public int? CountryId { get; set; }
        public Country Country { get; set; }
        public ICollection<MovieGenre> Genres { get; set; }
        public ICollection<Comment> Comments { get; set; }
        public ICollection<MoviePerson> People { get; set; }
        public uint? Duration { get; set; }
        public float? IMDBRating { get; set; }
        public ICollection<MovieRating> Ratings { get; set;}
    }
}
