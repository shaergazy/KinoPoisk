using BLL.Services.Implementation;
using BLL.Services.Interfaces;
using Common.Helpers;
using Common.Infrastructure;
using DAL;
using DAL.Models.Users;
using Data.Repositories.RepositoryInterfaces;
using DTO;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Repositories;
using System.Data;
namespace Common.CommonServices
{
    public static class Services
    {
        internal static void RegisterConnectionString(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AppDbContext>(x => x.UseSqlServer(configuration.GetConnectionString("Default")));
        }

        internal static void RegisterAuth(this IServiceCollection services)
        {
            services.AddIdentityCore<User>(x =>
            {
                x.Password.RequiredLength = 6;
                x.Password.RequireLowercase = false;
                x.Password.RequireUppercase = false;
                x.Password.RequireNonAlphanumeric = false;
                x.Password.RequireDigit = false;
            })
                .AddRoles<Role>()
                .AddEntityFrameworkStores<AppDbContext>()
                .AddDefaultTokenProviders();
        }

        internal static void RegisterJwtAuthorization(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
                {
                    options.RequireHttpsMetadata = false;
                    options.SaveToken = true;
                    options.TokenValidationParameters = JwtBuilder.Parameters(configuration);
                });
        }

        internal static void RegisterServiceUri(this IServiceCollection services, IConfiguration configuration)
        {
            var settings = configuration.GetSection(nameof(SettingsDto.ServiceUri)).Get<SettingsDto.ServiceUri>();
            AppConstants.BaseUri = settings.Self.BaseUri;
        }
        internal static void RegisterIOptions(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<SettingsDto.Mail>(x => configuration.GetSection("EmailSettings").Bind(x));
            services.Configure<SettingsDto.AuthorizeNetOptions>(x => configuration.GetSection("AuthorizeNet").Bind(x));
        }

        internal static void RegisterServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<DbContext>(sp => sp.GetRequiredService<AppDbContext>());
            services.RegisterIOptions(configuration);
            services.AddTransient<IEmailService, EmailService>();

            services.AddScoped(typeof(IGenericRepository<,>), typeof(GenericRepository<,>));
            services.AddScoped(typeof(IUnitOfWork<,>), typeof(UnitOfWork<,>));

            services.AddTransient<AuthService>();
            services.AddTransient(typeof(IGenericService<,,,,,>), typeof(GenericService<,,,,,>));
            services.AddTransient(typeof(ISearchableService<,,,,,,>), typeof(SearchableService<,,,,,,>));
            services.AddTransient<ICountryService, CountryService>();
            services.AddTransient<IGenreService, GenreService>();
            services.AddTransient<IPersonService, PersonService>();
            services.AddTransient<IAuthorizeNetService, AuthorizeNetService>();
            services.AddTransient<ISubscriptionService, SubscriptionService>();
            services.AddSingleton<OMDBService>(serviceProvider =>
            {
                var logger = serviceProvider.GetRequiredService<ILogger<OMDBService>>();
                return new OMDBService("fc8e73bf", logger);
            });
        }

        /// <summary>
        /// конфигурация маппера  
        /// </summary>
        /// <param name="services"></param>

        public static IServiceCollection AddCommonServices(this IServiceCollection services, IConfiguration configuration)
        {
            RegisterConnectionString(services, configuration);
            RegisterAuth(services);
            RegisterJwtAuthorization(services, configuration);
            RegisterServiceUri(services, configuration);
            RegisterServices(services, configuration);

            var assembly = typeof(IService).Assembly;
            var serviceInterfaces = assembly
                .GetTypes()
                .Where(t => typeof(IService).IsAssignableFrom(t) && t != typeof(IService) && t.IsInterface);
            var servicePairs = serviceInterfaces
                .Select(x => new
                {
                    serviceInterface = x,
                    serviceImplementaions = assembly
                        .GetTypes()
                        .Where(t => x.IsAssignableFrom(t) && t.IsClass)
                        .ToList()
                });
            foreach (var servicePair in servicePairs)
            {
                foreach (var implementation in servicePair.serviceImplementaions)
                {
                    services.AddTransient(servicePair.serviceInterface, implementation);
                }
            }

            return services;
        }
    }
}
