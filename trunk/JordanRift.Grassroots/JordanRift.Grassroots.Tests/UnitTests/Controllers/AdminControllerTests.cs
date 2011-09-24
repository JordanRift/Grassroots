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
    public class AdminControllerTests
    {
        private IOrganizationRepository organizationRepository;
        private ICampaignRepository campaignRepository;
        private IRoleRepository roleRepository;
        private AdminController controller;

        [SetUp]
        public void SetUp()
        {
            Mapper.CreateMap<Organization, OrganizationDetailsModel>();
        }

        [TearDown]
        public void TearDown()
        {
            FakeOrganizationRepository.Reset();
        }

       [Test]
        public void Index_Should_Return_View()
        {
            SetUpAdminController();
            var result = controller.Index();
            Assert.IsInstanceOf(typeof(ViewResult), result);
        }

        [Test]
        public void EditOrganization_Should_Return_View_When_Organization_Found()
        {
            SetUpAdminController();
            var result = controller.EditOrganization();
            Assert.IsInstanceOf(typeof(ViewResult), result);
        }

        [Test]
        public void EditOrganization_Should_Return_NotFound_When_Organization_Not_Found()
        {
            SetUpAdminController(false);
            var result = controller.EditOrganization();
            Assert.IsInstanceOf(typeof(HttpNotFoundResult), result);
        }

        [Test]
        public void UpdateOrganization_Should_Redirect_To_Index_When_Successful()
        {
            var organization = EntityHelpers.GetValidOrganization();
            var viewModel = Mapper.Map<Organization, OrganizationDetailsModel>(organization);
            SetUpAdminController(repoReadOnly: false);
            var result = controller.UpdateOrganization(viewModel);
            Assert.IsInstanceOf(typeof(RedirectToRouteResult), result);
            var actionName = ((RedirectToRouteResult) result).RouteValues["Action"];
            Assert.AreEqual("Index", actionName);
        }

        [Test]
        public void UpdateOrganization_Should_Redirect_To_EditOrganization_When_ModelState_Is_Invalid()
        {
            var organization = EntityHelpers.GetValidOrganization();
            var viewModel = Mapper.Map<Organization, OrganizationDetailsModel>(organization);
            SetUpAdminController();
            controller.ModelState.AddModelError("", "Uh oh...");
            var result = controller.UpdateOrganization(viewModel);
            Assert.IsInstanceOf(typeof(RedirectToRouteResult), result);
            var actionName = ((RedirectToRouteResult) result).RouteValues["Action"];
            Assert.AreEqual("EditOrganization", actionName);
        }

        [Test]
        public void GererateDefaultCampaign_Should_Return_200_If_Successful()
        {
            SetUpAdminController();
            FakeOrganizationRepository.Clear();
            FakeRoleRepository.Clear();
            var organization = EntityHelpers.GetValidOrganization();
            organization.Campaigns = new List<Campaign>();
            var campaign = EntityHelpers.GetValidCampaign();
            campaign.IsGeneralFund = true;
            campaign.StartDate = DateTime.Now.AddMonths(-1);
            campaignRepository.Add(campaign);
            var causeTemplate = EntityHelpers.GetValidCauseTemplate();
            causeTemplate.Campaigns = new List<Campaign>();
            organization.CauseTemplates = new List<CauseTemplate> { causeTemplate };
            var role = new Role { Name = "Root" };
            var userProfile = EntityHelpers.GetValidUserProfile();
            userProfile.Campaigns = new List<Campaign>();
            role.UserProfiles = new List<UserProfile> { userProfile };
            roleRepository.Add(role);
            organizationRepository.Add(organization);
            var result = controller.GenerateDetaultCampaign();
            Assert.IsInstanceOf<HttpStatusCodeResult>(result);
            var statusCode = result as HttpStatusCodeResult;
            Assert.AreEqual(statusCode.StatusCode, 200);
        }

        [Test]
        public void GenerateDefaultCampaign_Should_Return_403_If_Current_Month_Defaut_Exists()
        {
            SetUpAdminController();
            FakeOrganizationRepository.Clear();
            var organization = EntityHelpers.GetValidOrganization();
            organization.Campaigns = new List<Campaign>();
            var campaign = EntityHelpers.GetValidCampaign();
            campaign.IsGeneralFund = true;
            campaignRepository.Add(campaign);
            organizationRepository.Add(organization);
            var result = controller.GenerateDetaultCampaign();
            Assert.IsInstanceOf<HttpStatusCodeResult>(result);
            var statusCode = result as HttpStatusCodeResult;
            Assert.AreEqual(statusCode.StatusCode, 403);
        }

        [Test]
        public void GenerateDefaultCampaign_Should_Return_NotFound_If_CauseTemplate_Not_Found()
        {
            SetUpAdminController();
            FakeOrganizationRepository.Clear();
            FakeRoleRepository.Clear();
            var organization = EntityHelpers.GetValidOrganization();
            var campaign = EntityHelpers.GetValidCampaign();
            campaign.IsGeneralFund = true;
            campaign.StartDate = DateTime.Now.AddMonths(-1);
            campaignRepository.Add(campaign);
            organization.CauseTemplates = new List<CauseTemplate>();
            var role = new Role { Name = "Root" };
            var userProfile = EntityHelpers.GetValidUserProfile();
            role.UserProfiles = new List<UserProfile> { userProfile };
            roleRepository.Add(role);
            organizationRepository.Add(organization);
            var result = controller.GenerateDetaultCampaign();
            Assert.IsInstanceOf<HttpNotFoundResult>(result);
        }

        [Test]
        public void GenerateDefaultCampaign_Should_Return_NotFound_If_UserProfile_Not_Found()
        {
            SetUpAdminController();
            FakeOrganizationRepository.Clear();
            FakeRoleRepository.Clear();
            var organization = EntityHelpers.GetValidOrganization();
            var campaign = EntityHelpers.GetValidCampaign();
            campaign.IsGeneralFund = true;
            campaign.StartDate = DateTime.Now.AddMonths(-1);
            campaignRepository.Add(campaign);
            var causeTemplate = EntityHelpers.GetValidCauseTemplate();
            organization.CauseTemplates = new List<CauseTemplate> { causeTemplate };
            var role = new Role { Name = "Root" };
            role.UserProfiles = new List<UserProfile>();
            roleRepository.Add(role);
            organizationRepository.Add(organization);
            var result = controller.GenerateDetaultCampaign();
            Assert.IsInstanceOf<HttpNotFoundResult>(result);
        }

        private void SetUpAdminController(bool shouldFindOrganization = true, bool repoReadOnly = true)
        {
            organizationRepository = new FakeOrganizationRepository();
            campaignRepository = new FakeCampaignRepository();
            roleRepository = new FakeRoleRepository();

            if (!shouldFindOrganization)
            {
                FakeOrganizationRepository.Clear();
            }

            controller = new AdminController(campaignRepository, roleRepository)
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
