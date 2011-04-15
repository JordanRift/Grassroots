﻿//
// Copyright © 2011 Jordan Rift, LLC - All Rights Reserved
//
// THIS WORK IS LICENSED UNDER A CREATIVE COMMONS ATTRIBUTION-NONCOMMERCIAL-
// SHAREALIKE 3.0 UNPORTED LICENSE:
// http://creativecommons.org/licenses/by-nc-sa/3.0/
//

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Web.Mvc;

namespace JordanRift.Grassroots.Web.Models
{
	[MetadataType( typeof( OrganizationEditModel ) )]
	public class OrganizationEditModel
	{
		[Editable(false)]
		public int OrganizationID { get; set; }

		[Required]
		[DisplayName( "Organization Name" )]
		public string Name { get; set; }

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

		[DisplayName( "Organization's Twitter Username" )]
		public string TwitterName { get; set; }

		[DisplayName( "Gateway Type" )]
		[UIHint( "PaymentGatewayType" )]
		public int PaymentGatewayType { get; set; }

		[DisplayName( "Gateway API Username/Key" )]
		public string PaymentGatewayApiKey { get; set; }

		[DisplayName( "Gateway API Secret" )]
		public string PaymentGatewayApiSecret { get; set; }

        [AllowHtml]
		[DisplayName( "Video Embed (HTML)" )]
		public string VideoEmbedHtml { get; set; }
	}
}