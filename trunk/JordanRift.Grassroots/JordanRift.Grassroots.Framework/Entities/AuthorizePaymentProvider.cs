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
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using JordanRift.Grassroots.Framework.Services;

namespace JordanRift.Grassroots.Framework.Entities
{
    public class AuthorizePaymentProvider : IPaymentProvider
    {
        private const string RESPONSE_DELIMITER = "|";

        private const string API_URL = "https://secure.authorize.net/gateway/transact.dll";
        private const string ARB_API_URL = "https://api.authorize.net/xml/v1/request.api";
        private const string LOGIN_ID = "878nm4TtBa2";
        private const string TRANSACTION_KEY = "6V3Yt29kV9Tx6pTU";

        public string ApiUrl { get; set; }
        public string ArbUrl { get; set; }
        public string LoginID { get; set; }
        public string TransactionKey { get; set; }

        public AuthorizePaymentProvider()
        {
            ApiUrl = API_URL;
            ArbUrl = ARB_API_URL;
            LoginID = LOGIN_ID;
            TransactionKey = TRANSACTION_KEY;
        }

        public AuthorizePaymentProvider(string apiUrl, string arbUrl, string loginID, string transactionKey)
        {
            ApiUrl = apiUrl;
            ArbUrl = arbUrl;
            LoginID = loginID;
            TransactionKey = transactionKey;
        }

        /// <summary>
        /// Process method to call from production. Will pass an "isTest" value of false to the Process(payment, isTest) overload.
        /// </summary>
        /// <param name="payment">Payment object to pass to Authorize.net gateway</param>
        /// <returns>PaymentResponse object containing Authorize.net's response information</returns>
        public PaymentResponse Process(Payment payment)
        {
            if (payment.TransactionType == TransactionType.Recurring)
            {
                return ProcessRecurring(payment);
            }

            return ProcessOneTime(payment);
        }

#region OneTime

        /// <summary>
        /// Provides a hook into Authorize.net's AIM (one time) card processing API
        /// </summary>
        /// <param name="payment">Payment object to pass to Authorize.net gateway</param>
        /// <returns>PaymentResponse object containing Authorize.net's response information</returns>
        private PaymentResponse ProcessOneTime(Payment payment)
        {
            var postValues = BuildAimRequest(payment);
            StringBuilder postString = new StringBuilder();

            foreach (var field in postValues)
            {
                if (postString.Length > 0)
                {
                    postString.Append("&");
                }

                postString.AppendFormat("{0}={1}", field.Key, field.Value);
            }

            // create an HttpWebRequest object to communicate with Authorize.net
            HttpWebRequest request = (HttpWebRequest) WebRequest.Create(ApiUrl);
            request.Method = "POST";
            request.ContentLength = postString.Length;
            request.ContentType = "application/x-www-form-urlencoded";

            using (StreamWriter myWriter = new StreamWriter(request.GetRequestStream()))
            {
                myWriter.Write(postString.ToString());
                myWriter.Close();
            }

            string responseString;
            HttpWebResponse response = (HttpWebResponse) request.GetResponse();
            
            using (StreamReader responseStream = new StreamReader(response.GetResponseStream()))
            {
                responseString = responseStream.ReadToEnd();
                responseStream.Close();
            }

            // NOTE: From the Authorize API documentation:
            // individual elements of the array could be accessed to read certain response
            // fields.  For example, response_array[0] would return the Response Code,
            // response_array[2] would return the Response Reason Code.
            // for a list of response fields, please review the AIM Implementation Guide

            var responseArray = responseString.Split('|');
            var responseCode = (PaymentResponseCode) int.Parse(responseArray [0]);
            var reasonCode = int.Parse(responseArray [2]);
            var reasonText = responseArray [3];

            return new PaymentResponse(responseCode, reasonCode, reasonText);
        }

