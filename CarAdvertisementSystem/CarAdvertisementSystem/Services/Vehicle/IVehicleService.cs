namespace CarAdvertisementSystem.Services.Vehicle
{
    using System.Collections.Generic;
    using CarAdvertisementSystem.Models.Vehicle;
    public interface IVehicleService
    {
        VehicleQueryServiceModel All(
            string brand,
            string searchTerm,
            VehicleSorting sorting,
            string type,
            int currentPage,
            int vehiclesPerPage);
        bool IsBySeller(int id,int vehicleId);
        List<string> VehicleBrands();
        List<string> VehicleFuels();
        List<VehicleServiceModel> VehiclesByUser(string userId);
        IEnumerable<VehicleBrandViewModel> GetBrands();
        IEnumerable<VehicleTypeViewModel> GetTypes();
        IEnumerable<VehicleFuelViewModel> GetFuels();
        VehicleInfoServiceModel Info(int id);

        bool ValidBrand(int id);
        bool ValidFuel(int id);
        bool ValidType(int id);
        bool Edit(int id, string description, int brandId, string color, int doors, int fuelId, int horsePower, string imageUrl, int kilometers, string model, int price, int typeId, int year, int sellerId,bool isAdmin);
        List<string> GetTypesByName();
    }
}
