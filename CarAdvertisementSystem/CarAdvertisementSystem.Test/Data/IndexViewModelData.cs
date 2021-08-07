namespace CarAdvertisementSystem.Test.Data
{
    using CarAdvertisementSystem.Models.Home;
    public static class IndexViewModelData
    {
        public static IndexViewModel Return
            => new IndexViewModel
            {
                Vehicles=Vehicles.Return10ViewModels
            };

    }
}
