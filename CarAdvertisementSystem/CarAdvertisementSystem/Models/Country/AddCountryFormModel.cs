namespace CarAdvertisementSystem.Models.Country
{
    using System.ComponentModel.DataAnnotations;
    using static CarAdvertisementSystem.Data.Constants.Country;
    public class AddCountryFormModel
    {
        [Required]
        [StringLength(CountryNameMaxLength,MinimumLength =CountryNameMinLength,ErrorMessage ="Country name should be between {2} and {1} symbols")]
        [RegularExpression(@"^[A-Z][A-Za-z]*", ErrorMessage = "Use letters only please")]
        [Display(Name ="Country name")]
        public string Name { get; set; }
    }
}
