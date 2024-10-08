﻿using DAL;
using DAL.Models.Users;
using IdentityServer;
using IdentityServer4.Services;
using IdentityServer4.Validation;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        ConfigureServices(builder.Services, builder.Configuration);
        var app = builder.Build();

        Configure(app);

        app.Run();
    }

    static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

        services.AddIdentity<User, Role>()
            .AddEntityFrameworkStores<AppDbContext>()
            .AddDefaultTokenProviders();

        var connectionString = configuration.GetConnectionString("DefaultConnection");

        services.AddIdentityServer()
            .AddDeveloperSigningCredential()
            .AddResourceOwnerValidator<ResourceOwnerPasswordValidator>()
            .AddConfigurationStore(options =>
            {
                options.ConfigureDbContext = builder =>
                    builder.UseSqlServer(connectionString, sql => sql.MigrationsAssembly(typeof(Program).Assembly.FullName));
            })
            .AddOperationalStore(options =>
            {
                options.ConfigureDbContext = builder =>
                    builder.UseSqlServer(connectionString, sql => sql.MigrationsAssembly(typeof(Program).Assembly.FullName));

                options.EnableTokenCleanup = true;
                options.TokenCleanupInterval = 3600; 
            })
            .AddAspNetIdentity<User>();

        services.AddAuthorization(options =>
        {
        });

        services.AddControllers();
        services.AddMvc();

        services.AddScoped<IResourceOwnerPasswordValidator, ResourceOwnerPasswordValidator>();
        services.AddTransient<IProfileService, CustomProfileService>();
    }


    static void Configure(WebApplication app)
    {
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Error");
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.UseRouting();

        app.UseIdentityServer();
        app.UseAuthentication();
        app.UseAuthorization();
        SeedData.EnsureSeedData(app);
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapDefaultControllerRoute();
        });
    }
}
