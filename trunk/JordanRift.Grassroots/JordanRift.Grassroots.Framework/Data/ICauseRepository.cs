//
// Copyright © 2011 Jordan Rift, LLC - All Rights Reserved
//
// THIS WORK IS LICENSED UNDER A CREATIVE COMMONS ATTRIBUTION-NONCOMMERCIAL-
// SHAREALIKE 3.0 UNPORTED LICENSE:
// http://creativecommons.org/licenses/by-nc-sa/3.0/
//

using JordanRift.Grassroots.Framework.Entities.Models;
using System.Linq;

namespace JordanRift.Grassroots.Framework.Data
{
	public interface ICauseRepository
	{
		IQueryable<Cause> FindAllCauses();
		IQueryable<Cause> FindActiveCauses();
		IQueryable<Cause> FindCausesByCauseTemplateID(int causeTemplateID);
		Cause GetCauseByID( int id );
		void Add(Cause cause);
		void AddNote( CauseNote note );
		void Delete(Cause cause);
		void Save();
	}
}
