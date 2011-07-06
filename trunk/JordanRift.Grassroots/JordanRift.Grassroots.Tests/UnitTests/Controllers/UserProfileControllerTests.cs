//
// Grassroots is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// Grassroots is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with Grassroots.  If not, see <http://www.gnu.org/licenses/>.
//

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using AutoMapper;
using JordanRift.Grassroots.Framework.Entities.Models;
using JordanRift.Grassroots.Tests.Helpers;
using JordanRift.Grassroots.Web.Controllers;
using JordanRift.Grassroots.Framework.Data;
using JordanRift.Grassroots.Tests.Fakes;
using JordanRift.Grassroots.Web.Mailers;
using JordanRift.Grassroots.Web.Models;
using Mvc.Mailer;
using NUnit.Framework;
using Rhino.Mocks;

namespace JordanRift.Grassroots.Tests.UnitTests.Controllers
{
    [TestFixture]
    public class UserProfileControllerTests
    {
        private UserProfileController controller;
        private IUserProfileRepository repository;
        private MockRepository mocks;
        private UserProfile userProfile;

        [SetUp]
        public void SetUp()
        {
            userProfile = EntityHelpers.GetValidUserProfile();
            repository = new FakeUserProfileRepository();
            repository.Add(userProfile);
            mocks = new MockRepository();
            controller = GetUserProfileController(userProfile.UserProfileID);
            Mapper.CreateMap<UserProfile, UserProfileDetailsModel>();
        }

        [TearDown]
        public void TearDown()
        {
            FakeUserProfileRepository.Reset();
        }

        [Test]
        public void Index_Should_Return_View_If_UserProfile_Found()
        {
            userProfile.Email = "goodEmail";
            userProfile.Campaigns = new List<Campaign>();
            mocks.ReplayAll();
            var result = controller.Index();
            Assert.IsInstanceOf(typeof(ViewResult), result);
            mocks.VerifyAll();
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
            userProfile.Email = "goodEmail";
            var viewModel = Mapper.Map<UserProfile, UserProfileDetailsModel>(userProfile);
            var result = controller.Update(viewModel);
            Assert.IsInstanceOf(typeof(RedirectToRouteResult), result);
            Assert.AreEqual("Index", ((RedirectToRouteResult)result).RouteValues["Action"]);
        }

        [Test]
        public void Update_Should_Redirect_To_Edit_If_Model_Is_Not_Valid()
        {
            userProfile.Email = "goodEmail";
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
            userProfile.Email = "goodEmail";
            var viewModel = Mapper.Map<UserProfile, UserProfileDetailsModel>(userProfile);
            viewModel.FirstName = "NewFirstName";
            controller.Update(viewModel);
            Assert.AreEqual(userProfile.FirstName, viewModel.FirstName);
        }

        [Test]
        public void Map_Should_Update_LastName()
        {
            userProfile.Email = "goodEmail";
            var viewModel = Mapper.Map<UserProfile, UserProfileDetailsModel>(userProfile);
            viewModel.LastName = "NewLasttName";
            controller.Update(viewModel);
            Assert.AreEqual(userProfile.LastName, viewModel.LastName);
        }

        [Test]
        public void Map_Should_Update_Birthdate()
        {
            userProfile.Email = "goodEmail";
            var viewModel = Mapper.Map<UserProfile, UserProfileDetailsModel>(userProfile);
            viewModel.Birthdate = new DateTime(1981, 12, 1);
            controller.Update(viewModel);
            Assert.AreEqual(userProfile.Birthdate, viewModel.Birthdate);
        }

        [Test]
        public void Map_Should_Update_PrimaryPhone()
        {
            userProfile.Email = "goodEmail";
            var viewModel = Mapper.Map<UserProfile, UserProfileDetailsModel>(userProfile);
            viewModel.PrimaryPhone = "602-555-7777";
            controller.Update(viewModel);
            Assert.AreEqual(userProfile.PrimaryPhone, viewModel.PrimaryPhone);
        }

