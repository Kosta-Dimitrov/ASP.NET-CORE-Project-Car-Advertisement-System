namespace CarAdvertisementSystem.Controllers
{
    using CarAdvertisementSystem.Data;
    using CarAdvertisementSystem.Models.Vehicle;
    using Microsoft.AspNetCore.Mvc;
    using System.Collections.Generic;
    using System.Linq;

    public class VehicleController : Controller
    {
        private CarAdvertisementDbContext data;

        public VehicleController(CarAdvertisementDbContext data)
            => this.data = data;

       // public IActionResult Add() => View();

        public IActionResult Add() => View(new AddVehicleFormModel()
        {
            Types=this.GetTypes(data),
            Brands=this.GetBrands(data),
            Fuels=this.GetFuels(data)
        });

        [HttpPost]
        public IActionResult Add(AddVehicleFormModel vehicle)
        {
            return View();
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
                   }).ToList();
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
