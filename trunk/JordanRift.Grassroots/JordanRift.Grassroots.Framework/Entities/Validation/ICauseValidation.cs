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
    public interface ICauseValidation
    {
        [Required(ErrorMessage = "Name is required.")]
        string Name { get; set; }
        
        [Required(ErrorMessage = "Summary is required.")]
        string Summary { get; set; }

        [Required(ErrorMessage = "Description is required.")]
        [Display(Name = "Description")]
        string DescriptionHtml { get; set; }

        [Display(Name = "Video Embed Code")]
        string VideoEmbedHtml { get; set; }

        [Display(Name = "Image")]
        string ImagePath { get; set; }

        bool Active { get; set; }
    }
}
