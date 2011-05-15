//
// Copyright © 2011 Jordan Rift, LLC - All Rights Reserved
//
// THIS WORK IS LICENSED UNDER A CREATIVE COMMONS ATTRIBUTION-NONCOMMERCIAL-
// SHAREALIKE 3.0 UNPORTED LICENSE:
// http://creativecommons.org/licenses/by-nc-sa/3.0/
//

using System;

namespace JordanRift.Grassroots.Web.Models
{
    public class DonationDetailsModel
    {
        //
        // Campaign fields
        //

        public string Title { get; set; }
        public string UrlSlug { get; set; }
        
        //
        // User Profile fields
        //

        public string Email { get; set; }
        
        //
        // CampaignDonor fields
        //

        public decimal Amount { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DonationDate { get; set; }
        public string Comments { get; set; }
    }
}