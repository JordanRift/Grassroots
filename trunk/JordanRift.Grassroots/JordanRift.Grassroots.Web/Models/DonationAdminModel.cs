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

namespace JordanRift.Grassroots.Web.Models
{
    public class DonationAdminModel
    {
        //
        // Campaign Donor Info
        //

        // TODO: Add validation attributes
        public int CampaignDonorID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string AddressLine1;
        public string AddressLine2;
        public string City;
        public string State;
        public string ZipCode;
        public string PrimaryPhone;
        public decimal Amount { get; set; }
        public bool Approved { get; set; }
        public bool IsAnonymous { get; set; }

        //
        // Campaign Info
        //
        public string CampaignTitle { get; set; }
        public int CampaignID { get; set; }

        //
        // UserProfile Info
        //

        public int? UserProfileID { get; set; }
    }
}