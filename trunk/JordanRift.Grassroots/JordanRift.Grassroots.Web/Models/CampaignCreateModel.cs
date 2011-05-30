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
    public class CampaignCreateModel
    {
        [Required]
        [Display(Name = "Campaign Name")]
        public string Title { get; set; }
        
        [Required]
        [Display(Name = "Friendly URL")]
        [RegularExpression(@"^[a-zA-Z0-9]+$", ErrorMessage = "Please enter only alpha-numeric characters.")]
        [Remote("CheckUrlSlug", "Validation", ErrorMessage = "That friendly URL is already in use. Please try another one.")]
        public string UrlSlug { get; set; }

        public int CauseTemplateID { get; set; }
        public int CampaignType { get; set; }
        public bool AmountIsConfigurable { get; set; }
        public decimal DefaultAmount { get; set; }
        public string GoalName { get; set; }
        
        [DataType(DataType.Currency)]
        public decimal Amount { get; set; }
    }
}