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

namespace JordanRift.Grassroots.Framework.Entities
{
    public enum DataType
    {
        STRING = 1,
        INT = 2,
        DECIMAL = 3,
        BOOLEAN = 4
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

    public static class OrgSettingKeys
    {
        public const string ANALYTICS_TRACKING_CODE = "AnalyticsTrackingCode";
    }
}
