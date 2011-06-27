﻿//
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
using System.Collections.Specialized;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using JordanRift.Grassroots.Framework.Data;
using JordanRift.Grassroots.Framework.Entities.Models;
using JordanRift.Grassroots.Tests.Helpers;
using JordanRift.Grassroots.Web.Mailers;
using JordanRift.Grassroots.Web.Models;
using JordanRift.Grassroots.Web.Controllers;
using JordanRift.Grassroots.Tests.Fakes;
using Mvc.Mailer;
using NUnit.Framework;
using Rhino.Mocks;

namespace JordanRift.Grassroots.Tests.UnitTests.Controllers
{
    [TestFixture]
    public class AccountControllerTest
    {

        private IUserProfileRepository userProfileRepository;
        private UserProfile userProfile;

        [Test]
        public void UpdatePassword_Get_Returns_View()
        {
            // Arrange
            AccountController controller = GetAccountController();

            // Act
            ActionResult result = controller.ChangePassword();

            // Assert
            Assert.IsInstanceOf(typeof(ViewResult), result);
            Assert.AreEqual(10, ((ViewResult)result).ViewData["PasswordLength"]);
        }

        [Test]
        public void UpdatePassword_Post_Returns_Redirect_On_Success()
        {
            // Arrange
            AccountController controller = GetAccountController();
            ChangePasswordModel model = new ChangePasswordModel
                                            {
                                                OldPassword = "goodOldPassword",
                                                NewPassword = "goodNewPassword",
                                                ConfirmPassword = "goodNewPassword"
                                            };

            // Act
            ActionResult result = controller.UpdatePassword(model);

            // Assert
            Assert.IsInstanceOf(typeof(RedirectToRouteResult), result);
            RedirectToRouteResult redirectResult = (RedirectToRouteResult)result;
            Assert.AreEqual("ChangePasswordSuccess", redirectResult.RouteValues["action"]);
        }

        [Test]
        public void UpdatePassword_Post_Returns_Redirect_When_Password_Fails()
        {
            // Arrange
            AccountController controller = GetAccountController();
            ChangePasswordModel model = new ChangePasswordModel
                                            {
                                                OldPassword = "goodOldPassword",
                                                NewPassword = "badNewPassword",
                                                ConfirmPassword = "badNewPassword"
                                            };

            // Act
            ActionResult result = controller.UpdatePassword(model);

            // Assert
            Assert.IsInstanceOf(typeof(RedirectToRouteResult), result);
            Assert.IsNotNull(controller.TempData["ChangePasswordModel"]);
        }

        [Test]
        public void UpdatePassword_Post_Returns_Redirect_If_ModelState_Invalid()
        {
            // Arrange
            AccountController controller = GetAccountController();
            ChangePasswordModel model = new ChangePasswordModel
                                            {
                                                OldPassword = "goodOldPassword",
                                                NewPassword = "goodNewPassword",
                                                ConfirmPassword = "goodNewPassword"
                                            };

            controller.ModelState.AddModelError("", "Dummy error message.");

            // Act
            ActionResult result = controller.UpdatePassword(model);

            // Assert
            Assert.IsInstanceOf(typeof(RedirectToRouteResult), result);
            Assert.IsNotNull(controller.TempData["ChangePasswordModel"]);
        }

        [Test]
        public void ChangePasswordSuccess_Returns_View()
        {
            // Arrange
            AccountController controller = GetAccountController();

            // Act
            ActionResult result = controller.ChangePasswordSuccess();

            // Assert
            Assert.IsInstanceOf(typeof(ViewResult), result);
        }

        [Test]
        public void LogOff_Logs_Out_And_Redirects()
        {
            // Arrange
            AccountController controller = GetAccountController();

            // Act
            ActionResult result = controller.LogOff();

            // Assert
            Assert.IsInstanceOf(typeof(RedirectToRouteResult), result);
            RedirectToRouteResult redirectResult = (RedirectToRouteResult)result;
            Assert.AreEqual("Home", redirectResult.RouteValues["controller"]);
            Assert.AreEqual("Index", redirectResult.RouteValues["action"]);
            Assert.IsTrue(((MockFormsAuthenticationService)controller.FormsService).SignOut_WasCalled);
        }

