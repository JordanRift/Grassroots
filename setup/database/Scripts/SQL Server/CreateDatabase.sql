BEGIN TRANSACTION
/****** Object:  Table [dbo].[ELMAH_Error]    Script Date: 12/22/2011 12:57:57 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ELMAH_Error](
	[ErrorId] [uniqueidentifier] NOT NULL,
	[Application] [nvarchar](60) NOT NULL,
	[Host] [nvarchar](50) NOT NULL,
	[Type] [nvarchar](100) NOT NULL,
	[Source] [nvarchar](60) NOT NULL,
	[Message] [nvarchar](500) NOT NULL,
	[User] [nvarchar](50) NOT NULL,
	[StatusCode] [int] NOT NULL,
	[TimeUtc] [datetime] NOT NULL,
	[Sequence] [int] IDENTITY(1,1) NOT NULL,
	[AllXml] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_ELMAH_Error] PRIMARY KEY NONCLUSTERED 
(
	[ErrorId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[ELMAH_Error] ADD  CONSTRAINT [DF_ELMAH_Error_ErrorId]  DEFAULT (newid()) FOR [ErrorId]
GO

/****** Object:  StoredProcedure [dbo].[ELMAH_GetErrorsXml]    Script Date: 12/22/2011 12:58:20 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[ELMAH_GetErrorsXml]
(
    @Application NVARCHAR(60),
    @PageIndex INT = 0,
    @PageSize INT = 15,
    @TotalCount INT OUTPUT
)
AS 

    SET NOCOUNT ON

    DECLARE @FirstTimeUTC DATETIME
    DECLARE @FirstSequence INT
    DECLARE @StartRow INT
    DECLARE @StartRowIndex INT

    SELECT 
        @TotalCount = COUNT(1) 
    FROM 
        [ELMAH_Error]
    WHERE 
        [Application] = @Application

    -- Get the ID of the first error for the requested page

    SET @StartRowIndex = @PageIndex * @PageSize + 1

    IF @StartRowIndex <= @TotalCount
    BEGIN

        SET ROWCOUNT @StartRowIndex

        SELECT  
            @FirstTimeUTC = [TimeUtc],
            @FirstSequence = [Sequence]
        FROM 
            [ELMAH_Error]
        WHERE   
            [Application] = @Application
        ORDER BY 
            [TimeUtc] DESC, 
            [Sequence] DESC

    END
    ELSE
    BEGIN

        SET @PageSize = 0

    END

    -- Now set the row count to the requested page size and get
    -- all records below it for the pertaining application.

    SET ROWCOUNT @PageSize

    SELECT 
        errorId     = [ErrorId], 
        application = [Application],
        host        = [Host], 
        type        = [Type],
        source      = [Source],
        message     = [Message],
        [user]      = [User],
        statusCode  = [StatusCode], 
        time        = CONVERT(VARCHAR(50), [TimeUtc], 126) + 'Z'
    FROM 
        [ELMAH_Error] error
    WHERE
        [Application] = @Application
    AND
        [TimeUtc] <= @FirstTimeUTC
    AND 
        [Sequence] <= @FirstSequence
    ORDER BY
        [TimeUtc] DESC, 
        [Sequence] DESC
    FOR
        XML AUTO


GO

/****** Object:  StoredProcedure [dbo].[ELMAH_GetErrorXml]    Script Date: 12/22/2011 12:58:50 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[ELMAH_GetErrorXml]
(
    @Application NVARCHAR(60),
    @ErrorId UNIQUEIDENTIFIER
)
AS

    SET NOCOUNT ON

    SELECT 
        [AllXml]
    FROM 
        [ELMAH_Error]
    WHERE
        [ErrorId] = @ErrorId
    AND
        [Application] = @Application


GO

/****** Object:  StoredProcedure [dbo].[ELMAH_LogError]    Script Date: 12/22/2011 12:59:08 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[ELMAH_LogError]
(
    @ErrorId UNIQUEIDENTIFIER,
    @Application NVARCHAR(60),
    @Host NVARCHAR(30),
    @Type NVARCHAR(100),
    @Source NVARCHAR(60),
    @Message NVARCHAR(500),
    @User NVARCHAR(50),
    @AllXml NVARCHAR(MAX),
    @StatusCode INT,
    @TimeUtc DATETIME
)
AS

    SET NOCOUNT ON

    INSERT
    INTO
        [ELMAH_Error]
        (
            [ErrorId],
            [Application],
            [Host],
            [Type],
            [Source],
            [Message],
            [User],
            [AllXml],
            [StatusCode],
            [TimeUtc]
        )
    VALUES
        (
            @ErrorId,
            @Application,
            @Host,
            @Type,
            @Source,
            @Message,
            @User,
            @AllXml,
            @StatusCode,
            @TimeUtc
        )


GO

/****** Object:  Table [dbo].[gr_Organization]    Script Date: 12/22/2011 12:52:36 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[gr_Organization](
	[OrganizationID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](50) NOT NULL,
	[Tagline] [varchar](150) NOT NULL,
	[SummaryHtml] [varchar](max) NULL,
	[DescriptionHtml] [varchar](max) NULL,
	[ContactPhone] [varchar](20) NOT NULL,
	[ContactEmail] [varchar](30) NOT NULL,
	[YtdGoal] [decimal](18, 2) NULL,
	[FiscalYearStartMonth] [int] NULL,
	[FiscalYearStartDay] [int] NULL,
	[PaymentGatewayType] [int] NOT NULL,
	[PaymentGatewayApiUrl] [varchar](100) NULL,
	[PaymentGatewayApiKey] [varchar](100) NOT NULL,
	[PaymentGatewayApiSecret] [varchar](100) NOT NULL,
	[FacebookPageUrl] [varchar](200) NOT NULL,
	[VideoEmbedHtml] [varchar](1000) NOT NULL,
	[TwitterName] [varchar](50) NULL,
	[BlogRssUrl] [varchar](250) NULL,
	[ThemeName] [varchar](50) NOT NULL,
	[PaymentGatewayArbApiUrl] [varchar](100) NULL,
	[CreatedOn] [datetime] NOT NULL,
	[LastModifiedOn] [datetime] NOT NULL,
 CONSTRAINT [PK_myc_Organization] PRIMARY KEY CLUSTERED 
(
	[OrganizationID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

ALTER TABLE [dbo].[gr_Organization] ADD  CONSTRAINT [DF_myc_Organization_Name]  DEFAULT ('') FOR [Name]
GO

ALTER TABLE [dbo].[gr_Organization] ADD  CONSTRAINT [DF_gr_Organization_Tagline]  DEFAULT ('') FOR [Tagline]
GO

ALTER TABLE [dbo].[gr_Organization] ADD  CONSTRAINT [DF_myc_Organization_ContactPhone]  DEFAULT ('') FOR [ContactPhone]
GO

ALTER TABLE [dbo].[gr_Organization] ADD  CONSTRAINT [DF_myc_Organization_ContactEmail]  DEFAULT ('') FOR [ContactEmail]
GO

ALTER TABLE [dbo].[gr_Organization] ADD  CONSTRAINT [DF_gr_Organization_YtdGoal]  DEFAULT ((0)) FOR [YtdGoal]
GO

ALTER TABLE [dbo].[gr_Organization] ADD  CONSTRAINT [DF_gr_Organization_FiscalYearStartMonth]  DEFAULT ((1)) FOR [FiscalYearStartMonth]
GO

ALTER TABLE [dbo].[gr_Organization] ADD  CONSTRAINT [DF_gr_Organization_FiscalYearStartDay]  DEFAULT ((1)) FOR [FiscalYearStartDay]
GO

ALTER TABLE [dbo].[gr_Organization] ADD  CONSTRAINT [DF_gr_Organization_PaymentGatewayType]  DEFAULT ((-1)) FOR [PaymentGatewayType]
GO

ALTER TABLE [dbo].[gr_Organization] ADD  CONSTRAINT [DF_gr_Organization_PaymentGatewayApiKey]  DEFAULT ('') FOR [PaymentGatewayApiKey]
GO

ALTER TABLE [dbo].[gr_Organization] ADD  CONSTRAINT [DF_gr_Organization_PaymentGatewayApiSecret]  DEFAULT ('') FOR [PaymentGatewayApiSecret]
GO

ALTER TABLE [dbo].[gr_Organization] ADD  CONSTRAINT [DF_gr_Organization_FacebookPageUrl]  DEFAULT ('') FOR [FacebookPageUrl]
GO

ALTER TABLE [dbo].[gr_Organization] ADD  CONSTRAINT [DF_gr_Organization_VideoEmbedHtml]  DEFAULT ('') FOR [VideoEmbedHtml]
GO

ALTER TABLE [dbo].[gr_Organization] ADD  CONSTRAINT [DF_gr_Organization_TwitterName]  DEFAULT ('') FOR [TwitterName]
GO

ALTER TABLE [dbo].[gr_Organization] ADD  CONSTRAINT [DF_gr_Organization_ThemeName]  DEFAULT ('') FOR [ThemeName]
GO

ALTER TABLE [dbo].[gr_Organization] ADD  DEFAULT ('1/1/1900') FOR [CreatedOn]
GO

ALTER TABLE [dbo].[gr_Organization] ADD  DEFAULT ('1/1/1900') FOR [LastModifiedOn]
GO


/****** Object:  Table [dbo].[gr_Role]    Script Date: 12/22/2011 12:54:11 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[gr_Role](
	[RoleID] [int] IDENTITY(1,1) NOT NULL,
	[OrganizationID] [int] NOT NULL,
	[Name] [varchar](100) NOT NULL,
	[Description] [varchar](500) NOT NULL,
	[CreatedOn] [datetime] NOT NULL,
	[LastModifiedOn] [datetime] NOT NULL,
 CONSTRAINT [PK_gr_Role] PRIMARY KEY CLUSTERED 
(
	[RoleID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

ALTER TABLE [dbo].[gr_Role]  WITH CHECK ADD  CONSTRAINT [FK_gr_Role_gr_Organization] FOREIGN KEY([OrganizationID])
REFERENCES [dbo].[gr_Organization] ([OrganizationID])
GO

ALTER TABLE [dbo].[gr_Role] CHECK CONSTRAINT [FK_gr_Role_gr_Organization]
GO

ALTER TABLE [dbo].[gr_Role] ADD  CONSTRAINT [DF_gr_Role_Name]  DEFAULT ('') FOR [Name]
GO

ALTER TABLE [dbo].[gr_Role] ADD  CONSTRAINT [DF_gr_Role_Description]  DEFAULT ('') FOR [Description]
GO

ALTER TABLE [dbo].[gr_Role] ADD  DEFAULT ('1/1/1900') FOR [CreatedOn]
GO

ALTER TABLE [dbo].[gr_Role] ADD  DEFAULT ('1/1/1900') FOR [LastModifiedOn]
GO

/****** Object:  Table [dbo].[gr_OrganizationSetting]    Script Date: 12/22/2011 12:55:23 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[gr_OrganizationSetting](
	[OrganizationSettingID] [int] IDENTITY(1,1) NOT NULL,
	[OrganizationID] [int] NULL,
	[Name] [varchar](100) NOT NULL,
	[Value] [varchar](max) NOT NULL,
	[DataType] [int] NOT NULL,
	[CreatedOn] [datetime] NOT NULL,
	[LastModifiedOn] [datetime] NOT NULL,
 CONSTRAINT [PK_myc_OrganizationSetting] PRIMARY KEY CLUSTERED 
(
	[OrganizationSettingID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

ALTER TABLE [dbo].[gr_OrganizationSetting]  WITH CHECK ADD  CONSTRAINT [FK_myc_OrganizationSetting_myc_Organization] FOREIGN KEY([OrganizationID])
REFERENCES [dbo].[gr_Organization] ([OrganizationID])
GO

ALTER TABLE [dbo].[gr_OrganizationSetting] CHECK CONSTRAINT [FK_myc_OrganizationSetting_myc_Organization]
GO

ALTER TABLE [dbo].[gr_OrganizationSetting] ADD  CONSTRAINT [DF_myc_OrganizationSetting_Name]  DEFAULT ('') FOR [Name]
GO

ALTER TABLE [dbo].[gr_OrganizationSetting] ADD  CONSTRAINT [DF_myc_OrganizationSetting_Value]  DEFAULT ('') FOR [Value]
GO

ALTER TABLE [dbo].[gr_OrganizationSetting] ADD  CONSTRAINT [DF_myc_OrganizationSetting_DataType]  DEFAULT ((0)) FOR [DataType]
GO

ALTER TABLE [dbo].[gr_OrganizationSetting] ADD  DEFAULT ('1/1/1900') FOR [CreatedOn]
GO

ALTER TABLE [dbo].[gr_OrganizationSetting] ADD  DEFAULT ('1/1/1900') FOR [LastModifiedOn]
GO

/****** Object:  Table [dbo].[gr_UserProfile]    Script Date: 12/22/2011 12:56:10 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[gr_UserProfile](
	[UserProfileID] [int] IDENTITY(1,1) NOT NULL,
	[OrganizationID] [int] NOT NULL,
	[RoleID] [int] NULL,
	[FacebookID] [varchar](50) NULL,
	[FirstName] [varchar](30) NOT NULL,
	[LastName] [varchar](30) NOT NULL,
	[Birthdate] [datetime] NOT NULL,
	[Gender] [varchar](20) NOT NULL,
	[Email] [varchar](50) NOT NULL,
	[PrimaryPhone] [varchar](30) NULL,
	[AddressLine1] [varchar](200) NULL,
	[AddressLine2] [varchar](200) NULL,
	[City] [varchar](100) NULL,
	[State] [varchar](100) NULL,
	[ZipCode] [varchar](20) NOT NULL,
	[Consent] [bit] NOT NULL,
	[Active] [bit] NOT NULL,
	[IsActivated] [bit] NOT NULL,
	[ImagePath] [varchar](100) NULL,
	[ActivationHash] [varchar](500) NULL,
	[ActivationPin] [varchar](12) NULL,
	[LastActivationAttempt] [datetime] NOT NULL,
	[CreatedOn] [datetime] NOT NULL,
	[LastModifiedOn] [datetime] NOT NULL,
 CONSTRAINT [PK_myc_UserProfile] PRIMARY KEY CLUSTERED 
(
	[UserProfileID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

ALTER TABLE [dbo].[gr_UserProfile]  WITH CHECK ADD  CONSTRAINT [FK_gr_UserProfile_gr_Organization] FOREIGN KEY([OrganizationID])
REFERENCES [dbo].[gr_Organization] ([OrganizationID])
GO

ALTER TABLE [dbo].[gr_UserProfile] CHECK CONSTRAINT [FK_gr_UserProfile_gr_Organization]
GO

ALTER TABLE [dbo].[gr_UserProfile]  WITH CHECK ADD  CONSTRAINT [FK_gr_UserProfile_gr_Role] FOREIGN KEY([RoleID])
REFERENCES [dbo].[gr_Role] ([RoleID])
GO

ALTER TABLE [dbo].[gr_UserProfile] CHECK CONSTRAINT [FK_gr_UserProfile_gr_Role]
GO

ALTER TABLE [dbo].[gr_UserProfile] ADD  CONSTRAINT [DF_myc_UserProfile_FirstName]  DEFAULT ('') FOR [FirstName]
GO

ALTER TABLE [dbo].[gr_UserProfile] ADD  CONSTRAINT [DF_myc_UserProfile_LastName]  DEFAULT ('') FOR [LastName]
GO

ALTER TABLE [dbo].[gr_UserProfile] ADD  CONSTRAINT [DF_myc_UserProfile_Birthdate]  DEFAULT ('1/1/1900') FOR [Birthdate]
GO

ALTER TABLE [dbo].[gr_UserProfile] ADD  CONSTRAINT [DF_myc_UserProfile_Gender]  DEFAULT ('') FOR [Gender]
GO

ALTER TABLE [dbo].[gr_UserProfile] ADD  CONSTRAINT [DF_myc_UserProfile_Email]  DEFAULT ('') FOR [Email]
GO

ALTER TABLE [dbo].[gr_UserProfile] ADD  CONSTRAINT [DF_myc_UserProfile_PrimaryPhone]  DEFAULT ('') FOR [PrimaryPhone]
GO

ALTER TABLE [dbo].[gr_UserProfile] ADD  CONSTRAINT [DF_myc_UserProfile_AddressLine1]  DEFAULT ('') FOR [AddressLine1]
GO

ALTER TABLE [dbo].[gr_UserProfile] ADD  CONSTRAINT [DF_myc_UserProfile_AddressLine2]  DEFAULT ('') FOR [AddressLine2]
GO

ALTER TABLE [dbo].[gr_UserProfile] ADD  CONSTRAINT [DF_myc_UserProfile_City]  DEFAULT ('') FOR [City]
GO

ALTER TABLE [dbo].[gr_UserProfile] ADD  CONSTRAINT [DF_myc_UserProfile_State]  DEFAULT ('') FOR [State]
GO

ALTER TABLE [dbo].[gr_UserProfile] ADD  CONSTRAINT [DF_myc_UserProfile_ZipCode]  DEFAULT ('') FOR [ZipCode]
GO

ALTER TABLE [dbo].[gr_UserProfile] ADD  CONSTRAINT [DF_myc_UserProfile_ParentalConsent]  DEFAULT ((0)) FOR [Consent]
GO

ALTER TABLE [dbo].[gr_UserProfile] ADD  CONSTRAINT [DF_gr_UserProfile_Active]  DEFAULT ((1)) FOR [Active]
GO

ALTER TABLE [dbo].[gr_UserProfile] ADD  CONSTRAINT [DF_gr_UserProfile_IsActivated]  DEFAULT ((0)) FOR [IsActivated]
GO

ALTER TABLE [dbo].[gr_UserProfile] ADD  CONSTRAINT [DF_gr_UserProfile_ImagePath]  DEFAULT ('content/images/avatar.jpg') FOR [ImagePath]
GO

ALTER TABLE [dbo].[gr_UserProfile] ADD  CONSTRAINT [DF_gr_UserProfile_LastActivationAttempt]  DEFAULT ('1/1/1900') FOR [LastActivationAttempt]
GO

ALTER TABLE [dbo].[gr_UserProfile] ADD  DEFAULT ('1/1/1900') FOR [CreatedOn]
GO

ALTER TABLE [dbo].[gr_UserProfile] ADD  DEFAULT ('1/1/1900') FOR [LastModifiedOn]
GO

/****** Object:  Table [dbo].[gr_User]    Script Date: 12/22/2011 12:56:46 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[gr_User](
	[UserID] [int] IDENTITY(1,1) NOT NULL,
	[Username] [varchar](50) NOT NULL,
	[Password] [varchar](2000) NOT NULL,
	[UserProfileID] [int] NOT NULL,
	[IsActive] [bit] NOT NULL,
	[IsAuthorized] [bit] NOT NULL,
	[ForcePasswordChange] [bit] NOT NULL,
	[RegisterDate] [datetime] NOT NULL,
	[LastLoggedIn] [datetime] NOT NULL,
	[FailedLoginAttempts] [int] NOT NULL,
	[CreatedOn] [datetime] NOT NULL,
	[LastModifiedOn] [datetime] NOT NULL,
 CONSTRAINT [PK_gr_User_1] PRIMARY KEY CLUSTERED 
(
	[UserID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

ALTER TABLE [dbo].[gr_User]  WITH CHECK ADD  CONSTRAINT [FK_gr_User_gr_UserProfile] FOREIGN KEY([UserProfileID])
REFERENCES [dbo].[gr_UserProfile] ([UserProfileID])
GO

ALTER TABLE [dbo].[gr_User] CHECK CONSTRAINT [FK_gr_User_gr_UserProfile]
GO

ALTER TABLE [dbo].[gr_User] ADD  CONSTRAINT [DF_gr_User_Password]  DEFAULT ('') FOR [Password]
GO

ALTER TABLE [dbo].[gr_User] ADD  CONSTRAINT [DF_gr_User_IsActive]  DEFAULT ((0)) FOR [IsActive]
GO

ALTER TABLE [dbo].[gr_User] ADD  CONSTRAINT [DF_gr_User_IsAuthorized]  DEFAULT ((0)) FOR [IsAuthorized]
GO

ALTER TABLE [dbo].[gr_User] ADD  CONSTRAINT [DF_gr_User_ForcePasswordChange]  DEFAULT ((0)) FOR [ForcePasswordChange]
GO

ALTER TABLE [dbo].[gr_User] ADD  CONSTRAINT [DF_gr_User_RegisterDate]  DEFAULT ('1/1/1900') FOR [RegisterDate]
GO

ALTER TABLE [dbo].[gr_User] ADD  CONSTRAINT [DF_gr_User_LastLoggedIn]  DEFAULT ('1/1/1900') FOR [LastLoggedIn]
GO

ALTER TABLE [dbo].[gr_User] ADD  CONSTRAINT [DF_gr_User_FailedLoginAttempts]  DEFAULT ((0)) FOR [FailedLoginAttempts]
GO

ALTER TABLE [dbo].[gr_User] ADD  DEFAULT ('1/1/1900') FOR [CreatedOn]
GO

ALTER TABLE [dbo].[gr_User] ADD  DEFAULT ('1/1/1900') FOR [LastModifiedOn]
GO

/****** Object:  Table [dbo].[gr_Region]    Script Date: 12/22/2011 13:01:17 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[gr_Region](
	[RegionID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](50) NOT NULL,
	[Description] [varchar](200) NOT NULL,
	[CreatedOn] [datetime] NOT NULL,
	[LastModifiedOn] [datetime] NOT NULL,
 CONSTRAINT [PK_gr_Region] PRIMARY KEY CLUSTERED 
(
	[RegionID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

ALTER TABLE [dbo].[gr_Region] ADD  DEFAULT ('1/1/1900') FOR [CreatedOn]
GO

ALTER TABLE [dbo].[gr_Region] ADD  DEFAULT ('1/1/1900') FOR [LastModifiedOn]
GO

/****** Object:  Table [dbo].[gr_CauseTemplate]    Script Date: 12/22/2011 13:02:04 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[gr_CauseTemplate](
	[CauseTemplateID] [int] IDENTITY(1,1) NOT NULL,
	[OrganizationID] [int] NOT NULL,
	[Name] [varchar](100) NOT NULL,
	[ActionVerb] [varchar](50) NOT NULL,
	[GoalName] [varchar](50) NOT NULL,
	[CallToAction] [varchar](300) NOT NULL,
	[Active] [bit] NOT NULL,
	[AmountIsConfigurable] [bit] NOT NULL,
	[DefaultAmount] [decimal](7, 2) NOT NULL,
	[TimespanIsConfigurable] [bit] NOT NULL,
	[DefaultTimespanInDays] [int] NOT NULL,
	[Summary] [varchar](500) NOT NULL,
	[VideoEmbedHtml] [varchar](200) NOT NULL,
	[DescriptionHtml] [varchar](max) NOT NULL,
	[ImagePath] [varchar](200) NOT NULL,
	[BeforeImagePath] [varchar](200) NULL,
	[AfterImagePath] [varchar](200) NULL,
	[InstructionsOpenHtml] [varchar](max) NOT NULL,
	[InstructionsClosedHtml] [varchar](max) NOT NULL,
	[StatisticsHtml] [varchar](max) NULL,
	[CutOffDate] [datetime] NULL,
	[CreatedOn] [datetime] NOT NULL,
	[LastModifiedOn] [datetime] NOT NULL,
 CONSTRAINT [PK_gr_CauseType] PRIMARY KEY CLUSTERED 
(
	[CauseTemplateID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

ALTER TABLE [dbo].[gr_CauseTemplate]  WITH CHECK ADD  CONSTRAINT [FK_gr_CauseType_gr_Organization] FOREIGN KEY([OrganizationID])
REFERENCES [dbo].[gr_Organization] ([OrganizationID])
GO

ALTER TABLE [dbo].[gr_CauseTemplate] CHECK CONSTRAINT [FK_gr_CauseType_gr_Organization]
GO

ALTER TABLE [dbo].[gr_CauseTemplate] ADD  CONSTRAINT [DF_gr_CauseType_Name]  DEFAULT ('') FOR [Name]
GO

ALTER TABLE [dbo].[gr_CauseTemplate] ADD  CONSTRAINT [DF_gr_CauseType_ActionVerb]  DEFAULT ('') FOR [ActionVerb]
GO

ALTER TABLE [dbo].[gr_CauseTemplate] ADD  CONSTRAINT [DF_gr_CauseType_GoalName]  DEFAULT ('') FOR [GoalName]
GO

ALTER TABLE [dbo].[gr_CauseTemplate] ADD  CONSTRAINT [DF_gr_CauseTemplate_CallToAction]  DEFAULT ('') FOR [CallToAction]
GO

ALTER TABLE [dbo].[gr_CauseTemplate] ADD  CONSTRAINT [DF_gr_CauseType_Active]  DEFAULT ((1)) FOR [Active]
GO

ALTER TABLE [dbo].[gr_CauseTemplate] ADD  CONSTRAINT [DF_gr_CauseType_AmountIsConfigurable]  DEFAULT ((0)) FOR [AmountIsConfigurable]
GO

ALTER TABLE [dbo].[gr_CauseTemplate] ADD  CONSTRAINT [DF_gr_CauseType_DefaultAmount]  DEFAULT ((0)) FOR [DefaultAmount]
GO

ALTER TABLE [dbo].[gr_CauseTemplate] ADD  CONSTRAINT [DF_gr_CauseType_TimespanIsConfigurable]  DEFAULT ((0)) FOR [TimespanIsConfigurable]
GO

ALTER TABLE [dbo].[gr_CauseTemplate] ADD  CONSTRAINT [DF_gr_CauseType_DefaultTimespanInDays]  DEFAULT ((0)) FOR [DefaultTimespanInDays]
GO

ALTER TABLE [dbo].[gr_CauseTemplate] ADD  CONSTRAINT [DF_gr_CauseType_Summary]  DEFAULT ('') FOR [Summary]
GO

ALTER TABLE [dbo].[gr_CauseTemplate] ADD  CONSTRAINT [DF_gr_CauseType_VideoEmbedHtml]  DEFAULT ('') FOR [VideoEmbedHtml]
GO

ALTER TABLE [dbo].[gr_CauseTemplate] ADD  CONSTRAINT [DF_gr_CauseType_DescriptionHtml]  DEFAULT ('') FOR [DescriptionHtml]
GO

ALTER TABLE [dbo].[gr_CauseTemplate] ADD  CONSTRAINT [DF_gr_CauseTemplate_ImagePath]  DEFAULT ('') FOR [ImagePath]
GO

ALTER TABLE [dbo].[gr_CauseTemplate] ADD  CONSTRAINT [DF_gr_CauseTemplate_InstructionsOpenHtml]  DEFAULT ('') FOR [InstructionsOpenHtml]
GO

ALTER TABLE [dbo].[gr_CauseTemplate] ADD  CONSTRAINT [DF_gr_CauseTemplate_InstructionsClosedHtml]  DEFAULT ('') FOR [InstructionsClosedHtml]
GO

ALTER TABLE [dbo].[gr_CauseTemplate] ADD  DEFAULT ('1/1/1900') FOR [CreatedOn]
GO

ALTER TABLE [dbo].[gr_CauseTemplate] ADD  DEFAULT ('1/1/1900') FOR [LastModifiedOn]
GO

/****** Object:  Table [dbo].[gr_Cause]    Script Date: 12/22/2011 13:02:51 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[gr_Cause](
	[CauseID] [int] IDENTITY(1,1) NOT NULL,
	[OrganizationID] [int] NOT NULL,
	[CauseTemplateID] [int] NOT NULL,
	[RegionID] [int] NULL,
	[Name] [varchar](100) NOT NULL,
	[Active] [bit] NOT NULL,
	[Summary] [varchar](1000) NULL,
	[VideoEmbedHtml] [varchar](200) NOT NULL,
	[DescriptionHtml] [varchar](max) NULL,
	[ImagePath] [varchar](200) NOT NULL,
	[HoursVolunteered] [int] NULL,
	[BeforeImagePath] [varchar](200) NULL,
	[AfterImagePath] [varchar](200) NULL,
	[Latitude] [decimal](18, 15) NULL,
	[Longitude] [decimal](18, 15) NULL,
	[ReferenceNumber] [varchar](50) NULL,
	[IsCompleted] [bit] NOT NULL,
	[DateCompleted] [datetime] NULL,
	[CreatedOn] [datetime] NOT NULL,
	[LastModifiedOn] [datetime] NOT NULL,
 CONSTRAINT [PK_gr_Cause] PRIMARY KEY CLUSTERED 
(
	[CauseID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

ALTER TABLE [dbo].[gr_Cause]  WITH CHECK ADD  CONSTRAINT [FK_gr_Cause_gr_CauseTemplate] FOREIGN KEY([CauseTemplateID])
REFERENCES [dbo].[gr_CauseTemplate] ([CauseTemplateID])
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[gr_Cause] CHECK CONSTRAINT [FK_gr_Cause_gr_CauseTemplate]
GO

ALTER TABLE [dbo].[gr_Cause]  WITH CHECK ADD  CONSTRAINT [FK_gr_Cause_gr_Organization] FOREIGN KEY([OrganizationID])
REFERENCES [dbo].[gr_Organization] ([OrganizationID])
GO

ALTER TABLE [dbo].[gr_Cause] CHECK CONSTRAINT [FK_gr_Cause_gr_Organization]
GO

ALTER TABLE [dbo].[gr_Cause]  WITH CHECK ADD  CONSTRAINT [FK_gr_Cause_gr_Region] FOREIGN KEY([RegionID])
REFERENCES [dbo].[gr_Region] ([RegionID])
ON DELETE SET NULL
GO

ALTER TABLE [dbo].[gr_Cause] CHECK CONSTRAINT [FK_gr_Cause_gr_Region]
GO

ALTER TABLE [dbo].[gr_Cause] ADD  CONSTRAINT [DF_gr_Cause_Active]  DEFAULT ((1)) FOR [Active]
GO

ALTER TABLE [dbo].[gr_Cause] ADD  CONSTRAINT [DF_gr_Cause_VideoEmbedHtml]  DEFAULT ('') FOR [VideoEmbedHtml]
GO

ALTER TABLE [dbo].[gr_Cause] ADD  CONSTRAINT [DF_gr_Cause_DescriptionHtml]  DEFAULT ('') FOR [DescriptionHtml]
GO

ALTER TABLE [dbo].[gr_Cause] ADD  CONSTRAINT [DF_gr_Cause_ImagePath]  DEFAULT ('') FOR [ImagePath]
GO

ALTER TABLE [dbo].[gr_Cause] ADD  CONSTRAINT [DF_gr_Cause_HoursVolunteered]  DEFAULT ((0)) FOR [HoursVolunteered]
GO

ALTER TABLE [dbo].[gr_Cause] ADD  CONSTRAINT [DF_gr_Cause_IsComplete]  DEFAULT ((0)) FOR [IsCompleted]
GO

ALTER TABLE [dbo].[gr_Cause] ADD  CONSTRAINT [DF_gr_Cause_DateCompleted]  DEFAULT (((1)/(1))/(1900)) FOR [DateCompleted]
GO

ALTER TABLE [dbo].[gr_Cause] ADD  DEFAULT ('1/1/1900') FOR [CreatedOn]
GO

ALTER TABLE [dbo].[gr_Cause] ADD  DEFAULT ('1/1/1900') FOR [LastModifiedOn]
GO

/****** Object:  Table [dbo].[gr_Recipient]    Script Date: 12/22/2011 13:03:50 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[gr_Recipient](
	[RecipientID] [int] IDENTITY(1,1) NOT NULL,
	[CauseID] [int] NOT NULL,
	[FirstName] [varchar](30) NOT NULL,
	[LastName] [varchar](30) NOT NULL,
	[Birthdate] [datetime] NOT NULL,
	[CreatedOn] [datetime] NOT NULL,
	[LastModifiedOn] [datetime] NOT NULL,
 CONSTRAINT [PK_gr_Recipient] PRIMARY KEY CLUSTERED 
(
	[RecipientID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

ALTER TABLE [dbo].[gr_Recipient]  WITH CHECK ADD  CONSTRAINT [FK_gr_Recipient_gr_Cause] FOREIGN KEY([CauseID])
REFERENCES [dbo].[gr_Cause] ([CauseID])
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[gr_Recipient] CHECK CONSTRAINT [FK_gr_Recipient_gr_Cause]
GO

ALTER TABLE [dbo].[gr_Recipient] ADD  CONSTRAINT [DF_gr_Recipient_FirstName]  DEFAULT ('') FOR [FirstName]
GO

ALTER TABLE [dbo].[gr_Recipient] ADD  CONSTRAINT [DF_gr_Recipient_LastName]  DEFAULT ('') FOR [LastName]
GO

ALTER TABLE [dbo].[gr_Recipient] ADD  CONSTRAINT [DF_gr_Recipient_Birthdate]  DEFAULT ('1/1/1900') FOR [Birthdate]
GO

ALTER TABLE [dbo].[gr_Recipient] ADD  DEFAULT ('1/1/1900') FOR [CreatedOn]
GO

ALTER TABLE [dbo].[gr_Recipient] ADD  DEFAULT ('1/1/1900') FOR [LastModifiedOn]
GO

/****** Object:  Table [dbo].[gr_CauseNote]    Script Date: 12/22/2011 13:04:25 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[gr_CauseNote](
	[CauseNoteID] [int] IDENTITY(1,1) NOT NULL,
	[CauseID] [int] NOT NULL,
	[UserProfileID] [int] NOT NULL,
	[Text] [nvarchar](max) NOT NULL,
	[EntryDate] [datetime] NOT NULL,
	[CreatedOn] [datetime] NOT NULL,
	[LastModifiedOn] [datetime] NOT NULL,
 CONSTRAINT [PK_gr_CauseNote] PRIMARY KEY CLUSTERED 
(
	[CauseNoteID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[gr_CauseNote]  WITH CHECK ADD  CONSTRAINT [FK_gr_CauseNote_gr_Cause] FOREIGN KEY([CauseID])
REFERENCES [dbo].[gr_Cause] ([CauseID])
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[gr_CauseNote] CHECK CONSTRAINT [FK_gr_CauseNote_gr_Cause]
GO

ALTER TABLE [dbo].[gr_CauseNote]  WITH CHECK ADD  CONSTRAINT [FK_gr_CauseNote_gr_UserProfile] FOREIGN KEY([UserProfileID])
REFERENCES [dbo].[gr_UserProfile] ([UserProfileID])
GO

ALTER TABLE [dbo].[gr_CauseNote] CHECK CONSTRAINT [FK_gr_CauseNote_gr_UserProfile]
GO

ALTER TABLE [dbo].[gr_CauseNote] ADD  CONSTRAINT [DF_gr_CauseNote_NoteDate]  DEFAULT (getdate()) FOR [EntryDate]
GO

ALTER TABLE [dbo].[gr_CauseNote] ADD  DEFAULT ('1/1/1900') FOR [CreatedOn]
GO

ALTER TABLE [dbo].[gr_CauseNote] ADD  DEFAULT ('1/1/1900') FOR [LastModifiedOn]
GO

/****** Object:  Table [dbo].[gr_Campaign]    Script Date: 12/22/2011 13:04:54 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[gr_Campaign](
	[CampaignID] [int] IDENTITY(1,1) NOT NULL,
	[OrganizationID] [int] NOT NULL,
	[UserProfileID] [int] NOT NULL,
	[CauseTemplateID] [int] NOT NULL,
	[CauseID] [int] NULL,
	[Title] [varchar](100) NOT NULL,
	[Description] [varchar](max) NOT NULL,
	[ImagePath] [varchar](100) NOT NULL,
	[StartDate] [datetime] NOT NULL,
	[EndDate] [datetime] NOT NULL,
	[GoalAmount] [decimal](11, 2) NOT NULL,
	[UrlSlug] [varchar](30) NOT NULL,
	[CampaignType] [int] NULL,
	[IsGeneralFund] [bit] NOT NULL,
	[CreatedOn] [datetime] NOT NULL,
	[LastModifiedOn] [datetime] NOT NULL,
 CONSTRAINT [PK_myc_Campaign] PRIMARY KEY CLUSTERED 
(
	[CampaignID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY],
 CONSTRAINT [up_UrlSlug_Unique] UNIQUE NONCLUSTERED 
(
	[UrlSlug] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

ALTER TABLE [dbo].[gr_Campaign]  WITH CHECK ADD  CONSTRAINT [FK_gr_Campaign_gr_Cause] FOREIGN KEY([CauseID])
REFERENCES [dbo].[gr_Cause] ([CauseID])
GO

ALTER TABLE [dbo].[gr_Campaign] CHECK CONSTRAINT [FK_gr_Campaign_gr_Cause]
GO

ALTER TABLE [dbo].[gr_Campaign]  WITH CHECK ADD  CONSTRAINT [FK_gr_Campaign_gr_CauseTemplate] FOREIGN KEY([CauseTemplateID])
REFERENCES [dbo].[gr_CauseTemplate] ([CauseTemplateID])
GO

ALTER TABLE [dbo].[gr_Campaign] CHECK CONSTRAINT [FK_gr_Campaign_gr_CauseTemplate]
GO

ALTER TABLE [dbo].[gr_Campaign]  WITH CHECK ADD  CONSTRAINT [FK_gr_Campaign_gr_Organization] FOREIGN KEY([OrganizationID])
REFERENCES [dbo].[gr_Organization] ([OrganizationID])
GO

ALTER TABLE [dbo].[gr_Campaign] CHECK CONSTRAINT [FK_gr_Campaign_gr_Organization]
GO

ALTER TABLE [dbo].[gr_Campaign]  WITH CHECK ADD  CONSTRAINT [FK_gr_Campaign_gr_UserProfile] FOREIGN KEY([UserProfileID])
REFERENCES [dbo].[gr_UserProfile] ([UserProfileID])
GO

ALTER TABLE [dbo].[gr_Campaign] CHECK CONSTRAINT [FK_gr_Campaign_gr_UserProfile]
GO

ALTER TABLE [dbo].[gr_Campaign] ADD  CONSTRAINT [DF_myc_Campaign_Title]  DEFAULT ('') FOR [Title]
GO

ALTER TABLE [dbo].[gr_Campaign] ADD  CONSTRAINT [DF_myc_Campaign_Description]  DEFAULT ('') FOR [Description]
GO

ALTER TABLE [dbo].[gr_Campaign] ADD  CONSTRAINT [DF_myc_Campaign_StartDate]  DEFAULT (((1)/(1))/(1900)) FOR [StartDate]
GO

ALTER TABLE [dbo].[gr_Campaign] ADD  CONSTRAINT [DF_myc_Campaign_EndDate]  DEFAULT (((1)/(1))/(1900)) FOR [EndDate]
GO

ALTER TABLE [dbo].[gr_Campaign] ADD  CONSTRAINT [DF_myc_Campaign_GoalAmount]  DEFAULT ((0)) FOR [GoalAmount]
GO

ALTER TABLE [dbo].[gr_Campaign] ADD  CONSTRAINT [DF_myc_Campaign_UrlSlug]  DEFAULT ('') FOR [UrlSlug]
GO

ALTER TABLE [dbo].[gr_Campaign] ADD  CONSTRAINT [DF_gr_Campaign_CampaignType]  DEFAULT ((-1)) FOR [CampaignType]
GO

ALTER TABLE [dbo].[gr_Campaign] ADD  CONSTRAINT [DF_gr_Campaign_IsGeneralFund]  DEFAULT ((0)) FOR [IsGeneralFund]
GO

ALTER TABLE [dbo].[gr_Campaign] ADD  DEFAULT ('1/1/1900') FOR [CreatedOn]
GO

ALTER TABLE [dbo].[gr_Campaign] ADD  DEFAULT ('1/1/1900') FOR [LastModifiedOn]
GO

/****** Object:  Table [dbo].[gr_CampaignDonor]    Script Date: 12/22/2011 13:05:23 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[gr_CampaignDonor](
	[CampaignDonorID] [int] IDENTITY(1,1) NOT NULL,
	[CampaignID] [int] NOT NULL,
	[UserProfileID] [int] NULL,
	[FirstName] [varchar](50) NOT NULL,
	[LastName] [varchar](50) NOT NULL,
	[Comments] [varchar](500) NULL,
	[Amount] [decimal](11, 2) NOT NULL,
	[Email] [varchar](30) NOT NULL,
	[PrimaryPhone] [varchar](30) NOT NULL,
	[AddressLine1] [varchar](200) NOT NULL,
	[AddressLine2] [varchar](200) NULL,
	[City] [varchar](50) NOT NULL,
	[State] [varchar](25) NOT NULL,
	[ZipCode] [varchar](20) NOT NULL,
	[DonationDate] [datetime] NOT NULL,
	[Approved] [bit] NOT NULL,
	[ReferenceID] [varchar](50) NULL,
	[Notes] [varchar](max) NULL,
	[IsAnonymous] [bit] NOT NULL,
	[DisplayName] [varchar](50) NULL,
	[CreatedOn] [datetime] NOT NULL,
	[LastModifiedOn] [datetime] NOT NULL,
 CONSTRAINT [PK_myc_CampaignDonor] PRIMARY KEY CLUSTERED 
(
	[CampaignDonorID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

ALTER TABLE [dbo].[gr_CampaignDonor]  WITH CHECK ADD  CONSTRAINT [FK_gr_CampaignDonor_gr_Campaign] FOREIGN KEY([CampaignID])
REFERENCES [dbo].[gr_Campaign] ([CampaignID])
GO

ALTER TABLE [dbo].[gr_CampaignDonor] CHECK CONSTRAINT [FK_gr_CampaignDonor_gr_Campaign]
GO

ALTER TABLE [dbo].[gr_CampaignDonor]  WITH CHECK ADD  CONSTRAINT [FK_gr_CampaignDonor_gr_UserProfile] FOREIGN KEY([UserProfileID])
REFERENCES [dbo].[gr_UserProfile] ([UserProfileID])
GO

ALTER TABLE [dbo].[gr_CampaignDonor] CHECK CONSTRAINT [FK_gr_CampaignDonor_gr_UserProfile]
GO

ALTER TABLE [dbo].[gr_CampaignDonor] ADD  CONSTRAINT [DF_myc_CampaignDonor_Name]  DEFAULT ('') FOR [FirstName]
GO

ALTER TABLE [dbo].[gr_CampaignDonor] ADD  CONSTRAINT [DF_gr_CampaignDonor_LastName]  DEFAULT ('') FOR [LastName]
GO

ALTER TABLE [dbo].[gr_CampaignDonor] ADD  CONSTRAINT [DF_myc_CampaignDonor_Comments]  DEFAULT ('') FOR [Comments]
GO

ALTER TABLE [dbo].[gr_CampaignDonor] ADD  CONSTRAINT [DF_myc_CampaignDonor_Amount]  DEFAULT ((0)) FOR [Amount]
GO

ALTER TABLE [dbo].[gr_CampaignDonor] ADD  CONSTRAINT [DF_myc_CampaignDonor_Email]  DEFAULT ('') FOR [Email]
GO

ALTER TABLE [dbo].[gr_CampaignDonor] ADD  CONSTRAINT [DF_myc_CampaignDonor_PrimaryPhone]  DEFAULT ('') FOR [PrimaryPhone]
GO

ALTER TABLE [dbo].[gr_CampaignDonor] ADD  CONSTRAINT [DF_myc_CampaignDonor_AddressLine1]  DEFAULT ('') FOR [AddressLine1]
GO

ALTER TABLE [dbo].[gr_CampaignDonor] ADD  CONSTRAINT [DF_myc_CampaignDonor_City]  DEFAULT ('') FOR [City]
GO

ALTER TABLE [dbo].[gr_CampaignDonor] ADD  CONSTRAINT [DF_myc_CampaignDonor_State]  DEFAULT ('') FOR [State]
GO

ALTER TABLE [dbo].[gr_CampaignDonor] ADD  CONSTRAINT [DF_myc_CampaignDonor_ZipCode]  DEFAULT ('') FOR [ZipCode]
GO

ALTER TABLE [dbo].[gr_CampaignDonor] ADD  CONSTRAINT [DF_myc_CampaignDonor_DonationDate]  DEFAULT (getdate()) FOR [DonationDate]
GO

ALTER TABLE [dbo].[gr_CampaignDonor] ADD  CONSTRAINT [DF_gr_CampaignDonor_Approved]  DEFAULT ((0)) FOR [Approved]
GO

ALTER TABLE [dbo].[gr_CampaignDonor] ADD  CONSTRAINT [DF_gr_CampaignDonor_IsAnonymous]  DEFAULT ((0)) FOR [IsAnonymous]
GO

ALTER TABLE [dbo].[gr_CampaignDonor] ADD  DEFAULT ('1/1/1900') FOR [CreatedOn]
GO

ALTER TABLE [dbo].[gr_CampaignDonor] ADD  DEFAULT ('1/1/1900') FOR [LastModifiedOn]
GO
COMMIT