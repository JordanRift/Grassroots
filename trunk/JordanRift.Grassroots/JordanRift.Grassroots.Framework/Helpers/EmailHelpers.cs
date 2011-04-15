//
// Copyright © 2011 Jordan Rift, LLC - All Rights Reserved
//
// THIS WORK IS LICENSED UNDER A CREATIVE COMMONS ATTRIBUTION-NONCOMMERCIAL-
// SHAREALIKE 3.0 UNPORTED LICENSE:
// http://creativecommons.org/licenses/by-nc-sa/3.0/
//

using System.Text;

namespace JordanRift.Grassroots.Framework.Helpers
{
    public class EmailHelpers
    {
        private const string DOMAIN = "http://www.onemission.us/";


        // TODO: Build in support for Donation confirmation email in org settings
        public static string GetDonationMessage()
        {
            StringBuilder email = new StringBuilder();
            email.Append("<p>Thank you for your donation to One Mission!</p>");
            email.Append("<p>100% of your gift will go directly to serve &amp; develop communities in need.</p>");
            email.Append("<p>A tax receipt for your donation will be sent to this e-mail address in 3-4 weeks.</p>");
            email.Append("<p>Thank you.</p>");
            email.Append("<p>-The team at One Mission<br />");
            email.Append("<a href=\"http://www.onemission.us\">www.onemission.us</a></p>");
            return email.ToString();
        }
    }
}
