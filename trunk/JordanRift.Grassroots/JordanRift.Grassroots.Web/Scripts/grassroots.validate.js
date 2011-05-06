/// <reference path="jquery-1.5.2-vsdoc.js" />
/// <reference path="jquery.validate-vsdoc.js" />
/// <reference path="jquery.validate.unobtrusive.js" />

//
// Copyright © 2011 Jordan Rift, LLC - All Rights Reserved
//
// THIS WORK IS LICENSED UNDER A CREATIVE COMMONS ATTRIBUTION-NONCOMMERCIAL-
// SHAREALIKE 3.0 UNPORTED LICENSE:
// http://creativecommons.org/licenses/by-nc-sa/3.0/
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