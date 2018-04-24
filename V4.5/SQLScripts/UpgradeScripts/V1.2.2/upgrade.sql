/************** PLACE HOLDER TO PUT UPGRADE SCRIPTS IF ANY FOR V 1.2.2 ************/ 
/* 1/16/2014 NS added new columns to the Traveler_Status table for HA functionality */
USE [vitalsigns]
GO
ALTER TABLE Traveler_Status ADD HA bit
GO
ALTER TABLE Traveler_Status ADD TravelerServlet varchar(255)
GO

/* 1/24/2014 NS added new column to the Traveler_Status table for HA Datastore */
ALTER TABLE Traveler_Status ADD HA_Datastore_Status varchar(50)
GO

/* 1/21/2014 NS added a new menu item for the Traveler Data Store page */
UPDATE [MenuItems] SET [ImageURL] = '~/images/icons/dominoserver.gif'
WHERE [DisplayText] = 'IBM Domino Settings'
GO

UPDATE [MenuItems] SET [ImageURL] = '~/images/icons/sametime.gif'
WHERE [DisplayText] = 'IBM Sametime Settings'
GO

UPDATE [MenuItems] SET [ImageURL] = '~/images/icons/exchange.jpg'
WHERE [DisplayText] = 'Microsoft Exchange Settings'
GO

INSERT INTO [MenuItems]([ID],[DisplayText],[OrderNum],[ParentID],[PageLink],[Level],[RefName],[ImageURL])
VALUES(61,'Notes Traveler Data Store',1,5,'/Configurator/TravelerDataStore.aspx',2,'TravelerDataStore',
'~/images/icons/traveler-icon.png')
GO

/* 1/23/2014 NS added - update Server Settings Editor entry to point to the new page */
UPDATE [MenuItems] SET [PageLink] = '/Security/ServerSettingsEditor.aspx'
WHERE [DisplayText] = 'Server Settings Editor'
GO

/* Joe - Delete if already AlertsOn record exists, as it failed ontop of 1.2.1 release.
DELETE from [dbo].Settings where sname = 'AlertsOn'
GO
/* 1/6/2014 NS added - AlertsOn will store the True/False value */
INSERT [dbo].[Settings] ([sname], [svalue], [stype]) VALUES (N'AlertsOn', N'True', N'System.String')

/* 1/22/2014 NS added for VSPLUS-191 */
SET IDENTITY_INSERT [dbo].[EventsMaster] ON
INSERT [dbo].[EventsMaster] ([ID], [EventName], [ServerTypeID]) VALUES (54, N'Telnet', 1)
SET IDENTITY_INSERT [dbo].[EventsMaster] OFF
GO

-- VSPLUS-272 Login - Go to Dashboard as default
IF NOT EXISTS (select * from syscolumns where name= 'StartupURL' and id = object_id('dbo.USERS'))
	ALTER TABLE dbo.USERS ADD StartupURL VARCHAR(100) NULL
GO

IF NOT EXISTS (select * from syscolumns where id = object_id('dbo.UsersStartupURLs'))
	BEGIN
	CREATE TABLE dbo.UsersStartupURLs(
	ID [int] IDENTITY(1,1) NOT NULL,
	URL VARCHAR(100),
	Name VARCHAR(50),
	IsDashboard BIT,
	IsConfigurator BIT,
	IsConsoleComm BIT
	) ON [PRIMARY]
	END
GO

-- we can add more here
IF NOT EXISTS (SELECT * FROM dbo.UsersStartupURLs WHERE URL='~/Configurator/Default.aspx')
	INSERT INTO dbo.UsersStartupURLs(URL,Name,IsDashboard,IsConfigurator,IsConsoleComm) VALUES('~/Configurator/Default.aspx','Default Page',1,1,1)
IF NOT EXISTS (SELECT * FROM dbo.UsersStartupURLs WHERE URL='~/Configurator/LotusDominoServers.aspx')
	INSERT INTO dbo.UsersStartupURLs(URL,Name,IsDashboard,IsConfigurator,IsConsoleComm)  VALUES('~/Configurator/LotusDominoServers.aspx','Domino Servers',0,1,0)
