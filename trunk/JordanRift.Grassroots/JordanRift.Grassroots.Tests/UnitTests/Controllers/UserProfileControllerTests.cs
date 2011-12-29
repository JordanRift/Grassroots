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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using AutoMapper;
using JordanRift.Grassroots.Framework.Entities.Models;
using JordanRift.Grassroots.Tests.Helpers;
using JordanRift.Grassroots.Web.Controllers;
using JordanRift.Grassroots.Framework.Data;
using JordanRift.Grassroots.Tests.Fakes;
using JordanRift.Grassroots.Web.Mailers;
using JordanRift.Grassroots.Web.Models;
using Mvc.Mailer;
using NUnit.Framework;
using Rhino.Mocks;

namespace JordanRift.Grassroots.Tests.UnitTests.Controllers
{
    [TestFixture]
    public class UserProfileControllerTests
    {
        private UserProfileController controller;
        private IUserProfileRepository repository;
        private MockRepository mocks;
        private UserProfile userProfile;

        [SetUp]
        public void SetUp()
        {
            userProfile = EntityHelpers.GetValidUserProfile();
            repository = new FakeUserProfileRepository();
            repository.Add(userProfile);
            mocks = new MockRepository();
            controller = GetUserProfileController(userProfile.UserProfileID);
            Mapper.CreateMap<UserProfile, UserProfileDetailsModel>();
            Mapper.CreateMap<UserProfile, UserProfileAdminModel>();
        }

        [TearDown]
        public void TearDown()
        {
            FakeUserProfileRepository.Reset();
            FakeCauseRepository.Reset();
        }

        [Test]
        public void Index_Should_Return_View_If_UserProfile_Found()
        {
            userProfile.Email = "goodEmail";
            userProfile.Campaigns = new List<Campaign>();
            var result = controller.Index();
            Assert.IsInstanceOf(typeof(ViewResult), result);
        }

        [Test]
        public void Index_Should_Return_NotFound_If_UserProfile_Not_Found()
        {
            var result = controller.Index();
            Assert.IsInstanceOf(typeof(HttpNotFoundResult), result);
        }

        [Test]
        public void Edit_Should_Return_View_When_UserProfile_Found()
        {
            repository.Add(new UserProfile { Email = "goodEmail" });
            var result = controller.Edit();
            Assert.IsInstanceOf(typeof(ViewResult), result);
        }

        [Test]
        public void Edit_Should_Return_NotFound_When_UserProfile_Not_Found()
        {
            var result = controller.Edit();
            Assert.IsInstanceOf(typeof(HttpNotFoundResult), result);
        }

        [Test]
        public void Update_Should_Redirect_To_Index_If_Successful()
        {
            userProfile.Email = "goodEmail";
            var viewModel = Mapper.Map<UserProfile, UserProfileDetailsModel>(userProfile);
            var result = controller.Update(viewModel);
            Assert.IsInstanceOf(typeof(RedirectToRouteResult), result);
            Assert.AreEqual("Index", ((RedirectToRouteResult)result).RouteValues["Action"]);
        }

        [Test]
        public void Update_Should_Redirect_To_Edit_If_Model_Is_Not_Valid()
        {
            userProfile.Email = "goodEmail";
            var viewModel = Mapper.Map<UserProfile, UserProfileDetailsModel>(userProfile);
            controller.ModelState.AddModelError("", "Dummy error message.");
            var result = controller.Update(viewModel);
            Assert.IsInstanceOf(typeof(RedirectToRouteResult), result);
            Assert.AreEqual("Edit", ((RedirectToRouteResult)result).RouteValues["Action"]);
        }

        [Test]
        public void Update_Should_Return_Not_Found_If_UserProfile_Not_Found()
        {
            var viewModel = Mapper.Map<UserProfile, UserProfileDetailsModel>(EntityHelpers.GetValidUserProfile());
            var result = controller.Update(viewModel);
            Assert.IsInstanceOf(typeof(HttpNotFoundResult), result);
        }

