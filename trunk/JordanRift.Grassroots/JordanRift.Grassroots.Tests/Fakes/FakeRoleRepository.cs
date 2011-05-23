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
using System.Linq;
using JordanRift.Grassroots.Framework.Data;
using JordanRift.Grassroots.Framework.Entities.Models;

namespace JordanRift.Grassroots.Tests.Fakes
{
    [Obsolete("This class will be obsolete in future versions in favor of using Rhino Mocks. See DonateControllerTests for example of new pattern.")]
    class FakeRoleRepository : IRoleRepository
    {
        private static List<Role> roles;

        public void SetUpRepository()
        {
            roles = new List<Role>();
            for (int i = 0; i < 5; i++)
            {
                roles.Add(new Role
                {
                    RoleID = i + 1,
                    Name = "Role" + i,
                    Description = string.Format("this is role number {0}", i),
                });
            }
        }

        public IQueryable<Role> FindAllRoles()
        {
            return roles.AsQueryable();
        }

        public Role GetRoleByID(int id)
        {
            return roles.FirstOrDefault(r => r.RoleID == id);
        }

        public Role GetRoleByName(string name)
        {
            return roles.FirstOrDefault(r => r.Name.ToLower() == name.ToLower());
        }

        public void Add(Role role)
        {
            role.RoleID = roles.Count + 1;
            roles.Add(role);
        }

        public void Delete(Role role)
        {
            roles.Remove(role);
        }

        public void Save()
        {
        }
    }
}
