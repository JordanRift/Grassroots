//
// Copyright © 2011 Jordan Rift, LLC - All Rights Reserved
//
// THIS WORK IS LICENSED UNDER A CREATIVE COMMONS ATTRIBUTION-NONCOMMERCIAL-
// SHAREALIKE 3.0 UNPORTED LICENSE:
// http://creativecommons.org/licenses/by-nc-sa/3.0/
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
        private readonly ICampaignRepository campaignRepository;
        private readonly IUserProfileRepository userProfileRepository;
        private readonly IDonateMailer donateMailer;
        private readonly IPaymentProviderFactory paymentProviderFactory;

        public DonateController(ICampaignRepository campaignRepository, IUserProfileRepository userProfileRepository, 
            IDonateMailer donateMailer, IPaymentProviderFactory paymentProviderFactory)
        {
            this.campaignRepository = campaignRepository;
            this.userProfileRepository = userProfileRepository;
            this.donateMailer = donateMailer;
            this.paymentProviderFactory = paymentProviderFactory;
            Mapper.CreateMap<UserProfile, Payment>();
            Mapper.CreateMap<Payment, CampaignDonor>();
            Mapper.CreateMap<CampaignDonor, DonationDetailsModel>();
        }

        [OutputCache(Duration = 30, VaryByParam = "urlSlug")]
        public ActionResult Index(string slug = "")
        {
            using (new UnitOfWorkScope())
            {
                var campaign = campaignRepository.GetCampaignByUrlSlug(slug);
                UserProfile userProfile = null;

                if (User != null)
                {
                    userProfile = userProfileRepository.FindUserProfileByEmail(User.Identity.Name).FirstOrDefault();
                }

                var payment = TempData["Payment"] != null ? TempData["Payment"] as Payment : GetPayment(userProfile);

                if (campaign != null)
                {
                    // TODO: Consider creating an action filter for this
                    if (!campaign.IsActive)
                    {
                        TempData["UserFeedback"] = "Sorry, this campaign is inactive and can no longer receive donations.";
                        var urlSlug = slug;
                        return RedirectToAction("Index", "Campaign", new { slug = urlSlug });
                    }

                    ViewBag.Campaign = campaign;
                }

                return View(payment);
            }
        }

        [HttpPost]
        public ActionResult ProcessDonation(Payment model, string slug = "")
        {
            var urlSlug = slug;

            if (ModelState.IsValid)
            {
                using (new UnitOfWorkScope())
                {
                    var campaign = campaignRepository.GetCampaignByUrlSlug(slug);

                    if (campaign == null)
                    {
                        campaign = campaignRepository.GetDefaultCampaign();
                    }
                    else if (!campaign.IsActive)
                    {
                        // TODO: Consider creating an action filter attribute for this check
                        TempData["UserFeedback"] = "Sorry, this campaign is inactive and can no longer receive donations.";
                        return RedirectToAction("Index", "Campaign", new { slug = urlSlug });
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
                        SendNotifications(model, campaign, donation);
                        TempData["Donation"] = donation;
                        return RedirectToAction("ThankYou");
                    }

                    Logger.LogError(model, result);
                    TempData["ErrorMessage"] = result.ReasonText;
                }
            }

            TempData["Payment"] = model;
            return RedirectToAction("Index", new { slug = urlSlug });
        }

        public ActionResult ThankYou()
        {
            var donation = TempData["Donation"] as CampaignDonor;

            if (donation == null)
            {
                return HttpNotFound("The donation you are looking for could not be found.");
            }

            var viewModel = Mapper.Map<CampaignDonor, DonationDetailsModel>(donation);
            return View(viewModel);
        }

        private static Payment GetPayment(UserProfile userProfile)
        {
            return userProfile != null ? Mapper.Map<UserProfile, Payment>(userProfile) : new Payment();
        }

        private CampaignDonor GetDonation(Payment payment)
        {
            UserProfile userProfile = null;
            var donation = Mapper.Map<Payment, CampaignDonor>(payment);
            
            if (User != null)
            {
                userProfile = userProfileRepository.FindUserProfileByEmail(User.Identity.Name).FirstOrDefault();
            }

            if (userProfile != null)
            {
                if (userProfile.CampaignDonors == null)
                {
                    userProfile.CampaignDonors = new List<CampaignDonor>();
                }

                userProfile.CampaignDonors.Add(donation);
            }

            return donation;
        }

        private IPaymentProvider GetPaymentProvider()
        {
            paymentProviderFactory.ApiUrl = Organization.PaymentGatewayApiUrl;
            paymentProviderFactory.ApiKey = Organization.PaymentGatewayApiKey;
            paymentProviderFactory.ApiSecret = Organization.PaymentGatewayApiSecret;
            return paymentProviderFactory.GetPaymentProvider(Organization.PaymentGateway);
        }

        private void SendNotifications(Payment model, Campaign campaign, CampaignDonor donation)
        {
            // Send receipt of payment to user
            donateMailer.UserDonation(model).SendAsync();

            // Send notification to campaign owner
            var mailerModel = Mapper.Map<CampaignDonor, DonationDetailsModel>(donation);
            mailerModel.Title = campaign.Title;
            mailerModel.Email = campaign.UserProfile.Email;
            donateMailer.CampaignDonation(mailerModel).SendAsync();
        }
    }
}
