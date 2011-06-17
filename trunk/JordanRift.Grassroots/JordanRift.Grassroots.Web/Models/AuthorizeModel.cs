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

namespace JordanRift.Grassroots.Web.Models
{
    public class AuthorizeModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        
        [Display(Name = "Email Address")]
        [RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$",
            ErrorMessage = "Please enter a valid email address.")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        public string Url { get; set; }
        public string SenderName { get; set; }
        public string SenderEmail { get; set; }
        public string ActivationHash { get; set; }
    }
}