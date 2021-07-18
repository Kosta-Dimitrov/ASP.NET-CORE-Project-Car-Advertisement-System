namespace CarAdvertisementSystem.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using static Constants.Country;
    public class Country
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(CountryNameMaxLength)]
        public string Name { get; set; }

        public IEnumerable<Brand> Brands { get; set; } = new List<Brand>();
    }
}
