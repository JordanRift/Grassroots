//
// Copyright © 2011 Jordan Rift, LLC - All Rights Reserved
//
// THIS WORK IS LICENSED UNDER A CREATIVE COMMONS ATTRIBUTION-NONCOMMERCIAL-
// SHAREALIKE 3.0 UNPORTED LICENSE:
// http://creativecommons.org/licenses/by-nc-sa/3.0/
//

using System;
using System.Collections.Generic;
using System.Linq;
using JordanRift.Grassroots.Framework.Data;
using JordanRift.Grassroots.Framework.Entities.Models;
using JordanRift.Grassroots.Tests.Helpers;

namespace JordanRift.Grassroots.Tests.Fakes
{
    [Obsolete("This class will be obsolete in future versions in favor of using Rhino Mocks. See DonateControllerTests for example of new pattern.")]
    public class FakeUserProfileRepository : IUserProfileRepository
    {
        public static IList<UserProfile> profiles;

        public void SetUpRepository()
        {
            profiles = new List<UserProfile>();

            for (int i = 0; i < 5; i++)
            {
                var profile = EntityHelpers.GetValidUserProfile();
                profile.UserProfileID = i + 1;
                profile.Email = string.Format("email{0}@yahoo.com", i);
                profiles.Add(profile);
            }

            profiles.First().Email = "jon.appleseed@yahoo.com";
        }

        public UserProfile GetUserProfileByID(int id)
        {
            return profiles.FirstOrDefault(p => p.UserProfileID == id);
        }

        public UserProfile GetUserProfileByFacebookID(string facebookID)
        {
            return profiles.FirstOrDefault(p => p.FacebookID == facebookID);
        }

        public IEnumerable<UserProfile> FindUserProfileByEmail(string email)
        {
            return profiles.Where(p => p.Email.ToLower() == email.ToLower());
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
    }
}
