//
// Copyright © 2011 Jordan Rift, LLC - All Rights Reserved
//
// THIS WORK IS LICENSED UNDER A CREATIVE COMMONS ATTRIBUTION-NONCOMMERCIAL-
// SHAREALIKE 3.0 UNPORTED LICENSE:
// http://creativecommons.org/licenses/by-nc-sa/3.0/
//

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity.ModelConfiguration;
using JordanRift.Grassroots.Framework.Entities.Validation;

namespace JordanRift.Grassroots.Framework.Entities.Models
{
    [MetadataType(typeof(ICauseTemplateValidation))]
    [Table("gr_causetemplate")]
    public class CauseTemplate : ICauseTemplateValidation
    {
        [Key]
        public int CauseTemplateID { get; set; }
        public int OrganizationID { get; set; }
        public virtual Organization Organization { get; set; }
        public string Name { get; set; }
        public string ActionVerb { get; set; }
        public string GoalName { get; set; }
        public bool Active { get; set; }
        public bool AmountIsConfigurable { get; set; }
        public decimal DefaultAmount { get; set; }
        public bool TimespanIsConfigurable { get; set; }
        public int DefaultTimespanInDays { get; set; }
        public string Summary { get; set; }
        public string VideoEmbedHtml { get; set; }
        public string DescriptionHtml { get; set; }
        public string ImagePath { get; set; }

        public virtual ICollection<Campaign> Campaigns { get; set; }
        public virtual ICollection<Cause> Causes { get; set; }

        /// <summary>
        /// Used to create a new instance of a Cause object based on the CauseTemplate.
        /// </summary>
        /// <returns>New instance of a Cause object set to CauseTemplate's default values.</returns>
        public Cause CreateCause()
        {
            var cause = new Cause
                            {
                                Name = this.Name,
                                Summary = this.Summary,
                                DescriptionHtml = this.DescriptionHtml,
                                ImagePath = this.ImagePath,
                                VideoEmbedHtml = this.VideoEmbedHtml,
                                Active = true
                            };

            return cause;
        }
    }

    public class CauseTemplateConfiguration : EntityTypeConfiguration<CauseTemplate>
    {
        public CauseTemplateConfiguration()
        {
            this.HasRequired(t => t.Organization).WithMany(o => o.CauseTemplates).HasForeignKey(t => t.OrganizationID);
        }
    }
}
