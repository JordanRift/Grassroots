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
				CauseTemplateDetailsModel viewModel;

				if ( TempData["CauseTemplateErrors"] != null )
				{
					foreach ( var error in ( List<FileUpload>)TempData["CauseTemplateErrors"] )
					{
						ModelState.AddModelError( string.Empty, string.Format( "{0}: {1}", error.File.FileName, error.ErrorMessage ) );					
					}
					viewModel = TempData["CauseTemplateDetailsModel"] as CauseTemplateDetailsModel;
				}
				else
				{
					viewModel = Mapper.Map<CauseTemplate, CauseTemplateDetailsModel>( causeTemplate );
				}

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

			if ( ! ModelState.IsValid )
			{
				TempData["CauseTemplateDetailsModel"] = model;
				return RedirectToAction( "EditCauseTemplate", "Admin", new { id = model.CauseTemplateID } );
			}

			MapCauseTemplate( causeTemplate, model );

			// Now save any new images and associate the new image names (URL) with the causeTemplate.
			var results = SaveFiles( causeTemplate );
			var fileErrors = results.Where( r => r.IsError == true ).ToList<FileUpload>();
			if ( fileErrors.Count() > 0 )
			{
				TempData["CauseTemplateErrors"] = fileErrors;
				TempData["CauseTemplateDetailsModel"] = model;
				return RedirectToAction( "EditCauseTemplate", "Admin", new { id = model.CauseTemplateID } );
			}

			OrganizationRepository.Save();

			return RedirectToAction( "CauseTemplateList", "Admin" );
		}

		/// <summary>
		/// Pre-processes the uploaded files and calls a IFileSaveService provider to save
		/// the actual files.  Afterwards, it sets the new file name URLs onto the appropriate
		/// fields of the causeTemplate.
		/// </summary>
		/// <param name="causeTemplate"></param>
		/// <returns></returns>
		private List<FileUpload> SaveFiles( CauseTemplate causeTemplate )
		{
			// the regex for a valid image
		    Regex imageFilenameRegex = new Regex(@"(.*?)\.(jpg|jpeg|png|gif)$", RegexOptions.IgnoreCase);

			int index = 0;
			var fileUploadList = new List<FileUpload>();

			// Pre process the list of files and verify each is valid before calling IFileSaveService
			foreach ( string fileKey in Request.Files )
			{
				index++;
				HttpPostedFileBase file = Request.Files[fileKey] as HttpPostedFileBase;

				// No file was uploaded (form element was left blank)
				if ( file.ContentLength == 0 )
				{
					continue;
				}

				// Create a new FileUpload item
				var fileUpload = new FileUpload() { File = file, Length = file.ContentLength, IsError = false, Index = index };

				// Pull the existing (soon to be previous) file name from the CauseTemplate
				// and set into the fileUpload because the provider may choose to delete
				// the previous file.
				if ( imageFilenameRegex.IsMatch( file.FileName ) )
				{
					switch ( index )
					{
						case 1:
							fileUpload.PreviousFileName = causeTemplate.ImagePath;
							break;
						case 2:
							fileUpload.PreviousFileName = causeTemplate.BeforeImagePath;
							break;
						case 3:
							fileUpload.PreviousFileName = causeTemplate.AfterImagePath;
							break;
						default:
							break;
					}
				}
				else
				{
					fileUpload.IsError = true;
					fileUpload.ErrorMessage = "Invalid file type.";
				}

				fileUploadList.Add( fileUpload );
			}

			// Get the configured IFileSaveService from the factory
			IFileSaveService fileSaveService = FileSaveServiceFactory.GetFileSaveService();
			fileSaveService.SaveFiles( fileUploadList );

			// Post process files to set the new file name (URL) to the campaign's image
			foreach ( var fileItem in fileUploadList )
			{
				string oldFile = string.Empty;
				switch ( fileItem.Index )
				{
					case 1:
						causeTemplate.ImagePath = fileItem.NewFileName;
						break;
					case 2:
						causeTemplate.BeforeImagePath = fileItem.NewFileName;
						break;
					case 3:
						causeTemplate.AfterImagePath = fileItem.NewFileName;
						break;
					default:
						break;
				}
			}

			return fileUploadList;
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
		    causeTemplate.BeforeImagePath = model.BeforeImagePath;
		    causeTemplate.AfterImagePath = model.AfterImagePath;
			causeTemplate.Summary = model.Summary;
			causeTemplate.TimespanIsConfigurable = model.TimespanIsConfigurable;
			causeTemplate.VideoEmbedHtml = model.VideoEmbedHtml;
			causeTemplate.InstructionsOpenHtml = model.InstructionsOpenHtml;
			causeTemplate.InstructionsClosedHtml = model.InstructionsClosedHtml;
		    causeTemplate.StatisticsHtml = model.StatisticsHtml;
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
