USE [Grassroots]
GO
/****** Object:  Table [dbo].[gr_OrganizationSetting]    Script Date: 05/18/2011 17:11:27 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[gr_OrganizationSetting](
	[OrganizationSettingID] [int] IDENTITY(1,1) NOT NULL,
	[OrganizationID] [int] NULL,
	[Name] [varchar](100) NOT NULL CONSTRAINT [DF_myc_OrganizationSetting_Name]  DEFAULT (''),
	[Value] [varchar](max) NOT NULL CONSTRAINT [DF_myc_OrganizationSetting_Value]  DEFAULT (''),
	[DataType] [int] NOT NULL CONSTRAINT [DF_myc_OrganizationSetting_DataType]  DEFAULT ((0)),
 CONSTRAINT [PK_myc_OrganizationSetting] PRIMARY KEY CLUSTERED 
(
	[OrganizationSettingID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
ALTER TABLE [dbo].[gr_OrganizationSetting]  WITH CHECK ADD  CONSTRAINT [FK_myc_OrganizationSetting_myc_Organization] FOREIGN KEY([OrganizationID])
REFERENCES [dbo].[gr_Organization] ([OrganizationID])
GO
ALTER TABLE [dbo].[gr_OrganizationSetting] CHECK CONSTRAINT [FK_myc_OrganizationSetting_myc_Organization]