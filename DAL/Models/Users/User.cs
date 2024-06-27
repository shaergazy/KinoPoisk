using Common.Interfaces;
using Data.Models;
using Microsoft.AspNetCore.Identity;

namespace DAL.Entities.Users;

public class User : IdentityUser, IIdHas<string>
{
    public string FirstName { get; set; }

    public string LastName { get; set; }
    public ICollection<UserRole> Roles { get; set; }
    public ICollection<UserRating> Ratings { get; set; }
}
