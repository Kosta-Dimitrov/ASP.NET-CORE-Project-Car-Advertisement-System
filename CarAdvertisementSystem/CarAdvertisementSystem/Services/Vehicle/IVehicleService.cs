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
            int currentPage,
            int vehiclesPerPage);
        List<string> VehicleBrands();
        List<string> VehicleFuels();
    }
}
