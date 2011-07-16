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

using System.Web.Mvc;
using JordanRift.Grassroots.Framework.Services;
using JordanRift.Grassroots.Web.Controllers;
using NUnit.Framework;
using Rhino.Mocks;

namespace JordanRift.Grassroots.Tests.UnitTests.Controllers
{
    [TestFixture]
    public class HomeControllerTests
    {
        private HomeController controller;
        private ITwitterService twitterService;
        private IBlogService blogService;
        private MockRepository mocks;

        [SetUp]
        public void SetUp()
        {
            mocks = new MockRepository();
            SetUpController();
        }

        [Test]
        public  void Index_Should_Return_View()
        {
            var result = controller.Index();
            Assert.IsInstanceOf(typeof(ViewResult), result);
        }

        [Test]
        public void About_Should_Return_View()
        {
            var result = controller.About();
            Assert.IsInstanceOf(typeof(ViewResult), result);
        }

        [Test]
        public void Terms_Should_Return_View()
        {
            var result = controller.Terms();
            Assert.IsInstanceOf(typeof(ViewResult), result);
        }

        [Test]
        public void ProgressBar_Should_Return_View()
        {
            var result = controller.ProgressBar();
            Assert.IsInstanceOf(typeof(ViewResult), result);
        }

        [Test]
        public void Stats_Should_Return_View()
        {
            var result = controller.Stats();
            Assert.IsInstanceOf(typeof(ViewResult), result);
        }

        [Test]
        public void TwitterFeed_Should_Return_View()
        {
            var result = controller.TwitterFeed();
            Assert.IsInstanceOf(typeof(ViewResult), result);
        }

        [Test]
        public void BlogRssFeed_Should_Return_View()
        {
            var result = controller.BlogRssFeed();
            Assert.IsInstanceOf(typeof(ViewResult), result);
        }

        [Test]
        public void ThemeCss_Should_Return_View()
        {
            var result = controller.ThemeCss();
            Assert.IsInstanceOf(typeof(ViewResult), result);
        }

        private void SetUpController()
        {
            twitterService = mocks.DynamicMock<ITwitterService>();
            blogService = mocks.DynamicMock<IBlogService>();
            controller = new HomeController(twitterService, blogService);
        }
    }
}
