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
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using AutoMapper;
using JordanRift.Grassroots.Framework.Data;
using JordanRift.Grassroots.Framework.Entities.Models;
using JordanRift.Grassroots.Framework.Helpers;
using JordanRift.Grassroots.Framework.Services;
using JordanRift.Grassroots.Web.Helpers;
using JordanRift.Grassroots.Web.Mailers;
using JordanRift.Grassroots.Web.Models;
using Mvc.Mailer;

namespace JordanRift.Grassroots.Web.Controllers
{
    public class AccountController : GrassrootsControllerBase
    {
        private readonly IAccountMailer accountMailer;
        private readonly IUserProfileRepository userProfileRepository;

        public IFormsAuthenticationService FormsService { get; set; }
        public IMembershipService MembershipService { get; set; }

        public AccountController(IUserProfileRepository userProfileRepository, IAccountMailer accountMailer)
        {
            this.accountMailer = accountMailer;
            this.userProfileRepository = userProfileRepository;
            Mapper.CreateMap<RegisterModel, UserProfile>();
            Mapper.CreateMap<UserProfile, RegisterModel>();
        }

        ~AccountController()
        {
            userProfileRepository.Dispose();
        }

        protected override void Initialize(RequestContext requestContext)
        {
            if (FormsService == null)
            {
                FormsService = new FormsAuthenticationService();
            }

            if (MembershipService == null)
            {
                MembershipService = new AccountMembershipService();
            }


            base.Initialize(requestContext);
        }

        public ActionResult LogOn(string returnUrl = "")
        {
            var viewModel = TempData["LogOnModel"] as LogOnModel ?? new LogOnModel();
            ViewBag.ReturnUrl = returnUrl;
            return View(viewModel);
        }

        [HttpPost]
        public ActionResult AuthenticateUser(LogOnModel model, string returnUrl = "")
        {
            if (ModelState.IsValid)
            {
                using (userProfileRepository)
                {
                    var userProfile = userProfileRepository.FindUserProfileByEmail(model.Email).FirstOrDefault();

                    if (userProfile == null)
                    {
                        return RedirectToAction("Register", "Account");
                    }

                    if (!userProfile.IsActivated)
                    {
                        return RedirectToAction("AwaitingActivation", "Account");
                    }
                }

                if (MembershipService.ValidateUser(model.Email, model.Password))
                {
                    FormsService.SignIn(model.Email, model.RememberMe);

                    if (!string.IsNullOrEmpty(returnUrl))
                    {
                        return Redirect(returnUrl);
                    }

                    return RedirectToAction("Index", "UserProfile");
                }

                ModelState.AddModelError("", "The username or password you provided are incorrect.");
            }

            TempData["LogOnModel"] = model;
            return RedirectToAction("LogOn");
        }

        public ActionResult LogOff()
        {
            FormsService.SignOut();
            return RedirectToAction("Index", "Home");
        }

        public ActionResult Register()
        {
            var viewModel = TempData["RegisterModel"] as RegisterModel ?? new RegisterModel();
            ViewBag.PasswordLength = MembershipService.MinPasswordLength;
            return View(viewModel);
        }

        [HttpPost]
        public ActionResult RegisterUser(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                MembershipCreateStatus status;
                UserProfile userProfile;
                Organization organization;

                using (new UnitOfWorkScope())
                using (var transactionScope = new TransactionScope(TransactionScopeOption.RequiresNew))
                {
                    // This should ensure best compatiblity through a variety of SQL database environments 
                    // (e.g. - SQL Server, MySQL, SQL Azure).
                    userProfile = Mapper.Map<RegisterModel, UserProfile>(model);
                    organization = OrganizationRepository.GetDefaultOrganization(readOnly: false);

                    if (organization.UserProfiles == null)
                    {
                        organization.UserProfiles = new List<UserProfile>();
                    }

                    userProfile.Active = true;
                    userProfile.IsActivated = false;
                    var service = new GrassrootsMembershipService();
                    userProfile.ActivationHash = service.GetUserAuthorizationHash();
                    userProfile.LastActivationAttempt = DateTime.Now;
                    organization.UserProfiles.Add(userProfile);
                    OrganizationRepository.Save();
                    status = MembershipService.CreateUser(model.Email, model.Password, model.Email);
                    transactionScope.Complete();
                }

                if (status == MembershipCreateStatus.Success)
                {
                    accountMailer.Authorize(MapAuthorizeModel(userProfile, organization)).SendAsync();

                    return RedirectToAction("AwaitingActivation", "Account");
                }
            }

