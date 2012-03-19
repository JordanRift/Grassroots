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
using System.ComponentModel;
using System.Web.Mvc;
using JordanRift.Grassroots.Framework.Entities;
using JordanRift.Grassroots.Framework.Helpers;

namespace JordanRift.Grassroots.Web.Models.ModelBinders
{
    public class PaymentModelBinder : DefaultModelBinder
    {
        protected override void BindProperty(ControllerContext controllerContext, ModelBindingContext bindingContext, PropertyDescriptor propertyDescriptor)
        {
            var payment = bindingContext.Model as Payment;

            if (payment != null)
            {
                switch (propertyDescriptor.Name)
                {
                    case "Expiration":
                        var month = !string.IsNullOrEmpty(controllerContext.HttpContext.Request["Expiration.Month"]) ?
                            controllerContext.HttpContext.Request["Expiration.Month"] : "1";
                        var year = !string.IsNullOrEmpty(controllerContext.HttpContext.Request["Expiration.Year"]) ? 
                            controllerContext.HttpContext.Request["Expiration.Year"] : "1900";
                        var dateString = string.Format("{0}/{1}/1", year, month);
                        DateTime result;
                        payment.Expiration = DateTime.TryParse(dateString, out result) ? result : new DateTime(1900, 1, 1);
                        return;
                    case "PaymentType":
                        var paymentType = controllerContext.HttpContext.Request["PaymentType"].ToEnum<PaymentType>();
                        payment.PaymentType = paymentType;
                        return;
                    case "CheckType":
                        var checkType = controllerContext.HttpContext.Request["CheckType"].ToEnum<CheckType>();
                        payment.CheckType = checkType;
                        return;
                    case "TransactionType":
                         var value = controllerContext.HttpContext.Request["TransactionType"];

                        if (value != null)
                        {
                            value = value.Split(new[] { ',' })[0];
                        }

                        bool isRecurring = !string.IsNullOrEmpty(value) && bool.Parse(value);
                        payment.TransactionType = isRecurring ? TransactionType.Recurring : TransactionType.OneTime;
                        return;
                }
            }

            base.BindProperty(controllerContext, bindingContext, propertyDescriptor);
        }
    }
}