namespace CarAdvertisementSystem.Test.Pipeline
{
    using CarAdvertisementSystem.Controllers;
    using CarAdvertisementSystem.Models.Home;
    using MyTested.AspNetCore.Mvc;
    using Xunit;
    public class HomeControllerTest
    {
        [Fact]
        public void IndexReturnsViewWithRightModelAndData()
           => MyController<HomeController>
            .Instance()
            .WithData()//IndexViewModelData.Return)
            .Calling(c => c.Index())
            .ShouldReturn()
            .View(view => view
                   .WithModelOfType<IndexViewModel>());
        //          .Passing(model => model.Vehicles.Should().HaveCount(3)));

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
