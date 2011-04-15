/// <reference path="jquery-1.4.1-vsdoc.js" />

//
// Copyright © 2011 Jordan Rift, LLC - All Rights Reserved
//
// THIS WORK IS LICENSED UNDER A CREATIVE COMMONS ATTRIBUTION-NONCOMMERCIAL-
// SHAREALIKE 3.0 UNPORTED LICENSE:
// http://creativecommons.org/licenses/by-nc-sa/3.0/
//

$(function ()
{
    $(".payment-type").change(function ()
    {
        var val = $(this).val();

        if (val == "CC")
        {
            $(".bank-info").slideUp(function ()
            {
                $(".cc-info").slideDown();
            });
        }
        else
        {
            $(".cc-info").slideUp(function ()
            {
                $(".bank-info").slideDown();
            });
        }

        return false;
    });
});