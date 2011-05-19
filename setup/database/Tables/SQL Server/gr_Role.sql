USE [Grassroots]
GO
/****** Object:  Table [dbo].[gr_Role]    Script Date: 05/18/2011 17:11:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[gr_Role](
	[RoleID] [int] IDENTITY(1,1) NOT NULL,
	[OrganizationID] [int] NOT NULL,
	[Name] [varchar](100) NOT NULL CONSTRAINT [DF_gr_Role_Name]  DEFAULT (''),
	[Description] [varchar](500) NOT NULL CONSTRAINT [DF_gr_Role_Description]  DEFAULT (''),
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