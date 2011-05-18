//
// Copyright © 2011 Jordan Rift, LLC - All Rights Reserved
//
// THIS WORK IS LICENSED UNDER A CREATIVE COMMONS ATTRIBUTION-NONCOMMERCIAL-
// SHAREALIKE 3.0 UNPORTED LICENSE:
// http://creativecommons.org/licenses/by-nc-sa/3.0/
//

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using JordanRift.Grassroots.Framework.Entities.Validation;

namespace JordanRift.Grassroots.Framework.Entities.Models
{
    [MetadataType(typeof(IOrganizationValidation))]
    [Table("gr_Organization")]
    public class Organization : IOrganizationValidation, ICanCalculate
    {
        [Key]
        public int OrganizationID { get; set; }
        public string Name { get; set; }
        public string Tagline { get; set; }
        public string Summary { get; set; }
        public string DescriptionHtml { get; set; }
        public string ContactPhone { get; set; }
        public string ContactEmail { get; set; }
        
        public decimal YtdGoal { get; set; }
        public int FiscalYearStartMonth { get; set; }
        public int FiscalYearStartDay { get; set; }

        public int PaymentGatewayType { get; set; }
        public string PaymentGatewayApiUrl { get; set; }
        public string PaymentGatewayApiKey { get; set; }
        public string PaymentGatewayApiSecret { get; set; }

        public string FacebookPageUrl { get; set; }
        public string VideoEmbedHtml { get; set; }
        public string TwitterName { get; set; }
        public string BlogRssUrl { get; set; }
        public string ThemeName { get; set; }

        public virtual ICollection<OrganizationSetting> OrganizationSettings { get; set; }
        public virtual ICollection<CauseTemplate> CauseTemplates { get; set; }
        public virtual ICollection<Cause> Causes { get; set; }
        public virtual ICollection<Campaign> Campaigns { get; set; }
        public virtual ICollection<Role> Roles { get; set; }
        public virtual ICollection<UserProfile> UserProfiles { get; set; }

        [NotMapped]
        public PaymentGatewayType PaymentGateway
        {
            get { return  (PaymentGatewayType) PaymentGatewayType; }
            set { PaymentGatewayType = (int) value; }
        }

        [NotMapped]
        public DateTime FiscalYearStart
        {
            get
            {
                var currentMonth = DateTime.Now.Month;
                var currentDay = DateTime.Now.Day;
                int fiscalYear = DateTime.Now.Year;

                if ((FiscalYearStartMonth > currentMonth) ||
                    (FiscalYearStartMonth == currentMonth && FiscalYearStartDay > currentDay))
                {
                    fiscalYear = DateTime.Now.Year - 1;
                }

                return new DateTime(fiscalYear, FiscalYearStartMonth, FiscalYearStartDay);
            }
        }

        public Organization()
        {
            if (string.IsNullOrEmpty(ThemeName))
            {
                ThemeName = "grassroots-theme";
            }
        }

        public OrganizationSetting GetSetting(string key)
        {
            return OrganizationSettings.FirstOrDefault(s => s.Name == key);
        }

        /// <summary>
        /// Calculates the total raised across all campaigns of the organization during the current fiscal year.
        /// </summary>
        /// <returns>Total raised during current fiscal year</returns>
        public decimal CalculateTotalDonations()
        {
            var total = 0m;

            try
            {
                if (Campaigns != null)
                {
                    total = Campaigns.Sum(campaign => (from c in campaign.CampaignDonors
                                                       where c.Approved && (c.DonationDate >= FiscalYearStart && c.DonationDate <= DateTime.Now)
                                                       select c.Amount).Sum());
                }
            }
            catch (ObjectDisposedException) { }

            return total;
        }
    }

    public class OrganizationConfiguration : EntityTypeConfiguration<Organization>
    {
    }
}
