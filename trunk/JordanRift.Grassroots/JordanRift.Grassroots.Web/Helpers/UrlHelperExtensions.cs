//
// Copyright © 2011 Jordan Rift, LLC - All Rights Reserved
//
// THIS WORK IS LICENSED UNDER A CREATIVE COMMONS ATTRIBUTION-NONCOMMERCIAL-
// SHAREALIKE 3.0 UNPORTED LICENSE:
// http://creativecommons.org/licenses/by-nc-sa/3.0/
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