//
// Copyright © 2011 Jordan Rift, LLC - All Rights Reserved
//
// THIS WORK IS LICENSED UNDER A CREATIVE COMMONS ATTRIBUTION-NONCOMMERCIAL-
// SHAREALIKE 3.0 UNPORTED LICENSE:
// http://creativecommons.org/licenses/by-nc-sa/3.0/
//

using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using JordanRift.Grassroots.Framework.Data;
using JordanRift.Grassroots.Framework.Entities.Models;
using JordanRift.Grassroots.Framework.Helpers;
using JordanRift.Grassroots.Tests.Helpers;
using NUnit.Framework;

namespace JordanRift.Grassroots.Tests.IntegrationTests.Models
{
    [TestFixture]
    public class CauseTemplateRepositoryTests
    {
        private ICauseTemplateRepository causeTemplaterepository;
        private IOrganizationRepository organizationRepository;

        private CauseTemplate causeTemplate;
        private Organization organization;

        [SetUp]
        public void SetUp()
        {
            causeTemplaterepository = new CauseTemplateRepository();
            organizationRepository = new OrganizationRepository();
        }

        [Test]
        public void Add_Should_Add_CauseTemplate_To_Database()
        {
            using (new UnitOfWorkScope())
            using (new TransactionScope())
            {
                ArrangeCauseTemplateTest();
                var id = causeTemplate.CauseTemplateID;
                Assert.Greater(id, 0);
            }
        }

        [Test]
        public void FindAllCauseTemplates_Should_Return_List()
        {
            using (new UnitOfWorkScope())
            using (new TransactionScope())
            {
                ArrangeCauseTemplateTest();
                var results = causeTemplaterepository.FindAllCauseTemplates();
                Assert.IsNotNull(results);
                Assert.IsNotEmpty(results.ToList());
            }
        }

        [Test]
        public void FindActiveCauseTemplates_Should_Return_List()
        {
            using (new UnitOfWorkScope())
            using (new TransactionScope())
            {
                ArrangeCauseTemplateTest();
                var results = causeTemplaterepository.FindActiveCauseTemplates();
                Assert.IsNotNull(results);
                Assert.IsNotEmpty(results.ToList());
            }
        }

        [Test]
        public void GetCauseTemplateByID_Should_Load_CauseTemplate_From_Database()
        {
            using (new UnitOfWorkScope())
            using (new TransactionScope())
            {
                ArrangeCauseTemplateTest();
                var id = causeTemplate.CauseTemplateID;
                causeTemplate = null;
                causeTemplate = causeTemplaterepository.GetCauseTemplateByID(id);
                Assert.IsNotNull(causeTemplate);
                Assert.AreEqual(id, causeTemplate.CauseTemplateID);
            }
        }

        [Test]
        public void GetCauseTemplateByID_Should_Return_Null_When_CauseTemplateID_Not_Found()
        {
            using (new UnitOfWorkScope())
            using (new TransactionScope())
            {
                ArrangeCauseTemplateTest();
                var id = causeTemplate.CauseTemplateID + 1;
                var result = causeTemplaterepository.GetCauseTemplateByID(id);
                Assert.IsNull(result);
            }
        }

        [Test]
        public void Delete_Should_Delete_CauseTemplate_From_Database()
        {
            using (new UnitOfWorkScope())
            using (new TransactionScope())
            {
                ArrangeCauseTemplateTest();
                var id = causeTemplate.CauseTemplateID;
                causeTemplaterepository.Delete(causeTemplate);
                causeTemplaterepository.Save();
                causeTemplate = causeTemplaterepository.GetCauseTemplateByID(id);
                Assert.IsNull(causeTemplate);
            }
        }

        private void ArrangeCauseTemplateTest()
        {
            organization = EntityHelpers.GetValidOrganization();
            organization.CauseTemplates = new List<CauseTemplate>();
            organizationRepository.Add(organization);
            organizationRepository.Save();

            causeTemplate = EntityHelpers.GetValidCauseTemplate();
            organization.CauseTemplates.Add(causeTemplate);
            causeTemplaterepository.Save();
        }
    }
}
