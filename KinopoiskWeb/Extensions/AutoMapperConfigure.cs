using AutoMapper;

namespace KinopoiskWeb.Infrastructure
{
    public static class AutoMapperConfigure
    {
        public static void ConfigMapper(this IServiceCollection services)
        {
            services.AddSingleton(_ => new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new BLL.Infrastructure.AutoMapperProfile());
                cfg.AddProfile(new AutoMapperProfile());
            }).CreateMapper());
        }
    }
}
