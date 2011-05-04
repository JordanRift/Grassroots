//
// Copyright © 2011 Jordan Rift, LLC - All Rights Reserved
//
// THIS WORK IS LICENSED UNDER A CREATIVE COMMONS ATTRIBUTION-NONCOMMERCIAL-
// SHAREALIKE 3.0 UNPORTED LICENSE:
// http://creativecommons.org/licenses/by-nc-sa/3.0/
//

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
using JordanRift.Grassroots.Web.Mailers;
using JordanRift.Grassroots.Web.Models;
using Mvc.Mailer;

namespace JordanRift.Grassroots.Web.Controllers
{
    [HandleError]
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
                if (MembershipService.ValidateUser(model.Email, model.Password))
                {
                    FormsService.SignIn(model.Email, model.RememberMe);

                    if (!string.IsNullOrEmpty(returnUrl))
                    {
                        return Redirect(returnUrl);
                    }

                    return RedirectToAction("Index", "Home");
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

                using (new UnitOfWorkScope())
                using (var transactionScope = new TransactionScope())
                {
                    userProfile = Mapper.Map<RegisterModel, UserProfile>(model);

                    if (Organization.UserProfiles == null)
                    {
                        Organization.UserProfiles = new List<UserProfile>();
                    }

                    var gravatarService = new GravatarService();
                    userProfile.ImagePath = gravatarService.GetGravatarPictureUrl(userProfile.Email);
                    Organization.UserProfiles.Add(userProfile);
                    OrganizationRepository.Save();
                    status = MembershipService.CreateUser(model.Email, model.Password, model.Email);
                    transactionScope.Complete();
                }

                if (status == MembershipCreateStatus.Success)
                {
                    accountMailer.Welcome(model).SendAsync();
                    FormsService.SignIn(model.Email, false);
                    return RedirectToAction("Index", "UserProfile", new { id = userProfile.UserProfileID });
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
                    var userProfile = userProfileRepository.FindUserProfileByEmail(email).FirstOrDefault();
                    var mailModel = Mapper.Map<UserProfile, RegisterModel>(userProfile);
                    accountMailer.PasswordChange(mailModel).SendAsync();
                    return RedirectToAction("ChangePasswordSuccess");
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

        public ActionResult ResetPasswordSuccess()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ResetPassword(ForgotPasswordModel model)
        {
            if (ModelState.IsValid)
            {
                var newPassword = MembershipService.ResetPassword(model.Email);

                if (newPassword != null)
                {
                    var userProfile = userProfileRepository.FindUserProfileByEmail(model.Email).FirstOrDefault();
                    var mailerModel = Mapper.Map<UserProfile, RegisterModel>(userProfile);
                    mailerModel.Password = newPassword;
                    accountMailer.PasswordReset(mailerModel).SendAsync();
                    return RedirectToAction("ResetPasswordSuccess");
                }
            }

            TempData["ForgotPasswordModel"] = model;
            return RedirectToAction("ForgotPassword");
        }
    }
}
