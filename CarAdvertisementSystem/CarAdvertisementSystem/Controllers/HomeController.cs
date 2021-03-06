namespace CarAdvertisementSystem.Controllers
{
    using CarAdvertisementSystem.Data;
    using CarAdvertisementSystem.Models.Home;
    using Microsoft.AspNetCore.Mvc;
    using System.Collections.Generic;
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
             .Where(v=>v.IsDeleted==false)
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

            int totalUsers = this.data.Sellers.Count();
            int totalVehicles = this.data.Vehicles.Where(c=>c.IsDeleted==false).Count();
            int totalBrands =this.data.Brands.Count();
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
            => View();
    }
}
