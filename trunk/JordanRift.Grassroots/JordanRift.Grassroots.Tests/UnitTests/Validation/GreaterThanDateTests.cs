//
// Grassroots is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// Grassroots is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with Grassroots.  If not, see <http://www.gnu.org/licenses/>.
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
