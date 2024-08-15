using IdentityModel;
using IdentityServer4.Models;

namespace IdentityServer
{
    public static class Config
    {
        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new List<IdentityResource>
        {
            new IdentityResources.OpenId(),
            new IdentityResources.Profile(),
        };
        }

        public static IEnumerable<ApiScope> GetApiScopes()
        {
            return new List<ApiScope>
        {
            new ApiScope("api1", "My API")
        };
        }

        public static IEnumerable<ApiResource> GetApiResources()
        {
            return new List<ApiResource>
        {
            new ApiResource("api1", "My API")
            {
                Scopes = { "api1" },
                UserClaims = {JwtClaimTypes.Name, JwtClaimTypes.Role},
            }
        };
        }

        public static IEnumerable<Client> GetClients()
        {
            return new List<Client>
        {
            new Client
            {
                ClientId = "webapp",
                ClientName = "Web Application",
                AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                ClientSecrets = { new Secret("secret".Sha256()) },
                RedirectUris = { "https://localhost:44334/Account/Register" },
                PostLogoutRedirectUris = { "https://localhost:44334/Account/Logout" },
                AllowedScopes = { "openid", "profile", "api1" },
                AllowOfflineAccess = true
            },

             new Client
            {
                ClientId = "webApi",
                ClientName = "Web Application",
                AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                ClientSecrets = { new Secret("secret".Sha256()) },
                RedirectUris = { "https://localhost:44334/Account/Register" },
                PostLogoutRedirectUris = { "https://localhost:44334/Account/Logout" },
                AllowedScopes = { "openid", "profile", "api1" },
                AllowOfflineAccess = true
            }
        };
        }
    }

}
