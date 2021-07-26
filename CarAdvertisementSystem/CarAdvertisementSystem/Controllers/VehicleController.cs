namespace CarAdvertisementSystem.Controllers
{
    using CarAdvertisementSystem.Data;
    using CarAdvertisementSystem.Data.Models;
    using CarAdvertisementSystem.Models.Vehicle;
    using CarAdvertisementSystem.Services.Seller;
    using CarAdvertisementSystem.Services.Vehicle;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Claims;

    public class VehicleController : Controller
    {
        private IVehicleService vehicleService;
        private ISellerService sellerService;
        private CarAdvertisementDbContext data;

        public VehicleController(IVehicleService service, 
            CarAdvertisementDbContext data,
            ISellerService sellerService)
        {
            this.vehicleService = service;
            this.data = data;
            this.sellerService = sellerService;
        }

        public IActionResult All([FromQuery]AllVehiclesViewModel model)
        {
            
            model.Brands = this.vehicleService.VehicleBrands();
            model.Fuels = this.vehicleService.VehicleFuels();
            VehicleQueryServiceModel queryResult = this.vehicleService
                .All(model.Brand,
                model.SearchTerm,
                model.Sorting,
                model.CurrentPage,
                AllVehiclesViewModel.VehiclesPerPage);
            model.Vehicles = queryResult.Vehicles
                .Select(v=>new VehicleListingViewModel
                {
                    Id = v.Id,
                    Brand = v.BrandName,
                    Fuel = v.FuelName,
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
            //bool isSeller = this.data.
            //    Sellers.
            //    Any(s => s.UserId == userId);
            if (!this.sellerService.IsSeller(userId))
            {
                return RedirectToAction("Create","Seller");
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
            int sellerId = this.data
                .Sellers
                .Where(s => s.UserId == userId)
                .Select(s => s.Id)
                .FirstOrDefault();
            if (!sellerService.IsSeller(userId))
            {
                return RedirectToAction("Create", "Seller");
            }
            vehicle.Types = this.GetTypes(data);
            if (!this.vehicleService.ValidBrand(vehicle.BrandId))
            {
                ModelState.AddModelError(nameof(vehicle.BrandId), "Brand does not exists");
            }
            if (!this.vehicleService.ValidFuel(vehicle.FuelId))
            {
                ModelState.AddModelError(nameof(vehicle.FuelId), "Fuel does not exists");
            }
            if (!this.vehicleService.ValidType(vehicle.TypeId))
            {
                ModelState.AddModelError(nameof(vehicle.TypeId), "This type does not exists");
            }
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
                vehicle.Brands = this.vehicleService.GetBrands();
                vehicle.Types = this.vehicleService.GetTypes();
                vehicle.Fuels = this.vehicleService.GetFuels();
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
        public IActionResult Edit(int id)
        {
            string userId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            if (!this.sellerService.IsSeller(userId))
            {
                return RedirectToAction("Create", "Seller");
            }
            VehicleInfoServiceModel vehicleInfo = this.vehicleService.Info(id);
            return View(new AddVehicleFormModel
            {
                FuelId = vehicleInfo.FuelId,
                Description = vehicleInfo.Description,
                BrandId = vehicleInfo.BrandId,
                Color = vehicleInfo.Color,
                Doors = vehicleInfo.Doors,
                HorsePower = vehicleInfo.HorsePower,
                ImageUrl = vehicleInfo.ImageUrl,
                Kilometers = vehicleInfo.Kilometers,
                Model = vehicleInfo.Model,
                Price = vehicleInfo.Price,
                TypeId = vehicleInfo.TypeId,
                Year = vehicleInfo.Year,
                Brands=vehicleService.GetBrands(),
                Fuels=vehicleService.GetFuels(),
                Types=vehicleService.GetTypes()
            });

        }

        [HttpPost]
        [Authorize]
        public IActionResult Edit(int id, AddVehicleFormModel vehicle)
        {
            string userId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            int sellerId = this.data.Sellers.FirstOrDefault(s => s.UserId == userId).Id;
            if (!this.sellerService.IsSeller(userId))
            {
                return BadRequest();
            }
            if (!this.vehicleService.ValidBrand(vehicle.BrandId))
            {
                ModelState.AddModelError(nameof(vehicle.BrandId), "Brand does not exists");
            }
            if (!this.vehicleService.ValidFuel(vehicle.FuelId))
            {
                ModelState.AddModelError(nameof(vehicle.FuelId), "Fuel does not exists");
            }
            if (!this.vehicleService.ValidType(vehicle.TypeId))
            {
                ModelState.AddModelError(nameof(vehicle.TypeId), "This type does not exists");
            }
            if (vehicle.Doors > 0 && vehicle.TypeId == 3)
            {
                ModelState.AddModelError(nameof(vehicle.Doors), "Motor cannot have doors");
            }
            if (vehicle.Doors == 0 && vehicle.TypeId != 3)
            {
                ModelState.AddModelError(nameof(vehicle.Doors), $"Vehicle of type {vehicle.Types.ElementAt(vehicle.TypeId - 1).Name} must have at least 1 door");
            }
            if (!ModelState.IsValid)
            {
                vehicle.Brands = this.vehicleService.GetBrands();
                vehicle.Types = this.vehicleService.GetTypes();
                vehicle.Fuels = this.vehicleService.GetFuels();
                return View(vehicle);
            }
           bool isVehicleEdited= this.vehicleService.Edit(
                id,
                vehicle.Description,
                vehicle.BrandId,
                vehicle.Color,
                vehicle.Doors,
                vehicle.FuelId,
                vehicle.HorsePower,
                vehicle.ImageUrl,
                vehicle.Kilometers,
                vehicle.Model,
                vehicle.Price,
                vehicle.TypeId,
                vehicle.Year,
                sellerId);
            if (!isVehicleEdited)
            {
                return BadRequest();
            }
            else
            {
                return RedirectToAction("Mine", "Vehicle");
            }
        }
        [Authorize]
        public IActionResult Mine()
        {
            string myId =this.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            List<VehicleServiceModel> myVehicles = vehicleService.VehiclesByUser(myId);
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
