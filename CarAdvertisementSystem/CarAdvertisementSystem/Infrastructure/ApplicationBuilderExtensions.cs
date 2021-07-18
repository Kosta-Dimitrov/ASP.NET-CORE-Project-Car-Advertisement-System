namespace CarAdvertisementSystem.Infrastructure
{
    using Microsoft.AspNetCore.Builder;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.EntityFrameworkCore;

    using CarAdvertisementSystem.Data;

    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder PrepareDatabase(this IApplicationBuilder app)
        {
            using var scopedService = app.ApplicationServices.CreateScope();

            var data=scopedService.ServiceProvider.GetService<CarAdvertisementDbContext>();

            data.Database.Migrate();
            return app;
        }
    }
}
