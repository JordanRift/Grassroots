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
using AutoMapper;
using JordanRift.Grassroots.Framework.Data;
using JordanRift.Grassroots.Framework.Entities.Models;
using JordanRift.Grassroots.Web.Models;
using System.Collections.Generic;
using JordanRift.Grassroots.Framework.Services;
using System.Text.RegularExpressions;
using System.Web;
using JordanRift.Grassroots.Framework.Helpers;

namespace JordanRift.Grassroots.Web.Controllers
{
    public class CauseTemplateController : GrassrootsControllerBase
    {
        private readonly ICauseTemplateRepository causeTemplateRepository;
        private readonly ICauseRepository causeRepository;

        public CauseTemplateController(ICauseTemplateRepository causeTemplateRepository, ICauseRepository causeRepository)
        {
            this.causeTemplateRepository = causeTemplateRepository;
            this.causeRepository = causeRepository;
			Mapper.CreateMap<CauseTemplate, CauseTemplateDetailsModel>();
			Mapper.CreateMap<CauseTemplateDetailsModel, CauseTemplate>();
            Mapper.CreateMap<Cause, CauseDetailsModel>();
            Mapper.CreateMap<Recipient, RecipientDetailsModel>();
        }

        ~CauseTemplateController()
        {
            causeTemplateRepository.Dispose();
            causeRepository.Dispose();
        }

        [OutputCache(Duration = 150, VaryByParam = "none")]
        public ActionResult Index()
        {
            using (OrganizationRepository)
            {
                var organization = OrganizationRepository.GetDefaultOrganization(readOnly: false);
                var templates = organization.CauseTemplates.Where(t => t.Active);

                if (templates.Count() == 1)
                {
                    var templateModel = MapCauseTemplateDetails(templates.First());
                    return View("Details", templateModel);
                }

                var model = templates.Where(t => t.Active).Select(Mapper.Map<CauseTemplate, CauseTemplateDetailsModel>).ToList();
                return View("Index", model);
            }
        }

        [OutputCache(Duration = 150, VaryByParam = "id")]
        public ActionResult Details(int id = -1)
        {
            using (causeTemplateRepository)
            {
                var causeTemplate = causeTemplateRepository.GetCauseTemplateByID(id);

                if (causeTemplate == null)
                {
                    return HttpNotFound("The project type you are looking for could not be found.");
                }

                var model = MapCauseTemplateDetails(causeTemplate);
                return View(model);
            }
        }

        public ActionResult Search(int id = -1)
        {
            using (causeTemplateRepository)
            {
                var causeTemplate = causeTemplateRepository.GetCauseTemplateByID(id);

                if (causeTemplate == null)
                {
                    return HttpNotFound("The project type you are looking for could not be found.");
                }

                var model = MapCauseTemplateDetails(causeTemplate, shouldMapCauses: true);
                return View(model);
            }
        }

        [OutputCache(Duration = 150, VaryByParam = "id;referenceNumber")]
        public ActionResult CauseDetails(int id = -1, string referenceNumber = "")
        {
            bool isValid = true;

            if (id == -1 || string.IsNullOrEmpty(referenceNumber))
            {
                isValid = false;
            }

            using (causeRepository)
            {
                var cause = causeRepository.GetCauseByCauseTemplateIdAndReferenceNumber(id, referenceNumber);

                if (cause == null)
                {
                    isValid = false;
                }

                if (!isValid)
                {
                    TempData["ErrorMessage"] = "The project you are looking for was not found. Please try your search again.";
                    var causeTemplateID = id;
                    return RedirectToAction("Search", new { id = causeTemplateID });
                }

                var model = MapCauseDetails(cause);
                return View(model);
            }
        }

		#region Administrative Actions

        [Authorize(Roles = "Administrator")]
		public ActionResult List()
		{
            using (OrganizationRepository)
            {
                var organization = OrganizationRepository.GetDefaultOrganization(readOnly: false);
                var templates = organization.CauseTemplates;
                var model = templates.Select(Mapper.Map<CauseTemplate, CauseTemplateDetailsModel>).ToList();
                return View(model);
            }
		}

		[Authorize( Roles = "Administrator" )]
		public ActionResult Create()
		{
			return View();
		}

