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
