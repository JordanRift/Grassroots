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
