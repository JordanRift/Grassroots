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
using System.Web.Mvc;
using AutoMapper;
using JordanRift.Grassroots.Framework.Data;
using JordanRift.Grassroots.Framework.Entities.Models;
using JordanRift.Grassroots.Framework.Helpers;
using JordanRift.Grassroots.Framework.Services;
using JordanRift.Grassroots.Web.Models;

namespace JordanRift.Grassroots.Web.Controllers
{
    [Authorize(Roles = "Root,Administrator")]
    public class UserController : GrassrootsControllerBase
    {
        private readonly IUserProfileRepository userProfileRepository;

        public UserController(IUserProfileRepository userProfileRepository)
        {
            this.userProfileRepository = userProfileRepository;
            Mapper.CreateMap<UserProfile, PasswordAdminModel>();
        }

        ~UserController()
        {
            userProfileRepository.Dispose();
        }

        public ActionResult ChangePassword(int id = -1)
        {
            if (TempData["ModelErrors"] != null)
            {
                var errors = TempData["ModelErrors"] as List<string>;

                foreach (var error in errors)
                {
                    ModelState.AddModelError("", error);
                }
            }

            using (userProfileRepository)
            {
                var userProfile = userProfileRepository.GetUserProfileByID(id);

                if (userProfile == null)
                {
                    return HttpNotFound("The user you are looking for could not be found.");
                }

                var model = Mapper.Map<UserProfile, PasswordAdminModel>(userProfile);
                return View(model);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken(Salt = "AdminChangePassword")]
        public ActionResult SavePassword(PasswordAdminModel model)
        {
            if (!ModelState.IsValid)
            {
                TempData["ModelErrors"] = FindModelErrors();
                return RedirectToAction("ChangePassword", new { id = model.UserProfileID });
            }

            using (new UnitOfWorkScope())
            {
                var userProfile = userProfileRepository.GetUserProfileByID(model.UserProfileID);

                if (userProfile == null)
                {
                    return HttpNotFound("The user you are looking for could not be found.");
                }

                var user = userProfile.Users.FirstOrDefault();

                if (user != null)
                {
                    user.Password = GrassrootsMembershipService.HashPassword(model.Password, null);
                    user.ForcePasswordChange = model.ForcePasswordChange;
                }
                else
                {
                    userProfile.Users.Add(new User
                                              {
                                                  Username = userProfile.Email,
                                                  Password =
                                                      GrassrootsMembershipService.HashPassword(model.Password, null),
                                                  RegisterDate = DateTime.Now,
                                                  LastLoggedIn = DateTime.Now,
                                                  IsActive = true,
                                                  IsAuthorized = true,
                                                  ForcePasswordChange = model.ForcePasswordChange
                                              });
                }

                userProfileRepository.Save();
                TempData["UserFeedback"] = string.Format("{0}'s password has been reset successfully.", userProfile.FullName);
                return RedirectToAction("Admin", "UserProfile", new { id = userProfile.UserProfileID });
            }
        }
    }
}
