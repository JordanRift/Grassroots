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
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using JordanRift.Grassroots.Framework.Entities.Models;
using JordanRift.Grassroots.Framework.Entities.Validation;

namespace JordanRift.Grassroots.Framework.Entities
{
    [MetadataType(typeof(IPaymentValidation))]
    public class Payment : IPaymentValidation
    {
        public decimal Amount { get; set; }
        public PaymentType PaymentType { get; set; }
        public TransactionType TransactionType { get; set; }
        public int PaymentInterval { get; set; }

        public string AccountNumber { get; set; }
        public DateTime Expiration { get; set; }
        public string Cid { get; set; }

        public string RoutingNumber { get; set; }
        public string BankName { get; set; }
        public CheckType CheckType { get; set; }
        public string CheckNumber { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PrimaryPhone { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public string Comments { get; set; }
        public bool IsAnonymous { get; set; }
        public string DisplayName { get; set; }
        
        public bool IsValid
        {
            get { return Validate(); }
        }

        private List<string> errors;
        public IList<string> Errors { get { return errors; } }
        public DateTime SubscriptionStart { get; set; }
        public string Notes { get; set; }

        public Campaign Campaign { get; set; }
        public UserProfile Owner { get; set; }
        public Organization Organization { get; set; }

        public string GetFormattedDate()
        {
            return string.Format("{0}-{1}", Expiration.Year, Expiration.ToString("MM"));
        }

        private bool Validate()
        {
            bool result = true;
            errors = new List<string>();

            if (string.IsNullOrEmpty(AccountNumber))
            {
                result = false;
                errors.Add("Please enter an account number.");
            }

            if (PaymentType == PaymentType.CC)
            {
                if (Expiration < DateTime.Now)
                {
                    result = false;
                    errors.Add("Please enter a valid expiration date.");
                }
            }
            else
            {
                if (string.IsNullOrEmpty(RoutingNumber) ||
                    !Regex.IsMatch(RoutingNumber, @"^((0[0-9])|(1[0-2])|(2[1-9])|(3[0-2])|(6[1-9])|(7[0-2])|80)([0-9]{7})$"))
                {
                    result = false;
                    errors.Add("Please enter a valid bank routing number.");
                }

                if (string.IsNullOrEmpty(BankName))
                {
                    result = false;
                    errors.Add("Please enter your bank's name.");
                }

                if (string.IsNullOrEmpty(CheckNumber))
                {
                    result = false;
                    errors.Add("Please enter a valid check number.");
                }
            }

            return result;
        }

        public override string ToString()
        {
            return string.Format("Donation from {0} {1} of {2}, Email: {3}, Phone: {4}, Address: {5} {6}, {7} {8}, Notes: {9}",
                    FirstName, LastName, Amount, Email, PrimaryPhone, AddressLine1, City, State, ZipCode, Notes);
        }
    }
}
