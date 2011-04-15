//
// Copyright © 2011 Jordan Rift, LLC - All Rights Reserved
//
// THIS WORK IS LICENSED UNDER A CREATIVE COMMONS ATTRIBUTION-NONCOMMERCIAL-
// SHAREALIKE 3.0 UNPORTED LICENSE:
// http://creativecommons.org/licenses/by-nc-sa/3.0/
//

using System.Web.Mvc;
using JordanRift.Grassroots.Framework.Entities.Models;

namespace JordanRift.Grassroots.Web.Controllers
{
    public class ValidationController : Controller
    {
        /// <summary>
        /// Remote validation method to receive ajax call to confirm email address will be unique.
        /// </summary>
        /// <param name="email">email address to check against</param>
        /// <returns>JSON true/false result</returns>
        public JsonResult CheckEmail(string email)
        {
            // Flip boolean logic to return true if is valid (e.g. - Email does not exist in repository).
            var result = UserProfile.IsUnique(null, email, -1);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Remote validation method to receive ajax call to confirm that url slug will be unique.
        /// </summary>
        /// <param name="urlSlug">url slug to check against</param>
        /// <returns>JSON true/false result</returns>
        public JsonResult CheckUrlSlug(string urlSlug)
        {
            var result = Campaign.IsUnique(null, urlSlug, -1);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}
