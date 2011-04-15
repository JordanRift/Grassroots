//
// Copyright © 2011 Jordan Rift, LLC - All Rights Reserved
//
// THIS WORK IS LICENSED UNDER A CREATIVE COMMONS ATTRIBUTION-NONCOMMERCIAL-
// SHAREALIKE 3.0 UNPORTED LICENSE:
// http://creativecommons.org/licenses/by-nc-sa/3.0/
//

using System.Collections.Generic;

namespace JordanRift.Grassroots.Web.Models
{
    public class CampaignDetailsModel
    {
        public int CampaignID { get; set; }
        public int CauseTemplateID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public decimal GoalAmount { get; set; }
        public string UrlSlug { get; set; }
        public string ImagePath { get; set; }
        public List<CampaignDonorDetailsModel> CampaignDonors { get; set; }
    }
}