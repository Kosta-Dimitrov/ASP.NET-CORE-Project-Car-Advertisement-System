using CarAdvertisementSystem.Controllers;
using CarAdvertisementSystem.Models.Vehicle;
using MyTested.AspNetCore.Mvc;
using Xunit;

namespace CarAdvertisementSystem.Test.Routing
{
    public class VehicleControllerTest
    {
        [Fact]
        public void AllShouldBeMapped()
           => MyRouting
               .Configuration()
               .ShouldMap("/Vehicle/All")
               .To<VehicleController>(c => c.All(new AllVehiclesViewModel()));

        [Fact]
        public void MineShouldBeMapped()
           => MyRouting
               .Configuration()
               .ShouldMap("/Vehicle/Mine")
               .To<VehicleController>(c => c.Mine());

        [Fact]
        public void AddShouldBeMapped()
            => MyRouting
                .Configuration()
                .ShouldMap("/Vehicle/Add")
                .To<VehicleController>(c => c.Add());

        [Fact]
        public void AddPostShouldBeMapped()
            => MyRouting
                .Configuration()
                .ShouldMap(request => request
                    .WithPath("/Vehicle/Add")
                    .WithMethod(HttpMethod.Post))
                .To<VehicleController>(c => c.Add(With.Any<AddVehicleFormModel>()));
    }
}
