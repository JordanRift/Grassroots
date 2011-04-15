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
    [MetadataType(typeof(ICauseValidation))]
    [Table("gr_Cause")]
    public class Cause : ICauseValidation
    {
        [Key]
        public int CauseID { get; set; }
        public int OrganizationID { get; set; }
        public virtual Organization Organization { get; set; }
        public int CauseTemplateID { get; set; }
        public virtual CauseTemplate CauseTemplate { get; set; }
        public string Name { get; set; }
        public bool Active { get; set; }
        public string Summary { get; set; }
        public string VideoEmbedHtml { get; set; }
        public string DescriptionHtml { get; set; }
        public string ImagePath { get; set; }

        public virtual ICollection<Campaign> Campaigns { get; set; }
    }

    public class CauseConfiguration : EntityTypeConfiguration<Cause>
    {
        public CauseConfiguration()
        {
            this.HasRequired(c => c.Organization).WithMany(o => o.Causes).HasForeignKey(c => c.OrganizationID);
            this.HasRequired(c => c.CauseTemplate).WithMany(t => t.Causes).HasForeignKey(c => c.CauseTemplateID);
        }
    }
}
