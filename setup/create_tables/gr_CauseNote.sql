USE [Grassroots]
GO

/****** Object:  Table [dbo].[gr_CauseNote]    Script Date: 05/13/2011 11:05:55 ******/
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
 CONSTRAINT [PK_gr_CauseNote] PRIMARY KEY CLUSTERED 
(
	[CauseNoteID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[gr_CauseNote]  WITH CHECK ADD  CONSTRAINT [FK_gr_CauseNote_gr_UserProfile] FOREIGN KEY([UserProfileID])
REFERENCES [dbo].[gr_UserProfile] ([UserProfileID])
GO

ALTER TABLE [dbo].[gr_CauseNote] CHECK CONSTRAINT [FK_gr_CauseNote_gr_UserProfile]
GO

ALTER TABLE [dbo].[gr_CauseNote] ADD  CONSTRAINT [DF_gr_CauseNote_NoteDate]  DEFAULT (getdate()) FOR [EntryDate]
GO

