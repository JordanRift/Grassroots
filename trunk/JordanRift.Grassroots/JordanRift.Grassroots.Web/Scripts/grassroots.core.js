﻿/// <reference path="jquery-1.5.2-vsdoc.js" />

//
// Copyright © 2011 Jordan Rift, LLC - All Rights Reserved
//
// THIS WORK IS LICENSED UNDER A CREATIVE COMMONS ATTRIBUTION-NONCOMMERCIAL-
// SHAREALIKE 3.0 UNPORTED LICENSE:
// http://creativecommons.org/licenses/by-nc-sa/3.0/
//

Grassroots = {};

Grassroots.progressbar = function () {
    var $progressbar = $(".ui-progressbar-value");

    if ($progressbar.length > 0) {
        var px = $progressbar.outerWidth();
        $progressbar.css("width", "0px").show().animate({ width: '+=' + px + 'px' }, 2000);
        $(".progress-indicator").animate({ left: '+=' + px + 'px', opacity: 1 }, 2000);
    }
}

Grassroots.formui = function () {
    $(".datePicker").datepicker();
    $("fieldset").addClass("ui-widget").addClass("ui-corner-all");
    $("button, input:submit, input:button}").button();
    $(":input[type='text'], textarea, input[type='password'], input[type='email']").wijtextbox();
    $("select").wijdropdown();
    $(":input[type='radio']").wijradio();
    $(":input[type='checkbox']").wijcheckbox();
}

Grassroots.initPayment = function () {
    $(".bank-info").hide();
    $("#Amount").val('');

    $(".payment-type").change(function () {
        var val = $(this).val();

        if (val == "CC") {
            $(".bank-info").slideUp(function () {
                $(".cc-info").slideDown();
            });
        }
        else {
            $(".cc-info").slideUp(function () {
                $(".bank-info").slideDown();
            });
        }

        return false;
    });
}

Grassroots.initEmailForm = function () {
    $("#email-blast").button()
			.click(function () {
			    $("#campaign-email-form").dialog("open");
			    $(".ui-widget-overlay").show();
			    return false;
			});

    $("#campaign-email-form").dialog({
        autoOpen: false,
        height: 460,
        width: 520,
        modal: true,
        title: "Email your friends",
        closeText: "",
        buttons: {
            "Send email": function () {
                var $form = $("#campaign-email-form > form");

                if ($form.valid()) {
                    // trigger AJAX post to server to send email

                }

                return false;
            },
            Cancel: function () {
                $(this).dialog("close");
            }
        },
        close: function () {
            $(".ui-widget-overlay").hide();
            $("#campaign-email-form input, #campaign-email-form textarea").val("");

            // Reset jquery validation
            $(".ui-state-error")
                    .removeClass("field-validation-error")
                    .addClass("field-validation-valid");
        }
    });
}

/// <summary>
/// Since the official Tweet button doesn't support SSL, we'll roll our own for now.
/// </summary>
Grassroots.tweet = function (str) {
    mywindow = window.open('http://twitter.com/share?url=' + str, "Tweet_widow", "channelmode=no,directories=no,location=no,menubar=no,scrollbars=no,toolbar=no,status=no,width=500,height=375,left=300,top=200");
    mywindow.focus();
}