namespace CarAdvertisementSystem.Test.Routing
{
    using CarAdvertisementSystem.Controllers;
    using CarAdvertisementSystem.Models.Brand;
    using MyTested.AspNetCore.Mvc;
    using Xunit;
    public class BrandControllerTest
    {
        [Fact]
        public void AllShouldBeMapped()
            => MyRouting
                .Configuration()
                .ShouldMap("/Brand/All")
                .To<BrandController>(c => c.All());

        [Fact]
        public void AddShouldBeMapped()
            => MyRouting
                .Configuration()
                .ShouldMap("/Brand/Add")
                .To<BrandController>(c => c.Add());

        [Fact]
        public void AddPostShouldBeMapped()
            => MyRouting
                .Configuration()
                .ShouldMap(request => request
                    .WithPath("/Brand/Add")
                    .WithMethod(HttpMethod.Post))
                .To<BrandController>(c => c.Add(With.Any<AddBrandFormModel>()));
    }
}
