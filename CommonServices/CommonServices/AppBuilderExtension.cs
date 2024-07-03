using DAL;
using DAL.Seed;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Common.CommonServices
{
    public static class AppBuilderExtension
    {
        public static void InitializeDatabase(this IApplicationBuilder app)
        {
            using var scope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope();
            scope.ServiceProvider.GetRequiredService<AppDbContext>().Database.Migrate();
            var services = app.ApplicationServices.GetService<IServiceProvider>();
            DatabaseMigrator.SeedDatabaseAsync(services).GetAwaiter().GetResult();
        }
    }
}
