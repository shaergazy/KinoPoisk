using Common.Extensions;
using Common.Helpers;
using DTO;
using Microsoft.Extensions.FileProviders;

namespace KinopoiskWeb.Extensions
{
    public static class IApplicationBuilderExtension
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
    }
}
