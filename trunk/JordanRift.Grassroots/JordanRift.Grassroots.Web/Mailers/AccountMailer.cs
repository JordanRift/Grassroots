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
    public sealed class AccountMailer : MailerBase, IAccountMailer     
	{
		public AccountMailer()
		{
		    MasterName = "_EmailLayout";
		}

		
		public MailMessage Welcome(RegisterModel model)
		{
			var mailMessage = new MailMessage{Subject = "Ahoy there! Welcome aboard!"};
			mailMessage.To.Add(model.Email);
            ViewData = new ViewDataDictionary(model);
			PopulateBody(mailMessage, viewName: "Welcome");
			return mailMessage;
		}

		
		public MailMessage PasswordReset(RegisterModel model)
		{
			var mailMessage = new MailMessage{Subject = "Password Reset Notofication"};
			mailMessage.To.Add(model.Email);
            ViewData = new ViewDataDictionary(model);
			PopulateBody(mailMessage, viewName: "PasswordReset");
			return mailMessage;
		}

		
		public MailMessage PasswordChange(RegisterModel model)
		{
			var mailMessage = new MailMessage{Subject = "Password Change Notofication"};
			mailMessage.To.Add(model.Email);
            ViewData = new ViewDataDictionary(model);
			PopulateBody(mailMessage, viewName: "PasswordChange");
			return mailMessage;
		}		
	}
}