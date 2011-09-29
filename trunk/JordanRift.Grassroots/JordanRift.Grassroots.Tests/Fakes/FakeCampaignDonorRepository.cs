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

using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using JordanRift.Grassroots.Framework.Data;
using JordanRift.Grassroots.Framework.Entities;
using JordanRift.Grassroots.Framework.Entities.Models;
using JordanRift.Grassroots.Tests.Helpers;

namespace JordanRift.Grassroots.Tests.Fakes
{
    [Export(typeof(ICampaignDonorRepository))]
    public class FakeCampaignDonorRepository : ICampaignDonorRepository
    {
        public PriorityType Priority { get; set; }
        private static IList<CampaignDonor> donors;

        public FakeCampaignDonorRepository()
        {
            Priority = PriorityType.High;
        }

        static FakeCampaignDonorRepository()
        {
            SetUp();
        }

        private static void SetUp()
        {
            donors = new List<CampaignDonor>();

            for (int i = 0; i < 5; i++)
            {
                var donation = EntityHelpers.GetValidCampaignDonor();
                donation.CampaignDonorID = i + 1;
                donors.Add(donation);
            }
        }

        public static void Clear()
        {
            donors = new List<CampaignDonor>();
        }

        public static void Reset()
        {
            SetUp();
        }

        public void Dispose()
        {
        }

        public IQueryable<CampaignDonor> FindAllDonations()
        {
            return donors.AsQueryable();
        }

        public IQueryable<CampaignDonor> FindApprovedDonations()
        {
            return donors.Where(d => d.Approved).AsQueryable();
        }

        public CampaignDonor GetDonationByID(int id)
        {
            return donors.FirstOrDefault(d => d.CampaignDonorID == id);
        }

        public void Delete(CampaignDonor campaignDonor)
        {
            donors.Remove(campaignDonor);
        }

        public void Save()
        {
        }
    }
}
