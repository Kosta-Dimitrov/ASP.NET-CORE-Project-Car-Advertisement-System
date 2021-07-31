namespace CarAdvertisementSystem.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using CarAdvertisementSystem.Data;
    using CarAdvertisementSystem.Models.Country;
    using CarAdvertisementSystem.Data.Models;
    using System.Linq;
    using System.Collections.Generic;

    public class CountryController:Controller
    {
        private CarAdvertisementDbContext data;

        public CountryController(CarAdvertisementDbContext data)
            => this.data = data;

        [Authorize]
        public IActionResult Add()
            => View();

        [Authorize(Roles ="Administrator")]
        [HttpPost]
        public IActionResult Add(AddCountryFormModel newCountry)
        {
            if (!ModelState.IsValid)
            {
                return View(newCountry);
            }
            else
            {
                Country countryData = new Country
                {
                    Name = newCountry.Name
                };
                bool isCountryAlreadyIn = this.data.Countries.Where(c => c.Name.ToLower() == countryData.Name.ToLower()).Any();
                if (!isCountryAlreadyIn)
                {
                    this.data
                        .Countries
                        .Add(countryData);
                    data.SaveChanges();
                }
                return RedirectToAction("All", "Country");
            }
        }
        public IActionResult All()
        {
            List<CountryViewModel> allCountries = this.AllCountries();
            AllCountriesViewModel viewModel = new AllCountriesViewModel();
            allCountries.ForEach(c => viewModel.Countries.Add(c.Name, this.data.Brands.Where(b => b.Country.Name == c.Name).Count()));
            return View(viewModel);
        }

        private List<CountryViewModel> AllCountries()
        {
            return this.data
                .Countries
                .Select(c => new CountryViewModel
                {
                    Id = c.Id,
                    Name = c.Name
                }).OrderBy(c => c.Name)
                .ToList();
        }
    }
}
