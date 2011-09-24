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

namespace JordanRift.Grassroots.Web.Controllers
{
    public class CauseController : GrassrootsControllerBase
    {
        private const string ADMIN_ROLES = "Root,Administrator";

        [Authorize(Roles = ADMIN_ROLES)]
        public ActionResult Index(int id = -1)
        {
            return null;
        }

        [Authorize(Roles = ADMIN_ROLES)]
        public ActionResult List()
        {
            return null;
        }

        [Authorize(Roles = ADMIN_ROLES)]
        public ActionResult Admin(int id = -1)
        {
            return null;
        }
    }
}
