namespace CarAdvertisementSystem.Models.Home
{
    using System.Collections.Generic;
    public class IndexViewModel
    {
        public int TotalVehicles { get; set; }
        public int TotalUsers { get; set; }
        public int TotalBrands { get; set; }
        public List<VehicleIndexViewModel> Vehicles { get; set; }
    }
}
