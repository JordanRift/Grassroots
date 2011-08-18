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
using System.Transactions;
using JordanRift.Grassroots.Framework.Data;
using JordanRift.Grassroots.Framework.Entities.Models;
using JordanRift.Grassroots.Framework.Helpers;
using JordanRift.Grassroots.Framework.Services;
using JordanRift.Grassroots.Tests.Helpers;
using NUnit.Framework;

namespace JordanRift.Grassroots.IntegrationTests.IntegrationTests.Models
{
    [TestFixture]
    public class CampaignRepositoryTests
    {
        private ICampaignRepository campaignRepository;
        private IOrganizationRepository organizationRepository;

        private Organization organization;
        private UserProfile userProfile;
        private User user;
        private CauseTemplate causeTemplate;
        private Campaign campaign;

        [SetUp]
        public void SetUp()
        {
            campaignRepository = new CampaignRepository();
            organizationRepository = new OrganizationRepository();
        }

        [Test]
        public void Add_Should_Add_Campaign_To_Database()
        {
            using (new UnitOfWorkScope())
            using (new TransactionScope())
            {
                ArrangeCampaignTest();
                var id = campaign.CampaignID;
                Assert.Greater(id, 0);
            }
        }

        [Test]
        public void FindAllCampaigns_Should_Return_List_When_Campaigns_Found()
        {
            using (new UnitOfWorkScope())
            using (new TransactionScope())
            {
                ArrangeCampaignTest();
                var results = campaignRepository.FindAllCampaigns();
                Assert.IsNotNull(results);
                Assert.IsNotEmpty(results.ToList());
            }
        }

        [Test]
        public void FindActiveCampaigns_Should_Return_List_When_Campaigns_Found()
        {
            using (new UnitOfWorkScope())
            using (new TransactionScope())
            {
                ArrangeCampaignTest();
                var results = campaignRepository.FindActiveCampaigns();
                Assert.IsNotNull(results);
                Assert.IsNotEmpty(results.ToList());
            }
        }

        [Test]
        public void GetCampaignByID_Should_Load_Campaign_From_Database()
        {
            using (new UnitOfWorkScope())
            using (new TransactionScope())
            {
                ArrangeCampaignTest();
                var id = campaign.CampaignID;
                campaign = null;
                campaign = campaignRepository.GetCampaignByID(id);
                Assert.IsNotNull(campaign);
                Assert.AreEqual(campaign.CampaignID, id);
            }
        }

        [Test]
        public void GetCampaignByID_Should_Return_Null_When_CampaignID_Not_Found()
        {
            using (new UnitOfWorkScope())
            using (new TransactionScope())
            {
                ArrangeCampaignTest();
                var id = campaign.CampaignID + 1;
                var result = campaign = campaignRepository.GetCampaignByID(id);
                Assert.IsNull(result);
            }
        }

        [Test]
        public void GetCampaignByUrlSlug_Should_Load_Campaign_From_Database()
        {
            using (new UnitOfWorkScope())
            using (new TransactionScope())
            {
                ArrangeCampaignTest();
                var slug = campaign.UrlSlug;
                campaign = null;
                campaign = campaignRepository.GetCampaignByUrlSlug(slug);
                Assert.IsNotNull(campaign);
                Assert.AreEqual(campaign.UrlSlug, slug);
            }
        }

        [Test]
        public void GetCampaignByUrlSlug_Should_Return_Null_When_UrlSlug_Not_Found()
        {
            using (new UnitOfWorkScope())
            using (new TransactionScope())
            {
                ArrangeCampaignTest();
                var result = campaignRepository.GetCampaignByUrlSlug("non-existant-url");
                Assert.IsNull(result);
            }
        }

        [Test]
        public void Exists_Should_Return_True_When_UrlSlug_Found()
        {
            using (new UnitOfWorkScope())
            using (new TransactionScope())
            {
                ArrangeCampaignTest();
                var slug = campaign.UrlSlug;
                var result = campaignRepository.Exists(slug);
                Assert.True(result);
            }
        }

        [Test]
        public void Exists_Should_Return_False_When_UrlSlug_Not_Found()
        {
            using (new UnitOfWorkScope())
            using (new TransactionScope())
            {
                ArrangeCampaignTest();
                var result = campaignRepository.Exists("non-existant-url");
                Assert.False(result);
            }
        }

        [Test]
        public void Delete_Should_Remove_Campaign_From_Database()
        {
            using (new UnitOfWorkScope())
            using (new TransactionScope())
            {
                ArrangeCampaignTest();
                var slug = campaign.UrlSlug;
                campaignRepository.Delete(campaign);
                campaignRepository.Save();
                var result = campaignRepository.Exists(slug);
                Assert.IsFalse(result);
            }
        }

        private void ArrangeCampaignTest()
        {
            organization = EntityHelpers.GetValidOrganization() as Organization;
            organization.UserProfiles = new List<UserProfile>();
            organization.CauseTemplates = new List<CauseTemplate>();
            organization.Campaigns = new List<Campaign>();
            organizationRepository.Add(organization);
            organizationRepository.Save();

            userProfile = EntityHelpers.GetValidUserProfile();
            userProfile.Users = new List<User>();
            userProfile.Campaigns = new List<Campaign>();
            userProfile.UserProfileService = new UserProfileService(new UserProfileRepository());
            organization.UserProfiles.Add(userProfile);
            organizationRepository.Save();

            user = EntityHelpers.GetValidUser();
            userProfile.Users.Add(user);
            organizationRepository.Save();

            causeTemplate = EntityHelpers.GetValidCauseTemplate();
            causeTemplate.Campaigns = new List<Campaign>();
            organization.CauseTemplates.Add(causeTemplate);
            organizationRepository.Save();

            campaign = EntityHelpers.GetValidCampaign();
            campaign.CampaignService = new CampaignService(new CampaignRepository());
            organization.Campaigns.Add(campaign);
            userProfile.Campaigns.Add(campaign);
            causeTemplate.Campaigns.Add(campaign);
            campaignRepository.Save();
        }
    }
}
