using DAL.Models.Users;
using IdentityModel;
using IdentityServer4.Models;
using IdentityServer4.Validation;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace IdentityServer
{
    public class ResourceOwnerPasswordValidator : IResourceOwnerPasswordValidator
    {
        private readonly UserManager<User> _userManager;

        public ResourceOwnerPasswordValidator(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public async Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
        {
            var user = await _userManager.FindByNameAsync(context.UserName);
            if (user != null && await _userManager.CheckPasswordAsync(user, context.Password))
            {
                context.Result = new GrantValidationResult(subject: user.Id.ToString(), authenticationMethod: "password", claims: await GetUserClaims(user));
            }
            else
            {
                context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant, "Invalid credentials");
            }
        }

        private async Task<IEnumerable<Claim>> GetUserClaims(User user)
        {
            var roles = await _userManager.GetRolesAsync(user);

          
            var claims = new List<Claim>
            {
                new Claim(JwtClaimTypes.Name, user.UserName),
                // Add other claims as needed
            };

            foreach (var role in roles)
            {
                claims.Add(new Claim(JwtClaimTypes.Role, role));
            }

            var userClaims = await _userManager.GetClaimsAsync(user);
            claims.AddRange(userClaims);

            return claims;
        }
    }

}
