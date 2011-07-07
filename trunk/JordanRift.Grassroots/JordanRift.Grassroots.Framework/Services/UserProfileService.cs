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

using System.Linq;
using JordanRift.Grassroots.Framework.Data;
using JordanRift.Grassroots.Framework.Helpers;

namespace JordanRift.Grassroots.Framework.Services
{
    public class UserProfileService
    {
        private readonly IUserProfileRepository userProfileRepository;

        public UserProfileService()
        {
            var repositoryFactory = new RepositoryFactory<IUserProfileRepository>();
            userProfileRepository = repositoryFactory.GetRepository();
        }

        ~UserProfileService()
        {
            userProfileRepository.Dispose();
        }

        public UserProfileService(IUserProfileRepository userProfileRepository)
        {
            this.userProfileRepository = userProfileRepository;
        }

        public bool IsUnique(string email, int id)
        {
            using (userProfileRepository)
            {
                var result = true;
                var usersWithSameEmail = userProfileRepository.FindUserProfileByEmail(email).ToList();

                if (id <= 0 && usersWithSameEmail.Count > 0)  // Case: New User
                {
                    result = false;
                }
                else if (usersWithSameEmail.Count > 0 && usersWithSameEmail.Any(u => u.UserProfileID != id))  // Case: Email Change
                {
                    result = false;
                }

                // Case: Saving without changing email
                return result;
            }
        }
    }
}
