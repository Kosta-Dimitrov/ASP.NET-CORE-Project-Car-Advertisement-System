namespace CarAdvertisementSystem.Models.Vehicle
{
    using CarAdvertisementSystem.Data.Models;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    public class AddVehicleFormModel
    {
        public string Model { get; set; }
        public string Description { get; set; }
        public int Price { get; set; }
        public int HorsePower { get; set; }
        public int Doors { get; set; }
        [Display(Name ="Image URL")]
        public string ImageUrl { get; set; }
        public string Color { get; set; }
        public int Year { get; set; }

        [Display(Name = "Type")]
        public int TypeId { get; set; }
        public IEnumerable<VehicleTypeViewModel> Types { get; set; }

        [Display(Name = "Fuel")]
        public int FuelId { get; set; }
        public IEnumerable<VehicleFuelViewModel> Fuels { get; set; }

        [Display(Name = "Brand")]
        public int BrandId { get; set; }
        public IEnumerable<VehicleBrandViewModel> Brands { get; set; }
    }
}
