USE [Grassroots]
GO
/****** Object:  Table [dbo].[gr_UserProfile]    Script Date: 06/17/2011 11:46:28 ******/
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
	[FirstName] [varchar](30) NOT NULL CONSTRAINT [DF_myc_UserProfile_FirstName]  DEFAULT (''),
	[LastName] [varchar](30) NOT NULL CONSTRAINT [DF_myc_UserProfile_LastName]  DEFAULT (''),
	[Birthdate] [datetime] NOT NULL CONSTRAINT [DF_myc_UserProfile_Birthdate]  DEFAULT ('1/1/1900'),
	[Gender] [varchar](20) NOT NULL CONSTRAINT [DF_myc_UserProfile_Gender]  DEFAULT (''),
	[Email] [varchar](50) NOT NULL CONSTRAINT [DF_myc_UserProfile_Email]  DEFAULT (''),
	[PrimaryPhone] [varchar](30) NULL CONSTRAINT [DF_myc_UserProfile_PrimaryPhone]  DEFAULT (''),
	[AddressLine1] [varchar](200) NULL CONSTRAINT [DF_myc_UserProfile_AddressLine1]  DEFAULT (''),
	[AddressLine2] [varchar](200) NULL CONSTRAINT [DF_myc_UserProfile_AddressLine2]  DEFAULT (''),
	[City] [varchar](100) NULL CONSTRAINT [DF_myc_UserProfile_City]  DEFAULT (''),
	[State] [varchar](100) NULL CONSTRAINT [DF_myc_UserProfile_State]  DEFAULT (''),
	[ZipCode] [varchar](20) NOT NULL CONSTRAINT [DF_myc_UserProfile_ZipCode]  DEFAULT (''),
	[Consent] [bit] NOT NULL CONSTRAINT [DF_myc_UserProfile_ParentalConsent]  DEFAULT ((0)),
	[Active] [bit] NOT NULL CONSTRAINT [DF_gr_UserProfile_Active]  DEFAULT ((1)),
	[IsActivated] [bit] NOT NULL CONSTRAINT [DF_gr_UserProfile_IsActivated]  DEFAULT ((0)),
	[ImagePath] [varchar](100) NULL CONSTRAINT [DF_gr_UserProfile_ImagePath]  DEFAULT ('content/images/avatar.jpg'),
	[ActivationHash] [varchar](500) NULL,
	[LastActivationAttempt] [datetime] NOT NULL CONSTRAINT [DF_gr_UserProfile_LastActivationAttempt]  DEFAULT ('1/1/1900'),
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