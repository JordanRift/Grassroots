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
using System.Web.Mvc;
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
        private IPaymentProviderFactory paymentProviderFactory;
        private DonateController controller;
        private MockRepository mocks;

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
            mocks = new MockRepository();
            SetUpController();
        }

        [TearDown]
        public void TearDown()
        {
            FakeUserProfileRepository.Reset();
            FakeCampaignRepository.Reset();
            FakeOrganizationRepository.Reset();
            FakeUserRepository.Reset();
            FakeCampaignDonorRepository.Reset();
            controller = null;
            mocks = null;
        }


        [Test]
        public void Index_Should_Return_View()
        {
            var result = controller.Index();
            Assert.IsInstanceOf(typeof(ViewResult), result);
        }

        [Test]
        public void Index_Should_Return_View_Populated_By_UserProfile_If_Logged_In()
        {
            var userProfile = EntityHelpers.GetValidUserProfile();
            userProfile.Email = "goodEmail";
            userProfileRepository.Add(userProfile);
            var result = controller.Index() as ViewResult;
            dynamic model = result.Model;
            Assert.AreEqual(userProfile.Email, model.Email);
        }

        [Test]
        public void Index_Should_Return_View_Populated_With_Campaign_If_Good_UrlSlug_Provided()
        {
            var campaign = EntityHelpers.GetValidCampaign();
            campaign.UrlSlug = "goodCampaign";
            campaignRepository.Add(campaign);
            var result = controller.Index(campaign.UrlSlug) as ViewResult;
            Assert.AreEqual(campaign.UrlSlug, result.ViewBag.Campaign.UrlSlug);
        }

        [Test]
        public void Index_Should_Redirect_To_CampaignController_When_Campaign_Is_Not_Active()
        {
            var campaign = EntityHelpers.GetValidCampaign();
            campaign.StartDate = DateTime.Now.AddDays(-91);
            campaign.EndDate = DateTime.Now.AddDays(-1);
            campaign.UrlSlug = "goodCampaign";
            campaignRepository.Add(campaign);
            var result = controller.Index(campaign.UrlSlug);
            Assert.IsInstanceOf(typeof(RedirectToRouteResult), result);
            var controllerName = ((RedirectToRouteResult) result).RouteValues["Controller"];
            Assert.AreEqual("Campaign", controllerName);
        }

        [Test]
        public void ProcessDonation_Should_Redirect_To_ThankYou_When_Successful()
        {
            var payment = EntityHelpers.GetValidCCPayment();

            var campaign = EntityHelpers.GetValidCampaign();
            var userProfile = EntityHelpers.GetValidUserProfile();
            var organization = EntityHelpers.GetValidOrganization();
            campaign.UserProfile = userProfile;
            campaign.Title = "General";
            campaign.IsGeneralFund = true;
            campaign.CampaignDonors = new List<CampaignDonor>();
            campaign.Organization = organization;
            campaignRepository.Add(campaign);
            SetUpPaymentResponse(payment);
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
            var payment = EntityHelpers.GetValidCCPayment();

            var campaign = EntityHelpers.GetValidCampaign();
            var userProfile = EntityHelpers.GetValidUserProfile();
            var organization = EntityHelpers.GetValidOrganization();
            campaign.UserProfile = userProfile;
            campaign.UrlSlug = "goodCampaign";
            campaign.CampaignDonors = new List<CampaignDonor>();
            campaign.Organization = organization;
            campaignRepository.Add(campaign);
            SetUpPaymentResponse(payment);
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
            var payment = EntityHelpers.GetValidCCPayment();

            var campaign = EntityHelpers.GetValidCampaign();
            var userProfile = EntityHelpers.GetValidUserProfile();
            var organization = EntityHelpers.GetValidOrganization();
            campaign.UserProfile = userProfile;
            campaign.UrlSlug = "goodCampaign";
            campaign.CampaignDonors = new List<CampaignDonor>();
            campaign.Organization = organization;
            campaignRepository.Add(campaign);
            SetUpPaymentResponse(payment);
            mocks.ReplayAll();

            controller.ProcessDonation(payment, campaign.UrlSlug);
            Assert.IsTrue(payment.Notes.Contains(campaign.Title));
            Assert.IsTrue(payment.Notes.Contains(userProfile.FirstName));
            Assert.IsTrue(payment.Notes.Contains(userProfile.LastName));
        }

        [Test]
        public void ProcessDonation_Should_Append_Organization_Name_To_Payment_Note_When_UrlSlug_Present()
        {
            var payment = EntityHelpers.GetValidCCPayment();
            var campaign = EntityHelpers.GetValidCampaign();
            var userProfile = EntityHelpers.GetValidUserProfile();
            var organization = EntityHelpers.GetValidOrganization();
            campaign.UserProfile = userProfile;
            campaign.UrlSlug = "goodCampaign";
            campaign.CampaignDonors = new List<CampaignDonor>();
            campaign.Organization = organization;
            campaignRepository.Add(campaign);
            SetUpPaymentResponse(payment);
            mocks.ReplayAll();
            controller.ProcessDonation(payment, campaign.UrlSlug);
            Assert.IsTrue(payment.Notes.Contains(organization.Name));
        }

        [Test]
        public void ProcessDonatoin_Should_Append_Organization_Name_To_Payment_Notee_When_UrlSlug_Not_Present()
        {
            var payment = EntityHelpers.GetValidCCPayment();
            var campaign = EntityHelpers.GetValidCampaign();
            var userProfile = EntityHelpers.GetValidUserProfile();
            var organization = EntityHelpers.GetValidOrganization();
            campaign.UserProfile = userProfile;
            campaign.IsGeneralFund = true;
            campaign.CampaignDonors = new List<CampaignDonor>();
            campaign.Organization = organization;
            campaignRepository.Add(campaign);
            SetUpPaymentResponse(payment);
            mocks.ReplayAll();
            controller.ProcessDonation(payment);
            Assert.IsTrue(payment.Notes.Contains(organization.Name));
        }

        [Test]
        public void ProcessDonation_Should_Redirect_To_ThankYou_When_Successful_And_User_Logged_In()
        {
            var payment = EntityHelpers.GetValidCCPayment();

            var campaign = EntityHelpers.GetValidCampaign();
            var userProfile = EntityHelpers.GetValidUserProfile();
            var organization = EntityHelpers.GetValidOrganization();
            campaign.UserProfile = userProfile;
            campaign.Title = "General";
            campaign.IsGeneralFund = true;
            campaign.CampaignDonors = new List<CampaignDonor>();
            campaign.Organization = organization;
            campaignRepository.Add(campaign);

            userProfile.Email = "goodEmail";
            userProfileRepository.Add(userProfile);
            SetUpPaymentResponse(payment);
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
            SetUpPaymentResponse(payment);
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
            var payment = EntityHelpers.GetValidCCPayment();
            controller.ModelState.AddModelError("", "Something isn't right...");
            var result = controller.ProcessDonation(payment);
            Assert.IsInstanceOf(typeof(RedirectToRouteResult), result);
            var actionName = ((RedirectToRouteResult) result).RouteValues["Action"];
            Assert.AreEqual("Index", actionName);
        }

        [Test]
        public void ProcessDonation_Should_Redirect_To_Index_When_Provider_Fails()
        {
            var payment = EntityHelpers.GetValidCCPayment();

            var campaign = EntityHelpers.GetValidCampaign();
            campaign.Title = "General";
            campaign.CampaignDonors = new List<CampaignDonor>();
            campaign.Organization = EntityHelpers.GetValidOrganization();
            campaignRepository.Add(campaign);
            SetUpPaymentResponse(payment, false);
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
            var payment = EntityHelpers.GetValidCCPayment();

            var campaign = EntityHelpers.GetValidCampaign();
            campaign.UrlSlug = "goodCampaign";
            campaign.StartDate = DateTime.Now.AddDays(-91);
            campaign.EndDate = DateTime.Now.AddDays(-1);
            campaign.CampaignDonors = new List<CampaignDonor>();
            campaignRepository.Add(campaign);

            var result = controller.ProcessDonation(payment, campaign.UrlSlug);
            Assert.IsInstanceOf(typeof(RedirectToRouteResult), result);
            var controllerName = ((RedirectToRouteResult)result).RouteValues["Controller"];
            Assert.AreEqual("Campaign", controllerName);
        }

        [Test]
        public void ProcessDonation_Should_Set_DisplayName_To_Anonymous_When_Anonymous_Is_Selected()
        {
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
            SetUpPaymentResponse(payment);
            mocks.ReplayAll();

            controller.ProcessDonation(payment, campaign.UrlSlug);
            var donor = campaign.CampaignDonors.FirstOrDefault();
            Assert.IsNotNull(donor);
            Assert.AreEqual("Anonymous", donor.DisplayName);
        }

        [Test]
        public void ProcessDonation_Should_Set_IsAnonymous_To_True_When_Anonymous_Is_Selected()
        {
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
            SetUpPaymentResponse(payment);
            mocks.ReplayAll();

            controller.ProcessDonation(payment, campaign.UrlSlug);
            var donor = campaign.CampaignDonors.FirstOrDefault();
            Assert.IsNotNull(donor);
            Assert.IsTrue(donor.IsAnonymous);
        }

        [Test]
        public void ProcessDonation_Should_Set_DisplayName_To_FirstLast_When_DisplayName_Is_Blank()
        {
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
            SetUpPaymentResponse(payment);
            mocks.ReplayAll();

            controller.ProcessDonation(payment, campaign.UrlSlug);
            var donor = campaign.CampaignDonors.FirstOrDefault();
            Assert.IsNotNull(donor);
            Assert.AreEqual(string.Format("{0} {1}", donor.FirstName, donor.LastName), donor.DisplayName);
        }

        [Test]
        public void ProcessDonation_Should_Not_Set_DisplayName_If_Anonymous_Is_Not_Selected_And_DisplayName_Is_Not_Blank()
        {
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
            SetUpPaymentResponse(payment);
            mocks.ReplayAll();

            controller.ProcessDonation(payment, campaign.UrlSlug);
            var donor = campaign.CampaignDonors.FirstOrDefault();
            Assert.IsNotNull(donor);
            Assert.AreNotEqual(string.Format("{0} {1}", donor.FirstName, donor.LastName), donor.DisplayName);
        }

        [Test]
        public void ThankYou_Should_Return_View_When_Donation_Not_Null()
        {
            var payment = EntityHelpers.GetValidCCPayment();
            var campaign = EntityHelpers.GetValidCampaign();
            var donation = Mapper.Map<Payment, DonationDetailsModel>(payment);
            donation.UrlSlug = campaign.UrlSlug;
            controller.TempData["Donation"] = donation;
            var result = controller.ThankYou();
            Assert.IsInstanceOf(typeof(ViewResult), result);
        }

        [Test]
        public void ThankYou_Should_Return_NotFound_When_Donation_Is_Null()
        {
            var result = controller.ThankYou();
            Assert.IsInstanceOf(typeof(HttpNotFoundResult), result);
        }

        [Test]
        public void List_Should_Return_Donate_Grid_View()
        {
            var result = controller.List();
            Assert.IsInstanceOf<ViewResult>(result);
        }

        [Test]
        public void List_Should_Return_Populated_Donate_Grid_View()
        {
            var result = controller.List() as ViewResult;
            var model = result.Model as IEnumerable<DonationAdminModel>;
            Assert.Greater(model.Count(), 0);

        }

        [Test]
        public void Admin_Should_Return_View_If_CampaignDonor_Found()
        {
            var donation = EntityHelpers.GetValidCampaignDonor();
            donation.Campaign = EntityHelpers.GetValidCampaign();
            campaignDonorRepository.Add(donation);
            var result = controller.Admin(donation.CampaignDonorID);
            Assert.IsInstanceOf<ViewResult>(result);
        }

        [Test]
        public void Admin_Should_Return_NotFound_If_Campaign_Not_Found()
        {
            var result = controller.Admin();
            Assert.IsInstanceOf<HttpNotFoundResult>(result);
        }

        [Test]
        public void Destroy_Should_Return_Json_If_Ajax_Delete_Successful()
        {
            controller.Request.Stub(x => x["X-Requested-With"]).Return("XMLHttpRequest");
            var donation = EntityHelpers.GetValidCampaignDonor();
            campaignDonorRepository.Add(donation);
            var result = controller.Destroy(donation.CampaignDonorID);
            Assert.IsInstanceOf<JsonResult>(result);
        }

        [Test]
        public void Destroy_Should_Return_Redirect_If_Delete_Successful()
        {
            var donation = EntityHelpers.GetValidCampaignDonor();
            campaignDonorRepository.Add(donation);
            var result = controller.Destroy(donation.CampaignDonorID);
            Assert.IsInstanceOf<RedirectToRouteResult>(result);
        }

        [Test]
        public void Destroy_Should_Remove_CampaignDonor_If_Found()
        {
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
            var result = controller.Destroy();
            Assert.IsInstanceOf<HttpNotFoundResult>(result);
        }

        [Test]
        public void AdminUpdate_Should_Redirect_To_Campaign_If_Successful_And_Context_Set()
        {
            var donation = EntityHelpers.GetValidCampaignDonor();
            campaignDonorRepository.Add(donation);
            var model = Mapper.Map<CampaignDonor, DonationAdminModel>(donation);
            var result = controller.AdminUpdate(model);
            Assert.IsInstanceOf<RedirectToRouteResult>(result);
            var redirect = result as RedirectToRouteResult;
            Assert.AreEqual("Admin", redirect.RouteValues["Action"]);
            Assert.AreEqual("Campaign", redirect.RouteValues["Controller"]);
        }

        [Test]
        public void AdminUpdate_Should_Return_NotFound_If_CampaignDonor_Not_Found()
        {
            var donation = EntityHelpers.GetValidCampaignDonor();
            var model = Mapper.Map<CampaignDonor, DonationAdminModel>(donation);
            var result = controller.AdminUpdate(model);
            Assert.IsInstanceOf<HttpNotFoundResult>(result);
        }

        [Test]
        public void AdminUpdate_Should_Redirect_To_Admin_If_ModelState_Not_Valid()
        {
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

        [Test]
        public void AdminUpdate_Should_Populate_DisplayName_When_Anonymous_Selected_And_DisplayName_Left_Blank()
        {
            var donation = EntityHelpers.GetValidCampaignDonor();
            campaignDonorRepository.Add(donation);
            var model = Mapper.Map<CampaignDonor, DonationAdminModel>(donation);
            model.IsAnonymous = true;
            model.DisplayName = string.Empty;
            controller.AdminUpdate(model);
            Assert.AreEqual("Anonymous", donation.DisplayName);
        }

        [Test]
        public void New_Should_Return_View()
        {
            var result = controller.New();
            Assert.IsInstanceOf<ViewResult>(result);
        }

        [Test]
        public void New_Should_Return_Populated_View_When_Model_In_TempData()
        {
            var campaignDonor = EntityHelpers.GetValidCampaignDonor();
            var model = Mapper.Map<CampaignDonor, DonationAdminModel>(campaignDonor);
            controller.TempData["DonationAdminModel"] = model;
            var result = controller.New();
            var viewModel = (result as ViewResult).Model as DonationAdminModel;
            Assert.AreEqual(model.FirstName, viewModel.FirstName);
        }

        [Test]
        public void Create_Should_Redirect_To_Admin_When_Successful()
        {
            var campaignDonor = EntityHelpers.GetValidCampaignDonor();
            var campaign = EntityHelpers.GetValidCampaign();
            campaign.CampaignDonors = new List<CampaignDonor>();
            campaignRepository.Add(campaign);
            var model = Mapper.Map<CampaignDonor, DonationAdminModel>(campaignDonor);
            model.CampaignID = campaign.CampaignID;
            var result = controller.Create(model);
            Assert.IsInstanceOf<RedirectToRouteResult>(result);
            var redirect = result as RedirectToRouteResult;
            Assert.AreEqual("Admin", redirect.RouteValues["Action"]);
        }

        [Test]
        public void Create_Should_Redirect_To_New_When_ModelState_Is_Invalid()
        {
            var campaignDonor = EntityHelpers.GetValidCampaignDonor();
            var model = Mapper.Map<CampaignDonor, DonationAdminModel>(campaignDonor);
            controller.ModelState.AddModelError("", "Uh oh...");
            var result = controller.Create(model);
            Assert.IsInstanceOf<RedirectToRouteResult>(result);
            var redirect = result as RedirectToRouteResult;
            Assert.AreEqual("New", redirect.RouteValues["Action"]);
        }

        [Test]
        public void Create_Should_Add_Model_To_Repository_When_Successful()
        {
            var campaignDonor = EntityHelpers.GetValidCampaignDonor();
            var campaign = EntityHelpers.GetValidCampaign();
            campaign.CampaignDonors = new List<CampaignDonor>();
            campaignRepository.Add(campaign);
            var model = Mapper.Map<CampaignDonor, DonationAdminModel>(campaignDonor);
            model.CampaignID = campaign.CampaignID;
            controller.Create(model);
            campaignDonor = campaign.CampaignDonors.FirstOrDefault();
            Assert.IsNotNull(campaignDonor);
        }

        [Test]
        public void Create_Should_Associate_CampaignDonor_With_UserProfile_If_Emails_Match()
        {
            var campaignDonor = EntityHelpers.GetValidCampaignDonor();
            var campaign = EntityHelpers.GetValidCampaign();
            campaign.CampaignDonors = new List<CampaignDonor>();
            campaignRepository.Add(campaign);
            var userProfile = EntityHelpers.GetValidUserProfile();
            userProfile.CampaignDonors = new List<CampaignDonor>();
            userProfile.Email = campaignDonor.Email;
            userProfileRepository.Add(userProfile);
            var model = Mapper.Map<CampaignDonor, DonationAdminModel>(campaignDonor);
            model.CampaignID = campaign.CampaignID;
            controller.Create(model);
            campaignDonor = userProfile.CampaignDonors.FirstOrDefault();
            Assert.IsNotNull(campaignDonor);
        }

        [Test]
        public void Create_Should_Populate_DisplayName_When_Anonymous_Selected_And_DisplayName_Left_Blank()
        {
            var campaignDonor = EntityHelpers.GetValidCampaignDonor();
            var campaign = EntityHelpers.GetValidCampaign();
            campaign.CampaignDonors = new List<CampaignDonor>();
            campaignRepository.Add(campaign);
            var model = Mapper.Map<CampaignDonor, DonationAdminModel>(campaignDonor);
            model.IsAnonymous = true;
            model.DisplayName = string.Empty;
            model.CampaignID = campaign.CampaignID;
            controller.Create(model);
            campaignDonor = campaign.CampaignDonors.FirstOrDefault();
            Assert.AreEqual("Anonymous", campaignDonor.DisplayName);
        }

        [Test]
        public void ResendNotification_Should_Return_Redirect_When_Successfuil()
        {
            var campaignDonor = EntityHelpers.GetValidCampaignDonor();
            var campaign = EntityHelpers.GetValidCampaign();
            campaignDonor.Campaign = campaign;
            campaignDonorRepository.Add(campaignDonor);
            var result = controller.ResendNotification(campaignDonor.CampaignDonorID);
            Assert.IsInstanceOf<RedirectToRouteResult>(result);
        }

        [Test]
        public void ResendNotification_Should_Return_Json_When_Ajax_Successful()
        {
            var campaignDonor = EntityHelpers.GetValidCampaignDonor();
            var campaign = EntityHelpers.GetValidCampaign();
            campaignDonor.Campaign = campaign;
            campaignDonorRepository.Add(campaignDonor);
            controller.Request.Stub(r => r["X-Requested-With"]).Return("XMLHttpRequest");
            var result = controller.ResendNotification(campaignDonor.CampaignDonorID);
            Assert.IsInstanceOf<JsonResult>(result);
        }

        [Test]
        public void ResendNotification_Should_Return_NotFound_When_Donor_Not_Found()
        {
            var result = controller.ResendNotification();
            Assert.IsInstanceOf<HttpNotFoundResult>(result);
        }

        private void SetUpController()
        {
            var mailer = mocks.DynamicMock<IDonateMailer>();
            MailerBase.IsTestModeEnabled = true;
            paymentProviderFactory = mocks.DynamicMock<IPaymentProviderFactory>();

            controller = new DonateController(campaignRepository, userProfileRepository, mailer, paymentProviderFactory, campaignDonorRepository)
                             {
                                 OrganizationRepository = organizationRepository
                             };

            TestHelpers.MockBasicRequest(controller);
        }

        private void SetUpPaymentResponse(Payment payment, bool isPaymentApproved = true)
        {
            var paymentProvider = mocks.StrictMock<IPaymentProvider>();

            PaymentResponse response = isPaymentApproved ?
                    new PaymentResponse(PaymentResponseCode.Approved, -1, string.Empty) :
                    new PaymentResponse(PaymentResponseCode.Error, -1, string.Empty);

            Expect.Call(paymentProviderFactory.GetPaymentProvider(PaymentGatewayType.PayPal)).IgnoreArguments()
                .Return(paymentProvider);
            Expect.Call(paymentProvider.Process(payment)).IgnoreArguments().Return(response);
        }
    }
}
