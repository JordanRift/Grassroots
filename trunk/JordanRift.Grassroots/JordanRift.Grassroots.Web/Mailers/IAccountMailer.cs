using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Mvc.Mailer;
using System.Net.Mail;

namespace JordanRift.Grassroots.Web.Mailers
{ 
    public interface IAccountMailer
    {
				
		MailMessage Welcome();
		
				
		MailMessage PasswordReset();
		
				
		MailMessage PasswordChange();
		
		
	}
}