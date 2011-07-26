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

using System.Web.Mvc;
using AutoMapper;
using JordanRift.Grassroots.Framework.Entities.Models;
using JordanRift.Grassroots.Web.Models;

namespace JordanRift.Grassroots.Web.Controllers
{
    public class ErrorController : GrassrootsControllerBase
    {
        public ErrorController()
        {
            Mapper.CreateMap<Organization, OrganizationDetailsModel>();
            Mapper.CreateMap<OrganizationSetting, OrganizationSettingModel>();
        }

        [OutputCache(Duration = 60, VaryByParam = "aspxerrorpath")]
        public ActionResult Index()
        {
            using (OrganizationRepository)
            {
                var organization = OrganizationRepository.GetDefaultOrganization(readOnly: true);
                var model = Mapper.Map<Organization, OrganizationDetailsModel>(organization);
                return View("Error", model);
            }
        }

        [OutputCache(Duration = 60, VaryByParam = "aspxerrorpath")]
        public ActionResult NotFound()
        {
            using (OrganizationRepository)
            {
                var organization = OrganizationRepository.GetDefaultOrganization(readOnly: true);
                var model = Mapper.Map<Organization, OrganizationDetailsModel>(organization);
                return View("404", model);
            }
        }

        [OutputCache(Duration = 60, VaryByParam = "aspxerrorpath")]
        public ActionResult Forbidden()
        {
            using (OrganizationRepository)
            {
                var organization = OrganizationRepository.GetDefaultOrganization(readOnly: true);
                var model = Mapper.Map<Organization, OrganizationDetailsModel>(organization);
                return View("403", model);
            }
        }
    }
}
