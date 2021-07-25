namespace CarAdvertisementSystem.Controllers
{
    using CarAdvertisementSystem.Data;
    using CarAdvertisementSystem.Data.Models;
    using CarAdvertisementSystem.Models.Vehicle;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Claims;

    public class VehicleController : Controller
    {
        private CarAdvertisementDbContext data;

        public VehicleController(CarAdvertisementDbContext data)
            => this.data = data;

        public IActionResult All([FromQuery]AllVehiclesViewModel model)
        {
            List<Vehicle> vehiclesQuery = data.
                Vehicles.
                Include(v=>v.Brand).
                Include(v=>v.Fuel).
                ToList();

            if (!string.IsNullOrWhiteSpace(model.Brand))
            {
                vehiclesQuery = vehiclesQuery.
                    Where(v => v.Brand.Name == model.Brand).
                    ToList();
            }
            if (!string.IsNullOrWhiteSpace(model.SearchTerm))
            {
                vehiclesQuery = vehiclesQuery.Where
                    (v=>(v.Brand.Name.ToLower()+" "+v.Model.ToLower()).Contains(model.SearchTerm.ToLower())||
                    v.Description.ToLower().Contains(model.SearchTerm.ToLower())).
                    ToList();
            }
            switch(model.Sorting)
            {
                case VehicleSorting.Id:vehiclesQuery=vehiclesQuery.OrderByDescending(v => v.Id).ToList();break;
                case VehicleSorting.Price: vehiclesQuery =vehiclesQuery.OrderByDescending(v => v.Price).ToList();break;
                case VehicleSorting.Year: vehiclesQuery = vehiclesQuery.OrderByDescending(v => v.Year).ToList();break;
                default:break;
            }
            List<VehicleListingViewModel> vehicles = vehiclesQuery.
                Skip((model.CurrentPage-1)*AllVehiclesViewModel.VehiclesPerPage).
                Take(AllVehiclesViewModel.VehiclesPerPage).
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
            model.TotalVehicles = vehiclesQuery.Count();
            model.Brands = data.
                Brands.
                Select(b => b.Name).
                OrderBy(b => b).
                ToList();
            model.Fuels = data.
                Fuels.
                Select(f => f.Name).
                ToList();
            model.Vehicles = vehicles;
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
