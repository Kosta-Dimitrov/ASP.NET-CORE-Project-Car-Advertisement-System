namespace CarAdvertisementSystem.Services.Vehicle
{
    using System.Collections.Generic;
    public class VehicleQueryServiceModel
    {
       // public string Brand { get; set; }
       // public List<string> Brands { get; set; }
        public List<VehicleServiceModel> Vehicles { get; set; }
        //public List<string> Fuels { get; set; }
        //public string SearchTerm { get; set; }
        public int CurrentPage { get; set; } //= 1;
        public int TotalVehicles { get; set; }

        public int VehiclesPerPage { get; set; }
       // public VehicleSorting Sorting { get; set; }
    }
}
