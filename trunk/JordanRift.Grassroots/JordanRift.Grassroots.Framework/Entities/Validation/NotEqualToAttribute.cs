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
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class NotEqualToAttribute : ValidationAttribute, IClientValidatable
    {
        private const string DEFAULT_ERROR_MESSAGE = "{0} cannot be the same as {1}.";
 
        public string OtherProperty { get; private set; }
 
        public NotEqualToAttribute(string otherProperty) : base(DEFAULT_ERROR_MESSAGE)
        {
            if (string.IsNullOrEmpty(otherProperty))
            {
                throw new ArgumentNullException("otherProperty");
            }
 
            OtherProperty = otherProperty;
        }
 
        public override string FormatErrorMessage(string name)
        {
            return string.Format(ErrorMessageString, name, OtherProperty);
        }
 
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value != null)
            {
                var otherProperty = validationContext.ObjectInstance.GetType().GetProperty(OtherProperty); 
                var otherPropertyValue = otherProperty.GetValue(validationContext.ObjectInstance, null);
 
                if (value.Equals(otherPropertyValue))
                {
                    return new ValidationResult(FormatErrorMessage(validationContext.DisplayName));
                }
            }
 
            return ValidationResult.Success;
        }   

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            var clientValidationRule = new ModelClientValidationRule
                                           {
                                               ErrorMessage = FormatErrorMessage(metadata.GetDisplayName()),
                                               ValidationType = "notequalto"
                                           };

            clientValidationRule.ValidationParameters.Add("otherproperty", OtherProperty);

            return new[] { clientValidationRule };  
        }
    }
}
