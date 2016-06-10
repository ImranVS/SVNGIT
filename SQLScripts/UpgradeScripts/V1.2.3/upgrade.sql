/************** PLACE HOLDER TO PUT UPGRADE SCRIPTS IF ANY FOR V 1.2.3 ************/ 

/*Alan to modify Database Health columns for Crawford  */
Use VSS_Statistics
go
Alter Table Daily Add FolderCount INT NULL

/*Mukund: used for new report "Monitored Servers - Total licences used report (Configurator)"*/
USE [vitalsigns]
GO
INSERT INTO [vitalsigns].[dbo].[ReportItems] ([Name],[Category],[Description],[PageURL],[ImageURL],[ConfiguratorOnly],[isworking],[MaySchedule]) VALUES('Monitored Servers - Total licences used','Devices','Monitored Servers - Total licences used report (Configurator) ','../ConfiguratorReports/ServerMonitoringRpt.aspx',NULL,1,'True',NULL)

/*Mukund: used for RPR's internal pages access.*/
INSERT INTO [vitalsigns].[dbo].[Settings] ([sname] ,[svalue] ,[stype]) VALUES ('RPR Access Pwd' ,'Admin!23' ,'System.String')

CREATE TABLE [dbo].[RPRAccessPages](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](250) NULL,
	[Category] [nvarchar](250) NULL,
	[Description] [nvarchar](250) NULL,
	[PageURL] [nvarchar](500) NULL,
	[ImageURL] [nvarchar](250) NULL)
	
/*=======================================.*/
/* 1/30/2014 NS added for VSPLUS-322 - Sametime Servlet Port */
INSERT INTO [vitalsigns].[dbo].[Settings] ([sname], [svalue], [stype]) VALUES (N'Sametime Servlet Port', N'8088', N'System.String')

/*Mukund: 03 Feb 14 added for VSPLUS-197 - Exchange Service UI.*/

use vitalsigns
go
delete MenuItems where DisplayText='Microsoft Exchange'
go  
insert into MenuItems(Id,DisplayText ,OrderNum ,ParentID ,PageLink ,Level ,RefName ,ImageURL) values (62,'Microsoft Exchange',	2,	2,	'/Configurator/MSServersGrid.aspx',	2,	'MicrosoftExchangeServer',	'~/images/icons/exchange.jpg')
delete MenuItems where DisplayText='Exchange Server Credentials'
go  
insert into MenuItems(id,DisplayText ,OrderNum ,ParentID ,PageLink ,Level ,RefName ,ImageURL) values (63,'Exchange Server Credentials',	8,	10,	'/Configurator/PwrshellCredentials.aspx',	2,	'ExchangeServerCredentials',	'~/images/icons/exchange.jpg')
go
USE [VSS_Statistics]
GO

create PROCEDURE [dbo].[GetExchangeHourlyVals] 
	@dtfrom datetime,@DeviceName varchar(150),@StatName varchar(150)
AS
BEGIN

declare @dtto datetime
	DECLARE @Flag INT
	IF OBJECT_ID('tempdb..#table') IS NOT NULL  
	begin
		Drop table #table
	end
	Create Table #table(maxval int,dtfrom datetime)

	SET @Flag = 0
	WHILE (@Flag < 24)
	BEGIN
		set @dtto=@dtfrom
		set @dtfrom=DATEADD(HH,-1,@dtto) 

		
		
				insert into #table
				select isnull(Max(statvalue),0) as MaxVal,isnull(max(date),@dtto) as date from 
				[VSS_Statistics].[dbo].[ExchangeDailyStats]  where  [ServerName]=@DeviceName
				and StatName=@StatName and
				Date  between @dtfrom and @dtto
		
		SET @Flag = @Flag + 1
	END

	select * from #table
END

GO
use vitalsigns
go
CREATE TABLE [dbo].[ExchangeServerHealth](
	[ServerName] [nvarchar](50) NULL,
	[DNS] [nvarchar](50) NULL,
	[Ping] [nvarchar](50) NULL,
	[Uptime] [nvarchar](50) NULL,
	[Version] [nvarchar](50) NULL,
	[Roles] [nvarchar](50) NULL,
	[MailboxServerRoleServices] [nvarchar](50) NULL,
	[ClientAccessServerRoleServices] [nvarchar](50) NULL,
	[HubTransportServerRoleServices] [nvarchar](50) NULL,
	[QueueLength] [nvarchar](50) NULL,
	[TransportQueue] [nvarchar](50) NULL,
	[PFDBsMounted] [nvarchar](50) NULL,
	[MAPITest] [nvarchar](50) NULL,
	[MailFlowTest] [nvarchar](50) NULL,
	[UMSRoleServices] [nvarchar](50) NULL
) ON [PRIMARY]

GO
CREATE TABLE [dbo].[ExchangeMailBoxReport](
	[ServerName] [nvarchar](50) NULL,
	[DisplayName] [nvarchar](50) NULL,
	[LastMailboxLogon] [nvarchar](50) NULL,
	[LastLogonBy] [nvarchar](50) NULL,
	[ItemSize] [nvarchar](50) NULL,
	[DeletedItemSize] [nvarchar](50) NULL,
	[Items] [nvarchar](50) NULL,
	[Type] [nvarchar](50) NULL,
	[DatabaseName] [nvarchar](50) NULL
) ON [PRIMARY]

GO

