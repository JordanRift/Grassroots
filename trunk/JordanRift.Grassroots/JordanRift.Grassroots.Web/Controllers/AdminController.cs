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
using System.Web;
using System.IO;
using System.Text.RegularExpressions;
using System.Linq;
using System.Web.Mvc;

using AutoMapper;
using JordanRift.Grassroots.Web.Models;
using JordanRift.Grassroots.Framework.Entities.Models;

using JordanRift.Grassroots.Framework.Services;
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
			Mapper.CreateMap<CauseTemplateDetailsModel, CauseTemplate>();
		    Mapper.CreateMap<Cause, CauseDetailsModel>();
		    Mapper.CreateMap<Recipient, RecipientDetailsModel>();
		}

		//
		// GET: /Admin/

		public ActionResult Index()
		{
			return View();
		}

		#region CauseTemplate (aka Project) stuff


		#endregion

		public ActionResult EditOrganization()
		{
			// Perhaps someday we'll pull this from the route?
			var organization = OrganizationRepository.GetDefaultOrganization(readOnly: true);

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
			var organization = OrganizationRepository.GetDefaultOrganization(readOnly: false);

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
		    organization.SummaryHtml = model.SummaryHtml;
		    organization.DescriptionHtml = model.DescriptionHtml;
		    organization.PaymentGatewayApiUrl = model.PaymentGatewayApiUrl ?? "";
			organization.PaymentGatewayApiKey = model.PaymentGatewayApiKey ?? "";
			organization.PaymentGatewayApiSecret = model.PaymentGatewayApiSecret ?? "";
			organization.PaymentGatewayType = model.PaymentGatewayType;
			organization.PublicWebsiteUrl = model.PublicWebsiteUrl;
			organization.PublicAboutPageUrl = model.PublicAboutPageUrl;
		    organization.FacebookPageUrl = model.FacebookPageUrl;
			organization.TwitterName = model.TwitterName;
		    organization.BlogRssUrl = model.BlogRssUrl;
			organization.VideoEmbedHtml = model.VideoEmbedHtml;
		    organization.ThemeName = model.ThemeName;
		}
	}
}
