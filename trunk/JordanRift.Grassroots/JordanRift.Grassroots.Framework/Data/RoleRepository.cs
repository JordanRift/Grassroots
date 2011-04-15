//
// Copyright © 2011 Jordan Rift, LLC - All Rights Reserved
//
// THIS WORK IS LICENSED UNDER A CREATIVE COMMONS ATTRIBUTION-NONCOMMERCIAL-
// SHAREALIKE 3.0 UNPORTED LICENSE:
// http://creativecommons.org/licenses/by-nc-sa/3.0/
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
