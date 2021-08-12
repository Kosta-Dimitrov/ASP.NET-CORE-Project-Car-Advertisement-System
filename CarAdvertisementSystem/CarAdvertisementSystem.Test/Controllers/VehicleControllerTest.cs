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
                .Calling(c => c.Details(id,"test"))
                .ShouldHave()
                .ActionAttributes(attributes => attributes
                                    .AllowingAnonymousRequests())
                .AndAlso()
                .ShouldReturn()
                .BadRequest();
        }

        [Fact]
        public void DetailsShouldReturnView()
        {
            MyController<VehicleController>
                .Instance()
                .WithData(new List<Vehicle>
                {
                    new Vehicle
                    {
                        Id=1,
                        Brand=new Brand{Name="test"},
                        Model="test",
                        IsDeleted=false
                    } 
                })
                .Calling(c => c.Details(1, "test-test"))
                .ShouldHave()
                .ActionAttributes(attributes => attributes
                                    .AllowingAnonymousRequests())
                .AndAlso()
                .ShouldReturn()
                .View(view=>view.WithModelOfType<VehicleDetailsViewModel>());
        
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
        public void DeleteShouldReturnBadRequestIfUserIsUnauthorised()
        {
            MyController<VehicleController>
                .Instance()
                .WithData(new List<Vehicle>
                { 
                    new Vehicle
                    {
                        Id=1,
                        Model="testModel",
                        Brand=new Brand{Name="testBrand"}
                    }
                })
                .WithUser()
                .Calling(c => c.Delete(1,"testModel-testBrand"))
                .ShouldReturn()
                .BadRequest();
        }

        [Fact]
        public void DeleteShouldReturnBadRequestIfInvalidDataIsGiven()
        {
            MyController<VehicleController>
                .Instance()
                .WithUser(user => user.InRole("Administrator"))
                .Calling(c => c.Delete(1,"test"))
                .ShouldReturn()
                .BadRequest();
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

        [Fact]
        public void DeleteReturnsBadRequestIfVehicleIsNotDeletableAndHasAuthorisedAttribute()
        {
            MyController<VehicleController>
                .Instance()
                .WithUser()
                .Calling(c => c.Delete(1, "testinfo"))
                .ShouldHave()
                .ActionAttributes(attributes=>attributes.RestrictingForAuthorizedRequests())
                .AndAlso()
                .ShouldReturn()
                .BadRequest();
        }

        [Fact]
        public void DeleteReturnsBadRequestIfInformationIsNotRight()
        {
            MyController<VehicleController>
                .Instance()
                .WithUser()
                .WithData(new List<Vehicle>
                {
                    new Vehicle
                    {
                        Id=1,
                        Brand=new Brand{Name="testBrand" },
                        Model="testModel"
                    }
                })
                .Calling(c => c.Delete(1, "testinfo"))
                .ShouldReturn()
                .BadRequest();
        }
        [Fact]
        public void AddReturnsRedirectIfUserIsNotSellerOrAdmin()
        {
            MyController<VehicleController>
                 .Instance()
                 .WithUser()
                 .Calling(c => c.Add())
                 .ShouldReturn()
                 .RedirectToAction("Create", "Seller");
        }

        [Fact]
        public void AddReturnsViewWithCorrectModel()
        {
            MyController<VehicleController>
                .Instance()
                .WithUser(user=>user.InRole("Administrator"))
                .Calling(c => c.Add())
                .ShouldReturn()
                .View(view=>view.WithModelOfType<AddVehicleFormModel>());
        }
        [Fact]
        public void DeleteRedirectsToCorrectAction()
        {
            MyController<VehicleController>
                .Instance()
                .WithUser(user=>user.InRole("Administrator"))
                .WithData(new List<Vehicle>
                {
                    new Vehicle
                    {
                        Id=1,
                        Brand=new Brand{Name="test" },
                        Model="test",
                        IsDeleted=false,
                        SellerId=1
                    }
                })
                .WithData(new List<Seller>
                {
                    new Seller{Id=1}
                })
                .Calling(c => c.Delete(1, "test-test"))
                .ShouldReturn()
                .RedirectToAction("All", "Vehicle");
        }

        [Fact]
        public void EditShouldReturnIfOtherPersonTriesToEditBadRequest()
        {
            MyController<VehicleController>
               .Instance()
               .WithUser()
               .Calling(c => c.Edit(1, new AddVehicleFormModel()))
               .ShouldReturn()
               .BadRequest();
        }
        [Fact]
        public void EditReturnsBadrequestIfVehicleHasIncorrectId()
        {
            MyController<VehicleController>
                .Instance()
                .WithUser(user => user.InRole("Administrator"))
                .Calling(c => c.Edit(1))
                .ShouldReturn()
                .BadRequest();
        }
        [Theory]
        [InlineData(
          "testModel",
            "testDescription",
            10000,
            120,
            0,
            "testUrl",
            "testColor",
            2009,
            10000,
            3,
            3,
            1)]
        public void AddPostRedirectsIfDataIsNotValid(
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
            MyController<VehicleController>
                .Instance()
                .WithUser(user=>user.InRole("Administrator"))
                .Calling(c => c.Add(new AddVehicleFormModel
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
                    Price = price,
                }))
                .ShouldHave()
                .ActionAttributes(attributes => attributes.RestrictingForAuthorizedRequests())
               .AndAlso()
               .ShouldReturn()
               .View(view=>view.WithModelOfType<AddVehicleFormModel>());
        }

    }
}
