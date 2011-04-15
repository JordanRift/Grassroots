//
// Copyright © 2011 Jordan Rift, LLC - All Rights Reserved
//
// THIS WORK IS LICENSED UNDER A CREATIVE COMMONS ATTRIBUTION-NONCOMMERCIAL-
// SHAREALIKE 3.0 UNPORTED LICENSE:
// http://creativecommons.org/licenses/by-nc-sa/3.0/
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
