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

namespace JordanRift.Grassroots.Web.Models
{
	public class CauseDetailsModel
	{
		public int CauseID { get; set; }
		public int CauseTemplateID { get; set; }
        public string ReferenceNumber { get; set; }
		public string Name { get; set; }
        public string Summary { get; set; }
        public string ImagePath { get; set; }
		public string Region { get; set; }
        public string HoursServed { get; set; }
		public DateTime DateCompleted { get; set; }
        public string BeforeImagePath { get; set; }
        public string AfterImagePath { get; set; }
        public string VideoEmbedHtml { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public List<RecipientDetailsModel> Recipients { get; set; }
	}
}