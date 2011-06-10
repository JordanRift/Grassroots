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
using System.Data.Entity.ModelConfiguration;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using JordanRift.Grassroots.Framework.Entities.Validation;
using JordanRift.Grassroots.Framework.Services;

namespace JordanRift.Grassroots.Framework.Entities.Models
{
	[MetadataType(typeof(IUserProfileValidation))]
    [Table("gr_userprofile")]
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

        public virtual ICollection<User> Users { get; set; }
        public virtual ICollection<CampaignDonor> CampaignDonors { get; set; }
        public virtual ICollection<Campaign> Campaigns { get; set; }
        public virtual ICollection<CauseNote> CauseNotes { get; set; }

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

	    [NotMapped]
	    public string ImagePath
	    {
	        get { return GetProfileImagePath(); }
	    }


        public string GetProfileImagePath(ProfileImageSize size = ProfileImageSize.Thumbnail)
        {
            if (string.IsNullOrEmpty(FacebookID))
            {
                var facebookUrl = string.Format("https://graph.facebook.com/{0}/picture", FacebookID);

                switch (size)
                {
                    case ProfileImageSize.Full:
                        facebookUrl += "?type=normal";
                        break;
                    case ProfileImageSize.Thumbnail:
                    default:
                        facebookUrl += "?type=square";
                        break;
                }

                return facebookUrl;
            }

            var service = new GravatarService();
            return service.GetGravatarPictureUrl(Email, (int) size);
        }

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
        //public decimal CalculateTotalHoursServed()
        //{
        //    // TODO: Add calculation for hours served once serving is supported in teh data model
        //    var total = 0m;
        //    return total;
        //}

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
