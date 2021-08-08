namespace CarAdvertisementSystem.Services.Vehicle
{
    using System.Collections.Generic;
    public class VehicleQueryServiceModel
    {
        public List<VehicleServiceModel> Vehicles { get; set; }
        public int CurrentPage { get; set; }
        public int TotalVehicles { get; set; }

        public int VehiclesPerPage { get; set; }
    }
}
