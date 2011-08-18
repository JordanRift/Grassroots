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
    public class CampaignEmailBlastModel
    {
        public string UrlSlug { get; set; }
        public string Title { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        [Required]
        [Display(Name = "Email Addresses")]
        [RegularExpression(@"^((\s*[a-zA-Z0-9\._%-]+@[a-zA-Z0-9\.-]+\.[a-zA-Z]{2,4}\s*[,;]){1,4}?)?(\s*[a-zA-Z0-9\._%-]+@[a-zA-Z0-9\.-]+\.[a-zA-Z]{2,4})*$", 
            ErrorMessage = "Please enter between 1 and 5 valid email addresses")]
        public string EmailAddresses { get; set; }

        [Display(Name = "Email Message")]
        public string CustomMessage { get; set; }

        [Editable(false)]
        public string Url { get; set; }
    }
}