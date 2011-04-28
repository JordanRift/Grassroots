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
    public sealed class UserProfileMailer : MailerBase, IUserProfileMailer     
	{
		public UserProfileMailer()
		{
            MasterName = "_EmailLayout";
		}

		public MailMessage TaxInfo(UserProfileDetailsModel model)
		{
		    var mailMessage = new MailMessage { Subject = "So long, farewell..." };
			mailMessage.To.Add(model.Email);
			ViewData = new ViewDataDictionary(model);
			PopulateBody(mailMessage, viewName: "TaxInfo");
			return mailMessage;
		}

		public MailMessage WelcomeBack(UserProfileDetailsModel model)
		{
		    var mailMessage = new MailMessage { Subject = "Welcome back!" };
            mailMessage.To.Add(model.Email);
            ViewData = new ViewDataDictionary(model);
            PopulateBody(mailMessage, viewName: "WelcomeBack");
		    return mailMessage;
		}
	}
}