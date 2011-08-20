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
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using JordanRift.Grassroots.Framework.Entities.Models;
using Rhino.Mocks;

namespace JordanRift.Grassroots.Tests.Helpers
{
    public static class TestHelpers
    {
        public static Random random = new Random();

        public static decimal GetAmount()
        {
            return (decimal) random.NextDouble() * 10;
        }

        public static FormCollection GetUserProfileFormPost(UserProfile userProfile)
        {
            FormCollection form = new FormCollection
                                      {
                                          { "FirstName", userProfile.FirstName },
                                          { "LastName", userProfile.LastName },
                                          { "Birthdate", userProfile.Birthdate.ToString() },
                                          { "Gender", userProfile.Gender },
                                          { "Email", userProfile.Email },
                                          { "PrimaryPhone", userProfile.PrimaryPhone },
                                          { "AddressLine1", userProfile.AddressLine1 },
                                          { "AddressLine2", userProfile.AddressLine2 },
                                          { "City", userProfile.City },
                                          { "State", userProfile.State },
                                          { "ZipCode", userProfile.ZipCode },
                                          { "Consent", userProfile.Consent.ToString() }
                                      };
            return form;
        }

        public static void MockHttpContext(Controller controller, MockRepository mocks, bool isAuthenticated = true, bool postFiles = false)
        {
            // Mocking http request so MVC UrlHelper class will function normally under test
            // http://blog.muonlab.com/2010/02/22/how-to-use-mvcs-urlhelper-in-your-tests-with-rhinomocks/
            var routes = new RouteCollection();
            MvcApplication.RegisterRoutes(routes);

            var mockRequest = mocks.Stub<HttpRequestBase>();
            mockRequest.Stub(x => x.IsSecureConnection).Return(false);
            mockRequest.Stub(x => x.IsLocal).Return(false);
            mockRequest.Stub(x => x.ApplicationPath).Return("/");
            mockRequest.Stub(x => x.Url).Return(new Uri("http://localhost/a", UriKind.Absolute));
            mockRequest.Stub(x => x.ServerVariables).Return(new NameValueCollection());

            if (postFiles)
            {
                var postedFile = mocks.Stub<HttpPostedFileBase>();
                var postedFilesKeyCollection = mocks.Stub<HttpFileCollectionBase>();
                var fakeFileKeys = new List<string> { "file" };
                mockRequest.Stub(req => req.Files).Return(postedFilesKeyCollection);
                postedFilesKeyCollection.Stub(keys => keys.GetEnumerator()).Return(fakeFileKeys.GetEnumerator());
                postedFilesKeyCollection.Stub(keys => keys["file"]).Return(postedFile);
                postedFile.Stub(file => file.ContentLength).Return(8192);
                postedFile.Stub(file => file.FileName).Return("foo.jpg");
            }

            var mockResponse = mocks.Stub<HttpResponseBase>();
            mockResponse.Stub(x => x.ApplyAppPathModifier(Arg<string>.Is.Anything)).Return(null).WhenCalled(x => x.ReturnValue = x.Arguments[0]);

            var mockContext = mocks.Stub<HttpContextBase>();
            mockContext.Stub(x => x.Request).Return(mockRequest);
            mockContext.Stub(x => x.Response).Return(mockResponse);

            if (isAuthenticated)
            {
                mockContext.User = new GenericPrincipal(new GenericIdentity("goodEmail"), null /* roles */);
            }

            controller.ControllerContext = new ControllerContext(mockContext, new RouteData(), controller);
            controller.Url = new UrlHelper(new RequestContext(mockContext, new RouteData()), routes);
            mocks.ReplayAll();
        }
    }
}
