using DAL.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace DAL.Models.Users;

public class Role : IdentityRole, IIdHas<string>
{
    public ICollection<UserRole> Users { get; set; }
}
