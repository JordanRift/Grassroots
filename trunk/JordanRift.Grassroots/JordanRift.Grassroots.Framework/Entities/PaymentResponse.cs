//
// Copyright © 2011 Jordan Rift, LLC - All Rights Reserved
//
// THIS WORK IS LICENSED UNDER A CREATIVE COMMONS ATTRIBUTION-NONCOMMERCIAL-
// SHAREALIKE 3.0 UNPORTED LICENSE:
// http://creativecommons.org/licenses/by-nc-sa/3.0/
//

namespace JordanRift.Grassroots.Framework.Entities
{
    public class PaymentResponse
    {
        public PaymentResponseCode ResponseCode { get; set; }
        public int ReasonCode { get; set; }
        public string ReasonText { get; set; }

        public PaymentResponse(PaymentResponseCode responseCode, int reasonCode, string reasonText)
        {
            ResponseCode = responseCode;
            ReasonCode = reasonCode;
            ReasonText = reasonText;
        }
    }
}
