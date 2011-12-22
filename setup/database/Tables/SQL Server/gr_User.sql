USE [Grassroots]
GO

/****** Object:  Table [dbo].[gr_User]    Script Date: 12/22/2011 10:53:53 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[gr_User](
	[UserID] [int] IDENTITY(1,1) NOT NULL,
	[Username] [varchar](50) NOT NULL,
	[Password] [varchar](2000) NOT NULL,
	[UserProfileID] [int] NOT NULL,
	[IsActive] [bit] NOT NULL,
	[IsAuthorized] [bit] NOT NULL,
	[ForcePasswordChange] [bit] NOT NULL,
	[RegisterDate] [datetime] NOT NULL,
	[LastLoggedIn] [datetime] NOT NULL,
	[FailedLoginAttempts] [int] NOT NULL,
	[CreatedOn] [datetime] NOT NULL,
	[LastModifiedOn] [datetime] NOT NULL,
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
GO

ALTER TABLE [dbo].[gr_User] ADD  CONSTRAINT [DF_gr_User_Password]  DEFAULT ('') FOR [Password]
GO

ALTER TABLE [dbo].[gr_User] ADD  CONSTRAINT [DF_gr_User_IsActive]  DEFAULT ((0)) FOR [IsActive]
GO

ALTER TABLE [dbo].[gr_User] ADD  CONSTRAINT [DF_gr_User_IsAuthorized]  DEFAULT ((0)) FOR [IsAuthorized]
GO

ALTER TABLE [dbo].[gr_User] ADD  CONSTRAINT [DF_gr_User_ForcePasswordChange]  DEFAULT ((0)) FOR [ForcePasswordChange]
GO

ALTER TABLE [dbo].[gr_User] ADD  CONSTRAINT [DF_gr_User_RegisterDate]  DEFAULT ('1/1/1900') FOR [RegisterDate]
GO

ALTER TABLE [dbo].[gr_User] ADD  CONSTRAINT [DF_gr_User_LastLoggedIn]  DEFAULT ('1/1/1900') FOR [LastLoggedIn]
GO

ALTER TABLE [dbo].[gr_User] ADD  CONSTRAINT [DF_gr_User_FailedLoginAttempts]  DEFAULT ((0)) FOR [FailedLoginAttempts]
GO

ALTER TABLE [dbo].[gr_User] ADD  DEFAULT ('1/1/1900') FOR [CreatedOn]
GO

ALTER TABLE [dbo].[gr_User] ADD  DEFAULT ('1/1/1900') FOR [LastModifiedOn]
GO

