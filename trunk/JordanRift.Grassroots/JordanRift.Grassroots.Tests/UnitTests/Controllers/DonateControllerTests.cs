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
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using AutoMapper;
using JordanRift.Grassroots.Framework.Data;
using JordanRift.Grassroots.Framework.Entities;
using JordanRift.Grassroots.Framework.Entities.Models;
using JordanRift.Grassroots.Tests.Fakes;
using JordanRift.Grassroots.Tests.Helpers;
using JordanRift.Grassroots.Web.Controllers;
using JordanRift.Grassroots.Web.Mailers;
using JordanRift.Grassroots.Web.Models;
using Mvc.Mailer;
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
        private ICampaignDonorRepository campaignDonorRepository;
        private DonateController controller;

        [SetUp]
        public void SetUp()
        {
            organizationRepository = new FakeOrganizationRepository();
            campaignRepository = new FakeCampaignRepository();
            userProfileRepository = new FakeUserProfileRepository();
            campaignDonorRepository = new FakeCampaignDonorRepository();
            Mapper.CreateMap<Payment, CampaignDonor>();
            Mapper.CreateMap<Payment, DonationDetailsModel>();
            Mapper.CreateMap<CampaignDonor, DonationAdminModel>();
        }

        [TearDown]
        public void TearDown()
        {
            FakeUserProfileRepository.Reset();
            FakeCampaignRepository.Reset();
            FakeOrganizationRepository.Reset();
            FakeUserRepository.Reset();
            FakeCampaignDonorRepository.Reset();
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
            var userProfile = EntityHelpers.GetValidUserProfile();
            var organization = EntityHelpers.GetValidOrganization();
            campaign.UserProfile = userProfile;
            campaign.Title = "General";
            campaign.IsGeneralFund = true;
            campaign.CampaignDonors = new List<CampaignDonor>();
            campaign.Organization = (Organization) organization;
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
            var userProfile = EntityHelpers.GetValidUserProfile();
            var organization = EntityHelpers.GetValidOrganization();
            campaign.UserProfile = userProfile;
            campaign.UrlSlug = "goodCampaign";
            campaign.CampaignDonors = new List<CampaignDonor>();
            campaign.Organization = (Organization)organization;
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
        public void ProcessDonation_Should_Append_Campaign_Info_To_Payment_Note()
        {
            var mocks = new MockRepository();
            var payment = EntityHelpers.GetValidCCPayment();

            var campaign = EntityHelpers.GetValidCampaign();
            var userProfile = EntityHelpers.GetValidUserProfile();
            var organization = EntityHelpers.GetValidOrganization();
            campaign.UserProfile = userProfile;
            campaign.UrlSlug = "goodCampaign";
            campaign.CampaignDonors = new List<CampaignDonor>();
            campaign.Organization = (Organization)organization;
            campaignRepository.Add(campaign);

            SetUpController(mocks, payment);
            mocks.ReplayAll();

            var result = controller.ProcessDonation(payment, campaign.UrlSlug);
            Assert.IsTrue(payment.Notes.Contains(campaign.Title));
            Assert.IsTrue(payment.Notes.Contains(userProfile.FirstName));
            Assert.IsTrue(payment.Notes.Contains(userProfile.LastName));
        }

        [Test]
        public void ProcessDonation_Should_Redirect_To_ThankYou_When_Successful_And_User_Logged_In()
        {
            var mocks = new MockRepository();
            var payment = EntityHelpers.GetValidCCPayment();

            var campaign = EntityHelpers.GetValidCampaign();
            var userProfile = EntityHelpers.GetValidUserProfile();
            var organization = EntityHelpers.GetValidOrganization();
            campaign.UserProfile = userProfile;
            campaign.Title = "General";
            campaign.IsGeneralFund = true;
            campaign.CampaignDonors = new List<CampaignDonor>();
            campaign.Organization = (Organization)organization;
            campaignRepository.Add(campaign);

            //var userProfile = EntityHelpers.GetValidUserProfile();
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
        public void ProcessDonation_Should_Redirect_To_ThankYou_When_Successful_Campaign_Present_And_User_Logged_In()
        {
            var mocks = new MockRepository();
            var payment = EntityHelpers.GetValidCCPayment();

            var campaign = EntityHelpers.GetValidCampaign();
            var userProfile = EntityHelpers.GetValidUserProfile();
            var organization = EntityHelpers.GetValidOrganization();
            campaign.UserProfile = userProfile;
            campaign.UrlSlug = "goodCampaign";
            campaign.CampaignDonors = new List<CampaignDonor>();
            campaign.Organization = (Organization)organization;
            campaignRepository.Add(campaign);

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
        public void ProcessDonation_Should_Set_DisplayName_To_Anonymous_When_Anonymous_Is_Selected()
        {
            var mocks = new MockRepository();
            var payment = EntityHelpers.GetValidCCPayment();
            payment.IsAnonymous = true;

            var campaign = EntityHelpers.GetValidCampaign();
            var userProfile = EntityHelpers.GetValidUserProfile();
            var organization = EntityHelpers.GetValidOrganization();
            campaign.UserProfile = userProfile;
            campaign.UrlSlug = "goodCampaign";
            campaign.CampaignDonors = new List<CampaignDonor>();
            campaign.Organization = organization;
            campaignRepository.Add(campaign);

            userProfile.Email = "goodEmail";
            userProfileRepository.Add(userProfile);

            SetUpController(mocks, payment);
            mocks.ReplayAll();

            controller.ProcessDonation(payment, campaign.UrlSlug);
            var donor = campaign.CampaignDonors.FirstOrDefault();
            Assert.IsNotNull(donor);
            //Assert.IsTrue(donor.IsAnonymous);
            Assert.AreEqual("Anonymous", donor.DisplayName);
        }

        [Test]
        public void ProcessDonation_Should_Set_IsAnonymous_To_True_When_Anonymous_Is_Selected()
        {
            var mocks = new MockRepository();
            var payment = EntityHelpers.GetValidCCPayment();
            payment.IsAnonymous = true;

            var campaign = EntityHelpers.GetValidCampaign();
            var userProfile = EntityHelpers.GetValidUserProfile();
            var organization = EntityHelpers.GetValidOrganization();
            campaign.UserProfile = userProfile;
            campaign.UrlSlug = "goodCampaign";
            campaign.CampaignDonors = new List<CampaignDonor>();
            campaign.Organization = organization;
            campaignRepository.Add(campaign);

            userProfile.Email = "goodEmail";
            userProfileRepository.Add(userProfile);

            SetUpController(mocks, payment);
            mocks.ReplayAll();

            controller.ProcessDonation(payment, campaign.UrlSlug);
            var donor = campaign.CampaignDonors.FirstOrDefault();
            Assert.IsNotNull(donor);
            Assert.IsTrue(donor.IsAnonymous);
        }

        [Test]
        public void ProcessDonation_Should_Set_DisplayName_To_FirstLast_When_DisplayName_Is_Blank()
        {
            var mocks = new MockRepository();
            var payment = EntityHelpers.GetValidCCPayment();

            var campaign = EntityHelpers.GetValidCampaign();
            var userProfile = EntityHelpers.GetValidUserProfile();
            var organization = EntityHelpers.GetValidOrganization();
            campaign.UserProfile = userProfile;
            campaign.UrlSlug = "goodCampaign";
            campaign.CampaignDonors = new List<CampaignDonor>();
            campaign.Organization = organization;
            campaignRepository.Add(campaign);

            userProfile.Email = "goodEmail";
            userProfileRepository.Add(userProfile);

            SetUpController(mocks, payment);
            mocks.ReplayAll();

            controller.ProcessDonation(payment, campaign.UrlSlug);
            var donor = campaign.CampaignDonors.FirstOrDefault();
            Assert.IsNotNull(donor);
            Assert.AreEqual(string.Format("{0} {1}", donor.FirstName, donor.LastName), donor.DisplayName);
        }

        [Test]
        public void ProcessDonation_Should_Not_Set_DisplayName_If_Anonymous_Is_Not_Selected_And_DisplayName_Is_Not_Blank()
        {
            var mocks = new MockRepository();
            var payment = EntityHelpers.GetValidCCPayment();
            payment.DisplayName = "something else";

            var campaign = EntityHelpers.GetValidCampaign();
            var userProfile = EntityHelpers.GetValidUserProfile();
            var organization = EntityHelpers.GetValidOrganization();
            campaign.UserProfile = userProfile;
            campaign.UrlSlug = "goodCampaign";
            campaign.CampaignDonors = new List<CampaignDonor>();
            campaign.Organization = organization;
            campaignRepository.Add(campaign);

            userProfile.Email = "goodEmail";
            userProfileRepository.Add(userProfile);

            SetUpController(mocks, payment);
            mocks.ReplayAll();

            controller.ProcessDonation(payment, campaign.UrlSlug);
            var donor = campaign.CampaignDonors.FirstOrDefault();
            Assert.IsNotNull(donor);
            Assert.AreNotEqual(string.Format("{0} {1}", donor.FirstName, donor.LastName), donor.DisplayName);
        }

        [Test]
        public void ThankYou_Should_Return_View_When_Donation_Not_Null()
        {
            var mocks = new MockRepository();
            var payment = EntityHelpers.GetValidCCPayment();
            var campaign = EntityHelpers.GetValidCampaign();
            var donation = Mapper.Map<Payment, DonationDetailsModel>(payment);
            donation.UrlSlug = campaign.UrlSlug;
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

        [Test]
        public void List_Should_Return_Donate_Grid_View()
        {
            var mocks = new MockRepository();
            SetUpController(mocks);
            var result = controller.List();
            Assert.IsInstanceOf<ViewResult>(result);
        }

        [Test]
        public void List_Should_Return_Populated_Donate_Grid_View()
        {
            var mocks = new MockRepository();
            SetUpController(mocks);
            var result = controller.List() as ViewResult;
            var model = result.Model as IEnumerable<CampaignDonor>;
            Assert.Greater(model.Count(), 0);

        }

        [Test]
        public void Admin_Should_Return_View_If_CampaignDonor_Found()
        {
            var mocks = new MockRepository();
            SetUpController(mocks);
            var donation = EntityHelpers.GetValidCampaignDonor();
            donation.Campaign = EntityHelpers.GetValidCampaign();
            campaignDonorRepository.Add(donation);
            var result = controller.Admin(donation.CampaignDonorID);
            Assert.IsInstanceOf<ViewResult>(result);
        }

        [Test]
        public void Admin_Should_Return_NotFound_If_Campaign_Not_Found()
        {
            var mocks = new MockRepository();
            SetUpController(mocks);
            var result = controller.Admin();
            Assert.IsInstanceOf<HttpNotFoundResult>(result);
        }

        [Test]
        public void Destroy_Should_Return_Json_If_Ajax_Delete_Successful()
        {
            var mocks = new MockRepository();
            SetUpController(mocks);
            controller.Request.Stub(x => x["X-Requested-With"]).Return("XMLHttpRequest");
            var donation = EntityHelpers.GetValidCampaignDonor();
            campaignDonorRepository.Add(donation);
            var result = controller.Destroy(donation.CampaignDonorID);
            Assert.IsInstanceOf<JsonResult>(result);
        }

        [Test]
        public void Destroy_Should_Return_Redirect_If_Delete_Successful()
        {
            var mocks = new MockRepository();
            SetUpController(mocks);
            var donation = EntityHelpers.GetValidCampaignDonor();
            campaignDonorRepository.Add(donation);
            var result = controller.Destroy(donation.CampaignDonorID);
            Assert.IsInstanceOf<RedirectToRouteResult>(result);
        }

        [Test]
        public void Destroy_Should_Remove_CampaignDonor_If_Found()
        {
            var mocks = new MockRepository();
            SetUpController(mocks);
            var donation = EntityHelpers.GetValidCampaignDonor();
            campaignDonorRepository.Add(donation);
            var id = donation.CampaignDonorID;
            controller.Destroy(id);
            donation = campaignDonorRepository.GetDonationByID(id);
            Assert.IsNull(donation);
        }

        [Test]
        public void Destroy_Should_Return_NotFound_If_CampaignDonor_Not_Found()
        {
            var mocks = new MockRepository();
            SetUpController(mocks);
            var result = controller.Destroy();
            Assert.IsInstanceOf<HttpNotFoundResult>(result);
        }

        [Test]
        public void AdminUpdate_Should_Redirect_To_List_If_Successful()
        {
            var mocks = new MockRepository();
            SetUpController(mocks);
            var donation = EntityHelpers.GetValidCampaignDonor();
            campaignDonorRepository.Add(donation);
            var model = Mapper.Map<CampaignDonor, DonationAdminModel>(donation);
            var result = controller.AdminUpdate(model);
            Assert.IsInstanceOf<RedirectToRouteResult>(result);
            var redirect = result as RedirectToRouteResult;
            Assert.AreEqual("List", redirect.RouteValues["Action"]);
        }

        [Test]
        public void AdminUpdate_Should_Redirect_To_Campaign_If_Successful_And_Context_Set()
        {
            var mocks = new MockRepository();
            SetUpController(mocks);
            var donation = EntityHelpers.GetValidCampaignDonor();
            campaignDonorRepository.Add(donation);
            var model = Mapper.Map<CampaignDonor, DonationAdminModel>(donation);
            var result = controller.AdminUpdate(model, "campaign");
            Assert.IsInstanceOf<RedirectToRouteResult>(result);
            var redirect = result as RedirectToRouteResult;
            Assert.AreEqual("Admin", redirect.RouteValues["Action"]);
            Assert.AreEqual("Campaign", redirect.RouteValues["Controller"]);
        }

        [Test]
        public void AdminUpdate_Should_Return_NotFound_If_CampaignDonor_Not_Found()
        {
            var mocks = new MockRepository();
            SetUpController(mocks);
            var donation = EntityHelpers.GetValidCampaignDonor();
            var model = Mapper.Map<CampaignDonor, DonationAdminModel>(donation);
            var result = controller.AdminUpdate(model);
            Assert.IsInstanceOf<HttpNotFoundResult>(result);
        }

        [Test]
        public void AdminUpdate_Should_Redirect_To_Admin_If_ModelState_Not_Valid()
        {
            var mocks = new MockRepository();
            SetUpController(mocks);
            var donation = EntityHelpers.GetValidCampaignDonor();
            campaignDonorRepository.Add(donation);
            controller.ModelState.AddModelError("", "Uh oh...");
            var model = Mapper.Map<CampaignDonor, DonationAdminModel>(donation);
            var result = controller.AdminUpdate(model);
            Assert.IsInstanceOf<RedirectToRouteResult>(result);
            var redirect = result as RedirectToRouteResult;
            Assert.AreEqual("Admin", redirect.RouteValues["Action"]);
        }

        [Test]
        public void AdminUpdate_Should_Update_CampaignDonor_Properties_When_Successful()
        {
            var mocks = new MockRepository();
            SetUpController(mocks);
            var donation = EntityHelpers.GetValidCampaignDonor();
            var campaign = EntityHelpers.GetValidCampaign();
            campaignRepository.Add(campaign);
            donation.CampaignID = campaign.CampaignID;
            donation.Campaign = campaign;
            campaignDonorRepository.Add(donation);
            var id = donation.CampaignDonorID;
            var model = new DonationAdminModel
                            {
                                CampaignDonorID = id,
                                Amount = 1234.56m,
                                Email = "some-other-email",
                                FirstName = "some",
                                LastName = "guy",
                                AddressLine1 = "asdf",
                                AddressLine2 = "yald",
                                City = "townplace",
                                State = "al",
                                ZipCode = "92827",
                                PrimaryPhone = "23434234234",
                                Approved = false,
                                IsAnonymous = true
                            };

            controller.AdminUpdate(model);
            donation = campaignDonorRepository.GetDonationByID(id);
            Assert.AreEqual(model.CampaignDonorID, donation.CampaignDonorID);
            Assert.AreEqual(model.Amount, donation.Amount);
            Assert.AreEqual(model.Email, donation.Email);
            Assert.AreEqual(model.FirstName, donation.FirstName);
            Assert.AreEqual(model.LastName, donation.LastName);
            Assert.AreEqual(model.AddressLine1, donation.AddressLine1);
            Assert.AreEqual(model.AddressLine2, donation.AddressLine2);
            Assert.AreEqual(model.City, donation.City);
            Assert.AreEqual(model.State, donation.State);
            Assert.AreEqual(model.ZipCode, donation.ZipCode);
            Assert.AreEqual(model.PrimaryPhone, donation.PrimaryPhone);
            Assert.AreEqual(model.Approved, donation.Approved);
            Assert.AreEqual(model.IsAnonymous, donation.IsAnonymous);
        }

        private void SetUpController(MockRepository mocks, Payment payment = null, bool isPaymentApproved = true)
        {
            var mailer = mocks.DynamicMock<IDonateMailer>();
            MailerBase.IsTestModeEnabled = true;
            var paymentProviderFactory = mocks.DynamicMock<IPaymentProviderFactory>();
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

            controller = new DonateController(campaignRepository, userProfileRepository, mailer, paymentProviderFactory, campaignDonorRepository)
                             {
                                 OrganizationRepository = organizationRepository
                             };

            var context = MockRepository.GenerateStub<HttpContextBase>();
            var request = MockRepository.GenerateStub<HttpRequestBase>();
            context.Stub(x => x.Request).Return(request);
            context.User = new GenericPrincipal(new GenericIdentity("goodEmail"), null);
            controller.ControllerContext = new ControllerContext(context, new RouteData(), controller);
        }
    }
}
