namespace CarAdvertisementSystem.Test.Controllers
{
    using Xunit;
    using MyTested.AspNetCore.Mvc;
    using CarAdvertisementSystem.Controllers;
    using CarAdvertisementSystem.Models.Seller;
    using CarAdvertisementSystem.Data.Models;
    using System.Linq;

    public class SellerControllerTest
    {
        [Fact]
        public void CreateShouldShouldBeForAuthorisedUsers()
        {
            MyController<SellerController>
                .Instance()
                .Calling(c => c.Create())
                .ShouldHave()
                .ActionAttributes(attributes => attributes.RestrictingForAuthorizedRequests())
                .AndAlso()
                .ShouldReturn()
                .View();

        }
        [Theory]
        [InlineData("testUserName","+359876345652")]
        public void PostCreateShouldBeForAuthorizedUsersAndRedirect(
            string userName,
            string phoneNumber)
        {
            MyController<SellerController>
                .Instance(controller => controller.WithUser())
                .Calling(c => c.Create(new BecomeSellerFormModel
                {
                    Name=userName,
                    Phone=phoneNumber
                }))
                .ShouldHave()
                .ActionAttributes(attributes => attributes
                                             .RestrictingForHttpMethod(HttpMethod.Post)
                                             .RestrictingForAuthorizedRequests())
                .ValidModelState()
                .Data(data=>data.WithSet<Seller>(sellers=>
                {
                    sellers.Any(s => s.Name == userName && s.Phone == phoneNumber);
                }));
        }
    }
}
