﻿//
// Copyright © 2011 Jordan Rift, LLC - All Rights Reserved
//
// THIS WORK IS LICENSED UNDER A CREATIVE COMMONS ATTRIBUTION-NONCOMMERCIAL-
// SHAREALIKE 3.0 UNPORTED LICENSE:
// http://creativecommons.org/licenses/by-nc-sa/3.0/
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