        private Dictionary<string, string> BuildAimRequest(Payment payment)
        {
            var postValues = new Dictionary<string, string>
                                 {
                                     { "x_login", LoginID },
                                     { "x_tran_key", TransactionKey },
                                     { "x_delim_data", "TRUE" },
                                     { "x_delim_char", RESPONSE_DELIMITER },
                                     { "x_relay_response", "FALSE" },
                                     { "x_type", "AUTH_CAPTURE" },
                                     { "x_method", payment.PaymentType.ToString() },
                                     { "x_amount", payment.Amount.ToString(CultureInfo.InvariantCulture) },
                                     { "x_description", payment.ToString() },
                                     { "x_first_name", payment.FirstName },
                                     { "x_last_name", payment.LastName },
                                     { "x_address", payment.AddressLine1 },
                                     { "x_state", payment.State },
                                     { "x_zip", payment.ZipCode }
                                 };

            if (payment.PaymentType == PaymentType.CC)
            {
                postValues.Add("x_card_num", payment.AccountNumber);
                postValues.Add("x_exp_date", payment.GetFormattedDate());
                postValues.Add("x_card_code", payment.Cid);
            }
            else // ECheck
            {
                postValues.Add("x_bank_aba_code", payment.RoutingNumber);
                postValues.Add("x_bank_acct_num", payment.AccountNumber);
                postValues.Add("x_bank_acct_type", payment.CheckType.ToString());
                postValues.Add("x_bank_name", payment.BankName);
                postValues.Add("x_bank_acct_name", string.Format("{0} {1}", payment.FirstName, payment.LastName));
                postValues.Add("x_echeck_type", "WEB");
                postValues.Add("x_bank_check_number", payment.CheckNumber);
                postValues.Add("x_recurring_billing", "NO");
            }

            // NOTE: From the Authorize.net API documentation: 
            // Additional fields can be added here as outlined in the AIM integration
            // guide at: http://developer.authorize.net
            AppendCustomFields(postValues, payment);
            return postValues;
        }

        private void AppendCustomFields(IDictionary<string, string> postValues, Payment payment)
        {
            postValues.Add("x-donor-comments", payment.Comments.Substring(0, 255));

            if (payment.Campaign != null)
            {
                var campaign = payment.Campaign;
                postValues.Add("x-campaign-id", campaign.CampaignID.ToString(CultureInfo.InvariantCulture));
                postValues.Add("x-campaign-title", campaign.Title);
            }

            if (payment.Owner != null)
            {
                var userProfile = payment.Owner;
                postValues.Add("x-campaign-owner-name", userProfile.FullName);
                postValues.Add("x-campaign-owner-id", userProfile.UserProfileID.ToString(CultureInfo.InvariantCulture));
                postValues.Add("x-campaign-owner-email", userProfile.Email);
            }

            if (payment.Organization != null)
            {
                postValues.Add("x-organization-name", payment.Organization.Name);
            }
        }

#endregion

#region Recurring

        private PaymentResponse ProcessRecurring(Payment payment)
        {
            payment.SubscriptionStart = DateTime.Now.AddDays(1);
            var response = CreateSubscription(payment);
            PaymentResponse paymentResponse = new PaymentResponse(response.ResponseCode.ToUpper() == "OK" ? PaymentResponseCode.Approved : PaymentResponseCode.Error,
                -1, string.Join("|", response.Messages.ToArray()));
            return paymentResponse;
        }

        private SubscriptionResponse CreateSubscription(Payment payment)
        {
            var createRequest = new ARBCreateSubscriptionRequest();
            PopulateSubscription(createRequest, payment);

            object response = null;
            XmlDocument xmldoc;
            bool bResult = PostRequest(createRequest, out xmldoc);

            if (bResult)
            {
                ProcessXmlResponse(xmldoc, out response);
            }

            return ProcessResponse(response);
        }

        private void PopulateSubscription(ARBCreateSubscriptionRequest request, Payment payment)
        {
            ARBSubscriptionType sub = new ARBSubscriptionType();
            creditCardType creditCard = new creditCardType();
            bankAccountType bankAccount = new bankAccountType();

            sub.name = string.Format("{0} {1} Subscription", payment.FirstName, payment.LastName);

            if (payment.PaymentType == PaymentType.CC)
            {
                creditCard.cardNumber = payment.AccountNumber;
                creditCard.expirationDate = payment.GetFormattedDate();  // required format for API is YYYY-MM
                sub.payment = new paymentType { Item = creditCard };
            }
            else
            {
                bankAccount.accountNumber = payment.AccountNumber;
                bankAccount.accountTypeSpecified = true;
                bankAccount.bankName = payment.BankName;
                bankAccount.routingNumber = payment.RoutingNumber;
                bankAccount.accountType = GetAccountType(payment);
                bankAccount.nameOnAccount = payment.NameOnAccount;
                sub.payment = new paymentType { Item = bankAccount };
            }

            sub.billTo = new nameAndAddressType
            {
                firstName = payment.FirstName,
                lastName = payment.LastName
            };

            sub.paymentSchedule = new paymentScheduleType
            {
                startDate = payment.SubscriptionStart,
                startDateSpecified = true,
                totalOccurrences = 12,
                totalOccurrencesSpecified = true
            };

            sub.amount = payment.Amount;
            sub.amountSpecified = true;

            sub.paymentSchedule.interval = new paymentScheduleTypeInterval
            {
                length = 1,
                unit = ARBSubscriptionUnitEnum.months
            };

            sub.customer = new customerType { email = payment.Email };

            PopulateMerchantAuthentication(request);
            request.subscription = sub;
        }

