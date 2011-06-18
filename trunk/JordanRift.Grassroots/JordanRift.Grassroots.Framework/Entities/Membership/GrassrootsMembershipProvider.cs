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
using System.Web.Security;
using JordanRift.Grassroots.Framework.Services;

namespace JordanRift.Grassroots.Framework.Entities.Membership
{
    /// <summary>
    /// Custom membership provider to use Grassroots EF schema.
    /// Notes on implementing custom provider: http://msdn.microsoft.com/en-us/library/f1kyba5e.aspx
    /// </summary>
    public class GrassrootsMembershipProvider : MembershipProvider
    {
        private const string PASSWORD_STRENGTH_REGEX = "";
        private const int MAX_INVALID_PASSWORD_ATTEMPTS = 3;
        private const int PASSWORD_ATTEMP_WINDOW = 10;
        
        private const bool ENABLE_PASSWORD_RESET = true;
        private const bool ENABLE_PASSWORD_RETRIEVAL = true;
        private const bool REQUIRE_Q_AND_A = false;

        // Validated by User object
        private const int REQUIRED_NON_ALPHA_CHARS = 1;
        private const int MIN_PASSWORD_LENGTH = 6;
        private const bool UNIQUE_EMAIL_REQUIRED = true;
        
        public override string ApplicationName { get; set; }

        public override bool EnablePasswordReset
        {
            get { return ENABLE_PASSWORD_RESET; }
        }

        public override bool EnablePasswordRetrieval
        {
            get { return ENABLE_PASSWORD_RETRIEVAL; }
        }

        public override int MaxInvalidPasswordAttempts
        {
            get { return MAX_INVALID_PASSWORD_ATTEMPTS; }
        }

        public override int MinRequiredNonAlphanumericCharacters
        {
            get { return REQUIRED_NON_ALPHA_CHARS; }
        }

        public override int MinRequiredPasswordLength
        {
            get { return MIN_PASSWORD_LENGTH; }
        }

        public override int PasswordAttemptWindow
        {
            get { return PASSWORD_ATTEMP_WINDOW; }
        }

        public override MembershipPasswordFormat PasswordFormat
        {
            get { return MembershipPasswordFormat.Hashed; }
        }

        public override string PasswordStrengthRegularExpression
        {
            get { return PASSWORD_STRENGTH_REGEX; }
        }

        public override bool RequiresQuestionAndAnswer
        {
            get { return REQUIRE_Q_AND_A; }
        }

        public override bool RequiresUniqueEmail
        {
            get { return UNIQUE_EMAIL_REQUIRED; }
        }

        public override bool ChangePassword(string username, string oldPassword, string newPassword)
        {
            var service = new GrassrootsMembershipService();
            return service.ChangePassword(username, oldPassword, newPassword);
        }

        /// <summary>
        /// Intentionally not implemented. Requiring email address as Username. Password change logic will be implemented via email.
        /// </summary>
        [Obsolete("Intentionally not implemented. Requiring email address as Username. Password change logic will be implemented via email.")]
        public override bool ChangePasswordQuestionAndAnswer(string username, string password, string newPasswordQuestion, string newPasswordAnswer)
        {
            throw new NotSupportedException("Intentionally not implemented. Requiring email address as Username. Password change logic will be implemented via email.");
        }

        public override MembershipUser CreateUser(string username, string password, string email, string passwordQuestion, string passwordAnswer, bool isApproved, object providerUserKey, out MembershipCreateStatus status)
        {
            var service = new GrassrootsMembershipService();
            return service.CreateUser(username, password, email, passwordQuestion, passwordAnswer, isApproved, providerUserKey, out status, RequiresUniqueEmail);
        }

        /// <summary>
        /// Sets Active bool to null on User object. For tax purposes, we may not want to delete user accounts outright.
        /// </summary>
        /// <param name="username">Username to deactivate</param>
        /// <param name="deleteAllRelatedData"></param>
        /// <returns>Boolean indicating success</returns>
        public override bool DeleteUser(string username, bool deleteAllRelatedData)
        {
            var service = new GrassrootsMembershipService();
            return service.DeleteUser(username);
        }

        public override MembershipUserCollection FindUsersByEmail(string emailToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            return FindUsersByName(emailToMatch, pageIndex, pageSize, out totalRecords);
        }

        public override MembershipUserCollection FindUsersByName(string usernameToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            var service = new GrassrootsMembershipService();
            return service.FindUsersByName(usernameToMatch, out totalRecords);
        }

        public override MembershipUserCollection GetAllUsers(int pageIndex, int pageSize, out int totalRecords)
        {
            var service = new GrassrootsMembershipService();
            return service.GetAllUsers(pageIndex, pageSize, out totalRecords);
        }

        public override int GetNumberOfUsersOnline()
        {
            var service = new GrassrootsMembershipService();
            return service.GetNumberOfUsersOnline();
        }

        /// <summary>
        /// Purposefully not implemented. Security Q&A not supported with this provider.
        /// </summary>
        public override string GetPassword(string username, string answer)
        {
            throw new ProviderException("Password retrieval is not enabled in this provider.");
        }

        public override MembershipUser GetUser(string username, bool userIsOnline)
        {
            var service = new GrassrootsMembershipService();
            return service.GetUser(username);
        }

        public override MembershipUser GetUser(object providerUserKey, bool userIsOnline)
        {
            var username = providerUserKey.ToString();
            return GetUser(username, userIsOnline);
        }

        /// <summary>
        /// Email == username in data source.
        /// </summary>
        public override string GetUserNameByEmail(string email)
        {
            return email;
        }

        public override string ResetPassword(string username, string answer)
        {
            var service = new GrassrootsMembershipService();
            return service.ResetPassword(username, EnablePasswordReset);
        }

        public override bool UnlockUser(string userName)
        {
            var service = new GrassrootsMembershipService();
            return service.UnlockUser(userName);
        }

        public override void UpdateUser(MembershipUser user)
        {
            var service = new GrassrootsMembershipService();
            service.UpdateUser(user);
        }

        public override bool ValidateUser(string username, string password)
        {
            var service = new GrassrootsMembershipService();
            return service.ValidateUser(username, password, MaxInvalidPasswordAttempts);
        }
    }
}
