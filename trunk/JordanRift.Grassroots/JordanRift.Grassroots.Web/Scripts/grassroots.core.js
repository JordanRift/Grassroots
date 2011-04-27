﻿/// <reference path="jquery-1.4.4-vsdoc.js" />

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

//GrassrootsEvents = [];

//Configure RequireJS
//require({ priority: ['jquery-1.4.4.min'] },
//    ['jquery-1.4.4.min', 'modernizr-1.7.min', 'grassroots.ui'],
//    function () {
//        for (var i = 0; i < GrassrootsEvents.length; i++) {
//            eval(GrassrootsEvents[i]);
//        }
//    }
//);

/// <summary>
/// Since the official Tweet button doesn't support SSL, we'll roll our own for now.
/// </summary>
function twitterPop(str) {
    mywindow = window.open('http://twitter.com/share?url=' + str, "Tweet_widow", "channelmode=no,directories=no,location=no,menubar=no,scrollbars=no,toolbar=no,status=no,width=500,height=375,left=300,top=200");
    mywindow.focus();
}