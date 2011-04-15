//
// Copyright © 2011 Jordan Rift, LLC - All Rights Reserved
//
// THIS WORK IS LICENSED UNDER A CREATIVE COMMONS ATTRIBUTION-NONCOMMERCIAL-
// SHAREALIKE 3.0 UNPORTED LICENSE:
// http://creativecommons.org/licenses/by-nc-sa/3.0/
//

using JordanRift.Grassroots.Framework.Data;
using JordanRift.Grassroots.Framework.Helpers;

namespace JordanRift.Grassroots.Framework.Services
{
    public class CampaignService
    {
        private readonly ICampaignRepository campaignRepository;

        public CampaignService()
        {
            campaignRepository = RepositoryFactory.GetRepository<ICampaignRepository>();
        }

        public CampaignService(ICampaignRepository campaignRepository)
        {
            this.campaignRepository = campaignRepository;
        }

        public bool IsUnique(string urlSlug, int id)
        {
            var result = true;
            var campaignWithSameSlug = campaignRepository.GetCampaignByUrlSlug(urlSlug);

            if (id <= 0 && campaignWithSameSlug != null)  // Case: New Campaign
            {
                result = false;
            }
            else if (campaignWithSameSlug != null && campaignWithSameSlug.CampaignID != id)  // Case: UrlSlug Change
            {
                result = false;
            }

            // Case: Saving without changing UrlSlug
            return result;
        }
    }
}
