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

using System.Linq;
using System.Web.Security;
using JordanRift.Grassroots.Framework.Data;
using JordanRift.Grassroots.Framework.Services;
using JordanRift.Grassroots.Tests.Helpers;
using JordanRift.Grassroots.Web.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace JordanRift.Grassroots.Tests.Fakes
{
    public class MockMembershipService : IMembershipService
    {
        private IUserProfileRepository userProfileRepository;

        public MockMembershipService()
        {
            userProfileRepository = new FakeUserProfileRepository();
        }

        public int MinPasswordLength
        {
            get { return 10; }
        }

        public int MaxInvalidPasswordAttempts
        {
            get { return 3; }
        }

        public bool ValidateUser(string userName, string password)
        {
            return (userName == "goodEmail" && password == "goodPassword");
        }

        public MembershipCreateStatus CreateUser(string userName, string password, string email)
        {
            var userProfile = userProfileRepository.FindUserProfileByEmail(userName).FirstOrDefault();

            if (userProfile != null)
            {
                var user = EntityHelpers.GetValidUser();
                user.Username = userName;
                user.Password = GrassrootsMembershipService.HashPassword(password, new byte[0]);
                userProfile.Users.Add(user);
            }

            if (userName == "duplicateEmail")
            {
                return MembershipCreateStatus.DuplicateUserName;
            }

            // verify that values are what we expected
            Assert.AreEqual("goodPassword", password);
            Assert.AreEqual("goodEmail", email);

            return MembershipCreateStatus.Success;
        }

        public bool ChangePassword(string userName, string oldPassword, string newPassword)
        {
            return (userName == "goodEmail" && oldPassword == "goodOldPassword" && newPassword == "goodNewPassword");
        }

        public string ResetPassword(string email)
        {
            if (email == "info@jordanrift.com")
            {
                return "newscrambledpassword";
            }

            return null;
        }
    }
}
