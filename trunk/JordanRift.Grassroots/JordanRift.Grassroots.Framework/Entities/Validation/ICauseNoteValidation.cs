//
// Copyright © 2011 Jordan Rift, LLC - All Rights Reserved
//
// THIS WORK IS LICENSED UNDER A CREATIVE COMMONS ATTRIBUTION-NONCOMMERCIAL-
// SHAREALIKE 3.0 UNPORTED LICENSE:
// http://creativecommons.org/licenses/by-nc-sa/3.0/
//

using System.ComponentModel.DataAnnotations;

namespace JordanRift.Grassroots.Framework.Entities.Validation
{
	public interface ICauseNoteValidation
	{
		[Required( ErrorMessage = "Please enter some text for this note." )]
		string Text { get; set; }
	}
}
