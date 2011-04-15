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
using JordanRift.Grassroots.Framework.Entities;
using JordanRift.Grassroots.Framework.Entities.Models;
using JordanRift.Grassroots.Tests.Helpers;

namespace JordanRift.Grassroots.Tests.Fakes
{
    [Obsolete("This class will be obsolete in future versions in favor of using Rhino Mocks. See DonateControllerTests for example of new pattern.")]
    public class FakeOrganizationRepository : IOrganizationRepository
    {
        private static IList<Organization> organizations;

        public void SetUpRepository()
        {
            organizations = new List<Organization>();

            for (int i = 0; i < 1; i++)
            {
                var org = EntityHelpers.GetValidOrganization();
                org.OrganizationID = i + 1;
                organizations.Add(org);
                AddSettings(org);
            }
        }

        private void AddSettings(Organization org)
        {
            for (int i = 0; i < 5; i++)
            {
                org.OrganizationSettings.Add(new OrganizationSetting(
                    string.Format("setting{0}", i), "this is the value", DataType.STRING));
            }
        }

        public Organization GetOrganizationByID(int id)
        {
            return organizations.FirstOrDefault(o => o.OrganizationID == id);
        }

        public Organization GetDefaultOrganization()
        {
            return organizations.FirstOrDefault();
        }

        public void Add(Organization organization)
        {
            organization.OrganizationID = organizations.Count + 1;
            organizations.Add(organization);
        }

        public void Delete(Organization organization)
        {
            organizations.Remove(organization);
        }

        public void Save()
        {
        }
    }
}
