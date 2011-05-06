//
// Copyright © 2011 Jordan Rift, LLC - All Rights Reserved
//
// THIS WORK IS LICENSED UNDER A CREATIVE COMMONS ATTRIBUTION-NONCOMMERCIAL-
// SHAREALIKE 3.0 UNPORTED LICENSE:
// http://creativecommons.org/licenses/by-nc-sa/3.0/
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
		public string ImagePath { get; set; }
	}
}