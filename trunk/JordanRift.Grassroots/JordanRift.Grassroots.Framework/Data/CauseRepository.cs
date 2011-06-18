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
    public class CauseRepository : GrassrootsRepositoryBase, ICauseRepository
    {

		public IQueryable<Cause> FindAllCauses()
		{
			return ObjectContext.Causes;
		}

		public IQueryable<Cause> FindActiveCauses()
		{
			return from cause in ObjectContext.Causes
				   where cause.Active
				   orderby cause.Name
				   select cause;
		}

		/// <summary>
		/// Get all causes where the given user is either a donor or the campaigner.
		/// </summary>
		/// <param name="userProfileID">The userProfileID of either a donor or campaigner</param>
		/// <returns>a collection of Causes</returns>
		public IQueryable<Cause> FindCausesByUserProfileID( int userProfileID )
		{
			return from cause in ObjectContext.Causes
				   where cause.Campaigns.Any(
					camp => ( camp.CampaignDonors.Any( donor => donor.UserProfileID == userProfileID ) )
					||  camp.UserProfileID == userProfileID )
				   orderby cause.Name
				   select cause;
		}

		public IQueryable<Cause> FindCausesByCauseTemplateID(int causeTemplateID)
		{
			return from cause in ObjectContext.Causes
				   where cause.CauseTemplateID == causeTemplateID
				   orderby cause.Name
				   select cause;
		}

		public Cause GetCauseByID( int id )
		{
			return ObjectContext.Causes.FirstOrDefault( c => c.CauseID == id );
		}

        public Cause GetCauseByCauseTemplateIdAndReferenceNumber(int id, string referenceNumber)
        {
            return ObjectContext.Causes.FirstOrDefault(c => c.CauseTemplateID == id && c.ReferenceNumber.Equals(referenceNumber, StringComparison.CurrentCultureIgnoreCase));
        }

        public void Add( Cause cause )
		{
			ObjectContext.Causes.Add( cause );
		}

		public void AddNote( CauseNote note )
		{
			ObjectContext.CauseNotes.Add( note );
		}

		public void Delete( Cause cause )
		{
			ObjectContext.Causes.Remove( cause );
		}
	}
}
