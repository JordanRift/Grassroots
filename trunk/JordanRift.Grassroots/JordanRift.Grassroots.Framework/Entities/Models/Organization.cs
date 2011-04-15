﻿//
// Copyright © 2011 Jordan Rift, LLC - All Rights Reserved
//
// THIS WORK IS LICENSED UNDER A CREATIVE COMMONS ATTRIBUTION-NONCOMMERCIAL-
// SHAREALIKE 3.0 UNPORTED LICENSE:
// http://creativecommons.org/licenses/by-nc-sa/3.0/
//

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using JordanRift.Grassroots.Framework.Entities.Validation;

namespace JordanRift.Grassroots.Framework.Entities.Models
{
    [MetadataType(typeof(IOrganizationValidation))]
    [Table("gr_Organization")]
    public class Organization : IOrganizationValidation
    {
        [Key]
        public int OrganizationID { get; set; }
        public string Name { get; set; }
        public string ContactPhone { get; set; }
        public string ContactEmail { get; set; }
        public int PaymentGatewayType { get; set; }
        public string PaymentGatewayApiUrl { get; set; }
        public string PaymentGatewayApiKey { get; set; }
        public string PaymentGatewayApiSecret { get; set; }

        public string FacebookPageUrl { get; set; }
        public string VideoEmbedHtml { get; set; }
        public string TwitterName { get; set; }

        public virtual ICollection<OrganizationSetting> OrganizationSettings { get; set; }
        public virtual ICollection<CauseTemplate> CauseTemplates { get; set; }
        public virtual ICollection<Cause> Causes { get; set; }
        public virtual ICollection<Campaign> Campaigns { get; set; }
        public virtual ICollection<Role> Roles { get; set; }
        public virtual ICollection<UserProfile> UserProfiles { get; set; }

        [NotMapped]
        public PaymentGatewayType PaymentGateway
        {
            get { return  (PaymentGatewayType) PaymentGatewayType; }
            set { PaymentGatewayType = (int) value; }
        }

        public OrganizationSetting GetSetting(string key)
        {
            return OrganizationSettings.FirstOrDefault(s => s.Name == key);
        }
    }

    public class OrganizationConfiguration : EntityTypeConfiguration<Organization>
    {
    }
}
