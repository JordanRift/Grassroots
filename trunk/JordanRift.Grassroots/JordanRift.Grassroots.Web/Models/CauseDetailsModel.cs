﻿//
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
	public class CauseDetailsModel
	{
		[Editable( false )]
		public int CauseID { get; set; }

		[Editable( false )]
		public int CauseTemplateID { get; set; }

		[Required]
		public string Name { get; set; }

		public JordanRift.Grassroots.Framework.Entities.Models.Region Region { get; set; }

		public DateTime DateCompleted { get; set; }
	}
}