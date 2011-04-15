//
// Copyright © 2011 Jordan Rift, LLC - All Rights Reserved
//
// THIS WORK IS LICENSED UNDER A CREATIVE COMMONS ATTRIBUTION-NONCOMMERCIAL-
// SHAREALIKE 3.0 UNPORTED LICENSE:
// http://creativecommons.org/licenses/by-nc-sa/3.0/
//

using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Routing;
using AutoMapper;
using JordanRift.Grassroots.Framework.Data;
using JordanRift.Grassroots.Framework.Entities;
using JordanRift.Grassroots.Framework.Entities.Models;
using JordanRift.Grassroots.Framework.Services;
using JordanRift.Grassroots.Tests.Fakes;
using JordanRift.Grassroots.Tests.Helpers;
using JordanRift.Grassroots.Web.Controllers;
using NUnit.Framework;
using Rhino.Mocks;

namespace JordanRift.Grassroots.Tests.UnitTests.Controllers
{
    [TestFixture]
    public class DonateControllerTests
    {
        private IUserProfileRepository userProfileRepository;
        private ICampaignRepository campaignRepository;
        private IOrganizationRepository organizationRepository;
        private DonateController controller;

        [SetUp]
        public void SetUp()
        {
            organizationRepository = new FakeOrganizationRepository();
            ((FakeOrganizationRepository)organizationRepository).SetUpRepository();
            campaignRepository = new FakeCampaignRepository();
            ((FakeCampaignRepository)campaignRepository).SetUpRepository();
            userProfileRepository = new FakeUserProfileRepository();
            ((FakeUserProfileRepository)userProfileRepository).SetUpRepository();
            var fakeUserRepository = new FakeUserRepository();
            fakeUserRepository.SetUpRepository();
            Mapper.CreateMap<Payment, CampaignDonor>();
        }

        [Test]
        public void Index_Should_Return_View()
        {
            var mocks = new MockRepository();
            SetUpController(mocks);
            var result = controller.Index();
            Assert.IsInstanceOf(typeof(ViewResult), result);
        }

        [Test]
        public void Index_Should_Return_View_Populated_By_UserProfile_If_Logged_In()
        {
            var mocks = new MockRepository();
            var userProfile = EntityHelpers.GetValidUserProfile();
            userProfile.Email = "goodEmail";
            userProfileRepository.Add(userProfile);
            SetUpController(mocks);
            var result = controller.Index() as ViewResult;
            dynamic model = result.Model;
            Assert.AreEqual(userProfile.Email, model.Email);
        }

        [Test]
        public void Index_Should_Return_View_Populated_With_Campaign_If_Good_UrlSlug_Provided()
        {
            var mocks = new MockRepository();
            var campaign = EntityHelpers.GetValidCampaign();
            campaign.UrlSlug = "goodCampaign";
            campaignRepository.Add(campaign);
            SetUpController(mocks);
            var result = controller.Index(campaign.UrlSlug) as ViewResult;
            Assert.AreEqual(campaign.UrlSlug, result.ViewBag.Campaign.UrlSlug);
        }

        [Test]
        public void Index_Should_Redirect_To_CampaignController_When_Campaign_Is_Not_Active()
        {
            var mocks = new MockRepository();
            var campaign = EntityHelpers.GetValidCampaign();
            campaign.StartDate = DateTime.Now.AddDays(-91);
            campaign.EndDate = DateTime.Now.AddDays(-1);
            campaign.UrlSlug = "goodCampaign";
            campaignRepository.Add(campaign);
            SetUpController(mocks);
            var result = controller.Index(campaign.UrlSlug);
            Assert.IsInstanceOf(typeof(RedirectToRouteResult), result);
            var controllerName = ((RedirectToRouteResult) result).RouteValues["Controller"];
            Assert.AreEqual("Campaign", controllerName);
        }

