namespace CarAdvertisementSystem.Test.Controllers
{
    using Xunit;
    using MyTested.AspNetCore.Mvc;
    using CarAdvertisementSystem.Controllers;
    using CarAdvertisementSystem.Models.Vehicle;
    using System.Collections.Generic;
    using CarAdvertisementSystem.Services.Vehicle;
    using CarAdvertisementSystem.Data.Models;
    using System.Linq;

    public class VehicleControllerTest
    {
        [Theory]
        [InlineData(1)]
        public void DetailsShouldBeAvaliableForUnauthorisedAndReturnBadRequestIfInvalidIndexIsGiven(int id)
        {
            MyController<VehicleController>
                .Instance()
                .Calling(c => c.Details(id))
                .ShouldHave()
                .ActionAttributes(attributes => attributes
                                    .AllowingAnonymousRequests())
                .AndAlso()
                .ShouldReturn()
                .BadRequest();
        }

        [Fact]
        public void AddIsAvaliableForAdminAndReturnsViewWithCorrectModel()
        {
            MyController<VehicleController>
                .Instance()
                .WithUser(user => user.InRole("Administrator"))
                .Calling(c => c.Add())
                .ShouldReturn()
                .View(view => view.WithModelOfType<AddVehicleFormModel>());

        }

        [Fact]
        public void AddShouldReturnRedirectIfUserIsUnauthorised()
        {
            MyController<VehicleController>
                .Instance()
                .WithUser()
                .Calling(c => c.Add())
                .ShouldReturn()
                .RedirectToAction("Create", "Seller");
        }

        [Fact]
        public void AddShouldBeAvaliableForAdmin()
        {
            MyController<VehicleController>
                .Instance()
                .WithUser(user => user.InRole("Administrator"))
                .Calling(c => c.Add())
                .ShouldReturn()
                .View(view => view.WithModelOfType<AddVehicleFormModel>());
        }

        [Fact]
        public void AddIsUnavaliableForUnauthorised()
        {
            MyController<VehicleController>
                .Instance()
                .WithUser()
                .Calling(c => c.Add())
                .ShouldReturn()
                .Redirect(redirect => redirect.To<SellerController>(c => c.Create()));
        }

        [Fact]
        public void MineShouldReturnCorrectViewAndModel()
        {
            MyController<VehicleController>
                .Instance()
                .WithUser()
                .Calling(c => c.Mine())
                .ShouldReturn()
                .View(view => view.WithModelOfType<List<VehicleServiceModel>>());
        }

        [Fact]
        public void AllIsAvaliableForUnauthorisedAndShouldReturnCorrectViewAndModel()
        {
            MyController<VehicleController>
                .Instance(instance => instance.WithUser())
                .Calling(c => c.All(new AllVehiclesViewModel()))
                .ShouldHave()
                .ActionAttributes(attributes => attributes.AllowingAnonymousRequests())
                .AndAlso()
                .ShouldReturn()
                .View(view => view.WithModelOfType<AllVehiclesViewModel>());
        }

        [Fact]
        public void EditIsAvaliableForAdministrator()
        {
            MyController<VehicleController>
                .Instance()
                .WithUser(user => user.InRole("Administrator"))
                .Calling(c => c.Edit(With.Any<int>()))
                .ShouldReturn()
                .BadRequest();
        }

        [Fact]
        public void EditRedirectsCorrectlyIfUserIsNotSeller()
        {
            MyController<VehicleController>
                .Instance()
                .WithUser()
                .Calling(c => c.Edit(With.Any<int>()))
                .ShouldReturn()
                .RedirectToAction("Create", "Seller");
        }

        [Fact]
        public void AddPostRedirectsIfUserIsNotSeller()
        {
            MyController<VehicleController>
                .Instance()
                .WithUser()
                .Calling(c => c.Add())
                .ShouldReturn()
                .RedirectToAction("Create", "Seller");
        }


        [Theory]
        [InlineData(
            "testModel",
            "testDescription",
            10000,
            120,
            3,
            "testUrl",
            "testColor",
            2016,
            12300,
            3,
            2,
            1)]
        public void AddPostShouldSaveTheDataAndRedirectCorrectly(
            string model,
            string description,
            int price,
            int horsePower,
            int doors,
            string imageUrl,
            string color,
            int year,
            int kilometers,
            int typeId,
            int fuelId,
            int brandId)
        {

            MyPipeline
                .Configuration()
                .ShouldMap(request => request
                            .WithPath("/Vehicle/Add")
                            .WithUser(user => user.InRole("Administrator"))
                            .WithAntiForgeryToken()
                            .WithMethod(HttpMethod.Post)
                            .WithFormFields(new
                            {
                                Model = model,
                                Description = description,
                                HorsePower = horsePower,
                                Doors = doors,
                                ImageUrl = imageUrl,
                                Price = price,
                                Color = color,
                                Year = year,
                                Kilometers = kilometers,
                                TypeId = typeId,
                                FuelId = fuelId,
                                BrandId = brandId
                            }))
               .To<VehicleController>(c => c.Add(
                   new AddVehicleFormModel
                   {
                       BrandId = brandId,
                       Model = model,
                       Description = description,
                       HorsePower = horsePower,
                       Doors = doors,
                       ImageUrl = imageUrl,
                       Color = color,
                       Year = year,
                       Kilometers = kilometers,
                       TypeId = typeId,
                       FuelId = fuelId,
                       Price=price
                   }))
               .Which()
               .ShouldHave()
               .ActionAttributes(attributes => attributes
                    .RestrictingForHttpMethod(HttpMethod.Post)
                    .RestrictingForAuthorizedRequests())
                .ValidModelState()
                .Data(data => data
                    .WithSet<Vehicle>(vehicle => vehicle
                        .Any(v =>
                            v.Kilometers == kilometers
                            && v.ImageUrl == imageUrl
                            && v.Model == model
                            && v.Price == price
                            && v.TypeId == typeId
                            && v.Year == year
                            && v.BrandId == brandId
                            && v.Description == description
                            && v.HorsePower == horsePower
                            && v.Doors == doors
                            && v.Color == color
                            && v.FuelId == fuelId)))
                .AndAlso()
                .ShouldReturn()
                .RedirectToAction();
        }
    }
}
