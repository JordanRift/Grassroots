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

namespace JordanRift.Grassroots.Framework.Entities.Models
{
    [Table("gr_role")]
    public class Role
    {
        [Key]
        public int RoleID { get; set; }
        public int OrganizationID { get; set; }
        public virtual Organization Organization { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public virtual ICollection<UserProfile> UserProfiles { get; set; }
    }

    public class RoleConfiguration : EntityTypeConfiguration<Role>
    {
        public RoleConfiguration()
        {
            this.HasRequired(r => r.Organization).WithMany(o => o.Roles).HasForeignKey(r => r.OrganizationID);
        }
    }
}
