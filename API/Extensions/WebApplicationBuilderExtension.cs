using System.IO;
using API.Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.MSSqlServer;
using Common.CommonServices;


namespace API.Extensions;

internal static class WebApplicationBuilderExtension
{
    //internal static void RegisterSerilog(this WebApplicationBuilder builder)
    //{
    //    var logging = builder.Logging;
    //    logging.ClearProviders();

    //    var env = builder.Environment.EnvironmentName;
    //    var configuration = new ConfigurationBuilder()
    //        .SetBasePath(Directory.GetCurrentDirectory())
    //        .AddJsonFile("appsettings.json", false, true)
    //        .AddJsonFile($"appsettings.{env}.json", true)
    //        .Build();

    //    var connectionString = configuration.GetConnectionString("Default");
    //    var columnOptions = new ColumnOptions
    //    {
    //        TimeStamp = { ConvertToUtc = true, ColumnName = "CreatedTimeUtc", }
    //    };
    //    columnOptions.Store.Remove(StandardColumn.MessageTemplate);
    //    columnOptions.Store.Remove(StandardColumn.Properties);
    //    logging.AddSerilog(new LoggerConfiguration()
    //        .MinimumLevel.Override("Microsoft", LogEventLevel.Error)
    //        .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Error)
    //        .MinimumLevel.Override("Microsoft.AspNetCore.HttpLogging.HttpLoggingMiddleware", LogEventLevel.Information)
    //        .WriteTo.MSSqlServer(
    //            connectionString: connectionString,
    //            sinkOptions: new MSSqlServerSinkOptions
    //            {
    //                TableName = "Logs",
    //                AutoCreateSqlTable = true,
    //            },
    //            columnOptions: columnOptions)
    //        .CreateLogger());
    //}

    internal static void ConfigureServices(this WebApplicationBuilder builder)
    {
        var services = builder.Services;
        var configuration = builder.Configuration;

        services.RegisterCors(configuration);
        services.AddControllers(x => x.Filters.Add<NoContentFilter>());
        services.AddMvc();
        services.AddSignalR();
        services.AddHttpContextAccessor();

        services.RegisterIOptions(configuration);
        //services.RegisterConnectionString(configuration);
        //services.RegisterAuth();
        //services.RegisterJwtAuthorization(configuration);

        //services.RegisterServices();
        //services.RegisterServiceUri(configuration);
        services.RegisterSwagger();
        services.AddCommonServices(configuration);
    }

    internal static WebApplication Configure(this WebApplicationBuilder builder)
    {
        var app = builder.Build();

        if (builder.Environment.IsDevelopment())
            app.UseDeveloperExceptionPage();

        //app.UseMiddleware<ExceptionAndLoggerMiddleware>();

        app.UseHttpsRedirection();
        app.RegisterVirtualDir(builder.Configuration);
        app.UseRouting();
        app.UseCors();

        app.UseAuthentication();
        app.UseAuthorization();

        app.RegisterSwaggerUI();
        app.UseEndpoints(x => { 
            x.MapControllers();
        });
        app.InitializeDatabase();

        return app;
    }
}