            TempData["RegisterModel"] = model;
            return RedirectToAction("Register", "Account");
        }

        [Authorize]
        public ActionResult ChangePassword()
        {
            var viewModel = TempData["ChangePasswordModel"] as ChangePasswordModel ?? new ChangePasswordModel();
            ViewBag.PasswordLength = MembershipService.MinPasswordLength;
            return View(viewModel);
        }
        
        [Authorize]
        [HttpPost]
        public ActionResult UpdatePassword(ChangePasswordModel model)
        {
            if (ModelState.IsValid)
            {
                var email = User.Identity.Name;

                if (MembershipService.ChangePassword(email, model.OldPassword, model.NewPassword))
                {
                    using (userProfileRepository)
                    {
                        var userProfile = userProfileRepository.FindUserProfileByEmail(email).FirstOrDefault();
                        var mailModel = Mapper.Map<UserProfile, RegisterModel>(userProfile);
                        accountMailer.PasswordChange(mailModel).SendAsync();
                        return RedirectToAction("ChangePasswordSuccess");
                    }
                }

                ModelState.AddModelError("", "The current password is incorrect or the new password is invalid.");
            }

            TempData["ChangePasswordModel"] = model;
            return RedirectToAction("ChangePassword");
        }

        public ActionResult ChangePasswordSuccess()
        {
            return View();
        }

        public ActionResult ForgotPassword()
        {
            var viewModel = TempData["ForgotPasswordModel"] as ForgotPasswordModel ?? new ForgotPasswordModel();
            return View(viewModel);
        }

        [HttpPost]
        public ActionResult SendPasswordReset(ForgotPasswordModel model)
        {
            if (ModelState.IsValid)
            {
                using (userProfileRepository)
                {
                    var userProfile = userProfileRepository.FindUserProfileByEmail(model.Email).FirstOrDefault();

                    if (userProfile != null)
                    {
                        var service = new GrassrootsMembershipService();
                        userProfile.ActivationHash = service.GetUserAuthorizationHash();
                        userProfile.ActivationPin = service.GenerateRandomPin();
                        userProfile.LastActivationAttempt = DateTime.Now;
                        userProfileRepository.Save();
                        accountMailer.PasswordReset(MapPasswordReset(userProfile)).SendAsync();

                        return RedirectToAction("UpdatePassword");
                    }
                }

                TempData["UserFeedback"] = "The email you are looking for could not be found in our system.";
            }

            TempData["ForgotPasswordModel"] = model;
            return RedirectToAction("ForgotPassword");
        }

        public ActionResult UpdatePassword(string hash)
        {
            if (User != null && !string.IsNullOrEmpty(User.Identity.Name))
            {
                return RedirectToAction("Index", "UserProfile");
            }

            var model = new UpdatePasswordModel { ActivationHash = hash };
            return View(model);
        }

        [HttpPost]
        public ActionResult UpdateForgottenPassword(string hash, UpdatePasswordModel model)
        {
            if (ModelState.IsValid)
            {
                using (new UnitOfWorkScope())
                {
                    var userProfile = userProfileRepository.GetUserProfileByActivationHash(hash);
                    var service = new GrassrootsMembershipService();

                    if (userProfile == null)
                    {
                        TempData["UserFeedback"] = "The email you are looking for could not be found in our system.";
                        return RedirectToAction("ForgotPassword");
                    }

                    if (service.UpdatePassword(userProfile, model.ActivationPin, model.Password))
                    {
                        var mailModel = Mapper.Map<UserProfile, RegisterModel>(userProfile);
                        accountMailer.PasswordChange(mailModel).SendAsync();
                        TempData["UserFeedback"] = "Sweet! Your password has been changed. You can now log in with your new password.";
                        return RedirectToAction("LogOn");
                    }
                }
            }

            var activationHash = hash;
            return RedirectToAction("UpdatePassword", new { hash = activationHash });
        }

