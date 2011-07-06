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
using System.Configuration.Provider;
using System.Linq;
using JordanRift.Grassroots.Framework.Data;
using JordanRift.Grassroots.Framework.Entities.Models;
using JordanRift.Grassroots.Framework.Helpers;

namespace JordanRift.Grassroots.Framework.Services
{
	public class GrassrootsRoleService
	{
		private readonly IRoleRepository roleRepository;
		private readonly IUserRepository userRepository;
		private readonly IUserProfileRepository userProfileRepository;

		public GrassrootsRoleService()
		{
		    var userProfileRepositoryFactory = new RepositoryFactory<IUserProfileRepository>();
		    userProfileRepository = userProfileRepositoryFactory.GetRepository();

		    var userRepositoryFactory = new RepositoryFactory<IUserRepository>();
		    userRepository = userRepositoryFactory.GetRepository();

		    var roleRepositoryFactory = new RepositoryFactory<IRoleRepository>();
		    roleRepository = roleRepositoryFactory.GetRepository();
		}

		public bool IsUserInRole( string username, string roleName )
		{
			var user = userRepository.GetUserByName( username );

			if ( user != null && user.UserProfile != null && user.UserProfile.Role != null )
				return ( user.UserProfile.Role.Name == roleName );
			else
				return ( false );
		}

		public string[] GetRolesForUser( string username )
		{
			if ( string.IsNullOrEmpty(username) )
			{
				throw new ProviderException( "User name cannot be empty or null." );
			}

			var user = userRepository.GetUserByName( username );
			if ( user != null && user.UserProfile != null && user.UserProfile.Role != null )
				return ( new string[] { user.UserProfile.Role.Name } );
			else
				return new string[0];
		}

		public void CreateRole( string roleName )
		{
			if ( roleName == null )
				throw new ArgumentNullException( "Role name cannot be null." );
			if ( roleName == "" )
				throw new ArgumentException( "Role name cannot be empty." );
			if ( roleName.Contains( "," ) )
				throw new ArgumentException( "Role names cannot contain commas." );
			if ( RoleExists( roleName ) )
				throw new ProviderException( "Role name already exists." );
			if ( roleName.Length > 255 )
				throw new ProviderException( "Role name cannot exceed 255 characters." );

			Role role = new Role();
			role.Name = roleName;
			roleRepository.Add( role );
			roleRepository.Save();
		}

		public bool DeleteRole( string roleName, bool throwOnPopulatedRole )
		{
			if ( roleName == null )
				throw new ArgumentNullException( "Role name cannot be null." );
			if ( roleName == "" )
				throw new ArgumentException( "Role name cannot be empty." );

			if ( !RoleExists( roleName ) )
				throw new ProviderException( "Role does not exist." );

			if ( throwOnPopulatedRole && GetUsersInRole( roleName ).Length > 0 )
			{
				throw new ProviderException( "Cannot delete the role because there are still people who have that role." );
			}

			try
			{
				Role role = roleRepository.GetRoleByName( roleName );
				roleRepository.Delete( role );
				roleRepository.Save();
				return true;
			}
			catch
			{
				return false;
			}
		}

		public bool RoleExists( string roleName )
		{
			Role role = roleRepository.GetRoleByName( roleName );
			return ( role != null );
		}

		/// <summary>
		/// Adds the given users to a given role.
		/// A user can have only one role so if you call this method with more than one role
		/// an exception will be thrown.
		/// </summary>
		/// <param name="usernames">an array of userNames</param>
		/// <param name="roleNames">an array of roleNames (only 1 role is supported).</param>
		/// <exception cref="System.InvalidOperationException">Thrown if you call this method with more than one role.</exception>  
		public void AddUsersToRoles( string[] usernames, string[] roleNames )
		{
			// Until our system changes, this should prevent us from using this method incorrectly.
			if ( roleNames.Length > 1 )
			{
				throw new InvalidOperationException( "A user can have only one role in Grassroots. What are you trying to do?" );
			}

			var role = roleRepository.GetRoleByName( roleNames[0] );

			foreach ( string username in usernames )
			{
				//var userProfile = userProfileRepository.FindUserProfileByEmail( username ).FirstOrDefault();
				var user = userRepository.GetUserByName( username );
				var userProfile = user.UserProfile;

				if ( userProfile != null )
				{
					role.UserProfiles.Add( userProfile );
				}
			}

			roleRepository.Save();
		}

		/// <summary>
		/// Removes the given users from a given role.
		/// A user can have only one role so if you call this method with more than one role
		/// an exception will be thrown.
		/// </summary>
		/// <param name="usernames">an array of userNames</param>
		/// <param name="roleNames">an array of roleNames (only 1 role is supported).</param>
		/// <exception cref="System.InvalidOperationException">Thrown if you call this method with more than one role.</exception>  
		public void RemoveUsersFromRoles( string[] usernames, string[] roleNames )
		{
			// Until our system changes, this should prevent us from using this method incorrectly.
			if ( roleNames.Length > 1 )
			{
				throw new InvalidOperationException( "A user has only one role in Grassroots. What are you trying to do?" );
			}

			var role = new Role();

			foreach ( string username in usernames )
			{
				var user = userRepository.GetUserByName( username );
				user.UserProfile.Role = role;
			}

			userRepository.Save();
		}

		public string[] GetUsersInRole( string roleName )
		{
			var role = roleRepository.GetRoleByName( roleName );

			return ( from userProfile in role.UserProfiles
					 from user in userProfile.Users
					 select user.Username ).ToArray();
		}

		public string[] GetAllRoles()
		{
			return ( from role in roleRepository.FindAllRoles()
					 select role.Name ).ToArray();
		}

		/// <summary>
		/// Gets an array of user names in a role where the user name contains the specified user name to match.
		/// </summary>
		/// <param name="roleName"></param>
		/// <param name="usernameToMatch"></param>
		/// <returns>an array of usernames</returns>
		public string[] FindUsersInRole( string roleName, string usernameToMatch )
		{
			var role = roleRepository.GetRoleByName( roleName );

			return ( from userProfile in role.UserProfiles
					 from user in userProfile.Users
					 where user.Username.Contains( usernameToMatch )
					 select user.Username ).ToArray();
		}
	}
}
