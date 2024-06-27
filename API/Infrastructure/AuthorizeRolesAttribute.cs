//using Common.Enums;
//using Common.Extensions;
//using System.Linq;
//using Microsoft.AspNetCore.Authorization;

//namespace API.Infrastructure
//{
//    public class AuthorizeRolesAttribute : AuthorizeAttribute
//    {
//        public AuthorizeRolesAttribute(params RoleType[] allowedRoles)
//        {
//            Roles = string.Join(",", allowedRoles.Select(x => x.Description()));
//        }
//    }

//}