        [Test]
        public void ProcessDonation_Should_Redirect_To_ThankYou_When_Successful()
        {
            var mocks = new MockRepository();
            var payment = EntityHelpers.GetValidCCPayment();

            var campaign = EntityHelpers.GetValidCampaign();
            campaign.Title = "General";
            campaign.CampaignDonors = new List<CampaignDonor>();
            campaignRepository.Add(campaign);
            
            SetUpController(mocks, payment);
            mocks.ReplayAll();
            
            var result = controller.ProcessDonation(payment);
            Assert.IsInstanceOf(typeof(RedirectToRouteResult), result);
            var actionName = ((RedirectToRouteResult) result).RouteValues["Action"];
            Assert.AreEqual("ThankYou", actionName);
            mocks.VerifyAll();
        }

        [Test]
        public void ProcessDonation_Should_Redirect_To_ThankYou_When_Successful_And_Campaign_Present()
        {
            var mocks = new MockRepository();
            var payment = EntityHelpers.GetValidCCPayment();

            var campaign = EntityHelpers.GetValidCampaign();
            campaign.UrlSlug = "goodCampaign";
            campaign.CampaignDonors = new List<CampaignDonor>();
            campaignRepository.Add(campaign);

            SetUpController(mocks, payment);
            mocks.ReplayAll();

            var result = controller.ProcessDonation(payment, campaign.UrlSlug);
            Assert.IsInstanceOf(typeof(RedirectToRouteResult), result);
            var actionName = ((RedirectToRouteResult)result).RouteValues["Action"];
            Assert.AreEqual("ThankYou", actionName);
            mocks.VerifyAll();
        }

        [Test]
        public void ProcessDonation_Should_Redirect_To_ThankYou_When_Successful_And_User_Logged_In()
        {
            var mocks = new MockRepository();
            var payment = EntityHelpers.GetValidCCPayment();

            var campaign = EntityHelpers.GetValidCampaign();
            campaign.Title = "General";
            campaign.CampaignDonors = new List<CampaignDonor>();
            campaignRepository.Add(campaign);

            var userProfile = EntityHelpers.GetValidUserProfile();
            userProfile.Email = "goodEmail";
            userProfileRepository.Add(userProfile);

            SetUpController(mocks, payment);
            mocks.ReplayAll();

            var result = controller.ProcessDonation(payment);
            Assert.IsInstanceOf(typeof(RedirectToRouteResult), result);
            var actionName = ((RedirectToRouteResult)result).RouteValues["Action"];
            Assert.AreEqual("ThankYou", actionName);
            mocks.VerifyAll();
        }

        [Test]
        public void ProcessDonation_Should_Redirect_To_ThankYou_When_Successful_Campus_Present_And_User_Logged_In()
        {
            var mocks = new MockRepository();
            var payment = EntityHelpers.GetValidCCPayment();

            var campaign = EntityHelpers.GetValidCampaign();
            campaign.UrlSlug = "goodCampaign";
            campaign.CampaignDonors = new List<CampaignDonor>();
            campaignRepository.Add(campaign);

            var userProfile = EntityHelpers.GetValidUserProfile();
            userProfile.Email = "goodEmail";
            userProfileRepository.Add(userProfile);

            SetUpController(mocks, payment);
            mocks.ReplayAll();

            var result = controller.ProcessDonation(payment, campaign.UrlSlug);
            Assert.IsInstanceOf(typeof(RedirectToRouteResult), result);
            var actionName = ((RedirectToRouteResult)result).RouteValues["Action"];
            Assert.AreEqual("ThankYou", actionName);
            mocks.VerifyAll();
        }

        [Test]
        public void ProcessDonation_Should_Redirect_To_Index_When_ModelState_Is_Invalid()
        {
            var mocks = new MockRepository();
            var payment = EntityHelpers.GetValidCCPayment();
            SetUpController(mocks);
            controller.ModelState.AddModelError("", "Something isn't right...");
            var result = controller.ProcessDonation(payment);
            Assert.IsInstanceOf(typeof(RedirectToRouteResult), result);
            var actionName = ((RedirectToRouteResult) result).RouteValues["Action"];
            Assert.AreEqual("Index", actionName);
        }

