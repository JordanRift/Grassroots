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

using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace JordanRift.Grassroots.Framework.Entities
{
    public class AuthorizePaymentProvider : IPaymentProvider
    {
        private const string RESPONSE_DELIMITER = "|";

        private const string API_URL = "https://secure.authorize.net/gateway/transact.dll";
        private const string LOGIN_ID = "878nm4TtBa2";
        private const string TRANSACTION_KEY = "6V3Yt29kV9Tx6pTU";

        public string ApiUrl { get; set; }
        public string LoginID { get; set; }
        public string TransactionKey { get; set; }

        public AuthorizePaymentProvider()
        {
            ApiUrl = API_URL;
            LoginID = LOGIN_ID;
            TransactionKey = TRANSACTION_KEY;
        }

        public AuthorizePaymentProvider(string apiUrl, string loginID, string transactionKey)
        {
            ApiUrl = apiUrl;
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
            return ProcessOneTime(payment);
        }

        //public PaymentResponse Process(Payment payment, IAuthorizeArbService proxy)
        //{
        //    if (payment.TransactionType == TransactionType.OneTime)
        //    {
        //        return Process(payment);
        //    }

        //    return ProcessRecurring(payment, proxy);
        //}

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
            //string loginID = isTest ? TEST_LOGIN_ID : LOGIN_ID;
            //string transactionKey = isTest ? TEST_TRANSACTION_KEY : TRANSACTION_KEY;

            var postValues = new Dictionary<string, string>
                                 {
                                     { "x_login", LoginID },
                                     { "x_tran_key", TransactionKey },
                                     { "x_delim_data", "TRUE" },
                                     { "x_delim_char", RESPONSE_DELIMITER },
                                     { "x_relay_response", "FALSE" },
                                     { "x_type", "AUTH_CAPTURE" },
                                     { "x_method", payment.PaymentType.ToString() },
                                     { "x_amount", payment.Amount.ToString() },
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

            return postValues;
        }
    }
}
