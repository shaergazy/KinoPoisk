using DAL.Entities.Users;

namespace Data.Models
{
    public class UserRating
    {
        public Guid Id { get; set; }
        public User User { get; set; }
        public Rating Rating { get; set; }
    }
}
