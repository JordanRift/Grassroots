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

namespace JordanRift.Grassroots.Framework.Entities.Validation
{
    public interface ICauseTemplateValidation
    {
        [Required(ErrorMessage = "Action Verb is required.")]
        [Display(Name = "Action Verb")]
        string ActionVerb { get; set; }

        [Required(ErrorMessage = "Goal Name is required.")]
        [Display(Name = "Goal Name")]
        string GoalName { get; set; }

        [Required]
        string CallToAction { get; set; }

        [Display(Name = "Is Amount configurable by the user?")]
        bool AmountIsConfigurable { get; set; }

        [Required(ErrorMessage = "Default Amount is required.")]
        [Display(Name = "Default Amount")]
        decimal DefaultAmount { get; set; }

        [Display(Name = "Is Campaign Length configurable by the user?")]
        bool TimespanIsConfigurable { get; set; }

        [Required(ErrorMessage = "Default Campaign Length is required.")]
        [Display(Name = "Default Campaign Length")]
        int DefaultTimespanInDays { get; set; }

        [Required(ErrorMessage = "Name is required.")]
        string Name { get; set; }

        [Required(ErrorMessage = "Summary is required.")]
        string Summary { get; set; }

        [Required(ErrorMessage = "Description is required.")]
        [Display(Name = "Description")]
        string DescriptionHtml { get; set; }

        [Display(Name = "Video Embed Code")]
        string VideoEmbedHtml { get; set; }

        [Display(Name = "Image")]
        string ImagePath { get; set; }

        bool Active { get; set; }
    }
}
