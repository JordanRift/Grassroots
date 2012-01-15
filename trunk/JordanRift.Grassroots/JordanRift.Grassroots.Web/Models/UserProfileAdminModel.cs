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

        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        
        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required]
        [Display(Name = "Email address")]
        [RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$",
            ErrorMessage = "Please enter a valid email address.")]
        public string Email { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string City { get; set; }
        
        [UIHint("State")]
        public string State { get; set; }

        [Required]
        [Display(Name = "Zip Code")]
        public string ZipCode { get; set; }

        [Required]
        [Display(Name = "Primary Phone")]
        [RegularExpression(@"^(?:\([2-9]\d{2}\)\ ?|[2-9]\d{2}(?:\-?|\ ?))[2-9]\d{2}[- ]?\d{4}$",
            ErrorMessage = "Please enter a valid phone number.")]
        public string PrimaryPhone { get; set; }

        [UIHint("DateTime")]
        public DateTime Birthdate { get; set; }
        
        [UIHint("Gender")]
        public string Gender { get; set; }
        
        [Display(Name = "User has given consent to our terms?")]
        public bool Consent { get; set; }

        [Display(Name = "Account is active?")]
        public bool Active { get; set; }

        [Display(Name = "User has activated their account?")]
        public bool IsActivated { get; set; }

        [Display(Name = "Security Role")]
        [UIHint("Role")]
        public int? RoleID { get; set; }

        public string ActiveCampaignName { get; set; }
        public int ActiveCampaignID { get; set; }
    }
}