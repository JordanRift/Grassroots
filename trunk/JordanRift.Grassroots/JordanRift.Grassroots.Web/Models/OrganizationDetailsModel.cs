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
using System.ComponentModel;
using System.Web.Mvc;

namespace JordanRift.Grassroots.Web.Models
{
	[MetadataType( typeof( OrganizationDetailsModel ) )]
	public class OrganizationDetailsModel
	{
		[Editable(false)]
		public int OrganizationID { get; set; }

		[Required]
		[DisplayName( "Organization Name" )]
		public string Name { get; set; }

        [Required]
        public string Tagline { get; set; }

        [Required]
        [Display(Name = "Fiscal Year Start Month")]
        [Range(1, 12)]
        public int FiscalYearStartMonth { get; set; }

        [Required]
        [Display(Name = "Fiscal Year Start Day")]
        [Range(1, 31)]
        public int FiscalYearStartDay { get; set; }

        public string Summary { get; set; }

        [AllowHtml]
        [Display(Name = "Description (HTML)")]
        public string DescriptionHtml { get; set; }

		[Required]
		[RegularExpression( @"^(?:\([2-9]\d{2}\)\ ?|[2-9]\d{2}(?:\-?|\ ?))[2-9]\d{2}[- ]?\d{4}$",
			ErrorMessage = "Please enter a valid phone number: (xxx) xxx-xxxx" )]
		[DisplayName( "Administrator Phone" )]
		public string ContactPhone { get; set; }

		[Required]
		[DisplayName( "Administrator Email" )]
		[RegularExpression( @"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$",
			ErrorMessage = "Please enter a valid email address." )]
		public string ContactEmail { get; set; }

        [Required]
        [DisplayName("Year To Date Goal")]
        [DataType(DataType.Currency)]
        public decimal YtdGoal { get; set; }

		[DisplayName( "Organization's Twitter Username" )]
		public string TwitterName { get; set; }

        [DisplayName("Organization's Facebook Page")]
        public string FacebookPageUrl { get; set; }

		[DisplayName( "Gateway Type" )]
		[UIHint( "PaymentGatewayType" )]
        [Required(ErrorMessage = "Payment Gateway Type is required")]
		public int PaymentGatewayType { get; set; }

        [DisplayName("Gateway API URL")]
        [Required(ErrorMessage = "Payment Gateway API URL is required")]
        public string PaymentGatewayApiUrl { get; set; }

		[DisplayName( "Gateway API Username/Key" )]
        [Required]
		public string PaymentGatewayApiKey { get; set; }

		[DisplayName( "Gateway API Secret" )]
        [Required]
		public string PaymentGatewayApiSecret { get; set; }

        [AllowHtml]
		[DisplayName( "Video Embed (HTML)" )]
		public string VideoEmbedHtml { get; set; }

        [DisplayName("Blog RSS Feed URL")]
        public string BlogRssUrl { get; set; }

        [Required]
        [DisplayName("Theme Name")]
        public string ThemeName { get; set; }
	}
}