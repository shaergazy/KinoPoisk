using AuthorizeNet.Api.Contracts.V1;
using AuthorizeNet.Api.Controllers.Bases;
using Common.CommonServices;
using DAL.Models.Users;
using Hangfire;
using KinopoiskWeb.Extensions;
using KinopoiskWeb.Hubs;
using KinopoiskWeb.Infrastructure;
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
        services.AddRazorPages();
        services.AddSignalR(options =>
        {
            options.KeepAliveInterval = TimeSpan.FromMinutes(2);
            options.ClientTimeoutInterval = TimeSpan.FromMinutes(5);
        });
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

        QuestPDF.Settings.License = LicenseType.Community;
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        ApiOperationBase<ANetApiRequest, ANetApiResponse>.RunEnvironment = AuthorizeNet.Environment.SANDBOX;
        services.ConfigMapper();
        services.AddHttpContextAccessor();
    }
}

