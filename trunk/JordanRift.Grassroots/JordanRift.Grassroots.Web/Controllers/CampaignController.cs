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
using JordanRift.Grassroots.Framework.Data;
using JordanRift.Grassroots.Framework.Entities.Models;
using JordanRift.Grassroots.Framework.Helpers;
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
            Mapper.CreateMap<CampaignDonor, DonationDetailsModel>();
            Mapper.CreateMap<CampaignDetailsModel, Campaign>();
        }

        public ActionResult Index(string slug = "")
        {
            var campaign = campaignRepository.GetCampaignByUrlSlug(slug);

            if (slug == null || campaign == null)
            {
                return HttpNotFound("The Campaign you are looking for could not be found");
            }

            //TODO: ViewBag.CampaignEmailBlastModel = 
            var viewModel = Mapper.Map<Campaign, CampaignDetailsModel>(campaign);
            return View("Details", viewModel);
        }

        [Authorize]
        public ActionResult Create()
        {
            var viewModel = TempData["CampaignDetailsModel"] as CampaignDetailsModel ?? new CampaignDetailsModel();
            return View("Create", viewModel);
        }

        [Authorize]
        [HttpPost]
        public ActionResult CreateCampaign(CampaignDetailsModel model)
        {
            if (ModelState.IsValid)
            {
                using (new UnitOfWorkScope())
                {
                    var campaign = Mapper.Map<CampaignDetailsModel, Campaign>(model);
                    var userProfile = userProfileRepository.FindUserProfileByEmail(User.Identity.Name).FirstOrDefault();

                    if (userProfile.GetActiveCampaigns().Any())
                    {
                        TempData["ErrorMessage"] = "You do not have permission to create a new Campaign. You've already got one that's active.";
                        return RedirectToAction("Index");
                    }

                    var organization = userProfile.Organization;
                    var causeTemplate = organization.CauseTemplates.FirstOrDefault(t => t.CauseTemplateID == model.CauseTemplateID);

                    organization.Campaigns.Add(campaign);
                    causeTemplate.Campaigns.Add(campaign);
                    userProfile.Campaigns.Add(campaign);
                    campaignRepository.Save();
                }

                return RedirectToAction("Index", model.UrlSlug);
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

            if (User.Identity.Name != userProfile.Email)
            {
                TempData["ErrorMessage"] = "Sorry, you don't have permission to edit this Campaign.";
                return RedirectToAction("Index", new { slug = campaign.UrlSlug });
            }

            var viewModel = TempData["CampaignDetailsModel"] == null ? 
                TempData["CampaignDetailsModel"] as CampaignDetailsModel : 
                Mapper.Map<Campaign, CampaignDetailsModel>(campaign);
            return View("Edit", viewModel);
        }

        [Authorize]
        [HttpPost]
        public ActionResult Update(CampaignDetailsModel model)
        {
            var userProfile = userProfileRepository.FindUserProfileByEmail(User.Identity.Name).FirstOrDefault();
            var campaign = userProfile.GetActiveCampaigns().FirstOrDefault();

            if (campaign == null)
            {
                return HttpNotFound("The Campaign you are looking for could not be found.");
            }

            if (ModelState.IsValid)
            {
                Map(campaign, model);
                campaignRepository.Save();
                return RedirectToAction("Index");
            }

            TempData["CampaignDetailsModel"] = model;
            return RedirectToAction("Edit");
        }

        [Authorize]
        [HttpPost]
        public ActionResult SendEmail(CampaignEmailBlastModel model)
        {
            campaignMailer.CampaignEmailBlast(model).SendAsync();
            return Json(new { success = "true" });
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
                                Percent = percent,
                                GoalName = "Raised so far"
                            };

            return View("ProgressBar", model);
        }

        private static void Map(Campaign campaign, CampaignDetailsModel viewModel)
        {
            campaign.UrlSlug = viewModel.UrlSlug;
            campaign.Title = viewModel.Title;
            campaign.Description = viewModel.Description;
            campaign.ImagePath = viewModel.ImagePath;
        }
    }
}