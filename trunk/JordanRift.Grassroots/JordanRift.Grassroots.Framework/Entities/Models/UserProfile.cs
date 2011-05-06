//
// Copyright © 2011 Jordan Rift, LLC - All Rights Reserved
//
// THIS WORK IS LICENSED UNDER A CREATIVE COMMONS ATTRIBUTION-NONCOMMERCIAL-
// SHAREALIKE 3.0 UNPORTED LICENSE:
// http://creativecommons.org/licenses/by-nc-sa/3.0/
//

using System;
using System.Data.Entity.ModelConfiguration;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using JordanRift.Grassroots.Framework.Entities.Validation;
using JordanRift.Grassroots.Framework.Services;

namespace JordanRift.Grassroots.Framework.Entities.Models
{
	[MetadataType(typeof(IUserProfileValidation))]
    [Table("gr_UserProfile")]
    public class UserProfile : IUserProfileValidation, IValidatableObject, ICanCalculate
	{
        [Key]
        public int UserProfileID { get; set; }
        public int OrganizationID { get; set; }
        public virtual Organization Organization { get; set; }
        public int? RoleID { get; set; }
        public virtual Role Role { get; set; }
        public string FacebookID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime Birthdate { get; set; }
        public string Gender { get; set; }
        public string Email { get; set; }
        public string PrimaryPhone { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public bool Consent { get; set; }
        public bool Active { get; set; }
        public bool IsActivated { get; set; }
        public string ImagePath { get; set; }

        public virtual ICollection<User> Users { get; set; }
        public virtual ICollection<CampaignDonor> CampaignDonors { get; set; }
        public virtual ICollection<Campaign> Campaigns { get; set; }

        [NotMapped]
        public string FullName { get { return string.Format("{0} {1}", FirstName, LastName); } }

		[NotMapped]
		public int Age
		{
			get
			{
				DateTime now = DateTime.Today;
				int age = now.Year - Birthdate.Year;
				if ( Birthdate > now.AddYears( -age ) ) age--;
				return age;
			}
		}

        [NotMapped]
        public UserProfileService UserProfileService { get; set; }

        /// <summary>
        /// Attempts to get the UserProfile's Campaigns that are currently active.
        /// </summary>
        /// <returns>List of Campaigns</returns>
        public IEnumerable<Campaign> GetActiveCampaigns()
        {
            return Campaigns == null ? new List<Campaign>() : Campaigns.Where(c => c.IsActive);
        }

		/// <summary>
		/// Calculates the total hours served by the user.
		/// </summary>
		/// <returns>Total hours served</returns>
		public decimal CalculateTotalHoursServed()
		{
			// TODO:
			var total = 0m;
			return total;
		}

		/// <summary>
		/// Calculates the total donations given by the user.
		/// </summary>
		/// <returns>Total donations given</returns>
		public decimal CalculateTotalDonationsGiven()
		{
			var total = 0m;

			if ( CampaignDonors != null )
			{
				total = (from c in CampaignDonors
						where c.Approved
						select c.Amount ).Sum();
			}

			return total;
		}

		/// <summary>
		/// Calculates the total number of donations made by the user.
		/// </summary>
		/// <returns>Total number of donations made</returns>
		public int CalculateTotalNumberOfDonationsMade()
		{
			var total = 0;

			if ( CampaignDonors != null )
			{
				total = ( from c in CampaignDonors
						  where c.Approved
						  select c ).Count();
			}

			return total;
		}

		/// <summary>
		/// Calculates the total number of campaigns that the user made donations to.
		/// </summary>
		/// <returns>Total number of campaigns donated to</returns>
		public int CalculateTotalNumberOfCampaignsDonatedTo()
		{
			var total = 0;

			if ( CampaignDonors != null )
			{
				total = ( from c in CampaignDonors
						  where c.Approved
						  select c.CampaignID ).Distinct().Count();
			}

			return total;
		}
        /// <summary>
        /// Calculates the total raised by all of the User's campaigns.
        /// </summary>
        /// <returns>Total raised</returns>
        public decimal CalculateTotalDonations()
        {
            var total = 0m;

            if (Campaigns != null)
            {
                total = Campaigns.Sum(campaign => (from c in campaign.CampaignDonors
                                                   where c.Approved
                                                   select c.Amount).Sum());
            }

            return total;
        }

	    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Birthdate > DateTime.Now.AddYears(-13))
            {
                yield return new ValidationResult("You must be at least 13 years of age to donate.", new[] { "BirthDate" });
            }

            if (!Consent)
            {
                yield return new ValidationResult("You must authorize us to create a user profile on your behalf.", new[] { "Consent" });
            }

            if (!IsUnique(UserProfileService, Email, UserProfileID))
            {
                yield return new ValidationResult("This email address is already in use. Please choose another one.", new[] { "Email" });
            }
        }

	    public static bool IsUnique(UserProfileService userProfileService, string email, int id)
	    {
            if (userProfileService == null)
	        {
                userProfileService = new UserProfileService();
	        }

	        return userProfileService.IsUnique(email, id);
	    }
	}

    public class UserProfileConfiguration : EntityTypeConfiguration<UserProfile>
    {
        public UserProfileConfiguration()
        {
            this.HasRequired(u => u.Organization).WithMany(o => o.UserProfiles).HasForeignKey(u => u.OrganizationID);
            this.HasOptional(u => u.Role).WithMany(r => r.UserProfiles).HasForeignKey(u => u.RoleID);
        }
    }
}