IF NOT EXISTS (SELECT * FROM dbo.UsersStartupURLs WHERE URL='~/Configurator/LotusSametimeGrid.aspx')
	INSERT INTO dbo.UsersStartupURLs(URL,Name,IsDashboard,IsConfigurator,IsConsoleComm)  VALUES('~/Configurator/LotusSametimeGrid.aspx','Sametime Servers',0,1,0)
IF NOT EXISTS (SELECT * FROM dbo.UsersStartupURLs WHERE URL='~/Configurator/URLsGrid.aspx')
	INSERT INTO dbo.UsersStartupURLs(URL,Name,IsDashboard,IsConfigurator,IsConsoleComm)  VALUES('~/Configurator/URLsGrid.aspx','URLs',0,1,0)
IF NOT EXISTS (SELECT * FROM dbo.UsersStartupURLs WHERE URL='~/Dashboard/OverallHealth1.aspx')
	INSERT INTO dbo.UsersStartupURLs(URL,Name,IsDashboard,IsConfigurator,IsConsoleComm)  VALUES('~/Dashboard/OverallHealth1.aspx','Dashboard',1,0,0)
IF NOT EXISTS (SELECT * FROM dbo.UsersStartupURLs WHERE URL='~/Dashboard/SummaryLandscape.aspx')
	INSERT INTO dbo.UsersStartupURLs(URL,Name,IsDashboard,IsConfigurator,IsConsoleComm)  VALUES('~/Dashboard/SummaryLandscape.aspx','Executive Summary',1,0,0)
IF NOT EXISTS (SELECT * FROM dbo.UsersStartupURLs WHERE URL='~/Dashboard/SummaryEXJournal.aspx')
	INSERT INTO dbo.UsersStartupURLs(URL,Name,IsDashboard,IsConfigurator,IsConsoleComm)  VALUES('~/Dashboard/SummaryEXJournal.aspx','ExJournal Summary',1,0,0)
IF NOT EXISTS (SELECT * FROM dbo.UsersStartupURLs WHERE URL='~/Dashboard/MailDeliveryStatus.aspx')
	INSERT INTO dbo.UsersStartupURLs(URL,Name,IsDashboard,IsConfigurator,IsConsoleComm)  VALUES('~/Dashboard/MailDeliveryStatus.aspx','Mail Delivery Status',1,0,0)
IF NOT EXISTS (SELECT * FROM dbo.UsersStartupURLs WHERE URL='~/Dashboard/DeviceTypeList.aspx')
	INSERT INTO dbo.UsersStartupURLs(URL,Name,IsDashboard,IsConfigurator,IsConsoleComm)  VALUES('~/Dashboard/DeviceTypeList.aspx','Status List',1,0,0)


/* 1/22/2014 MD added new columns to the DominoServers table for 'Alert if the server is running for more than x days' functionality */
IF NOT EXISTS (select * from syscolumns where name= 'ServerDaysAlert' and id = object_id('dbo.DominoServers'))
	ALTER TABLE dbo.DominoServers ADD ServerDaysAlert int
GO

/****** Object:  Table [dbo].[Traveler_HA_Datastore]    Script Date: 01/24/2014 09:55:34 ******/
/* 1/24/2014 NS added new table [Traveler_HA_Datastore] */
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[Traveler_HA_Datastore](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[TravelerServicePoolName] [varchar](150) NOT NULL,
	[ServerName] [varchar](150) NOT NULL,
	[DataStore] [varchar](50) NOT NULL,
	[Port] [int] NULL,
	[UserName] [varchar](150) NULL,
	[Password] [varchar](150) NULL,
	[IntegratedSecurity] [bit] NOT NULL,
	[TestScanServer] [varchar](150) NULL,
	[UsedByServers] [varchar](255) NULL,
 CONSTRAINT [PK_Traveler_HA_Datastore] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

/* 1/24/2014 NS added - Domino HTTP User and Domino HTTP Password */
INSERT [dbo].[Settings] ([sname], [svalue], [stype]) VALUES (N'Domino HTTP Password', N'True', N'System.Byte[]')
INSERT [dbo].[Settings] ([sname], [svalue], [stype]) VALUES (N'Domino HTTP User', N'True', N'System.String')
