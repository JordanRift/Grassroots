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
        //[Display(Name = "Give your page a name")]
        [Display(Name = "Give your ride a name")]
        public string Title { get; set; }
        
        [Required]
        [Display(Name = "Friendly URL (letters & numbers only)")]
        [RegularExpression(@"^[a-zA-Z0-9]+$", ErrorMessage = "Please enter only letters and numbers (no spaces or special characters).")]
        [StringLength(30, MinimumLength = 5, ErrorMessage = "Campaign URL must be between 5 and 30 characters in length.")]
        [Remote("CheckUrlSlug", "Validation", ErrorMessage = "That friendly URL is already in use. Please try another one.")]
        public string UrlSlug { get; set; }

        public int CauseTemplateID { get; set; }
        public int CampaignType { get; set; }
        public bool AmountIsConfigurable { get; set; }
        public decimal DefaultAmount { get; set; }
        public string GoalName { get; set; }
        
        [Display(Name = "Amount (between $300 & $30,000)")]
        [DataType(DataType.Currency)]
        [RegularExpression(@"^\d+(\.\d{2})?$", ErrorMessage = "Please enter a valid amount.")]
        [Range(300.00, 30000.00, ErrorMessage = "Amount must be between $300.00 and $30,000.00.")]
        public string AmountString { get; set; }
    }
}