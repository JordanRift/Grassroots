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
using System.Web.Mvc;
using JordanRift.Grassroots.Framework.Entities.Models;

namespace JordanRift.Grassroots.Tests.Helpers
{
    public static class TestHelpers
    {
        public static Random random = new Random();

        public static decimal GetAmount()
        {
            return (decimal) random.NextDouble() * 10;
        }

        public static FormCollection GetUserProfileFormPost(UserProfile userProfile)
        {
            FormCollection form = new FormCollection
                                      {
                                          { "FirstName", userProfile.FirstName },
                                          { "LastName", userProfile.LastName },
                                          { "Birthdate", userProfile.Birthdate.ToString() },
                                          { "Gender", userProfile.Gender },
                                          { "Email", userProfile.Email },
                                          { "PrimaryPhone", userProfile.PrimaryPhone },
                                          { "AddressLine1", userProfile.AddressLine1 },
                                          { "AddressLine2", userProfile.AddressLine2 },
                                          { "City", userProfile.City },
                                          { "State", userProfile.State },
                                          { "ZipCode", userProfile.ZipCode },
                                          { "Consent", userProfile.Consent.ToString() }
                                      };
            return form;
        }
    }
}
