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

using System.Web.Mvc;
using JordanRift.Grassroots.Framework.Entities;
using JordanRift.Grassroots.Framework.Services;

namespace JordanRift.Grassroots.Web.Helpers.UI
{
    public abstract class GrassrootsWebViewPage<TModel> : WebViewPage<TModel>
    {
        private readonly CacheManager cacheManager;

        public string OrganizationName { get { return cacheManager.Get<string>(CacheKeys.ORG_NAME); } }
        public string OrganizationTagline { get { return cacheManager.Get<string>(CacheKeys.ORG_TAGLINE); } }
        public string ContactEmail { get { return cacheManager.Get<string>(CacheKeys.ORG_EMAIL); } }
        public string PublicWebsiteUrl { get { return cacheManager.Get<string>(CacheKeys.ORG_WEB_URL); } }
        public string PublicAboutPageUrl { get { return cacheManager.Get<string>(CacheKeys.ORG_ABOUT_URL); } }
        public string PublicServicesPageUrl { get { return cacheManager.Get<string>(CacheKeys.ORG_SERVICES_URL); } }
        public string NavigationHtml { get { return cacheManager.Get<string>(CacheKeys.ORG_NAV_HTML); } }
        public string FooterHtml { get { return cacheManager.Get<string>(CacheKeys.ORG_FOOTER_HTML); } }
        public string HomePageHtml { get { return cacheManager.Get<string>(CacheKeys.ORG_HOME_PAGE_HTML); } }
        public string DonationInstructionsHtml { get { return cacheManager.Get<string>(CacheKeys.ORG_DONATE_HTML); } }
        public string AnalyticsTrackingCode { get { return cacheManager.Get<string>(CacheKeys.ORG_ANALYTICS_CODE); } }
        public string CampaignUnavailableHtml { get { return cacheManager.Get<string>(CacheKeys.ORG_CAMPAIGNS_UNAVAILABLE_HTML); } }
        public string HomePageHeaderHtml { get { return cacheManager.Get<string>(CacheKeys.ORG_HOME_PAGE_HEADER_HTML); } }

        protected GrassrootsWebViewPage()
        {
            cacheManager = new CacheManager();
        }
    }
}