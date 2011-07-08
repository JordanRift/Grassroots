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

using System.Linq;
using System.Web.Mvc;
using AutoMapper;
using JordanRift.Grassroots.Framework.Entities.Models;
using JordanRift.Grassroots.Framework.Helpers;
using JordanRift.Grassroots.Framework.Services;
using JordanRift.Grassroots.Web.Models;

namespace JordanRift.Grassroots.Web.Controllers
{
    public class HomeController : GrassrootsControllerBase
    {
        private readonly ITwitterService twitterService;
        private readonly IBlogService blogService;
        private Organization organization;

        public HomeController(ITwitterService twitterService, IBlogService blogService)
        {
            this.twitterService = twitterService;
            this.blogService = blogService;
            organization = OrganizationRepository.GetDefaultOrganization(readOnly: true);
            Mapper.CreateMap<Organization, OrganizationDetailsModel>();
            Mapper.CreateMap<OrganizationSetting, OrganizationSettingModel>();
            Mapper.CreateMap<CauseTemplate, CauseTemplateDetailsModel>();
        }

        public ActionResult Index()
        {
            OrganizationDetailsModel model;
            model = Mapper.Map<Organization, OrganizationDetailsModel>(organization);
            return View("Index", model);
        }

        public ActionResult About()
        {
            var model = Mapper.Map<Organization, OrganizationDetailsModel>(organization);
            return View(model);
        }

        public ActionResult Terms()
        {
            return View();
        }

        [ChildActionOnly]
        [OutputCache(Duration = 120, VaryByParam = "none")]
        public ActionResult ProgressBar()
        {
            decimal total;
			decimal totalGoal;
			string goalName = "Total";

            using (OrganizationRepository)
            {
                // Grab fresh data from the db, rather than cached collection...
                organization = OrganizationRepository.GetDefaultOrganization(readOnly: false);

                if (organization.YtdGoal.HasValue && organization.YtdGoal > 0)
                {
                    total = organization.CalculateTotalDonationsYTD();
                    totalGoal = organization.YtdGoal.Value;
                    goalName = "Total YTD";
                }
                else
                {
                    total = organization.CalculateTotalDonations();
                    totalGoal = organization.CalculateGoalTotal();
                }

                var percent = 0;
                if (totalGoal > 0)
                {
                    percent = (total > totalGoal) ? 100 : (int) ((total / totalGoal) * 100);
                }

                var model = new ProgressBarModel
                                {
                                    Amount = total,
                                    GoalAmount = totalGoal,
                                    Percent = percent,
                                    GoalName = goalName,
                                    DisplayTooltip = true
                                };

                return View("ProgressBar", model);
            }
        }

        [ChildActionOnly]
        [OutputCache(Duration = 120, VaryByParam = "none")]
        public ActionResult Stats()
        {
            organization = OrganizationRepository.GetDefaultOrganization(readOnly: false);
            var causes = organization.GetCompletedCauses();
            var model = new OrganizationStatsModel()
                            {
                                ProjectsCompleted = causes != null ? causes.Count() : 0,
                                ProjectsCompletedLabel = ModelHelpers.GetCausesLabelText(causes),
                                DollarsRaised = organization.CalculateTotalDonations(),
                                Donations = organization.GetDonationCount(),
                                HoursServed = organization.Causes != null ? organization.Causes.Sum(c => c.HoursVolunteered.GetValueOrDefault()) : 0,
                            };

            return View(model);
        }

        /// <summary>
        /// Cache for 10 min, to try to minimize impact of request throttling by Twitter's public REST API.
        /// </summary>
        /// <returns></returns>
        [ChildActionOnly]
        [OutputCache(Duration = 600, VaryByParam = "none")]
        public ActionResult TwitterFeed()
        {
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
           var blogUrl = organization.BlogRssUrl;

            if (!string.IsNullOrEmpty(blogUrl))
            {
                var post = blogService.GetLatestPost(blogUrl);
                return View(post);
            }

            return null;
        }

        [ChildActionOnly]
        [OutputCache(Duration = 120, VaryByParam = "none")]
        public ActionResult ThemeCss()
        {
            organization = OrganizationRepository.GetDefaultOrganization(readOnly: false);
            var model = Mapper.Map<Organization, OrganizationDetailsModel>(organization);
            return View(model);
        }
    }
}
