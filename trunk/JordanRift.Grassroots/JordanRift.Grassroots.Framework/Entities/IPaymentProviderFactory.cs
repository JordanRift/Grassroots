//
// Copyright © 2011 Jordan Rift, LLC - All Rights Reserved
//
// THIS WORK IS LICENSED UNDER A CREATIVE COMMONS ATTRIBUTION-NONCOMMERCIAL-
// SHAREALIKE 3.0 UNPORTED LICENSE:
// http://creativecommons.org/licenses/by-nc-sa/3.0/
//

namespace JordanRift.Grassroots.Framework.Entities
{
    public interface IPaymentProviderFactory
    {
        string ApiUrl { get; set; }
        string ApiKey { get; set; }
        string ApiSecret { get; set; }
        IPaymentProvider GetPaymentProvider(PaymentGatewayType paymentGatewayType);
    }
}
