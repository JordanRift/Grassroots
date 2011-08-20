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
using JordanRift.Grassroots.Framework.Entities.Models;
using JordanRift.Grassroots.Framework.Helpers;

namespace JordanRift.Grassroots.Framework.Services
{
    public class CacheManager
    {
        public ICache cache;

        public CacheManager()
        {
            var factory = new CacheFactory();
            cache = factory.GetCache();
        }

        public T Get<T>(string key)
        {
            return (T) cache.Get(key);
        }

        public void Add(string key, object value)
        {
            cache.Add(key, value);

            if (value is Organization)
            {
                CacheViewData(value as Organization);
            }
        }

        public bool Exists(string key)
        {
            return cache.Exists(key);
        }

        public void Remove(string key, bool removingOrganization = false)
        {
            cache.Remove(key);

            if (removingOrganization)
            {
                ClearViewData();
            }
        }

        private void CacheViewData(Organization organization)
        {
            cache.Add(CacheKeys.ORG_NAME, organization.Name);
            cache.Add(CacheKeys.ORG_TAGLINE, organization.Tagline);
            cache.Add(CacheKeys.ORG_EMAIL, organization.ContactEmail);
            CacheSetting(CacheKeys.ORG_WEB_URL, organization.GetSetting(OrgSettingKeys.PUBLIC_WEBSITE_URL));
            CacheSetting(CacheKeys.ORG_ABOUT_URL, organization.GetSetting(OrgSettingKeys.PUBLIC_ABOUT_PAGE_URL));
            CacheSetting(CacheKeys.ORG_SERVICES_URL, organization.GetSetting(OrgSettingKeys.PUBLIC_SERVICES_PAGE_URL));
            CacheSetting(CacheKeys.ORG_NAV_HTML, organization.GetSetting(OrgSettingKeys.CUSTOM_NAVIGATION_HTML));
            CacheSetting(CacheKeys.ORG_HOME_PAGE_HTML, organization.GetSetting(OrgSettingKeys.CUSTOM_HOME_PAGE_HTML));
            CacheSetting(CacheKeys.ORG_DONATE_HTML, organization.GetSetting(OrgSettingKeys.DONATE_INSTRUCTIONS_HTML));
            CacheSetting(CacheKeys.ORG_FOOTER_HTML, organization.GetSetting(OrgSettingKeys.CUSTOM_FOOTER_HTML));
            CacheSetting(CacheKeys.ORG_ANALYTICS_CODE, organization.GetSetting(OrgSettingKeys.ANALYTICS_TRACKING_CODE));
        }

        private void CacheSetting(string key, OrganizationSetting setting)
        {
            if (setting != null)
            {
                cache.Add(key, setting.Value);
            }
        }

        private void ClearViewData()
        {
            var keys = ModelHelpers.GetKeys(typeof (CacheKeys));

            foreach (var key in keys)
            {
                cache.Remove(key);
            }
        }

    }
}
