namespace CarAdvertisementSystem.Test.Data
{
    using CarAdvertisementSystem.Models.Country;
    using System.Collections.Generic;

    public static class AllCountryViewModelData
    {
        public static AllCountriesViewModel Return
            => new AllCountriesViewModel
            {
                Countries = new Dictionary<string, int>
                {
                    { "test1",4},
                    { "test2",42},
                    { "test3",45},
                    { "test4",41},
                }
            };
    }
}
