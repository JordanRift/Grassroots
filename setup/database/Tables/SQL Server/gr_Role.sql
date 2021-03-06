USE [Grassroots]
GO

/****** Object:  Table [dbo].[gr_Role]    Script Date: 12/22/2011 10:53:38 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[gr_Role](
	[RoleID] [int] IDENTITY(1,1) NOT NULL,
	[OrganizationID] [int] NOT NULL,
	[Name] [varchar](100) NOT NULL,
	[Description] [varchar](500) NOT NULL,
	[CreatedOn] [datetime] NOT NULL,
	[LastModifiedOn] [datetime] NOT NULL,
 CONSTRAINT [PK_gr_Role] PRIMARY KEY CLUSTERED 
(
	[RoleID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

ALTER TABLE [dbo].[gr_Role]  WITH CHECK ADD  CONSTRAINT [FK_gr_Role_gr_Organization] FOREIGN KEY([OrganizationID])
REFERENCES [dbo].[gr_Organization] ([OrganizationID])
GO

ALTER TABLE [dbo].[gr_Role] CHECK CONSTRAINT [FK_gr_Role_gr_Organization]
GO

ALTER TABLE [dbo].[gr_Role] ADD  CONSTRAINT [DF_gr_Role_Name]  DEFAULT ('') FOR [Name]
GO

ALTER TABLE [dbo].[gr_Role] ADD  CONSTRAINT [DF_gr_Role_Description]  DEFAULT ('') FOR [Description]
GO

ALTER TABLE [dbo].[gr_Role] ADD  DEFAULT ('1/1/1900') FOR [CreatedOn]
GO

ALTER TABLE [dbo].[gr_Role] ADD  DEFAULT ('1/1/1900') FOR [LastModifiedOn]
GO

