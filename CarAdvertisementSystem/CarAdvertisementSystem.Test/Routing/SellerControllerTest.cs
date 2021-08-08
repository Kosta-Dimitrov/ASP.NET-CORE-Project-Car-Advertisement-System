namespace CarAdvertisementSystem.Test.Routing
{
    using CarAdvertisementSystem.Controllers;
    using CarAdvertisementSystem.Models.Seller;
    using MyTested.AspNetCore.Mvc;
    using Xunit;
    public class SellerControllerTest
    {
        [Fact]
        public void DoesBecomeHaveCorrectMap()
        {
            MyMvc
              .Routing()
              .ShouldMap("/Seller/Create")
              .To<SellerController>(c => c.Create());
        }
        [Fact]
        public void DoesBecomePostHaveCorrectMap()
        {
            MyRouting.Configuration()
                .ShouldMap(request => request
                    .WithPath("/Seller/Create")
                    .WithMethod(HttpMethod.Post))
                .To<SellerController>(c => c.Create(With.Any<BecomeSellerFormModel>()));
        }
    }
}
