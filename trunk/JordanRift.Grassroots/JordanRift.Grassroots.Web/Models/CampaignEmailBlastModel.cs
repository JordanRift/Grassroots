//
// Copyright © 2011 Jordan Rift, LLC - All Rights Reserved
//
// THIS WORK IS LICENSED UNDER A CREATIVE COMMONS ATTRIBUTION-NONCOMMERCIAL-
// SHAREALIKE 3.0 UNPORTED LICENSE:
// http://creativecommons.org/licenses/by-nc-sa/3.0/
//

using System.ComponentModel.DataAnnotations;

namespace JordanRift.Grassroots.Web.Models
{
    public class CampaignEmailBlastModel
    {
        public string UrlSlug { get; set; }
        public string Title { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        [Required]
        [Display(Name = "Email Addresses")]
        [RegularExpression(@"^((\s*[a-zA-Z0-9\._%-]+@[a-zA-Z0-9\.-]+\.[a-zA-Z]{2,4}\s*[,;]){1,5}?)?(\s*[a-zA-Z0-9\._%-]+@[a-zA-Z0-9\.-]+\.[a-zA-Z]{2,4})*$", 
            ErrorMessage = "Please enter between 1 and 5 valid email addresses")]
        public string EmailAddresses { get; set; }

        [Display(Name = "Email Message")]
        public string CustomMessage { get; set; }
    }
}