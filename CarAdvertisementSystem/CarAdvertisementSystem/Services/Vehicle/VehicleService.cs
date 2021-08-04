namespace CarAdvertisementSystem.Services.Vehicle
{
    using CarAdvertisementSystem.Data;
    using Microsoft.EntityFrameworkCore;
    using System.Collections.Generic;
    using System.Linq;
    using CarAdvertisementSystem.Data.Models;
    using CarAdvertisementSystem.Models.Vehicle;

    public class VehicleService:IVehicleService
    {
        private CarAdvertisementDbContext data;

        public VehicleService(CarAdvertisementDbContext data)
            =>this.data = data;
        public VehicleQueryServiceModel All(
            string brand,
            string searchTerm,
            VehicleSorting sorting,
            string type,
            int currentPage,
            int vehiclesPerPage)
        {
            List<Vehicle> vehiclesQuery = data.
                Vehicles
                .Include(v => v.Brand)
                .Include(v => v.Fuel)
                .Include(v=>v.Type)
                .ToList();

            if (!string.IsNullOrWhiteSpace(brand))
            {
                vehiclesQuery = vehiclesQuery.
                    Where(v => v.Brand.Name == brand).
                    ToList();
            }
            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                vehiclesQuery = vehiclesQuery.Where
                    (v => (v.Brand.Name.ToLower() + " " + v.Model.ToLower()).Contains(searchTerm.ToLower()) ||
                    v.Description.ToLower().Contains(searchTerm.ToLower())).
                    ToList();
            }
            if (!string.IsNullOrWhiteSpace(type))
            {
                vehiclesQuery = vehiclesQuery
                    .Where(v => v.Type.Name.ToLower() == type.ToLower())
                    .ToList();
            }
            switch (sorting)
            {
                case VehicleSorting.Id: vehiclesQuery = vehiclesQuery.OrderByDescending(v => v.Id).ToList(); break;
                case VehicleSorting.Price: vehiclesQuery = vehiclesQuery.OrderByDescending(v => v.Price).ToList(); break;
                case VehicleSorting.Year: vehiclesQuery = vehiclesQuery.OrderByDescending(v => v.Year).ToList(); break;
                default: break;
            }
            List<VehicleListingViewModel> vehicles = vehiclesQuery.
                Skip((currentPage - 1) *vehiclesPerPage).
                Take(vehiclesPerPage).
                Select(v => new VehicleListingViewModel
                {
                    Fuel = v.Fuel.Name,
                    Brand = v.Brand.Name,
                    HorsePower = v.HorsePower,
                    ImageUrl = v.ImageUrl,
                    Model = v.Model,
                    Price = v.Price,
                    Id = v.Id,
                    Year = v.Year
                }).ToList();

            int totalVehicles = vehiclesQuery.Count();
            List<VehicleServiceModel> vehiclesData = this.GetVehicles(vehicles);
            return new VehicleQueryServiceModel
            {
                TotalVehicles = totalVehicles,
                VehiclesPerPage=vehiclesPerPage,
                CurrentPage=currentPage,
                Vehicles=vehiclesData
            };

        }

        public bool Edit(int id, string description, int brandId, string color, int doors, int fuelId, int horsePower, string imageUrl, int kilometers, string model, int price, int typeId, int year,int sellerId,bool isAdmin)
        {
            Vehicle vehicle = this.data.Vehicles.Find(id);
            if (vehicle==null)
            {
                return false;
            }
            if (sellerId != vehicle.SellerId && !isAdmin)
            {
                return false;
            }
            vehicle.BrandId = brandId;
            vehicle.Color = color;
            vehicle.Description = description;
            vehicle.Doors = doors;
            vehicle.FuelId = fuelId;
            vehicle.HorsePower = horsePower;
            vehicle.ImageUrl = imageUrl;
            vehicle.Kilometers = kilometers;
            vehicle.Model = model;
            vehicle.Price = price;
            vehicle.TypeId = typeId;
            vehicle.Year = year;
            this.data.SaveChanges();
            return true;
        }

        public IEnumerable<VehicleBrandViewModel> GetBrands()
            =>data.Brands
                   .Select(b => new VehicleBrandViewModel()
                   {
                       Id = b.Id,
                       Name = b.Name
                   })
                   .OrderBy(b => b.Name)
                   .ToList();

        public IEnumerable<VehicleFuelViewModel> GetFuels()
                => data.Fuels
                   .Select(b => new VehicleFuelViewModel()
                   {
                       Id = b.Id,
                       Name = b.Name
                   }).ToList();

        public IEnumerable<VehicleTypeViewModel> GetTypes()
            => data.Types
                   .Select(t => new VehicleTypeViewModel
                   {
                       Id = t.Id,
                       Name = t.Name
                   }).ToList();

        public List<string> GetTypesByName()
            => data
            .Types
            .Select(t => t.Name)
            .ToList();

        public VehicleInfoServiceModel Info(int id)
        => this.data.Vehicles.Where(v=>v.Id==id).Select(v => new VehicleInfoServiceModel
        {
            FuelId=v.FuelId,
            FuelName=v.Fuel.Name,
            Description=v.Description,
            SellerId=v.SellerId,
            BrandId=v.BrandId,
            BrandName=v.Brand.Name,
            Color=v.Color,
            Doors=v.Doors,
            HorsePower=v.HorsePower,
            Id=v.Id,
            ImageUrl=v.ImageUrl,
            Kilometers=v.Kilometers,
            Model=v.Model,
            Price=v.Price,
            TypeId=v.TypeId,
            TypeName=v.Type.Name,
            Year=v.Year,
            UserId=v.Seller.UserId
        }).FirstOrDefault();

        public bool IsBySeller(int id, int vehicleId)
        => this.data
              .Vehicles
              .Any(v => v.Id == vehicleId && v.SellerId == id);

        public bool ValidBrand(int id)
        => this.data
                .Brands
                .Any(b => b.Id == id);

        public bool ValidFuel(int id)
        => this.data
               .Fuels
                .Any(f => f.Id == id);

        public bool ValidType(int id)
        => this.data
               .Types
               .Any(t => t.Id == id);

        public List<string> VehicleBrands()
            => data
                .Brands
                .Select(b => b.Name)
                .OrderBy(b => b)
                .ToList();

        public List<string> VehicleFuels()
            => data
                .Fuels
                .Select(f => f.Name)
                .ToList();

        public List<VehicleServiceModel> VehiclesByUser(string userId)
            => this.data
            .Vehicles
            .Where(v => v.Seller.UserId == userId)
            .Select(v => new VehicleServiceModel
            {
                FuelName = v.Fuel.Name,
                BrandName = v.Brand.Name,
                HorsePower = v.HorsePower,
                ImageUrl = v.ImageUrl,
                Model = v.Model,
                Price = v.Price,
                Id = v.Id,
                Year = v.Year,
            }).ToList();

        private  List<VehicleServiceModel> GetVehicles(List<VehicleListingViewModel> vehicles)
                 => vehicles.Select(v => new VehicleServiceModel
                 {
                     Id = v.Id,
                     BrandName = v.Brand,
                     FuelName=v.Fuel,
                     HorsePower=v.HorsePower,
                     Model=v.Model,
                     ImageUrl=v.ImageUrl,
                     Price=v.Price,
                     Year=v.Year
                 }).ToList();
    }
}
