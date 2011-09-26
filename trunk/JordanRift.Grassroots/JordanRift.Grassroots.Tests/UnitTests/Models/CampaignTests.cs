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
using System.Linq;
using JordanRift.Grassroots.Framework.Entities.Models;
using JordanRift.Grassroots.Tests.Helpers;
using NUnit.Framework;
using JordanRift.Grassroots.Framework.Data;
using JordanRift.Grassroots.Tests.Fakes;

namespace JordanRift.Grassroots.Tests.UnitTests.Models
{
    [TestFixture]
    public class CampaignTests
    {
		private ICampaignRepository repository;
        private CauseTemplate causeTemplate;
        private Campaign campaign;
		
		[SetUp]
		public void SetUp()
		{
		    repository = new FakeCampaignRepository();
            //((FakeCampaignRepository)repository).SetUpRepository();
            campaign = EntityHelpers.GetValidCampaign();
		    causeTemplate = EntityHelpers.GetValidCauseTemplate();

		    campaign.CauseTemplate = causeTemplate;
		}

		[Test]
		public void FindAllCampaigns_Should_Return_More_Than_Zero_Campaigns()
		{
			var campaigns = repository.FindAllCampaigns();
			Assert.IsTrue( campaigns.Count() > 0 );
		}

		[Test]
		public void FindActiveCampaigns_Should_Return_Less_Than_AllCampaigns()
		{
			var campaigns = repository.FindAllCampaigns();
			int countBefore = campaigns.Count();

			var theCampaign = campaigns.ElementAt( 0 );
            theCampaign.EndDate = DateTime.Now.AddDays(-10);
			int countAfter = repository.FindActiveCampaigns().Count();

			Assert.IsTrue( countAfter < countBefore );
		}

        [Test]
        public void Validate_Should_Return_Empty_When_UrlSlug_Is_Unique()
        {
            campaign.UrlSlug = "mysuperawesomecampaign";
            var results = campaign.Validate(null);
            Assert.IsEmpty(results.ToList());
        }

        [Test]
        public void Validate_Should_Return_ValidationResult_When_UrlSlug_Is_Not_Unique()
        {
            campaign.UrlSlug = "non-unique-slug";
            var results = campaign.Validate(null);
            Assert.IsNotEmpty(results.ToList());
        }

        [Test]
        public void Validate_Should_Return_ValidationResult_When_TimeSpan_Not_Equal_To_CauseTemplate()
        {
            campaign.StartDate = DateTime.Now.AddDays(1);
            campaign.EndDate = DateTime.Now;
            var results = campaign.Validate(null);
            Assert.IsNotEmpty(results.ToList());
        }

        [Test]
        public void Validate_Should_Return_Empty_When_TimeSpan_Equal_To_CauseTemplate()
        {
            campaign.UrlSlug = "mysuperawesomecampaign";
            var results = campaign.Validate(null);
            Assert.IsEmpty(results.ToList());
        }

        [Test]
        public void Validate_Should_Return_Empty_When_Goal_Amount_Matches_CauseTemplate()
        {
            campaign.UrlSlug = "mysuperawesomecampaign";
            var results = campaign.Validate(null);
            Assert.IsEmpty(results.ToList());
        }

        //[Test]
        //public void Validate_Should_Return_ValidationResult_When_Goal_Amount_Does_Not_Match_CauseTemplate()
        //{
        //    campaign.UrlSlug = "mysuperawesomecampaign";
        //    causeTemplate.DefaultAmount = 200;
        //    var results = campaign.Validate(null);
        //    Assert.IsNotEmpty(results.ToList());
        //}

        [Test]
        public void Validate_Should_Return_Empty_When_Amount_Does_Not_Match_CauseTemplate_That_Is_Configurable()
        {
            causeTemplate.AmountIsConfigurable = true;
            campaign.GoalAmount = (decimal) 2.00;
            campaign.UrlSlug = "mysuperawesomecampaign";
            var results = campaign.Validate(null);
            Assert.IsEmpty(results.ToList());
        }

        [Test]
        public void Validate_Should_Return_Empty_When_Length_Does_Not_Match_CauseTemplate_That_Is_Configurable()
        {
            causeTemplate.TimespanIsConfigurable = true;
            campaign.EndDate = DateTime.Now.AddDays(100);
            campaign.UrlSlug = "mysuperawesomecampaign";
            var results = campaign.Validate(null);
            Assert.IsEmpty(results.ToList());
        }
    }
}
