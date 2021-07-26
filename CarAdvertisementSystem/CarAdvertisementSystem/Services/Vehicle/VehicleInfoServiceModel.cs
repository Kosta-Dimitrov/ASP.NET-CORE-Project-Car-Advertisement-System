namespace CarAdvertisementSystem.Services.Vehicle
{
    public class VehicleInfoServiceModel:VehicleServiceModel
    {
        public string Description { get; set; }
        public int FuelId  { get; set; }
        public int BrandId { get; set; }
        public int TypeId { get; set; }
        public int Doors { get; set; }
        public string Color { get; set; }

        public int Kilometers { get; set; }
        public string UserId { get; set; }
        public int SellerId { get; set; }
    }
}
