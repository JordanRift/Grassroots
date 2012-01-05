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

using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using JordanRift.Grassroots.Framework.Data;
using JordanRift.Grassroots.Framework.Entities.Models;
using JordanRift.Grassroots.Tests.Fakes;
using JordanRift.Grassroots.Tests.Helpers;
using JordanRift.Grassroots.Web.Controllers;
using JordanRift.Grassroots.Web.Mailers;
using JordanRift.Grassroots.Web.Models;
using Mvc.Mailer;
using NUnit.Framework;
using Rhino.Mocks;

namespace JordanRift.Grassroots.Tests.UnitTests.Controllers
{
    [TestFixture]
    public class FacebookControllerTests
    {
        private IUserProfileRepository userProfileRepository;
        private IOrganizationRepository organizationRepository;
        private FacebookController controller;
        private MockRepository mocks;
        private const string GOOD_EMAIL = "goodEmail";
        private const string GOOD_PASSWORD = "goodPassword";

        [SetUp]
        public void SetUp()
        {
            userProfileRepository = new FakeUserProfileRepository();
            organizationRepository = new FakeOrganizationRepository();
            mocks = new MockRepository();
            SetUpController();
        }

        [TearDown]
        public void TearDown()
        {
            FakeUserProfileRepository.Reset();
            FakeOrganizationRepository.Reset();
            mocks = null;
        }

        [Test]
        public void Disconnect_Should_Return_View()
        {
            var userProfile = EntityHelpers.GetValidUserProfile();
            userProfile.Email = GOOD_EMAIL;
            userProfileRepository.Add(userProfile);
            var result = controller.Disconnect();
            Assert.IsInstanceOf<ViewResult>(result);
        }

        [Test]
        public void Disconnect_Should_Display_ModelErrors_If_Present_In_TempData()
        {
            var userProfile = EntityHelpers.GetValidUserProfile();
            userProfile.Email = GOOD_EMAIL;
            userProfileRepository.Add(userProfile);
            controller.TempData["ModelErrors"] = new List<string> { "uh oh..." };
            controller.Disconnect();
            Assert.IsFalse(controller.ModelState.IsValid);
        }

        [Test]
        public void Disconnect_Should_Populate_User_Info_In_Model()
        {
            var userProfile = EntityHelpers.GetValidUserProfile();
            userProfile.Email = GOOD_EMAIL;
            userProfileRepository.Add(userProfile);
            var result = controller.Disconnect() as ViewResult;
            var model = result.Model as FacebookDisconnectModel;
            Assert.AreEqual(userProfile.Email, model.Email);
            Assert.AreEqual(userProfile.FullName, model.FullName);
        }

        [Test]
        public void Disconnect_Should_Return_NotFound_If_UserProfile_Not_Found()
        {
            var result = controller.Disconnect();
            Assert.IsInstanceOf<HttpNotFoundResult>(result);
        }

        [Test]
        public void DisconnectAccount_Should_Redirect_To_UserProfile_When_Successful()
        {
            var userProfile = EntityHelpers.GetValidUserProfile();
            var user = EntityHelpers.GetValidUser();
            userProfile.Email = GOOD_EMAIL;
            userProfile.Users = new List<User> { user };
            userProfileRepository.Add(userProfile);
            var result = controller.DisconnectAccount(new FacebookDisconnectModel());
            Assert.IsInstanceOf<RedirectToRouteResult>(result);
            var redirect = result as RedirectToRouteResult;
            Assert.AreEqual("UserProfile", redirect.RouteValues["Controller"]);
            Assert.AreEqual("Index", redirect.RouteValues["Action"]);
        }

        [Test]
        public void DisconnectAccount_Should_Redirect_to_UserProfile_When_Successful_And_User_Not_Present()
        {
            var userProfile = EntityHelpers.GetValidUserProfile();
            userProfile.Email = GOOD_EMAIL;
            userProfile.Users = new List<User>();
            userProfileRepository.Add(userProfile);
            var result = controller.DisconnectAccount(new FacebookDisconnectModel{ Password = GOOD_PASSWORD });
            Assert.IsInstanceOf<RedirectToRouteResult>(result);
            var redirect = result as RedirectToRouteResult;
            Assert.AreEqual("UserProfile", redirect.RouteValues["Controller"]);
            Assert.AreEqual("Index", redirect.RouteValues["Action"]);
        }

        [Test]
        public void DisconnectAccount_Should_Redirect_To_Disconnect_If_ModelState_Is_Invalid()
        {
            controller.ModelState.AddModelError("", "errorz!");
            var result = controller.DisconnectAccount(new FacebookDisconnectModel());
            Assert.IsInstanceOf<RedirectToRouteResult>(result);
            var redirect = result as RedirectToRouteResult;
            Assert.AreEqual("Disconnect", redirect.RouteValues["Action"]);
        }

        [Test]
        public void DisconnectAccount_Should_Add_ModelErrors_To_TempData_If_ModelState_Is_Invalid()
        {
            controller.ModelState.AddModelError("", "boo!");
            controller.DisconnectAccount(new FacebookDisconnectModel());
            Assert.IsNotNull(controller.TempData["ModelErrors"]);
        }

        [Test]
        public void DisconnectAccount_Should_Return_NotFound_If_UserProfile_Not_Found()
        {
            var userProfile = EntityHelpers.GetValidUserProfile();
            userProfile.Email = "badEmail";
            userProfileRepository.Add(userProfile);
            var result = controller.DisconnectAccount(new FacebookDisconnectModel());
            Assert.IsInstanceOf<HttpNotFoundResult>(result);
        }

        [Test]
        public void DisconnectAccount_Should_Create_User_Record_If_None_Exists()
        {
            var userProfile = EntityHelpers.GetValidUserProfile();
            userProfile.Email = GOOD_EMAIL;
            userProfile.Users = new List<User>();
            userProfileRepository.Add(userProfile);
            controller.DisconnectAccount(new FacebookDisconnectModel{ Password = GOOD_PASSWORD });
            Assert.Greater(userProfile.Users.Count, 0);
        }

        [Test]
        public void DisconnectAccount_Should_Hash_Password_Before_Saving_New_User()
        {
            var userProfile = EntityHelpers.GetValidUserProfile();
            userProfile.Email = GOOD_EMAIL;
            userProfile.Users = new List<User>();
            userProfileRepository.Add(userProfile);
            controller.DisconnectAccount(new FacebookDisconnectModel{ Password = GOOD_PASSWORD });
            var user = userProfile.Users.First();
            Assert.AreNotEqual(GOOD_PASSWORD, user.Password);
        }

        [Test]
        public void DisconnectAccount_Should_Set_FacebookID_To_Null_When_Successful()
        {
            var userProfile = EntityHelpers.GetValidUserProfile();
            var user = EntityHelpers.GetValidUser();
            userProfile.Email = GOOD_EMAIL;
            userProfile.FacebookID = "123";
            userProfile.Users = new List<User> { user };
            userProfileRepository.Add(userProfile);
            controller.DisconnectAccount(new FacebookDisconnectModel());
            Assert.IsNull(userProfile.FacebookID);
        }

        private void SetUpController()
        {
            var mailer = mocks.DynamicMock<IAccountMailer>();
            MailerBase.IsTestModeEnabled = true;
            controller = new FacebookController(userProfileRepository, mailer)
                             {
                                 OrganizationRepository = organizationRepository,
                             };

            TestHelpers.MockBasicRequest(controller);
        }
    }
}
