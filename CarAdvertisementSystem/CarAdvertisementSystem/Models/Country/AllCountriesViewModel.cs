namespace CarAdvertisementSystem.Models.Country
{
    using System.Collections.Generic;
    public class AllCountriesViewModel
    {
        public Dictionary<string, int> Countries { get; set; } = new Dictionary<string, int>();
    }
}
