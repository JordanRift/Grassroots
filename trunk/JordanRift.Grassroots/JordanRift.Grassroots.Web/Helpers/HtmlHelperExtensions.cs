﻿//
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