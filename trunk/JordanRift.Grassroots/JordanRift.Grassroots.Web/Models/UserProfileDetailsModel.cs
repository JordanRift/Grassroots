//
// Copyright © 2011 Jordan Rift, LLC - All Rights Reserved
//
// THIS WORK IS LICENSED UNDER A CREATIVE COMMONS ATTRIBUTION-NONCOMMERCIAL-
// SHAREALIKE 3.0 UNPORTED LICENSE:
// http://creativecommons.org/licenses/by-nc-sa/3.0/
//

using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace JordanRift.Grassroots.Web.Models
{
	[MetadataType(typeof(UserProfileDetailsModel))]
	public class UserProfileDetailsModel
	{
		[Editable(false)]
		public string UserProfileID { get; set; }

		[Required]
		[DisplayName("First Name")]
		public string FirstName { get; set; }

		[Required]
		[DisplayName("Last Name")]
		public string LastName { get; set; }

		[Editable(false)]
		public string Email { get; set; }

		[Required]
		[UIHint("DateTime")]
		[DisplayName("Birthdate")]
		public DateTime Birthdate { get; set; }

		[Required]
		[UIHint("Gender")]
		public string Gender { get; set; }

		[Required]
		[DisplayName("Primary Phone")]
		[RegularExpression(@"^(?:\([2-9]\d{2}\)\ ?|[2-9]\d{2}(?:\-?|\ ?))[2-9]\d{2}[- ]?\d{4}$",
			ErrorMessage = "Please enter a valid phone number.")]
		public string PrimaryPhone { get; set; }

		[Required]
		[DisplayName("Street Address")]
		public string AddressLine1 { get; set; }
		public string AddressLine2 { get; set; }

		[Required]
		[DisplayName("City")]
		public string City { get; set; }

		[Required]
		[UIHint("State")]
		[DisplayName("State")]
		public string State { get; set; }

		[Required]
		[DisplayName("Zip")]
		[RegularExpression(@"(^\d{5}$)|(^\d{5}-\d{4}$)", ErrorMessage = "Please enter a valid zip code.")]
		public string ZipCode { get; set; }

		[Editable( false )]
		public string FacebookID { get; set; }

		[Editable( false )]
		public string ImagePath { get; set; }

		[Editable(false)]
		public int Age { get; set; }

		[Editable( false )]
		public decimal TotalRaised { get; set; }

		[Editable( false )]
		public decimal TotalHoursServed { get; set; }

		[Editable( false )]
		public int TotalDonationsMade { get; set; }

		[Editable( false )]
		public decimal TotalDonationsGiven { get; set; }

		[Editable( false )]
		public int TotalNumberCampaignsDonatedTo { get; set; }

        [Editable(false)]
        public DateTime LastVisit { get; set; }

		public List<CampaignDetailsModel> Campaigns { get; set; }
	}
}