namespace CarAdvertisementSystem.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using static CarAdvertisementSystem.Data.Constants.Seller;
    public class Seller
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(SellerNameMaxLength)]
        public string Name { get; set; }
        [Required]
        [MaxLength(SellerPhoneNumberMaxLength)]
        public string Phone { get; set; }

        public IEnumerable<Vehicle> Vehicles { get; set; } = new List<Vehicle>();

        [Required]
        public string UserId { get; set; }
    }
}
