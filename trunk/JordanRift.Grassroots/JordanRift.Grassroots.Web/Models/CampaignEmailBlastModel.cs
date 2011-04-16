//
// Copyright © 2011 Jordan Rift, LLC - All Rights Reserved
//
// THIS WORK IS LICENSED UNDER A CREATIVE COMMONS ATTRIBUTION-NONCOMMERCIAL-
// SHAREALIKE 3.0 UNPORTED LICENSE:
// http://creativecommons.org/licenses/by-nc-sa/3.0/
//

namespace JordanRift.Grassroots.Web.Models
{
    public class CampaignEmailBlastModel
    {
        public string Siteurl { get; set; }
        public string UrlSlug { get; set; }
        public string Title { get; set; }
        public string EmailAddresses { get; set; }
        public string CustomMessage { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}