//
// Copyright © 2011 Jordan Rift, LLC - All Rights Reserved
//
// THIS WORK IS LICENSED UNDER A CREATIVE COMMONS ATTRIBUTION-NONCOMMERCIAL-
// SHAREALIKE 3.0 UNPORTED LICENSE:
// http://creativecommons.org/licenses/by-nc-sa/3.0/
//

using System;
using System.Linq;
using JordanRift.Grassroots.Framework.Data;
using JordanRift.Grassroots.Framework.Entities.Models;
using JordanRift.Grassroots.Tests.Fakes;
using JordanRift.Grassroots.Tests.Helpers;
using NUnit.Framework;

namespace JordanRift.Grassroots.Tests.UnitTests.Models
{
    [TestFixture]
    public class UserProfileTests
	{
        private IUserProfileRepository repository;
        private UserProfile userProfile;

        [SetUp]
        public void SetUp()
        {
            repository = new FakeUserProfileRepository();
            ((FakeUserProfileRepository)repository).SetUpRepository();
            userProfile = EntityHelpers.GetValidUserProfile();
        }

        [Test]
        public void Validate_Should_Return_Empty_When_User_Profile_Is_Valid()
        {
            var results = userProfile.Validate(null);
            Assert.IsEmpty(results.ToList());
        }

        [Test]
        public void Validate_Should_Return_ValidationResult_When_UserProfile_Is_Younger_Than_13()
        {
            userProfile.Birthdate = DateTime.Now.AddYears(-12);
            var results = userProfile.Validate(null);
            Assert.IsNotEmpty(results.ToList());
        }

        [Test]
        public void Validate_Should_Return_Empty_When_UserProfile_Is_Exactly_13()
        {
            userProfile.Birthdate = DateTime.Now.AddYears(-13);
            var results = userProfile.Validate(null);
            Assert.IsEmpty(results.ToList());
        }

        [Test]
        public void Validate_Should_Return_Empty_When_UserProfile_Just_Turned_13()
        {
            userProfile.Birthdate = DateTime.Now.AddYears(-13).AddDays(-1);
            var results = userProfile.Validate(null);
            Assert.IsEmpty(results.ToList());
        }

        [Test]
        public void Validate_Should_Return_ValidationResult_When_Consent_Is_False()
        {
            userProfile.Consent = false;
            var results = userProfile.Validate(null);
            Assert.IsNotEmpty(results.ToList());
        }

        [Test]
        public void Validate_Should_Return_ValidationResult_When_Email_Is_Not_Unique()
        {
            userProfile.Email = "jon.appleseed@yahoo.com";
            var results = userProfile.Validate(null);
            Assert.IsNotEmpty(results.ToList());
        }

        [Test]
        public void Validate_Should_Return_ValidationResult_When_User_Changes_Email_To_One_That_Already_Exists()
        {
            repository.Add(userProfile);
            var results = userProfile.Validate(null);
            Assert.IsEmpty(results.ToList());

            userProfile.Email = "jon.appleseed@yahoo.com";
            results = userProfile.Validate(null);
            Assert.IsNotEmpty(results.ToList());
        }

        [Test]
        public void Validate_Should_Return_Empty_When_Email_Is_Present_But_UserID_Matches_Current_User()
        {
            repository.Add(userProfile);
            var results = userProfile.Validate(null);
            Assert.IsEmpty(results.ToList());
        }
	}
}
