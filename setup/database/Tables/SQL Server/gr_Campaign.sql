USE [Grassroots]
GO
/****** Object:  Table [dbo].[gr_Campaign]    Script Date: 05/27/2011 16:34:55 ******/
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
	[Title] [varchar](100) NOT NULL CONSTRAINT [DF_myc_Campaign_Title]  DEFAULT (''),
	[Description] [varchar](max) NOT NULL CONSTRAINT [DF_myc_Campaign_Description]  DEFAULT (''),
	[ImagePath] [varchar](100) NOT NULL,
	[StartDate] [datetime] NOT NULL CONSTRAINT [DF_myc_Campaign_StartDate]  DEFAULT (((1)/(1))/(1900)),
	[EndDate] [datetime] NOT NULL CONSTRAINT [DF_myc_Campaign_EndDate]  DEFAULT (((1)/(1))/(1900)),
	[GoalAmount] [decimal](11, 2) NOT NULL CONSTRAINT [DF_myc_Campaign_GoalAmount]  DEFAULT ((0)),
	[UrlSlug] [varchar](30) NOT NULL CONSTRAINT [DF_myc_Campaign_UrlSlug]  DEFAULT (''),
	[CampaignType] [int] NULL CONSTRAINT [DF_gr_Campaign_CampaignType]  DEFAULT ((-1)),
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