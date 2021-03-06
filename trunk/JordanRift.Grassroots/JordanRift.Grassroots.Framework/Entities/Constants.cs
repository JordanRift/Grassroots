﻿//
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

namespace JordanRift.Grassroots.Framework.Entities
{
    public enum CacheType
    {
        Http,
        InMemory,
        //MemCached,
        Null
    }

    public enum FileStorageType
    {
        FileSystem,
        Azure,
        Database,
        S3,
        Null
    }

    public enum PriorityType
    {
        Low = 0,
        Medium = 1,
        High = 2
    }

    public enum DataType
    {
        String = 1,
        Integer = 2,
        Decimal = 3,
        Boolean = 4
    }

    public enum PaymentGatewayType
    {
        Unknown = 0,
        Authorize = 1,
        PayPal = 2
    }

    public enum PaymentType
    {
        CC,
        ECheck
    }

    public enum TransactionType
    {
        OneTime,
        Recurring
    }

    public enum CheckType
    {
        Checking,
        Savings,
        BusinessChecking
    }

    public enum PaymentResponseCode
    {
        Unknown = 0,
        Approved = 1,
        Declined = 2,
        Error = 3,
        InReview = 4
    }

    public enum CampaignType
    {
        Unknown = 0,
        Birthday = 1,
        SportingEvent = 2,
        Groups = 3,
        Other = 4
    }

    public enum ProfileImageSize 
    { 
        Thumbnail = 48, 
        Full = 100
    }

    public static class EntityConstants
    {
        public const string DEFAULT_CAMPAIGN_IMAGE_PATH = "http://lorempixum.com/450/278/people/";
    }

    public static class ConfigConstants
    {
        public const string LOG_ITEM_REPOSITORY = "ILogItemRepositoryType";
        public const string ORGANIZATION_REPOSITORY = "IOrganizationRepositoryType";
        public const string USER_PROFILE_REPOSITORY = "IUserProfileRepositoryType";
        public const string CAMPAIGN_REPOSITORY = "ICampaignRepositoryType";
        public const string USER_REPOSITORY = "IUserRepositoryType";
        public const string MEMBERSHIP_PROVIDER_NAME = "GrassrootsMembershipProvider";
    }

    public static class CacheKeys
    {
        public const string ORG_NAME = "Organization.Name";
        public const string ORG_TAGLINE = "Organization.Tagline";
        public const string ORG_EMAIL = "Organization.ContactEmail";
        public const string ORG_WEB_URL = "Organization.PublicWebsiteUrl";
        public const string ORG_ABOUT_URL = "Organization.PublicAboutPageUrl";
        public const string ORG_SERVICES_URL = "Organization.PublicServicesPageUrl";
        public const string ORG_NAV_HTML = "Organization.NavigationHtml";
        public const string ORG_HOME_PAGE_HTML = "Organization.HomePageHtml";
        public const string ORG_DONATE_HTML = "Organization.DonationInstructionsHtml";
        public const string ORG_FOOTER_HTML = "Organization.FooterHtml";
        public const string ORG_ANALYTICS_CODE = "Organization.AnalyticsTrackingCode";
        public const string ORG_CAMPAIGNS_UNAVAILABLE_HTML = "Organization.CampaignsUnavailableHtml";
        public const string ORG_HOME_PAGE_HEADER_HTML = "Organization.CustomHomePageHeader";
    }

    public static class OrgSettingKeys
    {
        public const string ANALYTICS_TRACKING_CODE = "AnalyticsTrackingCode";
        public const string DONATE_INSTRUCTIONS_HTML = "DonateInstructionsHtml";
        public const string CUSTOM_NAVIGATION_HTML = "CustomNavigationHtml";
        public const string CUSTOM_HOME_PAGE_HTML = "CustomHomePageHtml";
        public const string CUSTOM_FOOTER_HTML = "CustomFooterHtml";
        public const string PUBLIC_WEBSITE_URL = "PublicWebsiteUrl";
        public const string PUBLIC_ABOUT_PAGE_URL = "PublicAboutPageUrl";
        public const string PUBLIC_SERVICES_PAGE_URL = "PublicServicesPageUrl";
        public const string DONATION_NOTIFICATION_ADDRESS = "DonationNotificationAddress";
        public const string CAMPAIGNS_UNAVAILABLE_HTML = "CampaignsUnavailableHtml";
        public const string HOME_PAGE_HEADER_HTML = "CustomHomePageHeader";
    }
}
