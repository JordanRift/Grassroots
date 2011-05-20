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
using JordanRift.Grassroots.Framework.Services;

namespace JordanRift.Grassroots.Framework.Entities.Models
{
    [MetadataType(typeof(ICampaignValidation))]
    [Table("gr_campaign")]
    public class Campaign : ICampaignValidation, IValidatableObject, ICanCalculate
    {
        [Key]
        public int CampaignID { get; set; }
        public int OrganizationID { get; set; }
        public virtual Organization Organization { get; set; }
        public int UserProfileID { get; set; }
        public virtual UserProfile UserProfile { get; set; }
        public int CauseTemplateID { get; set; }
        public virtual CauseTemplate CauseTemplate { get; set; }
        public int? CauseID { get; set; }
        public Cause Cause { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string ImagePath { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal GoalAmount { get; set; }
        public string UrlSlug { get; set; }

        public virtual ICollection<CampaignDonor> CampaignDonors { get; set; }

        /// <summary>
        /// Indicates whether or not DateTime.Now is currently between the Start and End dates of the Campaign.
        /// </summary>
        [NotMapped]
        public bool IsActive
        {
            get
            {
                return (DateTime.Now >= StartDate) && (DateTime.Now <= EndDate);
            }
        }

        [NotMapped]
        public CampaignService CampaignService { get; set; }

        /// <summary>
        /// ICampaignRepository instance for UrlSlug validation. Can be set manually or via Ninject and DependencyResolver
        /// </summary>
        //[Inject]
        //[NotMapped]
        //public ICampaignRepository CampaignRepository { get; set; }

        public Campaign()
        {
            LoadDefaultSettings();
        }

        private void LoadDefaultSettings()
        {
            if (CauseTemplate != null)
            {
                StartDate = DateTime.Now;
                EndDate = DateTime.Now.AddDays(CauseTemplate.DefaultTimespanInDays);
                GoalAmount = CauseTemplate.DefaultAmount;
            }
        }

        /// <summary>
        /// Calculates the total raised by the current campaign.
        /// </summary>
        /// <returns>Total raised</returns>
        public decimal CalculateTotalDonations()
        {
            var total = 0m;

            if (CampaignDonors != null)
            {
                total = (from c in CampaignDonors
                         where c.Approved
                         select c.Amount).Sum();
            }

            return total;
        }

        /// <summary>
        /// This method hooks into MVC 3's custom validation framework.
        /// </summary>
        /// <param name="validationContext"></param>
        /// <returns>ValidationResults based on custom rules</returns>
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (!CauseTemplate.AmountIsConfigurable && CauseTemplate.DefaultAmount != GoalAmount)
            {
                yield return new ValidationResult(string.Format("'Goal Amount' must equal {0}.", CauseTemplate.DefaultAmount), 
                    new[] { "GoalAmount" });
            }

            TimeSpan timeInDays = (EndDate - StartDate);

            if (!CauseTemplate.TimespanIsConfigurable && timeInDays.Days != CauseTemplate.DefaultTimespanInDays)
            {
                yield return new ValidationResult(string.Format("'End Date' must be {0} days after 'Start Date'.", CauseTemplate.DefaultTimespanInDays), 
                    new[] {"StartDate", "EndDate"});
            }

            if (!IsUnique(CampaignService, UrlSlug, CampaignID))
            {
                yield return new ValidationResult("'URL Slug' is already in use by another campaign.", new[] { "UrlSlug" });
            }
        }

        /// <summary>
        /// Indicates whether the Campaign's UrlSlug is unique within the context of the provided repository.
        /// </summary>
        /// <param name="campaignService">CampaignService to delegate validation to</param>
        /// <param name="urlSlug">UrlSlug to validate against</param>
        /// <param name="id">CampaignID to validate against</param>
        /// <returns>True/False based on whether or not the UrlSlug is unique to the repository</returns>
        public static bool IsUnique(CampaignService campaignService, string urlSlug, int id)
        {
            if (campaignService == null)
            {
                campaignService = new CampaignService();
            }

            return campaignService.IsUnique(urlSlug, id);
        }
    }

    public class CampaignConfiguration : EntityTypeConfiguration<Campaign>
    {
        public CampaignConfiguration()
        {
            this.HasRequired(c => c.UserProfile).WithMany(u => u.Campaigns).HasForeignKey(c => c.UserProfileID);
            this.HasRequired(c => c.CauseTemplate).WithMany(t => t.Campaigns).HasForeignKey(c => c.CauseTemplateID);
            this.HasOptional(c => c.Cause).WithMany(c => c.Campaigns).HasForeignKey(c => c.CauseID);
            this.HasRequired(c => c.Organization).WithMany(o => o.Campaigns).HasForeignKey(c => c.OrganizationID);
        }
    }
}
