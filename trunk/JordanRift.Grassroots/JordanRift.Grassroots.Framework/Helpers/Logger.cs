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
