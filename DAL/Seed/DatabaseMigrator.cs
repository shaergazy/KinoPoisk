using DAL.Entities;
using DAL.Entities.Users;
using DAL.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace DAL.Seed
{
    public class DatabaseMigrator
    {
        public static async Task SeedDatabaseAsync(IServiceProvider appServiceProvider)
        {
            await using var scope = appServiceProvider.CreateAsyncScope();
            var serviceProvider = scope.ServiceProvider;
            var logger = serviceProvider.GetRequiredService<ILogger<DatabaseMigrator>>();
            try
            {
                var _appDbContext = serviceProvider.GetRequiredService<AppDbContext>();
            }
            catch (Exception ex)
            {
                logger.LogCritical(ex, "Migration error");
            }

            try
            {
                var _appDbContext = serviceProvider.GetRequiredService<AppDbContext>();
                var roleManager = serviceProvider.GetRequiredService<RoleManager<Role>>();
                var userManager = serviceProvider.GetRequiredService<UserManager<User>>();
                await SeedRolesAsync(roleManager);
                await SeedAdminAsync(userManager);
                await SeedCountriesAsync(_appDbContext, logger);
            }
            catch (Exception ex)
            {
                logger.LogCritical(ex, "DB seed error");
            }
        }

        private static async Task SeedAdminAsync(UserManager<User> userManager)
        {
            if (await userManager.Users.AllAsync(us => us.Email != "admin@kfg.com"))
            {
                var admin = new User
                {
                    Email = "admin@kinopoisk.com",
                    UserName = "admin@kinopoisk.com",
                    EmailConfirmed = true,
                    PhoneNumber = "+10000000000",
                    PhoneNumberConfirmed = true,
                    FirstName = "Admin",
                    LastName = "Admin",
                    LockoutEnabled = false,
                };

                await userManager.CreateAsync(admin, "vrysmplpswd");
                await userManager.AddToRoleAsync(admin, nameof(RoleType.Admin));
            }

        }

        private static async Task SeedRolesAsync(RoleManager<Role> roleManager)
        {
            foreach (RoleType role in Enum.GetValues(typeof(RoleType)))
            {
                var normalizedRole = role.ToString();
                var dbRole = roleManager.Roles.FirstOrDefault(r => r.NormalizedName == normalizedRole);
                if (dbRole == null)
                {
                    var result = await roleManager.CreateAsync(new Role { Name = role.ToString() });
                    dbRole = roleManager.Roles.FirstOrDefault(r => r.NormalizedName == normalizedRole);
                }
            }
        }
        private static async Task SeedCountriesAsync(AppDbContext _appDbContext, ILogger logger)
        {
            try
            {
                var countries = new List<Country>
                {
                    new Country {Name = "Kyrgyzstan", ShortName = "kg", FlagLink = "https://flagpedia.net/data/flags/h80/kg.png"},
                    new Country {Name = "Russia", ShortName = "ru", FlagLink = "https://flagpedia.net/data/flags/h80/ru.png"},
                    new Country {Name = "Japan", ShortName = "jp", FlagLink = "https://flagpedia.net/data/flags/h80/jp.png"},
                    new Country {Name = "Australia", ShortName = "au", FlagLink = "https://flagpedia.net/data/flags/h80/au.png"},
                    new Country {Name = "Belarus", ShortName = "by", FlagLink = "https://flagpedia.net/data/flags/h80/by.png"},
                    new Country {Name = "Brazil", ShortName = "br", FlagLink = "https://flagpedia.net/data/flags/h80/br.png"},
                    new Country {Name = "Canada", ShortName = "ca", FlagLink = "https://flagpedia.net/data/flags/h80/ca.png"},
                    new Country {Name = "France", ShortName = "fr", FlagLink = "https://flagpedia.net/data/flags/h80/fr.png"},
                    new Country {Name = "Germany", ShortName = "de", FlagLink = "https://flagpedia.net/data/flags/h80/de.png"},
                    new Country {Name = "India", ShortName = "in", FlagLink = "https://flagpedia.net/data/flags/h80/in.png"},
                    new Country {Name = "United States", ShortName = "us", FlagLink = "https://flagpedia.net/data/flags/h80/us.png"},
                };

                if (!_appDbContext.Countries.Any())
                {
                    _appDbContext.Countries.AddRange(countries);
                }

                await _appDbContext.SaveChangesAsync();
            }
            catch (Exception exp)
            {
                logger.LogCritical(exp, "System couldn't seed countries");
            }
        }
    }
}