CREATE TABLE [dbo].[WindowsServices](
	[Service_Name] [varchar](500) NULL,
	[Status] [varchar](50) NULL,
	[ServerName] [varchar](150) NULL,
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[StartMode] [varchar](50) NULL,
	[DisplayName] [varchar](max) NULL,
	[Monitored] [bit] NULL,
 CONSTRAINT [PK_win_Services] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO


/*=======================================.*/

IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[MonitoringTables]'))
DROP VIEW [dbo].[MonitoringTables]
GO

CREATE VIEW [dbo].[MonitoringTables] (
	CREATOR,
	NAME,
	COLCOUNT,
	TYPE
)
AS SELECT
	CONVERT( VARCHAR,USER_NAME( A.uid ) ),
	CONVERT( VARCHAR, A.name ),
	( SELECT COUNT( * ) FROM dbo.syscolumns B WHERE B.id = A.id ), 'T' FROM dbo.sysobjects A WHERE A.type = 'U' and A.NAME in
	('Servers','DominoServers','URLs','SametimeServers'
	)
	AND (patindex('%' + CHAR(32) +  '%',A.NAME) + patindex('% %',A.NAME)) = 0 AND LEN(A.NAME) <= 30
	AND PATINDEX('% %', A.NAME) = 0 AND PATINDEX('%-%', A.NAME) = 0
	UNION SELECT CONVERT( VARCHAR, USER_NAME( A.uid ) ), CONVERT( VARCHAR, A.name ),
	( SELECT COUNT( * ) FROM dbo.syscolumns B WHERE B.id = A.id ), 'V' FROM dbo.sysobjects A WHERE A.type = 'V'


GO


IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ConfigurationsChanged]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[ConfigurationsChanged]
GO


CREATE PROCEDURE [dbo].[ConfigurationsChanged](
@sTableName varchar(30)
)
as
begin
if EXISTS(SELECT * FROM dbo.Settings  WHERE sname='ConfigurationsChanged')
	UPDATE dbo.Settings SET svalue ='1' WHERE sname='ConfigurationsChanged'
ELSE
	INSERT INTO Settings(sname,svalue,stype) VALUES('ConfigurationsChanged','1','System.Int32')
end

GO


IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[AddAuditTrailTrigger]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[AddAuditTrailTrigger]
GO


