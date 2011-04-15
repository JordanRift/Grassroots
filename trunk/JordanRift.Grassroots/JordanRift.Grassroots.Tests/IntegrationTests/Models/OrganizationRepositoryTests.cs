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
using JordanRift.Grassroots.Framework.Entities;
using JordanRift.Grassroots.Framework.Entities.Models;
using JordanRift.Grassroots.Tests.Helpers;
using NUnit.Framework;

namespace JordanRift.Grassroots.Tests.IntegrationTests.Models
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
                organization = repository.GetOrganizationByID(id);
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
                organization = repository.GetOrganizationByID(id);
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
            organization = EntityHelpers.GetValidOrganization();
            organization.OrganizationSettings = new List<OrganizationSetting>();
            repository.Add(organization);
            repository.Save();

            organizationSetting = new OrganizationSetting("the_key", "the_value", DataType.STRING);
            organization.OrganizationSettings.Add(organizationSetting);
            repository.Save();
        }
    }
}
