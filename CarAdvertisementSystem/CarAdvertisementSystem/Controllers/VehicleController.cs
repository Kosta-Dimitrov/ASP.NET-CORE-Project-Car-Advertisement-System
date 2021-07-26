namespace CarAdvertisementSystem.Controllers
{
    using CarAdvertisementSystem.Data;
    using CarAdvertisementSystem.Data.Models;
    using CarAdvertisementSystem.Models.Vehicle;
    using CarAdvertisementSystem.Services.Vehicle;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Claims;

    public class VehicleController : Controller
    {
        private IVehicleService service;
        private CarAdvertisementDbContext data;

        public VehicleController(IVehicleService service, CarAdvertisementDbContext data)
        {
            this.service = service;
            this.data = data;
        }

        public IActionResult All([FromQuery]AllVehiclesViewModel model)
        {
            
            model.Brands = this.service.VehicleBrands();
            model.Fuels = this.service.VehicleFuels();
            VehicleQueryServiceModel queryResult = this.service
                .All(model.Brand,
                model.SearchTerm,
                model.Sorting,
                model.CurrentPage,
                AllVehiclesViewModel.VehiclesPerPage);
            model.Vehicles = queryResult.Vehicles
                .Select(v=>new VehicleListingViewModel
                {
                    Id = v.Id,
                    Brand = v.Brand,
                    Fuel = v.Fuel,
                    HorsePower = v.HorsePower,
                    Model = v.Model,
                    ImageUrl = v.ImageUrl,
                    Price = v.Price,
                    Year = v.Year
                }).ToList();
                model.TotalVehicles = queryResult.TotalVehicles;
                return View(model);
        }

        [Authorize]
        public IActionResult Add()
        {
            string userId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            bool isSeller = this.data.
                Sellers.
                Any(s => s.UserId == userId);
            if (!isSeller)
            {
                return RedirectToAction("Create","Sellers");
            }
            return View(new AddVehicleFormModel
            {
                Types = this.GetTypes(data),
                Brands = this.GetBrands(data),
                Fuels = this.GetFuels(data)
            });
        }

        [HttpPost]
        [Authorize]
        public IActionResult Add(AddVehicleFormModel vehicle)
        {
            string userId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            bool isSeller = this.data.
                Sellers.
                Any(s => s.UserId == userId);
            int sellerId = this.data
                .Sellers
                .Where(s => s.UserId == userId)
                .Select(s => s.Id)
                .FirstOrDefault();
            if (!isSeller)
            {
                return RedirectToAction("Create", "Sellers");
            }
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
                    TypeId=vehicle.TypeId,
                    SellerId=sellerId
                };
                this.data.Vehicles.Add(vehicleData);
                this.data.SaveChanges();
            }
            return RedirectToAction(nameof(All));
        }
        [Authorize]
        public IActionResult Mine()
        {
            string myId =this.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            List<VehicleServiceModel> myVehicles = service.VehiclesByUser(myId);
            return View(myVehicles);
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
