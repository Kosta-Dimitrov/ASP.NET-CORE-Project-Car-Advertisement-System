namespace CarAdvertisementSystem.Services.Seller
{
    using CarAdvertisementSystem.Data;
    using System.Linq;

    public class SellerService : ISellerService
    {
        private CarAdvertisementDbContext data;

        public SellerService(CarAdvertisementDbContext data)
            => this.data = data;

        public bool IsSeller(string userId)
        {
            return this.data
                .Sellers
                .Any(s => s.UserId == userId);
        }
    }
}
