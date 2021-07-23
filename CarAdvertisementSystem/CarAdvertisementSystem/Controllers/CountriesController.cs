namespace CarAdvertisementSystem.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using CarAdvertisementSystem.Data;
    using CarAdvertisementSystem.Models.Country;
    using CarAdvertisementSystem.Data.Models;
    using System.Linq;

    public class CountriesController:Controller
    {
        private CarAdvertisementDbContext data;

        public CountriesController(CarAdvertisementDbContext data)
            => this.data = data;

        [Authorize]
        public IActionResult Add()
            => View();

        [Authorize]
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
                return RedirectToAction("All", "Countries");
            }
        }
    }
}
