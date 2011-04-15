//
// Copyright © 2011 Jordan Rift, LLC - All Rights Reserved
//
// THIS WORK IS LICENSED UNDER A CREATIVE COMMONS ATTRIBUTION-NONCOMMERCIAL-
// SHAREALIKE 3.0 UNPORTED LICENSE:
// http://creativecommons.org/licenses/by-nc-sa/3.0/
//

using System;
using System.Web;
using Elmah;
using JordanRift.Grassroots.Framework.Entities;
using ApplicationException = System.ApplicationException;

namespace JordanRift.Grassroots.Framework.Helpers
{
    public static class Logger
    {
        public static void LogError(Exception exception)
        {
            var context = HttpContext.Current;
            var log = ErrorLog.GetDefault(context);
            log.Log(new Error(exception, context));
        }

        public static void LogError(Payment payment, PaymentResponse response)
        {
            var context = HttpContext.Current;
            var log = ErrorLog.GetDefault(context);
            var ex = new ApplicationException(string.Format("{0} {1}'s donation of {2:C} was unable to be processed. Error Code: {3}. Description: {4}",
                payment.FirstName, payment.LastName, payment.Amount, response.ResponseCode, response.ReasonText));
            log.Log(new Error(ex, context));
        }
    }
}
