using Common.Extensions;
using Common.Helpers;
using DAL;
using DAL.Seed;
using DTO;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace API.Extensions;

internal static class IApplicationBuilderExtension
{
    internal static void RegisterVirtualDir(this IApplicationBuilder app, IConfiguration configuration)
    {
        var settings = configuration.GetSection(nameof(SettingsDto.VirtualDir)).Get<SettingsDto.VirtualDir>();
        var dir = Directory.GetCurrentDirectory().Combine(settings.BaseDir);
        dir.CreateDirectoryIfNotExist();
        app.UseFileServer(new FileServerOptions
        {
            FileProvider = new PhysicalFileProvider(dir),
            RequestPath = new PathString(settings.BaseSuffixUri)
        });
        AppConstants.RelativeFilesPath = settings.BaseDir;
        AppConstants.BaseDir = dir;
    }

    internal static void RegisterSwaggerUI(this IApplicationBuilder app)
    {
        app.UseSwagger();
        app.UseSwaggerUI(x =>
        {
            x.SwaggerEndpoint("/swagger/v1/swagger.json", "KFG api");
            x.RoutePrefix = string.Empty;
            x.DefaultModelExpandDepth(3);
            x.DefaultModelRendering(ModelRendering.Example);
            x.DefaultModelsExpandDepth(-1);
            x.DisplayOperationId();
            x.DisplayRequestDuration();
            x.DocExpansion(DocExpansion.None);
            x.EnableDeepLinking();
            x.EnableFilter();
            x.ShowExtensions();
        });
    }

    public static void InitializeDatabase(this IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope();
        scope.ServiceProvider.GetRequiredService<AppDbContext>().Database.Migrate();
        var services = app.ApplicationServices.GetService<IServiceProvider>();
        DatabaseMigrator.SeedDatabaseAsync(services).GetAwaiter().GetResult();
    }
}
