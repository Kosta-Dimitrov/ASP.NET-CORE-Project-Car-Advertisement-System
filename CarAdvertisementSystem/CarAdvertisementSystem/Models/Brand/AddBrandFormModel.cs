namespace CarAdvertisementSystem.Models.Brand
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using CarAdvertisementSystem.Models.Country;

    using static CarAdvertisementSystem.Data.Constants.Brand;
    using static CarAdvertisementSystem.Data.Constants.RegularExpressions;
    public class AddBrandFormModel
    {
        [Required]
        [StringLength(BrandNameMaxLength,MinimumLength =BrandNameMinLength,ErrorMessage ="Brand name should be between {2} and {1} symbols")]
        [RegularExpression(StartWithUppercaseAndOnlySymbols,ErrorMessage ="Brand name must start with uppercase and be only alphabet letters")]
        public string Name { get; set; }

        public int CountryId { get; set; }

        public List<CountryViewModel> Countries { get; set; }

    }
}
