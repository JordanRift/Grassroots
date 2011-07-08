using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using JordanRift.Grassroots.Framework.Helpers;

namespace JordanRift.Grassroots.Framework.Entities.Models
{
    public class OrganizationBase : ICanCalculate
    {
        [Key]
        public int OrganizationID { get; set; }
        public string Name { get; set; }
        public string Tagline { get; set; }
        public string SummaryHtml { get; set; }
        public string DescriptionHtml { get; set; }
        public string ContactPhone { get; set; }
        public string ContactEmail { get; set; }

        public decimal? YtdGoal { get; set; }
        public int? FiscalYearStartMonth { get; set; }
        public int? FiscalYearStartDay { get; set; }

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

                if (FiscalYearStartMonth.HasValue && FiscalYearStartDay.HasValue)
                {
                    return new DateTime(fiscalYear, FiscalYearStartMonth.Value, FiscalYearStartDay.Value);
                }

                return new DateTime(fiscalYear, 1, 1);
            }
        }

        [NotMapped]
        public PaymentGatewayType PaymentGateway
        {
            get { return (PaymentGatewayType)PaymentGatewayType; }
            set { PaymentGatewayType = (int)value; }
        }

        /// <summary>
        /// Calculates the total raised across all campaigns of the organization during the current fiscal year.
        /// </summary>
        /// <returns>Total raised during current fiscal year</returns>
        public decimal CalculateTotalDonationsYTD()
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
            catch (ObjectDisposedException ex) { Logger.LogError(ex); }

            return total;
        }

        /// <summary>
        /// Calculates the total raised across all campaigns of the organization.
        /// </summary>
        /// <returns>Total raised</returns>
        public decimal CalculateTotalDonations()
        {
            var total = 0m;

            try
            {
                if (Campaigns != null)
                {
                    total = Campaigns
                        .Where(c => c.Title != "General")
                        .Sum(campaign => (from c in campaign.CampaignDonors
                                          where c.Approved
                                          select c.Amount).Sum());
                }
            }
            catch (ObjectDisposedException ex) { Logger.LogError(ex); }

            return total;
        }

        /// <summary>
        /// Calculates the total of all goals of the organization.
        /// </summary>
        /// <returns>goal total</returns>
        public decimal CalculateGoalTotal()
        {
            var total = 0m;

            try
            {
                if (Campaigns != null)
                {
                    total = Campaigns.Where(c => c.Title != "General").Sum(c => c.GoalAmount);
                }
            }
            catch (ObjectDisposedException ex) { Logger.LogError(ex); }

            return total;
        }

        public int GetDonationCount()
        {
            var total = 0;

            try
            {
                if (Campaigns != null)
                {
                    total = (from c in Campaigns
                             where c.CampaignDonors != null && c.Title != "General"
                             from d in c.CampaignDonors
                             where d.Approved && (d.DonationDate >= FiscalYearStart && d.DonationDate <= DateTime.Now)
                             select d)
                            .Count();
                }
            }
            catch (ObjectDisposedException ex) { Logger.LogError(ex); }

            return total;
        }

        public OrganizationSetting GetSetting(string key)
        {
            try
            {
                return OrganizationSettings.FirstOrDefault(s => s.Name == key);
            }
            catch (ObjectDisposedException ex)
            {
                Logger.LogError(ex);
                return null;
            }
        }

        public IEnumerable<Cause> GetCompletedCauses()
        {
            try
            {
                return from c in Causes
                       where c.IsCompleted //&& c.Active
                       select c;
            }
            catch (ObjectDisposedException ex)
            {
                Logger.LogError(ex);
                return new List<Cause>();
            }
        }
    }
}
