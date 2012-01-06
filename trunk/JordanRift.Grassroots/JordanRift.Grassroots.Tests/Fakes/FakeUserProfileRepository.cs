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
using System.ComponentModel.Composition;
using System.Linq;
using JordanRift.Grassroots.Framework.Data;
using JordanRift.Grassroots.Framework.Entities;
using JordanRift.Grassroots.Framework.Entities.Models;
using JordanRift.Grassroots.Tests.Helpers;

namespace JordanRift.Grassroots.Tests.Fakes
{
    [Export(typeof(IUserProfileRepository))]
    public class FakeUserProfileRepository : IUserProfileRepository
    {
        public static IList<UserProfile> profiles;
        public PriorityType Priority { get; set; }

        static FakeUserProfileRepository()
        {
            SetUp();
        }

        private static void SetUp()
        {
            profiles = new List<UserProfile>();

            for (int i = 0; i < 5; i++)
            {
                var profile = EntityHelpers.GetValidUserProfile();
                profile.UserProfileID = i + 1;
                profile.Email = string.Format("email{0}@yahoo.com", i);
                profile.CampaignDonors = new List<CampaignDonor>();
                profiles.Add(profile);
            }

            profiles.First().Email = "jon.appleseed@yahoo.com";
        }

        public FakeUserProfileRepository()
        {
            Priority = PriorityType.High;
        }

        public static void Reset()
        {
            SetUp();
        }
        
        public UserProfile GetUserProfileByID(int id)
        {
            return profiles.FirstOrDefault(p => p.UserProfileID == id);
        }

        public UserProfile GetUserProfileByFacebookID(string facebookID)
        {
            return profiles.FirstOrDefault(p => p.FacebookID == facebookID);
        }

        public UserProfile GetUserProfileByActivationHash(string hash)
        {
            return profiles.FirstOrDefault(p => p.ActivationHash == hash);
        }

        public IQueryable<UserProfile> FindAllUserProfiles()
        {
            return profiles.AsQueryable();
        }

        public IQueryable<UserProfile> FindUserProfileByEmail(string email)
        {
            return profiles.Where(p => p.Email.ToLower() == email.ToLower()).AsQueryable();
        }

        public bool Exists(string email)
        {
            return profiles.Any(p => p.Email.ToLower() == email.ToLower());
        }

        public void Add(UserProfile userProfile)
        {
            userProfile.UserProfileID = profiles.Count + 1;
            //userProfile.UserID = new Guid();
            profiles.Add(userProfile);
        }

        public void Delete(UserProfile userProfile)
        {
            profiles.Remove(userProfile);
        }

        public void Save()
        {
        }

        public void Dispose()
        {
        }
    }
}
