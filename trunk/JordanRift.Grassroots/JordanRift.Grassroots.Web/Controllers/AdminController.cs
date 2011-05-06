﻿//
// Copyright © 2011 Jordan Rift, LLC - All Rights Reserved
//
// THIS WORK IS LICENSED UNDER A CREATIVE COMMONS ATTRIBUTION-NONCOMMERCIAL-
// SHAREALIKE 3.0 UNPORTED LICENSE:
// http://creativecommons.org/licenses/by-nc-sa/3.0/
//

using System.Linq;
using System.Web.Mvc;
using JordanRift.Grassroots.Web.Models;
using JordanRift.Grassroots.Framework.Entities.Models;
using AutoMapper;
using JordanRift.Grassroots.Framework.Helpers;

namespace JordanRift.Grassroots.Web.Controllers
{
	[Authorize(Roles = "Administrator")]
	public class AdminController : GrassrootsControllerBase
	{
		public AdminController()
		{
			Mapper.CreateMap<Organization, OrganizationDetailsModel>();
			Mapper.CreateMap<CauseTemplate, CauseTemplateDetailsModel>();
			Mapper.CreateMap<CauseTemplateCreateModel, CauseTemplate>();
		}

		//
		// GET: /Admin/

		public ActionResult Index()
		{
			return View();
		}

		#region CauseTemplate (aka Project) stuff

		public ActionResult EditCauseTemplate(int id)
		{
			var causeTemplate = Organization.CauseTemplates.FirstOrDefault( c => c.CauseTemplateID == id );

			if ( causeTemplate != null )
			{
				var viewModel = Mapper.Map<CauseTemplate, CauseTemplateDetailsModel>( causeTemplate );

				return View( viewModel );
			}

			return HttpNotFound( "The project could not be found." );
		}

		public ActionResult CauseTemplateList()
		{
			var templates = Organization.CauseTemplates;
			var model = templates.Select( Mapper.Map<CauseTemplate, CauseTemplateDetailsModel> ).ToList();
			return View( model );
		}

		public ActionResult CreateCauseTemplate()
		{
			return View();
		}

		[HttpPost]
		[Authorize]
		public ActionResult UpdateCauseTemplate( CauseTemplateDetailsModel model )
		{
			var causeTemplate = Organization.CauseTemplates.FirstOrDefault( c => c.CauseTemplateID == model.CauseTemplateID );

			if ( causeTemplate == null )
			{
				return HttpNotFound( "The project could not be found." );
			}

			if ( !ModelState.IsValid )
			{
				TempData["CauseTemplateDetailsModel"] = model;
				return RedirectToAction( "EditCauseTemplate", "Admin" );
			}

			MapCauseTemplate( causeTemplate, model );
			OrganizationRepository.Save();

			return RedirectToAction( "CauseTemplateList", "Admin" );
		}

		[HttpPost]
		[Authorize]
		public ActionResult CreateCauseTemplate( CauseTemplateCreateModel model )
		{
			if ( ! ModelState.IsValid )
			{
				TempData["CauseTemplateCreateModel"] = model;
				return RedirectToAction( "CreateCauseTemplate", "Admin" );
			}

			var causeTemplate = Mapper.Map<CauseTemplateCreateModel, CauseTemplate>( model );
			Organization.CauseTemplates.Add( causeTemplate );
			OrganizationRepository.Save();

			return RedirectToAction( "CauseTemplateList", "Admin" );
		}


		private static void MapCauseTemplate( CauseTemplate causeTemplate, CauseTemplateDetailsModel model )
		{
			causeTemplate.Name = model.Name;
			causeTemplate.ActionVerb = model.ActionVerb;
			causeTemplate.Active = model.Active;
			causeTemplate.AmountIsConfigurable = model.AmountIsConfigurable;
			causeTemplate.DefaultAmount = model.DefaultAmount;
			causeTemplate.DefaultTimespanInDays = model.DefaultTimespanInDays;
			causeTemplate.DescriptionHtml = model.DescriptionHtml;
			causeTemplate.GoalName = model.GoalName;
			causeTemplate.ImagePath = model.ImagePath;
			causeTemplate.Summary = model.Summary;
			causeTemplate.TimespanIsConfigurable = model.TimespanIsConfigurable;
			causeTemplate.VideoEmbedHtml = model.VideoEmbedHtml;
		}

		#endregion

		public ActionResult EditOrganization()
		{
			// Perhaps someday we'll pull this from the route?
			var organization = OrganizationRepository.GetDefaultOrganization();

			if ( organization != null )
			{
				OrganizationDetailsModel viewModel;

				if ( TempData["OrganizationEditModel"] != null )
				{
					viewModel = TempData["OrganizationEditModel"] as OrganizationDetailsModel;
				}
				else
				{
					viewModel = Mapper.Map<Organization, OrganizationDetailsModel>( organization );
				}

				return View( viewModel );
			}

			return HttpNotFound( "The organization could not be found." );
		}

		[HttpPost]
		public ActionResult UpdateOrganization( OrganizationDetailsModel model )
		{
			var organization = OrganizationRepository.GetDefaultOrganization();

			if ( ModelState.IsValid )
			{
				MapOrganization( organization, model );
				OrganizationRepository.Save();
				
				return RedirectToAction( "Index", "Admin" );
			}

			TempData["OrganizationEditModel"] = model;
			return RedirectToAction( "EditOrganization", "Admin" );
		}

		private static void MapOrganization( Organization organization, OrganizationDetailsModel model )
		{
			organization.Name = model.Name;
		    organization.Tagline = model.Tagline;
			organization.ContactEmail = model.ContactEmail;
			organization.ContactPhone = model.ContactPhone;
		    organization.YtdGoal = model.YtdGoal;
		    organization.FiscalYearStartMonth = model.FiscalYearStartMonth;
		    organization.FiscalYearStartDay = model.FiscalYearStartDay;
		    organization.Summary = model.Summary;
		    organization.DescriptionHtml = model.DescriptionHtml;
		    organization.PaymentGatewayApiUrl = model.PaymentGatewayApiUrl ?? "";
			organization.PaymentGatewayApiKey = model.PaymentGatewayApiKey ?? "";
			organization.PaymentGatewayApiSecret = model.PaymentGatewayApiSecret ?? "";
			organization.PaymentGatewayType = model.PaymentGatewayType;
		    organization.FacebookPageUrl = model.FacebookPageUrl;
			organization.TwitterName = model.TwitterName;
		    organization.BlogRssUrl = model.BlogRssUrl;
			organization.VideoEmbedHtml = model.VideoEmbedHtml;
		    organization.ThemeName = model.ThemeName;
		}
	}
}
