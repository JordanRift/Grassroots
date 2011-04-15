//
// Copyright © 2011 Jordan Rift, LLC - All Rights Reserved
//
// THIS WORK IS LICENSED UNDER A CREATIVE COMMONS ATTRIBUTION-NONCOMMERCIAL-
// SHAREALIKE 3.0 UNPORTED LICENSE:
// http://creativecommons.org/licenses/by-nc-sa/3.0/
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

        [Required]
        [DisplayName("Primary Phone")]
        [RegularExpression(@"^(?:\([2-9]\d{2}\)\ ?|[2-9]\d{2}(?:\-?|\ ?))[2-9]\d{2}[- ]?\d{4}$",
            ErrorMessage = "Please enter a valid phone number.")]
        string PrimaryPhone { get; set; }

        [Required]
        [DisplayName("Street Address")]
        string AddressLine1 { get; set; }

        [Required]
        [DisplayName("City")]
        string City { get; set; }

        [Required]
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
