//
// Copyright © 2011 Jordan Rift, LLC - All Rights Reserved
//
// THIS WORK IS LICENSED UNDER A CREATIVE COMMONS ATTRIBUTION-NONCOMMERCIAL-
// SHAREALIKE 3.0 UNPORTED LICENSE:
// http://creativecommons.org/licenses/by-nc-sa/3.0/
//

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
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
        /// <param name="value">Display text to render to the button</param>
        /// <returns>Html representation of a cancel button</returns>
        public static MvcHtmlString UiCancel(this HtmlHelper helper, string value = "Cancel")
        {
            return new MvcHtmlString(string.Format("<button id=\"cancel\" class=\"cancel ui-button ui-widget ui-state-default ui-corner-all ui-button-text-only\">{0}</button>", value));
        }

        //public static MvcHtmlString UiValidationFor<TModel, TValue>(this HtmlHelper<TModel> helper, Expression<Func<TModel, TValue>> expression)
        //{
        //    var metadata = ModelMetadata.FromLambdaExpression(expression, helper.ViewData);
        //    var htmlFieldName = ExpressionHelper.GetExpressionText(expression);
        //    var errorText = metadata.DisplayName ?? metadata.PropertyName ?? htmlFieldName.Split('.').Last();

        //    if (string.IsNullOrEmpty(errorText))
        //    {
        //        return MvcHtmlString.Empty;
        //    }

        //    var html = new StringBuilder();
        //    html.AppendLine("<div class=\"ui-widget field-validation-valid\">");
        //    html.AppendLine("<div class=\"ui-state-error ui-corner-all\" style=\"padding: 0 .7em;\">");
        //    html.AppendLine("<span class=\"ui-icon ui-icon-alert\" style=\"float: left; margin-right: .3em;\"></span>");
        //    html.AppendLine("<span data-valmsg-replace=\"true\" data-valmsg-for=\"" + metadata.PropertyName + "\">" + errorText + "</span>");
        //    html.AppendLine("</div>");
        //    html.AppendLine("</div>");
        //    return new MvcHtmlString(html.ToString());
        //}
    }
}