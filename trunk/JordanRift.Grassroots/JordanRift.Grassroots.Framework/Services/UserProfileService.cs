//
// Copyright © 2011 Jordan Rift, LLC - All Rights Reserved
//
// THIS WORK IS LICENSED UNDER A CREATIVE COMMONS ATTRIBUTION-NONCOMMERCIAL-
// SHAREALIKE 3.0 UNPORTED LICENSE:
// http://creativecommons.org/licenses/by-nc-sa/3.0/
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
            userProfileRepository = RepositoryFactory.GetRepository<IUserProfileRepository>();
        }

        public UserProfileService(IUserProfileRepository userProfileRepository)
        {
            this.userProfileRepository = userProfileRepository;
        }

        public bool IsUnique(string email, int id)
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
