//
// Copyright © 2011 Jordan Rift, LLC - All Rights Reserved
//
// THIS WORK IS LICENSED UNDER A CREATIVE COMMONS ATTRIBUTION-NONCOMMERCIAL-
// SHAREALIKE 3.0 UNPORTED LICENSE:
// http://creativecommons.org/licenses/by-nc-sa/3.0/
//

using System;
using System.Linq.Expressions;
using System.Web.Mvc;

namespace JordanRift.Grassroots.Web.Helpers
{
    public static class HtmlHelperExtensions
    {
        /// <summary>
        /// Html Helper method to render out a input submit button with jQuery UI classes appended to it.
        /// </summary>
        /// <param name="helper">HtmlHelper class to add extension to</param>
        /// <param name="text">Display text to render to the button</param>
        /// <returns>Html representation of a submit button</returns>
        public static MvcHtmlString UiSubmit(this HtmlHelper helper, string value)
        {
            return new MvcHtmlString(string.Format("<input type=\"submit\" value=\"{0}\" class=\"ui-button ui-widget ui-state-active ui-corner-all ui-button-text-only\" />", value));
        }

        /// <summary>
        /// Html Helper method to render out a cancel button with jQuery UI classes
        /// </summary>
        /// <param name="helper">HtmlHelper class to add extension to</param>
        /// <param name="text">Display text to render to the button</param>
        /// <returns>Html representation of a cancel button</returns>
        public static MvcHtmlString UiCancel(this HtmlHelper helper, string value = "Cancel")
        {
            return new MvcHtmlString(string.Format("<button id=\"cancel\" class=\"ui-button ui-widget ui-state-default ui-corner-all ui-button-text-only\">{0}</button>", value));
        }

        //public static MvcHtmlString UiValidationFor<TModel, TProperty>(this HtmlHelper<TModel> helper, Expression<Func<TModel, TProperty>> expression)
        //{
        //    var result = expression.Compile();
        //    result.Invoke();
        //}
    }
}