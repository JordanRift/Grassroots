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
            Mapper.CreateMap<Campaign, CampaignDetailsModel>();
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
                        var viewModel = SendNotifications(campaign, donation, model);
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

            //var campaign = donation.Campaign;
            //var viewModel = Mapper.Map<CampaignDonor, DonationDetailsModel>(donation);
            //viewModel.Title = campaign.Title;
            //viewModel.UrlSlug = campaign.UrlSlug;
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

        private IPaymentProvider GetPaymentProvider()
        {
            var organization = OrganizationRepository.GetDefaultOrganization(readOnly: true);
            paymentProviderFactory.ApiUrl = organization.PaymentGatewayApiUrl;
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
            donateMailer.UserDonation(mailerModel).SendAsync();

            // Send notification to campaign owner
            mailerModel.Email = campaign.UserProfile.Email;
            donateMailer.CampaignDonation(mailerModel).SendAsync();
            return mailerModel;
        }
    }
}
