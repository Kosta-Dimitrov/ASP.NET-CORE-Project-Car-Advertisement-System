namespace CarAdvertisementSystem.Test.Routing
{
    using Xunit;
    using MyTested.AspNetCore.Mvc;
    using CarAdvertisementSystem.Controllers;
    using CarAdvertisementSystem.Models.Country;

    public class CountryControllerTest
    {
        [Fact]
        public void AddShouldBeMapped()
            => MyRouting
                .Configuration()
                .ShouldMap("/Country/Add")
                .To<CountryController>(c => c.Add());

        [Fact]
        public void AddPostShouldBeMapped()
            => MyRouting
                .Configuration()
                .ShouldMap(request => request
                    .WithPath("/Country/Add")
                    .WithMethod(HttpMethod.Post))
                .To<CountryController>(c => c.Add(With.Any<AddCountryFormModel>()));

        [Fact]
        public void AllShouldBeMapped()
        {
             MyRouting
                .Configuration()
                .ShouldMap("/Country/All")
                .To<CountryController>(c => c.All());
        }
    }
}
