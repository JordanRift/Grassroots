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
    public interface ICauseRepository : IPriority, IDisposable
	{
		IQueryable<Cause> FindAllCauses();
		IQueryable<Cause> FindActiveCauses();
		IQueryable<Cause> FindCausesByCauseTemplateID(int causeTemplateID);
		IQueryable<Cause> FindCausesByUserProfileID(int userProfileID);
		Cause GetCauseByID( int id );
        Cause GetCauseByCauseTemplateIdAndReferenceNumber(int id, string referenceNumber);
		void Add(Cause cause);
		void AddNote( CauseNote note );
		void Delete(Cause cause);
		void Save();
	}
}
