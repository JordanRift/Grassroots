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
using System.ComponentModel.DataAnnotations;
using System.Data.Entity.ModelConfiguration;
using JordanRift.Grassroots.Framework.Entities.Validation;

namespace JordanRift.Grassroots.Framework.Entities.Models
{
	[MetadataType(typeof(ICauseNoteValidation))]
	[Table("gr_causenote")]
	public class CauseNote : Model, ICauseNoteValidation
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
		    this.HasRequired(u => u.UserProfile).WithMany(c => c.CauseNotes).HasForeignKey(d => d.UserProfileID);
		}
	}
}
