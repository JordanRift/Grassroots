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
using System.Web.Mvc;
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
            Mapper.CreateMap<CauseTemplate, CauseTemplateDetailsModel>();
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
            var result = controller.CauseDetails();
            Assert.IsInstanceOf(typeof(RedirectToRouteResult), result);
        }

        [Test]
        public void CauseDetails_Should_Return_View_If_Cause_Found()
        {
            var result = controller.CauseDetails(1, "abc");
            Assert.IsInstanceOf(typeof(ViewResult), result);
        }

        [Test]
        public void List_Should_Return_View()
        {
            var result = controller.List();
            Assert.IsInstanceOf(typeof(ViewResult), result);
        }

        [Test]
        public void Create_Should_Return_View()
        {
            var result = controller.Create();
            Assert.IsInstanceOf(typeof(ViewResult), result);
        }

        [Test]
        public void New_Should_Return_Redirect_If_ModelState_Is_Invalid()
        {
            controller.ModelState.AddModelError("", "Uh oh");
            var result = controller.New(new CauseTemplateDetailsModel());
            Assert.IsInstanceOf(typeof(RedirectToRouteResult), result);
            var model = controller.TempData["CauseTemplateCreateModel"];
            Assert.IsNotNull(model);
        }

        [Test]
        public void New_Should_Return_Redirect_If_CauseTemlate_Created()
        {
            var causeTemplate = EntityHelpers.GetValidCauseTemplate();
            var model = Mapper.Map<CauseTemplate, CauseTemplateDetailsModel>(causeTemplate);
            var result = controller.New(model);
            Assert.IsInstanceOf(typeof(RedirectToRouteResult), result);
            var responseModel = controller.TempData["CauseTemplateCreateModel"];
            Assert.IsNull(responseModel);
        }

        [Test]
        public void Edit_Should_Return_NotFound_If_CauseTemplate_Not_Found()
        {
            var result = controller.Edit(-1);
            Assert.IsInstanceOf(typeof(HttpNotFoundResult), result);
        }

        [Test]
        public void Edit_Should_Return_View_If_CauseTemplate_Found()
        {
            var result = controller.Edit(1);
            Assert.IsInstanceOf(typeof(ViewResult), result);
        }

        [Test]
        public void Update_Should_Return_NotFound_If_CauseTemplate_Not_Found()
        {
            var result = controller.Update(new CauseTemplateDetailsModel { CauseTemplateID = -1 });
            Assert.IsInstanceOf(typeof(HttpNotFoundResult), result);
        }

        [Test]
        public void Update_Should_Return_Redirect_If_ModelState_Is_Invalid()
        {
            controller.ModelState.AddModelError("", "Uh oh");
            var result = controller.Update(new CauseTemplateDetailsModel { CauseTemplateID = 1 });
            Assert.IsInstanceOf(typeof(RedirectToRouteResult), result);
            var model = controller.TempData["CauseTemplateDetailsModel"];
            Assert.IsNotNull(model);
        }

        [Test]
        public void Update_Should_Return_Redirect_If_Successful()
        {
            var causeTemplate = EntityHelpers.GetValidCauseTemplate();
            causeTemplateRepository.Add(causeTemplate);
            var model = Mapper.Map<CauseTemplate, CauseTemplateDetailsModel>(causeTemplate);
            var result = controller.Update(model);
            Assert.IsInstanceOf(typeof(RedirectToRouteResult), result);
            var message = controller.TempData["UserFeedback"];
            Assert.AreEqual("Your changes have been saved. Please allow a few minutes for them to take effect.", message);
        }

        //[Test]
        //public void Destroy_Should_Return_Json_If_Ajax_Delete_Successful()
        //{
        //    var mocks = new MockRepository();
        //    SetUpController(mocks);
        //    controller.Request.Stub(x => x["X-Requested-With"]).Return("XMLHttpRequest");
        //    var donation = EntityHelpers.GetValidCampaignDonor();
        //    campaignDonorRepository.Add(donation);
        //    var result = controller.Destroy(donation.CampaignDonorID);
        //    Assert.IsInstanceOf<JsonResult>(result);
        //}

        [Test]
        public void Destory_Should_return_Json_If_Ajax_Delete_Successful()
        {
            controller.Request.Stub(x => x["X-Requested-With"]).Return("XMLHttpRequest");
            var causeTemplate = EntityHelpers.GetValidCauseTemplate();
            causeTemplateRepository.Add(causeTemplate);
            var result = controller.Destroy(causeTemplate.CauseTemplateID);
            Assert.IsInstanceOf<JsonResult>(result);
        }

        [Test]
        public void Destroy_Should_Return_Redirect_If_Delete_Successful()
        {
            var causeTemplate = EntityHelpers.GetValidCauseTemplate();
            causeTemplateRepository.Add(causeTemplate);
            var result = controller.Destroy(causeTemplate.CauseTemplateID);
            Assert.IsInstanceOf<RedirectToRouteResult>(result);
        }

        [Test]
        public void Destroy_Should_Remove_CauseTemplate_If_Found()
        {
            var causeTemplate = EntityHelpers.GetValidCauseTemplate();
            causeTemplateRepository.Add(causeTemplate);
            var id = causeTemplate.CauseTemplateID;
            controller.Destroy(id);
            causeTemplate = causeTemplateRepository.GetCauseTemplateByID(id);
            Assert.IsNull(causeTemplate);
        }

        [Test]
        public void Destroy_Should_Return_NotFound_If_CauseTemplate_Not_Found()
        {
            var result = controller.Destroy();
            Assert.IsInstanceOf<HttpNotFoundResult>(result);
        }
        
        /// TODO: Dig a little deeper into the SaveFiles method to test some more use cases...
        /// * What if a single upload fails
        /// * What if 1 of 3 fails
        /// * What if 3 of 3 fails
        /// * etc...

        private CauseTemplateController GetController(bool isAuthenticated = false, bool postFiles = true)
        {
            organizationRepository = new FakeOrganizationRepository();
            causeTemplateRepository = new FakeCauseTemplateRepository();
            
            causeRepository = mocks.DynamicMock<ICauseRepository>();
            Expect.Call(causeRepository.GetCauseByCauseTemplateIdAndReferenceNumber(-1, "")).IgnoreArguments()
                .Return(EntityHelpers.GetValidCause());

            var causeTemplateController = new CauseTemplateController(causeTemplateRepository, causeRepository);
            organization = organizationRepository.GetDefaultOrganization();
            organization.CauseTemplates = new List<CauseTemplate> { EntityHelpers.GetValidCauseTemplate() };

            TestHelpers.MockHttpContext(causeTemplateController, mocks, isAuthenticated, postFiles);
            return causeTemplateController;
        }
    }
}
