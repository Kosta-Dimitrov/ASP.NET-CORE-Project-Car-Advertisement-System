namespace CarAdvertisementSystem.Test.Controllers
{
    using CarAdvertisementSystem.Controllers;
    using CarAdvertisementSystem.Data.Models;
    using CarAdvertisementSystem.Models.Brand;
    using MyTested.AspNetCore.Mvc;
    using System.Collections.Generic;
    using System.Linq;
    using Xunit;
    public class BrandControllerTest
    {
        [Fact]
        public void IsAddAvaliableOnlyForAuthorisedUsers()
        {
            MyController<BrandController>
                .Instance()
                .Calling(c => c.Add())
                .ShouldHave()
                .ActionAttributes(attributes => attributes.RestrictingForAuthorizedRequests("Administrator"))
                .AndAlso()
                .ShouldReturn()
                .View();
        }
        [Theory]
        [InlineData("TestBrand")]
        public void IsPostAddAvaliableForAuthorisedUsersAndRedirects(string brand)
        {
            MyController<BrandController>
                .Instance(controller => controller.WithUser())
                .Calling(c => c.Add(new AddBrandFormModel
                {
                    Name = brand
                }))
                .ShouldHave()
                .ActionAttributes(attributes => attributes
                            .RestrictingForAuthorizedRequests("Administrator")
                            .RestrictingForHttpMethod(HttpMethod.Post))
                .ValidModelState()
                .Data(data => data.WithSet<Brand>(b => b.Any(br => br.Name == brand)));                 
        }
        [Fact]
        public void IsAllAvaliableForNonAuthorisedUsers()
        {
            MyController<BrandController>
                .Instance()
                .Calling(c => c.All())
                .ShouldHave()
                .ActionAttributes(attributes => attributes.AllowingAnonymousRequests())
                .AndAlso()
                .ShouldReturn()
                .View(result => result.WithModelOfType<List<BrandViewModel>>());
        }
        [Fact]
        public void DoesAllReturnViewWithRightType()
        {
            MyController<BrandController>
                .Instance()
                .Calling(c => c.All())
                .ShouldReturn()
                .View(result => result.WithModelOfType<List<BrandViewModel>>());
        }
    }
}
