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
using System.ComponentModel.Composition;
using System.Linq;
using JordanRift.Grassroots.Framework.Entities;
using JordanRift.Grassroots.Framework.Entities.Models;

namespace JordanRift.Grassroots.Framework.Data
{
    [Export(typeof(ICampaignDonorRepository))]
    public class CampaignDonorRepository : GrassrootsRepositoryBase, ICampaignDonorRepository
    {
        public CampaignDonorRepository()
        {
            Priority = PriorityType.Low;
        }

        public IQueryable<CampaignDonor> FindAllDonations()
        {
            return ObjectContext.CampaignDonors;
        }

        public IQueryable<CampaignDonor> FindApprovedDonations()
        {
            return ObjectContext.CampaignDonors.Where(d => d.Approved);
        }

        public CampaignDonor GetDonationByID(int id)
        {
            return ObjectContext.CampaignDonors.FirstOrDefault(d => d.CampaignDonorID == id);
        }

        public void Add(CampaignDonor campaignDonor)
        {
            ObjectContext.CampaignDonors.Add(campaignDonor);
        }

        public void Delete(CampaignDonor campaignDonor)
        {
            ObjectContext.CampaignDonors.Remove(campaignDonor);
        }

        void ICampaignDonorRepository.Save()
        {
            base.Save();
        }
    }
}
