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
using System.Collections.Generic;
using System.Web;
using System.IO;
using System;
using System.Text.RegularExpressions;

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
					foreach ( var error in ( List<ViewDataUploadFilesResult>)TempData["CauseTemplateErrors"] )
					{
						ModelState.AddModelError( string.Empty, string.Format( "{0}: {1}", error.OriginalName, error.ErrorMessage ) );					
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
			var fileErrors = results.Where( r => r.IsError == true ).ToList<ViewDataUploadFilesResult>();
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
		/// TODO: implement this as a provider so we can create and use other
		/// file save providers (Amazon S3, Azure, etc.) easily.
		/// </summary>
		/// <param name="causeTemplate"></param>
		private List<ViewDataUploadFilesResult> SaveFiles( CauseTemplate causeTemplate )
		{
			// the regex for an image
		    Regex imageFilenameRegex = new Regex(@"(.*?)\.(jpg|jpeg|png|gif)$", RegexOptions.IgnoreCase);

			int index = 0;
			var results = new List<ViewDataUploadFilesResult>();

			foreach ( string fileKey in Request.Files )
			{
				index++;
				HttpPostedFileBase file = Request.Files[fileKey] as HttpPostedFileBase;

				if ( file.ContentLength == 0 )
				{
					continue;
				}

				var fileResult = new ViewDataUploadFilesResult(){OriginalName = file.FileName, Length = file.ContentLength, IsError = false};

				try
				{
					if ( imageFilenameRegex.IsMatch( file.FileName ) )
					{
						string fileExtension = Path.GetExtension( file.FileName );
						string newFileName = string.Format( "{0}{1}", Guid.NewGuid().ToString(), fileExtension );
						string relativePathFileName = Path.Combine( "Content", "UserContent", newFileName );
						string physicalPathFileName = Path.Combine( AppDomain.CurrentDomain.BaseDirectory, relativePathFileName );
						file.SaveAs( physicalPathFileName );
						fileResult.Name = relativePathFileName;
						string oldFile = string.Empty;
						switch ( index )
						{
							case 1:
								oldFile = causeTemplate.ImagePath;
								causeTemplate.ImagePath = relativePathFileName;
								break;
							case 2:
								oldFile = causeTemplate.BeforeImagePath;
								causeTemplate.BeforeImagePath = relativePathFileName;
								break;
							case 3:
								oldFile = causeTemplate.AfterImagePath;
								causeTemplate.AfterImagePath = relativePathFileName;
								break;
							default:
								break;
						}

						if ( oldFile != null && oldFile != string.Empty && ! oldFile.ToLower().StartsWith("http") )
						{
							oldFile = Request.MapPath( Path.Combine( "~", oldFile ) );
							if ( System.IO.File.Exists( oldFile ) )
							{
								System.IO.File.Delete( oldFile );
							}
						}
					}
					else
					{
						fileResult.IsError = true;
						fileResult.ErrorMessage = "Invalid file type.";
					}
				}
				catch ( HttpException  ex )
				{
					fileResult.IsError = true;
					fileResult.ErrorMessage = string.Format( "Unable to save file. {0}", ex.Message);
				};

				results.Add( fileResult );

			}
			return results;
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
		    organization.FacebookPageUrl = model.FacebookPageUrl;
			organization.TwitterName = model.TwitterName;
		    organization.BlogRssUrl = model.BlogRssUrl;
			organization.VideoEmbedHtml = model.VideoEmbedHtml;
		    organization.ThemeName = model.ThemeName;
		}
	}

	/// <summary>
	/// helper class for file uploading (TODO: possibly move to somewhere more appropriate or not?)
	/// </summary>
	public class ViewDataUploadFilesResult
	{
		public string OriginalName { get; set; } 
		public string Name { get; set; }
		public int Length { get; set; }
		public bool IsError { get; set; }
		public string ErrorMessage { get; set; }
	}

}
