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

		public MailMessage TaxInfo(DeactivateModel model)
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