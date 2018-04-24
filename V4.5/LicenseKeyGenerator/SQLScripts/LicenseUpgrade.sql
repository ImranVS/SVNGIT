--VSPLUS 1756  Durga 
USE [License]
GO
IF NOT EXISTS (select * from syscolumns where id = object_id('dbo.ServerTypes'))
BEGIN	
	CREATE TABLE [dbo].[ServerTypes](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[ServerType] [varchar](50) NOT NULL,
	[ServerTypeTable] [nvarchar](50) NULL,
	[FeatureId] [int] NULL,
	[UnitCost] [int] NULL,
 CONSTRAINT [PK_ServerType] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

END
GO


USE [License]
GO
DELETE FROM  [dbo].[ServerTypes]
SET IDENTITY_INSERT [dbo].[ServerTypes] ON 
INSERT [dbo].[ServerTypes] ([ID], [ServerType], [ServerTypeTable], [FeatureId], [UnitCost]) VALUES (1, N'Domino', N'DominoServers', 1, 100)
INSERT [dbo].[ServerTypes] ([ID], [ServerType], [ServerTypeTable], [FeatureId], [UnitCost]) VALUES (2, N'BES', N'', 3, 0)
INSERT [dbo].[ServerTypes] ([ID], [ServerType], [ServerTypeTable], [FeatureId], [UnitCost]) VALUES (3, N'Sametime', N'SametimeServers', 6, 0)
INSERT [dbo].[ServerTypes] ([ID], [ServerType], [ServerTypeTable], [FeatureId], [UnitCost]) VALUES (4, N'SharePoint', N'MailServices', 5, 50)
INSERT [dbo].[ServerTypes] ([ID], [ServerType], [ServerTypeTable], [FeatureId], [UnitCost]) VALUES (5, N'Exchange', N'ExchangeSettings', 2, 33)
INSERT [dbo].[ServerTypes] ([ID], [ServerType], [ServerTypeTable], [FeatureId], [UnitCost]) VALUES (6, N'Mail', N'', 4, 0)
INSERT [dbo].[ServerTypes] ([ID], [ServerType], [ServerTypeTable], [FeatureId], [UnitCost]) VALUES (7, N'URL', N'', 8, 0)
INSERT [dbo].[ServerTypes] ([ID], [ServerType], [ServerTypeTable], [FeatureId], [UnitCost]) VALUES (8, N'Network Device', N'Network Devices', 10, 0)
INSERT [dbo].[ServerTypes] ([ID], [ServerType], [ServerTypeTable], [FeatureId], [UnitCost]) VALUES (9, N'Notes Database', N'NotesDatabases', 1, 0)
INSERT [dbo].[ServerTypes] ([ID], [ServerType], [ServerTypeTable], [FeatureId], [UnitCost]) VALUES (10, N'General', N'ServerAttributes', 11, 0)
INSERT [dbo].[ServerTypes] ([ID], [ServerType], [ServerTypeTable], [FeatureId], [UnitCost]) VALUES (11, N'Mobile Users', N'Traveler_Devices', 9, 0)
INSERT [dbo].[ServerTypes] ([ID], [ServerType], [ServerTypeTable], [FeatureId], [UnitCost]) VALUES (12, N'Domino Cluster database', N'', 1, 0)
INSERT [dbo].[ServerTypes] ([ID], [ServerType], [ServerTypeTable], [FeatureId], [UnitCost]) VALUES (13, N'NotesMail Probe', N'', 4, 0)
INSERT [dbo].[ServerTypes] ([ID], [ServerType], [ServerTypeTable], [FeatureId], [UnitCost]) VALUES (14, N'Exchange Mail Flow', N'', 2, 0)
INSERT [dbo].[ServerTypes] ([ID], [ServerType], [ServerTypeTable], [FeatureId], [UnitCost]) VALUES (15, N'Lync', N'', 13, 33)
INSERT [dbo].[ServerTypes] ([ID], [ServerType], [ServerTypeTable], [FeatureId], [UnitCost]) VALUES (16, N'Windows', N'', 14, 33)
INSERT [dbo].[ServerTypes] ([ID], [ServerType], [ServerTypeTable], [FeatureId], [UnitCost]) VALUES (17, N'Cloud', N'', 15, 17)
INSERT [dbo].[ServerTypes] ([ID], [ServerType], [ServerTypeTable], [FeatureId], [UnitCost]) VALUES (18, N'Active Directory', N'', 16, 17)
INSERT [dbo].[ServerTypes] ([ID], [ServerType], [ServerTypeTable], [FeatureId], [UnitCost]) VALUES (19, N'Database Availability Group', N'DagSettings', 2, 0)
INSERT [dbo].[ServerTypes] ([ID], [ServerType], [ServerTypeTable], [FeatureId], [UnitCost]) VALUES (20, N'SNMP Devices', N'SNMP', 10, 13)
INSERT [dbo].[ServerTypes] ([ID], [ServerType], [ServerTypeTable], [FeatureId], [UnitCost]) VALUES (21, N'Office365', N'', 15, 0)
INSERT [dbo].[ServerTypes] ([ID], [ServerType], [ServerTypeTable], [FeatureId], [UnitCost]) VALUES (22, N'Websphere Server', N'Websphereserver', 11, 133)
INSERT [dbo].[ServerTypes] ([ID], [ServerType], [ServerTypeTable], [FeatureId], [UnitCost]) VALUES (23, N'Network Latency', N'NetworkLatency', NULL, 0)
INSERT [dbo].[ServerTypes] ([ID], [ServerType], [ServerTypeTable], [FeatureId], [UnitCost]) VALUES (24, N'Notes Traveler', N'Traveler_HA_Datastore', 1, 0)
INSERT [dbo].[ServerTypes] ([ID], [ServerType], [ServerTypeTable], [FeatureId], [UnitCost]) VALUES (25, N'Domino Cluster', N'DominoCluster', 1, 0)
SET IDENTITY_INSERT [dbo].[ServerTypes] OFF

------------- VSPLUS-2238  sowjanya
USE [License]
GO
IF NOT EXISTS (select * from syscolumns where id = object_id('dbo.UserPreferences'))
BEGIN
	CREATE TABLE [dbo].[UserPreferences](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[PreferenceName] [nvarchar](250) NULL,
	[PreferenceValue] [nvarchar](250) NULL,
	UserID int not null,
	Foreign key (UserID) references Users(ID) on delete cascade	
) ON [PRIMARY]
END
GO


USE [License]
GO
IF NOT EXISTS (select * from syscolumns where id = object_id('dbo.License'))
BEGIN

CREATE TABLE [dbo].[License](
	[LicenseKey] [varchar](400) NULL,
	[Units] [int] NULL,
	[InstallType] [varchar](50) NULL,
	[LicenseType] [varchar](100) NULL,
	[ExpirationDate] [datetime] NULL,
	[EncUnits] [varbinary](400) NULL,
	[CreateBy] [int] NULL,
	[CreatedOn] [datetime] NULL,
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[CompanyID] [int] NULL
) ON [PRIMARY]

END
GO


USE [License]
GO
IF NOT EXISTS (select * from syscolumns where id = object_id('dbo.LicenseCompanys'))
BEGIN

CREATE TABLE [dbo].[LicenseCompanys](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[CompanyName] [nvarchar](50) NULL
) ON [PRIMARY]

END
GO

USE [License]
GO
IF NOT EXISTS (select * from syscolumns where id = object_id('dbo.Roles'))
BEGIN

CREATE TABLE [dbo].[Roles](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[RoleName] [nvarchar](max) NULL,
	[AccessTo] [nvarchar](max) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

END
GO


USE [License]
GO
IF NOT EXISTS (select * from syscolumns where id = object_id('dbo.Users'))
BEGIN

CREATE TABLE [dbo].[Users](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[LoginName] [varchar](100) NOT NULL,
	[Password] [varchar](50) NULL,
	[FullName] [varchar](100) NOT NULL,
	[Email] [varchar](50) NOT NULL,
	[Status] [varchar](50) NOT NULL,
	[UserType] [varchar](50) NOT NULL,
	[SecurityQuestion1] [varchar](255) NULL,
	[SecurityQuestion1Answer] [varchar](100) NULL,
	[SecurityQuestion2] [varchar](255) NULL,
	[SecurityQuestion2Answer] [varchar](100) NULL,
 CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

END
GO



