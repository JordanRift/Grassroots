//
// Copyright © 2011 Jordan Rift, LLC - All Rights Reserved
//
// THIS WORK IS LICENSED UNDER A CREATIVE COMMONS ATTRIBUTION-NONCOMMERCIAL-
// SHAREALIKE 3.0 UNPORTED LICENSE:
// http://creativecommons.org/licenses/by-nc-sa/3.0/
//

using JordanRift.Grassroots.Framework.Data;
using JordanRift.Grassroots.Framework.Entities.Models;
using JordanRift.Grassroots.Tests.Fakes;
using JordanRift.Grassroots.Tests.Helpers;
using NUnit.Framework;

namespace JordanRift.Grassroots.Tests.UnitTests.Models
{
    [TestFixture]
    public class OrganizationTests
    {
        private IOrganizationRepository repository;
        private Organization organization;

        [SetUp]
        public void SetUp()
        {
            repository = new FakeOrganizationRepository();
            ((FakeOrganizationRepository)repository).SetUpRepository();
            organization = EntityHelpers.GetValidOrganization();
        }

        [Test]
        public void GetDefaultOrganization_Should_Return_Organization()
        {
            var org = repository.GetDefaultOrganization();
            Assert.IsNotNull(org);
            Assert.IsInstanceOf(typeof(Organization), org);
        }

        [Test]
        public void GetSetting_Should_Return_OrganizationSetting_If_Found()
        {
            var setting = organization.GetSetting("setting0");
            Assert.IsNotNull(setting);
            Assert.IsInstanceOf(typeof(OrganizationSetting), setting);
        }

        [Test]
        public void GetSetting_Should_Return_Null_If_Not_Found()
        {
            var setting = organization.GetSetting("george");
            Assert.IsNull(setting);
        }
    }
}
