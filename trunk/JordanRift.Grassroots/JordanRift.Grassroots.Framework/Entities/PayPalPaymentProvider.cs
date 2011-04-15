//
// Copyright © 2011 Jordan Rift, LLC - All Rights Reserved
//
// THIS WORK IS LICENSED UNDER A CREATIVE COMMONS ATTRIBUTION-NONCOMMERCIAL-
// SHAREALIKE 3.0 UNPORTED LICENSE:
// http://creativecommons.org/licenses/by-nc-sa/3.0/
//

using System;

namespace JordanRift.Grassroots.Framework.Entities
{
    public class PayPalPaymentProvider : IPaymentProvider
    {
        public PaymentResponse Process(Payment payment)
        {
            throw new NotImplementedException();
        }

        public PayPalPaymentProvider(string apiUrl, string loginID, string transactionKey)
        {
            
        }
    }
}
