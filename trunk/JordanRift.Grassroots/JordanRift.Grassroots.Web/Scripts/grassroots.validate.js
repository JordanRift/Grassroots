/// <reference path="jquery-1.5.2-vsdoc.js" />
/// <reference path="jquery.validate-vsdoc.js" />
/// <reference path="jquery.validate.unobtrusive.js" />

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

/// <summary>
/// Client implementations of validation for custom data annotation attributes
/// </summary>
(function ($) {
    /// <summary>
    /// Adding custom client validation logic for [GreaterThanDate] attribute on the server.
    /// </summary>
    $.validator.addMethod("greater", function (value, element, params) {
        var startDate = Date.parse($("#" + params).val());
        var endDate = Date.parse(value);
        return endDate > startDate;
    });

    /// <summary>
    /// Adding custom client validation logic for [GreaterThanDate] attribute on the server.
    /// </summary>
    $.validator.unobtrusive.adapters.addSingleVal("greater", "otherproperty");

    /// <summary>
    /// Adding custom client validation logic for [NotEqualTo] attribute on the server.
    /// </summary>
    $.validator.addMethod("notequalto", function (value, element, params) {
        if (!this.optional(element)) {
            var otherProp = $('#' + params)
            return (otherProp.val() != value);
        }

        return true;
    });

    /// <summary>
    /// Adding custom client validation logic for [NotEqualTo] attribute on the server.
    /// </summary>
    $.validator.unobtrusive.adapters.addSingleVal("notequalto", "otherproperty");
} (jQuery));