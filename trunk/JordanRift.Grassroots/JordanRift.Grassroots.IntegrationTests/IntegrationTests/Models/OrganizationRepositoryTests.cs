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
using System.Transactions;
using JordanRift.Grassroots.Framework.Data;
using JordanRift.Grassroots.Framework.Entities;
using JordanRift.Grassroots.Framework.Entities.Models;
using JordanRift.Grassroots.Tests.Helpers;
using NUnit.Framework;

namespace JordanRift.Grassroots.IntegrationTests.IntegrationTests.Models
{
    [TestFixture]
    public class OrganizationRepositoryTests
    {
        private IOrganizationRepository repository;
        private Organization organization;
        private OrganizationSetting organizationSetting;

        [SetUp]
        public void SetUp()
        {
            repository = new OrganizationRepository();
        }

        [Test]
        public void Add_Should_Add_Organization_To_Database()
        {
            using (new TransactionScope())
            {
                ArrangeOrganizationTest();
                Assert.Greater(organization.OrganizationID, 0);
            }
        }

        [Test]
        public void GetOrganizationByID_Should_Load_Organization_From_Database()
        {
            using (new TransactionScope())
            {
                ArrangeOrganizationTest();
                var id = organization.OrganizationID;
                organization = null;
                organization = repository.GetOrganizationByID(id) as Organization;
                Assert.IsNotNull(organization);
                Assert.AreEqual(organization.OrganizationID, id);
            }
        }

        [Test]
        public void GetOrganizationByID_Should_Return_Null_When_OrganizationID_Not_Found()
        {
            using (new TransactionScope())
            {
                ArrangeOrganizationTest();
                var id = organization.OrganizationID + 1;
                var result = repository.GetOrganizationByID(id);
                Assert.IsNull(result);
            }
        }

        [Test]
        public void GetDefaultOrganiation_Should_Load_Organization_From_Database()
        {
            using (new TransactionScope())
            {
                ArrangeOrganizationTest();
                var id = organization.OrganizationID;
                var result = repository.GetDefaultOrganization();
                Assert.IsNotNull(result);
                Assert.Greater(id, 0);
            }
        }

        [Test]
        public void Delete_Should_Delete_Organization_From_Database()
        {
            using (new TransactionScope())
            {
                ArrangeOrganizationTest();
                var id = organization.OrganizationID;
                repository.Delete(organization);
                repository.Save();
                organization = repository.GetOrganizationByID(id) as Organization;
                Assert.IsNull(organization);
            }
        }

        [Test]
        public void GetSetting_Should_Return_Valid_OrganizationSetting()
        {
            using (new TransactionScope())
            {
                ArrangeOrganizationTest();
                var setting = organization.GetSetting("the_key");
                Assert.AreEqual(organizationSetting.OrganizationSettingID, setting.OrganizationSettingID);
            }
        }

        private void ArrangeOrganizationTest()
        {
            organization = EntityHelpers.GetValidOrganization() as Organization;
            organization.OrganizationSettings = new List<OrganizationSetting>();
            repository.Add(organization);
            repository.Save();

            organizationSetting = new OrganizationSetting("the_key", "the_value", DataType.String);
            organization.OrganizationSettings.Add(organizationSetting);
            repository.Save();
        }
    }
}
