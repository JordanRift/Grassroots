//
// Copyright © 2011 Jordan Rift, LLC - All Rights Reserved
//
// THIS WORK IS LICENSED UNDER A CREATIVE COMMONS ATTRIBUTION-NONCOMMERCIAL-
// SHAREALIKE 3.0 UNPORTED LICENSE:
// http://creativecommons.org/licenses/by-nc-sa/3.0/
//

using System;
using System.Web.Mvc;
using System.Web.Routing;
using AutoMapper;
using JordanRift.Grassroots.Framework.Entities.Models;
using JordanRift.Grassroots.Tests.Helpers;
using JordanRift.Grassroots.Web.Controllers;
using JordanRift.Grassroots.Framework.Data;
using JordanRift.Grassroots.Tests.Fakes;
using JordanRift.Grassroots.Web.Models;
using NUnit.Framework;

namespace JordanRift.Grassroots.Tests.UnitTests.Controllers
{
    [TestFixture]
    public class UserProfileControllerTests
    {
        private UserProfileController controller;
        private IUserProfileRepository repository;

        [SetUp]
        public void SetUp()
        {
            controller = GetUserProfileController();
            Mapper.CreateMap<UserProfile, UserProfileDetailsModel>();
        }

        [Test]
        public void Index_Should_Return_View_If_UserProfile_Found()
        {
            var userProfile = EntityHelpers.GetValidUserProfile();
            userProfile.Email = "goodEmail";
            repository.Add(userProfile);
            var result = controller.Index();
            Assert.IsInstanceOf(typeof(ViewResult), result);
        }

        [Test]
        public void Index_Should_Return_NotFound_If_UserProfile_Not_Found()
        {
            var result = controller.Index();
            Assert.IsInstanceOf(typeof(HttpNotFoundResult), result);
        }

        [Test]
        public void Edit_Should_Return_View_When_UserProfile_Found()
        {
            repository.Add(new UserProfile { Email = "goodEmail" });
            var result = controller.Edit();
            Assert.IsInstanceOf(typeof(ViewResult), result);
        }

        [Test]
        public void Edit_Should_Return_NotFound_When_UserProfile_Not_Found()
        {
            var result = controller.Edit();
            Assert.IsInstanceOf(typeof(HttpNotFoundResult), result);
        }

        [Test]
        public void Update_Should_Redirect_To_Index_If_Successful()
        {
            var userProfile = EntityHelpers.GetValidUserProfile();
            userProfile.Email = "goodEmail";
            repository.Add(userProfile);
            var viewModel = Mapper.Map<UserProfile, UserProfileDetailsModel>(userProfile);
            var result = controller.Update(viewModel);
            Assert.IsInstanceOf(typeof(RedirectToRouteResult), result);
            Assert.AreEqual("Index", ((RedirectToRouteResult)result).RouteValues["Action"]);
        }

        [Test]
        public void Update_Should_Redirect_To_Edit_If_Model_Is_Not_Valid()
        {
            var userProfile = EntityHelpers.GetValidUserProfile();
            userProfile.Email = "goodEmail";
            repository.Add(userProfile);
            var viewModel = Mapper.Map<UserProfile, UserProfileDetailsModel>(userProfile);
            controller.ModelState.AddModelError("", "Dummy error message.");
            var result = controller.Update(viewModel);
            Assert.IsInstanceOf(typeof(RedirectToRouteResult), result);
            Assert.AreEqual("Edit", ((RedirectToRouteResult)result).RouteValues["Action"]);
        }

        [Test]
        public void Update_Should_Return_Not_Found_If_UserProfile_Not_Found()
        {
            var viewModel = Mapper.Map<UserProfile, UserProfileDetailsModel>(EntityHelpers.GetValidUserProfile());
            var result = controller.Update(viewModel);
            Assert.IsInstanceOf(typeof(HttpNotFoundResult), result);
        }

        [Test]
        public void Map_Should_Update_FirstName()
        {
            var userProfile = EntityHelpers.GetValidUserProfile();
            userProfile.Email = "goodEmail";
            repository.Add(userProfile);
            var viewModel = Mapper.Map<UserProfile, UserProfileDetailsModel>(userProfile);
            viewModel.FirstName = "NewFirstName";
            controller.Update(viewModel);
            Assert.AreEqual(userProfile.FirstName, viewModel.FirstName);
        }

        [Test]
        public void Map_Should_Update_LastName()
        {
            var userProfile = EntityHelpers.GetValidUserProfile();
            userProfile.Email = "goodEmail";
            repository.Add(userProfile);
            var viewModel = Mapper.Map<UserProfile, UserProfileDetailsModel>(userProfile);
            viewModel.LastName = "NewLasttName";
            controller.Update(viewModel);
            Assert.AreEqual(userProfile.LastName, viewModel.LastName);
        }

        [Test]
        public void Map_Should_Update_Birthdate()
        {
            var userProfile = EntityHelpers.GetValidUserProfile();
            userProfile.Email = "goodEmail";
            repository.Add(userProfile);
            var viewModel = Mapper.Map<UserProfile, UserProfileDetailsModel>(userProfile);
            viewModel.Birthdate = new DateTime(1981, 12, 1);
            controller.Update(viewModel);
            Assert.AreEqual(userProfile.Birthdate, viewModel.Birthdate);
        }

        [Test]
        public void Map_Should_Update_PrimaryPhone()
        {
            var userProfile = EntityHelpers.GetValidUserProfile();
            userProfile.Email = "goodEmail";
            repository.Add(userProfile);
            var viewModel = Mapper.Map<UserProfile, UserProfileDetailsModel>(userProfile);
            viewModel.PrimaryPhone = "602-555-7777";
            controller.Update(viewModel);
            Assert.AreEqual(userProfile.PrimaryPhone, viewModel.PrimaryPhone);
        }

