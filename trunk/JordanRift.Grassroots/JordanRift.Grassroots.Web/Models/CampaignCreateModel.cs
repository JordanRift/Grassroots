//
// Copyright © 2011 Jordan Rift, LLC - All Rights Reserved
//
// THIS WORK IS LICENSED UNDER A CREATIVE COMMONS ATTRIBUTION-NONCOMMERCIAL-
// SHAREALIKE 3.0 UNPORTED LICENSE:
// http://creativecommons.org/licenses/by-nc-sa/3.0/
//

using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace JordanRift.Grassroots.Web.Models
{
    public class CampaignCreateModel
    {
        [Required]
        public string Title { get; set; }

        [Required]
        [Display(Name = "In your own words...")]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }
        
        [Display(Name = "Image")]
        public string ImagePath { get; set; }
        
        [Required]
        [Display(Name = "Friendly URL")]
        [Remote("CheckUrlSlug", "Validation")]
        public string UrlSlug { get; set; }

        [Required]
        [UIHint("CauseTemplate")]
        public int CauseTemplateID { get; set; }
        public bool ShouldRenderDropdown { get; set; }
    }
}