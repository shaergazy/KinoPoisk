using API.Infrastructure;
using Common.CommonServices;
using Microsoft.IdentityModel.Tokens;


namespace API.Extensions;

internal static class WebApplicationBuilderExtension
{
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
        services.RegisterSwagger();
        services.AddCommonServices(configuration);
        services.ConfigMapper();
        services.AddAuthentication("Bearer")
            .AddJwtBearer("Bearer", options =>
            {
                options.Authority = configuration["IdentityServer:Authority"];
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateAudience = false
                };
            });

        services.AddAuthorization(options =>
        {
            options.AddPolicy("ApiScope", policy =>
            {
                policy.RequireAuthenticatedUser();
                policy.RequireClaim("scope", "api1");
            });
        });
    }

    internal static WebApplication Configure(this WebApplicationBuilder builder)
    {
        var app = builder.Build();

        if (builder.Environment.IsDevelopment())
            app.UseDeveloperExceptionPage();

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
