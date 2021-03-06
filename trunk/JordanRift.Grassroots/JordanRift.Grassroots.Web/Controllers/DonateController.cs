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

using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using AutoMapper;
using JordanRift.Grassroots.Framework.Data;
using JordanRift.Grassroots.Framework.Entities;
using JordanRift.Grassroots.Framework.Entities.Models;
using JordanRift.Grassroots.Framework.Helpers;
using JordanRift.Grassroots.Web.Mailers;
using JordanRift.Grassroots.Web.Models;
using Mvc.Mailer;

namespace JordanRift.Grassroots.Web.Controllers
{
    public class DonateController : GrassrootsControllerBase
    {
        private const string ADMIN_ROLES = "Root,Administrator";
        private readonly ICampaignRepository campaignRepository;
        private readonly IUserProfileRepository userProfileRepository;
        private readonly IDonateMailer donateMailer;
        private readonly IPaymentProviderFactory paymentProviderFactory;
        private readonly ICampaignDonorRepository campaignDonorRepository;

        public DonateController(ICampaignRepository campaignRepository, IUserProfileRepository userProfileRepository, 
            IDonateMailer donateMailer, IPaymentProviderFactory paymentProviderFactory, ICampaignDonorRepository campaignDonorRepository)
        {
            this.campaignRepository = campaignRepository;
            this.userProfileRepository = userProfileRepository;
            this.donateMailer = donateMailer;
            this.paymentProviderFactory = paymentProviderFactory;
            this.campaignDonorRepository = campaignDonorRepository;
            Mapper.CreateMap<UserProfile, Payment>();
            Mapper.CreateMap<Payment, CampaignDonor>();
            Mapper.CreateMap<CampaignDonor, DonationDetailsModel>();
            Mapper.CreateMap<Campaign, CampaignDetailsModel>();
            Mapper.CreateMap<CampaignDonor, DonationDetailsModel>();
        }

        ~DonateController()
        {
            campaignRepository.Dispose();
            userProfileRepository.Dispose();
        }

