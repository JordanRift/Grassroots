//
// Copyright © 2011 Jordan Rift, LLC - All Rights Reserved
//
// THIS WORK IS LICENSED UNDER A CREATIVE COMMONS ATTRIBUTION-NONCOMMERCIAL-
// SHAREALIKE 3.0 UNPORTED LICENSE:
// http://creativecommons.org/licenses/by-nc-sa/3.0/
//

using System.Linq;
using System.Web.Mvc;
using AutoMapper;
using JordanRift.Grassroots.Framework.Entities.Models;
using JordanRift.Grassroots.Framework.Services;
using JordanRift.Grassroots.Web.Models;

namespace JordanRift.Grassroots.Web.Controllers
{
    public class HomeController : GrassrootsControllerBase
    {
        private readonly ITwitterService twitterService;
        private readonly IBlogService blogService;

        public HomeController(ITwitterService twitterService, IBlogService blogService)
        {
            this.twitterService = twitterService;
            this.blogService = blogService;
            Mapper.CreateMap<Organization, OrganizationDetailsModel>();
            Mapper.CreateMap<CauseTemplate, CauseTemplateDetailsModel>();
        }

        public ActionResult Index()
        {
            var model = Mapper.Map<Organization, OrganizationDetailsModel>(Organization);
            return View("Index", model);
        }

        public ActionResult About()
        {
            var model = Mapper.Map<Organization, OrganizationDetailsModel>(Organization);
            return View(model);
        }

        public ActionResult Projects()
        {
            var templates = Organization.CauseTemplates;
            var model = templates.Select(Mapper.Map<CauseTemplate, CauseTemplateDetailsModel>).ToList();
            return View(model);
        }

        [ChildActionOnly]
        [OutputCache(Duration = 150, VaryByParam = "none")]
        public ActionResult ProgressBar()
        {
            var total = Organization.CalculateTotalDonations();
            var percent = (int) ((total / Organization.YtdGoal) * 100);
            var model = new ProgressBarModel
                            {
                                Amount = total,
                                GoalAmount = Organization.YtdGoal,
                                Percent = percent,
                                GoalName = "YTD Total"
                            };

            return View("ProgressBar", model);
        }

        [ChildActionOnly]
        [OutputCache(Duration = 300, VaryByParam = "none")]
        public ActionResult TwitterFeed()
        {
            var twitterName = Organization.TwitterName;

            if (!string.IsNullOrEmpty(twitterName))
            {
                var tweets = twitterService.GetTweets(twitterName);
                return View(tweets);
            }

            return null;
        }

        [ChildActionOnly]
        [OutputCache(Duration = 300, VaryByParam = "none")]
        public ActionResult BlogRssFeed()
        {
            var blogUrl = Organization.BlogRssUrl;

            if (!string.IsNullOrEmpty(blogUrl))
            {
                var post = blogService.GetLatestPost(blogUrl);
                return View(post);
            }

            return null;
        }

        [ChildActionOnly]
        [OutputCache(Duration = 30, VaryByParam = "none")]
        public ActionResult ThemeCss()
        {
            var model = Mapper.Map<Organization, OrganizationDetailsModel>(Organization);
            return View(model);
        }
    }
}
