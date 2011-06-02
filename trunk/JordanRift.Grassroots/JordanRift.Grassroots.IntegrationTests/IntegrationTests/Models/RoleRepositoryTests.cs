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
using JordanRift.Grassroots.Framework.Entities.Models;
using JordanRift.Grassroots.Framework.Helpers;
using JordanRift.Grassroots.Tests.Helpers;
using NUnit.Framework;

namespace JordanRift.Grassroots.IntegrationTests.IntegrationTests.Models
{
    [TestFixture]
    public class RoleRepositoryTests
    {
        private IRoleRepository roleRepository;
        private IOrganizationRepository organizationRepository;

        private Role role;
        private Organization organization;

        [SetUp]
        public void SetUp()
        {
            roleRepository = new RoleRepository();
            organizationRepository = new OrganizationRepository();
        }

        [Test]
        public void Add_Should_Add_Role_To_Database()
        {
            using (new UnitOfWorkScope())
            using (new TransactionScope())
            {
                ArrangeRoleTest();
                var id = role.RoleID;
                Assert.Greater(id, 0);
            }
        }

        [Test]
        public void GetRoleByID_Should_Load_Role_From_Database()
        {
            using (new UnitOfWorkScope())
            using (new TransactionScope())
            {
                ArrangeRoleTest();
                var id = role.RoleID;
                role = null;
                role = roleRepository.GetRoleByID(id);
                Assert.IsNotNull(role);
                Assert.AreEqual(id, role.RoleID);
            }
        }

        [Test]
        public void GetRoleByID_Should_Return_Null_When_RoleID_Not_Found()
        {
            using (new UnitOfWorkScope())
            using (new TransactionScope())
            {
                ArrangeRoleTest();
                var id = role.RoleID + 1;
                role = roleRepository.GetRoleByID(id);
                Assert.IsNull(role);
            }
        }

        [Test]
        public void Delete_Should_Delete_Role_From_Database()
        {
            using (new UnitOfWorkScope())
            using (new TransactionScope())
            {
                ArrangeRoleTest();
                var id = role.RoleID;
                roleRepository.Delete(role);
                roleRepository.Save();
                var result = roleRepository.GetRoleByID(id);
                Assert.IsNull(result);
            }
        }

        private void ArrangeRoleTest()
        {
            organization = EntityHelpers.GetValidOrganization();
            organization.Roles = new List<Role>();
            organizationRepository.Add(organization);
            organizationRepository.Save();

            role = EntityHelpers.GetValidRole();
            organization.Roles.Add(role);
            roleRepository.Save();
        }
    }
}
