using DAL.Entities;
using DAL.Entities.Users;

namespace Data.Models
{
    public class MovieRating
    {
        public int Id { get; set; }
        public Guid MovieId { get; set; }
        public Movie Movie { get; set; }
        public Guid UserId { get; set; }
        public User User { get; set; }
        public int StarCount { get; set; } 
    }
}
