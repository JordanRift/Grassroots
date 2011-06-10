USE [Grassroots]
GO
/****** Object:  Table [dbo].[gr_Cause]    Script Date: 06/10/2011 12:23:45 ******/
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
	[Active] [bit] NOT NULL CONSTRAINT [DF_gr_Cause_Active]  DEFAULT ((1)),
	[Summary] [varchar](500) NOT NULL,
	[VideoEmbedHtml] [varchar](200) NOT NULL CONSTRAINT [DF_gr_Cause_VideoEmbedHtml]  DEFAULT (''),
	[DescriptionHtml] [varchar](max) NOT NULL CONSTRAINT [DF_gr_Cause_DescriptionHtml]  DEFAULT (''),
	[ImagePath] [varchar](100) NOT NULL CONSTRAINT [DF_gr_Cause_ImagePath]  DEFAULT (''),
	[HoursVolunteered] [int] NULL CONSTRAINT [DF_gr_Cause_HoursVolunteered]  DEFAULT ((0)),
	[BeforeImagePath] [varchar](100) NULL,
	[AfterImagePath] [varchar](100) NULL,
	[Latitude] [decimal](18, 15) NULL,
	[Longitude] [decimal](18, 15) NULL,
	[ReferenceNumber] [varchar](50) NULL,
	[IsCompleted] [bit] NOT NULL CONSTRAINT [DF_gr_Cause_IsComplete]  DEFAULT ((0)),
	[DateCompleted] [datetime] NULL CONSTRAINT [DF_gr_Cause_DateCompleted]  DEFAULT (((1)/(1))/(1900)),
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