//
// Copyright © 2011 Jordan Rift, LLC - All Rights Reserved
//
// THIS WORK IS LICENSED UNDER A CREATIVE COMMONS ATTRIBUTION-NONCOMMERCIAL-
// SHAREALIKE 3.0 UNPORTED LICENSE:
// http://creativecommons.org/licenses/by-nc-sa/3.0/
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
            ((FakeCampaignRepository)repository).SetUpRepository();
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

        [Test]
        public void Validate_Should_Return_ValidationResult_When_Goal_Amount_Does_Not_Match_CauseTemplate()
        {
            campaign.UrlSlug = "mysuperawesomecampaign";
            causeTemplate.DefaultAmount = 200;
            var results = campaign.Validate(null);
            Assert.IsNotEmpty(results.ToList());
        }

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
