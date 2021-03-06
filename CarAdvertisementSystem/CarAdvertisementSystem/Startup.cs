namespace CarAdvertisementSystem
{
    using CarAdvertisementSystem.Data;
    using CarAdvertisementSystem.Data.Models;
    using CarAdvertisementSystem.Infrastructure;
    using CarAdvertisementSystem.Services.Seller;
    using CarAdvertisementSystem.Services.Vehicle;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    public class Startup
    {
        public Startup(IConfiguration configuration)
            =>Configuration = configuration;

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<CarAdvertisementDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));
            services.AddDatabaseDeveloperPageExceptionFilter();
            services.AddMemoryCache();

            services.AddDefaultIdentity<User>(options =>
            {
                options.SignIn.RequireConfirmedAccount = false;
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                
            })
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<CarAdvertisementDbContext>();
            services.AddControllersWithViews();
            services.AddTransient<IVehicleService, VehicleService>();
            services.AddTransient<ISellerService, SellerService>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.PrepareDatabase();
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapControllerRoute(
                    name: "Vehicle Details",
                    pattern: "/Vehicle/Details/{id}/{information}",
                    defaults: new { controller = "Vehicle", action = "Details" });
                endpoints.MapControllerRoute(
                    name: "Vehicle Delete",
                    pattern: "/Vehicle/Details/{id}/{information}",
                    defaults: new { controller = "Vehicle", action = "Delete" });
                endpoints.MapRazorPages();
            });


        }
    }
}
