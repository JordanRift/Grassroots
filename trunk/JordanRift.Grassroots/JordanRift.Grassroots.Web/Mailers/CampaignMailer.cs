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
    public sealed class CampaignMailer : MailerBase, ICampaignMailer     
	{
		public CampaignMailer()
		{
            MasterName = "_EmailLayout";
		}

		public MailMessage CampaignEmailBlast(CampaignEmailBlastModel model)
		{
            var mailMessage = new MailMessage
                                  {
                                      Subject = string.Format("{0} {1} would like you to take a gander at '{2}'", 
                                          model.FirstName, model.LastName, model.Title),
                                      From = new MailAddress(model.Email, string.Format("{0} {1}", 
                                          model.FirstName, model.LastName))
                                  };

		    var emails = model.EmailAddresses.Split(new[] { ',', ';' });

            foreach (var email in emails)
            {
                mailMessage.To.Add(email.Trim());
            }
            
            ViewData = new ViewDataDictionary(model);
            PopulateBody(mailMessage, viewName: "CampaignEmailBlast");
		    return mailMessage;
		}
	}
}