        [Test]
        public void Map_Should_Update_Gender()
        {
            var userProfile = EntityHelpers.GetValidUserProfile();
            userProfile.Email = "goodEmail";
            repository.Add(userProfile);
            var viewModel = Mapper.Map<UserProfile, UserProfileDetailsModel>(userProfile);
            viewModel.Gender = "female";
            controller.Update(viewModel);
            Assert.AreEqual(userProfile.Gender, viewModel.Gender);
        }

        [Test]
        public void Map_Should_Update_AddressLine1()
        {
            var userProfile = EntityHelpers.GetValidUserProfile();
            userProfile.Email = "goodEmail";
            repository.Add(userProfile);
            var viewModel = Mapper.Map<UserProfile, UserProfileDetailsModel>(userProfile);
            viewModel.AddressLine1 = "New Street Address";
            controller.Update(viewModel);
            Assert.AreEqual(userProfile.AddressLine1, viewModel.AddressLine1);
        }

        [Test]
        public void Map_Should_Update_AddressLine2()
        {
            var userProfile = EntityHelpers.GetValidUserProfile();
            userProfile.Email = "goodEmail";
            repository.Add(userProfile);
            var viewModel = Mapper.Map<UserProfile, UserProfileDetailsModel>(userProfile);
            viewModel.AddressLine2 = "New Address Line 2";
            controller.Update(viewModel);
            Assert.AreEqual(userProfile.AddressLine2, viewModel.AddressLine2);
        }

        [Test]
        public void Map_Should_Update_City()
        {
            var userProfile = EntityHelpers.GetValidUserProfile();
            userProfile.Email = "goodEmail";
            repository.Add(userProfile);
            var viewModel = Mapper.Map<UserProfile, UserProfileDetailsModel>(userProfile);
            viewModel.City = "New City";
            controller.Update(viewModel);
            Assert.AreEqual(userProfile.City, viewModel.City);
        }

        [Test]
        public void Map_Should_Update_State()
        {
            var userProfile = EntityHelpers.GetValidUserProfile();
            userProfile.Email = "goodEmail";
            repository.Add(userProfile);
            var viewModel = Mapper.Map<UserProfile, UserProfileDetailsModel>(userProfile);
            viewModel.State = "Alabama";
            controller.Update(viewModel);
            Assert.AreEqual(userProfile.State, viewModel.State);
        }

        [Test]
        public void Map_Should_Update_Zip()
        {
            var userProfile = EntityHelpers.GetValidUserProfile();
            userProfile.Email = "goodEmail";
            repository.Add(userProfile);
            var viewModel = Mapper.Map<UserProfile, UserProfileDetailsModel>(userProfile);
            viewModel.ZipCode = "85310";
            controller.Update(viewModel);
            Assert.AreEqual(userProfile.ZipCode, viewModel.ZipCode);
        }

        [Test]
        public void Deactivate_Should_Redirect_To_LogOff_If_Successful()
        {
            var userProfile = EntityHelpers.GetValidUserProfile();
            userProfile.Email = "goodEmail";
            repository.Add(userProfile);
            var result = controller.Deactivate();
            Assert.IsInstanceOf(typeof(RedirectToRouteResult), result);
            var action = ((RedirectToRouteResult) result).RouteValues["Action"];
            Assert.AreEqual("LogOff", action);
        }

        [Test]
        public void Deactivate_Should_Set_Active_To_False_If_Successful()
        {
            var userProfile = EntityHelpers.GetValidUserProfile();
            userProfile.Email = "goodEmail";
            repository.Add(userProfile);
            Assert.IsTrue(userProfile.Active);
            controller.Deactivate();
            Assert.IsFalse(userProfile.Active);
        }

        [Test]
        public void Deactivate_Should_Return_Not_Found_If_UserProfile_Not_Found()
        {
            var result = controller.Deactivate();
            Assert.IsInstanceOf(typeof(HttpNotFoundResult), result);
        }

        [Test]
        public void Reactivate_Should_Redirect_To_Index_If_Successful()
        {
            var userProfile = EntityHelpers.GetValidUserProfile();
            userProfile.Email = "goodEmail";
            repository.Add(userProfile);
            var result = controller.Reactivate();
            Assert.IsInstanceOf(typeof(RedirectToRouteResult), result);
            var action = ((RedirectToRouteResult) result).RouteValues["Action"];
            Assert.AreEqual("Index", action);
        }

        [Test]
        public void Reactivate_Should_Set_Active_To_True_If_Successful()
        {
            var userProfile = EntityHelpers.GetValidUserProfile();
            userProfile.Email = "goodEmail";
            userProfile.Active = false;
            repository.Add(userProfile);
            controller.Reactivate();
            Assert.IsTrue(userProfile.Active);
        }

        [Test]
        public void Reactivate_Should_Return_Not_Found_If_UserProfile_Not_Found()
        {
            var result = controller.Reactivate();
            Assert.IsInstanceOf(typeof (HttpNotFoundResult), result);
        }

        private UserProfileController GetUserProfileController()
        {
            repository = new FakeUserProfileRepository();
            ((FakeUserProfileRepository)repository).SetUpRepository();
            var upc = new UserProfileController(repository);
            upc.ControllerContext = new ControllerContext
                                        {
                                            Controller = upc,
                                            RequestContext = new RequestContext(new MockHttpContext(), new RouteData()),
                                        };

            return upc;
        }
    }
}