        [Test]
        public void LogOn_Returns_View()
        {
            // Arrange
            AccountController controller = GetAccountController();

            // Act
            ActionResult result = controller.LogOn();

            // Assert
            Assert.IsInstanceOf(typeof(ViewResult), result);
        }

        [Test]
        public void AuthenticateUser_Returns_Redirect_On_Success_Without_ReturnUrl()
        {
            // Arrange
            AccountController controller = GetAccountController();
            userProfile = EntityHelpers.GetValidUserProfile();
            userProfile.Email = "goodEmail";
            userProfileRepository.Add(userProfile);
            LogOnModel model = new LogOnModel
                                   {
                                       Email = "goodEmail",
                                       Password = "goodPassword",
                                       RememberMe = false
                                   };

            // Act
            ActionResult result = controller.AuthenticateUser(model, null);

            // Assert
            Assert.IsInstanceOf(typeof(RedirectToRouteResult), result);
            RedirectToRouteResult redirectResult = (RedirectToRouteResult)result;
            Assert.AreEqual("UserProfile", redirectResult.RouteValues["controller"]);
            Assert.AreEqual("Index", redirectResult.RouteValues["action"]);
            Assert.IsTrue(((MockFormsAuthenticationService)controller.FormsService).SignIn_WasCalled);
        }

        [Test]
        public void AuthenticateUser_Returns_Redirect_On_Success_With_ReturnUrl()
        {
            // Arrange
            AccountController controller = GetAccountController();
            userProfile = EntityHelpers.GetValidUserProfile();
            userProfile.Email = "goodEmail";
            userProfileRepository.Add(userProfile);
            LogOnModel model = new LogOnModel
                                   {
                                       Email = "goodEmail",
                                       Password = "goodPassword",
                                       RememberMe = false
                                   };

            // Act
            ActionResult result = controller.AuthenticateUser(model, "/someUrl");

            // Assert
            Assert.IsInstanceOf(typeof(RedirectResult), result);
            RedirectResult redirectResult = (RedirectResult)result;
            Assert.AreEqual("/someUrl", redirectResult.Url);
            Assert.IsTrue(((MockFormsAuthenticationService)controller.FormsService).SignIn_WasCalled);
        }

        [Test]
        public void AuthenticateUser_Returns_Redirect_If_ModelState_IsInvalid()
        {
            // Arrange
            AccountController controller = GetAccountController();
            LogOnModel model = new LogOnModel
                                   {
                                       Email = "goodEmail",
                                       Password = "goodPassword",
                                       RememberMe = false
                                   };

            controller.ModelState.AddModelError("", "Dummy error message.");

            // Act
            ActionResult result = controller.AuthenticateUser(model, null);

            // Assert
            Assert.IsInstanceOf(typeof(RedirectToRouteResult), result);
            Assert.IsNotNull(controller.TempData["LogOnModel"]);
        }

        [Test]
        public void AuthenticateUser_Returns_Redirect_If_ValidateUser_Fails()
        {
            // Arrange
            AccountController controller = GetAccountController();
            userProfile = EntityHelpers.GetValidUserProfile();
            userProfile.Email = "goodEmail";
            userProfileRepository.Add(userProfile);
            LogOnModel model = new LogOnModel
                                   {
                                       Email = "goodEmail",
                                       Password = "badPassword",
                                       RememberMe = false
                                   };

            // Act
            ActionResult result = controller.AuthenticateUser(model, null);

            // Assert
            Assert.IsInstanceOf(typeof(RedirectToRouteResult), result);
            Assert.IsNotNull(controller.TempData["LogOnModel"]);
        }

        [Test]
        public void Register_Returns_View()
        {
            // Arrange
            AccountController controller = GetAccountController();

            // Act
            ActionResult result = controller.Register();

            // Assert
            Assert.IsInstanceOf(typeof(ViewResult), result);
            Assert.AreEqual(10, ((ViewResult)result).ViewData["PasswordLength"]);
        }

        [Test]
        public void RegisterUser_Returns_Redirect_On_Success()
        {
            // Arrange
            AccountController controller = GetAccountController();
            RegisterModel model = new RegisterModel
                                      {
                                          Email = "goodEmail",
                                          Password = "goodPassword",
                                          ConfirmPassword = "goodPassword",
                                          FirstName = "Jason",
                                          LastName = "Powers",
                                          Birthdate = DateTime.Parse("1/1/1970"),
                                          Gender = "M",
                                          ZipCode = "85213",
                                          Consent = true
                                      };

            // Act
            ActionResult result = controller.RegisterUser(model);

            // Assert
            Assert.IsInstanceOf(typeof(RedirectToRouteResult), result);
            Assert.IsNull(controller.TempData["RegisterModel"]);
        }

