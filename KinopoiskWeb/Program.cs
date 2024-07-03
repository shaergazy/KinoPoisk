using AutoMapper;
using Common.CommonServices;
using KinopoiskWeb.Extensions;
using KinopoiskWeb.Infrastructure;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddRazorPages();
        builder.Services.AddCommonServices(builder.Configuration);
        builder.Services.ConfigMapper();
        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.RegisterVirtualDir(builder.Configuration);
        app.UseStaticFiles();

        app.UseRouting();

        app.UseAuthorization();

        app.MapRazorPages();
        app.InitializeDatabase();

        app.Run();
    }
}