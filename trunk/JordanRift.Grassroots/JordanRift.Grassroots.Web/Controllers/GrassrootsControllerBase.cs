﻿//
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

using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using JordanRift.Grassroots.Framework.Data;
using JordanRift.Grassroots.Framework.Helpers;

namespace JordanRift.Grassroots.Web.Controllers
{
    public abstract class GrassrootsControllerBase : Controller
    {
        private IOrganizationRepository organizationRepository;

        public IOrganizationRepository OrganizationRepository 
        { 
            get
            {
                if (organizationRepository == null)
                {
                    var repositoryFactory = new RepositoryFactory<IOrganizationRepository>();
                    organizationRepository = repositoryFactory.GetRepository();
                }

                return organizationRepository;
            } 
            
            set { organizationRepository = value; }
        }

        ~GrassrootsControllerBase()
        {
            organizationRepository.Dispose();
        }

        protected IEnumerable<string> FindModelErrors()
        {
            return from s in ModelState.Values 
                   from error in s.Errors 
                   select error.ErrorMessage;
        }
    }
}
