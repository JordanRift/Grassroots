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
using System.Collections.Generic;
using System.Configuration.Provider;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web.Security;
using JordanRift.Grassroots.Framework.Data;
using JordanRift.Grassroots.Framework.Entities.Models;
using JordanRift.Grassroots.Framework.Helpers;

namespace JordanRift.Grassroots.Framework.Services
{
    public class GrassrootsMembershipService
    {
        private readonly IUserProfileRepository userProfileRepository;
        private readonly IUserRepository userRepository;

        public GrassrootsMembershipService()
        {
            var userProfileRepositoryFactory = new RepositoryFactory<IUserProfileRepository>();
            userProfileRepository = userProfileRepositoryFactory.GetRepository();

            var userRepositoryFactory = new RepositoryFactory<IUserRepository>();
            userRepository = userRepositoryFactory.GetRepository();
        }

        public bool ChangePassword(string username, string oldPassword, string newPassword)
        {
            var user = userRepository.GetUserByName(username);

            if (user == null)
            {
                throw new ProviderException(string.Format("Unable to find user with email '{0}'.", username));
            }

            if (!VerifyPasswordHash(oldPassword, user.Password))
            {
                return false;
            }

            user.Password = HashPassword(newPassword, null);
            userRepository.Save();
            return true;
        }

        public static string HashPassword(string passwordText, byte[] saltBytes)
        {
            if (saltBytes == null)
            {
                const int MIN_SALT_SIZE = 4;
                const int MAX_SALT_SIZE = 8;
                Random random = new Random();
                var saltLength = random.Next(MIN_SALT_SIZE, MAX_SALT_SIZE);
                saltBytes = new byte[saltLength];
                RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
                rng.GetNonZeroBytes(saltBytes);
            }

            var plainTextBytes = Encoding.UTF8.GetBytes(passwordText);
            var plainTextWithSaltBytes = new byte[plainTextBytes.Length + saltBytes.Length];

            for (int i = 0; i < plainTextBytes.Length; i++)
            {
                plainTextWithSaltBytes[i] = plainTextBytes[i];
            }

            for (int i = 0; i < saltBytes.Length; i++)
            {
                plainTextWithSaltBytes[plainTextBytes.Length + i] = saltBytes[i];
            }

            var hash = new SHA1Managed();
            var hashBytes = hash.ComputeHash(plainTextWithSaltBytes);
            byte[] hashWithSaltBytes = new byte[hashBytes.Length + saltBytes.Length];

            for (int i = 0; i < hashBytes.Length; i++)
            {
                hashWithSaltBytes[i] = hashBytes[i];
            }

            for (int i = 0; i < saltBytes.Length; i++)
            {
                hashWithSaltBytes[hashBytes.Length + i] = saltBytes[i];
            }

            string hashValue = Convert.ToBase64String(hashWithSaltBytes);
            return hashValue;
        }

        public static bool VerifyPasswordHash(string plainText, string hashedPassword)
        {
            byte[] hashWithSaltBytes = Convert.FromBase64String(hashedPassword);
            const int HASH_SIZE_IN_BYTES = 160 / 8;

            if (hashWithSaltBytes.Length < HASH_SIZE_IN_BYTES)
            {
                return false;
            }

            byte[] saltBytes = new byte[hashWithSaltBytes.Length - HASH_SIZE_IN_BYTES];

            for (int i = 0; i < saltBytes.Length; i++)
            {
                saltBytes[i] = hashWithSaltBytes[HASH_SIZE_IN_BYTES + i];
            }

            string expectedHashString = HashPassword(plainText, saltBytes);
            return (hashedPassword == expectedHashString);

        }

        public MembershipUser CreateUser(string username, string password, string email, string passwordQuestion, string passwordAnswer, bool isApproved, object providerUserKey, out MembershipCreateStatus status, bool requiresUniqueEmail)
        {
            var userProfile = userProfileRepository.FindUserProfileByEmail(username).FirstOrDefault();

            if (userProfile == null)
            {
                throw new MembershipCreateUserException(string.Format("Unable to find UserProfile that corresponds to '{0}'.", username));
            }

            if (requiresUniqueEmail && UserProfile.IsUnique(null, email, 0))
            {
                status = MembershipCreateStatus.DuplicateEmail;
                return null;
            }

            try
            {
                var user = new User
                               {
                                   Username = username,
                                   Password = HashPassword(password, null),
                                   IsActive = true,
                                   IsAuthorized = false,
                                   RegisterDate = DateTime.Now,
                                   LastLoggedIn = DateTime.Now
                               };

                if (userProfile.Users == null)
                {
                    userProfile.Users = new List<User>();
                }

                userProfile.Users.Add(user);
                userRepository.Save();
                status = MembershipCreateStatus.Success;
                return user.GetMembershipUser();
            }
            catch (Exception)
            {
                status = MembershipCreateStatus.ProviderError;
                return null;
            }
        }

