namespace CarAdvertisementSystem.Test.Data
{
    using CarAdvertisementSystem.Models.Home;
    using System.Collections.Generic;
    using System.Linq;
    public static class Vehicles
    {
        public static List<VehicleIndexViewModel> Return10ViewModels
            => Enumerable.Range(0, 10)
            .Select(i => new VehicleIndexViewModel
            {
            }).ToList();
    }
}
