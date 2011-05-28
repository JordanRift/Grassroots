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

        [Editable(false)]
        public bool Active { get; set; }

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

        [Editable(false)]
        public string Role { get; set; }

        public bool CurrentUserIsOwner { get; set; }
	}
}