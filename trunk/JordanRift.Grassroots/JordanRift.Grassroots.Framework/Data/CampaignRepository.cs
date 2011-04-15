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

namespace JordanRift.Grassroots.Framework.Data
{
    public class CampaignRepository : GrassrootsRepositoryBase, ICampaignRepository
    {
		public IQueryable<Campaign> FindAllCampaigns()
		{
            return ObjectContext.Campaigns;
		}

		public IQueryable<Campaign> FindActiveCampaigns()
		{
            return from campaign in ObjectContext.Campaigns
				   where campaign.EndDate > DateTime.Now
				   orderby campaign.EndDate
				   select campaign;
		}

        public Campaign GetCampaignByID(int id)
        {
            return ObjectContext.Campaigns.FirstOrDefault(c => c.CampaignID == id);
        }

        public Campaign GetCampaignByUrlSlug(string urlSlug)
        {
            if (string.IsNullOrEmpty(urlSlug))
            {
                return null;
            }

            return ObjectContext.Campaigns.FirstOrDefault(c => c.UrlSlug.ToLower() == urlSlug.ToLower());
        }

        public Campaign GetDefaultCampaign()
        {
            return ObjectContext.Campaigns.FirstOrDefault(c => c.Title == "General");
        }

        public bool Exists(string urlSlug)
        {
            return ObjectContext.Campaigns.Any(c => c.UrlSlug.ToLower() == urlSlug.ToLower());
        }

        public void Add(Campaign campaign)
        {

            ObjectContext.Campaigns.Add(campaign);
        }

        public void AddDonor(CampaignDonor donor)
        {
            ObjectContext.CampaignDonors.Add(donor);
        }

        public void Delete(Campaign campaign)
        {
            ObjectContext.Campaigns.Remove(campaign);
        }

        void ICampaignRepository.Save()
        {
            base.Save();
        }
	}
}
