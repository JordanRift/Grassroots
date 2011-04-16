//
// Copyright © 2011 Jordan Rift, LLC - All Rights Reserved
//
// THIS WORK IS LICENSED UNDER A CREATIVE COMMONS ATTRIBUTION-NONCOMMERCIAL-
// SHAREALIKE 3.0 UNPORTED LICENSE:
// http://creativecommons.org/licenses/by-nc-sa/3.0/
//

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

        public IEnumerable<UserProfile> FindUserProfileByEmail(string email)
        {
            return ObjectContext.UserProfiles.Where(p => p.Email.ToLower() == email.ToLower());
        }

        public bool Exists(string email)
        {
            return ObjectContext.UserProfiles.Any(p => p.Email.ToLower() == email.ToLower());
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
