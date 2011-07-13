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

using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using JordanRift.Grassroots.Framework.Data;
using JordanRift.Grassroots.Framework.Entities;
using JordanRift.Grassroots.Framework.Entities.Models;
using JordanRift.Grassroots.Tests.Helpers;

namespace JordanRift.Grassroots.Tests.Fakes
{
    [Export(typeof(IOrganizationRepository))]
    public class FakeOrganizationRepository : IOrganizationRepository
    {
        private static IList<Organization> organizations;
        public PriorityType Priority { get; set; }

        static FakeOrganizationRepository()
        {
            SetUp();
        }

        private static void SetUp()
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

        public FakeOrganizationRepository()
        {
            Priority = PriorityType.High;
        }

        public static void Reset()
        {
            SetUp();
        }

        public static void Clear()
        {
            organizations = new List<Organization>();
        }

        private static void AddSettings(Organization org)
        {
            for (int i = 0; i < 5; i++)
            {
                org.OrganizationSettings.Add(new OrganizationSetting(
                    string.Format("setting{0}", i), "this is the value", DataType.String));
            }
        }

        public Organization GetOrganizationByID(int id)
        {
            return organizations.FirstOrDefault(o => o.OrganizationID == id);
        }

        public Organization GetDefaultOrganization(bool readOnly = true)
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

        public void DeleteSetting(OrganizationSetting organizationSetting)
        {
        }

        public void Save()
        {
        }

        public void Dispose()
        {
        }
    }
}
