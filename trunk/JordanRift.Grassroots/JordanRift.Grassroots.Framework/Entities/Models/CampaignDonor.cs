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
	[MetadataType(typeof(ICampaignDonorValidation))]
    [Table("gr_CampaignDonor")]
    public class CampaignDonor : ICampaignDonorValidation
	{
        [Key]
        public int CampaignDonorID { get; set; }
        public int CampaignID { get; set; }
        public virtual Campaign Campaign { get; set; }
        public int? UserProfileID { get; set; }
        public virtual UserProfile UserProfile { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Comments { get; set; }
        public decimal Amount { get; set; }
        public string Email { get; set; }
        public string PrimaryPhone { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public DateTime DonationDate { get; set; }
        public bool Approved { get; set; }
        public string ReferenceID { get; set; }
        public string Notes { get; set; }

        public CampaignDonor()
        {
            if (DonationDate <= new DateTime(1900, 1, 1))
            {
                DonationDate = DateTime.Now;
            }
        }
	}

    public class CampaignDonorConfiguration : EntityTypeConfiguration<CampaignDonor>
    {
        public CampaignDonorConfiguration()
        {
            this.HasRequired(d => d.Campaign).WithMany(c => c.CampaignDonors).HasForeignKey(d => d.CampaignID);
            this.HasOptional(d => d.UserProfile).WithMany(p => p.CampaignDonors).HasForeignKey(d => d.UserProfileID);
        }
    }
}
