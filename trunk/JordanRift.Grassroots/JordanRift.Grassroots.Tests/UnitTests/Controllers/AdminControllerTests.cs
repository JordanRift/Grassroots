//
// Copyright © 2011 Jordan Rift, LLC - All Rights Reserved
//
// THIS WORK IS LICENSED UNDER A CREATIVE COMMONS ATTRIBUTION-NONCOMMERCIAL-
// SHAREALIKE 3.0 UNPORTED LICENSE:
// http://creativecommons.org/licenses/by-nc-sa/3.0/
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
            mocks.VerifyAll();
        }

        [Test]
        public void UpdateOrganization_Should_Redirect_To_Index_When_Successful()
        {
            var organization = EntityHelpers.GetValidOrganization();
            var viewModel = Mapper.Map<Organization, OrganizationDetailsModel>(organization);
            var mocks = new MockRepository();
            SetUpAdminController(mocks);
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

        private void SetUpAdminController(MockRepository mocks, bool shouldFindOrganization = true)
        {
            organizationRepository = mocks.DynamicMock<IOrganizationRepository>();
            Expect.Call(organizationRepository.GetDefaultOrganization())
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
