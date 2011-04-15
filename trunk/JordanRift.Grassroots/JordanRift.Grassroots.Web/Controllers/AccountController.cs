//
// Copyright © 2011 Jordan Rift, LLC - All Rights Reserved
//
// THIS WORK IS LICENSED UNDER A CREATIVE COMMONS ATTRIBUTION-NONCOMMERCIAL-
// SHAREALIKE 3.0 UNPORTED LICENSE:
// http://creativecommons.org/licenses/by-nc-sa/3.0/
//

using System.Collections.Generic;
using System.Transactions;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using AutoMapper;
using JordanRift.Grassroots.Framework.Data;
using JordanRift.Grassroots.Framework.Entities.Models;
using JordanRift.Grassroots.Framework.Helpers;
using JordanRift.Grassroots.Framework.Services;
using JordanRift.Grassroots.Web.Models;

namespace JordanRift.Grassroots.Web.Controllers
{
    [HandleError]
    public class AccountController : GrassrootsControllerBase
    {
        private readonly IUserRepository userRepository;
        private readonly IEmailService emailService;

        public IFormsAuthenticationService FormsService { get; set; }
        public IMembershipService MembershipService { get; set; }

        public AccountController(IUserRepository userRepository, IEmailService emailService)
        {
            this.userRepository = userRepository;
            this.emailService = emailService;
            Mapper.CreateMap<RegisterModel, UserProfile>();
            Mapper.CreateMap<FacebookRegisterModel, UserProfile>();
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
                using (new UnitOfWorkScope())
                using (var transactionScope = new TransactionScope())
                {
                    var userProfile = Mapper.Map<RegisterModel, UserProfile>(model);
                    var organization = OrganizationRepository.GetDefaultOrganization();

                    if (organization.UserProfiles == null)
                    {
                        organization.UserProfiles = new List<UserProfile>();
                    }

                    organization.UserProfiles.Add(userProfile);
                    OrganizationRepository.Save();
                    var status = MembershipService.CreateUser(model.Email, model.Password, model.Email);
                    transactionScope.Complete();

                    if (status == MembershipCreateStatus.Success)
                    {
                        // TODO: Send email notification/request for account authorization to user
                        // Note: For email notifications, rather than buildilng strings, consider MvcMailer
                        // http://www.hanselman.com/blog/NuGetPackageOfTheWeek2MvcMailerSendsMailsWithASPNETMVCRazorViewsAndScaffolding.aspx
                        FormsService.SignIn(model.Email, false);
                        return RedirectToAction("Index", "UserProfile", new { id = userProfile.UserProfileID });
                    }
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
                if (MembershipService.ChangePassword(User.Identity.Name, model.OldPassword, model.NewPassword))
                {
                    // TODO: Send email notifiying user their password has changed.
                    // Note: For email notifications, rather than buildilng strings, consider MvcMailer
                    // http://www.hanselman.com/blog/NuGetPackageOfTheWeek2MvcMailerSendsMailsWithASPNETMVCRazorViewsAndScaffolding.aspx
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
                    // TODO: Implement this email functionality via org settings so they're configurable
                    // Note: For email notifications, rather than buildilng strings, consider MvcMailer
                    // http://www.hanselman.com/blog/NuGetPackageOfTheWeek2MvcMailerSendsMailsWithASPNETMVCRazorViewsAndScaffolding.aspx
                    emailService.SendTo(model.Email, "Password Reset", string.Format("This is your new password: {0}.", newPassword));
                    return RedirectToAction("ResetPasswordSuccess");
                }
            }

            TempData["ForgotPasswordModel"] = model;
            return RedirectToAction("ForgotPassword");
        }
    }
}
