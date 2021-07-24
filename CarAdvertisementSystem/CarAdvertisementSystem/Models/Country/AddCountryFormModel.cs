namespace CarAdvertisementSystem.Models.Country
{
    using System.ComponentModel.DataAnnotations;
    using static CarAdvertisementSystem.Data.Constants.Country;
    using static CarAdvertisementSystem.Data.Constants.RegularExpressions;
    public class AddCountryFormModel
    {
        [Required]
        [StringLength(CountryNameMaxLength,MinimumLength =CountryNameMinLength,ErrorMessage ="Country name should be between {2} and {1} symbols")]
        [RegularExpression(StartWithUppercaseAndOnlySymbols, ErrorMessage = "Use letters only and first symbol should be uppercase")]
        [Display(Name ="Country name")]
        public string Name { get; set; }
    }
}