        private bankAccountTypeEnum GetAccountType(Payment payment)
        {
            switch (payment.CheckType)
            {
                case CheckType.Savings: 
                    return bankAccountTypeEnum.savings;
                case CheckType.BusinessChecking: 
                    return bankAccountTypeEnum.businessChecking;
                case CheckType.Checking:
                default:
                    return bankAccountTypeEnum.checking;
            }
        }

        private void PopulateMerchantAuthentication(ANetApiRequest request)
        {
            request.merchantAuthentication = new merchantAuthenticationType
                                                 {
                                                     name = LoginID,
                                                     transactionKey = TransactionKey
                                                 };
        }

        private bool PostRequest(object apiRequest, out XmlDocument xmldoc)
        {
            bool bResult;

            xmldoc = null;

            try
            {
                HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(ArbUrl);
                webRequest.Method = "POST";
                webRequest.ContentType = "text/xml";
                webRequest.KeepAlive = true;

                XmlSerializer serializer = new XmlSerializer(apiRequest.GetType());
                XmlWriter writer = new XmlTextWriter(webRequest.GetRequestStream(), Encoding.UTF8);
                serializer.Serialize(writer, apiRequest);
                writer.Close();

                WebResponse webResponse = webRequest.GetResponse();
                xmldoc = new XmlDocument();
                xmldoc.Load(XmlReader.Create(webResponse.GetResponseStream()));

                bResult = true;
            }
            catch (Exception)
            {
                bResult = false;
            }

            return bResult;
        }

        private void ProcessXmlResponse(XmlDocument xmldoc, out object apiResponse)
        {
            apiResponse = null;

            try
            {
                XmlSerializer serializer;
                switch (xmldoc.DocumentElement.Name)
                {
                    case "ARBCreateSubscriptionResponse":
                        serializer = new XmlSerializer(typeof(ARBCreateSubscriptionResponse));
                        apiResponse = serializer.Deserialize(new StringReader(xmldoc.DocumentElement.OuterXml));
                        break;

                    case "ARBUpdateSubscriptionResponse":
                        serializer = new XmlSerializer(typeof(ARBUpdateSubscriptionResponse));
                        apiResponse = serializer.Deserialize(new StringReader(xmldoc.DocumentElement.OuterXml));
                        break;

                    case "ARBCancelSubscriptionResponse":
                        serializer = new XmlSerializer(typeof(ARBCancelSubscriptionResponse));
                        apiResponse = serializer.Deserialize(new StringReader(xmldoc.DocumentElement.OuterXml));
                        break;

                    case "ARBGetSubscriptionStatusResponse":
                        serializer = new XmlSerializer(typeof(ARBGetSubscriptionStatusResponse));
                        apiResponse = serializer.Deserialize(new StringReader(xmldoc.DocumentElement.OuterXml));
                        break;

                    case "ErrorResponse":
                        serializer = new XmlSerializer(typeof(ANetApiResponse));
                        apiResponse = serializer.Deserialize(new StringReader(xmldoc.DocumentElement.OuterXml));
                        break;
                }
            }
            catch (Exception)
            {
                apiResponse = null;
            }
        }


        private SubscriptionResponse ProcessResponse(object response)
        {
            ANetApiResponse baseResponse = (ANetApiResponse)response;
            return new SubscriptionResponse
                       {
                           ResponseCode = baseResponse.messages.resultCode.ToString(),
                           Messages = baseResponse.messages.message.Select(m => string.Format("{0}: {1}", m.code, m.text)).ToList()
                       };
        }

        #endregion
    }
}
