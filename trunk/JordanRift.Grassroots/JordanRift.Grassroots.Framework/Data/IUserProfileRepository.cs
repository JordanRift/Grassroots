//
// Copyright © 2011 Jordan Rift, LLC - All Rights Reserved
//
// THIS WORK IS LICENSED UNDER A CREATIVE COMMONS ATTRIBUTION-NONCOMMERCIAL-
// SHAREALIKE 3.0 UNPORTED LICENSE:
// http://creativecommons.org/licenses/by-nc-sa/3.0/
//

using System.Collections.Generic;
using JordanRift.Grassroots.Framework.Entities.Models;

namespace JordanRift.Grassroots.Framework.Data
{
    public interface IUserProfileRepository
    {
        UserProfile GetUserProfileByID(int id);
        UserProfile GetUserProfileByFacebookID(string facebookID);
        IEnumerable<UserProfile> FindUserProfileByEmail(string email);
        bool Exists(string email);
        void Add(UserProfile userProfile);
        void Delete(UserProfile userProfile);
        void Save();
    }
}
