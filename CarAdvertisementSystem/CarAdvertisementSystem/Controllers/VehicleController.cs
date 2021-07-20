namespace CarAdvertisementSystem.Controllers
{
    using CarAdvertisementSystem.Data;
    using CarAdvertisementSystem.Data.Models;
    using CarAdvertisementSystem.Models.Vehicle;
    using Microsoft.AspNetCore.Mvc;
    using System.Collections.Generic;
    using System.Linq;

    public class VehicleController : Controller
    {
        private CarAdvertisementDbContext data;

        public VehicleController(CarAdvertisementDbContext data)
            => this.data = data;

        public IActionResult All(string brand,string searchTerm)
        {
            List<Vehicle> vehiclesQuery = data.
                Vehicles.
                ToList();

            if (!string.IsNullOrWhiteSpace(brand))
            {
                vehiclesQuery = vehiclesQuery.
                    Where(v => v.Brand.Name == brand).
                    ToList();
            }
            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                vehiclesQuery= vehiclesQuery = data.Vehicles.Where
                    (v=>(v.Brand.Name.ToLower()+" "+v.Model.ToLower()).Contains(searchTerm.ToLower())||
                    v.Description.ToLower().Contains(searchTerm.ToLower())).
                    ToList();
            }
            List<VehicleListingViewModel> vehicles = vehiclesQuery.
                Select(v => new VehicleListingViewModel
              {
                  Fuel = v.Fuel.Name,
                  Brand = v.Brand.Name,
                  HorsePower = v.HorsePower,
                  ImageUrl = v.ImageUrl,
                  Model = v.Model,
                  Price=v.Price,
                  Id=v.Id,
                  Year=v.Year
              }).ToList();
            AllVehiclesViewModel viewModel = new AllVehiclesViewModel
            {
                Vehicles = vehicles,
                Brands = data.
                Brands.
                Select(b => b.Name).
                OrderBy(b => b).
                ToList(),
                Fuels = data.
                Fuels.
                Select(f => f.Name).
                ToList(),
                SearchTerm=searchTerm
            };
            return View(viewModel);
        }

        public IActionResult Add() => View(new AddVehicleFormModel()
        {
            Types=this.GetTypes(data),
            Brands=this.GetBrands(data),
            Fuels=this.GetFuels(data)
        });

        [HttpPost]
        public IActionResult Add(AddVehicleFormModel vehicle)
        {
            vehicle.Types = this.GetTypes(data);
            if (vehicle.Doors>0&&vehicle.TypeId==3)
            {
                ModelState.AddModelError(nameof(vehicle.Doors), "Motor cannot have doors");
            }
            if (vehicle.Doors==0&&vehicle.TypeId!=3)
            {
                ModelState.AddModelError(nameof(vehicle.Doors), $"Vehicle of type {vehicle.Types.ElementAt(vehicle.TypeId-1).Name} must have at least 1 door");
            }
            if (!ModelState.IsValid)
            {
                vehicle.Brands = this.GetBrands(data);
                vehicle.Types = this.GetTypes(data);
                vehicle.Fuels = this.GetFuels(data);
                return View(vehicle);
            }
            else
            {
                Vehicle vehicleData = new Vehicle
                {
                    BrandId = vehicle.BrandId,
                    Color = vehicle.Color,
                    Description = vehicle.Description,
                    Doors = vehicle.Doors,
                    Model = vehicle.Model,
                    Price = vehicle.Price,
                    HorsePower = vehicle.HorsePower,
                    ImageUrl=vehicle.ImageUrl,
                    Year=vehicle.Year,
                    FuelId=vehicle.FuelId,
                    TypeId=vehicle.TypeId
                };
                this.data.Vehicles.Add(vehicleData);
                this.data.SaveChanges();
            }
            return RedirectToAction(nameof(All));
        }

        private IEnumerable<VehicleTypeViewModel> GetTypes(CarAdvertisementDbContext data)
        {
            return data
                   .Types
                   .Select(t => new VehicleTypeViewModel
                   {
                       Id = t.Id,
                       Name = t.Name
                   }).ToList();
        }
        private IEnumerable<VehicleBrandViewModel> GetBrands(CarAdvertisementDbContext data)
        {
            return data
                   .Brands
                   .Select(b => new VehicleBrandViewModel()
                   {
                       Id = b.Id,
                       Name = b.Name
                   })
                   .OrderBy(b=>b.Name)
                   .ToList();
        }
        private IEnumerable<VehicleFuelViewModel> GetFuels(CarAdvertisementDbContext data)
        {
            return data
                   .Fuels
                   .Select(b => new VehicleFuelViewModel()
                   {
                       Id = b.Id,
                       Name = b.Name
                   }).ToList();
        }
    }
}
