namespace CarAdvertisementSystem.Test.Routing
{
    using Xunit;
    using MyTested.AspNetCore.Mvc;
    using CarAdvertisementSystem.Controllers;
    using CarAdvertisementSystem.Models.Seller;

    public class SellerControllerTest
    {
        [Fact]
        public void DoesCreateReturnView()
        {
            MyRouting
               .Configuration()
               .ShouldMap("/Seller/Create")
               .To<SellerController>(c => c.Create());
        }

        [Fact]
        public void DoesPostCreatGetMapped()
        {
            MyRouting
                .Configuration()
                .ShouldMap(request => request.WithPath("/Seller/Create").WithMethod(HttpMethod.Post))
                .To<SellerController>(c => c.Create(With.Any<BecomeSellerFormModel>()));
        }    
    }
}
