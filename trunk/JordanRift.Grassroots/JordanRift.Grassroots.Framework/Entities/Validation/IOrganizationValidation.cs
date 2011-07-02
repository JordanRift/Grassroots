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
using System.Web.Mvc;

namespace JordanRift.Grassroots.Framework.Entities.Validation
{
    public interface IOrganizationValidation
    {
        [Required(ErrorMessage = "Please enter your Organization's ")]
        string Name { get; set; }

        [RegularExpression(@"^(?:\([2-9]\d{2}\)\ ?|[2-9]\d{2}(?:\-?|\ ?))[2-9]\d{2}[- ]?\d{4}$",
            ErrorMessage = "Please enter a valid Contact Phone Number.")]
        string ContactPhone { get; set; }

        [Display(Name = "Year to date goal")]
        decimal? YtdGoal { get; set; }

        [Range(1, 12)]
        int? FiscalYearStartMonth { get; set; }

        [Range(1, 31)]
        int? FiscalYearStartDay { get; set; }

        [AllowHtml]
        [MaxLength]
        string SummaryHtml { get; set; }

        [AllowHtml]
        [MaxLength]
        string DescriptionHtml { get; set; }

        [AllowHtml]
        [MaxLength]
        string FooterHtml { get; set; }

        [RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$",
            ErrorMessage = "Please enter a valid Contact Email Address.")]
        [DataType(System.ComponentModel.DataAnnotations.DataType.EmailAddress)]
        string ContactEmail { get; set; }

        [AllowHtml]
        [StringLength(1000)]
        string VideoEmbedHtml { get; set; }
    }
}
