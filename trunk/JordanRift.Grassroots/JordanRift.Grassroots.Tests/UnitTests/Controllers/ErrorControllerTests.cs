﻿//
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

using System.Web.Mvc;
using JordanRift.Grassroots.Web.Controllers;
using NUnit.Framework;

namespace JordanRift.Grassroots.Tests.UnitTests.Controllers
{
    [TestFixture]
    public class ErrorControllerTests
    {
        private ErrorController controller;

        [SetUp]
        public void SetUp()
        {
            controller = new ErrorController();
        }

        [Test]
        public void Index_Should_Return_View()
        {
            var result = controller.Index();
            Assert.IsInstanceOf(typeof(ViewResult), result);
        }

        [Test]
        public void NotFound_Should_Return_View()
        {
            var result = controller.NotFound();
            Assert.IsInstanceOf(typeof(ViewResult), result);
        }

        [Test]
        public void Forbidden_Should_Return_View()
        {
            var result = controller.Forbidden();
            Assert.IsInstanceOf(typeof(ViewResult), result);
        }
    }
}
