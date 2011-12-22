USE [Grassroots]
GO

/****** Object:  Table [dbo].[gr_Campaign]    Script Date: 12/22/2011 10:51:49 ******/
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

