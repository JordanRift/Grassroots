﻿//
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
        public string ApiUrl { get; set; }
        public string ApiKey { get; set; }
        public string ApiSecret { get; set; }

        public PaymentProviderFactory()
        {
            
        }

        public PaymentProviderFactory(string apiUrl, string apiKey, string apiSecret)
        {
            ApiUrl = apiUrl;
            ApiKey = apiKey;
            ApiSecret = apiSecret;
        }

        public IPaymentProvider GetPaymentProvider(PaymentGatewayType paymentGatewayType)
        {
            switch (paymentGatewayType)
            {
                case PaymentGatewayType.Authorize:
                    return new AuthorizePaymentProvider(ApiUrl, ApiKey, ApiSecret);
                case PaymentGatewayType.PayPal:
                    return new PayPalPaymentProvider(ApiUrl, ApiKey, ApiSecret);
                default:
                    return null;
            }
        }
    }
}
