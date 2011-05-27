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
using JordanRift.Grassroots.Web.Models;
using JordanRift.Grassroots.Framework.Entities.Models;
using AutoMapper;

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
		}

		//
		// GET: /Admin/

		public ActionResult Index()
		{
			return View();
		}

		#region CauseTemplate (aka Project) stuff

		public ActionResult CauseTemplateList()
		{
		    var organization = OrganizationRepository.GetDefaultOrganization(readOnly: true);
			var templates = organization.CauseTemplates;
			var model = templates.Select( Mapper.Map<CauseTemplate, CauseTemplateDetailsModel> ).ToList();
			return View( model );
		}

        //public JsonResult LoadCauseTemplateList(int page = 1, int rows = 10)
        //{
        //    var organization = OrganizationRepository.GetDefaultOrganization(readOnly: true);
        //    var theTotal = organization.CauseTemplates.Count;
        //    //var pageNumber = page;
        //    var templates = organization.CauseTemplates.Skip((page - 1) * rows).Take(rows);

        //    return Json(new {
        //        rows = templates,
        //        totalrows = theTotal,
        //        totals = templates
        //    }, JsonRequestBehavior.AllowGet);
        //}

		public ActionResult CreateCauseTemplate()
		{
			return View();
		}

        [HttpPost]
        public ActionResult NewCauseTemplate(CauseTemplateDetailsModel model)
        {
            if (!ModelState.IsValid)
            {
                TempData["CauseTemplateCreateModel"] = model;
                return RedirectToAction("CreateCauseTemplate", "Admin");
            }

            var organization = OrganizationRepository.GetDefaultOrganization(readOnly: false);
            var causeTemplate = Mapper.Map<CauseTemplateDetailsModel, CauseTemplate>(model);
            organization.CauseTemplates.Add(causeTemplate);
            OrganizationRepository.Save();

            return RedirectToAction("CauseTemplateList", "Admin");
        }

        public ActionResult EditCauseTemplate(int id)
        {
            var organization = OrganizationRepository.GetDefaultOrganization(readOnly: true);
            var causeTemplate = organization.CauseTemplates.FirstOrDefault(c => c.CauseTemplateID == id);

            if (causeTemplate != null)
            {
                var viewModel = Mapper.Map<CauseTemplate, CauseTemplateDetailsModel>(causeTemplate);
                return View(viewModel);
            }

            return HttpNotFound("The project template could not be found.");
        }

		[HttpPost]
		public ActionResult UpdateCauseTemplate( CauseTemplateDetailsModel model )
		{
		    var organization = OrganizationRepository.GetDefaultOrganization(readOnly: false);
			var causeTemplate = organization.CauseTemplates.FirstOrDefault( c => c.CauseTemplateID == model.CauseTemplateID );

			if ( causeTemplate == null )
			{
				return HttpNotFound( "The project template could not be found." );
			}

			if ( !ModelState.IsValid )
			{
				TempData["CauseTemplateDetailsModel"] = model;
				return RedirectToAction( "EditCauseTemplate", "Admin", new { id = model.CauseTemplateID } );
			}

			MapCauseTemplate( causeTemplate, model );
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
		    organization.FacebookPageUrl = model.FacebookPageUrl;
			organization.TwitterName = model.TwitterName;
		    organization.BlogRssUrl = model.BlogRssUrl;
			organization.VideoEmbedHtml = model.VideoEmbedHtml;
		    organization.ThemeName = model.ThemeName;
		}
	}
}
