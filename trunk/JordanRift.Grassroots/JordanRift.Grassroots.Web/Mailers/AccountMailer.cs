//
// Grassroots is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// Grassroots is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with Grassroots.  If not, see <http://www.gnu.org/licenses/>.
//

using System;
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
		    var mailMessage = new MailMessage { Subject = "Ahoy there! Welcome aboard!" };
			mailMessage.To.Add(model.Email);
            ViewData = new ViewDataDictionary(model);
			PopulateBody(mailMessage, viewName: "Welcome");
			return mailMessage;
		}

		
		public MailMessage PasswordReset(RegisterModel model)
		{
		    var mailMessage = new MailMessage { Subject = "Password Reset Notofication" };
			mailMessage.To.Add(model.Email);
            ViewData = new ViewDataDictionary(model);
			PopulateBody(mailMessage, viewName: "PasswordReset");
			return mailMessage;
		}

		
		public MailMessage PasswordChange(RegisterModel model)
		{
		    var mailMessage = new MailMessage { Subject = "Password Change Notofication" };
			mailMessage.To.Add(model.Email);
            ViewData = new ViewDataDictionary(model);
			PopulateBody(mailMessage, viewName: "PasswordChange");
			return mailMessage;
		}

        public MailMessage Authorize(AuthorizeModel model)
        {
            var mailMessage = new MailMessage { Subject = "Email verification needed" };
            mailMessage.To.Add(model.Email);
            ViewData = new ViewDataDictionary(model);
            PopulateBody(mailMessage, viewName: "Authorize");
            return mailMessage;
        }
	}
}