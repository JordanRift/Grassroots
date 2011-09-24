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
using JordanRift.Grassroots.Framework.Entities;
using JordanRift.Grassroots.Framework.Helpers;
using JordanRift.Grassroots.Web.Models;
using JordanRift.Grassroots.Framework.Entities.Models;

namespace JordanRift.Grassroots.Web.Controllers
{
	public class AdminController : GrassrootsControllerBase
	{
	    private readonly ICampaignRepository campaignRepository;
	    private readonly IRoleRepository roleRepository;

		public AdminController(ICampaignRepository campaignRepository, IRoleRepository roleRepository)
		{
		    this.campaignRepository = campaignRepository;
		    this.roleRepository = roleRepository;
			Mapper.CreateMap<Organization, OrganizationDetailsModel>();
		    Mapper.CreateMap<OrganizationSetting, OrganizationSettingModel>();
		}

		//
		// GET: /Admin/
        [Authorize(Roles = "Root,Administrator")]
		public ActionResult Index()
		{
			return View();
		}

        [Authorize(Roles = "Root")]
		public ActionResult EditOrganization()
		{
            using (OrganizationRepository)
            {
                // Perhaps someday we'll pull this from the route?
                var organization = OrganizationRepository.GetDefaultOrganization(readOnly: false);

                if (organization != null)
                {
                    OrganizationDetailsModel viewModel;

                    if (TempData["OrganizationEditModel"] != null)
                    {
                        viewModel = TempData["OrganizationEditModel"] as OrganizationDetailsModel;
                    }
                    else
                    {
                        viewModel = MapOrganizationDetails(organization);
                    }

                    return View(viewModel);
                }
            }

		    return HttpNotFound( "The organization could not be found." );
		}

        [Authorize(Roles = "Root")]
		[HttpPost]
		public ActionResult UpdateOrganization( OrganizationDetailsModel model )
		{
            using (OrganizationRepository)
            {
                var organization = OrganizationRepository.GetDefaultOrganization(readOnly: false);

                if (ModelState.IsValid)
                {
                    MapOrganizationUpdate(organization, model);
                    MapOrganizationSettings(organization, model.OrganizationSettings);
                    OrganizationRepository.Save();
                    TempData["UserFeedback"] = "Your changes have been saved. Please allow a few minutes for them to take effect.";
                    return RedirectToAction("Index", "Admin");
                }
            }

		    TempData["OrganizationEditModel"] = model;
			return RedirectToAction( "EditOrganization", "Admin" );
		}

	    private void MapOrganizationSettings(Organization organization, IEnumerable<OrganizationSettingModel> settings)
	    {
            foreach (var setting in settings)
            {
                var orgSetting = organization.GetSetting(setting.Name);

                if (orgSetting != null)
                {
                    if (!string.IsNullOrEmpty(setting.Value))
                    {
                        orgSetting.Value = setting.Value;
                    }
                    else
                    {
                        OrganizationRepository.DeleteSetting(orgSetting);
                    }
                }
                else
                {
                    if (!string.IsNullOrEmpty(setting.Value))
                    {
                        organization.OrganizationSettings.Add(new OrganizationSetting
                                                                  {
                                                                      Name = setting.Name,
                                                                      Value = setting.Value,
                                                                      DataType = (int) DataType.String
                                                                  });
                    }
                }
                
            }
	    }

        /// <summary>
        /// Request to generate the "General Campaign" for the current month if none exists. Leave public so it can be automated.
        /// </summary>
        /// <returns>Http Status indicating success or error</returns>
        public ActionResult GenerateDetaultCampaign()
        {
            var defaultCampaign = campaignRepository.GetDefaultCampaign();

            if (defaultCampaign.StartDate.Month == DateTime.Now.Month)
            {
                return new HttpStatusCodeResult(403);
            }

            using (new UnitOfWorkScope())
            {
                var organization = OrganizationRepository.GetDefaultOrganization(readOnly: true);
                var causeTemplate = organization.CauseTemplates.FirstOrDefault();
                var root = roleRepository.GetRoot();
                var userProfile = root.UserProfiles.FirstOrDefault();

                if (causeTemplate == null || userProfile == null)
                {
                    return HttpNotFound("The default cause template or root user could not be found");
                }

                var today = DateTime.Now;
                var campaign = new Campaign
                                   {
                                       GoalAmount = causeTemplate.DefaultAmount,
                                       Title = string.Format("General Fund {0} {1}", today.Month, today.Year),
                                       UrlSlug = string.Format("{0}_{1}-{2}", organization.Name, today.Month, today.Year),
                                       StartDate = new DateTime(today.Year, today.Month, 1),
                                       // Get the first day of the current month
                                       EndDate = today.AddMonths(1).AddDays(-1),
                                       // Get the last day of the current month
                                       ImagePath = string.Empty,
                                       Description = string.Format("General Fund for month of {0}, {1}", today.Month, today.Year),
                                       IsGeneralFund = true
                                   };

                organization.Campaigns.Add(campaign);
                causeTemplate.Campaigns.Add(campaign);
                userProfile.Campaigns.Add(campaign);
                campaignRepository.Save();
            }

            // Success
            return new HttpStatusCodeResult(200);
        }

	    private static void MapOrganizationUpdate( Organization organization, OrganizationDetailsModel model )
		{
			organization.Name = model.Name;
		    organization.Tagline = model.Tagline;
			organization.ContactEmail = model.ContactEmail;
			organization.ContactPhone = model.ContactPhone;
		    organization.YtdGoal = model.YtdGoal;
		    organization.FiscalYearStartMonth = model.FiscalYearStartMonth;
		    organization.FiscalYearStartDay = model.FiscalYearStartDay;
		    organization.SummaryHtml = model.SummaryHtml;
		    organization.DescriptionHtml = model.DescriptionHtml;
		    organization.PaymentGatewayApiUrl = model.PaymentGatewayApiUrl ?? "";
	        organization.PaymentGatewayArbApiUrl = model.PaymentGatewayArbApiUrl ?? "";
			organization.PaymentGatewayApiKey = model.PaymentGatewayApiKey ?? "";
			organization.PaymentGatewayApiSecret = model.PaymentGatewayApiSecret ?? "";
			organization.PaymentGatewayType = model.PaymentGatewayType;
		    organization.FacebookPageUrl = model.FacebookPageUrl;
			organization.TwitterName = model.TwitterName;
		    organization.BlogRssUrl = model.BlogRssUrl;
			organization.VideoEmbedHtml = model.VideoEmbedHtml;
		    organization.ThemeName = model.ThemeName;
		}

        private static OrganizationDetailsModel MapOrganizationDetails(Organization organization)
        {
            var model = Mapper.Map<Organization, OrganizationDetailsModel>(organization);
            model.OrganizationSettings = new List<OrganizationSettingModel>();
            var keys = ModelHelpers.GetKeys(typeof(OrgSettingKeys));

            foreach (var key in keys)
            {
                var setting = organization.GetSetting(key);
                OrganizationSettingModel settingModel;
                
                if (setting != null)
                {
                    settingModel = Mapper.Map<OrganizationSetting, OrganizationSettingModel>(setting);
                }
                else
                {
                    settingModel = new OrganizationSettingModel
                                       {
                                           DataType = (int) DataType.String,
                                           Name = key
                                       };
                }
                
                model.OrganizationSettings.Add(settingModel);
            }

            return model;
        }
	}
}
