USE [Grassroots]
GO

/****** Object:  Table [dbo].[gr_CauseTemplate]    Script Date: 12/22/2011 10:52:33 ******/
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

