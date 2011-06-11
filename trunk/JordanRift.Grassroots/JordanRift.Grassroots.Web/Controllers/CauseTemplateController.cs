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
using JordanRift.Grassroots.Framework.Data;
using JordanRift.Grassroots.Framework.Entities.Models;
using JordanRift.Grassroots.Web.Models;

namespace JordanRift.Grassroots.Web.Controllers
{
    public class CauseTemplateController : Controller
    {
        private readonly ICauseTemplateRepository causeTemplateRepository;

        public CauseTemplateController(ICauseTemplateRepository causeTemplateRepository)
        {
            this.causeTemplateRepository = causeTemplateRepository;
            Mapper.CreateMap<CauseTemplate, CauseTemplateDetailsModel>();
        }

        //
        // GET: /CauseTemplate/

        public ActionResult Index()
        {
            return View();
        }

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
