//
// Copyright © 2011 Jordan Rift, LLC - All Rights Reserved
//
// THIS WORK IS LICENSED UNDER A CREATIVE COMMONS ATTRIBUTION-NONCOMMERCIAL-
// SHAREALIKE 3.0 UNPORTED LICENSE:
// http://creativecommons.org/licenses/by-nc-sa/3.0/
//

using System.ComponentModel.DataAnnotations;
using JordanRift.Grassroots.Framework.Entities.Validation;
using NUnit.Framework;

namespace JordanRift.Grassroots.Tests.UnitTests.Validation
{
    [TestFixture]
    public class NotEqualToTests
    {
        private class Strings
        {
            public string FirstString { get; set; }

            [NotEqualTo("FirstString")]
            public string SecondString { get; set; }
        }

        [Test]
        public void IsValid_Should_Return_Null_When_Values_Are_Valid()
        {
            var strings = new Strings { FirstString = "one", SecondString = "two" };
            var attribute = new NotEqualToAttribute("FirstString");
            var context = new ValidationContext(strings, null, null);
            var result = attribute.GetValidationResult(strings.SecondString, context);
            Assert.IsNull(result);
        }

        [Test]
        public void IsValid_Should_Return_ValidationResult_When_Values_Are_Invalid()
        {
            var strings = new Strings { FirstString = "one", SecondString = "one" };
            var attribute = new NotEqualToAttribute("FirstString");
            var context = new ValidationContext(strings, null, null);
            var result = attribute.GetValidationResult(strings.SecondString, context);
            Assert.IsNotNull(result);
        }
    }
}
