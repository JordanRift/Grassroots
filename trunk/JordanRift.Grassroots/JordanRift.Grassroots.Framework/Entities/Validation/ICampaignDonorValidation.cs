//
// Copyright © 2011 Jordan Rift, LLC - All Rights Reserved
//
// THIS WORK IS LICENSED UNDER A CREATIVE COMMONS ATTRIBUTION-NONCOMMERCIAL-
// SHAREALIKE 3.0 UNPORTED LICENSE:
// http://creativecommons.org/licenses/by-nc-sa/3.0/
//

using System.ComponentModel.DataAnnotations;

namespace JordanRift.Grassroots.Framework.Entities.Validation
{
	public interface ICampaignDonorValidation
	{
		[Required( ErrorMessage = "Please enter your first name." )]
		string FirstName { get; set; }

        [Required( ErrorMessage = "Please enter your last name." )]
        string LastName { get; set; }

        // NOTE: We used this one on One Mission. Not sure if it's more strict/accurate... just adding it for reference
        // @"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$"
        [Required(ErrorMessage = "Please enter your email address.")]
        [RegularExpression(@"^(?("")("".+?""@)|(([0-9a-zA-Z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-zA-Z])@))(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-zA-Z][-\w]*[0-9a-zA-Z]\.)+[a-zA-Z]{2,6}))$",
            ErrorMessage = "Please enter a valid email address.")]
        string Email { get; set; }

        [Required(ErrorMessage = "Please enter an amount.")]
        [Range(1.00, 999999999.00, ErrorMessage = "Please enter an amount between $1.00 USD and $900,000,000 USD.")]
        decimal Amount { get; set; }
	}
}
