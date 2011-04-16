//
// Copyright © 2011 Jordan Rift, LLC - All Rights Reserved
//
// THIS WORK IS LICENSED UNDER A CREATIVE COMMONS ATTRIBUTION-NONCOMMERCIAL-
// SHAREALIKE 3.0 UNPORTED LICENSE:
// http://creativecommons.org/licenses/by-nc-sa/3.0/
//

using System.Web.Mvc;
using JordanRift.Grassroots.Framework.Entities;
using JordanRift.Grassroots.Web.Models;
using Mvc.Mailer;
using System.Net.Mail;

namespace JordanRift.Grassroots.Web.Mailers
{ 
    public class DonateMailer : MailerBase, IDonateMailer     
	{
		public DonateMailer()
		{
			MasterName="_Layout";
		}

		
		public virtual MailMessage CampaignDonation(CampaignNotificationModel model)
		{
            var mailMessage = new MailMessage { Subject = string.Format("A donation was received to {0} campaign!", model.Title) };
			mailMessage.To.Add(model.Email);
            ViewData = new ViewDataDictionary(model);
			PopulateBody(mailMessage, viewName: "CampaignDonation");
			return mailMessage;
		}
		
		public virtual MailMessage UserDonation(Payment model)
		{
		    var mailMessage = new MailMessage { Subject = "Thank you for your generosity!" };
			mailMessage.To.Add(model.Email);
            ViewData = new ViewDataDictionary(model);
			PopulateBody(mailMessage, viewName: "UserDonation");
			return mailMessage;
		}

		public virtual MailMessage CampaignEmailBlast(CampaignEmailBlastModel model)
		{
            var mailMessage = new MailMessage
                                  {
                                      Subject = string.Format("{0} {1} would like you to take a gander at '{2}'", 
                                          model.FirstName, model.LastName, model.Title),
                                      From = new MailAddress(model.Email, string.Format("{0} {1}", 
                                          model.FirstName, model.LastName))
                                  };

		    var emails = model.EmailAddresses.Split(new[] { ',', ';', ' ' });

            foreach (var email in emails)
            {
                mailMessage.To.Add(email);
            }
            
            ViewData = new ViewDataDictionary(model);
            PopulateBody(mailMessage, viewName: "CampaignEmailBlast");
		    return mailMessage;
		}
	}
}