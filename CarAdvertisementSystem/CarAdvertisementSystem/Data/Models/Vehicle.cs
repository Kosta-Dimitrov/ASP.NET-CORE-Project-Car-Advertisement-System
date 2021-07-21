
namespace CarAdvertisementSystem.Data.Models
{
    using System.ComponentModel.DataAnnotations;
    using static Constants.Vehicle;
    public class Vehicle
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(ModelMaxLength)]
        public string Model { get; set; }

        [Required]
        public string Description { get; set; }

        public int Price { get; set; }
        public int HorsePower { get; set; }
        public int Doors { get; set; }
        public int Kilometers { get; set; }

        [Required]
        public string ImageUrl { get; set; }

        public string Color { get; set; }
        public int Year { get; set; }

        public Fuel Fuel { get; set; }
        public int FuelId { get; set; }

        public Type Type { get; set; }
        public int TypeId { get; set; }

        public Brand Brand { get; set; }
        public int BrandId { get; set; }

        public int SellerId { get; set; }
        public Seller Seller { get; set; }

    }
}
