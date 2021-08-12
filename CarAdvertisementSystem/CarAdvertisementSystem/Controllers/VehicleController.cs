namespace CarAdvertisementSystem.Controllers
{
    using CarAdvertisementSystem.Data;
    using CarAdvertisementSystem.Data.Models;
    using CarAdvertisementSystem.Models.Vehicle;
    using CarAdvertisementSystem.Services.Seller;
    using CarAdvertisementSystem.Services.Vehicle;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Caching.Memory;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Claims;

    public class VehicleController : Controller
    {
        private IVehicleService vehicleService;
        private ISellerService sellerService;
        private IMemoryCache cache;
        private CarAdvertisementDbContext data;

        [AllowAnonymous]
        public IActionResult Details(int id,string information)
        {
            Vehicle vehicle = this.data
                     .Vehicles
                    .Include(v => v.Brand)
                    .Include(v => v.Fuel)
                    .Include(v => v.Type)
                     .Where(v=>v.Id==id&&v.IsDeleted==false)
                     .FirstOrDefault();
            if (vehicle==null)
            {
                return BadRequest();
            }
            if (information!=vehicle.Brand.Name+"-"+vehicle.Model)
            {
                return BadRequest();
            }
            VehicleDetailsViewModel model =new VehicleDetailsViewModel
            {
                FuelName=vehicle.Fuel.Name,
                HorsePower=vehicle.HorsePower,
                BrandName=vehicle.Brand.Name,
                Color=vehicle.Color,
                Description=vehicle.Description,
                Doors=vehicle.Doors,
                ImageUrl=vehicle.ImageUrl,
                Kilometers=vehicle.Kilometers,
                Model=vehicle.Model,
                Price=vehicle.Price,
                TypeName=vehicle.Type.Name,
                Year=vehicle.Year
            };
            return View(model);
        }

        public VehicleController(IVehicleService vehicleService,
            CarAdvertisementDbContext data,
            ISellerService sellerService, 
            IMemoryCache cache)
        {
            this.vehicleService = vehicleService;
            this.data = data;
            this.sellerService = sellerService;
            this.cache = cache;
        }

        [AllowAnonymous]
        public IActionResult All([FromQuery]AllVehiclesViewModel model)
        {
            List<string> latestTypes =this.cache.Get<List<string>>("LatestTypesAllAction");
            if (latestTypes==null)
            {
                latestTypes = vehicleService.GetTypesByName();
                MemoryCacheEntryOptions cacheOptions = new MemoryCacheEntryOptions()
                    .SetAbsoluteExpiration(TimeSpan.FromDays(31));
                this.cache.Set("LatestTypesAllAction", latestTypes, cacheOptions);
            }
            model.Types = latestTypes;
            model.Brands = this.vehicleService.VehicleBrands();
            VehicleQueryServiceModel queryResult = this.vehicleService
                .All(
                model.Brand,
                model.SearchTerm,
                model.Sorting,
                model.Type,
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
            if (!this.sellerService.IsSeller(userId)&&!User.IsInRole("Administrator"))
            {
                return RedirectToAction("Create","Seller");
            }

            List<VehicleTypeViewModel> latestTypes = this.cache.Get<List<VehicleTypeViewModel>>("LatestTypes");
            if (latestTypes == null)
            {
                latestTypes = vehicleService.GetTypes().ToList();
                MemoryCacheEntryOptions cacheOptions = new MemoryCacheEntryOptions()
                    .SetAbsoluteExpiration(TimeSpan.FromDays(31));
                this.cache.Set("LatestTypes", latestTypes, cacheOptions);
            }
            List<VehicleFuelViewModel> latestFuels = this.cache.Get<List<VehicleFuelViewModel>>("LatestFuels");
            if (latestFuels==null)
            {
                latestFuels = this.vehicleService.GetFuels().ToList();
                MemoryCacheEntryOptions cacheOptions = new MemoryCacheEntryOptions()
                    .SetAbsoluteExpiration(TimeSpan.FromDays(31));
                this.cache.Set("LatestFuels", latestFuels, cacheOptions);
            }

            return View(new AddVehicleFormModel
            {
                Types = latestTypes,
                Brands = this.vehicleService.GetBrands(),
                Fuels = latestFuels
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
            if (!sellerService.IsSeller(userId)&&!User.IsInRole("Administrator"))
            {
                return RedirectToAction("Create", "Seller");
            }
            vehicle.Types = vehicleService.GetTypes();
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

            List<VehicleTypeViewModel> latestTypes = this.cache.Get<List<VehicleTypeViewModel>>("LatestTypes");
            if (latestTypes == null)
            {
                latestTypes = vehicleService.GetTypes().ToList();
                MemoryCacheEntryOptions cacheOptions = new MemoryCacheEntryOptions()
                    .SetAbsoluteExpiration(TimeSpan.FromDays(31));
                this.cache.Set("LatestTypes", latestTypes, cacheOptions);
            }
            List<VehicleFuelViewModel> latestFuels = this.cache.Get<List<VehicleFuelViewModel>>("LatestFuels");
            if (latestFuels == null)
            {
                latestFuels = this.vehicleService.GetFuels().ToList();
                MemoryCacheEntryOptions cacheOptions = new MemoryCacheEntryOptions()
                    .SetAbsoluteExpiration(TimeSpan.FromDays(31));
                this.cache.Set("LatestFuels", latestTypes, cacheOptions);
            }

            if (!ModelState.IsValid)
            {
                vehicle.Brands = this.vehicleService.GetBrands();
                vehicle.Types = latestTypes;
                vehicle.Fuels = latestFuels;
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
            if (!this.sellerService.IsSeller(userId)&&!User.IsInRole("Administrator"))
            {
                return RedirectToAction("Create", "Seller");
            }
            VehicleInfoServiceModel vehicleInfo = this.vehicleService.Info(id);
            if (vehicleInfo==null)
            {
                return BadRequest();
            }
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

        [Authorize]
        public IActionResult Delete(int id,string information)
        {
            string userId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            Vehicle vehicle = this.data
                .Vehicles
                .Where(v => v.Id == id && v.IsDeleted == false)
               .Include(v=>v.Brand)
               .Include(v=>v.Seller)
                .FirstOrDefault();
            if (vehicle==null)
            {
                return BadRequest();
            }
            if (information!=vehicle.Brand.Name+"-"+vehicle.Model)
            {
                return BadRequest();
            }
            if (!User.IsInRole("Administrator")&&vehicle.Seller.UserId != userId)
            {
                return BadRequest();
            }
            vehicle.IsDeleted = true;
            this.data.SaveChanges();
            this.TempData.Add("Successfully deleted vehicle", $"Successfully deleted {vehicle.Brand.Name} {vehicle.Model}");
            if (User.IsInRole("Administrator"))
            {
                return RedirectToAction("All","Vehicle");
            }
            else
            {
                return RedirectToAction("Mine", "Vehicle");
            }
        }

        [HttpPost]
        [Authorize]
        public IActionResult Edit(int id, AddVehicleFormModel vehicle)
        {
            string userId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            int sellerId = this.data.Sellers.Where(s => s.UserId == userId).Select(s=>s.Id).FirstOrDefault();
            bool isAdmin = User.IsInRole("Administrator");
            if (sellerId==0&& !isAdmin)
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

            List<VehicleTypeViewModel> latestTypes = this.cache.Get<List<VehicleTypeViewModel>>("LatestTypesEdit");
            if (latestTypes == null)
            {
                latestTypes = vehicleService.GetTypes().ToList();
                MemoryCacheEntryOptions cacheOptions = new MemoryCacheEntryOptions()
                    .SetAbsoluteExpiration(TimeSpan.FromDays(31));
                this.cache.Set("LatestTypesEdit", latestTypes, cacheOptions);
            }
            List<VehicleFuelViewModel> latestFuels = this.cache.Get<List<VehicleFuelViewModel>>("LatestFuelsEdit");
            if (latestFuels == null)
            {
                latestFuels = this.vehicleService.GetFuels().ToList();
                MemoryCacheEntryOptions cacheOptions = new MemoryCacheEntryOptions()
                    .SetAbsoluteExpiration(TimeSpan.FromDays(31));
                this.cache.Set("LatestFuelsEdit", latestTypes, cacheOptions);
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
                sellerId,
                isAdmin);
            if (!isVehicleEdited)
            {
                TempData["NotSuccess"] = "You could not edit the vehicle";
                return BadRequest();
            }
            else if (User.IsInRole("Administrator"))
            {
                this.TempData["Success"] = "Successfully edited";
                return RedirectToAction("All", "Vehicle");
            }
            else
            {
                this.TempData["Success"] = "Successfully edited";
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
      
    }
}
