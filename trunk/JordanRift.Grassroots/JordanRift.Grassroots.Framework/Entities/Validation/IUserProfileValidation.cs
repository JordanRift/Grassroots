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

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System;

namespace JordanRift.Grassroots.Framework.Entities.Validation
{
	public interface IUserProfileValidation
	{
		[Required]
        [DisplayName("First Name")]
		string FirstName { get; set; }

		[Required]
        [DisplayName("Last Name")]
		string LastName { get; set; }

        [Required]
        [UIHint("DateTime")]
        [DisplayName("Birthdate")]
		DateTime Birthdate { get; set; }

        [Required]
        [UIHint("Gender")]
        string Gender { get; set; }

        [Required]
        [DisplayName("Email address")]
        [RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$",
            ErrorMessage = "Please enter a valid email address.")]
        string Email { get; set; }

        [DisplayName("Primary Phone")]
        [RegularExpression(@"^(?:\([2-9]\d{2}\)\ ?|[2-9]\d{2}(?:\-?|\ ?))[2-9]\d{2}[- ]?\d{4}$",
            ErrorMessage = "Please enter a valid phone number.")]
        string PrimaryPhone { get; set; }

        [DisplayName("Street Address")]
        string AddressLine1 { get; set; }

        [DisplayName("City")]
        string City { get; set; }

        [UIHint("State")]
        [DisplayName("State")]
        string State { get; set; }

        [Required]
        [DisplayName("Zip")]
        [RegularExpression(@"(^\d{5}$)|(^\d{5}-\d{4}$)", ErrorMessage = "Please enter a valid zip code.")]
        string ZipCode { get; set; }

        [Required]
        [CustomValidation(typeof(UserProfileValidation), "ValidateConsent")]
        bool Consent { get; set; }
	}

    public class UserProfileValidation
    {
        public static ValidationResult ValidateConsent(bool consent)
        {
            // Only valid if true
            return consent ? ValidationResult.Success : new ValidationResult("Consent is required.");
        }
    }
}