        [Test]
        public void Map_Should_Update_Gender()
        {
            userProfile.Email = "goodEmail";
            var viewModel = Mapper.Map<UserProfile, UserProfileDetailsModel>(userProfile);
            viewModel.Gender = "female";
            controller.Update(viewModel);
            Assert.AreEqual(userProfile.Gender, viewModel.Gender);
        }

        [Test]
        public void Map_Should_Update_AddressLine1()
        {
            userProfile.Email = "goodEmail";
            var viewModel = Mapper.Map<UserProfile, UserProfileDetailsModel>(userProfile);
            viewModel.AddressLine1 = "New Street Address";
            controller.Update(viewModel);
            Assert.AreEqual(userProfile.AddressLine1, viewModel.AddressLine1);
        }

        [Test]
        public void Map_Should_Update_AddressLine2()
        {
            userProfile.Email = "goodEmail";
            var viewModel = Mapper.Map<UserProfile, UserProfileDetailsModel>(userProfile);
            viewModel.AddressLine2 = "New Address Line 2";
            controller.Update(viewModel);
            Assert.AreEqual(userProfile.AddressLine2, viewModel.AddressLine2);
        }

        [Test]
        public void Map_Should_Update_City()
        {
            userProfile.Email = "goodEmail";
            var viewModel = Mapper.Map<UserProfile, UserProfileDetailsModel>(userProfile);
            viewModel.City = "New City";
            controller.Update(viewModel);
            Assert.AreEqual(userProfile.City, viewModel.City);
        }

        [Test]
        public void Map_Should_Update_State()
        {
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
            userProfile.Email = "goodEmail";
            var viewModel = Mapper.Map<UserProfile, UserProfileDetailsModel>(userProfile);
            viewModel.ZipCode = "85310";
            controller.Update(viewModel);
            Assert.AreEqual(userProfile.ZipCode, viewModel.ZipCode);
        }

        [Test]
        public void Deactivate_Should_Redirect_To_LogOff_If_Successful()
        {
            userProfile.Email = "goodEmail";
            userProfile.Organization = EntityHelpers.GetValidOrganization();
            var result = controller.Deactivate();
            Assert.IsInstanceOf(typeof(RedirectToRouteResult), result);
            var action = ((RedirectToRouteResult) result).RouteValues["Action"];
            Assert.AreEqual("LogOff", action);
        }

        [Test]
        public void Deactivate_Should_Set_Active_To_False_If_Successful()
        {
            userProfile.Email = "goodEmail";
            userProfile.Organization = EntityHelpers.GetValidOrganization();
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
            userProfile.Email = "goodEmail";
            var result = controller.Reactivate();
            Assert.IsInstanceOf(typeof(RedirectToRouteResult), result);
            var action = ((RedirectToRouteResult) result).RouteValues["Action"];
            Assert.AreEqual("Index", action);
        }

        [Test]
        public void Reactivate_Should_Set_Active_To_True_If_Successful()
        {
            userProfile.Email = "goodEmail";
            userProfile.Active = false;
            controller.Reactivate();
            Assert.IsTrue(userProfile.Active);
        }

        [Test]
        public void Reactivate_Should_Return_Not_Found_If_UserProfile_Not_Found()
        {
            var result = controller.Reactivate();
            Assert.IsInstanceOf(typeof (HttpNotFoundResult), result);
        }

        private UserProfileController GetUserProfileController(int userProfileID = -1)
        {
            var causeRepository = mocks.StrictMock<ICauseRepository>();
            Expect.Call(causeRepository.FindCausesByUserProfileID(userProfileID)).Return(new List<Cause>().AsQueryable());
            var mailer = mocks.DynamicMock<IUserProfileMailer>();
            MailerBase.IsTestModeEnabled = true;
            var upc = new UserProfileController(repository, causeRepository, mailer);
            TestHelpers.MockHttpContext(upc);
            return upc;
        }
    }
}
