namespace CarAdvertisementSystem.Test.Controllers
{
    using CarAdvertisementSystem.Controllers;
    using CarAdvertisementSystem.Models.Home;
    using MyTested.AspNetCore.Mvc;
    using System.Linq;
    using Xunit;
    public class HomeControllerTest
    {
        [Fact]
        public void IndexActionReturnsCorrectViewAndModel()
        {
            MyController<HomeController>
            .Instance()
            .Calling(c => c.Index())
            .ShouldReturn()
            .View(view => view.WithModelOfType<IndexViewModel>());

        }

        [Fact]
        public void IndexReturnsViewWithRightModelAndData()
           => MyController<HomeController>
            .Instance()
            .Calling(c => c.Index())
            .ShouldReturn()
            .View(view => view
                   .WithModelOfType<IndexViewModel>());
    }
}
