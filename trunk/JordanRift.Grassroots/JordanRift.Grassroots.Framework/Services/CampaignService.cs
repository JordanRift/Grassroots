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
