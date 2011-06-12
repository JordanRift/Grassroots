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
        private readonly IOrganizationRepository organizationRepository;

        public CauseTemplateController(IOrganizationRepository organizationRepository, ICauseTemplateRepository causeTemplateRepository)
        {
            this.causeTemplateRepository = causeTemplateRepository;
            this.organizationRepository = organizationRepository;
            Mapper.CreateMap<CauseTemplate, CauseTemplateDetailsModel>();
        }

        [OutputCache(Duration = 150, VaryByParam = "none")]
        public ActionResult Index()
        {
            var organization = OrganizationRepository.GetDefaultOrganization(readOnly: true);
            var templates = organization.CauseTemplates;
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

        public ActionResult Search(int id = -1)
        {
            return View();
        }
    }
}
