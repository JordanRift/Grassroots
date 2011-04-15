//
// Copyright © 2011 Jordan Rift, LLC - All Rights Reserved
//
// THIS WORK IS LICENSED UNDER A CREATIVE COMMONS ATTRIBUTION-NONCOMMERCIAL-
// SHAREALIKE 3.0 UNPORTED LICENSE:
// http://creativecommons.org/licenses/by-nc-sa/3.0/
//

using System.Linq;
using JordanRift.Grassroots.Framework.Entities.Models;

namespace JordanRift.Grassroots.Framework.Data
{
    public class CauseTemplateRepository : GrassrootsRepositoryBase, ICauseTemplateRepository
    {
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
