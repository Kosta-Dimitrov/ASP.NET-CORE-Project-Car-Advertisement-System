namespace CarAdvertisementSystem.Controllers
{
    using CarAdvertisementSystem.Data;
    using CarAdvertisementSystem.Data.Models;
    using CarAdvertisementSystem.Models.Seller;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using System.Linq;
    using System.Security.Claims;

    public class SellersController:Controller
    {
        private CarAdvertisementDbContext data;

        public SellersController(CarAdvertisementDbContext data)
        {
            this.data = data;
        }

        [Authorize]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [Authorize]
        public IActionResult Create(BecomeSellerFormModel seller)
        {
            string userId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            bool isAlreadySeller = this.data.
                Sellers.
                Any(s => s.UserId == userId);
            if (isAlreadySeller)
            {
                return BadRequest();
            }
            if (!ModelState.IsValid)
            {
                return View(seller);
            }
            Seller sellerData = new Seller
            {
                UserId = userId,
                Name = seller.Name,
                Phone = seller.Phone,

            };
            this.data.Sellers.Add(sellerData);
            this.data.SaveChanges();
            return RedirectToAction("All","Vehicles");
        }
    }
}
