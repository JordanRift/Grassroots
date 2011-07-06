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

using System.Collections.Generic;
using System.Linq;
using JordanRift.Grassroots.Framework.Data;
using JordanRift.Grassroots.Framework.Entities.Membership;
using JordanRift.Grassroots.Framework.Entities.Models;
using JordanRift.Grassroots.Tests.Fakes;
using JordanRift.Grassroots.Tests.Helpers;
using NUnit.Framework;
using System.Text;

namespace JordanRift.Grassroots.Tests.UnitTests.Providers
{
	[TestFixture]
	public class RoleProviderTests
	{
		private GrassrootsRoleProvider roleProvider;

		private IOrganizationRepository organizationRepository;
		private IRoleRepository roleRepository;
		private IUserProfileRepository userProfileRepository;
		private IUserRepository userRepository;

		private User user;
		private User alternateUser;
		private Role role;
		private Role unusedRole;
	    private UserProfile userProfile;
	    private UserProfile altUserProfile;

		private const string ROLE_NAME = "Admin";

		[SetUp]
		public void SetUp()
		{
			organizationRepository = new FakeOrganizationRepository();
			roleRepository = new FakeRoleRepository();
			userProfileRepository = new FakeUserProfileRepository();
			userRepository = new FakeUserRepository();

			roleProvider = new GrassrootsRoleProvider();
		}

        [TearDown]
        public void TearDown()
        {
            FakeOrganizationRepository.Reset();
            FakeRoleRepository.Reset();
            FakeUserProfileRepository.Reset();
            FakeUserRepository.Reset();
        }

		[Test]
		public void CreateRole_Should_Add_Role_To_RoleRepository()
		{
			var roles = roleRepository.FindAllRoles();
			int initialCount = roles.Count();

			string NEWROLE = "NewRole";
			roleProvider.CreateRole( NEWROLE );

			var roles2 = roleRepository.FindAllRoles();

			Assert.IsTrue( roles2.Count() == initialCount + 1 );
		}

		[Test]
		public void CreateRole_Should_Error_If_Greater_Than_255_Chars()
		{
			StringBuilder name = new StringBuilder();
			for ( int i = 0; i <= 256; i++ )
				name.Append( "x" );

			Assert.Throws<System.Configuration.Provider.ProviderException>(
				delegate { roleProvider.CreateRole( name.ToString() );	});
		}

		[Test]
		public void CreateRole_Should_Error_If_Already_Exists()
		{
			ArrangeModelsForTest();
			
			Assert.Throws<System.Configuration.Provider.ProviderException>(
				delegate { roleProvider.CreateRole( ROLE_NAME ); } );
		}

		[Test]
		public void CreateRole_Should_Error_If_Role_Name_Contains_Comma()
		{
			Assert.Throws<System.ArgumentException>(
				delegate { roleProvider.CreateRole( "This,Role" );	} );
		}

		[Test]
		public void CreateRole_Should_Error_If_Empty_Role_Name()
		{
			Assert.Throws<System.ArgumentException>(
				delegate {	roleProvider.CreateRole( "" ); });
		}

		[Test]
		public void CreateRole_Should_Error_If_Null_Role()
		{
			Assert.Throws<System.ArgumentNullException>(
				delegate { roleProvider.CreateRole( null );	});
		}

		[Test]
		public void DeleteRole_Should_Return_True_On_Success()
		{
			ArrangeModelsForTest();
			var result = roleProvider.DeleteRole( unusedRole.Name, throwOnPopulatedRole: true);
			Assert.IsTrue( result );
		}

		[Test]
		public void DeleteRole_Should_Error_If_Not_Exists()
		{
			Assert.Throws<System.Configuration.Provider.ProviderException>(
				delegate { var result = roleProvider.DeleteRole( "SomeRoleThatDoesNotExist", throwOnPopulatedRole: true ); } );
		}

		[Test]
		public void DeleteRole_Should_Error_If_Role_Populated_And_Requests_Throw()
		{
			ArrangeModelsForTest();
			Assert.Throws<System.Configuration.Provider.ProviderException>(
				delegate { var result = roleProvider.DeleteRole( ROLE_NAME, throwOnPopulatedRole: true ); } );
		}

