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
using System.Linq;
using System.Net;
using System.Web.Mvc;
using AutoMapper;
using JordanRift.Grassroots.Framework.Data;
using JordanRift.Grassroots.Framework.Entities;
using JordanRift.Grassroots.Framework.Entities.Models;
using JordanRift.Grassroots.Framework.Helpers;
using JordanRift.Grassroots.Web.Helpers;
using JordanRift.Grassroots.Web.Mailers;
using JordanRift.Grassroots.Web.Models;
using Mvc.Mailer;

namespace JordanRift.Grassroots.Web.Controllers
{
    public class CampaignController : GrassrootsControllerBase
    {
        private readonly ICampaignRepository campaignRepository;
        private readonly IUserProfileRepository userProfileRepository;
        private readonly ICampaignMailer campaignMailer;

        public CampaignController(ICampaignRepository campaignRepository, IUserProfileRepository userProfileRepository, ICampaignMailer campaignMailer)
        {
            this.campaignRepository = campaignRepository;
            this.userProfileRepository = userProfileRepository;
            this.campaignMailer = campaignMailer;
            Mapper.CreateMap<Campaign, CampaignDetailsModel>();
            Mapper.CreateMap<UserProfile, CampaignDetailsModel>();
            Mapper.CreateMap<CauseTemplate, CampaignDetailsModel>();
            Mapper.CreateMap<CauseTemplate, CauseTemplateDetailsModel>();
            Mapper.CreateMap<Campaign, CampaignEmailBlastModel>();
            Mapper.CreateMap<CampaignDonor, DonationDetailsModel>();
            Mapper.CreateMap<CampaignDetailsModel, Campaign>();
            Mapper.CreateMap<CampaignCreateModel, Campaign>();
        }

        public ActionResult Index(string slug = "")
        {
            var campaign = campaignRepository.GetCampaignByUrlSlug(slug);

            if (slug == null || campaign == null)
            {
                return HttpNotFound("The Campaign you are looking for could not be found");
            }

            ViewBag.EmailBlastModel = MapEmailBlast(campaign);
            var viewModel = MapDetailsModel(campaign);
            return View("Details", viewModel);
        }
        
        [Authorize]
        public ActionResult GetStarted()
        {
            var organization = OrganizationRepository.GetDefaultOrganization(readOnly: true);
            var viewModel = new GetStartedModel
                                {
                                    CauseTemplates = organization.CauseTemplates
                                        .Where(t => t.Active)
                                        .Select(Mapper.Map<CauseTemplate, CauseTemplateDetailsModel>),
                                    CampaignType = (int) CampaignType.Unknown
                                };

            var defaultCauseTemplate = viewModel.CauseTemplates.FirstOrDefault();

            if (defaultCauseTemplate != null)
            {
                viewModel.CauseTemplateID = defaultCauseTemplate.CauseTemplateID;
            }
            
            return View(viewModel);
        }

        [Authorize]
        [HttpPost]
        public ActionResult Create(GetStartedModel model)
        {
            var viewModel = TempData["CampaignDetailsModel"] as CampaignCreateModel ?? new CampaignCreateModel();

            if (model.CauseTemplateID != -1)
            {
                var organization = OrganizationRepository.GetDefaultOrganization(readOnly: true);
                var causeTemplate = organization.CauseTemplates.FirstOrDefault(ct => ct.CauseTemplateID == model.CauseTemplateID);

                if (causeTemplate != null)
                {
                    viewModel.AmountIsConfigurable = causeTemplate.AmountIsConfigurable;
                    viewModel.DefaultAmount = causeTemplate.DefaultAmount;
                    viewModel.GoalName = causeTemplate.GoalName;
                    viewModel.CauseTemplateID = causeTemplate.CauseTemplateID;
                }
                else
                {
                    return RedirectToAction("GetStarted");
                }
            }

            viewModel.CampaignType = model.CampaignType;
            return View("Create", viewModel);
        }

