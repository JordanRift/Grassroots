//
// Copyright © 2011 Jordan Rift, LLC - All Rights Reserved
//
// THIS WORK IS LICENSED UNDER A CREATIVE COMMONS ATTRIBUTION-NONCOMMERCIAL-
// SHAREALIKE 3.0 UNPORTED LICENSE:
// http://creativecommons.org/licenses/by-nc-sa/3.0/
//

using System;
using System.Web.Mvc;
using JordanRift.Grassroots.Framework.Entities.Models;

namespace JordanRift.Grassroots.Tests.Helpers
{
    public static class TestHelpers
    {
        public static Random random = new Random();

        public static decimal GetAmount()
        {
            return (decimal) random.NextDouble() * 10;
        }

        public static FormCollection GetUserProfileFormPost(UserProfile userProfile)
        {
            FormCollection form = new FormCollection
                                      {
                                          { "FirstName", userProfile.FirstName },
                                          { "LastName", userProfile.LastName },
                                          { "Birthdate", userProfile.Birthdate.ToString() },
                                          { "Gender", userProfile.Gender },
                                          { "Email", userProfile.Email },
                                          { "PrimaryPhone", userProfile.PrimaryPhone },
                                          { "AddressLine1", userProfile.AddressLine1 },
                                          { "AddressLine2", userProfile.AddressLine2 },
                                          { "City", userProfile.City },
                                          { "State", userProfile.State },
                                          { "ZipCode", userProfile.ZipCode },
                                          { "Consent", userProfile.Consent.ToString() }
                                      };
            return form;
        }
    }
}