		[Test]
		public void DeleteRole_Should_Return_True_If_Role_Populated_And_Not_Requests_Throw()
		{
			ArrangeModelsForTest();
			var result = roleProvider.DeleteRole( ROLE_NAME, throwOnPopulatedRole: false );
			Assert.IsTrue( result );
		}

		[Test]
		public void DeleteRole_Should_Error_On_Null()
		{
			ArrangeModelsForTest();

			Assert.Throws<System.ArgumentNullException>(
				delegate { var result = roleProvider.DeleteRole( null, throwOnPopulatedRole: true ); } );
		}

		[Test]
		public void DeleteRole_Should_Error_On_Empty_Name()
		{
			ArrangeModelsForTest();

			Assert.Throws<System.ArgumentException>(
				delegate { var result = roleProvider.DeleteRole( "", throwOnPopulatedRole: true ); } );
		}

		[Test]
		public void IsUserInRole_Should_Return_True_When_User_Is()
		{
			ArrangeModelsForTest();
			var result = roleProvider.IsUserInRole( user.Username, ROLE_NAME );
			Assert.IsTrue( result );
		}

		[Test]
		public void IsUserInRole_Should_Return_False_If_User_Is_Not()
		{
			ArrangeModelsForTest();
			var result = roleProvider.IsUserInRole( user.Username, unusedRole.Name );
			Assert.IsFalse( result );
		}

		[Test]
		public void GetRolesForUser_Should_Return_One_Role_Becase_Users_Can_Have_Only_One()
		{
			ArrangeModelsForTest();
			var result = roleProvider.GetRolesForUser( user.Username );
			Assert.AreEqual( result.Count(), 1 );
		}

		[Test]
		public void RoleExists_Should_Return_True_When_It_Does()
		{
			ArrangeModelsForTest();
			var result = roleProvider.RoleExists( role.Name );
			Assert.IsTrue( result );
		}

		[Test]
		public void RoleExists_Should_Return_False_When_It_DoesNot()
		{
			ArrangeModelsForTest();
			var result = roleProvider.RoleExists( "FuzzyPumperKunckle" );
			Assert.IsFalse( result );
		}

		[Test]
		public void AddUsersToRoles_Should_Work_When_One_Role_Is_Supplied()
		{
			ArrangeModelsForTest();

			roleProvider.AddUsersToRoles( new string[] { user.Username, alternateUser.Username }, new string[] { unusedRole.Name } );

			string[] matches = roleProvider.FindUsersInRole( unusedRole.Name, alternateUser.Username );
			Assert.GreaterOrEqual( matches.Count(), 1 );
		}

		[Test]
		public void GetUsersInRole_Should_Return_One_Admin_In_This_Case()
		{
			ArrangeModelsForTest();

			string[] usernames = roleProvider.GetUsersInRole( role.Name );

			Assert.AreEqual( usernames.Count(), 1 );
		}

		[Test]
		public void GetAllRoles_Should_Return_One_Or_More_Roles()
		{
			ArrangeModelsForTest();

			string[] roles = roleProvider.GetAllRoles();
			Assert.IsNotEmpty( roles );
		}

