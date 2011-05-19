USE [Grassroots]
GO
/****** Object:  Table [dbo].[gr_CampaignDonor]    Script Date: 05/18/2011 17:09:45 ******/
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
	[FirstName] [varchar](50) NOT NULL CONSTRAINT [DF_myc_CampaignDonor_Name]  DEFAULT (''),
	[LastName] [varchar](50) NOT NULL CONSTRAINT [DF_gr_CampaignDonor_LastName]  DEFAULT (''),
	[Comments] [varchar](500) NULL CONSTRAINT [DF_myc_CampaignDonor_Comments]  DEFAULT (''),
	[Amount] [decimal](11, 2) NOT NULL CONSTRAINT [DF_myc_CampaignDonor_Amount]  DEFAULT ((0)),
	[Email] [varchar](30) NOT NULL CONSTRAINT [DF_myc_CampaignDonor_Email]  DEFAULT (''),
	[PrimaryPhone] [varchar](30) NOT NULL CONSTRAINT [DF_myc_CampaignDonor_PrimaryPhone]  DEFAULT (''),
	[AddressLine1] [varchar](200) NOT NULL CONSTRAINT [DF_myc_CampaignDonor_AddressLine1]  DEFAULT (''),
	[AddressLine2] [varchar](200) NULL,
	[City] [varchar](50) NOT NULL CONSTRAINT [DF_myc_CampaignDonor_City]  DEFAULT (''),
	[State] [varchar](25) NOT NULL CONSTRAINT [DF_myc_CampaignDonor_State]  DEFAULT (''),
	[ZipCode] [varchar](20) NOT NULL CONSTRAINT [DF_myc_CampaignDonor_ZipCode]  DEFAULT (''),
	[DonationDate] [datetime] NOT NULL CONSTRAINT [DF_myc_CampaignDonor_DonationDate]  DEFAULT (getdate()),
	[Approved] [bit] NOT NULL CONSTRAINT [DF_gr_CampaignDonor_Approved]  DEFAULT ((0)),
	[ReferenceID] [varchar](50) NULL,
	[Notes] [varchar](max) NULL,
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