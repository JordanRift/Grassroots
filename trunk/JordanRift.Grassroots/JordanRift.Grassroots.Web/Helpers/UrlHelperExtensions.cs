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
using System.Web.Mvc;

namespace JordanRift.Grassroots.Web.Helpers
{
    public static class UrlHelperExtensions
    {
        public static string ToPublicUrl(this UrlHelper helper, string relativeUri)
        {
            var uri = new Uri(relativeUri, UriKind.Relative);
            return helper.ToPublicUrl(uri);
        }

        public static string ToPublicUrl(this UrlHelper helper, Uri relativeUri)
        {
            var httpContext = helper.RequestContext.HttpContext;

            if (httpContext.Request.Url == null)
            {
                return null;
            }

            var uri = new UriBuilder
                          {
                              Host = httpContext.Request.Url.Host,
                              Path = "/",
                              Port = httpContext.Request.IsSecureConnection ? 443 : 80,
                              Scheme = httpContext.Request.Url.Scheme
                          };

            if (httpContext.Request.IsLocal)
            {
                uri.Port = httpContext.Request.Url.Port;
            }

            return new Uri(uri.Uri, relativeUri).AbsoluteUri;
        }
    }
}