//
// Copyright © 2011 Jordan Rift, LLC - All Rights Reserved
//
// THIS WORK IS LICENSED UNDER A CREATIVE COMMONS ATTRIBUTION-NONCOMMERCIAL-
// SHAREALIKE 3.0 UNPORTED LICENSE:
// http://creativecommons.org/licenses/by-nc-sa/3.0/
//

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace JordanRift.Grassroots.Web.Models
{
    public class CampaignDetailsModel
    {
        //
        // Campaign Details
        //

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
        
        public string ImagePath { get; set; }
        public DateTime StartDate { get; set; }
        
        //[GreaterThanDate("StartDate")]
        public DateTime EndDate { get; set; }

        //
        // Donations
        //
        
        public List<DonationDetailsModel> Donations { get; set; }

        //
        // User Profile Details
        //

        public string FirstName { get; set; }
        public string LastName { get; set; }


        //
        // Cause/CauseTemplate Details
        //

        public bool AmountIsConfigurable { get; set; }
        public bool TimespanIsConfigurable { get; set; }
        public string VideoEmbedHtml { get; set; }
    }
}