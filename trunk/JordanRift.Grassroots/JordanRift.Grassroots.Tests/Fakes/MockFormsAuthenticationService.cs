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
using JordanRift.Grassroots.Web.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace JordanRift.Grassroots.Tests.Fakes
{
    [Obsolete("This class will be obsolete in future versions in favor of using Rhino Mocks. See DonateControllerTests for example of new pattern.")]
    public class MockFormsAuthenticationService : IFormsAuthenticationService
    {
        public bool SignIn_WasCalled;
        public bool SignOut_WasCalled;

        public void SignIn(string userName, bool createPersistentCookie)
        {
            // verify that the arguments are what we expected
            Assert.AreEqual("goodEmail", userName);
            Assert.IsFalse(createPersistentCookie);

            SignIn_WasCalled = true;
        }

        public void SignOut()
        {
            SignOut_WasCalled = true;
        }
    }
}
