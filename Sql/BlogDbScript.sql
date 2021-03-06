USE [BlogManagement]
GO
/****** Object:  Table [dbo].[Roles]    Script Date: 08/22/2016 22:27:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Roles](
	[RoleID] [smallint] IDENTITY(1,1) NOT NULL,
	[RoleName] [varchar](50) NULL,
	[isActive] [bit] NULL,
 CONSTRAINT [PK_Roles] PRIMARY KEY CLUSTERED 
(
	[RoleID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
SET IDENTITY_INSERT [dbo].[Roles] ON
INSERT [dbo].[Roles] ([RoleID], [RoleName], [isActive]) VALUES (1, N'SuperAdmin', 1)
INSERT [dbo].[Roles] ([RoleID], [RoleName], [isActive]) VALUES (2, N'Admin', 1)
SET IDENTITY_INSERT [dbo].[Roles] OFF
/****** Object:  Table [dbo].[Users]    Script Date: 08/22/2016 22:27:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Users](
	[UserID] [int] IDENTITY(1,1) NOT NULL,
	[UserName] [varchar](200) NULL,
	[UserFirstName] [varchar](100) NULL,
	[UserLastName] [varchar](100) NULL,
	[UserPassword] [varchar](20) NULL,
	[CreatedDate] [datetime] NULL,
	[IP] [varchar](50) NULL,
	[isActive] [bit] NULL,
	[UserRole] [smallint] NULL,
 CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED 
(
	[UserID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
SET IDENTITY_INSERT [dbo].[Users] ON
INSERT [dbo].[Users] ([UserID], [UserName], [UserFirstName], [UserLastName], [UserPassword], [CreatedDate], [IP], [isActive], [UserRole]) VALUES (2, N'SuperAdmin', N'Emanuel', N'Ashiedu', N'1234', CAST(0x0000A667016727FB AS DateTime), N'::1', 1, 1)
SET IDENTITY_INSERT [dbo].[Users] OFF
/****** Object:  Table [dbo].[Blogs]    Script Date: 08/22/2016 22:27:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Blogs](
	[BlogID] [int] IDENTITY(1,1) NOT NULL,
	[BlogTitle] [nvarchar](500) NULL,
	[BlogSubTitle] [nvarchar](500) NULL,
	[BlogText] [ntext] NULL,
	[BlogCreatedDate] [datetime] NULL,
	[isActive] [bit] NULL,
	[CreatedBy] [int] NULL,
	[IP] [varchar](20) NULL,
	[BlogUpdatedDate] [datetime] NULL,
	[BlogUpdatedBy] [int] NULL,
 CONSTRAINT [PK_tblBlogs] PRIMARY KEY CLUSTERED 
(
	[BlogID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Default [DF_Blogs_BlogCreatedDate]    Script Date: 08/22/2016 22:27:46 ******/
ALTER TABLE [dbo].[Blogs] ADD  CONSTRAINT [DF_Blogs_BlogCreatedDate]  DEFAULT (getdate()) FOR [BlogCreatedDate]
GO
/****** Object:  Default [DF_tblBlogs_isActive]    Script Date: 08/22/2016 22:27:46 ******/
ALTER TABLE [dbo].[Blogs] ADD  CONSTRAINT [DF_tblBlogs_isActive]  DEFAULT ((1)) FOR [isActive]
GO
/****** Object:  Default [DF_Roles_isActive]    Script Date: 08/22/2016 22:27:46 ******/
ALTER TABLE [dbo].[Roles] ADD  CONSTRAINT [DF_Roles_isActive]  DEFAULT ((1)) FOR [isActive]
GO
/****** Object:  Default [DF_Users_CreatedDate]    Script Date: 08/22/2016 22:27:46 ******/
ALTER TABLE [dbo].[Users] ADD  CONSTRAINT [DF_Users_CreatedDate]  DEFAULT (getdate()) FOR [CreatedDate]
GO
/****** Object:  Default [DF_Users_isActive]    Script Date: 08/22/2016 22:27:46 ******/
ALTER TABLE [dbo].[Users] ADD  CONSTRAINT [DF_Users_isActive]  DEFAULT ((1)) FOR [isActive]
GO
/****** Object:  ForeignKey [FK_Blogs_CreatedBy]    Script Date: 08/22/2016 22:27:46 ******/
ALTER TABLE [dbo].[Blogs]  WITH CHECK ADD  CONSTRAINT [FK_Blogs_CreatedBy] FOREIGN KEY([CreatedBy])
REFERENCES [dbo].[Users] ([UserID])
GO
ALTER TABLE [dbo].[Blogs] CHECK CONSTRAINT [FK_Blogs_CreatedBy]
GO
/****** Object:  ForeignKey [FK_Users_Role]    Script Date: 08/22/2016 22:27:46 ******/
ALTER TABLE [dbo].[Users]  WITH CHECK ADD  CONSTRAINT [FK_Users_Role] FOREIGN KEY([UserRole])
REFERENCES [dbo].[Roles] ([RoleID])
GO
ALTER TABLE [dbo].[Users] CHECK CONSTRAINT [FK_Users_Role]
GO
