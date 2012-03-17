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
using JordanRift.Grassroots.Tests.Helpers;
using NUnit.Framework;
using JordanRift.Grassroots.Framework.Entities;

namespace JordanRift.Grassroots.IntegrationTests.IntegrationTests.Models
{
    [TestFixture]
    public class AimPaymentTests
    {
        private const string TEST_API_URL = "https://test.authorize.net/gateway/transact.dll";
        private const string TEST_ARB_API_URL = "https://apitest.authorize.net/xml/v1/request.api";
        private const string TEST_LOGIN_ID = "2E3jsfH7L5F";
        private const string TEST_TRANSACTION_KEY = "979cxZC5g8dDRf9b";
        
        private IPaymentProvider provider;

        [SetUp]
        public void SetUp()
        {
            provider = new AuthorizePaymentProvider(TEST_API_URL, TEST_ARB_API_URL, TEST_LOGIN_ID, TEST_TRANSACTION_KEY);
        }

        [Test]
        public void Process_Should_Return_Status_Code()
        {
            Payment payment = new Payment();
            var result = provider.Process(payment);
            Assert.AreNotEqual(result.ResponseCode, 0);
        }

        [Test]
        public void Process_Should_Return_Reason_Code()
        {
            Payment payment = new Payment();
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
                                      Amount = TestHelpers.GetAmount()
                                  };

            EntityHelpers.AddCampaignInfo(payment);
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
                                      Amount = TestHelpers.GetAmount()
                                  };

            EntityHelpers.AddCampaignInfo(payment);
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
                                      Amount = TestHelpers.GetAmount()
                                  };

            EntityHelpers.AddCampaignInfo(payment);
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
                                      Amount = TestHelpers.GetAmount()
                                  };

            EntityHelpers.AddCampaignInfo(payment);
            var result = provider.Process(payment);
            Assert.AreNotEqual(PaymentResponseCode.Approved, result.ResponseCode);
        }
    }
}
