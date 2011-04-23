//
// Copyright © 2011 Jordan Rift, LLC - All Rights Reserved
//
// THIS WORK IS LICENSED UNDER A CREATIVE COMMONS ATTRIBUTION-NONCOMMERCIAL-
// SHAREALIKE 3.0 UNPORTED LICENSE:
// http://creativecommons.org/licenses/by-nc-sa/3.0/
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

        [Required]
        [Display(Name = "Year to date goal")]
        decimal YtdGoal { get; set; }

        [Required]
        [Range(1, 12)]
        int FiscalYearStartMonth { get; set; }

        [Required]
        [Range(1, 31)]
        int FiscalYearStartDay { get; set; }

        [MaxLength]
        string Summary { get; set; }

        [AllowHtml]
        [MaxLength]
        string DescriptionHtml { get; set; }

        [RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$",
            ErrorMessage = "Please enter a valid Contact Email Address.")]
        [DataType(System.ComponentModel.DataAnnotations.DataType.EmailAddress)]
        string ContactEmail { get; set; }

        [AllowHtml]
        [StringLength(1000)]
        string VideoEmbedHtml { get; set; }
    }
}
