
namespace CarAdvertisementSystem.Data
{
    using CarAdvertisementSystem.Data.Models;
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;
    public class CarAdvertisementDbContext : IdentityDbContext
    {
        public CarAdvertisementDbContext(DbContextOptions<CarAdvertisementDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder
                .Entity<Vehicle>()
                .HasOne(v => v.Type)
                .WithMany(t => t.Vehicles)
                .HasForeignKey(v=>v.TypeId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .Entity<Vehicle>()
                .HasOne(v => v.Fuel)
                .WithMany(f => f.Vehicles)
                .HasForeignKey(v=>v.FuelId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Vehicle>()
                .HasOne(v=>v.Brand)
                .WithMany(b=>b.Vehicles)
                .HasForeignKey(v=>v.BrandId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Brand>()
                .HasOne(b => b.Country)
                .WithMany(c => c.Brands)
                .HasForeignKey(b => b.CountryId)
                .OnDelete(DeleteBehavior.Restrict);


            base.OnModelCreating(builder);
             
        }
        public DbSet<Vehicle> Vehicles { get; set; }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<Fuel> Fuels { get; set; }
        public DbSet<Type> Types { get; set; }
    }
}
