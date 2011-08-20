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

using System.ComponentModel.DataAnnotations;
using System.Data.Entity.ModelConfiguration;
using JordanRift.Grassroots.Framework.Entities.Validation;

namespace JordanRift.Grassroots.Framework.Entities.Models
{
    [MetadataType(typeof(IOrganizationSettingValidation))]
    [Table("gr_organizationsetting")]
    public class OrganizationSetting : IOrganizationSettingValidation
    {
        [Key]
        public int OrganizationSettingID { get; set; }
        public int OrganizationID { get; set; }
        public virtual Organization Organization { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
        public int DataType { get; set; }

        [NotMapped]
        public DataType Type
        {
            get { return (DataType) DataType; }
            set { DataType = (int) value; }
        }

        public OrganizationSetting()
        {
            Name = string.Empty;
            Value = string.Empty;
            Type = Entities.DataType.String;
        }

        public OrganizationSetting(string name, string value, DataType dataType)
        {
            Name = name;
            Value = value;
            Type = dataType;
        }
    }

    public class OrganizationSettingConfiguration : EntityTypeConfiguration<OrganizationSetting>
    {
        public OrganizationSettingConfiguration()
        {
            this.HasRequired(s => s.Organization).WithMany(o => o.OrganizationSettings).HasForeignKey(s => s.OrganizationID);
        }
    }
}
