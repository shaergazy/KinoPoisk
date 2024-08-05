using BLL.Services.Interfaces;
using Hangfire;

namespace KinopoiskWeb.Infrastructure
{
    public static class HangfireWorker
    {
        public static void StartRecurringJobs(this IApplicationBuilder app)
        {
            RecurringJob.AddOrUpdate<IMovieService>("UpdateImdbRatings", x => x.UpdateImdbRatings(), Cron.Daily);
        }
    }
}
