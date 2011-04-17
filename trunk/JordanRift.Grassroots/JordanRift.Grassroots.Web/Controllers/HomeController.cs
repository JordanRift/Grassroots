//
// Copyright © 2011 Jordan Rift, LLC - All Rights Reserved
//
// THIS WORK IS LICENSED UNDER A CREATIVE COMMONS ATTRIBUTION-NONCOMMERCIAL-
// SHAREALIKE 3.0 UNPORTED LICENSE:
// http://creativecommons.org/licenses/by-nc-sa/3.0/
//

using System.Web.Mvc;
using AutoMapper;
using JordanRift.Grassroots.Framework.Entities.Models;
using JordanRift.Grassroots.Web.Models;

namespace JordanRift.Grassroots.Web.Controllers
{
    public class HomeController : GrassrootsControllerBase
    {
        public HomeController()
        {
            Mapper.CreateMap<Organization, OrganizationDetailsModel>();
        }

        public ActionResult Index()
        {
            var model = Mapper.Map<Organization, OrganizationDetailsModel>(Organization);
            return View("Index", model);
        }

        public ActionResult About()
        {
            return View();
        }

        [ChildActionOnly]
        [OutputCache(Duration = 30, VaryByParam = "*")]
        public ActionResult ProgressBar()
        {
            var percent = 90;
            return View("ProgressBar", percent);
        }

        [ChildActionOnly]
        [OutputCache(Duration = 30, VaryByParam = "*")]
        public ActionResult TwitterFeed()
        {
            var twitterName = Organization.TwitterName;
            return View();
        }

        [ChildActionOnly]
        [OutputCache(Duration = 30, VaryByParam = "*")]
        public ActionResult BlogRssFeed()
        {
            var blogUrl = Organization.BlogRssUrl;
            return View();
        }
    }
}
