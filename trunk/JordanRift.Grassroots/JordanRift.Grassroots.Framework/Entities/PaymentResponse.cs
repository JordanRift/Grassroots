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

using System.Collections.Generic;

namespace JordanRift.Grassroots.Framework.Entities
{
    public class PaymentResponse
    {
        public PaymentResponseCode ResponseCode { get; set; }
        public int ReasonCode { get; set; }
        public string ReasonText { get; set; }
        //public IEnumerable<string> Messages { get; set; }

        public PaymentResponse() { }

        public PaymentResponse(PaymentResponseCode responseCode, int reasonCode, string reasonText)
        {
            ResponseCode = responseCode;
            ReasonCode = reasonCode;
            ReasonText = reasonText;
            //Messages = messages;
        }
    }

    public class SubscriptionResponse
    {
        public string ResponseCode { get; set; }
        public List<string> Messages { get; set; }
    }
}
