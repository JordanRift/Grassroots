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
	public class CauseTemplateDetailsModel
	{
		[Editable( false )]
		public int CauseTemplateID { get; set; }

		[Required]
		public string Name { get; set; }

		[Required]
		[DisplayName( "Action Verb" )]
		public string ActionVerb { get; set; }

		[Required]
		[DisplayName( "Goal Name" )]
		public string GoalName { get; set; }

		[Required]
		public bool Active { get; set; }

		[Required]
		[DisplayName( "Amount is Configurable/Changeable" )]
		public bool AmountIsConfigurable { get; set; }

		[Required]
		[DisplayName( "Default Amount" )]
		[DataType( DataType.Currency )]
		[Range( 1, 99999, ErrorMessage = "Please enter a valid amount more than 0 but less than 99,999." )]
		public decimal DefaultAmount { get; set; }

		[Required]
		[DisplayName( "Timespan is Configurable/Changeable" )]
		public bool TimespanIsConfigurable { get; set; }

		[Required]
		[DisplayName( "Default Timespan (in days)" )]
		[Range(1, 999, ErrorMessage = "Please enter a number more than 0 but less than 999 days." )]
		public int DefaultTimespanInDays { get; set; }

		[Required]
		[DataType( DataType.MultilineText )]
		public string Summary { get; set; }

		[Required]
		[AllowHtml]
		[DataType( DataType.MultilineText )]
		[DisplayName( "Video (HTML)" )]
		public string VideoEmbedHtml { get; set; }

		[Required]
		[AllowHtml]
		[DataType( DataType.MultilineText )]
		[DisplayName( "Description (HTML)" )]
		public string DescriptionHtml { get; set; }

		[Required]
		[AllowHtml]
		[DataType( DataType.MultilineText )]
		[DisplayName( "Instructions Open (HTML)" )]
		public string InstructionsOpenHtml { get; set; }

		[Required]
		[AllowHtml]
		[DataType( DataType.MultilineText )]
		[DisplayName( "Instructions Closed (HTML)" )]
		public string InstructionsClosedHtml { get; set; }

		[Required]
        [Display(Name = "Image Path")]
		public string ImagePath { get; set; }

        [Display(Name = "Before Image Path")]
        public string BeforeImagePath { get; set; }
        
        [Display(Name = "After Image Path")]
        public string AfterImagePath { get; set; }
	}
}