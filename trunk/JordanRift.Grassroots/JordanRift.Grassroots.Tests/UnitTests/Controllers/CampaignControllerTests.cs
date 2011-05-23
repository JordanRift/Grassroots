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
using System.Web.Mvc;
using System.Web.Routing;
using AutoMapper;
using JordanRift.Grassroots.Framework.Data;
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
    public class CampaignControllerTests
    {
        private ICampaignRepository campaignRepository;
        private IUserProfileRepository userProfileRepository;
        private CampaignController controller;

        [SetUp]
        public void SetUp()
        {
            controller = GetCampaignController();
            Mapper.CreateMap<Campaign, CampaignDetailsModel>();
            Mapper.CreateMap<Campaign, CampaignCreateModel>();
        }

        [Test]
        public void Index_Should_Return_NotFound_If_Campaign_Not_Found()
        {
            var result = controller.Index("badUrlSlug");
            Assert.IsInstanceOf(typeof(HttpNotFoundResult), result);
        }

        [Test]
        public void Index_Should_Return_NotFound_If_UrlSlug_Is_Empty()
        {
            var result = controller.Index(string.Empty);
            Assert.IsInstanceOf(typeof(HttpNotFoundResult), result);
        }

        [Test]
        public void Index_Should_Return_Not_Found_If_UrlSlug_Is_Null()
        {
            var result = controller.Index(null);
            Assert.IsInstanceOf(typeof(HttpNotFoundResult), result);
        }

        [Test]
        public void Index_Should_Return_Details_View_If_Campaign_Found()
        {
            var campaign = EntityHelpers.GetValidCampaign();
            campaign.UrlSlug = "goodUrlSlug";
            campaign.UserProfile = EntityHelpers.GetValidUserProfile();
            campaign.CauseTemplate = EntityHelpers.GetValidCauseTemplate();
            campaign.CampaignDonors = new List<CampaignDonor>();
            campaignRepository.Add(campaign);
            var result = controller.Index("goodUrlSlug");
            Assert.IsInstanceOf(typeof(ViewResult), result);
            var viewName = ((ViewResult) result).ViewName;
            Assert.AreEqual("Details", viewName);
        }

        [Test]
        public void Create_Should_Return_Create_View()
        {
            var result = controller.Create();
            Assert.IsInstanceOf(typeof(ViewResult), result);
            var viewName = ((ViewResult) result).ViewName;
            Assert.AreEqual("Create", viewName);
        }

        [Test]
        public void CreateCampaign_Should_Redirect_To_Index_If_Successful()
        {
            var campaign = EntityHelpers.GetValidCampaign();
            var viewModel = Mapper.Map<Campaign, CampaignCreateModel>(campaign);
            var userProfile = EntityHelpers.GetValidUserProfile();
            userProfile.Campaigns = new List<Campaign>();
            userProfile.Email = "goodEmail";
            var organization = EntityHelpers.GetValidOrganization();
            organization.Campaigns = new List<Campaign>();
            var causeTemplate = EntityHelpers.GetValidCauseTemplate();
            causeTemplate.Campaigns = new List<Campaign>();
            organization.CauseTemplates = new List<CauseTemplate> { causeTemplate };
            userProfile.Organization = organization;
            userProfileRepository.Add(userProfile);
            var result = controller.CreateCampaign(viewModel);
            Assert.IsInstanceOf(typeof(RedirectToRouteResult), result);
            var actionName = ((RedirectToRouteResult) result).RouteValues["Action"];
            Assert.AreEqual("Index", actionName);
        }

        [Test]
        public void CreateCampaign_Should_Redirect_To_Create_If_ModelState_Not_Valid()
        {
            var campaign = EntityHelpers.GetValidCampaign();
            var viewModel = Mapper.Map<Campaign, CampaignCreateModel>(campaign);
            controller.ModelState.AddModelError("", "Something bad has happened.");
            var result = controller.CreateCampaign(viewModel);
            Assert.IsInstanceOf(typeof(RedirectToRouteResult), result);
            var actionName = ((RedirectToRouteResult) result).RouteValues["Action"];
            Assert.AreEqual("Create", actionName);
        }

        [Test]
        public void CreateCampaign_Should_Redirect_To_Index_If_Active_Campaign_Already_Present()
        {
            var campaign = EntityHelpers.GetValidCampaign();
            var viewModel = Mapper.Map<Campaign, CampaignCreateModel>(campaign);
            var userProfile = EntityHelpers.GetValidUserProfile();
            userProfile.Email = "goodEmail";
            campaign.UserProfile = userProfile;
            userProfile.Campaigns = new List<Campaign> { campaign };
            userProfileRepository.Add(userProfile);
            var result = controller.CreateCampaign(viewModel);
            Assert.IsInstanceOf(typeof(RedirectToRouteResult), result);
            var actionName = ((RedirectToRouteResult) result).RouteValues["Action"];
            Assert.AreEqual("Index", actionName);
            Assert.IsNotNull(controller.TempData["ErrorMessage"]);
        }

        [Test]
        public void Edit_Should_Return_NotFound_If_Campaign_Not_Found()
        {
            var result = controller.Edit();
            Assert.IsInstanceOf(typeof(HttpNotFoundResult), result);
        }

        [Test]
        public void Edit_Should_Redirect_To_Index_If_UserProfile_Does_Not_Match_Campaign()
        {
            var campaign = EntityHelpers.GetValidCampaign();
            campaign.UrlSlug = "goodUrlSlug";
            var userProfile = EntityHelpers.GetValidUserProfile();
            campaign.UserProfile = userProfile;
            campaignRepository.Add(campaign);
            var result = controller.Edit(campaign.UrlSlug);
            Assert.IsInstanceOf(typeof(RedirectToRouteResult), result);
            var routeName = ((RedirectToRouteResult) result).RouteValues["Action"];
            Assert.AreEqual("Index", routeName);
        }

        [Test]
        public void Edit_Should_Return_View_If_UserProfile_Matches_Campaign()
        {
            var campaign = EntityHelpers.GetValidCampaign();
            campaign.UrlSlug = "goodUrlSlug";
            var causeTemplate = EntityHelpers.GetValidCauseTemplate();
            campaign.CauseTemplate = causeTemplate;
            campaign.CampaignDonors = new List<CampaignDonor>();
            var userProfile = EntityHelpers.GetValidUserProfile();
            userProfile.Email = "goodEmail";
            campaign.UserProfile = userProfile;
            campaignRepository.Add(campaign);
            var result = controller.Edit(campaign.UrlSlug);
            Assert.IsInstanceOf(typeof(ViewResult), result);
            var viewName = ((ViewResult) result).ViewName;
            Assert.AreEqual("Edit", viewName);
        }

        [Test]
        public void Update_Should_Redirect_To_Index_If_User_Is_Not_Campaign_Owner()
        {
            var campaign = EntityHelpers.GetValidCampaign();
            var viewModel = Mapper.Map<Campaign, CampaignDetailsModel>(campaign);
            var userProfile = EntityHelpers.GetValidUserProfile();
            userProfile.Email = "someOtherUser";
            campaign.UserProfile = userProfile;
            campaignRepository.Add(campaign);
            var result = controller.Update(viewModel, campaign.CampaignID);
            Assert.IsInstanceOf(typeof(RedirectToRouteResult), result);
            var actionName = ((RedirectToRouteResult)result).RouteValues["Action"];
            Assert.AreEqual("Index", actionName);
        }

        [Test]
        public void Update_Should_Redirect_To_Edit_If_ModelState_Not_Valid()
        {
            var campaign = EntityHelpers.GetValidCampaign();
            var viewModel = Mapper.Map<Campaign, CampaignDetailsModel>(campaign);
            var userProfile = EntityHelpers.GetValidUserProfile();
            userProfile.Email = "goodEmail";
            campaign.UserProfile = userProfile;
            campaignRepository.Add(campaign);
            controller.ModelState.AddModelError("", "Something bad has happened.");
            var result = controller.Update(viewModel, campaign.CampaignID);
            Assert.IsInstanceOf(typeof(RedirectToRouteResult), result);
            var actionName = ((RedirectToRouteResult) result).RouteValues["Action"];
            Assert.AreEqual("Edit", actionName);
        }

        [Test]
        public void Update_Should_Return_NotFound_If_Campaign_Not_Found()
        {
            var campaign = EntityHelpers.GetValidCampaign();
            var viewModel = Mapper.Map<Campaign, CampaignDetailsModel>(campaign);
            var userProfile = EntityHelpers.GetValidUserProfile();
            userProfile.Email = "goodEmail";
            userProfileRepository.Add(userProfile);
            var result = controller.Update(viewModel);
            Assert.IsInstanceOf(typeof(HttpNotFoundResult), result);
        }
        
        [Test]
        public void Update_Should_Redirect_To_Index_If_Successful()
        {
            var campaign = EntityHelpers.GetValidCampaign();
            var viewModel = Mapper.Map<Campaign, CampaignDetailsModel>(campaign);
            var causeTemplate = EntityHelpers.GetValidCauseTemplate();
            campaign.CauseTemplate = causeTemplate;
            var userProfile = EntityHelpers.GetValidUserProfile();
            userProfile.Email = "goodEmail";
            campaign.UserProfile = userProfile;
            campaignRepository.Add(campaign);
            var result = controller.Update(viewModel, campaign.CampaignID);
            Assert.IsInstanceOf(typeof(RedirectToRouteResult), result);
            var actionName = ((RedirectToRouteResult)result).RouteValues["Action"];
            Assert.AreEqual("Index", actionName);
        }

        [Test]
        public void Map_Should_Update_Campaign_Title()
        {
            var campaign = EntityHelpers.GetValidCampaign();
            var viewModel = Mapper.Map<Campaign, CampaignDetailsModel>(campaign);
            viewModel.Title = "New Title";
            var causeTemplate = EntityHelpers.GetValidCauseTemplate();
            campaign.CauseTemplate = causeTemplate;
            var userProfile = EntityHelpers.GetValidUserProfile();
            userProfile.Email = "goodEmail";
            campaign.UserProfile = userProfile;
            campaignRepository.Add(campaign);
            controller.Update(viewModel, campaign.CampaignID);
            Assert.AreEqual(campaign.Title, viewModel.Title);
        }

        [Test]
        public void Map_Should_Update_Campaign_Description()
        {
            var campaign = EntityHelpers.GetValidCampaign();
            var viewModel = Mapper.Map<Campaign, CampaignDetailsModel>(campaign);
            viewModel.Description = "New Description";
            var causeTemplate = EntityHelpers.GetValidCauseTemplate();
            campaign.CauseTemplate = causeTemplate;
            var userProfile = EntityHelpers.GetValidUserProfile();
            userProfile.Email = "goodEmail";
            campaign.UserProfile = userProfile;
            campaignRepository.Add(campaign);
            controller.Update(viewModel, campaign.CampaignID);
            Assert.AreEqual(campaign.Description, viewModel.Description);
        }

        [Test]
        public void Map_Should_Update_Campaign_UrlSlug()
        {
            var campaign = EntityHelpers.GetValidCampaign();
            var viewModel = Mapper.Map<Campaign, CampaignDetailsModel>(campaign);
            viewModel.UrlSlug = "newslug";
            var causeTemplate = EntityHelpers.GetValidCauseTemplate();
            campaign.CauseTemplate = causeTemplate;
            var userProfile = EntityHelpers.GetValidUserProfile();
            userProfile.Email = "goodEmail";
            campaign.UserProfile = userProfile;
            campaignRepository.Add(campaign);
            controller.Update(viewModel, campaign.CampaignID);
            Assert.AreEqual(campaign.UrlSlug, viewModel.UrlSlug);
        }

        //[Test]
        //public void Map_Should_Update_Campaign_ImagePath()
        //{
        //    var campaign = EntityHelpers.GetValidCampaign();
        //    var viewModel = Mapper.Map<Campaign, CampaignDetailsModel>(campaign);
        //    viewModel.ImagePath = "/new/image/path.jpg";
        //    var causeTemplate = EntityHelpers.GetValidCauseTemplate();
        //    campaign.CauseTemplate = causeTemplate;
        //    var userProfile = EntityHelpers.GetValidUserProfile();
        //    userProfile.Email = "goodEmail";
        //    campaign.UserProfile = userProfile;
        //    campaignRepository.Add(campaign);
        //    controller.Update(viewModel, campaign.CampaignID);
        //    Assert.AreEqual(campaign.ImagePath, viewModel.ImagePath);
        //}

        private CampaignController GetCampaignController()
        {
            var organizationRepository = new FakeOrganizationRepository();
            organizationRepository.SetUpRepository();
            var organization = organizationRepository.GetDefaultOrganization();
            organization.CauseTemplates = new List<CauseTemplate> { EntityHelpers.GetValidCauseTemplate() };
            campaignRepository = new FakeCampaignRepository();
            ((FakeCampaignRepository)campaignRepository).SetUpRepository();
            userProfileRepository = new FakeUserProfileRepository();
            ((FakeUserProfileRepository)userProfileRepository).SetUpRepository();
            var mocks = new MockRepository();
            var mailer = mocks.DynamicMock<ICampaignMailer>();
            MailerBase.IsTestModeEnabled = true;
            var upc = new CampaignController(campaignRepository, userProfileRepository, mailer)
                          {
                              OrganizationRepository = organizationRepository//,
                              //Organization = organization
                          };

            upc.ControllerContext = new ControllerContext
                                        {
                                            Controller = upc,
                                            RequestContext = new RequestContext(new MockHttpContext(), new RouteData()),
                                        };

            return upc;
        }
    }
}
