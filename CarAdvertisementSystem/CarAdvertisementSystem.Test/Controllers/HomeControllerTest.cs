namespace CarAdvertisementSystem.Test.Controllers
{
    using CarAdvertisementSystem.Controllers;
    using CarAdvertisementSystem.Models.Home;
    using CarAdvertisementSystem.Test.Data;
    using MyTested.AspNetCore.Mvc;
    using System.Collections.Generic;
    using System.Linq;
    using Xunit;
    public class HomeControllerTest
    {
        [Fact]
        public void IndexActionReturnsCorrectViewAndModel()
        {
            IndexViewModel model = new IndexViewModel
            {
                Vehicles = Enumerable.Range(0, 10)
                .Select(c => new VehicleIndexViewModel())
                .ToList()
            };
            MyController<HomeController>
            .Instance(instance => instance.WithData(model))
            .Calling(c => c.Index())
            .ShouldReturn()
            .View(view => view.WithModelOfType<IndexViewModel>());

        }
    }
}
