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
            var organization = OrganizationRepository.GetDefaultOrganization(readOnly: true);
            var model = Mapper.Map<Organization, OrganizationDetailsModel>(organization);
            return View("Index", model);
        }

        public ActionResult About()
        {
            var organization = OrganizationRepository.GetDefaultOrganization(readOnly: true);
            var model = Mapper.Map<Organization, OrganizationDetailsModel>(organization);
            return View(model);
        }

        public ActionResult Projects()
        {
            var organization = OrganizationRepository.GetDefaultOrganization(readOnly: true);
            var templates = organization.CauseTemplates;
            var model = templates.Where(t => t.Active).Select(Mapper.Map<CauseTemplate, CauseTemplateDetailsModel>).ToList();
            return View(model);
        }

        [ChildActionOnly]
        [OutputCache(Duration = 150, VaryByParam = "none")]
        public ActionResult ProgressBar()
        {
            var organization = OrganizationRepository.GetDefaultOrganization(readOnly: true);
            var total = organization.CalculateTotalDonations();
            var percent = (int) ((total / organization.YtdGoal) * 100);
            var model = new ProgressBarModel
                            {
                                Amount = total,
                                GoalAmount = organization.YtdGoal,
                                Percent = percent,
                                GoalName = "YTD Total"
                            };

            return View("ProgressBar", model);
        }

        /// <summary>
        /// Cache for 10 min, to try to minimize impact of request throttling by Twitter's public REST API.
        /// </summary>
        /// <returns></returns>
        [ChildActionOnly]
        [OutputCache(Duration = 600, VaryByParam = "none")]
        public ActionResult TwitterFeed()
        {
            var organization = OrganizationRepository.GetDefaultOrganization(readOnly: true);
            var twitterName = organization.TwitterName;

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
            var organization = OrganizationRepository.GetDefaultOrganization(readOnly: true);
            var blogUrl = organization.BlogRssUrl;

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
            var organization = OrganizationRepository.GetDefaultOrganization(readOnly: true);
            var model = Mapper.Map<Organization, OrganizationDetailsModel>(organization);
            return View(model);
        }
    }
}