CREATE PROCEDURE [dbo].[AddAuditTrailTrigger](
	@sTableName 	VARCHAR (250)
)  
AS BEGIN

   DECLARE @sLastCommand VARCHAR (8000)

  -- drop an exiting audit trail trigger
  Set @sLastCommand =
  'IF EXISTS (SELECT name FROM dbo.sysobjects 
  WHERE name = ''tr_Audit' + @sTableName + ''' AND type = ''TR'')		
  DROP TRIGGER dbo.tr_Audit' + @sTableName 
  EXEC (@sLastCommand)

  -- create the new one 
  Set @sLastCommand = 
  'CREATE TRIGGER dbo.tr_Audit' + @sTableName + ' ON ' + @sTableName + ' FOR INSERT,UPDATE,DELETE AS 
  DECLARE @sTbname VARCHAR(20) BEGIN

  SET @sTbname = ''' + @sTableName + ''''
  

  SET @sLastCommand = @sLastCommand +	
  '
  EXEC dbo.ConfigurationsChanged @sTbname
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
  DECLARE VSTableNames CURSOR FOR SELECT UPPER(NAME) FROM dbo.MonitoringTables 
	WHERE UPPER(TYPE) = 'T' AND NOT UPPER(NAME) LIKE '%SYS%' 
	ORDER BY NAME

  Open VSTableNames
  FETCH NEXT FROM VSTableNames into @sTableName

  WHILE @@FETCH_STATUS = 0 
	BEGIN
    -- drop an exiting audit trail trigger
    print 'Drop Trigger on: ' + @sTableName
    Set @sLastCommand =
    'IF EXISTS (SELECT name FROM dbo.sysobjects 
    WHERE name = ''tr_Audit' + @sTableName + ''' AND type = ''TR'')		
    DROP TRIGGER dbo.tr_Audit' + @sTableName 
    EXEC (@sLastCommand)
   
        print 'CREATE TRIGGER on: ' + @sTableName
        SET @sLastCommand =
		'AddAuditTrailTrigger ''' + @sTableName + '''' 
        EXEC( @sLastCommand )
      
    FETCH NEXT FROM VSTableNames into @sTableName
  END
  CLOSE VSTableNames
  DEALLOCATE VSTableNames
END

GO

IF exists (select * from dbo.sysobjects where id = object_id('dbo.AddAllAuditTrailTriggers'))
	BEGIN
	-- exec stored proc on every run to create ALL audit trail triggers of new tables 
	EXECUTE dbo.AddAllAuditTrailTriggers 
	END
go

-- Creation of ServerNodes table
USE [vitalsigns]
GO
CREATE TABLE [dbo].[ServerNodes](
	[NodeID] [int] NOT NULL,
	[NodeHostName] [varchar](50) NOT NULL,
	[NodeIPAddress] [varchar](150) NOT NULL,
	[NodeDescription] [varchar](50) NOT NULL,
	PRIMARY KEY(NodeID));
GO

USE [vitalsigns]
GO
--Insert MoniteredBy in DominoServers	
ALTER TABLE DominoServers
ADD [MonitoredBy] int NULL
GO

USE [vitalsigns]
GO
ALTER TABLE DominoServers
ADD FOREIGN KEY ([MonitoredBy])
REFERENCES ServerNodes (NodeID)
GO

USE [vitalsigns]
GO
--Insert MoniteredBy in SametimeServers	
ALTER TABLE SametimeServers
ADD [MonitoredBy] int NULL
GO

USE [vitalsigns]
GO
ALTER TABLE SametimeServers
ADD FOREIGN KEY ([MonitoredBy])
REFERENCES ServerNodes (NodeID)
GO

--Add Menu Item High Availability

USE [vitalsigns]
GO
INSERT INTO MenuItems VALUES(64,'High Availability',8,10,'/Security/HighAvailability.aspx',2,'HighAvailability','')
GO

--Server Settings editor
DELETE FROM [ProfilesMaster]
USE [vitalsigns]
GO
SET IDENTITY_INSERT [dbo].[ProfilesMaster] ON
INSERT [dbo].[ProfilesMaster] ([ID], [ServerTypeId], [AttributeName], [DefaultValue], [UnitOfMeasurement], [RelatedTable], [RelatedField]) VALUES (7, 1, N'Off Hours Scan Interval', N'10', N'minutes', N'DominoServers', N'OffHoursScanInterval')
INSERT [dbo].[ProfilesMaster] ([ID], [ServerTypeId], [AttributeName], [DefaultValue], [UnitOfMeasurement], [RelatedTable], [RelatedField]) VALUES (8, 1, N'Scan Interval', N'8', N'Minutes', N'DominoServers', N'Scan Interval')
INSERT [dbo].[ProfilesMaster] ([ID], [ServerTypeId], [AttributeName], [DefaultValue], [UnitOfMeasurement], [RelatedTable], [RelatedField]) VALUES (9, 1, N'Response Time Threshold', N'2000', N'milliseconds', N'DominoServers', N'ResponseThreshold')
INSERT [dbo].[ProfilesMaster] ([ID], [ServerTypeId], [AttributeName], [DefaultValue], [UnitOfMeasurement], [RelatedTable], [RelatedField]) VALUES (11, 1, N'Pending Mail Threshold', N'100', N'Count', N'DominoServers', N'PendingThreshold')
INSERT [dbo].[ProfilesMaster] ([ID], [ServerTypeId], [AttributeName], [DefaultValue], [UnitOfMeasurement], [RelatedTable], [RelatedField]) VALUES (12, 1, N'Dead Mail Threshold', N'100', N'Count', N'DominoServers', N'DeadThreshold')
INSERT [dbo].[ProfilesMaster] ([ID], [ServerTypeId], [AttributeName], [DefaultValue], [UnitOfMeasurement], [RelatedTable], [RelatedField]) VALUES (13, 1, N'Enable for Scan', N'0', N'True/False (1- True, 0-False)', N'DominoServers', N'Enabled')
INSERT [dbo].[ProfilesMaster] ([ID], [ServerTypeId], [AttributeName], [DefaultValue], [UnitOfMeasurement], [RelatedTable], [RelatedField]) VALUES (14, 1, N'Retry Interval', N'2', N'Minutes', N'DominoServers', N'RetryInterval')
INSERT [dbo].[ProfilesMaster] ([ID], [ServerTypeId], [AttributeName], [DefaultValue], [UnitOfMeasurement], [RelatedTable], [RelatedField]) VALUES (15, 1, N'Failure Threshold', N'2', N'No of failures', N'DominoServers', N'FailureThreshold')
INSERT [dbo].[ProfilesMaster] ([ID], [ServerTypeId], [AttributeName], [DefaultValue], [UnitOfMeasurement], [RelatedTable], [RelatedField]) VALUES (16, 1, N'Disk Space Threshold', N'0.10', N'Percentage free (eg:- for 10% = 0.10)', N'DominoServers', N'DiskSpaceThreshold')
INSERT [dbo].[ProfilesMaster] ([ID], [ServerTypeId], [AttributeName], [DefaultValue], [UnitOfMeasurement], [RelatedTable], [RelatedField]) VALUES (17, 1, N'Dead Mail Delete Threshold', N'100', N'Count', N'DominoServers', N'DeadMailDeleteThreshold')
INSERT [dbo].[ProfilesMaster] ([ID], [ServerTypeId], [AttributeName], [DefaultValue], [UnitOfMeasurement], [RelatedTable], [RelatedField]) VALUES (18, 1, N'Held Mail Threshold', N'100', N'Count', N'DominoServers', N'HeldThreshold')
INSERT [dbo].[ProfilesMaster] ([ID], [ServerTypeId], [AttributeName], [DefaultValue], [UnitOfMeasurement], [RelatedTable], [RelatedField]) VALUES (19, 1, N'Memory Threshold', N'20', N'Percentage', N'DominoServers', N'Memory_Threshold')
INSERT [dbo].[ProfilesMaster] ([ID], [ServerTypeId], [AttributeName], [DefaultValue], [UnitOfMeasurement], [RelatedTable], [RelatedField]) VALUES (20, 1, N'CPU Threshold', N'40', N'Percentage', N'DominoServers', N'CPU_Threshold')
INSERT [dbo].[ProfilesMaster] ([ID], [ServerTypeId], [AttributeName], [DefaultValue], [UnitOfMeasurement], [RelatedTable], [RelatedField]) VALUES (21, 1, N'Cluster Replication Delays Threshold', N'10', N'Minutes', N'DominoServers', N'Cluster_Rep_Delays_Threshold')
INSERT [dbo].[ProfilesMaster] ([ID], [ServerTypeId], [AttributeName], [DefaultValue], [UnitOfMeasurement], [RelatedTable], [RelatedField]) VALUES (22, 3, N'Enabled', N'0', N'True/False (1- True, 0-False)', N'SametimeServers', N'Enabled')
INSERT [dbo].[ProfilesMaster] ([ID], [ServerTypeId], [AttributeName], [DefaultValue], [UnitOfMeasurement], [RelatedTable], [RelatedField]) VALUES (23, 3, N'Scan Interval', N'8', N'Minutes', N'SametimeServers', N'ScanInterval')
INSERT [dbo].[ProfilesMaster] ([ID], [ServerTypeId], [AttributeName], [DefaultValue], [UnitOfMeasurement], [RelatedTable], [RelatedField]) VALUES (24, 3, N'Off Hours Scan Interval', N'15', N'minutes', N'SametimeServers', N'OffHoursScanInterval')
INSERT [dbo].[ProfilesMaster] ([ID], [ServerTypeId], [AttributeName], [DefaultValue], [UnitOfMeasurement], [RelatedTable], [RelatedField]) VALUES (25, 3, N'Retry Interval', N'2', N'minutes', N'SametimeServers', N'RetryInterval')
INSERT [dbo].[ProfilesMaster] ([ID], [ServerTypeId], [AttributeName], [DefaultValue], [UnitOfMeasurement], [RelatedTable], [RelatedField]) VALUES (26, 3, N'Response Threshold', N'2000', N'milliseconds', N'SametimeServers', N'ResponseThreshold')
INSERT [dbo].[ProfilesMaster] ([ID], [ServerTypeId], [AttributeName], [DefaultValue], [UnitOfMeasurement], [RelatedTable], [RelatedField]) VALUES (27, 7, N'Scan Interval', N'8', N'minutes', N'URLs', N'ScanInterval')
INSERT [dbo].[ProfilesMaster] ([ID], [ServerTypeId], [AttributeName], [DefaultValue], [UnitOfMeasurement], [RelatedTable], [RelatedField]) VALUES (28, 7, N'Off Hours Scan Interval', N'15', N'minutes', N'URLs', N'OffHoursScanInterval')
INSERT [dbo].[ProfilesMaster] ([ID], [ServerTypeId], [AttributeName], [DefaultValue], [UnitOfMeasurement], [RelatedTable], [RelatedField]) VALUES (29, 7, N'Enabled', N'0', N'True/False (1- True, 0-False)', N'URLs', N'Enabled')
INSERT [dbo].[ProfilesMaster] ([ID], [ServerTypeId], [AttributeName], [DefaultValue], [UnitOfMeasurement], [RelatedTable], [RelatedField]) VALUES (30, 7, N'Response Threshold', N'2000', N'milliseconds', N'URLs', N'ResponseThreshold')
INSERT [dbo].[ProfilesMaster] ([ID], [ServerTypeId], [AttributeName], [DefaultValue], [UnitOfMeasurement], [RelatedTable], [RelatedField]) VALUES (31, 7, N'Retry Interval', N'2', N'minutes', N'URLs', N'RetryInterval')
INSERT [dbo].[ProfilesMaster] ([ID], [ServerTypeId], [AttributeName], [DefaultValue], [UnitOfMeasurement], [RelatedTable], [RelatedField]) VALUES (32, 1, N'Category', N'Production', N'Category Type', N'DominoServers', N'Category')
INSERT [dbo].[ProfilesMaster] ([ID], [ServerTypeId], [AttributeName], [DefaultValue], [UnitOfMeasurement], [RelatedTable], [RelatedField]) VALUES (33, 1, N'BES Server', N'0', N'True/False (1- True, 0-False)', N'DominoServers', N'BES_Server')
INSERT [dbo].[ProfilesMaster] ([ID], [ServerTypeId], [AttributeName], [DefaultValue], [UnitOfMeasurement], [RelatedTable], [RelatedField]) VALUES (34, 1, N'BES Threshold', N'0', N'Count', N'DominoServers', N'BES_Threshold')
INSERT [dbo].[ProfilesMaster] ([ID], [ServerTypeId], [AttributeName], [DefaultValue], [UnitOfMeasurement], [RelatedTable], [RelatedField]) VALUES (35, 1, N'Advanced Mail Scan', N'0', N'True/False (1- True, 0-False)', N'DominoServers', N'AdvancedMailScan')
INSERT [dbo].[ProfilesMaster] ([ID], [ServerTypeId], [AttributeName], [DefaultValue], [UnitOfMeasurement], [RelatedTable], [RelatedField]) VALUES (36, 1, N'Scan DB Health', N'1', N'True/False (1- True, 0-False)', N'DominoServers', N'ScanDBHealth')
INSERT [dbo].[ProfilesMaster] ([ID], [ServerTypeId], [AttributeName], [DefaultValue], [UnitOfMeasurement], [RelatedTable], [RelatedField]) VALUES (37, 1, N'Server Days Alert', N'0', N'Number of Consecutive Days', N'DominoServers', N'ServerDaysAlert')
INSERT [dbo].[ProfilesMaster] ([ID], [ServerTypeId], [AttributeName], [DefaultValue], [UnitOfMeasurement], [RelatedTable], [RelatedField]) VALUES (38, 3, N'Category', N'Production', N'Category Type', N'SametimeServers', N'Category')
INSERT [dbo].[ProfilesMaster] ([ID], [ServerTypeId], [AttributeName], [DefaultValue], [UnitOfMeasurement], [RelatedTable], [RelatedField]) VALUES (39, 3, N'User Threshold', N'2', N'Count', N'SametimeServers', N'UserThreshold')
INSERT [dbo].[ProfilesMaster] ([ID], [ServerTypeId], [AttributeName], [DefaultValue], [UnitOfMeasurement], [RelatedTable], [RelatedField]) VALUES (40, 3, N'SSL', N'0', N'True/False (1- True, 0-False)', N'SametimeServers', N'SSL')
INSERT [dbo].[ProfilesMaster] ([ID], [ServerTypeId], [AttributeName], [DefaultValue], [UnitOfMeasurement], [RelatedTable], [RelatedField]) VALUES (41, 3, N'Chat Threshold', N'0', N'Count', N'SametimeServers', N'ChatThreshold')
INSERT [dbo].[ProfilesMaster] ([ID], [ServerTypeId], [AttributeName], [DefaultValue], [UnitOfMeasurement], [RelatedTable], [RelatedField]) VALUES (42, 3, N'NChat Threshold', N'0', N'Count', N'SametimeServers', N'NChatThreshold')
INSERT [dbo].[ProfilesMaster] ([ID], [ServerTypeId], [AttributeName], [DefaultValue], [UnitOfMeasurement], [RelatedTable], [RelatedField]) VALUES (43, 3, N'Places Threshold', N'0', N'Count', N'SametimeServers', N'PlacesThreshold')
SET IDENTITY_INSERT [dbo].[ProfilesMaster] OFF	
GO																																			
/*Mukund: 03 Feb 14 added for VSPLUS-197 - Exchange Service UI.*/

USE [vitalsigns]
GO


CREATE TABLE [dbo].[ExchangeDAGHealthCopyStatus](
	[ServerName] [nvarchar](50) NULL,
	[InfoDate] [datetime] NULL,
	[DatabaseName] [nchar](10) NULL,
	[ReplayQueueLength] [nchar](10) NULL,
	[DatabaseCopy] [nvarchar](100) NULL,
	[ActiveCopy] [nvarchar](100) NULL,
	[MailboxServer] [nvarchar](100) NULL,
	[Status] [nvarchar](100) NULL,
	[ContentIndex] [nvarchar](100) NULL,
	[CopyQueueLength] [nvarchar](100) NULL,
	[ActivationPreference] [nvarchar](100) NULL,
	[ReplayLagged] [nvarchar](100) NULL,
	[TruncationLagged] [nvarchar](100) NULL
) ON [PRIMARY]

GO


CREATE TABLE [dbo].[ExchangeDAGHealthCopySummary](
	[ServerName] [nvarchar](50) NULL,
	[InfoDate] [datetime] NULL,
	[Databasename] [nvarchar](50) NULL,
	[Mountedon] [nvarchar](50) NULL,
	[Preference] [nvarchar](50) NULL,
	[TotalCopies] [nvarchar](50) NULL,
	[HealthyCopies] [nvarchar](50) NULL,
	[UnhealthyCopies] [nvarchar](50) NULL,
	[HealthyQueues] [nvarchar](50) NULL,
	[UnhealthyQueues] [nvarchar](50) NULL,
	[LaggedQueues] [nvarchar](50) NULL,
	[HealthyIndexes] [nvarchar](50) NULL,
	[UnhealthyIndexes] [nvarchar](50) NULL
) ON [PRIMARY]

GO

CREATE TABLE [dbo].[ExchangeDAGHealthMemberReport](
	[ServerName] [nvarchar](50) NULL,
	[InfoDate] [datetime] NULL,
	[ClusterService] [nvarchar](50) NULL,
	[ReplayService] [nvarchar](50) NULL,
	[ActiveManager] [nvarchar](50) NULL,
	[TasksRpcListener] [nvarchar](50) NULL,
	[TcpListener] [nvarchar](50) NULL,
	[ServerLocatorService] [nvarchar](50) NULL,
	[DagMembersUp] [nvarchar](50) NULL,
	[ClusterNetwork] [nvarchar](50) NULL,
	[QuorumGroup] [nvarchar](50) NULL,
	[FileShareQuorum] [nvarchar](50) NULL,
	[DBCopySuspended] [nvarchar](50) NULL,
	[DBCopyFailed] [nvarchar](50) NULL,
	[DBInitializing] [nvarchar](50) NULL,
	[DBDisconnected] [nvarchar](50) NULL,
	[DBLogCopyKeepingUp] [nvarchar](50) NULL,
	[DBLogReplayKeepingUp] [nvarchar](50) NULL
) ON [PRIMARY]

GO

/*================================.*/
/* 2/5/2014 NS added for VSPLUS-187 */
USE [vitalsigns]
GO

/****** Object:  StoredProcedure [dbo].[GetAlertsWithAllEventsSelected]    Script Date: 02/05/2014 10:59:05 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Natallya Shkarayeva
-- Create date: 12/3/2013
-- Description:	
--
-- This stored procedure gets event/server information based on the user selections
-- on the Alert Definition page. The query returns selections that cover all event types 
-- for a specific category, i.e., 
-- all Domino events have been selected from the events grid and some servers have been selected
-- from the servers grid.
-- =============================================
ALTER PROCEDURE [dbo].[GetAlertsWithAllEventsSelected]
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    select distinct AlertKey,HoursIndicator,SendTo,CopyTo,BlindCopyTo,ISNULL(StartTime,'') StartTime,
    ISNULL(EndTime,'') EndTime,Day,Duration,SendSNMPTrap,AlertName,EventName,ServerType,ServerName from
	(
		select distinct t1.AlertKey AlertKey,t1.HoursIndicator HoursIndicator,t1.SendTo SendTo,t1.CopyTo CopyTo,
		t1.BlindCopyTo BlindCopyTo,t1.StartTime StartTime,t1.EndTime EndTime,t1.DAY Day,t1.Duration Duration,
		t1.SendSNMPTrap SendSNMPTrap,
		t2.AlertName AlertName,t8.EventName EventName,t3.ServerType ServerType,ISNULL(t5.ServerName,'') ServerName
		from AlertDetails t1 inner join AlertNames t2 on t1.AlertKey=t2.AlertKey
		inner join AlertEvents t7 on t1.AlertKey = t7.AlertKey 
		inner join EventsMaster t8 on t8.ServerTypeID = t7.ServerTypeID
		inner join ServerTypes t3 on t7.ServerTypeID = t3.ID
		inner join AlertServers t4 on t2.AlertKey=t4.AlertKey
		inner join Servers t5 on t5.ID=t4.ServerID and t8.ServerTypeID=t5.ServerTypeID
		inner join Locations t6 on t4.LocationID=t6.ID and t6.Location !='URL'
		where t7.EventID = 0
		union
		select distinct t1.AlertKey AlertKey,t1.HoursIndicator HoursIndicator,t1.SendTo SendTo,t1.CopyTo CopyTo,
		t1.BlindCopyTo BlindCopyTo,t1.StartTime StartTime,t1.EndTime EndTime,t1.DAY Day,t1.Duration Duration,
		t1.SendSNMPTrap SendSNMPTrap,
		t2.AlertName AlertName,t8.EventName EventName,t3.ServerType ServerType,ISNULL(t5.Name,'') ServerName
		from AlertDetails t1 inner join AlertNames t2 on t1.AlertKey=t2.AlertKey
		inner join AlertEvents t7 on t1.AlertKey = t7.AlertKey 
		inner join EventsMaster t8 on t8.ServerTypeID = t7.ServerTypeID
		inner join ServerTypes t3 on t7.ServerTypeID = t3.ID
		inner join AlertServers t4 on t2.AlertKey=t4.AlertKey
		inner join URLs t5 on t5.ID=t4.ServerID and t8.ServerTypeID=t5.ServerTypeID
		--1/31/2014 NS modified for testing
		--inner join Locations t6 on t4.LocationID=t6.ID and t6.Location ='URL'
		inner join Locations t6 on t4.LocationID=t6.ID and t5.LocationId=t6.ID
		where t7.EventID = 0
		union
		--2/6/2014 NS modified - need to account for ServerTypeID=0
		/*
		select t1.AlertKey AlertKey,t1.HoursIndicator HoursIndicator,t1.SendTo SendTo,t1.CopyTo CopyTo,
		t1.BlindCopyTo BlindCopyTo,t1.StartTime StartTime,t1.EndTime EndTime,t1.DAY Day,t1.Duration Duration,
		t1.SendSNMPTrap SendSNMPTrap,
		t2.AlertName AlertName,t4.EventName EventName,t5.ServerType ServerType,ISNULL(t6.ServerName,'') as ServerName
		from AlertDetails t1 inner join AlertNames t2 on t1.AlertKey=t2.AlertKey
		inner join AlertEvents t3 on t2.AlertKey=t3.AlertKey 
		inner join ServerTypes t5 on t3.ServerTypeID=t5.ID 
		inner join EventsMaster t4 on t4.ServerTypeID=t3.ServerTypeID
		left outer join Servers t6 on t6.ServerTypeID = t5.ID
		where t6.ServerName is null and t5.ServerType != 'URL' and t3.EventID=0
		*/
		select t1.AlertKey AlertKey,t1.HoursIndicator HoursIndicator,t1.SendTo SendTo,t1.CopyTo CopyTo,
		t1.BlindCopyTo BlindCopyTo,t1.StartTime StartTime,t1.EndTime EndTime,t1.DAY Day,t1.Duration Duration,
		t1.SendSNMPTrap SendSNMPTrap,
		t2.AlertName AlertName,t4.EventName EventName,t5.ServerType ServerType,ISNULL(t6.ServerName,'') as ServerName
		from AlertDetails t1 inner join AlertNames t2 on t1.AlertKey=t2.AlertKey
		inner join AlertEvents t3 on t2.AlertKey=t3.AlertKey,
		EventsMaster t4 inner join ServerTypes t5 on t4.ServerTypeID=t5.ID 		
		left outer join Servers t6 on t6.ServerTypeID = t5.ID
		where t6.ServerName is null and t5.ServerType != 'URL' and t3.EventID=0
	) as tmp
	order by AlertKey
END



GO

USE [vitalsigns]
GO
/****** Object:  StoredProcedure [dbo].[GetAlertsForAllServersByLocation]    Script Date: 02/05/2014 10:58:07 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
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
-- =============================================
ALTER PROCEDURE [dbo].[GetAlertsForAllServersByLocation]
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    select distinct AlertKey,HoursIndicator,SendTo,CopyTo,BlindCopyTo,ISNULL(StartTime,'') StartTime,
    ISNULL(EndTime,'') EndTime,Day,Duration,SendSNMPTrap,AlertName,EventName,ServerType,ServerName from
	(
		-- Select rows from the Servers table, do not include those with location URL
		select distinct t1.AlertKey AlertKey,t1.HoursIndicator HoursIndicator,t1.SendTo SendTo,t1.CopyTo CopyTo,
		t1.BlindCopyTo BlindCopyTo,t1.StartTime StartTime,t1.EndTime EndTime,t1.DAY Day,t1.Duration Duration,
		t1.SendSNMPTrap SendSNMPTrap,
		t2.AlertName AlertName,t8.EventName EventName,t3.ServerType ServerType,t5.ServerName ServerName
		from AlertDetails t1 inner join AlertNames t2 on t1.AlertKey=t2.AlertKey
		inner join AlertEvents t7 on t1.AlertKey = t7.AlertKey 
		inner join EventsMaster t8 on t8.ServerTypeID = t7.ServerTypeID and t8.ID=t7.EventID
		inner join ServerTypes t3 on t7.ServerTypeID = t3.ID
		inner join AlertServers t4 on t2.AlertKey=t4.AlertKey
		inner join Servers t5 on t8.ServerTypeID=t5.ServerTypeID and t5.LocationID=t4.LocationID
		inner join Locations t6 on t4.LocationID=t6.ID and t6.Location !='URL'
		where t4.ServerID=0
		union
		-- Select rows from the URL table, include those that have location URL
		select distinct t1.AlertKey AlertKey,t1.HoursIndicator HoursIndicator,t1.SendTo SendTo,t1.CopyTo CopyTo,
		t1.BlindCopyTo BlindCopyTo,t1.StartTime StartTime,t1.EndTime EndTime,t1.DAY Day,t1.Duration Duration,
		t1.SendSNMPTrap SendSNMPTrap,
		t2.AlertName AlertName,t8.EventName EventName,t3.ServerType ServerType,t5.Name ServerName
		from AlertDetails t1 inner join AlertNames t2 on t1.AlertKey=t2.AlertKey
		inner join AlertEvents t7 on t1.AlertKey = t7.AlertKey 
		inner join EventsMaster t8 on t8.ServerTypeID = t7.ServerTypeID and t8.ID=t7.EventID
		inner join ServerTypes t3 on t7.ServerTypeID = t3.ID
		inner join AlertServers t4 on t2.AlertKey=t4.AlertKey
		inner join URLs t5 on t8.ServerTypeID=t5.ServerTypeID
		--1/31/2014 NS modified for VSPLUS-187
		--inner join Locations t6 on t4.LocationID=t6.ID and t6.Location ='URL'
		inner join Locations t6 on t4.LocationID=t6.ID and t5.LocationId=t6.ID
		where t4.ServerID = 0
		union
		-- Select rows to cover cases when both Location and Event categories are selected as a whole,
		-- rows come from the Servers table
		--2/6/2014 NS modified - need to account for ServerTypeID=0
		/*
		select distinct t1.AlertKey AlertKey,t1.HoursIndicator HoursIndicator,t1.SendTo SendTo,t1.CopyTo CopyTo,
		t1.BlindCopyTo BlindCopyTo,t1.StartTime StartTime,t1.EndTime EndTime,t1.DAY Day,t1.Duration Duration,
		t1.SendSNMPTrap SendSNMPTrap,
		t2.AlertName AlertName,t8.EventName EventName,t3.ServerType ServerType,t5.ServerName ServerName
		from AlertDetails t1 inner join AlertNames t2 on t1.AlertKey=t2.AlertKey
		inner join AlertEvents t7 on t1.AlertKey = t7.AlertKey 
		inner join AlertServers t4 on t2.AlertKey=t4.AlertKey
		inner join EventsMaster t8 on t8.ServerTypeID = t7.ServerTypeID
		inner join ServerTypes t3 on t7.ServerTypeID = t3.ID
		inner join Servers t5 on t8.ServerTypeID=t5.ServerTypeID and t5.LocationID=t4.LocationID
		inner join Locations t6 on t4.LocationID=t6.ID 
		where t4.ServerID=0 and t7.EventID = 0
		*/
		select distinct t1.AlertKey AlertKey,t1.HoursIndicator HoursIndicator,t1.SendTo SendTo,t1.CopyTo CopyTo,
		t1.BlindCopyTo BlindCopyTo,t1.StartTime StartTime,t1.EndTime EndTime,t1.DAY Day,t1.Duration Duration,
		t1.SendSNMPTrap SendSNMPTrap,
		t2.AlertName AlertName,t8.EventName EventName,t3.ServerType ServerType,t5.ServerName ServerName
		from AlertDetails t1 inner join AlertNames t2 on t1.AlertKey=t2.AlertKey
		inner join AlertEvents t7 on t1.AlertKey = t7.AlertKey 
		inner join AlertServers t4 on t2.AlertKey=t4.AlertKey,
		EventsMaster t8	inner join ServerTypes t3 on t8.ServerTypeID = t3.ID
		inner join Servers t5 on t8.ServerTypeID=t5.ServerTypeID
		inner join Locations t6 on t5.LocationID=t6.ID 
		where t4.ServerID=0 and t7.EventID = 0 and t7.ServerTypeID=0
		union
		-- Select rows to cover cases when both Location and Event categories are selected as a whole,
		-- rows come from the URL table
		--2/6/2014 NS modified - need to account for ServerTypeID=0
		/*
		select distinct t1.AlertKey AlertKey,t1.HoursIndicator HoursIndicator,t1.SendTo SendTo,t1.CopyTo CopyTo,
		t1.BlindCopyTo BlindCopyTo,t1.StartTime StartTime,t1.EndTime EndTime,t1.DAY Day,t1.Duration Duration,
		t1.SendSNMPTrap SendSNMPTrap,
		t2.AlertName AlertName,t8.EventName EventName,t3.ServerType ServerType,t5.Name ServerName
		from AlertDetails t1 inner join AlertNames t2 on t1.AlertKey=t2.AlertKey
		inner join AlertEvents t7 on t1.AlertKey = t7.AlertKey 
		inner join AlertServers t4 on t2.AlertKey=t4.AlertKey
		inner join EventsMaster t8 on t8.ServerTypeID = t7.ServerTypeID
		inner join ServerTypes t3 on t7.ServerTypeID = t3.ID
		inner join URLs t5 on t8.ServerTypeID=t5.ServerTypeID and t5.LocationID=t4.LocationID
		inner join Locations t6 on t4.LocationID=t6.ID 
		where t4.ServerID=0 and t7.EventID = 0
		*/
		select distinct t1.AlertKey AlertKey,t1.HoursIndicator HoursIndicator,t1.SendTo SendTo,t1.CopyTo CopyTo,
		t1.BlindCopyTo BlindCopyTo,t1.StartTime StartTime,t1.EndTime EndTime,t1.DAY Day,t1.Duration Duration,
		t1.SendSNMPTrap SendSNMPTrap,
		t2.AlertName AlertName,t8.EventName EventName,t3.ServerType ServerType,t5.Name ServerName
		from AlertDetails t1 inner join AlertNames t2 on t1.AlertKey=t2.AlertKey
		inner join AlertEvents t7 on t1.AlertKey = t7.AlertKey 
		inner join AlertServers t4 on t2.AlertKey=t4.AlertKey,
		EventsMaster t8 inner join ServerTypes t3 on t8.ServerTypeID = t3.ID
		inner join URLs t5 on t8.ServerTypeID=t5.ServerTypeID
		inner join Locations t6 on t5.LocationID=t6.ID 
		where t4.ServerID=0 and t7.EventID = 0 and t7.ServerTypeID=0
	) as tmp
	order by AlertKey
END

GO
-- Author:		Chandrahas
-- Create date: 2/6/2014

ALTER TABLE Servers
ADD [MonitoredBy] int NULL
GO

USE [vitalsigns]
GO
ALTER TABLE Servers
ADD FOREIGN KEY ([MonitoredBy])
REFERENCES ServerNodes (NodeID)
GO

USE [vitalsigns]
GO
ALTER TABLE Urls
ADD [MonitoredBy] int NULL
GO

USE [vitalsigns]
GO
ALTER TABLE Urls
ADD FOREIGN KEY ([MonitoredBy])
REFERENCES ServerNodes (NodeID)
GO

USE [vitalsigns]
GO
ALTER TABLE ServerTypes
ADD ServerTypeTable nvarchar(50)
GO

USE [vitalsigns]
GO
UPDATE ServerTypes SET ServerTypeTable = 'DominoServers' WHERE ServerType = 'Domino'
UPDATE ServerTypes SET ServerTypeTable = '' WHERE ServerType = 'BES'
UPDATE ServerTypes SET ServerTypeTable = 'SametimeServers' WHERE ServerType = 'Sametime'
UPDATE ServerTypes SET ServerTypeTable = '' WHERE ServerType = 'SharePoint'
UPDATE ServerTypes SET ServerTypeTable = '' WHERE ServerType = 'Exchange'
UPDATE ServerTypes SET ServerTypeTable = 'MailServices' WHERE ServerType = 'Mail'
UPDATE ServerTypes SET ServerTypeTable = 'URLs' WHERE ServerType = 'URL'
UPDATE ServerTypes SET ServerTypeTable = 'Network Devices' WHERE ServerType = 'Network Device'
UPDATE ServerTypes SET ServerTypeTable = 'NotesDatabases' WHERE ServerType = 'Notes Database'
GO
-- =============================================
-- Author:		Chandrahas
-- Create date: 2/6/2014
-- Description:	
--
-- The stored procedure is altered to get the 
--MonitoredBy column required for HighAvailability.
--Also ensured it doesnot effect other screen where 
--the Serverstreelsit is used.
-- =============================================

USE [vitalsigns]
GO
ALTER procedure [dbo].[ServerLocations] as
Begin
declare @SrvLocations Table
(id int, LocId int, Name varchar(100),actid int,tbl varchar(50),srvtypeid int,ServerType varchar(100),description varchar(100),MonitoredBy varchar(100))

insert into @SrvLocations select id,null,Location,id,'Locations',null,null,null,null from Locations

Declare @count int
select @count=MAX(id) from Locations


Declare @ID int
Declare @LocationName varchar(100)
Declare @LocationID int
Declare @ServerTypeId int
Declare @ServerType varchar(100)
Declare @description varchar(100)
Declare @MonitoredBy varchar(100)

DECLARE db_cursor CURSOR FOR  
select ID,ServerName,LocationID,ServerTypeId,ServerType,X.description,NodeHostName as MonitoredBy from (select sr.ID,sr.ServerName,sr.LocationID,sr.ServerTypeId,srt.ServerType,sr.description,MonitoredBy from Servers sr,
ServerTypes srt where sr.ServerTypeId=srt.id ) as X LEFT JOIN ServerNodes sn on sn.NodeID = X.MonitoredBy
union
select ID,ServerName,LocationID,ServerTypeId,ServerType,X.TheURL,NodeHostName as MonitoredBy from (select sr.ID,sr.Name as ServerName,sr.LocationID,sr.ServerTypeId,srt.ServerType,sr.TheURL,MonitoredBy from URLs sr,
ServerTypes srt where sr.ServerTypeId=srt.id ) as X LEFT JOIN ServerNodes sn on sn.NodeID = X.MonitoredBy 
order by X.LocationID,X.ServerName

OPEN db_cursor   
FETCH NEXT FROM db_cursor into @ID,@LocationName,@LocationID,@ServerTypeId,@ServerType,@description,@MonitoredBy

WHILE @@FETCH_STATUS = 0   
BEGIN   
	Set @count=@count+1
	insert into @SrvLocations values(@count,@LocationID,@LocationName,@id,'Servers',@ServerTypeId,@ServerType,@description,@MonitoredBy)
	FETCH NEXT FROM db_cursor into @ID,@LocationName,@LocationID,@ServerTypeId,@ServerType,@description,@MonitoredBy
END   

CLOSE db_cursor   
DEALLOCATE db_cursor

select * from @SrvLocations-- order by LocId,Name
end
GO											

-- Updating the Log Level to 2 by default. Joe Thumma. 2/7/2014.
use vitalsigns
go
Delete from [dbo].[Settings] where sname = 'Log Level'

INSERT [dbo].[Settings] ([sname], [svalue], [stype]) VALUES (N'Log Level', N'2', N'System.Int32')