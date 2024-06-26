USE [lanstaller]
GO
/****** Object:  User [lanstaller]    Script Date: 30/06/2024 1:26:23 AM ******/
CREATE USER [lanstaller] FOR LOGIN [lanstaller] WITH DEFAULT_SCHEMA=[dbo]
GO
/****** Object:  User [lanstallerapi]    Script Date: 30/06/2024 1:26:23 AM ******/
CREATE USER [lanstallerapi] FOR LOGIN [lanstallerapi] WITH DEFAULT_SCHEMA=[dbo]
GO
ALTER ROLE [db_owner] ADD MEMBER [lanstaller]
GO
/****** Object:  Table [dbo].[tblCompatibility]    Script Date: 30/06/2024 1:26:24 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblCompatibility](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[software_id] [int] NOT NULL,
	[filename] [nvarchar](max) NOT NULL,
	[compat_type] [int] NOT NULL,
 CONSTRAINT [PK_tblCompatibility] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tblDirectories]    Script Date: 30/06/2024 1:26:24 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblDirectories](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[software_id] [int] NOT NULL,
	[path] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_tblDirectories] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tblFiles]    Script Date: 30/06/2024 1:26:24 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblFiles](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[source] [nvarchar](max) NOT NULL,
	[destination] [nvarchar](max) NOT NULL,
	[software_id] [int] NOT NULL,
	[filesize] [bigint] NULL,
	[hash_md5] [nvarchar](max) NULL,
 CONSTRAINT [PK_tblFiles] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tblFirewallExceptions]    Script Date: 30/06/2024 1:26:24 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblFirewallExceptions](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[filepath] [nvarchar](max) NOT NULL,
	[software_id] [int] NOT NULL,
	[rulename] [nvarchar](max) NULL,
	[proto_scope] [int] NULL,
	[port_scope] [int] NULL,
 CONSTRAINT [PK_tblFirewallExceptions] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tblImages]    Script Date: 30/06/2024 1:26:24 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblImages](
	[software_id] [int] NOT NULL,
	[small_image] [nvarchar](max) NULL,
 CONSTRAINT [PK_tblImages] PRIMARY KEY CLUSTERED 
(
	[software_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tblMessages]    Script Date: 30/06/2024 1:26:24 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblMessages](
	[id] [bigint] IDENTITY(1,1) NOT NULL,
	[message] [nvarchar](500) NOT NULL,
	[timestamp] [datetime] NOT NULL,
	[sender] [nvarchar](50) NULL,
 CONSTRAINT [PK_tblMessages] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tblPreferenceFiles]    Script Date: 30/06/2024 1:26:24 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblPreferenceFiles](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[filepath] [nvarchar](max) NOT NULL,
	[software_id] [int] NOT NULL,
	[target] [nvarchar](max) NOT NULL,
	[replace] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_tblPreferenceFiles] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tblRedist]    Script Date: 30/06/2024 1:26:24 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblRedist](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[name] [nvarchar](max) NOT NULL,
	[path] [nvarchar](max) NOT NULL,
	[filecheck] [nvarchar](max) NULL,
	[args] [nvarchar](max) NULL,
	[version] [nvarchar](max) NULL,
	[compressed] [int] NULL,
	[compressed_path] [nvarchar](max) NULL,
 CONSTRAINT [PK_tblRedist] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tblRedistUsage]    Script Date: 30/06/2024 1:26:24 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblRedistUsage](
	[redist_id] [int] NOT NULL,
	[software_id] [int] NOT NULL,
	[install_order] [int] NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tblRegistry]    Script Date: 30/06/2024 1:26:24 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblRegistry](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[hkey] [int] NOT NULL,
	[subkey] [nvarchar](max) NOT NULL,
	[value] [nvarchar](max) NOT NULL,
	[type] [int] NOT NULL,
	[data] [nvarchar](max) NOT NULL,
	[software_id] [int] NOT NULL,
 CONSTRAINT [PK_tblRegistry] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tblSecurityRegistration]    Script Date: 30/06/2024 1:26:24 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblSecurityRegistration](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[name] [nvarchar](max) NOT NULL,
	[regcode] [nvarchar](max) NOT NULL,
	[expiry] [datetime] NULL,
 CONSTRAINT [PK_tblSecurityRegistration] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tblSecurityTokens]    Script Date: 30/06/2024 1:26:24 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblSecurityTokens](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[token] [nvarchar](max) NOT NULL,
	[registration_date] [date] NOT NULL,
	[registration_id] [int] NOT NULL,
 CONSTRAINT [PK_tblSecurityTokens] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tblSerials]    Script Date: 30/06/2024 1:26:24 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblSerials](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[name] [nvarchar](max) NOT NULL,
	[instance] [int] NOT NULL,
	[regKey] [nvarchar](max) NULL,
	[regVal] [nvarchar](max) NULL,
	[software_id] [int] NOT NULL,
	[format] [nvarchar](max) NULL,
 CONSTRAINT [PK_tblSerials] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tblSerialsAvailable]    Script Date: 30/06/2024 1:26:24 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblSerialsAvailable](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[serial_id] [int] NOT NULL,
	[serial_value] [nvarchar](max) NOT NULL,
	[serial_used] [datetime] NULL,
 CONSTRAINT [PK_tblSerialsAvailable] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tblServers]    Script Date: 30/06/2024 1:26:24 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblServers](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[name] [nvarchar](max) NOT NULL,
	[address] [nvarchar](max) NOT NULL,
	[protocol] [int] NOT NULL,
	[priority] [int] NOT NULL,
 CONSTRAINT [PK_tblServers] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tblShortcut]    Script Date: 30/06/2024 1:26:24 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblShortcut](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[name] [nvarchar](max) NOT NULL,
	[location] [nvarchar](max) NOT NULL,
	[filepath] [nvarchar](max) NOT NULL,
	[runpath] [nvarchar](max) NOT NULL,
	[arguments] [nvarchar](max) NULL,
	[icon] [nvarchar](max) NOT NULL,
	[software_id] [int] NOT NULL,
 CONSTRAINT [PK_tblShortcut] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tblSoftware]    Script Date: 30/06/2024 1:26:24 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblSoftware](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[name] [nvarchar](max) NOT NULL,
	[mod_parent] [int] NULL,
 CONSTRAINT [PK_tblSoftware] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tblSystem]    Script Date: 30/06/2024 1:26:24 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblSystem](
	[setting] [nvarchar](max) NOT NULL,
	[data] [nvarchar](max) NOT NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
ALTER TABLE [dbo].[tblCompatibility]  WITH CHECK ADD  CONSTRAINT [FK_tblCompatibility_tblSoftware] FOREIGN KEY([software_id])
REFERENCES [dbo].[tblSoftware] ([id])
GO
ALTER TABLE [dbo].[tblCompatibility] CHECK CONSTRAINT [FK_tblCompatibility_tblSoftware]
GO
ALTER TABLE [dbo].[tblDirectories]  WITH CHECK ADD  CONSTRAINT [FK_tblDirectories_tblSoftware] FOREIGN KEY([software_id])
REFERENCES [dbo].[tblSoftware] ([id])
GO
ALTER TABLE [dbo].[tblDirectories] CHECK CONSTRAINT [FK_tblDirectories_tblSoftware]
GO
ALTER TABLE [dbo].[tblFiles]  WITH CHECK ADD  CONSTRAINT [FK_tblFiles_tblSoftware] FOREIGN KEY([software_id])
REFERENCES [dbo].[tblSoftware] ([id])
GO
ALTER TABLE [dbo].[tblFiles] CHECK CONSTRAINT [FK_tblFiles_tblSoftware]
GO
ALTER TABLE [dbo].[tblFirewallExceptions]  WITH CHECK ADD  CONSTRAINT [FK_tblFirewallExceptions_tblSoftware] FOREIGN KEY([software_id])
REFERENCES [dbo].[tblSoftware] ([id])
GO
ALTER TABLE [dbo].[tblFirewallExceptions] CHECK CONSTRAINT [FK_tblFirewallExceptions_tblSoftware]
GO
ALTER TABLE [dbo].[tblImages]  WITH CHECK ADD  CONSTRAINT [FK_tblImages_tblSoftware] FOREIGN KEY([software_id])
REFERENCES [dbo].[tblSoftware] ([id])
GO
ALTER TABLE [dbo].[tblImages] CHECK CONSTRAINT [FK_tblImages_tblSoftware]
GO
ALTER TABLE [dbo].[tblPreferenceFiles]  WITH CHECK ADD  CONSTRAINT [FK_tblPreferenceFiles_tblSoftware] FOREIGN KEY([software_id])
REFERENCES [dbo].[tblSoftware] ([id])
GO
ALTER TABLE [dbo].[tblPreferenceFiles] CHECK CONSTRAINT [FK_tblPreferenceFiles_tblSoftware]
GO
ALTER TABLE [dbo].[tblRedistUsage]  WITH CHECK ADD  CONSTRAINT [FK_tblRedistUsage_tblRedist] FOREIGN KEY([redist_id])
REFERENCES [dbo].[tblRedist] ([id])
GO
ALTER TABLE [dbo].[tblRedistUsage] CHECK CONSTRAINT [FK_tblRedistUsage_tblRedist]
GO
ALTER TABLE [dbo].[tblRedistUsage]  WITH CHECK ADD  CONSTRAINT [FK_tblRedistUsage_tblSoftware] FOREIGN KEY([software_id])
REFERENCES [dbo].[tblSoftware] ([id])
GO
ALTER TABLE [dbo].[tblRedistUsage] CHECK CONSTRAINT [FK_tblRedistUsage_tblSoftware]
GO
ALTER TABLE [dbo].[tblRegistry]  WITH CHECK ADD  CONSTRAINT [FK_tblRegistry_tblSoftware] FOREIGN KEY([software_id])
REFERENCES [dbo].[tblSoftware] ([id])
GO
ALTER TABLE [dbo].[tblRegistry] CHECK CONSTRAINT [FK_tblRegistry_tblSoftware]
GO
ALTER TABLE [dbo].[tblSecurityTokens]  WITH CHECK ADD  CONSTRAINT [FK_tblSecurityTokens_tblSecurityRegistration] FOREIGN KEY([registration_id])
REFERENCES [dbo].[tblSecurityRegistration] ([id])
GO
ALTER TABLE [dbo].[tblSecurityTokens] CHECK CONSTRAINT [FK_tblSecurityTokens_tblSecurityRegistration]
GO
ALTER TABLE [dbo].[tblSerialsAvailable]  WITH CHECK ADD  CONSTRAINT [FK_tblSerialsAvailable_tblSerials] FOREIGN KEY([serial_id])
REFERENCES [dbo].[tblSerials] ([id])
GO
ALTER TABLE [dbo].[tblSerialsAvailable] CHECK CONSTRAINT [FK_tblSerialsAvailable_tblSerials]
GO
ALTER TABLE [dbo].[tblShortcut]  WITH CHECK ADD  CONSTRAINT [FK_tblShortcut_tblSoftware] FOREIGN KEY([software_id])
REFERENCES [dbo].[tblSoftware] ([id])
GO
ALTER TABLE [dbo].[tblShortcut] CHECK CONSTRAINT [FK_tblShortcut_tblSoftware]
GO
