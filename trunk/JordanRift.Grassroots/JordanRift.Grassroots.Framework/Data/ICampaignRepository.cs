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
using JordanRift.Grassroots.Framework.Entities;
using JordanRift.Grassroots.Framework.Entities.Models;
using System.Linq;

namespace JordanRift.Grassroots.Framework.Data
{
    public interface ICampaignRepository : IPriority, IDisposable
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
