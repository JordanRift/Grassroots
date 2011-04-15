//
// Copyright © 2011 Jordan Rift, LLC - All Rights Reserved
//
// THIS WORK IS LICENSED UNDER A CREATIVE COMMONS ATTRIBUTION-NONCOMMERCIAL-
// SHAREALIKE 3.0 UNPORTED LICENSE:
// http://creativecommons.org/licenses/by-nc-sa/3.0/
//

using System.Collections.Generic;
using System.Configuration.Provider;
using System.Linq;
using System.Web.Security;
using JordanRift.Grassroots.Framework.Data;
using JordanRift.Grassroots.Framework.Entities.Membership;
using JordanRift.Grassroots.Framework.Entities.Models;
using JordanRift.Grassroots.Framework.Services;
using JordanRift.Grassroots.Tests.Fakes;
using JordanRift.Grassroots.Tests.Helpers;
using NUnit.Framework;

namespace JordanRift.Grassroots.Tests.UnitTests.Providers
{
    [TestFixture]
    public class MembershipProviderTests
    {
        private IUserRepository userRepository;
        private IUserProfileRepository userProfileRepository;
        private GrassrootsMembershipProvider membershipProvider;

        private UserProfile userProfile;
        private User user;
        private const string EMAIL = "info@jordanrift.com";
        private const string PASSWORD = "secret123";

        [SetUp]
        public void SetUp()
        {
            userRepository = new FakeUserRepository();
            ((FakeUserRepository) userRepository).SetUpRepository();
            userProfileRepository = new FakeUserProfileRepository();
            ((FakeUserProfileRepository) userProfileRepository).SetUpRepository();
            membershipProvider = new GrassrootsMembershipProvider();
            userProfile = EntityHelpers.GetValidUserProfile();
            userProfile.Users = new List<User>();
        }

        [Test]
        public void CreateUser_Should_Add_User_To_UserRepository()
        {
            ArrangeMembershipProviderCreateTest();
            MembershipCreateStatus status;
            membershipProvider.CreateUser(EMAIL, PASSWORD, EMAIL, null, null, true, EMAIL, out status);
            user = userProfile.Users.FirstOrDefault(u => u.Username == EMAIL);
            Assert.IsNotNull(user);
        }

        [Test]
        public void CreateUser_Should_Create_Hashed_Password()
        {
            ArrangeMembershipProviderCreateTest();
            MembershipCreateStatus status;
            membershipProvider.CreateUser(EMAIL, PASSWORD, EMAIL, null, null, true, EMAIL, out status);
            user = userProfile.Users.FirstOrDefault(u => u.Username == EMAIL);
            Assert.AreNotEqual("secret123", user.Password);
            Assert.IsNotNull(user.Password);
        }

        [Test]
        public void CreateUser_Should_Throw_Exception_If_UserProfile_Not_Found()
        {
            Assert.Throws<MembershipCreateUserException>(delegate
            {
                MembershipCreateStatus status;
                membershipProvider.CreateUser(EMAIL, PASSWORD, EMAIL, null, null, true, EMAIL, out status);
            });
        }

        [Test]
        public void CreateUser_Returns_Success_Status_On_Successful_User_Creation()
        {
            ArrangeMembershipProviderCreateTest();
            MembershipCreateStatus status;
            membershipProvider.CreateUser(EMAIL, PASSWORD, EMAIL, null, null, true, EMAIL, out status);
            Assert.AreEqual(MembershipCreateStatus.Success, status);
        }

        [Test]
        public void CreateUser_Returns_Null_On_Provider_Error()
        {
            ArrangeMembershipProviderCreateTest();
            MembershipCreateStatus status;
            
            // Pass null to password field to force exception when attempting password hash
            var membershipUser = membershipProvider.CreateUser(EMAIL, null, EMAIL, null, null, true, EMAIL, out status);
            Assert.IsNull(membershipUser);
        }

