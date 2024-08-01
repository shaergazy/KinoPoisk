using Common.CommonServices;
using DAL.Models.Users;
using Hangfire;
using KinopoiskWeb.Extensions;
using KinopoiskWeb.Infrastructure;
using Microsoft.AspNetCore.Authentication.Cookies;
using OfficeOpenXml;
using QuestPDF.Infrastructure;
using Serilog;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.Host.UseSerilog((context, loggerConfig) =>
        {
            loggerConfig.ReadFrom.Configuration(context.Configuration);
        });

        Log.Information("Starting the application");

        builder.Services.AddRazorPages();
        builder.Services.AddCommonServices(builder.Configuration);
        builder.Services.AddHangfire(x => x.UseSqlServerStorage(builder.Configuration.GetConnectionString("Default")));
        builder.Services.AddHangfireServer();

        builder.Services.AddIdentity<User, Role>(options =>
        {
            options.Password.RequireDigit = false;
            options.Password.RequireLowercase = false;
            options.Password.RequireUppercase = false;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequiredLength = 6;
        });

        builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(opt =>
            {
                opt.LoginPath = "/login";
                opt.LogoutPath = "/logout";
                opt.AccessDeniedPath = "/Account/AccessDenied";
            });

        builder.Services.AddAntiforgery(options =>
        {
            options.HeaderName = "RequestVerificationToken";
        });

        builder.Services.AddMemoryCache();

        QuestPDF.Settings.License = LicenseType.Community;
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        builder.Services.ConfigMapper();
        builder.Services.AddHttpContextAccessor();

        var app = builder.Build();

        // Configure the HTTP request pipeline
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Error");
            app.UseHsts();
            Log.Information("Production environment detected, configured exception handling and HSTS");
        }
        else
        {
            Log.Information("Development environment detected");
        }

        app.UseHttpsRedirection();
        app.RegisterVirtualDir(builder.Configuration);
        app.UseStaticFiles();
        app.UseRouting();
        app.UseAuthentication();
        app.UseAuthorization();

        app.MapRazorPages();
        app.UseSerilogRequestLogging();
        app.InitializeDatabase();
        app.UseHangfireDashboard("/dashboard");

        Log.Information("Application started successfully");

        app.Run();
    }
}
