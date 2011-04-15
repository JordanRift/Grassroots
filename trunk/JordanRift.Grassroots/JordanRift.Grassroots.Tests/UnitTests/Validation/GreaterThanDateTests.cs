//
// Copyright © 2011 Jordan Rift, LLC - All Rights Reserved
//
// THIS WORK IS LICENSED UNDER A CREATIVE COMMONS ATTRIBUTION-NONCOMMERCIAL-
// SHAREALIKE 3.0 UNPORTED LICENSE:
// http://creativecommons.org/licenses/by-nc-sa/3.0/
//

using System;
using System.ComponentModel.DataAnnotations;
using JordanRift.Grassroots.Framework.Entities.Validation;
using NUnit.Framework;

namespace JordanRift.Grassroots.Tests.UnitTests.Validation
{
    [TestFixture]
    public class GreaterThanDateTests
    {
        private class Dates
        {
            public DateTime FirstDate { get; set; }
            
            [GreaterThanDate("FirstDate")]
            public DateTime SecondDate { get; set; }
        }

        [Test]
        public void IsValid_Should_Return_Null_When_Values_Are_Valid()
        {
            var dates = new Dates { FirstDate = DateTime.Now, SecondDate = DateTime.Now.AddDays(1) };
            var attribute = new GreaterThanDateAttribute("FirstDate");
            var context = new ValidationContext(dates, null, null);
            var result = attribute.GetValidationResult(dates.SecondDate, context);
            Assert.IsNull(result);
        }

        [Test]
        public void IsValid_Should_Return_ValidationResult_When_Values_Are_Invalid()
        {
            var dates = new Dates { FirstDate = DateTime.Now, SecondDate = DateTime.Now.AddDays(-1) };
            var attribute = new GreaterThanDateAttribute("FirstDate");
            var context = new ValidationContext(dates, null, null);
            var result = attribute.GetValidationResult(dates.SecondDate, context);
            Assert.IsNotNull(result);
        }
    }
}