        [Test]
        public void CreateUser_Returns_ProvderError_Status_On_Provider_Error()
        {
            ArrangeMembershipProviderCreateTest();
            MembershipCreateStatus status;

            // Pass null to password field to force exception when attempting password hash
            membershipProvider.CreateUser(EMAIL, null, EMAIL, null, null, true, EMAIL, out status);
            Assert.AreEqual(MembershipCreateStatus.ProviderError, status);
        }

        [Test]
        public void DeleteUser_Should_Set_Active_To_False()
        {
            ArrangeModelsForTest();
            membershipProvider.DeleteUser(EMAIL, false);

            user = userRepository.GetUserByName(EMAIL);
            Assert.IsFalse(user.IsActive);
        }

        [Test]
        public void DeleteUser_Should_Set_UserProfile_Active_To_False()
        {
            ArrangeModelsForTest();
            membershipProvider.DeleteUser(EMAIL, false);

            userProfile = userProfileRepository.FindUserProfileByEmail(EMAIL).FirstOrDefault();
            Assert.IsFalse(userProfile.Active);
        }

        [Test]
        public void DeleteUser_Should_Return_True_On_Success()
        {
            ArrangeModelsForTest();
            var result = membershipProvider.DeleteUser(EMAIL, false);
            Assert.IsTrue(result);
        }

        [Test]
        public void DeleteUser_Should_Return_False_On_Failure()
        {
            var result = membershipProvider.DeleteUser(EMAIL, false);
            Assert.IsFalse(result);
        }

        [Test]
        public void FindUsersByName_Should_Return_List_When_User_Is_Found()
        {
            ArrangeModelsForTest();
            int totalRecords;
            var result = membershipProvider.FindUsersByName(EMAIL, 0, 0, out totalRecords);
            Assert.IsNotNull(result);
            Assert.IsNotEmpty(result);
        }

        [Test]
        public void FindUserByName_Total_Should_Be_Greater_Than_Zero_When_User_Is_Found()
        {
            ArrangeModelsForTest();
            int totalRecords;
            membershipProvider.FindUsersByName(EMAIL, 0, 0, out totalRecords);
            Assert.Greater(totalRecords, 0);
        }

        [Test]
        public void FindUserByName_Should_Return_Empty_When_User_Not_Found()
        {
            int totalRecords;
            var result = membershipProvider.FindUsersByName(EMAIL, 0, 0, out totalRecords);
            Assert.IsNotNull(result);
            Assert.IsEmpty(result);
        }

        [Test]
        public void FindUserByName_Total_Should_Be_Zero_When_User_Not_Found()
        {
            int totalRecords;
            membershipProvider.FindUsersByName(EMAIL, 0, 0, out totalRecords);
            Assert.AreEqual(totalRecords, 0);
        }

        [Test]
        public void GetAllUsers_Should_Return_List()
        {
            ArrangeModelsForTest();
            int totalRecords;
            var results = membershipProvider.GetAllUsers(0, 10, out totalRecords);
            Assert.IsNotNull(results);
            Assert.IsNotEmpty(results);
        }

        [Test]
        public void GetUser_Should_Return_MembershipUser_When_Username_Found()
        {
            ArrangeModelsForTest();
            var result = membershipProvider.GetUser(EMAIL, false);
            Assert.IsNotNull(result);
        }

        [Test]
        public void GetUser_Should_Return_Null_When_Username_Not_Found()
        {
            ArrangeModelsForTest();
            var result = membershipProvider.GetUser("non-existant-email@gmail.com", false);
            Assert.IsNull(result);
        }

        [Test]
        public void GetUser_Should_Return_MembershipUser_When_ProviderUserKey_Found()
        {
            ArrangeModelsForTest();
            var result = membershipProvider.GetUser((object) EMAIL, false);
            Assert.IsNotNull(result);
        }

        [Test]
        public void GetUser_Should_Return_Null_When_ProviderUserKey_Not_Found()
        {
            ArrangeModelsForTest();
            var result = membershipProvider.GetUser((object) "non-existant-email@gmail.com", false);
            Assert.IsNull(result);
        }

        [Test]
        public void ResetPassword_Should_Reset_Password_When_Username_Found()
        {
            ArrangeModelsForTest();
            var password = user.Password;
            var result = membershipProvider.ResetPassword(EMAIL, null);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(password, result);
        }

