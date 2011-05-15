//
// Copyright © 2011 Jordan Rift, LLC - All Rights Reserved
//
// THIS WORK IS LICENSED UNDER A CREATIVE COMMONS ATTRIBUTION-NONCOMMERCIAL-
// SHAREALIKE 3.0 UNPORTED LICENSE:
// http://creativecommons.org/licenses/by-nc-sa/3.0/
//

using System.Web.Mvc;
using JordanRift.Grassroots.Web.Models;
using Mvc.Mailer;
using System.Net.Mail;

namespace JordanRift.Grassroots.Web.Mailers
{ 
    public sealed class DonateMailer : MailerBase, IDonateMailer     
	{
		public DonateMailer()
		{
            MasterName = "_EmailLayout";
		}

		
		public MailMessage CampaignDonation(DonationDetailsModel model)
		{
            var mailMessage = new MailMessage { Subject = string.Format("A donation was received to {0} campaign!", model.Title) };
			mailMessage.To.Add(model.Email);
            ViewData = new ViewDataDictionary(model);
			PopulateBody(mailMessage, viewName: "CampaignDonation");
			return mailMessage;
		}
		
		public MailMessage UserDonation(DonationDetailsModel model)
		{
		    var mailMessage = new MailMessage { Subject = "Thank you for your generosity!" };
			mailMessage.To.Add(model.Email);
            ViewData = new ViewDataDictionary(model);
			PopulateBody(mailMessage, viewName: "UserDonation");
			return mailMessage;
		}
	}
}