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
using System.Linq;
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
    class RoleControllerTests
    {
        private IRoleRepository roleRepository;
        private IOrganizationRepository organizationRepository;
        private RoleController controller;

        [SetUp]
        public void SetUp()
        {
            organizationRepository = new FakeOrganizationRepository();
            roleRepository = new FakeRoleRepository();
            SetupController();
            Mapper.CreateMap<Role, RoleAdminModel>();
        }

        [TearDown]
        public void TearDown()
        {
            FakeOrganizationRepository.Reset();
            FakeRoleRepository.Reset();
            controller = null;
        }

        [Test]
        public void List_Should_Return_Role_Grid_View()
        {
            var result = controller.List();
            Assert.IsInstanceOf<ViewResult>(result);
        }

        [Test]
        public void List_Should_Return_Populated_Role_Grid_View()
        {
            var result = controller.List() as ViewResult;
            var model = result.Model as IEnumerable<RoleAdminModel>;
            Assert.Greater(model.Count(), 0);
        }

        [Test]
        public void Admin_Should_Return_View_If_Role_Found()
        {
            var role = EntityHelpers.GetValidRole();
            roleRepository.Add(role);
            var result = controller.Admin(role.RoleID);
            Assert.IsInstanceOf<ViewResult>(result);
        }

        [Test]
        public void Admin_Should_Return_NotFound_If_Role_Not_Found()
        {
            var result = controller.Admin();
            Assert.IsInstanceOf<HttpNotFoundResult>(result);
        }

        [Test]
        public void Admin_Should_Display_ModelErrors_If_Found_In_TempData()
        {
            var role = EntityHelpers.GetValidRole();
            roleRepository.Add(role);
            controller.TempData["ModelErrors"] = new List<string> { "something bad happened" };
            controller.Admin(role.RoleID);
            Assert.IsFalse(controller.ModelState.IsValid);
        }

        [Test]
        public void Admin_Should_Populate_Model_If_Found_In_TempData()
        {
            var role = EntityHelpers.GetValidRole();
            roleRepository.Add(role);
            var model = Mapper.Map<Role, RoleAdminModel>(role);
            controller.TempData["RoleAdminModel"] = model;
            controller.TempData["ModelErrors"] = new List<string> { "uh oh" };
            var result = controller.Admin(role.RoleID) as ViewResult;
            model = result.Model as RoleAdminModel;
            Assert.AreEqual(role.Name, model.Name);
        }

        [Test]
        public void Admin_Should_Return_Forbidden_If_IsSystemRole_Is_True()
        {
            var role = EntityHelpers.GetValidRole();
            role.IsSystemRole = true;
            roleRepository.Add(role);
            var result = controller.Admin(role.RoleID);
            Assert.IsInstanceOf<HttpStatusCodeResult>(result);
            var status = result as HttpStatusCodeResult;
            Assert.AreEqual(403, status.StatusCode);
        }

        [Test]
        public void Destroy_Should_Return_Json_If_Ajax_Delete_Successful()
        {
            controller.Request.Stub(x => x["X-Requested-With"]).Return("XMLHttpRequest");
            var role = EntityHelpers.GetValidRole();
            roleRepository.Add(role);
            var result = controller.Destroy(role.RoleID);
            Assert.IsInstanceOf<JsonResult>(result);
        }

        [Test]
        public void Destroy_Should_Return_Redirect_If_Delete_Successful()
        {
            var role = EntityHelpers.GetValidRole();
            roleRepository.Add(role);
            var result = controller.Destroy(role.RoleID);
            Assert.IsInstanceOf<RedirectToRouteResult>(result);
        }

        [Test]
        public void Destroy_Should_Remove_Role_If_Found()
        {
            var role = EntityHelpers.GetValidRole();
            roleRepository.Add(role);
            var id = role.RoleID;
            controller.Destroy(id);
            role = roleRepository.GetRoleByID(id);
            Assert.IsNull(role);
        }

        [Test]
        public void Destroy_Should_Return_NotFound_If_Role_Not_Found()
        {
            var result = controller.Destroy();
            Assert.IsInstanceOf<HttpNotFoundResult>(result);
        }

        [Test]
        public void Destroy_Should_Return_Forbidden_If_IsSystemRole_Is_True()
        {
            var role = EntityHelpers.GetValidRole();
            role.IsSystemRole = true;
            roleRepository.Add(role);
            var result = controller.Destroy(role.RoleID);
            Assert.IsInstanceOf<HttpStatusCodeResult>(result);
            var status = result as HttpStatusCodeResult;
            Assert.AreEqual(403, status.StatusCode);
        }

        [Test]
        public void Update_Should_Redirect_To_List_If_Successful()
        {
            var role = EntityHelpers.GetValidRole();
            roleRepository.Add(role);
            var model = Mapper.Map<Role, RoleAdminModel>(role);
            var result = controller.Update(model);
            Assert.IsInstanceOf<RedirectToRouteResult>(result);
            var redirect = result as RedirectToRouteResult;
            Assert.AreEqual("List", redirect.RouteValues["Action"]);
        }

        [Test]
        public void Update_Should_Return_NotFound_If_Role_Not_Found()
        {
            var role = EntityHelpers.GetValidRole();
            var model = Mapper.Map<Role, RoleAdminModel>(role);
            var result = controller.Update(model);
            Assert.IsInstanceOf<HttpNotFoundResult>(result);
        }

        [Test]
        public void Update_Should_Redirect_To_Admin_If_ModelState_Not_Valid()
        {
            var role = EntityHelpers.GetValidRole();
            roleRepository.Add(role);
            var model = Mapper.Map<Role, RoleAdminModel>(role);
            controller.ModelState.AddModelError("", "Doh!");
            var result = controller.Update(model);
            Assert.IsInstanceOf<RedirectToRouteResult>(result);
            var redirect = result as RedirectToRouteResult;
            Assert.AreEqual("Admin", redirect.RouteValues["Action"]);
        }

        [Test]
        public void Update_Should_Add_Model_To_TempData_If_ModelState_Not_Valid()
        {
            var role = EntityHelpers.GetValidRole();
            roleRepository.Add(role);
            var model = Mapper.Map<Role, RoleAdminModel>(role);
            controller.ModelState.AddModelError("", "Doh!");
            controller.Update(model);
            var result = controller.TempData["RoleAdminModel"];
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<RoleAdminModel>(result);
        }

        [Test]
        public void Update_Should_Add_Model_Errors_To_TempData_If_ModelState_Not_Valid()
        {
            var role = EntityHelpers.GetValidRole();
            roleRepository.Add(role);
            var model = Mapper.Map<Role, RoleAdminModel>(role);
            controller.ModelState.AddModelError("", "Doh!");
            controller.Update(model);
            var result = controller.TempData["ModelErrors"];
            Assert.IsNotNull(result);
        }

        [Test]
        public void Update_Should_Update_Role_Properties_When_Successful()
        {
            var role = EntityHelpers.GetValidRole();
            roleRepository.Add(role);
            var id = role.RoleID;
            var model = new RoleAdminModel
                            {
                                RoleID = id,
                                Name = "New Role",
                                Description = "Some description"
                            };

            controller.Update(model);
            role = roleRepository.GetRoleByID(id);
            Assert.AreEqual(model.Name, role.Name);
            Assert.AreEqual(model.Description, role.Description);
        }

        [Test]
        public void Update_Should_Return_Forbidden_If_IsSystemRole_Is_True()
        {
            var role = EntityHelpers.GetValidRole();
            role.IsSystemRole = true;
            roleRepository.Add(role);
            var model = Mapper.Map<Role, RoleAdminModel>(role);
            var result = controller.Update(model);
            Assert.IsInstanceOf<HttpStatusCodeResult>(result);
            var status = result as HttpStatusCodeResult;
            Assert.AreEqual(403, status.StatusCode);
        }

        [Test]
        public void New_Should_Return_View()
        {
            var result = controller.New();
            Assert.IsInstanceOf<ViewResult>(result);
        }

        [Test]
        public void New_Should_Return_Populated_View_When_Model_In_TempData()
        {
            var role = EntityHelpers.GetValidRole();
            var model = Mapper.Map<Role, RoleAdminModel>(role);
            controller.TempData["RoleAdminModel"] = model;
            var result = controller.New();
            var viewModel = (result as ViewResult).Model as RoleAdminModel;
            Assert.AreEqual(model.Name, viewModel.Name);
        }

        [Test]
        public void New_Should_Display_ModelErrors_If_Found_In_TempData()
        {
            controller.TempData["ModelErrors"] = new List<string> { "Oops!" };
            controller.New();
            Assert.IsFalse(controller.ModelState.IsValid);
        }

        [Test]
        public void Create_Should_Redirect_To_List_When_Successful()
        {
            var role = EntityHelpers.GetValidRole();
            var model = Mapper.Map<Role, RoleAdminModel>(role);
            var result = controller.Create(model);
            Assert.IsInstanceOf<RedirectToRouteResult>(result);
            var redirect = result as RedirectToRouteResult;
            Assert.AreEqual("List", redirect.RouteValues["Action"]);
        }

        [Test]
        public void Create_Should_Redirect_To_New_When_ModelState_Is_Invalid()
        {
            var role = EntityHelpers.GetValidRole();
            var model = Mapper.Map<Role, RoleAdminModel>(role);
            controller.ModelState.AddModelError("", "Doh!");
            var result = controller.Create(model);
            Assert.IsInstanceOf<RedirectToRouteResult>(result);
            var redirect = result as RedirectToRouteResult;
            Assert.AreEqual("New", redirect.RouteValues["Action"]);
        }

        [Test]
        public void Create_Should_Add_Model_To_TempData_When_ModelState_Is_Invalid()
        {
            var role = EntityHelpers.GetValidRole();
            var model = Mapper.Map<Role, RoleAdminModel>(role);
            controller.ModelState.AddModelError("", "Doh!");
            controller.Create(model);
            var result = controller.TempData["RoleAdminModel"];
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<RoleAdminModel>(result);
        }

        [Test]
        public void Create_Should_Add_Model_Errors_To_TempData_When_ModelState_Is_Invalid()
        {
            var role = EntityHelpers.GetValidRole();
            var model = Mapper.Map<Role, RoleAdminModel>(role);
            controller.ModelState.AddModelError("", "Doh!");
            controller.Create(model);
            var result = controller.TempData["ModelErrors"];
            Assert.IsNotNull(result);
        }

        [Test]
        public void Create_Should_Add_Model_To_Organization_When_Successful()
        {
            var role = EntityHelpers.GetValidRole();
            var model = Mapper.Map<Role, RoleAdminModel>(role);
            controller.Create(model);
            var organization = organizationRepository.GetDefaultOrganization();
            role = organization.Roles.FirstOrDefault();
            Assert.IsNotNull(role);
        }

        [Test]
        public void Create_Shoud_Set_IsSystemRole_To_False()
        {
            var role = EntityHelpers.GetValidRole();
            var model = Mapper.Map<Role, RoleAdminModel>(role);
            controller.Create(model);
            var organization = organizationRepository.GetDefaultOrganization();
            role = organization.Roles.FirstOrDefault();
            Assert.IsFalse(role.IsSystemRole);
        }

        public void SetupController()
        {
            controller = new RoleController(roleRepository)
                             {
                                 OrganizationRepository = organizationRepository
                             };

            TestHelpers.MockBasicRequest(controller);
        }
    }
}
