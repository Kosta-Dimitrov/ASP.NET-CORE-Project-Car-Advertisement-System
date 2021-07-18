namespace CarAdvertisementSystem.Infrastructure
{
    using Microsoft.AspNetCore.Builder;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.EntityFrameworkCore;
    using CarAdvertisementSystem.Data;
    using System.Linq;
    using CarAdvertisementSystem.Data.Models;

    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder PrepareDatabase(this IApplicationBuilder app)
        {
            using var scopedService = app.ApplicationServices.CreateScope();

            var data=scopedService.ServiceProvider.GetService<CarAdvertisementDbContext>();

            SeedCountries(data);
            SeedFuel(data);
            data.Database.Migrate();
            return app;
        }
        private static void SeedCountries(CarAdvertisementDbContext data)
        {
            if (data.Countries.Any())
            {
                return;
            }
            else
            {
                data.Countries.AddRange(new Country[]
                {
                    new Country{Name="Germany"},
                    new Country{Name="USA"},
                    new Country{Name="UK"},
                    new Country{Name="Spain"},
                    new Country{Name="Italy"},
                    new Country{Name="France"},
                    new Country{Name="Japan"},
                    new Country{Name="Bulgaria"},
                });
                data.SaveChanges();
            }
        }
        private static void SeedFuel(CarAdvertisementDbContext data)
        {
            if (data.Fuels.Any())
            {
                return;
            }
            else
            {
                data.Fuels.AddRange(new Fuel[]
                {
                    new Fuel{Name="Electricity"},
                    new Fuel{Name="Gasoline"},
                    new Fuel{Name="Diesel"},
                    new Fuel{Name="Hybrid"}
                });
                data.SaveChanges();
            }
        }

        private static void SeedTypes(CarAdvertisementDbContext data)
        {
            if (data.Types.Any())
            {
                return;
            }
            else
            {
                data.Types.AddRange(new Type[]
                {
                    new Type{Name="Car"},
                    new Type{Name="Truck"},
                    new Type{Name="Motor"},
                    new Type{Name="Bus"},
                    new Type{Name="Caravan"}
                });
                data.SaveChanges();
            }
        }
    }
}
