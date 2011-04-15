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

		public void Add( Cause cause )
		{
			ObjectContext.Causes.Add( cause );
		}

		public void Delete( Cause cause )
		{
			ObjectContext.Causes.Remove( cause );
		}
	}
}
