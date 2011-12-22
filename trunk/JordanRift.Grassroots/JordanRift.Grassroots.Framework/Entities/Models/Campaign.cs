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
    public class Campaign : Model, ICampaignValidation, IValidatableObject, ICanCalculate
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
        public int? CampaignType { get; set; }
        public bool IsGeneralFund { get; set; }

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
        public CampaignType TheCampaignType
        {
            get { return this.CampaignType.HasValue ? (Entities.CampaignType) this.CampaignType.Value : Entities.CampaignType.Unknown; }
            set { this.CampaignType = (int) value; }
        }

        [NotMapped]
        public CampaignService CampaignService { get; set; }

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
