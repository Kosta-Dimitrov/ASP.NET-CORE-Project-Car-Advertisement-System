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
            int currentPage,
            int vehiclesPerPage)
        {
            List<Vehicle> vehiclesQuery = data.
                Vehicles.
                Include(v => v.Brand).
                Include(v => v.Fuel).
                ToList();

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

        public List<string> VehicleBrands()
            => data.
                Brands.
                Select(b => b.Name).
                OrderBy(b => b).
                ToList();

        public List<string> VehicleFuels()
            => data.
                Fuels.
                Select(f => f.Name).
                ToList();

        public List<VehicleServiceModel> VehiclesByUser(string userId)
            => this.data
            .Vehicles
            .Where(v => v.Seller.UserId == userId)
            .Select(v => new VehicleServiceModel
            {
                Fuel = v.Fuel.Name,
                Brand = v.Brand.Name,
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
                     Brand = v.Brand,
                     Fuel=v.Fuel,
                     HorsePower=v.HorsePower,
                     Model=v.Model,
                     ImageUrl=v.ImageUrl,
                     Price=v.Price,
                     Year=v.Year
                 }).ToList();
    }
}
