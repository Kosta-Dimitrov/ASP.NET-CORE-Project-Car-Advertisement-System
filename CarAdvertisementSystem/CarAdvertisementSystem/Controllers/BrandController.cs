namespace CarAdvertisementSystem.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using CarAdvertisementSystem.Data;
    using CarAdvertisementSystem.Models.Brand;
    using System.Linq;
    using CarAdvertisementSystem.Data.Models;
    using System.Collections.Generic;
    using CarAdvertisementSystem.Models.Country;
    using Microsoft.EntityFrameworkCore;

    public class BrandController:Controller
    {
        private CarAdvertisementDbContext data;

        public BrandController(CarAdvertisementDbContext data)=>
            this.data = data;

        [Authorize(Roles = "Administrator")]
        public IActionResult Add()
        {
            return View(new AddBrandFormModel
            {
                Countries=this.AllCountries()
            });
        }

        [Authorize(Roles = "Administrator")]
        [HttpPost]
        public IActionResult Add(AddBrandFormModel newBrand)
        {
            if (ModelState.IsValid)
            {
                bool isBrandAlreadyIn = this.data
                    .Brands
                    .Where(b => b.Name.ToLower() == newBrand.Name.ToLower())
                    .Any();
                if (!isBrandAlreadyIn)
                {
                    this.data.Brands.Add(new Brand
                    {
                        Name = newBrand.Name,
                        CountryId = newBrand.CountryId
                    });
                    data.SaveChanges();
                }
                return RedirectToAction("All", "Brand");
            }
            newBrand.Countries = this.AllCountries();
            return View(newBrand);
        }

        public IActionResult All()
        {
            List<BrandViewModel> viewModel = new List<BrandViewModel>();
            List<Brand> allBrands =this.AllBrands();
            allBrands.ForEach(b => viewModel.Add(new BrandViewModel
            {
                Name = b.Name,
                CountryName = b.Country.Name,
                VehiclesCount = b.Vehicles.Count()
            }));
            viewModel=viewModel
                .OrderBy(b => b.Name)
                .ThenBy(b=>b.VehiclesCount)
                .ToList();
            return View(viewModel);
        }

        private List<Brand> AllBrands()
        {
            return this.data
                .Brands
                .Include(b => b.Country)
                .Include(b=>b.Vehicles)
                .ToList();
        }

        private List<CountryViewModel> AllCountries()
        {
            return this.data
                .Countries
                .Select(c => new CountryViewModel
                {
                    Id=c.Id,
                    Name = c.Name
                }).OrderBy(c => c.Name)
                .ToList();
        }
    }
}
