USE [Grassroots]
GO
/****** Object:  Table [dbo].[gr_Region]    Script Date: 05/27/2011 16:36:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[gr_Region](
	[RegionID] [int] IDENTITY(1,1) NOT NULL,
	[CauseTemplateID] [int] NOT NULL,
	[Name] [varchar](50) NOT NULL,
	[Description] [varchar](200) NOT NULL,
 CONSTRAINT [PK_gr_Region] PRIMARY KEY CLUSTERED 
(
	[RegionID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
ALTER TABLE [dbo].[gr_Region]  WITH CHECK ADD  CONSTRAINT [FK_gr_Region_gr_CauseTemplate] FOREIGN KEY([CauseTemplateID])
REFERENCES [dbo].[gr_CauseTemplate] ([CauseTemplateID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[gr_Region] CHECK CONSTRAINT [FK_gr_Region_gr_CauseTemplate]