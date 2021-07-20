namespace CarAdvertisementSystem.Models.Vehicle
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class AllVehiclesViewModel
    {
        public const int VehiclesPerPage = 3;
        public string Brand { get; set; }
        public List<string> Brands { get; set; }
        public List<VehicleListingViewModel> Vehicles { get; set; }
        public List<string> Fuels { get; set; }

        [Display(Name ="Search")]
        public string SearchTerm { get; set; }
        public int CurrentPage { get; set; } = 1;
        public int TotalVehicles { get; set; }

        public VehicleSorting Sorting{ get; set; }
    }
}
