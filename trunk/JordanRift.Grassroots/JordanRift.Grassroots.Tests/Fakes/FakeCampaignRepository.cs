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
using System.ComponentModel.Composition;
using System.Linq;
using JordanRift.Grassroots.Framework.Data;
using JordanRift.Grassroots.Framework.Entities;
using JordanRift.Grassroots.Framework.Entities.Models;
using JordanRift.Grassroots.Tests.Helpers;

namespace JordanRift.Grassroots.Tests.Fakes
{
    [Export(typeof(ICampaignRepository))]
    public class FakeCampaignRepository : ICampaignRepository
    {
        private static IList<Campaign> campaigns;
        public PriorityType Priority { get; set; }

        static FakeCampaignRepository()
        {
            SetUp();
        }

        private static void SetUp()
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

        public FakeCampaignRepository()
        {
            Priority = PriorityType.High;
        }

        public static void Empty()
        {
            campaigns = new List<Campaign>();
        }

        public static void Reset()
        {
            SetUp();
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

        public void Dispose()
        {
        }
    }
}