        public ActionResult Index(string slug = "")
        {
            using (new UnitOfWorkScope())
            {
                var campaign = campaignRepository.GetCampaignByUrlSlug(slug);
                var organizatin = OrganizationRepository.GetDefaultOrganization(readOnly: true);
                UserProfile userProfile = null;

                if (User != null)
                {
                    userProfile = userProfileRepository.FindUserProfileByEmail(User.Identity.Name).FirstOrDefault();
                }

                var payment = TempData["Payment"] != null ? TempData["Payment"] as Payment : GetPayment(userProfile);

                if (campaign != null)
                {
                    if (!campaign.IsActive)
                    {
                        TempData["UserFeedback"] = "Sorry, this campaign is inactive and can no longer receive donations.";
                        var urlSlug = slug;
                        return RedirectToAction("Index", "Campaign", new { slug = urlSlug });
                    }

                    var campaignModel = Mapper.Map<Campaign, CampaignDetailsModel>(campaign);
                    ViewBag.Campaign = campaignModel;
                }

                if (TempData["PaymentGatewayError"] != null)
                {
                    ModelState.AddModelError("PaymentGatewayError", TempData["PaymentGatewayError"].ToString());
                }

                ViewBag.CanDoRecurringBilling = !string.IsNullOrEmpty(organizatin.PaymentGatewayArbApiUrl);
                return View(payment);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken(Salt = "Donation")]
        public ActionResult ProcessDonation(Payment model, string slug = "")
        {
            var urlSlug = slug;

            if (ModelState.IsValid)
            {
                using (new UnitOfWorkScope())
                {
                    var campaign = campaignRepository.GetCampaignByUrlSlug(slug);
                    var organization = OrganizationRepository.GetDefaultOrganization(readOnly: true);
                    model.Organization = organization;

                    if (campaign == null)
                    {
                        campaign = campaignRepository.GetDefaultCampaign();
                        model.Campaign = campaign;
                        model.Notes = string.Format("Donation to {0} should be applied to general fund.",
                                                    organization.Name);
                    }
                    else if (!campaign.IsActive)
                    {
                        TempData["UserFeedback"] = "Sorry, this campaign is inactive and can no longer receive donations.";
                        return RedirectToAction("Index", "Campaign", new { slug = urlSlug });
                    }
                    else  // We know it's OK to process campaign donation. Add campaign info to Payment to pass through to payment gateway for reconciliation.
                    {
                        var userProfile = campaign.UserProfile;
                        model.Owner = userProfile;
                        model.Campaign = campaign;
                        model.Notes = string.Format("Donation to {0} should be applied to {1} campaign owned by {2}",
                                                    organization.Name, campaign.Title, userProfile.FullName);
                    }

                    var provider = GetPaymentProvider();
                    var result = provider.Process(model);

                    if (result.ResponseCode == PaymentResponseCode.Approved)
                    {
                        var donation = GetDonation(model);

                        if (campaign.CampaignDonors == null)
                        {
                            campaign.CampaignDonors = new List<CampaignDonor>();
                        }

                        donation.Approved = true;
                        campaign.CampaignDonors.Add(donation);
                        campaignRepository.Save();
                        var viewModel = SendNotifications(campaign, donation, model);
                        viewModel.IsGeneralFund = campaign.IsGeneralFund;
                        TempData["Donation"] = viewModel;
                        return RedirectToAction("ThankYou");
                    }

                    Logger.LogError(model, result);
                    TempData["PaymentGatewayError"] = result.ReasonText;
                }
            }

            TempData["Payment"] = model;
            return RedirectToAction("Index", new { slug = urlSlug });
        }

        public ActionResult ThankYou()
        {
            var viewModel = TempData["Donation"] as DonationDetailsModel;

            if (viewModel == null)
            {
                return HttpNotFound("The donation you are looking for could not be found.");
            }

            return View(viewModel);
        }

        private static Payment GetPayment(UserProfile userProfile)
        {
            Payment payment;
            
            if (userProfile != null)
            {
                payment = Mapper.Map<UserProfile, Payment>(userProfile);
                payment.NameOnAccount = userProfile.FullName;
            }
            else
            {
                payment = new Payment();
            }

            return payment;
        }

        private CampaignDonor GetDonation(Payment payment)
        {
            UserProfile userProfile;
            var donation = Mapper.Map<Payment, CampaignDonor>(payment);
            PopulateDisplayName(donation);
            
            userProfile = User.Identity.IsAuthenticated 
                ? userProfileRepository.FindUserProfileByEmail(User.Identity.Name).FirstOrDefault() 
                : userProfileRepository.FindUserProfileByEmail(payment.Email).FirstOrDefault();

            if (userProfile != null)
            {
                if (userProfile.CampaignDonors == null)
                {
                    userProfile.CampaignDonors = new List<CampaignDonor>();
                }

                UpdateUserProfile(userProfile, donation);
                userProfile.CampaignDonors.Add(donation);
            }

            return donation;
        }

        private static void UpdateUserProfile(UserProfile userProfile, CampaignDonor donation)
        {
            userProfile.AddressLine1 = donation.AddressLine1;
            userProfile.AddressLine2 = donation.AddressLine2;
            userProfile.City = donation.City;
            userProfile.State = donation.State;
            userProfile.PrimaryPhone = donation.PrimaryPhone;
            userProfile.ZipCode = donation.ZipCode;
        }

        private static void PopulateDisplayName(CampaignDonor donation)
        {
            if (donation.IsAnonymous && string.IsNullOrEmpty(donation.DisplayName))
            {
                donation.DisplayName = "Anonymous";
            }
            else if (!donation.IsAnonymous && string.IsNullOrEmpty(donation.DisplayName))
            {
                donation.DisplayName = string.Format("{0} {1}", donation.FirstName, donation.LastName);
            }
        }

        private IPaymentProvider GetPaymentProvider()
        {
            var organization = OrganizationRepository.GetDefaultOrganization(readOnly: true);
            paymentProviderFactory.ApiUrl = organization.PaymentGatewayApiUrl;
            paymentProviderFactory.ArbApiUrl = organization.PaymentGatewayArbApiUrl;
            paymentProviderFactory.ApiKey = organization.PaymentGatewayApiKey;
            paymentProviderFactory.ApiSecret = organization.PaymentGatewayApiSecret;
            return paymentProviderFactory.GetPaymentProvider(organization.PaymentGateway);
        }

        private DonationDetailsModel SendNotifications(Campaign campaign, CampaignDonor donation, Payment payment)
        {
            // Send receipt of payment to user
            var mailerModel = Mapper.Map<CampaignDonor, DonationDetailsModel>(donation);
            mailerModel.Title = campaign.Title;
            mailerModel.UrlSlug = campaign.UrlSlug;
            mailerModel.PaymentType = payment.PaymentType == PaymentType.CC ? "Credit/Debit Card" : "Electronic Check";
            mailerModel.TransactionType = payment.TransactionType;

            var organization = campaign.Organization;
            var bcc = organization.GetSetting(OrgSettingKeys.DONATION_NOTIFICATION_ADDRESS);

            if (bcc != null)
            {
                mailerModel.DonorNotificationEmail = bcc.Value;
            }

            donateMailer.UserDonation(mailerModel).SendAsync();

            // Send notification to campaign owner
            mailerModel.Email = campaign.UserProfile.Email;
            donateMailer.CampaignDonation(mailerModel).SendAsync();
            return mailerModel;
        }

#region Admin

        [Authorize(Roles = ADMIN_ROLES)]
        public ActionResult List()
        {
            var donations = campaignDonorRepository.FindAllDonations().ToList();
            var models = new List<DonationAdminModel>();

            foreach (var donation in donations)
            {
                var model = Mapper.Map<CampaignDonor, DonationAdminModel>(donation);
                models.Add(model);
            }

            return View(models);
        }

        [Authorize(Roles = ADMIN_ROLES)]
        public ActionResult New(int id = -1)
        {
            if (TempData["ModelErrors"] != null)
            {
                var errors = TempData["ModelErrors"] as IEnumerable<string> ?? new List<string>();

                foreach (var error in errors)
                {
                    ModelState.AddModelError("", error);
                }
            }

            var model = TempData["DonationAdminModel"] as DonationAdminModel ?? new DonationAdminModel();
            model.CampaignID = id;
            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = ADMIN_ROLES)]
        [ValidateAntiForgeryToken(Salt = "AdminCreateDonation")]
        public ActionResult Create(DonationAdminModel model)
        {
            if (!ModelState.IsValid)
            {
                TempData["ModelErrors"] = FindModelErrors();
                TempData["DonationAdminModel"] = model;
                return RedirectToAction("New");
            }

            CampaignDonor campaignDonor;

            using (new UnitOfWorkScope())
            {
                var displayName = model.IsAnonymous && string.IsNullOrEmpty(model.DisplayName)
                                      ? "Anonymous"
                                      : model.DisplayName;
                campaignDonor = new CampaignDonor
                                    {
                                        Amount = model.Amount,
                                        FirstName = model.FirstName,
                                        LastName = model.LastName,
                                        AddressLine1 = model.AddressLine1,
                                        AddressLine2 = model.AddressLine2,
                                        City = model.City,
                                        State = model.State,
                                        ZipCode = model.ZipCode,
                                        Email = model.Email,
                                        PrimaryPhone = model.PrimaryPhone,
                                        Approved = model.Approved,
                                        IsAnonymous = model.IsAnonymous,
                                        DisplayName = displayName
                                    };

                var campaign = campaignRepository.GetCampaignByID(model.CampaignID);
                campaign.CampaignDonors.Add(campaignDonor);
                var userProfile = userProfileRepository.FindUserProfileByEmail(model.Email).FirstOrDefault();

                if (userProfile != null)
                {
                    userProfile.CampaignDonors.Add(campaignDonor);
                }

                campaignDonorRepository.Save();
            }

            TempData["UserFeedback"] = string.Format("{0} {1}'s donation of {2} has been created successfully.",
                campaignDonor.FirstName, campaignDonor.LastName, campaignDonor.Amount);
            return RedirectToAction("Admin", new { controller = "Campaign", id = campaignDonor.CampaignID });
        }

