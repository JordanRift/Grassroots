USE [Grassroots]
GO
/****** Object:  Table [dbo].[gr_CauseTemplate]    Script Date: 06/03/2011 10:02:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[gr_CauseTemplate](
	[CauseTemplateID] [int] IDENTITY(1,1) NOT NULL,
	[OrganizationID] [int] NOT NULL,
	[Name] [varchar](100) NOT NULL CONSTRAINT [DF_gr_CauseType_Name]  DEFAULT (''),
	[ActionVerb] [varchar](50) NOT NULL CONSTRAINT [DF_gr_CauseType_ActionVerb]  DEFAULT (''),
	[GoalName] [varchar](50) NOT NULL CONSTRAINT [DF_gr_CauseType_GoalName]  DEFAULT (''),
	[Active] [bit] NOT NULL CONSTRAINT [DF_gr_CauseType_Active]  DEFAULT ((1)),
	[AmountIsConfigurable] [bit] NOT NULL CONSTRAINT [DF_gr_CauseType_AmountIsConfigurable]  DEFAULT ((0)),
	[DefaultAmount] [decimal](7, 2) NOT NULL CONSTRAINT [DF_gr_CauseType_DefaultAmount]  DEFAULT ((0)),
	[TimespanIsConfigurable] [bit] NOT NULL CONSTRAINT [DF_gr_CauseType_TimespanIsConfigurable]  DEFAULT ((0)),
	[DefaultTimespanInDays] [int] NOT NULL CONSTRAINT [DF_gr_CauseType_DefaultTimespanInDays]  DEFAULT ((0)),
	[Summary] [varchar](500) NOT NULL CONSTRAINT [DF_gr_CauseType_Summary]  DEFAULT (''),
	[VideoEmbedHtml] [varchar](200) NOT NULL CONSTRAINT [DF_gr_CauseType_VideoEmbedHtml]  DEFAULT (''),
	[DescriptionHtml] [varchar](max) NOT NULL CONSTRAINT [DF_gr_CauseType_DescriptionHtml]  DEFAULT (''),
	[ImagePath] [varchar](50) NOT NULL CONSTRAINT [DF_gr_CauseTemplate_ImagePath]  DEFAULT (''),
	[BeforeImagePath] [varchar](100) NULL,
	[AfterImagePath] [varchar](100) NULL,
	[InstructionsOpenHtml] [varchar](max) NOT NULL CONSTRAINT [DF_gr_CauseTemplate_InstructionsOpenHtml]  DEFAULT (''),
	[InstructionsClosedHtml] [varchar](max) NOT NULL CONSTRAINT [DF_gr_CauseTemplate_InstructionsClosedHtml]  DEFAULT (''),
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