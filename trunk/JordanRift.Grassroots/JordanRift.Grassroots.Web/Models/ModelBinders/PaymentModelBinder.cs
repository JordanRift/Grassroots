//
// Copyright © 2011 Jordan Rift, LLC - All Rights Reserved
//
// THIS WORK IS LICENSED UNDER A CREATIVE COMMONS ATTRIBUTION-NONCOMMERCIAL-
// SHAREALIKE 3.0 UNPORTED LICENSE:
// http://creativecommons.org/licenses/by-nc-sa/3.0/
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
                        payment.Expiration = new DateTime(int.Parse(year), int.Parse(month), 1);
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
                        string value = controllerContext.HttpContext.Request ["TransactionType"];

                        if (value != null)
                        {
                            value = value.Split(new[] { ',' }) [0];
                        }

                        bool isRecurring = !string.IsNullOrEmpty(value) ? bool.Parse(value) : false;
                        payment.TransactionType = isRecurring ? TransactionType.Recurring : TransactionType.OneTime;
                        return;
                }
            }

            base.BindProperty(controllerContext, bindingContext, propertyDescriptor);
        }
    }
}