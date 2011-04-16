//
// Copyright © 2011 Jordan Rift, LLC - All Rights Reserved
//
// THIS WORK IS LICENSED UNDER A CREATIVE COMMONS ATTRIBUTION-NONCOMMERCIAL-
// SHAREALIKE 3.0 UNPORTED LICENSE:
// http://creativecommons.org/licenses/by-nc-sa/3.0/
//

using System.Net.Mail;
using JordanRift.Grassroots.Framework.Entities;
using JordanRift.Grassroots.Web.Models;

namespace JordanRift.Grassroots.Web.Mailers
{ 
    public interface IDonateMailer
    {
		MailMessage CampaignDonation(DonationDetailsModel model);
		MailMessage UserDonation(Payment model);
    }
}