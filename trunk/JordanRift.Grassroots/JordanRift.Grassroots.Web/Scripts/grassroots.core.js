﻿/// <reference path="jquery-1.5.2-vsdoc.js" />

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

var Grassroots = (function() {
    function _causeSearch($el) {
        var id = $el.parents(".form").attr("data-cause-template-id");
        var referenceNumber = $(".reference-number").val();
                
        if (referenceNumber !== "") {
            window.location = "/Projects/Search/" + id + "/" + escape(referenceNumber);
        }
    }

    function _initPlaceholders() {
        $("input[type='text'], input[type='email']").each(function() {
            if ($(this).attr("placeholder") != "") {
                $(this).val($(this).attr("placeholder"));
            }
        });

        $("input[type='text'], input[type='email']").blur(function() {
            if ($(this).attr("placeholder") != "" && ($(this).val() == $(this).attr("placeholder") || $(this).val() == "")) {
                $(this).val($(this).attr("placeholder"));
            }
        });

        $("input[type='text'], input[type='email']").focus(function() {
            if ($(this).val() == $(this).attr("placeholder")) {
                $(this).val("");
            }
        });
    }

    return {
        progressbar: function () {
            var $progressbar = $(".ui-progressbar-value");

            if ($progressbar.length > 0) {
                var px = $progressbar.outerWidth();
                $progressbar.css("width", "0px").show().animate({ width: '+=' + px + 'px' }, 2000);
                $(".progress-indicator").animate({ left: '+=' + px + 'px', opacity: 1 }, 2000);
            }
        },
        formui: function () {
            $("#main .datePicker").datepicker();
            $("#main fieldset").addClass("ui-widget").addClass("ui-corner-all");
            $("#main button, #main input:submit, #main input:button}").button();
            $("#main :input[type='text'], #main textarea, #main input[type='password'], #main input[type='email']").wijtextbox();
            $("#campaign-email-form :input[type='text'], #campaign-email-form textarea, #campaign-email-form input[type='password'], #campaign-email-form input[type='email']").wijtextbox();
            $("#main select").wijdropdown();
            $("#main :input[type='radio']").wijradio();
            $("#main :input[type='checkbox']").wijcheckbox();

            if (!Modernizr.input.placeholder) {
                // If HTML5 placeholders aren't supported, use JS placeholders.
                _initPlaceholders();
            }
        },
        gridui: function (cols) {
            $(".grid").wijgrid({
                allowSorting: true,
                allowColSizing: true,
                culture: "en",
                allowPaging: true,
                ensureColumnsPxWidth: true,
                columns: cols,
            });
        },
        initPayment: function () {
            $(".bank-info").hide();
            
            if ($("#Amount").val() == "0") {
                $("#Amount").val('');
            }

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
        },
        initEmailForm: function () {
            $("#email-blast").click(function () {
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
                            var jsonData = '{ "Title":"' + $("#Title").val() + '", ';
                            jsonData += '"UrlSlug":"' + $("#UrlSlug").val() + '", ';
                            jsonData += '"Email":"' + $("#Email").val() + '", ';
                            jsonData += '"FirstName":"' + $("#FirstName").val() + '", ';
                            jsonData += '"LastName":"' + $("#LastName").val() + '", ';
                            jsonData += '"EmailAddresses":"' + $("#EmailAddresses").val() + '", ';
                            jsonData += '"CustomMessage":"' + $("#CustomMessage").val() + '" }';

                            $.ajax({
                                url: "/Campaign/SendEmail",
                                type: "POST",
                                contentType: "application/json",
                                dataType: "json",
                                data: jsonData,
                            })
                            .success(function(result) {
                                $(".notification").empty();
                                $("<p>Email sent successfully!</p>").appendTo(".notification");
                                $(".notification").fadeIn("normal", "swing", function() {
                                    setTimeout('$(".notification").fadeOut();', 3000);
                                });
                            })
                            .error(function (xhr, status, error) {
                                $(".notification").empty();
                                $("<p>There was a problem sending your email. Please try again.</p>").appendTo(".notification");
                                $(".notification").fadeIn("normal", "swing", function() {
                                    setTimeout('$(".notification").fadeOut();', 3000);
                                });
                            });

                            $(this).dialog("close");
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
        },
        tweet: function (str) {
            mywindow = window.open('http://twitter.com/share?url=' + str, "Tweet_widow", "channelmode=no,directories=no,location=no,menubar=no,scrollbars=no,toolbar=no,status=no,width=500,height=375,left=300,top=200");
            mywindow.focus();
        },
        getTweetCount: function(url) {
            var request = "http://urls.api.twitter.com/1/urls/count.json?url=" + escape(url) + "&callback=?";

            $.getJSON(request, function(data) {
                $(".count > a").text(data.count);
            });

            return false;
        },
        getStarted: function() {
            $(".cause-template").click(function() {
                $(".projects li").removeClass("selected");
                $(this).parent("li").addClass("selected");
                $("#CauseTemplateID").val($(this).attr("data-cause-template-id"));
                return false;
            });

            $(".campaign-type").click(function() {
                $(".campaign-types li").removeClass("selected");
                $(this).parent("li").addClass("selected");
                $("#CampaignType").val($(this).attr("data-campaign-type"));
                return false;
            });
        },
        initCauseSearch: function() {
            $(".project-details .search input:submit").click(function() {
                _causeSearch($(this));
                return false;
            });

            $(".reference-number").keypress(function(event) {
                if (event.keyCode == 13) {
                    _causeSearch($(this));
                }

                return true;
            });
        },
        initAccountActivation: function() {
            $(".show-activation-form").click(function() {
                $(".resend-activation").slideDown();
                return false;
            });
        }
    };
})();
