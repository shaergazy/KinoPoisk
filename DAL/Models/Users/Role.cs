using System.Collections.Generic;
using Common.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace DAL.Entities.Users;

public class Role : IdentityRole, IIdHas<string>
{
    public ICollection<UserRole> Users { get; set; }
}
