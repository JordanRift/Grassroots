//
// Copyright © 2011 Jordan Rift, LLC - All Rights Reserved
//
// THIS WORK IS LICENSED UNDER A CREATIVE COMMONS ATTRIBUTION-NONCOMMERCIAL-
// SHAREALIKE 3.0 UNPORTED LICENSE:
// http://creativecommons.org/licenses/by-nc-sa/3.0/
//

using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using JordanRift.Grassroots.Framework.Entities.Validation;

namespace JordanRift.Grassroots.Web.Models
{
    [MetadataType(typeof(IUserProfileValidation))]
    public class FacebookRegisterModel : IUserProfileValidation
    {
        [Required]
        public string FacebookID { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        [DisplayName("Email Address")]
        [Remote("CheckEmail", "Validation", ErrorMessage = "That email address is already in use. Please try another one.")]
        [RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$",
            ErrorMessage = "Please enter a valid email address.")]
        public string Email { get; set; }

        // User Profile
        [DataType(DataType.Text)]
        public string FirstName { get; set; }

        [DataType(DataType.Text)]
        public string LastName { get; set; }

        [DataType(DataType.Date)]
        public DateTime Birthdate { get; set; }

        [DataType(DataType.Text)]
        public string Gender { get; set; }

        [DataType(DataType.Text)]
        public string PrimaryPhone { get; set; }

        [DataType(DataType.Text)]
        public string AddressLine1 { get; set; }

        [DataType(DataType.Text)]
        public string AddressLine2 { get; set; }

        [DataType(DataType.Text)]
        public string City { get; set; }

        [DataType(DataType.Text)]
        public string State { get; set; }

        [DataType(DataType.Text)]
        public string ZipCode { get; set; }

        public bool Consent { get; set; }

        public string ImagePath { get; set; }
    }
}