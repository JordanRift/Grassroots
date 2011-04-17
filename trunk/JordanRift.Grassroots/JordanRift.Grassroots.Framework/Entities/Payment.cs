//
// Copyright © 2011 Jordan Rift, LLC - All Rights Reserved
//
// THIS WORK IS LICENSED UNDER A CREATIVE COMMONS ATTRIBUTION-NONCOMMERCIAL-
// SHAREALIKE 3.0 UNPORTED LICENSE:
// http://creativecommons.org/licenses/by-nc-sa/3.0/
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
        public TransactionType TransactionType { get; set; }
        public PaymentType PaymentType { get; set; }

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
        public string Phone { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        
        public bool IsValid
        {
            get { return Validate(); }
        }

        private List<string> errors;
        public IList<string> Errors { get { return errors; } }

        /// <summary>
        /// Subscription payment interval in months (ex: monthly = 1, quarterly = 3, semi-anually = 2, etc)
        /// </summary>
        public int PaymentInterval 
        { 
            get { return paymentInvetval; }
            set { paymentInvetval = value; } 
        }

        private int paymentInvetval = 1;

        public DateTime SubscriptionStart { get; set; }

        public string Notes { get; set; }

        public Payment()
        {
        }

        /*public Payment(TransactionType transactionType)
        {
            TransactionType = transactionType;
        }

        public Payment(UserProfile userProfile)
        {
            if (userProfile != null)
            {
                FirstName = userProfile.FirstName;
                LastName = userProfile.LastName;
                Email = userProfile.Email;
                Phone = userProfile.PrimaryPhone;
                AddressLine1 = string.Format("{0} {1}", userProfile.AddressLine1, userProfile.AddressLine2);
                City = userProfile.City;
                State = userProfile.State;
                ZipCode = userProfile.ZipCode;
            }
        }*/

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
            return string.Format("{0} donation from {1} {2} of {3}, Email: {4}, Phone: {5}, Address: {6} {7}, {8} {9}, Notes: {10}",
                    TransactionType == TransactionType.OneTime ? "One Time" : "Monthly", FirstName, LastName,
                    Amount, Email, Phone, AddressLine1, City, State,
                    ZipCode, Notes);
        }
    }
}