        public string ResetPassword(string username, bool enablePasswordReset)
        {
            if (!enablePasswordReset)
            {
                throw new NotSupportedException("Password reset is not supported.");
            }

            var user = userRepository.GetUserByName(username);

            if (user == null)
            {
                throw new MembershipPasswordException(string.Format("The user '{0}' could not be found.", username));
            }

            var newPassword = Membership.GeneratePassword(12, 4);
            user.Password = HashPassword(newPassword, null);
            userRepository.Save();
            return newPassword;
        }

        public bool ValidateUser(string username, string password, int maxInvalidPasswordAttempts)
        {
            var user = userRepository.GetUserByName(username);

            if (user == null)
            {
                return false;
            }

            if (VerifyPasswordHash(password, user.Password))
            {
                user.LastLoggedIn = DateTime.Now;
				user.FailedLoginAttempts = 0;
                userRepository.Save();
                return true;
            }

			user.FailedLoginAttempts++;
			userRepository.Save();

			// Throttle, as suggested by http://www.codinghorror.com/blog/2009/01/dictionary-attacks-101.html
			if ( user.FailedLoginAttempts > maxInvalidPasswordAttempts )
			{
				// sleep an extra second up to a max of 30 seconds
				int sleepFor = ( user.FailedLoginAttempts < 30 ) ? user.FailedLoginAttempts * 1000 : 30000;
				System.Threading.Thread.Sleep( sleepFor );
			}

			return false;
        }

        public bool DeleteUser(string username)
        {
            var user = userRepository.GetUserByName(username);

            if (user != null)
            {
                user.IsActive = false;
                var userProfile = user.UserProfile;
                userProfile.Active = false;
                userRepository.Save();
                return true;
            }

            return false;
        }

        public MembershipUserCollection FindUsersByName(string usernameToMatch, out int totalRecords)
        {
            totalRecords = 0;
            var user = userRepository.GetUserByName(usernameToMatch);
            var users = new MembershipUserCollection();

            if (user != null)
            {
                users.Add(user.GetMembershipUser());
                totalRecords = 1;
            }

            return users;
        }

        public MembershipUserCollection GetAllUsers(int pageIndex, int pageSize, out int totalRecords)
        {
            var users = userRepository.FindAllUsers();
            totalRecords = users.Count();
            var query = users.Skip(pageIndex * pageSize).Take(pageSize);
            return MapQuery(query);
        }

        private static MembershipUserCollection MapQuery(IEnumerable<User> query)
        {
            var users = new MembershipUserCollection();

            foreach (var user in query)
            {
                users.Add(user.GetMembershipUser());
            }

            return users;
        }

        public int GetNumberOfUsersOnline()
        {
            return userRepository.FindAllUsers()
                .Where(u => u.LastLoggedIn > (DateTime.Now.AddMinutes(Membership.UserIsOnlineTimeWindow * -1)))
                .Count();
        }

        public MembershipUser GetUser(string username)
        {
            var user = userRepository.GetUserByName(username);
            return user != null ? user.GetMembershipUser() : null;
        }

        public bool UnlockUser(string userName)
        {
            var user = userRepository.GetUserByName(userName);

            if (user == null)
            {
                return false;
            }

            user.IsActive = true;
            userRepository.Save();
            return true;
        }

        public void UpdateUser(MembershipUser user)
        {
            var grassrootsUser = userRepository.GetUserByName(user.UserName);
            grassrootsUser.Username = user.UserName;
            grassrootsUser.IsActive = user.IsApproved;
            grassrootsUser.IsAuthorized = user.IsLockedOut;
            userRepository.Save();
        }

        public string GetUserAuthorizationHash()
        {
            var authString = Guid.NewGuid().ToString();
            var md5 = new MD5CryptoServiceProvider();
            byte[] bytes = Encoding.ASCII.GetBytes(authString);
            bytes = md5.ComputeHash(bytes);
            string result = string.Empty;

            foreach (byte b in bytes)
            {
                result += b.ToString("x2");
            }

            return result;
        }

        public bool IsActivationHashValid(UserProfile userProfile)
        {
            var hoursElapsed = DateTime.Now - userProfile.LastActivationAttempt;
            return hoursElapsed.Hours <= 1;
        }

        /// <summary>
        /// Generates a secure random number for a user to verify password reset
        /// </summary>
        /// <returns>Random pin number</returns>
        public string GenerateRandomPin()
        {
            var rng = new RNGCryptoServiceProvider();
            byte[] tokenData = new byte[8];
            rng.GetBytes(tokenData);
            return Convert.ToBase64String(tokenData);
        }

        public bool UpdatePassword(UserProfile userProfile, string activationPin, string password)
        {
            if (activationPin == userProfile.ActivationPin && userProfile.Users.Any())
            {
                var user = userProfile.Users.First();
                user.Password = HashPassword(password, null);
                userProfile.ActivationPin = null;
                userProfileRepository.Save();
                return true;
            }

            return false;
        }
    }
}
