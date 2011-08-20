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

using System.ComponentModel.DataAnnotations;

namespace JordanRift.Grassroots.Framework.Entities.Validation
{
	public interface ICampaignDonorValidation
	{
		[Required( ErrorMessage = "Please enter your first name." )]
		string FirstName { get; set; }

        [Required( ErrorMessage = "Please enter your last name." )]
        string LastName { get; set; }

        [Required(ErrorMessage = "Please enter your email address.")]
        [RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$",
            ErrorMessage = "Please enter a valid email address.")]
        string Email { get; set; }

        [Required(ErrorMessage = "Please enter an amount.")]
        [Range(1.00, 999999999.00, ErrorMessage = "Please enter an amount between $1.00 USD and $900,000,000 USD.")]
        decimal Amount { get; set; }
	}
}
