//
// Copyright © 2011 Jordan Rift, LLC - All Rights Reserved
//
// THIS WORK IS LICENSED UNDER A CREATIVE COMMONS ATTRIBUTION-NONCOMMERCIAL-
// SHAREALIKE 3.0 UNPORTED LICENSE:
// http://creativecommons.org/licenses/by-nc-sa/3.0/
//

using System;
using System.Collections.Generic;
using System.Linq;
using JordanRift.Grassroots.Framework.Data;
using JordanRift.Grassroots.Framework.Entities.Models;
using JordanRift.Grassroots.Tests.Helpers;

namespace JordanRift.Grassroots.Tests.Fakes
{
    [Obsolete("This class will be obsolete in future versions in favor of using Rhino Mocks. See DonateControllerTests for example of new pattern.")]
    public class FakeCampaignRepository : ICampaignRepository
    {
        private static IList<Campaign> campaigns;

        public void SetUpRepository()
        {
            campaigns = new List<Campaign>();

            for (int i = 0; i < 5; i++)
            {
                var campaign = EntityHelpers.GetValidCampaign();
                campaign.CampaignID = i + 1;
                campaign.UrlSlug = "non-unique-slug";
                campaigns.Add(campaign);
            }

            campaigns.First().UrlSlug = "MyCampaign";
        }

		public IQueryable<Campaign> FindAllCampaigns()
		{
			return campaigns.AsQueryable<Campaign>();
		}

		public IQueryable<Campaign> FindActiveCampaigns()
		{
			return campaigns.AsQueryable<Campaign>().Where<Campaign>( c => c.EndDate > DateTime.Now );
		}

        public Campaign GetCampaignByID(int id)
        {
            return campaigns.FirstOrDefault(c => c.CampaignID == id);
        }

        public Campaign GetCampaignByUrlSlug(string urlSlug)
        {
            if (string.IsNullOrEmpty(urlSlug))
            {
                return null;
            }

            return campaigns.FirstOrDefault(c => c.UrlSlug.ToLower() == urlSlug.ToLower());
        }

        public Campaign GetDefaultCampaign()
        {
            return campaigns.FirstOrDefault(c => c.Title == "General");
        }

        public bool Exists(string urlSlug)
        {
            return campaigns.Any(c => c.UrlSlug == urlSlug);
        }

        public void Add(Campaign campaign)
        {
            campaign.CampaignID = campaigns.Count + 1;
            campaigns.Add(campaign);
        }

        public void AddDonor(CampaignDonor donor)
        {
        }

        public void Delete(Campaign campaign)
        {
            campaigns.Remove(campaign);
        }

        public void Save()
        {
        }
    }
}