        [Authorize]
        [HttpPost]
        public ActionResult CreateCampaign(CampaignCreateModel model)
        {
            if (ModelState.IsValid)
            {
                using (new UnitOfWorkScope())
                {
                    var campaign = Mapper.Map<CampaignCreateModel, Campaign>(model);
                    var userProfile = userProfileRepository.FindUserProfileByEmail(User.Identity.Name).FirstOrDefault();

                    if (userProfile.GetActiveCampaigns().Any())
                    {
                        TempData["ErrorMessage"] = "You do not have permission to create a new Campaign. You've already got one that's active.";
                        return RedirectToAction("Index");
                    }

                    var organization = userProfile.Organization;
                    var causeTemplate = organization.CauseTemplates.FirstOrDefault(t => t.CauseTemplateID == model.CauseTemplateID);
                    campaign.StartDate = DateTime.Now;
                    campaign.EndDate = DateTime.Now.AddDays(causeTemplate.DefaultTimespanInDays);
                    campaign.GoalAmount = causeTemplate.DefaultAmount;
                    campaign.Description = "You should type your campaign description in here...";

                    // TODO: Save image to disk and set path in campaign object (~/Content/UserContent/campaign/{campaignID}.jpg)
                    if (string.IsNullOrEmpty(campaign.ImagePath))
                    {
                        campaign.ImagePath = EntityConstants.DEFAULT_CAMPAIGN_IMAGE_PATH;
                    }

                    organization.Campaigns.Add(campaign);
                    causeTemplate.Campaigns.Add(campaign);
                    userProfile.Campaigns.Add(campaign);
                    campaignRepository.Save();
                }

                return RedirectToAction("Index", "Campaign", new { slug = model.UrlSlug });
            }

            TempData["CampaignDetailsModel"] = model;
            return RedirectToAction("Create");
        }

        [Authorize]
        public ActionResult Edit(string slug = "")
        {
            var campaign = campaignRepository.GetCampaignByUrlSlug(slug);

            if (campaign == null)
            {
                return HttpNotFound("The Campaign you are looking for could not be found");
            }

            var userProfile = campaign.UserProfile;

            if (User.Identity.Name.ToLower() != userProfile.Email.ToLower())
            {
                TempData["ErrorMessage"] = "Sorry, you don't have permission to edit this Campaign.";
                return RedirectToAction("Index", new { slug = campaign.UrlSlug });
            }

            //var viewModel = TempData["CampaignDetailsModel"] == null ? 
            //    TempData["CampaignDetailsModel"] as CampaignDetailsModel : 
            //    MapDetailsModel(campaign);
            var viewModel = MapDetailsModel(campaign);
            ViewBag.EmailBlastModel = MapEmailBlast(campaign);
            return View("Edit", viewModel);
        }

        [Authorize]
        [HttpPost]
        public ActionResult Update(CampaignDetailsModel model, int id = -1)
        {
            var campaign = campaignRepository.GetCampaignByID(id);

            if (campaign == null)
            {
                return HttpNotFound("The Campaign you are looking for could not be found.");
            }

            var userProfile = campaign.UserProfile;

            if (User.Identity.Name.ToLower() != userProfile.Email.ToLower())
            {
                TempData["ErrorMessage"] = "Sorry, you don't have permission to edit this Campaign.";
                return RedirectToAction("Index", new { slug = campaign.UrlSlug });
            }

            if (ModelState.IsValid)
            {
                MapCampaign(campaign, model);
                campaignRepository.Save();
                return RedirectToAction("Index", new { slug = campaign.UrlSlug });
            }

            TempData["CampaignDetailsModel"] = model;
            return RedirectToAction("Edit", new { slug = campaign.UrlSlug });
        }