        [Test]
        public void Map_Should_Update_FirstName()
        {
            userProfile.Email = "goodEmail";
            var viewModel = Mapper.Map<UserProfile, UserProfileDetailsModel>(userProfile);
            viewModel.FirstName = "NewFirstName";
            controller.Update(viewModel);
            Assert.AreEqual(userProfile.FirstName, viewModel.FirstName);
        }

        [Test]
        public void Map_Should_Update_LastName()
        {
            userProfile.Email = "goodEmail";
            var viewModel = Mapper.Map<UserProfile, UserProfileDetailsModel>(userProfile);
            viewModel.LastName = "NewLasttName";
            controller.Update(viewModel);
            Assert.AreEqual(userProfile.LastName, viewModel.LastName);
        }

        [Test]
        public void Map_Should_Update_Birthdate()
        {
            userProfile.Email = "goodEmail";
            var viewModel = Mapper.Map<UserProfile, UserProfileDetailsModel>(userProfile);
            viewModel.Birthdate = new DateTime(1981, 12, 1);
            controller.Update(viewModel);
            Assert.AreEqual(userProfile.Birthdate, viewModel.Birthdate);
        }

        [Test]
        public void Map_Should_Update_PrimaryPhone()
        {
            userProfile.Email = "goodEmail";
            var viewModel = Mapper.Map<UserProfile, UserProfileDetailsModel>(userProfile);
            viewModel.PrimaryPhone = "602-555-7777";
            controller.Update(viewModel);
            Assert.AreEqual(userProfile.PrimaryPhone, viewModel.PrimaryPhone);
        }

        [Test]
        public void Map_Should_Update_Gender()
        {
            userProfile.Email = "goodEmail";
            var viewModel = Mapper.Map<UserProfile, UserProfileDetailsModel>(userProfile);
            viewModel.Gender = "female";
            controller.Update(viewModel);
            Assert.AreEqual(userProfile.Gender, viewModel.Gender);
        }

        [Test]
        public void Map_Should_Update_AddressLine1()
        {
            userProfile.Email = "goodEmail";
            var viewModel = Mapper.Map<UserProfile, UserProfileDetailsModel>(userProfile);
            viewModel.AddressLine1 = "New Street Address";
            controller.Update(viewModel);
            Assert.AreEqual(userProfile.AddressLine1, viewModel.AddressLine1);
        }

        [Test]
        public void Map_Should_Update_AddressLine2()
        {
            userProfile.Email = "goodEmail";
            var viewModel = Mapper.Map<UserProfile, UserProfileDetailsModel>(userProfile);
            viewModel.AddressLine2 = "New Address Line 2";
            controller.Update(viewModel);
            Assert.AreEqual(userProfile.AddressLine2, viewModel.AddressLine2);
        }

        [Test]
        public void Map_Should_Update_City()
        {
            userProfile.Email = "goodEmail";
            var viewModel = Mapper.Map<UserProfile, UserProfileDetailsModel>(userProfile);
            viewModel.City = "New City";
            controller.Update(viewModel);
            Assert.AreEqual(userProfile.City, viewModel.City);
        }

        [Test]
        public void Map_Should_Update_State()
        {
            userProfile.Email = "goodEmail";
            repository.Add(userProfile);
            var viewModel = Mapper.Map<UserProfile, UserProfileDetailsModel>(userProfile);
            viewModel.State = "Alabama";
            controller.Update(viewModel);
            Assert.AreEqual(userProfile.State, viewModel.State);
        }

        [Test]
        public void Map_Should_Update_Zip()
        {
            userProfile.Email = "goodEmail";
            var viewModel = Mapper.Map<UserProfile, UserProfileDetailsModel>(userProfile);
            viewModel.ZipCode = "85310";
            controller.Update(viewModel);
            Assert.AreEqual(userProfile.ZipCode, viewModel.ZipCode);
        }
        
