//
// Copyright © 2011 Jordan Rift, LLC - All Rights Reserved
//
// THIS WORK IS LICENSED UNDER A CREATIVE COMMONS ATTRIBUTION-NONCOMMERCIAL-
// SHAREALIKE 3.0 UNPORTED LICENSE:
// http://creativecommons.org/licenses/by-nc-sa/3.0/
//

using System;
using JordanRift.Grassroots.Tests.Helpers;
using NUnit.Framework;
using JordanRift.Grassroots.Framework.Entities;

namespace JordanRift.Grassroots.Tests.IntegrationTests.Models
{
    [TestFixture]
    public class AimPaymentTests
    {
        private const string TEST_API_URL = "https://test.authorize.net/gateway/transact.dll";
        private const string TEST_LOGIN_ID = "2E3jsfH7L5F";
        private const string TEST_TRANSACTION_KEY = "979cxZC5g8dDRf9b";
        
        private IPaymentProvider provider;

        [SetUp]
        public void SetUp()
        {
            provider = new AuthorizePaymentProvider(TEST_API_URL, TEST_LOGIN_ID, TEST_TRANSACTION_KEY);
        }

        [Test]
        public void Process_Should_Return_Status_Code()
        {
            Payment payment = new Payment { TransactionType = TransactionType.OneTime };
            var result = provider.Process(payment);
            Assert.AreNotEqual(result.ResponseCode, 0);
        }

        [Test]
        public void Process_Should_Return_Reason_Code()
        {
            Payment payment = new Payment { TransactionType = TransactionType.OneTime };
            var result = provider.Process(payment);
            Assert.Greater(result.ReasonCode, 0);
        }

        [Test]
        public void Process_Should_Return_Approved_When_Valid_Payment_Is_Given()
        {
            Payment payment = new Payment
                                  {
                                      FirstName = "Johnny",
                                      LastName = "Appleseed",
                                      AddressLine1 = "123 My St",
                                      City = "New York",
                                      State = "NY",
                                      ZipCode = "12345",
                                      PaymentType = PaymentType.CC,
                                      AccountNumber = "4111111111111111",
                                      Expiration = new DateTime(2015, 1, 1),
                                      Cid = "123",
                                      Amount = TestHelpers.GetAmount(),
                                      TransactionType = TransactionType.OneTime
                                  };

            var result = provider.Process(payment);
            Assert.AreEqual(PaymentResponseCode.Approved, result.ResponseCode);
        }

        [Test]
        public void Process_Should_Return_Error_When_Invalid_Payment_Is_Given()
        {
            Payment payment = new Payment
                                  {
                                      AccountNumber = "42",
                                      Expiration = new DateTime(2015, 1, 1),
                                      Amount = TestHelpers.GetAmount(),
                                      TransactionType = TransactionType.OneTime
                                  };

            var result = provider.Process(payment);
            Assert.AreNotEqual(PaymentResponseCode.Approved, result.ResponseCode);
        }

        [Test]
        public void Process_Should_Return_Approved_When_Valid_ECheck_Is_Given()
        {
            Payment payment = new Payment
                                  {
                                      FirstName = "Johnny",
                                      LastName = "Appleseed",
                                      AddressLine1 = "123 My St",
                                      City = "New York",
                                      State = "NY",
                                      ZipCode = "12345",
                                      AccountNumber = "4111111111111111",
                                      RoutingNumber = "122105278",
                                      PaymentType = PaymentType.ECheck,
                                      CheckNumber = "1234",
                                      Amount = TestHelpers.GetAmount(),
                                      TransactionType = TransactionType.OneTime
                                  };

            var result = provider.Process(payment);
            Assert.AreEqual(PaymentResponseCode.Approved, result.ResponseCode);
        }

        [Test]
        public void Process_Should_Return_Error_When_Invalid_ECheck_Is_Given()
        {
            Payment payment = new Payment
                                  {
                                      FirstName = "Johnny",
                                      LastName = "Appleseed",
                                      AddressLine1 = "123 My St",
                                      City = "New York",
                                      State = "NY",
                                      ZipCode = "12345",
                                      AccountNumber = string.Empty,
                                      RoutingNumber = "122105278",
                                      PaymentType = PaymentType.ECheck,
                                      CheckNumber = "1234",
                                      Amount = TestHelpers.GetAmount(),
                                      TransactionType = TransactionType.OneTime
                                  };

            var result = provider.Process(payment);
            Assert.AreNotEqual(PaymentResponseCode.Approved, result.ResponseCode);
        }
    }
}
