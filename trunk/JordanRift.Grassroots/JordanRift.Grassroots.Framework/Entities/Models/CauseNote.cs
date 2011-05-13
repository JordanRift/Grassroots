//
// Copyright © 2011 Jordan Rift, LLC - All Rights Reserved
//
// THIS WORK IS LICENSED UNDER A CREATIVE COMMONS ATTRIBUTION-NONCOMMERCIAL-
// SHAREALIKE 3.0 UNPORTED LICENSE:
// http://creativecommons.org/licenses/by-nc-sa/3.0/
//

using System;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity.ModelConfiguration;
using JordanRift.Grassroots.Framework.Entities.Validation;

namespace JordanRift.Grassroots.Framework.Entities.Models
{
	[MetadataType(typeof(ICauseNoteValidation))]
	[Table("gr_CauseNote")]
	public class CauseNote : ICauseNoteValidation
	{
		[Key]
		public int CauseNoteID { get; set; }
		public int CauseID { get; set; }
		public virtual Cause Cause { get; set; }
		public int UserProfileID { get; set; }
		public virtual UserProfile UserProfile { get; set; }
		public string Text { get; set; }
		public DateTime EntryDate { get; set; }

		public CauseNote()
		{
			if ( EntryDate <= new DateTime( 1900, 1, 1 ) )
			{
				EntryDate = DateTime.Now;
			}
		}
	}

	public class CauseNoteConfiguration : EntityTypeConfiguration<CauseNote>
	{
		public CauseNoteConfiguration()
		{
			this.HasRequired( d => d.Cause ).WithMany( c => c.CauseNotes ).HasForeignKey( d => d.CauseID);
		}
	}
}
