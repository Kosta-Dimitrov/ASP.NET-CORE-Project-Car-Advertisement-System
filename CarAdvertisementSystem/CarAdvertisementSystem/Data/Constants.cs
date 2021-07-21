﻿namespace CarAdvertisementSystem.Data
{
    public class Constants
    {
        public class Vehicle
        {
            public const int ModelMinLength = 2;
            public const int ModelMaxLength = 20;
            public const int MinYearValue = 1965;
            public const int MaxYearValue = 2030;
            public const int VehicleDescriptionMinLength = 10;
            public const int VehicleMinHorsepower = 20;
            public const int VehicleMaxHorsepower = 675;
            public const int VehicleMinPrice = 100;
            public const int VehicleMaxPrice = 10_000_000;
        }

        public class Brand
        {
            public const int BrandNameMinLength = 3;
            public const int BrandNameMaxLength= 25;
        }
        
        public class Country
        {
            public const int CountryNameMaxLength = 25;
        }

        public class Seller
        {
            public const int SellerNameMaxLength= 20;
            public const int SellerPhoneNumberMaxLength = 15;
        }
    }
}