        [Test]
        public void DeactivateAccount_Should_Return_Not_Found_If_Email_Not_Found()
        {
            userProfile.Email = "badEmail";
            var result = controller.DeactivateAccount();
            Assert.IsInstanceOf(typeof(HttpNotFoundResult), result);
        }

        [Test]
        public void DeactivateAccount_Should_Return_View_If_Email_Found()
        {
            userProfile.Email = "goodEmail";
            var result = controller.DeactivateAccount();
            Assert.IsInstanceOf(typeof(ViewResult), result);
        }

        [Test]
        public void Deactivate_Should_Redirect_To_LogOff_If_Successful()
        {
            userProfile.Email = "goodEmail";
            userProfile.Organization = EntityHelpers.GetValidOrganization() as Organization;
            var result = controller.Deactivate();
            Assert.IsInstanceOf(typeof(RedirectToRouteResult), result);
            var action = ((RedirectToRouteResult) result).RouteValues["Action"];
            Assert.AreEqual("LogOff", action);
        }

        [Test]
        public void Deactivate_Should_Set_Active_To_False_If_Successful()
        {
            userProfile.Email = "goodEmail";
            userProfile.Organization = EntityHelpers.GetValidOrganization() as Organization;
            Assert.IsTrue(userProfile.Active);
            controller.Deactivate();
            Assert.IsFalse(userProfile.Active);
        }

        [Test]
        public void Deactivate_Should_Return_Not_Found_If_UserProfile_Not_Found()
        {
            var result = controller.Deactivate();
            Assert.IsInstanceOf(typeof(HttpNotFoundResult), result);
        }

        [Test]
        public void ReactivateAccount_Should_Return_Not_Found_If_Email_Not_Found()
        {
            userProfile.Email = "badEmail";
            var result = controller.ReactivateAccount();
            Assert.IsInstanceOf(typeof(HttpNotFoundResult), result);
        }

        [Test]
        public void ReactivateAccount_Should_Return_View_If_Email_Found()
        {
            userProfile.Email = "goodEmail";
            var result = controller.ReactivateAccount();
            Assert.IsInstanceOf(typeof(ViewResult), result);
        }

        [Test]
        public void Reactivate_Should_Redirect_To_Index_If_Successful()
        {
            userProfile.Email = "goodEmail";
            var result = controller.Reactivate();
            Assert.IsInstanceOf(typeof(RedirectToRouteResult), result);
            var action = ((RedirectToRouteResult) result).RouteValues["Action"];
            Assert.AreEqual("Index", action);
        }

        [Test]
        public void Reactivate_Should_Set_Active_To_True_If_Successful()
        {
            userProfile.Email = "goodEmail";
            userProfile.Active = false;
            controller.Reactivate();
            Assert.IsTrue(userProfile.Active);
        }

        [Test]
        public void Reactivate_Should_Return_Not_Found_If_UserProfile_Not_Found()
        {
            var result = controller.Reactivate();
            Assert.IsInstanceOf(typeof (HttpNotFoundResult), result);
        }

        [Test]
        public void List_Should_Return_Donate_Grid_View()
        {
            var result = controller.List();
            Assert.IsInstanceOf<ViewResult>(result);
        }

        [Test]
        public void List_Should_Return_Populated_Donate_Grid_View()
        {
            var result = controller.List() as ViewResult;
            var model = result.Model as IEnumerable<UserProfileAdminModel>;
            Assert.Greater(model.Count(), 0);
        }

        [Test]
        public void Admin_Should_Return_View_If_UserProfile_Found()
        {
            userProfile = EntityHelpers.GetValidUserProfile();
            repository.Add(userProfile);
            var result = controller.Admin(userProfile.UserProfileID);
            Assert.IsInstanceOf<ViewResult>(result);
        }

        [Test]
        public void Admin_Should_Return_NotFound_If_UserProfile_Not_Found()
        {
            var result = controller.Admin();
            Assert.IsInstanceOf<HttpNotFoundResult>(result);
        }

