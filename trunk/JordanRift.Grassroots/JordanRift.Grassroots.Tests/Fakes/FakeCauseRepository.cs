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
    [Export(typeof(ICauseRepository))]
    public class FakeCauseRepository : ICauseRepository
    {
        private static IList<Cause> causes;
        private static IList<CauseNote> causeNotes; 
        public PriorityType Priority { get; set; }

        public void Dispose()
        {
        }

        static FakeCauseRepository()
        {
            SetUp();
        }

        private static void SetUp()
        {
            causeNotes = new List<CauseNote>();
            causes = new List<Cause>();

            for (int i = 0; i < 5; i++)
            {
                var cause = EntityHelpers.GetValidCause();
                cause.CauseID = i + 1;
                causes.Add(cause);
            }
        }

        public static void Reset()
        {
            SetUp();
        }

        public FakeCauseRepository()
        {
            Priority = PriorityType.High;
        }

        public IQueryable<Cause> FindAllCauses()
        {
            return causes.AsQueryable();
        }

        public IQueryable<Cause> FindActiveCauses()
        {
            return causes.Where(c => c.Active).AsQueryable();
        }

        public IQueryable<Cause> FindCausesByCauseTemplateID(int causeTemplateID)
        {
            return causes.Where(c => c.CauseTemplateID == causeTemplateID).AsQueryable();
        }

        public IQueryable<Cause> FindCausesByUserProfileID(int userProfileID)
        {
            var campaign = EntityHelpers.GetValidCampaign();
            campaign.CampaignDonors = new List<CampaignDonor>();
            var result = new List<Cause>();

            foreach (var cause in causes)
            {
                if (campaign.CampaignDonors.Any(c => c.UserProfileID == userProfileID) || campaign.UserProfileID == userProfileID)
                {
                    result.Add(cause);
                }
            }

            return result.AsQueryable();
        }

        public Cause GetCauseByID(int id)
        {
            return causes.FirstOrDefault(c => c.CauseID == id);
        }

        public Cause GetCauseByCauseTemplateIdAndReferenceNumber(int id, string referenceNumber)
        {
            return causes.FirstOrDefault(c => c.CauseTemplateID == id && c.ReferenceNumber.ToLower() == referenceNumber.ToLower());
        }

        public void Add(Cause cause)
        {
            cause.CauseID = causes.Count + 1;
            causes.Add(cause);
        }

        public void AddNote(CauseNote note)
        {
            causeNotes.Add(note);
        }

        public void Delete(Cause cause)
        {
            causes.Remove(cause);
        }

        public void Save()
        {
        }
    }
}
