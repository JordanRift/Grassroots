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

using System.ComponentModel.Composition;
using System.Linq;
using JordanRift.Grassroots.Framework.Entities;
using JordanRift.Grassroots.Framework.Entities.Models;

namespace JordanRift.Grassroots.Framework.Data
{
    [Export(typeof(ICauseTemplateRepository))]
    public class CauseTemplateRepository : GrassrootsRepositoryBase, ICauseTemplateRepository
    {
        public CauseTemplateRepository()
        {
            Priority = PriorityType.Low;
        }

		public IQueryable<CauseTemplate> FindAllCauseTemplates()
		{
			return ObjectContext.CauseTemplates;
		}

		public IQueryable<CauseTemplate> FindActiveCauseTemplates()
		{
			return from causeTemplate in ObjectContext.CauseTemplates
				   where causeTemplate.Active == true
				   orderby causeTemplate.Name
				   select causeTemplate;
		}

		public CauseTemplate GetCauseTemplateByID( int id )
		{
			return ObjectContext.CauseTemplates.FirstOrDefault( c => c.CauseTemplateID == id );
		}

		public void Add( CauseTemplate causeTemplate )
		{
			ObjectContext.CauseTemplates.Add( causeTemplate );
		}

		public void Delete( CauseTemplate causeTemplate )
		{
			ObjectContext.CauseTemplates.Remove( causeTemplate );
		}
	}
}
