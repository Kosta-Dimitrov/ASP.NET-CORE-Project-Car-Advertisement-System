namespace CarAdvertisementSystem.Models.Seller
{
    using System.ComponentModel.DataAnnotations;
    using static CarAdvertisementSystem.Data.Constants.Seller;
    public class BecomeSellerFormModel
    {
        [Required]
        [StringLength(SellerNameMaxLength,MinimumLength=SellerNameMinLength, ErrorMessage ="Name should be between {2} and {1} symbols")]
        public string Name { get; set; }
        [Required]
        [Display(Name="Phone number")]
        [MaxLength(SellerPhoneNumberMaxLength)]
        public string Phone { get; set; }
    }
}
