using DAL.Enums;
using DAL.Models;
using DAL.Models.Users;
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
            if (await userManager.Users.AllAsync(us => us.Email != "admin@kinopoisk.com"))
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
            new Country
            {
                ShortName = "kg",
                FlagLink = "https://flagpedia.net/data/flags/h80/kg.png",
                IsOwnPicture = false,
                Translations = new List<TranslatableEntityField>
                {
                    new TranslatableEntityField { LanguageCode = LanguageCode.en, FieldType = TranslatableFieldType.Name, Value = "Kyrgyzstan" },
                    new TranslatableEntityField { LanguageCode = LanguageCode.ru, FieldType = TranslatableFieldType.Name, Value = "Кыргызстан" }
                }
            },
            new Country
            {
                ShortName = "ru",
                FlagLink = "https://flagpedia.net/data/flags/h80/ru.png",
                IsOwnPicture = false,
                Translations = new List<TranslatableEntityField>
                {
                    new TranslatableEntityField { LanguageCode = LanguageCode.en, FieldType = TranslatableFieldType.Name, Value = "Russia" },
                    new TranslatableEntityField { LanguageCode = LanguageCode.ru, FieldType = TranslatableFieldType.Name, Value = "Россия" }
                }
            },
            new Country
            {
                ShortName = "jp",
                FlagLink = "https://flagpedia.net/data/flags/h80/jp.png",
                IsOwnPicture = false,
                Translations = new List<TranslatableEntityField>
                {
                    new TranslatableEntityField { LanguageCode = LanguageCode.en, FieldType = TranslatableFieldType.Name, Value = "Japan" },
                    new TranslatableEntityField { LanguageCode = LanguageCode.ru, FieldType = TranslatableFieldType.Name, Value = "Япония" }
                }
            },
            new Country
            {
                ShortName = "au",
                FlagLink = "https://flagpedia.net/data/flags/h80/au.png",
                IsOwnPicture = false,
                Translations = new List<TranslatableEntityField>
                {
                    new TranslatableEntityField { LanguageCode = LanguageCode.en, FieldType = TranslatableFieldType.Name, Value = "Australia" },
                    new TranslatableEntityField { LanguageCode = LanguageCode.ru, FieldType = TranslatableFieldType.Name, Value = "Австралия" }
                }
            },
            new Country
            {
                ShortName = "by",
                FlagLink = "https://flagpedia.net/data/flags/h80/by.png",
                IsOwnPicture = false,
                Translations = new List<TranslatableEntityField>
                {
                    new TranslatableEntityField { LanguageCode = LanguageCode.en, FieldType = TranslatableFieldType.Name, Value = "Belarus" },
                    new TranslatableEntityField { LanguageCode = LanguageCode.ru, FieldType = TranslatableFieldType.Name, Value = "Беларусь" }
                }
            },
            new Country
            {
                ShortName = "br",
                FlagLink = "https://flagpedia.net/data/flags/h80/br.png",
                IsOwnPicture = false,
                Translations = new List<TranslatableEntityField>
                {
                    new TranslatableEntityField { LanguageCode = LanguageCode.en, FieldType = TranslatableFieldType.Name, Value = "Brazil" },
                    new TranslatableEntityField { LanguageCode = LanguageCode.ru, FieldType = TranslatableFieldType.Name, Value = "Бразилия" }
                }
            },
            new Country
            {
                ShortName = "ca",
                FlagLink = "https://flagpedia.net/data/flags/h80/ca.png",
                IsOwnPicture = false,
                Translations = new List<TranslatableEntityField>
                {
                    new TranslatableEntityField { LanguageCode = LanguageCode.en, FieldType = TranslatableFieldType.Name, Value = "Canada" },
                    new TranslatableEntityField { LanguageCode = LanguageCode.ru, FieldType = TranslatableFieldType.Name, Value = "Канада" }
                }
            },
            new Country
            {
                ShortName = "fr",
                FlagLink = "https://flagpedia.net/data/flags/h80/fr.png",
                IsOwnPicture = false,
                Translations = new List<TranslatableEntityField>
                {
                    new TranslatableEntityField { LanguageCode = LanguageCode.en, FieldType = TranslatableFieldType.Name, Value = "France" },
                    new TranslatableEntityField { LanguageCode = LanguageCode.ru, FieldType = TranslatableFieldType.Name, Value = "Франция" }
                }
            },
            new Country
            {
                ShortName = "de",
                FlagLink = "https://flagpedia.net/data/flags/h80/de.png",
                IsOwnPicture = false,
                Translations = new List<TranslatableEntityField>
                {
                    new TranslatableEntityField { LanguageCode = LanguageCode.en, FieldType = TranslatableFieldType.Name, Value = "Germany" },
                    new TranslatableEntityField { LanguageCode = LanguageCode.ru, FieldType = TranslatableFieldType.Name, Value = "Германия" }
                }
            },
            new Country
            {
                ShortName = "in",
                FlagLink = "https://flagpedia.net/data/flags/h80/in.png",
                IsOwnPicture = false,
                Translations = new List<TranslatableEntityField>
                {
                    new TranslatableEntityField { LanguageCode = LanguageCode.en, FieldType = TranslatableFieldType.Name, Value = "India" },
                    new TranslatableEntityField { LanguageCode = LanguageCode.ru, FieldType = TranslatableFieldType.Name, Value = "Индия" }
                }
            },
            new Country
            {
                ShortName = "us",
                FlagLink = "https://flagpedia.net/data/flags/h80/us.png",
                IsOwnPicture = false,
                Translations = new List<TranslatableEntityField>
                {
                    new TranslatableEntityField { LanguageCode = LanguageCode.en, FieldType = TranslatableFieldType.Name, Value = "United States" },
                    new TranslatableEntityField { LanguageCode = LanguageCode.ru, FieldType = TranslatableFieldType.Name, Value = "Соединенные Штаты" }
                }
            }
        };
                var existingCountries = await _appDbContext.Countries
                .Include(c => c.Translations)
                .ToListAsync();

                foreach (var country in countries)
                {
                    if (existingCountries.Any(x => x.Translations.Any(t => t.Value == country.Translations.FirstOrDefault(t => t.FieldType == TranslatableFieldType.Name && t.LanguageCode == LanguageCode.en)?.Value)))
                        continue;

                    _appDbContext.Countries.Add(country);
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
