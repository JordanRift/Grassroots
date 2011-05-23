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

using System.Linq;
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

            foreach (var email in emails.Where(email => !string.IsNullOrWhiteSpace(email)))
            {
                mailMessage.To.Add(email.Trim());
            }
            
            ViewData = new ViewDataDictionary(model);
            PopulateBody(mailMessage, viewName: "CampaignEmailBlast");
		    return mailMessage;
		}
	}
}