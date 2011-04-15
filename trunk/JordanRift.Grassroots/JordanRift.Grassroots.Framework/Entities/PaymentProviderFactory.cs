//
// Copyright © 2011 Jordan Rift, LLC - All Rights Reserved
//
// THIS WORK IS LICENSED UNDER A CREATIVE COMMONS ATTRIBUTION-NONCOMMERCIAL-
// SHAREALIKE 3.0 UNPORTED LICENSE:
// http://creativecommons.org/licenses/by-nc-sa/3.0/
//

namespace JordanRift.Grassroots.Framework.Entities
{
    public class PaymentProviderFactory : IPaymentProviderFactory
    {
        private readonly string apiUrl;
        private readonly string apiKey;
        private readonly string apiSecret;

        public PaymentProviderFactory(string apiUrl, string apiKey, string apiSecret)
        {
            this.apiUrl = apiUrl;
            this.apiKey = apiKey;
            this.apiSecret = apiSecret;
        }

        public IPaymentProvider GetPaymentProvider(PaymentGatewayType paymentGatewayType)
        {
            switch (paymentGatewayType)
            {
                case PaymentGatewayType.Authorize:
                    return new AuthorizePaymentProvider(apiUrl, apiKey, apiSecret);
                case PaymentGatewayType.PayPal:
                    return new PayPalPaymentProvider(apiUrl, apiKey, apiSecret);
                default:
                    return null;
            }
        }
    }
}