		[Authorize( Roles = "Administrator" )] 
		[HttpPost]
		public ActionResult New( CauseTemplateDetailsModel model )
		{
			if ( !ModelState.IsValid )
			{
				TempData["CauseTemplateCreateModel"] = model;
				return RedirectToAction( "Create" );
			}

            using (OrganizationRepository)
            {
                var organization = OrganizationRepository.GetDefaultOrganization(readOnly: false);
                var causeTemplate = Mapper.Map<CauseTemplateDetailsModel, CauseTemplate>(model);
                organization.CauseTemplates.Add(causeTemplate);
                OrganizationRepository.Save();
            }

		    return RedirectToAction( "List" );
		}

		[Authorize( Roles = "Administrator" )] 
		public ActionResult Edit( int id )
		{
            using (causeTemplateRepository)
            {
                var causeTemplate = causeTemplateRepository.GetCauseTemplateByID(id);

                if (causeTemplate != null)
                {
                    CauseTemplateDetailsModel viewModel;

                    if (TempData["CauseTemplateErrors"] != null)
                    {
                        foreach (var error in (List<FileUpload>) TempData["CauseTemplateErrors"])
                        {
                            ModelState.AddModelError(string.Empty, string.Format("{0}: {1}", error.File.FileName, error.ErrorMessage));
                        }
                        viewModel = TempData["CauseTemplateDetailsModel"] as CauseTemplateDetailsModel;
                    }
                    else
                    {
                        viewModel = Mapper.Map<CauseTemplate, CauseTemplateDetailsModel>(causeTemplate);
                    }

                    return View(viewModel);
                }
            }

		    return HttpNotFound( "The project template could not be found." );
		}


		[HttpPost]
		[Authorize( Roles = "Administrator" )] 
		public ActionResult Update( CauseTemplateDetailsModel model )
		{
            using (causeTemplateRepository)
            {
                var causeTemplate = causeTemplateRepository.GetCauseTemplateByID(model.CauseTemplateID);

                if (causeTemplate == null)
                {
                    return HttpNotFound("The project template could not be found.");
                }

                if (!ModelState.IsValid)
                {
                    TempData["CauseTemplateDetailsModel"] = model;
                    return RedirectToAction("Edit", new { id = model.CauseTemplateID });
                }

                MapCauseTemplate(causeTemplate, model);

                // Now save any new images and associate the new image names (URL) with the causeTemplate.
                var results = SaveFiles(causeTemplate);
                var fileErrors = results.Where(r => r.IsError).ToList();
                if (fileErrors.Count() > 0)
                {
                    TempData["CauseTemplateErrors"] = fileErrors;
                    TempData["CauseTemplateDetailsModel"] = model;
                    return RedirectToAction("Edit", new { id = model.CauseTemplateID });
                }

                OrganizationRepository.Save();
            }

		    TempData["UserFeedback"] = "Your changes have been saved. Please allow a few minutes for them to take effect.";
            return RedirectToAction( "List" );
		}

		/// <summary>
		/// Pre-processes the uploaded files and calls a IFileSaveService provider to save
		/// the actual files.  Afterwards, it sets the new file name URLs onto the appropriate
		/// fields of the causeTemplate.
		/// </summary>
		/// <param name="causeTemplate"></param>
		/// <returns></returns>
		private IEnumerable<FileUpload> SaveFiles( CauseTemplate causeTemplate )
		{
			// the regex for a valid image
			Regex imageFilenameRegex = new Regex( @"(.*?)\.(jpg|jpeg|png|gif)$", RegexOptions.IgnoreCase );

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
		    var factory = new FileSaveServiceFactory();
		    IFileSaveService fileSaveService = factory.GetService();
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
		    causeTemplate.CallToAction = model.CallToAction;
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

		private static CauseTemplateDetailsModel MapCauseTemplateDetails(CauseTemplate causeTemplate, bool shouldMapCauses = false)
        {
            var model = Mapper.Map<CauseTemplate, CauseTemplateDetailsModel>(causeTemplate);

            if (shouldMapCauses)
            {
                model.Causes = causeTemplate.Causes.Where(c => c.IsCompleted).Select(Mapper.Map<Cause, CauseDetailsModel>).ToList();
            }

            return model;
        }

        private static CauseDetailsModel MapCauseDetails(Cause cause)
        {
            var model = Mapper.Map<Cause, CauseDetailsModel>(cause);
            model.Region = cause.Region != null ? cause.Region.Name : string.Empty;
            model.Recipients = cause.Recipients.Select(Mapper.Map<Recipient, RecipientDetailsModel>).ToList();
            return model;
        }
    }
}
