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
using JordanRift.Grassroots.Framework.Entities;
using JordanRift.Grassroots.Framework.Entities.Models;

namespace JordanRift.Grassroots.Tests.Helpers
{
    public static class EntityHelpers
    {
		public static CauseTemplate GetValidCauseTemplate()
		{
		    return new CauseTemplate
		               {
		                   //Organization = GetValidOrganization(),
		                   Name = "House Cause",
		                   ActionVerb = "build",
		                   GoalName = "house",
		                   Active = true,
		                   AmountIsConfigurable = false,
		                   DefaultAmount = (decimal) 4000.00,
		                   TimespanIsConfigurable = false,
		                   DefaultTimespanInDays = 90,
		                   Summary = "We are going to build houses...",
		                   VideoEmbedHtml = "",
		                   DescriptionHtml = "<h1>Houses</h1>Everyone wants a house, right? ...",
		                   ImagePath = "content/images/causetemplate/template.jpg"
		               };
		}

        public static Campaign GetValidCampaign()
        {
            return new Campaign
                       {
                           //UserProfile = GetValidUserProfile(),
                           GoalAmount = (decimal) 4000.00,
                           StartDate = DateTime.Now,
                           EndDate = DateTime.Now.AddDays(90),
                           Title = "My Campaign",
                           Description = "This is my super awesome campaign",
                           //Organization = GetValidOrganization(),
                           UrlSlug = "mycampaign",
                           ImagePath = "content/images/campaigns/mycampaign.jpg"
                       };
        }

		public static CampaignDonor GetValidCampaignDonor()
		{
		    return new CampaignDonor
		               {
		                   UserProfileID = 1,
		                   FirstName = "Joe",
		                   LastName = "Sandwitches",
		                   Comments = "Thanks for letting me help!",
		                   Amount = (decimal) 10.00,
		                   Email = "joe@yahoo.com",
		                   //Campaign = GetValidCampaign()
		               };
		}

        public static Organization GetValidOrganization()
        {
            var org = new Organization
                          {
                              Name = "My Organization",
                              Tagline = "We are so cool",
                              SummaryHtml = "SummaryHtml",
                              DescriptionHtml = "<h1>Description</h1>",
                              ContactPhone = "(480) 321-1234",
                              ContactEmail = "joe@yahoo.com",
                              YtdGoal = 1000.00m,
                              FiscalYearStartDay = 1,
                              FiscalYearStartMonth = 1,
                              PaymentGatewayType = (int) PaymentGatewayType.Authorize,
                              PaymentGatewayApiUrl = "http://test.authorize.net",
                              PaymentGatewayApiKey = "12345",
                              PaymentGatewayApiSecret = "secret",
                              FacebookPageUrl = "facebook.com/myorganization",
                              VideoEmbedHtml = "<iframe />",
                              TwitterName = "@myorganization",
                              BlogRssUrl = "http://www.rss.com",
                              OrganizationSettings = new List<OrganizationSetting>()
                          };

            for (int i = 0; i < 5; i++)
            {
                org.OrganizationSettings.Add(new OrganizationSetting(
                    string.Format("setting{0}", i), "this is the value", DataType.STRING));
            }

            return org;
        }

		public static UserProfile GetValidUserProfile()
		{
		    var userProfile = new UserProfile
		                          {
		                              FirstName = "Jason",
		                              LastName = "Powers",
		                              Birthdate = DateTime.Parse("1/1/1970"),
		                              Gender = "M",
		                              Email = "jason@yahoo.com",
		                              PrimaryPhone = "(800) 555-1212",
		                              AddressLine1 = "555 S Main St",
		                              AddressLine2 = "",
		                              City = "Mesa",
		                              State = "AZ",
		                              ZipCode = "85213",
		                              Consent = true,
                                      Active = true,
                                      ImagePath = "content/images/users/1.jpg",
                                      Users = new List<User>()
		                          };

            userProfile.Users.Add(GetValidUser());
		    return userProfile;
		}

        public static Payment GetValidCCPayment()
        {
            return new Payment
                       {
                           FirstName = "Johnny",
                           LastName = "Appleseed",
                           AddressLine1 = "123 My St",
                           City = "New York",
                           State = "NY",
                           ZipCode = "85296",
                           PaymentType = PaymentType.CC,
                           AccountNumber = "4111111111111111",
                           Expiration = DateTime.Now.AddYears(1),
                           Cid = "123",
                           Amount = TestHelpers.GetAmount()
                       };
        }

        public static Payment GetValidCheckPayment()
        {
            return new Payment
                       {
                           FirstName = "Johnny",
                           LastName = "Appleseed",
                           AddressLine1 = "123 My St",
                           City = "New York",
                           State = "NY",
                           ZipCode = "85296",
                           PaymentType = PaymentType.ECheck,
                           AccountNumber = "4111111111111111",
                           RoutingNumber = "122105278",
                           BankName = "Bank of Awesome",
                           CheckNumber = "1234",
                           Amount = TestHelpers.GetAmount()
                       };
        }

        public static User GetValidUser()
        {
            return new User
                       {
                           Username = "jonny@gmail.com",
                           Password = "secret",
                           IsActive = true,
                           IsAuthorized = true,
                           ForcePasswordChange = false,
                           RegisterDate = DateTime.Now.AddDays(-3),
                           LastLoggedIn = DateTime.Now.AddMinutes(-5)
                       };
        }

        public static Role GetValidRole()
        {
            return new Role
                       {
                           Name = "Administrator",
                           Description = "System Administrator"
                       };
        }
    }
}
