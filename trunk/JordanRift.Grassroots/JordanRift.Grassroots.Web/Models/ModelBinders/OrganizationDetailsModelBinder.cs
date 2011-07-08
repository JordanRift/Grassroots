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
using System.ComponentModel;
using System.Web.Helpers;
using System.Web.Mvc;
using JordanRift.Grassroots.Framework.Helpers;

namespace JordanRift.Grassroots.Web.Models.ModelBinders
{
    public class OrganizationDetailsModelBinder : DefaultModelBinder
    {
        protected override void BindProperty(ControllerContext controllerContext, ModelBindingContext bindingContext, PropertyDescriptor propertyDescriptor)
        {
            var model = bindingContext.Model as OrganizationDetailsModel;
            controllerContext.Controller.ValidateRequest = false;

            if (model != null)
            {
                switch (propertyDescriptor.Name)
                {
                    case "OrganizationSettings":
                        BindSettings(model, controllerContext);
                        break;
                    default:
                        break;
                }
            }

            base.BindProperty(controllerContext, bindingContext, propertyDescriptor);
        }

        private void BindSettings(OrganizationDetailsModel model, ControllerContext controllerContext)
        {
            var request = controllerContext.HttpContext.Request.Unvalidated();
            var keys = ModelHelpers.GetOrgSettingKeys();
            model.OrganizationSettings = new List<OrganizationSettingModel>();

            foreach (var key in keys)
            {
                var formKey = string.Format("setting_{0}", key);
                var setting = new OrganizationSettingModel
                                  {
                                      Name = key,
                                      Value = !string.IsNullOrEmpty(request[formKey]) ? request[formKey] : string.Empty,
                                      DataType = 1
                                  };

                model.OrganizationSettings.Add(setting);
            }
        }
    }
}