		[Test]
		public void RemoveUsersFromRoles_Should_Cause_The_Role_To_Have_Zero_Users()
		{
			ArrangeModelsForTest();
			roleProvider.RemoveUsersFromRoles(new string[] { user.Username, alternateUser.Username }, new string[] { role.Name } );

			Assert.IsFalse( roleProvider.IsUserInRole( user.Username, role.Name ));
			Assert.IsFalse( roleProvider.IsUserInRole( alternateUser.Username, role.Name ) );
		}

		
		/// <summary>
		/// Sets up two users, two profiles, and two roles.
		/// The first user also has the first role.
		/// The second role (unusedRole) is unused.
		/// The second user (alternateUser) has no role.
		/// 
		/// VERY IMPORTANT NOTE!!!
		///   You will see some extra stiching below because we can't
		///   rely on EF since our mock/fake repos are not EF.
		/// 
		/// </summary>
		private void ArrangeModelsForTest()
		{
			Organization org = organizationRepository.GetDefaultOrganization();
			org.Roles = new List<Role>();

			//----------------
			// Roles
			//---------------

			// Set up the used role called "Admin"
			role = EntityHelpers.GetValidRole();
			role.UserProfiles = new List<UserProfile>();
			role.Name = ROLE_NAME;
			roleRepository.Add( role );

			// Set up the unused role
			unusedRole = EntityHelpers.GetValidRole();
			unusedRole.UserProfiles = new List<UserProfile>();
			unusedRole.Name = "AnUnusedRole";
			roleRepository.Add( unusedRole );

			roleRepository.Save();

			org.Roles.Add( role );
			org.Roles.Add( unusedRole );

			organizationRepository.Save();

			//-------------------------
			// Users and UserProfiles
			//-------------------------

			// Set up two profiles and save them
			userProfile = EntityHelpers.GetValidUserProfile();
			userProfile.Users = new List<User>();
			altUserProfile = EntityHelpers.GetValidUserProfile();
			altUserProfile.Users = new List<User>();

			userProfileRepository.Add( userProfile );
			userProfileRepository.Add( altUserProfile );
			userProfileRepository.Save();

			// Set up two users
			user = EntityHelpers.GetValidUser();
			alternateUser = EntityHelpers.GetValidUser();
			user.Username = "Frick@gmail.com";
			alternateUser.Username = "Frack@gmail.com";

			user.UserProfile = userProfile;				// needed because we don't have EF doing this for us
			alternateUser.UserProfile = altUserProfile;	// needed because we don't have EF doing this for us

			//  Save the users to the Repository
			userRepository.Add( user );
			userRepository.Add( alternateUser );
			userRepository.Save();

			// Now add the users to the UserProfiles
			userProfile.Users.Add( user );
			altUserProfile.Users.Add( alternateUser );

			userProfileRepository.Save();

			//-----------------------------------
			// Adding the UserProfile to the role
			//-----------------------------------

			// Now add the first "user" to the first "role"
			userProfile.Role = role;				// needed because we don't have EF doing this for us
			role.UserProfiles.Add( userProfile );
			organizationRepository.Save();
		}

		/// <summary>
		/// Sets up two users, two profiles, and two roles.
		/// The first user also has the first role.
		/// The second role (unusedRole) is unused.
		/// The second user (alternateUser) has no role.
		/// </summary>
		private void ArrangeModelsForTest_old()
		{
			//-------------------------
			// Users and UserProfiles
			//-------------------------

			// Set up two profiles and save them
			userProfile = EntityHelpers.GetValidUserProfile();
			userProfile.Users = new List<User>();
			altUserProfile = EntityHelpers.GetValidUserProfile();
			altUserProfile.Users = new List<User>();

			userProfileRepository.Add( userProfile );
			userProfileRepository.Add( altUserProfile );
			userProfileRepository.Save();

			// Set up two users
			user = EntityHelpers.GetValidUser();
			alternateUser = EntityHelpers.GetValidUser();
			user.Username = "Frick@gmail.com";
			alternateUser.Username = "Frack@gmail.com";

			// Now add the profiles to the users
			user.UserProfile = userProfile;
			alternateUser.UserProfile = altUserProfile;

			//  Save the users to the Repository
			userRepository.Add( user );
			userRepository.Add( alternateUser );
			userRepository.Save();

			//----------------
			// Roles
			//---------------

			// Set up one used role
			role = EntityHelpers.GetValidRole();
		    role.UserProfiles = new List<UserProfile>();
			role.Name = ROLE_NAME;
			roleRepository.Add( role );
			
			// Set up the unused role
			unusedRole = EntityHelpers.GetValidRole();
		    unusedRole.UserProfiles = new List<UserProfile>();
			unusedRole.Name = "AnUnusedRole";
			roleRepository.Add( unusedRole );

			// Save the roles
			roleRepository.Save();

			//-----------------------------
			// Adding roles to UserProfile
			//-----------------------------
            
			// Now add the first "user" to the first "role"
			userProfile.Role = role;
			userProfileRepository.Save();

		}
	}
}
