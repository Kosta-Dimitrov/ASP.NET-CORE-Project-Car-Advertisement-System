namespace CarAdvertisementSystem.Test.Pipeline
{
    using CarAdvertisementSystem.Controllers;
    using MyTested.AspNetCore.Mvc;
    using Xunit;
    public class SellerControllerTest
    {
        [Fact]
        public void DoesBecomeSellerReturnViewAndOnlyForAuthorised()
        {
            MyMvc
              .Routing()
              .ShouldMap("/Seller/Create")
              .To<SellerController>(c => c.Create())
              .Which()
              .ShouldHave()
              .ActionAttributes(attribute => attribute.RestrictingForAuthorizedRequests())
              .AndAlso()
              .ShouldReturn()
              .View();
        }
    }
}
