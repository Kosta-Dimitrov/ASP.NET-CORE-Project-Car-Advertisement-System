namespace CarAdvertisementSystem.Test.Routing
{
    using CarAdvertisementSystem.Controllers;
    using MyTested.AspNetCore.Mvc;
    using Xunit;
    public class HomeControllerTest
    {

        [Fact]
        public void IndexErrorReturnsView()
            =>
            MyMvc
            .Pipeline()
            .ShouldMap("/Home/Error")
            .To<HomeController>(c => c.Error())
            .Which(c => c.ShouldReturn().View());



    }
}