        [Test]
        public void ProcessDonation_Should_Redirect_To_Index_When_Provider_Fails()
        {
            var mocks = new MockRepository();
            var payment = EntityHelpers.GetValidCCPayment();

            var campaign = EntityHelpers.GetValidCampaign();
            campaign.Title = "General";
            campaign.CampaignDonors = new List<CampaignDonor>();
            campaignRepository.Add(campaign);

            SetUpController(mocks, payment, false);
            mocks.ReplayAll();

            var result = controller.ProcessDonation(payment);
            Assert.IsInstanceOf(typeof(RedirectToRouteResult), result);
            var actionName = ((RedirectToRouteResult)result).RouteValues["Action"];
            Assert.AreEqual("Index", actionName);
            mocks.VerifyAll();
        }

        [Test]
        public void ProcessDonation_Should_Redirect_To_CampaignController_When_Campaign_Is_Not_Active()
        {
            var mocks = new MockRepository();
            var payment = EntityHelpers.GetValidCCPayment();

            var campaign = EntityHelpers.GetValidCampaign();
            campaign.UrlSlug = "goodCampaign";
            campaign.StartDate = DateTime.Now.AddDays(-91);
            campaign.EndDate = DateTime.Now.AddDays(-1);
            campaign.CampaignDonors = new List<CampaignDonor>();
            campaignRepository.Add(campaign);

            SetUpController(mocks);

            var result = controller.ProcessDonation(payment, campaign.UrlSlug);
            Assert.IsInstanceOf(typeof(RedirectToRouteResult), result);
            var controllerName = ((RedirectToRouteResult)result).RouteValues["Controller"];
            Assert.AreEqual("Campaign", controllerName);
        }

        [Test]
        public void ThankYou_Should_Return_View_When_Donation_Not_Null()
        {
            var mocks = new MockRepository();
            var payment = EntityHelpers.GetValidCCPayment();
            var donation = Mapper.Map<Payment, CampaignDonor>(payment);
            SetUpController(mocks);
            controller.TempData["Donation"] = donation;
            var result = controller.ThankYou();
            Assert.IsInstanceOf(typeof(ViewResult), result);
        }

        [Test]
        public void ThankYou_Should_Return_NotFound_When_Donation_Is_Null()
        {
            var mocks = new MockRepository();
            SetUpController(mocks);
            var result = controller.ThankYou();
            Assert.IsInstanceOf(typeof(HttpNotFoundResult), result);
        }

        private void SetUpController(MockRepository mocks, Payment payment = null, bool isPaymentApproved = true)
        {
            // Note: May need to change emailService to a strict mock to track calls to notification methods
            //var emailService = mocks.DynamicMock<IEmailService>();
            var paymentProviderFactory = mocks.StrictMock<IPaymentProviderFactory>();
            var paymentProvider = mocks.StrictMock<IPaymentProvider>();

            if (payment != null)
            {
                PaymentResponse response = isPaymentApproved ? 
                    new PaymentResponse(PaymentResponseCode.Approved, -1, string.Empty) : 
                    new PaymentResponse(PaymentResponseCode.Error, -1, string.Empty);

                Expect.Call(paymentProviderFactory.GetPaymentProvider(PaymentGatewayType.PayPal)).IgnoreArguments()
                    .Return(paymentProvider);
                Expect.Call(paymentProvider.Process(payment)).IgnoreArguments().Return(response);
            }

            controller = new DonateController(campaignRepository, userProfileRepository, /*emailService,*/ paymentProviderFactory)
                             {
                                 OrganizationRepository = organizationRepository
                             };

            controller.ControllerContext = new ControllerContext
                                               {
                                                   Controller = controller,
                                                   RequestContext = new RequestContext(new MockHttpContext(), new RouteData())
                                               };
        }
    }
}
