USE [Grassroots]
GO

/****** Object:  Table [dbo].[gr_CampaignDonor]    Script Date: 12/22/2011 10:52:01 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[gr_CampaignDonor](
	[CampaignDonorID] [int] IDENTITY(1,1) NOT NULL,
	[CampaignID] [int] NOT NULL,
	[UserProfileID] [int] NULL,
	[FirstName] [varchar](50) NOT NULL,
	[LastName] [varchar](50) NOT NULL,
	[Comments] [varchar](500) NULL,
	[Amount] [decimal](11, 2) NOT NULL,
	[Email] [varchar](100) NOT NULL,
	[PrimaryPhone] [varchar](30) NOT NULL,
	[AddressLine1] [varchar](200) NOT NULL,
	[AddressLine2] [varchar](200) NULL,
	[City] [varchar](50) NOT NULL,
	[State] [varchar](25) NOT NULL,
	[ZipCode] [varchar](20) NOT NULL,
	[DonationDate] [datetime] NOT NULL,
	[Approved] [bit] NOT NULL,
	[ReferenceID] [varchar](50) NULL,
	[Notes] [varchar](max) NULL,
	[IsAnonymous] [bit] NOT NULL,
	[DisplayName] [varchar](50) NULL,
	[CreatedOn] [datetime] NOT NULL,
	[LastModifiedOn] [datetime] NOT NULL,
 CONSTRAINT [PK_myc_CampaignDonor] PRIMARY KEY CLUSTERED 
(
	[CampaignDonorID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

ALTER TABLE [dbo].[gr_CampaignDonor]  WITH CHECK ADD  CONSTRAINT [FK_gr_CampaignDonor_gr_Campaign] FOREIGN KEY([CampaignID])
REFERENCES [dbo].[gr_Campaign] ([CampaignID])
GO

ALTER TABLE [dbo].[gr_CampaignDonor] CHECK CONSTRAINT [FK_gr_CampaignDonor_gr_Campaign]
GO

ALTER TABLE [dbo].[gr_CampaignDonor]  WITH CHECK ADD  CONSTRAINT [FK_gr_CampaignDonor_gr_UserProfile] FOREIGN KEY([UserProfileID])
REFERENCES [dbo].[gr_UserProfile] ([UserProfileID])
GO

ALTER TABLE [dbo].[gr_CampaignDonor] CHECK CONSTRAINT [FK_gr_CampaignDonor_gr_UserProfile]
GO

ALTER TABLE [dbo].[gr_CampaignDonor] ADD  CONSTRAINT [DF_myc_CampaignDonor_Name]  DEFAULT ('') FOR [FirstName]
GO

ALTER TABLE [dbo].[gr_CampaignDonor] ADD  CONSTRAINT [DF_gr_CampaignDonor_LastName]  DEFAULT ('') FOR [LastName]
GO

ALTER TABLE [dbo].[gr_CampaignDonor] ADD  CONSTRAINT [DF_myc_CampaignDonor_Comments]  DEFAULT ('') FOR [Comments]
GO

ALTER TABLE [dbo].[gr_CampaignDonor] ADD  CONSTRAINT [DF_myc_CampaignDonor_Amount]  DEFAULT ((0)) FOR [Amount]
GO

ALTER TABLE [dbo].[gr_CampaignDonor] ADD  CONSTRAINT [DF_myc_CampaignDonor_Email]  DEFAULT ('') FOR [Email]
GO

ALTER TABLE [dbo].[gr_CampaignDonor] ADD  CONSTRAINT [DF_myc_CampaignDonor_PrimaryPhone]  DEFAULT ('') FOR [PrimaryPhone]
GO

ALTER TABLE [dbo].[gr_CampaignDonor] ADD  CONSTRAINT [DF_myc_CampaignDonor_AddressLine1]  DEFAULT ('') FOR [AddressLine1]
GO

ALTER TABLE [dbo].[gr_CampaignDonor] ADD  CONSTRAINT [DF_myc_CampaignDonor_City]  DEFAULT ('') FOR [City]
GO

ALTER TABLE [dbo].[gr_CampaignDonor] ADD  CONSTRAINT [DF_myc_CampaignDonor_State]  DEFAULT ('') FOR [State]
GO

ALTER TABLE [dbo].[gr_CampaignDonor] ADD  CONSTRAINT [DF_myc_CampaignDonor_ZipCode]  DEFAULT ('') FOR [ZipCode]
GO

ALTER TABLE [dbo].[gr_CampaignDonor] ADD  CONSTRAINT [DF_myc_CampaignDonor_DonationDate]  DEFAULT (getdate()) FOR [DonationDate]
GO

ALTER TABLE [dbo].[gr_CampaignDonor] ADD  CONSTRAINT [DF_gr_CampaignDonor_Approved]  DEFAULT ((0)) FOR [Approved]
GO

ALTER TABLE [dbo].[gr_CampaignDonor] ADD  CONSTRAINT [DF_gr_CampaignDonor_IsAnonymous]  DEFAULT ((0)) FOR [IsAnonymous]
GO

ALTER TABLE [dbo].[gr_CampaignDonor] ADD  DEFAULT ('1/1/1900') FOR [CreatedOn]
GO

ALTER TABLE [dbo].[gr_CampaignDonor] ADD  DEFAULT ('1/1/1900') FOR [LastModifiedOn]
GO

