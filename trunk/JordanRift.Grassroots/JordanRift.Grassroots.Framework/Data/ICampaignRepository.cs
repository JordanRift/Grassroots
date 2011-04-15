//
// Copyright © 2011 Jordan Rift, LLC - All Rights Reserved
//
// THIS WORK IS LICENSED UNDER A CREATIVE COMMONS ATTRIBUTION-NONCOMMERCIAL-
// SHAREALIKE 3.0 UNPORTED LICENSE:
// http://creativecommons.org/licenses/by-nc-sa/3.0/
//

using JordanRift.Grassroots.Framework.Entities.Models;
using System.Linq;

namespace JordanRift.Grassroots.Framework.Data
{
    public interface ICampaignRepository
    {
		IQueryable<Campaign> FindAllCampaigns();
		IQueryable<Campaign> FindActiveCampaigns();
        Campaign GetCampaignByID(int id);
        Campaign GetCampaignByUrlSlug(string urlSlug);
        Campaign GetDefaultCampaign();
        bool Exists(string urlSlug);
        void Add(Campaign campaign);
        void AddDonor(CampaignDonor donor);
        void Delete(Campaign campaign);
        void Save();
    }
}
