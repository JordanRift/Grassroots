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
    [Export(typeof(ICauseTemplateRepository))]
	public class FakeCauseTemplateRepository : ICauseTemplateRepository
	{
		private static IList<CauseTemplate> causeTemplates;
        public PriorityType Priority { get; set; }

		static FakeCauseTemplateRepository()
		{
		    SetUp();
		}

        private static void SetUp()
        {
            causeTemplates = new List<CauseTemplate>();

            for (int i = 0; i < 5; i++)
            {
                var causeTemplate = EntityHelpers.GetValidCauseTemplate();
                causeTemplate.CauseTemplateID = i + 1;
                causeTemplate.Name = causeTemplate.Name + " " + i;
                causeTemplates.Add(causeTemplate);
            }
        }

        public FakeCauseTemplateRepository()
        {
            Priority = PriorityType.High;
        }

        public static void Reset()
        {
            SetUp();
        }

		public IQueryable<CauseTemplate> FindAllCauseTemplates()
		{
			return causeTemplates.AsQueryable<CauseTemplate>();
		}

		public IQueryable<CauseTemplate> FindActiveCauseTemplates()
		{
			return causeTemplates.AsQueryable<CauseTemplate>().Where<CauseTemplate>( c => c.Active == true );
		}

		public CauseTemplate GetCauseTemplateByID( int id )
		{
			return causeTemplates.FirstOrDefault( c => c.CauseTemplateID == id );
		}

		public void Add( CauseTemplate causeTemplate )
		{
			causeTemplate.CauseTemplateID = causeTemplates.Count + 1;
			causeTemplates.Add( causeTemplate );
		}

		public void Delete( CauseTemplate causeTemplate )
		{
			causeTemplates.Remove( causeTemplate );
		}

		public void Save()
		{
		}

        public void Dispose()
        {
        }
	}
}