        public ActionResult AwaitingActivation()
        {
            if (User != null)
            {
                using (userProfileRepository)
                {
                    var userProfile = userProfileRepository.FindUserProfileByEmail(User.Identity.Name).FirstOrDefault();

                    if (userProfile != null && userProfile.IsActivated)
                    {
                        return RedirectToAction("Index", "UserProfile");
                    }
                }
            }

            return View();
        }

        [HttpPost]
        public ActionResult SendAuthorizationNote(AuthorizeModel model)
        {
            using (userProfileRepository)
            {
                var userProfile = userProfileRepository.FindUserProfileByEmail(model.Email).FirstOrDefault();
                var organization = OrganizationRepository.GetDefaultOrganization(readOnly: true);

                if (userProfile == null)
                {
                    TempData["UserFeedback"] = "We couldn't find that email address in our system. Are you sure that was the right one?";
                    return RedirectToAction("AwaitingActivation", "Account");
                }

                var service = new GrassrootsMembershipService();
                userProfile.ActivationHash = service.GetUserAuthorizationHash();
                userProfile.LastActivationAttempt = DateTime.Now;
                userProfileRepository.Save();
                accountMailer.Authorize(MapAuthorizeModel(userProfile, organization)).SendAsync();
            }

            TempData["UserFeedback"] = "We just sent you an email. Check your email account and follow the instructions inside.";
            return RedirectToAction("AwaitingActivation", "Account");
        }

        /// <summary>
        /// Check if activation hash is valid. 
        /// * If yes, set user profile IsActivated to true and redirect to login
        /// * If no, redirect to AwaitingActivation
        /// </summary>
        /// <param name="hash">ashed string to compare</param>
        /// <returns>Redirect based on result</returns>
        public RedirectToRouteResult Activate(string hash)
        {
            if (hash == null)
            {
                return RedirectToAction("AwaitingActivation", "Account");
            }

            using (userProfileRepository)
            {
                var userProfile = userProfileRepository.GetUserProfileByActivationHash(hash);
                var organization = OrganizationRepository.GetDefaultOrganization(readOnly: true);
                var service = new GrassrootsMembershipService();

                if (userProfile == null)
                {
                    TempData["UserFeedback"] = "Are you sure you have an account here?";
                    return RedirectToAction("Register");
                }

                if (service.IsActivationHashValid(userProfile))
                {
                    userProfile.IsActivated = true;
                    userProfileRepository.Save();
                    TempData["UserFeedback"] = "Sweet! Your account is activated. Please log in.";
                    accountMailer.Welcome(MapWelcomeModel(userProfile, organization)).SendAsync();
                    return RedirectToAction("LogOn", "Account");
                }
            }

            TempData["UserFeedback"] = "Looks like your activation request may have expired. Complete the form below to try again.";
            return RedirectToAction("AwaitingActivation", "Account");
        }

        private PasswordResetModel MapPasswordReset(UserProfile userProfile)
        {
            return new PasswordResetModel
                       {
                           FirstName = userProfile.FirstName,
                           Email = userProfile.Email,
                           ActivationPin = userProfile.ActivationPin,
                           Url = Url.ToPublicUrl(Url.Action("UpdatePassword", "Account", new { hash = userProfile.ActivationHash }))
                       };
        }

        private static WelcomeModel MapWelcomeModel(UserProfile userProfile, Organization organization)
        {
            return new WelcomeModel
                       {
                           Email = userProfile.Email,
                           FirstName = userProfile.FirstName,
                           ContactEmail = organization.ContactEmail,
                           OrganizationName = organization.Name
                       };
        }

        private AuthorizeModel MapAuthorizeModel(UserProfile userProfile, Organization organization)
        {
            return new AuthorizeModel
                       {
                           Email = userProfile.Email,
                           FirstName = userProfile.FirstName,
                           LastName = userProfile.LastName,
                           SenderEmail = organization.ContactEmail,
                           SenderName = organization.Name,
                           Url = Url.ToPublicUrl(Url.Action("Activate", "Account", new { hash = userProfile.ActivationHash }))
                       };
        }
    }
}
