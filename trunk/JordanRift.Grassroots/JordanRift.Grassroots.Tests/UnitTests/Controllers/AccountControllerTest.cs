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
using System.Linq;
using System.Web.Mvc;
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
        private ICampaignDonorRepository campaignDonorRepository;
        private UserProfile userProfile;
        private AccountController controller;
        private MockRepository mocks;

        [SetUp]
        public void SetUp()
        {
            userProfile = null;
            mocks = new MockRepository();
            SetUpController();
        }

        [TearDown]
        public void TearDown()
        {
            FakeUserProfileRepository.Reset();
            FakeOrganizationRepository.Reset();
            FakeUserRepository.Reset();
            controller = null;
            mocks = null;
        }

        [Test]
        public void ChangePassword_Returns_View()
        {
            ActionResult result = controller.ChangePassword();
            Assert.IsInstanceOf(typeof(ViewResult), result);
            Assert.AreEqual(10, ((ViewResult)result).ViewData["PasswordLength"]);
        }

        [Test]
        public void SavePassword_Returns_Redirect_On_Success()
        {
            var userProfile = EntityHelpers.GetValidUserProfile();
            userProfile.Email = "goodEmail";
            userProfileRepository.Add(userProfile);
            ChangePasswordModel model = new ChangePasswordModel
                                            {
                                                OldPassword = "goodOldPassword",
                                                NewPassword = "goodNewPassword",
                                                ConfirmPassword = "goodNewPassword"
                                            };

            ActionResult result = controller.SavePassword(model);
            Assert.IsInstanceOf(typeof(RedirectToRouteResult), result);
            RedirectToRouteResult redirectResult = (RedirectToRouteResult)result;
            Assert.AreEqual("ChangePasswordSuccess", redirectResult.RouteValues["action"]);
        }

        [Test]
        public void SavePassword_Returns_Redirect_When_Password_Fails()
        {
            var userProfile = EntityHelpers.GetValidUserProfile();
            userProfile.Email = "goodEmail";
            userProfileRepository.Add(userProfile);
            ChangePasswordModel model = new ChangePasswordModel
                                            {
                                                OldPassword = "goodOldPassword",
                                                NewPassword = "badNewPassword",
                                                ConfirmPassword = "badNewPassword"
                                            };

            ActionResult result = controller.SavePassword(model);
            Assert.IsInstanceOf(typeof(RedirectToRouteResult), result);
            Assert.IsNotNull(controller.TempData["ChangePasswordModel"]);
        }

        [Test]
        public void SavePassword_Returns_Redirect_If_ModelState_Invalid()
        {
            ChangePasswordModel model = new ChangePasswordModel
                                            {
                                                OldPassword = "goodOldPassword",
                                                NewPassword = "goodNewPassword",
                                                ConfirmPassword = "goodNewPassword"
                                            };

            controller.ModelState.AddModelError("", "Dummy error message.");
            ActionResult result = controller.SavePassword(model);
            Assert.IsInstanceOf(typeof(RedirectToRouteResult), result);
            Assert.IsNotNull(controller.TempData["ChangePasswordModel"]);
        }

        [Test]
        public void SavePassword_Redirects_To_Url_When_Present()
        {
            var userProfile = EntityHelpers.GetValidUserProfile();
            userProfile.Email = "goodEmail";
            userProfileRepository.Add(userProfile);
            ChangePasswordModel model = new ChangePasswordModel
                                            {
                                                OldPassword = "goodOldPassword",
                                                NewPassword = "goodNewPassword",
                                                ConfirmPassword = "goodNewPassword"
                                            };
            ActionResult result = controller.SavePassword(model, "foo.url");
            Assert.IsInstanceOf<RedirectResult>(result);
        }

        [Test]
        public void ChangePasswordSuccess_Returns_View()
        {
            ActionResult result = controller.ChangePasswordSuccess();
            Assert.IsInstanceOf(typeof(ViewResult), result);
        }

        [Test]
        public void LogOff_Logs_Out_And_Redirects()
        {
            ActionResult result = controller.LogOff();
            Assert.IsInstanceOf(typeof(RedirectToRouteResult), result);
            RedirectToRouteResult redirectResult = (RedirectToRouteResult)result;
            Assert.AreEqual("Home", redirectResult.RouteValues["controller"]);
            Assert.AreEqual("Index", redirectResult.RouteValues["action"]);
            Assert.IsTrue(((MockFormsAuthenticationService)controller.FormsService).SignOut_WasCalled);
        }

        [Test]
        public void LogOn_Returns_View()
        {
            ActionResult result = controller.LogOn();
            Assert.IsInstanceOf(typeof(ViewResult), result);
        }

        [Test]
        public void LogOn_Sets_LastLoginAttempt_In_TempData_When_Found_In_Model()
        {
            var model = new LogOnModel { LastLoginAttempt = DateTime.Now };
            controller.TempData["LogOnModel"] = model;
            controller.LogOn();
            var lastLogin = controller.TempData["LastLogOnAttempt"];
            Assert.IsNotNull(lastLogin);
        }

        [Test]
        public void AuthenticateUser_Returns_Redirect_On_Success_Without_ReturnUrl()
        {
            userProfile = EntityHelpers.GetValidUserProfile();
            userProfile.Email = "goodEmail";
            userProfileRepository.Add(userProfile);
            LogOnModel model = new LogOnModel
                                   {
                                       Email = "goodEmail",
                                       Password = "goodPassword",
                                       RememberMe = false
                                   };

            ActionResult result = controller.AuthenticateUser(model, null);
            Assert.IsInstanceOf(typeof(RedirectToRouteResult), result);
            RedirectToRouteResult redirectResult = (RedirectToRouteResult)result;
            Assert.AreEqual("UserProfile", redirectResult.RouteValues["controller"]);
            Assert.AreEqual("Index", redirectResult.RouteValues["action"]);
            Assert.IsTrue(((MockFormsAuthenticationService)controller.FormsService).SignIn_WasCalled);
        }

        [Test]
        public void AuthenticateUser_Returns_Redirect_On_Success_With_ReturnUrl()
        {
            userProfile = EntityHelpers.GetValidUserProfile();
            userProfile.Email = "goodEmail";
            userProfileRepository.Add(userProfile);
            LogOnModel model = new LogOnModel
                                   {
                                       Email = "goodEmail",
                                       Password = "goodPassword",
                                       RememberMe = false
                                   };

            ActionResult result = controller.AuthenticateUser(model, "/someUrl");
            Assert.IsInstanceOf(typeof(RedirectResult), result);
            RedirectResult redirectResult = (RedirectResult)result;
            Assert.AreEqual("/someUrl", redirectResult.Url);
            Assert.IsTrue(((MockFormsAuthenticationService)controller.FormsService).SignIn_WasCalled);
        }

        [Test]
        public void AuthenticateUser_Returns_Redirect_If_ModelState_IsInvalid()
        {
            LogOnModel model = new LogOnModel
                                   {
                                       Email = "goodEmail",
                                       Password = "goodPassword",
                                       RememberMe = false
                                   };

            controller.ModelState.AddModelError("", "Dummy error message.");
            ActionResult result = controller.AuthenticateUser(model, null);
            Assert.IsInstanceOf(typeof(RedirectToRouteResult), result);
            Assert.IsNotNull(controller.TempData["LogOnModel"]);
        }

        [Test]
        public void AuthenticateUser_Returns_Redirect_If_ValidateUser_Fails()
        {
            userProfile = EntityHelpers.GetValidUserProfile();
            userProfile.Email = "goodEmail";
            userProfileRepository.Add(userProfile);
            LogOnModel model = new LogOnModel
                                   {
                                       Email = "goodEmail",
                                       Password = "badPassword",
                                       RememberMe = false
                                   };

            ActionResult result = controller.AuthenticateUser(model, null);
            Assert.IsInstanceOf(typeof(RedirectToRouteResult), result);
            Assert.IsNotNull(controller.TempData["LogOnModel"]);
        }

        [Test]
        public void AuthenticateUser_Returns_Redirect_If_MaxPasswordAttempts_Met()
        {
            userProfile = EntityHelpers.GetValidUserProfile();
            userProfile.Email = "goodEmail";
            var user = userProfile.Users.First();
            user.FailedLoginAttempts = 3;
            userProfileRepository.Add(userProfile);
            var model = new LogOnModel { Email = "goodEmail", Password = "badPassword", RememberMe = false };
            var result = controller.AuthenticateUser(model, null);
            Assert.IsInstanceOf<RedirectToRouteResult>(result);
            var redirect = result as RedirectToRouteResult;
            Assert.AreEqual("LogOn", redirect.RouteValues["Action"]);
        }

        [Test]
        public void AuthenticateUser_Returns_Redirect_If_MaxPasswordAttempts_Exceeded()
        {
            userProfile = EntityHelpers.GetValidUserProfile();
            userProfile.Email = "goodEmail";
            var user = userProfile.Users.First();
            user.FailedLoginAttempts = 4;
            userProfileRepository.Add(userProfile);
            var model = new LogOnModel { Email = "goodEmail", Password = "badPassword", RememberMe = false };
            controller.TempData["LastLogOnAttempt"] = DateTime.Now;
            var result = controller.AuthenticateUser(model, null);
            Assert.IsInstanceOf<RedirectToRouteResult>(result);
            var redirect = result as RedirectToRouteResult;
            Assert.AreEqual("LogOn", redirect.RouteValues["Action"]);
            Assert.AreEqual(model.RemainingSeconds, user.FailedLoginAttempts);
        }

        [Test]
        public void AuthenticateUser_Sets_Wait_Time_In_Model_If_MaxPasswordAttempts_Met()
        {
            userProfile = EntityHelpers.GetValidUserProfile();
            userProfile.Email = "goodEmail";
            var user = userProfile.Users.First();
            user.FailedLoginAttempts = 3;
            userProfileRepository.Add(userProfile);
            var model = new LogOnModel { Email = "goodEmail", Password = "badPassword", RememberMe = false };
            controller.AuthenticateUser(model, null);
            model = controller.TempData["LogOnModel"] as LogOnModel;
            Assert.Greater(model.RemainingSeconds, 0);
        }

        [Test]
        public void AuthenticateUser_Sets_Last_Attempt_Date_In_Model_If_MaxPasswordAttempts_Met()
        {
            userProfile = EntityHelpers.GetValidUserProfile();
            userProfile.Email = "goodEmail";
            var user = userProfile.Users.First();
            user.FailedLoginAttempts = 3;
            userProfileRepository.Add(userProfile);
            var model = new LogOnModel { Email = "goodEmail", Password = "badPassword", RememberMe = false };
            controller.AuthenticateUser(model, null);
            model = controller.TempData["LogOnModel"] as LogOnModel;
            Assert.Greater(model.LastLoginAttempt, DateTime.MinValue);
        }

        [Test]
        public void AuthenticateUser_Redirects_To_ChangePassword_If_ForcePasswordChange_Is_True()
        {
            userProfile = EntityHelpers.GetValidUserProfile();
            userProfile.Email = "goodEmail";
            var user = userProfile.Users.First();
            user.ForcePasswordChange = true;
            userProfileRepository.Add(userProfile);
            var model = new LogOnModel { Email = "goodEmail", Password = "goodPassword", RememberMe = false };
            var result = controller.AuthenticateUser(model, null);
            Assert.IsInstanceOf<RedirectToRouteResult>(result);
            var redirect = result as RedirectToRouteResult;
            Assert.AreEqual("ChangePassword", redirect.RouteValues["Action"]);
        }

        [Test]
        public void Register_Returns_View()
        {
            ActionResult result = controller.Register();
            Assert.IsInstanceOf(typeof(ViewResult), result);
            Assert.AreEqual(10, ((ViewResult)result).ViewData["PasswordLength"]);
        }

        [Test]
        public void RegisterUser_Returns_Redirect_On_Success()
        {
            TestHelpers.MockBasicRequest(controller, mocks, false, true);
            mocks.ReplayAll();
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

            ActionResult result = controller.RegisterUser(model);
            Assert.IsInstanceOf(typeof(RedirectToRouteResult), result);
            Assert.IsNull(controller.TempData["RegisterModel"]);
        }

        [Test]
        public void RegisterUser_Returns_Redirect_If_Registration_Fails()
        {
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

            ActionResult result = controller.RegisterUser(model);
            Assert.IsInstanceOf(typeof(RedirectToRouteResult), result);
            Assert.IsNotNull(controller.TempData["RegisterModel"]);
        }

        [Test]
        public void RegisterUser_Returns_Redirect_If_ModelState_Is_Invalid()
        {
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

            ActionResult result = controller.RegisterUser(model);
            Assert.IsInstanceOf(typeof(RedirectToRouteResult), result);
            Assert.IsNotNull(controller.TempData["RegisterModel"]);
        }

        [Test]
        public void ForgotPassword_Returns_View()
        {
            var result = controller.ForgotPassword();
            Assert.IsInstanceOf(typeof(ViewResult), result);
        }

        [Test]
        public void UpdateForgottenPassword_Returns_Redirect_If_Hash_Not_Found()
        {
            var hash = "badHash";
            var model = new UpdatePasswordModel
                            {
                                ActivationHash = hash,
                                ActivationPin = "goodPin"
                            };

            var result = controller.UpdateForgottenPassword(hash, model);
            Assert.IsInstanceOf(typeof(RedirectToRouteResult), result);

            var message = controller.TempData["UserFeedback"];
            Assert.AreEqual("The email you are looking for could not be found in our system.", message);
        }

        [Test]
        public void UpdateForgottenPassword_Returns_Redirect_If_Hash_Found_And_Pins_Match()
        {
            userProfile = EntityHelpers.GetValidUserProfile();
            var hash = "goodHash";
            var pin = "goodPin";
            userProfile.ActivationHash = hash;
            userProfile.ActivationPin = pin;
            userProfileRepository.Add(userProfile);
            var model = new UpdatePasswordModel
                            {
                                ActivationHash = hash,
                                ActivationPin = pin,
                                Password = "secret",
                                ConfirmPassword = "secret"
                            };

            var result = controller.UpdateForgottenPassword(hash, model);
            Assert.IsInstanceOf(typeof(RedirectToRouteResult), result);

            var message = controller.TempData["UserFeedback"];
            Assert.AreEqual("Sweet! Your password has been changed. You can now log in with your new password.", message);
        }

        [Test]
        public void UpdateForgottenPassword_Returns_Redirect_If_Hash_Found_And_Pins_Do_Not_Match()
        {
            userProfile = EntityHelpers.GetValidUserProfile();
            var hash = "goodHash";
            var pin = "goodPin";
            userProfile.ActivationHash = hash;
            userProfile.ActivationPin = pin;
            userProfileRepository.Add(userProfile);
            var model = new UpdatePasswordModel
                            {
                                ActivationHash = hash,
                                ActivationPin = "badPin",
                                Password = "secret",
                                ConfirmPassword = "secret"
                            };

            var result = controller.UpdateForgottenPassword(hash, model);
            Assert.IsInstanceOf(typeof(RedirectToRouteResult), result);

            var message = controller.TempData["UserFeedback"];
            Assert.IsNull(message);
        }

        [Test]
        public void UpdateForgottenPassword_Returns_Redirect_If_ModelState_Is_Invalid()
        {
            controller.ModelState.AddModelError("", "Uh oh...");
            var result = controller.UpdateForgottenPassword("goodHash", new UpdatePasswordModel());
            Assert.IsInstanceOf(typeof(RedirectToRouteResult), result);

            var message = controller.TempData["UserFeedback"];
            Assert.IsNull(message);
        }

        [Test]
        public void AwaitingActivation_Returns_View_When_Not_Logged_In()
        {
            var result = controller.AwaitingActivation();
            Assert.IsInstanceOf(typeof(ViewResult), result);
        }

        [Test]
        public void AwaitingActivation_Returns_Redirect_When_User_Is_Logged_In()
        {
            //TestHelpers.MockHttpContext(controller, mocks, isAuthenticated: true);
            userProfile = EntityHelpers.GetValidUserProfile();
            userProfile.Email = "goodEmail";
            userProfileRepository.Add(userProfile);
            var result = controller.AwaitingActivation();
            Assert.IsInstanceOf(typeof(RedirectToRouteResult), result);
        }

        [Test]
        public void SendAuthorizationNote_Returns_Redirect()
        {
            var model = new AuthorizeModel
                            {
                                ActivationHash = "goodHash",
                                Email = "goodEmail",
                                FirstName = "Jonny",
                                LastName = "Appleseed",
                                SenderEmail = "senderEmail",
                                SenderName = "senderName",
                                Url = "goodUrl"
                            };

            var result = controller.SendAuthorizationNote(model);
            Assert.IsInstanceOf(typeof(RedirectToRouteResult), result);
        }

        [Test]
        public void Activate_Returns_Redirect_If_Hash_Is_Null()
        {
            var result = controller.Activate(null);
            Assert.IsInstanceOf(typeof(RedirectToRouteResult), result);
        }

        [Test]
        public void Activate_Returns_Redirect_If_Hash_Not_Found()
        {
            var result = controller.Activate("badHash");
            Assert.IsInstanceOf(typeof(RedirectToRouteResult), result);

            var message = controller.TempData["UserFeedback"];
            Assert.AreEqual("Are you sure you have an account here?", message);
        }

        [Test]
        public void Activate_Returns_Redirect_If_Hash_Is_Valid()
        {
            userProfile = EntityHelpers.GetValidUserProfile();
            userProfile.ActivationHash = "goodHash";
            userProfile.LastActivationAttempt = DateTime.Now.AddMinutes(-5);
            userProfileRepository.Add(userProfile);
            var result = controller.Activate("goodHash");
            Assert.IsInstanceOf(typeof(RedirectToRouteResult), result);

            var message = controller.TempData["UserFeedback"];
            Assert.AreEqual("Sweet! Your account is activated. Please log in.", message);
        }

        [Test]
        public void Activate_Returns_Redirect_If_Hash_Is_Not_Valid()
        {
            userProfile = EntityHelpers.GetValidUserProfile();
            userProfile.ActivationHash = "goodHash";
            userProfile.LastActivationAttempt = DateTime.Now.AddHours(-2);
            userProfileRepository.Add(userProfile);
            var result = controller.Activate("goodHash");
            Assert.IsInstanceOf(typeof(RedirectToRouteResult), result);

            var message = controller.TempData["UserFeedback"];
            Assert.AreEqual("Looks like your activation request may have expired. Complete the form below to try again.", message);
        }
        
        private void SetUpController()
        {
            var fakeOrganizationRepository = new FakeOrganizationRepository();
            userProfileRepository = new FakeUserProfileRepository();
            campaignDonorRepository = new FakeCampaignDonorRepository();

            //var mocks = new MockRepository();
            var fakeEmailService = mocks.DynamicMock<IAccountMailer>();
            MailerBase.IsTestModeEnabled = true;
            controller = new AccountController(userProfileRepository, fakeEmailService, campaignDonorRepository)
                                               {
                                                   FormsService = new MockFormsAuthenticationService(),
                                                   MembershipService = new MockMembershipService(),
                                                   OrganizationRepository = fakeOrganizationRepository
                                               };

            
            TestHelpers.MockBasicRequest(controller, mocks);

            //TestHelpers.MockHttpContext(c, mocks, postFiles: false);
            //return c;
        }
    }
}