        [Authorize(Roles = ADMIN_ROLES)]
        public ActionResult Admin(int id = -1, string context = "list")
        {
            DonationAdminModel model;

            if (TempData["ModelErrors"] != null)
            {
                var errors = TempData["ModelErrors"] as IEnumerable<string> ?? new List<string>();

                foreach (var error in errors)
                {
                    ModelState.AddModelError("", error);
                }

                model = TempData["DonationAdminModel"] as DonationAdminModel;
            }
            else
            {
                using (campaignDonorRepository)
                {
                    var campaignDonor = campaignDonorRepository.GetDonationByID(id);

                    if (campaignDonor == null)
                    {
                        return HttpNotFound("The donation you are looking for could not be found.");
                    }

                    model = MapAdminModel(campaignDonor);
                }
            }

            ViewBag.Context = context;
            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = ADMIN_ROLES)]
        [ValidateAntiForgeryToken(Salt = "AdminUpdateDonation")]
        public ActionResult AdminUpdate(DonationAdminModel model)
        {
            if (!ModelState.IsValid)
            {
                TempData["ModelErrors"] = FindModelErrors();
                TempData["DonationAdminModel"] = model;
                return RedirectToAction("Admin");
            }

            using (new UnitOfWorkScope())
            {
                var donation = campaignDonorRepository.GetDonationByID(model.CampaignDonorID);

                if (donation == null)
                {
                    return HttpNotFound("The donation you are looking for could not be found.");
                }

                MapCampaignDonor(model, donation);
                campaignDonorRepository.Save();
                TempData["UserFeedback"] = string.Format("{0} {1}'s donation of {2} has been saved.",
                    donation.FirstName, donation.LastName, donation.Amount);
            }

            return RedirectToAction("Admin", "Campaign", new { id = model.CampaignID });
        }