        [Test]
        public void ResetPassword_Should_Throw_Exception_When_Username_Not_Found()
        {
            Assert.Throws<MembershipPasswordException>(delegate
            {
                membershipProvider.ResetPassword("non-existant-email@gmail.com", null);
            });
        }

        [Test]
        public void UnlockUser_Should_Set_IsActive_To_True()
        {
            ArrangeModelsForTest();
            user.IsActive = false;
            membershipProvider.UnlockUser(EMAIL);
            var result = user.IsActive;
            Assert.IsTrue(result);
        }

        [Test]
        public void UnlockUser_Should_Return_True_When_Username_Found()
        {
            ArrangeModelsForTest();
            user.IsActive = false;
            var result = membershipProvider.UnlockUser(EMAIL);
            Assert.IsTrue(result);
        }

        [Test]
        public void UnlockUser_Should_Return_False_When_Username_Not_Found()
        {
            var result = membershipProvider.UnlockUser("non-existant-email@gmail.com");
            Assert.IsFalse(result);
        }

        [Test]
        public void ValidateUser_Should_Return_True_When_Username_And_Password_Match()
        {
            ArrangeModelsForTest();
            user.Password = GrassrootsMembershipService.HashPassword(PASSWORD, null);
            var result = membershipProvider.ValidateUser(EMAIL, PASSWORD);
            Assert.IsTrue(result);
        }

        [Test]
        public void ValidateUser_Should_Return_False_When_Password_Does_Not_Match()
        {
            ArrangeModelsForTest();
            user.Password = GrassrootsMembershipService.HashPassword(PASSWORD, null);
            var result = membershipProvider.ValidateUser(EMAIL, "badpassword");
            Assert.IsFalse(result);
        }

        [Test]
        public void ValidateUser_Should_Return_False_When_Username_Not_Found()
        {
            ArrangeModelsForTest();
            user.Password = GrassrootsMembershipService.HashPassword(PASSWORD, null);
            var result = membershipProvider.ValidateUser("non-existant-email@gmail.com", PASSWORD);
            Assert.IsFalse(result);
        }

        [Test]
        public void ValidateUser_Should_Return_False_When_Username_And_Password_Are_Incorrect()
        {
            ArrangeModelsForTest();
            user.Password = GrassrootsMembershipService.HashPassword(PASSWORD, null);
            var result = membershipProvider.ValidateUser("non-existant-email@gmail.com", "badpassword");
            Assert.IsFalse(result);
        }

        [Test]
        public void ChangePassword_Should_Return_True_When_Password_Change_Successful()
        {
            ArrangeModelsForTest();
            var result = membershipProvider.ChangePassword(EMAIL, PASSWORD, "newpassword");
            Assert.IsTrue(result);
        }

        [Test]
        public void ChangePassword_Should_Return_False_When_Password_Change_Fails()
        {
            ArrangeModelsForTest();
            var result = membershipProvider.ChangePassword(EMAIL, "badpassword", "newpassword");
            Assert.IsFalse(result);
        }

        [Test]
        public void ChangePassword_Should_Throw_Exception_When_Username_Not_Found()
        {
            Assert.Throws<ProviderException>(
                delegate { membershipProvider.ChangePassword("non-existant-email@gmail.com", PASSWORD, "newpassword"); });
        }

        private void ArrangeMembershipProviderCreateTest()
        {
            userProfile = EntityHelpers.GetValidUserProfile();
            userProfile.Email = EMAIL;
            userProfile.Users = new List<User>();
            userProfileRepository.Add(userProfile);
        }

        private void ArrangeModelsForTest()
        {
            userProfile.Email = EMAIL;
            userProfileRepository.Add(userProfile);
            userProfileRepository.Save();

            user = EntityHelpers.GetValidUser();
            user.Username = EMAIL;
            user.Password = GrassrootsMembershipService.HashPassword(PASSWORD, null);
            user.UserProfile = userProfile;
            userRepository.Add(user);
            userRepository.Save();
        }
    }
}
