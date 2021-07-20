namespace CarAdvertisementSystem.Controllers
{
    using CarAdvertisementSystem.Data;
    using CarAdvertisementSystem.Models;
    using CarAdvertisementSystem.Models.Home;
    using CarAdvertisementSystem.Models.Vehicle;
    using Microsoft.AspNetCore.Mvc;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    public class HomeController : Controller
    {
        private CarAdvertisementDbContext data;

        public HomeController(CarAdvertisementDbContext data)
            => this.data = data;

        public IActionResult Index()
        {
            List<VehicleIndexViewModel> vehicles = this.data
             .Vehicles
             .OrderByDescending(v => v.Id)
             .Select(v => new VehicleIndexViewModel
             {
                 Fuel = v.Fuel.Name,
                 Brand = v.Brand.Name,
                 ImageUrl = v.ImageUrl,
                 Model = v.Model,
                 Id = v.Id
             })
             .Take(3)
             .ToList();

            int totalUsers = data.Users.Count();
            int totalVehicles = vehicles.Count;
            int totalBrands = data.Brands.Count();
            IndexViewModel viewModel = new IndexViewModel
            { 
                TotalBrands=totalBrands,
                TotalVehicles=totalVehicles,
                Vehicles=vehicles,
                TotalUsers=totalUsers
            };

            return View(viewModel);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
