namespace CarAdvertisementSystem.Models.Vehicle
{
    using CarAdvertisementSystem.Data.Models;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using static CarAdvertisementSystem.Data.Constants.Vehicle;
    public class AddVehicleFormModel
    {
        [Required]
        [StringLength(ModelMaxLength,MinimumLength=ModelMinLength,ErrorMessage ="Model name should be between {2} and {1} symbols")]
        public string Model { get; set; }
        [Required]
        [StringLength(Int32.MaxValue,MinimumLength=VehicleDescriptionMinLength, ErrorMessage = "Description should be at least {2} symbols")]
        public string Description { get; set; }
        [Required]
        [Range(VehicleMinPrice,VehicleMaxPrice,ErrorMessage ="Vehicle price should be between {1} and 10 000 000 euro")]
        public int Price { get; set; }
        [Required]
        [Range(VehicleMinHorsepower,VehicleMaxHorsepower,ErrorMessage ="Horsepower should be between {1} and {2}")]
        public int HorsePower { get; set; }
        [Display(Name = "Number of doors")]
        public int Doors { get; set; }
        [Required]
        [Display(Name ="Image URL")]
        [Url(ErrorMessage ="Please provide real URL")]
        public string ImageUrl { get; set; }
        [Required]
        public string Color { get; set; }
        [Required]
        [Range(MinYearValue,MaxYearValue,ErrorMessage ="Year of manifacture should be between {1} and {2}")]
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
