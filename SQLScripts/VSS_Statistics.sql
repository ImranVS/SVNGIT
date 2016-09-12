USE [master]
GO

/****** Object:  Database [VSS_Statistics]    Script Date: 06/05/2015 14:09:41 ******/
IF  EXISTS (SELECT name FROM sys.databases WHERE name = N'VSS_Statistics')
BEGIN
ALTER DATABASE [VSS_Statistics] SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
DROP DATABASE [VSS_Statistics]
END
GO

USE [master]
GO
/****** Object:  Database [VSS_Statistics]    Script Date: 09/23/2013 17:01:25 ******/
CREATE DATABASE [VSS_Statistics] ON  PRIMARY 
( NAME = N'VSS_Statistics', FILENAME = N'$(dbpath)\VSS_Statistics.mdf' , SIZE = 1GB , MAXSIZE = UNLIMITED, FILEGROWTH = 500MB )
 LOG ON 
( NAME = N'VSS_Statistics_log', FILENAME = N'$(dbpath)\VSS_Statistics_log.ldf' , SIZE = 1GB  , MAXSIZE = 2GB , FILEGROWTH = 100MB)
GO
ALTER DATABASE [VSS_Statistics] SET COMPATIBILITY_LEVEL = 100
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [VSS_Statistics].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [VSS_Statistics] SET ANSI_NULL_DEFAULT OFF
GO
ALTER DATABASE [VSS_Statistics] SET ANSI_NULLS OFF
GO
ALTER DATABASE [VSS_Statistics] SET ANSI_PADDING OFF
GO
ALTER DATABASE [VSS_Statistics] SET ANSI_WARNINGS OFF
GO
ALTER DATABASE [VSS_Statistics] SET ARITHABORT OFF
GO
ALTER DATABASE [VSS_Statistics] SET AUTO_CLOSE OFF
GO
ALTER DATABASE [VSS_Statistics] SET AUTO_CREATE_STATISTICS ON
GO
ALTER DATABASE [VSS_Statistics] SET AUTO_SHRINK OFF
GO
ALTER DATABASE [VSS_Statistics] SET AUTO_UPDATE_STATISTICS ON
GO
ALTER DATABASE [VSS_Statistics] SET CURSOR_CLOSE_ON_COMMIT OFF
GO
ALTER DATABASE [VSS_Statistics] SET CURSOR_DEFAULT  GLOBAL
GO
ALTER DATABASE [VSS_Statistics] SET CONCAT_NULL_YIELDS_NULL OFF
GO
ALTER DATABASE [VSS_Statistics] SET NUMERIC_ROUNDABORT OFF
GO
ALTER DATABASE [VSS_Statistics] SET QUOTED_IDENTIFIER OFF
GO
ALTER DATABASE [VSS_Statistics] SET RECURSIVE_TRIGGERS OFF
GO
ALTER DATABASE [VSS_Statistics] SET  DISABLE_BROKER
GO
ALTER DATABASE [VSS_Statistics] SET AUTO_UPDATE_STATISTICS_ASYNC OFF
GO
ALTER DATABASE [VSS_Statistics] SET DATE_CORRELATION_OPTIMIZATION OFF
GO
ALTER DATABASE [VSS_Statistics] SET TRUSTWORTHY OFF
GO
ALTER DATABASE [VSS_Statistics] SET ALLOW_SNAPSHOT_ISOLATION OFF
GO
ALTER DATABASE [VSS_Statistics] SET PARAMETERIZATION SIMPLE
GO
ALTER DATABASE [VSS_Statistics] SET READ_COMMITTED_SNAPSHOT OFF
GO
ALTER DATABASE [VSS_Statistics] SET HONOR_BROKER_PRIORITY OFF
GO
ALTER DATABASE [VSS_Statistics] SET  READ_WRITE
GO
ALTER DATABASE [VSS_Statistics] SET RECOVERY SIMPLE
GO
ALTER DATABASE [VSS_Statistics] SET  MULTI_USER
GO
ALTER DATABASE [VSS_Statistics] SET PAGE_VERIFY CHECKSUM
GO
ALTER DATABASE [VSS_Statistics] SET DB_CHAINING OFF
GO
EXEC sys.sp_db_vardecimal_storage_format N'VSS_Statistics', N'ON'
GO
USE [VSS_Statistics]
GO
/****** Object:  User [vs]    Script Date: 09/23/2013 16:59:28 ******/
CREATE USER [vs] For LOGIN vs
GO
EXEC sp_addrolemember N'db_owner', N'vs'
go


USE [VSS_Statistics]
GO
EXEC sp_configure 'default language', 0 
RECONFIGURE 
GO

ALTER LOGIN VS WITH DEFAULT_LANGUAGE = English
GO

USE [VSS_Statistics]
GO

