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

using JordanRift.Grassroots.Framework.Entities;
using JordanRift.Grassroots.Framework.Helpers;
using NUnit.Framework;

namespace JordanRift.Grassroots.Tests.UnitTests.Helpers
{
    [TestFixture]
    public class ModelHelpersTests
    {
        [Test]
        public void GetOrgSettingKeys_Should_Return_List_Of_Known_Org_Setting_Keys()
        {
            var result = ModelHelpers.GetOrgSettingKeys();
            Assert.IsNotEmpty(result);
        }

        [Test]
        public void GetOrgSettingKeys_Should_Load_AnalyticsCode_Setting_Key()
        {
            var result = ModelHelpers.GetOrgSettingKeys();
            Assert.Contains(OrgSettingKeys.ANALYTICS_TRACKING_CODE, result);
        }

        [Test]
        public void GetOrgSettingKeys_Should_Load_DonateInstructionsHtml_Setting_Key()
        {
            var result = ModelHelpers.GetOrgSettingKeys();
            Assert.Contains(OrgSettingKeys.DONATE_INSTRUCTIONS_HTML, result);
        }

        [Test]
        public void GetOrgSettingKeys_Should_Load_CustomNavigationHtml_Setting_Key()
        {
            var result = ModelHelpers.GetOrgSettingKeys();
            Assert.Contains(OrgSettingKeys.CUSTOM_NAVIGATION_HTML, result);
        }

        [Test]
        public void GetOrgSettingKeys_Should_Load_CustomHomePageHtml_Setting_Key()
        {
            var result = ModelHelpers.GetOrgSettingKeys();
            Assert.Contains(OrgSettingKeys.CUSTOM_HOME_PAGE_HTML, result);
        }

        [Test]
        public void GetOrgSettingKeys_Should_Load_CustomFooterHtml_Setting_Key()
        {
            var result = ModelHelpers.GetOrgSettingKeys();
            Assert.Contains(OrgSettingKeys.CUSTOM_FOOTER_HTML, result);
        }

        [Test]
        public void GetOrgSettingKeys_Should_Load_PublicWebsiteUrl_Setting_Key()
        {
            var result = ModelHelpers.GetOrgSettingKeys();
            Assert.Contains(OrgSettingKeys.PUBLIC_WEBSITE_URL, result);
        }

        [Test]
        public void GetOrgSettingKeys_Should_Load_PublicAboutPageUrl_Setting_Key()
        {
            var result = ModelHelpers.GetOrgSettingKeys();
            Assert.Contains(OrgSettingKeys.PUBLIC_ABOUT_PAGE_URL, result);
        }

        [Test]
        public void GetOrgSettingKeys_Should_Load_PublicServicesPageUrl_Setting_Key()
        {
            var result = ModelHelpers.GetOrgSettingKeys();
            Assert.Contains(OrgSettingKeys.PUBLIC_SERVICES_PAGE_URL, result);
        }
    }
}