        [Test]
        public void RegisterUser_Returns_Redirect_If_Registration_Fails()
        {
            // Arrange
            AccountController controller = GetAccountController();
            RegisterModel model = new RegisterModel
                                      {
                                          Email = "duplicateEmail",
                                          Password = "goodPassword",
                                          ConfirmPassword = "goodPassword",
                                          FirstName = "Jason",
                                          LastName = "Powers",
                                          Birthdate = DateTime.Parse("1/1/1970"),
                                          Gender = "M",
                                          ZipCode = "85213",
                                          Consent = true
                                      };

            // Act
            ActionResult result = controller.RegisterUser(model);

            // Assert
            Assert.IsInstanceOf(typeof(RedirectToRouteResult), result);
            Assert.IsNotNull(controller.TempData["RegisterModel"]);
        }

        [Test]
        public void RegisterUser_Returns_Redirect_If_ModelState_Is_Invalid()
        {
            // Arrange
            AccountController controller = GetAccountController();
            RegisterModel model = new RegisterModel
                                      {
                                          Email = "goodEmail",
                                          Password = "goodPassword",
                                          ConfirmPassword = "goodPassword",
                                          FirstName = "Jason",
                                          LastName = "Powers",
                                          Birthdate = DateTime.Parse("1/1/1970"),
                                          Gender = "M",
                                          ZipCode = "85213",
                                          Consent = true
                                      };

            controller.ModelState.AddModelError("", "Dummy error message.");

            // Act
            ActionResult result = controller.RegisterUser(model);

            // Assert
            Assert.IsInstanceOf(typeof(RedirectToRouteResult), result);
            Assert.IsNotNull(controller.TempData["RegisterModel"]);
        }

        [Test]
        public void ForgotPassword_Returns_View()
        {
            var controller = GetAccountController();
            var result = controller.ForgotPassword();
            Assert.IsInstanceOf(typeof(ViewResult), result);
        }

        //[Test]
        //public void ResetPassword_Returns_Redirect_On_Success()
        //{
        //    var controller = GetAccountController();
        //    var userProfile = EntityHelpers.GetValidUserProfile();
        //    userProfile.Email = "info@jordanrift.com";
        //    userProfileRepository.Add(userProfile);
        //    var result = controller.ResetPassword(new ForgotPasswordModel { Email = "info@jordanrift.com" });
        //    Assert.IsInstanceOf(typeof(RedirectToRouteResult), result);
        //    var action = result as RedirectToRouteResult;
        //    var name = action.RouteValues["Action"];
        //    Assert.AreEqual("ResetPasswordSuccess", name);
        //}

        //[Test]
        //public void ResetPassword_Returns_Redirect_On_Failure()
        //{
        //    var controller = GetAccountController();
        //    var result = controller.ResetPassword(new ForgotPasswordModel { Email = "goodEmail" });
        //    Assert.IsInstanceOf(typeof(RedirectToRouteResult), result);
        //    var action = result as RedirectToRouteResult;
        //    var name = action.RouteValues["Action"];
        //    Assert.AreEqual("ForgotPassword", name);
        //}

        private AccountController GetAccountController()
        {
            var fakeOrganizationRepository = new FakeOrganizationRepository();
            fakeOrganizationRepository.SetUpRepository();
            userProfileRepository = new FakeUserProfileRepository();
            ((FakeUserProfileRepository)userProfileRepository).SetUpRepository();

            var mocks = new MockRepository();
            var fakeEmailService = mocks.DynamicMock<IAccountMailer>();
            MailerBase.IsTestModeEnabled = true;
            AccountController controller = new AccountController(userProfileRepository, fakeEmailService)
                                               {
                                                   FormsService = new MockFormsAuthenticationService(),
                                                   MembershipService = new MockMembershipService(),
                                                   OrganizationRepository = fakeOrganizationRepository
                                               };

            
            TestHelpers.MockHttpContext(controller);
            return controller;
        }
    }
}
