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
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using JordanRift.Grassroots.Framework.Entities.Validation;

namespace JordanRift.Grassroots.Web.Models
{
    public class CampaignDetailsModel
    {
        //
        // Campaign Details
        //

        [Editable(false)]
        public int CampaignID { get; set; }

        [Editable(false)]
        public int CauseTemplateID { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }
        
        [Required]
        [DataType(DataType.Currency)]
        [RegularExpression(@"^\d+(\.\d{2})?$", ErrorMessage = "Please enter a valid amount.")]
        [Range(300.00, 30000.00, ErrorMessage = "Amount must be between $300.00 and $30,000.00.")]
        public decimal GoalAmount { get; set; }
        
        [Editable(false)]
        public string UrlSlug { get; set; }
        public string ImagePath { get; set; }

        [UIHint("DateTime")]
        [Display(Name = "Start Date")]
        public DateTime StartDate { get; set; }
        
        [Required]
        [GreaterThanDate("StartDate")]
        [UIHint("DateTime")]
        [Display(Name = "End Date")]
        public DateTime EndDate { get; set; }

        //
        // Donations
        //
        
        public List<DonationDetailsModel> Donations { get; set; }

        //
        // User Profile Details
        //

        public string FirstName { get; set; }
        public string LastName { get; set; }
		public string UserImagePath { get; set; }

        //
        // Cause/CauseTemplate Details
        //

        public string CauseTemplateName { get; set; }
        public string CauseTempalteImagePath { get; set; }
        public string CauseTemplateBeforeImagePath { get; set; }
        public string CauseTemplateAfterImagePath { get; set; }
        public bool AmountIsConfigurable { get; set; }
        public bool TimespanIsConfigurable { get; set; }
        public string VideoEmbedHtml { get; set; }
		public bool IsActive { get; set; }
		public string InstructionsOpenHtml { get; set; }
		public string InstructionsClosedHtml { get; set; }
        public string CauseTemplateStatisticsHtml { get; set; }
        public string CallToAction { get; set; }

        //
        // Security
        //

        public bool CurrentUserIsOwner { get; set; }
    }
}