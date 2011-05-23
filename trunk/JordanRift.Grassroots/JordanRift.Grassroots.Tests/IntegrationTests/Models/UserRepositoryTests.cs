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
using System.Transactions;
using JordanRift.Grassroots.Framework.Data;
using JordanRift.Grassroots.Framework.Entities.Models;
using JordanRift.Grassroots.Framework.Helpers;
using JordanRift.Grassroots.Framework.Services;
using JordanRift.Grassroots.Tests.Helpers;
using NUnit.Framework;

namespace JordanRift.Grassroots.Tests.IntegrationTests.Models
{
    [TestFixture]
    public class UserRepositoryTests
    {
        private IUserRepository userRepository;
        private IUserProfileRepository userProfileRepository;
        private IOrganizationRepository organizationRepository;

        public Organization organization;
        public UserProfile userProfile;
        public User user;

        [SetUp]
        public void SetUp()
        {
            userRepository = new UserRepository();
            userProfileRepository = new UserProfileRepository();
            organizationRepository = new OrganizationRepository();
        }

        [Test]
        public void Add_Should_Add_User_To_Database()
        {
            using (new UnitOfWorkScope())
            using (new TransactionScope())
            {
                ArrangeUserTest();
                var id = user.UserID;
                Assert.Greater(id, 0);
            }
        }

        [Test]
        public void GetUserByName_Should_Load_User_From_Database()
        {
            using (new UnitOfWorkScope())
            using (new TransactionScope())
            {
                ArrangeUserTest();
                var username = user.Username;
                user = null;
                user = userRepository.GetUserByName(username);
                Assert.IsNotNull(user);
                Assert.AreEqual(username, user.Username);
            }
        }

        [Test]
        public void GetUserByName_Should_Load_Return_Null_When_Username_Not_Found()
        {
            using (new UnitOfWorkScope())
            using (new TransactionScope())
            {
                ArrangeUserTest();
                var result = userRepository.GetUserByName("nonexistantuser");
                Assert.IsNull(result);
            }
        }

        [Test]
        public void FindAllUsers_Should_Return_List()
        {
            using (new UnitOfWorkScope())
            using (new TransactionScope())
            {
                ArrangeUserTest();
                var result = userRepository.FindAllUsers();
                Assert.IsNotNull(result);
                Assert.IsNotEmpty(result.ToList());
            }
        }

        [Test]
        public void Delete_Should_Delete_User_From_Database()
        {
            using (new UnitOfWorkScope())
            using (new TransactionScope())
            {
                ArrangeUserTest();
                var username = user.Username;
                userRepository.Delete(user);
                userRepository.Save();
                user = userRepository.GetUserByName(username);
                Assert.IsNull(user);
            }
        }

        private void ArrangeUserTest()
        {
            organization = EntityHelpers.GetValidOrganization();
            organization.UserProfiles = new List<UserProfile>();
            organizationRepository.Add(organization);
            organizationRepository.Save();

            userProfile = EntityHelpers.GetValidUserProfile();
            userProfile.Users = new List<User>();
            userProfile.UserProfileService = new UserProfileService(new UserProfileRepository());
            organization.UserProfiles.Add(userProfile);
            organizationRepository.Save();

            user = EntityHelpers.GetValidUser();
            userProfile.Users.Add(user);
            userRepository.Save();
        }
    }
}
