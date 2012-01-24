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
using AutoMapper;
using JordanRift.Grassroots.Framework.Data;
using JordanRift.Grassroots.Framework.Entities.Models;
using JordanRift.Grassroots.Framework.Services;
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
    public class UserControllerTests
    {
        private IUserProfileRepository userProfileRepository;
        private UserController controller;
        private MockRepository mocks;

        [SetUp]
        public void SetUp()
        {
            userProfileRepository = new FakeUserProfileRepository();
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
        public void ChangePassword_Returns_View_When_User_Found()
        {
            var user = EntityHelpers.GetValidUserProfile();
            userProfileRepository.Add(user);
            var result = controller.ChangePassword(user.UserProfileID);
            Assert.IsInstanceOf<ViewResult>(result);
        }

        [Test]
        public void ChangePassword_Returns_NotFound_When_User_Not_Found()
        {
            var result = controller.ChangePassword();
            Assert.IsInstanceOf<HttpNotFoundResult>(result);
        }

        [Test]
        public void ChangePassword_Should_Display_ModelErrors_If_Found_In_TempData()
        {
            var userProfile = EntityHelpers.GetValidUserProfile();
            userProfileRepository.Add(userProfile);
            controller.TempData["ModelErrors"] = new List<string> { "something bad happened" };
            controller.ChangePassword(userProfile.UserProfileID);
            Assert.IsFalse(controller.ModelState.IsValid);
        }

        [Test]
        public void SavePassword_Should_Redirect_To_Admin_On_Success()
        {
            var userProfile = EntityHelpers.GetValidUserProfile();
            userProfileRepository.Add(userProfile);
            var model = Mapper.Map<UserProfile, PasswordAdminModel>(userProfile);
            model.Password = "goodPassword";
            model.ConfirmPassword = "goodPassword";
            var result = controller.SavePassword(model);
            Assert.IsInstanceOf<RedirectToRouteResult>(result);
            var redirect = result as RedirectToRouteResult;
            Assert.AreEqual("Admin", redirect.RouteValues["Action"]);
        }

        [Test]
        public void SavePassword_Should_Return_NotFound_If_UserProfile_Not_Found()
        {
            var userProfile = EntityHelpers.GetValidUserProfile();
            var model = Mapper.Map<UserProfile, PasswordAdminModel>(userProfile);
            model.Password = "goodPassword";
            model.ConfirmPassword = "goodPassword";
            var result = controller.SavePassword(model);
            Assert.IsInstanceOf<HttpNotFoundResult>(result);
        }

        [Test]
        public void SavePassword_Should_Redirect_To_ChangePassword_If_ModelState_Is_Invalid()
        {
            controller.ModelState.AddModelError("", "Oops");
            var result = controller.SavePassword(new PasswordAdminModel());
            Assert.IsInstanceOf<RedirectToRouteResult>(result);
            var redirect = result as RedirectToRouteResult;
            Assert.AreEqual("ChangePassword", redirect.RouteValues["Action"]);
        }

        [Test]
        public void SavePassword_Should_Update_Password_When_Successful()
        {
            const string newPasssword = "newPassword";
            var userProfile = EntityHelpers.GetValidUserProfile();
            var user = userProfile.Users.FirstOrDefault();
            userProfileRepository.Add(userProfile);
            var model = Mapper.Map<UserProfile, PasswordAdminModel>(userProfile);
            model.Password = newPasssword;
            model.ConfirmPassword = newPasssword;
            controller.SavePassword(model);
            Assert.IsTrue(GrassrootsMembershipService.VerifyPasswordHash(newPasssword, user.Password));
        }

        [Test]
        public void SavePassword_Should_Create_User_Record_When_None_Present()
        {
            const string newPasssword = "newPassword";
            var userProfile = EntityHelpers.GetValidUserProfile();
            userProfile.Users.Clear();
            userProfileRepository.Add(userProfile);
            var model = Mapper.Map<UserProfile, PasswordAdminModel>(userProfile);
            model.Password = newPasssword;
            model.ConfirmPassword = newPasssword;
            controller.SavePassword(model);
            var user = userProfile.Users.FirstOrDefault();
            Assert.IsNotNull(user);
            Assert.IsTrue(GrassrootsMembershipService.VerifyPasswordHash(newPasssword, user.Password));
        }

        [Test]
        public void SavePassword_Should_Set_ForcePasswordChange_To_True_When_Checked()
        {
            const string newPasssword = "newPassword";
            var userProfile = EntityHelpers.GetValidUserProfile();
            var user = userProfile.Users.FirstOrDefault();
            userProfileRepository.Add(userProfile);
            var model = Mapper.Map<UserProfile, PasswordAdminModel>(userProfile);
            model.Password = newPasssword;
            model.ConfirmPassword = newPasssword;
            model.ForcePasswordChange = true;
            controller.SavePassword(model);
            Assert.IsTrue(user.ForcePasswordChange);
        }

        private void SetUpController()
        {
            var mailer = mocks.DynamicMock<IUserMailer>();
            MailerBase.IsTestModeEnabled = true;
            controller = new UserController(userProfileRepository, mailer)
                             {
                                 OrganizationRepository = new FakeOrganizationRepository()
                             };

            TestHelpers.MockBasicRequest(controller);
        }
    }
}
