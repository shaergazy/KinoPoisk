using DAL.Models;
using DAL.Models.Users;
using System.ComponentModel.DataAnnotations;

namespace Data.Models
{
    public class MovieRating
    {
        [Key]
        public int Id { get; set; }
        public Guid MovieId { get; set; }
        public Movie Movie { get; set; }
        public Guid UserId { get; set; }
        public User User { get; set; }
        public int StarCount { get; set; } 
    }
}
