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
using System.Web.Mvc;

namespace JordanRift.Grassroots.Web.Models
{
    public class PasswordAdminModel
    {
        [Required]
        public int UserProfileID { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        [Display(Name = "Confirm Password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password must match.")]
        public string ConfirmPassword { get; set; }
        
        [Display(Name = "Force password change on next log in?")]
        public bool ForcePasswordChange { get; set; }
    }
}