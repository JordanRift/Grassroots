//
// Copyright © 2011 Jordan Rift, LLC - All Rights Reserved
//
// THIS WORK IS LICENSED UNDER A CREATIVE COMMONS ATTRIBUTION-NONCOMMERCIAL-
// SHAREALIKE 3.0 UNPORTED LICENSE:
// http://creativecommons.org/licenses/by-nc-sa/3.0/
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
    public class UserProfileRepositoryTests
    {
        private IUserProfileRepository userProfileRepository;
        private IOrganizationRepository organizationRepository;
        private IRoleRepository roleRepository;

        private Organization organization;
        private UserProfile userProfile;
        private Role role;

        [SetUp]
        public void SetUp()
        {
            userProfileRepository = new UserProfileRepository();
            organizationRepository = new OrganizationRepository();
            roleRepository = new RoleRepository();
        }

        [Test]
        public void Add_Should_Add_UserProfile_To_Database()
        {
            using (new UnitOfWorkScope())
            using (new TransactionScope())
            {
                ArrangeUserProfileTest();
                var id = userProfile.UserProfileID;
                Assert.Greater(id, 0);
            }
        }

        [Test]
        public void GetUserProfileByID_Should_Load_UserProfile_From_Database()
        {
            using (new UnitOfWorkScope())
            using (new TransactionScope())
            {
                ArrangeUserProfileTest();
                var id = userProfile.UserProfileID;
                userProfile = null;
                userProfile = userProfileRepository.GetUserProfileByID(id);
                Assert.IsNotNull(userProfile);
                Assert.AreEqual(userProfile.UserProfileID, id);
            }
        }

        [Test]
        public void GetUserProfileByEmail_Should_Return_Empty_When_Email_Does_Not_Exist()
        {
            using (new UnitOfWorkScope())
            using (new TransactionScope())
            {
                ArrangeUserProfileTest();
                var userProfiles = userProfileRepository.FindUserProfileByEmail("fictional-email@fictionalemail.com");
                Assert.IsNotNull(userProfiles);
                Assert.AreEqual(userProfiles.Count(), 0);
            }
        }

        [Test]
        public void GetUserProfileByEmail_Should_Load_UserProfile_From_Database()
        {
            using (new UnitOfWorkScope())
            using (new TransactionScope())
            {
                ArrangeUserProfileTest();
                var email = userProfile.Email;
                var userProfiles = userProfileRepository.FindUserProfileByEmail(email);
                Assert.IsNotNull(userProfiles);
                Assert.AreEqual(userProfiles.Count(), 1);
            }
        }

        [Test]
        public void Exists_Should_Return_True_If_Email_Exists_In_Database()
        {
            using (new UnitOfWorkScope())
            using (new TransactionScope())
            {
                ArrangeUserProfileTest();
                var email = userProfile.Email;
                var result = userProfileRepository.Exists(email);
                Assert.True(result);
            }
        }

        [Test]
        public void Exists_Should_Return_False_If_Email_Does_Not_Exist_In_Database()
        {
            using (new UnitOfWorkScope())
            using (new TransactionScope())
            {
                ArrangeUserProfileTest();
                var result = userProfileRepository.Exists("fictional-email@fictionalemail.com");
                Assert.False(result);
            }
        }

        [Test]
        public void Delete_Should_Delete_UserProfile_From_Database()
        {
            using (new UnitOfWorkScope())
            using (new TransactionScope())
            {
                ArrangeUserProfileTest();
                var id = userProfile.UserProfileID;
                userProfileRepository.Delete(userProfile);
                userProfileRepository.Save();
                userProfile = userProfileRepository.GetUserProfileByID(id);
                Assert.IsNull(userProfile);
            }
        }

        private void ArrangeUserProfileTest()
        {
            organization = EntityHelpers.GetValidOrganization();
            organization.Roles = new List<Role>();
            organization.UserProfiles = new List<UserProfile>();
            organizationRepository.Add(organization);
            organizationRepository.Save();

            role = EntityHelpers.GetValidRole();
            role.UserProfiles = new List<UserProfile>();
            organization.Roles.Add(role);
            roleRepository.Add(role);
            roleRepository.Save();

            userProfile = EntityHelpers.GetValidUserProfile();
            userProfile.UserProfileService = new UserProfileService(new UserProfileRepository());
            organization.UserProfiles.Add(userProfile);
            role.UserProfiles.Add(userProfile);
            userProfileRepository.Save();
        }
    }
}