        [HttpDelete]
        [Authorize(Roles = ADMIN_ROLES)]
        public ActionResult Destroy(int id = -1)
        {
            using (campaignDonorRepository)
            {
                var campaignDonor = campaignDonorRepository.GetDonationByID(id);

                if (campaignDonor == null)
                {
                    return HttpNotFound("The donation you are looking for could not be found.");
                }

                campaignDonorRepository.Delete(campaignDonor);
                campaignDonorRepository.Save();
            }

            if (Request.IsAjaxRequest())
            {
                return Json(new { success = true });
            }

            // TODO: Consider adding message to inform user of successful delete.
            return RedirectToAction("List");
        }

        [Authorize(Roles = ADMIN_ROLES)]
        public ActionResult ResendNotification(int id = -1)
        {
            var donation = campaignDonorRepository.GetDonationByID(id);

            if (donation == null)
            {
                return HttpNotFound("The donation you are looking for could not be found.");
            }

            ResendNotification(donation);

            if (Request.IsAjaxRequest())
            {
                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }

            TempData["UserFeedback"] = "Confirmation was successfully re-sent";
            return RedirectToAction("List");
        }

        private void ResendNotification(CampaignDonor donation)
        {
            var mailerModel = Mapper.Map<CampaignDonor, DonationDetailsModel>(donation);
            var campaign = donation.Campaign;
            mailerModel.Title = campaign.Title;
            mailerModel.UrlSlug = campaign.UrlSlug;
            mailerModel.TransactionType = TransactionType.OneTime;
            donateMailer.UserDonation(mailerModel).SendAsync();
        }

        private DonationAdminModel MapAdminModel(CampaignDonor campaignDonor)
        {
            var model = Mapper.Map<CampaignDonor, DonationAdminModel>(campaignDonor);
            var campaign = campaignDonor.Campaign;
            model.CampaignID = campaign.CampaignID;
            model.CampaignTitle = campaign.Title;
            model.UserProfileID = campaignDonor.UserProfileID;
            return model;
        }

        private void MapCampaignDonor(DonationAdminModel model, CampaignDonor campaignDonor)
        {
            campaignDonor.Amount = model.Amount;
            campaignDonor.DonationDate = model.DonationDate;
            campaignDonor.FirstName = model.FirstName;
            campaignDonor.LastName = model.LastName;
            campaignDonor.Email = model.Email;
            campaignDonor.PrimaryPhone = model.PrimaryPhone;
            campaignDonor.AddressLine1 = model.AddressLine1;
            campaignDonor.AddressLine2 = model.AddressLine2;
            campaignDonor.City = model.City;
            campaignDonor.State = model.State;
            campaignDonor.ZipCode = model.ZipCode;
            campaignDonor.IsAnonymous = model.IsAnonymous;
            var displayName = model.IsAnonymous && string.IsNullOrEmpty(model.DisplayName)
                                  ? "Anonymous"
                                  : model.DisplayName;
            campaignDonor.DisplayName = displayName;
            campaignDonor.Approved = model.Approved;

            if (campaignDonor.CampaignID != model.CampaignID)
            {
                var oldCampaign = campaignDonor.Campaign;
                var newCampaign = campaignRepository.GetCampaignByID(model.CampaignID);

                if (newCampaign == null)
                {
                    return;
                }

                oldCampaign.CampaignDonors.Remove(campaignDonor);
                newCampaign.CampaignDonors.Add(campaignDonor);
            }
        }

#endregion
    }
}
