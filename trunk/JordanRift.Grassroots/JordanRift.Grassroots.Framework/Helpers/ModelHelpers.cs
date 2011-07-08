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

using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using JordanRift.Grassroots.Framework.Entities;
using JordanRift.Grassroots.Framework.Entities.Models;

namespace JordanRift.Grassroots.Framework.Helpers
{
    public static class ModelHelpers
    {
        public static List<string> GetOrgSettingKeys()
        {
            var keys = new List<string>();

            var fields = typeof(OrgSettingKeys).GetFields(BindingFlags.Public | BindingFlags.Static);

            foreach (var field in fields)
            {
                if (field.IsLiteral)
                {
                    keys.Add(field.GetValue(null).ToString());
                }
            }

            return keys;
        }

        /// <summary>
        /// Returns a string describing the nature of the collectin of causes passed in.
        /// </summary>
        /// <param name="causes">Collection of causes</param>
        /// <returns>string describing the type of causes</returns>
        public static string GetCausesLabelText(IEnumerable<Cause> causes)
        {
            string labelText = "Projects Completed";

            if (causes == null)
            {
                return null;
            }

            var causeTemplatesCount = (from c in causes
                                       select c.CauseTemplateID).Distinct().Count();

            if (causes.Any() && causeTemplatesCount == 1)
            {
                var causeTemplate = causes.First().CauseTemplate;

                if (causeTemplate != null)
                {
                    labelText = string.Format("{0} {1}", causeTemplate.GoalName, causeTemplate.ActionVerb);
                }
            }

            return labelText;
        }

    }
}