        [Test]
        public void Destroy_Should_Return_Json_If_Ajax_Delete_Successful()
        {
            controller.Request.Stub(x => x["X-Requested-With"]).Return("XMLHttpRequest");
            userProfile = EntityHelpers.GetValidUserProfile();
            repository.Add(userProfile);
            var result = controller.Destroy(userProfile.UserProfileID);
            Assert.IsInstanceOf<JsonResult>(result);
        }

        [Test]
        public void Destroy_Should_Return_Redirect_If_Delete_Successful()
        {
            userProfile = EntityHelpers.GetValidUserProfile();
            repository.Add(userProfile);
            var result = controller.Destroy(userProfile.UserProfileID);
            Assert.IsInstanceOf<RedirectToRouteResult>(result);
        }

        [Test]
        public void Destroy_Should_Remove_UserProfile_If_Found()
        {
            userProfile = EntityHelpers.GetValidUserProfile();
            repository.Add(userProfile);
            var id = userProfile.UserProfileID;
            controller.Destroy(id);
            userProfile = repository.GetUserProfileByID(id);
            Assert.IsNull(userProfile);
        }

        [Test]
        public void Destroy_Should_Return_NotFound_If_UserProfile_Not_Found()
        {
            var result = controller.Destroy();
            Assert.IsInstanceOf<HttpNotFoundResult>(result);
        }

        [Test]
        public void AdminUpdate_Should_Redirect_To_List_If_Successful()
        {
            userProfile = EntityHelpers.GetValidUserProfile();
            repository.Add(userProfile);
            var model = Mapper.Map<UserProfile, UserProfileAdminModel>(userProfile);
            var result = controller.AdminUpdate(model);
            Assert.IsInstanceOf<RedirectToRouteResult>(result);
            var redirect = result as RedirectToRouteResult;
            Assert.AreEqual("List", redirect.RouteValues["Action"]);
        }

        [Test]
        public void AdminUpdate_Should_Return_NotFound_If_UserProfile_Not_Found()
        {
            userProfile = EntityHelpers.GetValidUserProfile();
            var model = Mapper.Map<UserProfile, UserProfileAdminModel>(userProfile);
            var result = controller.AdminUpdate(model);
            Assert.IsInstanceOf<HttpNotFoundResult>(result);
        }

        [Test]
        public void AdminUpdate_Should_Redirect_To_Admin_If_ModelState_Not_Valid()
        {
            userProfile = EntityHelpers.GetValidUserProfile();
            repository.Add(userProfile);
            controller.ModelState.AddModelError("", "Oops");
            var model = Mapper.Map<UserProfile, UserProfileAdminModel>(userProfile);
            var result = controller.AdminUpdate(model);
            Assert.IsInstanceOf<RedirectToRouteResult>(result);
            var redirect = result as RedirectToRouteResult;
            Assert.AreEqual("Admin", redirect.RouteValues["Action"]);
        }

        private UserProfileController GetUserProfileController(int userProfileID = -1)
        {
            var causeRepository = new FakeCauseRepository();
            //Expect.Call(causeRepository.FindCausesByUserProfileID(userProfileID)).Return(new List<Cause>().AsQueryable());
            
            var mailer = mocks.DynamicMock<IUserProfileMailer>();
            //mocks.ReplayAll();

            MailerBase.IsTestModeEnabled = true;
            var upc = new UserProfileController(repository, causeRepository, mailer);
            //TestHelpers.MockHttpContext(upc, mocks);
            var context = MockRepository.GenerateStub<HttpContextBase>();
            var request = MockRepository.GenerateStub<HttpRequestBase>();
            context.Stub(x => x.Request).Return(request);
            context.User = new GenericPrincipal(new GenericIdentity("goodEmail"), null);
            upc.ControllerContext = new ControllerContext(context, new RouteData(), upc);
            return upc;
        }
    }
}
