namespace CarAdvertisementSystem.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    public class Fuel
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public IEnumerable<Vehicle> Vehicles { get; set; } = new List<Vehicle>();
    }
}
