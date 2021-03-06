﻿//
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
using System.Globalization;
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
        private const string ADMIN_ROLES = "Root,Admnistrator";
        private readonly ICampaignRepository campaignRepository;
        private readonly ICauseTemplateRepository causeTemplateRepository;
        private readonly IUserProfileRepository userProfileRepository;
        private readonly ICampaignMailer campaignMailer;

        public CampaignController(ICampaignRepository campaignRepository, ICauseTemplateRepository causeTemplateRepository, IUserProfileRepository userProfileRepository, ICampaignMailer campaignMailer)
        {
            this.campaignRepository = campaignRepository;
            this.causeTemplateRepository = causeTemplateRepository;
            this.userProfileRepository = userProfileRepository;
            this.campaignMailer = campaignMailer;
            Mapper.CreateMap<Campaign, CampaignDetailsModel>();
            Mapper.CreateMap<UserProfile, CampaignDetailsModel>();
            Mapper.CreateMap<CauseTemplate, CampaignDetailsModel>();
            Mapper.CreateMap<Cause, CauseDetailsModel>();
            Mapper.CreateMap<Recipient, RecipientDetailsModel>();
            Mapper.CreateMap<CauseTemplate, CauseTemplateDetailsModel>();
            Mapper.CreateMap<Campaign, CampaignEmailBlastModel>();
            Mapper.CreateMap<CampaignDonor, DonationDetailsModel>();
            Mapper.CreateMap<CampaignDetailsModel, Campaign>();
            Mapper.CreateMap<CampaignCreateModel, Campaign>();
            Mapper.CreateMap<Campaign, CampaignAdminModel>();
            Mapper.CreateMap<CampaignAdminModel, Campaign>();
            Mapper.CreateMap<CampaignDonor, DonationAdminModel>();
        }

        ~CampaignController()
        {
            campaignRepository.Dispose();
            causeTemplateRepository.Dispose();
            userProfileRepository.Dispose();
        }

        public ActionResult Index(string slug = "")
        {
            using (new UnitOfWorkScope())
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
        }
        
        [Authorize]
        [OutputCache(Duration = 150, VaryByParam = "none")]
        public ActionResult GetStarted()
        {
            GetStartedModel viewModel;

            using (causeTemplateRepository)
            {
                var activeCauseTemplates = causeTemplateRepository.FindActiveCauseTemplates();
                viewModel = new GetStartedModel
                                    {
                                        CampaignType = (int) CampaignType.Unknown,
                                        CauseTemplates = new List<CauseTemplateDetailsModel>()
                                    };

                foreach (var causeTemplate in activeCauseTemplates)
                {
                    viewModel.CauseTemplates.Add(new CauseTemplateDetailsModel
                                                     {
                                                         CauseTemplateID = causeTemplate.CauseTemplateID,
                                                         Name = causeTemplate.Name,
                                                         ImagePath = causeTemplate.ImagePath
                                                     });
                }
            }

            var defaultCauseTemplate = viewModel.CauseTemplates.FirstOrDefault();

            if (defaultCauseTemplate != null)
            {
                viewModel.CauseTemplateID = defaultCauseTemplate.CauseTemplateID;
            }

            return View(viewModel);
        }

        [HttpGet]
        [Authorize]
        public ActionResult Create(int id)
        {
            return Create(new GetStartedModel
                              {
                                  CampaignType = (int) CampaignType.Other,
                                  CauseTemplateID = id
                              });
        }

        [Authorize]
        public ActionResult Create(GetStartedModel model)
        {
            var viewModel = TempData["CampaignDetailsModel"] as CampaignCreateModel ?? new CampaignCreateModel();

            if (model.CauseTemplateID != -1)
            {
                using (OrganizationRepository)
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
            }

            viewModel.CampaignType = model.CampaignType;
            return View("Create", viewModel);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken(Salt = "CampaignCreate")]
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
						return RedirectToAction( "Index", new { controller = "UserProfile", id = "" } );
                    }

                    var organization = userProfile.Organization;
                    var causeTemplate = organization.CauseTemplates.FirstOrDefault(t => t.CauseTemplateID == model.CauseTemplateID);

                    if (!string.IsNullOrEmpty(model.AmountString) && causeTemplate.AmountIsConfigurable)
                    {
                        campaign.GoalAmount = decimal.Parse(model.AmountString);
                    }
                    else
                    {
                        campaign.GoalAmount = causeTemplate.DefaultAmount;
                    }

                    campaign.StartDate = DateTime.Now;
                    campaign.EndDate = causeTemplate.CutOffDate == null
                                           ? DateTime.Now.AddDays(causeTemplate.DefaultTimespanInDays)
                                           : causeTemplate.CutOffDate.Value;
                    campaign.ImagePath = string.Empty;  // TODO: Refactor to either accept a file upload or remove field from db
                    campaign.Description = "You should say something about your campaign here...";
                    campaign.IsGeneralFund = false;

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
            using (campaignRepository)
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

                var viewModel = MapDetailsModel(campaign);
                ViewBag.EmailBlastModel = MapEmailBlast(campaign);
                return View("Edit", viewModel);
            }
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken(Salt = "EditCampaign")]
        public ActionResult Update(CampaignDetailsModel model, int id = -1)
        {
            using (campaignRepository)
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
        }

        // TODO: Find a good way to validate AJAX requests. Built in Anti-CSRF stuff looks for Request.Form and will fail any request that doesn't have it.
        [Authorize]
        [HttpPost]
        public ActionResult SendEmail(CampaignEmailBlastModel model)
        {
            using (campaignRepository)
            {
                var campaign = campaignRepository.GetCampaignByUrlSlug(model.UrlSlug);

                if (campaign != null)
                {
                    var userProfile = campaign.UserProfile;

                    // Only allow email to send if the campaign owner is currently logged in
                    if (User.Identity.Name.Equals(userProfile.Email, StringComparison.CurrentCultureIgnoreCase))
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
            }

            return Json(new { success = "false" });
        }

        [ChildActionOnly]
        [OutputCache(Duration = 120, VaryByParam = "id")]
        public ActionResult ProgressBar(int id)
        {
            ProgressBarModel model;

            using (campaignRepository)
            {
                var campaign = campaignRepository.GetCampaignByID(id);

                if (campaign == null)
                {
                    return HttpNotFound("The campaign you are looking for could not be found.");
                }

                var total = campaign.CalculateTotalDonations();
                var percent = total > campaign.GoalAmount ? 100 : (int) ((total / campaign.GoalAmount) * 100);
                model = new ProgressBarModel
                            {
                                Amount = total,
                                GoalAmount = campaign.GoalAmount,
                                Percent = percent,
                                GoalName = "Raised so far",
                                DisplayTooltip = false
                            };
            }

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

        private CampaignEmailBlastModel MapEmailBlast(Campaign campaign)
        {
            var organization = OrganizationRepository.GetDefaultOrganization(readOnly: true);
            var model = Mapper.Map<Campaign, CampaignEmailBlastModel>(campaign);
            var userProfile = campaign.UserProfile;
            model.FirstName = userProfile.FirstName;
            model.LastName = userProfile.LastName;
            //model.Email = userProfile.Email;
            model.Email = organization.ContactEmail;
            return model;
        }

        private CampaignDetailsModel MapDetailsModel(Campaign campaign)
        {
            var model = Mapper.Map<Campaign, CampaignDetailsModel>(campaign);
            var userProfile = campaign.UserProfile;
            var causeTemplate = campaign.CauseTemplate;
            model.FirstName = userProfile.FirstName;
            model.LastName = userProfile.LastName;
            model.UserProfileID = userProfile.UserProfileID;
			model.UserImagePath = userProfile.ImagePath;
            model.AmountIsConfigurable = causeTemplate.AmountIsConfigurable;
            model.TimespanIsConfigurable = causeTemplate.TimespanIsConfigurable;
			model.IsActive = campaign.IsActive;
            model.CauseTemplateName = causeTemplate.Name;
            model.CauseTempalteImagePath = causeTemplate.ImagePath;
            model.CauseTemplateBeforeImagePath = causeTemplate.BeforeImagePath;
            model.CauseTemplateAfterImagePath = causeTemplate.AfterImagePath;
            model.CallToAction = causeTemplate.CallToAction;
            model.VideoEmbedHtml = causeTemplate.VideoEmbedHtml;
			model.InstructionsOpenHtml = causeTemplate.InstructionsOpenHtml;
			model.InstructionsClosedHtml = causeTemplate.InstructionsClosedHtml;
            model.Donations = campaign.CampaignDonors
                .Where(d => d.Approved)
                .Select(Mapper.Map<CampaignDonor, DonationDetailsModel>).ToList();

            foreach (var d in model.Donations)
            {
                d.Title = campaign.Title;

                // Make sure DisplayName gets set for records that existed prior to update.
                if (string.IsNullOrEmpty(d.DisplayName))
                {
                    d.DisplayName = string.Format("{0} {1}", d.FirstName, d.LastName);
                }
            }

            model.CurrentUserIsOwner = (User.Identity.Name.ToLower() == userProfile.Email.ToLower());
            return model;
        }

#region Campaign Administration

        [Authorize(Roles = ADMIN_ROLES)]
        public ActionResult List()
        {
            using (campaignRepository)
            {
                // Calling ToList() here to prevent possible InvalidOperationException on databases that aren't configured
                // to allow multiple result sets.
                var list = campaignRepository.FindAllCampaigns().ToList();
                var viewModel = new List<CampaignAdminModel>();

                foreach (var c in list)
                {
                    var model = MapAdminModel(c);
                    model.CauseName = c.Cause != null ? c.Cause.Name : string.Empty;
                    viewModel.Add(model);
                }

                return View(viewModel);
            }
        }

        [Authorize(Roles = ADMIN_ROLES)]
        public ActionResult Admin(int id = -1)
        {
            CampaignAdminModel viewModel;

            if (TempData["ModelErrors"] != null)
            {
                var errors = TempData["ModelErrors"] as IEnumerable<string> ?? new List<string>();

                foreach (var error in errors)
                {
                    ModelState.AddModelError("", error);
                }

                viewModel = TempData["CampaignAdminModel"] as CampaignAdminModel;
            }
            else
            {
                using (campaignRepository)
                {
                    var campaign = campaignRepository.GetCampaignByID(id);

                    if (campaign == null)
                    {
                        return HttpNotFound("The campaign you are looking for could not be found.");
                    }

                    viewModel = MapAdminModel(campaign);
                }
            }
            
            return View(viewModel);
        }

        [HttpPost]
        [Authorize(Roles = ADMIN_ROLES)]
        [ValidateAntiForgeryToken(Salt = "AdminUpdateCampaign")]
        public ActionResult AdminUpdate(CampaignAdminModel model)
        {
            if (!ModelState.IsValid)
            {
                TempData["ModelErrors"] = FindModelErrors();
                TempData["CampaignAdminModel"] = model;
                return RedirectToAction("Admin");
            }

            using (campaignRepository)
            {
                var campaign = campaignRepository.GetCampaignByID(model.CampaignID);

                if (campaign == null)
                {
                    return HttpNotFound("The campaign you are looking for could not be found.");
                }

                MapCampaign(campaign, model);
                campaignRepository.Save();
                TempData["UserFeedback"] = string.Format("'{0}' has been updated successfully.", campaign.Title);
                return RedirectToAction("List");
            }
        }

        [HttpDelete]
        [Authorize(Roles = ADMIN_ROLES)]
        public ActionResult Destroy(int id = -1)
        {
            using (campaignRepository)
            {
                var campaign = campaignRepository.GetCampaignByID(id);

                if (campaign == null)
                {
                    return HttpNotFound("The campaign you are looking for could not be found.");
                }

                // To prevent data loss, move donations from this campaign into current general fund.
                var generalFund = campaignRepository.GetDefaultCampaign();
                var donations = campaign.CampaignDonors.ToList();

                foreach (var d in donations)
                {
                    campaign.CampaignDonors.Remove(d);
                    generalFund.CampaignDonors.Add(d);
                }

                campaignRepository.Delete(campaign);
                campaignRepository.Save();
            }

            if (Request.IsAjaxRequest())
            {
                return Json(new { success = true });
            }

            // TODO: Consider adding message to inform user of successful delete.
            return RedirectToAction("List");
        }

        private CampaignAdminModel MapAdminModel(Campaign campaign)
        {
            var model = Mapper.Map<Campaign, CampaignAdminModel>(campaign);
            model.AmountString = campaign.GoalAmount.ToString(CultureInfo.InvariantCulture);
            var userProfile = campaign.UserProfile;
            model.UserProfileID = userProfile.UserProfileID;
            model.FirstName = userProfile.FirstName;
            model.LastName = userProfile.LastName;
            var cause = campaign.Cause;

            if (cause != null)
            {
                model.CauseID = cause.CauseID;
                model.CauseName = cause.Name;
            }

            model.Donations = campaign.CampaignDonors.Select(Mapper.Map<CampaignDonor, DonationAdminModel>).ToList();
            return model;
        }

        private void MapCampaign(Campaign campaign, CampaignAdminModel model)
        {
            campaign.Title = model.Title;
            campaign.Description = model.Description;
            campaign.StartDate = model.StartDate;
            campaign.EndDate = model.EndDate;
            campaign.UrlSlug = model.UrlSlug;
            decimal amount;

            if (decimal.TryParse(model.AmountString, out amount))
            {
                campaign.GoalAmount = amount;
            }
        }

#endregion
    }
}