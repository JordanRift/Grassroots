//
// Copyright © 2011 Jordan Rift, LLC - All Rights Reserved
//
// THIS WORK IS LICENSED UNDER A CREATIVE COMMONS ATTRIBUTION-NONCOMMERCIAL-
// SHAREALIKE 3.0 UNPORTED LICENSE:
// http://creativecommons.org/licenses/by-nc-sa/3.0/
//

using System;
using System.Collections.Generic;
using System.Linq;
using JordanRift.Grassroots.Framework.Data;
using JordanRift.Grassroots.Framework.Entities.Models;
using JordanRift.Grassroots.Tests.Helpers;

namespace JordanRift.Grassroots.Tests.Fakes
{
    [Obsolete("This class will be obsolete in future versions in favor of using Rhino Mocks. See DonateControllerTests for example of new pattern.")]
	public class FakeCauseTemplateRepository : ICauseTemplateRepository
	{
		private static IList<CauseTemplate> causeTemplates;

		public void SetUpRepository()
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
	}
}
