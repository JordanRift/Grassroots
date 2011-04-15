//
// Copyright © 2011 Jordan Rift, LLC - All Rights Reserved
//
// THIS WORK IS LICENSED UNDER A CREATIVE COMMONS ATTRIBUTION-NONCOMMERCIAL-
// SHAREALIKE 3.0 UNPORTED LICENSE:
// http://creativecommons.org/licenses/by-nc-sa/3.0/
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
using JordanRift.Grassroots.Web.Models;
using NUnit.Framework;

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
            var viewModel = Mapper.Map<Campaign, CampaignDetailsModel>(campaign);
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
            var viewModel = Mapper.Map<Campaign, CampaignDetailsModel>(campaign);
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
            var viewModel = Mapper.Map<Campaign, CampaignDetailsModel>(campaign);
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
        public void Update_Should_Redirect_To_Edit_If_ModelState_Not_Valid()
        {
            var campaign = EntityHelpers.GetValidCampaign();
            var viewModel = Mapper.Map<Campaign, CampaignDetailsModel>(campaign);
            var userProfile = EntityHelpers.GetValidUserProfile();
            userProfile.Email = "goodEmail";
            userProfile.Campaigns = new List<Campaign> { campaign };
            userProfileRepository.Add(userProfile);
            controller.ModelState.AddModelError("", "Something bad has happened.");
            var result = controller.Update(viewModel);
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
            var userProfile = EntityHelpers.GetValidUserProfile();
            userProfile.Email = "goodEmail";
            userProfile.Campaigns = new List<Campaign> { campaign };
            userProfileRepository.Add(userProfile);
            var result = controller.Update(viewModel);
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
            var userProfile = EntityHelpers.GetValidUserProfile();
            userProfile.Email = "goodEmail";
            userProfile.Campaigns = new List<Campaign> { campaign };
            userProfileRepository.Add(userProfile);
            controller.Update(viewModel);
            Assert.AreEqual(campaign.Title, viewModel.Title);
        }

        [Test]
        public void Map_Should_Update_Campaign_Description()
        {
            var campaign = EntityHelpers.GetValidCampaign();
            var viewModel = Mapper.Map<Campaign, CampaignDetailsModel>(campaign);
            viewModel.Description = "New Description";
            var userProfile = EntityHelpers.GetValidUserProfile();
            userProfile.Email = "goodEmail";
            userProfile.Campaigns = new List<Campaign> { campaign };
            userProfileRepository.Add(userProfile);
            controller.Update(viewModel);
            Assert.AreEqual(campaign.Description, viewModel.Description);
        }

        [Test]
        public void Map_Should_Update_Campaign_UrlSlug()
        {
            var campaign = EntityHelpers.GetValidCampaign();
            var viewModel = Mapper.Map<Campaign, CampaignDetailsModel>(campaign);
            viewModel.UrlSlug = "newslug";
            var userProfile = EntityHelpers.GetValidUserProfile();
            userProfile.Email = "goodEmail";
            userProfile.Campaigns = new List<Campaign> { campaign };
            userProfileRepository.Add(userProfile);
            controller.Update(viewModel);
            Assert.AreEqual(campaign.UrlSlug, viewModel.UrlSlug);
        }

        [Test]
        public void Map_Should_Update_Campaign_ImagePath()
        {
            var campaign = EntityHelpers.GetValidCampaign();
            var viewModel = Mapper.Map<Campaign, CampaignDetailsModel>(campaign);
            viewModel.ImagePath = "/new/image/path.jpg";
            var userProfile = EntityHelpers.GetValidUserProfile();
            userProfile.Email = "goodEmail";
            userProfile.Campaigns = new List<Campaign> { campaign };
            userProfileRepository.Add(userProfile);
            controller.Update(viewModel);
            Assert.AreEqual(campaign.ImagePath, viewModel.ImagePath);
        }

        private CampaignController GetCampaignController()
        {
            campaignRepository = new FakeCampaignRepository();
            ((FakeCampaignRepository)campaignRepository).SetUpRepository();
            userProfileRepository = new FakeUserProfileRepository();
            ((FakeUserProfileRepository)userProfileRepository).SetUpRepository();
            var upc = new CampaignController(campaignRepository, userProfileRepository);
            upc.ControllerContext = new ControllerContext
                                        {
                                            Controller = upc,
                                            RequestContext = new RequestContext(new MockHttpContext(), new RouteData()),
                                        };

            return upc;
        }
    }
}
