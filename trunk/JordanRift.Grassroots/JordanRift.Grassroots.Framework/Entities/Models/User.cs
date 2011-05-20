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
using System.Web.Security;
using JordanRift.Grassroots.Framework.Entities.Validation;

namespace JordanRift.Grassroots.Framework.Entities.Models
{
    [MetadataType(typeof(IUserValidation))]
    [Table("gr_user")]
    public class User : IUserValidation
    {
        [Key]
        public int UserID { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public bool IsActive { get; set; }
        public bool IsAuthorized { get; set; }
        public bool ForcePasswordChange { get; set; }
        public DateTime RegisterDate { get; set; }
        public DateTime LastLoggedIn { get; set; }

        public int UserProfileID { get; set; }
        public virtual UserProfile UserProfile { get; set; }

        public MembershipUser GetMembershipUser()
        {
            var membershipUser = new MembershipUser(ConfigConstants.MEMBERSHIP_PROVIDER_NAME, Username, Username, Username, null, null, IsAuthorized,
                                                    IsActive, RegisterDate, LastLoggedIn, LastLoggedIn,
                                                    new DateTime(1900, 1, 1), new DateTime(1900, 1, 1));

            return membershipUser;
        }
    }

    public class UserConfiguration : EntityTypeConfiguration<User>
    {
        public UserConfiguration()
        {
            this.HasRequired(u => u.UserProfile).WithMany(u => u.Users).HasForeignKey(u => u.UserProfileID);
        }
    }
}
