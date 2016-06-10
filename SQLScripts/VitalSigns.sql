USE [master]
GO

/****** Object:  Database [vitalsigns]    Script Date: 06/05/2015 14:08:59 ******/
IF  EXISTS (SELECT name FROM sys.databases WHERE name = N'vitalsigns')
BEGIN
ALTER DATABASE [vitalsigns] SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
DROP DATABASE [vitalsigns]
END
GO

USE [master]
GO
/****** Object:  Database [vitalsigns]    Script Date: 09/23/2013 16:59:27 ******/
CREATE DATABASE [vitalsigns] ON  PRIMARY 
( NAME = N'VitalSigns', FILENAME = N'$(dbpath)\vitalsigns.mdf', SIZE = 1GB , MAXSIZE = UNLIMITED, FILEGROWTH = 100MB )
 LOG ON 
( NAME = N'VitalSigns_log',FILENAME = N'$(dbpath)\vitalsigns.ldf', SIZE = 500MB  , MAXSIZE = 2GB , FILEGROWTH = 100MB)
GO
ALTER DATABASE [vitalsigns] SET COMPATIBILITY_LEVEL = 100
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [vitalsigns].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [vitalsigns] SET ANSI_NULL_DEFAULT OFF
GO
ALTER DATABASE [vitalsigns] SET ANSI_NULLS OFF
GO
ALTER DATABASE [vitalsigns] SET ANSI_PADDING OFF
GO
ALTER DATABASE [vitalsigns] SET ANSI_WARNINGS OFF
GO
ALTER DATABASE [vitalsigns] SET ARITHABORT OFF
GO
ALTER DATABASE [vitalsigns] SET AUTO_CLOSE ON
GO
ALTER DATABASE [vitalsigns] SET AUTO_CREATE_STATISTICS ON
GO
ALTER DATABASE [vitalsigns] SET AUTO_SHRINK OFF
GO
ALTER DATABASE [vitalsigns] SET AUTO_UPDATE_STATISTICS ON
GO
ALTER DATABASE [vitalsigns] SET CURSOR_CLOSE_ON_COMMIT OFF
GO
ALTER DATABASE [vitalsigns] SET CURSOR_DEFAULT  GLOBAL
GO
ALTER DATABASE [vitalsigns] SET CONCAT_NULL_YIELDS_NULL OFF
GO
ALTER DATABASE [vitalsigns] SET NUMERIC_ROUNDABORT OFF
GO
ALTER DATABASE [vitalsigns] SET QUOTED_IDENTIFIER OFF
GO
ALTER DATABASE [vitalsigns] SET RECURSIVE_TRIGGERS OFF
GO
ALTER DATABASE [vitalsigns] SET  DISABLE_BROKER
GO
ALTER DATABASE [vitalsigns] SET AUTO_UPDATE_STATISTICS_ASYNC OFF
GO
ALTER DATABASE [vitalsigns] SET DATE_CORRELATION_OPTIMIZATION OFF
GO
ALTER DATABASE [vitalsigns] SET TRUSTWORTHY OFF
GO
ALTER DATABASE [vitalsigns] SET ALLOW_SNAPSHOT_ISOLATION OFF
GO
ALTER DATABASE [vitalsigns] SET PARAMETERIZATION SIMPLE
GO
ALTER DATABASE [vitalsigns] SET READ_COMMITTED_SNAPSHOT OFF
GO
ALTER DATABASE [vitalsigns] SET HONOR_BROKER_PRIORITY OFF
GO
ALTER DATABASE [vitalsigns] SET  READ_WRITE
GO
ALTER DATABASE [vitalsigns] SET RECOVERY SIMPLE
GO
ALTER DATABASE [vitalsigns] SET  MULTI_USER
GO
ALTER DATABASE [vitalsigns] SET PAGE_VERIFY CHECKSUM
GO
ALTER DATABASE [vitalsigns] SET DB_CHAINING OFF
GO
EXEC sys.sp_db_vardecimal_storage_format N'vitalsigns', N'ON'
GO

/****** Object:  User [vs]    Script Date: 09/23/2013 16:59:28 ******/

CREATE LOGIN vs WITH PASSWORD = 'V1talsign$';
GO

USE [vitalsigns]
GO
/****** Object:  User [vs]    Script Date: 09/23/2013 16:59:28 ******/
CREATE USER [vs] For LOGIN vs
GO
EXEC sp_addrolemember N'db_owner', N'vs'
go

/****** Object:  UserDefinedFunction [dbo].[VSDayOfWeek]    Script Date: 09/23/2013 16:59:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[VSDayOfWeek](@MaintDay as int)
RETURNS VARCHAR(10)
AS
BEGIN
DECLARE @rtDayofWeek VARCHAR(10)
SELECT @rtDayofWeek = CASE @MaintDay
WHEN 7 THEN 'Sunday'
WHEN 1 THEN 'Monday'
WHEN 2 THEN 'Tuesday'
WHEN 3 THEN 'Wednesday'
WHEN 4 THEN 'Thursday'
WHEN 5 THEN 'Friday'
WHEN 6 THEN 'Saturday'
END
RETURN (@rtDayofWeek)
END
GO
/* 2/20/2014 NS commented out the ScheduledReport table - old design, new design is at the end of the script */
/****** Object:  Table [dbo].[ScheduledReports]    Script Date: 09/23/2013 16:59:36 ******/
/*
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[ScheduledReports](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Title] [varchar](50) NOT NULL,
	[ReportName] [varchar](150) NOT NULL,
	[Frequency] [varchar](50) NOT NULL,
	[SendTo] [varchar](150) NOT NULL,
 CONSTRAINT [PK_ScheduledReports] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
*/

/****** Object:  Table [dbo].[ScheduledCommands]    Script Date: 09/23/2013 16:59:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ScheduledCommands](
	[Name] [nvarchar](150) NULL,
	[Enabled] [bit] NULL,
	[DominoServerName] [nvarchar](200) NULL,
	[ConsoleCommand] [nvarchar](50) NULL,
	[TimeOfDay] [datetime] NULL,
	[Sunday] [bit] NULL,
	[Monday] [bit] NULL,
	[Tuesday] [bit] NULL,
	[Wednesday] [bit] NULL,
	[Thursday] [bit] NULL,
	[Friday] [bit] NULL,
	[Saturday] [bit] NULL,
	[Key] [int] IDENTITY(1,1) NOT NULL,
 CONSTRAINT [ScheduledCommands$PrimaryKey] PRIMARY KEY CLUSTERED 
(
	[Key] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SametimeUsers]    Script Date: 09/23/2013 16:59:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[SametimeUsers](
	[UserName] [varchar](150) NULL,
	[Password] [varchar](150) NULL,
	[ID] [int] IDENTITY(1,1) NOT NULL,
 CONSTRAINT [PK_SametimeUsers] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[UserCustomPages]    Script Date: 09/23/2013 16:59:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[UserCustomPages](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[UserID] [int] NOT NULL,
	[URL] [varchar](250) NOT NULL,
	[Title] [varchar](250) NOT NULL,
	[IsPrivate] [bit] NOT NULL,
 CONSTRAINT [PK_UserCustomPages] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[URLs]    Script Date: 09/23/2013 16:59:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[URLs](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[TheURL] [nvarchar](255) NOT NULL,
	[Name] [nvarchar](255) NULL,
	[Category] [nvarchar](255) NULL,
	[ScanInterval] [int] NULL,
	[OffHoursScanInterval] [int] NULL,
	[NextScan] [datetime] NULL,
	[LastChecked] [datetime] NULL,
	[LastStatus] [nvarchar](50) NULL,
	[Enabled] [bit] NULL,
	[ResponseThreshold] [int] NULL,
	[RetryInterval] [int] NULL,
	[SearchString] [nvarchar](255) NULL,
	[AlertStringFound] [nvarchar](255) NULL,
	[UserName] [nvarchar](255) NULL,
	[PW] [nvarchar](255) NULL,
	[LocationId] [int] NULL,
	[ServerTypeId] [int] NULL,
	[FailureThreshold] [int] NULL,
	[MonitoredBy] [int] NULL,
 CONSTRAINT [URLs$PrimaryKey] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[TravelerStats]    Script Date: 09/23/2013 16:59:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[TravelerStats](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[TravelerServerName] [varchar](150) NOT NULL,
	[MailServerName] [varchar](150) NOT NULL,
	[Interval] [nchar](10) NOT NULL,
	[Delta] [int] NOT NULL,
	[OpenTimes] [int] NOT NULL,
	[DateUpdated] [datetime] NOT NULL,
 CONSTRAINT [PK_TravelerStats_1] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[UserSecurityQuestions]    Script Date: 09/23/2013 16:59:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[UserSecurityQuestions](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[SecurityQuestion] [varchar](255) NOT NULL,
 CONSTRAINT [PK_UserSecurityQuestions] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Users]    Script Date: 09/23/2013 16:59:36 ******/
USE [vitalsigns]
GO
/****** Object:  Table [dbo].[Users]    Script Date: 02/19/2014 01:32:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Users](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[LoginName] [varchar](100) NOT NULL,
	[Password] [varchar](50) NULL,
	[FullName] [varchar](100) NOT NULL,
	[Email] [varchar](50) NOT NULL,
	[Status] [varchar](50) NOT NULL,
	[SuperAdmin] [char](1) NOT NULL,
	[SecurityQuestion1] [varchar](255) NULL,
	[SecurityQuestion1Answer] [varchar](100) NULL,
	[SecurityQuestion2] [varchar](255) NULL,
	[SecurityQuestion2Answer] [varchar](100) NULL,
	[IsConfigurator] [bit] NULL,
	[IsDashboard] [bit] NULL,
	[Refreshtime] [int] NULL,
	[IsConsoleComm] [bit] NULL,
	[StartupURL] [varchar](100) NULL,
	[CustomBackground] [varchar](200) NULL,
	[CloudApplications] [bit] NULL,
	[OnPremisesApplications] [bit] NULL,
	[NetworkInfrastucture] [bit] NULL,
	[DominoServerMetrics] [bit] NULL,
	[IsFirstTimeLogin][bit] NULL,
	[cloudindex] [int] NULL,
    [premisesindex] [int] NULL,
    [networkindex] [int] NULL,
    [dockindex] [int] NULL,
    [cloudZone] [varchar](50) NULL,
    [premisesZone] [varchar](50) NULL,
    [networkZone] [varchar](50) NULL,
    [DockZone] [varchar](50) NULL,
    --2/11/2016 Durga Modified for VSPLUS 2595
	[TravelerZone] [varchar](MAX) NULL,
    [KeyUserDevicesZone] [varchar](MAX) NULL,
    [StatusZone] [varchar](MAX) NULL,
    [TravelerIndex] [int] NULL,
    [KeyUserDevicesIndex] [int] NULL,
    [StatusIndex] [int] NULL,
 CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Default [DF_Users_Refreshtime]    Script Date: 02/19/2014 01:32:54 ******/
ALTER TABLE [dbo].[Users] ADD  CONSTRAINT [DF_Users_Refreshtime]  DEFAULT ((30)) FOR [Refreshtime]
GO
/****** Object:  Default [DF__Users__GridRowCo__2610A626]    Script Date: 02/19/2014 01:32:54 ******/
--ALTER TABLE [dbo].[Users] ADD  DEFAULT ((10)) FOR [GridRowCount]
--GO
/****** Object:  Table [dbo].[UserReportFavorites]    Script Date: 09/23/2013 16:59:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserReportFavorites](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[UserID] [int] NOT NULL,
	[ReportID] [int] NOT NULL,
	[IsFavorite] [bit] NOT NULL,
	[NumOfClicks] [int] NOT NULL,
 CONSTRAINT [PK_UserReportFavorites] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UserMenuRestrictions]    Script Date: 09/23/2013 16:59:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserMenuRestrictions](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[UserID] [int] NULL,
	[MenuID] [int] NULL,
 CONSTRAINT [PK_UserMenuRestrictions] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

USE [vitalsigns]
GO


/****** Object:  Table [dbo].[Traveler_Devices]    Script Date: 03/26/2014 18:35:41 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Traveler_Devices](
	[UserName] [nvarchar](255) NULL,
	[DeviceName] [nvarchar](255) NULL,
	[ConnectionState] [nvarchar](255) NULL,
	[LastSyncTime] [datetime] NULL,
	[OS_Type] [nvarchar](255) NULL,
	[Client_Build] [nvarchar](255) NULL,
	[NotificationType] [nvarchar](255) NULL,
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[DocID] [nvarchar](255) NULL,
	[device_type] [nvarchar](255) NULL,
	[Access] [nvarchar](255) NULL,
	[Security_Policy] [nvarchar](255) NULL,
	[wipeRequested] [nvarchar](255) NULL,
	[wipeOptions] [nvarchar](255) NULL,
	[wipeStatus] [nvarchar](255) NULL,
	[SyncType] [nvarchar](255) NULL,
	[wipeSupported] [nvarchar](255) NULL,
	[ServerName] [nvarchar](255) NULL,
	[Approval] [nvarchar](255) NULL,
	[DeviceID] [nvarchar](150) NULL,
	[LastUpdated] [datetime] NULL,
	[MoreDetailsURL] [nvarchar](500) NULL,
	[IsMoreDetailsFetched] [bit] NULL,
	[OS_Type_Min] [nvarchar](255) NULL,
	[HAPoolName] [varchar] (150) NULL,
	[IsActive] [bit] NULL,
) ON [PRIMARY]
GO

CREATE INDEX IX_TD_DEVICE_NAME ON [dbo].[TRAVELER_DEVICES] ([DeviceName])
GO

CREATE INDEX IX_TD_OS_TYPE ON [dbo].[TRAVELER_DEVICES] ([OS_Type])
GO

CREATE UNIQUE INDEX IX_TD_SERVER_DEVICE ON [dbo].[TRAVELER_DEVICES] ([DeviceId],[ServerName])
GO

/****** Object:  UserDefinedFunction [dbo].[fnSplitString]    Script Date: 04/12/2014 00:38:14 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE FUNCTION [dbo].[fnSplitString] 
( 
    @string NVARCHAR(MAX), 
    @delimiter CHAR(1) 
) 
RETURNS @output TABLE(splitdata NVARCHAR(MAX) 
) 
BEGIN 
    DECLARE @start INT, @end INT 
    SELECT @start = 1, @end = CHARINDEX(@delimiter, @string) 
    WHILE @start < LEN(@string) + 1 BEGIN 
        IF @end = 0  
            SET @end = LEN(@string) + 1
       
        INSERT INTO @output (splitdata)  
        VALUES(SUBSTRING(@string, @start, @end - @start)) 
        SET @start = @end + 1 
        SET @end = CHARINDEX(@delimiter, @string, @start)
        
    END 
    RETURN 
END
Go
--VSPLUS-1864 Sowjanya
--Somaraju VSPLUS 2336
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

/* 3/29/2016 NS modified to represent Office 365 in a correct format */
CREATE procedure [dbo].[StatusByType](@Location varchar(8000)) as 
--declare @Location varchar(8000)
-- declare @type varchar(8000) 
-- set @type=''
DECLARE @SqlStr VARCHAR(8000) 
Declare @where  VARCHAR(8000) 
Declare @Empty varchar(100)
begin
set @where=''
if @Location<>'null'
begin
set @where=' and Location in(' + @Location+ ')'
end
--select @where
set @SqlStr='SELECT Type as TypeLoc,Issue,Maintenance,[Not Responding],OK
 FROM
((SELECT CASE WHEN Type=''Office365'' THEN ''Office 365'' ELSE Type END Type,StatusCode 
    FROM Status where Type is not null and Status <> ''Disabled'' and Location <>'''' 
and name in (SELECT ServerName FROM Servers union Select name from URLs where Enabled=1 union Select Name from O365Server where Enabled=1 union Select Name from MailServices where Enabled=1 union Select Name from CloudDetails where Enabled=1 union Select Name from NotesMailProbe where Enabled=1 union Select Name from [Network Devices] where Enabled=1 union Select Name from ExchangeMailProbe where Enabled=1 union Select Name from NotesDatabases where Enabled=1)
'+ @where +' )
    union all
    (SELECT rtrim(ltrim(b.splitdata)) + '' (*)'' as Type,StatusCode FROM (SELECT * FROM Status where Type is not null and Status <> ''Disabled'' and Location <>''''
and name in (SELECT ServerName FROM Servers union Select name from URLs where Enabled=1 union Select Name from O365Server where Enabled=1 union Select Name from MailServices where Enabled=1 union Select Name from CloudDetails where Enabled=1 union Select Name from NotesMailProbe where Enabled=1 union Select Name from [Network Devices] where Enabled=1 union Select Name from ExchangeMailProbe where Enabled=1 union Select Name from NotesDatabases where Enabled=1)
 '+ @where +') a CROSS APPLY dbo.fnSplitString(a.SecondaryRole,'';'') AS b)
 ) AS tbl
PIVOT
(
count(StatusCode)  
FOR StatusCode IN ([Issue],[Maintenance],[Not Responding],[OK])
) AS PivotedTable  where ([Issue]>0 or [Maintenance]>0 or [Not Responding]>0 or [OK]>0)'
exec(@SqlStr)
--select(@SqlStr)
end

GO
--VSPLUS-1864 Sowjanya
--Somaraju VSPLUS 2336
/****** Object:  StoredProcedure [dbo].[StatusByLocation]    Script Date: 06/20/2014 18:06:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[StatusByLocation](@type varchar(8000)) as 
--declare @type varchar(8000) 
--set @type=''
DECLARE @SqlStr VARCHAR(8000) 
Declare @where  VARCHAR(8000) 
begin
set @where=''
if @type<>'null'
begin
set @where=' and type in(' + @type + ')'
end
--select @where
set @SqlStr='SELECT location as TypeLoc,Issue,Maintenance,[Not Responding],OK
 FROM
(SELECT location ,StatusCode
    FROM Status where location is not null and Status <> ''Disabled'' and location <>''''
    and name in (SELECT ServerName FROM Servers union Select name from URLs where Enabled=1 union Select Name from O365Server where Enabled=1 union Select Name from CloudDetails where Enabled=1 union Select Name from MailServices where Enabled=1 union Select Name from NotesMailProbe where Enabled=1 union Select Name from [Network Devices] where Enabled=1 union Select Name from ExchangeMailProbe where Enabled=1 union Select Name from NotesDatabases where Enabled=1)
     '+ @where +'
     ) AS tbl
PIVOT
(
count(StatusCode)  
FOR StatusCode IN ([Issue],[Maintenance],[Not Responding],[OK])
) AS PivotedTable where ([Issue]>0 or [Maintenance]>0 or [Not Responding]>0 or [OK]>0)'
--select(@SqlStr)
exec(@SqlStr)
end

GO
--VSPLUS-1864 Sowjanya
--Somaraju VSPLUS 2336
/****** Object:  StoredProcedure [dbo].[StatusByCategory]    Script Date: 06/20/2014 17:51:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE procedure [dbo].[StatusByCategory](@Location varchar(8000)) as 
--declare @Location varchar(8000)
-- declare @type varchar(8000) 
-- set @type=''
DECLARE @SqlStr VARCHAR(8000) 
Declare @where  VARCHAR(8000) 
Declare @Empty varchar(100)
begin
set @where=''
if @Location<>'null'
begin
set @where=' and Location in(' + @Location+ ')'
end
--select @where
set @SqlStr='SELECT Category as TypeLoc,Issue,Maintenance,[Not Responding],OK
 FROM
((SELECT Category,StatusCode 
    FROM Status where Category is not null and Type is not null and Status <> ''Disabled'' and Location <>''''
    and name in (SELECT ServerName FROM Servers union Select name from URLs where Enabled=1 union Select Name from O365Server where Enabled=1 union Select Name from CloudDetails where Enabled=1 union Select Name from MailServices where Enabled=1 union Select Name from NotesMailProbe where Enabled=1 union Select Name from [Network Devices] where Enabled=1 union Select Name from ExchangeMailProbe where Enabled=1 union Select Name from NotesDatabases where Enabled=1) 
     '+ @where +' )
    union all
    (select Category + '' (Secondary Role)'' as Category,StatusCode
    FROM Status where  
    (SecondaryRole like (''%Sametime%'') or SecondaryRole like (''%Quickr%'') or SecondaryRole like (''%Traveler''))
    and name in (SELECT ServerName FROM Servers union Select name from URLs where Enabled=1 union Select Name from O365Server where Enabled=1 union Select Name from CloudDetails where Enabled=1 union Select Name from MailServices where Enabled=1 union Select Name from NotesMailProbe where Enabled=1 union Select Name from [Network Devices] where Enabled=1 union Select Name from ExchangeMailProbe where Enabled=1 union Select Name from NotesDatabases where Enabled=1)     
    )
    ) AS tbl
PIVOT
(
count(StatusCode)  
FOR StatusCode IN ([Issue],[Maintenance],[Not Responding],[OK])
) AS PivotedTable  where ([Issue]>0 or [Maintenance]>0 or [Not Responding]>0 or [OK]>0)'
exec(@SqlStr)
--select(@SqlStr)
end


GO

/****** Object:  Table [dbo].[Status]    Script Date: 09/23/2013 17:00:24 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Status](
	[Type] [nvarchar](200) NULL,
	[Location] [nvarchar](200) NULL,
	[Category] [nvarchar](150) NULL,
	[Name] [nvarchar](255) NULL,
	[Status] [nvarchar](255) NULL,
	[Details] [nvarchar](255) NULL,
	[LastUpdate] [datetime] NULL,
	[Description] [nvarchar](255) NULL,
	[PendingMail] [int] NULL,
	[DeadMail] [int] NULL,
	[MailDetails] [nvarchar](255) NULL,
	[Upcount] [int] NULL,
	[DownCount] [int] NULL,
	[UpPercent] [real] NULL,
	[ResponseTime] [int] NULL,
	[ResponseThreshold] [int] NULL,
	[PendingThreshold] [int] NULL,
	[DeadThreshold] [int] NULL,
	[UserCount] [int] NULL,
	[MyPercent] [real] NULL,
	[NextScan] [datetime] NULL,
	[DominoServerTasks] [nvarchar](255) NULL,
	[TypeANDName] [nvarchar](255) NOT NULL,
	[Icon] [int] NULL,
	[OperatingSystem] [nvarchar](100) NULL,
	[DominoVersion] [nvarchar](100) NULL,
	[UpMinutes] [float] NULL,
	[DownMinutes] [float] NULL,
	[UpPercentMinutes] [float] NULL,
	[PercentageChange] [real] NULL,
	[CPU] [float] NULL,
	[HeldMail] [float] NULL,
	[HeldMailThreshold] [float] NULL,
	[Severity] [int] NULL,
	[Memory] [float] NULL,
	[StatusCode] [nvarchar](50) NULL,
	[SecondaryRole] [nvarchar](255) NULL,
	[CPUThreshold] [float] NULL,
	[exjournal] [float] NULL,
	[exjournal1] [float] NULL,
	[exjournal2] [float] NULL,
	[ExJournalDate] [datetime] NULL,
	[ElapsedDays] [int] NULL,
	[LicensesUsed] [nvarchar](100) NULL,
	[SRPConnectionn] [nvarchar](100) NULL,
	[ContentMemory] [float] NULL,
 CONSTRAINT [Status$PrimaryKey] PRIMARY KEY CLUSTERED 
(
	[TypeANDName] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  UserDefinedFunction [dbo].[Split]    Script Date: 09/23/2013 17:00:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[Split]
(
	@RowData nvarchar(2000),
	@SplitOn nvarchar(5)
)  
RETURNS @RtnValue table 
(
	Id int identity(1,1),
	Data nvarchar(100)
) 
AS  
BEGIN 
	Declare @Cnt int
	Set @Cnt = 1

	While (Charindex(@SplitOn,@RowData)>0)
	Begin
		Insert Into @RtnValue (data)
		Select 
			Data = ltrim(rtrim(Substring(@RowData,1,Charindex(@SplitOn,@RowData)-1)))

		Set @RowData = Substring(@RowData,Charindex(@SplitOn,@RowData)+1,len(@RowData))
		Set @Cnt = @Cnt + 1
	End
	
	Insert Into @RtnValue (data)
	Select Data = ltrim(rtrim(@RowData))

	Return
END
GO
/****** Object:  Table [dbo].[Settings]    Script Date: 09/23/2013 17:00:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Settings](
	[sname] [nvarchar](250) NOT NULL,
	[svalue] [nvarchar](250) NULL,
	[stype] [nvarchar](250) NULL,
 CONSTRAINT [PK_Settings] PRIMARY KEY CLUSTERED 
(
	[sname] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ServerTypes]    Script Date: 09/23/2013 17:00:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[ServerTypes](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[ServerType] [varchar](50) NOT NULL,
	[ServerTypeTable] [nvarchar] (50) NULL,
	[FeatureId] [int] NULL,
	
 CONSTRAINT [PK_ServerType] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[ServerTaskSettings]    Script Date: 09/23/2013 17:00:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ServerTaskSettings](
	[TaskID] [int] NOT NULL,
	[ServerID] [int] NULL,
	[MyID] [int] IDENTITY(1,1) NOT NULL,
	[Enabled] [bit] NULL,
	[SendLoadCommand] [bit] NULL,
	[SendRestartCommand] [bit] NULL,
	[SendExitCommand] [bit] NULL,
	[RestartOffHours] [bit] NULL,
	[Modified_By] [int] NULL,
	[Modified_On] [date] NULL,
	[MinNoOfTasks] [int] NULL,
	[IsMinTasksEnabled] [bit] NULL,
	[SendMinTasksLoadCmd] [bit] NULL
 CONSTRAINT [ServerTaskSettings$PrimaryKey] PRIMARY KEY CLUSTERED 
(
	[MyID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

/****** Object:  Table [dbo].[Reports]    Script Date: 09/23/2013 17:00:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Reports](
	[Recipient] [nvarchar](250) NULL,
	[ReportName] [nvarchar](250) NULL,
	[Frequency] [nvarchar](250) NULL,
	[ID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ReportItems]    Script Date: 09/23/2013 17:00:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ReportItems](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](250) NULL,
	[Category] [nvarchar](250) NULL,
	[Description] [nvarchar](250) NULL,
	[PageURL] [nvarchar](500) NULL,
	[ImageURL] [nvarchar](250) NULL,
	[ConfiguratorOnly] [bit] NULL,
	[isworking] [nvarchar](50) NULL,
	[MaySchedule] [bit] NULL,
 CONSTRAINT [PK_ReportItems] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY],
 CONSTRAINT [IX_ReportItems] UNIQUE NONCLUSTERED 
(
	[Name] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

--Mukund VSPlus-1004, 14Oct14
USE [vitalsigns]
GO
if NOT exists (select * from syscolumns where id = object_id('dbo.SNMPDevices'))
CREATE TABLE [dbo].[SNMPDevices](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Description] [nvarchar](255) NULL,
	[Category] [nvarchar](255) NULL,
	[Port] [int] NULL,
	[Username] [nvarchar](255) NULL,
	[Password] [nvarchar](255) NULL,
	[Scanning Interval] [int] NULL,
	[OffHoursScanInterval] [int] NULL,
	[Next Scan] [datetime] NULL,
	[LastChecked] [datetime] NULL,
	[LastStatus] [nvarchar](50) NULL,
	[Enabled] [bit] NULL,
	[Location] [nvarchar](50) NULL,
	[Name] [nvarchar](50) NULL,
	[ResponseThreshold] [int] NULL,
	[RetryInterval] [int] NULL,
	[Address] [nvarchar](255) NULL,
	[LocationId] [int] NULL,
	[ServerTypeId] [int] NULL,
	[OID] [nvarchar](50) NULL,
	[ImageURL] [nvarchar](200) NULL, 
	[IncludeOnDashBoard] [bit] NULL,
 CONSTRAINT [SNMP Devices$PrimaryKey] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO


/****** Object:  Table [dbo].[Network Devices]    Script Date: 09/23/2013 17:00:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Network Devices](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Description] [nvarchar](255) NULL,
	[Category] [nvarchar](255) NULL,
	[Port] [int] NULL,
	[Username] [nvarchar](255) NULL,
	[Password] [nvarchar](255) NULL,
	[Scanning Interval] [int] NULL,
	[OffHoursScanInterval] [int] NULL,
	[Next Scan] [datetime] NULL,
	[LastChecked] [datetime] NULL,
	[LastStatus] [nvarchar](50) NULL,
	[Enabled] [bit] NULL,
	[Location] [nvarchar](50) NULL,
	[Name] [nvarchar](50) NULL,
	[ResponseThreshold] [int] NULL,
	[RetryInterval] [int] NULL,
	[Address] [nvarchar](255) NULL,
	[LocationId] [int],
	[ServerTypeId] [int],
	[ImageURL] [nvarchar](200) NULL,
	[IncludeOnDashBoard] [bit] NULL,
	[NetworkType] [nvarchar](50) NULL,
 CONSTRAINT [Network Devices$PrimaryKey] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[	]    Script Date: 09/23/2013 17:00:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
--VSPLUS-2474 DURGA
CREATE TABLE dbo.MenuItems
( 
	[ID] [int] NOT NULL IDENTITY(1, 1),
	[DisplayText] [varchar](50) NOT NULL,
	[OrderNum] [int] NOT NULL,
	[ParentID] [int] NULL,
	[PageLink] [varchar](150) NULL,
	[Level] [int] NULL,
	[RefName] [varchar](50) NULL,
	[ImageURL] [varchar](50) NULL,
	MenuArea [nvarchar] (50) null,
	SessionNames [nvarchar] (MAX) null,
	TimerEnable [bit] null,
	[OverrideSort] [int] NULL,
	CONSTRAINT [PK_MenuItems1] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[MaintenanceWindows]    Script Date: 09/23/2013 17:00:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MaintenanceWindows](
	[DeviceType] [nvarchar](200) NULL,
	[Name] [nvarchar](200) NULL,
	[MaintWindow] [nvarchar](255) NULL,
	[ID] [int] IDENTITY(1,1) NOT NULL,
 CONSTRAINT [MaintenanceWindows$PrimaryKey] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[MaintenanceSettings]    Script Date: 09/23/2013 17:00:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MaintenanceSettings](
	[Name] [nvarchar](200) NOT NULL,
	[Monday] [bit] NULL,
	[Tuesday] [bit] NULL,
	[Wednesday] [bit] NULL,
	[Thursday] [bit] NULL,
	[Friday] [bit] NULL,
	[Saturday] [bit] NULL,
	[Sunday] [bit] NULL,
	[StartTime] [nvarchar](50) NOT NULL,
	[EndTime] [nvarchar](50) NOT NULL,
 CONSTRAINT [MaintenanceSettings$PrimaryKey] PRIMARY KEY CLUSTERED 
(
	[Name] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

/****** Object:  Table [dbo].[Maintenance]    Script Date: 09/23/2013 17:00:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Maintenance](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](100) NOT NULL,
	[StartDate] [datetime] NULL,
	[StartTime] [datetime] NOT NULL,
	[Duration] [int] NOT NULL,
	[EndDate] [datetime] NULL,
	[MaintType] [varchar](50) NOT NULL,
	[MaintDaysList] [varchar](150) NOT NULL,
	[EndDateIndicator] [nchar](10) NOT NULL,
	[RecurrenceInfo] [ntext] NULL,
	[ReminderInfo] [ntext] NULL,
	[Label] [int] NULL,
	[AllDay] [bit] NULL,
	[Type] [int] NULL,
 CONSTRAINT [PK_Maintenance] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[NotesMailProbeHistory]    Script Date: 09/23/2013 17:00:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[NotesMailProbeHistory](
	[ProbeID] [int] IDENTITY(1,1) NOT NULL,
	[SentDateTime] [datetime] NULL,
	[SentTo] [nvarchar](255) NULL,
	[DeliveryThresholdInMinutes] [int] NULL,
	[DeliveryTimeInMinutes] [numeric](10,0) NULL,
	[SubjectKey] [nvarchar](255) NULL,
	[ArrivalAtMailBox] [datetime] NULL,
	[Status] [nvarchar](255) NULL,
	[Details] [nvarchar](255) NULL,
	[DeviceName] [nvarchar](255) NULL,
	[TargetServer] [nvarchar](200) NULL,
	[TargetDatabase] [nvarchar](200) NULL,
 CONSTRAINT [NotesMailProbeHistory$PrimaryKey] PRIMARY KEY CLUSTERED 
(
	[ProbeID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/* 4/23/2015 NS modified for VSPLUS-1297 */
/****** Object:  Table [dbo].[HoursIndicator]    Script Date: 09/23/2013 17:00:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[HoursIndicator](
	[ID] [int] IDENTITY(0,1) NOT NULL,
	[Type] [nvarchar](250) NULL,
	[Starttime] [nvarchar](50) NULL,
	[Duration] [int] NULL,
	[Issunday] [bit] NULL,
	[IsMonday] [bit] NULL,
	[IsTuesday] [bit] NULL,
	[IsWednesday] [bit] NULL,
	[IsThursday] [bit] NULL,
	[IsFriday] [bit] NULL,
	[Issaturday] [bit] NULL,
	[UseType] [int] NULL
	
 CONSTRAINT [PK_HoursIndicator] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  UserDefinedFunction [dbo].[GetSrvNameAbbr]    Script Date: 09/23/2013 17:00:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Natallya Shkarayeva
-- Create date: 10/16/2012
-- Description:	Get server name in abbreviated format
-- =============================================
CREATE FUNCTION [dbo].[GetSrvNameAbbr] 
(
	@ServerName as varchar(150)
)
RETURNS varchar(150)
AS
BEGIN
	-- Declare the return variable here
	DECLARE @AbbrName as varchar(150)

	-- Add the T-SQL statements to compute the return value here
	SET @AbbrName = REPLACE(@ServerName,'CN=','')
	SET @AbbrName = REPLACE(@AbbrName,'O=','')
	SET @AbbrName = REPLACE(@AbbrName,'OU=','')

	-- Return the result of the function
	RETURN @AbbrName

END
GO
/****** Object:  Table [dbo].[DominoDiskSpace]    Script Date: 09/23/2013 17:00:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DominoDiskSpace](
	[ServerName] [nvarchar](250) NULL,
	[DiskName] [nvarchar](250) NULL,
	[DiskFree] [float] NULL,
	[DiskSize] [float] NULL,
	[PercentFree] [float] NULL,
	[PercentUtilization] [float] NULL,
	[AverageQueueLength] [float] NULL,
	[Updated] [datetime] NULL,
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Threshold] [float] NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[DominoServerTaskStatus]    Script Date: 09/23/2013 17:00:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[DominoServerTaskStatus](
	[ServerName] [varchar](200) NULL,
	[TaskName] [varchar](100) NULL,
	[Monitored] [bit] NULL,
	[StatusSummary] [varchar](50) NULL,
	[PrimaryStatus] [varchar](200) NULL,
	[SecondaryStatus] [varchar](200) NULL,
	[LastUpdate] [datetime] NULL,
	[ID] [int] IDENTITY(1,1) NOT NULL,
 CONSTRAINT [PK_DominoServerTaskStatus] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[DominoServerTasks]    Script Date: 09/23/2013 17:00:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DominoServerTasks](
	[TaskID] [int] IDENTITY(1,1) NOT NULL,
	[TaskName] [nvarchar](100) NULL,
	[ConsoleString] [nvarchar](150) NULL,
	[RetryCount] [int] NULL,
	[FreezeDetect] [bit] NULL,
	[MaxBusyTime] [int] NULL,
	[IdleString] [nvarchar](100) NULL,
	[LoadString] [nvarchar](80) NULL,
 CONSTRAINT [DominoServerTasks$PrimaryKey] PRIMARY KEY CLUSTERED 
(
	[TaskID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  StoredProcedure [dbo].[getData]    Script Date: 09/23/2013 17:00:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[getData] AS 
 select table_name, column_name, data_type
  from information_schema.columns
  where table_name in
  (
   select table_name
   from Information_Schema.Tables
   where Table_Type='Base Table'
  ) order by table_name
GO
/****** Object:  Table [dbo].[MailHealth]    Script Date: 09/23/2013 17:00:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MailHealth](
	[ServerName] [nvarchar](255) NOT NULL,
	[Domino_Domain] [nvarchar](250) NULL,
	[Mailbox_Count] [int] NULL,
	[Mailbox_PerformanceIndex] [float] NULL,
	[Mail_Pending] [int] NULL,
	[Mail_Dead] [int] NULL,
	[Mail_Waiting] [int] NULL,
	[Mail_Held] [int] NULL,
	[Mail_MaximiumSizeDelivered] [int] NULL,
	[Mail_PeakMessageDeliveryTime] [nvarchar](255) NULL,
	[DeadThreshold] [int] NULL,
	[PendingThreshold] [int] NULL,
	[HeldMailThreshold] [int] NULL,
	[Mail_AverageDeliveryTime] [int] NULL,
	[Mail_AverageSizeDelivered] [int] NULL,
	[Mail_AverageServerHops] [int] NULL,
	[Mail_Transferred] [int] NULL,
	[Mail_Delivered] [int] NULL,
	[Mail_Transferred_NRPC] [int] NULL,
	[Mail_Transferred_SMTP] [int] NULL,
	[Mail_TransferThreads_Active] [int] NULL,
	[Mail_WaitingForDeliveryRetry] [int] NULL,
	[Mail_WaitingForDIR] [int] NULL,
	[Mail_WaitingForDNS] [int] NULL,
	[Mail_DeliveredSize_100KB_to_1MB] [int] NULL,
	[Mail_DeliveredSize_10KB_to_100KB] [int] NULL,
	[Mail_DeliveredSize_10MB_to_100MB] [int] NULL,
	[Mail_DeliveredSize_1KB_to_10KB] [int] NULL,
	[Mail_DeliveredSize_1MB_to_10MB] [int] NULL,
	[Mail_DeliveredSize_Under_1KB] [int] NULL,
	[Mail_Routed] [int] NULL,
	[Mail_PeakMessagesDelivered] [int] NULL,
	[Mail_PeakMessagesTransferred] [int] NULL,
	[Mail_PeakMessageTransferredTime] [nvarchar](255) NULL,
	[Mail_RecallFailures] [int] NULL,
	[Mail_WaitingRecipients] [int] NULL,
	[ID] [int] IDENTITY(1,1) NOT NULL,
 CONSTRAINT [MailHealth$PrimaryKey] PRIMARY KEY CLUSTERED 
(
	[ServerName] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Logos]    Script Date: 09/23/2013 17:00:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE dbo.Logos
( 
	[LogoID] [int] NOT NULL IDENTITY(1, 1),
	[LogoName] [nvarchar](50) NULL,
	[LogoImage] [varchar](255) NULL,
	CONSTRAINT [PK_Logos1] PRIMARY KEY CLUSTERED 
(
	[LogoID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[LogFile]    Script Date: 09/23/2013 17:00:25 ******/
--Mukund:VSPLUS-78 added NotRequiredKeyword 03Nov14
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[LogFile](
	[Keyword] [nvarchar](255) NULL,
	[RepeatOnce] [bit] NULL,
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[NotRequiredKeyword] [nvarchar](255) NULL,
	[Log] [bit] NULL,
	[AgentLog] [bit] NULL,
	[DominoEventLogId] [int] Not Null
 CONSTRAINT [LogFile$PrimaryKey] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Locations]    Script Date: 09/23/2013 17:00:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Locations](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Location] [varchar](50) NOT NULL,
	[City] [varchar](255) NULL,
	[State] [varchar](255) NULL,
	[Country] [varchar](255) NULL,
 CONSTRAINT [PK_Location] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[AlertHistory]    Script Date: 09/23/2013 17:00:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AlertHistory](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[DeviceName] [nvarchar](50) NULL,
	[DeviceType] [nvarchar](50) NULL,
	[AlertType] [nvarchar](255) NULL,
	[DateTimeOfAlert] [datetime] NULL,
	[DateTimeSent] [datetime] NULL,
	[DateTimeAlertCleared] [datetime] NULL,
	[EventType] [nvarchar](250) NULL,
	[Location] [nvarchar](250) NULL,
	[Details] [nvarchar](500) NULL,
 CONSTRAINT [AlertHistory$PrimaryKey] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

/****** Object:  Table [dbo].[AlertNames]    Script Date: 09/23/2013 17:00:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AlertNames](
	[AlertKey] [int] IDENTITY(1,1) NOT NULL,
	[AlertName] [nvarchar](250) NULL,
	[Templateid] [int] NULL,
 CONSTRAINT [PK_AlertNames] PRIMARY KEY CLUSTERED 
(
	[AlertKey] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Company]    Script Date: 09/23/2013 17:00:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Company](
	[CompanyName] [nvarchar](100) NOT NULL,
	[LogoPath] [nvarchar](100) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[BlackBerryServers]    Script Date: 09/23/2013 17:00:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[BlackBerryServers](
	[ServerID] [int] NULL,
	[Category] [nvarchar](255) NULL,
	[ScanInterval] [int] NULL,
	[OffHoursScanInterval] [int] NULL,
	[Enabled] [bit] NULL,
	[RetryInterval] [int] NULL,
	[key] [int] IDENTITY(1,1) NOT NULL,
	[SNMPCommunity] [nvarchar](100) NULL,
	[Messaging] [bit] NULL,
	[Controller] [bit] NULL,
	[Dispatcher] [bit] NULL,
	[Synchronization] [bit] NULL,
	[Policy] [bit] NULL,
	[MDS] [bit] NULL,
	[Attachment] [bit] NULL,
	[Alert] [bit] NULL,
	[Router] [bit] NULL,
	[AlertIP] [nvarchar](100) NULL,
	[RouterIP] [nvarchar](100) NULL,
	[AttachmentIP] [nvarchar](100) NULL,
	[OtherServices] [nvarchar](255) NULL,
	[MDSConnection] [bit] NULL,
	[BESVersion] [nvarchar](255) NULL,
	[MDSServices] [bit] NULL,
	[TimeZoneAdjustment] [int] NULL,
	[USDateFormat] [bit] NULL,
	[PendingThreshold] [int] NULL,
	[ExpiredThreshold] [int] NULL,
	[NotificationGroup] [nvarchar](255) NULL,
	[HAOption] [nvarchar](100) NULL,
    [HAPartner] [nvarchar](100) NULL,
 CONSTRAINT [BlackBerryServers$PrimaryKey] PRIMARY KEY CLUSTERED 
(
	[key] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

/****** Object:  Table [dbo].[BlackBerry]    Script Date: 09/23/2013 17:00:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[BlackBerry](
	[Enabled] [bit] NULL,
	[Name] [nvarchar](50) NULL,
	[NotesMailAddress] [nvarchar](255) NOT NULL,
	[Category] [nvarchar](255) NULL,
	[ScanInterval] [int] NULL,
	[OffHoursScanInterval] [int] NULL,
	[DeliveryThreshold] [int] NULL,
	[RetryInterval] [int] NULL,
	[DestinationServerID] [int] NULL,
	[DestinationDatabase] [nvarchar](50) NULL,
	[InternetMailAddress] [nvarchar](50) NULL,
	[NextScan] [datetime] NULL,
	[LastChecked] [datetime] NULL,
	[LastStatus] [nvarchar](50) NULL,
	[SourceServer] [nvarchar](255) NULL,
	[ConfirmationServerID] [int] NULL,
	[ConfirmationDatabase] [nvarchar](255) NULL,
 CONSTRAINT [BlackBerry$PrimaryKey] PRIMARY KEY CLUSTERED 
(
	[NotesMailAddress] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO


/****** Object:  Table [dbo].[Credentials]    Script Date: 09/23/2013 17:00:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Credentials](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[AliasName] [varchar](250) NULL,
	[UserID] [varchar](250) NULL,
	[Password] [varchar](250) NULL,
	[ServerTypeID] [int] NULL,
 CONSTRAINT [PK_Credentials] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO

/****** Object:  Table [dbo].[DominoConsoleCommands]    Script Date: 09/23/2013 17:00:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[DominoConsoleCommands](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[ServerName] [varchar](250) NOT NULL,
	[Command] [nvarchar](250) NOT NULL,
	[Submitter] [nvarchar](250) NOT NULL,
	[DateTimeSubmitted] [datetime] NOT NULL,
	[DateTimeProcessed] [datetime] NULL,
	[Result] [varchar](250) NULL,
	[Comments] [varchar](250) NULL,
 CONSTRAINT [PK_DominoConsoleCommands] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[DominoClusterHealth]    Script Date: 09/23/2013 17:00:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DominoClusterHealth](
	[ServerName] [nvarchar](250) NULL,
	[ClusterName] [nvarchar](250) NULL,
	[SecondsOnQueue] [int] NULL,
	[SecondsOnQueueMax] [int] NULL,
	[SecondsOnQueueAvg] [float] NULL,
	[LastUpdate] [datetime] NULL,
	[WorkQueueDepth] [int] NULL,
	[WorkQueueDepthMax] [int] NULL,
	[WorkQueueDepthAvg] [float] NULL,
	[Availability] [int] NULL,
	[AvailabilityThreshold] [int] NULL,
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Analysis] [nvarchar](255) NULL,
	/*2/16/2016 Sowmya Modified for VSPLUS 2455*/
	[ClusterScan] [nvarchar](50) NULL,
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Servers]    Script Date: 09/23/2013 17:00:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Servers](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[ServerName] [varchar](100) NOT NULL,
	[ServerTypeID] [int] NOT NULL,
	[Description] [varchar](255) NOT NULL,
	[LocationID] [int] NOT NULL,
	[IPAddress] [varchar](50) NOT NULL,
	[MonitoredBy] [int] NULL,
	[ProfileName] [nvarchar] (20) NULL,
	[BusinesshoursID] [int] NULL,
	--3/14/2016 Somaraju Addded for VSPLUS-2694,2697
	 [MonthlyOperatingCost] [decimal](18, 0) NULL,
	  [IdealUserCount] [int] NULL,
	--Chandrahas
	--,[Enabled] [bit] NULL,
	--[ScanInterval] [int] NULL,
	--[RetryInterval] [int] NULL,
	--[OffHourInterval] [int] NULL,
	--[Category] [varchar](100) NULL,
	--[CPU_Threshold] [int] NULL,
	--[MemThreshold] [int] NULL,
	--[ResponseTime] [int] NULL
 CONSTRAINT [PK_Servers] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO


CREATE UNIQUE INDEX IX_SERVERS_NAME_TYPE ON [dbo].[Servers] ([ServerName],[ServerTypeID])
GO

SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[DominoCluster]    Script Date: 09/23/2013 17:00:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DominoCluster](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[ServerID_A] [int] NULL,
	[Server_A_Directory] [nvarchar](255) NULL,
	[Server_A_ExcludeList] [nvarchar](255) NULL,
	[ServerID_B] [int] NULL,
	[Server_B_Directory] [nvarchar](255) NULL,
	[Server_B_ExcludeList] [nvarchar](255) NULL,
	[ServerID_C] [int] NULL,
	[Server_C_Directory] [nvarchar](255) NULL,
	[Server_C_ExcludeList] [nvarchar](255) NULL,
	[Missing_Replica_Alert] [bit] NULL,
	[First_Alert_Threshold] [float] NULL,
	[Second_Alert_Threshold] [float] NULL,
	[Enabled] [bit] NULL,
	[Name] [nvarchar](255) NULL,
	[ScanInterval] [int] NULL,
	[OffHoursScanInterval] [int] NULL,
	[RetryInterval] [int] NULL,
	[Category] [nvarchar](255) NULL,
	[ServerAName] [nvarchar](255) NULL,
	[ServerBName] [nvarchar](255) NULL,
	[ServerCName] [nvarchar](255) NULL,
 CONSTRAINT [DominoCluster$PrimaryKey] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO


/****** Object:  Table [dbo].[DiskSpace]    Script Date: 09/23/2013 17:00:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DiskSpace](
	[ServerName] [nvarchar](250) NULL,
	[DiskName] [nvarchar](250) NULL,
	[ServerType] [nvarchar](250) NULL,
	[DiskFree] [float] NULL,
	[DiskSize] [float] NULL,
	[PercentFree] [float] NULL,
	[PercentUtilization] [float] NULL,
	[AverageQueueLength] [float] NULL,
	[Updated] [datetime] NULL,
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Threshold] [float] NULL,
 CONSTRAINT [PK_DiskSpace] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  UserDefinedFunction [dbo].[DecodeMaintSchedule]    Script Date: 09/23/2013 17:00:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date, ,>
-- Description:	<Description, ,>
-- =============================================
CREATE FUNCTION [dbo].[DecodeMaintSchedule] 
(
	@MaintType as int,
	@MaintDaysList as varchar(150)
)
RETURNS varchar(150)
AS
BEGIN
	-- Declare the return variable here
	DECLARE @RetVal varchar(150)
	DECLARE @FinalRet varchar(150)
	DECLARE @Freq varchar(150)
	DECLARE @MaintDay varchar(150)
	DECLARE @ID int
	DECLARE @MaintDataCursor CURSOR

	SET @RetVal = ''
	SET @Freq = ''
	SET @MaintDataCursor = CURSOR FAST_FORWARD 

		FOR 
			SELECT * FROM dbo.Split(@MaintDaysList,',')

		OPEN @MaintDataCursor 
	
			FETCH NEXT FROM @MaintDataCursor 
			INTO @ID,@MaintDay
			
			WHILE @@FETCH_STATUS = 0 
	
			BEGIN
				--Print dbo.VSDayOfWeek(@MaintDay)
				SET @RetVal +=
				CASE 
				WHEN CHARINDEX(':',@MaintDay) > 0 THEN dbo.VSDayOfWeek(CAST(SUBSTRING(@MaintDay,1,1) as int)) + ','
				WHEN CHARINDEX(':',@MaintDay) = 0 THEN 
					CASE 
					WHEN @MaintType!=4 THEN dbo.VSDayOfWeek(CAST(@MaintDay as int)) + ','
					ELSE 'Day of month ' + @MaintDay + ','
					END
				END
				SET @Freq =
				CASE 
				WHEN CHARINDEX(':',@MaintDay) > 0 THEN 
					CASE
					WHEN @MaintType!=4 THEN 'Every ' + SUBSTRING(@MaintDay,LEN(@MaintDay),1) + ' weeks on '
					ELSE 'Every ' + SUBSTRING(@MaintDay,CHARINDEX(':',@MaintDay)+1,LEN(@MaintDay)-CHARINDEX(':',@MaintDay)) + ' '
					END
				WHEN CHARINDEX(':',@MaintDay) = 0 THEN ''
				END
				FETCH NEXT FROM @MaintDataCursor 
				INTO @ID,@MaintDay
			END
		CLOSE @MaintDataCursor 
		DEALLOCATE @MaintDataCursor
	-- Add the T-SQL statements to compute the return value here

	-- Return the result of the function
	SET @RetVal = CASE WHEN @Freq='' THEN SUBSTRING(@RetVal,1,LEN(@RetVal)-1) ELSE @Freq + SUBSTRING(@RetVal,1,LEN(@RetVal)-1) END
	RETURN @RetVal

END
GO
/****** Object:  Table [dbo].[AlertSentDetails]    Script Date: 09/23/2013 17:00:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AlertSentDetails](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[SentTo] [nvarchar](500) NULL,
	[AlertClearedDateTime] [datetime] NULL,
	[AlertHistoryID] [int] NULL,
	[AlertCreatedDateTime] [datetime] NULL,
	[AlertKey]	int NULL,
 CONSTRAINT [PK_AlertSentDetails] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO


/****** Object:  Table [dbo].[AlertDetails]    Script Date: 09/23/2013 17:00:25 ******/
/* 4/8/2014 NS added new column to the AlertDetails table for VSPLUS-519 */
/* 12/18/2014 NS added new columns to the table for VSPLUS-946, VSPLUS-1229 */
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[AlertDetails](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[AlertKey] [int] NULL,
	[HoursIndicator] [int] NULL,
	[SendTo] [varchar](500) NULL,
	[CopyTo] [varchar](250) NULL,
	[BlindCopyTo] [varchar](250) NULL,
	[StartTime] [varchar](100) NULL,
	[Day] [varchar](100) NULL,
	[SendSNMPTrap] [bit] NULL,
	[Duration] [int] NOT NULL,
	[EnablePersistentAlert] [bit] NULL,
	[SMSTo] [varchar](50) NULL,
	[ScriptID] [int] NULL,
 CONSTRAINT [PK_AlertDetails] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO

/* 12/29/2014 NS added VSPLUS-1229 */
/****** Object:  Table [dbo].[CustomScripts]    Script Date: 12/18/2014 14:12:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[CustomScripts](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[ScriptName] [varchar](150) NOT NULL,
	[ScriptCommand] [varchar](150) NOT NULL,
	[ScriptLocation] [varchar](250) NOT NULL,
 CONSTRAINT [PK_CustomScripts] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO

/* 10/21/2014 NS modified the table design by adding a new column for VSPLUS-730 */
/****** Object:  Table [dbo].[EventsMaster]    Script Date: 09/23/2013 17:00:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[EventsMaster](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[EventName] [varchar](250) NULL,
	[ServerTypeID] [int] NULL,
	[UpdateAlert] [bit] NULL,
	[AlertOnRepeat] [bit] NULL,
 CONSTRAINT [PK_EventsMaster] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  StoredProcedure [dbo].[GetAlertDetailsAll]    Script Date: 09/23/2013 17:00:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[GetAlertDetailsAll] as 

Declare @strBusinessHrs varchar(1000)
Declare @strOffHrs varchar(1000)
Declare @strSpecificHrs varchar(1000)
Declare @strAllHrs varchar(1000)
Declare @strQuery varchar(5000)

Declare @StartTime time
Declare @EndTime time
Declare @IsSetWeekDay varchar(10)
Declare @sqlquery varchar(1000)

Begin
Set @strBusinessHrs=''
Set @strOffHrs=''
Set @strSpecificHrs=''
Set @strAllHrs=''
Set @strQuery=''
-- Business Hours
-----------------
		select @StartTime=svalue from Settings where sname='BusinessHoursStart'		
		select @EndTime=svalue from Settings where sname='BusinessHoursEnd'
		select @IsSetWeekDay=svalue from Settings where sname=  'BusinessHours'+datename(dw,getdate())
		if @IsSetWeekDay=1 
			begin
			Set @strBusinessHrs='SELECT * FROM AlertDetails WHERE GETDATE() BETWEEN  CONVERT(datetime, CONVERT(VARCHAR(10), GETDATE(), 111) + ''  '+ CONVERT(VARCHAR(10), @StartTime)+''') and 
				DATEADD(MI, Duration, CONVERT(datetime, CONVERT(VARCHAR(10), GETDATE(), 111) + '' '+CONVERT(VARCHAR(10), @EndTime)+'''))
				and HoursIndicator=0 and day like ''%'' + datename(dw,getdate()) +''%''
				or
				GETDATE() BETWEEN
				DATEADD(DD,-1,CONVERT(datetime, CONVERT(VARCHAR(10), GETDATE(), 111) + ''  '+ CONVERT(VARCHAR(10), @StartTime)+''')) and 
				DATEADD(DD,-1,CONVERT(datetime, CONVERT(VARCHAR(10), GETDATE(), 111) + ''  '+ CONVERT(VARCHAR(10), @EndTime)+'''))
				and HoursIndicator=0 and day like ''%'' + datename(dw,getdate()) +''%'''
			end
-- Off Hours
-----------------
		select @StartTime=svalue from Settings where sname='BusinessHoursStart'	
		select @EndTime=svalue from Settings where sname='BusinessHoursEnd'
		select @IsSetWeekDay=svalue from Settings where sname=  'BusinessHours'+datename(dw,getdate())
			if @IsSetWeekDay=1 
			begin
			Set @strOffHrs='SELECT * FROM AlertDetails WHERE (GETDATE() BETWEEN  CONVERT(datetime, CONVERT(VARCHAR(10), GETDATE(), 111) + ''  '+ CONVERT(VARCHAR(10), @EndTime)+''') and 
				DATEADD(DD,1,DATEADD(MI, Duration, CONVERT(datetime, CONVERT(VARCHAR(10), GETDATE(), 111) + ''  '+ CONVERT(VARCHAR(10), @StartTime)+''')))) 
				and HoursIndicator=1 and day like ''%'' + datename(dw,getdate()) +''%''
				or
				(GETDATE() BETWEEN
				DATEADD(DD,-1,CONVERT(datetime, CONVERT(VARCHAR(10), GETDATE(), 111) + ''  '+ CONVERT(VARCHAR(10), @EndTime)+''')) and 
				CONVERT(datetime, CONVERT(VARCHAR(10), GETDATE(), 111) + ''  '+ CONVERT(VARCHAR(10), @StartTime)+'''))  
				and HoursIndicator=1 and day like ''%'' + datename(dw,getdate()) +''%'''
			end
-- Specific Hours
-----------------
		select @StartTime=svalue from Settings where sname='BusinessHoursStart'	
		select @EndTime=svalue from Settings where sname='BusinessHoursEnd'
		select @IsSetWeekDay=svalue from Settings where sname=  'BusinessHours'+datename(dw,getdate())
			if @IsSetWeekDay=1 
			begin
			Set @strSpecificHrs='SELECT * FROM AlertDetails WHERE '+
			'GETDATE() BETWEEN CONVERT(datetime, CONVERT(VARCHAR(10), GETDATE(), 111) + '' '' + LTRIM(RIGHT(CONVERT(VARCHAR(20), StartTime, 100), 7))) and DATEADD(MI, Duration, CONVERT(datetime, CONVERT(VARCHAR(10), GETDATE(), 111) + '' '' + LTRIM(RIGHT(CONVERT(VARCHAR(20), StartTime, 100), 7))))		
			 and HoursIndicator=2 and day like ''%'' + datename(dw,getdate()) +''%''
			 or
			GETDATE() BETWEEN DATEADD(DD,-1,CONVERT(datetime, CONVERT(VARCHAR(10), GETDATE(), 111) + '' '' + LTRIM(RIGHT(CONVERT(VARCHAR(20), StartTime, 100), 7)))) and DATEADD(DD,-1,DATEADD(MI, Duration, CONVERT(datetime, CONVERT(VARCHAR(10), GETDATE(), 111) + '' '' + LTRIM(RIGHT(CONVERT(VARCHAR(20), StartTime, 100), 7)))))'		
				
			end
-- All Hours
-----------------
		Set @strAllHrs='SELECT * FROM AlertDetails where HoursIndicator=3'
-----------------

--Add union to all of the strings to combine relevant selects and execute 	
if @strBusinessHrs<>''
begin
	Set @strBusinessHrs=@strBusinessHrs + ' union '
end
if @strOffHrs<>''
begin
	Set @strOffHrs=@strOffHrs + ' union '
end
if @strSpecificHrs<>''
begin
	Set @strSpecificHrs=@strSpecificHrs + ' union '
end

Set @strQuery=@strBusinessHrs+@strOffHrs+@strSpecificHrs+@strAllHrs
exec( @strQuery)

--if CHARINDEX(' union ',@strQuery)>0
--begin
--	Set @strQuery= SUBSTRING(@strQuery,0,LEN(@strQuery)-7)-- CHARINDEX(' union ',@strQuery)-1)	
--end
--select  @strBusinessHrs
--select  @strOffHrs
--select  @strSpecificHrs
--select  @strAllHrs
--select @strQuery

End
GO
/****** Object:  StoredProcedure [dbo].[GetRecentScanResults]    Script Date: 09/23/2013 17:00:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[GetRecentScanResults]
	@ServerType as varchar(150), @TimeInterval as int
AS
BEGIN
	SELECT COUNT(*) N FROM STATUS 
	WHERE Type = @ServerType AND Status <> 'Scanning' AND 
	Status <> 'Not Scanned' AND Status <> 'Disabled' AND 
	Status <> 'Insufficient Licenses' 
	AND LastUpdate >= DATEADD(minute,@TimeInterval,GETDATE()) 
END
GO


/****** Object:  Table [dbo].[Traveler_Status]    Script Date: 09/23/2013 16:59:36 ******/
/* 1/16/2014 NS added new columns to the Traveler_Status table for HA functionality */
/* 1/24/2014 NS added new column to the Traveler_Status table for HA Datastore */
/* 5/15/2015 NS added new column to the Traveler_Status table for ResourceConstraint VSPLUS-1754 */
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Traveler_Status](
	[ServerName] [nvarchar](255) NULL,
	[Status] [nvarchar](255) NULL,
	[Details] [nvarchar](255) NULL,
	[Users] [int] NULL,
	[IncrementalSyncs] [int] NULL,
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Devices] [int] NULL,
	[HTTP_Status] [nchar](255) NULL,
	[HTTP_Details] [nchar](255) NULL,
	[HTTP_PeakConnections] [int] NULL,
	[HTTP_MaxConfiguredConnections] [int] NULL,
	[TravelerVersion] [nchar](255) NULL,
	[Availability_Index] [int] NULL,
	[HA] [bit] NULL,
	[HAPoolName] [varchar](255) NULL,
	[TravelerServlet] [varchar](255) NULL,
	[HA_Datastore_Status] [varchar](50) NULL,
	[HeartBeat] [varchar](50) NULL,
	[DevicesAPIStatus] [varchar](25) NULL,
	[DominoServerId] [int] NULL,
	[ResourceConstraint] [varchar](50) NULL
) ON [PRIMARY]
GO

ALTER TABLE TRAVELER_STATUS
ADD FOREIGN KEY ([DominoServerId])
REFERENCES Servers (ID)
ON DELETE CASCADE

GO
/* 12/8/2015 NS modified for VSPLUS-2227 */
/****** Object:  Table [dbo].[ServerMaintenance]    Script Date: 09/23/2013 17:00:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ServerMaintenance](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[MaintID] [int] NOT NULL,
	[ServerID] [int] NOT NULL,
	[ServerTypeID] [int] NOT NULL,
	[DeviceID] [nvarchar](250) NULL,
 CONSTRAINT [PK_ServerMaintenance] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UserLocationRestrictions]    Script Date: 09/23/2013 17:00:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserLocationRestrictions](
	[UserID] [int] NOT NULL,
	[LocationID] [int] NOT NULL,
 CONSTRAINT [PK_UserLocationRestrictions] PRIMARY KEY CLUSTERED 
(
	[UserID] ASC,
	[LocationID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  View [dbo].[TravelerGridData]    Script Date: 09/23/2013 17:00:28 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[TravelerGridData]
AS

SELECT MailServerName, [000-001], [001-002], [002-005], [005-010], [010-030], [030-060], 
[060-120], [120-INF], DateUpdated 
FROM
(SELECT mailservername, interval, delta,dateupdated 
    FROM TravelerStats
    where MailServerName != '' and DateUpdated = (select MAX(dateupdated) from dbo.TravelerStats )) AS SourceTable
PIVOT
(
AVG(delta)
FOR interval IN ([000-001], [001-002], [002-005], [005-010], [010-030], [030-060], 
[060-120], [120-INF])
) AS PivotTable
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[32] 4[30] 2[20] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'TravelerGridData'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=1 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'TravelerGridData'
GO
/****** Object:  Table [dbo].[UserServerTypeRestrictions]    Script Date: 09/23/2013 17:00:28 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserServerTypeRestrictions](
	[UserID] [int] NOT NULL,
	[ServerTypeID] [int] NOT NULL,
 CONSTRAINT [PK_UserServerTypeRestrictions] PRIMARY KEY CLUSTERED 
(
	[UserID] ASC,
	[ServerTypeID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UserServerRestrictions]    Script Date: 09/23/2013 17:00:28 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserServerRestrictions](
	[UserID] [int] NOT NULL,
	[ServerID] [int] NOT NULL,
 CONSTRAINT [PK_UserRestrictions] PRIMARY KEY CLUSTERED 
(
	[UserID] ASC,
	[ServerID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SametimeServers]    Script Date: 09/23/2013 17:00:28 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SametimeServers](
	[ServerID] [int] NULL,
	[Category] [nvarchar](255) NULL,
	[UserThreshold] [int] NULL,
	[ChatThreshold] [int] NULL,
	[NChatThreshold] [int] NULL,
	[PlacesThreshold] [int] NULL,
	[Enabled] [bit] NULL,
	[ScanInterval] [int] NULL,
	[OffHoursScanInterval] [int] NULL,
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[RetryInterval] [int] NULL,
	[ResponseThreshold] [int] NULL,
	[nserver] [bit] NULL,
	[stcommlaunch] [bit] NULL,
	[stcommunity] [bit] NULL,
	[stconfigurationapp] [bit] NULL,
	[stplaces] [bit] NULL,
	[stmux] [bit] NULL,
	[stusers] [bit] NULL,
	[stonlinedir] [bit] NULL,
	[stdirectory] [bit] NULL,
	[stlogger] [bit] NULL,
	[stlinks] [bit] NULL,
	[stprivacy] [bit] NULL,
	[stsecurity] [bit] NULL,
	[stpresencemgr] [bit] NULL,
	[stpresencesubmgr] [bit] NULL,
	[steventserver] [bit] NULL,
	[stpolicy] [bit] NULL,
	[stconfigurationbridge] [bit] NULL,
	[stadminsrv] [bit] NULL,
	[stuserstorage] [bit] NULL,
	[stchatlogging] [bit] NULL,
	[stpolling] [bit] NULL,
	[stpresencecompatmgr] [bit] NULL,
	[SSL] [bit] NULL,
	[stservicemanager] [bit] NULL,
	[stresolve] [bit] NULL,
	[stconference] [bit] NULL,
	[FailureThreshold] [int] NULL,
	[MonitoredBy] [int] NULL,
	[srvawareness] [bit] NULL,
[srvdirectory] [bit] NULL,
[srvstorage] [bit] NULL,
[srvbuddylist] [bit] NULL,
[srvplace] [bit] NULL,
[srvlookup] [bit] NULL,
[srvtestchat] [bit] NULL,
[srvtestmeeting] [bit] NULL,
[generalport] [int] NULL,
[proxytype] [varchar](200) NULL,
[proxyprotocol] [varchar](200) NULL,
[db2hostname] [varchar](200) NULL,
[db2databasename] [varchar](200) NULL,
[enabledb2port] [bit] NULL,
[db2port] [varchar](200) NULL,
[srvquery] [bit] NULL,
[CredentialID] [int] NULL,
[Platform] [nvarchar] (50) NULL,
[WsScanMeetingServer] [bit] NULL,
[WsMeetingHost] [nvarchar](255) NULL,
[WsMeetingRequireSSL] [bit] NULL,
[WsMeetingPort] [int] NULL,
[WsScanMediaServer] [bit] NULL,
[WsMediaHost] [nvarchar](255) NULL,
[WsMediaRequireSSL] [bit] NULL,
[WsMediaPort] [int] NULL,
[ChatUser1CredentialsId] [INT],
[ChatUser2CredentialsId] [INT],
[STExtendedStatsPort] [INT],
[STScanExtendedStats] [BIT],
[TestChatSimulation] [BIT],
 CONSTRAINT [SametimeServers$PrimaryKey] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
--VSPLUS 594 Durga Feedback--
use [vitalsigns]
GO
IF NOT EXISTS (select * from syscolumns where id = object_id('dbo.Feedback'))
BEGIN	SET ANSI_NULLS ON

SET QUOTED_IDENTIFIER ON

CREATE TABLE [dbo].[Feedback](
	[Subject] [nvarchar](50) NULL,
	[Type] [nvarchar](50) NULL,
	[Message] [nvarchar](200) NULL,
	[Status] [nvarchar](50) NULL,
	[Attachments] [nvarchar](100) NULL,
	[ID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]
END
GO
--VSPLUS 1100,378 Sowjanya ScanSettings--
USE [vitalsigns]
GO
IF NOT EXISTS (select * from syscolumns where id = object_id('dbo.ScanSettings'))
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ScanSettings](
	[sname] [nvarchar](250) NOT NULL,
	[svalue] [nvarchar](250) NULL,
	[stype] [nvarchar](250) NULL,
	[EnableReport] [bit] NULL,
	[ScanInterval] [int] NULL,
	[Priority] [int] NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[NetworkMaster]    Script Date: 11/25/2014 17:32:10 ******/
USE [vitalsigns]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[NetworkMaster](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](max) NULL,
	[Image] [nvarchar](100) NULL
) ON [PRIMARY]
GO

/****** Object:  Table [dbo].[NotesMailProbe]    Script Date: 09/23/2013 17:00:28 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[NotesMailProbe](
	[Enabled] [bit] NULL,
	[Name] [nvarchar](50) NOT NULL,
	[NotesMailAddress] [nvarchar](255) NOT NULL,
	[Category] [nvarchar](255) NULL,
	[ScanInterval] [int] NULL,
	[OffHoursScanInterval] [int] NULL,
	[DeliveryThreshold] [int] NULL,
	[RetryInterval] [int] NULL,
	[DestinationServerID] [int] NULL,
	[DestinationDatabase] [nvarchar](50) NULL,
	[SourceServer] [nvarchar](50) NULL,
	[EchoService] [bit] NULL,
	[ReplyTo] [nvarchar](150) NULL,
	[Filename] [nvarchar](255) NULL,
 CONSTRAINT [NotesMailProbe$PrimaryKey] PRIMARY KEY CLUSTERED 
(
	[Name] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[NotesDatabases]    Script Date: 09/23/2013 17:00:28 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[NotesDatabases](
	[Name] [nvarchar](255) NULL,
	[ServerID] [int] NULL,
	[Category] [nvarchar](255) NULL,
	[ScanInterval] [int] NULL,
	[OffHoursScanInterval] [int] NULL,
	[Enabled] [bit] NULL,
	[ResponseThreshold] [int] NULL,
	[RetryInterval] [int] NULL,
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[ServerName] [nvarchar](100) NULL,
	[FileName] [nvarchar](100) NULL,
	[TriggerType] [nvarchar](100) NULL,
	[TriggerValue] [float] NULL,
	[AboveBelow] [nvarchar](255) NULL,
	[ReplicationDestination] [nvarchar](255) NULL,
	[InitiateReplication] [bit] NULL,
 CONSTRAINT [NotesDatabases$PrimaryKey] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[MailServices]    Script Date: 09/23/2013 17:00:28 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MailServices](
	[Address] [nvarchar](50) NULL,
	[Name] [nvarchar](50) NULL,
	[Description] [nvarchar](255) NULL,
	[Category] [nvarchar](255) NULL,
	[ScanInterval] [int] NULL,
	[OffHoursScanInterval] [int] NULL,
	[NextScan] [datetime] NULL,
	[LastChecked] [datetime] NULL,
	[LastStatus] [nvarchar](50) NULL,
	[Enabled] [bit] NULL,
	[ResponseThreshold] [int] NULL,
	[RetryInterval] [int] NULL,
	[key] [int] IDENTITY(1,1) NOT NULL,
	[Port] [smallint] NULL,
	[FailureThreshold] [smallint] NULL,
	[LocationId] [int] NULL,
	[ServerTypeId] [int],
 CONSTRAINT [MailServices$PrimaryKey] PRIMARY KEY CLUSTERED 
(
	[key] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[MailServices]  WITH CHECK ADD  CONSTRAINT [fk_MailServices_LocationID] FOREIGN KEY([LocationId])
REFERENCES [dbo].[Locations] ([ID])
GO

ALTER TABLE [dbo].[MailServices] CHECK CONSTRAINT [fk_MailServices_LocationID]
GO

/****** Object:  StoredProcedure [dbo].[GetAlertDetails]    Script Date: 09/23/2013 17:00:28 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
Create procedure [dbo].[GetAlertDetails](@HoursIndicator int) as 

--Declare @HoursIndicator int
Declare @StartTime time
Declare @EndTime time
Declare @IsSetWeekDay varchar(10)
Declare @sqlquery varchar(1000)
--set @HoursIndicator=1
Begin
	if @HoursIndicator=0 -- Business Hours
		begin
		select @StartTime=svalue from Settings where sname='BusinessHoursStart'
		
		select @EndTime=svalue from Settings where sname='BusinessHoursEnd'
		select @IsSetWeekDay=svalue from Settings where sname=  'BusinessHours'+datename(dw,getdate())
			if @IsSetWeekDay=1 
			begin
				SELECT * FROM AlertDetails WHERE GETDATE() BETWEEN  CONVERT(datetime, CONVERT(VARCHAR(10), GETDATE(), 111) + ' ' + CONVERT(VARCHAR(10), @StartTime)) and 
				DATEADD(MI, Duration, CONVERT(datetime, CONVERT(VARCHAR(10), GETDATE(), 111) + ' ' + CONVERT(VARCHAR(10), @EndTime)))
				and HoursIndicator=0 and day like '%' + datename(dw,getdate()) +'%'
				or
				GETDATE() BETWEEN
				DATEADD(DD,-1,CONVERT(datetime, CONVERT(VARCHAR(10), GETDATE(), 111) + ' ' + CONVERT(VARCHAR(10), @StartTime))) and 
				DATEADD(DD,-1,CONVERT(datetime, CONVERT(VARCHAR(10), GETDATE(), 111) + ' ' + CONVERT(VARCHAR(10), @EndTime)))
				and HoursIndicator=0 and day like '%' + datename(dw,getdate()) +'%'
			end
	end
	else if @HoursIndicator=1 -- Off Hours
		begin
		select @StartTime=svalue from Settings where sname='BusinessHoursStart'	
		select @EndTime=svalue from Settings where sname='BusinessHoursEnd'
		select @IsSetWeekDay=svalue from Settings where sname=  'BusinessHours'+datename(dw,getdate())
			if @IsSetWeekDay=1 
			begin
			SELECT * FROM AlertDetails WHERE (GETDATE() BETWEEN  CONVERT(datetime, CONVERT(VARCHAR(10), GETDATE(), 111) + ' ' + CONVERT(VARCHAR(10), @EndTime)) and 
				DATEADD(DD,1,DATEADD(MI, Duration, CONVERT(datetime, CONVERT(VARCHAR(10), GETDATE(), 111) + ' ' + CONVERT(VARCHAR(10), @StartTime))))) 
				and HoursIndicator=1 and day like '%' + datename(dw,getdate()) +'%'
				or
				(GETDATE() BETWEEN
				DATEADD(DD,-1,CONVERT(datetime, CONVERT(VARCHAR(10), GETDATE(), 111) + ' ' + CONVERT(VARCHAR(10), @EndTime))) and 
				CONVERT(datetime, CONVERT(VARCHAR(10), GETDATE(), 111) + ' ' + CONVERT(VARCHAR(10), @StartTime)))  
				and HoursIndicator=1 and day like '%' + datename(dw,getdate()) +'%'
			end
	end
	else if @HoursIndicator=2 -- Specific Hours
	begin
		select @StartTime=svalue from Settings where sname='BusinessHoursStart'	
		select @EndTime=svalue from Settings where sname='BusinessHoursEnd'
		select @IsSetWeekDay=svalue from Settings where sname=  'BusinessHours'+datename(dw,getdate())
			if @IsSetWeekDay=1 
			begin
			SELECT * FROM AlertDetails WHERE GETDATE() BETWEEN CONVERT(datetime, CONVERT(VARCHAR(10), GETDATE(), 111) + ' ' + LTRIM(RIGHT(CONVERT(VARCHAR(20), StartTime, 100), 7))) and DATEADD(MI, Duration, CONVERT(datetime, CONVERT(VARCHAR(10), GETDATE(), 111) + ' ' + LTRIM(RIGHT(CONVERT(VARCHAR(20), StartTime, 100), 7))))		
				and HoursIndicator=2 and day like '%' + datename(dw,getdate()) +'%'
				or
				(GETDATE() BETWEEN
				DATEADD(DD,-1,CONVERT(datetime, CONVERT(VARCHAR(10), GETDATE(), 111) + ' ' + CONVERT(VARCHAR(10), @EndTime))) and 
				CONVERT(datetime, CONVERT(VARCHAR(10), GETDATE(), 111) + ' ' + CONVERT(VARCHAR(10), @StartTime)))  
				and HoursIndicator=2 and day like '%' + datename(dw,getdate()) +'%'
			end
	end
	else -- All Hours
	begin
		SELECT * FROM AlertDetails where HoursIndicator=3 --and day like '%' + datename(dw,getdate()) +'%'
	end

End
GO
/****** Object:  Table [dbo].[DominoServers]    Script Date: 09/23/2013 17:00:28 ******/
/* 8/7/2014 NS modified - added CredentialsID column for VSPLUS-853 */
/* 6/25/2015 NS modified  - added EXJournal columns for VSPLUS-1802 */
/* 2/22/2016 NS modified - added AvailabilityIndexThreshold for VSPLUS-2641 */
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DominoServers](
	[ServerID] [int] NULL,
	[Category] [nvarchar](255) NULL,
	[PendingThreshold] [int] NULL,
	[DeadThreshold] [int] NULL,
	[Enabled] [bit] NULL,
	[Scan Interval] [int] NULL,
	[OffHoursScanInterval] [int] NULL,
	[Key] [int] IDENTITY(1,1) NOT NULL,
	[MailDirectory] [nvarchar](50) NULL,
	[RetryInterval] [int] NULL,
	[ResponseThreshold] [int] NULL,
	[BES_Server] [bit] NULL,
	[BES_Threshold] [int] NULL,
	[FailureThreshold] [int] NULL,
	[SearchString] [nvarchar](255) NULL,
	[AdvancedMailScan] [bit] NULL,
	[DeadMailDeleteThreshold] [int] NULL,
	[DiskSpaceThreshold] [float] NULL,
	[HeldThreshold] [float] NULL,
	[ScanDBHealth] [bit] NULL,
	[NotificationGroup] [nvarchar](255) NULL,
	[Memory_Threshold] [float] NULL,
	[CPU_Threshold] [float] NULL,
	[Cluster_Rep_Delays_Threshold] [float] NULL,
	[Load_Cluster_Rep_Delays_Threshold] [float] NULL,
	[Modified_By] [int] NULL,
	[Modified_On] [date] NULL,
	[ServerDaysAlert] [int] NULL,
	[MonitoredBy] [int] NULL,
	[RequireSSL] [bit] NULL,
	[ExternalAlias] [nvarchar](100) NULL,
	[CredentialsID] [int] NULL,
	[CheckMailThreshold] [int] NULL,
	[scanlog] [bit] NULL,
	[scanagentlog] [bit] NULL,
	[SendRouterRestart] [bit] NULL,
	[EnableTravelerBackend] [bit] NULL,
	[ScanServlet] [bit] NULL,
	[ScanTravelerServer] [bit] NOT NULL,
	[EXJStartTime] [nvarchar](50) NULL,
	[EXJDuration] [int] NULL,
	[EXJLookBackDuration] [int] NULL,
	[EXJEnabled] [bit] NOT NULL,
	[AvailabilityIndexThreshold] [int] NULL,
 CONSTRAINT [DominoServers$PrimaryKey] PRIMARY KEY CLUSTERED 
(
	[Key] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[DominoServers] ADD  DEFAULT ((1)) FOR [ScanTravelerServer]
GO
/* 6/25/2015 NS added for VSPLUS-1802 */
ALTER TABLE [dbo].[DominoServers] ADD  CONSTRAINT [DF_DominoServers_EXJEnabled]  DEFAULT ((0)) FOR [EXJEnabled]
GO

/****** Object:  Table [dbo].[DominoCustomStatValues]    Script Date: 09/23/2013 17:00:28 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DominoCustomStatValues](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[ServerID] [int] NULL,
	[ServerName] [nvarchar](50) NULL,
	[StatName] [nvarchar](50) NULL,
	[ThresholdValue] [float] NULL,
	[GreaterThanORLessThan] [nvarchar](50) NULL,
	[TimesInARow] [int] NULL,
	[ConsoleCommand] [nvarchar](255) NULL,
 CONSTRAINT [DominoCustomStatValues$PrimaryKey] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AlertEvents]    Script Date: 09/23/2013 17:00:28 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AlertEvents](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[AlertKey] [int] NULL,
	[EventID] [int] NULL,
	[ServerTypeID] [int] NULL,
 CONSTRAINT [PK_AlertEvents] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AlertServers]    Script Date: 09/23/2013 17:00:28 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/* 12/5/2013 NS added ServerTypeID column */
CREATE TABLE [dbo].[AlertServers](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[AlertKey] [int] NULL,
	[ServerID] [int] NULL,
	[LocationID] [int] NULL,
	[ServerTypeID] [int] NULL
 CONSTRAINT [PK_AlertServers] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/* 11/4/2015 NS modified for VSPLUS-2023*/
/* 11/10/2015 NS modified for VSPLUS-2023*/
/****** Object:  View [dbo].[ConfigUsersList]    Script Date: 09/23/2013 17:00:28 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[ConfigUsersList]
AS
(SELECT     FullName, t5.ID, ServerName AS Name, LocationID LocId, Location, ServerTypeID srvtypeid, ServerType
FROM         Users t5, servers t1 INNER JOIN
                      servertypes t2 ON ServerTypeId = t2.ID INNER JOIN
                      locations t3 ON LocationID = t3.ID
UNION
SELECT     FullName, t5.ID, Name, LocationID LocID, Location, 7, 'URL'
FROM         Users t5, URLs t1 INNER JOIN
                      servertypes t2 ON t2.ID = 7 INNER JOIN
                      locations t3 ON LocationID = t3.ID)
EXCEPT
(SELECT     FullName, t5.ID, ServerName AS Name, t1.LocationID LocId, Location, ServerTypeID srvtypeid, ServerType
 FROM         Servers t1 INNER JOIN
                        Locations t2 ON t1.LocationID = t2.ID INNER JOIN
                        ServerTypes t3 ON ServerTypeID = t3.ID INNER JOIN
                        UserLocationRestrictions t4 ON t4.LocationId = t2.ID INNER JOIN
                        Users t5 ON t4.UserID = t5.ID
UNION
SELECT     FullName, t5.ID, t1.Name, t1.LocationID LocId, Location, 7, 'URL'
FROM         URLs t1 INNER JOIN
                      Locations t2 ON t1.LocationID = t2.ID INNER JOIN
                      UserLocationRestrictions t4 ON t4.LocationId = t2.ID INNER JOIN
                      Users t5 ON t4.UserID = t5.ID
UNION
SELECT     FullName, t5.ID, ServerName AS Name, t1.LocationID LocId, Location, ServerTypeID srvtypeid, ServerType
FROM         Servers t1 INNER JOIN
                      Locations t2 ON t1.LocationID = t2.ID INNER JOIN
                      ServerTypes t3 ON ServerTypeID = t3.ID INNER JOIN
                      UserServerRestrictions t4 ON t4.ServerId = t1.ID INNER JOIN
                      Users t5 ON t4.UserID = t5.ID
UNION
SELECT     FullName, t5.ID, t1.Name, t1.LocationID LocId, Location, 7, 'URL'
FROM         URLs t1 INNER JOIN
                      Locations t2 ON LocationID = t2.ID INNER JOIN
                      UserServerRestrictions t4 ON t4.ServerId = t1.ID INNER JOIN
                      Users t5 ON t4.UserID = t5.ID)
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[6] 4[37] 2[39] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'ConfigUsersList'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=1 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'ConfigUsersList'
GO
/****** Object:  View [dbo].[AlertDefinitions1_view]    Script Date: 09/23/2013 17:00:28 ******/
/* 1/13/2014 NS modified the view - AlertLocations and AlertDeviceTypes tables no longer exists */
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE view [dbo].[AlertDefinitions1_view] as
SELECT      ad.SendTo, ad.CopyTo, ad.BlindCopyTo, ad.Day, ad.SendSNMPTrap, st.ServerType, an.AlertKey, 
                      lo.Location, srv.ServerName, evt.EventName
FROM         dbo.AlertDetails AS ad INNER JOIN
                      dbo.AlertNames AS an ON ad.AlertKey = an.AlertKey INNER JOIN
                      dbo.AlertServers AS asr ON an.AlertKey = asr.AlertKey left outer JOIN
                      dbo.ServerTypes AS st ON asr.ServerTypeID = st.ID INNER JOIN
                      dbo.Locations AS lo ON asr.LocationID = lo.ID left JOIN
                      dbo.Servers AS srv ON asr.ServerID = srv.ID left outer JOIN
                      dbo.AlertEvents AS ae ON an.AlertKey = ae.AlertKey left outer JOIN
                      dbo.EventsMaster AS evt ON ae.EventID = evt.ID
WHERE     (GETDATE() BETWEEN CONVERT(datetime, CONVERT(VARCHAR(10), GETDATE(), 111) + ' ' + ad.StartTime) AND DATEADD(MI, ad.Duration, 
                      CONVERT(datetime, CONVERT(VARCHAR(10), GETDATE(), 111) + ' ' + ad.StartTime)))
GO
/****** Object:  Default [DF_TravelerStats_TravelerServerName]    Script Date: 09/23/2013 16:59:36 ******/
ALTER TABLE [dbo].[TravelerStats] ADD  CONSTRAINT [DF_TravelerStats_TravelerServerName]  DEFAULT ('dom1') FOR [TravelerServerName]
GO
/****** Object:  Default [DF_Users_Refreshtime]    Script Date: 09/23/2013 16:59:36 ******/
--ALTER TABLE [dbo].[Users] ADD  CONSTRAINT [DF_Users_Refreshtime]  DEFAULT ((30)) FOR [Refreshtime]
--GO
/****** Object:  Default [DF_DominoServers_Modified_By]    Script Date: 09/23/2013 17:00:28 ******/
ALTER TABLE [dbo].[DominoServers] ADD  CONSTRAINT [DF_DominoServers_Modified_By]  DEFAULT ((0)) FOR [Modified_By]
GO
/****** Object:  ForeignKey [FK_AlertSentDetails_AlertHistory]    Script Date: 09/23/2013 17:00:25 ******/
ALTER TABLE [dbo].[AlertSentDetails]  WITH CHECK ADD  CONSTRAINT [FK_AlertSentDetails_AlertHistory] FOREIGN KEY([AlertHistoryID])
REFERENCES [dbo].[AlertHistory] ([ID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AlertSentDetails] CHECK CONSTRAINT [FK_AlertSentDetails_AlertHistory]
GO
/****** Object:  ForeignKey [FK_AlertLocations_AlertNames]    Script Date: 09/23/2013 17:00:25 ******/
/* 1/13/2014 NS commented out - AlertLocations and AlertDeviceTypes tables no longer exists */

/****** Object:  ForeignKey [FK_AlertDetails_AlertNames]    Script Date: 09/23/2013 17:00:25 ******/
ALTER TABLE [dbo].[AlertDetails]  WITH CHECK ADD  CONSTRAINT [FK_AlertDetails_AlertNames] FOREIGN KEY([AlertKey])
REFERENCES [dbo].[AlertNames] ([AlertKey])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AlertDetails] CHECK CONSTRAINT [FK_AlertDetails_AlertNames]
GO

/* 12/18/2014 NS added for VSPLUS-946, VSPLUS-1229 */
ALTER TABLE [dbo].[AlertDetails]  WITH CHECK ADD  CONSTRAINT [FK_AlertDetails_CustomScripts] FOREIGN KEY([ScriptID])
REFERENCES [dbo].[CustomScripts] ([ID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AlertDetails] CHECK CONSTRAINT [FK_AlertDetails_CustomScripts]
GO

/* 7/10/2015 NS added for VSPLUS-1926 */
ALTER TABLE [dbo].[AlertDetails]  WITH CHECK ADD  CONSTRAINT [FK_AlertDetails_HoursIndicator] FOREIGN KEY([HoursIndicator])
REFERENCES [dbo].[HoursIndicator] ([ID])
GO
ALTER TABLE [dbo].[AlertDetails] CHECK CONSTRAINT [FK_AlertDetails_HoursIndicator]
GO

/****** Object:  ForeignKey [FK_EventsMaster_ServerTypes]    Script Date: 09/23/2013 17:00:25 ******/
ALTER TABLE [dbo].[EventsMaster]  WITH CHECK ADD  CONSTRAINT [FK_EventsMaster_ServerTypes] FOREIGN KEY([ServerTypeID])
REFERENCES [dbo].[ServerTypes] ([ID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[EventsMaster] CHECK CONSTRAINT [FK_EventsMaster_ServerTypes]
GO

/****** Object:  ForeignKey [FK_Servers_Location]    Script Date: 09/23/2013 17:00:26 ******/
ALTER TABLE [dbo].[Servers]  WITH CHECK ADD  CONSTRAINT [FK_Servers_Location] FOREIGN KEY([LocationID])
REFERENCES [dbo].[Locations] ([ID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Servers] CHECK CONSTRAINT [FK_Servers_Location]
GO
/****** Object:  ForeignKey [FK_ServerMaintenance_Maintenance]    Script Date: 09/23/2013 17:00:26 ******/
ALTER TABLE [dbo].[ServerMaintenance]  WITH CHECK ADD  CONSTRAINT [FK_ServerMaintenance_Maintenance] FOREIGN KEY([MaintID])
REFERENCES [dbo].[Maintenance] ([ID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ServerMaintenance] CHECK CONSTRAINT [FK_ServerMaintenance_Maintenance]
GO
/****** Object:  ForeignKey [FK_UserLocationRestrictions_Location]    Script Date: 09/23/2013 17:00:26 ******/
ALTER TABLE [dbo].[UserLocationRestrictions]  WITH CHECK ADD  CONSTRAINT [FK_UserLocationRestrictions_Location] FOREIGN KEY([LocationID])
REFERENCES [dbo].[Locations] ([ID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[UserLocationRestrictions] CHECK CONSTRAINT [FK_UserLocationRestrictions_Location]
GO
/****** Object:  ForeignKey [FK_UserLocationRestrictions_User]    Script Date: 09/23/2013 17:00:26 ******/
ALTER TABLE [dbo].[UserLocationRestrictions]  WITH CHECK ADD  CONSTRAINT [FK_UserLocationRestrictions_User] FOREIGN KEY([UserID])
REFERENCES [dbo].[Users] ([ID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[UserLocationRestrictions] CHECK CONSTRAINT [FK_UserLocationRestrictions_User]
GO
/****** Object:  ForeignKey [FK_UserServerTypeRestrictions_ServerType]    Script Date: 09/23/2013 17:00:28 ******/
ALTER TABLE [dbo].[UserServerTypeRestrictions]  WITH CHECK ADD  CONSTRAINT [FK_UserServerTypeRestrictions_ServerType] FOREIGN KEY([ServerTypeID])
REFERENCES [dbo].[ServerTypes] ([ID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[UserServerTypeRestrictions] CHECK CONSTRAINT [FK_UserServerTypeRestrictions_ServerType]
GO
/****** Object:  ForeignKey [FK_UserServerTypeRestrictions_User]    Script Date: 09/23/2013 17:00:28 ******/
ALTER TABLE [dbo].[UserServerTypeRestrictions]  WITH CHECK ADD  CONSTRAINT [FK_UserServerTypeRestrictions_User] FOREIGN KEY([UserID])
REFERENCES [dbo].[Users] ([ID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[UserServerTypeRestrictions] CHECK CONSTRAINT [FK_UserServerTypeRestrictions_User]
GO
/****** Object:  ForeignKey [FK_UserServerRestrictions_Server]    Script Date: 09/23/2013 17:00:28 ******/
ALTER TABLE [dbo].[UserServerRestrictions]  WITH CHECK ADD  CONSTRAINT [FK_UserServerRestrictions_Server] FOREIGN KEY([ServerID])
REFERENCES [dbo].[Servers] ([ID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[UserServerRestrictions] CHECK CONSTRAINT [FK_UserServerRestrictions_Server]
GO
/****** Object:  ForeignKey [FK_UserServerRestrictions_User]    Script Date: 09/23/2013 17:00:28 ******/
ALTER TABLE [dbo].[UserServerRestrictions]  WITH CHECK ADD  CONSTRAINT [FK_UserServerRestrictions_User] FOREIGN KEY([UserID])
REFERENCES [dbo].[Users] ([ID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[UserServerRestrictions] CHECK CONSTRAINT [FK_UserServerRestrictions_User]
GO
/****** Object:  ForeignKey [FK_SametimeServers_Servers]    Script Date: 09/23/2013 17:00:28 ******/
ALTER TABLE [dbo].[SametimeServers]  WITH CHECK ADD  CONSTRAINT [FK_SametimeServers_Servers] FOREIGN KEY([ServerID])
REFERENCES [dbo].[Servers] ([ID])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[SametimeServers] CHECK CONSTRAINT [FK_SametimeServers_Servers]
GO

/****** Object:  ForeignKey [FK_NotesMailProbe_Servers]    Script Date: 09/23/2013 17:00:28 ******/
ALTER TABLE [dbo].[NotesMailProbe]  WITH CHECK ADD  CONSTRAINT [FK_NotesMailProbe_Servers] FOREIGN KEY([DestinationServerID])
REFERENCES [dbo].[Servers] ([ID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[NotesMailProbe] CHECK CONSTRAINT [FK_NotesMailProbe_Servers]
GO
/****** Object:  ForeignKey [FK_NotesDatabases_Servers]    Script Date: 09/23/2013 17:00:28 ******/
ALTER TABLE [dbo].[NotesDatabases]  WITH CHECK ADD  CONSTRAINT [FK_NotesDatabases_Servers] FOREIGN KEY([ServerID])
REFERENCES [dbo].[Servers] ([ID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[NotesDatabases] CHECK CONSTRAINT [FK_NotesDatabases_Servers]
GO

/****** Object:  ForeignKey [FK_DominoServers_Servers]    Script Date: 09/23/2013 17:00:28 ******/
ALTER TABLE [dbo].[DominoServers]  WITH CHECK ADD  CONSTRAINT [FK_DominoServers_Servers] FOREIGN KEY([ServerID])
REFERENCES [dbo].[Servers] ([ID])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[DominoServers] CHECK CONSTRAINT [FK_DominoServers_Servers]
GO

/* 8/7/2014 NS added for VSPLUS-853 */
ALTER TABLE [dbo].[DominoServers]  WITH CHECK ADD  CONSTRAINT [FK_DominoServers_Credentials] FOREIGN KEY([CredentialsID])
REFERENCES [dbo].[Credentials] ([ID])
GO

ALTER TABLE [dbo].[DominoServers] CHECK CONSTRAINT [FK_DominoServers_Credentials]
GO
/****** Object:  ForeignKey [FK_DominoCustomStatValues_Servers]    Script Date: 09/23/2013 17:00:28 ******/
ALTER TABLE [dbo].[DominoCustomStatValues]  WITH CHECK ADD  CONSTRAINT [FK_DominoCustomStatValues_Servers] FOREIGN KEY([ServerID])
REFERENCES [dbo].[Servers] ([ID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[DominoCustomStatValues] CHECK CONSTRAINT [FK_DominoCustomStatValues_Servers]
GO
/****** Object:  ForeignKey [FK_AlertEvents_AlertNames]    Script Date: 09/23/2013 17:00:28 ******/
ALTER TABLE [dbo].[AlertEvents]  WITH CHECK ADD  CONSTRAINT [FK_AlertEvents_AlertNames] FOREIGN KEY([AlertKey])
REFERENCES [dbo].[AlertNames] ([AlertKey])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AlertEvents] CHECK CONSTRAINT [FK_AlertEvents_AlertNames]
GO
/****** Object:  ForeignKey [FK_AlertEvents_EventsMaster]    Script Date: 09/23/2013 17:00:28 ******/
/* 11/14/2013 NS commented out the FK constraint below */
/*
ALTER TABLE [dbo].[AlertEvents]  WITH CHECK ADD  CONSTRAINT [FK_AlertEvents_EventsMaster] FOREIGN KEY([EventID])
REFERENCES [dbo].[EventsMaster] ([ID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AlertEvents] CHECK CONSTRAINT [FK_AlertEvents_EventsMaster]
GO
*/
/****** Object:  ForeignKey [FK_AlertServers_AlertNames]    Script Date: 09/23/2013 17:00:28 ******/
ALTER TABLE [dbo].[AlertServers]  WITH CHECK ADD  CONSTRAINT [FK_AlertServers_AlertNames] FOREIGN KEY([AlertKey])
REFERENCES [dbo].[AlertNames] ([AlertKey])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AlertServers] CHECK CONSTRAINT [FK_AlertServers_AlertNames]
GO
/****** Object:  ForeignKey [FK_AlertServers_Servers]    Script Date: 09/23/2013 17:00:28 ******/
/* 11/14/2013 NS commented out the FK constraint below */
/*
ALTER TABLE [dbo].[AlertServers]  WITH CHECK ADD  CONSTRAINT [FK_AlertServers_Servers] FOREIGN KEY([ServerID])
REFERENCES [dbo].[Servers] ([ID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AlertServers] CHECK CONSTRAINT [FK_AlertServers_Servers]
GO
*/

/******Mukund:VSPlus-984: Object:  StoredProcedure [dbo].[ServerLocations]    Script Date: 10/11/2014 14:55:39 ******/
CREATE procedure [dbo].[ServerLocations] as
Begin
declare @SrvLocations Table
(id int, LocId int, Name varchar(100),actid int,tbl varchar(50),srvtypeid int,ServerType varchar(100),description varchar(100))

insert into @SrvLocations select id,null,Location,id,'Locations',null,null,null from Locations 

Declare @count int
select @count=MAX(id) from Locations


Declare @ID int
Declare @LocationName varchar(100)
Declare @LocationID int
Declare @ServerTypeId int
Declare @ServerType varchar(100)
Declare @description varchar(100)


DECLARE db_cursor CURSOR FOR  
select sr.ID,sr.ServerName,sr.LocationID,sr.ServerTypeId,srt.ServerType,sr.description from Servers sr,
ServerTypes srt,SelectedFeatures ft  where sr.ServerTypeId=srt.id and ft.FeatureId=srt.FeatureId
union
select sr.ID,sr.Name as ServerName,sr.LocationID,sr.ServerTypeId,srt.ServerType, sr.TheURL
from URLs sr,ServerTypes srt,SelectedFeatures ft   where sr.ServerTypeId=srt.id  and ft.FeatureId=srt.FeatureId
order by sr.LocationID,sr.ServerName

OPEN db_cursor   
FETCH NEXT FROM db_cursor into @ID,@LocationName,@LocationID,@ServerTypeId,@ServerType,@description

WHILE @@FETCH_STATUS = 0   
BEGIN   
	Set @count=@count+1
	insert into @SrvLocations values(@count,@LocationID,@LocationName,@id,'Servers',@ServerTypeId,@ServerType,@description)
	FETCH NEXT FROM db_cursor into @ID,@LocationName,@LocationID,@ServerTypeId,@ServerType,@description
END   

CLOSE db_cursor   
DEALLOCATE db_cursor

select * from @SrvLocations order by tbl,Name
end
GO

/* 10/22/2014 NS modified for VSPLUS-730 */
/* 5/13/2015 NS modified for VSPLUS-1736 */
--/******Mukund:VSPlus-984:  Object:  StoredProcedure [dbo].[ServerTypeEvents]    Script Date: 10/11/2014 10:21:29 ******/
CREATE procedure [dbo].[ServerTypeEvents] as
Begin
declare @SrvEvents Table
(id int, SrvId int, Name varchar(100),actid int,tbl varchar(50),AlertOnRepeat bit)

insert into @SrvEvents select st.id,null,st.ServerType,st.id,'ServerTypes',0 from ServerTypes st,SelectedFeatures ft 
 where ft.FeatureId=st.FeatureId
-- select id,null,ServerType,id,'ServerTypes' from ServerTypes
--select * from Features ft inner join ServerTypes st on ft.id=st.FeatureId inner join EventsMaster et on st.ID=et.ServerTypeID  
Declare @count int
select @count=MAX(id) from ServerTypes


Declare @ID int
Declare @EventName varchar(100)
Declare @ServerTypeID int
Declare @AlertOnRepeat bit

DECLARE db_cursor CURSOR FOR  
select em.ID,em.EventName,em.ServerTypeID,em.AlertOnRepeat from EventsMaster em,ServerTypes st,SelectedFeatures ft   
where ft.FeatureId=st.FeatureId and em.ServerTypeID =st.id
order by em.ServerTypeID,em.EventName
--select ID,EventName,ServerTypeID from EventsMaster order by ServerTypeID,EventName
 

OPEN db_cursor   
FETCH NEXT FROM db_cursor into @ID,@EventName,@ServerTypeID,@AlertOnRepeat

WHILE @@FETCH_STATUS = 0   
BEGIN   
	Set @count=@count+1
	insert into @SrvEvents values(@count,@ServerTypeID,@EventName,@id,'EventsMaster',@AlertOnRepeat)
	FETCH NEXT FROM db_cursor into @ID,@EventName,@ServerTypeID,@AlertOnRepeat
END   

CLOSE db_cursor   
DEALLOCATE db_cursor

select * from @SrvEvents --order by SrvId,Name
order by Name
end
GO
Create Procedure [dbo].[GetAlertsById](@AlertKey varchar(10)) as
Begin
--Declare @AlertKey varchar(10)
--set @AlertKey='76'

begin --EVENTS + SERVER TYPES
------------------------------
	begin --Events - All  & Server types - Selected
		Declare @strEvnAllSrvSel varchar(10)
		Declare @SqlEvnAllSrvSel varchar(max )
		set @SqlEvnAllSrvSel =''

		Select @strEvnAllSrvSel=COUNT(*) from AlertEvents where AlertKey=@AlertKey and eventid=0 and ServerTypeID<>0 and ServerTypeID not in
		(select ID from ServerTypes where servertype='URL'
		union select ID from ServerTypes where servertype='Network Device'
		union select ID from ServerTypes where servertype='Mail')
			if @strEvnAllSrvSel>=1
			begin
				set @SqlEvnAllSrvSel ='select distinct EventName from EventsMaster evn, AlertEvents aevn 
									where aevn.eventid=0 and aevn.ServerTypeID<>0  and aevn.AlertKey='+@AlertKey+'
									and evn.ServerTypeID=aevn.ServerTypeID and aevn.ServerTypeID not in
									(select ID from ServerTypes where servertype=''URL''
									union select ID from ServerTypes where servertype=''Network Device''
									union select ID from ServerTypes where servertype=''Mail'')'
			end
	end

	begin --Events - All  & URL types - Selected
		Declare @strEvnAllURLSel varchar(10)
		Declare @SqlEvnAllURLSel varchar(max )
		set @SqlEvnAllURLSel =''
		Select @strEvnAllURLSel=COUNT(*) from AlertEvents where AlertKey=@AlertKey and eventid=0 and ServerTypeID<>0 and ServerTypeID =
		(select ID from ServerTypes where servertype='URL')
		if @strEvnAllURLSel>=1
			begin						
				set @SqlEvnAllURLSel ='select distinct EventName from EventsMaster evn, AlertEvents aevn 
									where aevn.eventid=0 and aevn.ServerTypeID<>0   and aevn.AlertKey='+@AlertKey+'
									and evn.ServerTypeID=aevn.ServerTypeID and aevn.ServerTypeID =
		(select ID from ServerTypes where servertype=''URL'')'					
			end
	end

	begin --Events - All  & [Network Devices] types - Selected
		Declare @strEvnAllNtwSel varchar(10)
		Declare @SqlEvnAllNtwSel varchar(max )
		set @SqlEvnAllNtwSel =''
		Select @strEvnAllNtwSel=COUNT(*) from AlertEvents where AlertKey=@AlertKey and eventid=0 and ServerTypeID<>0 and ServerTypeID =
		(select ID from ServerTypes where servertype='Network Device')
		if @strEvnAllNtwSel>=1
			begin						
				set @SqlEvnAllNtwSel ='select distinct EventName from EventsMaster evn, AlertEvents aevn 
									where aevn.eventid=0 and aevn.ServerTypeID<>0   and aevn.AlertKey='+@AlertKey+'
									and evn.ServerTypeID=aevn.ServerTypeID and aevn.ServerTypeID =
		(select ID from ServerTypes where servertype=''Network Device'')'									
			end
	end

	begin --Events - All  & MailServices types - Selected
		Declare @strEvnAllMailSel varchar(10)
		Declare @SqlEvnAllMailSel varchar(max )
		set @SqlEvnAllMailSel =''
		Select @strEvnAllMailSel=COUNT(*) from AlertEvents where AlertKey=@AlertKey and eventid=0 and ServerTypeID<>0 and ServerTypeID =
		(select ID from ServerTypes where servertype='Mail')
		if @strEvnAllMailSel>=1
			begin						
				set @SqlEvnAllMailSel ='select distinct EventName from EventsMaster evn, AlertEvents aevn 
									where aevn.eventid=0 and aevn.ServerTypeID<>0   and aevn.AlertKey='+@AlertKey+'
									and evn.ServerTypeID=aevn.ServerTypeID and aevn.ServerTypeID =
		(select ID from ServerTypes where servertype=''Mail'')'						
			end
	end

	begin --Events - Selected  & Server types - Selected
		Declare @strEvnSelSrvSel varchar(10)
		Declare @SqlEvnSelSrvSel varchar(max )
		set @SqlEvnSelSrvSel =''
		Select @strEvnSelSrvSel=COUNT(*) from AlertEvents where AlertKey=@AlertKey and eventid<>0 and ServerTypeID<>0 and ServerTypeID not in
		(select ID from ServerTypes where servertype='URL'
		union select ID from ServerTypes where servertype='Network Device'
		union select ID from ServerTypes where servertype='Mail')
		if @strEvnSelSrvSel>=1
			begin
				set @SqlEvnSelSrvSel ='select distinct EventName from EventsMaster evn, AlertEvents aevn 
									where aevn.eventid<>0 and aevn.ServerTypeID<>0   and aevn.AlertKey='+@AlertKey+'
									and evn.ID=aevn.EventID and evn.ServerTypeID=aevn.ServerTypeID and aevn.ServerTypeID not in
									(select ID from ServerTypes where servertype=''URL''
									union select ID from ServerTypes where servertype=''Network Device''
									union select ID from ServerTypes where servertype=''Mail'')'
			end
	end

	begin --Events - Selected  & URL types - Selected
		Declare @strEvnSelURLSel varchar(10)
		Declare @SqlEvnSelURLSel varchar(max )
		set @SqlEvnSelURLSel =''
		Select @strEvnSelURLSel=COUNT(*) from AlertEvents where AlertKey=@AlertKey and eventid<>0 and ServerTypeID<>0 and ServerTypeID =
		(select ID from ServerTypes where servertype='URL')
		if @strEvnSelURLSel>=1
			begin						
				set @SqlEvnSelURLSel ='select distinct EventName from EventsMaster evn, AlertEvents aevn 
									where aevn.eventid<>0 and aevn.ServerTypeID<>0   and aevn.AlertKey='+@AlertKey+'
									and evn.ID=aevn.EventID and evn.ServerTypeID=aevn.ServerTypeID and aevn.ServerTypeID =
									(select ID from ServerTypes where servertype=''URL'')'								

			end
	end

	begin --Events - Selected  & [Network Devices] types - Selected
		Declare @strEvnSelNtwSel varchar(10)
		Declare @SqlEvnSelNtwSel varchar(max )
		set @SqlEvnSelNtwSel =''
		Select @strEvnSelNtwSel=COUNT(*) from AlertEvents where AlertKey=@AlertKey and eventid<>0 and ServerTypeID<>0 and ServerTypeID =
		(select ID from ServerTypes where servertype='Network Device')
		if @strEvnSelNtwSel>=1
			begin						
				set @SqlEvnSelNtwSel ='select distinct EventName from EventsMaster evn, AlertEvents aevn 
									where aevn.eventid<>0 and aevn.ServerTypeID<>0   and aevn.AlertKey='+@AlertKey+'
									and evn.ID=aevn.EventID and evn.ServerTypeID=aevn.ServerTypeID and aevn.ServerTypeID =
		(select ID from ServerTypes where servertype=''Network Device'')'							

			end
	end

	begin --Events - Selected  & MailServices types - Selected
		Declare @strEvnSelMailSel varchar(10)
		Declare @SqlEvnSelMailSel varchar(max )
		set @SqlEvnSelMailSel =''
		Select @strEvnSelMailSel=COUNT(*) from AlertEvents where AlertKey=@AlertKey and eventid<>0 and ServerTypeID<>0 and ServerTypeID =
		(select ID from ServerTypes where servertype='Mail')
		if @strEvnSelMailSel>=1
			begin						
				set @SqlEvnSelMailSel ='select distinct EventName from EventsMaster evn, AlertEvents aevn 
									where aevn.eventid<>0 and aevn.ServerTypeID<>0   and aevn.AlertKey='+@AlertKey+'
									and evn.ID=aevn.EventID and evn.ServerTypeID=aevn.ServerTypeID and aevn.ServerTypeID =
		(select ID from ServerTypes where servertype=''Mail'')'										

			end
	end

	begin --Events - All & Server types - All
		Declare @strEvnAllSrvAll varchar(10)
		Declare @SqlEvnAllSrvAll varchar(max)
		set @SqlEvnAllSrvAll =''
		Select @strEvnAllSrvAll=COUNT(*) from AlertEvents where AlertKey=@AlertKey and eventid=0 and ServerTypeID=0
		if @strEvnAllSrvAll>=1
			begin
				set @SqlEvnAllSrvAll ='select distinct EventName from EventsMaster'
			end
	end

end


begin--SERVERS + LOCATIONS
	------------------------------

	begin --Servers - All  & Locations -Selected
		Declare @strSrvAllLocSel varchar(10)
		Declare @SqlSrvAllLocSel varchar(max )
		Declare @SqlURLAllLocSel varchar(max )
		Declare @SqlNtwAllLocSel varchar(max )
		Declare @SqlMailAllLocSel varchar(max )
		set @SqlSrvAllLocSel =''
		set @SqlURLAllLocSel =''
		Select @strSrvAllLocSel=COUNT(*) from AlertServers where AlertKey=@AlertKey and ServerID=0 and LocationID<>0 

		if @strSrvAllLocSel>=1
			begin
				begin--Servers - All  & Locations -Selected
				set @SqlSrvAllLocSel ='select distinct ServerName from Servers srv , AlertServers asrv 
									where asrv.ServerID=0 and  asrv.LocationID<>0 and 
									srv.LocationID=asrv.LocationID and asrv.AlertKey='+@AlertKey +' 
									and srv.ServerTypeId not in(select ID from ServerTypes where servertype=''URL''
									union select ID from ServerTypes where servertype=''Network Device''
									union select ID from ServerTypes where servertype=''Mail'')'
			end
			
				begin--URLs - All  & Locations -Selected						
					set @SqlURLAllLocSel ='select distinct name as ServerName from URLs srv , AlertServers asrv 
										where asrv.ServerID=0 and  asrv.LocationID<>0 and 
										srv.LocationID=asrv.LocationID and asrv.AlertKey='+@AlertKey +' 
										and srv.ServerTypeId in(select ID from ServerTypes where servertype=''URL'')'
				end
				
				begin--[Network Devices] - All  & Locations -Selected						
					set @SqlNtwAllLocSel ='select distinct name as ServerName from [Network Devices] srv , AlertServers asrv 
										where asrv.ServerID=0 and  asrv.LocationID<>0 and 
										srv.LocationID=asrv.LocationID and asrv.AlertKey='+@AlertKey +' 
										and srv.ServerTypeId in(select ID from ServerTypes where servertype=''Network Device'')'
				end
				
				begin--MailServices - All  & Locations -Selected						
					set @SqlMailAllLocSel ='select distinct name as ServerName from MailServices srv , AlertServers asrv 
										where asrv.ServerID=0 and  asrv.LocationID<>0 and 
										srv.LocationID=asrv.LocationID and asrv.AlertKey='+@AlertKey +' 
										and srv.ServerTypeId in(select ID from ServerTypes where servertype=''Mail'')'
				end
			end
	end

	begin--Servers - Selected  & Locations -Selected
		Declare @strSrvSelLocSel varchar(10)
		Declare @SqlSrvSelLocSel varchar(max )
		Declare @SqlURLSelLocSel varchar(max )
		Declare @SqlNtwSelLocSel varchar(max )
		Declare @SqlMailSelLocSel varchar(max )
		set @SqlSrvSelLocSel =''
		set @SqlURLSelLocSel =''
		Select @strSrvSelLocSel=COUNT(*) from AlertServers where AlertKey=@AlertKey and ServerID<>0 and LocationID<>0
		if @strSrvSelLocSel>=1
			begin
				begin--Servers - Selected  & Locations -Selected
					set @SqlSrvSelLocSel ='select distinct ServerName from Servers srv, AlertServers asrv 
									where  asrv.ServerID<>0 and asrv.LocationID<>0 and
									srv.ID=asrv.ServerID and asrv.AlertKey='+@AlertKey+' 
									and srv.ServerTypeId not in(select ID from ServerTypes where servertype=''URL''
									union select ID from ServerTypes where servertype=''Network Device''
									union select ID from ServerTypes where servertype=''Mail'')'
				end
									
				begin--URLs - Selected  & Locations -Selected
				set @SqlURLSelLocSel ='select distinct name as ServerName from URLs srv, AlertServers asrv 
									where  asrv.ServerID<>0 and asrv.LocationID<>0 and
									srv.ID=asrv.ServerID and asrv.AlertKey='+@AlertKey+' 
									and srv.ServerTypeId in(select ID from ServerTypes where servertype=''URL'')'	
				end
				
				begin--[Network Devices] - Selected  & Locations -Selected
				set @SqlNtwSelLocSel ='select distinct name as ServerName from [Network Devices] srv, AlertServers asrv 
									where  asrv.ServerID<>0 and asrv.LocationID<>0 and
									srv.ID=asrv.ServerID and asrv.AlertKey='+@AlertKey+' 
									and srv.ServerTypeId in(select ID from ServerTypes where servertype=''Network Device'')'	
				end		
				
				begin--MailServices - Selected  & Locations -Selected
				set @SqlMailSelLocSel ='select distinct name as ServerName from MailServices srv, AlertServers asrv 
									where  asrv.ServerID<>0 and asrv.LocationID<>0 and
									srv.ServerID=asrv.ServerID and asrv.AlertKey='+@AlertKey+' 
									and srv.ServerTypeId in(select ID from ServerTypes where servertype=''Mail'')'	
				end							
			end
	end

	begin --Servers - All & Locations -All
	Declare @strSrvAllLocAll varchar(10)
	Declare @SqlSrvAllLocAll varchar(max )
	set @SqlSrvAllLocAll =''
	Select @strSrvAllLocAll=COUNT(*) from AlertServers where AlertKey=@AlertKey and ServerID=0 and LocationID=0
	if @strSrvAllLocAll>=1
		begin
			set @SqlSrvAllLocAll ='select distinct ServerName from Servers 
									union select distinct name as ServerName from URLs
									union select distinct name from [Network Devices]
									union select distinct name from MailServices' 
		end
end

end


begin-- Union of All Events & All Locations

	begin --Union of Server Events & Server Location sqls 

		begin --Union of Server Events sqls 

		Declare @SQLEvnSrv varchar(max)
		Declare @SqlEvnAllSrvAll_EvnSrv varchar(max)
		set @SQLEvnSrv=''

		select @SqlEvnAllSrvSel= case when @SqlEvnAllSrvSel<>'' then @SqlEvnAllSrvSel + ' union ' else '' end
		select @SqlEvnSelSrvSel= case when @SqlEvnSelSrvSel<>'' then @SqlEvnSelSrvSel + ' union ' else '' end
		select @SqlEvnAllSrvAll_EvnSrv= case when @SqlEvnAllSrvAll<>'' then @SqlEvnAllSrvAll + ' union ' else '' end

		set @SQLEvnSrv=@SqlEvnAllSrvSel+@SqlEvnSelSrvSel+@SqlEvnAllSrvAll_EvnSrv
		if(ltrim(rtrim(SUBSTRING(@SQLEvnSrv,LEN(@SQLEvnSrv)-4,5)))='union')
		begin
		set @SQLEvnSrv=SUBSTRING(@SQLEvnSrv,0,LEN(@SQLEvnSrv)-5)
		--select @SQLEvnSrv
		end
	end

		begin --Union of Server Location sqls 
		Declare @SqlLocSrv varchar(max )
		Declare @SqlSrvAllLocAll_SrvLoc varchar(max )
		set @SqlLocSrv =''

		select @SqlSrvAllLocSel= case when @SqlSrvAllLocSel<>'' then @SqlSrvAllLocSel + ' union ' else '' end
		select @SqlSrvSelLocSel= case when @SqlSrvSelLocSel<>'' then @SqlSrvSelLocSel + ' union ' else '' end
		select @SqlSrvAllLocAll_SrvLoc= case when @SqlSrvAllLocAll<>'' then @SqlSrvAllLocAll + ' union ' else '' end

		set @SqlLocSrv=@SqlSrvAllLocSel+@SqlSrvSelLocSel+@SqlSrvAllLocAll_SrvLoc
		--select @SqlLocSrv
		--select SUBSTRING(@SqlLocSrv,LEN(@SqlLocSrv)-4,5)
		if(ltrim(rtrim(SUBSTRING(@SqlLocSrv,LEN(@SqlLocSrv)-4,5)))='union')
			begin
			set @SqlLocSrv=SUBSTRING(@SqlLocSrv,0,LEN(@SqlLocSrv)-5)
			
			end
	end
	
Declare @ServerSqlMain varchar(max)
set @ServerSqlMain=''

	if(@SQLEvnSrv<>'' and @SqlLocSrv<>'')
	begin
	set @ServerSqlMain='select * from AlertHistory
						where datetimealertcleared is null and
						AlertType in ('+@SQLEvnSrv+')
						and
						DeviceName in ('+@SqlLocSrv+')
						and devicetype not in (''URL'',''Network Device'',''Mail'')'
	end
end

	begin --Union of URL Events & URL Location sqls
		begin--Union of URL Events sqls 
		Declare @SqlEvnURL varchar(max)
		Declare @SqlLocURL varchar(max )
		Declare @SqlEvnAllSrvAll_EvnURL varchar(max )
		set @SqlEvnURL =''
		set @SqlLocURL =''

		select @SqlEvnAllURLSel= case when @SqlEvnAllURLSel<>'' then @SqlEvnAllURLSel + ' union ' else '' end
		select @SqlEvnSelURLSel= case when @SqlEvnSelURLSel<>'' then @SqlEvnSelURLSel + ' union ' else '' end
		select @SqlEvnAllSrvAll_EvnURL= case when @SqlEvnAllSrvAll<>'' then @SqlEvnAllSrvAll + ' union ' else '' end

		set @SqlEvnURL=@SqlEvnAllURLSel+@SqlEvnSelURLSel+@SqlEvnAllSrvAll_EvnURL
		if(ltrim(rtrim(SUBSTRING(@SqlEvnURL,LEN(@SqlEvnURL)-4,5)))='union')
			begin
			set @SqlEvnURL=SUBSTRING(@SqlEvnURL,0,LEN(@SqlEvnURL)-5)
			
			end
	end

		begin --Union of URL Locations sqls 
		Declare @SqlSrvAllLocAll_UrlLoc varchar(max)
		select @SqlURLAllLocSel= case when @SqlURLAllLocSel<>'' then @SqlURLAllLocSel + ' union ' else '' end
		select @SqlURLSelLocSel= case when @SqlURLSelLocSel<>'' then @SqlURLSelLocSel + ' union ' else '' end
		select @SqlSrvAllLocAll_UrlLoc= case when @SqlSrvAllLocAll<>'' then @SqlSrvAllLocAll + ' union ' else '' end


		set @SqlLocURL=@SqlURLAllLocSel+@SqlURLSelLocSel+@SqlSrvAllLocAll_UrlLoc
		
		if(ltrim(rtrim(SUBSTRING(@SqlLocURL,LEN(@SqlLocURL)-4,5)))='union')
		begin
		set @SqlLocURL=SUBSTRING(@SqlLocURL,0,LEN(@SqlLocURL)-5)
		--select @SqlLocURL
		end
		declare @URLSqlMain varchar(max)
		if(@SqlEvnURL<>'' and @SqlLocURL<>'')
		begin
		set @URLSqlMain='select * from AlertHistory
							where datetimealertcleared is null and
							AlertType in ('+@SqlEvnURL+')
							and
							DeviceName in ('+@SqlLocURL+')
							and devicetype in (''URL'')'
		end
	end	
end

	begin --Union of [Network Devices] Events & [Network Devices] Location sqls
		begin--Union of [Network Devices] Events sqls 
		Declare @SqlEvnNtw varchar(max)
		Declare @SqlEvnAllSrvAll_EvnNtw varchar(max)
		Declare @SqlLocNtw varchar(max )
		set @SqlEvnNtw =''
		set @SqlLocNtw =''

		select @SqlEvnAllNtwSel= case when @SqlEvnAllNtwSel<>'' then @SqlEvnAllNtwSel + ' union ' else '' end
		select @SqlEvnSelNtwSel= case when @SqlEvnSelNtwSel<>'' then @SqlEvnSelNtwSel + ' union ' else '' end
		select @SqlEvnAllSrvAll_EvnNtw= case when @SqlEvnAllSrvAll<>'' then @SqlEvnAllSrvAll + ' union ' else '' end

		set @SqlEvnNtw=@SqlEvnAllNtwSel+@SqlEvnSelNtwSel+@SqlEvnAllSrvAll_EvnNtw
		if(ltrim(rtrim(SUBSTRING(@SqlEvnNtw,LEN(@SqlEvnNtw)-4,5)))='union')
			begin
			set @SqlEvnNtw=SUBSTRING(@SqlEvnNtw,0,LEN(@SqlEvnNtw)-5)
			end
	end

		begin --Union of [Network Devices] Locations sqls 
		declare  @SqlSrvAllLocAll_NtwLoc varchar(max)
		select @SqlNtwAllLocSel= case when @SqlNtwAllLocSel<>'' then @SqlNtwAllLocSel + ' union ' else '' end
		select @SqlNtwSelLocSel= case when @SqlNtwSelLocSel<>'' then @SqlNtwSelLocSel + ' union ' else '' end
		select @SqlSrvAllLocAll_NtwLoc= case when @SqlSrvAllLocAll<>'' then @SqlSrvAllLocAll + ' union ' else '' end


		set @SqlLocNtw=@SqlNtwAllLocSel+@SqlNtwSelLocSel+@SqlSrvAllLocAll_NtwLoc
		if(ltrim(rtrim(SUBSTRING(@SqlLocNtw,LEN(@SqlLocNtw)-4,5)))='union')
		begin
		set @SqlLocNtw=SUBSTRING(@SqlLocNtw,0,LEN(@SqlLocNtw)-5)
		
		end
		declare @NtwSqlMain varchar(max)
		if(@SqlEvnNtw<>'' and @SqlLocNtw<>'')
		begin
		set @NtwSqlMain='select * from AlertHistory
							where datetimealertcleared is null and
							AlertType in ('+@SqlEvnNtw+')
							and
							DeviceName in ('+@SqlLocNtw+')
							and devicetype in (''Network Device'')'
		end
	end	
end

	begin --Union of MailServices Events & MailServices Location sqls
		begin--Union of [Network Devices] Events sqls 
		Declare @SqlEvnMail varchar(max)
		Declare @SqlEvnAllSrvAll_EvnMail varchar(max)
		Declare @SqlLocMail varchar(max )
		set @SqlEvnMail =''
		set @SqlLocMail =''

		select @SqlEvnAllMailSel= case when @SqlEvnAllMailSel<>'' then @SqlEvnAllMailSel + ' union ' else '' end
		select @SqlEvnSelMailSel= case when @SqlEvnSelMailSel<>'' then @SqlEvnSelMailSel + ' union ' else '' end
		select @SqlEvnAllSrvAll_EvnMail= case when @SqlEvnAllSrvAll<>'' then @SqlEvnAllSrvAll + ' union ' else '' end

		set @SqlEvnMail=@SqlEvnAllMailSel+@SqlEvnSelMailSel+@SqlEvnAllSrvAll_EvnMail
		if(ltrim(rtrim(SUBSTRING(@SqlEvnMail,LEN(@SqlEvnMail)-4,5)))='union')
			begin
			set @SqlEvnMail=SUBSTRING(@SqlEvnMail,0,LEN(@SqlEvnMail)-5)
			end
	end

		begin --Union of MailServices Locations sqls 
		declare @SqlSrvAllLocAll_MailLoc varchar(max)
		select @SqlMailAllLocSel= case when @SqlMailAllLocSel<>'' then @SqlMailAllLocSel + ' union ' else '' end
		select @SqlMailSelLocSel= case when @SqlMailSelLocSel<>'' then @SqlMailSelLocSel + ' union ' else '' end
		select @SqlSrvAllLocAll_MailLoc= case when @SqlSrvAllLocAll<>'' then @SqlSrvAllLocAll + ' union ' else '' end


		set @SqlLocMail=@SqlMailAllLocSel+@SqlMailSelLocSel+@SqlSrvAllLocAll_MailLoc
		if(ltrim(rtrim(SUBSTRING(@SqlLocMail,LEN(@SqlLocMail)-4,5)))='union')
		begin
		set @SqlLocMail=SUBSTRING(@SqlLocMail,0,LEN(@SqlLocMail)-5)
		
		end
		declare @MailSqlMain varchar(max)
		if(@SqlEvnMail<>'' and @SqlLocMail<>'')
		begin
		set @MailSqlMain='select * from AlertHistory
							where datetimealertcleared is null and
							AlertType in ('+@SqlEvnMail+')
							and
							DeviceName in ('+@SqlLocMail+')
							and devicetype in (''Mail'')'
		end
	end	
end

end

begin --Union of all records of Alert History sqls
-----------------------------------------------
	Declare @FullSQL varchar(max)

		select @ServerSqlMain= case when @ServerSqlMain<>'' then @ServerSqlMain + ' union ' else '' end
		select @URLSqlMain= case when @URLSqlMain<>'' then @URLSqlMain + ' union ' else '' end
		select @NtwSqlMain= case when @NtwSqlMain<>'' then @NtwSqlMain + ' union ' else '' end
		select @MailSqlMain= case when @MailSqlMain<>'' then @MailSqlMain + ' union ' else '' end

		set @FullSQL=@ServerSqlMain+@URLSqlMain+@NtwSqlMain+@MailSqlMain
		if(ltrim(rtrim(SUBSTRING(@FullSQL,LEN(@FullSQL)-4,5)))='union')
			begin
			set @FullSQL=SUBSTRING(@FullSQL,0,LEN(@FullSQL)-5)
			end

	select  @FullSQL=isnull(@FullSQL,'select * from AlertHistory where 1=2')
	exec(@FullSQL)
	--select  @FullSQL
end

End





GO

CREATE Procedure [dbo].[GetClearedAlertsById] as
Begin

Declare @SqlMain varchar(max)
set @SqlMain=''

set @SqlMain='select ah.*,asd.Id as sentid, sentTo from AlertSentDetails asd, AlertHistory ah where asd.alerthistoryid=ah.ID and
asd.AlertClearedDateTime is null and ah.DateTimeAlertCleared is not null'

--select @SqlMain
exec(@SqlMain)		
	
End


GO

/* 5/6/2014 NS modified for VSPLUS-602, ThresholdType - new column */
CREATE TABLE [dbo].[DominoDiskSettings](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[ServerName] [nvarchar](250) NULL,
	[DiskName] [nvarchar](250) NULL,
	[Threshold] [float] NULL,
	[ThresholdType] [varchar](10) NULL,
	[DiskInfo] [nvarchar](MAX) NULL	
) ON [PRIMARY]

GO

-- =============================================
-- Author:		Natallya Shkarayeva	
-- Create date: 12/3/2013 
-- Description:	[GetAlertHistory]
--
-- This stored procedure returns all records from the AlertHistory table
-- where DateTimeAlertCleared value is Null which means the alert is active.
--
-- Modified date: 4/10/2014 for VSPLUS-519
-- Description: 
-- Added DateTimeOfAlert in order to keep track of the original time the alert was entered to stop 
-- alerting if persistent alert duration is set to a number of hours.
-- Modified Date: 06/20/2014
-- Description:
-- controlling the number of records to be sent when the user has configured TotalMaximumAlertPerDay
-- =============================================
CREATE PROCEDURE [dbo].[GetAlertHistory]
	
AS
BEGIN
declare
@sSQL varchar(2000),
@nMaxAlertsPerDay int,
@nCurrentAlertsToday int,
@nCurrentAlertsCleared int,
@nAlertsToSend int
	-- SET NOCOUNT ON added to prevent extra result sets from
	
	SET NOCOUNT ON;
	/*
	get max alerts configured from settings table
	check how many alerts are sent today from table AlertSentDetails
	check what the difference is between them, if the limit has reached or exceeded, then do not send any rows and set the "AlerThReached" flag to True for all the records
	if the limit has not reached then send only the remaining records 
	*/
	CREATE TABLE #TempTable (
	ID 		varchar(100),
	DeviceName			nvarchar(100),
	DeviceType			nvarchar(100),
	AlertType			nvarchar(510),
	Location			nvarchar(500),
	Details				nvarchar(1000),
	DateTimeOfAlert		datetime
)
--one for cleareddate is not null *2
--one for createddatetime and cleared is null
	SELECT @nMaxAlertsPerDay=SVALUE FROM Settings WHERE sname='TotalMaximumAlertPerDay'
	if @nMaxAlertsPerDay is null
		set @nMaxAlertsPerDay=0
	-- allbets off if the setting is missing or set to 0
	if @nMaxAlertsPerDay > 0
		begin
		SELECT @nCurrentAlertsToday =COUNT(*) from AlertSentDetails where DATEPART(d,AlertCreatedDateTime)=DATEPART(d,GETDATE()) AND AlertCreatedDateTime IS NOT NULL AND AlertClearedDateTime IS NULL
		SELECT @nCurrentAlertsCleared =COUNT(*) from AlertSentDetails where DATEPART(d,AlertClearedDateTime)=DATEPART(d,GETDATE()) AND AlertClearedDateTime IS NOT NULL
		set @nCurrentAlertsCleared = @nCurrentAlertsCleared *2
		set @nCurrentAlertsToday=@nCurrentAlertsToday + @nCurrentAlertsCleared
		if @nCurrentAlertsToday>= @nMaxAlertsPerDay
			set @nAlertsToSend=0
		else
			set @nAlertsToSend= @nMaxAlertsPerDay-@nCurrentAlertsToday
		
		set @sSQL = ' INSERT INTO #TempTable(ID, DeviceName, DeviceType, AlertType, Location, Details, DateTimeOfAlert) SELECT top ' + convert(varchar,@nAlertsToSend) + ' ID, DeviceName,
		 DeviceType, AlertType, Location, Details, DateTimeOfAlert    FROM AlertHistory WHERE DateTimeAlertCleared IS NULL 	ORDER BY ID ASC'
		 --AND (AlertThresholdReached IS NULL OR AlertThresholdReached=0)
		EXECUTE(@sSQL)
		end
	Else
		begin
		set @sSQL = ' INSERT INTO #TempTable(ID, DeviceName, DeviceType, AlertType, Location, Details, DateTimeOfAlert) SELECT ID, DeviceName,
		 DeviceType, AlertType, Location, Details, DateTimeOfAlert    FROM AlertHistory WHERE DateTimeAlertCleared IS NULL 	ORDER BY ID ASC'
		 EXECUTE(@sSQL)
		END
SELECT * FROM #TempTable
END



GO

-- =============================================
-- Author:		Natallya Shkarayeva
-- Create date: 12/3/2013
-- Description:	
--
-- This stored procedure gets event/server information based on the user selections
-- on the Alert Definition page. The query returns selections that cover all servers
-- for a specific location category, i.e., 
-- all Phoenix events have been selected from the servers grid and some Domino events have been selected
-- from the events grid, i.e., 'Not Responding' and 'Slow'.
-- The stored procedure will also cover the case where ALL event types and ALL locations have been selected.
--
-- Modified date: 4/7/2014 for VSPLUS-519
-- Description: 
-- Added a new key to the select clause to identify the alerts with persistent alerting enabled.
-- =============================================
CREATE PROCEDURE [dbo].[GetAlertsForAllServersByLocation]
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    select distinct AlertKey,HoursIndicator,SendTo,CopyTo,BlindCopyTo,ISNULL(StartTime,'') StartTime,
    Day,Duration,SendSNMPTrap,AlertName,EventName,ServerType,ServerName,
    EnablePersistentAlert from
	(
		-- Select rows from the URL table, include those that have location URL
		select distinct t1.AlertKey AlertKey,t1.HoursIndicator HoursIndicator,t1.SendTo SendTo,t1.CopyTo CopyTo,
		t1.BlindCopyTo BlindCopyTo,t1.StartTime StartTime,t1.DAY Day,t1.Duration Duration,
		t1.SendSNMPTrap SendSNMPTrap,
		t2.AlertName AlertName,t8.EventName EventName,t3.ServerType ServerType,t5.Name ServerName,
		t1.EnablePersistentAlert EnablePersistentAlert
		from AlertDetails t1 inner join AlertNames t2 on t1.AlertKey=t2.AlertKey
		inner join AlertEvents t7 on t1.AlertKey = t7.AlertKey 
		inner join EventsMaster t8 on t8.ServerTypeID = t7.ServerTypeID and t8.ID=t7.EventID
		inner join ServerTypes t3 on t7.ServerTypeID = t3.ID
		inner join AlertServers t4 on t2.AlertKey=t4.AlertKey
		inner join URLs t5 on t8.ServerTypeID=t5.ServerTypeID
		inner join Locations t6 on t4.LocationID=t6.ID and t6.Location ='URL'
		where t4.ServerID = 0
		union
		-- 3/5/2014 NS added
		-- ALL Locations, multiple selections from a single event category (Servers)
		select distinct t1.AlertKey AlertKey,t1.HoursIndicator HoursIndicator,t1.SendTo SendTo,t1.CopyTo CopyTo,
		t1.BlindCopyTo BlindCopyTo,t1.StartTime StartTime,t1.DAY Day,t1.Duration Duration,
		t1.SendSNMPTrap SendSNMPTrap,
		t2.AlertName AlertName,t8.EventName EventName,t3.ServerType ServerType,t5.ServerName ServerName,
		t1.EnablePersistentAlert EnablePersistentAlert
		from AlertDetails t1 inner join AlertNames t2 on t1.AlertKey=t2.AlertKey
		inner join AlertEvents t7 on t1.AlertKey = t7.AlertKey 
		inner join AlertServers t4 on t2.AlertKey=t4.AlertKey
		inner join EventsMaster t8 on t7.EventID=t8.ID, Servers t5, ServerTypes t3
		where t4.ServerID=0 and t4.ServerTypeID=0 and t4.LocationID=0 and
		t8.ServerTypeID=t5.ServerTypeID and t3.ID=t5.ServerTypeID
		-- 3/5/2014 NS added
		-- ALL Locations, multiple selections from a single event category (URLs)
		union
		select distinct t1.AlertKey AlertKey,t1.HoursIndicator HoursIndicator,t1.SendTo SendTo,t1.CopyTo CopyTo,
		t1.BlindCopyTo BlindCopyTo,t1.StartTime StartTime,t1.DAY Day,t1.Duration Duration,
		t1.SendSNMPTrap SendSNMPTrap,
		t2.AlertName AlertName,t8.EventName EventName,t3.ServerType ServerType,t5.Name ServerName,
		t1.EnablePersistentAlert EnablePersistentAlert
		from AlertDetails t1 inner join AlertNames t2 on t1.AlertKey=t2.AlertKey
		inner join AlertEvents t7 on t1.AlertKey = t7.AlertKey 
		inner join AlertServers t4 on t2.AlertKey=t4.AlertKey
		inner join EventsMaster t8 on t7.EventID=t8.ID, URLs t5, ServerTypes t3
		where t4.ServerID=0 and t4.ServerTypeID=0 and t4.LocationID=0 and
		t8.ServerTypeID=t5.ServerTypeID and t3.ID=t5.ServerTypeID
		-- 3/5/2014 NS added
		-- ALL Locations, multiple Event categories (Servers)
		union
		select distinct t1.AlertKey AlertKey,t1.HoursIndicator HoursIndicator,t1.SendTo SendTo,t1.CopyTo CopyTo,
		t1.BlindCopyTo BlindCopyTo,t1.StartTime StartTime,t1.DAY Day,t1.Duration Duration,
		t1.SendSNMPTrap SendSNMPTrap,
		t2.AlertName AlertName,t8.EventName EventName,t3.ServerType ServerType,t5.ServerName ServerName,
		t1.EnablePersistentAlert EnablePersistentAlert
		from AlertDetails t1 inner join AlertNames t2 on t1.AlertKey=t2.AlertKey
		inner join AlertEvents t7 on t1.AlertKey = t7.AlertKey 
		inner join AlertServers t4 on t2.AlertKey=t4.AlertKey
		inner join EventsMaster t8 on t8.ServerTypeID=t7.ServerTypeID
		inner join Servers t5 on t5.ServerTypeID=t8.ServerTypeID
		inner join ServerTypes t3 on t3.ID=t5.ServerTypeID
		where t4.ServerID=0 and t7.EventID = 0 and t4.ServerTypeID=0 and t4.LocationID=0
		-- 3/5/2014 NS added
		-- ALL Locations, multiple Event categories (URLs)
		union
		select distinct t1.AlertKey AlertKey,t1.HoursIndicator HoursIndicator,t1.SendTo SendTo,t1.CopyTo CopyTo,
		t1.BlindCopyTo BlindCopyTo,t1.StartTime StartTime,t1.DAY Day,t1.Duration Duration,
		t1.SendSNMPTrap SendSNMPTrap,
		t2.AlertName AlertName,t8.EventName EventName,t3.ServerType ServerType,t5.Name ServerName,
		t1.EnablePersistentAlert EnablePersistentAlert
		from AlertDetails t1 inner join AlertNames t2 on t1.AlertKey=t2.AlertKey
		inner join AlertEvents t7 on t1.AlertKey = t7.AlertKey 
		inner join AlertServers t4 on t2.AlertKey=t4.AlertKey
		inner join EventsMaster t8 on t8.ServerTypeID=t7.ServerTypeID
		inner join URLs t5 on t5.ServerTypeID=t8.ServerTypeID
		inner join ServerTypes t3 on t3.ID=t5.ServerTypeID
		where t4.ServerID=0 and t7.EventID = 0 and t4.ServerTypeID=0 and t4.LocationID=0
		-- 3/5/2014 NS added
		-- Multiple Location categories, multiple Event categories (Servers)
		union
		select distinct t1.AlertKey AlertKey,t1.HoursIndicator HoursIndicator,t1.SendTo SendTo,t1.CopyTo CopyTo,
		t1.BlindCopyTo BlindCopyTo,t1.StartTime StartTime,t1.DAY Day,t1.Duration Duration,
		t1.SendSNMPTrap SendSNMPTrap,
		t2.AlertName AlertName,t8.EventName EventName,t3.ServerType ServerType,t5.ServerName ServerName,
		t1.EnablePersistentAlert EnablePersistentAlert
		from AlertDetails t1 inner join AlertNames t2 on t1.AlertKey=t2.AlertKey
		inner join AlertEvents t7 on t1.AlertKey = t7.AlertKey 
		inner join AlertServers t4 on t2.AlertKey=t4.AlertKey
		inner join EventsMaster t8 on t8.ServerTypeID=t7.ServerTypeID
		inner join Servers t5 on t5.ServerTypeID=t8.ServerTypeID
		inner join ServerTypes t3 on t3.ID=t5.ServerTypeID
		where t4.ServerID=0 and t7.EventID = 0 and t4.ServerTypeID=0
		and t5.LocationID=t4.LocationID
		-- 3/5/2014 NS added
		-- Multiple Location categories, multiple Event categories (URLs)
		union
		select distinct t1.AlertKey AlertKey,t1.HoursIndicator HoursIndicator,t1.SendTo SendTo,t1.CopyTo CopyTo,
		t1.BlindCopyTo BlindCopyTo,t1.StartTime StartTime,t1.DAY Day,t1.Duration Duration,
		t1.SendSNMPTrap SendSNMPTrap,
		t2.AlertName AlertName,t8.EventName EventName,t3.ServerType ServerType,t5.Name ServerName,
		t1.EnablePersistentAlert EnablePersistentAlert
		from AlertDetails t1 inner join AlertNames t2 on t1.AlertKey=t2.AlertKey
		inner join AlertEvents t7 on t1.AlertKey = t7.AlertKey 
		inner join AlertServers t4 on t2.AlertKey=t4.AlertKey
		inner join EventsMaster t8 on t8.ServerTypeID=t7.ServerTypeID
		inner join URLs t5 on t5.ServerTypeID=t8.ServerTypeID
		inner join ServerTypes t3 on t3.ID=t5.ServerTypeID
		where t4.ServerID=0 and t7.EventID = 0 and t4.ServerTypeID=0 
		and t4.LocationID=t5.LocationId
		-- 3/5/2014 NS added
		-- Multiple Location categories, single Event category multiple values (Servers)
		union
		select distinct t1.AlertKey AlertKey,t1.HoursIndicator HoursIndicator,t1.SendTo SendTo,t1.CopyTo CopyTo,
		t1.BlindCopyTo BlindCopyTo,t1.StartTime StartTime,t1.DAY Day,t1.Duration Duration,
		t1.SendSNMPTrap SendSNMPTrap,
		t2.AlertName AlertName,t8.EventName EventName,t3.ServerType ServerType,t5.ServerName ServerName,
		t1.EnablePersistentAlert EnablePersistentAlert
		from AlertDetails t1 inner join AlertNames t2 on t1.AlertKey=t2.AlertKey
		inner join AlertEvents t7 on t1.AlertKey = t7.AlertKey 
		inner join AlertServers t4 on t2.AlertKey=t4.AlertKey
		inner join EventsMaster t8 on t8.ServerTypeID=t7.ServerTypeID and t8.ID=t7.EventID
		inner join Servers t5 on t5.ServerTypeID=t8.ServerTypeID and t4.LocationID=t5.LocationID
		inner join ServerTypes t3 on t3.ID=t5.ServerTypeID
		where t4.ServerID=0 and t4.ServerTypeID=0
		and t5.LocationID=t4.LocationID
		-- 3/5/2014 NS added
		-- Multiple Location categories, single Event category multiple values (URLs)
		union
		select distinct t1.AlertKey AlertKey,t1.HoursIndicator HoursIndicator,t1.SendTo SendTo,t1.CopyTo CopyTo,
		t1.BlindCopyTo BlindCopyTo,t1.StartTime StartTime,t1.DAY Day,t1.Duration Duration,
		t1.SendSNMPTrap SendSNMPTrap,
		t2.AlertName AlertName,t8.EventName EventName,t3.ServerType ServerType,t5.Name ServerName,
		t1.EnablePersistentAlert EnablePersistentAlert
		from AlertDetails t1 inner join AlertNames t2 on t1.AlertKey=t2.AlertKey
		inner join AlertEvents t7 on t1.AlertKey = t7.AlertKey 
		inner join AlertServers t4 on t2.AlertKey=t4.AlertKey
		inner join EventsMaster t8 on t8.ServerTypeID=t7.ServerTypeID and t8.ID=t7.EventID
		inner join URLs t5 on t5.ServerTypeID=t8.ServerTypeID and t4.LocationID=t5.LocationID
		inner join ServerTypes t3 on t3.ID=t5.ServerTypeID
		where t4.ServerID=0 and t4.ServerTypeID=0
		and t5.LocationID=t4.LocationID
	) as tmp
	order by AlertKey
END
GO
-- =============================================
-- Author:		Natallya Shkarayeva
-- Create date: 12/3/2013
-- Description:	[GetAlertsForSelectedEventsServers]
--
-- This stored procedure gets event/server information based on the user selections
-- on the Alert Definition page. The query returns only the individual selections, i.e., if a Domino
-- event 'Not Responding' is selected from the list of events and 'azphxweb2/RPRWyatt' is selected
-- from the list of servers. The selections from the event types and servers may be multiple but not all
-- for a specific event type or server location.
-- If there are selections that cover all event types for a specific category, i.e., 
-- all Domino events have been selected, those results are returned by the stored procedure 
-- GetAlertsWithAllEventsSelected. 
-- If there are selections that cover all servers for a specific location, i.e., 
-- all Phoenix servers have been selected, those results are returned by the stored procedure
-- GetAlertsForAllServersByLocation.
--
-- Modified date: 4/7/2014 for VSPLUS-519
-- Modified date: 12/18/2014 for VSPLUS-1238
-- Description: 
-- Added a new key to the select clause to identify the alerts with persistent alerting enabled.
-- =============================================
CREATE PROCEDURE [dbo].[GetAlertsForSelectedEventsServers]
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    select distinct AlertKey,HoursIndicator,SendTo,CopyTo,BlindCopyTo,ISNULL(StartTime,'') StartTime,
   Day,Duration,SendSNMPTrap,AlertName,EventName,ServerType,ServerName,
    EnablePersistentAlert,SMSTo,ScriptName,ScriptCommand,ScriptLocation
    from
	(
/* Case 1: ALL servers/locations, one or more full or partial Event Categories */
		select distinct t1.AlertKey AlertKey,t1.HoursIndicator HoursIndicator,t1.SendTo SendTo,t1.CopyTo CopyTo,
		t1.BlindCopyTo BlindCopyTo,t1.StartTime StartTime,t1.DAY Day,t1.Duration Duration,
		t1.SendSNMPTrap SendSNMPTrap,
		t2.AlertName AlertName,t8.EventName EventName,t3.ServerType ServerType,t4.Name ServerName,
		t1.EnablePersistentAlert EnablePersistentAlert,ISNULL(t1.SMSTo,'') SMSTo,ISNULL(t7.ScriptName,'') ScriptName,
		ISNULL(t7.ScriptCommand,'') ScriptCommand,ISNULL(t7.ScriptLocation,'') ScriptLocation
		from AlertDetails t1 inner join AlertNames t2 on t1.AlertKey=t2.AlertKey
		left outer join CustomScripts t7 on t1.ScriptID=t7.ID
		inner join AlertEvents t6 on t1.AlertKey=t6.AlertKey
		inner join AlertServers t5 on t1.AlertKey=t5.AlertKey
		inner join DeviceInventory t4 on t6.ServerTypeID=t4.DeviceTypeID
		inner join ServerTypes t3 on t4.DeviceTypeID=t3.ID
		inner join EventsMaster t8 on t6.EventID=t8.ID 
		where t5.ServerTypeID=0 and t5.ServerID=0 and t5.LocationID=0
		union
		select distinct t1.AlertKey AlertKey,t1.HoursIndicator HoursIndicator,t1.SendTo SendTo,t1.CopyTo CopyTo,
		t1.BlindCopyTo BlindCopyTo,t1.StartTime StartTime,t1.DAY Day,t1.Duration Duration,
		t1.SendSNMPTrap SendSNMPTrap,
		t2.AlertName AlertName,t8.EventName EventName,t3.ServerType ServerType,t4.Name ServerName,
		t1.EnablePersistentAlert EnablePersistentAlert,ISNULL(t1.SMSTo,'') SMSTo,ISNULL(t7.ScriptName,'') ScriptName,
		ISNULL(t7.ScriptCommand,'') ScriptCommand,ISNULL(t7.ScriptLocation,'') ScriptLocation
		from AlertDetails t1 inner join AlertNames t2 on t1.AlertKey=t2.AlertKey
		left outer join CustomScripts t7 on t1.ScriptID=t7.ID
		inner join AlertEvents t6 on t1.AlertKey=t6.AlertKey
		inner join AlertServers t5 on t1.AlertKey=t5.AlertKey
		inner join DeviceInventory t4 on t6.ServerTypeID=t4.DeviceTypeID
		inner join ServerTypes t3 on t4.DeviceTypeID=t3.ID
		inner join EventsMaster t8 on t8.ServerTypeID=t6.ServerTypeID
		where t5.ServerTypeID=0 and t5.ServerID=0 and t5.LocationID=0 and t6.EventID=0
		union
/* Case 2: ALL servers/locations, ALL Event Categories */
		select distinct t1.AlertKey AlertKey,t1.HoursIndicator HoursIndicator,t1.SendTo SendTo,t1.CopyTo CopyTo,
		t1.BlindCopyTo BlindCopyTo,t1.StartTime StartTime,t1.DAY Day,t1.Duration Duration,
		t1.SendSNMPTrap SendSNMPTrap,
		t2.AlertName AlertName,t8.EventName EventName,t3.ServerType ServerType,t4.Name ServerName,
		t1.EnablePersistentAlert EnablePersistentAlert,ISNULL(t1.SMSTo,'') SMSTo,ISNULL(t7.ScriptName,'') ScriptName,
		ISNULL(t7.ScriptCommand,'') ScriptCommand,ISNULL(t7.ScriptLocation,'') ScriptLocation
		from AlertDetails t1 inner join AlertNames t2 on t1.AlertKey=t2.AlertKey
		left outer join CustomScripts t7 on t1.ScriptID=t7.ID
		inner join AlertEvents t6 on t1.AlertKey=t6.AlertKey
		inner join AlertServers t5 on t1.AlertKey=t5.AlertKey,
		DeviceInventory t4 
		inner join ServerTypes t3 on t4.DeviceTypeID=t3.ID
		inner join EventsMaster t8 on t4.DeviceTypeID=t8.ServerTypeID
		where t6.EventID=0 and t6.ServerTypeID=0 and t5.ServerTypeID=0 and t5.ServerID=0 and t5.LocationID=0
		union
/* Case 3: One or more full servers/locations, ALL Event Categories */
		select distinct t1.AlertKey AlertKey,t1.HoursIndicator HoursIndicator,t1.SendTo SendTo,t1.CopyTo CopyTo,
		t1.BlindCopyTo BlindCopyTo,t1.StartTime StartTime,t1.DAY Day,t1.Duration Duration,
		t1.SendSNMPTrap SendSNMPTrap,
		t2.AlertName AlertName,t8.EventName EventName,t3.ServerType ServerType,t4.Name ServerName,
		t1.EnablePersistentAlert EnablePersistentAlert,ISNULL(t1.SMSTo,'') SMSTo,ISNULL(t7.ScriptName,'') ScriptName,
		ISNULL(t7.ScriptCommand,'') ScriptCommand,ISNULL(t7.ScriptLocation,'') ScriptLocation
		from AlertDetails t1 inner join AlertNames t2 on t1.AlertKey=t2.AlertKey
		left outer join CustomScripts t7 on t1.ScriptID=t7.ID
		inner join AlertEvents t6 on t1.AlertKey=t6.AlertKey
		inner join AlertServers t5 on t1.AlertKey=t5.AlertKey
		inner join Locations t9 on t5.LocationID=t9.ID
		inner join DeviceInventory t4 on t9.ID=t4.LocationID
		inner join ServerTypes t3 on t4.DeviceTypeID=t3.ID
		inner join EventsMaster t8 on t4.DeviceTypeID=t8.ServerTypeID
		where t6.EventID=0 and t6.ServerTypeID=0 and t5.ServerTypeID=0
		union
		select distinct t1.AlertKey AlertKey,t1.HoursIndicator HoursIndicator,t1.SendTo SendTo,t1.CopyTo CopyTo,
		t1.BlindCopyTo BlindCopyTo,t1.StartTime StartTime,t1.DAY Day,t1.Duration Duration,
		t1.SendSNMPTrap SendSNMPTrap,
		t2.AlertName AlertName,t8.EventName EventName,t3.ServerType ServerType,t4.Name ServerName,
		t1.EnablePersistentAlert EnablePersistentAlert,ISNULL(t1.SMSTo,'') SMSTo,ISNULL(t7.ScriptName,'') ScriptName,
		ISNULL(t7.ScriptCommand,'') ScriptCommand,ISNULL(t7.ScriptLocation,'') ScriptLocation
		from AlertDetails t1 inner join AlertNames t2 on t1.AlertKey=t2.AlertKey
		left outer join CustomScripts t7 on t1.ScriptID=t7.ID
		inner join AlertEvents t6 on t1.AlertKey=t6.AlertKey
		inner join AlertServers t5 on t1.AlertKey=t5.AlertKey
		inner join DeviceInventory t4 on t4.LocationID IS NULL
		inner join ServerTypes t3 on t4.DeviceTypeID=t3.ID
		inner join EventsMaster t8 on t4.DeviceTypeID=t8.ServerTypeID
		where t6.EventID=0 and t6.ServerTypeID=0 and t5.ServerTypeID=0
		union
/* Case 4: One or more full servers/locations, one or more full Event Categories */
		select distinct t1.AlertKey AlertKey,t1.HoursIndicator HoursIndicator,t1.SendTo SendTo,t1.CopyTo CopyTo,
		t1.BlindCopyTo BlindCopyTo,t1.StartTime StartTime,t1.DAY Day,t1.Duration Duration,
		t1.SendSNMPTrap SendSNMPTrap,
		t2.AlertName AlertName,t8.EventName EventName,t3.ServerType ServerType,t4.Name ServerName,
		t1.EnablePersistentAlert EnablePersistentAlert,ISNULL(t1.SMSTo,'') SMSTo,ISNULL(t7.ScriptName,'') ScriptName,
		ISNULL(t7.ScriptCommand,'') ScriptCommand,ISNULL(t7.ScriptLocation,'') ScriptLocation
		from AlertDetails t1 inner join AlertNames t2 on t1.AlertKey=t2.AlertKey
		left outer join CustomScripts t7 on t1.ScriptID=t7.ID
		inner join AlertEvents t6 on t1.AlertKey=t6.AlertKey
		inner join AlertServers t5 on t1.AlertKey=t5.AlertKey
		inner join Locations t9 on t5.LocationID=t9.ID
		inner join DeviceInventory t4 on t9.ID=t4.LocationID
		inner join ServerTypes t3 on t4.DeviceTypeID=t3.ID
		inner join EventsMaster t8 on t4.DeviceTypeID=t8.ServerTypeID
		where t6.ServerTypeID=t4.DeviceTypeID and t6.EventID=0 and t5.ServerTypeID=0
		union
		select distinct t1.AlertKey AlertKey,t1.HoursIndicator HoursIndicator,t1.SendTo SendTo,t1.CopyTo CopyTo,
		t1.BlindCopyTo BlindCopyTo,t1.StartTime StartTime,t1.DAY Day,t1.Duration Duration,
		t1.SendSNMPTrap SendSNMPTrap,
		t2.AlertName AlertName,t8.EventName EventName,t3.ServerType ServerType,t4.Name ServerName,
		t1.EnablePersistentAlert EnablePersistentAlert,ISNULL(t1.SMSTo,'') SMSTo,ISNULL(t7.ScriptName,'') ScriptName,
		ISNULL(t7.ScriptCommand,'') ScriptCommand,ISNULL(t7.ScriptLocation,'') ScriptLocation
		from AlertDetails t1 inner join AlertNames t2 on t1.AlertKey=t2.AlertKey
		left outer join CustomScripts t7 on t1.ScriptID=t7.ID
		inner join AlertEvents t6 on t1.AlertKey=t6.AlertKey
		inner join AlertServers t5 on t1.AlertKey=t5.AlertKey
		inner join DeviceInventory t4 on t4.LocationID IS NULL
		inner join ServerTypes t3 on t4.DeviceTypeID=t3.ID
		inner join EventsMaster t8 on t4.DeviceTypeID=t8.ServerTypeID 
		where t6.ServerTypeID=t4.DeviceTypeID and t6.EventID=0 and t5.ServerTypeID=0
		union
/* Case 5: One or more partial servers/locations, one or more partial Event Categories */		
		select distinct t1.AlertKey AlertKey,t1.HoursIndicator HoursIndicator,t1.SendTo SendTo,t1.CopyTo CopyTo,
		t1.BlindCopyTo BlindCopyTo,t1.StartTime StartTime,t1.DAY Day,t1.Duration Duration,
		t1.SendSNMPTrap SendSNMPTrap,
		t2.AlertName AlertName,t8.EventName EventName,t3.ServerType ServerType,t4.Name ServerName,
		t1.EnablePersistentAlert EnablePersistentAlert,ISNULL(t1.SMSTo,'') SMSTo,ISNULL(t7.ScriptName,'') ScriptName,
		ISNULL(t7.ScriptCommand,'') ScriptCommand,ISNULL(t7.ScriptLocation,'') ScriptLocation
		from AlertDetails t1 inner join AlertNames t2 on t1.AlertKey=t2.AlertKey
		left outer join CustomScripts t7 on t1.ScriptID=t7.ID
		inner join AlertEvents t6 on t1.AlertKey=t6.AlertKey
		inner join AlertServers t5 on t1.AlertKey=t5.AlertKey
		inner join Locations t9 on t5.LocationID=t9.ID
		inner join DeviceInventory t4 on t9.ID=t4.LocationID and t5.ServerID=t4.DeviceID
		inner join ServerTypes t3 on t4.DeviceTypeID=t3.ID
		inner join EventsMaster t8 on t4.DeviceTypeID=t8.ServerTypeID
		where t6.ServerTypeID=t4.DeviceTypeID and t5.LocationID=t4.LocationID and t6.EventID=t8.ID
		union
		select distinct t1.AlertKey AlertKey,t1.HoursIndicator HoursIndicator,t1.SendTo SendTo,t1.CopyTo CopyTo,
		t1.BlindCopyTo BlindCopyTo,t1.StartTime StartTime,t1.DAY Day,t1.Duration Duration,
		t1.SendSNMPTrap SendSNMPTrap,
		t2.AlertName AlertName,t8.EventName EventName,t3.ServerType ServerType,t4.Name ServerName,
		t1.EnablePersistentAlert EnablePersistentAlert,ISNULL(t1.SMSTo,'') SMSTo,ISNULL(t7.ScriptName,'') ScriptName,
		ISNULL(t7.ScriptCommand,'') ScriptCommand,ISNULL(t7.ScriptLocation,'') ScriptLocation
		from AlertDetails t1 inner join AlertNames t2 on t1.AlertKey=t2.AlertKey
		left outer join CustomScripts t7 on t1.ScriptID=t7.ID
		inner join AlertEvents t6 on t1.AlertKey=t6.AlertKey
		inner join AlertServers t5 on t1.AlertKey=t5.AlertKey
		inner join DeviceInventory t4 on t4.LocationID IS NULL
		inner join ServerTypes t3 on t4.DeviceTypeID=t3.ID
		inner join EventsMaster t8 on t4.DeviceTypeID=t8.ServerTypeID
		where t6.ServerTypeID=t4.DeviceTypeID and t6.EventID=t8.ID
		union
/* Case 6: One or more full servers/locations, one or more partial Event Categories */		
		select distinct t1.AlertKey AlertKey,t1.HoursIndicator HoursIndicator,t1.SendTo SendTo,t1.CopyTo CopyTo,
		t1.BlindCopyTo BlindCopyTo,t1.StartTime StartTime,t1.DAY Day,t1.Duration Duration,
		t1.SendSNMPTrap SendSNMPTrap,
		t2.AlertName AlertName,t8.EventName EventName,t3.ServerType ServerType,t4.Name ServerName,
		t1.EnablePersistentAlert EnablePersistentAlert,ISNULL(t1.SMSTo,'') SMSTo,ISNULL(t7.ScriptName,'') ScriptName,
		ISNULL(t7.ScriptCommand,'') ScriptCommand,ISNULL(t7.ScriptLocation,'') ScriptLocation
		from AlertDetails t1 inner join AlertNames t2 on t1.AlertKey=t2.AlertKey
		left outer join CustomScripts t7 on t1.ScriptID=t7.ID
		inner join AlertEvents t6 on t1.AlertKey=t6.AlertKey
		inner join AlertServers t5 on t1.AlertKey=t5.AlertKey
		inner join Locations t9 on t5.LocationID=t9.ID
		inner join DeviceInventory t4 on t9.ID=t4.LocationID
		inner join ServerTypes t3 on t4.DeviceTypeID=t3.ID
		inner join EventsMaster t8 on t4.DeviceTypeID=t8.ServerTypeID
		where t6.ServerTypeID=t4.DeviceTypeID and t6.EventID=t8.ID and t5.ServerID=0
		union
		select distinct t1.AlertKey AlertKey,t1.HoursIndicator HoursIndicator,t1.SendTo SendTo,t1.CopyTo CopyTo,
		t1.BlindCopyTo BlindCopyTo,t1.StartTime StartTime,t1.DAY Day,t1.Duration Duration,
		t1.SendSNMPTrap SendSNMPTrap,
		t2.AlertName AlertName,t8.EventName EventName,t3.ServerType ServerType,t4.Name ServerName,
		t1.EnablePersistentAlert EnablePersistentAlert,ISNULL(t1.SMSTo,'') SMSTo,ISNULL(t7.ScriptName,'') ScriptName,
		ISNULL(t7.ScriptCommand,'') ScriptCommand,ISNULL(t7.ScriptLocation,'') ScriptLocation
		from AlertDetails t1 inner join AlertNames t2 on t1.AlertKey=t2.AlertKey
		left outer join CustomScripts t7 on t1.ScriptID=t7.ID
		inner join AlertEvents t6 on t1.AlertKey=t6.AlertKey
		inner join AlertServers t5 on t1.AlertKey=t5.AlertKey
		inner join DeviceInventory t4 on t4.LocationID IS NULL
		inner join ServerTypes t3 on t4.DeviceTypeID=t3.ID
		inner join EventsMaster t8 on t4.DeviceTypeID=t8.ServerTypeID
		where t6.ServerTypeID=t4.DeviceTypeID and t6.EventID=t8.ID and t5.ServerID = 0
		union
/* Case 7: One or more full Event categories, one or more partial servers/locations */
		select distinct t1.AlertKey AlertKey,t1.HoursIndicator HoursIndicator,t1.SendTo SendTo,t1.CopyTo CopyTo,
		t1.BlindCopyTo BlindCopyTo,t1.StartTime StartTime,t1.DAY Day,t1.Duration Duration,
		t1.SendSNMPTrap SendSNMPTrap,
		t2.AlertName AlertName,t8.EventName EventName,t3.ServerType ServerType,t4.Name ServerName,
		t1.EnablePersistentAlert EnablePersistentAlert,ISNULL(t1.SMSTo,'') SMSTo,ISNULL(t7.ScriptName,'') ScriptName,
		ISNULL(t7.ScriptCommand,'') ScriptCommand,ISNULL(t7.ScriptLocation,'') ScriptLocation
		from AlertDetails t1 inner join AlertNames t2 on t1.AlertKey=t2.AlertKey
		left outer join CustomScripts t7 on t1.ScriptID=t7.ID
		inner join AlertEvents t6 on t1.AlertKey=t6.AlertKey
		inner join AlertServers t5 on t1.AlertKey=t5.AlertKey
		inner join Locations t9 on t5.LocationID=t9.ID
		inner join DeviceInventory t4 on t9.ID=t4.LocationID
		inner join ServerTypes t3 on t4.DeviceTypeID=t3.ID
		inner join EventsMaster t8 on t4.DeviceTypeID=t8.ServerTypeID
		where t6.ServerTypeID=t4.DeviceTypeID and t5.ServerID=t4.DeviceID and t6.EventID=0
		union
		select distinct t1.AlertKey AlertKey,t1.HoursIndicator HoursIndicator,t1.SendTo SendTo,t1.CopyTo CopyTo,
		t1.BlindCopyTo BlindCopyTo,t1.StartTime StartTime,t1.DAY Day,t1.Duration Duration,
		t1.SendSNMPTrap SendSNMPTrap,
		t2.AlertName AlertName,t8.EventName EventName,t3.ServerType ServerType,t4.Name ServerName,
		t1.EnablePersistentAlert EnablePersistentAlert,ISNULL(t1.SMSTo,'') SMSTo,ISNULL(t7.ScriptName,'') ScriptName,
		ISNULL(t7.ScriptCommand,'') ScriptCommand,ISNULL(t7.ScriptLocation,'') ScriptLocation
		from AlertDetails t1 inner join AlertNames t2 on t1.AlertKey=t2.AlertKey
		left outer join CustomScripts t7 on t1.ScriptID=t7.ID
		inner join AlertEvents t6 on t1.AlertKey=t6.AlertKey
		inner join AlertServers t5 on t1.AlertKey=t5.AlertKey
		inner join DeviceInventory t4 on t4.LocationID IS NULL
		inner join ServerTypes t3 on t4.DeviceTypeID=t3.ID
		inner join EventsMaster t8 on t4.DeviceTypeID=t8.ServerTypeID
		where t6.ServerTypeID=t4.DeviceTypeID and t6.EventID=0
		union
/* Case 8: One or more full servers/locations, ALL Event Categories */
		select distinct t1.AlertKey AlertKey,t1.HoursIndicator HoursIndicator,t1.SendTo SendTo,t1.CopyTo CopyTo,
		t1.BlindCopyTo BlindCopyTo,t1.StartTime StartTime,t1.DAY Day,t1.Duration Duration,
		t1.SendSNMPTrap SendSNMPTrap,
		t2.AlertName AlertName,t8.EventName EventName,t3.ServerType ServerType,t4.Name ServerName,
		t1.EnablePersistentAlert EnablePersistentAlert,ISNULL(t1.SMSTo,'') SMSTo,ISNULL(t7.ScriptName,'') ScriptName,
		ISNULL(t7.ScriptCommand,'') ScriptCommand,ISNULL(t7.ScriptLocation,'') ScriptLocation
		from AlertDetails t1 inner join AlertNames t2 on t1.AlertKey=t2.AlertKey
		left outer join CustomScripts t7 on t1.ScriptID=t7.ID
		inner join AlertEvents t6 on t1.AlertKey=t6.AlertKey
		inner join AlertServers t5 on t1.AlertKey=t5.AlertKey
		inner join Locations t9 on t5.LocationID=t9.ID
		inner join DeviceInventory t4 on t9.ID=t4.LocationID and t5.ServerID=t4.DeviceID 
		inner join ServerTypes t3 on t4.DeviceTypeID=t3.ID
		inner join EventsMaster t8 on t4.DeviceTypeID=t8.ServerTypeID
		where t6.EventID=0 and t6.ServerTypeID=0
		union
		select distinct t1.AlertKey AlertKey,t1.HoursIndicator HoursIndicator,t1.SendTo SendTo,t1.CopyTo CopyTo,
		t1.BlindCopyTo BlindCopyTo,t1.StartTime StartTime,t1.DAY Day,t1.Duration Duration,
		t1.SendSNMPTrap SendSNMPTrap,
		t2.AlertName AlertName,t8.EventName EventName,t3.ServerType ServerType,t4.Name ServerName,
		t1.EnablePersistentAlert EnablePersistentAlert,ISNULL(t1.SMSTo,'') SMSTo,ISNULL(t7.ScriptName,'') ScriptName,
		ISNULL(t7.ScriptCommand,'') ScriptCommand,ISNULL(t7.ScriptLocation,'') ScriptLocation
		from AlertDetails t1 inner join AlertNames t2 on t1.AlertKey=t2.AlertKey
		left outer join CustomScripts t7 on t1.ScriptID=t7.ID
		inner join AlertEvents t6 on t1.AlertKey=t6.AlertKey
		inner join AlertServers t5 on t1.AlertKey=t5.AlertKey
		inner join DeviceInventory t4 on t4.LocationID IS NULL
		inner join ServerTypes t3 on t4.DeviceTypeID=t3.ID
		inner join EventsMaster t8 on t4.DeviceTypeID=t8.ServerTypeID
		where t6.EventID=0 and t6.ServerTypeID=0
	) as tmp
	order by AlertKey
END

GO
-- =============================================
-- Author:		Natallya Shkarayeva
-- Create date: 12/3/2013
-- Description:	GetAlertsWithAllEventsSelected
--
-- This stored procedure gets event/server information based on the user selections
-- on the Alert Definition page. The query returns selections that cover all event types 
-- for a specific category, i.e., 
-- all Domino events have been selected from the events grid and some servers have been selected
-- from the servers grid.
--
-- Modified date: 4/7/2014 for VSPLUS-519
-- Description: 
-- Added a new key to the select clause to identify the alerts with persistent alerting enabled.
-- =============================================
CREATE PROCEDURE [dbo].[GetAlertsWithAllEventsSelected]
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    select distinct AlertKey,HoursIndicator,SendTo,CopyTo,BlindCopyTo,ISNULL(StartTime,'') StartTime,
   Day,Duration,SendSNMPTrap,AlertName,EventName,ServerType,ServerName,
    EnablePersistentAlert from
	(
		-- 3/5/2014 NS added
		-- Multiple full categories of Events (Domino, Exchange), 
		-- some servers (single location, Phoenix) - Servers
		select distinct t1.AlertKey AlertKey,t1.HoursIndicator HoursIndicator,t1.SendTo SendTo,t1.CopyTo CopyTo,
		t1.BlindCopyTo BlindCopyTo,t1.StartTime StartTime,t1.DAY Day,t1.Duration Duration,
		t1.SendSNMPTrap SendSNMPTrap,
		t2.AlertName AlertName,t8.EventName EventName,t3.ServerType ServerType,ISNULL(t5.ServerName,'') ServerName,
		t1.EnablePersistentAlert EnablePersistentAlert
		from AlertDetails t1 inner join AlertNames t2 on t1.AlertKey=t2.AlertKey
		inner join AlertEvents t7 on t1.AlertKey = t7.AlertKey 
		inner join EventsMaster t8 on t8.ServerTypeID = t7.ServerTypeID
		inner join ServerTypes t3 on t7.ServerTypeID = t3.ID
		inner join AlertServers t4 on t2.AlertKey=t4.AlertKey
		inner join Servers t5 on t5.ID=t4.ServerID and t8.ServerTypeID=t5.ServerTypeID and t5.LocationID=t4.LocationID
		inner join Locations t6 on t4.LocationID=t6.ID and t6.Location !='URL'
		where t7.EventID = 0
		-- 3/5/2014 NS added
		-- Multiple full categories of Events (Domino, Exchange), 
		-- some servers (single location, Phoenix) - URLs
		union
		select distinct t1.AlertKey AlertKey,t1.HoursIndicator HoursIndicator,t1.SendTo SendTo,t1.CopyTo CopyTo,
		t1.BlindCopyTo BlindCopyTo,t1.StartTime StartTime,t1.DAY Day,t1.Duration Duration,
		t1.SendSNMPTrap SendSNMPTrap,
		t2.AlertName AlertName,t8.EventName EventName,t3.ServerType ServerType,ISNULL(t5.Name,'') ServerName,
		t1.EnablePersistentAlert EnablePersistentAlert
		from AlertDetails t1 inner join AlertNames t2 on t1.AlertKey=t2.AlertKey
		inner join AlertEvents t7 on t1.AlertKey = t7.AlertKey 
		inner join EventsMaster t8 on t8.ServerTypeID = t7.ServerTypeID
		inner join ServerTypes t3 on t7.ServerTypeID = t3.ID
		inner join AlertServers t4 on t2.AlertKey=t4.AlertKey
		inner join URLs t5 on t5.ID=t4.ServerID and t8.ServerTypeID=t5.ServerTypeID and t5.LocationId=t4.LocationID
		inner join Locations t6 on t4.LocationID=t6.ID and t6.Location !='URL'
		where t7.EventID = 0
		union
		select distinct t1.AlertKey AlertKey,t1.HoursIndicator HoursIndicator,t1.SendTo SendTo,t1.CopyTo CopyTo,
		t1.BlindCopyTo BlindCopyTo,t1.StartTime StartTime,t1.DAY Day,t1.Duration Duration,
		t1.SendSNMPTrap SendSNMPTrap,
		t2.AlertName AlertName,t8.EventName EventName,t3.ServerType ServerType,ISNULL(t5.Name,'') ServerName,
		t1.EnablePersistentAlert EnablePersistentAlert
		from AlertDetails t1 inner join AlertNames t2 on t1.AlertKey=t2.AlertKey
		inner join AlertEvents t7 on t1.AlertKey = t7.AlertKey 
		inner join EventsMaster t8 on t8.ServerTypeID = t7.ServerTypeID
		inner join ServerTypes t3 on t7.ServerTypeID = t3.ID
		inner join AlertServers t4 on t2.AlertKey=t4.AlertKey
		inner join URLs t5 on t5.ID=t4.ServerID and t8.ServerTypeID=t5.ServerTypeID
		inner join Locations t6 on t4.LocationID=t6.ID and t6.Location ='URL'
		where t7.EventID = 0
		union
		select t1.AlertKey AlertKey,t1.HoursIndicator HoursIndicator,t1.SendTo SendTo,t1.CopyTo CopyTo,
		t1.BlindCopyTo BlindCopyTo,t1.StartTime StartTime,t1.DAY Day,t1.Duration Duration,
		t1.SendSNMPTrap SendSNMPTrap,
		t2.AlertName AlertName,t4.EventName EventName,t5.ServerType ServerType,ISNULL(t6.ServerName,'') as ServerName,
		t1.EnablePersistentAlert EnablePersistentAlert
		from AlertDetails t1 inner join AlertNames t2 on t1.AlertKey=t2.AlertKey
		inner join AlertEvents t3 on t2.AlertKey=t3.AlertKey 
		inner join ServerTypes t5 on t3.ServerTypeID=t5.ID 
		inner join EventsMaster t4 on t4.ServerTypeID=t3.ServerTypeID
		left outer join Servers t6 on t6.ServerTypeID = t5.ID
		where t6.ServerName is null and t5.ServerType != 'URL' and t3.EventID=0
		-- 3/5/2014 NS added 
		-- Full event category, i.e., Domino
		-- and a few servers from the Servers grid - device names come from the Servers table
		union
		select distinct t1.AlertKey AlertKey,t1.HoursIndicator HoursIndicator,t1.SendTo SendTo,t1.CopyTo CopyTo,
		t1.BlindCopyTo BlindCopyTo,t1.StartTime StartTime,t1.DAY Day,t1.Duration Duration,
		t1.SendSNMPTrap SendSNMPTrap,
		t2.AlertName AlertName,t8.EventName EventName,t3.ServerType ServerType,ISNULL(t5.ServerName,'') ServerName,
		t1.EnablePersistentAlert EnablePersistentAlert
		from AlertDetails t1 inner join AlertNames t2 on t1.AlertKey=t2.AlertKey
		inner join AlertEvents t7 on t1.AlertKey = t7.AlertKey 
		--inner join EventsMaster t8 on t8.ServerTypeID = t7.ServerTypeID
		inner join AlertServers t4 on t2.AlertKey=t4.AlertKey
		inner join Servers t5 on t5.ID=t4.ServerID 
		inner join EventsMaster t8 on t8.ServerTypeID=t5.ServerTypeID
		inner join ServerTypes t3 on t8.ServerTypeID = t3.ID
		inner join Locations t6 on t4.LocationID=t6.ID and t6.Location !='URL'
		where t7.EventID = 0 and t7.ServerTypeID = 0
		-- 3/5/2014 NS added 
		-- Full event category, i.e., Domino
		-- and few servers from the Servers grid - device names come from the URLs table
		union
		select distinct t1.AlertKey AlertKey,t1.HoursIndicator HoursIndicator,t1.SendTo SendTo,t1.CopyTo CopyTo,
		t1.BlindCopyTo BlindCopyTo,t1.StartTime StartTime,t1.DAY Day,t1.Duration Duration,
		t1.SendSNMPTrap SendSNMPTrap,
		t2.AlertName AlertName,t8.EventName EventName,t3.ServerType ServerType,ISNULL(t5.Name,'') ServerName,
		t1.EnablePersistentAlert EnablePersistentAlert
		from AlertDetails t1 inner join AlertNames t2 on t1.AlertKey=t2.AlertKey
		inner join AlertEvents t7 on t1.AlertKey = t7.AlertKey 
		--inner join EventsMaster t8 on t8.ServerTypeID = t7.ServerTypeID
		inner join AlertServers t4 on t2.AlertKey=t4.AlertKey 
		inner join EventsMaster t8 on t8.ServerTypeID=t4.ServerTypeID
		inner join ServerTypes t3 on t8.ServerTypeID = t3.ID
		inner join Locations t6 on t4.LocationID=t6.ID
		inner join URLs t5 on t5.LocationId=t6.ID and t5.ServerTypeId=t3.ID
		where t7.EventID = 0 and t7.ServerTypeID = 0
		-- 3/5/2014 NS added 
		-- ALL Event categories, i.e., Domino, BES, etc.
		-- and ALL Locations from the Servers grid - device names come from Servers table
		union
		select distinct t1.AlertKey AlertKey,t1.HoursIndicator HoursIndicator,t1.SendTo SendTo,t1.CopyTo CopyTo,
		t1.BlindCopyTo BlindCopyTo,t1.StartTime StartTime,t1.DAY Day,t1.Duration Duration,
		t1.SendSNMPTrap SendSNMPTrap,
		t2.AlertName AlertName,t8.EventName EventName,t3.ServerType ServerType,ISNULL(t5.ServerName,'') ServerName,
		t1.EnablePersistentAlert EnablePersistentAlert
		from AlertDetails t1 inner join AlertNames t2 on t1.AlertKey=t2.AlertKey
		inner join AlertEvents t7 on t1.AlertKey = t7.AlertKey 
		inner join AlertServers t4 on t2.AlertKey=t4.AlertKey,
		Servers t5,EventsMaster t8,ServerTypes t3, Locations t6 
		where t5.ServerTypeID=t3.ID and t3.ID=t8.ServerTypeID and t5.LocationID=t6.ID and
		t7.EventID = 0 and t7.ServerTypeID = 0 and t4.ServerID=0 and t4.ServerTypeID=0 and t4.LocationID=0
		-- 3/5/2014 NS added 
		-- ALL Event categories, i.e., Domino, BES, etc.
		-- and ALL Locations from the Servers grid - device names come from URLs table
		union
		select distinct t1.AlertKey AlertKey,t1.HoursIndicator HoursIndicator,t1.SendTo SendTo,t1.CopyTo CopyTo,
		t1.BlindCopyTo BlindCopyTo,t1.StartTime StartTime,t1.DAY Day,t1.Duration Duration,
		t1.SendSNMPTrap SendSNMPTrap,
		t2.AlertName AlertName,t8.EventName EventName,t3.ServerType ServerType,ISNULL(t5.Name,'') ServerName,
		t1.EnablePersistentAlert EnablePersistentAlert
		from AlertDetails t1 inner join AlertNames t2 on t1.AlertKey=t2.AlertKey
		inner join AlertEvents t7 on t1.AlertKey = t7.AlertKey 
		inner join AlertServers t4 on t2.AlertKey=t4.AlertKey,
		URLs t5,EventsMaster t8,ServerTypes t3, Locations t6 
		where t5.ServerTypeID=t3.ID and t3.ID=t8.ServerTypeID and t5.LocationID=t6.ID and
		t7.EventID = 0 and t7.ServerTypeID = 0 and t4.ServerID=0 and t4.ServerTypeID=0 and t4.LocationID=0
		-- 3/6/2014 NS added
		-- ALL Event categories, single/multiple full Location categories, i.e., Phoenix and Boston
		-- device names come from the Servers table
		union
		select distinct t1.AlertKey AlertKey,t1.HoursIndicator HoursIndicator,t1.SendTo SendTo,t1.CopyTo CopyTo,
		t1.BlindCopyTo BlindCopyTo,t1.StartTime StartTime,t1.DAY Day,t1.Duration Duration,
		t1.SendSNMPTrap SendSNMPTrap,
		t2.AlertName AlertName,t8.EventName EventName,t3.ServerType ServerType,ISNULL(t5.ServerName,'') ServerName,
		t1.EnablePersistentAlert EnablePersistentAlert
		from AlertDetails t1 inner join AlertNames t2 on t1.AlertKey=t2.AlertKey
		inner join AlertEvents t7 on t1.AlertKey = t7.AlertKey 
		inner join AlertServers t4 on t2.AlertKey=t4.AlertKey
		inner join	Servers t5 on t5.LocationID=t4.LocationID 
		inner join EventsMaster t8 on t8.ServerTypeID=t5.ServerTypeID
		inner join ServerTypes t3 on t3.ID=t5.ServerTypeID 
		inner join Locations t6 on t6.ID=t4.LocationID
		where t5.ServerTypeID=t3.ID and t3.ID=t8.ServerTypeID and t5.LocationID=t6.ID and
		t7.EventID = 0 and t7.ServerTypeID = 0 and t4.ServerID=0 and t4.ServerTypeID=0
		-- 3/6/2014 NS added
		-- ALL Event categories, single/multiple full Location categories, i.e., Phoenix and Boston
		-- device names come from the URLs table
		union
		select t1.AlertKey AlertKey,t1.HoursIndicator HoursIndicator,t1.SendTo SendTo,t1.CopyTo CopyTo,
		t1.BlindCopyTo BlindCopyTo,t1.StartTime StartTime,t1.DAY Day,t1.Duration Duration,
		t1.SendSNMPTrap SendSNMPTrap,
		t2.AlertName AlertName,t4.EventName EventName,t5.ServerType ServerType,NotesMailProbe.Name as ServerName,
		t1.EnablePersistentAlert EnablePersistentAlert
		from AlertDetails t1 inner join AlertNames t2 on t1.AlertKey=t2.AlertKey
		inner join AlertEvents t3 on t2.AlertKey=t3.AlertKey 
		inner join ServerTypes t5 on t5.ID =13
		inner join EventsMaster t4 on t4.ServerTypeID=13
		,NotesMailProbe 
		where t3.EventID=0 and t3.ServerTypeID =0
		union
		select t1.AlertKey AlertKey,t1.HoursIndicator HoursIndicator,t1.SendTo SendTo,t1.CopyTo CopyTo,
		t1.BlindCopyTo BlindCopyTo,t1.StartTime StartTime,t1.DAY Day,t1.Duration Duration,
		t1.SendSNMPTrap SendSNMPTrap,
		t2.AlertName AlertName,t4.EventName EventName,t5.ServerType ServerType,mu.DeviceId + '-' + td.UserName as ServerName,
		t1.EnablePersistentAlert EnablePersistentAlert
		from AlertDetails t1 inner join AlertNames t2 on t1.AlertKey=t2.AlertKey
		inner join AlertEvents t3 on t2.AlertKey=t3.AlertKey 
		inner join ServerTypes t5 on t5.ID =11
		inner join EventsMaster t4 on t4.ServerTypeID=11,
		MobileUserThreshold mu
		inner join Traveler_Devices td on td.DeviceID =mu.DeviceId
		where t3.EventID=0 and t3.ServerTypeID =0
		union
		select distinct t1.AlertKey AlertKey,t1.HoursIndicator HoursIndicator,t1.SendTo SendTo,t1.CopyTo CopyTo,
		t1.BlindCopyTo BlindCopyTo,t1.StartTime StartTime,t1.DAY Day,t1.Duration Duration,
		t1.SendSNMPTrap SendSNMPTrap,
		t2.AlertName AlertName,t8.EventName EventName,t3.ServerType ServerType,ISNULL(t5.Name,'') ServerName,
		t1.EnablePersistentAlert EnablePersistentAlert
		from AlertDetails t1 inner join AlertNames t2 on t1.AlertKey=t2.AlertKey
		inner join AlertEvents t7 on t1.AlertKey = t7.AlertKey 
		inner join AlertServers t4 on t2.AlertKey=t4.AlertKey
		inner join	URLs t5 on t5.LocationID=t4.LocationID 
		inner join EventsMaster t8 on t8.ServerTypeID=t5.ServerTypeID
		inner join ServerTypes t3 on t3.ID=t5.ServerTypeID 
		inner join Locations t6 on t6.ID=t4.LocationID
		where t5.ServerTypeID=t3.ID and t3.ID=t8.ServerTypeID and t5.LocationID=t6.ID and
		t7.EventID = 0 and t7.ServerTypeID = 0 and t4.ServerID=0 and t4.ServerTypeID=0
	) as tmp
	order by AlertKey
END


GO
-- =============================================
-- Author:		Natallya Shkarayeva
-- Create date: 12/3/2013
-- Modified date: 4/23/2015 for VSPLUS-1297
-- Description:	[ShouldAlertGoOutNow]
--
-- This stored procedure evaluates the alert parameters, i.e., StartTime, duration, type of alert, days to
-- notify, etc. and returns 1 if the entered parameters indicate that an alert should be sent now
-- or 0 if the time is outside of the currently active time frame. 
-- Business Hours information is based on the HoursIndicator table values.
-- =============================================
CREATE PROCEDURE [dbo].[ShouldAlertGoOutNow](@StartTime as varchar(50), @Duration as int, @DaysStr as varchar(200), @IntType as int)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	Declare @IsDayIncluded as int
    Declare @IsSpecific as int
    Declare @StartTimeDT as datetime
    
    Select @StartTimeDT = CONVERT(time,@StartTime)
    if @IntType != 3
		begin
			Select @IsSpecific = case when (convert(time,GETDATE()) >= convert(time,@StartTimeDT) and 
			convert(time,GETDATE()) <= convert(time,dateadd(minute,@Duration,@StartTimeDT))) then 1 else 0 end
		end
	else
		Select @IsSpecific = 0
    Select @IsDayIncluded = case when CHARINDEX(datename(dw,GETDATE()),@DaysStr) > 0 then 1 else 0 end
    
    if @IntType !=3
		Return @IsSpecific & @IsDayIncluded
	else
		Return 1
END

GO



USE [vitalsigns]
GO

/* 1/8/2013 NS modified - added PK */
/*
Create Table DailyTasks
(
ID INT NOT NULL IDENTITY(1,1),
SourceTableName Varchar(150),
SourceAggregation Varchar(150),
SourceStatName Varchar(150),
DestinationTableName Varchar(150),
DestinationStatName Varchar(150)
)
*/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[DailyTasks](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[SourceTableName] [varchar](150) NOT NULL,
	[SourceAggregation] [varchar](150) NOT NULL,
	[SourceStatName] [varchar](150) NOT NULL,
	[DestinationTableName] [varchar](150) NULL,
	[DestinationStatName] [varchar](150) NULL,
	QueryType int,
 CONSTRAINT [PK_DailyTasks] PRIMARY KEY CLUSTERED 
(
	[SourceTableName] ASC,
	[SourceAggregation] ASC,
	[SourceStatName] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

USE [vitalsigns]
GO
CREATE TABLE [dbo].[ProfilesMaster](
 [ID] [int] IDENTITY(1,1) NOT NULL,
 [ServerTypeId] [int] NULL,
 [AttributeName] [nvarchar](100) NULL,
 [DefaultValue] [nvarchar](100) NULL,
 [UnitOfMeasurement] [nvarchar](100) NULL,
 [RelatedTable] [nvarchar](100) NULL,
 [RelatedField] [nvarchar](100) NULL,
 [RoleType] [nvarchar](100) NULL,
 [ProfileId] [int]  NULL,
 [isSelected] [bit]  NULL
) ON [PRIMARY]

GO


USE [vitalsigns]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ProfileNames](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[ProfileName] [nvarchar](20) NULL,
 CONSTRAINT [PK_ProfileNames] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO


USE [vitalsigns]
GO
CREATE TABLE [dbo].[UserProfileMaster](
 [ID] [int] IDENTITY(1,1) NOT NULL,
 [Name] [nvarchar](50) NULL
) ON [PRIMARY]
GO

USE [vitalsigns]
GO
CREATE TABLE [dbo].[UserProfileDetailed](
 [ID] [int] IDENTITY(1,1) NOT NULL,
 [UserProfileMasterID] [int] NULL,
 [ProfilesMasterID] [int] NULL,
 [Value] [nvarchar](100) NULL
) ON [PRIMARY]
GO

USE [vitalsigns]
GO
CREATE TABLE dbo.UsersStartupURLs(
 ID [int] IDENTITY(1,1) NOT NULL,
 URL VARCHAR(100),
 Name VARCHAR(50),
 IsDashboard BIT,
 IsConfigurator BIT,
 IsConsoleComm BIT
) ON [PRIMARY]
	
GO

/****** Object:  Table [dbo].[Traveler_HA_Datastore]    Script Date: 01/24/2014 09:55:34 ******/
/* 1/24/2014 NS added new table [Traveler_HA_Datastore] */
/* 7/10/2014 NS modified the table for VSPLUS-806, new column DatabaseName */
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
	[DatabaseName] [varchar](150) NOT NULL DEFAULT 'Traveler'
 CONSTRAINT [PK_Traveler_HA_Datastore] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

-- DRS: We do not need this view anymore.
USE [vitalsigns]
GO

IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[MonitoringTables]'))
DROP VIEW [dbo].[MonitoringTables]
GO


IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ConfigurationsChanged]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[ConfigurationsChanged]
GO


CREATE PROCEDURE [dbo].[ConfigurationsChanged](
@sTableName varchar(30),
@sServiceName varchar(255)
)
as
begin
Declare @individual varchar(255)
WHILE LEN(@sServiceName) > 0    
BEGIN     
	IF PATINDEX('%,%',@sServiceName) > 0     
	BEGIN      
	   SET @individual = SUBSTRING(@sServiceName, 0, PATINDEX('%,%',@sServiceName))         
	   SET @sServiceName = SUBSTRING(@sServiceName, LEN(@individual + ',') + 1, LEN(@sServiceName))               
	END     
	ELSE     
	BEGIN      
	   SET @individual = @sServiceName      
	   SET @sServiceName = NULL         
	END     
	declare @cmd varchar(2000) 
	set @cmd = 'INSERT INTO NodeDetails (NodeID, Name, Value) SELECT ID, ''' + @individual + ' - UpdateCollection'' Name, 1 FROM Nodes WHERE ID NOT IN (SELECT NodeID FROM NodeDetails WHERE Name=''' + @individual + ' - UpdateCollection'')'
	EXEC( @cmd )
	set @cmd = 'Update NodeDetails set Value=1 where Name = ''' + @individual + ' - UpdateCollection'''
	EXEC( @cmd )
END  
END

GO


IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[AddAuditTrailTrigger]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[AddAuditTrailTrigger]
GO


CREATE PROCEDURE [dbo].[AddAuditTrailTrigger](
	@sTableName 	VARCHAR (250),
	@sServiceName   VARCHAR (250)
)  
AS BEGIN

   DECLARE @sLastCommand VARCHAR (8000)
  -- drop an exiting audit trail trigger
  Set @sLastCommand =
  'IF EXISTS (SELECT name FROM dbo.sysobjects 
  WHERE name = ''tr_Audit' + replace(@sTableName,' ','_') + ''' AND type = ''TR'')		
  DROP TRIGGER dbo.tr_Audit' + replace(@sTableName,' ','_') 
  EXEC (@sLastCommand)

  -- create the new one 
  Set @sLastCommand = 
  'CREATE TRIGGER dbo.tr_Audit' + replace(@sTableName,' ','_') + ' ON [' + @sTableName + '] FOR INSERT,UPDATE,DELETE AS 
  DECLARE @sTbname VARCHAR(20)
  BEGIN
  SET @sTbname = ''' + @sTableName + ''''
  

  SET @sLastCommand = @sLastCommand +	
  '
  EXEC dbo.ConfigurationsChanged @sTbname,''' + @sServiceName + '''
  END
'
  EXEC (@sLastCommand)
END

GO


IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[AddAllAuditTrailTriggers]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[AddAllAuditTrailTriggers]
GO


CREATE PROCEDURE [dbo].[AddAllAuditTrailTriggers]   
AS BEGIN

  DECLARE @sLastCommand VARCHAR (2000)
    DECLARE @sTableName VARCHAR (250)
    DECLARE @sServiceName VARCHAR (250)
    DECLARE VSTableNames CURSOR FOR SELECT UPPER(TableName), UPPER(ServiceCollectionType) FROM dbo.MonitoringTablesToCollections ORDER BY TableName
    DECLARE VSOldTables CURSOR FOR SELECT name FROM dbo.sysobjects WHERE name like 'tr_Audit%' and name not in 
    (select 'tr_Audit' + UPPER(REPLACE(TableName, ' ', '_')) From MonitoringTablesToCollections)  AND type = 'TR' order by name
  
    Open VSOldTables
    FETCH NEXT FROM VSOldTables into @sTableName
  
    WHILE @@FETCH_STATUS = 0 
  	BEGIN
  	print 'Dropping old triggers...'
      -- drop an exiting audit trail trigger
      print 'Drop Trigger on: ' + @sTableName
      Set @sLastCommand =
      'IF EXISTS (SELECT name FROM dbo.sysobjects 
      WHERE name = ''' + replace(@sTableName,' ','_') + ''' AND type = ''TR'')		
      DROP TRIGGER dbo.' + replace(@sTableName,' ','_') 
      print @sLastCommand
      EXEC (@sLastCommand)
        
      FETCH NEXT FROM VSOldTables into @sTableName
    END
    CLOSE VSOldTables
    DEALLOCATE VSOldTables
  
  
    Open VSTableNames
    FETCH NEXT FROM VSTableNames into @sTableName,@sServiceName
  
    WHILE @@FETCH_STATUS = 0 
  	BEGIN
      -- drop an exiting audit trail trigger
      print 'Drop Trigger on: ' + @sTableName
      Set @sLastCommand =
      'IF EXISTS (SELECT name FROM dbo.sysobjects 
      WHERE name = ''tr_Audit' + replace(@sTableName,' ','_') + ''' AND type = ''TR'')		
      DROP TRIGGER dbo.tr_Audit' + replace(@sTableName,' ','_') 
      EXEC (@sLastCommand)
     
          print 'CREATE TRIGGER on: ' + @sTableName
          SET @sLastCommand =
  		'AddAuditTrailTrigger ''' + @sTableName + ''',''' + @sServiceName + ''''
          EXEC( @sLastCommand )
        
      FETCH NEXT FROM VSTableNames into @sTableName,@sServiceName
    END
    CLOSE VSTableNames
  DEALLOCATE VSTableNames
END

GO

/*Mukund changes 02/07/14 */
USE [vitalsigns]
GO
CREATE TABLE [dbo].[RPRAccessPages](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](250) NULL,
	[Category] [nvarchar](250) NULL,
	[Description] [nvarchar](250) NULL,
	[PageURL] [nvarchar](500) NULL,
	[ImageURL] [nvarchar](250) NULL)
Go

/*-----------*/

/****** Object:  Table [dbo].[ServerNodes]    Script Date: 02/07/2014  ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE dbo.ServerNodes
( 
	[NodeID] [int] NOT NULL,
	[NodeHostName] [varchar](50) NOT NULL,
	[NodeIPAddress] [varchar](150) NOT NULL,
	[NodeDescription] [varchar](50) NOT NULL,
	CONSTRAINT [PK_ServerNodes1] PRIMARY KEY CLUSTERED 
(
	[NodeID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

ALTER TABLE [dbo].[DominoServers]  WITH CHECK ADD  CONSTRAINT [FK_DominoServers_ServersNodes] FOREIGN KEY([MonitoredBy])
REFERENCES [dbo].[ServerNodes] ([NodeID])
ON UPDATE CASCADE
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[SametimeServers]  WITH CHECK ADD  CONSTRAINT [FK_SametimeServers_ServersNodes] FOREIGN KEY([MonitoredBy])
REFERENCES [dbo].[ServerNodes] ([NodeID])
ON UPDATE CASCADE
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[Servers]  WITH CHECK ADD  CONSTRAINT [FK_Servers_ServersNodes] FOREIGN KEY([MonitoredBy])
REFERENCES [dbo].[ServerNodes] ([NodeID])
ON UPDATE NO ACTION
ON DELETE NO ACTION
GO

ALTER TABLE [dbo].[URLs]  WITH CHECK ADD  CONSTRAINT [FK_URLs_ServersNodes] FOREIGN KEY([MonitoredBy])
REFERENCES [dbo].[ServerNodes] ([NodeID])
ON UPDATE CASCADE
ON DELETE CASCADE
GO



USE VitalSigns
Go


CREATE TABLE [dbo].[MSServerSettings](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[ServerID] [int] NULL,
	[ScanInterval] [int] NULL,
	[CredentialsID] [int] NULL,
	[Enabled] [bit] NULL,
	[RetryInterval] [int] NULL,
	[ResponseThreshold] [int] NULL,
	[OffscanInterval] [int] NULL,
	[FailuresbfrAlert] [int] NULL,
	[DiskSpace] [float] NULL,
	[CpuUtilization] [float] NULL,
	[NetwrkConn] [bit] NULL,
	[MemoryUsageAlert] [float] NULL,
	[IpAddress] [nvarchar](50) NULL,
 CONSTRAINT [PK_MSServerSettings] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

CREATE TABLE [dbo].[ServerThresholds](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[ServerID] [int] NULL,
	[ParameterID] [int] NULL,
	[Threshold] [int] NULL,
	[Type] [varchar](50) NULL,
	[ApplyAlert] [bit] NULL,
	[PSID] [int] NOT NULL,
 CONSTRAINT [PK_ServerThresholds] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO


/* 2/20/2014 NS added for VSPLUS-36 */
USE [vitalsigns]
GO

/****** Object:  Table [dbo].[ScheduledReports]    Script Date: 02/19/2014 16:33:01 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[ScheduledReports](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[ReportID] [int] NOT NULL,
	[Frequency] [varchar](50) NOT NULL,
	[Days] [varchar](150) NOT NULL,
	[SpecificDay] [int] NOT NULL,
	[SendTo] [varchar](250) NOT NULL,
	[CopyTo] [varchar](250) NULL,
	[BlindCopyTo] [varchar](250) NULL,
	[Title] [varchar](250) NOT NULL,
	[Body] [varchar](250) NULL,
	[FileFormat] [varchar](10) NOT NULL,
 CONSTRAINT [PK_ScheduledReports] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

USE [vitalsigns]
GO

/****** Object:  StoredProcedure [dbo].[ShouldScheduledReportsBeSent]    Script Date: 02/19/2014 16:41:25 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[ShouldScheduledReportsBeSent](@Frequency as varchar(50),@Days as varchar(150),@SpecificDay as int)
	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	DECLARE @RunReportWeekly as int
	DECLARE @RunReportMonthly as int

	SELECT @RunReportWeekly = CASE WHEN CHARINDEX(datename(dw,GETDATE()),@Days) > 0 THEN 1 ELSE 0 END
	SELECT @RunReportMonthly = CASE WHEN DAY(GETDATE())=@SpecificDay THEN 1 ELSE 0 END
	
	IF @Frequency = 'Daily'
		RETURN 1
	ELSE IF @Frequency = 'Weekly'
		RETURN @RunReportWeekly
	ELSE IF @Frequency = 'Monthly'
		RETURN @RunReportMonthly
	ELSE
		RETURN 0
END    

GO


-- =============================================
-- Author:		<Chandrahas>
-- Create date: <03/03/2014>
-- Description:	<Created UserPreference Table for VS348>
-- =============================================
USE [vitalsigns]
GO

/****** Object:  Table [dbo].[UserPreferences]    Script Date: 03/03/2014 11:16:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[UserPreferences](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[PreferenceName] [nvarchar](250) NULL,
	[PreferenceValue] [nvarchar](250) NULL,
	[UserID] [int] NOT NULL
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[UserPreferences]  WITH CHECK ADD FOREIGN KEY([UserID])
REFERENCES [dbo].[Users] ([ID])
ON DELETE CASCADE
GO

USE [vitalsigns]
GO
create table DiskSettings(
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[ServerID] int NOT NULL,
	[DiskName] [varchar](100) NOT NULL,
	[Threshold] [int] NOT NULL,
	[ThresholdType] [varchar](10) NULL,
	Foreign key (ServerID) references Servers(ID) on delete cascade	
)

GO

USE [vitalsigns]
GO
create table ExchangeSettings(
	[ServerId] [int] NOT NULL,
	[RoleCAS] [bit] not null default 0,
	[RoleHub] [bit] not null default 0,
	[RoleEdge] [bit] not null default 0,
	[RoleMailBox] [bit] not null default 0,
	[CASSmtp] [bit] not null default 0,
	[CASPop3] [bit] not null default 0,
	[CASImap] [bit] not null default 0,
	[CASOARPC] [bit] not null default 0,
	[CASOWA] [bit] not null default 0,
	[CASActiveSync] [bit] not null default 0,
	[CASEWS] [bit] not null default 0,
	[CASECP] [bit] not null default 0,
	[CASAutoDiscovery] [bit] not null default 0,
	[CASOAB] [bit] not null default 0,
	[SubQThreshold] int,
	[PoisonQThreshold] int,
	[UnReachableQThreshold] int,
	[TotalQThreshold] int,
	VersionNo    VARCHAR(20) NOT NULL,
	ActiveSyncCredentialsId int,
	[EnableLatencyTest] [bit] NULL,
	[LatencyYellowThreshold] [int] NULL,
	[LatencyRedThreshold] [int] NULL,
	[ShadowQThreshold] int,
	[AuthenticationType] [varchar](50) NULL
	Foreign key ([ServerId]) references Servers(ID) on delete cascade	
)

GO

USE [vitalsigns]
GO
create table StatusDetail(
	[ID] int Identity(1,1) not null primary key,
	[TypeAndName] [nvarchar](255) NOT NULL,
	[Category] [nvarchar](100) NOT NULL,
	[TestName] [nvarchar](100) NULL,
	[Result] [nvarchar](100) NULL,
	[LastUpdate] [datetime] NOT NULL,
	[Details] [varchar](100) NULL
)
GO
--CY: Delete Default foreing key constraint if exists
USE [vitalsigns]
GO
DECLARE @ConstraintName nvarchar(200)
SELECT @ConstraintName=f.name 
FROM sys.foreign_keys AS f
INNER JOIN sys.foreign_key_columns AS fc 
   ON f.object_id = fc.constraint_object_id 
WHERE f.parent_object_id = OBJECT_ID('StatusDetail') 
and parent_column_id = (SELECT column_id FROM sys.columns
                        WHERE NAME = N'TypeAndName'
                        AND object_id = OBJECT_ID(N'dbo.StatusDetail'));
IF @ConstraintName IS NOT NULL
EXEC('ALTER TABLE dbo.StatusDetail DROP CONSTRAINT ' + @ConstraintName)
GO
--CY: Adding named foreign key constraint
/****** Object:  ForeignKey [FK_Status_StatusDetail]    Script Date: 04/07/2014******/
ALTER TABLE [dbo].[StatusDetail]  WITH CHECK ADD  CONSTRAINT [FK_Status_StatusDetail] FOREIGN KEY([TypeANDName])
REFERENCES [dbo].[Status] ([TypeANDName])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[StatusDetail] CHECK CONSTRAINT [FK_Status_StatusDetail]
GO


/* 9/18/2014 NS modified for VSPLUS-951 */
/* 10/1/2015 NS modified for VSPLUS-2150 */
USE [vitalsigns]
GO
create table ClusterDatabaseDetails(
ID int not null identity(1,1),
ClusterID int not null,
DatabaseTitle [nvarchar] (250) null,
DatabaseName nvarchar (200) not null,
DocCountA int not null default 0,
DocCountB int not null default 0,
DocCountC int not null default 0,
DBSizeA float not null default 0,
DBSizeB float not null default 0,
DBSizeC float not null default 0,
[Description] nvarchar (500) not null,
LastScanned datetime default getdate(),
[ReplicaID] [varchar](50) NOT NULL DEFAULT '',
Foreign key (ClusterID) references DominoCluster(ID) on delete cascade	
)
GO

--Mukund 13Mar14: Exchange changes
CREATE TABLE [dbo].[ServerAttributes](
	[ServerID] [int] NOT NULL,
	[Enabled] [bit] NULL,
	[ScanInterval] [int] NULL,
	[RetryInterval] [int] NULL,
	[OffHourInterval] [int] NULL,
	[Category] [varchar](100) NULL,
	[CPU_Threshold] [int] NULL,
	[MemThreshold] [int] NULL,
	[ResponseTime] [int] NULL,
	[ConsFailuresBefAlert] [int] NULL,
	[ConsOvrThresholdBefAlert] [int] NULL,
	[ScanDAGHealth] [bit] NULL,
	[CredentialsId] [int] NULL,
	[IsPrereqsDone] [bit] NULL,
) ON [PRIMARY]

GO
CREATE TABLE [dbo].[RolesMaster](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[RoleName] [varchar](100) NOT NULL,
	[ServerTypeId] [int] NOT NULL
) ON [PRIMARY]

GO

USE VitalSigns
Go
Alter table RolesMaster
Add constraint PK_Rolesmaster primary key(id)
Go

CREATE TABLE [dbo].[ServiceVersionRole](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ServiceId] [int] NOT NULL,
	[VersionNo] [varchar](20) NOT NULL,
	[RoleId] [int] NOT NULL
) ON [PRIMARY]

GO
CREATE TABLE [dbo].[ServiceMaster](
	[ServiceId] [int] IDENTITY(1,1) NOT NULL,
	[ServiceName] [varchar](200) NOT NULL,
	[ServiceShortName] [varchar](200) NOT NULL,
	[SecurityContext] [varchar](200) NOT NULL,
	[ServiceDescription] [varchar](500) NOT NULL,
	[DefaultStartupType] [varchar](100) NOT NULL,
	[Required] [char](1) NOT NULL,
	[Custom] [bit] NOT NULL
) ON [PRIMARY]
GO
CREATE TABLE [dbo].[ServerVersions](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[VersionNo] [varchar](20) NULL
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[ServerServices](
	[ServerId] [int] NOT NULL,
	[SVRId] [int] NOT NULL
) ON [PRIMARY]
GO


--Chandrahas 3/17/2014
USE [VitalSigns]
GO

/****** Object:  Table [dbo].[ExgMailHealth]    Script Date: 03/17/2014 11:09:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[ExgMailHealth](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ServerName] [varchar](200) NOT NULL,
	[Status] [varchar](100) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ServerName] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

USE [VitalSigns]
GO

/****** Object:  Table [dbo].[ExgMailHealthDetails]    Script Date: 03/17/2014 11:09:30 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[ExgMailHealthDetails](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[ServerName] [varchar](200) NOT NULL,
	[DatabaseName] [varchar](200) NOT NULL,
	[MailBoxes] [int] NULL,
	[Size] [float] NULL,
	[WhiteSpaceSize] [float] NULL
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

ALTER TABLE [dbo].[ExgMailHealthDetails]  WITH CHECK ADD FOREIGN KEY([ServerName])
REFERENCES [dbo].[ExgMailHealth] ([ServerName])
GO

USE [VitalSigns]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ExchangeServerLocations]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[ExchangeServerLocations]
GO

/****** Object:  StoredProcedure [dbo].[ExchangeServerLocations] **/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Chandrahas Yeruva
-- Create date: 3/7/2014
-- Description:	
--The procedure is for getting the Server Locations Details for the Exchange Servers
-- 
-- =============================================
--CY altered procedure as per Table changes 03/27

USE [VitalSigns]
GO
/****** Object:  StoredProcedure [dbo].[ExchangeServerLocations]    Script Date: 03/16/2014 18:53:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[ExchangeServerLocations] as
Begin

declare @SrvLocations Table
(id int, LocId int, Name varchar(100),actid int,tbl varchar(50),srvtypeid int,ServerType varchar(100),description varchar(100),MonitoredBy varchar(100), RoleType varchar(100))

insert into @SrvLocations select id,null,Location,id,'Locations',null,null,null,null,null from Locations

Declare @count int
select @count=MAX(id) from Locations


Declare @ID int
Declare @LocationName varchar(100)
Declare @LocationID int
Declare @ServerTypeId int
Declare @ServerType varchar(100)
Declare @description varchar(100)
Declare @MonitoredBy varchar(100)
Declare @RoleType varchar(100)

	
		DECLARE db_cursor CURSOR FOR  
		select s.ID,s.ServerName,s.LocationID,s.ServerTypeId,st.ServerType,s.description,s.MonitoredBy,rm.RoleName from Servers s,ServerRoles sr, RolesMaster rm, ServerTypes st where s.ServerTypeID = st.ID 
		and st.servertype = 'Exchange' and sr.ServerId = s.ID and sr.RoleId = rm.Id
		union
		select s.ID,s.Name as ServerName,s.LocationID,s.ServerTypeId,st.ServerType,s.TheURL as description,s.MonitoredBy,rm.RoleName from URLs s,ServerRoles sr, RolesMaster rm, ServerTypes st where s.ServerTypeID = st.ID 
		and st.servertype = 'Exchange' and sr.ServerId = s.ID and sr.RoleId = rm.Id		
		
		OPEN db_cursor   
		FETCH NEXT FROM db_cursor into @ID,@LocationName,@LocationID,@ServerTypeId,@ServerType,@description,@MonitoredBy,@RoleType

		WHILE @@FETCH_STATUS = 0   
		BEGIN   
		Set @count=@count+1
		insert into @SrvLocations values(@count,@LocationID,@LocationName,@id,'Servers',@ServerTypeId,@ServerType,@description,@MonitoredBy,@RoleType)
		FETCH NEXT FROM db_cursor into @ID,@LocationName,@LocationID,@ServerTypeId,@ServerType,@description,@MonitoredBy,@RoleType
		END
		CLOSE db_cursor   
		DEALLOCATE db_cursor

		select * from @SrvLocations-- order by LocId,Name
end
Go

USE [VitalSigns]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CASResults]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[CASResults]
GO

/****** Object:  StoredProcedure [dbo].[CASResults] **/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


-- =============================================
-- Author:		Chandrahas	
-- Create date: 16/3/20134
-- Modified date: 7/22/2014, 7/29/2014 
-- Modified by: Natallya Shkarayeva, Natallya Shkarayeva
-- Description:	[CASResults]
-- Change description: removed all N/A values, replaced with empty strings; modified the Active Sync value,
-- the new value is Overall ActiveSync
-- This stored procedure returns all exchnage servers with CAS as role
-- =============================================

-- //4/25/2016 Sowjanya modified for VSPLUS-2850

USE [VitalSigns]
GO
/****** Object:  StoredProcedure [dbo].[CASResults]    Script Date: 03/16/2014 18:53:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[CASResults]
AS 
Begin
select Name,redirectto,isnull([SMTP],'') as [SMTP],isnull([RPC],'') as [RPC],isnull([IMAP],'') as [IMAP],isnull([OWA],'') as [OWA],
isnull([POP3],'') as [POP3],isnull([Active Sync],'') as [Active Sync],isnull([EWS],'') as [EWS],isnull([Auto Discovery],'') as [Auto Discovery],isnull([Services],'') as [Services] from
((select st.Name, sd.TestName,sd.Result,'ExchangeServerDetailsPage3.aspx?Name=' + st.Name + '&Type=' + st.Type + '&LastDate='+convert(varchar,st.LastUpdate) as redirectto
from Status st inner join StatusDetail sd 
on st.TypeANDName = sd.TypeAndName and st.Type='Exchange' and (sd.Category in ('CAS'))) 
union
(select srv.Name,'services' as [TestName], Case when MIN(srv.result)= 0 then 'Fail'
when MIN(srv.result)= 1 then 'Issue' else 'OK' end as Result,srv.redirectto 
 from (select srvs.Name, srvs.TestName,case when exg.required = 'R' then
case when srvs.result = 'running' then '2' else '0' end 
else case when srvs.result = 'running' then '2' else '1' end end as result,srvs.redirectto
 from (select st.Name, sd.TestName,sd.Result,'ExchangeServerDetailsPage3.aspx?Name=' + st.Name + '&Type=' + st.Type + '&LastDate='+convert(varchar,st.LastUpdate) as redirectto
  from Status st inner join StatusDetail sd 
on st.TypeANDName = sd.TypeAndName where sd.Category in ('Services')) as srvs  inner join ServiceMaster exg
on srvs.TestName = exg.ServiceName) as srv where srv.Name in 
(select st.Name from Status st inner join StatusDetail sd 
on st.TypeANDName = sd.TypeAndName and sd.Category in ('CAS'))
group by srv.Name,srv.redirectto)) as sts
pivot
(
 max(result) for testname in ([SMTP],[RPC],[IMAP],[OWA],[POP3],[Active Sync],[EWS],[Auto Discovery],[Services])
) as pv

end

GO


--Mukund 21Mar14
USE [vitalsigns]
GO

CREATE TABLE [dbo].[DAGStatus](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[DAGName] [nvarchar](50) NOT NULL,
	[TotalMailBoxes] [int] NULL,
	[TotalDatabases] [int] NULL,
	[FileWitnessSereverName] [nvarchar](200) NULL,
	[Status] [nvarchar](200) NULL,
	[FileWitnessServerStatus] [nvarchar](200) NULL,
	[ServerId] [int] NULL,
 CONSTRAINT [PK_DAGStatus] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
ALTER TABLE [dbo].[DAGStatus]  WITH CHECK ADD FOREIGN KEY([ServerId])
REFERENCES [dbo].[Servers] ([ID])
ON DELETE CASCADE
GO

CREATE TABLE [dbo].[DAGMembers](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[ServerName] [nvarchar](200) NOT NULL,
	[DAGID] [int] NULL,
	[ClusterService] [nvarchar](200) NULL,
	[RelayService] [nvarchar](200) NULL,
	[ActiveMgr] [nvarchar](200) NULL,
	[TasksRPCListener] [nvarchar](200) NULL,
	[TPCListner] [nvarchar](200) NULL,
	[DAGMembersUP] [nvarchar](200) NULL,
	[ClusterNetwork] [nvarchar](200) NULL,
	[QuorumGroup] [nvarchar](200) NULL,
	[FileShareQuorum] [nvarchar](200) NULL,
	[DBCopySuspend] [nvarchar](200) NULL,
	[DBDisconnected] [nvarchar](200) NULL,
	[DBLogCopyKeepingUP] [nvarchar](200) NULL,
	[DBLogReplayKeepingUP] [nvarchar](200) NULL,
 CONSTRAINT [PK_DAGMembers] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[DAGMembers]  WITH CHECK ADD  CONSTRAINT [FK_DAGMembers_DAGStatus] FOREIGN KEY([DAGID])
REFERENCES [dbo].[DAGStatus] ([ID])
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[DAGMembers] CHECK CONSTRAINT [FK_DAGMembers_DAGStatus]
GO

CREATE TABLE [dbo].[DAGDatabase](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[DAGMemberId] [int] NOT NULL,
	[DatabaseName] [nvarchar](200) NULL,
	[Activation Preference] [int] NULL,
	[CopyQueue] [nvarchar](200) NULL,
	[ReplayQueue] [nvarchar](200) NULL,
	[ReplayLagged] [nvarchar](200) NULL,
	[TruncationLagged] [nvarchar](200) NULL,
	[ContendIndex] nvarchar(200) NULL,
	[IsActive] nvarchar(50) NULL,
 CONSTRAINT [PK_DAGDatabase] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[DAGDatabase]  WITH CHECK ADD  CONSTRAINT [FK_DAGDatabase_DAGMembers] FOREIGN KEY([DAGMemberId])
REFERENCES [dbo].[DAGMembers] ([ID])
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[DAGDatabase] CHECK CONSTRAINT [FK_DAGDatabase_DAGMembers]
GO


--CY: Created table ServerRoles for server and role mapping

USE VitalSigns
Go
Create Table ServerRoles
( ServerId int not null,
RoleId int not null,
Foreign Key (ServerId) references servers(id) ON DELETE CASCADE,
Foreign Key (RoleId) references RolesMaster(id)
)
Go

USE VitalSigns
Go
CREATE TABLE dbo.MobileUserThreshold(
DeviceId varchar(200) NOT NULL,
SyncTimeThreshold	int
)
Go
CREATE UNIQUE INDEX UX_DeviceId ON [dbo].[MobileUserThreshold] ([DeviceId])

Go

-- VSPLUS-555 The mobile devices traveler bar graph should be consolidated into general categories of devices
CREATE TABLE dbo.DeviceTypeTranslation(
DeviceType varchar(100) NOT NULL,
TranslatedValue	varchar(100) NOT NULL,
OSName varchar(50) NULL
)
go

CREATE UNIQUE INDEX UX_DEVICE_TRANSLATION ON dbo.DeviceTypeTranslation(DeviceType)
GO

CREATE TABLE dbo.OSTypeTranslation(
OSType varchar(100) NOT NULL,
TranslatedValue	varchar(100) NOT NULL,
OSName varchar(50) NULL
)
go

CREATE UNIQUE INDEX UX_OS_TRANSLATION ON dbo.OSTypeTranslation(OSType)
GO

--Mukund 25Apr14, VSPLUS-425: Menu items in dashboard should be database driven...

USE [vitalsigns]
GO

/****** Object:  Table [dbo].[Features]    Script Date: 04/25/2014 11:51:04 ******/

CREATE TABLE [dbo].[Features](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NULL,
 CONSTRAINT [PK_Features] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[FeatureMenus]    Script Date: 04/25/2014 11:51:12 ******/


CREATE TABLE [dbo].[FeatureMenus](
	[FeatureID] [int] NULL,
	[MenuID] [int] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[SelectedFeatures]    Script Date: 04/25/2014 11:51:33 ******/

CREATE TABLE [dbo].[SelectedFeatures](
	[FeatureID] [int] NULL
) ON [PRIMARY]

GO

CREATE TABLE [dbo].[MobileAppUsers](
	[DeviceId] Varchar(255) NULL,
	[OSType] Varchar(100) NULL,
	[DeviceType] Varchar(100) NULL
) ON [PRIMARY]

GO

CREATE VIEW dbo.V_DominoDiskSpace
AS
SELECT * FROM DominoDiskSpace WHERE DiskFree IS NOT NULL AND DiskSize IS NOT NULL
GO

--Mukund 25Jun14, VE-24:Disk Health Page to include Exchange Servers

USE [vitalsigns]
GO

CREATE VIEW [dbo].[V_DiskSpace]
AS
SELECT     ServerName, DiskName, DiskFree, DiskSize, PercentFree, PercentUtilization, AverageQueueLength, Updated, ID, Threshold
FROM         dbo.DiskSpace
WHERE     (DiskFree IS NOT NULL) AND (DiskSize IS NOT NULL)

GO 


USE [vitalsigns]
GO

CREATE FUNCTION dbo.fn_GetVSVersion
/*
			Copyright (c) RPR Wyatt

VERSION:		1.3.5

DESCRIPTION: 		seed the VS_MANAGEMENT table with a version value
			
			Example Use:
			
				
			
DEPENDENCIES:		none

DESIGN:			Dhanraj Seri

MODIFIED:

CUSTOMIZED:

dd MMM YYYY JTM: XXXXX: example mod

*/
( )
RETURNS VARCHAR(20) AS
BEGIN 
DECLARE @sVer AS VARCHAR(250)


SET @sVer = '3.5.0'
RETURN (@sVer)

END
go


USE [vitalsigns]
GO

CREATE TABLE dbo.VS_MANAGEMENT(
	[CATEGORY]		[VARCHAR] (20)	NOT NULL ,
	[VALUE]			[VARCHAR] (20)	NULL ,
	[LAST_UPDATE]	[DATETIME]		NOT NULL ,
	[DESCRIPTION]	[VARCHAR] (254) NULL ,
	[UPDATE_BY]		[VARCHAR] (50)	NULL CONSTRAINT [DF_VS_MANAGEMENT_UPDATE_BY] DEFAULT (suser_sname())
)
go

--Dhiren Added tables for Exchange Mail probe
--VSPLUS-2347 Sowjanya
USE [vitalsigns]
GO
CREATE TABLE [dbo].[ExchangeMailProbe](
		[Enabled] [bit] NULL,
		[Name] [nvarchar](50) NOT NULL,
		[ExchangeMailAddress] [nvarchar](255) NOT NULL,
		[Category] [nvarchar](255) NULL,
		[ScanInterval] [int] NULL,
		[OffHoursScanInterval] [int] NULL,
		[DeliveryThreshold] [int] NULL,
		[RetryInterval] [int] NULL,
		[SourceServerID] [int] NULL,
		[FileName] [nvarchar](255) NULL,
	PRIMARY KEY CLUSTERED 
	(
		[Name] ASC
	)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
	) ON [PRIMARY]

	
	
	ALTER TABLE [dbo].[ExchangeMailProbe]  WITH CHECK ADD  CONSTRAINT [FK_ExchangeMailProbe_Servers] FOREIGN KEY([SourceServerID])
	REFERENCES [dbo].[Servers] ([ID])
	ON DELETE CASCADE
	
	
	ALTER TABLE [dbo].[ExchangeMailProbe] CHECK CONSTRAINT [FK_ExchangeMailProbe_Servers]
GO

CREATE TABLE [dbo].[ExchangeMailProbeHistory](
	[ProbeID] [int] NOT NULL IDENTITY(1,1),
	[SentDateTime] [datetime] NULL,
	[SentTo] [nvarchar](255) NULL,
	[DeliveryThresholdInMinutes] [int] NULL,
	[DeliveryTimeInMinutes] [numeric] (10, 0) NULL,
	[SubjectKey] [nvarchar] (255) NULL,
	[ArrivalAtMailBox] [datetime] NULL,
	[Status] [nvarchar] (255) NULL,
	[Details] [nvarchar] (255) NULL,
	[DeviceName] [nvarchar] (255) NULL,
	[TargetServer] [nvarchar] (200) NULL,
	[TargetDatabase] [nvarchar] (200) NULL,
	PRIMARY KEY (ProbeID)	
) ON [PRIMARY]

GO

/* 8/1/2014 NS added for VSPLUS-846 */
USE [vitalsigns]
GO

/****** Object:  Table [dbo].[LyncServers]    Script Date: 08/01/2014 10:41:11 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[LyncServers](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[ServerID] [int] NULL,
	[Category] [nvarchar](255) NULL,
	[ScanInterval] [int] NULL,
	[RetryInterval] [int] NULL,
	[OffHoursScanInterval] [int] NULL,
	[ResponseThreshold] [int] NULL,
	[FailureThreshold] [int] NULL,
	[MemoryThreshold] [float] NULL,
	[CPUThreshold] [float] NULL,
	[CredentialsID] [int] NULL,
	[Enabled] [bit] NULL,
 CONSTRAINT [PK_LyncServers] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[LyncServers]  WITH CHECK ADD  CONSTRAINT [FK_LyncServers_Credentials] FOREIGN KEY([CredentialsID])
REFERENCES [dbo].[Credentials] ([ID])
GO

ALTER TABLE [dbo].[LyncServers] CHECK CONSTRAINT [FK_LyncServers_Credentials]
GO

ALTER TABLE [dbo].[LyncServers]  WITH CHECK ADD  CONSTRAINT [FK_LyncServers_Servers] FOREIGN KEY([ServerID])
REFERENCES [dbo].[Servers] ([ID])
ON UPDATE CASCADE
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[LyncServers] CHECK CONSTRAINT [FK_LyncServers_Servers]
GO

USE [vitalsigns]
GO

CREATE TABLE [dbo].[WindowsServices](
	[Service_Name] [varchar](500) NULL,
	[Status] [varchar](50) NULL,
	[ServerName] [varchar](150) NULL,
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[StartMode] [varchar](50) NULL,
	[DisplayName] [varchar](max) NULL,
	[Monitored] [bit] NULL,
	[DateStamp] [datetime] NULL,
	[ServerRequired] [bit] NULL,
	[ServerTypeId]	[INT] NULL,
	CONSTRAINT [PK_win_Services] PRIMARY KEY CLUSTERED 
	(
		[ID] ASC
	)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO




USE [vitalsigns]
GO


/****** Object:  Table [dbo].[ExchangeMailboxOverview]    Script Date: 08/18/2014 15:50:31 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[ExchangeMailboxOverview](
	[ServerName] [varchar](50) NULL,
	[DatabaseName] [varchar](50) NULL,
	[SizeMB] [float] NULL,
	[WhiteSpaceMB] [float] NULL,
	[TotalMailboxes] [int] NULL,
	[DisconnectMailboxes] [int] NULL,
	[ConnectedMailboxes] [int] NULL,
	[LastUpdated] [datetime] NULL
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

USE [vitalsigns]
GO

/****** Object:  StoredProcedure [dbo].[ServerCredentials]    Script Date: 08/22/2014 15:56:42 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO



CREATE procedure [dbo].[ServerCredentials](@ServerTypeFilter varchar(100)) as
Begin
declare @SrvLocations Table
(id int, LocId int, Name varchar(100),actid int,tbl varchar(50),srvtypeid int,ServerType varchar(100),description varchar(100))

insert into @SrvLocations select id,null,Location,id,'Locations',null,null,null from (select distinct loc.id, loc.Location from Locations loc inner join servers sr on loc.id = sr.LocationID WHERE sr.ServerTypeID in (1,5)) as tbl

Declare @count int
select @count=MAX(id) from Locations


Declare @ID int
Declare @LocationName varchar(100)
Declare @LocationID int
Declare @ServerTypeId int
Declare @ServerType varchar(100)
Declare @description varchar(100)

if(@ServerTypeFilter = '')
begin
	DECLARE db_cursor CURSOR FOR  
	select sr.ID,sr.ServerName,sr.LocationID,sr.ServerTypeId,srt.ServerType,sr.description from Servers sr,
	ServerTypes srt where sr.ServerTypeId=srt.id 
	order by sr.LocationID,sr.ServerName
end
else
begin
	DECLARE db_cursor CURSOR FOR  
	select sr.ID,sr.ServerName,sr.LocationID,sr.ServerTypeId,srt.ServerType,sr.description from Servers sr,
	ServerTypes srt where sr.ServerTypeId=srt.id and srt.ServerType=@ServerTypeFilter
	order by sr.LocationID,sr.ServerName
end

OPEN db_cursor   
FETCH NEXT FROM db_cursor into @ID,@LocationName,@LocationID,@ServerTypeId,@ServerType,@description

WHILE @@FETCH_STATUS = 0   
BEGIN   
	Set @count=@count+1
	insert into @SrvLocations values(@count,@LocationID,@LocationName,@id,'Servers',@ServerTypeId,@ServerType,@description)
	FETCH NEXT FROM db_cursor into @ID,@LocationName,@LocationID,@ServerTypeId,@ServerType,@description
END   

CLOSE db_cursor   
DEALLOCATE db_cursor

select * from @SrvLocations order by tbl,Name
end


GO



USE [vitalsigns]
GO

/****** Object:  Table [dbo].[Status_History]    Script Date: 09/08/2014 16:46:34 ******/
/* Used to store the Status History from the trigger posted next */
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Status_History](
	[TypeANDName] [nvarchar](255) NULL,
	[OldStatus] [nvarchar](255) NULL,
	[OldStatusCode] [nvarchar](255) NULL,
	[NewStatus] [nvarchar](255) NULL,
	[NewStatusCode] [nvarchar](255) NULL,
	[DateStatusUpdated] [datetime] NULL,
	[ID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO







USE [vitalsigns]
GO

/****** Object:  Trigger [dbo].[StatusBackupUpdate]    Script Date: 09/08/2014 16:43:49 ******/
/* This Trigger stores the statuses to show when a status canges */
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TRIGGER [dbo].[StatusBackupUpdate] ON [dbo].[Status] FOR Update AS
declare @TypeANDName varchar(100),
 @OldStatus varchar(100),
 @OldStatusCode varchar(100),
 @NewStatus varchar(100),
 @NewStatusCode varchar(100),
 @StatusHistoryDate datetime
begin
if UPDATE(STATUS)
BEGIN


SELECT
@TypeANDName = i.TypeANDName,
@NewStatusCode = i.StatusCode, @NewStatus = i.Status
from Inserted i


select @OldStatus=NewStatus, @OldStatusCode=NewStatusCode, @StatusHistoryDate=DateStatusUpdated from 
(select top 1 * from Status_History where TypeANDName=@TypeANDName order by datestatusupdated desc) as tbl

if(@NewStatus = 'Scanning' or @NewStatus = 'Not Scanned' or @NewStatus is null or @NewStatusCode is null)
begin
	/*do nothing */
	/*insert into Status_History (TypeANDName, OldStatus, OldStatusCode, NewStatus, NewStatusCode, DateStatusUpdated) values
		(@TypeANDName + '-test1', @OldStatus, @OldStatusCode, @NewStatus, @NewStatusCode, getdate())*/
	print 'Nothing'
end
else if(@OldStatus = @NewStatus and @OldStatusCode = @NewStatusCode and (Convert(date,@StatusHistoryDate) = Convert(date, getdate()) or @NewStatusCode <> 'Not Responding'))
begin
	/*do nothing again.  Same status*/
	/*insert into Status_History (TypeANDName, OldStatus, OldStatusCode, NewStatus, NewStatusCode, DateStatusUpdated) values
		(@TypeANDName + 'test2', @OldStatus, @OldStatusCode, @NewStatus, @NewStatusCode, getdate())*/
	print 'Nothing Again'
end
else
	insert into Status_History (TypeANDName, OldStatus, OldStatusCode, NewStatus, NewStatusCode, DateStatusUpdated) values
		(@TypeANDName, @OldStatus, @OldStatusCode, @NewStatus, @NewStatusCode, getdate())

end
end


GO




USE [vitalsigns]
GO

/****** Object:  StoredProcedure [dbo].[StatusChanges]    Script Date: 09/08/2014 16:48:55 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE procedure [dbo].[StatusChanges](@time int) as 

declare @Type as varchar(100)
declare @Location as varchar(255)
declare @Name as varchar(100)
declare @Details as varchar(255)
declare @TypeANDName as varchar(255)
declare @LastStatusChange as datetime
declare @NewStatus as varchar(100)
declare @NewStatusCode as varchar(100)
declare @OldStatus as varchar(100)
declare @OldStatusCode as varchar(100)


declare @StatusTable Table
(Type varchar(100), Location varchar(255), Name varchar(100), Details varchar(255), TypeANDName varchar(255),
LastStatusChange datetime, NewStatus varchar(50), NewStatusCode varchar(50), OldStatus varchar(50), OldStatusCode varchar(50))


/* gets entries that are under the threshold, sorts them so NR is at the top and OK at the bottom, then by time since last update*/
insert into @StatusTable 
Select Type,isnull((select Location from Locations where ID=LocationID),Location) as Location,Name,
st.Details,st.TypeandName,sh.DateStatusUpdated as LastStatusChange, sh.NewStatus as NewStatus, sh.NewStatusCode as NewStatusCode,
sh.OldStatus, sh.OldStatusCode
from [VitalSigns].[dbo].[Status] st
inner join
(select  ID,ServerName,LocationID,ServerTypeID FROM Servers union 
SELECT ID, Name,LocationID,ServerTypeID FROM URLs where Enabled=1  union 
select  0,Name,0,13 FROM NotesMailProbe where Enabled=1 union 
select [key],Name,LocationID,ServerTypeID FROM MailServices where Enabled=1 union 
select ID,Name,LocationID,ServerTypeID FROM [Network Devices] where Enabled=1 union 
select  0,Name,0,14 FROM ExchangeMailProbe where Enabled=1) s  on s.ServerName=st.Name AND st.type = (select ServerType from ServerTypes where s.ServerTypeID=ServerTypes.ID)
inner join 
(
select distinct * from [vitalsigns].[dbo].[Status_History] sh1 where DateStatusUpdated = (select max(sh2.DateStatusUpdated) from 
[vitalsigns].[dbo].[Status_History] sh2 where sh1.TypeANDName=sh2.TypeANDName)
) as sh on st.TypeANDName = sh.TypeANDName
where st.Name = s.ServerName and s.ServerTypeID in(select id from ServerTypes stp where stp.ServerType=st.Type) 
and sh.OldStatus is not null and datediff(mi,sh.DateStatusUpdated,getdate()) < @time and sh.DateStatusUpdated = ( Select max(DateStatusUpdated) from Status_History where TypeANDName=st.TypeANDName)
and DateStatusUpdated >= DATEADD(day, -1, GETDATE())
order by (case sh.OldStatusCode when 'Not Responding' then 0
when 'Issue' then 1 
when 'In Maintenance' then 2 
when 'OK' then 3 end), sh.DateStatusUpdated DESC


/*gets the entries that are outside the threshold and sorts them solely based off time*/
insert into @StatusTable 
Select Type,isnull((select Location from Locations where ID=LocationID),Location) as Location,Name,
st.Details,st.TypeandName,sh.DateStatusUpdated as LastStatusChange, sh.NewStatus as NewStatus, sh.NewStatusCode as NewStatusCode,
sh.OldStatus, sh.OldStatusCode
from [VitalSigns].[dbo].[Status] st
inner join 
(select  ID,ServerName,LocationID,ServerTypeID FROM Servers union 
SELECT ID, Name,LocationID,ServerTypeID FROM URLs where Enabled=1  union 
select  0,Name,0,13 FROM NotesMailProbe where Enabled=1 union 
select [key],Name,LocationID,ServerTypeID FROM MailServices where Enabled=1 union 
select ID,Name,LocationID,ServerTypeID FROM [Network Devices] where Enabled=1 union 
select  0,Name,0,14 FROM ExchangeMailProbe where Enabled=1) s  on s.ServerName=st.Name AND st.type = (select ServerType from ServerTypes where s.ServerTypeID=ServerTypes.ID)
inner join 
(
select distinct * from [vitalsigns].[dbo].[Status_History] sh1 where DateStatusUpdated = (select max(sh2.DateStatusUpdated) from 
[vitalsigns].[dbo].[Status_History] sh2 where sh1.TypeANDName=sh2.TypeANDName)
) as sh on st.TypeANDName = sh.TypeANDName
where st.Name = s.ServerName and s.ServerTypeID in(select id from ServerTypes stp where stp.ServerType=st.Type) 
and sh.OldStatus is not null and datediff(mi,sh.DateStatusUpdated,getdate()) > @time and sh.DateStatusUpdated = ( Select max(DateStatusUpdated) from Status_History where TypeANDName=st.TypeANDName)
and DateStatusUpdated >= DATEADD(day, -1, GETDATE())
order by sh.DateStatusUpdated DESC


select * from @StatusTable


GO


--Mukund 30Sep14, Generic Windows related
USE [vitalsigns] 
GO
CREATE TABLE [dbo].[WindowsServers](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[ServerID] [int] NULL,
	[Category] [nvarchar](255) NULL,
	[ScanInterval] [int] NULL,
	[RetryInterval] [int] NULL,
	[OffHoursScanInterval] [int] NULL,
	[ResponseThreshold] [int] NULL,
	[FailureThreshold] [int] NULL,
	[MemoryThreshold] [float] NULL,
	[CPUThreshold] [float] NULL,
	[CredentialsID] [int] NULL,
	[Enabled] [bit] NULL
) ON [PRIMARY]
Go

/*VE-123 06Oct14 Mukund added*/

CREATE TABLE [dbo].[DagSettings](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[ServerID] [int] NOT NULL,
	[PrimaryConnection] [nvarchar](max) NULL,
	[BackupConnection] [nvarchar](max) NULL,
	[ReplyQThreshold] [int] NULL,
	[CopyQThreshold] [int] NULL,
 CONSTRAINT [PK_DagSettings] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
--VSPLUS-970 Mukund added 07Oct14
CREATE TABLE [dbo].[CloudMaster](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](max) NULL,
	[Image] [nvarchar](100) NULL,
	[Url] [nvarchar](max) NULL,
 CONSTRAINT [PK_CloudDetails] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

CREATE TABLE [dbo].[CloudDetails](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](255) NULL,
	[Category] [nvarchar](255) NULL,
	[ScanInterval] [int] NULL,
	[OffHoursScanInterval] [int] NULL,
	[NextScan] [datetime] NULL,
	[LastChecked] [datetime] NULL,
	[LastStatus] [nvarchar](50) NULL,
	[Enabled] [bit] NULL,
	[ResponseThreshold] [int] NULL,
	[RetryInterval] [int] NULL,
	[SearchString] [nvarchar](255) NULL,
	[AlertStringFound] [nvarchar](255) NULL,
	[UserName] [nvarchar](255) NULL,
	[PW] [nvarchar](255) NULL,
	[Location] [nvarchar](255) NULL,
	[ServerTypeId] [int] NULL,
	[FailureThreshold] [int] NULL,
	[MonitoredBy] [int] NULL,
	[Imageurl] [nvarchar](100) NULL,
	[Url] [nvarchar](50) NULL,
 CONSTRAINT [PK_CloudApplicationsMaster] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]


/******Mukund:21-Oct-14, Object:  Table [dbo].[ActiveDirectoryTest]    Script Date: 10/20/2014 17:50:55 ******/


CREATE TABLE [dbo].[ActiveDirectoryTest](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[ServerID] [int] NULL,
	[LogOnTest] [nvarchar](200) NULL,
	[QueryTest] [nvarchar](200) NULL,
	[LDAPPortTest] [nvarchar](200) NULL,
	[DNS] [nvarchar](200) NULL,
	[DomainController] [nvarchar](200) NULL,
	[ADMembersUP] [nvarchar](200) NULL,
	[ClusterNetwork] [nvarchar](200) NULL,
	[Advertising] [nvarchar](200) NULL,
	[FrsSysVol] [nvarchar](200) NULL,
	[Replications] [nvarchar](200) NULL,
	[Services] [nvarchar](200) NULL,
	[FsmoCheck] [nvarchar](200) NULL,
	[LastScanDate] [datetime] NULL,
 CONSTRAINT [PK_ActiveDirectoryTest] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO


--WS 10/21/14 VE-143

USE [vitalsigns]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[DAGQueueThresholds](
	[DAGName] [nvarchar](50) NOT NULL,
	[ServerName] [nvarchar](50) NOT NULL,
	[ServerTypeId] [nvarchar](50) NOT NULL,
	[DatabaseName] [nvarchar](50) NOT NULL,
	[ReplayQueueThreshold] [nvarchar](50) NOT NULL,
	[CopyQueueThreshold] [nvarchar](50) NOT NULL
) ON [PRIMARY]

GO

/* 10/21/2014 NS added for VSPLUS-730 */
USE [vitalsigns]
GO

/****** Object:  Table [dbo].[AlertRepeatOccurrence]    Script Date: 10/20/2014 20:28:56 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[AlertRepeatOccurrence](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[DeviceName] [nvarchar](50) NOT NULL,
	[DeviceType] [nvarchar](50) NOT NULL,
	[AlertType] [nvarchar](255) NOT NULL,
	[DateCreated] [datetime] NOT NULL,
 CONSTRAINT [PK_AlertRepeatOccurrence] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

CREATE TABLE [dbo].[DagDatabaseDetails](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[DAGID] [int] NOT NULL,
	[DatabaseName] [nvarchar](50) NOT NULL,
	[StorageGroup] [nvarchar](50) NULL,
	[Mounted] [bit] NULL,
	[BackupInProgress] [bit] NULL,
	[OnlineMaintInProg] [bit] NULL,
	[LastFullBackup] [datetime] NULL,
	[LastIncrementalBackup] [datetime] NULL,
	[LastDifferentialBackup] [datetime] NULL,
	[LastCopyBackup] [datetime] NULL
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[DagDatabaseDetails]  WITH CHECK ADD FOREIGN KEY([DAGID])
REFERENCES [dbo].[DAGStatus] ([ID])
ON DELETE CASCADE

GO

-- 11/12/2014 Mouli VSPLUS 1119--
USE [vitalsigns]
GO
/****** Object:  Table [dbo].[SharePointSettings]    Script Date: 11/12/2014 11:34:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SharePointSettings](
	[ServerId] [int]  NOT NULL,
	[Sname] [nvarchar](250) NULL,
	[Svalue] [nvarchar](250) NULL
)
GO


/****** 11/20/2014 Swathi VSPLUS 1081 ******/
USE [vitalsigns]
GO
/****** Object:  Table [dbo].[WindowsStatus]    Script Date: 11/20/2014 12:30:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
 IF NOT EXISTS (select * from syscolumns where id = object_id('dbo.WindowsStatus'))
CREATE TABLE [dbo].[WindowsStatus](
	[ServerID] [int] NULL,
	[Sname] [nvarchar](200) NULL,
	[Svalue] [nvarchar](200) NULL
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[ActiveDirectoryReplicationSummary](
	[ServerName] [nvarchar](50) NOT NULL,
	[SourceServer] [nvarchar](50) NOT NULL,
	[LargestDelta] [nvarchar](50) NOT NULL,
	[Fails] [nvarchar](50) NOT NULL,
	[DirectoryPartitions] [nvarchar](50) NOT NULL,
	[LastScanTime] [datetime] NOT NULL
) ON [PRIMARY]

GO


CREATE TABLE [dbo].[Office365AccountStats](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[ServerID] [int] NULL,
	[TotalActiveUserMailBoxes] [int] NULL,
	[AccountName] [nvarchar](200) NULL,
	[ActiveUnits] [int] NULL,
	[WarningUnits] [int] NULL,
	[ConsumedUnits] [int] NULL,
	[LicenseType] [nvarchar](200) NULL,
	[PrimaryDomain] [nvarchar](200) NULL,
	[Office365Version] [nvarchar](50) NULL,
	[CompanyDisplayName] [nvarchar](100) NULL,
	[PreferredLanguage] [nvarchar](20) NULL,
	[Street] [nvarchar](200) NULL,
	[City] [nvarchar](200) NULL,
	[State] [nvarchar](200) NULL,
	[PostalCode] [nvarchar](20) NULL,
	[Country] [nvarchar](200) NULL,
	[Telephone] [nvarchar](200) NULL,
	[TechnicalNotificationEmails] [nvarchar](200) NULL,
	[LastUpdatedDate] [datetime] NULL,
 CONSTRAINT [PK_Office365AccountStats] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO


USE [VitalSigns]
GO


CREATE TABLE [dbo].[O365Server](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](255) NULL,
	[Category] [nvarchar](255) NULL,
	[ScanInterval] [int] NULL,
	[OffHoursScanInterval] [int] NULL,
	[NextScan] [datetime] NULL,
	[LastChecked] [datetime] NULL,
	[LastStatus] [nvarchar](50) NULL,
	[Enabled] [bit] NULL,
	[ResponseThreshold] [int] NULL,
	[RetryInterval] [int] NULL,
	[SearchString] [nvarchar](255) NULL,
	[AlertStringFound] [nvarchar](255) NULL,
	[UserName] [nvarchar](255) NULL,
	[PW] [nvarchar](255) NULL,
	[Location] [nvarchar](255) NULL,
	[ServerTypeId] [int] NULL,
	[FailureThreshold] [int] NULL,
	[MonitoredBy] [int] NULL,
	[Imageurl] [nvarchar](50) NULL,
	[Url] [nvarchar](50) NULL,
	[Mode] [nvarchar](50) NULL,
    [ServerName] [nvarchar](100) NULL,
    [CredentialsId] [int] NULL,
    [Costperuser] [int] NULL,
 CONSTRAINT [PK_O365Master] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

/* Table  [dbo].[TestsMaster]*/
USE [vitalsigns]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TestsMaster](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ServerType] [int] NULL,
	[Tests] [nvarchar](50) NULL,
	[Type] [nvarchar](50) NULL
	--SWATHI VSPLUS 2226
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

/* Sowjanya 1311,1314 */
/* Table  [dbo].[Office365Tests]*/
/* Somaraju VSPLUS-2607 */
USE [vitalsigns]
GO
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[Office365Tests](
	[Id] [int] NOT NULL IDENTITY(1,1),
	[ServerId] [int] NULL,
	[EnableSimulationTests] [bit] NULL,
	[ResponseThreshold] [int] NULL,
	[Type] [varchar](254) NULL,
	[Tests] [varchar](254) NULL,
 CONSTRAINT [PK_Office365Tests] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

/*WS ADDED FOR SHAREPOINT CHANGES*/
USE [VitalSigns]
GO

CREATE TABLE [dbo].[SharePointFarms](
	[ServerId] [int] NOT NULL,
	[Farm] [varchar](255) NOT NULL,
	[LastUpdated] [varchar](255) NOT NULL
) ON [PRIMARY]

GO

CREATE TABLE [dbo].[SharePointDatabaseDetails](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[WebAppName] [varchar](255) NOT NULL,
	[DatabaseName] [varchar](255) NOT NULL,
	[DatabaseID] [varchar](255) NOT NULL,
	[DatabaseSiteCount] [int] NOT NULL,
	[MaxSiteCountThreshold] [int] NOT NULL,
	[WarningSiteCountThreshold] [int] NOT NULL,
	[IsDBReadOnly] [bit] NOT NULL,
	[DatabaseServer] [varchar](255) NOT NULL,
 CONSTRAINT [PK_SharePointDatabaseDetails] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

/* 12/18/2014 NS added for VSPLUS-1238*/
USE [vitalsigns]
GO

/****** Object:  Table [dbo].[DeviceInventory]    Script Date: 12/08/2014 15:34:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[DeviceInventory](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](150) NOT NULL,
	[DeviceID] [int] NULL,
	[DeviceTypeID] [int] NOT NULL,
	[LocationID] [int] NULL,
	[AssignedNodeId] [int] NULL,
	[CurrentNodeId] [int] NULL,
 CONSTRAINT [PK_DeviceInventory] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

CREATE UNIQUE INDEX IX_DI_NAME_ID_TYPE ON [dbo].[DeviceInventory] ([Name],[DeviceId],[DeviceTypeID])
GO

SET ANSI_PADDING OFF
GO

/* 12/18/2014 NS added for VSPLUS-1238 */
/* BEGIN - creating triggers for the device tables */
USE [vitalsigns]
GO

/****** Object:  Trigger [dbo].[tr_DeleteDeviceInventoryCloudDetails]    Script Date: 12/09/2014 12:18:23 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Natallya Shkarayeva
-- Create date: 12/18/2014
-- Description:	Delete trigger will update the DeviceInventory table
-- =============================================
CREATE TRIGGER [dbo].[tr_DeleteDeviceInventoryCloudDetails]
   ON  [dbo].[CloudDetails]
   FOR DELETE
AS 
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	DELETE childTable FROM DeviceInventory childTable
    WHERE exists (SELECT id FROM deleted where deleted.id=childTable.DeviceID and 
    childTable.DeviceTypeID=17)

END

GO
USE [vitalsigns]
GO

/****** Object:  Trigger [dbo].[tr_InsertDeviceInventoryCloudDetails]    Script Date: 12/09/2014 12:18:45 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Natallya Shkarayeva
-- Create date: 12/18/2014
-- Description:	Insert trigger will update the DeviceInventory table
-- =============================================
CREATE TRIGGER [dbo].[tr_InsertDeviceInventoryCloudDetails]
   ON  [dbo].[CloudDetails]
   FOR INSERT
AS 
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    INSERT INTO DeviceInventory
        (Name, DeviceID, DeviceTypeID, LocationID)
    SELECT
        Name, ID, 17, NULL
        FROM inserted

END

GO
USE [vitalsigns]
GO

/****** Object:  Trigger [dbo].[tr_UpdateDeviceInventoryCloudDetails]    Script Date: 12/09/2014 12:19:11 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Natallya Shkarayeva
-- Create date: 12/18/2014
-- Description:	Update trigger will update the DeviceInventory table
-- Modified date: 4/27/2015 by Natallya Shkarayeva
-- Modification description: when Enabled changes, either add a new record or delete an existing one
-- from DeviceInventory
-- =============================================
CREATE TRIGGER [dbo].[tr_UpdateDeviceInventoryCloudDetails]
   ON  [dbo].[CloudDetails]
   AFTER UPDATE
AS 
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	IF UPDATE (Name)
	BEGIN
		UPDATE DeviceInventory
		SET    DeviceInventory.Name = Inserted.Name
		FROM   DeviceInventory  JOIN inserted ON DeviceInventory.DeviceID = inserted.ID
		AND  DeviceInventory.DeviceTypeID=17
	END
	
	IF UPDATE([Enabled])
	BEGIN
		IF (SELECT [Enabled] FROM Deleted) = 0 AND (SELECT [Enabled] FROM Inserted) = 1
		BEGIN
			INSERT INTO DeviceInventory
			(Name, DeviceID, DeviceTypeID, LocationID)
			SELECT
				Name, ID, 17, NULL
			FROM inserted
				WHERE inserted.[Enabled] = 1	
		END
		ELSE
		 IF (SELECT [Enabled] FROM Inserted) = 0 AND (SELECT [Enabled] FROM Deleted) = 1
		 BEGIN
			DELETE d FROM DeviceInventory AS d INNER JOIN Inserted AS i ON d.DeviceID = i.ID
			AND  d.DeviceTypeID=17 AND i.[Enabled] = 0
		 END
	END
END

GO
USE [vitalsigns]
GO

/****** Object:  Trigger [dbo].[tr_DeleteDeviceInventoryExchangeMailProbe]    Script Date: 12/09/2014 12:27:13 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Natallya Shkarayeva
-- Create date: 12/18/2014
-- Description:	Delete trigger will update the DeviceInventory table
-- =============================================
CREATE TRIGGER [dbo].[tr_DeleteDeviceInventoryExchangeMailProbe]
   ON [dbo].[ExchangeMailProbe]
   FOR DELETE
AS 
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    DELETE childTable FROM DeviceInventory childTable
    WHERE exists (SELECT Name FROM deleted where deleted.Name=childTable.Name and 
    childTable.DeviceTypeID=14)
END

GO
USE [vitalsigns]
GO

/****** Object:  Trigger [dbo].[tr_InsertDeviceInventoryExchangeMailProbe]    Script Date: 12/09/2014 12:27:38 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Natallya Shkarayeva
-- Create date: 12/18/2014
-- Description:	Insert trigger will update the DeviceInventory table
-- =============================================
CREATE TRIGGER [dbo].[tr_InsertDeviceInventoryExchangeMailProbe]
   ON  [dbo].[ExchangeMailProbe]
   FOR INSERT
AS 
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    INSERT INTO DeviceInventory
        (Name, DeviceID, DeviceTypeID, LocationID)
    SELECT
        Name, NULL, 14, NULL
        FROM inserted

END

GO
USE [vitalsigns]
GO

/****** Object:  Trigger [dbo].[tr_UpdateDeviceInventoryExchangeMailProbe]    Script Date: 12/09/2014 12:27:58 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Natallya Shkarayeva
-- Create date: 12/18/2014
-- Description:	Update trigger will update the DeviceInventory table
-- Modified date: 4/27/2015 by Natallya Shkarayeva
-- Modification description: when Enabled changes, either add a new record or delete an existing one
-- from DeviceInventory
-- =============================================
CREATE TRIGGER [dbo].[tr_UpdateDeviceInventoryExchangeMailProbe]
   ON  [dbo].[ExchangeMailProbe]
   AFTER UPDATE
AS 
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	IF UPDATE (Name)
	BEGIN
		UPDATE DeviceInventory
		SET    DeviceInventory.Name = (SELECT Inserted.Name FROM Inserted)
		FROM   DeviceInventory  JOIN deleted ON DeviceInventory.Name = deleted.Name 
		WHERE  DeviceInventory.DeviceTypeID=14
	END
	
	IF UPDATE([Enabled])
	BEGIN
		IF (SELECT [Enabled] FROM Deleted) = 0 AND (SELECT [Enabled] FROM Inserted) = 1
		BEGIN
			INSERT INTO DeviceInventory
			(Name, DeviceID, DeviceTypeID, LocationID)
			SELECT
				Name, NULL, 14, NULL
			FROM inserted
				WHERE inserted.[Enabled] = 1	
		END
		ELSE
		 IF (SELECT [Enabled] FROM Inserted) = 0 AND (SELECT [Enabled] FROM Deleted) = 1
		 BEGIN
			DELETE d FROM DeviceInventory AS d INNER JOIN Inserted AS i ON d.Name = i.Name
			AND  d.DeviceTypeID=14 AND i.[Enabled] = 0
		 END
	END
END

GO
USE [vitalsigns]
GO

/****** Object:  Trigger [dbo].[tr_DeleteDeviceInventoryMailServices]    Script Date: 12/09/2014 12:13:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Natallya Shkarayeva
-- Create date: 12/18/2014
-- Description:	Delete trigger will update the DeviceInventory table
-- =============================================
CREATE TRIGGER [dbo].[tr_DeleteDeviceInventoryMailServices]
   ON  [dbo].[MailServices]
   FOR DELETE
AS 
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    DELETE childTable FROM DeviceInventory childTable
    WHERE exists (SELECT [key] FROM deleted where deleted.[key]=childTable.DeviceID and 
    childTable.DeviceTypeID=6)

END

GO
USE [vitalsigns]
GO

/****** Object:  Trigger [dbo].[tr_InsertDeviceInventoryMailServices]    Script Date: 12/09/2014 12:13:31 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Natallya Shkarayeva
-- Create date: 12/18/2014
-- Description:	Insert trigger will update the DeviceInventory table
-- =============================================
CREATE TRIGGER [dbo].[tr_InsertDeviceInventoryMailServices]
   ON  [dbo].[MailServices]
   FOR INSERT
AS 
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    INSERT INTO DeviceInventory
        (Name, DeviceID, DeviceTypeID, LocationID)
    SELECT
        Name, [key], 6, NULL
        FROM inserted

END

GO
USE [vitalsigns]
GO

/****** Object:  Trigger [dbo].[tr_UpdateDeviceInventoryMailServices]    Script Date: 12/09/2014 12:13:51 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Natallya Shkarayeva
-- Create date: 12/18/2014
-- Description:	Update trigger will update the DeviceInventory table
-- Modified date: 4/27/2015 by Natallya Shkarayeva
-- Modification description: when Enabled changes, either add a new record or delete an existing one
-- from DeviceInventory
-- =============================================
CREATE TRIGGER [dbo].[tr_UpdateDeviceInventoryMailServices]
   ON  [dbo].[MailServices]
   AFTER UPDATE
AS 
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	IF UPDATE (Name)
	BEGIN
		UPDATE DeviceInventory
		SET    DeviceInventory.Name = Inserted.Name
		FROM   DeviceInventory  JOIN inserted ON DeviceInventory.DeviceID = inserted.[key]
		AND  DeviceInventory.DeviceTypeID=6
	END
	
	IF UPDATE([Enabled])
	BEGIN
		IF (SELECT [Enabled] FROM Deleted) = 0 AND (SELECT [Enabled] FROM Inserted) = 1
		BEGIN
			INSERT INTO DeviceInventory
			(Name, DeviceID, DeviceTypeID, LocationID)
			SELECT
				Name, [key], 6, NULL
			FROM inserted
				WHERE inserted.[Enabled] = 1	
		END
		ELSE
		 IF (SELECT [Enabled] FROM Inserted) = 0 AND (SELECT [Enabled] FROM Deleted) = 1
		 BEGIN
			DELETE d FROM DeviceInventory AS d INNER JOIN Inserted AS i ON d.DeviceID = i.[key]
			AND  d.DeviceTypeID=6 AND i.[Enabled] = 0
		 END
	END
END

GO
USE [vitalsigns]
GO

/****** Object:  Trigger [dbo].[tr_DeleteDeviceInventoryMobileUserThreshold]    Script Date: 12/09/2014 12:11:39 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Natallya Shkarayeva
-- Create date: 12/18/2014
-- Description:	Delete trigger will update the DeviceInventory table
-- =============================================
CREATE TRIGGER [dbo].[tr_DeleteDeviceInventoryMobileUserThreshold]
   ON  [dbo].[MobileUserThreshold]
   FOR DELETE
AS 
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    DELETE childTable FROM DeviceInventory childTable
    WHERE exists (SELECT UserName+'-'+deleted.DeviceID  FROM deleted,Traveler_Devices td where deleted.DeviceId=td.DeviceID and 
    childTable.DeviceTypeID=11)

END

GO
USE [vitalsigns]
GO

/****** Object:  Trigger [dbo].[tr_InsertDeviceInventoryMobileUserThreshold]    Script Date: 12/09/2014 12:12:10 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Natallya Shkarayeva
-- Create date: 12/18/2014
-- Description:	Delete trigger will update the DeviceInventory table
-- =============================================
CREATE TRIGGER [dbo].[tr_InsertDeviceInventoryMobileUserThreshold]
   ON  [dbo].[MobileUserThreshold]
   FOR INSERT
AS 
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    INSERT INTO DeviceInventory
        (Name, DeviceID, DeviceTypeID, LocationID)
    SELECT
        top 1 td.UserName+'-'+td.DeviceID, NULL, 11, NULL
        FROM inserted i,Traveler_Devices  td where td.DeviceID=i.DeviceId

END

GO

USE [vitalsigns]
GO

/****** Object:  Trigger [dbo].[tr_UpdateDeviceInventoryMobileUserThreshold]    Script Date: 12/09/2014 12:12:35 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Natallya Shkarayeva
-- Create date: 12/18/2014
-- Description:	Update trigger will update the DeviceInventory table
-- =============================================
CREATE TRIGGER [dbo].[tr_UpdateDeviceInventoryMobileUserThreshold]
   ON  [dbo].[MobileUserThreshold]
   AFTER UPDATE
AS 
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	IF UPDATE (DeviceId)
	BEGIN
		UPDATE DeviceInventory
		SET    DeviceInventory.Name = TD.UserName+'-'+TD.DeviceID  
		FROM   DeviceInventory  JOIN inserted ON DeviceInventory.Name = inserted.DeviceId
		AND  DeviceInventory.DeviceTypeID=11
		INNER JOIN Traveler_Devices TD ON inserted.DeviceId =TD.DeviceID 
	END
END

GO
USE [vitalsigns]
GO

/****** Object:  Trigger [dbo].[tr_DeleteDeviceInventoryNetworkDevices]    Script Date: 12/09/2014 11:27:45 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Natallya Shkarayeva
-- Create date: 12/18/2014
-- Description:	Delete trigger will update the DeviceInventory table
-- =============================================
CREATE TRIGGER [dbo].[tr_DeleteDeviceInventoryNetworkDevices]
   ON  [dbo].[Network Devices]
   FOR DELETE
AS 
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    DELETE childTable FROM DeviceInventory childTable
    WHERE exists (SELECT id FROM deleted where deleted.id=childTable.DeviceID and 
    childTable.DeviceTypeID=8)

END

GO
USE [vitalsigns]
GO

/****** Object:  Trigger [dbo].[tr_InsertDeviceInventoryNetworkDevices]    Script Date: 12/08/2014 17:42:31 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Natallya Shkarayeva
-- Create date: 12/18/2014
-- Description:	Delete trigger will update the DeviceInventory table
-- =============================================
CREATE TRIGGER [dbo].[tr_InsertDeviceInventoryNetworkDevices]
   ON  [dbo].[Network Devices]
   FOR INSERT
AS 
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    INSERT INTO DeviceInventory
        (Name, DeviceID, DeviceTypeID, LocationID)
    SELECT
        Name, ID, 8, NULL
        FROM inserted

END

GO
USE [vitalsigns]
GO

/****** Object:  Trigger [dbo].[tr_UpdateDeviceInventoryNetworkDevices]    Script Date: 12/08/2014 18:04:50 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Natallya Shkarayeva
-- Create date: 12/18/2014
-- Description:	Update trigger will update the DeviceInventory table
-- Modified date: 4/27/2015 by Natallya Shkarayeva
-- Modification description: when Enabled changes, either add a new record or delete an existing one
-- from DeviceInventory
-- =============================================
CREATE TRIGGER [dbo].[tr_UpdateDeviceInventoryNetworkDevices]
   ON  [dbo].[Network Devices]
   AFTER UPDATE
AS 
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	IF UPDATE (Name)
	BEGIN
		UPDATE DeviceInventory
		SET    DeviceInventory.Name = Inserted.Name
		FROM   DeviceInventory  JOIN inserted ON DeviceInventory.DeviceID = inserted.ID
		AND  DeviceInventory.DeviceTypeID=8
	END
	
	IF UPDATE([Enabled])
	BEGIN
		IF (SELECT [Enabled] FROM Deleted) = 0 AND (SELECT [Enabled] FROM Inserted) = 1
		BEGIN
			INSERT INTO DeviceInventory
			(Name, DeviceID, DeviceTypeID, LocationID)
			SELECT
				Name, ID, 8, NULL
			FROM inserted
				WHERE inserted.[Enabled] = 1	
		END
		ELSE
		 IF (SELECT [Enabled] FROM Inserted) = 0 AND (SELECT [Enabled] FROM Deleted) = 1
		 BEGIN
			DELETE d FROM DeviceInventory AS d INNER JOIN Inserted AS i ON d.DeviceID = i.ID
			AND  d.DeviceTypeID=8 AND i.[Enabled] = 0
		 END
	END
END

GO
USE [vitalsigns]
GO

/****** Object:  Trigger [dbo].[tr_DeleteDeviceInventoryNotesDatabases]    Script Date: 12/09/2014 11:28:12 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Natallya Shkarayeva
-- Create date: 12/18/2014
-- Description:	Delete trigger will update the DeviceInventory table
-- =============================================
CREATE TRIGGER [dbo].[tr_DeleteDeviceInventoryNotesDatabases]
   ON  [dbo].[NotesDatabases]
   FOR DELETE
AS 
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    DELETE childTable FROM DeviceInventory childTable
    WHERE exists (SELECT id FROM deleted where deleted.id=childTable.DeviceID and 
    childTable.DeviceTypeID=9)

END

GO
USE [vitalsigns]
GO

/****** Object:  Trigger [dbo].[tr_InsertDeviceInventoryNotesDatabases]    Script Date: 12/08/2014 16:42:51 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Natallya Shkarayeva
-- Create date: 12/18/2014
-- Description:	Insert trigger will update the DeviceInventory table
-- =============================================
CREATE TRIGGER [dbo].[tr_InsertDeviceInventoryNotesDatabases]
   ON  [dbo].[NotesDatabases]
   FOR INSERT
AS 
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	INSERT INTO DeviceInventory
        (Name, DeviceID, DeviceTypeID, LocationID)
    SELECT
        Name, ID, 9, NULL
        FROM inserted
END

GO
USE [vitalsigns]
GO

/****** Object:  Trigger [dbo].[tr_UpdateDeviceInventoryNotesDatabases]    Script Date: 12/08/2014 15:32:30 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Natallya Shkarayeva
-- Create date: 12/18/2014
-- Description:	Update trigger will update the DeviceInventory table
-- Modified date: 4/27/2015 by Natallya Shkarayeva
-- Modification description: when Enabled changes, either add a new record or delete an existing one
-- from DeviceInventory
-- =============================================
CREATE TRIGGER [dbo].[tr_UpdateDeviceInventoryNotesDatabases]
   ON  [dbo].[NotesDatabases]
   AFTER UPDATE
AS 
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	IF UPDATE (Name)
	BEGIN
		UPDATE DeviceInventory
		SET    DeviceInventory.Name = Inserted.Name
		FROM   DeviceInventory  JOIN inserted ON DeviceInventory.DeviceID = inserted.ID
		AND  DeviceInventory.DeviceTypeID=9
	End	
	
	IF UPDATE([Enabled])
	BEGIN
		IF (SELECT [Enabled] FROM Deleted) = 0 AND (SELECT [Enabled] FROM Inserted) = 1
		BEGIN
			INSERT INTO DeviceInventory
			(Name, DeviceID, DeviceTypeID, LocationID)
			SELECT
				Name, ID, 9, NULL
			FROM inserted
				WHERE inserted.[Enabled] = 1	
		END
		ELSE
		 IF (SELECT [Enabled] FROM Inserted) = 0 AND (SELECT [Enabled] FROM Deleted) = 1
		 BEGIN
			DELETE d FROM DeviceInventory AS d INNER JOIN Inserted AS i ON d.DeviceID = i.ID
			AND  d.DeviceTypeID=9 AND i.[Enabled] = 0
		 END
	END
END

GO
USE [vitalsigns]
GO

/****** Object:  Trigger [dbo].[tr_DeleteDeviceInventoryNotesMailProbe]    Script Date: 12/09/2014 12:26:43 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Natallya Shkarayeva
-- Create date: 12/18/2014
-- Description:	Delete trigger will update the DeviceInventory table
-- =============================================
CREATE TRIGGER [dbo].[tr_DeleteDeviceInventoryNotesMailProbe]
   ON  [dbo].[NotesMailProbe]
   FOR DELETE
AS 
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    DELETE childTable FROM DeviceInventory childTable
    WHERE exists (SELECT Name FROM deleted where deleted.Name=childTable.Name and 
    childTable.DeviceTypeID=13)

END

GO
USE [vitalsigns]
GO

/****** Object:  Trigger [dbo].[tr_InsertDeviceInventoryNotesMailProbe]    Script Date: 12/08/2014 18:03:49 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Natallya Shkarayeva
-- Create date: 12/18/2014
-- Description:	Insert trigger will update the DeviceInventory table
-- =============================================
CREATE TRIGGER [dbo].[tr_InsertDeviceInventoryNotesMailProbe]
   ON  [dbo].[NotesMailProbe]
   FOR INSERT
AS 
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    INSERT INTO DeviceInventory
        (Name, DeviceID, DeviceTypeID, LocationID)
    SELECT
        Name, NULL, 13, NULL
        FROM inserted

END

GO
USE [vitalsigns]
GO

/****** Object:  Trigger [dbo].[tr_UpdateDeviceInventoryNotesMailProbe]    Script Date: 12/08/2014 15:32:58 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Natallya Shkarayeva
-- Create date: 12/18/2014
-- Description:	Update trigger will update the DeviceInventory table
-- Modified date: 4/27/2015 by Natallya Shkarayeva
-- Modification description: when Enabled changes, either add a new record or delete an existing one
-- from DeviceInventory
-- =============================================
CREATE TRIGGER [dbo].[tr_UpdateDeviceInventoryNotesMailProbe]
   ON  [dbo].[NotesMailProbe]
   AFTER UPDATE
AS 
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    IF UPDATE (Name)
    BEGIN
		UPDATE DeviceInventory
		SET    DeviceInventory.Name = (SELECT Name from Inserted)
		FROM   DeviceInventory  JOIN deleted ON DeviceInventory.Name = deleted.Name 
		WHERE  DeviceInventory.DeviceTypeID=13
	END
	
	IF UPDATE([Enabled])
	BEGIN
		IF (SELECT [Enabled] FROM Deleted) = 0 AND (SELECT [Enabled] FROM Inserted) = 1
		BEGIN
			INSERT INTO DeviceInventory
			(Name, DeviceID, DeviceTypeID, LocationID)
			SELECT
				Name, NULL, 13, NULL
			FROM inserted
				WHERE inserted.[Enabled] = 1	
		END
		ELSE
		 IF (SELECT [Enabled] FROM Inserted) = 0 AND (SELECT [Enabled] FROM Deleted) = 1
		 BEGIN
			DELETE d FROM DeviceInventory AS d INNER JOIN Inserted AS i ON d.Name = i.Name
			AND  d.DeviceTypeID=13 AND i.[Enabled] = 0
		 END
	END
END

GO
USE [vitalsigns]
GO

/****** Object:  Trigger [dbo].[tr_DeleteDeviceInventoryServers]    Script Date: 12/09/2014 11:29:12 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Natallya Shkarayeva
-- Create date: 12/18/2014
-- Description:	Delete trigger will update the DeviceInventory table
-- =============================================
CREATE TRIGGER [dbo].[tr_DeleteDeviceInventoryServers]
   ON  [dbo].[Servers]
   FOR DELETE
AS 
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
    
    DELETE childTable FROM DeviceInventory childTable
    WHERE exists (SELECT id FROM deleted where deleted.id=childTable.DeviceID and 
    deleted.ServerTypeID=childTable.DeviceTypeID)

END

GO
USE [vitalsigns]
GO

/****** Object:  Trigger [dbo].[tr_InsertDeviceInventoryServers]    Script Date: 12/08/2014 17:27:32 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Natallya Shkarayeva
-- Create date: 12/18/2014
-- Description:	Insert trigger will update the DeviceInventory table
-- =============================================
CREATE TRIGGER [dbo].[tr_InsertDeviceInventoryServers]
   ON  [dbo].[Servers]
   FOR INSERT
AS 
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    INSERT INTO DeviceInventory
        (Name, DeviceID, DeviceTypeID, LocationID)
    SELECT
        ServerName, ID, ServerTypeID, LocationID
        FROM inserted

END

GO
USE [vitalsigns]
GO

/****** Object:  Trigger [dbo].[tr_UpdateDeviceInventoryServers]    Script Date: 12/09/2014 13:01:07 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Natallya Shkarayeva
-- Create date: 12/18/2014
-- Description:	Update trigger will update the DeviceInventory table
-- =============================================
CREATE TRIGGER [dbo].[tr_UpdateDeviceInventoryServers]
   ON  [dbo].[Servers]
   AFTER UPDATE
AS 
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
		
	IF (UPDATE (ServerName) or UPDATE (LocationID))
	BEGIN
		UPDATE DeviceInventory
		SET    DeviceInventory.Name = Inserted.ServerName, DeviceInventory.LocationID = Inserted.LocationId
		FROM   DeviceInventory  JOIN inserted ON DeviceInventory.DeviceID = inserted.ID
		AND  DeviceInventory.DeviceTypeID=inserted.ServerTypeID
	END
END
GO

USE [vitalsigns]
GO

/****** Object:  Trigger [dbo].[tr_DeleteDeviceInventoryURLs]    Script Date: 12/09/2014 12:34:59 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Natallya Shkarayeva
-- Create date: 12/18/2014
-- Description:	Delete trigger will update the DeviceInventory table
-- =============================================
CREATE TRIGGER [dbo].[tr_DeleteDeviceInventoryURLs]
   ON  [dbo].[URLs]
   FOR DELETE
AS 
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    DELETE childTable FROM DeviceInventory childTable
    WHERE exists (SELECT ID FROM deleted where deleted.ID=childTable.DeviceID and 
    childTable.DeviceTypeID=7)

END

GO
USE [vitalsigns]
GO

/****** Object:  Trigger [dbo].[tr_InsertDeviceInventoryURLs]    Script Date: 12/09/2014 12:35:19 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Natallya Shkarayeva
-- Create date: 12/18/2014
-- Description:	Insert trigger will update the DeviceInventory table
-- Modified date: 4/21/2015 by Natallya Shkarayeva
-- Modification description: LocationID for URLs should not be NULL, changed NULL to LocationID
-- =============================================
CREATE TRIGGER [dbo].[tr_InsertDeviceInventoryURLs]
   ON  [dbo].[URLs]
   FOR INSERT
AS 
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	INSERT INTO DeviceInventory
        (Name, DeviceID, DeviceTypeID, LocationID)
    SELECT
        Name, ID, 7, LocationID
        FROM inserted

END

GO
USE [vitalsigns]
GO

/****** Object:  Trigger [dbo].[tr_UpdateDeviceInventoryURLs]    Script Date: 12/09/2014 12:35:42 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Natallya Shkarayeva
-- Create date: 12/18/2014
-- Description:	Update trigger will update the DeviceInventory table
-- Modified date: 4/21/2015 by Natallya Shkarayeva
-- Modification description: LocationID for URLs should not be NULL, added LocationID
-- Modified date: 4/27/2015 by Natallya Shkarayeva
-- Modification description: when Enabled changes, either add a new record or delete an existing one
-- from DeviceInventory
-- =============================================
CREATE TRIGGER [dbo].[tr_UpdateDeviceInventoryURLs]
   ON  [dbo].[URLs]
   AFTER UPDATE
AS 
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	IF (UPDATE (Name) or UPDATE (LocationID))
	BEGIN
		UPDATE DeviceInventory
		SET    DeviceInventory.Name = Inserted.Name, DeviceInventory.LocationID = Inserted.LocationId
		FROM   DeviceInventory  JOIN inserted ON DeviceInventory.DeviceID = inserted.ID
		AND  DeviceInventory.DeviceTypeID=inserted.ServerTypeID
	END
	
	IF UPDATE([Enabled])
	BEGIN
		IF (SELECT [Enabled] FROM Deleted) = 0 AND (SELECT [Enabled] FROM Inserted) = 1
		BEGIN
			INSERT INTO DeviceInventory
			(Name, DeviceID, DeviceTypeID, LocationID)
			SELECT
				Name, ID, 7, LocationID
			FROM inserted
				WHERE inserted.[Enabled] = 1	
		END
		ELSE
		 IF (SELECT [Enabled] FROM Inserted) = 0 AND (SELECT [Enabled] FROM Deleted) = 1
		 BEGIN
			DELETE d FROM DeviceInventory AS d INNER JOIN Inserted AS i ON d.DeviceID = i.ID
			AND  d.DeviceTypeID=i.ServerTypeID AND i.[Enabled] = 0
		 END
	END
END

GO


/****************  3/10/15 WS ADDED MORE TRIGGERS FOR MISSING SERVERTYPES FOR HA ***********************/

/* 5/11/2016 NS modified for VSPLUS-2921 */
/* ServerType should be Domino Cluster database, not Domino Cluster */
IF EXISTS(SELECT * FROM sys.triggers WHERE name = 'tr_DeleteDeviceInventoryDominoCluster')
DROP TRIGGER [dbo].[tr_DeleteDeviceInventoryDominoCluster]
GO
CREATE TRIGGER [dbo].[tr_DeleteDeviceInventoryDominoCluster]
   ON  [dbo].[DominoCluster]
   FOR DELETE
AS 
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    DELETE childTable FROM DeviceInventory childTable
    WHERE exists (SELECT Name FROM deleted where deleted.Name=childTable.Name and 
    childTable.DeviceTypeID=(Select ID From ServerTypes where ServerType = 'Domino Cluster database'))

END
GO

IF EXISTS(SELECT * FROM sys.triggers WHERE name = 'tr_InsertDeviceInventoryDominoCluster')
DROP TRIGGER [dbo].[tr_InsertDeviceInventoryDominoCluster]
GO
CREATE TRIGGER [dbo].[tr_InsertDeviceInventoryDominoCluster]
   ON  [dbo].[DominoCluster]
   FOR INSERT
AS 
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	INSERT INTO DeviceInventory
        (Name, DeviceID, DeviceTypeID, LocationID)
    SELECT
        Name, ID, (select ID from ServerTypes where ServerType='Domino Cluster database'), NULL
        FROM inserted

END
GO

/* 4/29/2015 NS modified for VSPLUS-1686 */
IF EXISTS(SELECT * FROM sys.triggers WHERE name = 'tr_UpdateDeviceInventoryDominoCluster')
DROP TRIGGER [dbo].[tr_UpdateDeviceInventoryDominoCluster]
GO
CREATE TRIGGER [dbo].[tr_UpdateDeviceInventoryDominoCluster]
   ON  [dbo].[DominoCluster]
   AFTER UPDATE
AS 
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    DECLARE @NewName as varchar(50)
	
	SELECT @NewName=inserted.Name FROM inserted
	IF UPDATE (Name)
	BEGIN
		UPDATE DeviceInventory
		SET    DeviceInventory.Name = (Select Name From Inserted)
		FROM   DeviceInventory  JOIN deleted ON DeviceInventory.Name = deleted.Name 
		WHERE  DeviceInventory.DeviceTypeID=(select ID from ServerTypes where ServerType='Domino Cluster database')
	END
	
	IF UPDATE([Enabled])
	BEGIN
		IF (SELECT [Enabled] FROM Deleted) = 0 AND (SELECT [Enabled] FROM Inserted) = 1
		BEGIN
			INSERT INTO DeviceInventory
			(Name, DeviceID, DeviceTypeID, LocationID)
			SELECT
				Name, ID, (select ID from ServerTypes where ServerType='Domino Cluster database'), NULL
				FROM inserted i
				WHERE i.[Enabled] = 1
		END
		ELSE
		 IF (SELECT [Enabled] FROM Inserted) = 0 AND (SELECT [Enabled] FROM Deleted) = 1
		 BEGIN
			DELETE d FROM DeviceInventory AS d INNER JOIN Inserted AS i ON d.DeviceID = i.ID
			AND  d.DeviceTypeID=(Select ID From ServerTypes where ServerType = 'Domino Cluster database') AND i.[Enabled] = 0
		 END	
	END
END
GO







/* END - creating triggers for the device tables */



USE [VitalSigns]
GO


CREATE TABLE [dbo].[O365LYNCP2PSessionReport](
	[ServerId] [int] NOT NULL,
	[AccountName] [varchar](254) NULL,
	[TotalP2PSessions] [int] NULL,
	[P2PIMSessions] [int] NULL,
	[P2PAudioSessions] [int] NULL,
	[P2PVideoSessions] [int] NULL,
	[P2PApplicationSharingSessions] [int] NULL,
	[P2PFileTransferSessions] [int] NULL
) ON [PRIMARY]

GO

CREATE TABLE [dbo].[O365LYNCPAVTimeReport](
	[ServerId] [int] NOT NULL,
	[AccountName] [varchar](254) NULL,
	[TotalAudioMinutes] [int] NULL,
	[TotalVideoMinutes] [int] NULL
) ON [PRIMARY]

GO

CREATE TABLE [dbo].[O365LYNCConferenceReport](
	[ServerId] [int] NOT NULL,
	[AccountName] [varchar](254) NULL,
	[TotalConferences] [int] NULL,
	[AVConferences] [int] NULL,
	[IMConferences] [int] NULL,
	[ApplicationSharingConferences] [int] NULL,
	[WebConferences] [int] NULL,
	[TelephonyConferences] [int] NULL
) ON [PRIMARY]

GO

CREATE TABLE [dbo].[O365LYNCStats](
	[ServerId] [int] NOT NULL,
	[AccountName] [varchar](254) NULL,
	[ActiveUsers] [int] NULL,
	[ActiveIMUsers] [int] NULL,
	[ActiveAudioUsers] [int] NULL,
	[ActiveVideousers] [int] NULL,
	[ActiveApplicationSharingUsers] [int] NULL,
	[ActiveFileTransferUsers] [int] NULL
) ON [PRIMARY]

GO

CREATE TABLE [dbo].[O365LYNCDevices](
	[ServerId] [int] NOT NULL,
	[AccountName] [varchar](254) NULL,
	[WindowsUsers] [int] NULL,
	[WindowsPhoneUsers] [int] NULL,
	[AndroidUsers] [int] NULL,
	[iPhoneUsers] [int] NULL,
	[iPadUsers] [int] NULL
) ON [PRIMARY]

GO


USE [vitalsigns]
GO

/****** Object:  Table [dbo].[SharePointFarmHealth]    Script Date: 12/23/2014 10:54:29 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[SharePointFarmHealth](
	[FarmName] [varchar](250) NOT NULL,
	[LogOnTest] [bit] NULL,
	[UploadTest] [bit] NULL,
	[SiteCollectionTest] [bit] NULL
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

USE [vitalsigns]
GO

/****** Object:  Table [dbo].[SharePointFarmSettings]    Script Date: 12/23/2014 10:55:52 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[SharePointFarmSettings](
	[FarmName] [varchar](250) NOT NULL,
	[LogOnTest] [bit] NULL,
	[SiteCreationTest] [bit] NULL,
	[FileUploadTest] [bit] NULL,
	[TestApplicationURL] [varchar](250) NULL
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO


/*1322-Swathi*/
USE [vitalsigns]
GO
/****** Object:  Table [dbo].[WebsphereCell]    Script Date: 01/22/2015 19:16:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[WebsphereCell](
	[CellID] [int] IDENTITY(1,1) NOT NULL,
	[CellName] [nvarchar](200) NULL,
	[HostName] [nvarchar](200) NULL,
	[ConnectionType] [nvarchar](50) NULL,
	[PortNo] [int] NULL,
	[GlobalSecurity] [bit] NULL,
	[SametimeId] [int] NULL,
	[CredentialsID] [int] NULL,
	[Realm] [nvarchar](max) NULL,
	[Certification] [nvarchar](max) NULL,
	[Name][nvarchar](max) NULL,
	--3/11/2016 sowjanya Modified for VSPLUS-2650
	[IBMConnectionSID] [int] NULL,
	 CONSTRAINT [PK_WebsphereCell] PRIMARY KEY CLUSTERED 
(
	[CellID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[WebsphereNode]    Script Date: 01/22/2015 19:16:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[WebsphereNode](
	[NodeID] [int] IDENTITY(1,1) NOT NULL,
	[NodeName] [nvarchar](200) NULL,
	[CellID] [int] NULL,
	[JVMs] [nvarchar](200) NULL,
	[Status] [nvarchar](200) NULL,
	[HostName] [nvarchar](200) NULL,
 CONSTRAINT [PK_WebsphereNode] PRIMARY KEY CLUSTERED 
(
	[NodeID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[WebsphereCellStats]    Script Date: 01/22/2015 19:16:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[WebsphereCellStats](
	[CellID] [int] NULL,
	[CellName] [nvarchar](200) NULL,
	[Status] [nvarchar](200) NULL,
	[LastScan] [datetime] NULL,
	[TotalJVM] [int] NULL,
	[MonitoredJVMs] [int] NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[WebsphereServer]    Script Date: 01/22/2015 19:16:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[WebsphereServer](
	[ServerID] [int] NOT NULL,
	[NodeID] [int] NULL,
	[CellID] [int] NULL,
	[ServerName] [nvarchar](200) NULL,
	[AvgThreadPool] [float] NULL,
	[ActiveThreadCount] [float] NULL,
	[CurrentHeap] [nvarchar](max) NULL,
	[MaxHeap] [nvarchar](max) NULL,
	[FreeMemory] [nvarchar](max) NULL,
	[CPUUtilization] [float] NULL,
	[Uptime] [float] NULL,
	[HungThreadCount] [float] NULL,
	[DumpGenerated] [nvarchar](max) NULL,
	[Status] [nvarchar](max) NULL,
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Enabled] [bit] NULL,
	[HostName] [nvarchar](max) NULL,
) ON [PRIMARY]
GO
/****** Object:  ForeignKey [FK__Websphere__CellI__74C42EAC]    Script Date: 01/22/2015 19:16:33 ******/
ALTER TABLE [dbo].[WebsphereCellStats]  WITH CHECK ADD FOREIGN KEY([CellID])
REFERENCES [dbo].[WebsphereCell] ([CellID])
GO
/****** Object:  ForeignKey [FK__Websphere__CellI__6B3AC472]    Script Date: 01/22/2015 19:16:33 ******/
ALTER TABLE [dbo].[WebsphereNode]  WITH CHECK ADD FOREIGN KEY([CellID])
REFERENCES [dbo].[WebsphereCell] ([CellID])
GO
/****** Object:  ForeignKey [FK__Websphere__CellI__70F39DC8]    Script Date: 01/22/2015 19:16:33 ******/
ALTER TABLE [dbo].[WebsphereServer]  WITH CHECK ADD FOREIGN KEY([CellID])
REFERENCES [dbo].[WebsphereCell] ([CellID])
GO
/****** Object:  ForeignKey [FK__Websphere__NodeI__6F0B5556]    Script Date: 01/22/2015 19:16:33 ******/
ALTER TABLE [dbo].[WebsphereServer]  WITH CHECK ADD FOREIGN KEY([NodeID])
REFERENCES [dbo].[WebsphereNode] ([NodeID])
GO
/****** Object:  ForeignKey [FK__Websphere__Serve__6E17311D]    Script Date: 01/22/2015 19:16:33 ******/
ALTER TABLE [dbo].[WebsphereServer]  WITH CHECK ADD FOREIGN KEY([ServerID])
REFERENCES [dbo].[Servers] ([ID])

/* 1684 swathi*/

ALTER TABLE [dbo].[WebsphereServer]  WITH CHECK ADD  CONSTRAINT [FK_Servers_WebsphereServer] FOREIGN KEY([ServerID])
REFERENCES [dbo].[Servers] ([ID])
ON DELETE CASCADE
GO

USE [vitalsigns]
GO

/****** Object:  Table [dbo].[NetworkLatency]    Script Date: 1/23/2015 5:48:22 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[NetworkLatency](
	[NetworkLatencyId] [int] IDENTITY(1,1) NOT NULL,
	[TestName] [nvarchar](50) NULL,
	[ScanInterval] [int] NULL,
	[TestDuration] [int] NULL,
	[Enable] [bit] NULL,
 CONSTRAINT [PK_NetworkLatency_1] PRIMARY KEY CLUSTERED 
(
	[NetworkLatencyId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

CREATE TABLE [dbo].[NetworkLatencyServers](
 [ServerID] [int] NULL,
 [NetworkLatencyId] [int] NULL,
 [LatencyYellowThreshold] [int] NULL,
 [LatencyRedThreshold] [int] NULL,
 [Enabled] [bit] NULL,
 [id] [int] IDENTITY(1,1) NOT NULL,
 CONSTRAINT [PK_NetworkLatencyServers] PRIMARY KEY CLUSTERED 
(
 [id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]



ALTER TABLE [dbo].[NetworkLatencyServers]  WITH CHECK ADD  CONSTRAINT [FK_NetworkLatencyServers_Servers] FOREIGN KEY([ServerID])
REFERENCES [dbo].[Servers] ([ID])
ON DELETE CASCADE
GO


ALTER TABLE [dbo].[NetworkLatencyServers]  WITH CHECK ADD FOREIGN KEY([NetworkLatencyId])
REFERENCES [dbo].[NetworkLatency] ([NetworkLatencyId])

GO

--Mukund 24Jan15
USE [vitalsigns]
GO

/****** Object:  StoredProcedure [dbo].[WSNodes]    Script Date: 24-Jan-15 5:52:38 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE procedure [dbo].[WSNodes] 
@CellID as int
as
Begin

declare @SrvEvents Table
(id int, SrvId int, Name varchar(100),actid int,tbl varchar(50),AlertOnRepeat bit,Status varchar(100),HostName varchar(100))

insert into @SrvEvents select st.NodeID,null,st.NodeName,st.NodeID,'Nodes',0,null,null from WebsphereNode st,WebsphereCell ft 
 where ft.CellID=st.CellID  and ft.CellID=@CellID  and st.nodeid in
 (
  select st.NodeID from WebsphereServer st,WebsphereCell ft   
where ft.CellID=st.CellID and ft.CellID=@CellID  and (st.Enabled is null or st.Enabled ='False')
 ) 
 order by st.NodeID

Declare @count int
select @count=MAX(NodeID) from WebsphereNode 


Declare @ID int
Declare @EventName varchar(100)
Declare @ServerTypeID int
Declare @AlertOnRepeat bit
Declare @Status varchar(100)
Declare @HostName varchar(100)

DECLARE db_cursor CURSOR FOR  
select st.ServerID,st.ServerName, st.NodeID,st.Status,st.HostName from WebsphereServer st,WebsphereCell ft   
where ft.CellID=st.CellID and ft.CellID=@CellID  and (st.Enabled is null or st.Enabled ='False')
order by st.NodeID,st.ServerName

OPEN db_cursor   
FETCH NEXT FROM db_cursor into @ID,@EventName,@ServerTypeID,@Status,@HostName

WHILE @@FETCH_STATUS = 0   
BEGIN   
 Set @count=@count+1
 insert into @SrvEvents values(@count,@ServerTypeID,@EventName,@id,'Servers',0, @Status,@HostName)
 FETCH NEXT FROM db_cursor into @ID,@EventName,@ServerTypeID,@Status,@HostName
END   

CLOSE db_cursor   
DEALLOCATE db_cursor

select * from @SrvEvents  --order by SrvId,Name
end
GO


/*WS ADDED HERE */

USE [vitalsigns]
GO

/****** Object:  Table [dbo].[NetworkDevicesDetails]    Script Date: 02/03/2015 16:13:16 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[NetworkDevicesDetails](
	[NetworkID] [int] NULL,
	[StatName] [nvarchar](50) NULL,
	[StatValue] [nvarchar](50) NULL
) ON [PRIMARY]

GO



/* 1321 Durga */
USE [vitalsigns]
GO
/****** Object:  StoredProcedure [dbo].[WSsametimeNodes]    Script Date: 07/09/2015 02:08:07 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE procedure [dbo].[WSsametimeNodes] 
@CellID as int
as
Begin

declare @SrvEvents Table
(id int, SrvId int, Name varchar(100),actid int,tbl varchar(50),AlertOnRepeat bit,Status varchar(100),HostName varchar(100))

insert into @SrvEvents select st.NodeID,null,st.NodeName,st.NodeID,'Nodes',0,NULL,NULL from WebsphereNode st,WebsphereCell ft 
 where ft.CellID=st.CellID  and ft.CellID=@CellID and st.nodeid in
 (
  select st.NodeID from WebsphereServer st,WebsphereCell ft,SametimeServers smt 
where ft.CellID=st.CellID and ft.CellID=@CellID and smt.ServerID=ft.SametimeId and (st.Enabled is null or st.Enabled ='False')
 ) 
 order by st.NodeID

Declare @count int
select @count=MAX(NodeID) from WebsphereNode 


Declare @ID int
Declare @EventName varchar(100)
Declare @ServerTypeID int
Declare @AlertOnRepeat bit
Declare @Status varchar(100)
Declare @HostName varchar(100)

DECLARE db_cursor CURSOR FOR  
select st.ServerID,st.ServerName, st.NodeID,st.Status,st.HostName from WebsphereServer st,WebsphereCell ft   
where ft.CellID=st.CellID and ft.CellID=@CellID and  (st.Enabled is null or st.Enabled ='False')
order by st.NodeID,st.ServerName

OPEN db_cursor   
FETCH NEXT FROM db_cursor into @ID,@EventName,@ServerTypeID,@Status,@HostName

WHILE @@FETCH_STATUS = 0   
BEGIN   
	Set @count=@count+1
	insert into @SrvEvents values(@count,@ServerTypeID,@EventName,@id,'Servers',0, @Status,@HostName)
	FETCH NEXT FROM db_cursor into @ID,@EventName,@ServerTypeID,@Status,@HostName
END   

CLOSE db_cursor   
DEALLOCATE db_cursor

select * from @SrvEvents  --order by SrvId,Name
end

GO
USE [vitalsigns]
GO
CREATE procedure [dbo].[WSsametimeservers] 
@CellID as int
as
Begin

declare @SrvEvents Table
(id int, SrvId int, Name varchar(100),actid int,tbl varchar(50),AlertOnRepeat bit,Enabled bit)

insert into @SrvEvents select st.NodeID,null,st.NodeName,st.NodeID,'Nodes',0,0 from WebsphereNode st,WebsphereCell ft 
 where ft.CellID=st.CellID  and ft.CellID=@CellID and st.nodeid in
 (
  select st.NodeID from WebsphereServer st,WebsphereCell ft,SametimeServers smt 
where ft.CellID=st.CellID and ft.CellID=@CellID and smt.ServerID=ft.SametimeId and ( st.Enabled ='True')
 ) 
 order by st.NodeID

Declare @count int
select @count=MAX(NodeID) from WebsphereNode 


Declare @ID int
Declare @EventName varchar(100)
Declare @ServerTypeID int
Declare @AlertOnRepeat bit
Declare @Enabled bit

DECLARE db_cursor CURSOR FOR  
select st.ServerID,st.ServerName, st.NodeID,st.Enabled from WebsphereServer st,WebsphereCell ft   
where ft.CellID=st.CellID and ft.CellID=@CellID  and (st.Enabled ='True')
order by st.NodeID,st.ServerName

OPEN db_cursor   
FETCH NEXT FROM db_cursor into @ID,@EventName,@ServerTypeID,@Enabled

WHILE @@FETCH_STATUS = 0   
BEGIN   
	Set @count=@count+1
	insert into @SrvEvents values(@count,@ServerTypeID,@EventName,@id,'Servers',0,@Enabled)
	FETCH NEXT FROM db_cursor into @ID,@EventName,@ServerTypeID,@Enabled
END   

CLOSE db_cursor   
DEALLOCATE db_cursor

select * from @SrvEvents  --order by SrvId,Name
end
GO
/*END OF WS ADDITIONS */


--2/17/15 WS added VSPLUS 1247

USE [vitalsigns]
GO

/****** Object:  Table [dbo].[SharePointSiteCollections]    Script Date: 02/17/2015 17:35:59 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[SharePointSiteCollections](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[SiteCollection] [nvarchar](255) NULL,
	[SizeMB] [nvarchar](50) NULL,
	[SiteCount] int NULL,
	[Owner] [nvarchar](250) NULL,
	[Date] [datetime] NULL,
	[FarmName] [nvarchar](255) NULL
) ON [PRIMARY]

GO



--vsplus -1552 niranjan
/****** Object:  StoredProcedure [dbo].[sp_MenuSorting]    Script Date: 03/05/2015 15:50:43 ******/
--VSPLUS 1631 DURGA
--VSPLUS-2474 DURGA
USE [vitalsigns]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[sp_MenuSorting]  
 @UserId int , 
 @MenuArea  NVARCHAR(200) ,
 @Level NVARCHAR(10)
 as
begin


create TABLE #mitem ( ID int, DisplayText varchar(200),OrderNum int,ParentID int,PageLink varchar(250),Level  NVARCHAR(10),
RefName varchar(200),ImageURL varchar(200),MenuArea nvarchar(200),SessionNames nvarchar(max),TimerEnable bit,OverrideSort int)


insert into #mitem
SELECT distinct t1.* FROM Menuitems t1,SelectedFeatures sf,FeatureMenus fm where fm.FeatureID=sf.FeatureID and 
fm.MenuID=t1.ID and  MenuArea=@MenuArea and  [Level]<=@Level and ID not in (select MenuID from UserMenuRestrictions where UserId=@UserId)  
and  parentid is null
ORDER BY OverrideSort,orderNum

insert into #mitem
SELECT distinct t1.* FROM Menuitems t1,SelectedFeatures sf,FeatureMenus fm where fm.FeatureID=sf.FeatureID and 
fm.MenuID=t1.ID and  MenuArea=@MenuArea and  [Level]<=@Level and ID not in (select MenuID from UserMenuRestrictions where UserId=@UserId)  
and  parentid is not null
ORDER BY ParentID,OverrideSort,DisplayText,
ordernum

select * from #mitem

drop table #mitem

end
GO

--vsplus 1555 Durga 
USE [vitalsigns]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[SpecificServerLocations] 
 (@Page  as VARCHAR(50) ,@control as  VARCHAR(50)) as 
Begin
declare @SrvLocations Table
(id int, LocId int, Name varchar(100),actid int,tbl varchar(50),srvtypeid int,ServerType varchar(100),description varchar(100))

insert into @SrvLocations select id,null,Location,id,'Locations',null,null,null from Locations 
--select * from @SrvLocations
Declare @count int
select @count=MAX(id) from Locations


Declare @ID int
Declare @LocationName varchar(100)
Declare @LocationID int
Declare @ServerTypeId int
Declare @ServerType varchar(100)
Declare @description varchar(100)


DECLARE db_cursor CURSOR FOR  
select sr.ID,sr.ServerName,sr.LocationID,sr.ServerTypeId,srt.ServerType,sr.description from Servers sr,
ServerTypes srt,SelectedFeatures ft  where sr.ServerTypeId=srt.id and ft.FeatureId=srt.FeatureId and sr.ServerTypeId  not in (select ServertypeID from 

Servertypeexcludelist where Page=@Page and 

Control=@control)
union
select sr.ID,sr.Name as ServerName,sr.LocationID,sr.ServerTypeId,srt.ServerType, sr.TheURL
from URLs sr,ServerTypes srt,SelectedFeatures ft   where sr.ServerTypeId=srt.id  and ft.FeatureId=srt.FeatureId and sr.ServerTypeId  not in (select ServertypeID from 

Servertypeexcludelist where Page=@Page and 

Control=@control)
order by sr.LocationID,sr.ServerName

OPEN db_cursor   
FETCH NEXT FROM db_cursor into @ID,@LocationName,@LocationID,@ServerTypeId,@ServerType,@description

WHILE @@FETCH_STATUS = 0   
BEGIN   
	Set @count=@count+1
	insert into @SrvLocations values(@count,@LocationID,@LocationName,@id,'Servers',@ServerTypeId,@ServerType,@description)
	FETCH NEXT FROM db_cursor into @ID,@LocationName,@LocationID,@ServerTypeId,@ServerType,@description
END   

CLOSE db_cursor   
DEALLOCATE db_cursor

select * from @SrvLocations order by tbl,Name
end
GO

USE [vitalsigns]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Servertypeexcludelist](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Page] [nvarchar](50) NULL,
	[Control] [nvarchar](50) NULL,
	[ServertypeID] [int] NULL
) ON [PRIMARY]

GO




/* 3/9/15 WS ADDED FOR HA SUPPORT */

/* NODES*/

USE [vitalsigns]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[NodeDetails](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[NodeId] [int] NULL,
	[Name] [nvarchar](255) NULL,
	[Value] [nvarchar](255) NULL
) ON [PRIMARY]

GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Nodes](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](255) CONSTRAINT [UQ_Nodes_Name] UNIQUE NONCLUSTERED  NOT NULL,
	[HostName] [nvarchar](255) NULL,
	[Alive] [smallint] NULL,
	[Version] [nvarchar](50) NULL,
	[CredentialsID] [int] NULL,
	[NodeType] [nvarchar](255) NULL,
	[LoadFactor] [float] NULL,
	[NodeTime] [datetime] NULL,
	[Pulse] [datetime] NULL,
	[IsPrimaryNode] [bit] NULL,
	[LocationID] [int] NULL,
	[IsConfiguredPrimaryNode] [bit] NULL,
	[isDisabled] [bit] NULL,
	
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[Nodes] ADD  CONSTRAINT [DF_HostName]  DEFAULT (N'') FOR [HostName]
GO

ALTER TABLE [dbo].[Nodes] ADD  CONSTRAINT [DF_Alive]  DEFAULT (N'0') FOR [Alive]
GO

ALTER TABLE [dbo].[Nodes] ADD  CONSTRAINT [DF_Version]  DEFAULT (N'1.0.0.0') FOR [Version]
GO

ALTER TABLE [dbo].[Nodes] ADD  CONSTRAINT [DF_NodeType]  DEFAULT (N'') FOR [NodeType]
GO

ALTER TABLE [dbo].[Nodes] ADD  CONSTRAINT [DF_LoadFactor]  DEFAULT (N'10') FOR [LoadFactor]
GO

ALTER TABLE [dbo].[Nodes] ADD  CONSTRAINT [DF_IsPrimaryNode]  DEFAULT (N'0') FOR [IsPrimaryNode]
GO


IF EXISTS(SELECT * FROM sys.triggers WHERE name = 'tr_DeleteDeviceInventoryNetworkLatency')
DROP TRIGGER [dbo].[tr_DeleteDeviceInventoryNetworkLatency]
GO
CREATE TRIGGER [dbo].[tr_DeleteDeviceInventoryNetworkLatency]
   ON  [dbo].[NetworkLatency]
   FOR DELETE
AS 
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    DELETE childTable FROM DeviceInventory childTable
    WHERE exists (SELECT TestName FROM deleted where deleted.TestName=childTable.Name and 
    childTable.DeviceTypeID=(Select ID From ServerTypes where ServerType = 'Network Latency'))

END
GO

IF EXISTS(SELECT * FROM sys.triggers WHERE name = 'tr_InsertDeviceInventoryNetworkLatency')
DROP TRIGGER [dbo].[tr_InsertDeviceInventoryNetworkLatency]
GO
CREATE TRIGGER [dbo].[tr_InsertDeviceInventoryNetworkLatency]
   ON  [dbo].[NetworkLatency]
   FOR INSERT
AS 
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	INSERT INTO DeviceInventory
        (Name, DeviceID, DeviceTypeID, LocationID)
    SELECT
        TestName, NetworkLatencyId, (select ID from ServerTypes where ServerType='Network Latency'), NULL
        FROM inserted

END
GO

/* 5/1/2015 NS modified for VSPLUS-1686 */
IF EXISTS(SELECT * FROM sys.triggers WHERE name = 'tr_UpdateDeviceInventoryNetworkLatency')
DROP TRIGGER [dbo].[tr_UpdateDeviceInventoryNetworkLatency]
GO
CREATE TRIGGER [dbo].[tr_UpdateDeviceInventoryNetworkLatency]
   ON  [dbo].[NetworkLatency]
   AFTER UPDATE
AS 
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	IF UPDATE (TestName)
	BEGIN
		UPDATE DeviceInventory
		SET    DeviceInventory.Name = (Select TestName from Inserted)
		FROM   DeviceInventory  JOIN deleted ON DeviceInventory.Name = deleted.TestName 
		WHERE  DeviceInventory.DeviceTypeID=(select ID from ServerTypes where ServerType='Network Latency')
	END
	
	IF UPDATE([Enable])
	BEGIN
		IF (SELECT [Enable] FROM Deleted) = 0 AND (SELECT [Enable] FROM Inserted) = 1
		BEGIN
			INSERT INTO DeviceInventory
			(Name, DeviceID, DeviceTypeID, LocationID)
			SELECT
				TestName, NetworkLatencyId, 23, NULL
			FROM inserted
				WHERE inserted.[Enable] = 1	
		END
		ELSE
		 IF (SELECT [Enable] FROM Inserted) = 0 AND (SELECT [Enable] FROM Deleted) = 1
		 BEGIN
			DELETE d FROM DeviceInventory AS d INNER JOIN Inserted AS i ON d.DeviceID = i.NetworkLatencyId
			AND  d.DeviceTypeID=23 AND i.[Enable] = 0
		 END
	END
END
GO

USE [VitalSigns]
GO

if not exists(select	*
	from	sys.certificates
		inner join	sys.key_encryptions
				on	sys.key_encryptions.thumbprint = sys.certificates.thumbprint
	where	(sys.certificates.[name] = 'EncryptLicenseKey'))
begin

CREATE MASTER KEY ENCRYPTION
BY PASSWORD = 'VSLicense1234!@#$'

CREATE CERTIFICATE EncryptLicenseKey
WITH SUBJECT = 'VSLicense'

CREATE SYMMETRIC KEY VSLicenseKey
WITH ALGORITHM = TRIPLE_DES ENCRYPTION
BY CERTIFICATE EncryptLicenseKey

end
GO

IF NOT EXISTS (select * from syscolumns where id = object_id('dbo.License'))
BEGIN
CREATE TABLE [dbo].[License](
	[LicenseKey] [varchar](400) NULL,
	[Units] [int] NULL,
	[InstallType] [varchar](50) NULL,
	[CompanyName] [varchar](150) NULL,
	[LicenseType] [varchar](100) NULL,
	[ExpirationDate] [datetime] NULL,
	[EncUnits] [varbinary](400) NULL
) ON [PRIMARY]
END
GO

IF NOT EXISTS (select * from syscolumns where id = object_id('dbo.ServerTypeLicenses'))
BEGIN
CREATE TABLE [dbo].[ServerTypeLicenses](
	[ServerTypeId] [int] NOT NULL,
	[UnitCost] [float] NOT NULL,
	[UnitsPurchased] [float] NULL,
	[UnitsUsed] [float] NULL,
	[EncUnitCost] [varbinary](400) NULL
) ON [PRIMARY]
END
GO

IF EXISTS (select * from syscolumns where id = object_id('dbo.ServerTypeLicenses'))
BEGIN
Delete from dbo.ServerTypeLicenses
END
GO





USE [VitalSigns]
GO

/****** Object:  StoredProcedure [dbo].[PR_CheckLicense]    Script Date: 03/24/2015 20:23:39 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PR_CheckLicense]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[PR_CheckLicense]
GO

USE [VitalSigns]
GO

/****** Object:  StoredProcedure [dbo].[PR_CheckLicense]    Script Date: 03/24/2015 20:23:39 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE procedure [dbo].[PR_CheckLicense]
(
@nServerTypeId int, 
@nDeviceid int,
@retVal bit OUTPUT
) WITH ENCRYPTION
AS

DECLARE 
@nActualLoad  float,
@nAssigned int,
@nActual int,
@nUnits int,
@sLicType varchar(50),
@dExpDate datetime,
@bTempRet bit,
@nUnitCost float
BEGIN
OPEN MASTER KEY DECRYPTION BY PASSWORD = 'VSLicense1234!@#$'
OPEN SYMMETRIC KEY VSLicenseKey DECRYPTION
BY CERTIFICATE EncryptLicenseKey


create table #TmpLic(cnt float)
set @nUnits=0
-- first get how many licenses have been used
insert into #TmpLic(cnt)
select COUNT(DeviceTypeID) * STL.UnitCost  from DeviceInventory DI,ServerTypeLicenses STL 
where (DI.CurrentNodeId IS NOT NULL  AND DI.CurrentNodeId >0) AND DI.DeviceTypeID = STL.ServerTypeId group by DI.DeviceTypeID , STL.UnitCost


SELECT @nActualLoad=SUM(CNT) FROM #TmpLic 
if @nActualLoad IS NULL
	set @nActualLoad=0
SELECT @nUnits=Units,@sLicType=LicenseType,@dExpDate=Expirationdate from dbo.License
if exists (select * from ServerTypeLicenses where ServerTypeId=@nServerTypeId)
	select @nUnitCost=UnitCost from dbo.ServerTypeLicenses where ServerTypeId=@nServerTypeId
else
	select @nUnitCost=1
if (@nUnits >= @nActualLoad)
begin
if (@nUnits -@nActualLoad) >= @nUnitCost and @dExpDate > GETDATE()
	set @bTempRet =1
else
	begin
	print('no licenses left')
	print(convert(varchar,@nUnitCost)  +'no licenses left')
	set @bTempRet =0
	end
end
else
begin
	if @nUnitCost=0
		set @bTempRet=1
	else
		set @bTempRet=0
end
PRINT(CONVERT(VARCHAR,@nActualLoad))
close SYMMETRIC KEY VSLicenseKey
CLOSE MASTER KEY 
SET @retVal=@bTempRet
END

GO

USE [vitalsigns]
GO

Create table dbo.SystemMessagesTemp(
ID int IDENTITY(1,1),
Details VARCHAR(250),
MessageType bit,
DateCreated datetime
)
go

USE [VitalSigns]
GO

/****** Object:  StoredProcedure [dbo].[PR_EncDecLicense]    Script Date: 03/24/2015 20:23:07 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PR_EncDecLicense]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[PR_EncDecLicense]
GO

USE [VitalSigns]
GO

/****** Object:  StoredProcedure [dbo].[PR_EncDecLicense]    Script Date: 03/24/2015 20:23:07 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[PR_EncDecLicense]
	@bEncDec bit WITH ENCRYPTION
AS

BEGIN
Print('Dummy Proc to elimiate circular reference. The actual Proc will be re-created later in the script.')
END
GO

USE [VitalSigns]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PR_RefreshServerCollection]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[PR_RefreshServerCollection]
GO

USE [VitalSigns]
GO

/****** Object:  StoredProcedure [dbo].[PR_RefreshServerCollection]    Script Date: 03/24/2015 20:24:08 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE procedure [dbo].[PR_RefreshServerCollection]
(
@nTriggerRefresh int
)

as
declare
@isHAOn int,
--@nNoOfNodes int,
--@nLatestNode int,
@nMinRow	integer,
@nMaxRow	integer	,
@nMinNodesRow	integer,
@nMaxNodesRow	integer	,
@nCurrentDeviceid int,
@nCurrentNodeId int,
@nAssignedNodeId int,
@nNodesDown int,
@nNodesUp int,
@nTempNodeid int,
@fTempLoadFactor float,
@fActualLoad float,
@nServerTypeId int,
@nActualLoad  float,
@nAssigned float,
@nActual float,
@nTotalAssigned float,
@nTotalActual float,
@nTotalActualLoad float,
@nSumLoadfactor float,
@nCountLoadfactor float,
@IsLiceExist bit,
@sumTempNodeFactor float,
@diID int,
@dbVersion varchar(50),
@nLocationId int,
@UpdateThisServer bit,
@incrementNode int,
@deviceName varchar(256),
@sysMessageId int,
@systemMessage varchar(254)

begin

SET NOCOUNT ON
-- Decrypt the License
exec PR_EncDecLicense 1
-- first check if HA is turned ON
select @isHAOn=COUNT(*) from dbo.License where InstallType ='HA'
select @dbVersion = Value from VS_MANAGEMENT where Category = 'VS_VERSION'
set @systemMessage='Master Service has Stopped Running. Please re-start it.'
if @isHAOn =0
begin
	-- first get the node that is currently active, and set the remaining nodes as inactive
	if exists(select id from Nodes where Alive=1)
		update dbo.Nodes set Pulse=NULL,Alive=0,IsPrimaryNode =0 where ID not in (select id from Nodes where Alive=1)
	else
	-- if cannot find the currently active node, then find out the node whihc has the latest nodetime and set the other as inactive
		update dbo.Nodes set Pulse=NULL,Alive=0,IsPrimaryNode =0 where ID not in(select top 1 id from nodes order by NodeTime desc)
	--update dbo.Nodes set Pulse=NULL,Alive=0,IsPrimaryNode =0 where ID >(Select MIN(ID) FROM dbo.Nodes) --if more than one node is prewsent then make the last nodes inactive
end

-- if the primary node has no location, set the location
SELECT @nLocationId=ID from dbo.Locations  where id in(select top 1 ID from Locations )
if @nLocationId is not null
begin
	if exists(select * from dbo.Nodes where IsPrimaryNode =1 and LocationID is null)
	begin
		update dbo.Nodes set LocationID=@nLocationId where IsPrimaryNode =1 and LocationID is null
		if @isHAOn =1
		if not exists(select * from SystemMessagesTemp where Details='Default Location has been assigned to the Primary Node automatically by the System. Please change it to the correct one.')
			insert into dbo.SystemMessagesTemp(Details,MessageType,DateCreated) values('Default Location has been assigned to the Primary Node automatically by the System. Please change it to the correct one.',1,Getdate())
	end
end

CREATE TABLE #tbActiveNodes(
	rownum int IDENTITY (1, 1) Primary key NOT NULL,
	NAME		varchar(256),
	NodeID	integer,
	Loadfactor float,
	CurrentLoad float
)

select @nNodesDown=COUNT(*) from Nodes where Alive=1 and (DATEDIFF(mi,Pulse,GETDATE()) >5 or Version <> @dbVersion or isDisabled=1)
select @nNodesUp=COUNT(*) from Nodes where ( Alive IS NULL or Alive =0) and DATEDIFF(mi,Pulse,GETDATE()) <5 and Version = @dbVersion and (isDisabled=0 or isDisabled is null)

-- the nodes that just came up and the nods that are still active
INSERT INTO #tbActiveNodes(NAME,NodeID,Loadfactor ) SELECT NAME,ID,ISNULL(LoadFactor,0) from dbo.Nodes where DATEDIFF(mi,Pulse,GETDATE()) <5 and Version = @dbVersion and (isDisabled=0 or isDisabled is null) order by ID
--in case of loadfactor is 0 we need to set some temporary values
select @sumTempNodeFactor=SUM(loadfactor) from #tbActiveNodes 
if (@sumTempNodeFactor <> 100)
begin
	--select @sumTempNodeFactor=(100-@sumTempNodeFactor)/COUNT(*) from #tbActiveNodes where Loadfactor =0
	select @sumTempNodeFactor =100/COUNT(*) from #tbActiveNodes
	update #tbActiveNodes set Loadfactor =@sumTempNodeFactor/100 --where Loadfactor =0
end
else
	update #tbActiveNodes set Loadfactor =Loadfactor/100 --where Loadfactor =0
	


print('Nodes Down: ' + convert(varchar,@nNodesDown))
print('Nodes Up: ' + convert(varchar,@nNodesUp))
 select * from #tbActiveNodes
-- one of the node is down/not responding for more than 5 mins or new nodes came up
if (@nNodesDown + @nNodesUp +@nTriggerRefresh) >0
BEGIN
print('something changed')
CREATE TABLE #tbServersTemp(
	rownum int IDENTITY (1, 1) Primary key NOT NULL,
	NAME		varchar(256),
	DEVICE_ID	integer,
	DEVICE_TYPE	integer,
	ASSIGNED_NODE_ID INTEGER,
	CURRENT_NODE_ID INTEGER,
	ID INTEGER
)
--set all the servers node as -1, licensing issue
update dbo.DeviceInventory set CurrentNodeId =-1
INSERT INTO #tbServersTemp(NAME,DEVICE_ID,DEVICE_TYPE,ASSIGNED_NODE_ID,CURRENT_NODE_ID,ID) SELECT NAME,ISNULL(DEVICEID,1),DEVICETYPEID,AssignedNodeId,CurrentNodeId,ID FROM dbo.DeviceInventory with(nolock) order by DeviceID asc


UPDATE dbo.Nodes SET Alive = 0 WHERE DATEDIFF(mi,Pulse,GETDATE()) > 5 or Version <> @dbVersion or isDisabled = 1
UPDATE dbo.Nodes SET Alive = 1 WHERE DATEDIFF(mi,Pulse,GETDATE()) < 5 and Version = @dbVersion and (isDisabled=0 or isDisabled is null)

if not exists (select * from dbo.Nodes where Alive=1)
begin
if not exists(select * from SystemMessages where Details=@systemMessage and DateCleared is null)
	begin
			insert into dbo.SystemMessages(Details,DateCreated) values(@systemMessage,Getdate())
			select @sysMessageId=@@IDENTITY 
			IF NOT EXISTS(SELECT * FROM UserSystemMessages WHERE SysMsgID=@sysMessageId)
			begin
			 INSERT INTO UserSystemMessages (SysMsgID,UserID)
					 SELECT t1.ID SysMsgID,t2.ID UserID FROM SystemMessages t1, Users t2  WHERE t1.DateCleared IS NULL AND t1.ID=@sysMessageId 
			END
	end
end
else
begin
	if exists(select * from SystemMessages where Details=@systemMessage and DateCleared IS null)
	begin
 		Update SystemMessages  set DateCleared=GETDATE() where Details=@systemMessage and DateCleared IS null
 		DELETE  t1 FROM UserSystemMessages t1 INNER JOIN SystemMessages t2 ON t2.ID=t1.SysMsgID WHERE t2.Details=@systemMessage
 	end
end

-- if the primary node goes down, then set the other node the primary, pick the ID which has the least ID(or the oldest and strongest node)
UPDATE dbo.Nodes set isConfiguredPrimaryNode = 0 where id <> (SELECT MIN(ID) from dbo.Nodes where isConfiguredPrimaryNode = 1)
if exists(select id FROM dbo.Nodes where Alive=1 and isConfiguredPrimaryNode=1)
	begin
	--reset the node which was set as primary
	update dbo.Nodes set isPrimaryNode=0 --where ID IN(select id FROM dbo.Nodes where Alive=0 and isPrimaryNode=1)
	update dbo.Nodes set isPrimaryNode=1 where isConfiguredPrimaryNode = 1
	end
else 
	begin
	update dbo.Nodes set isPrimaryNode=0
	update dbo.Nodes set isPrimaryNode=1 where ID=(Select MIN(ID) from dbo.Nodes where Alive = 1)
	end

set @incrementNode=0
SELECT @nMinRow=MIN(rownum), @nMaxRow=MAX(rownum) FROM #tbServersTemp
WHILE @nMinRow <= @nMaxRow
	BEGIN
		SELECT @nCurrentDeviceid=DEVICE_ID,@nCurrentNodeId=CURRENT_NODE_ID,@nAssignedNodeId=ASSIGNED_NODE_ID,@nServerTypeId=DEVICE_TYPE,@diID=ID,@deviceName=NAME  FROM #tbServersTemp WHERE rownum=@nMinRow
		print('Re-Assign Current Server. Id:' + convert(varchar,@diID) + '. NAME:' +@deviceName )
		SELECT @nMinNodesRow=MIN(rownum), @nMaxNodesRow=MAX(rownum) FROM #tbActiveNodes
		
		-- compute the load for the nodes and update the temp table
			--if (@incrementNode)=@nMaxNodesRow
			--	set @incrementNode=0
			--set @nMinNodesRow=@incrementNode+1
			
			WHILE @nMinNodesRow <= @nMaxNodesRow
			BEGIN
			print('++++++++++++++++++++++++++++++ START +++++++++++++++++++++++++++++++++')
				select @nTempNodeId=NodeID, @fTempLoadFactor=Loadfactor from #tbActiveNodes where rownum=@nMinNodesRow
				
				select @nAssigned=count(*) from #tbServersTemp with(nolock)   where DEVICE_TYPE=@nServerTypeId and CURRENT_NODE_ID=@nTempNodeId
				select @nActual=count(*) from #tbServersTemp with(nolock)   where DEVICE_TYPE=@nServerTypeId 
				
				select @nTotalAssigned=count(*) from #tbServersTemp with(nolock)   where CURRENT_NODE_ID=@nTempNodeId
				select @nTotalActual=count(*) from #tbServersTemp with(nolock)   where CURRENT_NODE_ID <> -1
				
				print('Node-d:' +convert(varchar,@nTempNodeId))
				print('Server Type ID:' +convert(varchar,@nServerTypeId))
				print('Assigned Servers:' +convert(varchar,@nAssigned))
				print('Total Servers:' +convert(varchar,@nActual))
				
				if @nActual >0
					set @nActualLoad=((@nAssigned)/@nActual)
				else
					set @nActualLoad=0
					
				if @nTotalActual >0
					set @nTotalActualLoad=((@nTotalAssigned)/@nTotalActual)
				else
					set @nTotalActualLoad=0
					
				print('Actual Computed Load for this Node is:' +convert(varchar,@nActualLoad) + '. But Load Factor for this node is set at:' +convert(varchar,(@fTempLoadFactor)))
				print('Total Actual Computed Load for this Node is:' +convert(varchar,@nTotalActualLoad) + '. But Load Factor for this node is set at:' +convert(varchar,(@fTempLoadFactor)))
				if (@nActualLoad <= (@fTempLoadFactor) and @nTotalActualLoad <= (@fTempLoadFactor))
					set @UpdateThisServer=1
				if @UpdateThisServer=0 and @nMinNodesRow = @nMaxNodesRow
				set @UpdateThisServer=1
				if @UpdateThisServer=1
					begin
						print('update this node for this server')
						EXEC dbo.PR_CheckLicense @nServerTypeId,@nCurrentDeviceid,@IsLiceExist output
						if (@IsLiceExist =1)
						begin
							if @nAssignedNodeId >0 and EXISTS(SELECT * FROM #tbActiveNodes where NodeID = @nAssignedNodeId) 
								set @nTempNodeId=@nAssignedNodeId
							
							update dbo.DeviceInventory set CurrentNodeId=@nTempNodeId where ID=@diID
							update #tbServersTemp set CURRENT_NODE_ID=@nTempNodeId where ID=@diID
						end
						else
							update dbo.DeviceInventory set CurrentNodeId=-1 where ID=@diID 
						set @nMinNodesRow=@nMaxNodesRow
						--set @incrementNode=@incrementNode+1
					end
				else
					print('Sorry, Node Overloaded.')
					set @UpdateThisServer=0
				
				if @nMinNodesRow = @nMaxNodesRow
					set @incrementNode=0
					set @nMinNodesRow = @nMinNodesRow + 1
				print('++++++++++++++++++++++++++++++ FINISH +++++++++++++++++++++++++++++++++')
			END
	set @nMinRow = @nMinRow + 1
	END

DROP TABLE #tbServersTemp
update dbo.NodeDetails set Value='1' where Name like '%- UpdateCollection%'	
END
drop table #tbActiveNodes
end

GO


USE [VitalSigns]
GO

/****** Object:  StoredProcedure [dbo].[PR_EncDecLicense]    Script Date: 03/24/2015 20:23:07 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PR_EncDecLicense]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[PR_EncDecLicense]
GO

USE [VitalSigns]
GO

/****** Object:  StoredProcedure [dbo].[PR_EncDecLicense]    Script Date: 03/24/2015 20:23:07 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[PR_EncDecLicense]
	@bEncDec bit WITH ENCRYPTION
AS
DECLARE
@decLicenseKey varchar(400),
@singleItem varchar(200),
@iTemp int
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	OPEN MASTER KEY DECRYPTION BY PASSWORD = 'VSLicense1234!@#$'
	OPEN SYMMETRIC KEY VSLicenseKey DECRYPTION
	BY CERTIFICATE EncryptLicenseKey
	if @bEncDec =0
	begin
		UPDATE dbo.License  SET encUnits = ENCRYPTBYKEY(KEY_GUID('VSLicenseKey'),convert(varchar,InstallType+';' + convert(varchar,Units) +';' + convert(varchar,ExpirationDate,101)) )
		UPDATE ServerTypeLicenses SET EncUnitCost = ENCRYPTBYKEY(KEY_GUID('VSLicenseKey'),convert(varchar,UnitCost) )
	end
	else
	begin
		set @iTemp=0
		select @decLicenseKey=CONVERT(varchar(400), DECRYPTBYKEY(encUnits)) from dbo.License
		UPDATE ServerTypeLicenses SET UnitCost = ISNULL(CONVERT(float,CONVERT(varchar(50), DECRYPTBYKEY(encUnitCost))),1)
		print @decLicenseKey
		WHILE Len(@decLicenseKey) > 0
		begin
			if patindex('%;%',@decLicenseKey) > 0
				begin
				Set @singleItem = substring(@decLicenseKey, 0, patindex('%;%',@decLicenseKey))
				Set @decLicenseKey = substring(@decLicenseKey, patindex('%;%',@decLicenseKey) + 1, Len(@decLicenseKey) - patindex('%;%',@decLicenseKey))
				end
			Else
				begin
				Set @singleItem = @decLicenseKey
				Set @decLicenseKey = ''
				end
				if @iTemp=0
					Update dbo.License set InstallType=@singleItem
				else if @iTemp=1
					Update dbo.License set Units=convert(integer,@singleItem)
				else if @iTemp=2
					Update dbo.License set ExpirationDate =convert(varchar,@singleItem,101)
					
					set @iTemp =@iTemp+1
		end
	end
	
	CLOSE SYMMETRIC KEY VSLicenseKey
CLOSE MASTER KEY
	if @bEncDec =0
		exec dbo.PR_RefreshServerCollection 1
END

GO
USE [VitalSigns]
GO
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[tr_DeleteNodes]') and OBJECTPROPERTY(id, N'IsTrigger') = 1)
	drop trigger [dbo].[tr_DeleteNodes]
GO

/****** Object:  Trigger [dbo].[tr_DeleteNodes]    Script Date: 03/04/2015 14:55:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TRIGGER [dbo].[tr_DeleteNodes]
   ON  [dbo].[Nodes]
   FOR DELETE
AS 
declare
	@nodeId 			int
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
		exec PR_RefreshServerCollection 1

END
GO


USE [VitalSigns]
GO
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[tr_UpdateNodes]') and OBJECTPROPERTY(id, N'IsTrigger') = 1)
	drop trigger [dbo].[tr_UpdateNodes]
GO

/****** Object:  Trigger [dbo].[tr_UpdateNodes]    Script Date: 03/04/2015 14:56:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TRIGGER [dbo].[tr_UpdateNodes]
   ON  [dbo].[Nodes]
   FOR UPDATE
AS 
declare
	@nodeId 			int
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	If UPDATE (NodeTime)
		BEGIN
		SELECT @nodeId=ID from inserted
			update dbo.Nodes set Pulse=GETDATE() where ID=@nodeId 
			exec PR_RefreshServerCollection 0
		END
	

END
GO


USE [VitalSigns]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[tr_InsertRefreshServerCollection]') and OBJECTPROPERTY(id, N'IsTrigger') = 1)
	drop trigger [dbo].[tr_InsertRefreshServerCollection]
GO

CREATE TRIGGER [dbo].[tr_RefreshCollection]
   ON  [dbo].[DeviceInventory]
   FOR INSERT, DELETE
AS 
declare
@sServerType varchar(50),
@sName varchar(150),
@sTypeAndName varchar(200)
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	-- delete the serverinfo from status tables
	select @sServerType =ServerType,@sName=NAME from deleted d,ServerTypes STP where d.DeviceTypeID is not null and d.DeviceTypeID=STP.ID 
	if @sServerType IS NOT NULL AND @sName IS NOT NULL
	begin
	delete from StatusDetail where TypeAndName =(select TypeANDName from status where Name=@sName AND TYPE=@sServerType)
	delete from Status where Name=@sName AND TYPE=@sServerType
	end
    exec PR_RefreshServerCollection 1

END

GO
--  Sowjanya 1564 ticket ---
USE [vitalsigns]
GO


SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING OFF
GO

CREATE TABLE [dbo].[Traveler_Devices_temp](
	[UserName] [nvarchar](255) NULL,
	[DeviceName] [nvarchar](255) NULL,
	[ConnectionState] [nvarchar](255) NULL,
	[LastSyncTime] [datetime] NULL,
	[OS_Type] [nvarchar](255) NULL,
	[Client_Build] [nvarchar](255) NULL,
	[NotificationType] [nvarchar](255) NULL,
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[DocID] [nvarchar](255) NULL,
	[device_type] [nvarchar](255) NULL,
	[Access] [nvarchar](255) NULL,
	[Security_Policy] [nvarchar](255) NULL,
	[wipeRequested] [nvarchar](255) NULL,
	[wipeOptions] [nvarchar](255) NULL,
	[wipeStatus] [nvarchar](255) NULL,
	[SyncType] [nvarchar](255) NULL,
	[wipeSupported] [nvarchar](255) NULL,
	[ServerName] [nvarchar](255) NULL,
	[Approval] [nvarchar](255) NULL,
	[DeviceID] [nvarchar](150) NULL,
	[LastUpdated] [datetime] NULL,
	[MoreDetailsURL] [nvarchar](500) NULL,
	[IsMoreDetailsFetched] [bit] NULL,
	[OS_Type_Min] [nvarchar](255) NULL,
	[HAPoolName] [varchar](150) NULL,
	[IsActive] [bit] NULL
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

/*3/18/15 WS Added for Node Flags */

USE [vitalsigns]
GO


SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[MonitoringTablesToCollections](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[TableName] [nvarchar](255) NULL,
	[ServiceCollectionType] [nvarchar](255) NULL
) ON [PRIMARY]

GO



/****** Object:  Trigger [dbo].[NodeInsertedInits]    Script Date: 03/18/2015 10:19:08 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE TRIGGER [dbo].[NodeInsertedInits] ON [dbo].[Nodes] FOR INSERT AS 
  
	--Handles single services
  
	INSERT INTO NodeDetails (NodeId, Name, Value) 
	select distinct (select ID from InserteD), ServiceCollectionType + ' - UpdateCollection','0' from [MonitoringTablesToCollections] 
	where ServiceCollectionType is not null and not PATINDEX('%,%',ServiceCollectionType) > 0

	--Handles comma seperated services
	
	DECLARE db_cursor CURSOR FOR  
	SELECT ServiceCollectionType
	FROM [MonitoringTablesToCollections]
	WHERE ServiceCollectionType is not null and PATINDEX('%,%',ServiceCollectionType) > 0 
	
	Declare @Services varchar(255)
	Declare @individual varchar(255)
	OPEN db_cursor  
	FETCH NEXT FROM db_cursor INTO @Services  

	WHILE @@FETCH_STATUS = 0  
	BEGIN  
		select @@fetch_status 
		   
		WHILE LEN(@Services) > 0
		BEGIN
			IF PATINDEX('%,%',@Services) > 0
			BEGIN
				SET @individual = SUBSTRING(@Services, 0, PATINDEX('%,%',@Services))
				SET @Services = SUBSTRING(@Services, LEN(@individual + ',') + 1, LEN(@Services))
			END
			ELSE
			BEGIN
				SET @individual = @Services
				SET @Services = NULL
			END
			if not exists (select * from NodeDetails where NodeId = (select ID from InserteD) and Name = @individual + ' - UpdateCollection')
			begin
				INSERT INTO NodeDetails (NodeId, Name, Value) VALUES ((select ID from InserteD), @individual+ ' - UpdateCollection', 0)
			end
		END
		 
		FETCH NEXT FROM db_cursor INTO @Services  
		   
	END  

	CLOSE db_cursor  
	DEALLOCATE db_cursor 
	
	
	



GO

/* 4/14/2015 NS added for VSPLUS-1414 */
USE [vitalsigns]
GO

/****** Object:  Table [dbo].[SystemMessages]    Script Date: 04/14/2015 12:26:45 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[SystemMessages](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Details] [varchar](250) NOT NULL,
	[DateCreated] [datetime] NOT NULL,
	[DateCleared] [datetime] NULL,
 CONSTRAINT [PK_SystemMessages] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

USE [vitalsigns]
GO

/****** Object:  Table [dbo].[UserSystemMessages]    Script Date: 04/14/2015 12:27:42 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[UserSystemMessages](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[SysMsgID] [int] NOT NULL,
	[UserID] [int] NOT NULL,
	[DateDismissed] [datetime] NULL,
	[DateDisplayed] [datetime] NULL,
 CONSTRAINT [PK_UserSystemMessages_1] PRIMARY KEY CLUSTERED 
(
	[SysMsgID] ASC,
	[UserID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[UserSystemMessages]  WITH CHECK ADD  CONSTRAINT [FK_UserSystemMessages_SystemMessages] FOREIGN KEY([SysMsgID])
REFERENCES [dbo].[SystemMessages] ([ID])
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[UserSystemMessages] CHECK CONSTRAINT [FK_UserSystemMessages_SystemMessages]
GO

ALTER TABLE [dbo].[UserSystemMessages]  WITH CHECK ADD  CONSTRAINT [FK_UserSystemMessages_Users] FOREIGN KEY([UserID])
REFERENCES [dbo].[Users] ([ID])
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[UserSystemMessages] CHECK CONSTRAINT [FK_UserSystemMessages_Users]
GO






/* 4/14/2015 NS added for VSPLUS-219 */
USE [vitalsigns]
GO

/****** Object:  Table [dbo].[EscalationDetails]    Script Date: 04/14/2015 12:30:33 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[EscalationDetails](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[AlertKey] [int] NULL,
	[EscalateTo] [varchar](250) NULL,
	[EscalationInterval] [int] NULL,
	[SMSTo] [varchar](250) NULL,
	[ScriptID] [int] NULL,
 CONSTRAINT [PK_EscalationDetails] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

ALTER TABLE [dbo].[EscalationDetails]  WITH CHECK ADD  CONSTRAINT [FK_EscalationDetails_AlertNames] FOREIGN KEY([AlertKey])
REFERENCES [dbo].[AlertNames] ([AlertKey])
GO

ALTER TABLE [dbo].[EscalationDetails] CHECK CONSTRAINT [FK_EscalationDetails_AlertNames]
GO

ALTER TABLE [dbo].[EscalationDetails]  WITH CHECK ADD  CONSTRAINT [FK_EscalationDetails_CustomScripts] FOREIGN KEY([ScriptID])
REFERENCES [dbo].[CustomScripts] ([ID])
GO

ALTER TABLE [dbo].[EscalationDetails] CHECK CONSTRAINT [FK_EscalationDetails_CustomScripts]
GO

USE [vitalsigns]
GO

/****** Object:  Table [dbo].[EscalationSentDetails]    Script Date: 04/14/2015 12:31:02 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[EscalationSentDetails](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[SentTo] [varchar](250) NULL,
	[EscalationCreatedDateTime] [datetime] NULL,
	[AlertHistoryID] [int] NULL,
	[EscalationID] [int] NULL,
 CONSTRAINT [PK_EscalationSentDetails] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

ALTER TABLE [dbo].[EscalationSentDetails]  WITH CHECK ADD  CONSTRAINT [FK_EscalationSentDetails_AlertHistory] FOREIGN KEY([AlertHistoryID])
REFERENCES [dbo].[AlertHistory] ([ID])
GO

ALTER TABLE [dbo].[EscalationSentDetails] CHECK CONSTRAINT [FK_EscalationSentDetails_AlertHistory]
GO

--VSPLUS 596 DURGA Adding a tab Business Hours In Server Settings Editor
--VSPLUS 1768 Durga
/****** Object:  StoredProcedure [dbo].[BusinessHours]    Script Date: 4/22/2015 6:51:00 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******Mukund:VSPlus-984: Object:  StoredProcedure [dbo].[ServerLocations]    Script Date: 10/11/2014 14:55:39 ******/
CREATE procedure [dbo].[BusinessHours]
(@Page  as VARCHAR(50) ,@control as  VARCHAR(50)) as
Begin
declare @SrvLocations Table
(id int, LocId int, Name varchar(100),actid int,tbl varchar(50),srvtypeid int,ServerType varchar(100),description varchar(100),Businesshours varchar(100))

insert into @SrvLocations select id,null,Location,id,'Locations',null,null,null,null from Locations 

Declare @count int
select @count=MAX(id) from Locations


Declare @ID int
Declare @LocationName varchar(100)
Declare @LocationID int
Declare @ServerTypeId int
Declare @ServerType varchar(100)
Declare @description varchar(100)
Declare @Businesshours varchar(100)


DECLARE db_cursor CURSOR FOR  
select sr.ID,sr.ServerName,sr.LocationID,sr.ServerTypeId,srt.ServerType,sr.description,hi.Type from Servers sr,
ServerTypes srt,SelectedFeatures ft,HoursIndicator hi  where sr.ServerTypeId=srt.id and ft.FeatureId=srt.FeatureId and hi.ID=sr.BusinesshoursID and sr.ServerTypeID not in (select ServertypeID from 

Servertypeexcludelist where Page=@Page and 

Control=@control)
union
select sr.ID,sr.Name as ServerName,sr.LocationID,sr.ServerTypeId,srt.ServerType, sr.TheURL,sr.Category
from URLs sr,ServerTypes srt,SelectedFeatures ft  where sr.ServerTypeId=srt.id  and ft.FeatureId=srt.FeatureId and sr.ServerTypeId  not in (select ServertypeID from 

Servertypeexcludelist where Page=@Page and 

Control=@control)
order by sr.LocationID,sr.ServerName

OPEN db_cursor   
FETCH NEXT FROM db_cursor into @ID,@LocationName,@LocationID,@ServerTypeId,@ServerType,@description,@Businesshours

WHILE @@FETCH_STATUS = 0   
BEGIN   
	Set @count=@count+1
	insert into @SrvLocations values(@count,@LocationID,@LocationName,@id,'Servers',@ServerTypeId,@ServerType,@description,@Businesshours)
	FETCH NEXT FROM db_cursor into @ID,@LocationName,@LocationID,@ServerTypeId,@ServerType,@description,@Businesshours
END   

CLOSE db_cursor   
DEALLOCATE db_cursor

select * from @SrvLocations order by tbl,Name
end

GO

--VSPLUS 1655 Sowjanya
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[O365Nodes](
	[O365ServerID] [int] NOT NULL,
	[NodeID] [int] NOT NULL
) ON [PRIMARY]

GO

/* 4/27/2015 NS added for VSPLUS-1686 */
USE [vitalsigns]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Natallya Shkarayeva
-- Create date: 4/27/2015, VSPLUS-1686
-- Description:	Update trigger will add/remove a Domino Server to/from the DeviceInventory table
-- based on the value of Enabled.
-- =============================================
CREATE TRIGGER [dbo].[tr_UpdateDeviceInventoryDominoServers]
   ON  [dbo].[DominoServers]
   AFTER UPDATE
AS 
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for trigger here
	IF UPDATE([Enabled])
	BEGIN
		IF (SELECT [Enabled] FROM Deleted) = 0 AND (SELECT [Enabled] FROM Inserted) = 1
		BEGIN
			INSERT INTO DeviceInventory
			(Name, DeviceID, DeviceTypeID, LocationID)
			SELECT
				s.ServerName, ServerID, 1, s.LocationID
				FROM inserted i INNER JOIN Servers s ON i.ServerID=s.ID
				WHERE i.[Enabled] = 1
		END
		ELSE
		 IF (SELECT [Enabled] FROM Inserted) = 0 AND (SELECT [Enabled] FROM Deleted) = 1
		 BEGIN
			DELETE d FROM DeviceInventory AS d INNER JOIN Inserted AS i ON d.DeviceID = i.ServerID
			AND  d.DeviceTypeID=1 AND i.[Enabled] = 0
		 END	
	END
END
GO

USE [vitalsigns]
GO

/****** Object:  Trigger [dbo].[tr_UpdateDeviceInventorySametimeServers]    Script Date: 04/28/2015 13:33:52 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Natallya Shkarayeva
-- Create date: 4/28/2015
-- Description:	VSPLUS-1686
-- =============================================
CREATE TRIGGER [dbo].[tr_UpdateDeviceInventorySametimeServers]
   ON  [dbo].[SametimeServers]
   AFTER UPDATE
AS 
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for trigger here
	IF UPDATE([Enabled])
	BEGIN
		IF (SELECT [Enabled] FROM Deleted) = 0 AND (SELECT [Enabled] FROM Inserted) = 1
		BEGIN
			INSERT INTO DeviceInventory
			(Name, DeviceID, DeviceTypeID, LocationID)
			SELECT
				s.ServerName, ServerID, 3, s.LocationID
				FROM inserted i INNER JOIN Servers s ON i.ServerID=s.ID
				WHERE i.[Enabled] = 1
		END
		ELSE
		 IF (SELECT [Enabled] FROM Inserted) = 0 AND (SELECT [Enabled] FROM Deleted) = 1
		 BEGIN
			DELETE d FROM DeviceInventory AS d INNER JOIN Inserted AS i ON d.DeviceID = i.ServerID
			AND  d.DeviceTypeID=3 AND i.[Enabled] = 0
		 END
	END
END

GO

USE [vitalsigns]
GO

/****** Object:  Trigger [dbo].[tr_UpdateDeviceInventoryServerAttributes]    Script Date: 04/28/2015 13:33:52 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Natallya Shkarayeva
-- Create date: 4/28/2015
-- Description:	VSPLUS-1686
-- =============================================
CREATE TRIGGER [dbo].[tr_UpdateDeviceInventoryServerAttributes]
   ON  [dbo].[ServerAttributes]
   AFTER UPDATE
AS 
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for trigger here
	IF UPDATE([Enabled])
	BEGIN
		IF (SELECT [Enabled] FROM Deleted) = 0 AND (SELECT [Enabled] FROM Inserted) = 1
		BEGIN
			INSERT INTO DeviceInventory
			(Name, DeviceID, DeviceTypeID, LocationID)
			SELECT
				s.ServerName, ServerID, s.ServerTypeID, s.LocationID
				FROM inserted i INNER JOIN Servers s ON i.ServerID=s.ID
				WHERE i.[Enabled] = 1
		END
		ELSE
		 IF (SELECT [Enabled] FROM Inserted) = 0 AND (SELECT [Enabled] FROM Deleted) = 1
		 BEGIN
			DELETE d FROM DeviceInventory AS d INNER JOIN Servers s ON d.DeviceID=s.ID
			AND d.DeviceTypeID=s.ServerTypeID INNER JOIN Inserted AS i ON d.DeviceID = i.ServerID
			AND i.[Enabled] = 0
		 END	
	END
END
GO

-- =============================================
-- Author:		Natallya Shkarayeva
-- Create date: 4/29/2015
-- Description:	VSPLUS-1686
-- =============================================
CREATE TRIGGER [dbo].[tr_UpdateDeviceInventoryO365Server]
   ON  [dbo].[O365Server]
   AFTER UPDATE
AS 
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	-- Insert statements for trigger here
	IF UPDATE([Enabled])
	BEGIN
		IF (SELECT [Enabled] FROM Deleted) = 0 AND (SELECT [Enabled] FROM Inserted) = 1
		BEGIN
			INSERT INTO DeviceInventory
			(Name, DeviceID, DeviceTypeID, LocationID)
			SELECT
				Name, ID, 21, NULL
			FROM inserted
				WHERE inserted.[Enabled] = 1	
		END
		ELSE
		 IF (SELECT [Enabled] FROM Inserted) = 0 AND (SELECT [Enabled] FROM Deleted) = 1
		 BEGIN
			DELETE d FROM DeviceInventory AS d INNER JOIN Inserted AS i ON d.DeviceID = i.ID
			AND  d.DeviceTypeID=21 AND i.[Enabled] = 0
		 END	
	END
END

GO

-- =============================================
-- Author:		Natallya Shkarayeva
-- Create date: 4/29/2015
-- Description:	VSPLUS-1686
-- =============================================
CREATE TRIGGER [dbo].[tr_InsertDeviceInventoryO365Server]
   ON  [dbo].[O365Server]
   FOR INSERT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for trigger here
	INSERT INTO DeviceInventory
		(Name, DeviceID, DeviceTypeID, LocationID)
	SELECT
		Name, ID, 21, NULL
	FROM inserted i where i.Enabled =1
END

GO

USE [vitalsigns]
GO

/****** Object:  Trigger [dbo].[tr_UpdateDeviceInventoryWindowsServers]    Script Date: 05/01/2015 11:10:49 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Natallya Shkarayeva
-- Create date: 5/1/2015
-- Description:	VSPLUS-1686
-- =============================================
CREATE TRIGGER [dbo].[tr_UpdateDeviceInventoryWindowsServers]
   ON  [dbo].[WindowsServers]
   AFTER UPDATE
AS 
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	IF UPDATE([Enabled])
	BEGIN
		IF (SELECT [Enabled] FROM Deleted) = 0 AND (SELECT [Enabled] FROM Inserted) = 1
		BEGIN
			INSERT INTO DeviceInventory
			(Name, DeviceID, DeviceTypeID, LocationID)
			SELECT
				s.ServerName, ServerID, 16, s.LocationID
				FROM inserted i INNER JOIN Servers s ON i.ServerID=s.ID
				WHERE i.[Enabled] = 1
		END
		ELSE
		 IF (SELECT [Enabled] FROM Inserted) = 0 AND (SELECT [Enabled] FROM Deleted) = 1
		 BEGIN
			DELETE d FROM DeviceInventory AS d INNER JOIN Inserted AS i ON d.DeviceID = i.ServerID
			AND  d.DeviceTypeID=16 AND i.[Enabled] = 0
		 END
	END
END

GO
--VSPLUS 1704 Sowjanya
/****** Object:  Table [dbo].[VS_AssemblyVersionInfo]    Script Date: 5/5/2015 6:32:23 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[VS_AssemblyVersionInfo](
	[AssemblyName] [varchar](50) NULL,
	[AssemblyVersion] [nvarchar](50) NULL,
	[ProductVersion] [nvarchar](50) NULL,
	[BuildDate] [datetime] NULL,
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[FileArea] [nvarchar](50) NULL,
	[NodeName] [nvarchar](255) NULL
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO

ALTER TABLE [dbo].[VS_AssemblyVersionInfo]  WITH CHECK ADD  CONSTRAINT [fk_VS_AssemblyVersionInfo_NodeName_Node_Name] FOREIGN KEY([NodeName])
REFERENCES [dbo].[Nodes] ([Name])
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[VS_AssemblyVersionInfo] CHECK CONSTRAINT [fk_VS_AssemblyVersionInfo_NodeName_Node_Name]
GO

/* 5/7/2015 NS added for VSPLUS-1622 */
USE [vitalsigns]
GO

/****** Object:  StoredProcedure [dbo].[GetAlertHistoryByAlertKey]    Script Date: 05/07/2015 11:37:15 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Natallya Shkarayeva
-- Create date: 5/6/2015
-- Description:	the stored procedure gets alert history information based on the alert definition key
-- parameter. If the parameter value is 0, all alert history table records are returned.
-- Modified date: 2/12/2016 for VSPLUS-2578
-- =============================================
CREATE PROCEDURE [dbo].[GetAlertHistoryByAlertKey](@AlertKey varchar(10)) 
AS
DECLARE @GetAll as int
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	IF @AlertKey = 0
	BEGIN
		SELECT * FROM AlertHistory
	END
	ELSE
	BEGIN
		select distinct * from
		(select t10.*, t1.AlertKey
		from AlertDetails t1 inner join AlertNames t2 on t1.AlertKey=t2.AlertKey
		left outer join CustomScripts t7 on t1.ScriptID=t7.ID
		inner join AlertEvents t6 on t1.AlertKey=t6.AlertKey
		inner join AlertServers t5 on t1.AlertKey=t5.AlertKey
		inner join DeviceInventory t4 on t6.ServerTypeID=t4.DeviceTypeID
		inner join ServerTypes t3 on t4.DeviceTypeID=t3.ID
		inner join EventsMaster t8 on t6.EventID=t8.ID 
		inner join AlertHistory t10 on t4.Name=t10.DeviceName and t3.ServerType=t10.DeviceType
		and t10.AlertType LIKE t8.EventName + '%'
		where t5.ServerTypeID=0 and t5.ServerID=0 and t5.LocationID=0
		union
		select t10.*, t1.AlertKey
		from AlertDetails t1 inner join AlertNames t2 on t1.AlertKey=t2.AlertKey
		left outer join CustomScripts t7 on t1.ScriptID=t7.ID
		inner join AlertEvents t6 on t1.AlertKey=t6.AlertKey
		inner join AlertServers t5 on t1.AlertKey=t5.AlertKey
		inner join DeviceInventory t4 on t6.ServerTypeID=t4.DeviceTypeID
		inner join ServerTypes t3 on t4.DeviceTypeID=t3.ID
		inner join EventsMaster t8 on t8.ServerTypeID=t6.ServerTypeID
		inner join AlertHistory t10 on t4.Name=t10.DeviceName and t3.ServerType=t10.DeviceType
		and t10.AlertType LIKE t8.EventName + '%'
		where t5.ServerTypeID=0 and t5.ServerID=0 and t5.LocationID=0 and t6.EventID=0
		union
		select t10.*, t1.AlertKey
		from AlertDetails t1 inner join AlertNames t2 on t1.AlertKey=t2.AlertKey
		left outer join CustomScripts t7 on t1.ScriptID=t7.ID
		inner join AlertEvents t6 on t1.AlertKey=t6.AlertKey
		inner join AlertServers t5 on t1.AlertKey=t5.AlertKey,
		DeviceInventory t4 
		inner join ServerTypes t3 on t4.DeviceTypeID=t3.ID
		inner join EventsMaster t8 on t4.DeviceTypeID=t8.ServerTypeID
		inner join AlertHistory t10 on t4.Name=t10.DeviceName and t3.ServerType=t10.DeviceType
		and t10.AlertType LIKE t8.EventName + '%'
		where t6.EventID=0 and t6.ServerTypeID=0 and t5.ServerTypeID=0 and t5.ServerID=0 and t5.LocationID=0
		union
		select t10.*, t1.AlertKey
		from AlertDetails t1 inner join AlertNames t2 on t1.AlertKey=t2.AlertKey
		left outer join CustomScripts t7 on t1.ScriptID=t7.ID
		inner join AlertEvents t6 on t1.AlertKey=t6.AlertKey
		inner join AlertServers t5 on t1.AlertKey=t5.AlertKey
		inner join Locations t9 on t5.LocationID=t9.ID
		inner join DeviceInventory t4 on t9.ID=t4.LocationID
		inner join ServerTypes t3 on t4.DeviceTypeID=t3.ID
		inner join EventsMaster t8 on t4.DeviceTypeID=t8.ServerTypeID
		inner join AlertHistory t10 on t4.Name=t10.DeviceName and t3.ServerType=t10.DeviceType
		and t10.AlertType LIKE t8.EventName + '%'
		where t6.EventID=0 and t6.ServerTypeID=0 and t5.ServerTypeID=0
		union
		select t10.*, t1.AlertKey
		from AlertDetails t1 inner join AlertNames t2 on t1.AlertKey=t2.AlertKey
		left outer join CustomScripts t7 on t1.ScriptID=t7.ID
		inner join AlertEvents t6 on t1.AlertKey=t6.AlertKey
		inner join AlertServers t5 on t1.AlertKey=t5.AlertKey
		inner join DeviceInventory t4 on t4.LocationID IS NULL
		inner join ServerTypes t3 on t4.DeviceTypeID=t3.ID
		inner join EventsMaster t8 on t4.DeviceTypeID=t8.ServerTypeID
		inner join AlertHistory t10 on t4.Name=t10.DeviceName and t3.ServerType=t10.DeviceType
		and t10.AlertType LIKE t8.EventName + '%'
		where t6.EventID=0 and t6.ServerTypeID=0 and t5.ServerTypeID=0
		union
		select t10.*, t1.AlertKey
		from AlertDetails t1 inner join AlertNames t2 on t1.AlertKey=t2.AlertKey
		left outer join CustomScripts t7 on t1.ScriptID=t7.ID
		inner join AlertEvents t6 on t1.AlertKey=t6.AlertKey
		inner join AlertServers t5 on t1.AlertKey=t5.AlertKey
		inner join Locations t9 on t5.LocationID=t9.ID
		inner join DeviceInventory t4 on t9.ID=t4.LocationID
		inner join ServerTypes t3 on t4.DeviceTypeID=t3.ID
		inner join EventsMaster t8 on t4.DeviceTypeID=t8.ServerTypeID
		inner join AlertHistory t10 on t4.Name=t10.DeviceName and t3.ServerType=t10.DeviceType
		and t10.AlertType LIKE t8.EventName + '%'
		where t6.ServerTypeID=t4.DeviceTypeID and t6.EventID=0 and t5.ServerTypeID=0
		union
		select t10.*, t1.AlertKey
		from AlertDetails t1 inner join AlertNames t2 on t1.AlertKey=t2.AlertKey
		left outer join CustomScripts t7 on t1.ScriptID=t7.ID
		inner join AlertEvents t6 on t1.AlertKey=t6.AlertKey
		inner join AlertServers t5 on t1.AlertKey=t5.AlertKey
		inner join DeviceInventory t4 on t4.LocationID IS NULL
		inner join ServerTypes t3 on t4.DeviceTypeID=t3.ID
		inner join EventsMaster t8 on t4.DeviceTypeID=t8.ServerTypeID 
		inner join AlertHistory t10 on t4.Name=t10.DeviceName and t3.ServerType=t10.DeviceType
		and t10.AlertType LIKE t8.EventName + '%'
		where t6.ServerTypeID=t4.DeviceTypeID and t6.EventID=0 and t5.ServerTypeID=0
		union
		select t10.*, t1.AlertKey
		from AlertDetails t1 inner join AlertNames t2 on t1.AlertKey=t2.AlertKey
		left outer join CustomScripts t7 on t1.ScriptID=t7.ID
		inner join AlertEvents t6 on t1.AlertKey=t6.AlertKey
		inner join AlertServers t5 on t1.AlertKey=t5.AlertKey
		inner join Locations t9 on t5.LocationID=t9.ID
		inner join DeviceInventory t4 on t9.ID=t4.LocationID and t5.ServerID=t4.DeviceID
		inner join ServerTypes t3 on t4.DeviceTypeID=t3.ID
		inner join EventsMaster t8 on t4.DeviceTypeID=t8.ServerTypeID
		inner join AlertHistory t10 on t4.Name=t10.DeviceName and t3.ServerType=t10.DeviceType
		and t10.AlertType LIKE t8.EventName + '%'
		where t6.ServerTypeID=t4.DeviceTypeID and t5.LocationID=t4.LocationID and t6.EventID=t8.ID
		union
		select t10.*, t1.AlertKey
		from AlertDetails t1 inner join AlertNames t2 on t1.AlertKey=t2.AlertKey
		left outer join CustomScripts t7 on t1.ScriptID=t7.ID
		inner join AlertEvents t6 on t1.AlertKey=t6.AlertKey
		inner join AlertServers t5 on t1.AlertKey=t5.AlertKey
		inner join DeviceInventory t4 on t4.LocationID IS NULL
		inner join ServerTypes t3 on t4.DeviceTypeID=t3.ID
		inner join EventsMaster t8 on t4.DeviceTypeID=t8.ServerTypeID
		inner join AlertHistory t10 on t4.Name=t10.DeviceName and t3.ServerType=t10.DeviceType
		and t10.AlertType LIKE t8.EventName + '%'
		where t6.ServerTypeID=t4.DeviceTypeID and t6.EventID=t8.ID
		union
		select t10.*, t1.AlertKey
		from AlertDetails t1 inner join AlertNames t2 on t1.AlertKey=t2.AlertKey
		left outer join CustomScripts t7 on t1.ScriptID=t7.ID
		inner join AlertEvents t6 on t1.AlertKey=t6.AlertKey
		inner join AlertServers t5 on t1.AlertKey=t5.AlertKey
		inner join Locations t9 on t5.LocationID=t9.ID
		inner join DeviceInventory t4 on t9.ID=t4.LocationID
		inner join ServerTypes t3 on t4.DeviceTypeID=t3.ID
		inner join EventsMaster t8 on t4.DeviceTypeID=t8.ServerTypeID
		inner join AlertHistory t10 on t4.Name=t10.DeviceName and t3.ServerType=t10.DeviceType
		and t10.AlertType LIKE t8.EventName + '%'
		where t6.ServerTypeID=t4.DeviceTypeID and t6.EventID=t8.ID and t5.ServerID=0
		union
		select t10.*, t1.AlertKey
		from AlertDetails t1 inner join AlertNames t2 on t1.AlertKey=t2.AlertKey
		left outer join CustomScripts t7 on t1.ScriptID=t7.ID
		inner join AlertEvents t6 on t1.AlertKey=t6.AlertKey
		inner join AlertServers t5 on t1.AlertKey=t5.AlertKey
		inner join DeviceInventory t4 on t4.LocationID IS NULL
		inner join ServerTypes t3 on t4.DeviceTypeID=t3.ID
		inner join EventsMaster t8 on t4.DeviceTypeID=t8.ServerTypeID
		inner join AlertHistory t10 on t4.Name=t10.DeviceName and t3.ServerType=t10.DeviceType
		and t10.AlertType LIKE t8.EventName + '%'
		where t6.ServerTypeID=t4.DeviceTypeID and t6.EventID=t8.ID and t5.ServerID = 0
		union
		select t10.*, t1.AlertKey
		from AlertDetails t1 inner join AlertNames t2 on t1.AlertKey=t2.AlertKey
		left outer join CustomScripts t7 on t1.ScriptID=t7.ID
		inner join AlertEvents t6 on t1.AlertKey=t6.AlertKey
		inner join AlertServers t5 on t1.AlertKey=t5.AlertKey
		inner join Locations t9 on t5.LocationID=t9.ID
		inner join DeviceInventory t4 on t9.ID=t4.LocationID
		inner join ServerTypes t3 on t4.DeviceTypeID=t3.ID
		inner join EventsMaster t8 on t4.DeviceTypeID=t8.ServerTypeID
		inner join AlertHistory t10 on t4.Name=t10.DeviceName and t3.ServerType=t10.DeviceType
		and t10.AlertType LIKE t8.EventName + '%'
		where t6.ServerTypeID=t4.DeviceTypeID and t5.ServerID=t4.DeviceID and t6.EventID=0
		union
		select t10.*, t1.AlertKey
		from AlertDetails t1 inner join AlertNames t2 on t1.AlertKey=t2.AlertKey
		left outer join CustomScripts t7 on t1.ScriptID=t7.ID
		inner join AlertEvents t6 on t1.AlertKey=t6.AlertKey
		inner join AlertServers t5 on t1.AlertKey=t5.AlertKey
		inner join DeviceInventory t4 on t4.LocationID IS NULL
		inner join ServerTypes t3 on t4.DeviceTypeID=t3.ID
		inner join EventsMaster t8 on t4.DeviceTypeID=t8.ServerTypeID
		inner join AlertHistory t10 on t4.Name=t10.DeviceName and t3.ServerType=t10.DeviceType
		and t10.AlertType LIKE t8.EventName + '%'
		where t6.ServerTypeID=t4.DeviceTypeID and t6.EventID=0
		union
		select distinct t10.*, t1.AlertKey
		from AlertDetails t1 inner join AlertNames t2 on t1.AlertKey=t2.AlertKey
		left outer join CustomScripts t7 on t1.ScriptID=t7.ID
		inner join AlertEvents t6 on t1.AlertKey=t6.AlertKey
		inner join AlertServers t5 on t1.AlertKey=t5.AlertKey
		inner join Locations t9 on t5.LocationID=t9.ID
		inner join DeviceInventory t4 on t9.ID=t4.LocationID and t5.ServerID=t4.DeviceID 
		inner join ServerTypes t3 on t4.DeviceTypeID=t3.ID
		inner join EventsMaster t8 on t4.DeviceTypeID=t8.ServerTypeID
		inner join AlertHistory t10 on t4.Name=t10.DeviceName and t3.ServerType=t10.DeviceType
		and t10.AlertType LIKE t8.EventName + '%'
		where t6.EventID=0 and t6.ServerTypeID=0
		union
		select t10.*, t1.AlertKey
		from AlertDetails t1 inner join AlertNames t2 on t1.AlertKey=t2.AlertKey
		left outer join CustomScripts t7 on t1.ScriptID=t7.ID
		inner join AlertEvents t6 on t1.AlertKey=t6.AlertKey
		inner join AlertServers t5 on t1.AlertKey=t5.AlertKey
		inner join DeviceInventory t4 on t4.LocationID IS NULL
		inner join ServerTypes t3 on t4.DeviceTypeID=t3.ID
		inner join EventsMaster t8 on t4.DeviceTypeID=t8.ServerTypeID
		inner join AlertHistory t10 on t4.Name=t10.DeviceName and t3.ServerType=t10.DeviceType
		and t10.AlertType LIKE t8.EventName + '%'
		where t6.EventID=0 and t6.ServerTypeID=0
		) as atmp
		where atmp.AlertKey=@AlertKey or @GetAll = 1
	END
END

GO

--VSPLUS-1746 Sowjanya
create table TravelerStatusReasons(
[ID] int Identity(1,1) not null primary key,
[ServerName] [nvarchar](150) NOT NULL,
[Details] [varchar](500) NULL,
[LastUpdate] [datetime] NULL
)
GO

/* 5/15/15 WS Added - VSPLUS-1766 */

ALTER TABLE [dbo].[Servers]  WITH CHECK ADD FOREIGN KEY([BusinesshoursID])
REFERENCES [dbo].[HoursIndicator] ([ID])
GO

/* 5/29/15 WS Added - VSPLUS-1789 */

USE [vitalsigns]
GO

/****** Object:  StoredProcedure [dbo].[GetMaxDominoScanIntervalByHours]    Script Date: 05/29/2015 13:49:44 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO



CREATE PROCEDURE [dbo].[GetMaxDominoScanIntervalByHours]   
AS BEGIN

  
	declare @dayOfWeek varchar(50), @query varchar(max)
	select @dayOfWeek = DateName(dw, getdate())

	set @query='

	

	select max
	( 
		Case
			When ScanToday = 0 then OffHoursScanInterval
				
			When Cast(GetDate() as Date) <> Cast(EndDateTime as Date) then 
			(
				Case
					When GetDate() > dateadd(day, -1, StartDateTime) and GetDate() < dateadd(day, -1, EndDateTime) Then ScanInterval
					Else OffHoursScanInterval
				End
			)
			Else
			(
				Case
					When GetDate() > StartDateTime and GetDate() < EndDateTime Then ScanInterval
					Else OffHoursScanInterval
				End
			)
		end
	) as MaxScanInterval

	from 
	(
		select StartDateTime, (DateAdd(minute, Duration, StartDateTime)) as EndDateTime, StartTime, Duration, ScanInterval, OffHoursScanInterval, ScanToday
		from
		(
			select Cast(DATEADD(day, DATEDIFF(day,''19000101'',getDate()), CAST(StartTime AS DATETIME)) as DateTime2(0)) as StartDateTime , 
			StartTime, Duration, ds.[Scan Interval] ScanInterval, ds.OffHoursScanInterval, hr.is' + @dayOfWeek + ' ScanToday
			from Servers sr
			inner join DominoServers ds on sr.id=ds.ServerID and ds.Enabled=1
			inner join HoursIndicator hr on hr.id=sr.BusinessHoursId
		) tbl
	) tbl2
	


	'

	exec(@query)

  
  
  
END



GO


/* 6/2/2015 NS added for VSPLUS-1661 */
USE [vitalsigns]
GO

/****** Object:  Table [dbo].[FeatureReports]    Script Date: 05/18/2015 13:45:38 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[FeatureReports](
	[FeatureID] [int] NULL,
	[ReportID] [int] NULL
) ON [PRIMARY]

GO



/* 6/8/15 WS added for VSPLUS 1816 */

USE [vitalsigns]
GO

/****** Object:  StoredProcedure [dbo].[InBusinessHoursByServer]    Script Date: 06/08/2015 17:10:37 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[InBusinessHoursByServer]
@ServerName varchar(100)
AS BEGIN
	
	-- This SP will return 1 if the server is in business hours or 0 if it is not

	declare @query varchar(max)

	set @query = '

	CREATE TABLE #TempTable (
		StartDateTime 			DateTime2(0),
		StartTime				Time,
		Duration				int,
		ScanToday				bit,
		ScanYesterday			bit
	)

	insert into #TempTable (StartDateTime, StartTime, Duration, ScanYesterday, ScanToday) 
	select Cast(DATEADD(day, DATEDIFF(day,''19000101'',getDate()), CAST(StartTime AS DATETIME)) as DateTime2(0)) as StartDateTime , 
	StartTime, Duration, 
	hr.is' + (select DateName(dw, DateAdd(day, -1, getdate()))) + ' ScanYesterday, 
	hr.is' + (select DateName(dw, DateAdd(day, 0, getdate()))) + ' ScanToday
	from Servers sr
	inner join HoursIndicator hr on hr.id=sr.BusinessHoursId
	where  sr.ServerName = ''' + @ServerName + '''

	select count(*) InBusinessHours
	From
	(
		select StartDateTime, DateAdd(minute, Duration, StartDateTime) EndDateTime from #TempTable where ScanToday=1
		union
		select DateAdd(day, -1, StartDateTime) StartDateTime, DateAdd(minute, Duration, DateAdd(day, -1, StartDateTime)) EndDateTime from #TempTable where ScanYesterday=1
	) tbl where StartDateTime < getDate() and getDate() < EndDateTime





	drop table #TempTable

	'

	exec(@query)
End


GO

/* 6/25/2015 NS added for VSPLUS-1802 */
/* 10/9/2015 NS modified for VSPLUS-2196 */
USE [vitalsigns]
GO
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[EXJournalStats](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[EXJournalDB] [varchar](50) NOT NULL,
	[Delta] [int] NOT NULL,
	[DocCount] [int] NOT NULL,
	[DateUpdated] [datetime] NOT NULL,
	[ServerName] [nvarchar](255) NOT NULL,
 CONSTRAINT [PK_EXJournalStats] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO


/* 6/26/2015 NS added for VSPLUS-1802 */
USE [vitalsigns]
GO

/****** Object:  Table [dbo].[DailyCleanup]    Script Date: 06/25/2015 17:05:29 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[DailyCleanup](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[DBName] [varchar](50) NOT NULL,
	[TableName] [varchar](50) NOT NULL,
	[ParameterType] [varchar](50) NULL,
	[Parameter] [varchar](150) NULL,
	[Condition] [varchar](50) NULL,
	[Value] [varchar](150) NULL,
 CONSTRAINT [PK_DailyCleanup] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

USE [VitalSigns]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PR_SetActiveDevices]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[PR_SetActiveDevices]
GO

USE [VitalSigns]
GO

create procedure [dbo].[PR_SetActiveDevices]

as
declare

@nMinRow	integer,
@nMaxRow	integer	,
@Id int,
@sId varchar(max),
@sql varchar(max),
@deviceId varchar(200)

begin

SET NOCOUNT ON

CREATE TABLE #tbDevicesTemp(
	rownum int IDENTITY (1, 1) Primary key NOT NULL,
		DEVICE_ID	varchar(200)
	)


CREATE TABLE #tbDevicesTemp2(
	rownum int IDENTITY (1, 1) Primary key NOT NULL,
		ID INT,
		DEVICE_ID	varchar(200),
		LastSyncTime Date
	)

INSERT INTO #tbDevicesTemp(DEVICE_ID) SELECT distinct deviceid from Traveler_Devices_temp  with (nolock)
INSERT INTO #tbDevicesTemp2(ID,LastSyncTime,DEVICE_ID) SELECT  ID,LastSyncTime,DeviceID   from Traveler_Devices_temp  with (nolock)

set @sId=''
SELECT @nMinRow=MIN(rownum), @nMaxRow=MAX(rownum) FROM #tbDevicesTemp
WHILE @nMinRow <= @nMaxRow
	BEGIN
		select @deviceid=device_id from #tbDevicesTemp where rownum =@nMinRow
		select top 1 @id=ID from #tbDevicesTemp2 with (nolock) where DEVICE_ID=@deviceId order by LastSyncTime desc
		if @sId =''
			set @sId=CONVERT(varchar,@id)
		else
			set @sId += ','+CONVERT(varchar,@id)
			
	set @nMinRow = @nMinRow + 1
	END
	if @sId <> ''
	begin
	set @sql ='update Traveler_Devices_temp set IsActive =1 where ID in ('+@sId+')'
	--PRINT (CONVERT( VARCHAR(24), GETDATE(), 121))
	begin transaction
	update Traveler_Devices_temp set IsActive =0
	--PRINT (CONVERT( VARCHAR(24), GETDATE(), 121))
	execute(@sql)
	commit transaction
	--PRINT (CONVERT( VARCHAR(24), GETDATE(), 121))
	end

DROP TABLE #tbDevicesTemp
DROP TABLE #tbDevicesTemp2
END

go


/* 7/22/2015 NS added for VSPLUS-1562 */
USE [vitalsigns]
GO

/****** Object:  Table [dbo].[AlertEmergencyContacts]    Script Date: 07/22/2015 15:12:20 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[AlertEmergencyContacts](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Email] [nvarchar](150) NOT NULL,
 CONSTRAINT [PK_AlertEmergencyContacts] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO



/*8/21/15 WS modified for Traveler Devices Loop */

USE [vitalsigns]
GO

/****** Object:  UserDefinedTableType [dbo].[Traveler_Temp_Table_TVP]    Script Date: 08/24/2015 15:37:31 ******/
IF  EXISTS (SELECT * FROM sys.types st JOIN sys.schemas ss ON st.schema_id = ss.schema_id WHERE st.name = N'Traveler_Temp_Table_TVP' AND ss.name = N'dbo')
DROP TYPE [dbo].[Traveler_Temp_Table_TVP]
GO

USE [vitalsigns]
GO

/****** Object:  UserDefinedTableType [dbo].[Traveler_Temp_Table_TVP]    Script Date: 08/24/2015 15:37:31 ******/
CREATE TYPE [dbo].[Traveler_Temp_Table_TVP] AS TABLE(
	[UserName] [varchar](255) NULL,
	[Client_Build] [varchar](255) NULL,
	[ServerName] [varchar](255) NULL,
	[DeviceName] [varchar](255) NULL,
	[LastSyncTime] [datetime] NULL,
	[OS_Type] [varchar](255) NULL,
	[OS_Type_Min] [varchar](255) NULL,
	[SyncType] [varchar](255) NULL,
	[MoreDetailsURL] [varchar](255) NULL,
	[LastUpdated] [datetime] NULL,
	[HAPoolName] [varchar](255) NULL,
	[DeviceID] [varchar](255) NULL
)
GO

USE [vitalsigns]
GO

/****** Object:  StoredProcedure [dbo].[MoveTravelerTempTable]    Script Date: 08/21/2015 11:33:01 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[MoveTravelerTempTable]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[MoveTravelerTempTable]
GO

USE [vitalsigns]
GO

/****** Object:  StoredProcedure [dbo].[MoveTravelerTempTable]    Script Date: 08/21/2015 11:33:01 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROC [dbo].[MoveTravelerTempTable](@ServerName varchar(255))

AS BEGIN
	DECLARE @RetryCounter int,@RetryMax int
	
	Set @RetryMax = 5
	SET @RetryCounter = 0
	
	Retry:
	BEGIN TRANSACTION
	BEGIN TRY
		
		if exists(select * from Traveler_Devices_temp WHERE ServerName=@ServerName) Delete from Traveler_Devices  WHERE  ServerName=@ServerName

		INSERT INTO Traveler_Devices (Client_Build, DeviceID, ServerName, UserName, DeviceName, LastSyncTime, OS_Type,  OS_Type_Min, SyncType, MoreDetailsURL,LastUpdated,HAPoolName,ConnectionState,Access,wipeSupported,Security_Policy,Approval,IsMoreDetailsFetched, IsActive) 
		select Client_Build, DeviceID, ServerName, UserName, DeviceName, LastSyncTime, OS_Type,  OS_Type_Min, SyncType, MoreDetailsURL,LastUpdated,HAPoolName,ConnectionState,Access,wipeSupported,Security_Policy,Approval,IsMoreDetailsFetched,IsActive from Traveler_Devices_temp  WHERE ServerName=@ServerName
	COMMIT TRANSACTION;  --<-- If nothing went wrong
	END TRY
	BEGIN CATCH
		ROLLBACK TRANSACTION
		IF ERROR_NUMBER() = 1205
		BEGIN
			SET @RetryCounter = @RetryCounter + 1
			IF (@RetryCounter < @RetryMax)
			BEGIN
				WAITFOR DELAY '00:00:03'
				GOTO Retry
			END
		END
		ELSE
		BEGIN
			DECLARE @ERROR_MESSAGE varchar(max)
			set @ERROR_MESSAGE = ERROR_MESSAGE()
			RAISERROR(@ERROR_MESSAGE, 18, 1)
		END
	END CATCH

END


GO


USE [vitalsigns]
GO

/****** Object:  StoredProcedure [dbo].[UpdateTravelerTempTableTVP]    Script Date: 08/21/2015 11:33:52 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UpdateTravelerTempTableTVP]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[UpdateTravelerTempTableTVP]
GO

USE [vitalsigns]
GO

/****** Object:  StoredProcedure [dbo].[UpdateTravelerTempTableTVP]    Script Date: 08/21/2015 11:33:52 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE  Procedure [dbo].[UpdateTravelerTempTableTVP] 
@tbl   [dbo].[Traveler_Temp_Table_TVP] READONLY
AS 
BEGIN
	SET NOCOUNT ON;
	DECLARE @RetryCounter int,@RetryMax int
	
	Set @RetryMax = 5
	SET @RetryCounter = 0
	
	
	Retry:
	BEGIN TRANSACTION
	BEGIN TRY
	
	
		-- Update Statement 
		UPDATE tempTbl
		SET tempTbl.Client_Build = tbl.Client_Build, 
			tempTbl.DeviceID = tbl.DeviceID, 
			tempTbl.ServerName = tbl.ServerName, 
			tempTbl.UserName = tbl.UserName, 
			tempTbl.DeviceName = tbl.DeviceName, 
			tempTbl.LastSyncTime = tbl.LastSyncTime, 
			tempTbl.OS_Type = tbl.OS_Type,  
			tempTbl.OS_Type_Min = tbl.OS_Type_Min, 
			tempTbl.SyncType = tbl.SyncType, 
			tempTbl.MoreDetailsURL = tbl.MoreDetailsURL, 
			tempTbl.LastUpdated = tbl.LastUpdated, 
			tempTbl.HAPoolName = tbl.HAPoolName
		FROM Traveler_Devices_Temp tempTbl INNER JOIN @tbl tbl
		ON tempTbl.DeviceID = tbl.DeviceID AND tempTbl.ServerName=tbl.ServerName

		-- Insert Statement
		INSERT INTO Traveler_Devices_Temp (Client_Build, DeviceID, ServerName, UserName, DeviceName, 
			LastSyncTime, OS_Type,  OS_Type_Min, SyncType, MoreDetailsURL, LastUpdated, HAPoolName) 
		SELECT Client_Build, DeviceID, ServerName, UserName, DeviceName, 
			LastSyncTime, OS_Type,  OS_Type_Min, SyncType, MoreDetailsURL, LastUpdated, HAPoolName
		FROM @tbl tbl
		WHERE NOT EXISTS (SELECT 1 
						  FROM Traveler_Devices_Temp
						  WHERE DeviceID = tbl.DeviceID AND ServerName = tbl.ServerName)

		COMMIT TRANSACTION;  --<-- If nothing went wrong
	END TRY
	BEGIN CATCH
		ROLLBACK TRANSACTION
		IF ERROR_NUMBER() = 1205
		BEGIN
			SET @RetryCounter = @RetryCounter + 1
			IF (@RetryCounter < @RetryMax)
			BEGIN
				WAITFOR DELAY '00:00:03'
				GOTO Retry
			END
		END
		ELSE
		BEGIN
			DECLARE @ERROR_MESSAGE varchar(max)
			set @ERROR_MESSAGE = ERROR_MESSAGE()
			RAISERROR(@ERROR_MESSAGE, 18, 1)
		END
	END CATCH

END
GO
--VSPLUS 1669 Somaraju
/****** Object:  Table [dbo].[EventsTemplate]    Script Date: 09/03/2015 11:32:14 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[EventsTemplate](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](max) NULL,
	[EventID] [nvarchar](max) NULL
) ON [PRIMARY]

GO
/****** Object:  UserDefinedFunction [dbo].[SplitDelimiterString]    Script Date: 08/14/2015 11:01:24 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

create FUNCTION [dbo].[SplitDelimiterString] (@StringWithDelimiter VARCHAR(8000), @Delimiter VARCHAR )

RETURNS @ItemTable TABLE (Item VARCHAR(8000))

AS
BEGIN
    DECLARE @StartingPosition INT;
    DECLARE @ItemInString VARCHAR(8000);

    SELECT @StartingPosition = 1;
    --Return if string is null or empty
    IF LEN(@StringWithDelimiter) = 0 OR @StringWithDelimiter IS NULL RETURN; 
    
    WHILE @StartingPosition > 0
    BEGIN
        --Get starting index of delimiter .. If string
        --doesn't contain any delimiter than it will returl 0 
        SET @StartingPosition = CHARINDEX(@Delimiter,@StringWithDelimiter); 
        
        --Get item from string        
        IF @StartingPosition > 0                
            SET @ItemInString = SUBSTRING(@StringWithDelimiter,0,@StartingPosition)
        ELSE
            SET @ItemInString = @StringWithDelimiter;
        --If item isn't empty than add to return table    
        IF( LEN(@ItemInString) > 0)
            INSERT INTO @ItemTable(Item) VALUES (@ItemInString);            
        
        --Remove inserted item from string
        SET @StringWithDelimiter = SUBSTRING(@StringWithDelimiter,@StartingPosition + 
                     LEN(@Delimiter),LEN(@StringWithDelimiter) - @StartingPosition)
        
        --Break loop if string is empty
        IF LEN(@StringWithDelimiter) = 0 BREAK;
    END
     
    RETURN
END
GO
/****** Object:  StoredProcedure [dbo].[ServerTypeEventsbytemplate]    Script Date: 08/14/2015 11:00:37 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE procedure [dbo].[ServerTypeEventsbytemplate] 
@IDS  nvarchar(Max)
 AS
Begin

declare @SrvEvents Table
(id int, SrvId int, Name varchar(100),actid int,tbl varchar(50),AlertOnRepeat bit)

insert into @SrvEvents select st.id,null,st.ServerType,st.id,'ServerTypes',0 from ServerTypes st,SelectedFeatures ft 
 where ft.FeatureId=st.FeatureId and st.id in ((SELECT * FROM SplitDelimiterString(@IDS,',')))
-- select id,null,ServerType,id,'ServerTypes' from ServerTypes
--select * from Features ft inner join ServerTypes st on ft.id=st.FeatureId inner join EventsMaster et on st.ID=et.ServerTypeID  
Declare @count int
select @count=MAX(id) from ServerTypes


Declare @ID int
Declare @EventName varchar(100)
Declare @ServerTypeID int
Declare @AlertOnRepeat bit

DECLARE db_cursor CURSOR FOR  
select em.ID,em.EventName,em.ServerTypeID,em.AlertOnRepeat from EventsMaster em,ServerTypes st,SelectedFeatures ft   
where ft.FeatureId=st.FeatureId and em.ServerTypeID =st.id and st.id in((SELECT * FROM SplitDelimiterString(@IDS,',')))
order by em.ServerTypeID,em.EventName
--select ID,EventName,ServerTypeID from EventsMaster order by ServerTypeID,EventName
 

OPEN db_cursor   
FETCH NEXT FROM db_cursor into @ID,@EventName,@ServerTypeID,@AlertOnRepeat

WHILE @@FETCH_STATUS = 0   
BEGIN   
	Set @count=@count+1
	insert into @SrvEvents values(@count,@ServerTypeID,@EventName,@id,'EventsMaster',@AlertOnRepeat)
	FETCH NEXT FROM db_cursor into @ID,@EventName,@ServerTypeID,@AlertOnRepeat
END   

CLOSE db_cursor   
DEALLOCATE db_cursor

select * from @SrvEvents --order by SrvId,Name
order by Name
end

GO
/****** Object:  StoredProcedure [dbo].[Getselectedeventbytemplate]    Script Date: 08/14/2015 10:59:03 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE proc [dbo].[Getselectedeventbytemplate]
@ID  Int
 AS
Begin
select em.*,em.ID as EventID,st.ServerType from EventsMaster em inner join ServerTypes st on em.ServerTypeID=st.ID   where em.ID in (
  SELECT   
     Split.a.value('.', 'VARCHAR(100)') AS String  
 FROM  (SELECT [id],  
         CAST ('<M>' + REPLACE([EventID], ',', '</M><M>') + '</M>' AS XML) AS String  
     FROM  EventsTemplate where Id=@ID   ) AS A CROSS APPLY String.nodes ('/M') AS Split(a))Order By st.ServerType
     End
GO
/****** Object:  StoredProcedure [dbo].[GetselectedeventsbyID]    Script Date: 08/14/2015 10:59:50 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

create proc [dbo].[GetselectedeventsbyID]
 @ID  Int
 AS
Begin
 select*,ID as EventID from EventsMaster where ID in (
  SELECT   
     Split.a.value('.', 'VARCHAR(100)') AS String  
 FROM  (SELECT [id],  
         CAST ('<M>' + REPLACE([EventID], ',', '</M><M>') + '</M>' AS XML) AS String  
     FROM  EventsTemplate  where ID=@ID) AS A CROSS APPLY String.nodes ('/M') AS Split(a))
     End
GO

/*9/30/15 WS altered for VSPLUS-2193 */
USE [vitalsigns]
GO

/****** Object:  Table [dbo].[WebsphereServerDetails]    Script Date: 09/25/2015 14:48:13 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[WebsphereServerDetails](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[ServerID] [int] NOT NULL,
	[ProcessID] [int] NOT NULL,
	[UpTimeSeconds] [int] NOT NULL
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[WebsphereServerDetails]  WITH CHECK ADD  CONSTRAINT [FK_Servers_WebsphereServerDetails] FOREIGN KEY([ServerID])
REFERENCES [dbo].[Servers] ([ID])
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[WebsphereServerDetails] CHECK CONSTRAINT [FK_Servers_WebsphereServerDetails]
GO

/* 10/6/2015 NS added for VSPLUS-2170 */
USE [vitalsigns]
GO

/****** Object:  UserDefinedFunction [dbo].[ConcatUserRestrictions]    Script Date: 09/28/2015 14:45:46 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE FUNCTION [dbo].[ConcatUserRestrictions](@UserId SMALLINT)
RETURNS VARCHAR(MAX) AS
BEGIN
  DECLARE @Servers VARCHAR(MAX)
  DECLARE @Locations VARCHAR(MAX)
  DECLARE @FinalStr VARCHAR(MAX) 
  SELECT @Servers = COALESCE(@Servers + ', ', '') + COALESCE(t5.ServerName,'')
  FROM [Users] t1
	LEFT OUTER JOIN  dbo.UserLocationRestrictions t2 ON t1.ID=t2.UserID LEFT OUTER JOIN dbo.Locations t4 ON
	t2.LocationID=t4.ID
	LEFT OUTER JOIN dbo.UserServerRestrictions t3 ON t1.ID=t3.UserID LEFT OUTER JOIN dbo.Servers t5 ON
	t3.ServerID=t5.ID
  WHERE t1.ID = @UserId 
  GROUP BY t5.ServerName
  SELECT @Locations = COALESCE(@Locations + ', ', '') + COALESCE(t4.Location,'')
  FROM [Users] t1
	LEFT OUTER JOIN  dbo.UserLocationRestrictions t2 ON t1.ID=t2.UserID LEFT OUTER JOIN dbo.Locations t4 ON
	t2.LocationID=t4.ID
	LEFT OUTER JOIN dbo.UserServerRestrictions t3 ON t1.ID=t3.UserID LEFT OUTER JOIN dbo.Servers t5 ON
	t3.ServerID=t5.ID
  WHERE t1.ID = @UserId 
  GROUP BY t4.Location
  IF @Servers != ''
	SELECT @Servers = 'Server(s): ' + @Servers
  IF @Locations != ''
	SELECT @Locations = 'Location(s): ' + @Locations
  IF @Locations != '' AND @Servers != ''
	SELECT @FinalStr = @Locations + ';  ' + @Servers
  ELSE
	IF @Locations != ''
		SELECT @FinalStr = @Locations
	ELSE
		IF @Servers != ''
			SELECT @FinalStr = @Servers
		ELSE
			SELECT @FinalStr = ''
  RETURN @FinalStr
END
GO

/* 10/9/2015 NS added for VSPLUS-2252 */
USE [vitalsigns]
GO

/****** Object:  Table [dbo].[DominoServerDetails]    Script Date: 10/09/2015 15:11:10 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[DominoServerDetails](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[ServerID] [int] NOT NULL,
	[ElapsedTimeSeconds] [int] NOT NULL,
	[VersionArchitecture] [varchar](50) NOT NULL,
	[CPUCount] [int] NOT NULL,
 CONSTRAINT [PK_DominoServerDetails] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

ALTER TABLE [dbo].[DominoServerDetails]  WITH CHECK ADD  CONSTRAINT [FK_DominoServerDetails_Servers] FOREIGN KEY([ServerID])
REFERENCES [dbo].[Servers] ([ID])
GO

ALTER TABLE [dbo].[DominoServerDetails] CHECK CONSTRAINT [FK_DominoServerDetails_Servers]
GO


/*10/26/15 WS Added for VSPLUS-1249*/

USE [vitalsigns]
GO

/****** Object:  Table [dbo].[SharePointTimerJobs]    Script Date: 10/26/2015 13:05:55 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[SharePointTimerJobs](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[JobName] [varchar](250) NOT NULL,
	[ServerName] [nvarchar](250) NULL,
	[WebApplicationName] [nvarchar](250) NULL,
	[Status] [nvarchar](250) NULL,
	[StartTime] [datetime] NULL,
	[EndTime] [datetime] NULL,
	[DataBaseName] [varchar](250) NULL,
	[ErrorMessage] [varchar](250) NULL,
	[Farm] [varchar](250) NULL,
	[Schedule] [varchar](250) NULL
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO



USE [VitalSigns]
GO

IF NOT EXISTS (select * from syscolumns where id = object_id('dbo.ELS'))
BEGIN
CREATE TABLE [dbo].[ELS](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](250) NULL,
 CONSTRAINT [PK_EventNames] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

END
GO

IF NOT EXISTS (select * from syscolumns where id = object_id('dbo.ELSMaster'))
BEGIN
CREATE TABLE [dbo].[ELSMaster](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[EventName] [varchar](254) NOT NULL,
	[Eventid] [varchar](20) NULL,
	[EventKey] [varchar](254) NULL,
	[EventLevel] [varchar](100) NOT NULL,
	[Source] [varchar](254) NULL,
	[TaskCategory] [varchar](100) NULL,
	[AliasName] [varchar](100) NULL
) ON [PRIMARY]
END
GO

IF NOT EXISTS (select * from syscolumns where id = object_id('dbo.ELSDetail'))
BEGIN
CREATE TABLE [dbo].[ELSDetail](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[EventId] [int] NOT NULL,
	[ELSId] [int] NOT NULL,
	[ServerId] [int] NOT NULL,
	[LastScanId] [int] NULL,
	[LocationId] [int] NULL
) ON [PRIMARY]
END
GO

IF NOT EXISTS (select * from syscolumns where id = object_id('dbo.EventHistory'))
BEGIN
CREATE TABLE [dbo].[EventHistory](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[IndexNo] [int] NULL,
	[EventTime] [datetime] NULL,
	[EntryType] [varchar](100) NULL,
	[Source] [varchar](100) NULL,
	[InstanceId] [int] NULL,
	[MessageDetails] [varchar](2000) NULL,
	[DeviceName] [varchar](254) NULL,
	[DeviceType] [varchar](100) NULL,
	[LastUpdated] [datetime] NULL,
	[AliasName] [varchar](100) NULL,
	[LogName] [varchar](100) NULL
) ON [PRIMARY]
END
GO


USE [VitalSigns]
GO

/****** Object:  StoredProcedure [dbo].[ServerLocationsMS]    Script Date: 11/02/2015 12:21:50 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ServerLocationsMS]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[ServerLocationsMS]
GO

USE [VitalSigns]
GO

/****** Object:  StoredProcedure [dbo].[ServerLocationsMS]    Script Date: 11/02/2015 12:21:50 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

/******Mukund:VSPlus-984: Object:  StoredProcedure [dbo].[ServerLocations]    Script Date: 10/11/2014 14:55:39 ******/
CREATE procedure [dbo].[ServerLocationsMS] as
Begin
declare @SrvLocations Table
(id int, LocId int, Name varchar(100),actid int,tbl varchar(50),srvtypeid int,ServerType varchar(100),description varchar(100))

insert into @SrvLocations select id,null,Location,id,'Locations',null,null,null from Locations 

Declare @count int
select @count=MAX(id) from Locations


Declare @ID int
Declare @LocationName varchar(100)
Declare @LocationID int
Declare @ServerTypeId int
Declare @ServerType varchar(100)
Declare @description varchar(100)


DECLARE db_cursor CURSOR FOR  
select sr.ID,sr.ServerName,sr.LocationID,sr.ServerTypeId,srt.ServerType,sr.description from Servers sr,
ServerTypes srt,SelectedFeatures ft  where sr.ServerTypeId=srt.id and ft.FeatureId=srt.FeatureId and srt.ServerType in('SharePoint','Exchange','Windows','Active Directory')
union
select sr.ID,sr.Name as ServerName,sr.LocationID,sr.ServerTypeId,srt.ServerType, sr.TheURL
from URLs sr,ServerTypes srt,SelectedFeatures ft   where sr.ServerTypeId=srt.id  and ft.FeatureId=srt.FeatureId and srt.ServerType in('SharePoint','Exchange','Windows','Active Directory')
order by sr.LocationID,sr.ServerName

OPEN db_cursor   
FETCH NEXT FROM db_cursor into @ID,@LocationName,@LocationID,@ServerTypeId,@ServerType,@description

WHILE @@FETCH_STATUS = 0   
BEGIN   
	Set @count=@count+1
	insert into @SrvLocations values(@count,@LocationID,@LocationName,@id,'Servers',@ServerTypeId,@ServerType,@description)
	FETCH NEXT FROM db_cursor into @ID,@LocationName,@LocationID,@ServerTypeId,@ServerType,@description
END   

CLOSE db_cursor   
DEALLOCATE db_cursor

select * from @SrvLocations order by tbl,Name
end

GO


--Somaraju VSPLUS 2284
/****** Object:  View [dbo].[viewdiskspace]    Script Date: 11/18/2015 08:38:31 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE view [dbo].[viewdiskspace]
as
SELECT DISTINCT ddsp.[ServerName],
                ddsp.[DiskName],
                dds.[DiskName] ddsdiskname,
                ddsp.[DiskFree],
                ddsp.[DiskSize],
                ddsp.[PercentFree],
                ddsp.[PercentUtilization],
                dds.threshold Threshold,
                dds.ThresholdType
FROM DominoDiskSpace ddsp
INNER JOIN DominoDiskSettings dds ON ddsp.servername=dds.servername
WHERE dds.diskname='AllDisks'
  OR dds.diskname='NoAlerts'
UNION
SELECT DISTINCT ddsp.[ServerName],
                ddsp.[DiskName],
                dds.[DiskName] ddsdiskname,
                ddsp.[DiskFree],
                ddsp.[DiskSize],
                ddsp.[PercentFree],
                ddsp.[PercentUtilization],
                dds.threshold,
                dds.ThresholdType
FROM DiskSpace ddsp
INNER JOIN servers S On S.ServerName=ddsp.ServerName 
INNER JOIN ServerTypes ST On ST.ServerType=ddsp.ServerType 
INNER JOIN DiskSettings dds ON dds.ServerID=s.ID
WHERE dds.diskname='AllDisks'
  OR dds.diskname='NoAlerts'
UNION
SELECT DISTINCT ddsp.[ServerName],
                ddsp.[DiskName],
                dds.[DiskName] ddsdiskname,
                ddsp.[DiskFree],
                ddsp.[DiskSize],
                ddsp.[PercentFree],
                ddsp.[PercentUtilization],
                dds.threshold,
                dds.ThresholdType
FROM DominoDiskSpace ddsp
INNER JOIN DominoDiskSettings dds ON ddsp.servername=dds.servername
WHERE dds.diskname=ddsp.diskname
UNION
SELECT DISTINCT ddsp.[ServerName],
                ddsp.[DiskName],
                dds.[DiskName] ddsdiskname,
                ddsp.[DiskFree],
                ddsp.[DiskSize],
                ddsp.[PercentFree],
                ddsp.[PercentUtilization],
                dds.threshold,
                dds.ThresholdType
FROM DiskSpace ddsp
INNER JOIN servers S On S.ServerName=ddsp.ServerName 
INNER JOIN ServerTypes ST On ST.ServerType=ddsp.ServerType 
INNER JOIN DiskSettings dds ON dds.ServerID=s.ID 
WHERE dds.diskname=ddsp.diskname
GO


/****** Object:  View [dbo].[diskhealthstatusnew]    Script Date: 11/18/2015 08:37:50 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

Create view [dbo].[diskhealthstatusnew]
as
Select servername ,diskname,DiskSize,diskfree,ThresholdType,
CASE WHEN ThresholdType = 'Percent'  THEN Threshold else '0.0' END AS RedthresholdPercent,
CASE WHEN ThresholdType = 'Percent'  THEN DiskSize*(Threshold/100) when ThresholdType = 'GB' then Threshold else '0.0' END AS RedThresholdValue
,
--CASE WHEN ThresholdType = 'GB'  THEN Threshold else '0.0' END AS RedthresholdGB,
--CASE WHEN ThresholdType = 'Percent' THEN PercentFree else '0.0' END AS ValPercentFree,
( select svalue from settings where sname='DiskYellowThreshold')
AS yellowthreshold,
Case WHEN ThresholdType = 'Percent' then (DiskSize*(Threshold/100)*(select svalue from settings where sname='DiskYellowThreshold')/100) when ThresholdType='GB' then ((DiskSize*(Threshold)*(select svalue from settings where sname='DiskYellowThreshold')/100))else '0.0' end as yellowthresholdvaluebytype from 
[viewdiskspace]


GO
--Somaraju VSPLUS 2284
/****** Object:  View [dbo].[diskheastatusnew]    Script Date: 11/20/2015 08:37:50 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE view [dbo].[diskheastatusnew]
as
Select servername ,diskname,DiskSize,diskfree,ThresholdType,ddsdiskname,
CASE WHEN ThresholdType = 'Percent'  THEN Threshold else '0.0' END AS RedthresholdPercent,
CASE WHEN ThresholdType = 'Percent'  THEN DiskSize*(Threshold/100) when ThresholdType = 'GB' then Threshold else '0.0' END AS RedThresholdValue
,
--CASE WHEN ThresholdType = 'GB'  THEN Threshold else '0.0' END AS RedthresholdGB,
--CASE WHEN ThresholdType = 'Percent' THEN PercentFree else '0.0' END AS ValPercentFree,
( select svalue from settings where sname='DiskYellowThreshold')
AS yellowthreshold,
Case WHEN ThresholdType = 'Percent' then (DiskSize*(Threshold/100)*(select svalue from settings where sname='DiskYellowThreshold')/100) when ThresholdType='GB' then ((DiskSize*(Threshold)*(select svalue from settings where sname='DiskYellowThreshold')/100))else '0.0' end as yellowthresholdvaluebytype from 
[viewdiskspace]


GO
/****** Object:  View [dbo].[diskyellowvalue]    Script Date: 11/18/2015 08:38:14 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

create view [dbo].[diskyellowvalue]
AS
select *,  (yellowthresholdvaluebytype+RedThresholdValue)YellowThresholdvalue
from diskheastatusnew 


GO
--VSPLUS 2300 Sowjanya
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******Mukund:VSPlus-984: Object:  StoredProcedure [dbo].[ServerLocations]    Script Date: 10/11/2014 14:55:39 ******/
CREATE procedure [dbo].[ServerLocationsDS] as
Begin
declare @SrvLocations Table
(id int, LocId int, Name varchar(100),actid int,tbl varchar(50),srvtypeid int,ServerType varchar(100),description varchar(100))

insert into @SrvLocations select id,null,Location,id,'Locations',null,null,null from Locations 

Declare @count int
select @count=MAX(id) from Locations


Declare @ID int
Declare @LocationName varchar(100)
Declare @LocationID int
Declare @ServerTypeId int
Declare @ServerType varchar(100)
Declare @description varchar(100)


DECLARE db_cursor CURSOR FOR  
select sr.ID,sr.ServerName,sr.LocationID,sr.ServerTypeId,srt.ServerType,sr.description from Servers sr,
ServerTypes srt,SelectedFeatures ft  where sr.ServerTypeId=srt.id and ft.FeatureId=srt.FeatureId and srt.ServerType in('Domino')
union
select sr.ID,sr.Name as ServerName,sr.LocationID,sr.ServerTypeId,srt.ServerType, sr.TheURL
from URLs sr,ServerTypes srt,SelectedFeatures ft   where sr.ServerTypeId=srt.id  and ft.FeatureId=srt.FeatureId and srt.ServerType in('Domino')
order by sr.LocationID,sr.ServerName

OPEN db_cursor   
FETCH NEXT FROM db_cursor into @ID,@LocationName,@LocationID,@ServerTypeId,@ServerType,@description

WHILE @@FETCH_STATUS = 0   
BEGIN   
	Set @count=@count+1
	insert into @SrvLocations values(@count,@LocationID,@LocationName,@id,'Servers',@ServerTypeId,@ServerType,@description)
	FETCH NEXT FROM db_cursor into @ID,@LocationName,@LocationID,@ServerTypeId,@ServerType,@description
END   

CLOSE db_cursor   
DEALLOCATE db_cursor

select * from @SrvLocations order by tbl,Name
end
GO
/****** Object:  Table [dbo].[DominoEventLogServers]    Script Date: 12/14/2015 21:02:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DominoEventLogServers](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[DominoEventLogId] [int] NULL,
	[ServerID] [int] NULL,
	[LocationID] [int] NULL,
	[ServerTypeID] [int] NULL,
 CONSTRAINT [PK_DominoEventLogServers] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[DominoEventLog]    Script Date: 12/14/2015 21:02:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DominoEventLog](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](250) NULL,
 CONSTRAINT [PK_EventsNames] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

USE [VitalSigns]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ExchangeServerLocations]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[ExchangeServerLocations]
GO

/****** Object:  StoredProcedure [dbo].[ExchangeServerLocations] **/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Chandrahas Yeruva
-- Create date: 3/7/2014
-- Description:	
--The procedure is for getting the Server Locations Details for the Exchange Servers
-- 
-- =============================================
--CY altered procedure as per Table changes 03/27

USE [VitalSigns]
GO
/****** Object:  StoredProcedure [dbo].[ExchangeServerLocations]    Script Date: 03/16/2014 18:53:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[ExchangeServerLocations] as
Begin

declare @SrvLocations Table
(id int, LocId int, Name varchar(100),actid int,tbl varchar(50),srvtypeid int,ServerType varchar(100),description varchar(100),MonitoredBy varchar(100), RoleType varchar(100))

insert into @SrvLocations select id,null,Location,id,'Locations',null,null,null,null,null from Locations

Declare @count int
select @count=MAX(id) from Locations


Declare @ID int
Declare @LocationName varchar(100)
Declare @LocationID int
Declare @ServerTypeId int
Declare @ServerType varchar(100)
Declare @description varchar(100)
Declare @MonitoredBy varchar(100)
Declare @RoleType varchar(100)

	
		DECLARE db_cursor CURSOR FOR 
		
		 
		select s.ID,s.ServerName,s.LocationID,s.ServerTypeId,st.ServerType,s.description,s.MonitoredBy,rm.RoleName 
		from Servers s
		inner join ServerTypes st on st.ID=s.ServerTypeID and st.servertype='Exchange'
		left join ServerRoles sr on sr.ServerId = s.ID
		left join RolesMaster rm on rm.ID=sr.RoleId	
	
		OPEN db_cursor   
		FETCH NEXT FROM db_cursor into @ID,@LocationName,@LocationID,@ServerTypeId,@ServerType,@description,@MonitoredBy,@RoleType

		WHILE @@FETCH_STATUS = 0   
		BEGIN   
		Set @count=@count+1
		insert into @SrvLocations values(@count,@LocationID,@LocationName,@id,'Servers',@ServerTypeId,@ServerType,@description,@MonitoredBy,@RoleType)
		FETCH NEXT FROM db_cursor into @ID,@LocationName,@LocationID,@ServerTypeId,@ServerType,@description,@MonitoredBy,@RoleType
		END
		CLOSE db_cursor   
		DEALLOCATE db_cursor

		select * from @SrvLocations-- order by LocId,Name
end

Go

USE [vitalsigns]
GO
/****** Object:  StoredProcedure [dbo].[SharePointServerLocations]    Script Date: 11/10/2014 16:51:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[SharePointServerLocations] as
Begin

declare @SrvLocations Table
(id int, LocId int, Name varchar(100),actid int,tbl varchar(50),srvtypeid int,ServerType varchar(100),description varchar(100),MonitoredBy varchar(100), RoleType varchar(100))

insert into @SrvLocations select id,null,Location,id,'Locations',null,null,null,null,null from Locations

Declare @count int
select @count=MAX(id) from Locations


Declare @ID int
Declare @LocationName varchar(100)
Declare @LocationID int
Declare @ServerTypeId int
Declare @ServerType varchar(100)
Declare @description varchar(100)
Declare @MonitoredBy varchar(100)
Declare @RoleType varchar(100)

	
		DECLARE db_cursor CURSOR FOR 
		
		 
		select s.ID,s.ServerName,s.LocationID,s.ServerTypeId,st.ServerType,s.description,s.MonitoredBy,rm.RoleName 
		from Servers s
		inner join ServerTypes st on st.ID=s.ServerTypeID and st.servertype='SharePoint'
		left join ServerRoles sr on sr.ServerId = s.ID
		left join RolesMaster rm on rm.ID=sr.RoleId	
	
		OPEN db_cursor   
		FETCH NEXT FROM db_cursor into @ID,@LocationName,@LocationID,@ServerTypeId,@ServerType,@description,@MonitoredBy,@RoleType

		WHILE @@FETCH_STATUS = 0   
		BEGIN   
		Set @count=@count+1
		insert into @SrvLocations values(@count,@LocationID,@LocationName,@id,'Servers',@ServerTypeId,@ServerType,@description,@MonitoredBy,@RoleType)
		FETCH NEXT FROM db_cursor into @ID,@LocationName,@LocationID,@ServerTypeId,@ServerType,@description,@MonitoredBy,@RoleType
		END
		CLOSE db_cursor   
		DEALLOCATE db_cursor

		select * from @SrvLocations-- order by LocId,Name
end
GO


USE [VitalSigns]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ActiveDirectoryServerLocations]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[ActiveDirectoryServerLocations]
GO

/****** Object:  StoredProcedure [dbo].[ActiveDirectoryServerLocations] **/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


USE [vitalsigns]
GO
/****** Object:  StoredProcedure [dbo].[ActiveDirectoryServerLocations]    Script Date: 11/10/2014 16:51:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[ActiveDirectoryServerLocations] as
Begin

declare @SrvLocations Table
(id int, LocId int, Name varchar(100),actid int,tbl varchar(50),srvtypeid int,ServerType varchar(100),description varchar(100),MonitoredBy varchar(100), RoleType varchar(100))

insert into @SrvLocations select id,null,Location,id,'Locations',null,null,null,null,null from Locations

Declare @count int
select @count=MAX(id) from Locations


Declare @ID int
Declare @LocationName varchar(100)
Declare @LocationID int
Declare @ServerTypeId int
Declare @ServerType varchar(100)
Declare @description varchar(100)
Declare @MonitoredBy varchar(100)
Declare @RoleType varchar(100)

	
		DECLARE db_cursor CURSOR FOR 
		
		 
		select s.ID,s.ServerName,s.LocationID,s.ServerTypeId,st.ServerType,s.description,s.MonitoredBy,rm.RoleName 
		from Servers s
		inner join ServerTypes st on st.ID=s.ServerTypeID and st.servertype='Active Directory'
		left join ServerRoles sr on sr.ServerId = s.ID
		left join RolesMaster rm on rm.ID=sr.RoleId	
	
		OPEN db_cursor   
		FETCH NEXT FROM db_cursor into @ID,@LocationName,@LocationID,@ServerTypeId,@ServerType,@description,@MonitoredBy,@RoleType

		WHILE @@FETCH_STATUS = 0   
		BEGIN   
		Set @count=@count+1
		insert into @SrvLocations values(@count,@LocationID,@LocationName,@id,'Servers',@ServerTypeId,@ServerType,@description,@MonitoredBy,@RoleType)
		FETCH NEXT FROM db_cursor into @ID,@LocationName,@LocationID,@ServerTypeId,@ServerType,@description,@MonitoredBy,@RoleType
		END
		CLOSE db_cursor   
		DEALLOCATE db_cursor

		select * from @SrvLocations-- order by LocId,Name
end
GO

USE VitalSigns
Go
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RestructureTables]') AND type in (N'P', N'PC'))
/****** Object:  StoredProcedure [dbo].[RestructureTables]    Script Date: 18 Feb 2014 ******/
DROP PROCEDURE [dbo].[RestructureTables]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Mukund Dunakhe
-- Create date: 18 Feb 2014
-- Description:	Restructuring Tables without Auto Increment column
-- =============================================
CREATE PROCEDURE RestructureTables
AS
BEGIN
	SET NOCOUNT ON;

Begin -- Restructuring MenuItems
declare @menuIdentity int
select @menuIdentity=COLUMNPROPERTY(object_id(TABLE_NAME), COLUMN_NAME, 'IsIdentity') from INFORMATION_SCHEMA.COLUMNS
where TABLE_SCHEMA = 'dbo' and TABLE_NAME='MenuItems' and COLUMN_NAME='ID' order by TABLE_NAME
--Check if identity column exists. If does not exist start assigning
if (@menuIdentity=0)
begin
	IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS WHERE CONSTRAINT_TYPE='PRIMARY KEY' AND TABLE_SCHEMA='dbo'
			AND TABLE_NAME = 'MenuItems' AND CONSTRAINT_NAME = 'PK_MenuItems1')
	BEGIN
		EXEC('ALTER TABLE [dbo].[MenuItems] DROP CONSTRAINT [PK_MenuItems1]')
	END

	CREATE TABLE dbo.MenuItemsTemp
	( 
		[ID] [int] NOT NULL IDENTITY(1, 1),
		[DisplayText] [varchar](50) NOT NULL,
		[OrderNum] [int] NOT NULL,
		[ParentID] [int] NULL,
		[PageLink] [varchar](150) NULL,
		[Level] [int] NULL,
		[RefName] [varchar](50) NULL,
		[ImageURL] [varchar](50) NULL,
		CONSTRAINT [PK_MenuItems1] PRIMARY KEY CLUSTERED 
	(
		[ID] ASC
	)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
	) ON [PRIMARY]

	--insert data from old table to temp table
	SET IDENTITY_INSERT dbo.MenuItemsTemp ON 
	insert into dbo.MenuItemsTemp ([ID],[DisplayText],[OrderNum],[ParentID],[PageLink],[Level],[RefName],[ImageURL])
	Select * From dbo.MenuItems 
	SET IDENTITY_INSERT dbo.MenuItemsTemp OFF

	-- dropping old table 
	DROP TABLE dbo.MenuItems

	--renaming temp table to original table
	Exec sp_rename 'MenuItemsTemp', 'MenuItems'
end
End

Begin -- Restructuring Logos
declare @logoIdentity int
select @logoIdentity=COLUMNPROPERTY(object_id(TABLE_NAME), COLUMN_NAME, 'IsIdentity') from INFORMATION_SCHEMA.COLUMNS
where TABLE_SCHEMA = 'dbo' and TABLE_NAME='Logos' and COLUMN_NAME='LogoID' order by TABLE_NAME
--Check if identity column exists. If does not exist start assigning
if (@logoIdentity=0)
begin
	IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS WHERE CONSTRAINT_TYPE='PRIMARY KEY' AND TABLE_SCHEMA='dbo'
			AND TABLE_NAME = 'Logos' AND CONSTRAINT_NAME = 'PK_Logos1')
	BEGIN
		EXEC('ALTER TABLE [dbo].[Logos] DROP CONSTRAINT [PK_Logos1]')
	END
	CREATE TABLE dbo.LogosTemp
	( 
		[LogoID] [int] NOT NULL IDENTITY(1, 1),
		[LogoName] [nvarchar](50) NULL,
		[LogoImage] [varchar](255) NULL,
		CONSTRAINT [PK_Logos1] PRIMARY KEY CLUSTERED 
	(
		[LogoID] ASC
	)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
	) ON [PRIMARY]

	--insert data from old table to temp table
	SET IDENTITY_INSERT dbo.LogosTemp ON 
	insert into dbo.LogosTemp ([LogoID],[LogoName],[LogoImage])
	Select * From dbo.Logos 
	SET IDENTITY_INSERT dbo.LogosTemp OFF

	-- dropping old table 
	DROP TABLE dbo.Logos

	--renaming temp table to original table
	Exec sp_rename 'LogosTemp', 'Logos'
end
End

Begin -- Restructuring ServerNodes
declare @servernodesIdentity int
select @servernodesIdentity=COLUMNPROPERTY(object_id(TABLE_NAME), COLUMN_NAME, 'IsIdentity') from INFORMATION_SCHEMA.COLUMNS
where TABLE_SCHEMA = 'dbo' and TABLE_NAME='ServerNodes' and COLUMN_NAME='NodeID' order by TABLE_NAME
--Check if identity column exists. If does not exist start assigning
if (@servernodesIdentity=0)
begin
	IF EXISTS (select * FROM sys.key_constraints where name='PK_ServerNodes1')
	BEGIN
		IF EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_URLs_ServersNodes]'))
		begin
		EXEC('ALTER TABLE [dbo].[URLs] DROP CONSTRAINT [FK_URLs_ServersNodes]')
		end
		IF EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Servers_ServersNodes]'))
		begin
		EXEC('ALTER TABLE [dbo].[Servers] DROP CONSTRAINT [FK_Servers_ServersNodes]')
		end
		IF EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_SametimeServers_ServersNodes]'))
		begin
		EXEC('ALTER TABLE [dbo].[SametimeServers] DROP CONSTRAINT [FK_SametimeServers_ServersNodes]')
		end
		IF EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_DominoServers_ServersNodes]'))
		begin
		EXEC('ALTER TABLE [dbo].[DominoServers] DROP CONSTRAINT [FK_DominoServers_ServersNodes]')
		end
		EXEC('ALTER TABLE [dbo].[ServerNodes] DROP CONSTRAINT [PK_ServerNodes1]')
	END
	CREATE TABLE dbo.ServerNodesTemp
	( 
		[NodeID] [int] NOT NULL IDENTITY(1, 1),
		[NodeHostName] [varchar](50) NOT NULL,
		[NodeIPAddress] [varchar](150) NOT NULL,
		[NodeDescription] [varchar](50) NOT NULL,
		CONSTRAINT [PK_ServerNodes1] PRIMARY KEY CLUSTERED 
	(
		[NodeID] ASC
	)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
	) ON [PRIMARY]

	--insert data from old table to temp table
	SET IDENTITY_INSERT dbo.ServerNodesTemp ON 
	insert into dbo.ServerNodesTemp ([NodeID],[NodeHostName],[NodeIPAddress],[NodeDescription]) 
	Select * From dbo.ServerNodes 
	SET IDENTITY_INSERT dbo.ServerNodesTemp OFF

	-- dropping old table 
	DROP TABLE dbo.ServerNodes

	--renaming temp table to original table
	Exec sp_rename 'ServerNodesTemp', 'ServerNodes'
	
	ALTER TABLE [dbo].[DominoServers]  WITH CHECK ADD  CONSTRAINT [FK_DominoServers_ServersNodes] FOREIGN KEY([MonitoredBy])
	REFERENCES [dbo].[ServerNodes] ([NodeID])
	ON UPDATE CASCADE
	ON DELETE CASCADE

	ALTER TABLE [dbo].[SametimeServers]  WITH CHECK ADD  CONSTRAINT [FK_SametimeServers_ServersNodes] FOREIGN KEY([MonitoredBy])
	REFERENCES [dbo].[ServerNodes] ([NodeID])
	ON UPDATE CASCADE
	ON DELETE CASCADE

	ALTER TABLE [dbo].[Servers]  WITH CHECK ADD  CONSTRAINT [FK_Servers_ServersNodes] FOREIGN KEY([MonitoredBy])
	REFERENCES [dbo].[ServerNodes] ([NodeID])
	ON UPDATE NO ACTION
	ON DELETE NO ACTION

	ALTER TABLE [dbo].[URLs]  WITH CHECK ADD  CONSTRAINT [FK_URLs_ServersNodes] FOREIGN KEY([MonitoredBy])
	REFERENCES [dbo].[ServerNodes] ([NodeID])
	ON UPDATE CASCADE
	ON DELETE CASCADE

end
End


END

Go


USE [vitalsigns]
GO

/****** Object:  Table [dbo].[ExchangeDatabaseSettings]    Script Date: 12/29/2015 15:00:31 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ExchangeDatabaseSettings](
	[ServerName] [nvarchar](50) NOT NULL,
	[ServerTypeId] [nvarchar](50) NOT NULL,
	[DatabaseName] [nvarchar](50) NOT NULL,
	[WhiteSpaceThreshold] [nvarchar](50) NOT NULL,
	[DatabaseSizeThreshold] [nvarchar](50) NOT NULL
) ON [PRIMARY]

GO




Exec RestructureTables
Go
--VitalSignsDailytasks Cleanup # kiran #
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[CleanUpData]

AS
BEGIN
		--CleanupTravelerData
		DELETE FROM TravelerStats where CONVERT(date, DateUpdated) < CONVERT(date, GETDATE()-1)

		--CleanupTravelerDevices
		DELETE FROM Traveler_Devices_temp 

		--CleanupAlerts
		DELETE FROM AlertHistory WHERE DateTimeOfAlert < CONVERT(date, GETDATE()-30)

		--CleanupTravelerStats
		DELETE FROM TravelerStats WHERE DateUpdated < GETDATE()

		--CleanupLogAlerts
		DELETE FROM AlertHistory WHERE AlertType like '%log file%' 

		-- CleanUpStatusTables
		DELETE FROM StatusDetail

		--CleanupStatusTables
		DELETE FROM [Status]
END
Go

/*WS Added for IBM Connections*/

--USE [vitalsigns]
--GO

--/****** Object:  Table [dbo].[IBMConnectionsTests]    Script Date: 02/22/2016 15:02:27 ******/
--SET ANSI_NULLS ON
--GO

--SET QUOTED_IDENTIFIER ON
--GO

--SET ANSI_PADDING OFF
--GO

--CREATE TABLE [dbo].[IBMConnectionsTests](
--	[Id] [int] NOT NULL,
--	[ServerId] [int] NULL,
--	[EnableSimulationTests] [bit] NULL,
--	[ResponseThreshold] [int] NULL
--) ON [PRIMARY]

--GO

--SET ANSI_PADDING OFF
--GO

--ALTER TABLE [dbo].[IBMConnectionsTests]  WITH CHECK ADD  CONSTRAINT [FK__IBMConnectionsTests__TestsMaster] FOREIGN KEY([Id])
--REFERENCES [dbo].[TestsMaster] ([Id])
--GO

--ALTER TABLE [dbo].[IBMConnectionsTests] CHECK CONSTRAINT [FK__IBMConnectionsTests__TestsMaster]
--GO




USE [vitalsigns]
GO

/****** Object:  Table [dbo].[IBMConnectionsServers]    Script Date: 02/22/2016 15:36:28 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[IBMConnectionsServers](
	[ServerID] [int] NOT NULL,
	[ProxyServerType] [varchar](250) NULL,
	[ProxyServerProtocall] [varchar](250) NULL,
	[DBHostName] [varchar](250) NULL,
	[DBName] [varchar](250) NULL,
	[DBPort] [int] NULL,
	[DBCredentialsID] [int] NULL,
	[WSCellID] [int] NULL,
	[Category] [nvarchar](255) NULL,
    [ScanInterval] [int] NULL,
    [OffHoursScanInterval] [int] NULL,
    [RetryInterval] [int] NULL,
    [ResponseThreshold] [int] NULL,
    [Enabled] [bit] NULL,
    [FailureThreshold] [int] NULL,
    [CredentialID] [int] NULL,
    [ChatUser1CredentialsId] [int] NULL,
    [ChatUser2CredentialsId] [int] NULL,
    [TestChatSimulation] [bit] NULL,
    [SSL] [bit] NULL,
 
    [EnableDB2port] [bit] NULL,
 
     [URL] [nvarchar](max) NULL,
     [PortNumber] [int] NULL
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

ALTER TABLE [dbo].[IBMConnectionsServers]  WITH CHECK ADD  CONSTRAINT [FK_IBMConnectionsServers_Servers] FOREIGN KEY([ServerID])
REFERENCES [dbo].[Servers] ([ID])
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[IBMConnectionsServers] CHECK CONSTRAINT [FK_IBMConnectionsServers_Servers]
GO

ALTER TABLE [dbo].[IBMConnectionsServers]  WITH CHECK ADD  CONSTRAINT [FK_IBMConnectionsServers_WebsphereCell] FOREIGN KEY([WSCellID])
REFERENCES [dbo].[WebsphereCell] ([CellID])
GO

ALTER TABLE [dbo].[IBMConnectionsServers] CHECK CONSTRAINT [FK_IBMConnectionsServers_WebsphereCell]
GO


CREATE TABLE [dbo].[O365Groups](
	[ServerId] [int] NOT NULL,
	[GroupId] [varchar](254) NOT NULL,
	[GroupName] [varchar](254) NOT NULL,
	[GroupType] [varchar](254) NOT NULL,
	[GroupDescription] [varchar](1000) NULL
) ON [PRIMARY]

GO

CREATE TABLE [dbo].[O365MSOLUsers](
	[ServerId] [int] NOT NULL,
	[DisplayName] [varchar](254) NOT NULL,
	[FirstName] [varchar](254) NOT NULL,
	[LastName] [varchar](254) NOT NULL,
	[UserPrincipalName] [varchar](1000) NULL,
	[UserType] [varchar](254) NULL,
	[Title] [varchar](254) NULL,
	[IsLicensed] [bit] NULL,
	[Department] [varchar](254) NULL,
	[LastUpdated] [datetime] NULL,
	--Somaraju VSPLUS 2714
	[StrongPasswordRequired] [bit] NULL,
	[PasswordNeverExpires] [bit] NULL,
) ON [PRIMARY]

GO

CREATE TABLE [dbo].[O365UserGroups](
	[GroupId] [varchar](254) NULL,
	[UserPrincipalName] [varchar](1000) NULL
) ON [PRIMARY]

GO
--3/11/2016 sowjanya Modified for VSPLUS-2650
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

Create procedure [dbo].[WSIBMConnectionsNodes] 
@CellID as int
as
Begin

declare @SrvEvents Table
(id int, SrvId int, Name varchar(100),actid int,tbl varchar(50),AlertOnRepeat bit,Status varchar(100),HostName varchar(100))

insert into @SrvEvents select st.NodeID,null,st.NodeName,st.NodeID,'Nodes',0,NULL,NULL from WebsphereNode st,WebsphereCell ft 
 where ft.CellID=st.CellID  and ft.CellID=@CellID and st.nodeid in
 (
  select st.NodeID from WebsphereServer st,WebsphereCell ft,IBMConnectionsServers ICS 
where ft.CellID=st.CellID and ft.CellID=@CellID and ICS.ServerID=ft.IBMConnectionSID and (st.Enabled is null or st.Enabled ='False')
 ) 
 order by st.NodeID

Declare @count int
select @count=MAX(NodeID) from WebsphereNode 


Declare @ID int
Declare @EventName varchar(100)
Declare @ServerTypeID int
Declare @AlertOnRepeat bit
Declare @Status varchar(100)
Declare @HostName varchar(100)

DECLARE db_cursor CURSOR FOR  
select st.ServerID,st.ServerName, st.NodeID,st.Status,st.HostName from WebsphereServer st,WebsphereCell ft   
where ft.CellID=st.CellID and ft.CellID=@CellID and  (st.Enabled is null or st.Enabled ='False')
order by st.NodeID,st.ServerName

OPEN db_cursor   
FETCH NEXT FROM db_cursor into @ID,@EventName,@ServerTypeID,@Status,@HostName

WHILE @@FETCH_STATUS = 0   
BEGIN   
 Set @count=@count+1
 insert into @SrvEvents values(@count,@ServerTypeID,@EventName,@id,'Servers',0, @Status,@HostName)
 FETCH NEXT FROM db_cursor into @ID,@EventName,@ServerTypeID,@Status,@HostName
END   

CLOSE db_cursor   
DEALLOCATE db_cursor

select * from @SrvEvents  --order by SrvId,Name
end
GO


--/*WS added for IBM Connections */

--USE [vitalsigns]
--GO

--/****** Object:  Table [dbo].[IBMConnectionsServers]    Script Date: 03/14/2016 09:54:03 ******/
--SET ANSI_NULLS ON
--GO

--SET QUOTED_IDENTIFIER ON
--GO

--SET ANSI_PADDING ON
--GO

--CREATE TABLE [dbo].[IBMConnectionsServers](
--	[ServerID] [int] NOT NULL,
--	[ProxyServerType] [varchar](250) NULL,
--	[ProxyServerProtocall] [varchar](250) NULL,
--	[DBHostName] [varchar](250) NULL,
--	[DBName] [varchar](250) NULL,
--	[DBPort] [int] NULL,
--	[DBCredentialsID] [int] NULL,
--	[WSCellID] [int] NULL
--) ON [PRIMARY]

--GO

--SET ANSI_PADDING OFF
--GO

--ALTER TABLE [dbo].[IBMConnectionsServers]  WITH CHECK ADD  CONSTRAINT [FK_IBMConnectionsServers_Servers] FOREIGN KEY([ServerID])
--REFERENCES [dbo].[Servers] ([ID])
--ON DELETE CASCADE
--GO

--ALTER TABLE [dbo].[IBMConnectionsServers] CHECK CONSTRAINT [FK_IBMConnectionsServers_Servers]
--GO

--ALTER TABLE [dbo].[IBMConnectionsServers]  WITH CHECK ADD  CONSTRAINT [FK_IBMConnectionsServers_WebsphereCell] FOREIGN KEY([WSCellID])
--REFERENCES [dbo].[WebsphereCell] ([CellID])
--GO

--ALTER TABLE [dbo].[IBMConnectionsServers] CHECK CONSTRAINT [FK_IBMConnectionsServers_WebsphereCell]
--GO



USE [vitalsigns]
GO

/****** Object:  Table [dbo].[IBMConnectionsTests]    Script Date: 03/14/2016 09:57:15 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[IBMConnectionsTests](
	[Id] [int] NOT NULL,
	[ServerId] [int] NULL,
	[EnableSimulationTests] [bit] NULL,
	[ResponseThreshold] [int] NULL
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[IBMConnectionsTests]  WITH CHECK ADD  CONSTRAINT [FK__IBMConnectionsTests__TestsMaster] FOREIGN KEY([Id])
REFERENCES [dbo].[TestsMaster] ([Id])
GO

ALTER TABLE [dbo].[IBMConnectionsTests] CHECK CONSTRAINT [FK__IBMConnectionsTests__TestsMaster]
GO
--3/31/2016  Somaraju Modified for VSPLUS-2528 
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[O365ServiceDetails](
 [ServerId] [int] NULL,
 [ServiceName] [nvarchar](max) NULL,
 [ServiceID] [nvarchar](150) NULL,
 [StartTime] [datetime] NULL,
 [EndTime] [datetime] NULL,
 [Status] [nvarchar](max) NULL,
 [EventType] [nvarchar](50) NULL,
 [Message] [nvarchar](max) NULL
) ON [PRIMARY]

GO

--20/4/2016 Somaraju Modified for VSPLUS-2613
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[O365UserswithLicensesandServices](
 [DisplayName] [nvarchar](150) NULL,
 [XMLConfiguration] [nvarchar](max) NULL,
 [ServerID] [int] NULL
) ON [PRIMARY]

GO

--5/31/15 WS Added for 2890

USE [vitalsigns]
GO

--IBM Connections Objects

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[IbmConnectionsObjects](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](255) NOT NULL,
	[Type] [varchar](255) NOT NULL,
	[DateCreated] [datetime] NOT NULL,
	[DateLastModified] [datetime] NOT NULL,
	[ServerID] [int] NOT NULL,
	[OwnerId] [int] NULL,
	[ParentObjectId] [int] NULL,
	[GUID] [varchar](255) NULL,
 CONSTRAINT [PK_IbmConnectionsObjects] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

ALTER TABLE [dbo].[IbmConnectionsObjects]  WITH CHECK ADD  CONSTRAINT [IbmConnectionsObjects_ServerID_Servers_ID] FOREIGN KEY([ServerID])
REFERENCES [dbo].[Servers] ([ID])
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[IbmConnectionsObjects] CHECK CONSTRAINT [IbmConnectionsObjects_ServerID_Servers_ID]
GO

--IBM Connections Tags

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[IbmConnectionsTags](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Tag] [varchar](255) NOT NULL,
 CONSTRAINT [PK_IbmConnectionsTags] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

--IBM Connections Users

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[IbmConnectionsUsers](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[DisplayName] [varchar](255) NOT NULL,
	[GUID] [varchar](255) NOT NULL,
	[ServerID] [int] NOT NULL,
 CONSTRAINT [PK_IbmConnectionsUsers] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

ALTER TABLE [dbo].[IbmConnectionsUsers]  WITH CHECK ADD  CONSTRAINT [IbmConnectionsUsers_ServerID_Servers_ID] FOREIGN KEY([ServerID])
REFERENCES [dbo].[Servers] ([ID])
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[IbmConnectionsUsers] CHECK CONSTRAINT [IbmConnectionsUsers_ServerID_Servers_ID]
GO

--IBM Connections Users Objects

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[IbmConnectionsObjectUsers](
	[ObjectId] [int] NOT NULL,
	[UserId] [int] NOT NULL
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[IbmConnectionsObjectUsers]  WITH CHECK ADD  CONSTRAINT [IbmConnectionsObjectUsers_ObjectId_IbmConnectionsObjects_ID] FOREIGN KEY([ObjectId])
REFERENCES [dbo].[IbmConnectionsObjects] ([ID])
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[IbmConnectionsObjectUsers] CHECK CONSTRAINT [IbmConnectionsObjectUsers_ObjectId_IbmConnectionsObjects_ID]
GO

ALTER TABLE [dbo].[IbmConnectionsObjectUsers]  WITH CHECK ADD  CONSTRAINT [IbmConnectionsObjectUsers_UserId_IbmConnectionsUsers_ID] FOREIGN KEY([UserId])
REFERENCES [dbo].[IbmConnectionsUsers] ([ID])
GO

ALTER TABLE [dbo].[IbmConnectionsObjectUsers] CHECK CONSTRAINT [IbmConnectionsObjectUsers_UserId_IbmConnectionsUsers_ID]
GO

--IBM Connections Object Tags

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[IbmConnectionsObjectTags](
	[ObjectId] [int] NOT NULL,
	[TagId] [int] NOT NULL
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[IbmConnectionsObjectTags]  WITH CHECK ADD  CONSTRAINT [IbmConnectionsObjectTags_ObjectId_IbmConnectionsObjects_ID] FOREIGN KEY([ObjectId])
REFERENCES [dbo].[IbmConnectionsObjects] ([ID])
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[IbmConnectionsObjectTags] CHECK CONSTRAINT [IbmConnectionsObjectTags_ObjectId_IbmConnectionsObjects_ID]
GO

ALTER TABLE [dbo].[IbmConnectionsObjectTags]  WITH CHECK ADD  CONSTRAINT [IbmConnectionsObjectTags_TagId_IbmConnectionsTags_ID] FOREIGN KEY([TagId])
REFERENCES [dbo].[IbmConnectionsTags] ([ID])
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[IbmConnectionsObjectTags] CHECK CONSTRAINT [IbmConnectionsObjectTags_TagId_IbmConnectionsTags_ID]
GO

--IBM Connections Community

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[IbmConnectionsCommunity](
	[ObjectID] [int] NOT NULL,
	[CommunityType] [nvarchar](255) NOT NULL,
 CONSTRAINT [PK_IbmConnectionsCommunity] PRIMARY KEY CLUSTERED 
(
	[ObjectID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[IbmConnectionsCommunity]  WITH CHECK ADD  CONSTRAINT [IbmConnectionsCommunity_ObjectID_IbmConnectionsObjects_ID] FOREIGN KEY([ObjectID])
REFERENCES [dbo].[IbmConnectionsObjects] ([ID])
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[IbmConnectionsCommunity] CHECK CONSTRAINT [IbmConnectionsCommunity_ObjectID_IbmConnectionsObjects_ID]
GO





-- +++++++++++ NO CHANGES BELOW THIS POINT +++++++++++
USE [vitalsigns]
GO

DECLARE @VS_MANAGEMENT_VSBUILD varchar(50)
SET @VS_MANAGEMENT_VSBUILD = '3.5.0_DAILY_09'
INSERT INTO dbo.VS_MANAGEMENT(CATEGORY,VALUE,LAST_UPDATE,DESCRIPTION,UPDATE_BY) VALUES 
('VS_BUILD',@VS_MANAGEMENT_VSBUILD,getdate(),'This indicates the current build that was installed.',CURRENT_USER)
go

INSERT INTO dbo.VS_MANAGEMENT ( CATEGORY, VALUE, LAST_UPDATE, DESCRIPTION ) VALUES 
( 'VS_VERSION', dbo.fn_GetVSVersion( ), GETDATE(), 'This value indicates the fundamental VS database version, which should match the base application version.' )
go
-- 1133 We need to provide the build date on a page somewhere
INSERT INTO dbo.VS_MANAGEMENT ( CATEGORY, VALUE, LAST_UPDATE, DESCRIPTION ) VALUES 
( 'VS_New', dbo.fn_GetVSVersion( ), GETDATE(), 'This indicates the original version and run date of the NEW script.' )
go
