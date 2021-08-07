namespace CarAdvertisementSystem.Test.Routing
{
    using Xunit;
    using MyTested.AspNetCore.Mvc;
    using CarAdvertisementSystem.Controllers;

    public class HomeControllerTest
    {
        [Fact]
        public void DoesErrorGetMapped()
        {
            MyRouting
                .Configuration()
                .ShouldMap("/Home/Error")
                .To<HomeController>(c => c.Error());
        }
        [Fact]
        public void DoesIndexRouteGetMapped()
        {
            MyRouting
                .Configuration()
                .ShouldMap("/")
                .To<HomeController>(c => c.Index());
        }
    }
}
