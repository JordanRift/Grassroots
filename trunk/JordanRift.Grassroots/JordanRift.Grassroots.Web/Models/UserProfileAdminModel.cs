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
using System.ComponentModel.DataAnnotations;
using JordanRift.Grassroots.Framework.Entities.Validation;

namespace JordanRift.Grassroots.Web.Models
{
    public class UserProfileAdminModel : IUserProfileValidation
    {
        public int UserProfileID { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public string PrimaryPhone { get; set; }
        public DateTime Birthdate { get; set; }
        public string Gender { get; set; }
        
        [Display(Name = "User has given consent to our terms?")]
        public bool Consent { get; set; }

        [Display(Name = "Account is active?")]
        public bool Active { get; set; }

        [Display(Name = "User has activated their account?")]
        public bool IsActivated { get; set; }


        public string ActiveCampaignName { get; set; }
        public int ActiveCampaignID { get; set; }
    }
}