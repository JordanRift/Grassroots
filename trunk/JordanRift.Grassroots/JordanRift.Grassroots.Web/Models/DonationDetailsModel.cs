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

namespace JordanRift.Grassroots.Web.Models
{
    public class DonationDetailsModel
    {
        //
        // Campaign fields
        //

        public string Title { get; set; }
        public string UrlSlug { get; set; }
        
        //
        // User Profile fields
        //

        public string Email { get; set; }
        public int? UserProfileID { get; set; }
        
        //
        // CampaignDonor fields
        //

        public decimal Amount { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public DateTime DonationDate { get; set; }
        public string Comments { get; set; }

        //
        // Payment fields
        //

        public string PaymentType { get; set; }

        //
        // Organization fields
        //

        public string DonorNotificationEmail { get; set; }
    }
}