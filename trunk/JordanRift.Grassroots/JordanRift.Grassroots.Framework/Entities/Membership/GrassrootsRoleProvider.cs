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

using System.Web.Security;
using JordanRift.Grassroots.Framework.Services;

namespace JordanRift.Grassroots.Framework.Entities.Membership
{
	/// <summary>
	/// Custom role provider to use Grassroots EF schema.
	/// Notes on implementing custom provider: http://msdn.microsoft.com/en-us/library/8fw7xh74.aspx
	/// </summary>
	public class GrassrootsRoleProvider : RoleProvider
	{
		public override string ApplicationName { get; set; }

		public override string Description { get { return "A role provider to manage application roles to/from the Grassroots system."; } }

		#region Public Methods
		
		public override bool IsUserInRole(string username, string roleName)
		{
			var service = new GrassrootsRoleService();
			return service.IsUserInRole( username, roleName );
		}

		public override string[] GetRolesForUser(string username)
		{
			var service = new GrassrootsRoleService();
			return service.GetRolesForUser( username );
		}

		public override void CreateRole(string roleName)
		{
			var service = new GrassrootsRoleService();
			service.CreateRole( roleName );
		}

		public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
		{
			var service = new GrassrootsRoleService();
			return service.DeleteRole( roleName, throwOnPopulatedRole );
		}

		public override bool RoleExists( string roleName )
		{
			var service = new GrassrootsRoleService();
			return service.RoleExists( roleName );
		}

		/// <summary>
		/// Adds the given users to a given role.
		/// A user can have only one role so if you call this method with more than one role
		/// an exception will be thrown.
		/// </summary>
		/// <param name="usernames">an array of userNames</param>
		/// <param name="roleNames">an array of roleNames (only 1 role is supported).</param>
		/// <exception cref="System.InvalidOperationException">Thrown if you call this method with more than one role.</exception>  
		public override void AddUsersToRoles(string[] usernames, string[] roleNames)
		{
			var service = new GrassrootsRoleService();
			service.AddUsersToRoles( usernames, roleNames );
		}

		/// <summary>
		/// Removes the given users from a given role.
		/// A user can have only one role so if you call this method with more than one role
		/// an exception will be thrown.
		/// </summary>
		/// <param name="usernames">an array of userNames</param>
		/// <param name="roleNames">an array of roleNames (only 1 role is supported).</param>
		/// <exception cref="System.InvalidOperationException">Thrown if you call this method with more than one role.</exception>  
		public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
		{
			var service = new GrassrootsRoleService();
			service.RemoveUsersFromRoles( usernames, roleNames );
		}

		public override string[] GetUsersInRole(string roleName)
		{
			var service = new GrassrootsRoleService();
			return service.GetUsersInRole( roleName );
		}

		public override string[] GetAllRoles()
		{
			var service = new GrassrootsRoleService();
			return service.GetAllRoles();
		}

		/// <summary>
		/// Gets an array of user names in a role where the user name contains the specified user name to match.
		/// </summary>
		/// <param name="roleName"></param>
		/// <param name="usernameToMatch"></param>
		/// <returns>an array of usernames</returns>
		public override string[] FindUsersInRole(string roleName, string usernameToMatch)
		{
			var service = new GrassrootsRoleService();
			return service.FindUsersInRole( roleName, usernameToMatch );
		}

		#endregion
	}
}
