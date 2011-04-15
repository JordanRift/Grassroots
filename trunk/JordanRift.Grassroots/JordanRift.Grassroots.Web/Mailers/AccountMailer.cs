using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Mvc.Mailer;
using System.Net.Mail;

namespace JordanRift.Grassroots.Web.Mailers
{ 
    public class AccountMailer : MailerBase, IAccountMailer     
	{
		public AccountMailer():
			base()
		{
			MasterName="_Layout";
		}

		
		public virtual MailMessage Welcome()
		{
			var mailMessage = new MailMessage{Subject = "Welcome"};
			
			//mailMessage.To.Add("some-email@example.com");
			//ViewBag.Data = someObject;
			PopulateBody(mailMessage, viewName: "Welcome");

			return mailMessage;
		}

		
		public virtual MailMessage PasswordReset()
		{
			var mailMessage = new MailMessage{Subject = "PasswordReset"};
			
			//mailMessage.To.Add("some-email@example.com");
			//ViewBag.Data = someObject;
			PopulateBody(mailMessage, viewName: "PasswordReset");

			return mailMessage;
		}

		
		public virtual MailMessage PasswordChange()
		{
			var mailMessage = new MailMessage{Subject = "PasswordChange"};
			
			//mailMessage.To.Add("some-email@example.com");
			//ViewBag.Data = someObject;
			PopulateBody(mailMessage, viewName: "PasswordChange");

			return mailMessage;
		}

		
	}
}