using AutoMapper;
using BLL.Services;
using Common.Helpers;
using Common.Infrastructure;
using DAL;
using DAL.Entities.Users;
using Data.Repositories.RepositoryInterfaces;
using DTO;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Repositories;
using System.Configuration;
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

        internal static void RegisterServices(this IServiceCollection services)
        {
            services.AddScoped<AppDbContext>();

            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddTransient<AuthService>();
        }

        /// <summary>
        /// конфигурация маппера  
        /// </summary>
        /// <param name="services"></param>
        public static void ConfigMapper(this IServiceCollection services)
        {
            services.AddSingleton(_ => new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new AutoMapperProfile());
            }).CreateMapper());
        }

        public static IServiceCollection AddCommonServices(this IServiceCollection services, IConfiguration configuration)
        {
            RegisterConnectionString(services, configuration);
            RegisterAuth(services);
            RegisterJwtAuthorization(services, configuration);
            RegisterServiceUri(services, configuration);
            ConfigMapper(services);
            RegisterServices(services);

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
