using Microsoft.AspNetCore.Identity;

namespace DAL.Models.Users
{
    public class Role : IdentityRole
    {
        public ICollection<User> Users { get; set; }
    }
}