        [Authorize]
        [HttpPost]
        public ActionResult SendEmail(CampaignEmailBlastModel model)
        {
            var campaign = campaignRepository.GetCampaignByUrlSlug(model.UrlSlug);

            if (campaign != null)
            {
                var userProfile = campaign.UserProfile;

                // Only allow email to send if the campaign owner is currently logged in
                if (User.Identity.Name.ToLower() == userProfile.Email.ToLower())
                {
                    if (ModelState.IsValid)
                    {
                        model.Url = Url.ToPublicUrl(Url.Action("Index", "Campaign", new { slug = model.UrlSlug }));
                        campaignMailer.CampaignEmailBlast(model).SendAsync();
                        return Json(new { success = "true" });
                    }
                }
                else
                {
                    // If not, return 403 status code
                    Response.StatusCode = (int) HttpStatusCode.Forbidden;
                }
            }

            return Json(new { success = "false" });
        }

        [ChildActionOnly]
        [OutputCache(Duration = 120, VaryByParam = "id")]
        public ActionResult ProgressBar(int id)
        {
            var campaign = campaignRepository.GetCampaignByID(id);

            if (campaign == null)
            {
                return HttpNotFound("The campaign you are looking for could not be found.");
            }

            var total = campaign.CalculateTotalDonations();
            var percent = (int) ((total / campaign.GoalAmount) * 100);
            var model = new ProgressBarModel
                            {
                                Amount = total,
                                GoalAmount = campaign.GoalAmount,
                                Percent = percent,
                                GoalName = "Raised so far"
                            };

            return View("ProgressBar", model);
        }

        private static void MapCampaign(Campaign campaign, CampaignDetailsModel viewModel)
        {
            campaign.Title = viewModel.Title;
            campaign.Description = viewModel.Description;
            var template = campaign.CauseTemplate;

            if (template.AmountIsConfigurable)
            {
                campaign.GoalAmount = viewModel.GoalAmount;
            }

            if (template.TimespanIsConfigurable)
            {
                campaign.EndDate = viewModel.EndDate;
            }
        }

        private static CampaignEmailBlastModel MapEmailBlast(Campaign campaign)
        {
            var model = Mapper.Map<Campaign, CampaignEmailBlastModel>(campaign);
            var userProfile = campaign.UserProfile;
            model.FirstName = userProfile.FirstName;
            model.LastName = userProfile.LastName;
            model.Email = userProfile.Email;
            return model;
        }

        private CampaignDetailsModel MapDetailsModel(Campaign campaign)
        {
            var model = Mapper.Map<Campaign, CampaignDetailsModel>(campaign);
            var userProfile = campaign.UserProfile;
            var causeTemplate = campaign.CauseTemplate;
            model.FirstName = userProfile.FirstName;
            model.LastName = userProfile.LastName;
			model.UserImagePath = userProfile.ImagePath;
            model.AmountIsConfigurable = causeTemplate.AmountIsConfigurable;
            model.TimespanIsConfigurable = causeTemplate.TimespanIsConfigurable;
			model.IsActive = campaign.IsActive;
            model.CauseTemplateName = causeTemplate.Name;
            model.CauseTempalteImagePath = causeTemplate.ImagePath;
            model.CauseTemplateBeforeImagePath = causeTemplate.BeforeImagePath;
            model.CauseTemplateAfterImagePath = causeTemplate.AfterImagePath;
            model.VideoEmbedHtml = causeTemplate.VideoEmbedHtml;
			model.InstructionsOpenHtml = causeTemplate.InstructionsOpenHtml;
			model.InstructionsClosedHtml = causeTemplate.InstructionsClosedHtml;
            model.CauseTemplateStatisticsHtml = causeTemplate.StatisticsHtml;
            model.Donations = campaign.CampaignDonors
                .Where(d => d.Approved)
                .Select(Mapper.Map<CampaignDonor, DonationDetailsModel>).ToList();

            foreach (var d in model.Donations)
            {
                d.Title = campaign.Title;
            }

            model.CurrentUserIsOwner = (User.Identity.Name.ToLower() == userProfile.Email.ToLower());
            return model;
        }
    }
}