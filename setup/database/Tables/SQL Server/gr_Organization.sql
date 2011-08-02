USE [Grassroots]
GO
/****** Object:  Table [dbo].[gr_Organization]    Script Date: 08/02/2011 09:14:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[gr_Organization](
	[OrganizationID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](50) NOT NULL CONSTRAINT [DF_myc_Organization_Name]  DEFAULT (''),
	[Tagline] [varchar](150) NOT NULL CONSTRAINT [DF_gr_Organization_Tagline]  DEFAULT (''),
	[SummaryHtml] [varchar](max) NULL,
	[DescriptionHtml] [varchar](max) NULL,
	[ContactPhone] [varchar](20) NOT NULL CONSTRAINT [DF_myc_Organization_ContactPhone]  DEFAULT (''),
	[ContactEmail] [varchar](30) NOT NULL CONSTRAINT [DF_myc_Organization_ContactEmail]  DEFAULT (''),
	[YtdGoal] [decimal](18, 2) NULL CONSTRAINT [DF_gr_Organization_YtdGoal]  DEFAULT ((0)),
	[FiscalYearStartMonth] [int] NULL CONSTRAINT [DF_gr_Organization_FiscalYearStartMonth]  DEFAULT ((1)),
	[FiscalYearStartDay] [int] NULL CONSTRAINT [DF_gr_Organization_FiscalYearStartDay]  DEFAULT ((1)),
	[PaymentGatewayType] [int] NOT NULL CONSTRAINT [DF_gr_Organization_PaymentGatewayType]  DEFAULT ((-1)),
	[PaymentGatewayApiUrl] [varchar](100) NULL,
	[PaymentGatewayApiKey] [varchar](100) NOT NULL CONSTRAINT [DF_gr_Organization_PaymentGatewayApiKey]  DEFAULT (''),
	[PaymentGatewayApiSecret] [varchar](100) NOT NULL CONSTRAINT [DF_gr_Organization_PaymentGatewayApiSecret]  DEFAULT (''),
	[FacebookPageUrl] [varchar](200) NOT NULL CONSTRAINT [DF_gr_Organization_FacebookPageUrl]  DEFAULT (''),
	[VideoEmbedHtml] [varchar](1000) NOT NULL CONSTRAINT [DF_gr_Organization_VideoEmbedHtml]  DEFAULT (''),
	[TwitterName] [varchar](50) NULL CONSTRAINT [DF_gr_Organization_TwitterName]  DEFAULT (''),
	[BlogRssUrl] [varchar](250) NULL,
	[ThemeName] [varchar](50) NOT NULL CONSTRAINT [DF_gr_Organization_ThemeName]  DEFAULT (''),
 CONSTRAINT [PK_myc_Organization] PRIMARY KEY CLUSTERED 
(
	[OrganizationID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF