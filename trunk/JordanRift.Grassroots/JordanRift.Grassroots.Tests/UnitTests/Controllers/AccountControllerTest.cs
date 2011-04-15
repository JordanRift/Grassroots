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
using JordanRift.Grassroots.Framework.Services;
using JordanRift.Grassroots.Web.Models;
using JordanRift.Grassroots.Web.Controllers;
using JordanRift.Grassroots.Tests.Fakes;
using NUnit.Framework;
using Rhino.Mocks;

namespace JordanRift.Grassroots.Tests.UnitTests.Controllers
{

    [TestFixture]
    public class AccountControllerTest
    {

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
            Assert.AreEqual("Home", redirectResult.RouteValues["controller"]);
            Assert.AreEqual("Index", redirectResult.RouteValues["action"]);
            Assert.IsTrue(((MockFormsAuthenticationService)controller.FormsService).SignIn_WasCalled);
        }

        [Test]
        public void AuthenticateUser_Returns_Redirect_On_Success_With_ReturnUrl()
        {
            // Arrange
            AccountController controller = GetAccountController();
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
                                          PrimaryPhone = "(800) 555-1212",
                                          AddressLine1 = "555 S Main St",
                                          AddressLine2 = "",
                                          City = "Mesa",
                                          State = "AZ",
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
                                          PrimaryPhone = "(800) 555-1212",
                                          AddressLine1 = "555 S Main St",
                                          AddressLine2 = "",
                                          City = "Mesa",
                                          State = "AZ",
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
                                          PrimaryPhone = "(800) 555-1212",
                                          AddressLine1 = "555 S Main St",
                                          AddressLine2 = "",
                                          City = "Mesa",
                                          State = "AZ",
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

        [Test]
        public void ResetPassword_Returns_Redirect_On_Success()
        {
            var controller = GetAccountController();
            var result = controller.ResetPassword(new ForgotPasswordModel { Email = "info@jordanrift.com" });
            Assert.IsInstanceOf(typeof(RedirectToRouteResult), result);
            var action = result as RedirectToRouteResult;
            var name = action.RouteValues["Action"];
            Assert.AreEqual("ResetPasswordSuccess", name);
        }

        [Test]
        public void ResetPassword_Returns_Redirect_On_Failure()
        {
            var controller = GetAccountController();
            var result = controller.ResetPassword(new ForgotPasswordModel { Email = "bad-email@jordanrift.com" });
            Assert.IsInstanceOf(typeof(RedirectToRouteResult), result);
            var action = result as RedirectToRouteResult;
            var name = action.RouteValues["Action"];
            Assert.AreEqual("ForgotPassword", name);
        }

        private static AccountController GetAccountController()
        {
            var fakeOrganizationRepository = new FakeOrganizationRepository();
            fakeOrganizationRepository.SetUpRepository();
            var fakeUserProfileRepository = new FakeUserProfileRepository();
            fakeUserProfileRepository.SetUpRepository();
            var fakeUserRepository = new FakeUserRepository();
            fakeUserRepository.SetUpRepository();
            var fakeEmailService = MockRepository.GenerateMock<IEmailService>();
            AccountController controller = new AccountController(fakeUserRepository, fakeEmailService)
                                               {
                                                   FormsService = new MockFormsAuthenticationService(),
                                                   MembershipService = new MockMembershipService()
                                               };

            controller.OrganizationRepository = fakeOrganizationRepository;
            controller.ControllerContext = new ControllerContext
                                               {
                                                   Controller = controller,
                                                   RequestContext =
                                                       new RequestContext(new MockHttpContext(), new RouteData())
                                               };

            return controller;
        }
    }
}
