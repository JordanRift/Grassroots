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
using JordanRift.Grassroots.Framework.Data;
using JordanRift.Grassroots.Framework.Entities;
using JordanRift.Grassroots.Framework.Entities.Models;
using JordanRift.Grassroots.Framework.Helpers;

namespace JordanRift.Grassroots.Web.Helpers.UI
{
    public abstract class GrassrootsWebViewPage<TModel> : WebViewPage<TModel>
    {
        private readonly Organization organization;

        public string OrganizationName
        {
            get { return organization.Name; }
        }

        public string OrganizationTagline
        {
            get { return organization.Tagline; }
        }

        public string ContactEmail
        {
            get { return organization.ContactEmail; }
        }

        public string PublicWebsiteUrl
        {
            get { return organization.PublicWebsiteUrl; }
        }

        public string PublicAboutPageUrl
        {
            get { return organization.PublicAboutPageUrl; }
        }

        public string PublicServicesPageUrl
        {
            get { return organization.PublicServicesPageUrl; }
        }

        public string NavigationHtml
        {
            get { return organization.NavigationHtml; }
        }

        public string FooterHtml
        {
            get { return organization.FooterHtml; }
        }

        public string AnalyticsTrackingCode
        {
            get
            {
                var setting = organization.GetSetting(OrgSettingKeys.ANALYTICS_TRACKING_CODE);
                return setting != null ? setting.Value : null;
            }
        }

        protected GrassrootsWebViewPage()
        {
            var repositoryFactory = new RepositoryFactory<IOrganizationRepository>();

            using (var organizationRepository = repositoryFactory.GetRepository())
            {
                organization = organizationRepository.GetDefaultOrganization(readOnly: true);
            }
        }
    }
}