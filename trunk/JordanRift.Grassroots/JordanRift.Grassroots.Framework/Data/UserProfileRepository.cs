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
using System.Linq;
using JordanRift.Grassroots.Framework.Entities.Models;

namespace JordanRift.Grassroots.Framework.Data
{
    public class UserProfileRepository : GrassrootsRepositoryBase, IUserProfileRepository
    {
        public UserProfile GetUserProfileByID(int id)
        {
            return ObjectContext.UserProfiles.FirstOrDefault(p => p.UserProfileID == id);
        }

        public UserProfile GetUserProfileByFacebookID(string facebookID)
        {
            return ObjectContext.UserProfiles.FirstOrDefault(p => p.FacebookID == facebookID);
        }

        public UserProfile GetUserProfileByActivationHash(string hash)
        {
            return ObjectContext.UserProfiles.FirstOrDefault(p => p.ActivationHash == hash);
        }

        public IEnumerable<UserProfile> FindUserProfileByEmail(string email)
        {
            return ObjectContext.UserProfiles.Where(p => p.Email.Equals(email, StringComparison.CurrentCultureIgnoreCase));
        }

        public bool Exists(string email)
        {
            return ObjectContext.UserProfiles.Any(p => p.Email.Equals(email, StringComparison.CurrentCultureIgnoreCase));
        }

        public void Add(UserProfile userProfile)
        {
            ObjectContext.UserProfiles.Add(userProfile);
        }

        public void Delete(UserProfile userProfile)
        {
            ObjectContext.UserProfiles.Remove(userProfile);
        }

        void IUserProfileRepository.Save()
        {
            base.Save();
        }
    }
}
