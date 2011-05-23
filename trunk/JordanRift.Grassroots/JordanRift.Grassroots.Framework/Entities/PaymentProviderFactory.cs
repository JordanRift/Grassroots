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
            ValidateSettings();

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

        private void ValidateSettings()
        {
            if (string.IsNullOrEmpty(ApiUrl))
            {
                throw new ArgumentException("ApiUrl must be set.");
            }

            if (string.IsNullOrEmpty(ApiKey))
            {
                throw new ArgumentException("ApiKey must be set.");
            }

            if (string.IsNullOrEmpty(ApiSecret))
            {
                throw new ArgumentException("ApiSecret must be set.");
            }
        }
    }
}
