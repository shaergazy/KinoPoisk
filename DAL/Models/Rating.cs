using DAL.Entities;

namespace Data.Models
{
    public class Rating
    {
        public int Id { get; set; }
        public float UserRating { get; set; }
        public string MovieId { get; set; }
        public Movie Movie { get; set; }
        public ICollection<UserRating> Users { get; set; }
    }
}
