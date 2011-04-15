//
// Copyright © 2011 Jordan Rift, LLC - All Rights Reserved
//
// THIS WORK IS LICENSED UNDER A CREATIVE COMMONS ATTRIBUTION-NONCOMMERCIAL-
// SHAREALIKE 3.0 UNPORTED LICENSE:
// http://creativecommons.org/licenses/by-nc-sa/3.0/
//

using System;
using System.Net.Mail;
using JordanRift.Grassroots.Framework.Data;
using JordanRift.Grassroots.Framework.Entities;
using JordanRift.Grassroots.Framework.Helpers;

namespace JordanRift.Grassroots.Framework.Services
{
    public class SmtpEmailService : IEmailService
    {
        private delegate void SendEmailDelegate(string emailAddress, string fromEmailAddress, string subject, string message);
        private readonly IOrganizationRepository organizationRepository;

        public SmtpEmailService()
        {
            organizationRepository = RepositoryFactory.GetRepository<IOrganizationRepository>();
        }

        public SmtpEmailService(IOrganizationRepository organizationRepository)
        {
            this.organizationRepository = organizationRepository;
        }

        public void SendTo(string emailAddress, string subject, string message)
        {
            var organization = organizationRepository.GetDefaultOrganization();
            SendAsync(emailAddress, organization.ContactEmail, subject, message);
        }

        public void SendTo(Payment payment)
        {
            var organization = organizationRepository.GetDefaultOrganization();
            var donateEmail = organization.GetSetting(OrgSettingKeys.ORGANIZATION_DONATION_EMAIL_ADDRESS);
            var subject = organization.GetSetting(OrgSettingKeys.ORGANIZATION_DONATION_EMAIL_SUBJECT);
            var body = organization.GetSetting(OrgSettingKeys.ORGANIZATION_DONATION_EMAIL_BODY);

            SendAsync(payment.Email, donateEmail.Value, subject.Value, body.Value);
            SendAsync(donateEmail.Value, organization.ContactEmail, string.Format("Donation to {0}", organization.Name), payment.Notes);
        }

        private static void SendSmtp(string emailAddress, string fromEmailAddress, string subject, string message)
        {
            MailMessage email = new MailMessage();
            email.To.Add(emailAddress);
            email.From = new MailAddress(fromEmailAddress, "One Mission");
            email.Subject = subject;
            email.Body = message;
            email.IsBodyHtml = true;
            SmtpClient client = new SmtpClient();

            try
            {
                client.Send(email);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
            }
        }

        private static void SendAsync(string emailAddress, string fromEmailAddress, string subject, string message)
        {
            SendEmailDelegate del = SendSmtp;
            AsyncCallback callback = Callback;
            del.BeginInvoke(emailAddress, fromEmailAddress, subject, message, callback, null);
        }

        private static void Callback(IAsyncResult result)
        {
        }
    }
}
