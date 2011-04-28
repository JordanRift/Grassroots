//
// Copyright © 2011 Jordan Rift, LLC - All Rights Reserved
//
// THIS WORK IS LICENSED UNDER A CREATIVE COMMONS ATTRIBUTION-NONCOMMERCIAL-
// SHAREALIKE 3.0 UNPORTED LICENSE:
// http://creativecommons.org/licenses/by-nc-sa/3.0/
//

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace JordanRift.Grassroots.Web.Models
{
    public class CampaignDetailsModel
    {
        [Editable(false)]
        public int CampaignID { get; set; }

        [Editable(false)]
        public int CauseTemplateID { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }
        
        [Required]
        public decimal GoalAmount { get; set; }
        
        [Required]
        public string UrlSlug { get; set; }
        
        //[Required]
        public string ImagePath { get; set; }
        
        public List<DonationDetailsModel> Donations { get; set; }
    }
}