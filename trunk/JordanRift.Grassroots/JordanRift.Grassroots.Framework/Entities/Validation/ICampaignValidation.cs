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

namespace JordanRift.Grassroots.Framework.Entities.Validation
{
    public interface ICampaignValidation
    {
        [Required(ErrorMessage = "Please enter a Campaign Title.")]
        string Title { get; set; }

        [Required(ErrorMessage = "Please enter a Campaign Description.")]
        string Description { get; set; }

        [Required(ErrorMessage = "Please enter a Campaign Start Date.")]
        DateTime StartDate { get; set; }

        [Required(ErrorMessage = "Please enter a Campaign End Date.")]
        [GreaterThanDate("StartDate")]
        DateTime EndDate { get; set; }

        [Required(ErrorMessage = "Please enter a Campaign Amount.")]
        decimal GoalAmount { get; set; }

        [Required(ErrorMessage = "Please enter a Campaign URL.")]
        [StringLength(30, MinimumLength = 5, ErrorMessage = "Campaign URL must be between 5 and 30 characters in length.")]
        [RegularExpression(@"^[a-zA-Z0-9]+$", ErrorMessage = "Please enter only alpha-numeric characters.")]
        //[Remote("CheckUrlSlug", "Validation", ErrorMessage = "This URL slug is already in use. Please try another one.")]
        string UrlSlug { get; set; }
    }
}
