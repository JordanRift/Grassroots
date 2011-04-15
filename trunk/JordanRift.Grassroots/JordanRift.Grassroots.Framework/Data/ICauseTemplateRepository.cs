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
    public interface ICauseTemplateRepository
    {
		IQueryable<CauseTemplate> FindAllCauseTemplates();
		IQueryable<CauseTemplate> FindActiveCauseTemplates();
		CauseTemplate GetCauseTemplateByID( int id );
		void Add(CauseTemplate causeTemplate);
		void Delete(CauseTemplate causeTemplate);
        void Save();
    }
}
