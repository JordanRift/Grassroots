//
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

    public enum TransactionType
    {
        OneTime,
        Recurring
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
        public const string IS_FIXED_GOAL_AMOUNT = "MyC.IsFixedGoalAmount";
        public const string GOAL_AMOUNT = "MyC.GoalAmount";
        public const string ORGANIZATION_DOMAIN = "MyC.OrganizationDomain";
        public const string ORGANIZATION_CAMPAIGN_LENGTH = "MyC.OrganizationCampaignLength";
        public const string ORGANIZATION_DONATION_EMAIL_BODY = "MyC.OrganizationDonationEmailBody";
        public const string ORGANIZATION_DONATION_EMAIL_SUBJECT = "MyC.OrganizationDonationEmailSubject";
        public const string ORGANIZATION_DONATION_EMAIL_ADDRESS = "MyC.OrganizationDonationToEmailAddress";
    }
}
