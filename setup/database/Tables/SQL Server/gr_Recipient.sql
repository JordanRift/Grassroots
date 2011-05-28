USE [Grassroots]
GO
/****** Object:  Table [dbo].[gr_Recipient]    Script Date: 05/27/2011 17:18:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[gr_Recipient](
	[RecipientID] [int] IDENTITY(1,1) NOT NULL,
	[CauseID] [int] NOT NULL,
	[FirstName] [varchar](30) NOT NULL CONSTRAINT [DF_gr_Recipient_FirstName]  DEFAULT (''),
	[LastName] [varchar](30) NOT NULL CONSTRAINT [DF_gr_Recipient_LastName]  DEFAULT (''),
	[Birthdate] [datetime] NOT NULL CONSTRAINT [DF_gr_Recipient_Birthdate]  DEFAULT ('1/1/1900'),
 CONSTRAINT [PK_gr_Recipient] PRIMARY KEY CLUSTERED 
(
	[RecipientID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
ALTER TABLE [dbo].[gr_Recipient]  WITH CHECK ADD  CONSTRAINT [FK_gr_Recipient_gr_Cause] FOREIGN KEY([CauseID])
REFERENCES [dbo].[gr_Cause] ([CauseID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[gr_Recipient] CHECK CONSTRAINT [FK_gr_Recipient_gr_Cause]