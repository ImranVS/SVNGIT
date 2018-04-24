
/*************************Start of ProfilesMaster Added data for URL, Sametime, etc., ********************************************/
GO
USE [vitalsigns]
GO

Delete from [dbo].[ProfilesMaster] 
GO

/****** Object:  Table [dbo].[ProfilesMaster]    Script Date: 12/10/2013 16:37:00 ******/
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
SET IDENTITY_INSERT [dbo].[ProfilesMaster] OFF

/*************************End  of ProfilesMaster Added data for URL, Sametime, etc., ********************************************/

/************************* Start of ScanResults Added for DBHealth Server for multithreaded *************************************/
/* 12/17/2013 NS commented out - the table is getting created in 1.1 */
/*
USE [VSS_Statistics]
GO

CREATE TABLE [dbo].[ScanResults](
[ID] [int] IDENTITY(1,1) NOT NULL,
[ScanDate] [datetime] NULL,
[ServerName] [nvarchar](250) NULL,
[ScanCount] [int] NULL,
[DatabaseCount] [int] NULL)

Alter TABLE [dbo].[Daily] Add Temp Bit NULL
*/
/************************* End of ScanResults Added for DBHealth Server for multithreaded *************************************/


/*********************************************Start  of Daily Task Cleanup ********************************************/

USE [VSS_Statistics]
GO

CREATE TABLE [dbo].[ConsolidationResults](
[ID] [int] IDENTITY(1,1) NOT NULL,
[ScanDate] [datetime] NULL,
[Result] [nvarchar](250) NULL)

/* 12/16/2013 NS added GO statement after each command below, otherwise, the server complains */
Alter Table DeviceDailyStats Add ServerName varchar(250)
GO
Update DeviceDailyStats Set ServerName=DeviceName
GO
Alter Table DeviceDailyStats Drop Column DeviceName
GO

/*************************Start of GetDeviceHourlyVals Change of DeviceName to ServerName ********************************************/

USE [VSS_Statistics]
GO
/****** Object:  StoredProcedure [dbo].[GetDeviceHourlyVals]    Script Date: 12/12/2013 11:00:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[GetDeviceHourlyVals] 
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
	Create Table #table(maxval int,dtfrom datetime)

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
				[VSS_Statistics].[dbo].[DominoDailyStats]  where  [ServerName]=@DeviceName
				and StatName=@StatName and
				Date  between @dtfrom and @dtto
			end

		SET @Flag = @Flag + 1
	END

	select * from #table
END
GO
/*************************End of GetDeviceHourlyVals Change of DeviceName to ServerName ********************************************/

USE [vitalsigns]
GO

/* Delete the data for BlackBerryProbeStats and NotesMailStats as we are not going to use this table anymore... */

delete from dailytasks where SourceTableName in ('BlackBerryProbeStats', 'NotesMailStats')
delete from dailytasks where SourceTableName = 'DeviceDailyStats' and SourceStatName = 'DeviceType'

/* Daily tasks : Delete Duplicate Records and add the new ones */ 
/* 12/17/2013 NS modified the query below - removed * from delete query */
delete from DailyTasks where SourceStatName in ('Places','Mem.PercentUsed')
SET IDENTITY_INSERT [dbo].[DailyTasks] ON
INSERT [dbo].[DailyTasks] ([ID], [SourceTableName], [SourceAggregation], [SourceStatName], [DestinationTableName], [DestinationStatName]) VALUES (178, N'SametimeDailyStats', N'AVG', N'Places', N'SametimeSummaryStats', N'Places')
INSERT [dbo].[DailyTasks] ([ID], [SourceTableName], [SourceAggregation], [SourceStatName], [DestinationTableName], [DestinationStatName]) VALUES (31, N'DominoDailyStats', N'AVG', N'Mem.PercentUsed', N'DominoSummaryStats', N'Mem.PercentUsed')
SET IDENTITY_INSERT [dbo].[DailyTasks] OFF
GO
/*********************************************End of Daily Task Cleanup ********************************************/

