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
using JordanRift.Grassroots.Framework.Helpers;
using JordanRift.Grassroots.Web.Models;

namespace JordanRift.Grassroots.Web.Controllers
{
    [Authorize(Roles = "Root,Administrator")]
    public class RoleController : GrassrootsControllerBase
    {
        private readonly IRoleRepository roleRepository;

        public RoleController(IRoleRepository roleRepository)
        {
            this.roleRepository = roleRepository;
            Mapper.CreateMap<Role, RoleAdminModel>();
        }

        public ActionResult List()
        {
            var roles = roleRepository.FindAllRoles().ToList();
            var models = new List<RoleAdminModel>();

            foreach (var role in roles)
            {
                var model = Mapper.Map<Role, RoleAdminModel>(role);
                models.Add(model);
            }

            return View(models);
        }

        public ActionResult New()
        {
            if (TempData["ModelErrors"] != null)
            {
                var errors = TempData["ModelErrors"] as IEnumerable<string>;

                foreach (var error in errors)
                {
                    ModelState.AddModelError("", error);
                }
            }

            var model = TempData["RoleAdminModel"] as RoleAdminModel ?? new RoleAdminModel();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken(Salt = "AdminCreateRole")]
        public ActionResult Create(RoleAdminModel model)
        {
            if (!ModelState.IsValid)
            {
                TempData["ModelErrors"] = FindModelErrors();
                TempData["RoleAdminModel"] = model;
                return RedirectToAction("New");
            }

            using (new UnitOfWorkScope())
            {
                var organization = OrganizationRepository.GetDefaultOrganization(readOnly: false);

                // All roles that are created outside of the db install script are not System Roles
                var role = new Role
                               {
                                   Name = model.Name,
                                   Description = model.Description,
                                   IsSystemRole = false
                               };

                organization.Roles.Add(role);
                roleRepository.Save();
                TempData["UserFeedback"] = string.Format("'{0}' was created successfully", role.Name);
                return RedirectToAction("List", new { id = role.RoleID });
            }
        }

        public ActionResult Admin(int id = -1)
        {
            RoleAdminModel model;

            if (TempData["ModelErrors"] != null)
            {
                var errors = TempData["ModelErrors"] as IEnumerable<string>;

                foreach (var error in errors)
                {
                    ModelState.AddModelError("", error);
                }

                model = TempData["RoleAdminModel"] as RoleAdminModel;
            }
            else
            {
                using (roleRepository)
                {
                    var role = roleRepository.GetRoleByID(id);

                    if (role == null)
                    {
                        return HttpNotFound("The role you are looking for could not be found.");
                    }

                    // If role is a "System Role" we don't want anybody editing or deleting it.
                    if (role.IsSystemRole)
                    {
                        return new HttpStatusCodeResult(403, "You do not have permission to edit this security role.");
                    }

                    model = Mapper.Map<Role, RoleAdminModel>(role);
                }
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken(Salt = "AdminUpdateRole")]
        public ActionResult Update(RoleAdminModel model)
        {
            if (!ModelState.IsValid)
            {
                TempData["ModelErrors"] = FindModelErrors();
                TempData["RoleAdminModel"] = model;
                return RedirectToAction("Admin", new { id = model.RoleID });
            }

            using (roleRepository)
            {
                var role = roleRepository.GetRoleByID(model.RoleID);

                if (role == null)
                {
                    return HttpNotFound("The role you are looking for could not be found.");
                }
                
                // If role is a "System Role" we don't want anybody editing or deleting it.
                if (role.IsSystemRole)
                {
                    return new HttpStatusCodeResult(403, "You do not have permission to edit this security role.");
                }

                MapRole(model, role);
                roleRepository.Save();
                TempData["UserFeedback"] = string.Format("'{0}' was updated successfully.", role.Name);
            }

            return RedirectToAction("List");
        }

        [HttpDelete]
        public ActionResult Destroy(int id = -1)
        {
            using (roleRepository)
            {
                var role = roleRepository.GetRoleByID(id);

                if (role == null)
                {
                    return HttpNotFound("The role you are looking for could not be found.");
                }

                // If role is a "System Role" we don't want anybody editing or deleting it.
                if (role.IsSystemRole)
                {
                    return new HttpStatusCodeResult(403, "You do not have permission to delete this security role.");
                }

                roleRepository.Delete(role);
                roleRepository.Save();
            }

            if (Request.IsAjaxRequest())
            {
                return Json(new { success = true });
            }

            return RedirectToAction("List");
        }

        private static void MapRole(RoleAdminModel model, Role role)
        {
            role.Name = model.Name;
            role.Description = model.Description;
        }
    }
}
