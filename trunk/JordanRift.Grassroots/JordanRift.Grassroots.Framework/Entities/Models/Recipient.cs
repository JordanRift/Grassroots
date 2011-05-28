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

namespace JordanRift.Grassroots.Framework.Entities.Models
{
    [Table("gr_recipient")]
    public class Recipient
    {
        [Key]
        public int RecipientID { get; set; }
        public int CauseID { get; set; }
        public virtual Cause Cause { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime Birthdate { get; set; }
    }

    public class RecipientConfiguration : EntityTypeConfiguration<Recipient>
    {
        public RecipientConfiguration()
        {
            this.HasRequired(r => r.Cause).WithMany(c => c.Recipients).HasForeignKey(r => r.CauseID);
        }
    }
}
