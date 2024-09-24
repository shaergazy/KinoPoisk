using AuthorizeNet.Api.Contracts.V1;
using AuthorizeNet.Api.Controllers.Bases;
using Common.CommonServices;
using DAL.Models.Users;
using Hangfire;
using KinopoiskWeb.Extensions;
using KinopoiskWeb.Hubs;
using KinopoiskWeb.Infrastructure;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Extensions.Options;
using OfficeOpenXml;
using QuestPDF.Infrastructure;
using Serilog;
using System.Globalization;

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

        ConfigureServices(builder.Services, builder.Configuration);

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
        var localizationOptions = app.Services.GetService<IOptions<RequestLocalizationOptions>>().Value;
        app.UseRequestLocalization(localizationOptions);
        app.UseHttpsRedirection();
        app.RegisterVirtualDir(builder.Configuration);
        app.UseStaticFiles();
        app.UseHangfireDashboard("/dashboard");

        app.UseRouting();
        app.UseAuthentication();
        app.UseAuthorization();

        app.MapRazorPages();
        app.UseSerilogRequestLogging();
        app.InitializeDatabase();
        app.StartRecurringJobs();

        app.MapHub<SupportChatHub>("/supportChatHub");
        app.MapHub<NotificationHub>("/notificationHub");

        Log.Information("Application started successfully");

        app.Run();
    }

    private static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
    {
        services.AddRazorPages()
            .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix)
            .AddDataAnnotationsLocalization()
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve;
                options.JsonSerializerOptions.WriteIndented = true;
            });

        services.AddSignalR();
        services.AddCommonServices(configuration);
        services.AddHangfire(x => x.UseSqlServerStorage(configuration.GetConnectionString("Default")));
        services.AddHangfireServer();

        services.AddIdentity<User, Role>(options =>
        {
            options.Password.RequireDigit = false;
            options.Password.RequireLowercase = false;
            options.Password.RequireUppercase = false;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequiredLength = 6;
        });

        services.AddAuthentication(options =>
        {
            options.DefaultScheme = "Cookies";
            options.DefaultChallengeScheme = "oidc";
        })
        .AddCookie(opt =>
        {
            opt.LoginPath = "/login";
            opt.LogoutPath = "/logout";
            opt.AccessDeniedPath = "/Account/AccessDenied";
        })
        .AddOpenIdConnect("oidc", options =>
        {
            options.Authority = configuration["IdentityServer:Authority"];
            options.ClientId = configuration["IdentityServer:ClientId"];
            options.ClientSecret = configuration["IdentityServer:ClientSecret"];
            options.ResponseType = "code";
            options.SaveTokens = true;
            options.GetClaimsFromUserInfoEndpoint = true;
        });
        services.AddAuthorization();

        services.AddAntiforgery(options =>
        {
            options.HeaderName = "RequestVerificationToken";
        });

        services.AddMemoryCache();

        services.AddLocalization(options => options.ResourcesPath = "Resources");

        services.AddMvc()
        .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix)
        .AddDataAnnotationsLocalization();

        services.Configure<RequestLocalizationOptions>(options =>
        {
            var supportedCultures = new[]
            {
            new CultureInfo("en"),
            new CultureInfo("ru")
        };

            options.DefaultRequestCulture = new RequestCulture("ru");
            options.SupportedCultures = supportedCultures;
            options.SupportedUICultures = supportedCultures;
            options.SetDefaultCulture("ru");
            options.RequestCultureProviders.Insert(0, new CookieRequestCultureProvider());
            var culture = CultureInfo.CurrentCulture;
            Log.Information("Current culture: {Culture}", culture);


            options.RequestCultureProviders.Insert(0, new QueryStringRequestCultureProvider());
        });

        services.AddControllersWithViews()
            .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix)
            .AddDataAnnotationsLocalization();

        QuestPDF.Settings.License = LicenseType.Community;
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        ApiOperationBase<ANetApiRequest, ANetApiResponse>.RunEnvironment = AuthorizeNet.Environment.SANDBOX;
        services.ConfigMapper();
        services.AddHttpContextAccessor();
    }
}

