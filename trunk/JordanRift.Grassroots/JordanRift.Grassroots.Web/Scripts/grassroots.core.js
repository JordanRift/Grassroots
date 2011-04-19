/// <reference path="jquery-1.4.4-vsdoc.js" />

//
// Copyright © 2011 Jordan Rift, LLC - All Rights Reserved
//
// THIS WORK IS LICENSED UNDER A CREATIVE COMMONS ATTRIBUTION-NONCOMMERCIAL-
// SHAREALIKE 3.0 UNPORTED LICENSE:
// http://creativecommons.org/licenses/by-nc-sa/3.0/
//

/// <summary>
/// Since the official Tweet button doesn't support SSL, we'll roll our own for now.
/// </summary>
function twitterPop(str) {
    mywindow = window.open('http://twitter.com/share?url=' + str, "Tweet_widow", "channelmode=no,directories=no,location=no,menubar=no,scrollbars=no,toolbar=no,status=no,width=500,height=375,left=300,top=200");
    mywindow.focus();
}