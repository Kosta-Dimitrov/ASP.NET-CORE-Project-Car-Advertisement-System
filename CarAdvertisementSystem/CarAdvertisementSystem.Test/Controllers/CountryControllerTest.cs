namespace CarAdvertisementSystem.Test.Controllers
{
    using Xunit;
    using MyTested.AspNetCore.Mvc;
    using CarAdvertisementSystem.Controllers;
    using CarAdvertisementSystem.Models.Country;
    using System.Collections.Generic;
    using System.Linq;
    using CarAdvertisementSystem.Data.Models;

    public class CountryControllerTest
    {
        [Fact]
        public void AddReturnsView()
        {
            MyController<CountryController>
                .Instance()
                .Calling(c => c.Add())
                .ShouldReturn()
                .View();
        }

        [Fact]
        public void CountryControllerIsAvaliableOnlyForAuthorisedUsers()
        {
            MyController<CountryController>
            .ShouldHave()
            .Attributes(attributes => attributes
             .RestrictingForAuthorizedRequests("Administrator"));
        }

        [Fact]     
        public void CountryAllIsAvaliableForAnonimousAndReturnsCorrectViewAndModel()
        {
            MyController<CountryController>
                .Instance()
                .Calling(c => c.All())
                .ShouldHave()
                .ActionAttributes(attributes => attributes
                                    .AllowingAnonymousRequests())
                .AndAlso()
                .ShouldReturn()
                .View(view=>view.WithModelOfType<AllCountriesViewModel>());
        }

        [Theory]
        [InlineData("TestCountry")]
        public void ChangeTheName(string name)
        {
            MyPipeline
                .Configuration()
                .ShouldMap(request => request
                            .WithPath("/Country/Add")
                            .WithUser(user => user.InRole("Administrator"))
                            .WithAntiForgeryToken()
                            .WithMethod(HttpMethod.Post)
                            .WithFormFields(new
                            {
                                Name = name
                            }))
               .To<CountryController>(c => c.Add(new AddCountryFormModel
               {
                   Name = name
               }))
               .Which()
               .ShouldHave()
               .ActionAttributes(attributes => attributes
                    .RestrictingForHttpMethod(HttpMethod.Post)
                    .RestrictingForAuthorizedRequests())
                .ValidModelState()
                .Data(data => data
                    .WithSet<Country>(country => country
                        .Any(c =>
                            c.Name == name)));
        }
    }
}
