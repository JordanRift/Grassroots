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
using JordanRift.Grassroots.Web.Controllers;
using NUnit.Framework;

namespace JordanRift.Grassroots.Tests.UnitTests.Controllers
{
    [TestFixture]
    public class ValidationControllerTests
    {
        private ValidationController controller;

        [SetUp]
        public void SetUp()
        {
            controller = new ValidationController();
        }

        [Test]
        public void CheckEmail_Should_Return_True_If_Email_Is_Unique()
        {
            var result = controller.CheckEmail("unique-email@yahoo.com");
            var jsonResult  = Convert.ToBoolean(result.Data);
            Assert.IsTrue(jsonResult);
        }

        [Test]
        public void CheckEmail_Should_Return_False_If_Email_Is_Not_Unique()
        {
            var result = controller.CheckEmail("jon.appleseed@yahoo.com");
            var jsonResult = Convert.ToBoolean(result.Data);
            Assert.IsFalse(jsonResult);
        }

        [Test]
        public void CheckUrlSlug_Should_Return_True_If_UrlSlug_Is_Unique()
        {
            var result = controller.CheckUrlSlug("uniqueslug");
            var jsonResult = Convert.ToBoolean(result.Data);
            Assert.IsTrue(jsonResult);
        }

        [Test]
        public void CheckUrlSlug_Should_Return_False_If_UrlSlug_Is_Not_Unique()
        {
            var result = controller.CheckUrlSlug("non-unique-slug");
            var jsonResult = Convert.ToBoolean(result.Data);
            Assert.IsFalse(jsonResult);
        }
    }
}
