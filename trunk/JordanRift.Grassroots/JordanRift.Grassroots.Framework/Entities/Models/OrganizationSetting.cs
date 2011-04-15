//
// Copyright © 2011 Jordan Rift, LLC - All Rights Reserved
//
// THIS WORK IS LICENSED UNDER A CREATIVE COMMONS ATTRIBUTION-NONCOMMERCIAL-
// SHAREALIKE 3.0 UNPORTED LICENSE:
// http://creativecommons.org/licenses/by-nc-sa/3.0/
//

using System.ComponentModel.DataAnnotations;
using System.Data.Entity.ModelConfiguration;
using JordanRift.Grassroots.Framework.Entities.Validation;

namespace JordanRift.Grassroots.Framework.Entities.Models
{
    [MetadataType(typeof(IOrganizationSettingValidation))]
    [Table("gr_OrganizationSetting")]
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
            Type = Entities.DataType.STRING;
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
