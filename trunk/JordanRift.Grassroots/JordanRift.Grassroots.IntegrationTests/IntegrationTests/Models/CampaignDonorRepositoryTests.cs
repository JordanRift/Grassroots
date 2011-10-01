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
    public class CampaignDonorRepositoryTests
    {
        private TransactionScope transactionScope;
        private ICampaignDonorRepository campaignDonorRepository;
        private ICampaignRepository campaignRepository;
        private IOrganizationRepository organizationRepository;

        private Organization organization;
        private UserProfile userProfile;
        private User user;
        private CauseTemplate causeTemplate;
        private Campaign campaign;
        private CampaignDonor campaignDonor;

        [SetUp]
        public void SetUp()
        {
            campaignRepository = new CampaignRepository();
            organizationRepository = new OrganizationRepository();
            campaignDonorRepository = new CampaignDonorRepository();
            transactionScope = new TransactionScope();
        }

        [TearDown]
        public void TearDown()
        {
            transactionScope.Dispose();
        }

        [Test]
        public void Add_Should_Add_CampaignDonor_To_Database()
        {
            using (new UnitOfWorkScope())
            {
                Arrange();
                var id = campaignDonor.CampaignDonorID;
                Assert.Greater(id, 0);
            }
        }

        [Test]
        public void FindAllDonations_Should_Return_List_When_CampaignDonors_Found()
        {
            using (new UnitOfWorkScope())
            {
                Arrange();
                var result = campaignDonorRepository.FindAllDonations();
                Assert.IsNotNull(result);
                Assert.IsNotEmpty(result.ToList());
            }
        }

        [Test]
        public void FindApprovedDonations_Should_Return_List_When_CampaignDonors_Found()
        {
            using (new UnitOfWorkScope())
            {
                Arrange();
                var result = campaignDonorRepository.FindApprovedDonations();
                Assert.IsNotNull(result);
                Assert.IsNotEmpty(result.ToList());
            }
        }

        [Test]
        public void GetDonationByID_Should_Return_CampaignDonor_When_Found()
        {
            using (new UnitOfWorkScope())
            {
                Arrange();
                var result = campaignDonorRepository.GetDonationByID(campaignDonor.CampaignDonorID);
                Assert.IsNotNull(result);
                Assert.AreEqual(campaignDonor.CampaignDonorID, result.CampaignDonorID);
            }
        }

        [Test]
        public void Delete_Should_Remove_CampaignDonor_From_Database()
        {
            using (new UnitOfWorkScope())
            {
                Arrange();
                var id = campaignDonor.CampaignDonorID;
                campaignDonorRepository.Delete(campaignDonor);
                campaignDonorRepository.Save();
                var result = campaignDonorRepository.GetDonationByID(id);
                Assert.IsNull(result);
            }
        }

        private void Arrange()
        {
            organization = EntityHelpers.GetValidOrganization();
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

            campaignDonor = EntityHelpers.GetValidCampaignDonor();
            campaign.CampaignDonors = new List<CampaignDonor>();
            campaign.CampaignDonors.Add(campaignDonor);
            userProfile.CampaignDonors = new List<CampaignDonor>();
            userProfile.CampaignDonors.Add(campaignDonor);
            campaignDonorRepository.Save();
        }
    }
}
