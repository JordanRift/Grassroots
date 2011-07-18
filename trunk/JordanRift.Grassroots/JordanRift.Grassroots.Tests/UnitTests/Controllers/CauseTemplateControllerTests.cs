﻿//
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
using System.Web.Mvc;
using JordanRift.Grassroots.Framework.Data;
using JordanRift.Grassroots.Framework.Entities.Models;
using JordanRift.Grassroots.Tests.Fakes;
using JordanRift.Grassroots.Tests.Helpers;
using JordanRift.Grassroots.Web.Controllers;
using NUnit.Framework;
using Rhino.Mocks;

namespace JordanRift.Grassroots.Tests.UnitTests.Controllers
{
    [TestFixture]
    public class CauseTemplateControllerTests
    {
        private CauseTemplateController controller;
        private ICauseTemplateRepository causeTemplateRepository;
        private IOrganizationRepository organizationRepository;
        private ICauseRepository causeRepository;
        private Organization organization;
        private MockRepository mocks;

        [SetUp]
        public void SetUp()
        {
            mocks = new MockRepository();
            controller = GetController();
        }

        [TearDown]
        public void TearDown()
        {
            FakeCauseTemplateRepository.Reset();
        }

        [Test]
        public void Index_Should_Return_View()
        {
            var result = controller.Index();
            Assert.IsInstanceOf(typeof(ViewResult), result);
        }

        [Test]
        public void Index_Should_Return_Details_View_If_Single_CauseTemplate_Found()
        {
            var result = controller.Index();
            Assert.IsInstanceOf(typeof(ViewResult), result);
            var viewResult = result as ViewResult;
            Assert.AreEqual("Details", viewResult.ViewName);
        }

        [Test]
        public void Index_Should_Return_Index_View_If_Multiple_CauseTemplates_Found()
        {
            organization.CauseTemplates.Add(EntityHelpers.GetValidCauseTemplate());
            var result = controller.Index();
            Assert.IsInstanceOf(typeof(ViewResult), result);
            var viewResult = result as ViewResult;
            Assert.AreEqual("Index", viewResult.ViewName);
        }

        [Test]
        public void Details_Should_Return_NotFound_If_CauseTemplate_Not_Found()
        {
            var result = controller.Details();
            Assert.IsInstanceOf(typeof(HttpNotFoundResult), result);
        }

        [Test]
        public void Details_Should_Return_View_If_CauseTemplate_Found()
        {
            var result = controller.Details(1);
            Assert.IsInstanceOf(typeof(ViewResult), result);
        }

        [Test]
        public void Search_Should_Return_NotFound_If_CauseTemplate_Not_Found()
        {
            var result = controller.Search();
            Assert.IsInstanceOf(typeof(HttpNotFoundResult), result);
        }

        [Test]
        public void Search_Should_Return_View_If_CauseTemplate_Found()
        {
            var causeTemplate = causeTemplateRepository.GetCauseTemplateByID(1);
            causeTemplate.Causes = new List<Cause>();
            var result = controller.Search(1);
            Assert.IsInstanceOf(typeof(ViewResult), result);
        }

        [Test]
        public void CauseDetails_Should_Return_Redirect_If_Cause_Not_Found()
        {
            mocks.ReplayAll();
            var result = controller.CauseDetails();
            Assert.IsInstanceOf(typeof(RedirectToRouteResult), result);
            mocks.VerifyAll();
        }

        [Test]
        public void CauseDetails_Should_Return_View_If_Cause_Found()
        {
            mocks.ReplayAll();
            var result = controller.CauseDetails(1, "abc");
            Assert.IsInstanceOf(typeof(ViewResult), result);
            mocks.VerifyAll();
        }

        private CauseTemplateController GetController(bool isAuthenticated = false)
        {
            organizationRepository = new FakeOrganizationRepository();
            causeTemplateRepository = new FakeCauseTemplateRepository();
            causeRepository = mocks.DynamicMock<ICauseRepository>();
            Expect.Call(causeRepository.GetCauseByCauseTemplateIdAndReferenceNumber(-1, "")).IgnoreArguments()
                .Return(EntityHelpers.GetValidCause());

            var causeTemplateController = new CauseTemplateController(causeTemplateRepository, causeRepository);
            organization = organizationRepository.GetDefaultOrganization();
            organization.CauseTemplates = new List<CauseTemplate> { EntityHelpers.GetValidCauseTemplate() };

            TestHelpers.MockHttpContext(causeTemplateController);
            return causeTemplateController;
        }
    }
}
