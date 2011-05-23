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

using System.Linq;
using JordanRift.Grassroots.Framework.Services;
using NUnit.Framework;

namespace JordanRift.Grassroots.Tests.IntegrationTests.Services
{
    [TestFixture]
    class TwitterServiceTests
    {
        [Test]
        public void GetTweets_Sould_Load_Statuses_From_Twitter()
        {
            var service = new TwitterService();
            var tweets = service.GetTweets("grassrootsproj");
            Assert.IsNotEmpty(tweets.ToList());
        }
    }
}
