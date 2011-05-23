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
using JordanRift.Grassroots.Framework.Entities.Models;

namespace JordanRift.Grassroots.Framework.Data
{
	public class RoleRepository : GrassrootsRepositoryBase, IRoleRepository
	{
		public IQueryable<Role> FindAllRoles()
		{
			return ObjectContext.Roles.AsQueryable();
		}

		public Role GetRoleByID(int id)
		{
			return ObjectContext.Roles.FirstOrDefault(r => r.RoleID == id);
		}

		public Role GetRoleByName( string name )
		{
			return ObjectContext.Roles.FirstOrDefault( r => r.Name == name );
		}

		public void Add(Role role)
		{
			ObjectContext.Roles.Add(role);
		}

		public void Delete(Role role)
		{
			ObjectContext.Roles.Remove(role);
		}

		void IRoleRepository.Save()
		{
			base.Save();
		}
	}
}
