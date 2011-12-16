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

namespace JordanRift.Grassroots.Web.Models
{
    public class DonationAdminModel
    {
        //
        // Campaign Donor Info
        //

        public int CampaignDonorID { get; set; }
        
        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        
        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Display(Name = "Display Name (Optional)")]
        public string DisplayName { get; set; }

        [Required]
        [RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$",
            ErrorMessage = "Please enter a valid email address.")]
        public string Email { get; set; }
        
        [Required]
        [Display(Name = "Street Address")]
        public string AddressLine1;
        public string AddressLine2;
        
        [Required]
        public string City;
        
        [Required]
        public string State;
        
        [Required]
        [Display(Name = "Zip Code")]
        public string ZipCode;

        [Required]
        [Display(Name = "Phone Number")]
        public string PrimaryPhone { get; set; }
        
        [Required]
        public decimal Amount { get; set; }

        [Required]
        public DateTime DonationDate { get; set; }
        public bool IsApproved { get; set; }
        public bool IsAnonymous { get; set; }

        //
        // Campaign Info
        //
        public string CampaignTitle { get; set; }
        
        [Required]
        public int CampaignID { get; set; }

        //
        // UserProfile Info
        //

        public int? UserProfileID { get; set; }
    }
}