//
// Copyright © 2011 Jordan Rift, LLC - All Rights Reserved
//
// THIS WORK IS LICENSED UNDER A CREATIVE COMMONS ATTRIBUTION-NONCOMMERCIAL-
// SHAREALIKE 3.0 UNPORTED LICENSE:
// http://creativecommons.org/licenses/by-nc-sa/3.0/
//

using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

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
        [Remote("CheckUrlSlug", "Validation", ErrorMessage = "This URL slug is already in use. Please try another one.")]
        string UrlSlug { get; set; }
    }
}
