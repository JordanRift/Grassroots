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

namespace JordanRift.Grassroots.Framework.Entities.Validation
{
    public class GreaterThanDateAttribute : ValidationAttribute, IClientValidatable
    {
        protected string OtherPropertyName { get; set; }

        public GreaterThanDateAttribute(string otherPropertyName) : base("{0} must be greater than {1}")
        {
            OtherPropertyName = otherPropertyName;
        }

        public override string FormatErrorMessage(string name)
        {
            return string.Format(ErrorMessageString, name, OtherPropertyName);
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var firstValue = value as IComparable;
            var secondValue = GetSecondComparable(validationContext);

            if (firstValue != null && secondValue != null)
            {
                if (firstValue.CompareTo(secondValue) < 1)
                {
                    return new ValidationResult(FormatErrorMessage(validationContext.DisplayName));
                }
            }

            return ValidationResult.Success;
        }

        protected IComparable GetSecondComparable(ValidationContext validationContext)
        {
            var propertyInfo = validationContext.ObjectType.GetProperty(OtherPropertyName);

            if (propertyInfo != null)
            {
                var secondValue = propertyInfo.GetValue(validationContext.ObjectInstance, null);
                return secondValue as IComparable;
            }

            return null;
        }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            var clientValidationRule = new ModelClientValidationRule
                           {
                               ErrorMessage = FormatErrorMessage(metadata.GetDisplayName()),
                               ValidationType = "greater"
                           };

            clientValidationRule.ValidationParameters.Add("otherproperty", OtherPropertyName);
            
            return new[] { clientValidationRule };
        }
    }
}
