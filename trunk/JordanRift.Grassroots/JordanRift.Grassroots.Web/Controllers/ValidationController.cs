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
using JordanRift.Grassroots.Framework.Entities.Models;
using JordanRift.Grassroots.Framework.Helpers;
using JordanRift.Grassroots.Framework.Services;

namespace JordanRift.Grassroots.Web.Controllers
{
    public class ValidationController : Controller
    {
        /// <summary>
        /// Remote validation method to receive ajax call to confirm email address will be unique.
        /// </summary>
        /// <param name="email">email address to check against</param>
        /// <param name="userProfileID">user profile ID to check against in the database</param>
        /// <returns>JSON true/false result</returns>
        public JsonResult CheckEmail(string email, int userProfileID = -1)
        {
            // Flip boolean logic to return true if is valid (e.g. - Email does not exist in repository).
            var result = UserProfile.IsUnique(null, email, userProfileID);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Remote validation method to receive ajax call to confirm that url slug will be unique.
        /// </summary>
        /// <param name="urlSlug">url slug to check against</param>
        /// <param name="campaignID">campaign ID to check against in the database</param>
        /// <returns>JSON true/false result</returns>
        public JsonResult CheckUrlSlug(string urlSlug, int campaignID = -1)
        {
            var result = Campaign.IsUnique(null, urlSlug, campaignID);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult CheckFacebookAccount(string facebookID)
        {
            using (new UnitOfWorkScope())
            {
                var service = new UserProfileService();
                var result = service.IsFacebookAccountUnique(facebookID);
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }
    }
}
