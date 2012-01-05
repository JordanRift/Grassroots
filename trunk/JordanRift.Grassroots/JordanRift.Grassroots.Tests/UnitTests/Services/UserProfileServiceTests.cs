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

using JordanRift.Grassroots.Framework.Data;
using JordanRift.Grassroots.Framework.Services;
using JordanRift.Grassroots.Tests.Fakes;
using JordanRift.Grassroots.Tests.Helpers;
using NUnit.Framework;

namespace JordanRift.Grassroots.Tests.UnitTests.Services
{
    [TestFixture]
    public class UserProfileServiceTests
    {
        private IUserProfileRepository userProfileRepository;
        private UserProfileService service;
        private const string FACEBOOK_ID = "1234567890";

        [SetUp]
        public void SetUp()
        {
            userProfileRepository = new FakeUserProfileRepository();
            service = new UserProfileService(userProfileRepository);
        }

        [TearDown]
        public void TearDown()
        {
            FakeUserProfileRepository.Reset();
        }

        [Test]
        public void IsFacebookAccountUnique_Should_Return_True_If_FacebookID_Not_Found()
        {
            var result = service.IsFacebookAccountUnique(FACEBOOK_ID);
            Assert.IsTrue(result);
        }

        [Test]
        public void IsFacebookAccountUnique_Should_Return_False_If_FacebookID_Found()
        {
            var userProfile = EntityHelpers.GetValidUserProfile();
            userProfile.FacebookID = FACEBOOK_ID;
            userProfileRepository.Add(userProfile);
            var result = service.IsFacebookAccountUnique(FACEBOOK_ID);
            Assert.IsFalse(result);
        }
    }
}
