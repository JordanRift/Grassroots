//
// Copyright © 2011 Jordan Rift, LLC - All Rights Reserved
//
// THIS WORK IS LICENSED UNDER A CREATIVE COMMONS ATTRIBUTION-NONCOMMERCIAL-
// SHAREALIKE 3.0 UNPORTED LICENSE:
// http://creativecommons.org/licenses/by-nc-sa/3.0/
//

using System.Collections.Generic;
using System.Transactions;
using JordanRift.Grassroots.Framework.Data;
using JordanRift.Grassroots.Framework.Entities.Models;
using JordanRift.Grassroots.Framework.Helpers;
using JordanRift.Grassroots.Tests.Helpers;
using NUnit.Framework;

namespace JordanRift.Grassroots.Tests.IntegrationTests.Models
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