CREATE TABLE [dbo].[ExchangeMailFiles](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[ScanDate] [datetime] NULL,
	[Database] [nvarchar](200) NULL,
	[DisplayName] [nvarchar](200) NULL,
	[Server] [nvarchar](200) NULL,
	[IssueWarningQuota] [nvarchar](200) NULL,
	[ProhibitSendQuota] [nvarchar](200) NULL,
	[ProhibitSendReceiveQuota] [nvarchar](200) NULL,
	[TotalItemSizeInMB] [float] NULL,
	[ItemCount] [int] NULL,
	[StorageLimitStatus] [nvarchar](200) NULL,
	[LastUpdated] [datetime] NULL,
 CONSTRAINT [ExchangeDaily$PrimaryKey] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO


/****** Object:  Table [dbo].[DominoSummaryStats]    Script Date: 09/23/2013 17:01:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DominoSummaryStats](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[ServerName] [nvarchar](50) NULL,
	[Date] [datetime] NULL,
	[StatName] [nvarchar](50) NULL,
	[StatValue] [float] NULL,
	[WeekNumber] [int] NULL,
	[MonthNumber] [int] NULL,
	[YearNumber] [int] NULL,
	[DayNumber] [int] NULL,
 CONSTRAINT [DominoSummaryStats$PrimaryKey] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/* 11/12/2015 NS added for VSPLUS-2331 */
/****** Object:  Index [DominoSummaryStats_DateIndex]    Script Date: 11/12/2015 14:36:49 ******/
CREATE NONCLUSTERED INDEX [DominoSummaryStats_DateIndex] ON [dbo].[DominoSummaryStats] 
(
	[Date] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO

/****** Object:  Table [dbo].[DominoDailyStats]    Script Date: 09/23/2013 17:01:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DominoDailyStats](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[ServerName] [nvarchar](50) NULL,
	[Date] [datetime] NULL,
	[StatName] [nvarchar](200) NULL,
	[StatValue] [float] NULL,
	[WeekNumber] [int] NULL,
	[MonthNumber] [int] NULL,
	[YearNumber] [int] NULL,
	[DayNumber] [int] NULL,
	[HourNumber] [int] NULL,
 CONSTRAINT [DominoDailyStats$PrimaryKey] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[DeviceUpTimeStats]    Script Date: 09/23/2013 17:01:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DeviceUpTimeStats](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[DeviceType] [nvarchar](100) NULL,
	[DeviceName] [nvarchar](100) NULL,
	[Date] [datetime] NULL,
	[StatName] [nvarchar](50) NULL,
	[StatValue] [float] NULL,
	[WeekNumber] [int] NULL,
	[MonthNumber] [int] NULL,
	[YearNumber] [int] NULL,
	[DayNumber] [int] NULL,
 CONSTRAINT [DeviceUpTimeStats$PrimaryKey] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

USE [VSS_Statistics]
GO

/****** Object:  Table [dbo].[DeviceDailyStats]    Script Date: 12/13/2013 16:41:03 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO


SET ANSI_PADDING OFF
GO



/****** Object:  Table [dbo].[DeviceDailyStats]    Script Date: 09/23/2013 17:01:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DeviceDailyStats](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[DeviceType] [nvarchar](100) NULL,
	[Date] [datetime] NULL,
	[StatName] [nvarchar](50) NULL,
	[StatValue] [float] NULL,
	[WeekNumber] [int] NULL,
	[MonthNumber] [int] NULL,
	[YearNumber] [int] NULL,
	[DayNumber] [int] NULL,
	[ServerName] [varchar](250) NULL
) ON [PRIMARY]
GO

/****** Object:  Table [dbo].[DailySummary]    Script Date: 09/23/2013 17:01:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DailySummary](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Server] [nvarchar](200) NULL,
	[ScanDate] [datetime] NULL,
	[DatabaseCount] [float] NULL,
	[DatabaseSize] [float] NULL,
 CONSTRAINT [DailySummary$PrimaryKey] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Daily]    Script Date: 09/23/2013 17:01:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Daily](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[ScanDate] [datetime] NULL,
	[FileName] [nvarchar](200) NULL,
	[Title] [nvarchar](200) NULL,
	[FileSize] [float] NULL,
	[Server] [nvarchar](200) NULL,
	[DesignTemplateName] [nvarchar](100) NULL,
	[Quota] [float] NULL,
	[FTIndexed] [bit] NULL,
	[EnabledForClusterReplication] [bit] NULL,
	[ReplicaID] [nvarchar](255) NULL,
	[ODS] [float] NULL,
	[Status] [nvarchar](255) NULL,
	[DocumentCount] [int] NULL,
	[Categories] [nvarchar](255) NULL,
	[Created] [datetime] NULL,
	[CurrentAccessLevel] [nvarchar](255) NULL,
	[FTIndexFrequency] [nvarchar](255) NULL,
	[IsInService] [bit] NULL,
	[Folder] [nvarchar](255) NULL,
	[IsPrivateAddressBook] [bit] NULL,
	[IsPublicAddressBook] [bit] NULL,
	[LastFixup] [datetime] NULL,
	[LastFTIndexed] [datetime] NULL,
	[PercentUsed] [float] NULL,
	[Details] [nvarchar](255) NULL,
	[LastModified] [datetime] NULL,
	[EnabledForReplication] [bit] NULL,
	[IsMailFile] [bit] NULL,
	[InboxDocCount] [int] NULL,
	[Q_PlaceBotCount] [int] NULL,
	[Q_CustomFormCount] [int] NULL,
	[PersonDocID] [nvarchar](255) NULL,
	[FileNamePath] [nvarchar](255) NULL,
	[FolderCount] [int] NULL,
 CONSTRAINT [Daily$PrimaryKey] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Weekly]    Script Date: 09/23/2013 17:01:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Weekly](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[ScanDate] [datetime] NULL,
	[FileName] [nvarchar](50) NULL,
	[FileTitle] [nvarchar](50) NULL,
	[FileSize] [float] NULL,
	[MailServer] [nvarchar](50) NULL,
 CONSTRAINT [Weekly$PrimaryKey] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SametimeSummaryStats]    Script Date: 09/23/2013 17:01:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SametimeSummaryStats](
	[ServerName] [nvarchar](250) NULL,
	[Date] [datetime] NULL,
	[StatName] [nvarchar](250) NULL,
	[StatValue] [float] NULL,
	[WeekNumber] [int] NULL,
	[MonthNumber] [int] NULL,
	[YearNumber] [int] NULL,
	[DayNumber] [int] NULL,
	[ID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]
GO
/* 11/12/2015 NS added for VSPLUS-2331 */
/****** Object:  Index [SametimeSummaryStats_DateIndex]    Script Date: 11/12/2015 14:38:41 ******/
CREATE NONCLUSTERED INDEX [SametimeSummaryStats_DateIndex] ON [dbo].[SametimeSummaryStats] 
(
	[Date] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO

/****** Object:  Table [dbo].[SametimeDailyStats]    Script Date: 09/23/2013 17:01:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SametimeDailyStats](
	[ServerName] [nvarchar](250) NULL,
	[Date] [datetime] NULL,
	[StatName] [nvarchar](250) NULL,
	[StatValue] [float] NULL,
	[WeekNumber] [int] NULL,
	[MonthNumber] [int] NULL,
	[YearNumber] [int] NULL,
	[DayNumber] [int] NULL,
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[HourNumber] [int] NULL
) ON [PRIMARY]
GO
/****** VSPlus-1185 Swathi ****/
USE [VSS_Statistics]
GO

/****** Object:  Table [dbo].[MailLatencyStats]    Script Date: 11/28/2014 13:28:46 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[MailLatencyStats](
	[sourceserver] [nvarchar](100) NULL,
	[DestinationServer] [nvarchar](100) NULL,
	[Latency] [float] NULL,
	[Date] [datetime] NULL
) ON [PRIMARY]

GO



/****** Object:  Table [dbo].[QuickrPlaces]    Script Date: 09/23/2013 17:01:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[QuickrPlaces](
	[PlaceKey] [nvarchar](200) NOT NULL,
	[ServerName] [nvarchar](250) NULL,
	[PlaceName] [nvarchar](250) NULL,
	[PlaceTitle] [nvarchar](max) NULL,
	[PlaceDescription] [nvarchar](50) NOT NULL,
	[PlaceType] [nvarchar](250) NULL,
	[PlaceOwner] [nvarchar](250) NULL,
	[PlaceManagers] [nvarchar](max) NULL,
	[PlaceCreated] [datetime] NULL,
	[PlaceHostName] [nvarchar](250) NULL,
	[PlaceBotCount] [int] NULL,
	[CustomFormCount] [int] NULL,
	[PlaceLastAccessed] [datetime] NULL,
	[PlaceLastModified] [datetime] NULL,
	[DocumentReadCounts] [int] NULL,
	[LoginCount] [int] NULL,
	[LastDayUses] [int] NULL,
	[LastDayReads] [int] NULL,
	[LastDayWrites] [int] NULL,
	[LastWeekReads] [int] NULL,
	[LastWeekWrites] [int] NULL,
	[LastMonthReads] [int] NULL,
	[LastMonthWrites] [int] NULL,
 CONSTRAINT [PK_QuickrPlaces] PRIMARY KEY CLUSTERED 
(
	[PlaceKey] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Outages]    Script Date: 09/23/2013 17:01:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Outages](
	[ServerID] [int] NOT NULL,
	[DateTimeDown] [datetime] NULL,
	[DateTimeUp] [datetime] NULL,
	[Description] [nchar](300) NULL,
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[ServerTypeID] [int] NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[NotesMailStats]    Script Date: 09/23/2013 17:01:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[NotesMailStats](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NULL,
	[Date] [datetime] NULL,
	[StatName] [nvarchar](50) NULL,
	[StatValue] [float] NULL,
 CONSTRAINT [NotesMailStats$PrimaryKey] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Monthly]    Script Date: 09/23/2013 17:01:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Monthly](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[ScanDate] [datetime] NULL,
	[FileName] [nvarchar](50) NULL,
	[FileTitle] [nvarchar](50) NULL,
	[FileSize] [float] NULL,
	[MailServer] [nvarchar](50) NULL,
	[MonthNumber] [int] NULL,
	[YearNumber] [int] NULL,
 CONSTRAINT [Monthly$PrimaryKey] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  UserDefinedFunction [dbo].[Month_Name]    Script Date: 09/23/2013 17:01:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create Function [dbo].[Month_Name](@Number as int)
returns varchar(50)
as begin 
declare @Monthname as varchar(50)
set @Monthname=''
 select @Monthname=
Case @Number
when 1 then 
  'January'
when 2 then 
  'Feb'
 else 'other'
 end 
 return @Monthname
 end
GO
/****** Object:  Table [dbo].[BlackBerrySummaryStats]    Script Date: 09/23/2013 17:01:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[BlackBerrySummaryStats](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[ServerName] [nvarchar](50) NULL,
	[Date] [datetime] NULL,
	[StatName] [nvarchar](50) NULL,
	[StatValue] [float] NULL,
	[WeekNumber] [int] NULL,
	[MonthNumber] [int] NULL,
	[YearNumber] [int] NULL,
	[DayNumber] [int] NULL,
 CONSTRAINT [BlackBerrySummaryStats$PrimaryKey] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[BlackBerryProbeStats]    Script Date: 09/23/2013 17:01:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[BlackBerryProbeStats](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NULL,
	[Date] [datetime] NULL,
	[StatName] [nvarchar](50) NULL,
	[StatValue] [float] NULL,
 CONSTRAINT [BlackBerryProbeStats$PrimaryKey] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[BESDailyStats]    Script Date: 09/23/2013 17:01:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[BESDailyStats](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[ServerName] [nvarchar](50) NULL,
	[Date] [datetime] NULL,
	[StatName] [nvarchar](50) NULL,
	[StatValue] [float] NULL,
	[WeekNumber] [int] NULL,
	[MonthNumber] [int] NULL,
	[YearNumber] [int] NULL,
	[DayNumber] [int] NULL,
	[HourNumber] [int] NULL,
 CONSTRAINT [BESDailyStats$PrimaryKey] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  UserDefinedFunction [dbo].[CalcNumDaysinMonth]    Script Date: 09/23/2013 17:01:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/* 2/12/2015 NS modified for VSPLUS-1429 */
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date, ,>
-- Description:	<Description, ,>
-- =============================================
CREATE FUNCTION [dbo].[CalcNumDaysinMonth] 
(
	@MonthVal as datetime
)
RETURNS int
AS
BEGIN
	DECLARE @NumDays int
	SET @NumDays = 0
	
	IF MONTH(@MonthVal) = MONTH(GETDATE()) AND YEAR(@MonthVal) = YEAR(GETDATE())
	BEGIN
		-- Calculate number of days to date in the current month minus 1
		SET @NumDays = day(@MonthVal)-1
    END
    ELSE
    BEGIN
		-- Calculate number of days in a month
		SET @NumDays = datediff(day, dateadd(day, 1-day(@MonthVal), @MonthVal),
              dateadd(month, 1, dateadd(day, 1-day(@MonthVal), @MonthVal)))
    END
    RETURN @NumDays
END

GO

-- =============================================
-- Author:		Natallya Shkarayeva
-- Create date: 8/8/2014 - modified, 8/11/2014 - modified, 2/6/2015 - modified
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[CalcAvgDiskConsumtpion]
	@ServerTypeIn as varchar(50),@ServerNameIn as varchar(50),@DiskNameIn as varchar(10),@DiskConsumption as  float output
AS
BEGIN
	DECLARE @ServerName varchar(50) 
	DECLARE @Date datetime
	DECLARE @DiskName varchar(10)
	DECLARE @StatValue float
	DECLARE @PrevStatValue float
	DECLARE @CurrStatValue float
	DECLARE @StatValueDiff float
	DECLARE @Count int
	DECLARE @DiskUsageCursor CURSOR
	
	SET @Count = 0
	SET @StatValueDiff = 0
	
	IF @ServerTypeIn != ''
	BEGIN
		IF CHARINDEX('Domino',@ServerTypeIn) != 0
		BEGIN
			SET @DiskUsageCursor = CURSOR FAST_FORWARD 

				FOR 
					SELECT ServerName,Date,SUBSTRING(SUBSTRING(StatName,6,LEN(StatName)),1,CHARINDEX('.',SUBSTRING(StatName,6,LEN(StatName)))-1) DiskName, 
					ISNULL(StatValue,0)/1024/1024 StatValue
					FROM dbo.DominoSummaryStats
					WHERE StatName LIKE 'Disk%Free' AND DATEDIFF(dd,0,Date) >= DATEDIFF(dd,0,DATEADD(dd,-30,
						(SELECT MAX(Date) FROM DominoSummaryStats WHERE StatName LIKE 'Disk%Free' AND ServerName = @ServerNameIn)
					)) AND ServerName = @ServerNameIn
					AND SUBSTRING(SUBSTRING(StatName,6,LEN(StatName)),1,CHARINDEX('.',SUBSTRING(StatName,6,LEN(StatName)))-1)= @DiskNameIn
					UNION
					SELECT ServerName,Date,SUBSTRING(SUBSTRING(StatName,6,LEN(StatName)),1,LEN(StatName)) DiskName, 
					ISNULL(StatValue,0)/1024/1024 StatValue
					FROM dbo.MicrosoftSummaryStats mt1 INNER JOIN vitalsigns.dbo.ServerTypes mt2 ON mt1.ServerTypeId=mt2.ID
					WHERE StatName LIKE 'Disk.%' AND DATEDIFF(dd,0,Date) >= DATEDIFF(dd,0,DATEADD(dd,-30,
						(SELECT MAX(Date) FROM MicrosoftSummaryStats t1 INNER JOIN vitalsigns.dbo.ServerTypes t2
						ON t1.ServerTypeId=t2.ID
						WHERE StatName LIKE 'Disk.%' AND ServerName = @ServerNameIn AND t2.ServerType IN(@ServerTypeIn))
					)) AND ServerName = @ServerNameIn AND mt2.ServerType IN (@ServerTypeIn)
					AND SUBSTRING(SUBSTRING(StatName,6,LEN(StatName)),1,LEN(StatName))= @DiskNameIn
					ORDER BY Date
					
				OPEN @DiskUsageCursor
				
					FETCH NEXT FROM @DiskUsageCursor 
					INTO @ServerName, @Date, @DiskName, @StatValue
					SET @PrevStatValue = @StatValue
					SET @CurrStatValue = @StatValue
					WHILE @@FETCH_STATUS = 0 
			
					BEGIN 
						
						SET @StatValueDiff = @StatValueDiff + (@PrevStatValue - @CurrStatValue)
						SET @Count = @Count + 1
						FETCH NEXT FROM @DiskUsageCursor 
						INTO @ServerName, @Date, @DiskName, @StatValue
						SET @PrevStatValue = @CurrStatValue
						SET @CurrStatValue = @StatValue
						--Print 'Diff: ' + CAST(@StatValueDiff as varchar(100))
						--Print 'Count: ' + CAST(@Count as varchar(20))
					END 
					
				CLOSE @DiskUsageCursor 
				DEALLOCATE @DiskUsageCursor
				IF @Count = 0 OR @Count = 1
					SET @DiskConsumption = 0
				ELSE
					SET @DiskConsumption = ROUND(@StatValueDiff/(@Count-1),1)
		END
		ELSE -- Domino is not in @ServerTypeIn
		BEGIN
			SET @DiskUsageCursor = CURSOR FAST_FORWARD 

				FOR 
					SELECT ServerName,Date,SUBSTRING(SUBSTRING(StatName,6,LEN(StatName)),1,LEN(StatName)) DiskName, 
					ISNULL(StatValue,0)/1024/1024 StatValue
					FROM dbo.MicrosoftSummaryStats mt1 INNER JOIN vitalsigns.dbo.ServerTypes mt2 ON mt1.ServerTypeId=mt2.ID
					WHERE StatName LIKE 'Disk.%' AND DATEDIFF(dd,0,Date) >= DATEDIFF(dd,0,DATEADD(dd,-30,
						(SELECT MAX(Date) FROM MicrosoftSummaryStats t1 INNER JOIN vitalsigns.dbo.ServerTypes t2
						ON t1.ServerTypeId=t2.ID
						WHERE StatName LIKE 'Disk.%' AND ServerName = @ServerNameIn AND t2.ServerType IN(@ServerTypeIn))
					)) AND ServerName = @ServerNameIn AND mt2.ServerType IN (@ServerTypeIn)
					AND SUBSTRING(SUBSTRING(StatName,6,LEN(StatName)),1,LEN(StatName))= @DiskNameIn
					ORDER BY Date
					
				OPEN @DiskUsageCursor
				
					FETCH NEXT FROM @DiskUsageCursor 
					INTO @ServerName, @Date, @DiskName, @StatValue
					SET @PrevStatValue = @StatValue
					SET @CurrStatValue = @StatValue
					WHILE @@FETCH_STATUS = 0 
			
					BEGIN 
						
						SET @StatValueDiff = @StatValueDiff + (@PrevStatValue - @CurrStatValue)
						SET @Count = @Count + 1
						FETCH NEXT FROM @DiskUsageCursor 
						INTO @ServerName, @Date, @DiskName, @StatValue
						SET @PrevStatValue = @CurrStatValue
						SET @CurrStatValue = @StatValue
						--Print 'Diff: ' + CAST(@StatValueDiff as varchar(100))
						--Print 'Count: ' + CAST(@Count as varchar(20))
					END 
					
				CLOSE @DiskUsageCursor 
				DEALLOCATE @DiskUsageCursor
				IF @Count = 0 OR @Count = 1
					SET @DiskConsumption = 0
				ELSE
					SET @DiskConsumption = ROUND(@StatValueDiff/(@Count-1),1)
		END
	END
	ELSE --ServerType is empty
	BEGIN
		SET @DiskUsageCursor = CURSOR FAST_FORWARD 

			FOR 
				SELECT ServerName,Date,SUBSTRING(SUBSTRING(StatName,6,LEN(StatName)),1,CHARINDEX('.',SUBSTRING(StatName,6,LEN(StatName)))-1) DiskName, 
				ISNULL(StatValue,0)/1024/1024 StatValue
				FROM dbo.DominoSummaryStats
				WHERE StatName LIKE 'Disk%Free' AND DATEDIFF(dd,0,Date) >= DATEDIFF(dd,0,DATEADD(dd,-30,
					(SELECT MAX(Date) FROM DominoSummaryStats WHERE StatName LIKE 'Disk%Free' AND ServerName = @ServerNameIn)
				)) AND ServerName = @ServerNameIn
				AND SUBSTRING(SUBSTRING(StatName,6,LEN(StatName)),1,CHARINDEX('.',SUBSTRING(StatName,6,LEN(StatName)))-1)= @DiskNameIn
				UNION
				SELECT ServerName,Date,SUBSTRING(SUBSTRING(StatName,6,LEN(StatName)),1,LEN(StatName)) DiskName, 
				ISNULL(StatValue,0)/1024/1024 StatValue
				FROM dbo.MicrosoftSummaryStats
				WHERE StatName LIKE 'Disk.%' AND DATEDIFF(dd,0,Date) >= DATEDIFF(dd,0,DATEADD(dd,-30,
					(SELECT MAX(Date) FROM MicrosoftSummaryStats WHERE StatName LIKE 'Disk.%' AND ServerName = @ServerNameIn)
				)) AND ServerName = @ServerNameIn
				AND SUBSTRING(SUBSTRING(StatName,6,LEN(StatName)),1,LEN(StatName))= @DiskNameIn
				ORDER BY Date
				
			OPEN @DiskUsageCursor
			
				FETCH NEXT FROM @DiskUsageCursor 
				INTO @ServerName, @Date, @DiskName, @StatValue
				SET @PrevStatValue = @StatValue
				SET @CurrStatValue = @StatValue
				WHILE @@FETCH_STATUS = 0 
		
				BEGIN 
					
					SET @StatValueDiff = @StatValueDiff + (@PrevStatValue - @CurrStatValue)
					SET @Count = @Count + 1
					FETCH NEXT FROM @DiskUsageCursor 
					INTO @ServerName, @Date, @DiskName, @StatValue
					SET @PrevStatValue = @CurrStatValue
					SET @CurrStatValue = @StatValue
					--Print 'Diff: ' + CAST(@StatValueDiff as varchar(100))
					--Print 'Count: ' + CAST(@Count as varchar(20))
				END 
				
			CLOSE @DiskUsageCursor 
			DEALLOCATE @DiskUsageCursor
			IF @Count = 0 OR @Count = 1
				SET @DiskConsumption = 0
			ELSE
				SET @DiskConsumption = ROUND(@StatValueDiff/(@Count-1),1)
	END
END


GO

/****** Object:  StoredProcedure [dbo].[CalcAvgDiskConsumptionTotal]    Script Date: 09/23/2013 17:02:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Natallya Shkarayeva
-- Create date: 8/11/2014 - added Exchange, 2/6/2015 - modified
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[CalcAvgDiskConsumptionTotal]
	@ServerTypeIn as varchar(50), @ServerNameIn as varchar(50),@ExactMatch as int, @DiskConsumption as  float output
AS
BEGIN
	DECLARE @Date datetime
	DECLARE @StatValue float
	DECLARE @PrevStatValue float
	DECLARE @CurrStatValue float
	DECLARE @StatValueDiff float
	DECLARE @Count int
	DECLARE @XMLList xml
	DECLARE @DiskUsageCursor CURSOR
	
	SET @Count = 0
	SET @StatValueDiff = 0
	
	IF @ServerTypeIn != ''
	BEGIN 
		IF CHARINDEX('Domino',@ServerTypeIn) != 0
		BEGIN
			IF @ServerNameIn != ''
			BEGIN
				IF @ExactMatch != 0
				BEGIN
					SET @XMLList=cast('<i>'+replace(@ServerNameIn,',','</i><i>')+'</i>' as xml)
				SET @DiskUsageCursor = CURSOR FAST_FORWARD
				FOR 
					SELECT (SUM(StatValue))/1024/1024 StatValue, Date FROM dbo.DominoSummaryStats
					WHERE StatName LIKE 'Disk%Free' AND DATEDIFF(dd,0,Date) >= DATEDIFF(dd,0,DATEADD(dd,-30,
						(SELECT MAX(Date) FROM DominoSummaryStats WHERE StatName LIKE 'Disk%Free' AND ServerName IN 
						(SELECT x.i.value('.','varchar(100)') from @XMLList.nodes('i') x(i)))
						)) AND ServerName IN (SELECT x.i.value('.','varchar(100)') from @XMLList.nodes('i') x(i))
					GROUP BY Date
					UNION
					SELECT SUM(StatValue)/1024/1024 StatValue, Date FROM dbo.MicrosoftSummaryStats mt1
					INNER JOIN vitalsigns.dbo.ServerTypes mt2 ON mt1.ServerTypeId=mt2.ID
					WHERE StatName LIKE 'Disk.%' AND DATEDIFF(dd,0,Date) >= DATEDIFF(dd,0,DATEADD(dd,-30,
						(SELECT MAX(Date) FROM MicrosoftSummaryStats t1 INNER JOIN vitalsigns.dbo.ServerTypes t2
						ON t1.ServerTypeId=t2.ID
						WHERE StatName LIKE 'Disk.%' AND ServerName IN 
						(SELECT x.i.value('.','varchar(100)') from @XMLList.nodes('i') x(i)) AND t2.ServerType IN (@ServerTypeIn)
						))) AND ServerName IN (SELECT x.i.value('.','varchar(100)') from @XMLList.nodes('i') x(i))
						AND mt2.ServerType IN (@ServerTypeIn)
					GROUP BY Date
					ORDER BY Date
				OPEN @DiskUsageCursor
				
					FETCH NEXT FROM @DiskUsageCursor 
					INTO @StatValue,@Date
					SET @PrevStatValue = @StatValue
					SET @CurrStatValue = @StatValue
					WHILE @@FETCH_STATUS = 0 
			
					BEGIN 
						--Print 'ServerName: ' + @ServerNameIn
						SET @StatValueDiff = @StatValueDiff + (@PrevStatValue - @CurrStatValue)
						SET @Count = @Count + 1
						FETCH NEXT FROM @DiskUsageCursor 
						INTO @StatValue,@Date
						SET @PrevStatValue = @CurrStatValue
						SET @CurrStatValue = @StatValue
						--Print 'Diff: ' + CAST(@StatValueDiff as varchar(100))
						--Print 'Count: ' + CAST(@Count as varchar(20))
					END 
				END
				ELSE
				BEGIN
					SET @DiskUsageCursor = CURSOR FAST_FORWARD
					FOR 
						SELECT (SUM(StatValue))/1024/1024 StatValue FROM dbo.DominoSummaryStats
						WHERE StatName LIKE 'Disk%Free' AND DATEDIFF(dd,0,Date) >= DATEDIFF(dd,0,DATEADD(dd,-30,
							(SELECT MAX(Date) FROM DominoSummaryStats WHERE StatName LIKE 'Disk%Free' AND 
							ServerName LIKE '%' + @ServerNameIn + '%')
							)) AND ServerName LIKE '%' + @ServerNameIn + '%'
						GROUP BY Date,StatName
						UNION
						SELECT SUM(StatValue)/1024/1024 StatValue FROM dbo.MicrosoftSummaryStats mt1
						INNER JOIN vitalsigns.dbo.ServerTypes mt2 ON mt1.ServerTypeId=mt2.ID
						WHERE StatName LIKE 'Disk.%' AND DATEDIFF(dd,0,Date) >= DATEDIFF(dd,0,DATEADD(dd,-30,
							(SELECT MAX(Date) FROM MicrosoftSummaryStats t1 INNER JOIN vitalsigns.dbo.ServerTypes t2
							ON t1.ServerTypeId=t2.ID
							WHERE StatName LIKE 'Disk.%' AND 
							ServerName LIKE '%' + @ServerNameIn + '%' AND t2.ServerType IN (@ServerTypeIn))
							)) AND ServerName LIKE '%' + @ServerNameIn + '%' AND mt2.ServerType IN (@ServerTypeIn)
						GROUP BY Date,StatName
						
					OPEN @DiskUsageCursor
					
						FETCH NEXT FROM @DiskUsageCursor 
						INTO @StatValue
						SET @PrevStatValue = @StatValue
						SET @CurrStatValue = @StatValue
						WHILE @@FETCH_STATUS = 0 
				
						BEGIN 
							--Print 'ServerName: ' + @ServerNameIn
							SET @StatValueDiff = @StatValueDiff + (@PrevStatValue - @CurrStatValue)
							SET @Count = @Count + 1
							FETCH NEXT FROM @DiskUsageCursor 
							INTO @StatValue
							SET @PrevStatValue = @CurrStatValue
							SET @CurrStatValue = @StatValue
							--Print 'Diff: ' + CAST(@StatValueDiff as varchar(100))
							--Print 'Count: ' + CAST(@Count as varchar(20))
						END 
				END
			END
			ELSE --ServerName is empty
			BEGIN
				SET @DiskUsageCursor = CURSOR FAST_FORWARD
				FOR 
					SELECT (SUM(StatValue))/1024/1024 StatValue FROM dbo.DominoSummaryStats
					WHERE StatName LIKE 'Disk%Free' AND DATEDIFF(dd,0,Date) >= DATEDIFF(dd,0,DATEADD(dd,-30,
						(SELECT MAX(Date) FROM DominoSummaryStats WHERE StatName LIKE 'Disk%Free')
					))
					GROUP BY Date,StatName
					UNION
					SELECT SUM(StatValue)/1024/1024 StatValue FROM dbo.MicrosoftSummaryStats mt1
					INNER JOIN vitalsigns.dbo.ServerTypes mt2 ON mt1.ServerTypeId=mt2.ID
					WHERE StatName LIKE 'Disk.%' AND DATEDIFF(dd,0,Date) >= DATEDIFF(dd,0,DATEADD(dd,-30,
						(SELECT MAX(Date) FROM MicrosoftSummaryStats t1 INNER JOIN vitalsigns.dbo.ServerTypes t2
						ON t1.ServerTypeId=t2.ID
						WHERE StatName LIKE 'Disk.%'
						AND t2.ServerType IN (@ServerTypeIn))
					)) AND mt2.ServerType IN (@ServerTypeIn)
					GROUP BY Date,StatName
					
				OPEN @DiskUsageCursor
				
					FETCH NEXT FROM @DiskUsageCursor 
					INTO @StatValue
					SET @PrevStatValue = @StatValue
					SET @CurrStatValue = @StatValue
					WHILE @@FETCH_STATUS = 0 
			
					BEGIN 
						SET @StatValueDiff = @StatValueDiff + (@PrevStatValue - @CurrStatValue)
						SET @Count = @Count + 1
						FETCH NEXT FROM @DiskUsageCursor 
						INTO @StatValue
						SET @PrevStatValue = @CurrStatValue
						SET @CurrStatValue = @StatValue
						--Print 'Diff: ' + CAST(@StatValueDiff as varchar(100))
						--Print 'Count: ' + CAST(@Count as varchar(20))
					END 
			END	
		END
		ELSE -- Domino is not one of the selected Server Types
		BEGIN -- BEGIN Domino is not one of the selected Server Types
			IF @ServerNameIn != ''
			BEGIN
				IF @ExactMatch != 0
				BEGIN
					SET @XMLList=cast('<i>'+replace(@ServerNameIn,',','</i><i>')+'</i>' as xml)
				SET @DiskUsageCursor = CURSOR FAST_FORWARD
				FOR 
					SELECT SUM(StatValue)/1024/1024 StatValue, Date FROM dbo.MicrosoftSummaryStats mt1
					INNER JOIN vitalsigns.dbo.ServerTypes mt2 ON mt1.ServerTypeId=mt2.ID
					WHERE StatName LIKE 'Disk.%' AND DATEDIFF(dd,0,Date) >= DATEDIFF(dd,0,DATEADD(dd,-30,
						(SELECT MAX(Date) FROM MicrosoftSummaryStats t1 INNER JOIN vitalsigns.dbo.ServerTypes t2
						ON t1.ServerTypeId=t2.ID
						WHERE StatName LIKE 'Disk.%' AND ServerName IN 
						(SELECT x.i.value('.','varchar(100)') from @XMLList.nodes('i') x(i)) AND t2.ServerType IN (@ServerTypeIn))
						)) AND ServerName IN (SELECT x.i.value('.','varchar(100)') from @XMLList.nodes('i') x(i))
						AND mt2.ServerType IN (@ServerTypeIn)
					GROUP BY Date
					ORDER BY Date
				OPEN @DiskUsageCursor
				
					FETCH NEXT FROM @DiskUsageCursor 
					INTO @StatValue,@Date
					SET @PrevStatValue = @StatValue
					SET @CurrStatValue = @StatValue
					WHILE @@FETCH_STATUS = 0 
			
					BEGIN 
						--Print 'ServerName: ' + @ServerNameIn
						SET @StatValueDiff = @StatValueDiff + (@PrevStatValue - @CurrStatValue)
						SET @Count = @Count + 1
						FETCH NEXT FROM @DiskUsageCursor 
						INTO @StatValue,@Date
						SET @PrevStatValue = @CurrStatValue
						SET @CurrStatValue = @StatValue
						--Print 'Diff: ' + CAST(@StatValueDiff as varchar(100))
						--Print 'Count: ' + CAST(@Count as varchar(20))
					END 
				END
				ELSE
				BEGIN
					SET @DiskUsageCursor = CURSOR FAST_FORWARD
					FOR 
						SELECT SUM(StatValue)/1024/1024 StatValue FROM dbo.MicrosoftSummaryStats mt1
						INNER JOIN vitalsigns.dbo.ServerTypes mt2 ON mt1.ServerTypeId=mt2.ID
						WHERE StatName LIKE 'Disk.%' AND DATEDIFF(dd,0,Date) >= DATEDIFF(dd,0,DATEADD(dd,-30,
							(SELECT MAX(Date) FROM MicrosoftSummaryStats t1 INNER JOIN vitalsigns.dbo.ServerTypes t2
							ON t1.ServerTypeId=t2.ID
							WHERE StatName LIKE 'Disk.%' AND 
							ServerName LIKE '%' + @ServerNameIn + '%' AND t2.ServerType IN (@ServerTypeIn))
							)) AND ServerName LIKE '%' + @ServerNameIn + '%' AND mt2.ServerType IN (@ServerTypeIn)
						GROUP BY Date,StatName
						
					OPEN @DiskUsageCursor
					
						FETCH NEXT FROM @DiskUsageCursor 
						INTO @StatValue
						SET @PrevStatValue = @StatValue
						SET @CurrStatValue = @StatValue
						WHILE @@FETCH_STATUS = 0 
				
						BEGIN 
							--Print 'ServerName: ' + @ServerNameIn
							SET @StatValueDiff = @StatValueDiff + (@PrevStatValue - @CurrStatValue)
							SET @Count = @Count + 1
							FETCH NEXT FROM @DiskUsageCursor 
							INTO @StatValue
							SET @PrevStatValue = @CurrStatValue
							SET @CurrStatValue = @StatValue
							--Print 'Diff: ' + CAST(@StatValueDiff as varchar(100))
							--Print 'Count: ' + CAST(@Count as varchar(20))
						END 
				END
			END
			ELSE --ServerName is empty
			BEGIN
				SET @DiskUsageCursor = CURSOR FAST_FORWARD
				FOR 
					SELECT SUM(StatValue)/1024/1024 StatValue FROM dbo.MicrosoftSummaryStats mt1
					INNER JOIN vitalsigns.dbo.ServerTypes mt2 ON mt1.ServerTypeId=mt2.ID
					WHERE StatName LIKE 'Disk.%' AND DATEDIFF(dd,0,Date) >= DATEDIFF(dd,0,DATEADD(dd,-30,
						(SELECT MAX(Date) FROM MicrosoftSummaryStats t1 INNER JOIN vitalsigns.dbo.ServerTypes t2
						ON t1.ServerTypeId=t2.ID
						WHERE StatName LIKE 'Disk.%'
						AND t2.ServerType IN (@ServerTypeIn))
					)) AND mt2.ServerType IN (@ServerTypeIn)
					GROUP BY Date,StatName
					
				OPEN @DiskUsageCursor
				
					FETCH NEXT FROM @DiskUsageCursor 
					INTO @StatValue
					SET @PrevStatValue = @StatValue
					SET @CurrStatValue = @StatValue
					WHILE @@FETCH_STATUS = 0 
			
					BEGIN 
						SET @StatValueDiff = @StatValueDiff + (@PrevStatValue - @CurrStatValue)
						SET @Count = @Count + 1
						FETCH NEXT FROM @DiskUsageCursor 
						INTO @StatValue
						SET @PrevStatValue = @CurrStatValue
						SET @CurrStatValue = @StatValue
						--Print 'Diff: ' + CAST(@StatValueDiff as varchar(100))
						--Print 'Count: ' + CAST(@Count as varchar(20))
					END 
			END
		END -- END Domino is not one of the selected Server Types
	END
	ELSE --ServerType is empty
	BEGIN
		IF @ServerNameIn != ''
		BEGIN
			IF @ExactMatch != 0
			BEGIN
				SET @XMLList=cast('<i>'+replace(@ServerNameIn,',','</i><i>')+'</i>' as xml)
			SET @DiskUsageCursor = CURSOR FAST_FORWARD
			FOR 
				SELECT (SUM(StatValue))/1024/1024 StatValue, Date FROM dbo.DominoSummaryStats
				WHERE StatName LIKE 'Disk%Free' AND DATEDIFF(dd,0,Date) >= DATEDIFF(dd,0,DATEADD(dd,-30,
					(SELECT MAX(Date) FROM DominoSummaryStats WHERE StatName LIKE 'Disk%Free' AND ServerName IN 
					(SELECT x.i.value('.','varchar(100)') from @XMLList.nodes('i') x(i)))
					)) AND ServerName IN (SELECT x.i.value('.','varchar(100)') from @XMLList.nodes('i') x(i))
				GROUP BY Date
				UNION
				SELECT SUM(StatValue)/1024/1024 StatValue, Date FROM dbo.MicrosoftSummaryStats
				WHERE StatName LIKE 'Disk.%' AND DATEDIFF(dd,0,Date) >= DATEDIFF(dd,0,DATEADD(dd,-30,
					(SELECT MAX(Date) FROM MicrosoftSummaryStats WHERE StatName LIKE 'Disk.%' AND ServerName IN 
					(SELECT x.i.value('.','varchar(100)') from @XMLList.nodes('i') x(i)))
					)) AND ServerName IN (SELECT x.i.value('.','varchar(100)') from @XMLList.nodes('i') x(i))
				GROUP BY Date
				ORDER BY Date
			OPEN @DiskUsageCursor
			
				FETCH NEXT FROM @DiskUsageCursor 
				INTO @StatValue,@Date
				SET @PrevStatValue = @StatValue
				SET @CurrStatValue = @StatValue
				WHILE @@FETCH_STATUS = 0 
		
				BEGIN 
					--Print 'ServerName: ' + @ServerNameIn
					SET @StatValueDiff = @StatValueDiff + (@PrevStatValue - @CurrStatValue)
					SET @Count = @Count + 1
					FETCH NEXT FROM @DiskUsageCursor 
					INTO @StatValue,@Date
					SET @PrevStatValue = @CurrStatValue
					SET @CurrStatValue = @StatValue
					--Print 'Diff: ' + CAST(@StatValueDiff as varchar(100))
					--Print 'Count: ' + CAST(@Count as varchar(20))
				END 
			END
			ELSE
			BEGIN
				SET @DiskUsageCursor = CURSOR FAST_FORWARD
				FOR 
					SELECT (SUM(StatValue))/1024/1024 StatValue FROM dbo.DominoSummaryStats
					WHERE StatName LIKE 'Disk%Free' AND DATEDIFF(dd,0,Date) >= DATEDIFF(dd,0,DATEADD(dd,-30,
						(SELECT MAX(Date) FROM DominoSummaryStats WHERE StatName LIKE 'Disk%Free' AND 
						ServerName LIKE '%' + @ServerNameIn + '%')
						)) AND ServerName LIKE '%' + @ServerNameIn + '%'
					GROUP BY Date,StatName
					UNION
					SELECT SUM(StatValue)/1024/1024 StatValue FROM dbo.MicrosoftSummaryStats
					WHERE StatName LIKE 'Disk.%' AND DATEDIFF(dd,0,Date) >= DATEDIFF(dd,0,DATEADD(dd,-30,
						(SELECT MAX(Date) FROM MicrosoftSummaryStats WHERE StatName LIKE 'Disk.%' AND 
						ServerName LIKE '%' + @ServerNameIn + '%')
						)) AND ServerName LIKE '%' + @ServerNameIn + '%'
					GROUP BY Date,StatName
					
				OPEN @DiskUsageCursor
				
					FETCH NEXT FROM @DiskUsageCursor 
					INTO @StatValue
					SET @PrevStatValue = @StatValue
					SET @CurrStatValue = @StatValue
					WHILE @@FETCH_STATUS = 0 
			
					BEGIN 
						--Print 'ServerName: ' + @ServerNameIn
						SET @StatValueDiff = @StatValueDiff + (@PrevStatValue - @CurrStatValue)
						SET @Count = @Count + 1
						FETCH NEXT FROM @DiskUsageCursor 
						INTO @StatValue
						SET @PrevStatValue = @CurrStatValue
						SET @CurrStatValue = @StatValue
						--Print 'Diff: ' + CAST(@StatValueDiff as varchar(100))
						--Print 'Count: ' + CAST(@Count as varchar(20))
					END 
			END
		END
		ELSE --ServerName is empty
		BEGIN
			SET @DiskUsageCursor = CURSOR FAST_FORWARD
			FOR 
				SELECT (SUM(StatValue))/1024/1024 StatValue FROM dbo.DominoSummaryStats
				WHERE StatName LIKE 'Disk%Free' AND DATEDIFF(dd,0,Date) >= DATEDIFF(dd,0,DATEADD(dd,-30,
					(SELECT MAX(Date) FROM DominoSummaryStats WHERE StatName LIKE 'Disk%Free')
				))
				GROUP BY Date,StatName
				UNION
				SELECT SUM(StatValue)/1024/1024 StatValue FROM dbo.MicrosoftSummaryStats
				WHERE StatName LIKE 'Disk.%' AND DATEDIFF(dd,0,Date) >= DATEDIFF(dd,0,DATEADD(dd,-30,
					(SELECT MAX(Date) FROM MicrosoftSummaryStats WHERE StatName LIKE 'Disk.%')
				))
				GROUP BY Date,StatName
				
			OPEN @DiskUsageCursor
			
				FETCH NEXT FROM @DiskUsageCursor 
				INTO @StatValue
				SET @PrevStatValue = @StatValue
				SET @CurrStatValue = @StatValue
				WHILE @@FETCH_STATUS = 0 
		
				BEGIN 
					SET @StatValueDiff = @StatValueDiff + (@PrevStatValue - @CurrStatValue)
					SET @Count = @Count + 1
					FETCH NEXT FROM @DiskUsageCursor 
					INTO @StatValue
					SET @PrevStatValue = @CurrStatValue
					SET @CurrStatValue = @StatValue
					--Print 'Diff: ' + CAST(@StatValueDiff as varchar(100))
					--Print 'Count: ' + CAST(@Count as varchar(20))
				END 
		END
	END
	CLOSE @DiskUsageCursor 
	DEALLOCATE @DiskUsageCursor
	IF @Count = 0 OR @Count = 1
		SET @DiskConsumption = 0
	ELSE
		SET @DiskConsumption = ROUND(@StatValueDiff/(@Count-1),1)
	--Print 'Consumption: ' + CAST(@DiskConsumption as varchar(20))
END


GO

/****** Object:  View [dbo].[maxtime]    Script Date: 09/23/2013 17:02:20 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create view [dbo].[maxtime] as
Select distinct ServerName, MAX(convert(varchar(5),Date,108)) as  Hour  from [VSS_Statistics].[dbo].[DeviceDailyStats]group by ServerName
GO
/****** Object:  View [dbo].[maxstat]    Script Date: 09/23/2013 17:02:20 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
Create view [dbo].[maxstat] as
select distinct ServerName, MAX(statvalue) as MAXval from [VSS_Statistics].[dbo].[DeviceDailyStats]group by ServerName
GO
/****** Object:  StoredProcedure [dbo].[GetDeviceHourlyVals]    Script Date: 09/23/2013 17:02:20 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Mukund Dunakhe
-- VSPLUS-900	Performance problems with loading "Server Detailed" page.
-- Create date: 03 Nov 2014
-- Description:	Get the data for Dashboard graphs for Domino Servers
-- =============================================
CREATE PROCEDURE [dbo].[GetDeviceHourlyVals] 
	@dtfrom datetime,@DeviceName varchar(150),@StatName varchar(150)
AS
BEGIN
Declare @dtto datetime
--set	@dtto=DATEADD(hour,-24,@dtfrom)
set	@dtto=@dtfrom
set	@dtfrom = DATEADD(hour,-24,@dtfrom)
	
		if @StatName = 'ResponseTime'
			begin
				Select isnull(Max(statvalue),0) as maxval,CONVERT(datetime, CAST(CONVERT(DATE, [Date]) as varchar) + ' ' +CAST(DATEPART(hour, DATEADD(hour,1,[Date])) as varchar(2)) + ':00') as dtfrom from DeviceDailyStats 
				where ServerName=@DeviceName and statname=@StatName
				and [Date] >= @dtfrom
				and [Date] < @dtto
				group by CONVERT(datetime, CAST(CONVERT(DATE, [Date]) as varchar) + ' ' +CAST(DATEPART(hour, DATEADD(hour,1,[Date])) as varchar(2)) + ':00')
			end
		else
			begin
			
				Select isnull(Max(statvalue),0) as maxval,CONVERT(datetime, CAST(CONVERT(DATE, [Date]) as varchar) + ' ' +CAST(DATEPART(hour, DATEADD(hour,1,[Date])) as varchar(2)) + ':00') as dtfrom from DominoDailyStats 
				where ServerName=@DeviceName and statname=@StatName
				and [Date] >= @dtfrom
				and [Date] < @dtto
				group by CONVERT(datetime, CAST(CONVERT(DATE, [Date]) as varchar) + ' ' +CAST(DATEPART(hour, DATEADD(hour,1,[Date])) as varchar(2)) + ':00')
				
				
			end
END
GO
/****** Object:  UserDefinedFunction [dbo].[CalculateAvgDiskConsumtpion]    Script Date: 09/23/2013 17:02:20 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date, ,>
-- Description:	<Description, ,>
-- =============================================
CREATE FUNCTION [dbo].[CalculateAvgDiskConsumtpion]
(
	@ServerNameIn as varchar(50),@DiskNameIn as varchar(10)
)
RETURNS float
AS
BEGIN
	DECLARE @ServerName varchar(50) 
	DECLARE @Date datetime
	DECLARE @DiskName varchar(10)
	DECLARE @StatValue float
	DECLARE @PrevStatValue float
	DECLARE @CurrStatValue float
	DECLARE @StatValueDiff float
	DECLARE @Count int
	DECLARE @DiskUsageCursor CURSOR
	DECLARE @DiskConsumption float
	
	SET @Count = 0
	SET @StatValueDiff = 0
	SET @DiskUsageCursor = CURSOR FAST_FORWARD 

		FOR 
			SELECT ServerName,Date,SUBSTRING(SUBSTRING(StatName,6,LEN(StatName)),1,CHARINDEX('.',SUBSTRING(StatName,6,LEN(StatName)))-1) DiskName, 
			StatValue FROM dbo.DominoSummaryStats
			WHERE StatName LIKE 'Disk%Free' AND DATEDIFF(dd,0,Date) >= DATEDIFF(dd,0,DATEADD(dd,-30,
				(SELECT MAX(Date) FROM DominoSummaryStats WHERE StatName LIKE 'Disk%Free' AND ServerName = @ServerNameIn)
			)) AND ServerName = @ServerNameIn
			AND SUBSTRING(SUBSTRING(StatName,6,LEN(StatName)),1,CHARINDEX('.',SUBSTRING(StatName,6,LEN(StatName)))-1)= @DiskNameIn
			ORDER BY Date
			
		OPEN @DiskUsageCursor
		
			FETCH NEXT FROM @DiskUsageCursor 
			INTO @ServerName, @Date, @DiskName, @StatValue
			SET @PrevStatValue = @StatValue
			SET @CurrStatValue = @StatValue
			WHILE @@FETCH_STATUS = 0 
	
			BEGIN 
				
				SET @StatValueDiff = @StatValueDiff + (@PrevStatValue - @CurrStatValue)
				SET @Count = @Count + 1
				FETCH NEXT FROM @DiskUsageCursor 
				INTO @ServerName, @Date, @DiskName, @StatValue
				SET @PrevStatValue = @CurrStatValue
				SET @CurrStatValue = @StatValue
				--Print 'Diff: ' + CAST(@StatValueDiff as varchar(100))
				--Print 'Count: ' + CAST(@Count as varchar(20))
			END 
			
		CLOSE @DiskUsageCursor 
		DEALLOCATE @DiskUsageCursor
		IF @Count = 0 OR @Count = 1
			SET @DiskConsumption = 0
		ELSE
			SET @DiskConsumption = ROUND(((@StatValueDiff/(@Count-1))/1024)/1024,1)
	RETURN @DiskConsumption

END
GO


--Creating Database tables
USE [VSS_Statistics]
GO

/****** Object:  Table [dbo].[DeviceDailyStats]    Script Date: 12/06/2013 22:10:56 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[DeviceUpTimeSummaryStats](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[DeviceType] [nvarchar](100) NULL,
	[ServerName] [nvarchar](100) NULL,
	[Date] [datetime] NULL,
	[StatName] [nvarchar](50) NULL,
	[StatValue] [float] NULL,
	[WeekNumber] [int] NULL,
	[MonthNumber] [int] NULL,
	[YearNumber] [int] NULL,
	[DayNumber] [int] NULL
) ON [PRIMARY]

GO
/* 11/12/2015 NS added for VSPLUS-2331 */
/****** Object:  Index [DeviceUpTimeSummaryStats_DateIndex]    Script Date: 11/12/2015 14:37:57 ******/
CREATE NONCLUSTERED INDEX [DeviceUpTimeSummaryStats_DateIndex] ON [dbo].[DeviceUpTimeSummaryStats] 
(
	[Date] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO

USE [VSS_Statistics]
GO

/****** Object:  Table [dbo].[DeviceDailyStats]    Script Date: 12/06/2013 22:14:38 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[DeviceDailySummaryStats ](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[DeviceType] [nvarchar](100) NULL,
	[ServerName] [nvarchar](100) NULL,
	[Date] [datetime] NULL,
	[StatName] [nvarchar](50) NULL,
	[StatValue] [float] NULL,
	[WeekNumber] [int] NULL,
	[MonthNumber] [int] NULL,
	[YearNumber] [int] NULL,
	[DayNumber] [int] NULL
) ON [PRIMARY]

GO
/* 11/12/2015 NS added for VSPLUS-2331 */
/****** Object:  Index [DeviceDailySummaryStats_DateIndex]    Script Date: 11/12/2015 14:37:37 ******/
CREATE NONCLUSTERED INDEX [DeviceDailySummaryStats_DateIndex] ON [dbo].[DeviceDailySummaryStats ] 
(
	[Date] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO

USE [VSS_Statistics]
GO

/****** Object:  Table [dbo].[BESDailyStats]    Script Date: 12/06/2013 22:17:55 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[BESDailySummaryStats](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[ServerName] [nvarchar](250) NULL,
	[Date] [datetime] NULL,
	[StatName] [nvarchar](250) NULL,
	[StatValue] [float] NULL,
	[WeekNumber] [int] NULL,
	[MonthNumber] [int] NULL,
	[YearNumber] [int] NULL,
	[DayNumber] [int] NULL,
	[HourNumber] [int] NULL
) ON [PRIMARY]

GO

USE [VSS_Statistics]
GO

/****** Object:  Table [dbo].[MicrosoftDailyStats]    Script Date: 12/06/2013 22:17:55 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[MicrosoftDailyStats](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[ServerName] [nvarchar](250) NULL,
	[ServerTypeId] [int] NULL,
	[Date] [datetime] NULL,
	[StatName] [nvarchar](250) NULL,
	[StatValue] [float] NULL,
	[WeekNumber] [int] NULL,
	[MonthNumber] [int] NULL,
	[YearNumber] [int] NULL,
	[DayNumber] [int] NULL,
	[HourNumber] [int] NULL,	
	Details [nvarchar](250) NULL
) ON [PRIMARY]

GO

USE [VSS_Statistics]
GO


SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[MicrosoftSummaryStats](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[ServerName] [nvarchar](250) NULL,
	[ServerTypeId] [int] NULL,
	[Date] [datetime] NULL,
	[StatName] [nvarchar](250) NULL,
	[StatValue] [float] NULL,
	[WeekNumber] [int] NULL,
	[MonthNumber] [int] NULL,
	[YearNumber] [int] NULL,
	[DayNumber] [int] NULL
) ON [PRIMARY]

GO
/* 11/12/2015 NS added for VSPLUS-2331 */
/****** Object:  Index [MicrosoftSummaryStats_DateIndex]    Script Date: 11/12/2015 14:38:22 ******/
CREATE NONCLUSTERED INDEX [MicrosoftSummaryStats_DateIndex] ON [dbo].[MicrosoftSummaryStats] 
(
	[Date] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
/************************* Start of ScanResults Added for DBHealth Server for multithreaded *************************************/

USE [VSS_Statistics]
GO

CREATE TABLE [dbo].[ScanResults](
[ID] [int] IDENTITY(1,1) NOT NULL,
[ScanDate] [datetime] NULL,
[ServerName] [nvarchar](250) NULL,
[ScanCount] [int] NULL,
[DatabaseCount] [int] NULL)

Alter TABLE [dbo].[Daily] Add Temp Bit NULL

/************************* End of ScanResults Added for DBHealth Server for multithreaded *************************************/


/*********************************************Start  of Daily Task Cleanup ********************************************/
USE [VSS_Statistics]
GO

CREATE TABLE [dbo].[ConsolidationResults](
[ID] [int] IDENTITY(1,1) NOT NULL,
[ScanDate] [datetime] NULL,
[Result] [nvarchar](250) NULL)

/*********************************************End of Daily Task Cleanup ********************************************/

GO
/*-----------*/


USE [VSS_Statistics]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Mukund Dunakhe
-- Create date: 18 Feb 2014
-- Description:	Get the data for Dashboard graphs for Notes Mail Probe
-- =============================================
-- =============================================
-- Author:		Mukund Dunakhe
-- VSPLUS-900	Performance problems with loading "Server Detailed" page.
-- Create date: 03 Nov 2014
-- Description:	Get the data for Dashboard graphs for Notes Mail Probe
-- =============================================
Create PROCEDURE [dbo].[GetNotesHourlyVals] 
	@dtfrom datetime,@DeviceName varchar(150),@StatName varchar(150)
AS
BEGIN

Declare @dtto datetime
--set	@dtto=DATEADD(hour,-24,@dtfrom)
set	@dtto=@dtfrom
set	@dtfrom = DATEADD(hour,-24,@dtfrom)

 				Select isnull(Max(statvalue),0) as maxval,CONVERT(datetime, CAST(CONVERT(DATE, [Date]) as varchar) + ' ' +CAST(DATEPART(hour, DATEADD(hour,1,[Date])) as varchar(2)) + ':00') as dtfrom from NotesMailStats 
				where Name=@DeviceName and statname=@StatName
				and [Date] >= @dtfrom
				and [Date] < @dtto
				group by CONVERT(datetime, CAST(CONVERT(DATE, [Date]) as varchar) + ' ' +CAST(DATEPART(hour, DATEADD(hour,1,[Date])) as varchar(2)) + ':00')

END


GO

/* 5/9/2014 NS added for VSPLUS-557 */

USE [VSS_Statistics]
GO
--2/11/2016 Durga Modified for VSPLUS 2174
/****** Object:  Table [dbo].[TravelerSummaryStats]    Script Date: 05/08/2014 16:56:04 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[TravelerSummaryStats](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[TravelerServerName] [varchar](150) NOT NULL,
	[MailServerName] [varchar](150) NOT NULL,
	[StatName] [varchar](50) NOT NULL,
	[Int000001] [float] NULL,
	[Int001002] [float] NULL,
	[Int002005] [float] NULL,
	[Int005010] [float] NULL,
	[Int010030] [float] NULL,
	[Int030060] [float] NULL,
	[Int060120] [float] NULL,
	[Int120INF] [float] NULL,
	[DateUpdated] [datetime] NOT NULL,
 CONSTRAINT [PK_TravelerSummaryStats] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO



USE [VSS_Statistics]
GO

/****** Object:  StoredProcedure [dbo].[PopulateTravelerSummaryStats]    Script Date: 05/08/2014 16:55:21 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Natallya Shkarayeva
-- Create date: 5/9/2014 
-- Description:	Load daily stats into the TravelerSummaryStats table
-- Modified date: 9/11/2015 
-- Modified comments: VSPLUS-2025 - the daily task service runs at midnight so the selection date should be changed
--2/11/2016 Durga Modified for VSPLUS 2174
-- =============================================
CREATE PROCEDURE [dbo].[PopulateTravelerSummaryStats]
	@StatName as varchar(50)
AS
BEGIN
	IF @StatName = 'OpenTimesDelta'
	BEGIN
		INSERT INTO TravelerSummaryStats (TravelerServerName,MailServerName,Statname,Int000001,Int001002,
		Int002005,Int005010,Int010030,Int030060,Int060120,Int120INF,DateUpdated)
		SELECT TravelerServerName,MailServerName,@StatName,
		ISNULL([000-001],0) as [000-001], ISNULL([001-002],0) as [001-002], ISNULL([002-005],0) as [002-005], 
		ISNULL([005-010],0) as [005-010], ISNULL([010-030],0) as [010-030], ISNULL([030-060],0) as [030-060], 
		ISNULL([060-120],0) as [060-120], ISNULL([120-INF],0) as [120-INF], 
		DATEADD(dd,0,DATEDIFF(dd,0,dateupdated)) as DateUpdated
		FROM(
			SELECT TravelerServerName,Mailservername, Interval, Delta, 
			DATEADD(dd,0,DATEDIFF(dd,0,dateupdated)) as DateUpdated
			FROM [vitalsigns].[dbo].[TravelerStats]
			WHERE DateUpdated < DATEDIFF(dd,0,GETDATE())
			and MailServerName !='')
		AS sourcetable
		PIVOT
		(AVG(Delta) FOR Interval in ([000-001], [001-002], [002-005], [005-010], [010-030], [030-060],[060-120], [120-INF])) as pivottabble
	END
	ELSE IF @StatName = 'CumulativeTimesMin'
	BEGIN
		INSERT INTO TravelerSummaryStats (TravelerServerName,MailServerName,Statname,Int000001,Int001002,
		Int002005,Int005010,Int010030,Int030060,Int060120,Int120INF,DateUpdated)
		SELECT TravelerServerName,MailServerName,@StatName,
		ISNULL([000-001],0) as [000-001], ISNULL([001-002],0) as [001-002], ISNULL([002-005],0) as [002-005], 
		ISNULL([005-010],0) as [005-010], ISNULL([010-030],0) as [010-030], ISNULL([030-060],0) as [030-060], 
		ISNULL([060-120],0) as [060-120], ISNULL([120-INF],0) as [120-INF], 
		DATEADD(dd,0,DATEDIFF(dd,0,dateupdated)) as DateUpdated
		FROM(
			SELECT TravelerServerName,Mailservername, Interval, OpenTimes, 
			DATEADD(dd,0,DATEDIFF(dd,0,dateupdated)) as DateUpdated
			FROM [vitalsigns].[dbo].[TravelerStats]
			WHERE DateUpdated = (SELECT MIN(DateUpdated) from [vitalsigns].[dbo].[TravelerStats]
			WHERE DateUpdated < DATEDIFF(dd,0,GETDATE()))
			and MailServerName !='')
		AS sourcetable
		PIVOT
		(SUM(OpenTimes) FOR Interval in ([000-001], [001-002], [002-005], [005-010], [010-030], [030-060],[060-120], [120-INF])) as pivottabble
	END
	ELSE IF @StatName = 'CumulativeTimesMax'
	BEGIN
		INSERT INTO TravelerSummaryStats (TravelerServerName,MailServerName,Statname,Int000001,Int001002,
		Int002005,Int005010,Int010030,Int030060,Int060120,Int120INF,DateUpdated)
		SELECT TravelerServerName,MailServerName,@StatName,
		ISNULL([000-001],0) as [000-001], ISNULL([001-002],0) as [001-002], ISNULL([002-005],0) as [002-005], 
		ISNULL([005-010],0) as [005-010], ISNULL([010-030],0) as [010-030], ISNULL([030-060],0) as [030-060], 
		ISNULL([060-120],0) as [060-120], ISNULL([120-INF],0) as [120-INF], 
		DATEADD(dd,0,DATEDIFF(dd,0,dateupdated)) as DateUpdated
		FROM(
			SELECT TravelerServerName,Mailservername, Interval, OpenTimes, 
			DATEADD(dd,0,DATEDIFF(dd,0,dateupdated)) as DateUpdated
			FROM [vitalsigns].[dbo].[TravelerStats]
			WHERE DateUpdated = (SELECT MAX(DateUpdated) from [vitalsigns].[dbo].[TravelerStats]
			WHERE DateUpdated < DATEDIFF(dd,0,GETDATE()))
			and MailServerName !='')
		AS sourcetable
		PIVOT
		(SUM(OpenTimes) FOR Interval in ([000-001], [001-002], [002-005], [005-010], [010-030], [030-060],[060-120], [120-INF])) as pivottabble
	END
END

GO


--Mukund 26-Aug-14
USE [VSS_Statistics]

GO

--Mukund 15Sep14, Sharepoint, BES related
Use VSS_Statistics

Go

--Mukund 15Sep14, Sharepoint, BES related
Use VSS_Statistics
Go
-- =============================================
-- Author:		Mukund Dunakhe
-- VSPLUS-900	Performance problems with loading "Server Detailed" page.
-- Create date: 03 Nov 2014
-- Description:	Get the data for Dashboard graphs for BES servers
-- =============================================
CREATE PROCEDURE [dbo].[GetBESHourlyVals] 
 @dtfrom datetime,@deviceName varchar(150),@StatName varchar(150)
AS
BEGIN
Declare @dtto datetime
--set	@dtto=DATEADD(hour,-24,@dtfrom)
set	@dtto=@dtfrom
set	@dtfrom = DATEADD(hour,-24,@dtfrom)
	

 				Select isnull(Max(statvalue),0) as maxval,CONVERT(datetime, CAST(CONVERT(DATE, [Date]) as varchar) + ' ' +CAST(DATEPART(hour, DATEADD(hour,1,[Date])) as varchar(2)) + ':00') as dtfrom from BESDailyStats 
				where ServerName=@DeviceName and statname=@StatName
				and [Date] >= @dtfrom
				and [Date] < @dtto
				group by CONVERT(datetime, CAST(CONVERT(DATE, [Date]) as varchar) + ' ' +CAST(DATEPART(hour, DATEADD(hour,1,[Date])) as varchar(2)) + ':00')

END

Go


--Mukund VSPLUS-1018,1019: 11-Oct-2014

USE [VSS_Statistics]
GO



/****** Mukund VSPLUS-1023:Object:  StoredProcedure [dbo].[GetURLDeviceHourlyVals]    Script Date: 10/17/2014 17:51:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create PROCEDURE [dbo].[GetURLDeviceHourlyVals] 
	@dtfrom datetime,@DeviceName varchar(150),@StatName varchar(150)
AS
BEGIN
--	declare @dtfrom datetime
--set @dtfrom='2012-08-29 15:00:00' 

declare @dtto datetime
	DECLARE @Flag INT
	IF OBJECT_ID('tempdb..#table') IS NOT NULL  
	begin
		Drop table #table
	end
	Create Table #table(maxval float,dtfrom datetime)

	SET @Flag = 0
	WHILE (@Flag < 24)
	BEGIN
		set @dtto=@dtfrom
		set @dtfrom=DATEADD(HH,-1,@dtto) 

		
		--select isnull(Max(statvalue),0) as MaxVal,@dtto from 
		--1/14/2013 NS modified
		if @StatName = 'ResponseTime'
			begin
				insert into #table
				--12/12/2013 NS modified - column name change from DeviceName to ServerName
				--select isnull(Max(statvalue),0) as MaxVal,isnull(max(date),@dtto) as date from 
				--[VSS_Statistics].[dbo].[DeviceDailyStats]  where  [DeviceName]=@DeviceName
				--and StatName=@StatName and
				--Date  between @dtfrom and @dtto
				select isnull(Max(statvalue),0) as MaxVal,isnull(max(date),@dtto) as date from 
				[VSS_Statistics].[dbo].[DeviceDailyStats]  where  [ServerName]=@DeviceName
				and StatName=@StatName and
				Date  between @dtfrom and @dtto
			end
		else
			begin
				insert into #table
				select isnull(Max(statvalue),0) as MaxVal,isnull(max(date),@dtto) as date from 
				[VSS_Statistics].[dbo].[DeviceDailyStats]  where  [ServerName]=@DeviceName
				and StatName=@StatName and
				Date  between @dtfrom and @dtto
			end

		SET @Flag = @Flag + 1
	END

	select * from #table
END
GO


/****** Mukund VSPLUS-1023: Object:  StoredProcedure [dbo].[GetSNMPDevicesHourlyVals]    Script Date: 10/17/2014 14:55:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
Create PROCEDURE [dbo].[GetSNMPDevicesHourlyVals] 
 @dtfrom datetime,@deviceName varchar(150),@StatName varchar(150)
AS
BEGIN
-- declare @dtfrom datetime
--set @dtfrom='2012-08-29 15:00:00' 

declare @dtto datetime
 DECLARE @Flag INT
 IF OBJECT_ID('tempdb..#table') IS NOT NULL  
 begin
  Drop table #table
 end
 Create Table #table(maxval float,dtfrom datetime)

 SET @Flag = 0
 WHILE (@Flag < 24)
 BEGIN
  set @dtto=@dtfrom
  --set @dtto='2014-08-22 13:50:00.000'
  set @dtfrom=DATEADD(HH,-1,@dtto) 

  if @StatName = 'ResponseTime'
   begin
    insert into #table
    select isnull(Max(statvalue),0) as MaxVal,isnull(max(date),@dtto) as date from 
    [VSS_Statistics].[dbo].[DeviceDailyStats]  where  [ServerName]=@DeviceName
    and StatName=@StatName and
    Date  between @dtfrom and @dtto
   end
  else
   begin
    insert into #table
    select isnull(Max(statvalue),0) as MaxVal,isnull(max(date),@dtto) as date from 
    [VSS_Statistics].[dbo].[DeviceDailyStats]  where  [ServerName]=@DeviceName
    and StatName=@StatName and
    Date  between @dtfrom and @dtto
   end

  SET @Flag = @Flag + 1
 END

 select * from #table
END
GO
/****** Mukund VSPLUS-1023: Object:  StoredProcedure [dbo].[GetNetworkDevicesHourlyVals]    Script Date: 10/17/2014 14:55:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
Create PROCEDURE [dbo].[GetNetworkDevicesHourlyVals] 
 @dtfrom datetime,@deviceName varchar(150),@StatName varchar(150)
AS
BEGIN
-- declare @dtfrom datetime
--set @dtfrom='2012-08-29 15:00:00' 

declare @dtto datetime
 DECLARE @Flag INT
 IF OBJECT_ID('tempdb..#table') IS NOT NULL  
 begin
  Drop table #table
 end
 Create Table #table(maxval float,dtfrom datetime)

 SET @Flag = 0
 WHILE (@Flag < 24)
 BEGIN
  set @dtto=@dtfrom
  --set @dtto='2014-08-22 13:50:00.000'
  set @dtfrom=DATEADD(HH,-1,@dtto) 

  if @StatName = 'ResponseTime'
   begin
    insert into #table
    select isnull(Max(statvalue),0) as MaxVal,isnull(max(date),@dtto) as date from 
    [VSS_Statistics].[dbo].[DeviceDailyStats]  where  [ServerName]=@DeviceName
    and StatName=@StatName and
    Date  between @dtfrom and @dtto
   end
  else
   begin
    insert into #table
    select isnull(Max(statvalue),0) as MaxVal,isnull(max(date),@dtto) as date from 
    [VSS_Statistics].[dbo].[DeviceDailyStats]  where  [ServerName]=@DeviceName
    and StatName=@StatName and
    Date  between @dtfrom and @dtto
   end

  SET @Flag = @Flag + 1
 END

 select * from #table
END
GO

/****** Mukund VSPLUS-1023:  Object:  StoredProcedure [dbo].[cloudHourlyVals]    Script Date: 10/20/2014 18:07:12 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create PROCEDURE [dbo].[CloudHourlyVals] 
 @dtfrom datetime,@deviceName varchar(150),@StatName varchar(150)
AS
BEGIN
-- declare @dtfrom datetime
--set @dtfrom='2012-08-29 15:00:00' 

declare @dtto datetime
 DECLARE @Flag INT
 IF OBJECT_ID('tempdb..#table') IS NOT NULL  
 begin
  Drop table #table
 end
 Create Table #table(maxval float,dtfrom datetime)

 SET @Flag = 0
 WHILE (@Flag < 24)
 BEGIN
  set @dtto=@dtfrom
  --set @dtto='2014-08-22 13:50:00.000'
  set @dtfrom=DATEADD(HH,-1,@dtto) 

  if @StatName = 'ResponseTime'
   begin
    insert into #table
    select isnull(Max(statvalue),0) as MaxVal,isnull(max(date),@dtto) as date from 
    [VSS_Statistics].[dbo].[DeviceDailyStats]  where  [ServerName]=@DeviceName
    and StatName=@StatName and
    Date  between @dtfrom and @dtto
   end
  else
   begin
    insert into #table
    select isnull(Max(statvalue),0) as MaxVal,isnull(max(date),@dtto) as date from 
    [VSS_Statistics].[dbo].[DeviceDailyStats]  where  [ServerName]=@DeviceName
    and StatName=@StatName and
    Date  between @dtfrom and @dtto
   end

  SET @Flag = @Flag + 1
 END

 select * from #table
END
GO

USE [VSS_Statistics]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Mukund Dunakhe
-- VSPLUS-900	Performance problems with loading "Server Detailed" page.
-- Create date: 03 Nov 2014
-- Description:	Get the data for Dashboard graphs for Microsoft servers
-- =============================================
CREATE  PROCEDURE [dbo].[GetMicrosoftHourlyVals] 
	@dtfrom datetime,@DeviceName varchar(150),@StatName varchar(150),@ServerTypeId varchar(50)
AS
BEGIN
Declare @dtto datetime
--set	@dtto=DATEADD(hour,-24,@dtfrom)
set	@dtto=@dtfrom
set	@dtfrom = DATEADD(hour,-24,@dtfrom)
	

 				Select isnull(Max(statvalue),0) as maxval,CONVERT(datetime, CAST(CONVERT(DATE, [Date]) as varchar) + ' ' +CAST(DATEPART(hour, DATEADD(hour,1,[Date])) as varchar(2)) + ':00') as dtfrom from MicrosoftDailyStats 
				where ServerName=@DeviceName and statname=@StatName
				and [Date] >= @dtfrom
				and [Date] < @dtto
				group by CONVERT(datetime, CAST(CONVERT(DATE, [Date]) as varchar) + ' ' +CAST(DATEPART(hour, DATEADD(hour,1,[Date])) as varchar(2)) + ':00')

End


GO

/****** VSPLUS-1056-Object:  StoredProcedure [dbo].[GetSametimeHourlyVals]    Script Date: 10/28/2014 16:11:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
 -- =============================================
-- Author:		Mukund Dunakhe
-- VSPLUS-900	Performance problems with loading "Server Detailed" page.
-- Create date: 03 Nov 2014
-- Description:	Get the data for Dashboard graphs for Notes Mail Probe
-- =============================================
Create PROCEDURE [dbo].[GetSametimeHourlyVals] 
 @dtfrom datetime,@deviceName varchar(150),@StatName varchar(150)
AS
BEGIN
Declare @dtto datetime
--set	@dtto=DATEADD(hour,-24,@dtfrom)
set	@dtto=@dtfrom
set	@dtfrom = DATEADD(hour,-24,@dtfrom)

 				Select isnull(Max(statvalue),0) as maxval,CONVERT(datetime, CAST(CONVERT(DATE, [Date]) as varchar) + ' ' +CAST(DATEPART(hour, DATEADD(hour,1,[Date])) as varchar(2)) + ':00') as dtfrom from SametimeDailyStats 
				where ServerName=@DeviceName and statname=@StatName
				and [Date] >= @dtfrom
				and [Date] < @dtto
				group by CONVERT(datetime, CAST(CONVERT(DATE, [Date]) as varchar) + ' ' +CAST(DATEPART(hour, DATEADD(hour,1,[Date])) as varchar(2)) + ':00')

END
GO

--Latency related: Mukund
USE [VSS_Statistics]
GO
CREATE TABLE [dbo].[MailLatencyDailyStats](
	[sourceserver] [nvarchar](100) NULL,
	[DestinationServer] [nvarchar](100) NULL,
	[Latency] [float] NULL,
	[Date] [datetime] NULL
) ON [PRIMARY]

GO
--Somaraju--
USE [VSS_Statistics]
GO
/****** Object:  StoredProcedure [dbo].[GetMailLatencyDailyStats]    Script Date: 12/03/2014 11:50:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

/****** Object:  StoredProcedure [dbo].[GetMicrosoftHourlyVals]    Script Date: 2/10/2014 8:24:58 AM ******/
-- =============================================
-- Author:		Mukund Dunakhe
-- VSPLUS-900	Performance problems with loading "Server Detailed" page.
-- Create date: 03 Nov 2014
-- Description:	Get the data for Dashboard graphs for Microsoft servers
-- =============================================
CREATE  PROCEDURE [dbo].[GetMailLatencyDailyStats] 
	@dtfrom datetime,@sourceserver varchar(150),@DestinationServer varchar(150)
AS
BEGIN
Declare @dtto datetime
--set	@dtto=DATEADD(hour,-24,@dtfrom)
set	@dtto=@dtfrom
set	@dtfrom = DATEADD(hour,-24,@dtfrom)
	

 				Select isnull(Max(Latency),0) as maxval,CONVERT(datetime, CAST(CONVERT(DATE, [Date]) as varchar) + ' ' +CAST(DATEPART(hour, DATEADD(hour,1,[Date])) as varchar(2)) + ':00') as dtfrom from MailLatencyDailyStats 
				where sourceserver=@sourceserver and DestinationServer=@DestinationServer
				and [Date] >= @dtfrom
				and [Date] < @dtto
				group by CONVERT(datetime, CAST(CONVERT(DATE, [Date]) as varchar) + ' ' +CAST(DATEPART(hour, DATEADD(hour,1,[Date])) as varchar(2)) + ':00')

End
---Mukund 12/06/2014 :: HeatMap stored procedure

/****** Object:  StoredProcedure [dbo].[GetExchangeHeatmap] :Mukund   Script Date: 12/06/2014 12:56:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE proc [dbo].[GetExchangeHeatmap]
as
begin

DECLARE @cols AS NVARCHAR(MAX),    @query  AS NVARCHAR(MAX),@maxcols AS NVARCHAR(MAX),@maxcolsred AS NVARCHAR(MAX),@maxcolsyellow AS NVARCHAR(MAX),
@colsred AS NVARCHAR(MAX),@colsyellow AS NVARCHAR(MAX)


SET @cols = STUFF((SELECT distinct ',' + QUOTENAME(c.DestinationServer) 
            FROM MailLatencyStats c
            FOR XML PATH(''), TYPE
            ).value('.', 'NVARCHAR(MAX)') 
        ,1,1,'')
        
 SET @maxcols = STUFF((SELECT distinct ',MAX(' + QUOTENAME(c.DestinationServer) +') as ' + QUOTENAME('To '+c.DestinationServer) 
            FROM MailLatencyStats c
            FOR XML PATH(''), TYPE
            ).value('.', 'NVARCHAR(MAX)') 
        ,1,1,'')
        

set @query = 'SELECT 
[sourceserver] as ''From Server'',' + @maxcols + ',LatencyRedThreshold,LatencyYellowThreshold,[Date]
FROM (
SELECT 
[sourceserver],[DestinationServer],case when Latency=-1 then ''--'' else CAST(Latency AS varchar(20)) end as [Latency],es.LatencyRedThreshold,es.LatencyYellowThreshold,[Date]
from [MailLatencyStats] mls  INNER JOIN vitalsigns.dbo.Servers srv on mls.sourceserver =srv.ServerName
 inner join vitalsigns.dbo.ExchangeSettings es  on srv.ID=es.ServerId
 
) AS query
PIVOT (MAX(Latency)
      FOR DestinationServer IN (' + @cols + ')) AS Pivot0
      GROUP BY
sourceserver,LatencyRedThreshold,LatencyYellowThreshold,Date
'
            Exec(@query )
            

            end


Go

USE [VSS_Statistics]
GO

CREATE TABLE [dbo].[O365AdditionalMailDetails](
	[Server] [varchar](100) NULL,
	[DisplayName] [varchar](200) NULL,
	[LastLogonTime] [datetime] NULL,
	[LastLoggOffTime] [datetime] NULL,
	[IsActive] [bit] NULL,
	[MailBoxType] [varchar](100) NULL
) ON [PRIMARY]

GO

/*1322-Swathi*/

USE [VSS_Statistics]
GO

CREATE PROCEDURE [dbo].[GetWebSphereHourlyVals] 
 @dtfrom datetime,@deviceName varchar(150),@StatName varchar(150)
AS
BEGIN
Declare @dtto datetime
--set @dtto=DATEADD(hour,-24,@dtfrom)
set @dtto=@dtfrom
set @dtfrom = DATEADD(hour,-24,@dtfrom)

     Select isnull(Max(statvalue),0) as maxval,CONVERT(datetime, CAST(CONVERT(DATE, [Date]) as varchar) + ' ' +CAST(DATEPART(hour, DATEADD(hour,1,[Date])) as varchar(2)) + ':00') as dtfrom from WebSphereDailyStats 
    where ServerName=@DeviceName and statname=@StatName
    --and [Date] >= @dtfrom
    --and [Date] < @dtto
    group by CONVERT(datetime, CAST(CONVERT(DATE, [Date]) as varchar) + ' ' +CAST(DATEPART(hour, DATEADD(hour,1,[Date])) as varchar(2)) + ':00')

END

GO


/****** Object:  Table [dbo].[NetworkLatencyStats]    Script Date: 1/23/2015 5:48:22 AM ******/
USE [VSS_Statistics]
GO

CREATE TABLE [dbo].[NetworkLatencyStats](
[NetworkLatencyId] [int],
	[sourceserver] [nvarchar](100) NULL,
	[DestinationServer] [nvarchar](100) NULL,
	[Latency] [float] NULL,
	[Date] [datetime] NULL
) ON [PRIMARY]

GO


/* 2/10/15 WS added */
USE [VSS_Statistics]
GO

/****** Object:  StoredProcedure [dbo].[GetNetworkDeviceHourlyVals]    Script Date: 02/03/2015 16:21:22 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[GetNetworkDeviceHourlyVals] 
	@dtfrom datetime,@DeviceName varchar(150),@StatName varchar(150)
AS
BEGIN
Declare @dtto datetime
--set	@dtto=DATEADD(hour,-24,@dtfrom)
set	@dtto=@dtfrom
set	@dtfrom = DATEADD(hour,-24,@dtfrom)

	Select isnull(Max(statvalue),0) as maxval,CONVERT(datetime, CAST(CONVERT(DATE, [Date]) as varchar) + ' ' +CAST(DATEPART(hour, DATEADD(hour,1,[Date])) as varchar(2)) + ':00') as dtfrom from DeviceDailyStats 
	where ServerName=@DeviceName and statname=@StatName
	and [Date] >= @dtfrom
	and [Date] < @dtto
	group by CONVERT(datetime, CAST(CONVERT(DATE, [Date]) as varchar) + ' ' +CAST(DATEPART(hour, DATEADD(hour,1,[Date])) as varchar(2)) + ':00')


END

GO

/*End of WS addations*/

--Websphere changes Sowjanya
USE [VSS_Statistics]
GO
CREATE TABLE [dbo].[WebSphereDailyStats](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[ServerName] [nvarchar](250) NULL,
	[Date] [datetime] NULL,
	[StatName] [nvarchar](250) NULL,
	[StatValue] [float] NULL,
	[WeekNumber] [int] NULL,
	[MonthNumber] [int] NULL,
	[YearNumber] [int] NULL,
	[DayNumber] [int] NULL,
	[HourNumber] [int] NULL,
	[Details] [nvarchar](250) NULL
) ON [PRIMARY]

GO

--2/18/15 WS ADDED

USE [VSS_Statistics]
GO
/****** Object:  StoredProcedure [dbo].[GetExchangeHeatmap]    Script Date: 02/18/2015 17:07:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
Create proc [dbo].[GetNetworkLatencyHeatmap](
	@NetworkLatencyID 	VARCHAR (250)
)  
as
begin

DECLARE @cols AS NVARCHAR(MAX),    @query  AS NVARCHAR(MAX),@maxcols AS NVARCHAR(MAX),@maxcolsred AS NVARCHAR(MAX),@maxcolsyellow AS NVARCHAR(MAX),
@colsred AS NVARCHAR(MAX),@colsyellow AS NVARCHAR(MAX)


SET @cols = STUFF((SELECT distinct ',' + QUOTENAME(c.DestinationServer) 
            FROM NetworkLatencyStats c WHERE NetworkLatencyID=@NetworkLatencyID
            FOR XML PATH(''), TYPE
            ).value('.', 'NVARCHAR(MAX)') 
        ,1,1,'')
        
 SET @maxcols = STUFF((SELECT distinct ',isnull(MAX(' + QUOTENAME(c.DestinationServer) +'),''--'') as ' + QUOTENAME('To '+c.DestinationServer) 
            FROM NetworkLatencyStats c WHERE NetworkLatencyID=@NetworkLatencyID
            FOR XML PATH(''), TYPE
            ).value('.', 'NVARCHAR(MAX)') 
        ,1,1,'')
        

set @query = '
SELECT 
[sourceserver] as ''From Server'',' + @maxcols + ',LatencyRedThreshold,LatencyYellowThreshold
FROM (
SELECT 
[sourceserver],[DestinationServer],case when Latency=-1 then ''--'' when Latency is null then ''--'' else CAST(Latency AS varchar(20)) end as [Latency],es.LatencyRedThreshold,es.LatencyYellowThreshold,[Date]
from NetworkLatencyStats nls  
INNER JOIN vitalsigns.dbo.Servers srv on nls.sourceserver =srv.ServerName
inner join vitalsigns.dbo.NetworkLatencyServers es  on srv.ID=es.ServerId and nls.NetworkLatencyID=es.NetworkLatencyID
WHERE nls.NetworkLatencyID=' + @NetworkLatencyID + ' 
) AS query
PIVOT (MAX(Latency)
      FOR DestinationServer IN (' + @cols + ')) AS Pivot0
      GROUP BY
sourceserver,LatencyRedThreshold,LatencyYellowThreshold
'
            Exec(@query )
            

end

GO

/* 9/28/15 WS Added for VSPLUS-2193 */

USE [VSS_Statistics]
GO

/****** Object:  StoredProcedure [dbo].[GetWebSphereHourlyVals]    Script Date: 09/24/2015 13:58:58 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[GetWebSphereScanVals] 
 @dtfrom datetime,@deviceName varchar(150),@StatName varchar(150)
AS
BEGIN
Declare @dtto datetime
--set @dtto=DATEADD(hour,-24,@dtfrom)
set @dtto=@dtfrom
set @dtfrom = DATEADD(hour,-24,@dtfrom)

     Select isnull(statvalue,0) as maxval,[Date] as dtfrom from WebSphereDailyStats 
    where ServerName=@DeviceName and statname=@StatName
    --and [Date] >= @dtfrom
    --and [Date] < @dtto
    

END


GO

/* 10/26/15 WS Added for VSPLUS-1347 */

USE [VSS_Statistics]
GO

/****** Object:  Table [dbo].[SharePointSiteRelativeUrl]    Script Date: 10/26/2015 13:22:59 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[SharePointSiteRelativeUrl](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[ServerRelativeUrl] [nvarchar](250) NOT NULL,
 CONSTRAINT [PK_SharePointSiteRelativeUrl] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO


USE [VSS_Statistics]
GO

/****** Object:  Table [dbo].[SharePointUsers]    Script Date: 10/26/2015 13:24:20 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[SharePointUsers](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[UserName] [nvarchar](250) NOT NULL,
 CONSTRAINT [PK_SharePointUsers] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO


USE [VSS_Statistics]
GO

/****** Object:  Table [dbo].[SharePointWebTrafficDailyStats]    Script Date: 10/26/2015 13:24:25 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[SharePointWebTrafficDailyStats](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[ServerName] [nvarchar](250) NULL,
	[UrlId] [int] NULL,
	[UserId] [int] NULL,
	[StatValue] [float] NULL,
	[Date] [datetime] NULL,
	[WeekNumber] [int] NULL,
	[MonthNumber] [int] NULL,
	[YearNumber] [int] NULL,
	[DayNumber] [int] NULL,
	[HourNumber] [int] NULL
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[SharePointWebTrafficDailyStats]  WITH CHECK ADD  CONSTRAINT [FK_UserID_SharePointSiteRelativeUrl_ID] FOREIGN KEY([UrlId])
REFERENCES [dbo].[SharePointSiteRelativeUrl] ([ID])
GO

ALTER TABLE [dbo].[SharePointWebTrafficDailyStats] CHECK CONSTRAINT [FK_UserID_SharePointSiteRelativeUrl_ID]
GO

ALTER TABLE [dbo].[SharePointWebTrafficDailyStats]  WITH CHECK ADD  CONSTRAINT [FK_UserID_SharePointUsers_ID] FOREIGN KEY([UserId])
REFERENCES [dbo].[SharePointUsers] ([ID])
GO

ALTER TABLE [dbo].[SharePointWebTrafficDailyStats] CHECK CONSTRAINT [FK_UserID_SharePointUsers_ID]
GO


--Durga VSPLUS 2281
--VSPLUS-2281  Kiran Dadireddy
/****** Object:  StoredProcedure [dbo].[DominoDailyCleanup]    Script Date: 11/16/2015 15:22:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
Create Procedure [dbo].[DominoDailyCleanup] @month int
AS
Begin

Declare @startdate datetime
Declare @enddate datetime
declare @count int
 select @startdate =  min(Date) FROM DominoDailyStats 
select @enddate = DATEADD(month, 0-@month, GETDATE()) 
 print @startdate
  print @enddate
if @startdate<=@enddate
begin
Select @count=(select count(*) from dominodailystats where date>=@startdate and date<= @enddate)
print 'Toatal Records'+ CONVERT(VARCHAR(50),@count)
 WHILE(@count >0)
 begin
 BEGIN TRANSACTION DELETEDOMINODAILYSTATS
 ;with CTE as 
 (select top(10000)* from dominodailystats where date>=@startdate and date<= @enddate)
 Delete from CTE; 
 print @enddate
 --SET @i = @i+1
 COMMIT TRANSACTION DELETEDOMINODAILYSTATS
 print 'Deleted records'+CONVERT(VARCHAR(50),@count)
  Select @count=(select count(*) from dominodailystats where date>=@startdate and date<= @enddate)
end
 
end

End
Go
GO

--VitalSignsDailytasks Cleanup # kiran #

/****** Object:  StoredProcedure [dbo].[CleanUpObsoleteData]    Script Date: 1/4/2016 2:39:25 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[CleanUpObsoleteData]
(
@DelimatedString VARCHAR(4000)
)
AS
BEGIN
	DECLARE @Temp table(SourceTableName varchar(100))
	DECLARE @sql_string Nvarchar(max) = ''
	DECLARE   @count int
	DECLARE @SourceTableName varchar(100)
	
	INSERT INTO  @Temp SELECT Item FROM vitalsigns.dbo.SplitDelimiterString(@DelimatedString,',')
	WHILE (Select Count(*) From @Temp) > 0
	BEGIN
		SELECT Top 1 @SourceTableName = SourceTableName FROM @Temp
		SET @count=1
		 WHILE(@count >0)
			 BEGIN
				SET @count=0
				BEGIN TRANSACTION DELETEDOMINODAILYSTATS
				SET @sql_string = N' ;with CTE as 
				 (SELECT top(50000)* FROM '+@SourceTableName+' WHERE date<= (GETDATE()-30))
				 DELETE from CTE; 	
				 SELECT @pCount=count(*) from '+@SourceTableName+' WHERE date<= (GETDATE()-30);'
				EXECUTE sp_executesql @sql_string,	N'@pCount int output',@pCount = @count output 				
				COMMIT TRANSACTION DELETEDOMINODAILYSTATS
			 END
		DELETE @Temp WHERE SourceTableName = @SourceTableName
	END
	DELETE FROM NotesMailStats where date <(GETDATE()+1)

END
GO


-- VSPLUS-2210 Somaraju
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
Create  PROCEDURE [dbo].[GetMicrosoftOffice365HourlyVals] 
 @dtfrom datetime,@DeviceName varchar(150),@StatName varchar(150)
AS
BEGIN
Declare @dtto datetime
--set @dtto=DATEADD(hour,-24,@dtfrom)
set @dtto=@dtfrom
set @dtfrom = DATEADD(hour,-24,@dtfrom)
 
if(@DeviceName='All')
begin
     Select distinct isnull(Max(statvalue),0) as maxval,CONVERT(datetime, CAST(CONVERT(DATE, [Date]) as varchar) + ' ' +CAST(DATEPART(hour, DATEADD(hour,1,[Date])) as varchar(2)) + ':00') as dtfrom from MicrosoftDailyStats 
    where ServerName in (select name from [vitalsigns].[dbo].O365Server where Mode='Dir Sync'  )and statname in('DirSyncActual@'+@StatName,'DirSyncEstimated@'+@StatName) and ServerTypeId='21' --and statname=@StatName
    and [Date] >= @dtfrom
    and [Date] < @dtto
    group by CONVERT(datetime, CAST(CONVERT(DATE, [Date]) as varchar) + ' ' +CAST(DATEPART(hour, DATEADD(hour,1,[Date])) as varchar(2)) + ':00')
end
else
   begin
   
    Select distinct isnull(Max(statvalue),0) as maxval,CONVERT(datetime, CAST(CONVERT(DATE, [Date]) as varchar) + ' ' +CAST(DATEPART(hour, DATEADD(hour,1,[Date])) as varchar(2)) + ':00') as dtfrom from MicrosoftDailyStats 
    where ServerName=@DeviceName and statname in('DirSyncActual@'+@StatName,'DirSyncEstimated@'+@StatName)and ServerTypeId='21'
    and [Date] >= @dtfrom
    and [Date] < @dtto
    group by CONVERT(datetime, CAST(CONVERT(DATE, [Date]) as varchar) + ' ' +CAST(DATEPART(hour, DATEADD(hour,1,[Date])) as varchar(2)) + ':00')
    
    
   end
End
go

/* WS Added for Connections */

USE [VSS_Statistics]
GO

/****** Object:  Table [dbo].[IbmConnectionsDailyStats]    Script Date: 03/14/2016 13:41:42 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[IbmConnectionsDailyStats](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[ServerName] [nvarchar](50) NULL,
	[Date] [datetime] NULL,
	[StatName] [nvarchar](200) NULL,
	[StatValue] [float] NULL,
	[WeekNumber] [int] NULL,
	[MonthNumber] [int] NULL,
	[YearNumber] [int] NULL,
	[DayNumber] [int] NULL,
	[HourNumber] [int] NULL,
 CONSTRAINT [IbmConnectionsDailyStats$PrimaryKey] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO


USE [VSS_Statistics]
GO

/****** Object:  Table [dbo].[IbmConnectionsSummaryStats]    Script Date: 03/14/2016 13:44:26 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[IbmConnectionsSummaryStats](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[ServerName] [nvarchar](50) NULL,
	[Date] [datetime] NULL,
	[StatName] [nvarchar](50) NULL,
	[StatValue] [float] NULL,
	[WeekNumber] [int] NULL,
	[MonthNumber] [int] NULL,
	[YearNumber] [int] NULL,
	[DayNumber] [int] NULL,
 CONSTRAINT [IbmConnectionsSummaryStats$PrimaryKey] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO


USE [VSS_Statistics]
GO

/****** Object:  Table [dbo].[IbmConnectionsTopStats]    Script Date: 03/14/2016 13:44:35 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[IbmConnectionsTopStats](
	[ServerId] [int] NULL,
	[ServerName] [nvarchar](50) NULL,
	[Ranking] [int] NULL,
	[Name] [nvarchar](200) NULL,
	[UsageCount] [float] NULL,
	[Type] [nvarchar](200) NULL,
	[DateTime] [datetime] NULL
) ON [PRIMARY]

GO

--27/4/2016 Durga added for VSPLUS-2806
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


Create view [dbo].[GetHourlydataforEXJournalDocCountTotal]
AS
    
      select datepart(hour, date) as Hour,  avg(StatValue) as StatValue,ServerName from [DominoDailyStats]  WHERE StatName='EXJournal.DocCount.Total'

       and CAST(date AS DATE) = CAST(Getdate() AS DATE) group by  datepart(hour, date),ServerName

GO
