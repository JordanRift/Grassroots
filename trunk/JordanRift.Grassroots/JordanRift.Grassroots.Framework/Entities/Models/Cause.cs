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
using JordanRift.Grassroots.Framework.Entities.Validation;

namespace JordanRift.Grassroots.Framework.Entities.Models
{
	[MetadataType( typeof( ICauseValidation ) )]
	[Table( "gr_cause" )]
	public class Cause : Model, ICauseValidation
	{
		[Key]
		public int CauseID { get; set; }
		public int OrganizationID { get; set; }
		public virtual Organization Organization { get; set; }
		public int CauseTemplateID { get; set; }
		public virtual CauseTemplate CauseTemplate { get; set; }
        public int? RegionID { get; set; }
	    public virtual Region Region { get; set; }
		public string Name { get; set; }
		public bool Active { get; set; }
		public string Summary { get; set; }
		public string VideoEmbedHtml { get; set; }
		public string DescriptionHtml { get; set; }
		public string ImagePath { get; set; }
        public int? HoursVolunteered { get; set; }
	    public string BeforeImagePath { get; set; }
        public string AfterImagePath { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public string ReferenceNumber { get; set; }
        public bool IsCompleted { get; set; }
        public DateTime? DateCompleted { get; set; }

		public virtual ICollection<Campaign> Campaigns { get; set; }
		public virtual ICollection<CauseNote> CauseNotes { get; set; }
        public virtual ICollection<Recipient> Recipients { get; set; }

		/// <summary>
		/// Used to create a new instance of a CauseNote object for this cause.
		/// </summary>
		/// <returns>New note for this cause.</returns>
		public CauseNote CreateNote()
		{
			var note = new CauseNote
							{
								CauseID = this.CauseID
							};

			return note;
		}
	}

	public class CauseConfiguration : EntityTypeConfiguration<Cause>
	{
		public CauseConfiguration()
		{
			this.HasRequired(c => c.Organization).WithMany(o => o.Causes).HasForeignKey(c => c.OrganizationID);
			this.HasRequired(c => c.CauseTemplate).WithMany(t => t.Causes).HasForeignKey(c => c.CauseTemplateID);
		    this.HasOptional(c => c.Region).WithMany(r => r.Causes).HasForeignKey(c => c.RegionID);
		}
	}
}
