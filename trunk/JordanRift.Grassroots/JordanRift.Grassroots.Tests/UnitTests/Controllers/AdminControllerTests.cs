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

using System.Web.Mvc;
using System.Web.Routing;
using AutoMapper;
using JordanRift.Grassroots.Framework.Data;
using JordanRift.Grassroots.Framework.Entities.Models;
using JordanRift.Grassroots.Tests.Fakes;
using JordanRift.Grassroots.Tests.Helpers;
using JordanRift.Grassroots.Web.Controllers;
using JordanRift.Grassroots.Web.Models;
using NUnit.Framework;
using Rhino.Mocks;

namespace JordanRift.Grassroots.Tests.UnitTests.Controllers
{
    [TestFixture]
    public class AdminControllerTests
    {
        private IOrganizationRepository organizationRepository;
        private AdminController controller;

        [SetUp]
        public void SetUp()
        {
            Mapper.CreateMap<Organization, OrganizationDetailsModel>();
        }


       [Test]
        public void Index_Should_Return_View()
        {
            var mocks = new MockRepository();
            SetUpAdminController(mocks);
            var result = controller.Index();
            Assert.IsInstanceOf(typeof(ViewResult), result);
        }

        [Test]
        public void EditOrganization_Should_Return_View_When_Organization_Found()
        {
            var mocks = new MockRepository();
            SetUpAdminController(mocks);
            mocks.ReplayAll();
            var result = controller.EditOrganization();
            Assert.IsInstanceOf(typeof(ViewResult), result);
            mocks.VerifyAll();
        }

        [Test]
        public void EditOrganization_Should_Return_NotFound_When_Organization_Not_Found()
        {
            var mocks = new MockRepository();
            SetUpAdminController(mocks, false);
            mocks.ReplayAll();
            var result = controller.EditOrganization();
            Assert.IsInstanceOf(typeof(HttpNotFoundResult), result);
        }

        [Test]
        public void UpdateOrganization_Should_Redirect_To_Index_When_Successful()
        {
            var organization = EntityHelpers.GetValidOrganization();
            var viewModel = Mapper.Map<Organization, OrganizationDetailsModel>(organization);
            var mocks = new MockRepository();
            SetUpAdminController(mocks, repoReadOnly: false);
            mocks.ReplayAll();
            var result = controller.UpdateOrganization(viewModel);
            Assert.IsInstanceOf(typeof(RedirectToRouteResult), result);
            var actionName = ((RedirectToRouteResult) result).RouteValues["Action"];
            Assert.AreEqual("Index", actionName);
            mocks.VerifyAll();
        }

        [Test]
        public void UpdateOrganization_Should_Redirect_To_EditOrganization_When_ModelState_Is_Invalid()
        {
            var organization = EntityHelpers.GetValidOrganization();
            var viewModel = Mapper.Map<Organization, OrganizationDetailsModel>(organization);
            var mocks = new MockRepository();
            SetUpAdminController(mocks);
            controller.ModelState.AddModelError("", "Uh oh...");
            mocks.ReplayAll();
            var result = controller.UpdateOrganization(viewModel);
            Assert.IsInstanceOf(typeof(RedirectToRouteResult), result);
            var actionName = ((RedirectToRouteResult) result).RouteValues["Action"];
            Assert.AreEqual("EditOrganization", actionName);
        }

        private void SetUpAdminController(MockRepository mocks, bool shouldFindOrganization = true, bool repoReadOnly = true)
        {
            organizationRepository = mocks.DynamicMock<IOrganizationRepository>();
            Expect.Call(organizationRepository.GetDefaultOrganization(readOnly: repoReadOnly))
                .Return(shouldFindOrganization ? EntityHelpers.GetValidOrganization() : null);

            controller = new AdminController
                             {
                                 OrganizationRepository = organizationRepository
                             };

            controller.ControllerContext = new ControllerContext
                                               {
                                                   Controller = controller,
                                                   RequestContext = new RequestContext(new MockHttpContext(), new RouteData())
                                               };
        }
    }
}
