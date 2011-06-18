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

            return ObjectContext.Campaigns.FirstOrDefault(c => c.UrlSlug.Equals(urlSlug, StringComparison.CurrentCultureIgnoreCase));
        }

        public Campaign GetDefaultCampaign()
        {
            return ObjectContext.Campaigns.FirstOrDefault(c => c.Title == "General");
        }

        public bool Exists(string urlSlug)
        {
            return ObjectContext.Campaigns.Any(c => c.UrlSlug.Equals(urlSlug, StringComparison.CurrentCultureIgnoreCase));
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
