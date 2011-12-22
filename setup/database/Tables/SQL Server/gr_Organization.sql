USE [Grassroots]
GO

/****** Object:  Table [dbo].[gr_Organization]    Script Date: 12/22/2011 10:52:42 ******/
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

