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
        public string BeforeImagePath { get; set; }
        public string AfterImagePath { get; set; }

        public virtual ICollection<Campaign> Campaigns { get; set; }
        public virtual ICollection<Cause> Causes { get; set; }
        public virtual ICollection<Region> Regions { get; set; }

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
