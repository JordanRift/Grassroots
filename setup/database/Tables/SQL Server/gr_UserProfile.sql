USE [Grassroots]
GO

/****** Object:  Table [dbo].[gr_UserProfile]    Script Date: 12/22/2011 10:54:08 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[gr_UserProfile](
	[UserProfileID] [int] IDENTITY(1,1) NOT NULL,
	[OrganizationID] [int] NOT NULL,
	[RoleID] [int] NULL,
	[FacebookID] [varchar](50) NULL,
	[FirstName] [varchar](30) NOT NULL,
	[LastName] [varchar](30) NOT NULL,
	[Birthdate] [datetime] NOT NULL,
	[Gender] [varchar](20) NOT NULL,
	[Email] [varchar](100) NOT NULL,
	[PrimaryPhone] [varchar](30) NULL,
	[AddressLine1] [varchar](200) NULL,
	[AddressLine2] [varchar](200) NULL,
	[City] [varchar](100) NULL,
	[State] [varchar](100) NULL,
	[ZipCode] [varchar](20) NOT NULL,
	[Consent] [bit] NOT NULL,
	[Active] [bit] NOT NULL,
	[IsActivated] [bit] NOT NULL,
	[ImagePath] [varchar](100) NULL,
	[ActivationHash] [varchar](500) NULL,
	[ActivationPin] [varchar](12) NULL,
	[LastActivationAttempt] [datetime] NOT NULL,
	[CreatedOn] [datetime] NOT NULL,
	[LastModifiedOn] [datetime] NOT NULL,
 CONSTRAINT [PK_myc_UserProfile] PRIMARY KEY CLUSTERED 
(
	[UserProfileID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

ALTER TABLE [dbo].[gr_UserProfile]  WITH CHECK ADD  CONSTRAINT [FK_gr_UserProfile_gr_Organization] FOREIGN KEY([OrganizationID])
REFERENCES [dbo].[gr_Organization] ([OrganizationID])
GO

ALTER TABLE [dbo].[gr_UserProfile] CHECK CONSTRAINT [FK_gr_UserProfile_gr_Organization]
GO

ALTER TABLE [dbo].[gr_UserProfile]  WITH CHECK ADD  CONSTRAINT [FK_gr_UserProfile_gr_Role] FOREIGN KEY([RoleID])
REFERENCES [dbo].[gr_Role] ([RoleID])
GO

ALTER TABLE [dbo].[gr_UserProfile] CHECK CONSTRAINT [FK_gr_UserProfile_gr_Role]
GO

ALTER TABLE [dbo].[gr_UserProfile] ADD  CONSTRAINT [DF_myc_UserProfile_FirstName]  DEFAULT ('') FOR [FirstName]
GO

ALTER TABLE [dbo].[gr_UserProfile] ADD  CONSTRAINT [DF_myc_UserProfile_LastName]  DEFAULT ('') FOR [LastName]
GO

ALTER TABLE [dbo].[gr_UserProfile] ADD  CONSTRAINT [DF_myc_UserProfile_Birthdate]  DEFAULT ('1/1/1900') FOR [Birthdate]
GO

ALTER TABLE [dbo].[gr_UserProfile] ADD  CONSTRAINT [DF_myc_UserProfile_Gender]  DEFAULT ('') FOR [Gender]
GO

ALTER TABLE [dbo].[gr_UserProfile] ADD  CONSTRAINT [DF_myc_UserProfile_Email]  DEFAULT ('') FOR [Email]
GO

ALTER TABLE [dbo].[gr_UserProfile] ADD  CONSTRAINT [DF_myc_UserProfile_PrimaryPhone]  DEFAULT ('') FOR [PrimaryPhone]
GO

ALTER TABLE [dbo].[gr_UserProfile] ADD  CONSTRAINT [DF_myc_UserProfile_AddressLine1]  DEFAULT ('') FOR [AddressLine1]
GO

ALTER TABLE [dbo].[gr_UserProfile] ADD  CONSTRAINT [DF_myc_UserProfile_AddressLine2]  DEFAULT ('') FOR [AddressLine2]
GO

ALTER TABLE [dbo].[gr_UserProfile] ADD  CONSTRAINT [DF_myc_UserProfile_City]  DEFAULT ('') FOR [City]
GO

ALTER TABLE [dbo].[gr_UserProfile] ADD  CONSTRAINT [DF_myc_UserProfile_State]  DEFAULT ('') FOR [State]
GO

ALTER TABLE [dbo].[gr_UserProfile] ADD  CONSTRAINT [DF_myc_UserProfile_ZipCode]  DEFAULT ('') FOR [ZipCode]
GO

ALTER TABLE [dbo].[gr_UserProfile] ADD  CONSTRAINT [DF_myc_UserProfile_ParentalConsent]  DEFAULT ((0)) FOR [Consent]
GO

ALTER TABLE [dbo].[gr_UserProfile] ADD  CONSTRAINT [DF_gr_UserProfile_Active]  DEFAULT ((1)) FOR [Active]
GO

ALTER TABLE [dbo].[gr_UserProfile] ADD  CONSTRAINT [DF_gr_UserProfile_IsActivated]  DEFAULT ((0)) FOR [IsActivated]
GO

ALTER TABLE [dbo].[gr_UserProfile] ADD  CONSTRAINT [DF_gr_UserProfile_ImagePath]  DEFAULT ('content/images/avatar.jpg') FOR [ImagePath]
GO

ALTER TABLE [dbo].[gr_UserProfile] ADD  CONSTRAINT [DF_gr_UserProfile_LastActivationAttempt]  DEFAULT ('1/1/1900') FOR [LastActivationAttempt]
GO

ALTER TABLE [dbo].[gr_UserProfile] ADD  DEFAULT ('1/1/1900') FOR [CreatedOn]
GO

ALTER TABLE [dbo].[gr_UserProfile] ADD  DEFAULT ('1/1/1900') FOR [LastModifiedOn]
GO

