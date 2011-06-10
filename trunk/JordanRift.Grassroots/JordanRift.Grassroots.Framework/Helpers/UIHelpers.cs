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
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using JordanRift.Grassroots.Framework.Data;
using JordanRift.Grassroots.Framework.Entities;
using JordanRift.Grassroots.Framework.Entities.Models;

namespace JordanRift.Grassroots.Framework.Helpers
{
	public static class UIHelpers
	{
	    public static IDictionary<string, string> CauseTemplateDictionary;

	    public static SelectList CauseTemplateSelectList
	    {
	        get
	        {
                if (CauseTemplateDictionary == null)
                {
                    CauseTemplateDictionary = new Dictionary<string, string>();
                    var repository = RepositoryFactory.GetRepository<IOrganizationRepository>();
                    var organization = repository.GetDefaultOrganization(readOnly: true);

                    foreach (var ct in organization.CauseTemplates)
                    {
                        if (ct.Active)
                        {
                            CauseTemplateDictionary.Add(
                                new KeyValuePair<string, string>(ct.CauseTemplateID.ToString(), ct.Name));
                        }
                    }
                }

                return new SelectList(CauseTemplateDictionary, "Key", "Value");
	        }
	    }

		public static readonly IDictionary<string, string> ExpMonthDictionary = new Dictionary<string, string> {
			{"01", "1"},
			{"02", "2"},
			{"03", "3"},
			{"04", "4"},
			{"05", "5"},
			{"06", "6"},
			{"07", "7"},
			{"08", "8"},
			{"09", "9"},
			{"10", "10"},
			{"11", "11"},
			{"12", "12"}
		};

		public static SelectList ExpMonthSelectList
		{
			get { return new SelectList(ExpMonthDictionary, "Value", "Key"); }
		}

		public static IDictionary<string, string> ExpYearDictionary;

		public static SelectList ExpYearSelectList
		{
			get
			{
				if (ExpYearDictionary == null)
				{
					ExpYearDictionary = new Dictionary<string, string>();

					for (int i = 0; i < 5; i++)
					{
						string year = (DateTime.Now.Year + i).ToString();
						ExpYearDictionary.Add(year, year);
					}
				}

				return new SelectList(ExpYearDictionary, "Value", "Key");
			}
		}

		public static readonly IDictionary<string, string> CheckTypeDictionary = new Dictionary<string, string> {
			{"Checking", "Checking"},
			{"Savings", "Savings"},
			{"Business Checking", "BusinessChecking"}
		};

		public static SelectList CheckTypeSelectList
		{
			get { return new SelectList(CheckTypeDictionary, "Value", "Key"); }
		}

		public static readonly IDictionary<string, string> PaymentTypeDictionary = new Dictionary<string, string> {
			{"Visa", "CC"},
			{"Master Card", "CC"},
			{"American Express", "CC"},
			{"Discover", "CC"},
			{"Electronic Check", "ECheck"}
		};

		public static SelectList PaymentTypeSelectList
		{
			get { return new SelectList(PaymentTypeDictionary, "Value", "Key"); }
		}

		public static readonly IDictionary<string, string> GenderDictionary = new Dictionary<string, string> {
			{"Male", "male"},
			{"Female", "female"}
		};

		public static SelectList GenderSelectList
		{
			get { return new SelectList(GenderDictionary, "Value", "Key"); }
		}

		public static readonly IDictionary<string, string> MaritalStatusDictionary = new Dictionary<string, string> {
			{"Single", "single"},
			{"Married", "married"},
			{"Divorced", "divorced"}
		};

		public static SelectList MaritalStatusSelectList
		{
			get { return new SelectList(MaritalStatusDictionary, "Value", "Key"); }
		}

		public static readonly IDictionary<string, string> StateDictionary = new Dictionary<string, string> {
			{"Alabama", "AL"},
			{"Alaska", "AK"},
			{"American Samoa", "AS"},
			{"Arizona", "AZ"},
			{"Arkansas", "AR"},
			{"California ", "CA"},
			{"Colorado ", "CO"},
			{"Connecticut", "CT"},
			{"Deleware", "DE"},
			{"District of Columbia", "DC"},
			{"Federated States of Micronesia", "FM"},
			{"Florida", "FL"},
			{"Georgia", "GA"},
			{"Guam ", "GU"},
			{"Hawaii", "HI"},
			{"Idaho", "ID"},
			{"Illinois", "IL"},
			{"Indiana", "IN"},
			{"Iowa", "IA"},
			{"Kansas", "KS"},
			{"Kentucky", "KY"},
			{"Louisiana", "LA"},
			{"Maine", "ME"},
			{"Marshall Islands", "MH"},
			{"Maryland", "MD"},
			{"Massachusetts", "MA"},
			{"Michigan", "MI"},
			{"Minnesota", "MN"},
			{"Mississippi", "MS"},
			{"Missouri", "MO"},
			{"Montana", "MT"},
			{"Nebraska", "NE"},
			{"Nevada", "NV"},
			{"New Hampshire", "NH"},
			{"New Jersey", "NJ"},
			{"New Mexico", "NM"},
			{"New York", "NY"},
			{"North Carolina", "NC"},
			{"North Dakota", "ND"},
			{"Northern Mariana Islands", "MP"},
			{"Ohio", "OH"},
			{"Oklahoma", "OK"},
			{"Oregon", "OR"},
			{"Palau", "PW"},
			{"Pennsylvania", "PA"},
			{"Puerto Rico", "PR"},
			{"Rhode Island", "RI"},
			{"South Carolina", "SC"},
			{"South Dakota", "SD"},
			{"Tennessee", "TN"},
			{"Texas", "TX"},
			{"Utah", "UT"},
			{"Vermont", "VT"},
			{"Virgin Islands", "VI"},
			{"Virginia", "VA"},
			{"Washington", "WA"},
			{"West Virginia", "WV"},
			{"Wisconsin", "WI"},
			{"Wyoming", "WY"}
		};

		public static SelectList StateSelectList
		{
			get { return new SelectList(StateDictionary, "Value", "Key"); }
		}

		#region Extension Methods

		/// <summary>
		/// Cool little method from http://guyellisrocks.com/coding/asp-net-mvc-dropdownlist-from-enum/
		/// Converts an enum type to an IDictionary&lt;int,string&gt;
		/// Although this will appear as an extension method on all type objects
		/// it will throw an exception on all types except enum
		/// </summary>
		/// <param name="type"></param>
		/// <returns></returns>
		public static IDictionary<int, string> EnumToDictionary( this Type type )
		{
			if ( ! type.IsEnum )
			{
				throw new InvalidCastException( "The EnumToDictionary() extension method can only be used on types of enum. All other types will throw this exception." );
			}

			// The Enum.GetValues() function returns an array of objects which are actually ints,
			// that's why we cast them to ints before calling the ToDictionary function.
			// The ToDictionary() extension method takes two lambda expressions each of which
			// returns the type that corresponds to the dictionary's types. Because we've cast
			// the array to ints the first param in the ToDictionary is used as is. The second
			// param gets the name of the Enum.
			return Enum.GetValues( type ).Cast<Int32>().ToDictionary( a => a, a => Enum.GetName( type, a ) );
		}

		public static List<int> Cast(this Array collection)
		{
			var myList = new List<int>();
			// code to cast your collection to something and add it to the list
			return myList;
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

		#endregion
	}
}
