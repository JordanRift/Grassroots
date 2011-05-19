USE [Grassroots]
GO
/****** Object:  Table [dbo].[gr_User]    Script Date: 05/18/2011 17:11:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[gr_User](
	[UserID] [int] IDENTITY(1,1) NOT NULL,
	[Username] [varchar](50) NOT NULL,
	[Password] [varchar](2000) NOT NULL CONSTRAINT [DF_gr_User_Password]  DEFAULT (''),
	[UserProfileID] [int] NOT NULL,
	[IsActive] [bit] NOT NULL CONSTRAINT [DF_gr_User_IsActive]  DEFAULT ((0)),
	[IsAuthorized] [bit] NOT NULL CONSTRAINT [DF_gr_User_IsAuthorized]  DEFAULT ((0)),
	[ForcePasswordChange] [bit] NOT NULL CONSTRAINT [DF_gr_User_ForcePasswordChange]  DEFAULT ((0)),
	[RegisterDate] [datetime] NOT NULL CONSTRAINT [DF_gr_User_RegisterDate]  DEFAULT ('1/1/1900'),
	[LastLoggedIn] [datetime] NOT NULL CONSTRAINT [DF_gr_User_LastLoggedIn]  DEFAULT ('1/1/1900'),
 CONSTRAINT [PK_gr_User_1] PRIMARY KEY CLUSTERED 
(
	[UserID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
ALTER TABLE [dbo].[gr_User]  WITH CHECK ADD  CONSTRAINT [FK_gr_User_gr_UserProfile] FOREIGN KEY([UserProfileID])
REFERENCES [dbo].[gr_UserProfile] ([UserProfileID])
GO
ALTER TABLE [dbo].[gr_User] CHECK CONSTRAINT [FK_gr_User_gr_UserProfile]