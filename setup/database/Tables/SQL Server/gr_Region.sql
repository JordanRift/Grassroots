USE [Grassroots]
GO

/****** Object:  Table [dbo].[gr_Region]    Script Date: 12/22/2011 10:53:27 ******/
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

