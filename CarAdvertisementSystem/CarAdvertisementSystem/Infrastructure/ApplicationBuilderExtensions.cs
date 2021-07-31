namespace CarAdvertisementSystem.Infrastructure
{
    using Microsoft.AspNetCore.Builder;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.EntityFrameworkCore;
    using CarAdvertisementSystem.Data;
    using System.Linq;
    using CarAdvertisementSystem.Data.Models;
    using System;
    using Microsoft.AspNetCore.Identity;
    using System.Threading.Tasks;

    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder PrepareDatabase(this IApplicationBuilder app)
        {
            using var scopedService = app.ApplicationServices.CreateScope();
            var serviceProvider = scopedService.ServiceProvider;

            var data=serviceProvider.GetRequiredService<CarAdvertisementDbContext>();

            SeedCountries(data);

            SeedFuel(data);

            SeedTypes(data);

            SeedBrands(data);

            SeedAdmiistrator(serviceProvider);

            data.Database.Migrate();
            return app;
        }

        private static void SeedAdmiistrator( IServiceProvider service)
        {
            var userManager = service.GetRequiredService <UserManager<User>>();
            var roleManager = service.GetRequiredService<RoleManager<IdentityRole>>();

            Task.Run(async () =>
            {
                if (await roleManager.RoleExistsAsync("Administrator"))
                {
                    return;
                }
                var role = new IdentityRole
                {
                    Name = "Administrator"
                };
                await roleManager.CreateAsync(role);

                var user = new User
                {
                    Email = "admin@cas.com",
                    Name = "Administrator",
                    UserName = "admin@cas.com"
                };
                await userManager.CreateAsync(user, "admin123");
               await userManager.AddToRoleAsync(user, "Administrator");

            })
            .GetAwaiter()
            .GetResult();
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
                data.Types.AddRange(new Data.Models.Type[]
                {
                    new Data.Models.Type{Name="Car"},
                    new Data.Models.Type{Name="Truck"},
                    new Data.Models.Type{Name="Motor"},
                    new Data.Models.Type{Name="Bus"},
                    new Data.Models.Type{Name="Caravan"}
                });
                data.SaveChanges();
            }
        }

        private static void SeedBrands(CarAdvertisementDbContext data)
        {
            if (data.Brands.Any())
            {
                return;
            }
            else
            {
                data.Brands.AddRange(new Brand[]
                {
                    new Brand
                    {
                        CountryId=1,
                        Name="Mercedes-Benz"
                    },
                    new Brand
                    {
                        CountryId=1,
                        Name="Audi"
                    },
                    new Brand
                    {
                        CountryId=1,
                        Name="Volkswagen"
                    },
                    new Brand
                    {
                        CountryId=5,
                        Name="Ferrari"
                    },
                    new Brand
                    {
                        CountryId=3,
                        Name="Bentley"
                    },
                    new Brand
                    {
                        CountryId=6,
                        Name="Peugeot"
                    },
                });
                data.SaveChanges();
            }
        }
    }
}
