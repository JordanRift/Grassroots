//
// Copyright © 2011 Jordan Rift, LLC - All Rights Reserved
//
// THIS WORK IS LICENSED UNDER A CREATIVE COMMONS ATTRIBUTION-NONCOMMERCIAL-
// SHAREALIKE 3.0 UNPORTED LICENSE:
// http://creativecommons.org/licenses/by-nc-sa/3.0/
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
        public ActionResult Create()
        {
            var viewModel = TempData["CampaignDetailsModel"] as CampaignCreateModel ?? new CampaignCreateModel();
            var organization = OrganizationRepository.GetDefaultOrganization(readOnly: true);
            var templates = organization.CauseTemplates.Where(ct => ct.Active);

            if (templates.Count() > 1)
            {
                viewModel.ShouldRenderDropdown = true;
            }
            else
            {
                viewModel.ShouldRenderDropdown = false;
                viewModel.CauseTemplateID = templates.First().CauseTemplateID;
            }

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
            campaign.UrlSlug = viewModel.UrlSlug;
            campaign.Title = viewModel.Title;
            campaign.Description = viewModel.Description;
            //campaign.ImagePath = viewModel.ImagePath;
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
            model.AmountIsConfigurable = causeTemplate.AmountIsConfigurable;
            model.TimespanIsConfigurable = causeTemplate.TimespanIsConfigurable;
            model.VideoEmbedHtml = causeTemplate.VideoEmbedHtml;
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