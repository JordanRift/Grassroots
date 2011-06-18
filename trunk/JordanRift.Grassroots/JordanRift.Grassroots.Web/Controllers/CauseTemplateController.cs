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
            Mapper.CreateMap<Cause, CauseDetailsModel>();
            Mapper.CreateMap<Recipient, RecipientDetailsModel>();
        }

        [OutputCache(Duration = 150, VaryByParam = "none")]
        public ActionResult Index()
        {
            var organization = OrganizationRepository.GetDefaultOrganization(readOnly: true);
            var templates = organization.CauseTemplates.Where(t => t.Active);

            if (templates.Count() == 1)
            {
                var templateModel = Mapper.Map<CauseTemplate, CauseTemplateDetailsModel>(templates.First());
                return View("Details", templateModel);
            }

            var model = templates.Where(t => t.Active).Select(Mapper.Map<CauseTemplate, CauseTemplateDetailsModel>).ToList();
            return View(model);
        }

        [OutputCache(Duration = 150, VaryByParam = "id")]
        public ActionResult Details(int id = -1)
        {
            var causeTemplate = causeTemplateRepository.GetCauseTemplateByID(id);

            if (causeTemplate == null)
            {
                return HttpNotFound("The project type you are looking for could not be found.");
            }

            var model = Mapper.Map<CauseTemplate, CauseTemplateDetailsModel>(causeTemplate);
            return View(model);
        }

        [OutputCache(Duration = 150, VaryByParam = "id")]
        public ActionResult Search(int id = -1)
        {
            var causeTemplate = causeTemplateRepository.GetCauseTemplateByID(id);

            if (causeTemplate == null)
            {
                return HttpNotFound("The project type you are looking for could not be found.");
            }

            var model = Mapper.Map<CauseTemplate, CauseTemplateDetailsModel>(causeTemplate);
            return View(model);
        }

        [OutputCache(Duration = 150, VaryByParam = "referenceNumber")]
        public ActionResult CauseDetails(string referenceNumber = "")
        {
            var cause = causeRepository.GetCauseByReferenceNumber(referenceNumber);

            if (cause == null)
            {
                return HttpNotFound("The project you are looking for could not be found.");
            }

            var model = MapCauseDetails(cause);
            return View(model);
        }

        private CauseDetailsModel MapCauseDetails(Cause cause)
        {
            var model = Mapper.Map<Cause, CauseDetailsModel>(cause);
            model.Region = cause.Region != null ? cause.Region.Name : string.Empty;
            model.Recipients = cause.Recipients.Select(Mapper.Map<Recipient, RecipientDetailsModel>).ToList();
            return model;
        }
    }
}
