USE [vitalsigns]
GO
PRINT N' Executing V1.1 upgrade scripts ......... ';

USE [vitalsigns]
GO
EXEC sp_configure 'default language', 0 
GO
RECONFIGURE 
GO

USE [VSS_Statistics]
GO
EXEC sp_configure 'default language', 0 
RECONFIGURE 
GO

ALTER LOGIN VS WITH DEFAULT_LANGUAGE = English
GO

USE [vitalsigns]
GO

IF EXISTS(SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.fn_GetVSVersion') AND xtype IN (N'FN', N'IF', N'TF'))
	DROP FUNCTION dbo.fn_GetVSVersion
go

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

if NOT exists (select * from syscolumns where id = object_id('dbo.VS_MANAGEMENT'))
CREATE TABLE dbo.VS_MANAGEMENT(
	[CATEGORY]		[VARCHAR] (20)	NOT NULL ,
	[VALUE]			[VARCHAR] (20)	NULL ,
	[LAST_UPDATE]	[DATETIME]		NOT NULL ,
	[DESCRIPTION]	[VARCHAR] (254) NULL ,
	[UPDATE_BY]		[VARCHAR] (50)	NULL CONSTRAINT [DF_VS_MANAGEMENT_UPDATE_BY] DEFAULT (suser_sname())
)
go

USE [vitalsigns]
GO

IF NOT EXISTS (select * from syscolumns where id = object_id('dbo.DailyTasks'))
BEGIN	
	Create Table DailyTasks
	(ID INT NOT NULL IDENTITY(1,1),
	SourceTableName Varchar(150),
	SourceAggregation Varchar(150),
	SourceStatName Varchar(150),
	DestinationTableName Varchar(150),
	DestinationStatName Varchar(150),
	QueryType int)
END
GO

USE [vitalsigns]
GO
/****** Object:  Table [dbo].[DailyTasks]    Script Date: 12/05/2013 18:44:56 ******/
DELETE FROM [DailyTasks]
GO

/* 2/2/2015 NS modified for VSPLUS-1333 - commented out majority of Sametime stats */
/* 2/3/2015 NS modified for VSPLUS-1333 - changed the aggregation function to MAX for MaxConcurrentLoggedInUsers */
/* 1/24/2016 NS commented out for VSPLUS-1921,VSPLUS-1823
ALL Sametime stats - the stats are consolidated by the VitalSignsCore service */
SET IDENTITY_INSERT [dbo].[DailyTasks] ON
INSERT [dbo].[DailyTasks] ([ID], [SourceTableName], [SourceAggregation], [SourceStatName], [DestinationTableName], [DestinationStatName]) VALUES (9, N'DeviceDailyStats', N'AVG', N'DeviceType', N'DeviceDailyStats', N'DailyResponseAverage')
INSERT [dbo].[DailyTasks] ([ID], [SourceTableName], [SourceAggregation], [SourceStatName], [DestinationTableName], [DestinationStatName]) VALUES (11, N'DominoDailyStats', N'AVG', N'Mail.AverageServerHops', N'DominoSummaryStats', N'Mail.AverageServerHops')
INSERT [dbo].[DailyTasks] ([ID], [SourceTableName], [SourceAggregation], [SourceStatName], [DestinationTableName], [DestinationStatName]) VALUES (12, N'DominoDailyStats', N'AVG', N'Mail.AverageDeliverTime', N'DominoSummaryStats', N'Mail.AverageDeliverTime')
INSERT [dbo].[DailyTasks] ([ID], [SourceTableName], [SourceAggregation], [SourceStatName], [DestinationTableName], [DestinationStatName]) VALUES (13, N'DominoDailyStats', N'AVG', N'Mail.AverageSizeDelivered', N'DominoSummaryStats', N'Mail.AverageSizeDelivered')
INSERT [dbo].[DailyTasks] ([ID], [SourceTableName], [SourceAggregation], [SourceStatName], [DestinationTableName], [DestinationStatName]) VALUES (14, N'DominoDailyStats', N'SUM', N'Mail.Delivered', N'DominoSummaryStats', N'Mail.Delivered')
INSERT [dbo].[DailyTasks] ([ID], [SourceTableName], [SourceAggregation], [SourceStatName], [DestinationTableName], [DestinationStatName]) VALUES (15, N'DominoDailyStats', N'SUM', N'Mail.TotalRouted', N'DominoSummaryStats', N'Mail.TotalRouted')
INSERT [dbo].[DailyTasks] ([ID], [SourceTableName], [SourceAggregation], [SourceStatName], [DestinationTableName], [DestinationStatName]) VALUES (16, N'DominoDailyStats', N'SUM', N'Mail.Transferred', N'DominoSummaryStats', N'Mail.Transferred')
INSERT [dbo].[DailyTasks] ([ID], [SourceTableName], [SourceAggregation], [SourceStatName], [DestinationTableName], [DestinationStatName]) VALUES (17, N'DominoDailyStats', N'SUM', N'SMTP.MessagesProcessed', N'DominoSummaryStats', N'SMTP.MessagesProcessed')
INSERT [dbo].[DailyTasks] ([ID], [SourceTableName], [SourceAggregation], [SourceStatName], [DestinationTableName], [DestinationStatName]) VALUES (18, N'DominoDailyStats', N'SUM', N'Mail.TransferFailures', N'DominoSummaryStats', N'Mail.TransferFailures')
INSERT [dbo].[DailyTasks] ([ID], [SourceTableName], [SourceAggregation], [SourceStatName], [DestinationTableName], [DestinationStatName]) VALUES (19, N'DominoDailyStats', N'SUM', N'Mail.TotalPending', N'DominoSummaryStats', N'Mail.TotalPending')
INSERT [dbo].[DailyTasks] ([ID], [SourceTableName], [SourceAggregation], [SourceStatName], [DestinationTableName], [DestinationStatName]) VALUES (20, N'DominoDailyStats', N'AVG', N'Server.AvailabilityIndex', N'DominoSummaryStats', N'Server.AvailabilityIndex')
INSERT [dbo].[DailyTasks] ([ID], [SourceTableName], [SourceAggregation], [SourceStatName], [DestinationTableName], [DestinationStatName]) VALUES (21, N'DominoDailyStats', N'AVG', N'Server.ExpansionFactor', N'DominoSummaryStats', N'Server.ExpansionFactor')
INSERT [dbo].[DailyTasks] ([ID], [SourceTableName], [SourceAggregation], [SourceStatName], [DestinationTableName], [DestinationStatName]) VALUES (22, N'DominoDailyStats', N'AVG', N'Server.Users', N'DominoSummaryStats', N'Server.Users')
INSERT [dbo].[DailyTasks] ([ID], [SourceTableName], [SourceAggregation], [SourceStatName], [DestinationTableName], [DestinationStatName]) VALUES (23, N'DominoDailyStats', N'AVG', N'Server.Trans.PerMinute', N'DominoSummaryStats', N'Server.Trans.PerMinute')
INSERT [dbo].[DailyTasks] ([ID], [SourceTableName], [SourceAggregation], [SourceStatName], [DestinationTableName], [DestinationStatName]) VALUES (25, N'DominoDailyStats', N'AVG', N'Mem.PercentAvailable', N'DominoSummaryStats', N'Mem.PercentAvailable')
INSERT [dbo].[DailyTasks] ([ID], [SourceTableName], [SourceAggregation], [SourceStatName], [DestinationTableName], [DestinationStatName]) VALUES (26, N'DominoDailyStats', N'AVG', N'Mem.PercentUsed', N'DominoSummaryStats', N'Mem.PercentUsed')
INSERT [dbo].[DailyTasks] ([ID], [SourceTableName], [SourceAggregation], [SourceStatName], [DestinationTableName], [DestinationStatName]) VALUES (27, N'DominoDailyStats', N'AVG', N'Mem.Free', N'DominoSummaryStats', N'Mem.Free')
INSERT [dbo].[DailyTasks] ([ID], [SourceTableName], [SourceAggregation], [SourceStatName], [DestinationTableName], [DestinationStatName]) VALUES (28, N'DominoDailyStats', N'AVG', N'Replica.Failed', N'DominoSummaryStats', N'Replica.Failed')
INSERT [dbo].[DailyTasks] ([ID], [SourceTableName], [SourceAggregation], [SourceStatName], [DestinationTableName], [DestinationStatName]) VALUES (29, N'DominoDailyStats', N'AVG', N'Pending Mail', N'DominoSummaryStats', N'Pending Mail')
INSERT [dbo].[DailyTasks] ([ID], [SourceTableName], [SourceAggregation], [SourceStatName], [DestinationTableName], [DestinationStatName]) VALUES (30, N'DominoDailyStats', N'AVG', N'Dead Mail', N'DominoSummaryStats', N'Dead Mail')
/* Removing this as there as this is a duplicate record */
/* INSERT [dbo].[DailyTasks] ([ID], [SourceTableName], [SourceAggregation], [SourceStatName], [DestinationTableName], [DestinationStatName]) VALUES (31, N'DominoDailyStats', N'AVG', N'Mem.PercentUsed', N'DominoSummaryStats', N'Mem.PercentUsed') */ 

INSERT [dbo].[DailyTasks] ([ID], [SourceTableName], [SourceAggregation], [SourceStatName], [DestinationTableName], [DestinationStatName]) VALUES (32, N'DominoDailyStats', N'AVG', N'Platform.System.PctCombinedCpuUtil', N'DominoSummaryStats', N'Platform.System.PctCombinedCpuUtil')
INSERT [dbo].[DailyTasks] ([ID], [SourceTableName], [SourceAggregation], [SourceStatName], [DestinationTableName], [DestinationStatName]) VALUES (33, N'DominoDailyStats', N'AVG', N'Platform.Memory.RAM.TotalMBytes', N'DominoSummaryStats', N'Platform.Memory.RAM.TotalMBytes')
INSERT [dbo].[DailyTasks] ([ID], [SourceTableName], [SourceAggregation], [SourceStatName], [DestinationTableName], [DestinationStatName]) VALUES (34, N'DominoDailyStats', N'AVG', N'Platform.Memory.RAM.AvailMBytes', N'DominoSummaryStats', N'Platform.Memory.RAM.AvailMBytes')
INSERT [dbo].[DailyTasks] ([ID], [SourceTableName], [SourceAggregation], [SourceStatName], [DestinationTableName], [DestinationStatName]) VALUES (35, N'DominoDailyStats', N'AVG', N'Platform.Memory.KBFree', N'DominoSummaryStats', N'Platform.Memory.KBFree')
INSERT [dbo].[DailyTasks] ([ID], [SourceTableName], [SourceAggregation], [SourceStatName], [DestinationTableName], [DestinationStatName]) VALUES (36, N'DominoDailyStats', N'AVG', N'Replica.Cluster.WorkQueueDepth', N'DominoSummaryStats', N'Replica.Cluster.WorkQueueDepth')
INSERT [dbo].[DailyTasks] ([ID], [SourceTableName], [SourceAggregation], [SourceStatName], [DestinationTableName], [DestinationStatName]) VALUES (37, N'DominoDailyStats', N'AVG', N'Replica.Cluster.Failed', N'DominoSummaryStats', N'Replica.Cluster.Failed')
INSERT [dbo].[DailyTasks] ([ID], [SourceTableName], [SourceAggregation], [SourceStatName], [DestinationTableName], [DestinationStatName]) VALUES (38, N'DominoDailyStats', N'AVG', N'Replica.Cluster.SecondsOnQueue', N'DominoSummaryStats', N'Replica.Cluster.SecondsOnQueue')
INSERT [dbo].[DailyTasks] ([ID], [SourceTableName], [SourceAggregation], [SourceStatName], [DestinationTableName], [DestinationStatName]) VALUES (39, N'DominoDailyStats', N'AVG', N'Replica.Cluster.RetryWaiting', N'DominoSummaryStats', N'Replica.Cluster.RetryWaiting')
INSERT [dbo].[DailyTasks] ([ID], [SourceTableName], [SourceAggregation], [SourceStatName], [DestinationTableName], [DestinationStatName]) VALUES (40, N'DominoDailyStats', N'SUM', N'Server.Cluster.OpenRedirects.Failover.Successful', N'DominoSummaryStats', N'Server.Cluster.OpenRedirects.Failover.Successful')
INSERT [dbo].[DailyTasks] ([ID], [SourceTableName], [SourceAggregation], [SourceStatName], [DestinationTableName], [DestinationStatName]) VALUES (41, N'DominoDailyStats', N'SUM', N'Server.Cluster.OpenRedirects.Failover.Unsuccessful', N'DominoSummaryStats', N'Server.Cluster.OpenRedirects.Failover.Unsuccessful')
INSERT [dbo].[DailyTasks] ([ID], [SourceTableName], [SourceAggregation], [SourceStatName], [DestinationTableName], [DestinationStatName]) VALUES (42, N'DominoDailyStats', N'SUM', N'Server.Cluster.OpenRedirects.LoadBalance.Successful', N'DominoSummaryStats', N'Server.Cluster.OpenRedirects.LoadBalance.Successful')
INSERT [dbo].[DailyTasks] ([ID], [SourceTableName], [SourceAggregation], [SourceStatName], [DestinationTableName], [DestinationStatName]) VALUES (43, N'DominoDailyStats', N'SUM', N'Server.Cluster.OpenRedirects.LoadBalance.Unsuccessful', N'DominoSummaryStats', N'Server.Cluster.OpenRedirects.LoadBalance.Unsuccessful')
INSERT [dbo].[DailyTasks] ([ID], [SourceTableName], [SourceAggregation], [SourceStatName], [DestinationTableName], [DestinationStatName]) VALUES (44, N'DominoDailyStats', N'SUM', N'Domino.Command.OpenDocument', N'DominoSummaryStats', N'Domino.Command.OpenDocument')
INSERT [dbo].[DailyTasks] ([ID], [SourceTableName], [SourceAggregation], [SourceStatName], [DestinationTableName], [DestinationStatName]) VALUES (45, N'DominoDailyStats', N'SUM', N'Domino.Command.CreateDocument', N'DominoSummaryStats', N'Domino.Command.CreateDocument')
INSERT [dbo].[DailyTasks] ([ID], [SourceTableName], [SourceAggregation], [SourceStatName], [DestinationTableName], [DestinationStatName]) VALUES (46, N'DominoDailyStats', N'SUM', N'Domino.Command.DeleteDocument', N'DominoSummaryStats', N'Domino.Command.DeleteDocument')
INSERT [dbo].[DailyTasks] ([ID], [SourceTableName], [SourceAggregation], [SourceStatName], [DestinationTableName], [DestinationStatName]) VALUES (47, N'DominoDailyStats', N'SUM', N'Domino.Command.OpenDatabase', N'DominoSummaryStats', N'Domino.Command.OpenDatabase')
INSERT [dbo].[DailyTasks] ([ID], [SourceTableName], [SourceAggregation], [SourceStatName], [DestinationTableName], [DestinationStatName]) VALUES (48, N'DominoDailyStats', N'SUM', N'Domino.Command.OpenView', N'DominoSummaryStats', N'Domino.Command.OpenView')
INSERT [dbo].[DailyTasks] ([ID], [SourceTableName], [SourceAggregation], [SourceStatName], [DestinationTableName], [DestinationStatName]) VALUES (49, N'DominoDailyStats', N'SUM', N'Domino.Command.Total', N'DominoSummaryStats', N'Domino.Command.Total')
INSERT [dbo].[DailyTasks] ([ID], [SourceTableName], [SourceAggregation], [SourceStatName], [DestinationTableName], [DestinationStatName]) VALUES (50, N'BlackBerryProbeStats', N'AVG', N'DeliveryTime.Seconds', N'DeviceDailyStats', N'DailyDeliveryTimeAverage')
INSERT [dbo].[DailyTasks] ([ID], [SourceTableName], [SourceAggregation], [SourceStatName], [DestinationTableName], [DestinationStatName]) VALUES (51, N'NotesMailStats', N'AVG', N'DeliveryTime.Seconds', N'DeviceDailyStats', N'DailyDeliveryTimeAverage')
INSERT [dbo].[DailyTasks] ([ID], [SourceTableName], [SourceAggregation], [SourceStatName], [DestinationTableName], [DestinationStatName]) VALUES (52, N'DeviceDailyStats', N'AVG', N'ResponseTime', N'DeviceDailyStats', N'DailyResponseAverage')
/*
INSERT [dbo].[DailyTasks] ([ID], [SourceTableName], [SourceAggregation], [SourceStatName], [DestinationTableName], [DestinationStatName]) VALUES (53, N'SametimeDailyStats', N'AVG', N'AverageFileTransferSize', N'SametimeSummaryStats', N'AverageFileTransferSize')
INSERT [dbo].[DailyTasks] ([ID], [SourceTableName], [SourceAggregation], [SourceStatName], [DestinationTableName], [DestinationStatName]) VALUES (54, N'SametimeDailyStats', N'AVG', N'AverageFileTransferSizeInInterval', N'SametimeSummaryStats', N'AverageFileTransferSizeInInterval')
INSERT [dbo].[DailyTasks] ([ID], [SourceTableName], [SourceAggregation], [SourceStatName], [DestinationTableName], [DestinationStatName]) VALUES (55, N'SametimeDailyStats', N'AVG', N'AverageFileTransferTime', N'SametimeSummaryStats', N'AverageFileTransferTime')
INSERT [dbo].[DailyTasks] ([ID], [SourceTableName], [SourceAggregation], [SourceStatName], [DestinationTableName], [DestinationStatName]) VALUES (56, N'SametimeDailyStats', N'AVG', N'AvgConcurrentLoggedInUsers', N'SametimeSummaryStats', N'AvgConcurrentLoggedInUsers')
INSERT [dbo].[DailyTasks] ([ID], [SourceTableName], [SourceAggregation], [SourceStatName], [DestinationTableName], [DestinationStatName]) VALUES (57, N'SametimeDailyStats', N'AVG', N'AvgConcurrentLogins', N'SametimeSummaryStats', N'AvgConcurrentLogins')
INSERT [dbo].[DailyTasks] ([ID], [SourceTableName], [SourceAggregation], [SourceStatName], [DestinationTableName], [DestinationStatName]) VALUES (58, N'SametimeDailyStats', N'AVG', N'BroadcastConnectionsDirect', N'SametimeSummaryStats', N'BroadcastConnectionsDirect')
INSERT [dbo].[DailyTasks] ([ID], [SourceTableName], [SourceAggregation], [SourceStatName], [DestinationTableName], [DestinationStatName]) VALUES (59, N'SametimeDailyStats', N'AVG', N'BroadcastConnectionsHTTP', N'SametimeSummaryStats', N'BroadcastConnectionsHTTP')
INSERT [dbo].[DailyTasks] ([ID], [SourceTableName], [SourceAggregation], [SourceStatName], [DestinationTableName], [DestinationStatName]) VALUES (60, N'SametimeDailyStats', N'AVG', N'BroadcastMulticastStreams', N'SametimeSummaryStats', N'BroadcastMulticastStreams')
INSERT [dbo].[DailyTasks] ([ID], [SourceTableName], [SourceAggregation], [SourceStatName], [DestinationTableName], [DestinationStatName]) VALUES (61, N'SametimeDailyStats', N'AVG', N'BroadcastUnicastStreams', N'SametimeSummaryStats', N'BroadcastUnicastStreams')
INSERT [dbo].[DailyTasks] ([ID], [SourceTableName], [SourceAggregation], [SourceStatName], [DestinationTableName], [DestinationStatName]) VALUES (62, N'SametimeDailyStats', N'AVG', N'ClientsInFinishedMeetings', N'SametimeSummaryStats', N'ClientsInFinishedMeetings')
INSERT [dbo].[DailyTasks] ([ID], [SourceTableName], [SourceAggregation], [SourceStatName], [DestinationTableName], [DestinationStatName]) VALUES (63, N'SametimeDailyStats', N'AVG', N'ConcurrentImCnls', N'SametimeSummaryStats', N'ConcurrentImCnls')
INSERT [dbo].[DailyTasks] ([ID], [SourceTableName], [SourceAggregation], [SourceStatName], [DestinationTableName], [DestinationStatName]) VALUES (64, N'SametimeDailyStats', N'AVG', N'ConcurrentLoggedInUsers', N'SametimeSummaryStats', N'ConcurrentLoggedInUsers')
INSERT [dbo].[DailyTasks] ([ID], [SourceTableName], [SourceAggregation], [SourceStatName], [DestinationTableName], [DestinationStatName]) VALUES (65, N'SametimeDailyStats', N'AVG', N'ConcurrentLogins', N'SametimeSummaryStats', N'ConcurrentLogins')
INSERT [dbo].[DailyTasks] ([ID], [SourceTableName], [SourceAggregation], [SourceStatName], [DestinationTableName], [DestinationStatName]) VALUES (66, N'SametimeDailyStats', N'AVG', N'ConcurrentNWChats', N'SametimeSummaryStats', N'ConcurrentNWChats')
INSERT [dbo].[DailyTasks] ([ID], [SourceTableName], [SourceAggregation], [SourceStatName], [DestinationTableName], [DestinationStatName]) VALUES (67, N'SametimeDailyStats', N'AVG', N'DurationOfFinishedMeetings', N'SametimeSummaryStats', N'DurationOfFinishedMeetings')
INSERT [dbo].[DailyTasks] ([ID], [SourceTableName], [SourceAggregation], [SourceStatName], [DestinationTableName], [DestinationStatName]) VALUES (68, N'SametimeDailyStats', N'AVG', N'FailedLoginOperations', N'SametimeSummaryStats', N'FailedLoginOperations')
INSERT [dbo].[DailyTasks] ([ID], [SourceTableName], [SourceAggregation], [SourceStatName], [DestinationTableName], [DestinationStatName]) VALUES (69, N'SametimeDailyStats', N'AVG', N'H323ClientConnections', N'SametimeSummaryStats', N'H323ClientConnections')
INSERT [dbo].[DailyTasks] ([ID], [SourceTableName], [SourceAggregation], [SourceStatName], [DestinationTableName], [DestinationStatName]) VALUES (70, N'SametimeDailyStats', N'AVG', N'ImMsgs', N'SametimeSummaryStats', N'ImMsgs')
INSERT [dbo].[DailyTasks] ([ID], [SourceTableName], [SourceAggregation], [SourceStatName], [DestinationTableName], [DestinationStatName]) VALUES (71, N'SametimeDailyStats', N'AVG', N'Ims', N'SametimeSummaryStats', N'Ims')
INSERT [dbo].[DailyTasks] ([ID], [SourceTableName], [SourceAggregation], [SourceStatName], [DestinationTableName], [DestinationStatName]) VALUES (72, N'SametimeDailyStats', N'AVG', N'IncompleteLoginOperations', N'SametimeSummaryStats', N'IncompleteLoginOperations')
INSERT [dbo].[DailyTasks] ([ID], [SourceTableName], [SourceAggregation], [SourceStatName], [DestinationTableName], [DestinationStatName]) VALUES (73, N'SametimeDailyStats', N'AVG', N'InstantAppshareClients', N'SametimeSummaryStats', N'InstantAppshareClients')
INSERT [dbo].[DailyTasks] ([ID], [SourceTableName], [SourceAggregation], [SourceStatName], [DestinationTableName], [DestinationStatName]) VALUES (74, N'SametimeDailyStats', N'AVG', N'InstantAudioClients', N'SametimeSummaryStats', N'InstantAudioClients')
INSERT [dbo].[DailyTasks] ([ID], [SourceTableName], [SourceAggregation], [SourceStatName], [DestinationTableName], [DestinationStatName]) VALUES (75, N'SametimeDailyStats', N'AVG', N'InstantChatClients', N'SametimeSummaryStats', N'InstantChatClients')
INSERT [dbo].[DailyTasks] ([ID], [SourceTableName], [SourceAggregation], [SourceStatName], [DestinationTableName], [DestinationStatName]) VALUES (76, N'SametimeDailyStats', N'AVG', N'InstantMeetingClients', N'SametimeSummaryStats', N'InstantMeetingClients')
INSERT [dbo].[DailyTasks] ([ID], [SourceTableName], [SourceAggregation], [SourceStatName], [DestinationTableName], [DestinationStatName]) VALUES (77, N'SametimeDailyStats', N'AVG', N'InstantMeetings', N'SametimeSummaryStats', N'InstantMeetings')
INSERT [dbo].[DailyTasks] ([ID], [SourceTableName], [SourceAggregation], [SourceStatName], [DestinationTableName], [DestinationStatName]) VALUES (78, N'SametimeDailyStats', N'AVG', N'InstantMeetingsAppshare', N'SametimeSummaryStats', N'InstantMeetingsAppshare')
INSERT [dbo].[DailyTasks] ([ID], [SourceTableName], [SourceAggregation], [SourceStatName], [DestinationTableName], [DestinationStatName]) VALUES (79, N'SametimeDailyStats', N'AVG', N'InstantMeetingsAudio', N'SametimeSummaryStats', N'InstantMeetingsAudio')
INSERT [dbo].[DailyTasks] ([ID], [SourceTableName], [SourceAggregation], [SourceStatName], [DestinationTableName], [DestinationStatName]) VALUES (80, N'SametimeDailyStats', N'AVG', N'InstantMeetingsChat', N'SametimeSummaryStats', N'InstantMeetingsChat')
INSERT [dbo].[DailyTasks] ([ID], [SourceTableName], [SourceAggregation], [SourceStatName], [DestinationTableName], [DestinationStatName]) VALUES (81, N'SametimeDailyStats', N'AVG', N'InstantMeetingsPolling', N'SametimeSummaryStats', N'InstantMeetingsPolling')
INSERT [dbo].[DailyTasks] ([ID], [SourceTableName], [SourceAggregation], [SourceStatName], [DestinationTableName], [DestinationStatName]) VALUES (82, N'SametimeDailyStats', N'AVG', N'InstantMeetingsSendWebPage', N'SametimeSummaryStats', N'InstantMeetingsSendWebPage')
INSERT [dbo].[DailyTasks] ([ID], [SourceTableName], [SourceAggregation], [SourceStatName], [DestinationTableName], [DestinationStatName]) VALUES (83, N'SametimeDailyStats', N'AVG', N'InstantMeetingsVideo', N'SametimeSummaryStats', N'InstantMeetingsVideo')
INSERT [dbo].[DailyTasks] ([ID], [SourceTableName], [SourceAggregation], [SourceStatName], [DestinationTableName], [DestinationStatName]) VALUES (84, N'SametimeDailyStats', N'AVG', N'InstantMeetingsWhiteboard', N'SametimeSummaryStats', N'InstantMeetingsWhiteboard')
INSERT [dbo].[DailyTasks] ([ID], [SourceTableName], [SourceAggregation], [SourceStatName], [DestinationTableName], [DestinationStatName]) VALUES (85, N'SametimeDailyStats', N'AVG', N'InstantPollingClients', N'SametimeSummaryStats', N'InstantPollingClients')
INSERT [dbo].[DailyTasks] ([ID], [SourceTableName], [SourceAggregation], [SourceStatName], [DestinationTableName], [DestinationStatName]) VALUES (86, N'SametimeDailyStats', N'AVG', N'InstantSendWebPageClients', N'SametimeSummaryStats', N'InstantSendWebPageClients')
INSERT [dbo].[DailyTasks] ([ID], [SourceTableName], [SourceAggregation], [SourceStatName], [DestinationTableName], [DestinationStatName]) VALUES (87, N'SametimeDailyStats', N'AVG', N'InstantVideoClients', N'SametimeSummaryStats', N'InstantVideoClients')
INSERT [dbo].[DailyTasks] ([ID], [SourceTableName], [SourceAggregation], [SourceStatName], [DestinationTableName], [DestinationStatName]) VALUES (88, N'SametimeDailyStats', N'AVG', N'InstantWhiteboardClients', N'SametimeSummaryStats', N'InstantWhiteboardClients')
INSERT [dbo].[DailyTasks] ([ID], [SourceTableName], [SourceAggregation], [SourceStatName], [DestinationTableName], [DestinationStatName]) VALUES (89, N'SametimeDailyStats', N'Max', N'MaxConcurrentLoggedInUsers', N'SametimeSummaryStats', N'MaxConcurrentLoggedInUsers')
INSERT [dbo].[DailyTasks] ([ID], [SourceTableName], [SourceAggregation], [SourceStatName], [DestinationTableName], [DestinationStatName]) VALUES (90, N'SametimeDailyStats', N'AVG', N'MaxConcurrentLogins', N'SametimeSummaryStats', N'MaxConcurrentLogins')
INSERT [dbo].[DailyTasks] ([ID], [SourceTableName], [SourceAggregation], [SourceStatName], [DestinationTableName], [DestinationStatName]) VALUES (91, N'SametimeDailyStats', N'AVG', N'MaxFileTransferSize', N'SametimeSummaryStats', N'MaxFileTransferSize')
INSERT [dbo].[DailyTasks] ([ID], [SourceTableName], [SourceAggregation], [SourceStatName], [DestinationTableName], [DestinationStatName]) VALUES (92, N'SametimeDailyStats', N'AVG', N'MaxFileTransferSizeInInterval', N'SametimeSummaryStats', N'MaxFileTransferSizeInInterval')
INSERT [dbo].[DailyTasks] ([ID], [SourceTableName], [SourceAggregation], [SourceStatName], [DestinationTableName], [DestinationStatName]) VALUES (93, N'SametimeDailyStats', N'AVG', N'MaxFileTransferTime', N'SametimeSummaryStats', N'MaxFileTransferTime')
INSERT [dbo].[DailyTasks] ([ID], [SourceTableName], [SourceAggregation], [SourceStatName], [DestinationTableName], [DestinationStatName]) VALUES (94, N'SametimeDailyStats', N'AVG', N'MaxImCnls', N'SametimeSummaryStats', N'MaxImCnls')
INSERT [dbo].[DailyTasks] ([ID], [SourceTableName], [SourceAggregation], [SourceStatName], [DestinationTableName], [DestinationStatName]) VALUES (95, N'SametimeDailyStats', N'AVG', N'MaxMeetingsNum', N'SametimeSummaryStats', N'MaxMeetingsNum')
INSERT [dbo].[DailyTasks] ([ID], [SourceTableName], [SourceAggregation], [SourceStatName], [DestinationTableName], [DestinationStatName]) VALUES (96, N'SametimeDailyStats', N'AVG', N'MaxNWChats', N'SametimeSummaryStats', N'MaxNWChats')
INSERT [dbo].[DailyTasks] ([ID], [SourceTableName], [SourceAggregation], [SourceStatName], [DestinationTableName], [DestinationStatName]) VALUES (97, N'SametimeDailyStats', N'AVG', N'MaxPlacesNum', N'SametimeSummaryStats', N'MaxPlacesNum')
INSERT [dbo].[DailyTasks] ([ID], [SourceTableName], [SourceAggregation], [SourceStatName], [DestinationTableName], [DestinationStatName]) VALUES (98, N'SametimeDailyStats', N'AVG', N'MeetingsNum', N'SametimeSummaryStats', N'MeetingsNum')
INSERT [dbo].[DailyTasks] ([ID], [SourceTableName], [SourceAggregation], [SourceStatName], [DestinationTableName], [DestinationStatName]) VALUES (99, N'SametimeDailyStats', N'AVG', N'MinConcurrentLoggedInUsers', N'SametimeSummaryStats', N'MinConcurrentLoggedInUsers')
INSERT [dbo].[DailyTasks] ([ID], [SourceTableName], [SourceAggregation], [SourceStatName], [DestinationTableName], [DestinationStatName]) VALUES (100, N'SametimeDailyStats', N'AVG', N'MinConcurrentLogins', N'SametimeSummaryStats', N'MinConcurrentLogins')
INSERT [dbo].[DailyTasks] ([ID], [SourceTableName], [SourceAggregation], [SourceStatName], [DestinationTableName], [DestinationStatName]) VALUES (101, N'SametimeDailyStats', N'AVG', N'NewImCnls', N'SametimeSummaryStats', N'NewImCnls')
GO
print 'Processed 100 total records'
INSERT [dbo].[DailyTasks] ([ID], [SourceTableName], [SourceAggregation], [SourceStatName], [DestinationTableName], [DestinationStatName]) VALUES (102, N'SametimeDailyStats', N'AVG', N'Number_Of_Ims_with_external_users', N'SametimeSummaryStats', N'Number_Of_Ims_with_external_users')
INSERT [dbo].[DailyTasks] ([ID], [SourceTableName], [SourceAggregation], [SourceStatName], [DestinationTableName], [DestinationStatName]) VALUES (103, N'SametimeDailyStats', N'AVG', N'NumberOfAttributesChanges', N'SametimeSummaryStats', N'NumberOfAttributesChanges')
INSERT [dbo].[DailyTasks] ([ID], [SourceTableName], [SourceAggregation], [SourceStatName], [DestinationTableName], [DestinationStatName]) VALUES (104, N'SametimeDailyStats', N'AVG', N'NumberOfAttributesChangesInPM', N'SametimeSummaryStats', N'NumberOfAttributesChangesInPM')
INSERT [dbo].[DailyTasks] ([ID], [SourceTableName], [SourceAggregation], [SourceStatName], [DestinationTableName], [DestinationStatName]) VALUES (105, N'SametimeDailyStats', N'AVG', N'NumberOfBuddyListChannelCreations', N'SametimeSummaryStats', N'NumberOfBuddyListChannelCreations')
INSERT [dbo].[DailyTasks] ([ID], [SourceTableName], [SourceAggregation], [SourceStatName], [DestinationTableName], [DestinationStatName]) VALUES (106, N'SametimeDailyStats', N'AVG', N'NumberOfFetchRequests', N'SametimeSummaryStats', N'NumberOfFetchRequests')
INSERT [dbo].[DailyTasks] ([ID], [SourceTableName], [SourceAggregation], [SourceStatName], [DestinationTableName], [DestinationStatName]) VALUES (107, N'SametimeDailyStats', N'AVG', N'NumberOfFileTransferFailures', N'SametimeSummaryStats', N'NumberOfFileTransferFailures')
INSERT [dbo].[DailyTasks] ([ID], [SourceTableName], [SourceAggregation], [SourceStatName], [DestinationTableName], [DestinationStatName]) VALUES (108, N'SametimeDailyStats', N'AVG', N'NumberOfFileTransferFailuresDueToPolicy', N'SametimeSummaryStats', N'NumberOfFileTransferFailuresDueToPolicy')
INSERT [dbo].[DailyTasks] ([ID], [SourceTableName], [SourceAggregation], [SourceStatName], [DestinationTableName], [DestinationStatName]) VALUES (109, N'SametimeDailyStats', N'AVG', N'NumberOfFileTransferFailuresDueToVirus', N'SametimeSummaryStats', N'NumberOfFileTransferFailuresDueToVirus')
INSERT [dbo].[DailyTasks] ([ID], [SourceTableName], [SourceAggregation], [SourceStatName], [DestinationTableName], [DestinationStatName]) VALUES (110, N'SametimeDailyStats', N'AVG', N'NumberOfFileTransfers', N'SametimeSummaryStats', N'NumberOfFileTransfers')
INSERT [dbo].[DailyTasks] ([ID], [SourceTableName], [SourceAggregation], [SourceStatName], [DestinationTableName], [DestinationStatName]) VALUES (111, N'SametimeDailyStats', N'AVG', N'NumberOfFileTransferSuccesses', N'SametimeSummaryStats', N'NumberOfFileTransferSuccesses')
INSERT [dbo].[DailyTasks] ([ID], [SourceTableName], [SourceAggregation], [SourceStatName], [DestinationTableName], [DestinationStatName]) VALUES (112, N'SametimeDailyStats', N'AVG', N'NumberOfLogins', N'SametimeSummaryStats', N'NumberOfLogins')
INSERT [dbo].[DailyTasks] ([ID], [SourceTableName], [SourceAggregation], [SourceStatName], [DestinationTableName], [DestinationStatName]) VALUES (113, N'SametimeDailyStats', N'AVG', N'NumberOfLogouts', N'SametimeSummaryStats', N'NumberOfLogouts')
INSERT [dbo].[DailyTasks] ([ID], [SourceTableName], [SourceAggregation], [SourceStatName], [DestinationTableName], [DestinationStatName]) VALUES (114, N'SametimeDailyStats', N'AVG', N'NumberOfNotificationsFromExternalUsers', N'SametimeSummaryStats', N'NumberOfNotificationsFromExternalUsers')
INSERT [dbo].[DailyTasks] ([ID], [SourceTableName], [SourceAggregation], [SourceStatName], [DestinationTableName], [DestinationStatName]) VALUES (115, N'SametimeDailyStats', N'AVG', N'NumberOfNotificationsToExternalUsers', N'SametimeSummaryStats', N'NumberOfNotificationsToExternalUsers')
INSERT [dbo].[DailyTasks] ([ID], [SourceTableName], [SourceAggregation], [SourceStatName], [DestinationTableName], [DestinationStatName]) VALUES (116, N'SametimeDailyStats', N'AVG', N'NumberOfOfflineStatusChanges', N'SametimeSummaryStats', N'NumberOfOfflineStatusChanges')
INSERT [dbo].[DailyTasks] ([ID], [SourceTableName], [SourceAggregation], [SourceStatName], [DestinationTableName], [DestinationStatName]) VALUES (117, N'SametimeDailyStats', N'AVG', N'NumberOfPrivacyChangesInPM', N'SametimeSummaryStats', N'NumberOfPrivacyChangesInPM')
INSERT [dbo].[DailyTasks] ([ID], [SourceTableName], [SourceAggregation], [SourceStatName], [DestinationTableName], [DestinationStatName]) VALUES (118, N'SametimeDailyStats', N'AVG', N'NumberOfStatusChanges', N'SametimeSummaryStats', N'NumberOfStatusChanges')
INSERT [dbo].[DailyTasks] ([ID], [SourceTableName], [SourceAggregation], [SourceStatName], [DestinationTableName], [DestinationStatName]) VALUES (119, N'SametimeDailyStats', N'AVG', N'NumberOfStatusChangesInPM', N'SametimeSummaryStats', N'NumberOfStatusChangesInPM')
INSERT [dbo].[DailyTasks] ([ID], [SourceTableName], [SourceAggregation], [SourceStatName], [DestinationTableName], [DestinationStatName]) VALUES (120, N'SametimeDailyStats', N'AVG', N'NumberOfSubscribes', N'SametimeSummaryStats', N'NumberOfSubscribes')
INSERT [dbo].[DailyTasks] ([ID], [SourceTableName], [SourceAggregation], [SourceStatName], [DestinationTableName], [DestinationStatName]) VALUES (121, N'SametimeDailyStats', N'AVG', N'NumberOfSubscribesFromExternalUsers', N'SametimeSummaryStats', N'NumberOfSubscribesFromExternalUsers')
INSERT [dbo].[DailyTasks] ([ID], [SourceTableName], [SourceAggregation], [SourceStatName], [DestinationTableName], [DestinationStatName]) VALUES (122, N'SametimeDailyStats', N'AVG', N'NumberOfSubscribesOnExternalUsers', N'SametimeSummaryStats', N'NumberOfSubscribesOnExternalUsers')
INSERT [dbo].[DailyTasks] ([ID], [SourceTableName], [SourceAggregation], [SourceStatName], [DestinationTableName], [DestinationStatName]) VALUES (123, N'SametimeDailyStats', N'AVG', N'NumberOfUnsubscribes', N'SametimeSummaryStats', N'NumberOfUnsubscribes')
INSERT [dbo].[DailyTasks] ([ID], [SourceTableName], [SourceAggregation], [SourceStatName], [DestinationTableName], [DestinationStatName]) VALUES (124, N'SametimeDailyStats', N'AVG', N'NumberOfUnSubscribesFromExternalUsers', N'SametimeSummaryStats', N'NumberOfUnSubscribesFromExternalUsers')
INSERT [dbo].[DailyTasks] ([ID], [SourceTableName], [SourceAggregation], [SourceStatName], [DestinationTableName], [DestinationStatName]) VALUES (125, N'SametimeDailyStats', N'AVG', N'NumberOfUnSubscribesOnExternalUsers', N'SametimeSummaryStats', N'NumberOfUnSubscribesOnExternalUsers')
INSERT [dbo].[DailyTasks] ([ID], [SourceTableName], [SourceAggregation], [SourceStatName], [DestinationTableName], [DestinationStatName]) VALUES (126, N'SametimeDailyStats', N'AVG', N'NumberOfUnWatchedUsers', N'SametimeSummaryStats', N'NumberOfUnWatchedUsers')
INSERT [dbo].[DailyTasks] ([ID], [SourceTableName], [SourceAggregation], [SourceStatName], [DestinationTableName], [DestinationStatName]) VALUES (127, N'SametimeDailyStats', N'AVG', N'NumberOfWatchedUsers', N'SametimeSummaryStats', N'NumberOfWatchedUsers')
INSERT [dbo].[DailyTasks] ([ID], [SourceTableName], [SourceAggregation], [SourceStatName], [DestinationTableName], [DestinationStatName]) VALUES (128, N'SametimeDailyStats', N'AVG', N'NWs', N'SametimeSummaryStats', N'NWs')
INSERT [dbo].[DailyTasks] ([ID], [SourceTableName], [SourceAggregation], [SourceStatName], [DestinationTableName], [DestinationStatName]) VALUES (129, N'SametimeDailyStats', N'AVG', N'Places', N'SametimeSummaryStats', N'Places')
INSERT [dbo].[DailyTasks] ([ID], [SourceTableName], [SourceAggregation], [SourceStatName], [DestinationTableName], [DestinationStatName]) VALUES (130, N'SametimeDailyStats', N'AVG', N'PlacesChatMsgs', N'SametimeSummaryStats', N'PlacesChatMsgs')
INSERT [dbo].[DailyTasks] ([ID], [SourceTableName], [SourceAggregation], [SourceStatName], [DestinationTableName], [DestinationStatName]) VALUES (131, N'SametimeDailyStats', N'AVG', N'PlacesConcurrentMembers', N'SametimeSummaryStats', N'PlacesConcurrentMembers')
INSERT [dbo].[DailyTasks] ([ID], [SourceTableName], [SourceAggregation], [SourceStatName], [DestinationTableName], [DestinationStatName]) VALUES (132, N'SametimeDailyStats', N'AVG', N'PlacesMaxConcurrentMembers', N'SametimeSummaryStats', N'PlacesMaxConcurrentMembers')
INSERT [dbo].[DailyTasks] ([ID], [SourceTableName], [SourceAggregation], [SourceStatName], [DestinationTableName], [DestinationStatName]) VALUES (133, N'SametimeDailyStats', N'AVG', N'PlacesNum', N'SametimeSummaryStats', N'PlacesNum')
INSERT [dbo].[DailyTasks] ([ID], [SourceTableName], [SourceAggregation], [SourceStatName], [DestinationTableName], [DestinationStatName]) VALUES (134, N'SametimeDailyStats', N'AVG', N'remaining_connections', N'SametimeSummaryStats', N'remaining_connections')
INSERT [dbo].[DailyTasks] ([ID], [SourceTableName], [SourceAggregation], [SourceStatName], [DestinationTableName], [DestinationStatName]) VALUES (135, N'SametimeDailyStats', N'AVG', N'ScheduledAppshareClients', N'SametimeSummaryStats', N'ScheduledAppshareClients')
INSERT [dbo].[DailyTasks] ([ID], [SourceTableName], [SourceAggregation], [SourceStatName], [DestinationTableName], [DestinationStatName]) VALUES (136, N'SametimeDailyStats', N'AVG', N'ScheduledAudioClients', N'SametimeSummaryStats', N'ScheduledAudioClients')
INSERT [dbo].[DailyTasks] ([ID], [SourceTableName], [SourceAggregation], [SourceStatName], [DestinationTableName], [DestinationStatName]) VALUES (137, N'SametimeDailyStats', N'AVG', N'ScheduledChatClients', N'SametimeSummaryStats', N'ScheduledChatClients')
INSERT [dbo].[DailyTasks] ([ID], [SourceTableName], [SourceAggregation], [SourceStatName], [DestinationTableName], [DestinationStatName]) VALUES (138, N'SametimeDailyStats', N'AVG', N'ScheduledH323Clients', N'SametimeSummaryStats', N'ScheduledH323Clients')
INSERT [dbo].[DailyTasks] ([ID], [SourceTableName], [SourceAggregation], [SourceStatName], [DestinationTableName], [DestinationStatName]) VALUES (139, N'SametimeDailyStats', N'AVG', N'ScheduledMeetingClients', N'SametimeSummaryStats', N'ScheduledMeetingClients')
INSERT [dbo].[DailyTasks] ([ID], [SourceTableName], [SourceAggregation], [SourceStatName], [DestinationTableName], [DestinationStatName]) VALUES (140, N'SametimeDailyStats', N'AVG', N'ScheduledMeetings', N'SametimeSummaryStats', N'ScheduledMeetings')
INSERT [dbo].[DailyTasks] ([ID], [SourceTableName], [SourceAggregation], [SourceStatName], [DestinationTableName], [DestinationStatName]) VALUES (141, N'SametimeDailyStats', N'AVG', N'ScheduledMeetingsAppshare', N'SametimeSummaryStats', N'ScheduledMeetingsAppshare')
INSERT [dbo].[DailyTasks] ([ID], [SourceTableName], [SourceAggregation], [SourceStatName], [DestinationTableName], [DestinationStatName]) VALUES (142, N'SametimeDailyStats', N'AVG', N'ScheduledMeetingsAudio', N'SametimeSummaryStats', N'ScheduledMeetingsAudio')
INSERT [dbo].[DailyTasks] ([ID], [SourceTableName], [SourceAggregation], [SourceStatName], [DestinationTableName], [DestinationStatName]) VALUES (143, N'SametimeDailyStats', N'AVG', N'ScheduledMeetingsBroadcast', N'SametimeSummaryStats', N'ScheduledMeetingsBroadcast')
INSERT [dbo].[DailyTasks] ([ID], [SourceTableName], [SourceAggregation], [SourceStatName], [DestinationTableName], [DestinationStatName]) VALUES (144, N'SametimeDailyStats', N'AVG', N'ScheduledMeetingsChat', N'SametimeSummaryStats', N'ScheduledMeetingsChat')
INSERT [dbo].[DailyTasks] ([ID], [SourceTableName], [SourceAggregation], [SourceStatName], [DestinationTableName], [DestinationStatName]) VALUES (145, N'SametimeDailyStats', N'AVG', N'ScheduledMeetingsNetmeeting', N'SametimeSummaryStats', N'ScheduledMeetingsNetmeeting')
INSERT [dbo].[DailyTasks] ([ID], [SourceTableName], [SourceAggregation], [SourceStatName], [DestinationTableName], [DestinationStatName]) VALUES (146, N'SametimeDailyStats', N'AVG', N'ScheduledMeetingsNonBroadcast', N'SametimeSummaryStats', N'ScheduledMeetingsNonBroadcast')
INSERT [dbo].[DailyTasks] ([ID], [SourceTableName], [SourceAggregation], [SourceStatName], [DestinationTableName], [DestinationStatName]) VALUES (147, N'SametimeDailyStats', N'AVG', N'ScheduledMeetingsPolling', N'SametimeSummaryStats', N'ScheduledMeetingsPolling')
INSERT [dbo].[DailyTasks] ([ID], [SourceTableName], [SourceAggregation], [SourceStatName], [DestinationTableName], [DestinationStatName]) VALUES (148, N'SametimeDailyStats', N'AVG', N'ScheduledMeetingsSendWebPage', N'SametimeSummaryStats', N'ScheduledMeetingsSendWebPage')
INSERT [dbo].[DailyTasks] ([ID], [SourceTableName], [SourceAggregation], [SourceStatName], [DestinationTableName], [DestinationStatName]) VALUES (149, N'SametimeDailyStats', N'AVG', N'ScheduledMeetingsVideo', N'SametimeSummaryStats', N'ScheduledMeetingsVideo')
INSERT [dbo].[DailyTasks] ([ID], [SourceTableName], [SourceAggregation], [SourceStatName], [DestinationTableName], [DestinationStatName]) VALUES (150, N'SametimeDailyStats', N'AVG', N'ScheduledMeetingsWhiteboard', N'SametimeSummaryStats', N'ScheduledMeetingsWhiteboard')
INSERT [dbo].[DailyTasks] ([ID], [SourceTableName], [SourceAggregation], [SourceStatName], [DestinationTableName], [DestinationStatName]) VALUES (151, N'SametimeDailyStats', N'AVG', N'ScheduledPollingClients', N'SametimeSummaryStats', N'ScheduledPollingClients')
INSERT [dbo].[DailyTasks] ([ID], [SourceTableName], [SourceAggregation], [SourceStatName], [DestinationTableName], [DestinationStatName]) VALUES (152, N'SametimeDailyStats', N'AVG', N'ScheduledSendWebPageClients', N'SametimeSummaryStats', N'ScheduledSendWebPageClients')
INSERT [dbo].[DailyTasks] ([ID], [SourceTableName], [SourceAggregation], [SourceStatName], [DestinationTableName], [DestinationStatName]) VALUES (153, N'SametimeDailyStats', N'AVG', N'ScheduledVideoClients', N'SametimeSummaryStats', N'ScheduledVideoClients')
INSERT [dbo].[DailyTasks] ([ID], [SourceTableName], [SourceAggregation], [SourceStatName], [DestinationTableName], [DestinationStatName]) VALUES (154, N'SametimeDailyStats', N'AVG', N'ScheduledWhiteboardClients', N'SametimeSummaryStats', N'ScheduledWhiteboardClients')
INSERT [dbo].[DailyTasks] ([ID], [SourceTableName], [SourceAggregation], [SourceStatName], [DestinationTableName], [DestinationStatName]) VALUES (155, N'SametimeDailyStats', N'AVG', N'StartedMeetings', N'SametimeSummaryStats', N'StartedMeetings')
INSERT [dbo].[DailyTasks] ([ID], [SourceTableName], [SourceAggregation], [SourceStatName], [DestinationTableName], [DestinationStatName]) VALUES (156, N'SametimeDailyStats', N'AVG', N'StartedNWChats', N'SametimeSummaryStats', N'StartedNWChats')
INSERT [dbo].[DailyTasks] ([ID], [SourceTableName], [SourceAggregation], [SourceStatName], [DestinationTableName], [DestinationStatName]) VALUES (157, N'SametimeDailyStats', N'AVG', N'StartedPlaces', N'SametimeSummaryStats', N'StartedPlaces')
INSERT [dbo].[DailyTasks] ([ID], [SourceTableName], [SourceAggregation], [SourceStatName], [DestinationTableName], [DestinationStatName]) VALUES (158, N'SametimeDailyStats', N'AVG', N'SuccessfulLoginOperations', N'SametimeSummaryStats', N'SuccessfulLoginOperations')
INSERT [dbo].[DailyTasks] ([ID], [SourceTableName], [SourceAggregation], [SourceStatName], [DestinationTableName], [DestinationStatName]) VALUES (159, N'SametimeDailyStats', N'AVG', N'SuccessfulUserDownOperations', N'SametimeSummaryStats', N'SuccessfulUserDownOperations')
INSERT [dbo].[DailyTasks] ([ID], [SourceTableName], [SourceAggregation], [SourceStatName], [DestinationTableName], [DestinationStatName]) VALUES (160, N'SametimeDailyStats', N'AVG', N'SuccessfulUserUpOperations', N'SametimeSummaryStats', N'SuccessfulUserUpOperations')
INSERT [dbo].[DailyTasks] ([ID], [SourceTableName], [SourceAggregation], [SourceStatName], [DestinationTableName], [DestinationStatName]) VALUES (161, N'SametimeDailyStats', N'AVG', N'T120ClientConnections', N'SametimeSummaryStats', N'T120ClientConnections')
INSERT [dbo].[DailyTasks] ([ID], [SourceTableName], [SourceAggregation], [SourceStatName], [DestinationTableName], [DestinationStatName]) VALUES (162, N'SametimeDailyStats', N'AVG', N'ThinClientConnectionsDirect', N'SametimeSummaryStats', N'ThinClientConnectionsDirect')
INSERT [dbo].[DailyTasks] ([ID], [SourceTableName], [SourceAggregation], [SourceStatName], [DestinationTableName], [DestinationStatName]) VALUES (163, N'SametimeDailyStats', N'AVG', N'ThinClientConnectionsHTTP', N'SametimeSummaryStats', N'ThinClientConnectionsHTTP')
INSERT [dbo].[DailyTasks] ([ID], [SourceTableName], [SourceAggregation], [SourceStatName], [DestinationTableName], [DestinationStatName]) VALUES (164, N'SametimeDailyStats', N'AVG', N'ThinClientConnectionsHTTPS', N'SametimeSummaryStats', N'ThinClientConnectionsHTTPS')
INSERT [dbo].[DailyTasks] ([ID], [SourceTableName], [SourceAggregation], [SourceStatName], [DestinationTableName], [DestinationStatName]) VALUES (165, N'SametimeDailyStats', N'AVG', N'TotalLogins', N'SametimeSummaryStats', N'TotalLogins')
INSERT [dbo].[DailyTasks] ([ID], [SourceTableName], [SourceAggregation], [SourceStatName], [DestinationTableName], [DestinationStatName]) VALUES (166, N'SametimeDailyStats', N'AVG', N'TotalLogouts', N'SametimeSummaryStats', N'TotalLogouts')
INSERT [dbo].[DailyTasks] ([ID], [SourceTableName], [SourceAggregation], [SourceStatName], [DestinationTableName], [DestinationStatName]) VALUES (167, N'SametimeDailyStats', N'AVG', N'TotalNumberOfsubscribes', N'SametimeSummaryStats', N'TotalNumberOfsubscribes')
INSERT [dbo].[DailyTasks] ([ID], [SourceTableName], [SourceAggregation], [SourceStatName], [DestinationTableName], [DestinationStatName]) VALUES (168, N'SametimeDailyStats', N'AVG', N'TotalNumberOfSubscribesFromExternalUsers', N'SametimeSummaryStats', N'TotalNumberOfSubscribesFromExternalUsers')
INSERT [dbo].[DailyTasks] ([ID], [SourceTableName], [SourceAggregation], [SourceStatName], [DestinationTableName], [DestinationStatName]) VALUES (169, N'SametimeDailyStats', N'AVG', N'TotalNumberOfSubscribesOnExternalUsers', N'SametimeSummaryStats', N'TotalNumberOfSubscribesOnExternalUsers')
INSERT [dbo].[DailyTasks] ([ID], [SourceTableName], [SourceAggregation], [SourceStatName], [DestinationTableName], [DestinationStatName]) VALUES (170, N'SametimeDailyStats', N'AVG', N'TotalNumberOfWatched', N'SametimeSummaryStats', N'TotalNumberOfWatched')
INSERT [dbo].[DailyTasks] ([ID], [SourceTableName], [SourceAggregation], [SourceStatName], [DestinationTableName], [DestinationStatName]) VALUES (171, N'SametimeDailyStats', N'AVG', N'TotalNWChats', N'SametimeSummaryStats', N'TotalNWChats')
INSERT [dbo].[DailyTasks] ([ID], [SourceTableName], [SourceAggregation], [SourceStatName], [DestinationTableName], [DestinationStatName]) VALUES (172, N'SametimeDailyStats', N'AVG', N'TotalTwoWayChats', N'SametimeSummaryStats', N'TotalTwoWayChats')
INSERT [dbo].[DailyTasks] ([ID], [SourceTableName], [SourceAggregation], [SourceStatName], [DestinationTableName], [DestinationStatName]) VALUES (173, N'SametimeDailyStats', N'AVG', N'TotalUniqueLogins', N'SametimeSummaryStats', N'TotalUniqueLogins')
INSERT [dbo].[DailyTasks] ([ID], [SourceTableName], [SourceAggregation], [SourceStatName], [DestinationTableName], [DestinationStatName]) VALUES (174, N'SametimeDailyStats', N'AVG', N'UserLoginsNum', N'SametimeSummaryStats', N'UserLoginsNum')
INSERT [dbo].[DailyTasks] ([ID], [SourceTableName], [SourceAggregation], [SourceStatName], [DestinationTableName], [DestinationStatName]) VALUES (175, N'SametimeDailyStats', N'AVG', N'UsersNum', N'SametimeSummaryStats', N'UsersNum')
INSERT [dbo].[DailyTasks] ([ID], [SourceTableName], [SourceAggregation], [SourceStatName], [DestinationTableName], [DestinationStatName]) VALUES (176, N'SametimeDailyStats', N'AVG', N'Chat Sessions', N'SametimeSummaryStats', N'Chat Sessions')
INSERT [dbo].[DailyTasks] ([ID], [SourceTableName], [SourceAggregation], [SourceStatName], [DestinationTableName], [DestinationStatName]) VALUES (177, N'SametimeDailyStats', N'AVG', N'n-Way Chat Sessions', N'SametimeSummaryStats', N'n-Way Chat Sessions')
INSERT [dbo].[DailyTasks] ([ID], [SourceTableName], [SourceAggregation], [SourceStatName], [DestinationTableName], [DestinationStatName]) VALUES (178, N'SametimeDailyStats', N'AVG', N'Places', N'SametimeSummaryStats', N'Places') 
*/
INSERT [dbo].[DailyTasks] ([ID], [SourceTableName], [SourceAggregation], [SourceStatName], [DestinationTableName], [DestinationStatName]) VALUES (180, N'DominoDailyStats', N'Max', N'Platform.System.PctCombinedCpuUtil', N'DominoSummaryStats', N'Platform.System.PctCombinedCpuUtil.Max')
INSERT [dbo].[DailyTasks] ([ID], [SourceTableName], [SourceAggregation], [SourceStatName], [DestinationTableName], [DestinationStatName]) VALUES (181, N'DeviceUpTimeStats', N'AVG', N'HourlyUpPercent', N'DeviceUpTimeStats', N'DailyUpTimePercent')
INSERT [dbo].[DailyTasks] ([ID], [SourceTableName], [SourceAggregation], [SourceStatName], [DestinationTableName], [DestinationStatName]) VALUES (182, N'DeviceUpTimeStats', N'SUM', N'HourlyDownTimeMinutes', N'DeviceUpTimeStats', N'DailyDownTimeMinutes')
INSERT [dbo].[DailyTasks] ([ID], [SourceTableName], [SourceAggregation], [SourceStatName], [DestinationTableName], [DestinationStatName]) VALUES (183, N'DeviceUpTimeStats', N'AVG', N'HourlyOnTargetPercent', N'DeviceUpTimeStats', N'DailyOnTargetPercent')
/* 1/13/2015 NS added for VSPLUS-1331 */
/*INSERT [dbo].[DailyTasks] ([ID], [SourceTableName], [SourceAggregation], [SourceStatName], [DestinationTableName], [DestinationStatName]) VALUES (184, N'SametimeDailyStats', N'AVG', N'Users', N'SametimeSummaryStats', N'Users')*/
/* 10/8/2015 NS added for VSPLUS-2208 */
INSERT [dbo].[DailyTasks] ([ID], [SourceTableName], [SourceAggregation], [SourceStatName], [DestinationTableName], [DestinationStatName]) VALUES (185, N'DominoDailyStats', N'AVG', N'Traveler.Memory.Java.Current', N'DominoSummaryStats', N'MemoryJava')
INSERT [dbo].[DailyTasks] ([ID], [SourceTableName], [SourceAggregation], [SourceStatName], [DestinationTableName], [DestinationStatName]) VALUES (186, N'DominoDailyStats', N'AVG', N'Traveler.Memory.C.Current', N'DominoSummaryStats', N'MemoryC')
INSERT [dbo].[DailyTasks] ([ID], [SourceTableName], [SourceAggregation], [SourceStatName], [DestinationTableName], [DestinationStatName], [QueryType]) VALUES (187, N'MicrosoftDailyStats', N'AVG', N'EDGE@Submission#Queues', N'MicrosoftSummaryStats', N'EDGE@Submission#Queues', 1)
INSERT [dbo].[DailyTasks] ([ID], [SourceTableName], [SourceAggregation], [SourceStatName], [DestinationTableName], [DestinationStatName], [QueryType]) VALUES (188, N'MicrosoftDailyStats', N'AVG', N'ResponseTime', N'MicrosoftSummaryStats', N'ResponseTime', 1)
INSERT [dbo].[DailyTasks] ([ID], [SourceTableName], [SourceAggregation], [SourceStatName], [DestinationTableName], [DestinationStatName], [QueryType]) VALUES (189, N'MicrosoftDailyStats', N'AVG', N'HUB@Submission#Queues', N'MicrosoftSummaryStats', N'HUB@Submission#Queues', 1)
INSERT [dbo].[DailyTasks] ([ID], [SourceTableName], [SourceAggregation], [SourceStatName], [DestinationTableName], [DestinationStatName], [QueryType]) VALUES (190, N'MicrosoftDailyStats', N'SUM', N'CAS@RPCClient#User.Count', N'MicrosoftSummaryStats', N'CAS@RPCClient#User.Count', 1)
INSERT [dbo].[DailyTasks] ([ID], [SourceTableName], [SourceAggregation], [SourceStatName], [DestinationTableName], [DestinationStatName], [QueryType]) VALUES (191, N'MicrosoftDailyStats', N'SUM', N'CAS@OWAClient#User.Count', N'MicrosoftSummaryStats', N'CAS@OWAClient#User.Count', 1)
INSERT [dbo].[DailyTasks] ([ID], [SourceTableName], [SourceAggregation], [SourceStatName], [DestinationTableName], [DestinationStatName], [QueryType]) VALUES (192, N'MicrosoftDailyStats', N'AVG', N'Mem.PercentAvailable', N'MicrosoftSummaryStats', N'Mem.PercentAvailable', 1)
INSERT [dbo].[DailyTasks] ([ID], [SourceTableName], [SourceAggregation], [SourceStatName], [DestinationTableName], [DestinationStatName], [QueryType]) VALUES (193, N'MicrosoftDailyStats', N'Max', N'Platform.System.PctCombinedCpuUtil', N'MicrosoftSummaryStats', N'Platform.System.PctCombinedCpuUtil', 1)
INSERT [dbo].[DailyTasks] ([ID], [SourceTableName], [SourceAggregation], [SourceStatName], [DestinationTableName], [DestinationStatName], [QueryType]) VALUES (194, N'DominoDailyStats', N'SUM', N'Traveler.IncrementalDeviceSyncs', N'DominoSummaryStats', N'TotalDeviceSyncs', 1)
INSERT [dbo].[DailyTasks] ([ID], [SourceTableName], [SourceAggregation], [SourceStatName], [DestinationTableName], [DestinationStatName], [QueryType]) VALUES (195, N'MicrosoftDailyStats', N'AVG', N'Lync@ChatLatency', N'MicrosoftSummaryStats', N'Lync@ChatLatency', 1)
INSERT [dbo].[DailyTasks] ([ID], [SourceTableName], [SourceAggregation], [SourceStatName], [DestinationTableName], [DestinationStatName], [QueryType]) VALUES (196, N'MicrosoftDailyStats', N'AVG', N'Lync@GroupChatLatency', N'MicrosoftSummaryStats', N'Lync@GroupChatLatency', 1)
INSERT [dbo].[DailyTasks] ([ID], [SourceTableName], [SourceAggregation], [SourceStatName], [DestinationTableName], [DestinationStatName], [QueryType]) VALUES (197, N'MicrosoftDailyStats', N'SUM', N'Lync@FEConnections', N'MicrosoftSummaryStats', N'Lync@FEConnections', 1)
INSERT [dbo].[DailyTasks] ([ID], [SourceTableName], [SourceAggregation], [SourceStatName], [DestinationTableName], [DestinationStatName], [QueryType]) VALUES (198, N'MicrosoftDailyStats', N'SUM', N'Lync@LyncEnabledUsers', N'MicrosoftSummaryStats', N'Lync@LyncEnabledUsers', 1)
INSERT [dbo].[DailyTasks] ([ID], [SourceTableName], [SourceAggregation], [SourceStatName], [DestinationTableName], [DestinationStatName], [QueryType]) VALUES (199, N'MicrosoftDailyStats', N'SUM', N'Lync@UsersConnected', N'MicrosoftSummaryStats', N'Lync@UsersConnected', 1)
INSERT [dbo].[DailyTasks] ([ID], [SourceTableName], [SourceAggregation], [SourceStatName], [DestinationTableName], [DestinationStatName], [QueryType]) VALUES (200, N'MicrosoftDailyStats', N'SUM', N'Lync@ClientVersionsConnected', N'MicrosoftSummaryStats', N'Lync@ClientVersionsConnected', 1)
INSERT [dbo].[DailyTasks] ([ID], [SourceTableName], [SourceAggregation], [SourceStatName], [DestinationTableName], [DestinationStatName], [QueryType]) VALUES (201, N'MicrosoftDailyStats', N'SUM', N'Lync@VoiceEnabledUsers', N'MicrosoftSummaryStats', N'Lync@VoiceEnabledUsers', 1)
INSERT [dbo].[DailyTasks] ([ID], [SourceTableName], [SourceAggregation], [SourceStatName], [DestinationTableName], [DestinationStatName], [QueryType]) VALUES (202, N'MicrosoftDailyStats', N'AVG', N'Disk.C:.Free', N'MicrosoftSummaryStats', N'Disk.C:', 1)
INSERT [dbo].[DailyTasks] ([ID], [SourceTableName], [SourceAggregation], [SourceStatName], [DestinationTableName], [DestinationStatName], [QueryType]) VALUES (203, N'MicrosoftDailyStats', N'AVG', N'Disk.E:.Free', N'MicrosoftSummaryStats', N'Disk.E:', 1)
INSERT [dbo].[DailyTasks] ([ID], [SourceTableName], [SourceAggregation], [SourceStatName], [DestinationTableName], [DestinationStatName], [QueryType]) VALUES (204, N'MicrosoftDailyStats', N'AVG', N'Disk.F:.Free', N'MicrosoftSummaryStats', N'Disk.F:', 1)
INSERT [dbo].[DailyTasks] ([ID], [SourceTableName], [SourceAggregation], [SourceStatName], [DestinationTableName], [DestinationStatName], [QueryType]) VALUES (205, N'MicrosoftDailyStats', N'AVG', N'Disk.C:\MailBoxes\MB1\.Free', N'MicrosoftSummaryStats', N'Disk.C:\MailBoxes\MB1\', 1)
INSERT [dbo].[DailyTasks] ([ID], [SourceTableName], [SourceAggregation], [SourceStatName], [DestinationTableName], [DestinationStatName], [QueryType]) VALUES (206, N'MicrosoftDailyStats', N'AVG', N'Disk.C:\MailBoxes\MB2\.Free', N'MicrosoftSummaryStats', N'Disk.C:\MailBoxes\MB2\', 1)
INSERT [dbo].[DailyTasks] ([ID], [SourceTableName], [SourceAggregation], [SourceStatName], [DestinationTableName], [DestinationStatName], [QueryType]) VALUES (207, N'MicrosoftDailyStats', N'MAX', N'CAS@RPCClient#User.Count', N'MicrosoftSummaryStats', N'MaxCAS@RPCClient#User.Count', 1)
INSERT [dbo].[DailyTasks] ([ID], [SourceTableName], [SourceAggregation], [SourceStatName], [DestinationTableName], [DestinationStatName], [QueryType]) VALUES (208, N'MicrosoftDailyStats', N'MAX', N'CAS@OWAClient#User.Count', N'MicrosoftSummaryStats', N'MaxCAS@OWAClient#User.Count', 1)	
INSERT [dbo].[DailyTasks] ([ID], [SourceTableName], [SourceAggregation], [SourceStatName], [DestinationTableName], [DestinationStatName], [QueryType]) VALUES (209, N'MicrosoftDailyStats', N'AVG', N'CAS@RPCClient#User.Count', N'MicrosoftSummaryStats', N'AvgCAS@RPCClient#User.Count', 1)
INSERT [dbo].[DailyTasks] ([ID], [SourceTableName], [SourceAggregation], [SourceStatName], [DestinationTableName], [DestinationStatName], [QueryType]) VALUES (210, N'MicrosoftDailyStats', N'AVG', N'CAS@OWAClient#User.Count', N'MicrosoftSummaryStats', N'AvgCAS@OWAClient#User.Count', 1)
INSERT [dbo].[DailyTasks] ([ID], [SourceTableName], [SourceAggregation], [SourceStatName], [DestinationTableName], [DestinationStatName], [QueryType]) VALUES (211, N'MicrosoftDailyStats', N'MAX', N'CAS@ActiveSync#User.Count', N'MicrosoftSummaryStats', N'MaxCAS@ActiveSync#User.Count', 1)
INSERT [dbo].[DailyTasks] ([ID], [SourceTableName], [SourceAggregation], [SourceStatName], [DestinationTableName], [DestinationStatName], [QueryType]) VALUES (212, N'MicrosoftDailyStats', N'AVG', N'CAS@ActiveSync#User.Count', N'MicrosoftSummaryStats', N'AvgCAS@ActiveSync#User.Count', 1)
INSERT [dbo].[DailyTasks] ([ID], [SourceTableName], [SourceAggregation], [SourceStatName], [DestinationTableName], [DestinationStatName], [QueryType]) VALUES (213, N'MicrosoftDailyStats', N'AVG', N'CAS@Shadow#Queues', N'MicrosoftSummaryStats', N'CAS@Shadow#Queues', 1)
INSERT [dbo].[DailyTasks] ([ID], [SourceTableName], [SourceAggregation], [SourceStatName], [DestinationTableName], [DestinationStatName], [QueryType]) VALUES (214, N'MicrosoftDailyStats', N'AVG', N'CAS@Submission#Queues', N'MicrosoftSummaryStats', N'CAS@Submission#Queues', 1)
INSERT [dbo].[DailyTasks] ([ID], [SourceTableName], [SourceAggregation], [SourceStatName], [DestinationTableName], [DestinationStatName], [QueryType]) VALUES (215, N'MicrosoftDailyStats', N'AVG', N'CAS@Unreachable#Queues', N'MicrosoftSummaryStats', N'CAS@Unreachable#Queues', 1)
INSERT [dbo].[DailyTasks] ([ID], [SourceTableName], [SourceAggregation], [SourceStatName], [DestinationTableName], [DestinationStatName], [QueryType]) VALUES (216, N'MicrosoftDailyStats', N'AVG', N'Hub@Shadow#Queues', N'MicrosoftSummaryStats', N'Hub@Shadow#Queues', 1)
INSERT [dbo].[DailyTasks] ([ID], [SourceTableName], [SourceAggregation], [SourceStatName], [DestinationTableName], [DestinationStatName], [QueryType]) VALUES (217, N'MicrosoftDailyStats', N'AVG', N'Hub@Unreachable#Queues', N'MicrosoftSummaryStats', N'Hub@Unreachable#Queues', 1)
INSERT [dbo].[DailyTasks] ([ID], [SourceTableName], [SourceAggregation], [SourceStatName], [DestinationTableName], [DestinationStatName], [QueryType]) VALUES (218, N'MicrosoftDailyStats', N'AVG', N'Mail_DeliverySuccessRate', N'MicrosoftSummaryStats', N'Mail_DeliverySuccessRate', 1)
INSERT [dbo].[DailyTasks] ([ID], [SourceTableName], [SourceAggregation], [SourceStatName], [DestinationTableName], [DestinationStatName], [QueryType]) VALUES (219, N'MicrosoftDailyStats', N'AVG', N'Mem.PercentUsed', N'MicrosoftSummaryStats', N'Mem.PercentUsed', 1)
INSERT [dbo].[DailyTasks] ([ID], [SourceTableName], [SourceAggregation], [SourceStatName], [DestinationTableName], [DestinationStatName], [QueryType]) VALUES (220, N'MicrosoftDailyStats', N'AVG', N'NetworkBytesReceivedPerSec', N'MicrosoftSummaryStats', N'NetworkBytesReceivedPerSec', 1)
INSERT [dbo].[DailyTasks] ([ID], [SourceTableName], [SourceAggregation], [SourceStatName], [DestinationTableName], [DestinationStatName], [QueryType]) VALUES (221, N'MicrosoftDailyStats', N'AVG', N'NetworkBytesSentPerSec', N'MicrosoftSummaryStats', N'NetworkBytesSentPerSec', 1)
INSERT [dbo].[DailyTasks] ([ID], [SourceTableName], [SourceAggregation], [SourceStatName], [DestinationTableName], [DestinationStatName], [QueryType]) VALUES (222, N'MicrosoftDailyStats', N'AVG', N'SP@ActiveSessions', N'MicrosoftSummaryStats', N'SP@ActiveSessions', 1)
INSERT [dbo].[DailyTasks] ([ID], [SourceTableName], [SourceAggregation], [SourceStatName], [DestinationTableName], [DestinationStatName], [QueryType]) VALUES (223, N'MicrosoftDailyStats', N'AVG', N'SP@ActiveThreads', N'MicrosoftSummaryStats', N'SP@ActiveThreads', 1)
INSERT [dbo].[DailyTasks] ([ID], [SourceTableName], [SourceAggregation], [SourceStatName], [DestinationTableName], [DestinationStatName], [QueryType]) VALUES (224, N'MicrosoftDailyStats', N'AVG', N'SP@ApplicationRestarts', N'MicrosoftSummaryStats', N'SP@ApplicationRestarts', 1)
INSERT [dbo].[DailyTasks] ([ID], [SourceTableName], [SourceAggregation], [SourceStatName], [DestinationTableName], [DestinationStatName], [QueryType]) VALUES (225, N'MicrosoftDailyStats', N'AVG', N'SP@CurPageReq', N'MicrosoftSummaryStats', N'SP@CurPageReq', 1)
INSERT [dbo].[DailyTasks] ([ID], [SourceTableName], [SourceAggregation], [SourceStatName], [DestinationTableName], [DestinationStatName], [QueryType]) VALUES (226, N'MicrosoftDailyStats', N'AVG', N'SP@ExeQueries', N'MicrosoftSummaryStats', N'SP@ExeQueries', 1)
INSERT [dbo].[DailyTasks] ([ID], [SourceTableName], [SourceAggregation], [SourceStatName], [DestinationTableName], [DestinationStatName], [QueryType]) VALUES (227, N'MicrosoftDailyStats', N'AVG', N'SP@ExeTime', N'MicrosoftSummaryStats', N'SP@ExeTime', 1)
INSERT [dbo].[DailyTasks] ([ID], [SourceTableName], [SourceAggregation], [SourceStatName], [DestinationTableName], [DestinationStatName], [QueryType]) VALUES (228, N'MicrosoftDailyStats', N'AVG', N'SP@IncomingPageReq', N'MicrosoftSummaryStats', N'SP@IncomingPageReq', 1)
INSERT [dbo].[DailyTasks] ([ID], [SourceTableName], [SourceAggregation], [SourceStatName], [DestinationTableName], [DestinationStatName], [QueryType]) VALUES (229, N'MicrosoftDailyStats', N'AVG', N'SP@QueuedRequests', N'MicrosoftSummaryStats', N'SP@QueuedRequests', 1)
INSERT [dbo].[DailyTasks] ([ID], [SourceTableName], [SourceAggregation], [SourceStatName], [DestinationTableName], [DestinationStatName], [QueryType]) VALUES (230, N'MicrosoftDailyStats', N'AVG', N'SP@RejectedRequests', N'MicrosoftSummaryStats', N'SP@RejectedRequests', 1)
INSERT [dbo].[DailyTasks] ([ID], [SourceTableName], [SourceAggregation], [SourceStatName], [DestinationTableName], [DestinationStatName], [QueryType]) VALUES (231, N'MicrosoftDailyStats', N'AVG', N'SP@RejPageReq', N'MicrosoftSummaryStats', N'SP@RejPageReq', 1)
INSERT [dbo].[DailyTasks] ([ID], [SourceTableName], [SourceAggregation], [SourceStatName], [DestinationTableName], [DestinationStatName], [QueryType]) VALUES (232, N'MicrosoftDailyStats', N'AVG', N'SP@RequestExecutionTime', N'MicrosoftSummaryStats', N'SP@RequestExecutionTime', 1)
INSERT [dbo].[DailyTasks] ([ID], [SourceTableName], [SourceAggregation], [SourceStatName], [DestinationTableName], [DestinationStatName], [QueryType]) VALUES (233, N'MicrosoftDailyStats', N'AVG', N'SP@RequestsPerSecond', N'MicrosoftSummaryStats', N'SP@RequestsPerSecond', 1)
INSERT [dbo].[DailyTasks] ([ID], [SourceTableName], [SourceAggregation], [SourceStatName], [DestinationTableName], [DestinationStatName], [QueryType]) VALUES (234, N'MicrosoftDailyStats', N'AVG', N'SP@RequestsRejected', N'MicrosoftSummaryStats', N'SP@RequestsRejected', 1)
INSERT [dbo].[DailyTasks] ([ID], [SourceTableName], [SourceAggregation], [SourceStatName], [DestinationTableName], [DestinationStatName], [QueryType]) VALUES (235, N'MicrosoftDailyStats', N'AVG', N'SP@RequestWaitTime', N'MicrosoftSummaryStats', N'SP@RequestWaitTime', 1)
INSERT [dbo].[DailyTasks] ([ID], [SourceTableName], [SourceAggregation], [SourceStatName], [DestinationTableName], [DestinationStatName], [QueryType]) VALUES (236, N'MicrosoftDailyStats', N'AVG', N'SP@RPRR', N'MicrosoftSummaryStats', N'SP@RPRR', 1)
INSERT [dbo].[DailyTasks] ([ID], [SourceTableName], [SourceAggregation], [SourceStatName], [DestinationTableName], [DestinationStatName], [QueryType]) VALUES (237, N'MicrosoftDailyStats', N'AVG', N'SP@SQLQueryExeTime', N'MicrosoftSummaryStats', N'SP@SQLQueryExeTime', 1)
INSERT [dbo].[DailyTasks] ([ID], [SourceTableName], [SourceAggregation], [SourceStatName], [DestinationTableName], [DestinationStatName], [QueryType]) VALUES (238, N'MicrosoftDailyStats', N'AVG', N'SP@TotalBytesReceivedPerSec', N'MicrosoftSummaryStats', N'SP@TotalBytesReceivedPerSec', 1)
INSERT [dbo].[DailyTasks] ([ID], [SourceTableName], [SourceAggregation], [SourceStatName], [DestinationTableName], [DestinationStatName], [QueryType]) VALUES (239, N'MicrosoftDailyStats', N'AVG', N'SP@TotalBytesSentPerSec', N'MicrosoftSummaryStats', N'SP@TotalBytesSentPerSec', 1)
INSERT [dbo].[DailyTasks] ([ID], [SourceTableName], [SourceAggregation], [SourceStatName], [DestinationTableName], [DestinationStatName], [QueryType]) VALUES (240, N'MicrosoftDailyStats', N'AVG', N'SP@TotalRequests', N'MicrosoftSummaryStats', N'SP@TotalRequests', 1)
INSERT [dbo].[DailyTasks] ([ID], [SourceTableName], [SourceAggregation], [SourceStatName], [DestinationTableName], [DestinationStatName], [QueryType]) VALUES (241, N'MicrosoftDailyStats', N'AVG', N'SP@WebServiceConnectionAttempts', N'MicrosoftSummaryStats', N'SP@WebServiceConnectionAttempts', 1)
INSERT [dbo].[DailyTasks] ([ID], [SourceTableName], [SourceAggregation], [SourceStatName], [DestinationTableName], [DestinationStatName], [QueryType]) VALUES (242, N'MicrosoftDailyStats', N'AVG', N'SP@WorkerProcessRestarts', N'MicrosoftSummaryStats', N'SP@WorkerProcessRestarts', 1)
INSERT [dbo].[DailyTasks] ([ID], [SourceTableName], [SourceAggregation], [SourceStatName], [DestinationTableName], [DestinationStatName], [QueryType]) VALUES (243, N'MicrosoftDailyStats', N'SUM', N'Mail_DeliverCount', N'MicrosoftSummaryStats', N'Mail_DeliverCount', 1)
INSERT [dbo].[DailyTasks] ([ID], [SourceTableName], [SourceAggregation], [SourceStatName], [DestinationTableName], [DestinationStatName], [QueryType]) VALUES (244, N'MicrosoftDailyStats', N'SUM', N'Mail_FailCount', N'MicrosoftSummaryStats', N'Mail_FailCount', 1)
INSERT [dbo].[DailyTasks] ([ID], [SourceTableName], [SourceAggregation], [SourceStatName], [DestinationTableName], [DestinationStatName], [QueryType]) VALUES (245, N'MicrosoftDailyStats', N'SUM', N'Mail_ReceivedCount', N'MicrosoftSummaryStats', N'Mail_ReceivedCount', 1)
INSERT [dbo].[DailyTasks] ([ID], [SourceTableName], [SourceAggregation], [SourceStatName], [DestinationTableName], [DestinationStatName], [QueryType]) VALUES (246, N'MicrosoftDailyStats', N'SUM', N'Mail_ReceivedSizeMB', N'MicrosoftSummaryStats', N'Mail_ReceivedSizeMB', 1)
INSERT [dbo].[DailyTasks] ([ID], [SourceTableName], [SourceAggregation], [SourceStatName], [DestinationTableName], [DestinationStatName], [QueryType]) VALUES (247, N'MicrosoftDailyStats', N'SUM', N'Mail_SentCount', N'MicrosoftSummaryStats', N'Mail_SentCount', 1)
INSERT [dbo].[DailyTasks] ([ID], [SourceTableName], [SourceAggregation], [SourceStatName], [DestinationTableName], [DestinationStatName], [QueryType]) VALUES (248, N'MicrosoftDailyStats', N'SUM', N'Mail_SentSizeMB', N'MicrosoftSummaryStats', N'Mail_SentSizeMB', 1)
INSERT [dbo].[DailyTasks] ([ID], [SourceTableName], [SourceAggregation], [SourceStatName], [DestinationTableName], [DestinationStatName], [QueryType]) VALUES (249, N'MicrosoftDailyStats', N'AVG', N'AD@QueryLatency', N'MicrosoftSummaryStats', N'AD@QueryLatency', 1)
INSERT [dbo].[DailyTasks] ([ID], [SourceTableName], [SourceAggregation], [SourceStatName], [DestinationTableName], [DestinationStatName], [QueryType]) VALUES (250, N'MicrosoftDailyStats', N'SUM', N'CAS@ActiveSync#User.Count', N'MicrosoftSummaryStats', N'CAS@ActiveSync#User.Count', 1)
/*2/19/2016 Durga Added for VSPLUS-2174*/
INSERT [dbo].[DailyTasks] ([ID], [SourceTableName], [SourceAggregation], [SourceStatName], [DestinationTableName], [DestinationStatName], [QueryType]) VALUES (251, N'DominoDailyStats', N'AVG', N'Http.CurrentConnections', N'DominoSummaryStats', N'HTTP sessions', 1)
/* 3/15/16 WS Added for Connections */
INSERT [dbo].[DailyTasks] ([ID], [SourceTableName], [SourceAggregation], [SourceStatName], [DestinationTableName], [DestinationStatName], [QueryType]) VALUES (252, N'IbmConnectionsDailyStats', N'AVG', N'Create.Blog.TimeMs', N'IbmConnectionsSummaryStats', N'Create.Blog.TimeMs', 1)
INSERT [dbo].[DailyTasks] ([ID], [SourceTableName], [SourceAggregation], [SourceStatName], [DestinationTableName], [DestinationStatName], [QueryType]) VALUES (253, N'IbmConnectionsDailyStats', N'AVG', N'Create.Bookmark.TimeMs', N'IbmConnectionsSummaryStats', N'Create.Bookmark.TimeMs', 1)
INSERT [dbo].[DailyTasks] ([ID], [SourceTableName], [SourceAggregation], [SourceStatName], [DestinationTableName], [DestinationStatName], [QueryType]) VALUES (254, N'IbmConnectionsDailyStats', N'AVG', N'Create.Community.TimeMs', N'IbmConnectionsSummaryStats', N'Create.Community.TimeMs', 1)
INSERT [dbo].[DailyTasks] ([ID], [SourceTableName], [SourceAggregation], [SourceStatName], [DestinationTableName], [DestinationStatName], [QueryType]) VALUES (255, N'IbmConnectionsDailyStats', N'AVG', N'Create.File.TimeMs', N'IbmConnectionsSummaryStats', N'Create.File.TimeMs', 1)
INSERT [dbo].[DailyTasks] ([ID], [SourceTableName], [SourceAggregation], [SourceStatName], [DestinationTableName], [DestinationStatName], [QueryType]) VALUES (256, N'IbmConnectionsDailyStats', N'AVG', N'Create.Wiki.TimeMs', N'IbmConnectionsSummaryStats', N'Create.Wiki.TimeMs', 1)
INSERT [dbo].[DailyTasks] ([ID], [SourceTableName], [SourceAggregation], [SourceStatName], [DestinationTableName], [DestinationStatName], [QueryType]) VALUES (257, N'IbmConnectionsDailyStats', N'AVG', N'Search.Profile.TimeMs', N'IbmConnectionsSummaryStats', N'Search.Profile.TimeMs', 1)
INSERT [dbo].[DailyTasks] ([ID], [SourceTableName], [SourceAggregation], [SourceStatName], [DestinationTableName], [DestinationStatName], [QueryType]) VALUES (258, N'IbmConnectionsDailyStats', N'AVG', N'Create.Activity.TimeMs', N'IbmConnectionsSummaryStats', N'Create.Activity.TimeMs', 1)
INSERT [dbo].[DailyTasks] ([ID], [SourceTableName], [SourceAggregation], [SourceStatName], [DestinationTableName], [DestinationStatName], [QueryType]) VALUES (259, N'IbmConnectionsDailyStats', N'AVG', N'ResponseTime', N'IbmConnectionsSummaryStats', N'ResponseTime', 1)
INSERT [dbo].[DailyTasks] ([ID], [SourceTableName], [SourceAggregation], [SourceStatName], [DestinationTableName], [DestinationStatName], [QueryType]) VALUES (260, N'IbmConnectionsDailyStats', N'AVG', N'Create.Forum.TimeMs', N'IbmConnectionsSummaryStats', N'Create.Forum.TimeMs', 1)
SET IDENTITY_INSERT [dbo].[DailyTasks] OFF
--createing ApplServerSettings menu item
/****** Object:  Table [dbo].[Network Master]    Script Date: 12/05/2013 18:44:56 ******/


--12/12/2013 NS added a new menu item to set server settings in bulk
USE [vitalsigns]
GO
Delete from MenuItems where id=59
Go
IF OBJECTPROPERTY(OBJECT_ID('dbo.MenuItems'), 'TableHasIdentity') = 1 
BEGIN
	SET IDENTITY_INSERT [dbo].[MenuItems] ON
END 
INSERT [dbo].[MenuItems] ([ID], [DisplayText], [OrderNum], [ParentID], [PageLink], [Level], [RefName], [ImageURL]) VALUES (59, N'Server Settings Editor', 7, 2, N'/Security/ApplyServerSettings.aspx', 2, N'ApplyServerSettings', N'')
IF OBJECTPROPERTY(OBJECT_ID('dbo.MenuItems'), 'TableHasIdentity') = 1 
BEGIN
	SET IDENTITY_INSERT [dbo].[MenuItems] OFF
END 

IF NOT EXISTS (select * from syscolumns where id = object_id('dbo.ProfilesMaster'))
BEGIN
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
	[isSelected] [bit]  NULL,
	) ON [PRIMARY]
END
GO

/* 1201 Niranjan */
USE [vitalsigns]
GO
IF NOT EXISTS (select * from syscolumns where id = object_id('dbo.ProfileNames'))
BEGIN

SET ANSI_NULLS ON

SET QUOTED_IDENTIFIER ON

CREATE TABLE [dbo].[ProfileNames](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[ProfileName] [nvarchar](20) NULL,
 CONSTRAINT [PK_ProfileNames] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO

USE [vitalsigns]
GO
IF NOT EXISTS (select * from syscolumns where name= 'DatabaseTitle' and id = object_id('dbo.ClusterDatabaseDetails'))
BEGIN	
	ALTER TABLE dbo.ClusterDatabaseDetails ADD DatabaseTitle [nvarchar] (250) null
END
GO	
USE [vitalsigns]
GO
/* 1201 Niranjan*/
IF NOT EXISTS (select * from syscolumns where name= 'ProfileName' and id = object_id('dbo.Servers'))
BEGIN 
 ALTER TABLE dbo.Servers ADD ProfileName [nvarchar] (20) null
END
GO
USE [vitalsigns]
GO
IF NOT EXISTS (select * from syscolumns where name= 'RoleType' and id = object_id('dbo.ProfilesMaster'))
BEGIN	
	ALTER TABLE dbo.ProfilesMaster ADD RoleType [nvarchar] (100) null
END
GO
/* 1201 Niranjan*/

USE [vitalsigns]
GO
IF NOT EXISTS (select * from syscolumns where name= 'ProfileId' and id = object_id('dbo.ProfilesMaster'))
BEGIN 
 ALTER TABLE dbo.ProfilesMaster ADD ProfileId [int]  null
END
GO

USE [vitalsigns]
GO
IF NOT EXISTS (select * from syscolumns where name= 'isSelected' and id = object_id('dbo.ProfilesMaster'))
BEGIN 
 ALTER TABLE dbo.ProfilesMaster ADD isSelected [bit]  null
END
GO	

IF NOT EXISTS (select * from syscolumns where id = object_id('dbo.UserProfileMaster'))
BEGIN
	CREATE TABLE [dbo].[UserProfileMaster](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NULL
	) ON [PRIMARY]
END
GO


IF NOT EXISTS (select * from syscolumns where id = object_id('dbo.UserProfileDetailed'))
BEGIN
	CREATE TABLE [dbo].[UserProfileDetailed](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[UserProfileMasterID] [int] NULL,
	[ProfilesMasterID] [int] NULL,
	[Value] [nvarchar](100) NULL
	) ON [PRIMARY]
END
GO
--Swathi 1058--





USE [VSS_Statistics]
GO
/****** Object:  Table [dbo].[DeviceDailyStats]    Script Date: 12/06/2013 22:10:56 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

ALTER TABLE [dbo].[Daily] ALTER COLUMN [Title] [nvarchar](200) NULL
GO

ALTER TABLE [dbo].[Daily] ALTER COLUMN [FileName] [nvarchar](200) NULL
GO

ALTER TABLE [dbo].[Daily] ALTER COLUMN [Server] [nvarchar](200) NULL
GO


IF NOT EXISTS (select * from syscolumns where id = object_id('dbo.DeviceUpTimeSummaryStats'))
BEGIN
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
END
GO

USE [VSS_Statistics]
GO

/****** Object:  Table [dbo].[DeviceDailyStats]    Script Date: 12/06/2013 22:14:38 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS (select * from syscolumns where id = object_id('dbo.DeviceDailySummaryStats'))
BEGIN
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
END
GO


USE [VSS_Statistics]
GO

/****** Object:  Table [dbo].[BESDailyStats]    Script Date: 12/06/2013 22:17:55 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS (select * from syscolumns where id = object_id('dbo.BESDailySummaryStats'))
BEGIN
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
END
GO


USE [VSS_Statistics]
GO

/****** Object:  Table [dbo].[MicrosoftDailyStats]    Script Date: 12/06/2013 22:17:55 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

if NOT exists (select * from syscolumns where id = object_id('dbo.MicrosoftDailyStats'))
BEGIN
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
END
GO

USE [VSS_Statistics]
GO


SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO
if NOT exists (select * from syscolumns where id = object_id('dbo.MicrosoftSummaryStats'))
BEGIN
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
END
GO

USE [vitalsigns]
GO
IF EXISTS(SELECT * FROM syscolumns where id=Object_id('dailytasks'))
BEGIN
	UPDATE dailytasks SET destinationtablename = 'DeviceDailySummaryStats' WHERE destinationtablename ='DeviceDailyStats'
	UPDATE dailytasks SET destinationtablename = 'DeviceUpTimeSummaryStats' WHERE destinationtablename ='DeviceUpTimeStats' 
END
GO


USE [VSS_Statistics]
GO
IF NOT EXISTS (select * from syscolumns where id = object_id('dbo.ScanResults'))
BEGIN
	CREATE TABLE [dbo].[ScanResults](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[ScanDate] [datetime] NULL,
	[ServerName] [nvarchar](250) NULL,
	[ScanCount] [int] NULL,
	[DatabaseCount] [int] NULL)
END
GO

IF NOT EXISTS (select * from syscolumns where name= 'Temp' and id = object_id('dbo.Daily'))
BEGIN
	Alter TABLE [dbo].[Daily] Add Temp Bit NULL
END

PRINT N'Executing V1.1.1 upgrade scripts .........';


/*************************Start of ProfilesMaster Added data for URL, Sametime, etc., ********************************************/
GO
USE [vitalsigns]
GO
IF NOT EXISTS (select * from ProfileNames where ProfileName='Default' )
BEGIN
SET IDENTITY_INSERT [dbo].[ProfileNames] ON

INSERT [dbo].[ProfileNames]([ID], [ProfileName]) VALUES (0, 'Default')

SET IDENTITY_INSERT [dbo].[ProfileNames] OFF

END
USE [vitalsigns]
GO

Delete from [dbo].[ProfilesMaster] 
GO

																						
/*Mukund: 03 Feb 14 added for VSPLUS-197 - Exchange Service UI.*/
/****** Object:  Table [dbo].[ProfilesMaster]    Script Date: 12/10/2013 16:37:00 ******/

SET IDENTITY_INSERT [dbo].[ProfilesMaster] ON
INSERT [dbo].[ProfilesMaster] ([ID], [ServerTypeId], [AttributeName], [DefaultValue], [UnitOfMeasurement], [RelatedTable], [RelatedField], [RoleType],[ProfileId],[isSelected]) VALUES (146, 18, N'Enabled', N'1', N'True/False (1- True, 0-False', N'ServerAttributes', N'Enabled', NULL,0,0)
INSERT [dbo].[ProfilesMaster] ([ID], [ServerTypeId], [AttributeName], [DefaultValue], [UnitOfMeasurement], [RelatedTable], [RelatedField], [RoleType],[ProfileId],[isSelected]) VALUES (147, 4, N'Scan Interval', N'8', N'Minutes', N'ServerAttributes', N'ScanInterval', N'',0,0)
INSERT [dbo].[ProfilesMaster] ([ID], [ServerTypeId], [AttributeName], [DefaultValue], [UnitOfMeasurement], [RelatedTable], [RelatedField], [RoleType],[ProfileId],[isSelected]) VALUES (148, 4, N'Retry Interval', N'3', N'Minutes', N'ServerAttributes', N'RetryInterval', N'',0,0)

INSERT [dbo].[ProfilesMaster] ([ID], [ServerTypeId], [AttributeName], [DefaultValue], [UnitOfMeasurement], [RelatedTable], [RelatedField], [RoleType],[ProfileId],[isSelected]) VALUES (149, 4, N'Off Hours Scan Interval', N'10', N'Minutes', N'ServerAttributes', N'OffHourInterval', N'',0,0)
INSERT [dbo].[ProfilesMaster] ([ID], [ServerTypeId], [AttributeName], [DefaultValue], [UnitOfMeasurement], [RelatedTable], [RelatedField], [RoleType],[ProfileId],[isSelected]) VALUES (150, 4, N'Response Threshold', N'200', N'milliseconds', N'ServerAttributes', N'ResponseTime', N'',0,0)
INSERT [dbo].[ProfilesMaster] ([ID], [ServerTypeId], [AttributeName], [DefaultValue], [UnitOfMeasurement], [RelatedTable], [RelatedField], [RoleType],[ProfileId],[isSelected]) VALUES (151, 4, N'Failure Threshold', N'2', N'Consecutive Failures', N'ServerAttributes', N'ConsFailuresBefAlert', N'',0,0)
INSERT [dbo].[ProfilesMaster] ([ID], [ServerTypeId], [AttributeName], [DefaultValue], [UnitOfMeasurement], [RelatedTable], [RelatedField], [RoleType],[ProfileId],[isSelected]) VALUES (152, 6, N'Scan Interval', N'8', N'Minutes', N'DominoServers', N'ScanInterval', N'',0,0)
INSERT [dbo].[ProfilesMaster] ([ID], [ServerTypeId], [AttributeName], [DefaultValue], [UnitOfMeasurement], [RelatedTable], [RelatedField], [RoleType],[ProfileId],[isSelected]) VALUES (153, 6, N'Retry Interval', N'4', N'Minutes', N'DominoServers', N'RetryInterval', N'',0,0)
INSERT [dbo].[ProfilesMaster] ([ID], [ServerTypeId], [AttributeName], [DefaultValue], [UnitOfMeasurement], [RelatedTable], [RelatedField], [RoleType],[ProfileId],[isSelected]) VALUES (154, 6, N'Off-Hours Scan Interval', N'30', N'Minutes', N'DominoServers', N'OffHourInterval', N'',0,0)
INSERT [dbo].[ProfilesMaster] ([ID], [ServerTypeId], [AttributeName], [DefaultValue], [UnitOfMeasurement], [RelatedTable], [RelatedField], [RoleType],[ProfileId],[isSelected]) VALUES (155, 6, N'Delivery Threshold', N'5', N'Minutes', N'DominoServers', N'DeliveryThreshold', N'',0,0)
INSERT [dbo].[ProfilesMaster] ([ID], [ServerTypeId], [AttributeName], [DefaultValue], [UnitOfMeasurement], [RelatedTable], [RelatedField], [RoleType],[ProfileId],[isSelected]) VALUES (156, 9, N'Scan Interval', N'1', N'Minutes', N'DominoServers', N'ScanInterval', N'',0,0)
INSERT [dbo].[ProfilesMaster] ([ID], [ServerTypeId], [AttributeName], [DefaultValue], [UnitOfMeasurement], [RelatedTable], [RelatedField], [RoleType],[ProfileId],[isSelected]) VALUES (157, 9, N'Retry Interval', N'4', N'Minutes', N'DominoServers', N'RetryInterval', N'',0,0)
INSERT [dbo].[ProfilesMaster] ([ID], [ServerTypeId], [AttributeName], [DefaultValue], [UnitOfMeasurement], [RelatedTable], [RelatedField], [RoleType],[ProfileId],[isSelected]) VALUES (158, 9, N'Off-Hours Scan Interval', N'30', N'Minutes', N'DominoServers', N'OffHourInterval', N'',0,0)
INSERT [dbo].[ProfilesMaster] ([ID], [ServerTypeId], [AttributeName], [DefaultValue], [UnitOfMeasurement], [RelatedTable], [RelatedField], [RoleType],[ProfileId],[isSelected]) VALUES (159, 20, N'Scan Interval', N'8', N'Minutes', N'ServerAttributes', N'ScanInterval', N'',0,0)
INSERT [dbo].[ProfilesMaster] ([ID], [ServerTypeId], [AttributeName], [DefaultValue], [UnitOfMeasurement], [RelatedTable], [RelatedField], [RoleType],[ProfileId],[isSelected]) VALUES (160, 20, N'Retry Interval', N'4', N'Minutes', N'ServerAttributes', N'RetryInterval', N'',0,0)
INSERT [dbo].[ProfilesMaster] ([ID], [ServerTypeId], [AttributeName], [DefaultValue], [UnitOfMeasurement], [RelatedTable], [RelatedField], [RoleType],[ProfileId],[isSelected]) VALUES (161, 20, N'Off-Hours Scan Interval', N'30', N'Minutes', N'ServerAttributes', N'OffHourInterval', N'',0,0)
INSERT [dbo].[ProfilesMaster] ([ID], [ServerTypeId], [AttributeName], [DefaultValue], [UnitOfMeasurement], [RelatedTable], [RelatedField], [RoleType],[ProfileId],[isSelected]) VALUES (162, 20, N'Response Threshold', N'200', N'milliseconds', N'ServerAttributes', N'ResponseTime', N'',0,0)
INSERT [dbo].[ProfilesMaster] ([ID], [ServerTypeId], [AttributeName], [DefaultValue], [UnitOfMeasurement], [RelatedTable], [RelatedField], [RoleType],[ProfileId],[isSelected]) VALUES (163, 12, N'Scan Interval', N'8', N'Minutes', N'DominoServers', N'ScanInterval', N'',0,0)
INSERT [dbo].[ProfilesMaster] ([ID], [ServerTypeId], [AttributeName], [DefaultValue], [UnitOfMeasurement], [RelatedTable], [RelatedField], [RoleType],[ProfileId],[isSelected]) VALUES (164, 12, N'Off-Hours Scan Interval', N'30', N'Minutes', N'DominoServers', N'OffHourInterval', N'',0,0)
INSERT [dbo].[ProfilesMaster] ([ID], [ServerTypeId], [AttributeName], [DefaultValue], [UnitOfMeasurement], [RelatedTable], [RelatedField], [RoleType],[ProfileId],[isSelected]) VALUES (166, 4, N'Memory Threshold', N'.9', N'Percentage Used (eg:- for 90% = 0.90)', N'ServerAttributes', N'MemoryThreshold', N'',0,0)
INSERT [dbo].[ProfilesMaster] ([ID], [ServerTypeId], [AttributeName], [DefaultValue], [UnitOfMeasurement], [RelatedTable], [RelatedField], [RoleType],[ProfileId],[isSelected]) VALUES (167, 4, N'CPU Threshold', N'.9', N'Percentage Used (eg:- for 90% = 0.90)', N'ServerAttributes', N'CPUThreshold', N'',0,0)
INSERT [dbo].[ProfilesMaster] ([ID], [ServerTypeId], [AttributeName], [DefaultValue], [UnitOfMeasurement], [RelatedTable], [RelatedField], [RoleType],[ProfileId],[isSelected]) VALUES (168, 12, N'Difference Threshold', N'0.9', N'Percentage Used (eg:- for 90% = 0.90)', N'DominoServers', N'Difference Threshold', N'',0,0)
INSERT [dbo].[ProfilesMaster] ([ID], [ServerTypeId], [AttributeName], [DefaultValue], [UnitOfMeasurement], [RelatedTable], [RelatedField], [RoleType],[ProfileId],[isSelected]) VALUES (169, 5, N'LatencyYellowThreshold', N'10', N'Seconds', N'ExchangeSettings', N'LatencyYellowThreshold', N'',0,0)
INSERT [dbo].[ProfilesMaster] ([ID], [ServerTypeId], [AttributeName], [DefaultValue], [UnitOfMeasurement], [RelatedTable], [RelatedField], [RoleType],[ProfileId],[isSelected]) VALUES (170, 5, N'LatencyRedThreshold', N'10', N'Seconds', N'ExchangeSettings', N'LatencyRedThreshold', N'',0,0)
--INSERT [dbo].[ProfilesMaster] ([ID], [ServerTypeId], [AttributeName], [DefaultValue], [UnitOfMeasurement], [RelatedTable], [RelatedField], [RoleType],[ProfileId],[isSelected]) VALUES (171, 5, N'Enabled For Latency Test', N'1', N'True/False (1- True, 0-False)', N'ExchangeSettings', N'Enabled', N'Mailbox',0,0)
INSERT [dbo].[ProfilesMaster] ([ID], [ServerTypeId], [AttributeName], [DefaultValue], [UnitOfMeasurement], [RelatedTable], [RelatedField], [RoleType],[ProfileId],[isSelected]) VALUES (172, 18, N'Category', N'Production', N'string', N'ServerAttributes', N'Category', N'',0,0)
INSERT [dbo].[ProfilesMaster] ([ID], [ServerTypeId], [AttributeName], [DefaultValue], [UnitOfMeasurement], [RelatedTable], [RelatedField], [RoleType],[ProfileId],[isSelected]) VALUES (173, 18, N'Enabled for Scanning', N'1', N'True/False (1- True, 0-False)', N'ServerAttributes', N'Enabled', N'',0,0)
INSERT [dbo].[ProfilesMaster] ([ID], [ServerTypeId], [AttributeName], [DefaultValue], [UnitOfMeasurement], [RelatedTable], [RelatedField] ,[RoleType],[ProfileId],[isSelected]) VALUES (7, 1, N'Off Hours Scan Interval', N'10', N'minutes', N'DominoServers', N'OffHoursScanInterval','',0,0)
INSERT [dbo].[ProfilesMaster] ([ID], [ServerTypeId], [AttributeName], [DefaultValue], [UnitOfMeasurement], [RelatedTable], [RelatedField] ,[RoleType],[ProfileId],[isSelected]) VALUES (8, 1, N'Scan Interval', N'8', N'Minutes', N'DominoServers', N'Scan Interval','',0,0)
INSERT [dbo].[ProfilesMaster] ([ID], [ServerTypeId], [AttributeName], [DefaultValue], [UnitOfMeasurement], [RelatedTable], [RelatedField] ,[RoleType],[ProfileId],[isSelected]) VALUES (9, 1, N'Response Time Threshold', N'2000', N'milliseconds', N'DominoServers', N'ResponseThreshold','',0,0)
INSERT [dbo].[ProfilesMaster] ([ID], [ServerTypeId], [AttributeName], [DefaultValue], [UnitOfMeasurement], [RelatedTable], [RelatedField] ,[RoleType],[ProfileId],[isSelected]) VALUES (11, 1, N'Pending Mail Threshold', N'100', N'Count', N'DominoServers', N'PendingThreshold','',0,0)
INSERT [dbo].[ProfilesMaster] ([ID], [ServerTypeId], [AttributeName], [DefaultValue], [UnitOfMeasurement], [RelatedTable], [RelatedField] ,[RoleType],[ProfileId],[isSelected]) VALUES (12, 1, N'Dead Mail Threshold', N'100', N'Count', N'DominoServers', N'DeadThreshold','',0,0)
INSERT [dbo].[ProfilesMaster] ([ID], [ServerTypeId], [AttributeName], [DefaultValue], [UnitOfMeasurement], [RelatedTable], [RelatedField] ,[RoleType],[ProfileId],[isSelected]) VALUES (13, 1, N'Enable for Scan', N'0', N'True/False (1- True, 0-False)', N'DominoServers', N'Enabled','',0,0)
INSERT [dbo].[ProfilesMaster] ([ID], [ServerTypeId], [AttributeName], [DefaultValue], [UnitOfMeasurement], [RelatedTable], [RelatedField] ,[RoleType],[ProfileId],[isSelected]) VALUES (14, 1, N'Retry Interval', N'2', N'Minutes', N'DominoServers', N'RetryInterval','',0,0)
INSERT [dbo].[ProfilesMaster] ([ID], [ServerTypeId], [AttributeName], [DefaultValue], [UnitOfMeasurement], [RelatedTable], [RelatedField] ,[RoleType],[ProfileId],[isSelected]) VALUES (15, 1, N'Failure Threshold', N'2', N'No of failures', N'DominoServers', N'FailureThreshold','',0,0)
--INSERT [dbo].[ProfilesMaster] ([ID], [ServerTypeId], [AttributeName], [DefaultValue], [UnitOfMeasurement], [RelatedTable], [RelatedField] ,[RoleType]) VALUES (16, 1, N'Disk Space Threshold', N'0.10', N'Percentage free (eg:- for 10% = 0.10)', N'DominoServers', N'DiskSpaceThreshold','')
INSERT [dbo].[ProfilesMaster] ([ID], [ServerTypeId], [AttributeName], [DefaultValue], [UnitOfMeasurement], [RelatedTable], [RelatedField] ,[RoleType],[ProfileId],[isSelected]) VALUES (17, 1, N'Dead Mail Delete Threshold', N'100', N'Count', N'DominoServers', N'DeadMailDeleteThreshold','',0,0)
INSERT [dbo].[ProfilesMaster] ([ID], [ServerTypeId], [AttributeName], [DefaultValue], [UnitOfMeasurement], [RelatedTable], [RelatedField] ,[RoleType],[ProfileId],[isSelected]) VALUES (18, 1, N'Held Mail Threshold', N'100', N'Count', N'DominoServers', N'HeldThreshold','',0,0)
INSERT [dbo].[ProfilesMaster] ([ID], [ServerTypeId], [AttributeName], [DefaultValue], [UnitOfMeasurement], [RelatedTable], [RelatedField] ,[RoleType],[ProfileId],[isSelected]) VALUES (19, 1, N'Memory Threshold', N'0.90', N'Percentage Used (eg:- for 90% = 0.90)', N'DominoServers', N'Memory_Threshold','',0,0)
INSERT [dbo].[ProfilesMaster] ([ID], [ServerTypeId], [AttributeName], [DefaultValue], [UnitOfMeasurement], [RelatedTable], [RelatedField] ,[RoleType],[ProfileId],[isSelected]) VALUES (20, 1, N'CPU Threshold', N'0.90', N'Percentage Used (eg:- for 90% = 0.90)', N'DominoServers', N'CPU_Threshold','',0,0)
INSERT [dbo].[ProfilesMaster] ([ID], [ServerTypeId], [AttributeName], [DefaultValue], [UnitOfMeasurement], [RelatedTable], [RelatedField] ,[RoleType],[ProfileId],[isSelected]) VALUES (21, 1, N'Cluster Replication Delays Threshold', N'10', N'Minutes', N'DominoServers', N'Cluster_Rep_Delays_Threshold','',0,0)
INSERT [dbo].[ProfilesMaster] ([ID], [ServerTypeId], [AttributeName], [DefaultValue], [UnitOfMeasurement], [RelatedTable], [RelatedField] ,[RoleType],[ProfileId],[isSelected]) VALUES (22, 3, N'Enabled', N'0', N'True/False (1- True, 0-False)', N'SametimeServers', N'Enabled','',0,0)
INSERT [dbo].[ProfilesMaster] ([ID], [ServerTypeId], [AttributeName], [DefaultValue], [UnitOfMeasurement], [RelatedTable], [RelatedField] ,[RoleType],[ProfileId],[isSelected]) VALUES (23, 3, N'Scan Interval', N'8', N'Minutes', N'SametimeServers', N'ScanInterval','',0,0)
INSERT [dbo].[ProfilesMaster] ([ID], [ServerTypeId], [AttributeName], [DefaultValue], [UnitOfMeasurement], [RelatedTable], [RelatedField] ,[RoleType],[ProfileId],[isSelected]) VALUES (24, 3, N'Off Hours Scan Interval', N'15', N'minutes', N'SametimeServers', N'OffHoursScanInterval','',0,0)
INSERT [dbo].[ProfilesMaster] ([ID], [ServerTypeId], [AttributeName], [DefaultValue], [UnitOfMeasurement], [RelatedTable], [RelatedField] ,[RoleType],[ProfileId],[isSelected]) VALUES (25, 3, N'Retry Interval', N'2', N'minutes', N'SametimeServers', N'RetryInterval','',0,0)
INSERT [dbo].[ProfilesMaster] ([ID], [ServerTypeId], [AttributeName], [DefaultValue], [UnitOfMeasurement], [RelatedTable], [RelatedField] ,[RoleType],[ProfileId],[isSelected]) VALUES (26, 3, N'Response Threshold', N'2000', N'milliseconds', N'SametimeServers', N'ResponseThreshold','',0,0)
INSERT [dbo].[ProfilesMaster] ([ID], [ServerTypeId], [AttributeName], [DefaultValue], [UnitOfMeasurement], [RelatedTable], [RelatedField] ,[RoleType],[ProfileId],[isSelected]) VALUES (27, 7, N'Scan Interval', N'8', N'minutes', N'URLs', N'ScanInterval','',0,0)
INSERT [dbo].[ProfilesMaster] ([ID], [ServerTypeId], [AttributeName], [DefaultValue], [UnitOfMeasurement], [RelatedTable], [RelatedField] ,[RoleType],[ProfileId],[isSelected]) VALUES (28, 7, N'Off Hours Scan Interval', N'15', N'minutes', N'URLs', N'OffHoursScanInterval','',0,0)
INSERT [dbo].[ProfilesMaster] ([ID], [ServerTypeId], [AttributeName], [DefaultValue], [UnitOfMeasurement], [RelatedTable], [RelatedField] ,[RoleType],[ProfileId],[isSelected]) VALUES (29, 7, N'Enabled', N'0', N'True/False (1- True, 0-False)', N'URLs', N'Enabled','',0,0)
INSERT [dbo].[ProfilesMaster] ([ID], [ServerTypeId], [AttributeName], [DefaultValue], [UnitOfMeasurement], [RelatedTable], [RelatedField] ,[RoleType],[ProfileId],[isSelected]) VALUES (30, 7, N'Response Threshold', N'2000', N'milliseconds', N'URLs', N'ResponseThreshold','',0,0)
INSERT [dbo].[ProfilesMaster] ([ID], [ServerTypeId], [AttributeName], [DefaultValue], [UnitOfMeasurement], [RelatedTable], [RelatedField] ,[RoleType],[ProfileId],[isSelected]) VALUES (31, 7, N'Retry Interval', N'2', N'minutes', N'URLs', N'RetryInterval','',0,0)
INSERT [dbo].[ProfilesMaster] ([ID], [ServerTypeId], [AttributeName], [DefaultValue], [UnitOfMeasurement], [RelatedTable], [RelatedField] ,[RoleType],[ProfileId],[isSelected]) VALUES (32, 5, N'Memory Threshold', N'0.90', N'Percentage Used (eg:- for 90% = 0.90)', N'ServerAttributes', N'MemThreshold','',0,0)
INSERT [dbo].[ProfilesMaster] ([ID], [ServerTypeId], [AttributeName], [DefaultValue], [UnitOfMeasurement], [RelatedTable], [RelatedField] ,[RoleType],[ProfileId],[isSelected]) VALUES (33, 5, N'Enabled', N'1', N'True/False (1- True, 0-False)', N'ServerAttributes', N'Enabled','',0,0)
--11-04-2016 Durga Modified for VSPLUS-2742
INSERT [dbo].[ProfilesMaster] ([ID], [ServerTypeId], [AttributeName], [DefaultValue], [UnitOfMeasurement], [RelatedTable], [RelatedField] ,[RoleType],[ProfileId],[isSelected]) VALUES (34, 5, N'Submission Queue Threshold', N'100', N'count', N'ExchangeSettings', N'SubQThreshold','',0,0)
	/* 21/3/2016 VSPLUS 2747 Sowmya*/
--INSERT [dbo].[ProfilesMaster] ([ID], [ServerTypeId], [AttributeName], [DefaultValue], [UnitOfMeasurement], [RelatedTable], [RelatedField] ,[RoleType],[ProfileId],[isSelected]) VALUES (35, 5, N'Poison Queue Threshold', N'10', N'count', N'ExchangeSettings', N'PoisonQThreshold','',0,0)
--11-04-2016 Durga Modified for VSPLUS-2742
INSERT [dbo].[ProfilesMaster] ([ID], [ServerTypeId], [AttributeName], [DefaultValue], [UnitOfMeasurement], [RelatedTable], [RelatedField] ,[RoleType],[ProfileId],[isSelected]) VALUES (36, 5, N'UnReachable Queue Threshold', N'100', N'count', N'ExchangeSettings', N'UnReachableQThreshold','',0,0)
INSERT [dbo].[ProfilesMaster] ([ID], [ServerTypeId], [AttributeName], [DefaultValue], [UnitOfMeasurement], [RelatedTable], [RelatedField] ,[RoleType],[ProfileId],[isSelected]) VALUES (37, 5, N'Total Queue Threshold', N'10', N'count', N'ExchangeSettings', N'TotalQThreshold','',0,0)
/* 21/3/2016 VSPLUS 2747 Sowmya*/
--INSERT [dbo].[ProfilesMaster] ([ID], [ServerTypeId], [AttributeName], [DefaultValue], [UnitOfMeasurement], [RelatedTable], [RelatedField] ,[RoleType],[ProfileId],[isSelected]) VALUES (38, 5, N'Submission Queue Threshold', N'10', N'count', N'ExchangeSettings', N'SubQThreshold','',0,0)
--11-04-2016 Durga Modified for VSPLUS-2742
INSERT [dbo].[ProfilesMaster] ([ID], [ServerTypeId], [AttributeName], [DefaultValue], [UnitOfMeasurement], [RelatedTable], [RelatedField] ,[RoleType],[ProfileId],[isSelected]) VALUES (39, 5, N'Poison Queue Threshold', N'100', N'count', N'ExchangeSettings', N'PoisonQThreshold','',0,0)
	/* 21/3/2016 VSPLUS 2747 Sowmya*/
--INSERT [dbo].[ProfilesMaster] ([ID], [ServerTypeId], [AttributeName], [DefaultValue], [UnitOfMeasurement], [RelatedTable], [RelatedField] ,[RoleType],[ProfileId],[isSelected]) VALUES (40, 5, N'UnReachable Queue Threshold', N'10', N'count', N'ExchangeSettings', N'UnReachableQThreshold','',0,0)
--INSERT [dbo].[ProfilesMaster] ([ID], [ServerTypeId], [AttributeName], [DefaultValue], [UnitOfMeasurement], [RelatedTable], [RelatedField] ,[RoleType],[ProfileId],[isSelected]) VALUES (41, 5, N'Total Queue Threshold', N'10', N'count', N'ExchangeSettings', N'TotalQThreshold','',0,0)
INSERT [dbo].[ProfilesMaster] ([ID], [ServerTypeId], [AttributeName], [DefaultValue], [UnitOfMeasurement], [RelatedTable], [RelatedField] ,[RoleType],[ProfileId],[isSelected]) VALUES (42, 5, N'SMTP', N'0', N'True/False (1- True, 0-False)', N'ExchangeSettings', N'CASSmtp','CAS',0,0)
INSERT [dbo].[ProfilesMaster] ([ID], [ServerTypeId], [AttributeName], [DefaultValue], [UnitOfMeasurement], [RelatedTable], [RelatedField] ,[RoleType],[ProfileId],[isSelected]) VALUES (43, 5, N'POP3', N'0', N'True/False (1- True, 0-False)', N'ExchangeSettings', N'CASPop3','CAS',0,0)
INSERT [dbo].[ProfilesMaster] ([ID], [ServerTypeId], [AttributeName], [DefaultValue], [UnitOfMeasurement], [RelatedTable], [RelatedField] ,[RoleType],[ProfileId],[isSelected]) VALUES (44, 5, N'IMAP', N'0', N'True/False (1- True, 0-False)', N'ExchangeSettings', N'CASImap','CAS',0,0)
INSERT [dbo].[ProfilesMaster] ([ID], [ServerTypeId], [AttributeName], [DefaultValue], [UnitOfMeasurement], [RelatedTable], [RelatedField] ,[RoleType],[ProfileId],[isSelected]) VALUES (45, 5, N'Active Sync', N'0', N'True/False (1- True, 0-False)', N'ExchangeSettings', N'CASActiveSync','CAS',0,0)
INSERT [dbo].[ProfilesMaster] ([ID], [ServerTypeId], [AttributeName], [DefaultValue], [UnitOfMeasurement], [RelatedTable], [RelatedField] ,[RoleType],[ProfileId],[isSelected]) VALUES (46, 5, N'ECP', N'0', N'True/False (1- True, 0-False)', N'ExchangeSettings', N'CASECP','CAS',0,0)
INSERT [dbo].[ProfilesMaster] ([ID], [ServerTypeId], [AttributeName], [DefaultValue], [UnitOfMeasurement], [RelatedTable], [RelatedField] ,[RoleType],[ProfileId],[isSelected]) VALUES (47, 5, N'Auto Discovery', N'0', N'True/False (1- True, 0-False)', N'ExchangeSettings', N'CASAutoDiscovery','CAS',0,0)
INSERT [dbo].[ProfilesMaster] ([ID], [ServerTypeId], [AttributeName], [DefaultValue], [UnitOfMeasurement], [RelatedTable], [RelatedField] ,[RoleType],[ProfileId],[isSelected]) VALUES (48, 5, N'Outlook Anywhere', N'0', N'True/False (1- True, 0-False)', N'ExchangeSettings', N'CASEWS','CAS',0,0)
INSERT [dbo].[ProfilesMaster] ([ID], [ServerTypeId], [AttributeName], [DefaultValue], [UnitOfMeasurement], [RelatedTable], [RelatedField] ,[RoleType],[ProfileId],[isSelected]) VALUES (49, 5, N'OWA', N'0', N'True/False (1- True, 0-False)', N'ExchangeSettings', N'CASOWA','CAS',0,0)
INSERT [dbo].[ProfilesMaster] ([ID], [ServerTypeId], [AttributeName], [DefaultValue], [UnitOfMeasurement], [RelatedTable], [RelatedField] ,[RoleType],[ProfileId],[isSelected]) VALUES (50, 5, N'Outlook Native RPC', N'0', N'True/False (1- True, 0-False)', N'ExchangeSettings', N'CASOARPC','CAS',0,0)
INSERT [dbo].[ProfilesMaster] ([ID], [ServerTypeId], [AttributeName], [DefaultValue], [UnitOfMeasurement], [RelatedTable], [RelatedField] ,[RoleType],[ProfileId],[isSelected]) VALUES (51, 5, N'Offline Address Book', N'0', N'True/False (1- True, 0-False)', N'ExchangeSettings', N'CASOAB','CAS',0,0)
INSERT [dbo].[ProfilesMaster] ([ID], [ServerTypeId], [AttributeName], [DefaultValue], [UnitOfMeasurement], [RelatedTable], [RelatedField] ,[RoleType],[ProfileId],[isSelected]) VALUES (52, 10, N'Off Hours Scan Interval', N'10', N'minutes', N'ServerAttributes', N'OffHourInterval','',0,0)
INSERT [dbo].[ProfilesMaster] ([ID], [ServerTypeId], [AttributeName], [DefaultValue], [UnitOfMeasurement], [RelatedTable], [RelatedField] ,[RoleType],[ProfileId],[isSelected]) VALUES (53, 10, N'Response Time Threshold', N'10', N'milliseconds', N'ServerAttributes', N'ResponseTime','',0,0)
INSERT [dbo].[ProfilesMaster] ([ID], [ServerTypeId], [AttributeName], [DefaultValue], [UnitOfMeasurement], [RelatedTable], [RelatedField] ,[RoleType],[ProfileId],[isSelected]) VALUES (54, 10, N'Scan Interval', N'10', N'minutes', N'ServerAttributes', N'ScanInterval','',0,0)
INSERT [dbo].[ProfilesMaster] ([ID], [ServerTypeId], [AttributeName], [DefaultValue], [UnitOfMeasurement], [RelatedTable], [RelatedField] ,[RoleType],[ProfileId],[isSelected]) VALUES (55, 10, N'Retry Interval', N'10', N'minutes', N'ServerAttributes', N'RetryInterval','',0,0)
INSERT [dbo].[ProfilesMaster] ([ID], [ServerTypeId], [AttributeName], [DefaultValue], [UnitOfMeasurement], [RelatedTable], [RelatedField] ,[RoleType],[ProfileId],[isSelected]) VALUES (56, 10, N'Category', N'10', N'string', N'ServerAttributes', N'Category','',0,0)
INSERT [dbo].[ProfilesMaster] ([ID], [ServerTypeId], [AttributeName], [DefaultValue], [UnitOfMeasurement], [RelatedTable], [RelatedField] ,[RoleType],[ProfileId],[isSelected]) VALUES (57, 10, N'Consecutive Failures before Alert', N'10', N'count', N'ServerAttributes', N'ConsFailuresBefAlert','',0,0)
INSERT [dbo].[ProfilesMaster] ([ID], [ServerTypeId], [AttributeName], [DefaultValue], [UnitOfMeasurement], [RelatedTable], [RelatedField] ,[RoleType],[ProfileId],[isSelected]) VALUES (58, 10, N'CPU Threshold', N'10', N'minutes', N'ServerAttributes', N'CPU_Threshold','',0,0)
INSERT [dbo].[ProfilesMaster] ([ID], [ServerTypeId], [AttributeName], [DefaultValue], [UnitOfMeasurement], [RelatedTable], [RelatedField] ,[RoleType],[ProfileId],[isSelected]) VALUES (59, 10, N'Consecutive Over-threshold before Alert', N'10', N'minutes', N'ServerAttributes', N'ConsOvrThresholdBefAlert','',0,0)
INSERT [dbo].[ProfilesMaster] ([ID], [ServerTypeId], [AttributeName], [DefaultValue], [UnitOfMeasurement], [RelatedTable], [RelatedField] ,[RoleType],[ProfileId],[isSelected]) VALUES (60, 1, N'Category', N'Production', N'string', N'DominoServers', N'Category','',0,0)
INSERT [dbo].[ProfilesMaster] ([ID], [ServerTypeId], [AttributeName], [DefaultValue], [UnitOfMeasurement], [RelatedTable], [RelatedField] ,[RoleType],[ProfileId],[isSelected]) VALUES (61, 1, N'BES Server', N'0', N'True/False (1- True, 0-False)', N'DominoServers', N'BES_Server','',0,0)
INSERT [dbo].[ProfilesMaster] ([ID], [ServerTypeId], [AttributeName], [DefaultValue], [UnitOfMeasurement], [RelatedTable], [RelatedField] ,[RoleType],[ProfileId],[isSelected]) VALUES (62, 1, N'BES Threshold', N'0', N'Count', N'DominoServers', N'BES_Threshold','',0,0)
INSERT [dbo].[ProfilesMaster] ([ID], [ServerTypeId], [AttributeName], [DefaultValue], [UnitOfMeasurement], [RelatedTable], [RelatedField] ,[RoleType],[ProfileId],[isSelected]) VALUES (63, 1, N'Advanced Mail Scan', N'0', N'True/False (1- True, 0-False)', N'DominoServers', N'AdvancedMailScan','',0,0)
INSERT [dbo].[ProfilesMaster] ([ID], [ServerTypeId], [AttributeName], [DefaultValue], [UnitOfMeasurement], [RelatedTable], [RelatedField] ,[RoleType],[ProfileId],[isSelected]) VALUES (64, 1, N'Scan DB Health', N'1', N'True/False (1- True, 0-False)', N'DominoServers', N'ScanDBHealth','',0,0)
INSERT [dbo].[ProfilesMaster] ([ID], [ServerTypeId], [AttributeName], [DefaultValue], [UnitOfMeasurement], [RelatedTable], [RelatedField] ,[RoleType],[ProfileId],[isSelected]) VALUES (65, 1, N'Server Days Alert', N'0', N'Number of Consecutive Days', N'DominoServers', N'ServerDaysAlert','',0,0)
INSERT [dbo].[ProfilesMaster] ([ID], [ServerTypeId], [AttributeName], [DefaultValue], [UnitOfMeasurement], [RelatedTable], [RelatedField] ,[RoleType],[ProfileId],[isSelected]) VALUES (66, 3, N'Category', N'Production', N'string', N'SametimeServers', N'Category','',0,0)
INSERT [dbo].[ProfilesMaster] ([ID], [ServerTypeId], [AttributeName], [DefaultValue], [UnitOfMeasurement], [RelatedTable], [RelatedField] ,[RoleType],[ProfileId],[isSelected]) VALUES (67, 3, N'User Threshold', N'2', N'Count', N'SametimeServers', N'UserThreshold','',0,0)
INSERT [dbo].[ProfilesMaster] ([ID], [ServerTypeId], [AttributeName], [DefaultValue], [UnitOfMeasurement], [RelatedTable], [RelatedField] ,[RoleType],[ProfileId],[isSelected]) VALUES (68, 3, N'SSL', N'0', N'True/False (1- True, 0-False)', N'SametimeServers', N'SSL','',0,0)
INSERT [dbo].[ProfilesMaster] ([ID], [ServerTypeId], [AttributeName], [DefaultValue], [UnitOfMeasurement], [RelatedTable], [RelatedField] ,[RoleType],[ProfileId],[isSelected]) VALUES (69, 3, N'Chat Threshold', N'0', N'Count', N'SametimeServers', N'ChatThreshold','',0,0)
INSERT [dbo].[ProfilesMaster] ([ID], [ServerTypeId], [AttributeName], [DefaultValue], [UnitOfMeasurement], [RelatedTable], [RelatedField] ,[RoleType],[ProfileId],[isSelected]) VALUES (70, 3, N'NChat Threshold', N'0', N'Count', N'SametimeServers', N'NChatThreshold','',0,0)
INSERT [dbo].[ProfilesMaster] ([ID], [ServerTypeId], [AttributeName], [DefaultValue], [UnitOfMeasurement], [RelatedTable], [RelatedField] ,[RoleType],[ProfileId],[isSelected]) VALUES (71, 3, N'Places Threshold', N'0', N'Count', N'SametimeServers', N'PlacesThreshold','',0,0)
INSERT [dbo].[ProfilesMaster] ([ID], [ServerTypeId], [AttributeName], [DefaultValue], [UnitOfMeasurement], [RelatedTable], [RelatedField], [RoleType],[ProfileId],[isSelected]) VALUES (72, 2, N'Enabled', N'0', N'True/False (1- True, 0-False)', N'BlackBerryServers', N'Enabled', N'',0,0)
INSERT [dbo].[ProfilesMaster] ([ID], [ServerTypeId], [AttributeName], [DefaultValue], [UnitOfMeasurement], [RelatedTable], [RelatedField], [RoleType],[ProfileId],[isSelected]) VALUES (73, 2, N'Scan Interval', N'10', N'minutes', N'BlackBerryServers', N'ScanInterval', N'',0,0)
INSERT [dbo].[ProfilesMaster] ([ID], [ServerTypeId], [AttributeName], [DefaultValue], [UnitOfMeasurement], [RelatedTable], [RelatedField], [RoleType],[ProfileId],[isSelected]) VALUES (74, 2, N'Off Hours Scan Interval', N'10', N'minutes', N'BlackBerryServers', N'OffHoursScanInterval', N'',0,0)
INSERT [dbo].[ProfilesMaster] ([ID], [ServerTypeId], [AttributeName], [DefaultValue], [UnitOfMeasurement], [RelatedTable], [RelatedField], [RoleType],[ProfileId],[isSelected]) VALUES (75, 2, N'Retry Interval', N'10', N'minutes', N'BlackBerryServers', N'RetryInterval', N'',0,0)
INSERT [dbo].[ProfilesMaster] ([ID], [ServerTypeId], [AttributeName], [DefaultValue], [UnitOfMeasurement], [RelatedTable], [RelatedField], [RoleType],[ProfileId],[isSelected]) VALUES (76, 2, N'Messaging', N'0', N'True/False (1- True, 0-False)', N'BlackBerryServers', N'Messaging', N'',0,0)
INSERT [dbo].[ProfilesMaster] ([ID], [ServerTypeId], [AttributeName], [DefaultValue], [UnitOfMeasurement], [RelatedTable], [RelatedField], [RoleType],[ProfileId],[isSelected]) VALUES (77, 2, N'Controller', N'0', N'True/False (1- True, 0-False)', N'BlackBerryServers', N'Controller', N'',0,0)
INSERT [dbo].[ProfilesMaster] ([ID], [ServerTypeId], [AttributeName], [DefaultValue], [UnitOfMeasurement], [RelatedTable], [RelatedField], [RoleType],[ProfileId],[isSelected]) VALUES (78, 2, N'Dispatcher', N'0', N'True/False (1- True, 0-False)', N'BlackBerryServers', N'Dispatcher', N'',0,0)
INSERT [dbo].[ProfilesMaster] ([ID], [ServerTypeId], [AttributeName], [DefaultValue], [UnitOfMeasurement], [RelatedTable], [RelatedField], [RoleType],[ProfileId],[isSelected]) VALUES (79, 2, N'Synchronization', N'0', N'True/False (1- True, 0-False)', N'BlackBerryServers', N'Synchronization', N'',0,0)
INSERT [dbo].[ProfilesMaster] ([ID], [ServerTypeId], [AttributeName], [DefaultValue], [UnitOfMeasurement], [RelatedTable], [RelatedField], [RoleType],[ProfileId],[isSelected]) VALUES (80, 2, N'Policy', N'0', N'True/False (1- True, 0-False)', N'BlackBerryServers', N'Policy', N'',0,0)
INSERT [dbo].[ProfilesMaster] ([ID], [ServerTypeId], [AttributeName], [DefaultValue], [UnitOfMeasurement], [RelatedTable], [RelatedField], [RoleType],[ProfileId],[isSelected]) VALUES (81, 2, N'MDS', N'0', N'True/False (1- True, 0-False)', N'BlackBerryServers', N'MDS', N'',0,0)
INSERT [dbo].[ProfilesMaster] ([ID], [ServerTypeId], [AttributeName], [DefaultValue], [UnitOfMeasurement], [RelatedTable], [RelatedField], [RoleType],[ProfileId],[isSelected]) VALUES (82, 2, N'Attachment', N'0', N'True/False (1- True, 0-False)', N'BlackBerryServers', N'Attachment', N'',0,0)
INSERT [dbo].[ProfilesMaster] ([ID], [ServerTypeId], [AttributeName], [DefaultValue], [UnitOfMeasurement], [RelatedTable], [RelatedField], [RoleType],[ProfileId],[isSelected]) VALUES (83, 2, N'Alert', N'0', N'True/False (1- True, 0-False)', N'BlackBerryServers', N'Alert', N'',0,0)
INSERT [dbo].[ProfilesMaster] ([ID], [ServerTypeId], [AttributeName], [DefaultValue], [UnitOfMeasurement], [RelatedTable], [RelatedField], [RoleType],[ProfileId],[isSelected]) VALUES (84, 2, N'Router', N'0', N'True/False (1- True, 0-False)', N'BlackBerryServers', N'Router', N'',0,0)
INSERT [dbo].[ProfilesMaster] ([ID], [ServerTypeId], [AttributeName], [DefaultValue], [UnitOfMeasurement], [RelatedTable], [RelatedField], [RoleType],[ProfileId],[isSelected]) VALUES (85, 2, N'MDSConnection', N'0', N'True/False (1- True, 0-False)', N'BlackBerryServers', N'MDSConnection', N'',0,0)
INSERT [dbo].[ProfilesMaster] ([ID], [ServerTypeId], [AttributeName], [DefaultValue], [UnitOfMeasurement], [RelatedTable], [RelatedField], [RoleType],[ProfileId],[isSelected]) VALUES (86, 2, N'MDSServices', N'0', N'True/False (1- True, 0-False)', N'BlackBerryServers', N'MDSServices', N'',0,0)
INSERT [dbo].[ProfilesMaster] ([ID], [ServerTypeId], [AttributeName], [DefaultValue], [UnitOfMeasurement], [RelatedTable], [RelatedField], [RoleType],[ProfileId],[isSelected]) VALUES (87, 2, N'PendingThreshold', N'10', N'minutes', N'BlackBerryServers', N'PendingThreshold', N'',0,0)
INSERT [dbo].[ProfilesMaster] ([ID], [ServerTypeId], [AttributeName], [DefaultValue], [UnitOfMeasurement], [RelatedTable], [RelatedField], [RoleType],[ProfileId],[isSelected]) VALUES (88, 2, N'ExpiredThreshold', N'10', N'minutes', N'BlackBerryServers', N'ExpiredThreshold', N'',0,0)
INSERT [dbo].[ProfilesMaster] ([ID], [ServerTypeId], [AttributeName], [DefaultValue], [UnitOfMeasurement], [RelatedTable], [RelatedField], [RoleType],[ProfileId],[isSelected]) VALUES (89, 2, N'SNMPCommunity', N'', N'string', N'BlackBerryServers', N'SNMPCommunity', N'',0,0)
INSERT [dbo].[ProfilesMaster] ([ID], [ServerTypeId], [AttributeName], [DefaultValue], [UnitOfMeasurement], [RelatedTable], [RelatedField], [RoleType],[ProfileId],[isSelected]) VALUES (90, 2, N'Alert IP', N'', N'string', N'BlackBerryServers', N'AlertIP', N'',0,0)
INSERT [dbo].[ProfilesMaster] ([ID], [ServerTypeId], [AttributeName], [DefaultValue], [UnitOfMeasurement], [RelatedTable], [RelatedField], [RoleType],[ProfileId],[isSelected]) VALUES (91, 2, N'Router IP', N'', N'string', N'BlackBerryServers', N'RouterIP', N'',0,0)
INSERT [dbo].[ProfilesMaster] ([ID], [ServerTypeId], [AttributeName], [DefaultValue], [UnitOfMeasurement], [RelatedTable], [RelatedField], [RoleType],[ProfileId],[isSelected]) VALUES (92, 2, N'AttachmentIP ', N'', N'string', N'BlackBerryServers', N'AttachmentIP', N'',0,0)
INSERT [dbo].[ProfilesMaster] ([ID], [ServerTypeId], [AttributeName], [DefaultValue], [UnitOfMeasurement], [RelatedTable], [RelatedField], [RoleType],[ProfileId],[isSelected]) VALUES (93, 2, N'Other Services', N'', N'string', N'BlackBerryServers', N'OtherServices', N'',0,0)
INSERT [dbo].[ProfilesMaster] ([ID], [ServerTypeId], [AttributeName], [DefaultValue], [UnitOfMeasurement], [RelatedTable], [RelatedField], [RoleType],[ProfileId],[isSelected]) VALUES (94, 2, N'BES Version', N'', N'string', N'BlackBerryServers', N'BESVersion', N'',0,0)
INSERT [dbo].[ProfilesMaster] ([ID], [ServerTypeId], [AttributeName], [DefaultValue], [UnitOfMeasurement], [RelatedTable], [RelatedField], [RoleType],[ProfileId],[isSelected]) VALUES (95, 2, N'TimeZone Adjustment', N'0', N'integer', N'BlackBerryServers', N'TimeZoneAdjustment', N'',0,0)
INSERT [dbo].[ProfilesMaster] ([ID], [ServerTypeId], [AttributeName], [DefaultValue], [UnitOfMeasurement], [RelatedTable], [RelatedField], [RoleType],[ProfileId],[isSelected]) VALUES (96, 2, N'USDateFormat', N'0', N'True/False (1- True, 0-False)', N'BlackBerryServers', N'USDateFormat', N'',0,0)
INSERT [dbo].[ProfilesMaster] ([ID], [ServerTypeId], [AttributeName], [DefaultValue], [UnitOfMeasurement], [RelatedTable], [RelatedField], [RoleType],[ProfileId],[isSelected]) VALUES (97, 2, N'Notification Group', N'', N'string', N'BlackBerryServers', N'NotificationGroup', N'',0,0)
INSERT [dbo].[ProfilesMaster] ([ID], [ServerTypeId], [AttributeName], [DefaultValue], [UnitOfMeasurement], [RelatedTable], [RelatedField], [RoleType],[ProfileId],[isSelected]) VALUES (98, 5, N'Scan Interval', N'10', N'minutes', N'ServerAttributes', N'ScanInterval', N'',0,0)
INSERT [dbo].[ProfilesMaster] ([ID], [ServerTypeId], [AttributeName], [DefaultValue], [UnitOfMeasurement], [RelatedTable], [RelatedField], [RoleType],[ProfileId],[isSelected]) VALUES (99, 5, N'Retry Interval', N'10', N'minutes', N'ServerAttributes', N'RetryInterval', N'',0,0)
INSERT [dbo].[ProfilesMaster] ([ID], [ServerTypeId], [AttributeName], [DefaultValue], [UnitOfMeasurement], [RelatedTable], [RelatedField], [RoleType],[ProfileId],[isSelected]) VALUES (100, 5, N'Off Hours Scan Interval', N'10', N'minutes', N'ServerAttributes', N'OffHourInterval', N'',0,0)
INSERT [dbo].[ProfilesMaster] ([ID], [ServerTypeId], [AttributeName], [DefaultValue], [UnitOfMeasurement], [RelatedTable], [RelatedField], [RoleType],[ProfileId],[isSelected]) VALUES (101, 5, N'Category', N'Production', N'string', N'ServerAttributes', N'Category', N'',0,0)
INSERT [dbo].[ProfilesMaster] ([ID], [ServerTypeId], [AttributeName], [DefaultValue], [UnitOfMeasurement], [RelatedTable], [RelatedField], [RoleType],[ProfileId],[isSelected]) VALUES (102, 5, N'CPU Threshold', N'.9', N'Percentage Used (eg:- for 90% = 0.90)', N'ServerAttributes', N'CPU_Threshold', N'',0,0)
INSERT [dbo].[ProfilesMaster] ([ID], [ServerTypeId], [AttributeName], [DefaultValue], [UnitOfMeasurement], [RelatedTable], [RelatedField], [RoleType],[ProfileId],[isSelected]) VALUES (103, 5, N'Response Time Threshold', N'2000', N'milliseconds', N'ServerAttributes', N'ResponseTime', N'',0,0)
INSERT [dbo].[ProfilesMaster] ([ID], [ServerTypeId], [AttributeName], [DefaultValue], [UnitOfMeasurement], [RelatedTable], [RelatedField], [RoleType],[ProfileId],[isSelected]) VALUES (104, 5, N'Consecutive Failures Before Alert', N'10', N'count', N'ServerAttributes', N'ConsFailuresBefAlert', N'',0,0)
INSERT [dbo].[ProfilesMaster] ([ID], [ServerTypeId], [AttributeName], [DefaultValue], [UnitOfMeasurement], [RelatedTable], [RelatedField], [RoleType],[ProfileId],[isSelected]) VALUES (105, 15, N'Scan Interval', N'10', N'minutes', N'LyncServers', N'ScanInterval', N'',0,0)
INSERT [dbo].[ProfilesMaster] ([ID], [ServerTypeId], [AttributeName], [DefaultValue], [UnitOfMeasurement], [RelatedTable], [RelatedField], [RoleType],[ProfileId],[isSelected]) VALUES (106, 15, N'Retry Interval', N'10', N'minutes', N'LyncServers', N'RetryInterval', N'',0,0)
INSERT [dbo].[ProfilesMaster] ([ID], [ServerTypeId], [AttributeName], [DefaultValue], [UnitOfMeasurement], [RelatedTable], [RelatedField], [RoleType],[ProfileId],[isSelected]) VALUES (107, 15, N'Off Hours Scan Interval', N'10', N'minutes', N'LyncServers', N'OffHourInterval', N'',0,0)
INSERT [dbo].[ProfilesMaster] ([ID], [ServerTypeId], [AttributeName], [DefaultValue], [UnitOfMeasurement], [RelatedTable], [RelatedField], [RoleType],[ProfileId],[isSelected]) VALUES (108, 15, N'Response Time Threshold', N'2000', N'milliseconds', N'LyncServers', N'ResponseTime', N'',0,0)
INSERT [dbo].[ProfilesMaster] ([ID], [ServerTypeId], [AttributeName], [DefaultValue], [UnitOfMeasurement], [RelatedTable], [RelatedField], [RoleType],[ProfileId],[isSelected]) VALUES (109, 15, N'Consecutive Failures Before Alert', N'10', N'count', N'LyncServers', N'ConsFailuresBefAlert', N'',0,0)
INSERT [dbo].[ProfilesMaster] ([ID], [ServerTypeId], [AttributeName], [DefaultValue], [UnitOfMeasurement], [RelatedTable], [RelatedField], [RoleType],[ProfileId],[isSelected]) VALUES (110, 15, N'Memory Threshold', N'0.90', N'Percentage Used (eg:- for 90% = 0.90)', N'LyncServers', N'MemThreshold', N'',0,0)
INSERT [dbo].[ProfilesMaster] ([ID], [ServerTypeId], [AttributeName], [DefaultValue], [UnitOfMeasurement], [RelatedTable], [RelatedField], [RoleType],[ProfileId],[isSelected]) VALUES (111, 15, N'Enabled', N'1', N'True/False (1- True, 0-False)', N'LyncServers', N'Enabled', N'',0,0)
INSERT [dbo].[ProfilesMaster] ([ID], [ServerTypeId], [AttributeName], [DefaultValue], [UnitOfMeasurement], [RelatedTable], [RelatedField], [RoleType],[ProfileId],[isSelected]) VALUES (112, 15, N'CPU Threshold', N'.9', N'Percentage Used (eg:- for 90% = 0.90)', N'LyncServers', N'CPU_Threshold', N'',0,0)
INSERT [dbo].[ProfilesMaster] ([ID], [ServerTypeId], [AttributeName], [DefaultValue], [UnitOfMeasurement], [RelatedTable], [RelatedField], [RoleType],[ProfileId],[isSelected]) VALUES (113, 5, N'Server Days Alert', N'0', N'Number of Consecutive Days', N'ServerAttributes', N'ConsOvrThresholdBefAlert', N'',0,0)
--SSE for DAG, ExchMailprobe, NotesMailProbe, Windows Servers, AD, Cloud Applications.
INSERT [dbo].[ProfilesMaster] ([ID], [ServerTypeId], [AttributeName], [DefaultValue], [UnitOfMeasurement], [RelatedTable], [RelatedField], [RoleType],[ProfileId],[isSelected]) VALUES (114, 19, N'Reply Queue Threshold', N'2', N'messages', N'DagSettings', N'ReplyQThreshold', N'',0,0)
INSERT [dbo].[ProfilesMaster] ([ID], [ServerTypeId], [AttributeName], [DefaultValue], [UnitOfMeasurement], [RelatedTable], [RelatedField], [RoleType],[ProfileId],[isSelected]) VALUES (115, 19, N'Copy Queue Threshold', N'2', N'messages', N'DagSettings', N'CopyQThreshold', N'',0,0)
INSERT [dbo].[ProfilesMaster] ([ID], [ServerTypeId], [AttributeName], [DefaultValue], [UnitOfMeasurement], [RelatedTable], [RelatedField], [RoleType],[ProfileId],[isSelected]) VALUES (116, 19, N'Scan Interval', N'10', N'Minutes', N'ServerAttributes', N'ScanInterval', N'',0,0)
INSERT [dbo].[ProfilesMaster] ([ID], [ServerTypeId], [AttributeName], [DefaultValue], [UnitOfMeasurement], [RelatedTable], [RelatedField], [RoleType],[ProfileId],[isSelected]) VALUES (117, 19, N'Retry Interval', N'1', N'Minutes', N'ServerAttributes', N'RetryInterval', N'',0,0)
INSERT [dbo].[ProfilesMaster] ([ID], [ServerTypeId], [AttributeName], [DefaultValue], [UnitOfMeasurement], [RelatedTable], [RelatedField], [RoleType],[ProfileId],[isSelected]) VALUES (118, 19, N'Off Hour Interval', N'10', N'Minutes', N'ServerAttributes', N'OffHourInterval', N'',0,0)
INSERT [dbo].[ProfilesMaster] ([ID], [ServerTypeId], [AttributeName], [DefaultValue], [UnitOfMeasurement], [RelatedTable], [RelatedField], [RoleType],[ProfileId],[isSelected]) VALUES (119, 14, N'Scan Interval', N'8', N'Minutes', N'ExchangeMailProbe', N'ScanInterval', N'',0,0)
INSERT [dbo].[ProfilesMaster] ([ID], [ServerTypeId], [AttributeName], [DefaultValue], [UnitOfMeasurement], [RelatedTable], [RelatedField], [RoleType],[ProfileId],[isSelected]) VALUES (120, 14, N'Off-Hours Scan Interval', N'30', N'Minutes', N'ExchangeMailProbe', N'OffHoursScanInterval', N'',0,0)
INSERT [dbo].[ProfilesMaster] ([ID], [ServerTypeId], [AttributeName], [DefaultValue], [UnitOfMeasurement], [RelatedTable], [RelatedField], [RoleType],[ProfileId],[isSelected]) VALUES (121, 14, N'Retry Interval', N'5', N'Minutes', N'ExchangeMailProbe', N'RetryInterval', N'',0,0)
INSERT [dbo].[ProfilesMaster] ([ID], [ServerTypeId], [AttributeName], [DefaultValue], [UnitOfMeasurement], [RelatedTable], [RelatedField], [RoleType],[ProfileId],[isSelected]) VALUES (122, 14, N'Delivery Threshold', N'5', N'Minutes', N'ExchangeMailProbe', N'DeliveryThreshold', N'',0,0)
INSERT [dbo].[ProfilesMaster] ([ID], [ServerTypeId], [AttributeName], [DefaultValue], [UnitOfMeasurement], [RelatedTable], [RelatedField], [RoleType],[ProfileId],[isSelected]) VALUES (123, 18, N'Scan Interval', N'8', N'Minutes', N'ServerAttributes', N'ScanInterval', N'',0,0)
INSERT [dbo].[ProfilesMaster] ([ID], [ServerTypeId], [AttributeName], [DefaultValue], [UnitOfMeasurement], [RelatedTable], [RelatedField], [RoleType],[ProfileId],[isSelected]) VALUES (124, 18, N'Retry Interval', N'3', N'Minutes', N'ServerAttributes', N'RetryInterval', N'',0,0)
INSERT [dbo].[ProfilesMaster] ([ID], [ServerTypeId], [AttributeName], [DefaultValue], [UnitOfMeasurement], [RelatedTable], [RelatedField], [RoleType],[ProfileId],[isSelected]) VALUES (125, 18, N'Off-Hours Scan Interval', N'10', N'Minutes', N'ServerAttributes', N'OffHourInterval', N'',0,0)
INSERT [dbo].[ProfilesMaster] ([ID], [ServerTypeId], [AttributeName], [DefaultValue], [UnitOfMeasurement], [RelatedTable], [RelatedField], [RoleType],[ProfileId],[isSelected]) VALUES (126, 18, N'Response Threshold', N'200', N'milliseconds', N'ServerAttributes', N'ResponseTime', N'',0,0)
INSERT [dbo].[ProfilesMaster] ([ID], [ServerTypeId], [AttributeName], [DefaultValue], [UnitOfMeasurement], [RelatedTable], [RelatedField], [RoleType],[ProfileId],[isSelected]) VALUES (127, 18, N'Failures before Alert', N'2', N'Consecutive Failures', N'ServerAttributes', N'ConsFailuresBefAlert', N'',0,0)
INSERT [dbo].[ProfilesMaster] ([ID], [ServerTypeId], [AttributeName], [DefaultValue], [UnitOfMeasurement], [RelatedTable], [RelatedField], [RoleType],[ProfileId],[isSelected]) VALUES (128, 16, N'Scan Interval', N'8', N'Minutes', N'ServerAttributes', N'ScanInterval', N'',0,0)
INSERT [dbo].[ProfilesMaster] ([ID], [ServerTypeId], [AttributeName], [DefaultValue], [UnitOfMeasurement], [RelatedTable], [RelatedField], [RoleType],[ProfileId],[isSelected]) VALUES (129, 16, N'Retry Interval', N'2', N'Minutes', N'ServerAttributes', N'RetryInterval', N'',0,0)
INSERT [dbo].[ProfilesMaster] ([ID], [ServerTypeId], [AttributeName], [DefaultValue], [UnitOfMeasurement], [RelatedTable], [RelatedField], [RoleType],[ProfileId],[isSelected]) VALUES (130, 16, N'Off-Hours Scan Interval', N'15', N'Minutes', N'ServerAttributes', N'OffHourInterval', N'',0,0)
INSERT [dbo].[ProfilesMaster] ([ID], [ServerTypeId], [AttributeName], [DefaultValue], [UnitOfMeasurement], [RelatedTable], [RelatedField], [RoleType],[ProfileId],[isSelected]) VALUES (131, 16, N'Response Threshold', N'5000', N'milliseconds', N'ServerAttributes', N'ResponseTime', N'',0,0)
INSERT [dbo].[ProfilesMaster] ([ID], [ServerTypeId], [AttributeName], [DefaultValue], [UnitOfMeasurement], [RelatedTable], [RelatedField], [RoleType],[ProfileId],[isSelected]) VALUES (132, 16, N'Failure Threshold', N'2', N'Consecutive Failures', N'ServerAttributes', N'ConsFailuresBefAlert', N'',0,0)
INSERT [dbo].[ProfilesMaster] ([ID], [ServerTypeId], [AttributeName], [DefaultValue], [UnitOfMeasurement], [RelatedTable], [RelatedField], [RoleType],[ProfileId],[isSelected]) VALUES (133, 16, N'Memory Threshold', N'.9', N'Percentage Used (eg:- for 90% = 0.90)', N'ServerAttributes', N'MemThreshold', N'',0,0)
INSERT [dbo].[ProfilesMaster] ([ID], [ServerTypeId], [AttributeName], [DefaultValue], [UnitOfMeasurement], [RelatedTable], [RelatedField], [RoleType],[ProfileId],[isSelected]) VALUES (134, 16, N'CPU Threshold', N'.9', N'Percentage Used (eg:- for 90% = 0.90)', N'ServerAttributes', N'CPU_Threshold', N'',0,0)
INSERT [dbo].[ProfilesMaster] ([ID], [ServerTypeId], [AttributeName], [DefaultValue], [UnitOfMeasurement], [RelatedTable], [RelatedField], [RoleType],[ProfileId],[isSelected]) VALUES (135, 16, N'Enabled', N'1', N'True/False (1- True, 0-False)', N'ServerAttributes', N'Enabled', N'',0,0)
INSERT [dbo].[ProfilesMaster] ([ID], [ServerTypeId], [AttributeName], [DefaultValue], [UnitOfMeasurement], [RelatedTable], [RelatedField], [RoleType],[ProfileId],[isSelected]) VALUES (136, 13, N'Scan Interval', N'8', N'Minutes', N'NotesMailProbe', N'ScanInterval', N'',0,0)
INSERT [dbo].[ProfilesMaster] ([ID], [ServerTypeId], [AttributeName], [DefaultValue], [UnitOfMeasurement], [RelatedTable], [RelatedField], [RoleType],[ProfileId],[isSelected]) VALUES (137, 13, N'Off-Hours Scan Interval', N'30', N'Minutes', N'NotesMailProbe', N'OffHoursScanInterval', N'',0,0)
INSERT [dbo].[ProfilesMaster] ([ID], [ServerTypeId], [AttributeName], [DefaultValue], [UnitOfMeasurement], [RelatedTable], [RelatedField], [RoleType],[ProfileId],[isSelected]) VALUES (138, 13, N'Retry Interval', N'5', N'Minutes', N'NotesMailProbe', N'RetryInterval', N'',0,0)
INSERT [dbo].[ProfilesMaster] ([ID], [ServerTypeId], [AttributeName], [DefaultValue], [UnitOfMeasurement], [RelatedTable], [RelatedField], [RoleType],[ProfileId],[isSelected]) VALUES (139, 13, N'Delivery Threshold', N'5', N'Minutes', N'NotesMailProbe', N'DeliveryThreshold', N'',0,0)
INSERT [dbo].[ProfilesMaster] ([ID], [ServerTypeId], [AttributeName], [DefaultValue], [UnitOfMeasurement], [RelatedTable], [RelatedField], [RoleType],[ProfileId],[isSelected]) VALUES (140, 17, N'Scan Interval', N'8', N'Minutes', N'CloudDetails', N'ScanInterval', N'',0,0)
INSERT [dbo].[ProfilesMaster] ([ID], [ServerTypeId], [AttributeName], [DefaultValue], [UnitOfMeasurement], [RelatedTable], [RelatedField], [RoleType],[ProfileId],[isSelected]) VALUES (141, 17, N'Retry Interval', N'2', N'Minutes', N'CloudDetails', N'RetryInterval', N'',0,0)
INSERT [dbo].[ProfilesMaster] ([ID], [ServerTypeId], [AttributeName], [DefaultValue], [UnitOfMeasurement], [RelatedTable], [RelatedField], [RoleType],[ProfileId],[isSelected]) VALUES (142, 17, N'Off-Hours Scan Interval', N'30', N'Minutes', N'CloudDetails', N'OffHoursScanInterval', N'',0,0)
INSERT [dbo].[ProfilesMaster] ([ID], [ServerTypeId], [AttributeName], [DefaultValue], [UnitOfMeasurement], [RelatedTable], [RelatedField], [RoleType],[ProfileId],[isSelected]) VALUES (143, 17, N'Response Threshold', N'2500', N'milliseconds', N'CloudDetails', N'ResponseThreshold', N'',0,0)
INSERT [dbo].[ProfilesMaster] ([ID], [ServerTypeId], [AttributeName], [DefaultValue], [UnitOfMeasurement], [RelatedTable], [RelatedField], [RoleType],[ProfileId],[isSelected]) VALUES (144, 17, N'Failures before Alert', N'2', N'Consecutive Failures', N'CloudDetails', N'FailureThreshold', N'',0,0)
INSERT [dbo].[ProfilesMaster] ([ID], [ServerTypeId], [AttributeName], [DefaultValue], [UnitOfMeasurement], [RelatedTable], [RelatedField], [RoleType],[ProfileId],[isSelected]) VALUES (145, 19, N'Enabled', N'1', N'True/False (1- True, 0-False)', N'ServerAttributes', N'Enabled', N'',0,0)
INSERT [dbo].[ProfilesMaster] ([ID], [ServerTypeId], [AttributeName], [DefaultValue], [UnitOfMeasurement], [RelatedTable], [RelatedField], [RoleType],[ProfileId],[isSelected]) VALUES (169, 5, N'LatencyYellowThreshold', N'10', N'Seconds', N'ExchangeSettings', N'LatencyYellowThreshold', N'Mailbox',0,0)
INSERT [dbo].[ProfilesMaster] ([ID], [ServerTypeId], [AttributeName], [DefaultValue], [UnitOfMeasurement], [RelatedTable], [RelatedField], [RoleType],[ProfileId],[isSelected]) VALUES (170, 5, N'LatencyRedThreshold', N'10', N'Seconds', N'ExchangeSettings', N'LatencyRedThreshold', N'Mailbox',0,0)
INSERT [dbo].[ProfilesMaster] ([ID], [ServerTypeId], [AttributeName], [DefaultValue], [UnitOfMeasurement], [RelatedTable], [RelatedField], [RoleType],[ProfileId],[isSelected]) VALUES (171, 5, N'Enabled For Latency Test', N'1', N'True/False (1- True, 0-False)', N'ExchangeSettings', N'EnableLatencyTest', N'Mailbox',0,0)
INSERT [dbo].[ProfilesMaster] ([ID], [ServerTypeId], [AttributeName], [DefaultValue], [UnitOfMeasurement], [RelatedTable], [RelatedField], [RoleType],[ProfileId],[isSelected]) VALUES (172, 18, N'Category', N'Production', N'string', N'ServerAttributes', N'Category', N'',0,0)
INSERT [dbo].[ProfilesMaster] ([ID], [ServerTypeId], [AttributeName], [DefaultValue], [UnitOfMeasurement], [RelatedTable], [RelatedField], [RoleType],[ProfileId],[isSelected]) VALUES (173, 18, N'Enabled for Scanning', N'1', N'True/False (1- True, 0-False)', N'ServerAttributes', N'Enabled', N'',0,0)
--11-04-2016 Durga Modified for VSPLUS-2742
INSERT [dbo].[ProfilesMaster] ([ID], [ServerTypeId], [AttributeName], [DefaultValue], [UnitOfMeasurement], [RelatedTable], [RelatedField] ,[RoleType],[ProfileId],[isSelected]) VALUES (174, 5, N'Shadow Queue Threshold', N'100', N'count', N'ExchangeSettings', N'ShadowQThreshold','',0,0)
	/* 21/3/2016 VSPLUS 2747 Sowmya*/
--INSERT [dbo].[ProfilesMaster] ([ID], [ServerTypeId], [AttributeName], [DefaultValue], [UnitOfMeasurement], [RelatedTable], [RelatedField] ,[RoleType],[ProfileId],[isSelected]) VALUES (175, 5, N'Shadow Queue Threshold', N'100', N'count', N'ExchangeSettings', N'ShadowQThreshold','',0,0)
/* 4/4/2016 NS modified for VSPLUS-2769: the default value for a Sharepoint server should be enabled */	
INSERT [dbo].[ProfilesMaster] ([ID], [ServerTypeId], [AttributeName], [DefaultValue], [UnitOfMeasurement], [RelatedTable], [RelatedField], [RoleType],[ProfileId],[isSelected]) VALUES (176, 4, N'Enabled', N'1', N'True/False (1- True, 0-False)', N'ServerAttributes', N'Enabled', N'',0,0)
INSERT [dbo].[ProfilesMaster] ([ID], [ServerTypeId], [AttributeName], [DefaultValue], [UnitOfMeasurement], [RelatedTable], [RelatedField], [RoleType],[ProfileId],[isSelected]) VALUES (177, 4, N'Category', N'Production', N'string', N'ServerAttributes', N'Category', N'',0,0)
/* VSPLUS 1296 Swathi*/
INSERT [dbo].[ProfilesMaster] ([ID], [ServerTypeId], [AttributeName], [DefaultValue], [UnitOfMeasurement], [RelatedTable], [RelatedField], [RoleType],[ProfileId],[isSelected]) VALUES (178, 16, N'Category', N'Production', N'string', N'ServerAttributes', N'Category', N'',0,0)
INSERT [dbo].[ProfilesMaster] ([ID], [ServerTypeId], [AttributeName], [DefaultValue], [UnitOfMeasurement], [RelatedTable], [RelatedField], [RoleType], [ProfileId], [isSelected]) VALUES (179, 22, N'Enabled', N'1', N'True/False (1- True, 0-False', N'ServerAttributes', N'Enabled', NULL, 0, 0)
INSERT [dbo].[ProfilesMaster] ([ID], [ServerTypeId], [AttributeName], [DefaultValue], [UnitOfMeasurement], [RelatedTable], [RelatedField], [RoleType], [ProfileId], [isSelected]) VALUES (180, 22, N'Category', N'Production', N'string', N'ServerAttributes', N'Category', N'', 0, 0)
INSERT [dbo].[ProfilesMaster] ([ID], [ServerTypeId], [AttributeName], [DefaultValue], [UnitOfMeasurement], [RelatedTable], [RelatedField], [RoleType], [ProfileId], [isSelected]) VALUES (181, 22, N'Enabled for Scanning', N'1', N'True/False (1- True, 0-False)', N'ServerAttributes', N'Enabled', N'', 0, 0)
INSERT [dbo].[ProfilesMaster] ([ID], [ServerTypeId], [AttributeName], [DefaultValue], [UnitOfMeasurement], [RelatedTable], [RelatedField], [RoleType], [ProfileId], [isSelected]) VALUES (182, 22, N'Scan Interval', N'8', N'Minutes', N'ServerAttributes', N'ScanInterval', N'', 0, 0)
INSERT [dbo].[ProfilesMaster] ([ID], [ServerTypeId], [AttributeName], [DefaultValue], [UnitOfMeasurement], [RelatedTable], [RelatedField], [RoleType], [ProfileId], [isSelected]) VALUES (183, 22, N'Retry Interval', N'3', N'Minutes', N'ServerAttributes', N'RetryInterval', N'', 0, 0)
INSERT [dbo].[ProfilesMaster] ([ID], [ServerTypeId], [AttributeName], [DefaultValue], [UnitOfMeasurement], [RelatedTable], [RelatedField], [RoleType], [ProfileId], [isSelected]) VALUES (184, 22, N'Off Hours Scan Interval', N'10', N'Minutes', N'ServerAttributes', N'OffHourInterval', N'', 0, 0)
INSERT [dbo].[ProfilesMaster] ([ID], [ServerTypeId], [AttributeName], [DefaultValue], [UnitOfMeasurement], [RelatedTable], [RelatedField], [RoleType], [ProfileId], [isSelected]) VALUES (185, 22, N'Response Threshold', N'200', N'milliseconds', N'ServerAttributes', N'ResponseTime', N'', 0, 0)
INSERT [dbo].[ProfilesMaster] ([ID], [ServerTypeId], [AttributeName], [DefaultValue], [UnitOfMeasurement], [RelatedTable], [RelatedField], [RoleType], [ProfileId], [isSelected]) VALUES (186, 22, N'Failure Threshold', N'2', N'Consecutive Failures', N'ServerAttributes', N'ConsFailuresBefAlert', N'', 0, 0)
INSERT [dbo].[ProfilesMaster] ([ID], [ServerTypeId], [AttributeName], [DefaultValue], [UnitOfMeasurement], [RelatedTable], [RelatedField], [RoleType], [ProfileId], [isSelected]) VALUES (187, 22, N'Average Thread Poll', N'3', NULL, N'WebsphereServer', N'Average Thread Poll', NULL, NULL, NULL)
INSERT [dbo].[ProfilesMaster] ([ID], [ServerTypeId], [AttributeName], [DefaultValue], [UnitOfMeasurement], [RelatedTable], [RelatedField], [RoleType], [ProfileId], [isSelected]) VALUES (188, 22, N'Heap Current', N'3', NULL, N'WebsphereServer', N'Heap Current', NULL, NULL, NULL)
INSERT [dbo].[ProfilesMaster] ([ID], [ServerTypeId], [AttributeName], [DefaultValue], [UnitOfMeasurement], [RelatedTable], [RelatedField], [RoleType], [ProfileId], [isSelected]) VALUES (189, 22, N'Up Time', N'3', NULL, N'WebsphereServer', N'Up Time', NULL, NULL, NULL)
INSERT [dbo].[ProfilesMaster] ([ID], [ServerTypeId], [AttributeName], [DefaultValue], [UnitOfMeasurement], [RelatedTable], [RelatedField], [RoleType], [ProfileId], [isSelected]) VALUES (190, 22, N'Dump Generator', N'3', NULL, N'WebsphereServer', N'Dump Generator', NULL, NULL, NULL)
INSERT [dbo].[ProfilesMaster] ([ID], [ServerTypeId], [AttributeName], [DefaultValue], [UnitOfMeasurement], [RelatedTable], [RelatedField], [RoleType], [ProfileId], [isSelected]) VALUES (191, 22, N'Active Thread Count', N'3', NULL, N'WebsphereServer', N'Active Thread Count', NULL, NULL, NULL)
INSERT [dbo].[ProfilesMaster] ([ID], [ServerTypeId], [AttributeName], [DefaultValue], [UnitOfMeasurement], [RelatedTable], [RelatedField], [RoleType], [ProfileId], [isSelected]) VALUES (192, 22, N'Maximum Heap', N'3', NULL, N'WebsphereServer', N'Maximum Heap', NULL, NULL, NULL)
INSERT [dbo].[ProfilesMaster] ([ID], [ServerTypeId], [AttributeName], [DefaultValue], [UnitOfMeasurement], [RelatedTable], [RelatedField], [RoleType], [ProfileId], [isSelected]) VALUES (193, 22, N'Hung Thread Count', N'3', NULL, N'WebsphereServer', N'Hung Thread Count', NULL, NULL, NULL)
/*1473 swathi*/
INSERT [dbo].[ProfilesMaster] ([ID], [ServerTypeId], [AttributeName], [DefaultValue], [UnitOfMeasurement], [RelatedTable], [RelatedField], [RoleType],[ProfileId],[isSelected]) VALUES (194, 4, N'LatencyYellowThreshold', N'10', N'Seconds', N'ServerAttributes', N'LatencyYellowThreshold', N'',0,0)
INSERT [dbo].[ProfilesMaster] ([ID], [ServerTypeId], [AttributeName], [DefaultValue], [UnitOfMeasurement], [RelatedTable], [RelatedField], [RoleType],[ProfileId],[isSelected]) VALUES (195, 4, N'LatencyRedThreshold', N'10', N'Seconds', N'ServerAttributes', N'LatencyRedThreshold', N'',0,0)
INSERT [dbo].[ProfilesMaster] ([ID], [ServerTypeId], [AttributeName], [DefaultValue], [UnitOfMeasurement], [RelatedTable], [RelatedField], [RoleType],[ProfileId],[isSelected]) VALUES (196, 15, N'LatencyYellowThreshold', N'10', N'Seconds', N'ServerAttributes', N'LatencyYellowThreshold', N'',0,0)
INSERT [dbo].[ProfilesMaster] ([ID], [ServerTypeId], [AttributeName], [DefaultValue], [UnitOfMeasurement], [RelatedTable], [RelatedField], [RoleType],[ProfileId],[isSelected]) VALUES (197, 15, N'LatencyRedThreshold', N'10', N'Seconds', N'ServerAttributes', N'LatencyRedThreshold', N'',0,0)

INSERT [dbo].[ProfilesMaster] ([ID], [ServerTypeId], [AttributeName], [DefaultValue], [UnitOfMeasurement], [RelatedTable], [RelatedField], [RoleType],[ProfileId],[isSelected]) VALUES (198, 16, N'LatencyYellowThreshold', N'10', N'Seconds', N'ServerAttributes', N'LatencyYellowThreshold', N'',0,0)
INSERT [dbo].[ProfilesMaster] ([ID], [ServerTypeId], [AttributeName], [DefaultValue], [UnitOfMeasurement], [RelatedTable], [RelatedField], [RoleType],[ProfileId],[isSelected]) VALUES (199, 16, N'LatencyRedThreshold', N'10', N'Seconds', N'ServerAttributes', N'LatencyRedThreshold', N'',0,0)

INSERT [dbo].[ProfilesMaster] ([ID], [ServerTypeId], [AttributeName], [DefaultValue], [UnitOfMeasurement], [RelatedTable], [RelatedField], [RoleType],[ProfileId],[isSelected]) VALUES (200, 18, N'LatencyYellowThreshold', N'10', N'Seconds', N'ServerAttributes', N'LatencyYellowThreshold', N'',0,0)
INSERT [dbo].[ProfilesMaster] ([ID], [ServerTypeId], [AttributeName], [DefaultValue], [UnitOfMeasurement], [RelatedTable], [RelatedField], [RoleType],[ProfileId],[isSelected]) VALUES (201, 18, N'LatencyRedThreshold', N'10', N'Seconds', N'ServerAttributes', N'LatencyRedThreshold', N'',0,0)

INSERT [dbo].[ProfilesMaster] ([ID], [ServerTypeId], [AttributeName], [DefaultValue], [UnitOfMeasurement], [RelatedTable], [RelatedField], [RoleType],[ProfileId],[isSelected]) VALUES (202, 21, N'LatencyYellowThreshold', N'10', N'Seconds', N'ServerAttributes', N'LatencyYellowThreshold', N'',0,0)
INSERT [dbo].[ProfilesMaster] ([ID], [ServerTypeId], [AttributeName], [DefaultValue], [UnitOfMeasurement], [RelatedTable], [RelatedField], [RoleType],[ProfileId],[isSelected]) VALUES (203, 21, N'LatencyRedThreshold', N'10', N'Seconds', N'ServerAttributes', N'LatencyRedThreshold', N'',0,0)

INSERT [dbo].[ProfilesMaster] ([ID], [ServerTypeId], [AttributeName], [DefaultValue], [UnitOfMeasurement], [RelatedTable], [RelatedField] ,[RoleType],[ProfileId],[isSelected]) VALUES (204, 1, N'Enable Servlet Scan', N'1', N'True/False (1- True, 0-False)', N'DominoServers', N'ScanServlet','',0,0)

INSERT [dbo].[ProfilesMaster] ([ID], [ServerTypeId], [AttributeName], [DefaultValue], [UnitOfMeasurement], [RelatedTable], [RelatedField] ,[RoleType],[ProfileId],[isSelected]) VALUES (205, 1, N'Scan Log.nsf', N'0', N'True/False (1- True, 0-False)', N'DominoServers', N'scanlog','',0,0)
INSERT [dbo].[ProfilesMaster] ([ID], [ServerTypeId], [AttributeName], [DefaultValue], [UnitOfMeasurement], [RelatedTable], [RelatedField] ,[RoleType],[ProfileId],[isSelected]) VALUES (206, 1, N'Scan AgentLog.nsf', N'0', N'True/False (1- True, 0-False)', N'DominoServers', N'scanagentlog','',0,0)
INSERT [dbo].[ProfilesMaster] ([ID], [ServerTypeId], [AttributeName], [DefaultValue], [UnitOfMeasurement], [RelatedTable], [RelatedField] ,[RoleType],[ProfileId],[isSelected]) VALUES (207, 1, N'Load Cluster Replication Delays Threshold', N'10', N'Minutes', N'DominoServers', N'Load_Cluster_Rep_Delays_Threshold','',0,0)
/* 6/30/2015 NS added for VSPLUS-1802 */
INSERT [dbo].[ProfilesMaster] ([ID], [ServerTypeId], [AttributeName], [DefaultValue], [UnitOfMeasurement], [RelatedTable], [RelatedField], [RoleType], [ProfileId], [isSelected]) VALUES (208, 1, N'Enabled for EXJournal Scanning', N'1', N'True/False (1- True, 0-False)', N'DominoServers', N'EXJEnabled', N'', 0, 0)
INSERT [dbo].[ProfilesMaster] ([ID], [ServerTypeId], [AttributeName], [DefaultValue], [UnitOfMeasurement], [RelatedTable], [RelatedField], [RoleType], [ProfileId], [isSelected]) VALUES (209, 1, N'EXJournal Look Back Start Time', N'8:00 AM', N'string', N'DominoServers', N'EXJStartTime', N'', 0, 0)
/* 10/16/2015 NS modified for VSPLUS-2272 */
INSERT [dbo].[ProfilesMaster] ([ID], [ServerTypeId], [AttributeName], [DefaultValue], [UnitOfMeasurement], [RelatedTable], [RelatedField], [RoleType], [ProfileId], [isSelected]) VALUES (210, 1, N'EXJournal Look Back Duration', N'120', N'Minutes', N'DominoServers', N'EXJDuration', N'', 0, 0)
INSERT [dbo].[ProfilesMaster] ([ID], [ServerTypeId], [AttributeName], [DefaultValue], [UnitOfMeasurement], [RelatedTable], [RelatedField], [RoleType], [ProfileId], [isSelected]) VALUES (211, 1, N'EXJournal Look Back Period', N'10', N'Minutes', N'DominoServers', N'EXJLookBackDuration', N'', 0, 0)
--Somaraju VSPLUS 2733
INSERT [dbo].[ProfilesMaster] ([ID], [ServerTypeId], [AttributeName], [DefaultValue], [UnitOfMeasurement], [RelatedTable], [RelatedField], [RoleType], [ProfileId], [isSelected]) VALUES (212, 10, N'MonthlyOperatingCost', N'0', N'count', N'Servers', N'MonthlyOperatingCost', N'', 0, 0)
INSERT [dbo].[ProfilesMaster] ([ID], [ServerTypeId], [AttributeName], [DefaultValue], [UnitOfMeasurement], [RelatedTable], [RelatedField], [RoleType], [ProfileId], [isSelected]) VALUES (213, 10, N'IdealUserCount', N'0', N'count', N'Servers', N'IdealUserCount', N'', 0, 0)
--Somaraju VSPLUS-2730
INSERT [dbo].[ProfilesMaster] ([ID], [ServerTypeId], [AttributeName], [DefaultValue], [UnitOfMeasurement], [RelatedTable], [RelatedField], [RoleType], [ProfileId], [isSelected]) VALUES (214, 5, N'Http', N'http://', N'string', N'Servers', N'IPAddress', N'', 0, NULL)
INSERT [dbo].[ProfilesMaster] ([ID], [ServerTypeId], [AttributeName], [DefaultValue], [UnitOfMeasurement], [RelatedTable], [RelatedField], [RoleType], [ProfileId], [isSelected]) VALUES (215, 5, N'HttpSecure', N'https://', N'string', N'Servers', N'IPAddress', N'', 0, NULL)
SET IDENTITY_INSERT [dbo].[ProfilesMaster] OFF

/*************************End  of ProfilesMaster Added data for URL, Sametime, etc., ********************************************/

/************************* Start of ScanResults Added for DBHealth Server for multithreaded *************************************/
/* 12/17/2013 NS commented out - the table is getting created in 1.1 */



USE [VSS_Statistics]
GO
IF NOT EXISTS (select * from syscolumns where id = object_id('dbo.ScanResults'))
BEGIN
	CREATE TABLE [dbo].[ScanResults](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[ScanDate] [datetime] NULL,
	[ServerName] [nvarchar](250) NULL,
	[ScanCount] [int] NULL,
	[DatabaseCount] [int] NULL)
END

IF NOT EXISTS (select * from syscolumns where name= 'Temp' and id = object_id('dbo.Daily'))
BEGIN
	Alter TABLE [dbo].[Daily] Add Temp Bit NULL
END
GO


/************************* End of ScanResults Added for DBHealth Server for multithreaded *************************************/


/*********************************************Start  of Daily Task Cleanup ********************************************/
USE [VSS_Statistics]
GO

IF NOT EXISTS (select * from syscolumns where id = object_id('dbo.ConsolidationResults'))
BEGIN
	CREATE TABLE [dbo].[ConsolidationResults](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[ScanDate] [datetime] NULL,
	[Result] [nvarchar](250) NULL)
END
GO



/* 12/16/2013 NS added GO statement after each command below, otherwise, the server complains */
USE VSS_Statistics
GO
IF EXISTS(select * from syscolumns where id = object_id('dbo.DeviceDailyStats') and name='DeviceName')
BEGIN
	EXEC SP_RENAME 'DeviceDailyStats.DeviceName' , 'ServerName'
END

USE [vitalsigns]
GO

/* Delete the data for BlackBerryProbeStats and NotesMailStats as we are not going to use this table anymore... */
IF Exists(select * from syscolumns where id = object_id('dbo.DailyTasks'))
BEGIN
	delete from dailytasks where SourceTableName in ('BlackBerryProbeStats', 'NotesMailStats')
	delete from dailytasks where SourceTableName = 'DeviceDailyStats' and SourceStatName = 'DeviceType'
END

PRINT N'Executing V1.2 upgrade scripts .........'

/************** PLACE HOLDER TO PUT UPGRADE SCRIPTS IF ANY FOR V 1.2 ************/ 
/* 12/23/2013 NS added - delete FK from the AlertServers and AlertEvents tables */
USE [vitalsigns]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_AlertEvents_EventsMaster]') AND parent_object_id = OBJECT_ID(N'[dbo].[AlertServers]'))
BEGIN
	ALTER TABLE [dbo].[AlertServers] DROP CONSTRAINT [FK_AlertEvents_EventsMaster]
END
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_AlertServers_Servers]') AND parent_object_id = OBJECT_ID(N'[dbo].[AlertServers]'))
BEGIN
	ALTER TABLE [dbo].[AlertServers] DROP CONSTRAINT [FK_AlertServers_Servers]
END
GO

PRINT N'Executing V1.2.1 upgrade scripts .........';

/************** PLACE HOLDER TO PUT UPGRADE SCRIPTS IF ANY FOR V 1.2.1 ************/ 


/* 1/10/2014 NS added set columns to not NULL before setting the PK */
IF EXISTS (select * from syscolumns where name= 'SourceTableName' and id = object_id('dbo.DailyTasks'))
BEGIN	
	ALTER TABLE DailyTasks ALTER COLUMN SourceTableName VARCHAR(150) NOT NULL
END
IF EXISTS (select * from syscolumns where name= 'SourceAggregation' and id = object_id('dbo.DailyTasks'))
BEGIN
	ALTER TABLE DailyTasks ALTER COLUMN SourceAggregation VARCHAR(150) NOT NULL
END
IF EXISTS (select * from syscolumns where name= 'SourceStatName' and id = object_id('dbo.DailyTasks'))
BEGIN
	ALTER TABLE DailyTasks ALTER COLUMN SourceStatName VARCHAR(150) NOT NULL
END

/* 1/9/2014 NS added PK for DailyTasks */
USE [vitalsigns]
GO

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS WHERE CONSTRAINT_TYPE = 'PRIMARY KEY' AND TABLE_NAME = 'DailyTasks')
BEGIN
	ALTER TABLE DailyTasks ADD PRIMARY KEY(SourceTableName,SourceAggregation,SourceStatName)
END
GO

USE [vitalsigns]
GO
IF EXISTS(select * from syscolumns where id=Object_id('dbo.AlertDefinitions'))
BEGIN	
	drop table dbo.[AlertDefinitions]
--drop table dbo.[AlertDeviceTypes]
END
IF EXISTS(select * from syscolumns where id=Object_id('dbo.AlertExceptions'))
BEGIN	
	drop table dbo.[AlertExceptions]
--drop table dbo.[AlertLocations]
END
IF EXISTS(select * from syscolumns where id=Object_id('dbo.Apointments'))
BEGIN	
	drop table dbo.[Appointments]
END
IF EXISTS(select * from syscolumns where id=Object_id('dbo.BBHistory'))
BEGIN	
	drop table dbo.[BBHistory]
END
IF EXISTS(select * from syscolumns where id=Object_id('dbo.BlackBerry Users'))
BEGIN
	drop table dbo.[BlackBerry Users]
END
IF EXISTS(select * from syscolumns where id=Object_id('dbo.DNS_Servers'))
BEGIN	
	drop table dbo.[DNS_Servers]
END
IF EXISTS(select * from syscolumns where id=Object_id('dbo.Resources'))
BEGIN	
	drop table dbo.[Resources]
END
GO

PRINT N'Executing V1.2.2 upgrade scripts .........';
Use vitalsigns
IF NOT EXISTS (select * from syscolumns where name= 'HA' and id = object_id('dbo.Traveler_Status'))
BEGIN	
	ALTER TABLE dbo.Traveler_Status ADD HA BIT
END
GO

IF NOT EXISTS (select * from syscolumns where name= 'TravelerServlet' and id = object_id('dbo.Traveler_Status'))
BEGIN	
	ALTER TABLE Traveler_Status ADD TravelerServlet varchar(255)
END
GO

IF NOT EXISTS (select * from syscolumns where name= 'HA_Datastore_Status' and id = object_id('dbo.Traveler_Status'))
BEGIN	
	ALTER TABLE Traveler_Status ADD HA_Datastore_Status varchar(50)
END
GO

IF EXISTS (select * from syscolumns where id = object_id('dbo.[MenuItems]'))
BEGIN
	UPDATE [MenuItems] SET [ImageURL] = '~/images/icons/dominoserver.gif'
	WHERE [DisplayText] = 'IBM Domino Settings'
END
GO
IF EXISTS (select * from syscolumns where id = object_id('dbo.[MenuItems]'))
BEGIN
	UPDATE [MenuItems] SET [ImageURL] = '~/images/icons/sametime.gif'
	WHERE [DisplayText] = 'IBM Sametime Settings'
END
GO
IF EXISTS (select * from syscolumns where id = object_id('dbo.[MenuItems]'))
BEGIN
	UPDATE [MenuItems] SET [ImageURL] = '~/images/icons/exchange.jpg'
	WHERE [DisplayText] = 'Microsoft Exchange Settings'
END
GO

DELETE from MenuItems where DisplayText= 'Notes Traveler Data Store'
Go

IF OBJECTPROPERTY(OBJECT_ID('dbo.MenuItems'), 'TableHasIdentity') = 1 
BEGIN
	SET IDENTITY_INSERT [dbo].[MenuItems] ON
END 
INSERT INTO [MenuItems]([ID],[DisplayText],[OrderNum],[ParentID],[PageLink],[Level],[RefName],[ImageURL]) VALUES(60,'Notes Traveler Data Store',1,5,'/Configurator/TravelerDataStore.aspx',2,'TravelerDataStore','~/images/icons/traveler-icon.png')
IF OBJECTPROPERTY(OBJECT_ID('dbo.MenuItems'), 'TableHasIdentity') = 1 
BEGIN
	SET IDENTITY_INSERT [dbo].[MenuItems] OFF
END 

IF EXISTS (select * from MenuItems WHERE DisplayText='Server Settings Editor')
BEGIN
	UPDATE [MenuItems] SET [PageLink] = '/Security/ServerSettingsEditor.aspx'
	WHERE [DisplayText] = 'Server Settings Editor'
END
GO


/* Joe - Delete if already AlertsOn record exists, as it failed ontop of 1.2.1 release.*/
/* 11/12/2015 NS commented out for VSPLUS-2310*/
	/* DELETE from [dbo].Settings where sname = 'AlertsOn' */
/* 1/6/2014 NS added - AlertsOn will store the True/False value */
/* 11/12/2015 NS commented out for VSPLUS-2310*/
	/* INSERT [dbo].[Settings] ([sname], [svalue], [stype]) VALUES (N'AlertsOn', N'True', N'System.String')	*/
/* 11/12/2015 NS added for VSPLUS-2310 */
IF NOT EXISTS (select * from Settings WHERE sname='AlertsOn')
BEGIN
	INSERT [dbo].[Settings] ([sname], [svalue], [stype]) VALUES (N'AlertsOn', N'True', N'System.String')
END
ELSE
	IF (select svalue from Settings WHERE sname='AlertsOn') != 'True' AND (select svalue from Settings WHERE sname='AlertsOn') != 'False'
	BEGIN
		DELETE from [dbo].Settings where sname = 'AlertsOn'
		INSERT [dbo].[Settings] ([sname], [svalue], [stype]) VALUES (N'AlertsOn', N'True', N'System.String')
	END
GO


/* 6/17/2016 AF added for VSPLUS-3073 */
IF EXISTS( select * from syscolumns where id=object_id('dbo.EventsMaster'))
BEGIN
	SET IDENTITY_INSERT [dbo].[EventsMaster] ON
	DELETE from [dbo].[EventsMaster] where EventName = 'Mailbox'
	INSERT INTO [dbo].[EventsMaster] ([ID], [EventName], [ServerTypeID]) VALUES (185, N'Mailbox', 1)
	SET IDENTITY_INSERT [dbo].[EventsMaster] OFF
END
GO


/* 1/22/2014 NS added for VSPLUS-191 */
IF EXISTS( select * from syscolumns where id=object_id('dbo.EventsMaster'))
BEGIN
	SET IDENTITY_INSERT [dbo].[EventsMaster] ON
	DELETE from [dbo].[EventsMaster] where EventName = 'Telnet'
	INSERT INTO [dbo].[EventsMaster] ([ID], [EventName], [ServerTypeID]) VALUES (54, N'Telnet', 1)
	SET IDENTITY_INSERT [dbo].[EventsMaster] OFF
END
GO


-- VSPLUS-272 Login - Go to Dashboard as default
IF NOT EXISTS (select * from syscolumns where name= 'StartupURL' and id = object_id('dbo.USERS'))
BEGIN	
	ALTER TABLE dbo.USERS ADD StartupURL VARCHAR(100) NULL
END
GO

Use vitalsigns
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

/****** Object:  Table [dbo].[Traveler_HA_Datastore]    Script Date: 01/24/2014 09:55:34 ******/
/* 1/24/2014 NS added new table [Traveler_HA_Datastore] */
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO
IF NOT EXISTS (select * from syscolumns where id = object_id('dbo.Traveler_HA_Datastore'))
BEGIN	
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
	([ID] ASC)
	WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
	) ON [PRIMARY]
END
GO

-- we can add more here

	DELETE FROM dbo.UsersStartupURLs WHERE URL='~/Configurator/Default.aspx'
	INSERT INTO dbo.UsersStartupURLs(URL,Name,IsDashboard,IsConfigurator,IsConsoleComm) VALUES('~/Configurator/Default.aspx','Default Page',1,1,1)
	
	DELETE FROM dbo.UsersStartupURLs WHERE URL='~/Configurator/LotusDominoServers.aspx'
	INSERT INTO dbo.UsersStartupURLs(URL,Name,IsDashboard,IsConfigurator,IsConsoleComm)  VALUES('~/Configurator/LotusDominoServers.aspx','Domino Servers',0,1,0)
	
	DELETE FROM dbo.UsersStartupURLs WHERE URL='~/Configurator/LotusSametimeGrid.aspx'
	INSERT INTO dbo.UsersStartupURLs(URL,Name,IsDashboard,IsConfigurator,IsConsoleComm)  VALUES('~/Configurator/LotusSametimeGrid.aspx','Sametime Servers',0,1,0)

	DELETE FROM dbo.UsersStartupURLs WHERE URL='~/Configurator/URLsGrid.aspx'
	INSERT INTO dbo.UsersStartupURLs(URL,Name,IsDashboard,IsConfigurator,IsConsoleComm)  VALUES('~/Configurator/URLsGrid.aspx','URLs',0,1,0)

	DELETE FROM dbo.UsersStartupURLs WHERE URL='~/Dashboard/OverallHealth1.aspx'
	INSERT INTO dbo.UsersStartupURLs(URL,Name,IsDashboard,IsConfigurator,IsConsoleComm)  VALUES('~/Dashboard/OverallHealth1.aspx','Dashboard',1,0,0)

	DELETE FROM dbo.UsersStartupURLs WHERE URL='~/Dashboard/SummaryLandscape.aspx'
	INSERT INTO dbo.UsersStartupURLs(URL,Name,IsDashboard,IsConfigurator,IsConsoleComm)  VALUES('~/Dashboard/SummaryLandscape.aspx','Executive Summary',1,0,0)

	DELETE FROM dbo.UsersStartupURLs WHERE URL='~/Dashboard/SummaryEXJournal.aspx'
	INSERT INTO dbo.UsersStartupURLs(URL,Name,IsDashboard,IsConfigurator,IsConsoleComm)  VALUES('~/Dashboard/SummaryEXJournal.aspx','ExJournal Summary',1,0,0)

	DELETE FROM dbo.UsersStartupURLs WHERE URL='~/Dashboard/MailDeliveryStatus.aspx'
	INSERT INTO dbo.UsersStartupURLs(URL,Name,IsDashboard,IsConfigurator,IsConsoleComm)  VALUES('~/Dashboard/MailDeliveryStatus.aspx','Mail Delivery Status',1,0,0)

	
	DELETE FROM dbo.UsersStartupURLs WHERE URL='~/Dashboard/DeviceTypeList.aspx'
	INSERT INTO dbo.UsersStartupURLs(URL,Name,IsDashboard,IsConfigurator,IsConsoleComm)  VALUES('~/Dashboard/DeviceTypeList.aspx','Status List',1,0,0)
	GO

IF NOT EXISTS (select * from syscolumns where name= 'ServerDaysAlert' and id = object_id('dbo.DominoServers'))
BEGIN	
	ALTER TABLE dbo.DominoServers ADD ServerDaysAlert int
END
GO

SET ANSI_PADDING OFF
GO

/* 1/24/2014 NS added - Domino HTTP User and Domino HTTP Password */
/* 3/4/2014 AF added IF condition to preserver prior values*/

/* 8/14/2014 NS removed for VSPLUS-856 - Domino HTTP User and Domino HTTP Password, 
now use multiple credentials for Traveler */
/*
IF NOT EXISTS(select * from Settings where sname = 'Domino HTTP User')
BEGIN
INSERT [dbo].[Settings] ([sname], [svalue], [stype]) VALUES (N'Domino HTTP Password', N'', N'System.Byte[]')
INSERT [dbo].[Settings] ([sname], [svalue], [stype]) VALUES (N'Domino HTTP User', N'', N'System.String')
END
*/

Print N' Executing V1.2.3 upgrade scripts ......... '
Use VSS_Statistics
go
IF NOT EXISTS(SELECT * from sys.columns WHERE Name = N'FolderCount' and Object_ID = Object_ID(N'Daily'))
BEGIN
	Alter Table Daily Add FolderCount INT NULL
END


/*somaraju,niranjan 2/12/14*/
DELETE from [vitalsigns].[dbo].[Settings] where sname = 'EnableLatencyReport' 
INSERT [vitalsigns].[dbo].[Settings] ([sname]) VALUES (N'EnableLatencyReport')
DELETE from [vitalsigns].[dbo].[Settings] where sname = 'LatencyScanInterval' 
INSERT [vitalsigns].[dbo].[Settings] ([sname]) VALUES (N'LatencyScanInterval')

/*Mukund: used for RPR's internal pages access.*/
DELETE from [vitalsigns].[dbo].[Settings] where sname = 'RPR Access Pwd' 
INSERT INTO [vitalsigns].[dbo].[Settings] ([sname] ,[svalue] ,[stype]) VALUES ('RPR Access Pwd' ,'Admin!23' ,'System.String')
USE [vitalsigns]
GO
IF NOT EXISTS(select * from syscolumns where id=object_id('dbo.RPRAccessPages'))
BEGIN
	CREATE TABLE [dbo].[RPRAccessPages](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](250) NULL,
	[Category] [nvarchar](250) NULL,
	[Description] [nvarchar](250) NULL,
	[PageURL] [nvarchar](500) NULL,
	[ImageURL] [nvarchar](250) NULL)
END	
/*=======================================.*/
/* 1/30/2014 NS added for VSPLUS-322 - Sametime Servlet Port */

USE vitalsigns
GO
DELETE from dbo.Settings where sname = 'Sametime Servlet Port'
go
INSERT INTO [vitalsigns].[dbo].[Settings] ([sname], [svalue], [stype]) VALUES (N'Sametime Servlet Port', N'8088', N'System.String')
go


use vitalsigns
go

IF NOT EXISTS(select * from syscolumns where id=object_id('[ExchangeServerHealth]'))
BEGIN	
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
END
GO
	
IF NOT EXISTS(select * from syscolumns where id=object_id('[ExchangeMailBoxReport]'))
BEGIN	
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
END
GO

IF NOT EXISTS(select * from syscolumns where id=object_id('[WindowsServices]'))	
BEGIN	
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
END
GO




/*=======================================.*/



IF exists (select * from dbo.sysobjects where id = object_id('dbo.AddAllAuditTrailTriggers'))
BEGIN
-- exec stored proc on every run to create ALL audit trail triggers of new tables 
	EXECUTE dbo.AddAllAuditTrailTriggers 
END
go

-- Creation of ServerNodes table
USE [vitalsigns]
GO
IF NOT EXISTS(select * from syscolumns where id=object_id('[ServerNodes]'))	
BEGIN
	CREATE TABLE [dbo].[ServerNodes](
	[NodeID] [int] NOT NULL,
	[NodeHostName] [varchar](50) NOT NULL,
	[NodeIPAddress] [varchar](150) NOT NULL,
	[NodeDescription] [varchar](50) NOT NULL,
	PRIMARY KEY(NodeID));
END
GO

USE [vitalsigns]
GO
--Insert MoniteredBy in DominoServers	
IF NOT EXISTS(select * from syscolumns where Name = N'MonitoredBy' and id=object_id('[DominoServers]'))	 
BEGIN
	ALTER TABLE DominoServers
	ADD [MonitoredBy] int NULL

	ALTER TABLE DominoServers
	ADD FOREIGN KEY ([MonitoredBy])
	REFERENCES ServerNodes (NodeID)
END
GO

USE [vitalsigns]
GO
--Insert MoniteredBy in SametimeServers	
IF NOT EXISTS(select * from syscolumns where Name = N'MonitoredBy' and id=object_id('[SametimeServers]'))
BEGIN
	ALTER TABLE SametimeServers
	ADD [MonitoredBy] int NULL

	ALTER TABLE SametimeServers
	ADD FOREIGN KEY ([MonitoredBy])
	REFERENCES ServerNodes (NodeID)
END
GO

--Add Menu Item High Availability

USE [vitalsigns]
go
	DELETE FROM MenuItems where DisplayText='High Availability'
	go

IF OBJECTPROPERTY(OBJECT_ID('dbo.MenuItems'), 'TableHasIdentity') = 1 
BEGIN
	SET IDENTITY_INSERT [dbo].[MenuItems] ON
END 	
	INSERT INTO MenuItems([ID], [DisplayText], [OrderNum], [ParentID], [PageLink], [Level], [RefName], [ImageURL]) VALUES(61,'High Availability',8,10,'/Security/HighAvailability.aspx',2,'HighAvailability','')
	
IF OBJECTPROPERTY(OBJECT_ID('dbo.MenuItems'), 'TableHasIdentity') = 1 
BEGIN
	SET IDENTITY_INSERT [dbo].[MenuItems] OFF
END 


USE [vitalsigns]
GO

IF NOT EXISTS(select * from syscolumns where id=object_id('[ExchangeDAGHealthCopyStatus]'))	
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

IF NOT EXISTS(select * from syscolumns where id=object_id('[ExchangeDAGHealthCopySummary]'))	
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

IF NOT EXISTS(select * from syscolumns where id=object_id('[ExchangeDAGHealthMemberReport]'))	
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

IF NOT EXISTS(SELECT * from sys.columns WHERE Name = N'MonitoredBy' and Object_ID = Object_ID(N'Servers'))
BEGIN	
	ALTER TABLE Servers
	ADD [MonitoredBy] int NULL
	
	ALTER TABLE Servers
	ADD FOREIGN KEY ([MonitoredBy])
	REFERENCES ServerNodes (NodeID)
	
END
GO

USE [vitalsigns]
GO
IF NOT EXISTS(SELECT * from sys.columns WHERE Name = N'MonitoredBy' and Object_ID = Object_ID(N'Urls'))
BEGIN
	ALTER TABLE Urls
	ADD [MonitoredBy] int NULL
		
	ALTER TABLE Urls
	ADD FOREIGN KEY ([MonitoredBy])
	REFERENCES ServerNodes (NodeID)
END
GO

USE [vitalsigns]
GO
IF NOT EXISTS(SELECT * from sys.columns WHERE Name = N'ServerTypeTable' and Object_ID = Object_ID(N'ServerTypes'))
BEGIN	
	ALTER TABLE ServerTypes
	ADD ServerTypeTable nvarchar(50)
END
GO
--Durga VSPLUS-2410
USE [vitalsigns]
GO
IF NOT EXISTS(SELECT * from ServerTypes WHERE ServerType = N'General')
BEGIN	
	Insert into ServerTypes(ID,ServerType,ServerTypeTable) values(10,'General','ServerAttributes')
END
GO
select * from ServerTypes
USE [vitalsigns]
GO
IF EXISTS(SELECT * FROM syscolumns where id=Object_id('ServerTypes'))
BEGIN	
	UPDATE ServerTypes SET ServerTypeTable = 'DominoServers' WHERE ServerType = 'Domino'
	UPDATE ServerTypes SET ServerTypeTable = '' WHERE ServerType = 'BES'
	UPDATE ServerTypes SET ServerTypeTable = 'SametimeServers' WHERE ServerType = 'Sametime'
	UPDATE ServerTypes SET ServerTypeTable = '' WHERE ServerType = 'SharePoint'
	UPDATE ServerTypes SET ServerTypeTable = 'ExchangeSettings' WHERE ServerType = 'Exchange'
	UPDATE ServerTypes SET ServerTypeTable = 'MailServices' WHERE ServerType = 'Mail'
	UPDATE ServerTypes SET ServerTypeTable = 'URLs' WHERE ServerType = 'URL'
	UPDATE ServerTypes SET ServerTypeTable = 'Network Devices' WHERE ServerType = 'Network Device'
	UPDATE ServerTypes SET ServerTypeTable = 'NotesDatabases' WHERE ServerType = 'Notes Database'
	UPDATE ServerTypes SET ServerTypeTable = 'ServerAttributes' WHERE ServerType = 'General'
	UPDATE ServerTypes SET ServerTypeTable = 'DagSettings' WHERE ServerType = 'Database Availability Group'
END
GO

use vitalsigns
go
Delete from [dbo].[Settings] where sname = 'Log Level'
INSERT [dbo].[Settings] ([sname], [svalue], [stype]) VALUES (N'Log Level', N'2', N'System.Int32')
GO

PRINT N' Executing V1.2.4 upgrade scripts ......... ';

IF NOT EXISTS (select * from Settings where sname= 'Enable Domino Console Commands' )
BEGIN	
	INSERT [dbo].[Settings] ([sname], [svalue], [stype]) VALUES (N'Enable Domino Console Commands', N'True', N'System.String')
END

GO

/* 2/12/2014 NS added two new events for Domino */
use vitalsigns
go
IF NOT EXISTS (select * from EventsMaster where EventName='Traveler Status Yellow' )
BEGIN	
	SET IDENTITY_INSERT [dbo].[EventsMaster] ON
	INSERT [dbo].[EventsMaster] ([ID], [EventName], [ServerTypeID]) VALUES (55, N'Traveler Status Yellow', 1)
	SET IDENTITY_INSERT [dbo].[EventsMaster] OFF
END
GO


IF NOT EXISTS (select * from EventsMaster where EventName='Traveler Status Red' )
BEGIN	
	SET IDENTITY_INSERT [dbo].[EventsMaster] ON
	INSERT [dbo].[EventsMaster] ([ID], [EventName], [ServerTypeID]) VALUES (56, N'Traveler Status Red', 1)
	SET IDENTITY_INSERT [dbo].[EventsMaster] OFF
END
GO

/* 2/19/2014 AF added new column for Traveler Health */
USE [vitalsigns]
GO
IF NOT EXISTS (select * from syscolumns where name= 'HeartBeat' and id = object_id('dbo.Traveler_Status'))
BEGIN
	Alter TABLE [dbo].[Traveler_Status]Add [HeartBeat] [varchar](50) NULL
END 

Go 
IF NOT EXISTS (select * from EventsMaster where EventName='Traveler Servlet' )
BEGIN	
	SET IDENTITY_INSERT [dbo].[EventsMaster] ON
	INSERT [dbo].[EventsMaster] ([ID], [EventName], [ServerTypeID]) VALUES (57, N'Traveler Servlet', 1)
	SET IDENTITY_INSERT [dbo].[EventsMaster] OFF
END
GO

--Chandrahas: Added new column GridRowCount to Users Table
USE [vitalsigns]
GO
IF NOT EXISTS (select * from syscolumns where name= 'GridRowCount' and id = object_id('dbo.Users'))
BEGIN
	ALTER TABLE Users 
	ADD [GridRowCount] [INT] DEFAULT 10
END
GO

--Mukund 20Feb14 for Notes dashboard display form
USE [vitalsigns]
GO
delete from Settings where sname='ScanNotesMailProbeASAP'
go
INSERT [dbo].[Settings] ([sname], [svalue], [stype]) Values (N'ScanNotesMailProbeASAP',N'N/A',N'System.String')
go


IF NOT EXISTS(select * from syscolumns where id=object_id('[MSServerSettings]'))     
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

IF NOT EXISTS(select * from syscolumns where id=object_id('[ServerThresholds]'))     
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

IF NOT EXISTS(select * from syscolumns where id=object_id('[ExchangeQueues]'))     
CREATE TABLE [dbo].[ExchangeQueues](
	[ServerName] [nvarchar](50) NULL,
	[InfoDate] [datetime] NULL,
	[IdentityName] [nvarchar](50) NULL,
	[IdentityAction] [nvarchar](50) NULL,
	[DeliveryType] [nvarchar](50) NULL,
	[Status] [nvarchar](50) NULL,
	[MessageCount] [nvarchar](50) NULL,
	[NextHopDomain] [nvarchar](50) NULL
) ON [PRIMARY]

GO

IF NOT EXISTS (select * from syscolumns where name= 'AliasName' and id = object_id('dbo.Credentials'))
BEGIN
	Alter TABLE [dbo].[Credentials] Add AliasName varchar(250) NULL
END


IF NOT EXISTS (select * from syscolumns where name= 'UserID' and id = object_id('dbo.Credentials'))
BEGIN
	Alter TABLE [dbo].[Credentials] Add UserID varchar(500) NULL
END

IF EXISTS (select * from syscolumns where name= 'ServerID' and id = object_id('dbo.Credentials'))
BEGIN
	Alter TABLE [dbo].[Credentials] Drop Column ServerID
END

/* 2/26/2014 NS modified for VSPLUS-36 */
USE [vitalsigns]
GO
IF NOT EXISTS(select * from syscolumns where id=object_id('[ScheduledReports]'))  
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

/* 2/26/2014 NS added for VSPLUS-36 */
IF NOT EXISTS (select * from syscolumns where id=object_id('[ScheduledReports]') and name='ReportID')
BEGIN
	ALTER TABLE [dbo].[ScheduledReports] Add ReportID int 
END
IF NOT EXISTS (select * from syscolumns where id=object_id('[ScheduledReports]') and name='Days')
BEGIN
	ALTER TABLE [dbo].[ScheduledReports] Add Days varchar(150) 
END
IF NOT EXISTS (select * from syscolumns where id=object_id('[ScheduledReports]') and name='SpecificDay')
BEGIN
	ALTER TABLE [dbo].[ScheduledReports] Add SpecificDay int 
END
IF EXISTS (select * from syscolumns where id = object_id('dbo.ScheduledReports') and name= 'SendTo')
BEGIN
	Alter TABLE [dbo].[ScheduledReports] Alter Column SendTo varchar(250) NOT NULL
END
IF NOT EXISTS (select * from syscolumns where id=object_id('[ScheduledReports]') and name='CopyTo')
BEGIN
	ALTER TABLE [dbo].[ScheduledReports] Add CopyTo varchar(250) NULL
END
IF NOT EXISTS (select * from syscolumns where id=object_id('[ScheduledReports]') and name='BlindCopyTo')
BEGIN
	ALTER TABLE [dbo].[ScheduledReports] Add BlindCopyTo varchar(250) NULL
END
IF EXISTS (select * from syscolumns where id = object_id('dbo.ScheduledReports') and name= 'Title')
BEGIN
	Alter TABLE [dbo].[ScheduledReports] Alter Column Title varchar(250)  
END
IF NOT EXISTS (select * from syscolumns where id=object_id('[ScheduledReports]') and name='Body')
BEGIN
	ALTER TABLE [dbo].[ScheduledReports] Add Body varchar(250) NULL
END
IF NOT EXISTS (select * from syscolumns where id=object_id('[ScheduledReports]') and name='FileFormat')
BEGIN
	ALTER TABLE [dbo].[ScheduledReports] Add FileFormat varchar(10) 
END
IF EXISTS (select * from syscolumns where id = object_id('dbo.ScheduledReports') and name= 'ReportName')
BEGIN
	Alter TABLE [dbo].[ScheduledReports] Drop Column ReportName
END



/*VitalStatus removed, MD 21Feb14*/
USE [vitalsigns]
GO
	DELETE FROM UserMenuRestrictions where MenuID in (Select ID from MenuItems  where DisplayText='VitalStatus')
	DELETE FROM MenuItems where DisplayText='VitalStatus'
GO 


PRINT N' Executing V1.3 upgrade scripts ......... ';

/*NotesMailProbe primary key changed from NotesMailAddress to Name, MD 27Feb14*/
USE [vitalsigns]
GO
if exists(select * FROM sys.key_constraints where name='NotesMailProbe$PrimaryKey')
begin
	ALTER TABLE [dbo].[NotesMailProbe] DROP CONSTRAINT [NotesMailProbe$PrimaryKey]
end
	ALTER TABLE [dbo].[NotesMailProbe]  ALTER COLUMN Name nvarchar(50) NOT NULL
	ALTER TABLE [dbo].[NotesMailProbe] ADD CONSTRAINT [NotesMailProbe$PrimaryKey] PRIMARY KEY (Name)

/* Chandrahas: Updates for Maintenace Table, Menu Item and Creattion of UserPreferences Table*/
USE [vitalsigns]
GO
IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
            WHERE TABLE_NAME = 'Maintenance' 
           AND  COLUMN_NAME = 'Name')
BEGIN
	ALTER TABLE Maintenance
	ALTER COLUMN [Name] [varchar](100) NOT NULL
END

USE [vitalsigns]
GO
IF EXISTS( SELECT * FROM MenuItems where DisplayText = 'Preferences')
BEGIN
UPDATE MenuItems SET PageLink = '~/Configurator/UserPreferences.aspx' where DisplayText = 'Preferences'
END

USE [vitalsigns]
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



--Mukund 04Mar14 VSPLUS-447, Remove Links for PowerShell
USE [vitalsigns]
GO
	DELETE FROM UserMenuRestrictions where MenuID in (Select ID from MenuItems  where DisplayText='PowerShell Scripts')
	delete from menuitems where DisplayText='PowerShell Scripts'
go

--Chandrahas: Delete column GridRowCount to Users Table
USE [vitalsigns]
GO
DECLARE @ConstraintName nvarchar(200)
SELECT @ConstraintName = Name FROM SYS.DEFAULT_CONSTRAINTS
WHERE PARENT_OBJECT_ID = OBJECT_ID('dbo.users')
AND PARENT_COLUMN_ID = (SELECT column_id FROM sys.columns
                        WHERE NAME = N'GridRowCount'
                        AND object_id = OBJECT_ID(N'dbo.users'))
IF @ConstraintName IS NOT NULL
EXEC('ALTER TABLE dbo.users DROP CONSTRAINT ' + @ConstraintName)
GO

USE [vitalsigns]
GO
IF EXISTS (select * from syscolumns where name= 'GridRowCount' and id = object_id('dbo.Users'))
BEGIN
	ALTER TABLE Users 
	DROP COLUMN [GridRowCount]
END
GO


USE [vitalsigns]
GO
IF NOT EXISTS (select * from syscolumns where id = object_id('dbo.DiskSettings'))
BEGIN
create table DiskSettings(
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[ServerID] int NOT NULL,
	[DiskName] [varchar](100) NOT NULL,
	[Threshold] [int] NOT NULL,
	Foreign key (ServerID) references Servers(ID) on delete cascade	
)
END
GO

USE [vitalsigns]
GO
IF NOT EXISTS (select * from syscolumns where id = object_id('dbo.ExchangeSettings'))
BEGIN
create table ExchangeSettings(
	[ServerId] [int] NOT NULL,
	[RoleCAS] [bit] not null default 0,
	[RoleHub] [bit] not null default 0,
	[RoleEdge] [bit] not null default 0,
	[RoleMailBox] [bit] not null default 0,
	[RoleUrl] [bit] not null default 0,
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
	[VersionNo] VARCHAR(20) NOT NULL,
	ActiveSyncCredentialsId int,
	[EnableLatencyTest] [bit] NULL,
	[LatencyYellowThreshold] [int] NULL,
	[LatencyRedThreshold] [int] NULL,
	Foreign key ([ServerId]) references Servers(ID) on delete cascade	
)
END
GO


USE [vitalsigns]
GO
IF NOT EXISTS (select * from syscolumns where id = object_id('dbo.StatusDetail'))
BEGIN
create table StatusDetail(
	[ID] int Identity(1,1) not null primary key,
	[TypeAndName] [nvarchar](255) NOT NULL,
	[Category] [nvarchar](100) NOT NULL,
	[TestName] [nvarchar](100) NULL,
	[Result] [nvarchar](100) NULL,
	[LastUpdate] [datetime] NOT NULL,
	[Details] [varchar](100) NULL
	--CY:Added a named foreign key constraint with on cascade delete
	--foreign key ([TypeAndName]) references Status([TypeANDName])
)
END
GO

/* 10/1/2015 NS modified for VSPLUS-2150 */
USE [vitalsigns]
GO
IF NOT EXISTS (select * from syscolumns where id = object_id('dbo.ClusterDatabaseDetails'))
BEGIN
	create table ClusterDatabaseDetails(
	ID int not null identity(1,1),
	ClusterID int not null,
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
	Foreign key (ClusterID) references DominoCluster(ID)
	)
END
GO

--Mukund 13Mar14: Exchange changes
Declare @Constraint  varchar(max)
declare @Command  varchar(max)
IF EXISTS (select * from syscolumns where name= 'RoleCAS' and id = object_id('dbo.ExchangeSettings'))
BEGIN
set @Constraint=''
set @Command=''
SELECT @Constraint=default_constraints.name FROM  sys.all_columns INNER JOIN sys.tables ON all_columns.object_id = tables.object_id INNER  JOIN  sys.schemas ON tables.schema_id = schemas.schema_id  INNER JOIN sys.default_constraints ON all_columns.default_object_id = default_constraints.object_id WHERE  schemas.name = 'dbo' AND tables.name = 'ExchangeSettings' AND all_columns.name = 'RoleCAS'
if(@Constraint<>'')
begin
	select @Command = 'ALTER TABLE ExchangeSettings drop constraint ' + @Constraint
	execute (@Command)
end
ALTER TABLE [dbo].[ExchangeSettings] Drop Column RoleCAS 
End

IF EXISTS (select * from syscolumns where name= 'RoleHub' and id = object_id('dbo.ExchangeSettings'))
BEGIN
set @Constraint=''
set @Command=''
SELECT @Constraint=default_constraints.name FROM  sys.all_columns INNER JOIN sys.tables ON all_columns.object_id = tables.object_id INNER  JOIN  sys.schemas ON tables.schema_id = schemas.schema_id  INNER JOIN sys.default_constraints ON all_columns.default_object_id = default_constraints.object_id WHERE  schemas.name = 'dbo' AND tables.name = 'ExchangeSettings' AND all_columns.name = 'RoleHub'
if(@Constraint<>'')
begin
	select @Command = 'ALTER TABLE ExchangeSettings drop constraint ' + @Constraint
	execute (@Command)
end
ALTER TABLE [dbo].[ExchangeSettings] Drop Column RoleHub 
End

IF EXISTS (select * from syscolumns where name= 'RoleEdge' and id = object_id('dbo.ExchangeSettings'))
BEGIN
set @Constraint=''
set @Command=''
SELECT @Constraint=default_constraints.name FROM  sys.all_columns INNER JOIN sys.tables ON all_columns.object_id = tables.object_id INNER  JOIN  sys.schemas ON tables.schema_id = schemas.schema_id  INNER JOIN sys.default_constraints ON all_columns.default_object_id = default_constraints.object_id WHERE  schemas.name = 'dbo' AND tables.name = 'ExchangeSettings' AND all_columns.name = 'RoleEdge'
if(@Constraint<>'')
begin
	select @Command = 'ALTER TABLE ExchangeSettings drop constraint ' + @Constraint
	execute (@Command)
end
ALTER TABLE [dbo].[ExchangeSettings] Drop Column RoleEdge 
End

IF EXISTS (select * from syscolumns where name= 'RoleMailBox' and id = object_id('dbo.ExchangeSettings'))
BEGIN
set @Constraint=''
set @Command=''
SELECT @Constraint=default_constraints.name FROM  sys.all_columns INNER JOIN sys.tables ON all_columns.object_id = tables.object_id INNER  JOIN  sys.schemas ON tables.schema_id = schemas.schema_id  INNER JOIN sys.default_constraints ON all_columns.default_object_id = default_constraints.object_id WHERE  schemas.name = 'dbo' AND tables.name = 'ExchangeSettings' AND all_columns.name = 'RoleMailBox'
if(@Constraint<>'')
begin
	select @Command = 'ALTER TABLE ExchangeSettings drop constraint ' + @Constraint
	execute (@Command)
end
ALTER TABLE [dbo].[ExchangeSettings] Drop Column RoleMailBox 
End

IF EXISTS (select * from syscolumns where name= 'RoleUrl' and id = object_id('dbo.ExchangeSettings'))
BEGIN
set @Constraint=''
set @Command=''
SELECT @Constraint=default_constraints.name FROM  sys.all_columns INNER JOIN sys.tables ON all_columns.object_id = tables.object_id INNER  JOIN  sys.schemas ON tables.schema_id = schemas.schema_id  INNER JOIN sys.default_constraints ON all_columns.default_object_id = default_constraints.object_id WHERE  schemas.name = 'dbo' AND tables.name = 'ExchangeSettings' AND all_columns.name = 'RoleUrl'
if(@Constraint<>'')
begin
	select @Command = 'ALTER TABLE ExchangeSettings drop constraint ' + @Constraint
	execute (@Command)
end
ALTER TABLE [dbo].[ExchangeSettings] Drop Column RoleUrl 
End

IF NOT EXISTS (select * from syscolumns where name= 'VersionNo' and id = object_id('dbo.ExchangeSettings'))
BEGIN
ALTER TABLE [dbo].[ExchangeSettings] ADD VersionNo VARCHAR(20) 
End

/****** Object:  Table [dbo].[ServerAttributes]    Script Date: 03/13/2014 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ServerAttributes]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[ServerAttributes](
	[ServerID] [int] NOT NULL,
	[Enabled] [bit] NULL,
	[ScanInterval] [int] NULL,
	[RetryInterval] [int] NULL,
	[OffHourInterval] [int] NULL,
	[Category] [varchar](100) NULL,
	[CPU_Threshold] [float] NULL,
	[MemThreshold] [float] NULL,
	[ResponseTime] [int] NULL,
	[ConsFailuresBefAlert] [int] NULL,
	[ConsOvrThresholdBefAlert] [int] NULL
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[RolesMaster]    Script Date: 03/13/2014  ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RolesMaster]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[RolesMaster](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[RoleName] [varchar](100) NOT NULL,
	[ServerTypeId] [int] NOT NULL
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[ServiceVersionRole]    Script Date: 03/13/2014  ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ServiceVersionRole]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[ServiceVersionRole](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ServiceId] [int] NOT NULL,
	[VersionNo] [varchar](20) NOT NULL,
	[RoleId] [int] NOT NULL
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[ServiceMaster]    Script Date: 03/13/2014 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ServiceMaster]') AND type in (N'U'))
BEGIN
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
END
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[ServerVersions]    Script Date: 03/13/2014 *****/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ServerVersions]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[ServerVersions](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[VersionNo] [varchar](20) NULL
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[ServerServices]    Script Date: 03/13/2014  ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ServerServices]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[ServerServices](
	[ServerId] [int] NOT NULL,
	[SVRId] [int] NOT NULL
) ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[ExchangeSettings]    Script Date: 03/13/2014  ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ExchangeSettings]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[ExchangeSettings](
	[ServerId] [int] NOT NULL,
	[CASSmtp] [bit] NOT NULL DEFAULT ((0)),
	[CASPop3] [bit] NOT NULL DEFAULT ((0)),
	[CASImap] [bit] NOT NULL DEFAULT ((0)),
	[CASOARPC] [bit] NOT NULL DEFAULT ((0)),
	[CASOWA] [bit] NOT NULL DEFAULT ((0)),
	[CASActiveSync] [bit] NOT NULL DEFAULT ((0)),
	[CASEWS] [bit] NOT NULL DEFAULT ((0)),
	[CASECP] [bit] NOT NULL DEFAULT ((0)),
	[CASAutoDiscovery] [bit] NOT NULL DEFAULT ((0)),
	[CASOAB] [bit] NOT NULL DEFAULT ((0)),
	[HubSubQThreshold] [int] NULL,
	[HubPoisonQThreshold] [int] NULL,
	[HubUnReachableQThreshold] [int] NULL,
	[HubTotalQThreshold] [int] NULL,
	[EdgeSubQThreshold] [int] NULL,
	[EdgePosQThreshold] [int] NULL,
	[EdgeUnReachableQThreshold] [int] NULL,
	[EdgeTotalQThreshold] [int] NULL,
	[VersionNo] [varchar](20) NULL,
	[LatencyYellowThreshold] [int] NULL,
	[LatencyRedThreshold] [int] NULL,
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
/****** Object:  ForeignKey [FK__ExchangeS__Serve__24285DB4]    Script Date: 03/13/2014 ******/
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__ExchangeS__Serve__24285DB4]') AND parent_object_id = OBJECT_ID(N'[dbo].[ExchangeSettings]'))
ALTER TABLE [dbo].[ExchangeSettings]  WITH CHECK ADD  CONSTRAINT [FK__ExchangeS__Serve__24285DB4] FOREIGN KEY([ServerId])
REFERENCES [dbo].[Servers] ([ID])
--ON DELETE CASCADE
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__ExchangeS__Serve__24285DB4]') AND parent_object_id = OBJECT_ID(N'[dbo].[ExchangeSettings]'))
ALTER TABLE [dbo].[ExchangeSettings] CHECK CONSTRAINT [FK__ExchangeS__Serve__24285DB4]
GO



--Mukund, Exchange related changes 15Mar14
IF NOT EXISTS (select * from syscolumns where name= 'TestType' and id = object_id('dbo.StatusDetail'))
BEGIN
ALTER TABLE [dbo].[StatusDetail] ADD TestType VARCHAR(20) 
End

IF NOT EXISTS (select * from syscolumns where id = object_id('dbo.ExMailHealth'))
BEGIN
CREATE TABLE [dbo].[ExMailHealth](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[ServerName] [nvarchar](255) NOT NULL,
	[Database] [nvarchar](250) NULL,
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
	[Size] [int] NULL,
	[Whitespace] [int] NULL,
 CONSTRAINT [PK_ExMailHealth] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END



USE [VitalSigns]
GO
IF NOT EXISTS (select * from syscolumns where id = object_id('dbo.ExgMailHealth'))
BEGIN
CREATE TABLE [dbo].[ExgMailHealth](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ServerName] [varchar](200) NOT NULL,
	[Status] [varchar](100) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ServerName] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO

IF NOT EXISTS (select * from syscolumns where id = object_id('dbo.ExgMailHealthDetails'))
BEGIN
CREATE TABLE [dbo].[ExgMailHealthDetails](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[ServerName] [varchar](200) NOT NULL,
	[DatabaseName] [varchar](200) NOT NULL,
	[MailBoxes] [int] NULL,
	[Size] [float] NULL,
	[WhiteSpaceSize] [float] NULL
) ON [PRIMARY]

ALTER TABLE [dbo].[ExgMailHealthDetails]  WITH CHECK ADD FOREIGN KEY([ServerName])
REFERENCES [dbo].[ExgMailHealth] ([ServerName])

END

--Mukund 21Mar14
USE [vitalsigns]
GO
IF NOT EXISTS (select * from syscolumns where id = object_id('dbo.DAGStatus'))
BEGIN
CREATE TABLE [dbo].[DAGStatus](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[DAGName] [nvarchar](50) NOT NULL,
	[TotalMailBoxes] [int] NULL,
	[TotalDatabases] [int] NULL,
	[FileWitnessSereverName] [nvarchar](200) NULL,
	[Status] [nvarchar](200) NULL,
 CONSTRAINT [PK_DAGStatus] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END

IF NOT EXISTS (select * from syscolumns where id = object_id('dbo.DAGMembers'))
BEGIN

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

ALTER TABLE [dbo].[DAGMembers]  WITH CHECK ADD  CONSTRAINT [FK_DAGMembers_DAGStatus] FOREIGN KEY([DAGID])
REFERENCES [dbo].[DAGStatus] ([ID])

ALTER TABLE [dbo].[DAGMembers] CHECK CONSTRAINT [FK_DAGMembers_DAGStatus]

end

IF NOT EXISTS (select * from syscolumns where id = object_id('dbo.DAGDatabase'))
begin
CREATE TABLE [dbo].[DAGDatabase](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[DAGMemberId] [int] NOT NULL,
	[DatabaseName] [nvarchar](200) NULL,
	[Activation Preference] [int] NULL,
	[CopyQueue] [nvarchar](200) NULL,
	[ReplayQueue] [nvarchar](200) NULL,
	[ReplayLagged] [nvarchar](200) NULL,
	[TruncationLagged] [nvarchar](200) NULL,
	[ContendIndex] [int] NULL,
 CONSTRAINT [PK_DAGDatabase] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

ALTER TABLE [dbo].[DAGDatabase]  WITH CHECK ADD  CONSTRAINT [FK_DAGDatabase_DAGMembers] FOREIGN KEY([DAGMemberId])
REFERENCES [dbo].[DAGMembers] ([ID])

ALTER TABLE [dbo].[DAGDatabase] CHECK CONSTRAINT [FK_DAGDatabase_DAGMembers]

end


--Mukund 24Mar14
use [vitalsigns]
go
IF EXISTS (select * from syscolumns where name= 'ContendIndex' and id = object_id('dbo.DAGDatabase'))
BEGIN	
alter table [dbo].[DAGDatabase] alter column ContendIndex nvarchar(200)
End

GO
IF NOT EXISTS (select * from syscolumns where name= 'LastUpdated' and id = object_id('dbo.Traveler_Devices'))
BEGIN
	ALTER TABLE [dbo].[Traveler_Devices] ADD LastUpdated Datetime
END

-- Joe Thumma. Update all existing traveler device data for improving performance. 

update TravelerStats set MailServerName = dbo.GetSrvNameAbbr(MailServerName)
update TravelerStats set TravelerServerName = dbo.GetSrvNameAbbr(TravelerServerName)

-- JT - Add Server Days Alert.
Go 
IF NOT EXISTS (select * from EventsMaster where EventName='Reboot Overdue')
BEGIN	
	SET IDENTITY_INSERT [dbo].[EventsMaster] ON
	INSERT [dbo].[EventsMaster] ([ID], [EventName], [ServerTypeID]) VALUES (58, N'Reboot Overdue', 1)
	SET IDENTITY_INSERT [dbo].[EventsMaster] OFF
END
GO



--Mukund Exchange new tables entry

USE [vitalsigns]
GO
/****** Object:  Table [dbo].[ServiceVersionRole]    Script Date: 03/15/2014 17:18:29 ******/

SET IDENTITY_INSERT [dbo].[ServiceVersionRole] ON
IF NOT EXISTS(select * from [ServiceVersionRole] where Id = 1)
BEGIN
INSERT [dbo].[ServiceVersionRole] ([Id], [ServiceId], [VersionNo], [RoleId]) VALUES (1, 1, N'2010', 1)
END
IF NOT EXISTS(select * from [ServiceVersionRole] where Id = 2)
BEGIN
INSERT [dbo].[ServiceVersionRole] ([Id], [ServiceId], [VersionNo], [RoleId]) VALUES (2, 1, N'2010', 2)
END
IF NOT EXISTS(select * from [ServiceVersionRole] where Id = 3)
BEGIN
INSERT [dbo].[ServiceVersionRole] ([Id], [ServiceId], [VersionNo], [RoleId]) VALUES (3, 1, N'2010', 4)
END
IF NOT EXISTS(select * from [ServiceVersionRole] where Id = 4)
BEGIN
INSERT [dbo].[ServiceVersionRole] ([Id], [ServiceId], [VersionNo], [RoleId]) VALUES (4, 1, N'2010', 5)
END
IF NOT EXISTS(select * from [ServiceVersionRole] where Id = 5)
BEGIN
INSERT [dbo].[ServiceVersionRole] ([Id], [ServiceId], [VersionNo], [RoleId]) VALUES (5, 21, N'2010', 3)
END
IF NOT EXISTS(select * from [ServiceVersionRole] where Id = 6)
BEGIN
INSERT [dbo].[ServiceVersionRole] ([Id], [ServiceId], [VersionNo], [RoleId]) VALUES (6, 13, N'2010', 2)
END
IF NOT EXISTS(select * from [ServiceVersionRole] where Id = 7)
BEGIN
INSERT [dbo].[ServiceVersionRole] ([Id], [ServiceId], [VersionNo], [RoleId]) VALUES (7, 22, N'2010', 3)
END
IF NOT EXISTS(select * from [ServiceVersionRole] where Id = 8)
BEGIN
INSERT [dbo].[ServiceVersionRole] ([Id], [ServiceId], [VersionNo], [RoleId]) VALUES (8, 22, N'2010', 4)
END
IF NOT EXISTS(select * from [ServiceVersionRole] where Id = 9)
BEGIN
INSERT [dbo].[ServiceVersionRole] ([Id], [ServiceId], [VersionNo], [RoleId]) VALUES (9, 23, N'2010', 3)
END
IF NOT EXISTS(select * from [ServiceVersionRole] where Id = 10)
BEGIN
INSERT [dbo].[ServiceVersionRole] ([Id], [ServiceId], [VersionNo], [RoleId]) VALUES (10, 27, N'2010', 4)
END
IF NOT EXISTS(select * from [ServiceVersionRole] where Id = 11)
BEGIN
INSERT [dbo].[ServiceVersionRole] ([Id], [ServiceId], [VersionNo], [RoleId]) VALUES (11, 14, N'2010', 2)
END
IF NOT EXISTS(select * from [ServiceVersionRole] where Id = 12)
BEGIN
INSERT [dbo].[ServiceVersionRole] ([Id], [ServiceId], [VersionNo], [RoleId]) VALUES (12, 14, N'2010', 5)
END
IF NOT EXISTS(select * from [ServiceVersionRole] where Id = 13)
BEGIN
INSERT [dbo].[ServiceVersionRole] ([Id], [ServiceId], [VersionNo], [RoleId]) VALUES (13, 15, N'2010', 2)
END
IF NOT EXISTS(select * from [ServiceVersionRole] where Id = 14)
BEGIN
INSERT [dbo].[ServiceVersionRole] ([Id], [ServiceId], [VersionNo], [RoleId]) VALUES (14, 16, N'2010', 2)
END
IF NOT EXISTS(select * from [ServiceVersionRole] where Id = 15)
BEGIN
INSERT [dbo].[ServiceVersionRole] ([Id], [ServiceId], [VersionNo], [RoleId]) VALUES (15, 2, N'2010', 1)
END
IF NOT EXISTS(select * from [ServiceVersionRole] where Id = 16)
BEGIN
INSERT [dbo].[ServiceVersionRole] ([Id], [ServiceId], [VersionNo], [RoleId]) VALUES (16, 3, N'2010', 1)
END
IF NOT EXISTS(select * from [ServiceVersionRole] where Id = 17)
BEGIN
INSERT [dbo].[ServiceVersionRole] ([Id], [ServiceId], [VersionNo], [RoleId]) VALUES (17, 4, N'2010', 1)
END
IF NOT EXISTS(select * from [ServiceVersionRole] where Id = 18)
BEGIN
INSERT [dbo].[ServiceVersionRole] ([Id], [ServiceId], [VersionNo], [RoleId]) VALUES (18, 17, N'2010', 2)
END
IF NOT EXISTS(select * from [ServiceVersionRole] where Id = 19)
BEGIN
INSERT [dbo].[ServiceVersionRole] ([Id], [ServiceId], [VersionNo], [RoleId]) VALUES (19, 33, N'2010', 1)
END
IF NOT EXISTS(select * from [ServiceVersionRole] where Id = 20)
BEGIN
INSERT [dbo].[ServiceVersionRole] ([Id], [ServiceId], [VersionNo], [RoleId]) VALUES (20, 33, N'2010', 2)
END
IF NOT EXISTS(select * from [ServiceVersionRole] where Id = 21)
BEGIN
INSERT [dbo].[ServiceVersionRole] ([Id], [ServiceId], [VersionNo], [RoleId]) VALUES (21, 33, N'2010', 3)
END
IF NOT EXISTS(select * from [ServiceVersionRole] where Id = 22)
BEGIN
INSERT [dbo].[ServiceVersionRole] ([Id], [ServiceId], [VersionNo], [RoleId]) VALUES (22, 33, N'2010', 4)
END
IF NOT EXISTS(select * from [ServiceVersionRole] where Id = 23)
BEGIN
INSERT [dbo].[ServiceVersionRole] ([Id], [ServiceId], [VersionNo], [RoleId]) VALUES (23, 33, N'2010', 5)
END
IF NOT EXISTS(select * from [ServiceVersionRole] where Id = 24)
BEGIN
INSERT [dbo].[ServiceVersionRole] ([Id], [ServiceId], [VersionNo], [RoleId]) VALUES (24, 18, N'2010', 2)
END
IF NOT EXISTS(select * from [ServiceVersionRole] where Id = 25)
BEGIN
INSERT [dbo].[ServiceVersionRole] ([Id], [ServiceId], [VersionNo], [RoleId]) VALUES (25, 19, N'2010', 2)
END
IF NOT EXISTS(select * from [ServiceVersionRole] where Id = 26)
BEGIN
INSERT [dbo].[ServiceVersionRole] ([Id], [ServiceId], [VersionNo], [RoleId]) VALUES (26, 19, N'2010', 4)
END
IF NOT EXISTS(select * from [ServiceVersionRole] where Id = 27)
BEGIN
INSERT [dbo].[ServiceVersionRole] ([Id], [ServiceId], [VersionNo], [RoleId]) VALUES (27, 5, N'2010', 1)
END
IF NOT EXISTS(select * from [ServiceVersionRole] where Id = 28)
BEGIN
INSERT [dbo].[ServiceVersionRole] ([Id], [ServiceId], [VersionNo], [RoleId]) VALUES (28, 6, N'2010', 1)
END
IF NOT EXISTS(select * from [ServiceVersionRole] where Id = 29)
BEGIN
INSERT [dbo].[ServiceVersionRole] ([Id], [ServiceId], [VersionNo], [RoleId]) VALUES (29, 6, N'2010', 2)
END
IF NOT EXISTS(select * from [ServiceVersionRole] where Id = 30)
BEGIN
INSERT [dbo].[ServiceVersionRole] ([Id], [ServiceId], [VersionNo], [RoleId]) VALUES (30, 7, N'2010', 1)
END
IF NOT EXISTS(select * from [ServiceVersionRole] where Id = 31)
BEGIN
INSERT [dbo].[ServiceVersionRole] ([Id], [ServiceId], [VersionNo], [RoleId]) VALUES (31, 8, N'2010', 1)
END
IF NOT EXISTS(select * from [ServiceVersionRole] where Id = 32)
BEGIN
INSERT [dbo].[ServiceVersionRole] ([Id], [ServiceId], [VersionNo], [RoleId]) VALUES (32, 34, N'2010', 1)
END
IF NOT EXISTS(select * from [ServiceVersionRole] where Id = 33)
BEGIN
INSERT [dbo].[ServiceVersionRole] ([Id], [ServiceId], [VersionNo], [RoleId]) VALUES (33, 34, N'2010', 2)
END
IF NOT EXISTS(select * from [ServiceVersionRole] where Id = 34)
BEGIN
INSERT [dbo].[ServiceVersionRole] ([Id], [ServiceId], [VersionNo], [RoleId]) VALUES (34, 34, N'2010', 3)
END
IF NOT EXISTS(select * from [ServiceVersionRole] where Id = 35)
BEGIN
INSERT [dbo].[ServiceVersionRole] ([Id], [ServiceId], [VersionNo], [RoleId]) VALUES (35, 34, N'2010', 4)
END
IF NOT EXISTS(select * from [ServiceVersionRole] where Id = 36)
BEGIN
INSERT [dbo].[ServiceVersionRole] ([Id], [ServiceId], [VersionNo], [RoleId]) VALUES (36, 34, N'2010', 5)
END
IF NOT EXISTS(select * from [ServiceVersionRole] where Id = 37)
BEGIN
INSERT [dbo].[ServiceVersionRole] ([Id], [ServiceId], [VersionNo], [RoleId]) VALUES (37, 35, N'2010', 5)
END
IF NOT EXISTS(select * from [ServiceVersionRole] where Id = 38)
BEGIN
INSERT [dbo].[ServiceVersionRole] ([Id], [ServiceId], [VersionNo], [RoleId]) VALUES (38, 9, N'2010', 1)
END
IF NOT EXISTS(select * from [ServiceVersionRole] where Id = 39)
BEGIN
INSERT [dbo].[ServiceVersionRole] ([Id], [ServiceId], [VersionNo], [RoleId]) VALUES (39, 10, N'2010', 1)
END
IF NOT EXISTS(select * from [ServiceVersionRole] where Id = 40)
BEGIN
INSERT [dbo].[ServiceVersionRole] ([Id], [ServiceId], [VersionNo], [RoleId]) VALUES (40, 24, N'2010', 3)
END
IF NOT EXISTS(select * from [ServiceVersionRole] where Id = 41)
BEGIN
INSERT [dbo].[ServiceVersionRole] ([Id], [ServiceId], [VersionNo], [RoleId]) VALUES (41, 24, N'2010', 4)
END
IF NOT EXISTS(select * from [ServiceVersionRole] where Id = 42)
BEGIN
INSERT [dbo].[ServiceVersionRole] ([Id], [ServiceId], [VersionNo], [RoleId]) VALUES (42, 11, N'2010', 1)
END
IF NOT EXISTS(select * from [ServiceVersionRole] where Id = 43)
BEGIN
INSERT [dbo].[ServiceVersionRole] ([Id], [ServiceId], [VersionNo], [RoleId]) VALUES (43, 11, N'2010', 3)
END
IF NOT EXISTS(select * from [ServiceVersionRole] where Id = 44)
BEGIN
INSERT [dbo].[ServiceVersionRole] ([Id], [ServiceId], [VersionNo], [RoleId]) VALUES (44, 11, N'2010', 4)
END
IF NOT EXISTS(select * from [ServiceVersionRole] where Id = 45)
BEGIN
INSERT [dbo].[ServiceVersionRole] ([Id], [ServiceId], [VersionNo], [RoleId]) VALUES (45, 32, N'2010', 5)
END
IF NOT EXISTS(select * from [ServiceVersionRole] where Id = 46)
BEGIN
INSERT [dbo].[ServiceVersionRole] ([Id], [ServiceId], [VersionNo], [RoleId]) VALUES (46, 12, N'2010', 1)
END
IF NOT EXISTS(select * from [ServiceVersionRole] where Id = 47)
BEGIN
INSERT [dbo].[ServiceVersionRole] ([Id], [ServiceId], [VersionNo], [RoleId]) VALUES (47, 12, N'2010', 4)
END

IF NOT EXISTS(select * from [ServiceVersionRole] where Id = 48)
BEGIN
INSERT [dbo].[ServiceVersionRole] ([Id], [ServiceId], [VersionNo], [RoleId]) VALUES (48, 1, N'2007', 1)
END
IF NOT EXISTS(select * from [ServiceVersionRole] where Id = 49)
BEGIN
INSERT [dbo].[ServiceVersionRole] ([Id], [ServiceId], [VersionNo], [RoleId]) VALUES (49, 1, N'2007', 2)
END
IF NOT EXISTS(select * from [ServiceVersionRole] where Id = 50)
BEGIN
INSERT [dbo].[ServiceVersionRole] ([Id], [ServiceId], [VersionNo], [RoleId]) VALUES (50, 1, N'2007', 4)
END
IF NOT EXISTS(select * from [ServiceVersionRole] where Id = 51)
BEGIN
INSERT [dbo].[ServiceVersionRole] ([Id], [ServiceId], [VersionNo], [RoleId]) VALUES (51, 1, N'2007', 5)
END
IF NOT EXISTS(select * from [ServiceVersionRole] where Id = 52)
BEGIN
INSERT [dbo].[ServiceVersionRole] ([Id], [ServiceId], [VersionNo], [RoleId]) VALUES (52, 21, N'2007', 3)
END
IF NOT EXISTS(select * from [ServiceVersionRole] where Id = 53)
BEGIN
INSERT [dbo].[ServiceVersionRole] ([Id], [ServiceId], [VersionNo], [RoleId]) VALUES (53, 22, N'2007', 3)
END
IF NOT EXISTS(select * from [ServiceVersionRole] where Id = 54)
BEGIN
INSERT [dbo].[ServiceVersionRole] ([Id], [ServiceId], [VersionNo], [RoleId]) VALUES (54, 22, N'2007', 4)
END
IF NOT EXISTS(select * from [ServiceVersionRole] where Id = 55)
BEGIN
INSERT [dbo].[ServiceVersionRole] ([Id], [ServiceId], [VersionNo], [RoleId]) VALUES (55, 23, N'2007', 3)
END
IF NOT EXISTS(select * from [ServiceVersionRole] where Id = 56)
BEGIN
INSERT [dbo].[ServiceVersionRole] ([Id], [ServiceId], [VersionNo], [RoleId]) VALUES (56, 27, N'2007', 4)
END
IF NOT EXISTS(select * from [ServiceVersionRole] where Id = 57)
BEGIN
INSERT [dbo].[ServiceVersionRole] ([Id], [ServiceId], [VersionNo], [RoleId]) VALUES (57, 14, N'2007', 2)
END
IF NOT EXISTS(select * from [ServiceVersionRole] where Id = 58)
BEGIN
INSERT [dbo].[ServiceVersionRole] ([Id], [ServiceId], [VersionNo], [RoleId]) VALUES (58, 14, N'2007', 5)
END
IF NOT EXISTS(select * from [ServiceVersionRole] where Id = 59)
BEGIN
INSERT [dbo].[ServiceVersionRole] ([Id], [ServiceId], [VersionNo], [RoleId]) VALUES (59, 16, N'2007', 2)
END
IF NOT EXISTS(select * from [ServiceVersionRole] where Id = 60)
BEGIN
INSERT [dbo].[ServiceVersionRole] ([Id], [ServiceId], [VersionNo], [RoleId]) VALUES (60, 2, N'2007', 1)
END
IF NOT EXISTS(select * from [ServiceVersionRole] where Id = 61)
BEGIN
INSERT [dbo].[ServiceVersionRole] ([Id], [ServiceId], [VersionNo], [RoleId]) VALUES (61, 3, N'2007', 1)
END
IF NOT EXISTS(select * from [ServiceVersionRole] where Id = 62)
BEGIN
INSERT [dbo].[ServiceVersionRole] ([Id], [ServiceId], [VersionNo], [RoleId]) VALUES (62, 4, N'2007', 1)
END
IF NOT EXISTS(select * from [ServiceVersionRole] where Id = 63)
BEGIN
INSERT [dbo].[ServiceVersionRole] ([Id], [ServiceId], [VersionNo], [RoleId]) VALUES (63, 33, N'2007', 1)
END
IF NOT EXISTS(select * from [ServiceVersionRole] where Id = 64)
BEGIN
INSERT [dbo].[ServiceVersionRole] ([Id], [ServiceId], [VersionNo], [RoleId]) VALUES (64, 33, N'2007', 2)
END
IF NOT EXISTS(select * from [ServiceVersionRole] where Id = 65)
BEGIN
INSERT [dbo].[ServiceVersionRole] ([Id], [ServiceId], [VersionNo], [RoleId]) VALUES (65, 33, N'2007', 3)
END
IF NOT EXISTS(select * from [ServiceVersionRole] where Id = 66)
BEGIN
INSERT [dbo].[ServiceVersionRole] ([Id], [ServiceId], [VersionNo], [RoleId]) VALUES (66, 33, N'2007', 4)
END
IF NOT EXISTS(select * from [ServiceVersionRole] where Id = 67)
BEGIN
INSERT [dbo].[ServiceVersionRole] ([Id], [ServiceId], [VersionNo], [RoleId]) VALUES (67, 33, N'2007', 5)
END
IF NOT EXISTS(select * from [ServiceVersionRole] where Id = 68)
BEGIN
INSERT [dbo].[ServiceVersionRole] ([Id], [ServiceId], [VersionNo], [RoleId]) VALUES (68, 18, N'2007', 2)
END
IF NOT EXISTS(select * from [ServiceVersionRole] where Id = 69)
BEGIN
INSERT [dbo].[ServiceVersionRole] ([Id], [ServiceId], [VersionNo], [RoleId]) VALUES (69, 5, N'2007', 1)
END
IF NOT EXISTS(select * from [ServiceVersionRole] where Id = 70)
BEGIN
INSERT [dbo].[ServiceVersionRole] ([Id], [ServiceId], [VersionNo], [RoleId]) VALUES (70, 7, N'2007', 1)
END
IF NOT EXISTS(select * from [ServiceVersionRole] where Id = 71)
BEGIN
INSERT [dbo].[ServiceVersionRole] ([Id], [ServiceId], [VersionNo], [RoleId]) VALUES (71, 34, N'2007', 1)
END
IF NOT EXISTS(select * from [ServiceVersionRole] where Id = 72)
BEGIN
INSERT [dbo].[ServiceVersionRole] ([Id], [ServiceId], [VersionNo], [RoleId]) VALUES (72, 34, N'2007', 2)
END
IF NOT EXISTS(select * from [ServiceVersionRole] where Id = 73)
BEGIN
INSERT [dbo].[ServiceVersionRole] ([Id], [ServiceId], [VersionNo], [RoleId]) VALUES (73, 35, N'2007', 5)
END
IF NOT EXISTS(select * from [ServiceVersionRole] where Id = 74)
BEGIN
INSERT [dbo].[ServiceVersionRole] ([Id], [ServiceId], [VersionNo], [RoleId]) VALUES (74, 9, N'2007', 1)
END
IF NOT EXISTS(select * from [ServiceVersionRole] where Id = 75)
BEGIN
INSERT [dbo].[ServiceVersionRole] ([Id], [ServiceId], [VersionNo], [RoleId]) VALUES (75, 24, N'2007', 3)
END
IF NOT EXISTS(select * from [ServiceVersionRole] where Id = 76)
BEGIN
INSERT [dbo].[ServiceVersionRole] ([Id], [ServiceId], [VersionNo], [RoleId]) VALUES (76, 24, N'2007', 4)
END
IF NOT EXISTS(select * from [ServiceVersionRole] where Id = 77)
BEGIN
INSERT [dbo].[ServiceVersionRole] ([Id], [ServiceId], [VersionNo], [RoleId]) VALUES (77, 11, N'2007', 1)
END
IF NOT EXISTS(select * from [ServiceVersionRole] where Id = 78)
BEGIN
INSERT [dbo].[ServiceVersionRole] ([Id], [ServiceId], [VersionNo], [RoleId]) VALUES (78, 11, N'2007', 3)
END
IF NOT EXISTS(select * from [ServiceVersionRole] where Id = 79)
BEGIN
INSERT [dbo].[ServiceVersionRole] ([Id], [ServiceId], [VersionNo], [RoleId]) VALUES (79, 11, N'2007', 4)
END
IF NOT EXISTS(select * from [ServiceVersionRole] where Id = 80)
BEGIN
INSERT [dbo].[ServiceVersionRole] ([Id], [ServiceId], [VersionNo], [RoleId]) VALUES (80, 32, N'2007', 5)
END
IF NOT EXISTS(select * from [ServiceVersionRole] where Id = 81)
BEGIN
INSERT [dbo].[ServiceVersionRole] ([Id], [ServiceId], [VersionNo], [RoleId]) VALUES (81, 12, N'2007', 1)
END

SET IDENTITY_INSERT [dbo].[ServiceVersionRole] OFF
/****** Object:  Table [dbo].[ServiceMaster]    Script Date: 03/15/2014 17:18:29 ******/

SET IDENTITY_INSERT [dbo].[ServiceMaster] ON
IF NOT EXISTS(select * from ServiceMaster where ServiceName = 'Microsoft Exchange Active Directory Topology')
BEGIN
	INSERT [dbo].[ServiceMaster] ([ServiceId], [ServiceName], [ServiceShortName], [SecurityContext], [ServiceDescription], [DefaultStartupType], [Required], [Custom]) VALUES (1, N'Microsoft Exchange Active Directory Topology', N'MSExchangeADTopology', N'Local System', N'Provides Active Directory topology information to Exchange services. If this service is stopped, most Exchange services are unable to start. This service has no dependencies.', N'Automatic', N'R', 0)
END
IF NOT EXISTS(select * from ServiceMaster where ServiceName = 'Microsoft Exchange Information Store')
BEGIN
INSERT [dbo].[ServiceMaster] ([ServiceId], [ServiceName], [ServiceShortName], [SecurityContext], [ServiceDescription], [DefaultStartupType], [Required], [Custom]) VALUES (2, N'Microsoft Exchange Information Store', N'MSExchangeIS', N'Local System', N'Manages the Exchange Information Store. This includes mailbox databases and public folder databases. If this service is stopped, mailbox databases and public folder databases on this computer are unavailable. If this service is disabled, any services that explicitly depend on it will fail to start. This service is dependent on the RPC, Server, Windows Event Log, and Workstation services.', N'Automatic', N'R', 0)
END
IF NOT EXISTS(select * from ServiceMaster where ServiceName = 'Microsoft Exchange Mail Submission Service')
BEGIN
INSERT [dbo].[ServiceMaster] ([ServiceId], [ServiceName], [ServiceShortName], [SecurityContext], [ServiceDescription], [DefaultStartupType], [Required], [Custom]) VALUES (3, N'Microsoft Exchange Mail Submission Service', N'MSExchangeMailSubmission', N'Local System', N'Submits messages from the Mailbox server to Exchange 2010 Hub Transport servers. This service is dependent upon the Microsoft Exchange Active Directory Topology service.', N'Automatic', N'R', 0)
END
IF NOT EXISTS(select * from ServiceMaster where ServiceName = 'Microsoft Exchange Mailbox Assistants')
BEGIN
INSERT [dbo].[ServiceMaster] ([ServiceId], [ServiceName], [ServiceShortName], [SecurityContext], [ServiceDescription], [DefaultStartupType], [Required], [Custom]) VALUES (4, N'Microsoft Exchange Mailbox Assistants', N'MSExchangeMailboxAssistants', N'Local System', N'Performs background processing of mailboxes in the Exchange store. This service is dependent upon the Microsoft Exchange Active Directory Topology service.', N'Automatic', N'R', 0)
END
IF NOT EXISTS(select * from ServiceMaster where ServiceName = 'Microsoft Exchange Replication Service')
BEGIN
INSERT [dbo].[ServiceMaster] ([ServiceId], [ServiceName], [ServiceShortName], [SecurityContext], [ServiceDescription], [DefaultStartupType], [Required], [Custom]) VALUES (5, N'Microsoft Exchange Replication Service', N'MSExchangeRepl', N'Local System', N'Provides replication functionality for mailbox databases on Mailbox servers in a database availability group (DAG) and database mount functionality for all Mailbox servers. This service is dependent upon the Microsoft Exchange Active Directory Topology service.', N'Automatic', N'R', 0)
END
IF NOT EXISTS(select * from ServiceMaster where ServiceName = 'Microsoft Exchange RPC Client Access')
BEGIN
INSERT [dbo].[ServiceMaster] ([ServiceId], [ServiceName], [ServiceShortName], [SecurityContext], [ServiceDescription], [DefaultStartupType], [Required], [Custom]) VALUES (6, N'Microsoft Exchange RPC Client Access', N'MSExchangeRPC', N'Network Service', N'Manages client RPC connections for Exchange. This service is dependent upon the Microsoft Exchange Active Directory Topology service.', N'Automatic', N'O', 0)
END
IF NOT EXISTS(select * from ServiceMaster where ServiceName = 'Microsoft Exchange Search Indexer')
BEGIN
INSERT [dbo].[ServiceMaster] ([ServiceId], [ServiceName], [ServiceShortName], [SecurityContext], [ServiceDescription], [DefaultStartupType], [Required], [Custom]) VALUES (7, N'Microsoft Exchange Search Indexer', N'MSExchangeSearch', N'Local System', N'Drives indexing of mailbox content, which improves the performance of content search. This service is dependent upon the Microsoft Exchange Active Directory Topology and Microsoft Search (Exchange Server) services.', N'Automatic', N'O', 0)
END
IF NOT EXISTS(select * from ServiceMaster where ServiceName = 'Microsoft Exchange Server Extension for Windows Server Backup')
BEGIN
INSERT [dbo].[ServiceMaster] ([ServiceId], [ServiceName], [ServiceShortName], [SecurityContext], [ServiceDescription], [DefaultStartupType], [Required], [Custom]) VALUES (8, N'Microsoft Exchange Server Extension for Windows Server Backup', N'WSBExchange', N'Local System', N'Enables Windows Server Backup users to back up and recover application data for Microsoft Exchange. This service has no dependencies.', N'Manual', N'O', 0)
END
IF NOT EXISTS(select * from ServiceMaster where ServiceName = 'Microsoft Exchange System Attendant')
BEGIN
INSERT [dbo].[ServiceMaster] ([ServiceId], [ServiceName], [ServiceShortName], [SecurityContext], [ServiceDescription], [DefaultStartupType], [Required], [Custom]) VALUES (9, N'Microsoft Exchange System Attendant', N'MSExchangeSA', N'Local System', N'Forwards directory lookups to a global catalog server for legacy Outlook clients, generates e-mail addresses and OABs, updates free/busy information for legacy clients, and maintains permissions and group memberships for the server. If this service is disabled, any services that explicitly depend on it will fail to start. This service is dependent on the RPC, Server, Windows Event Log, and Workstation services.', N'Automatic', N'R', 0)
END
IF NOT EXISTS(select * from ServiceMaster where ServiceName = 'Microsoft Exchange Throttling')
BEGIN
INSERT [dbo].[ServiceMaster] ([ServiceId], [ServiceName], [ServiceShortName], [SecurityContext], [ServiceDescription], [DefaultStartupType], [Required], [Custom]) VALUES (10, N'Microsoft Exchange Throttling', N'MSExchangeThrottling', N'Network Service', N'Limits the rate of user operations. This service is dependent upon the Microsoft Exchange Active Directory Topology service.', N'Automatic', N'R', 0)
END
IF NOT EXISTS(select * from ServiceMaster where ServiceName = 'Microsoft Exchange Transport Log Search')
BEGIN
INSERT [dbo].[ServiceMaster] ([ServiceId], [ServiceName], [ServiceShortName], [SecurityContext], [ServiceDescription], [DefaultStartupType], [Required], [Custom]) VALUES (11, N'Microsoft Exchange Transport Log Search', N'MSExchangeTransportLogSearch', N'Local System', N'Provides remote search capability for Microsoft Exchange Transport log files. On Hub Transport servers, this service is dependent upon the Microsoft Exchange Active Directory Topology service. On Edge Transport servers, this service is dependent upon the Microsoft Exchange ADAM service.', N'Automatic', N'O', 0)
END
IF NOT EXISTS(select * from ServiceMaster where ServiceName = 'Microsoft Search (Exchange Server)')
BEGIN
INSERT [dbo].[ServiceMaster] ([ServiceId], [ServiceName], [ServiceShortName], [SecurityContext], [ServiceDescription], [DefaultStartupType], [Required], [Custom]) VALUES (12, N'Microsoft Search (Exchange Server)', N'msftesql-Exchange', N'Local System', N'This is a Microsoft Exchange-customized version of Microsoft Search. This service is dependent on the RPC service.', N'Manual', N'O', 0)
END
IF NOT EXISTS(select * from ServiceMaster where ServiceName = 'Microsoft Exchange Address Book')
BEGIN
INSERT [dbo].[ServiceMaster] ([ServiceId], [ServiceName], [ServiceShortName], [SecurityContext], [ServiceDescription], [DefaultStartupType], [Required], [Custom]) VALUES (13, N'Microsoft Exchange Address Book', N'MSExchangeAB', N'Local System', N'Manages client address book connections. This service is dependent upon the Microsoft Exchange Active Directory Topology service.', N'Automatic', N'R', 0)
END
IF NOT EXISTS(select * from ServiceMaster where ServiceName = 'Microsoft Exchange File Distribution')
BEGIN
INSERT [dbo].[ServiceMaster] ([ServiceId], [ServiceName], [ServiceShortName], [SecurityContext], [ServiceDescription], [DefaultStartupType], [Required], [Custom]) VALUES (14, N'Microsoft Exchange File Distribution', N'MSExchangeFDS', N'Local System', N'Distributes offline address book (OAB) and custom Unified Messaging prompts. This service is dependent upon the Microsoft Exchange Active Directory Topology and Workstation services.', N'Automatic', N'R', 0)
END
IF NOT EXISTS(select * from ServiceMaster where ServiceName = 'Microsoft Exchange Forms-Based Authentication')
BEGIN
INSERT [dbo].[ServiceMaster] ([ServiceId], [ServiceName], [ServiceShortName], [SecurityContext], [ServiceDescription], [DefaultStartupType], [Required], [Custom]) VALUES (15, N'Microsoft Exchange Forms-Based Authentication', N'MSExchangeFBA', N'Local System', N'Provides forms-based authentication to Microsoft Office Outlook Web App and the Exchange Control Panel. If this service is stopped, Outlook Web App and the Exchange Control Panel won''t authenticate users. This service has no dependencies.', N'Automatic', N'R', 0)
END
IF NOT EXISTS(select * from ServiceMaster where ServiceName = 'Microsoft Exchange IMAP4')
BEGIN
INSERT [dbo].[ServiceMaster] ([ServiceId], [ServiceName], [ServiceShortName], [SecurityContext], [ServiceDescription], [DefaultStartupType], [Required], [Custom]) VALUES (16, N'Microsoft Exchange IMAP4', N'MSExchangeIMAP4', N'Network Service', N'Provides IMAP4 service to clients. If this service is stopped, clients won''t be able to connect to this computer using the IMAP4 protocol. This service is dependent upon the Microsoft Exchange Active Directory Topology service.', N'Manual', N'O', 0)
END
IF NOT EXISTS(select * from ServiceMaster where ServiceName = 'Microsoft Exchange Mailbox Replication Service')
BEGIN
INSERT [dbo].[ServiceMaster] ([ServiceId], [ServiceName], [ServiceShortName], [SecurityContext], [ServiceDescription], [DefaultStartupType], [Required], [Custom]) VALUES (17, N'Microsoft Exchange Mailbox Replication Service', N'MSExchangeMailboxReplication', N'Local System', N'Processes mailbox moves and move requests. This service is dependent upon the Microsoft Exchange Active Directory Topology and Net.Tcp Port Sharing service.', N'Automatic', N'O', 0)
END
IF NOT EXISTS(select * from ServiceMaster where ServiceName = 'Microsoft Exchange POP3')
BEGIN
INSERT [dbo].[ServiceMaster] ([ServiceId], [ServiceName], [ServiceShortName], [SecurityContext], [ServiceDescription], [DefaultStartupType], [Required], [Custom]) VALUES (18, N'Microsoft Exchange POP3', N'MSExchangePOP3', N'Network Service', N'Provides POP3 service to clients. If this service is stopped, clients can''t connect to this computer using the POP3 protocol. This service is dependent upon the Microsoft Exchange Active Directory Topology service.', N'Manual', N'O', 0)
END
IF NOT EXISTS(select * from ServiceMaster where ServiceName = 'Microsoft Exchange Protected Service Host')
BEGIN
INSERT [dbo].[ServiceMaster] ([ServiceId], [ServiceName], [ServiceShortName], [SecurityContext], [ServiceDescription], [DefaultStartupType], [Required], [Custom]) VALUES (19, N'Microsoft Exchange Protected Service Host', N'MSExchangeProtectedServiceHost', N'Local System', N'Provides a host for several Exchange services that must be protected from other services. This service is dependent upon the Microsoft Exchange Active Directory Topology service.', N'Automatic', N'R', 0)
END
IF NOT EXISTS(select * from ServiceMaster where ServiceName = 'Microsoft Exchange ADAM')
BEGIN
INSERT [dbo].[ServiceMaster] ([ServiceId], [ServiceName], [ServiceShortName], [SecurityContext], [ServiceDescription], [DefaultStartupType], [Required], [Custom]) VALUES (21, N'Microsoft Exchange ADAM', N'ADAM_MSExchange', N'Network Service', N'Stores configuration data and recipient data on the Edge Transport server. This service represents the named instance of Active Directory Lightweight Directory Service (AD LDS) that''s automatically created by Setup during Edge Transport server installation. This service is dependent upon the COM+ Event System service.', N'Automatic', N'R', 0)
END
IF NOT EXISTS(select * from ServiceMaster where ServiceName = 'Microsoft Exchange Credential Service')
BEGIN
INSERT [dbo].[ServiceMaster] ([ServiceId], [ServiceName], [ServiceShortName], [SecurityContext], [ServiceDescription], [DefaultStartupType], [Required], [Custom]) VALUES (23, N'Microsoft Exchange Credential Service', N'MSExchangeEdgeCredential', N'Local System', N'Monitors credential changes in AD LDS and installs the changes on the Edge Transport server. This service is dependent upon the Microsoft Exchange ADAM service.', N'Automatic', N'R', 0)
END
IF NOT EXISTS(select * from ServiceMaster where ServiceName = 'Microsoft Exchange Transport')
BEGIN
INSERT [dbo].[ServiceMaster] ([ServiceId], [ServiceName], [ServiceShortName], [SecurityContext], [ServiceDescription], [DefaultStartupType], [Required], [Custom]) VALUES (24, N'Microsoft Exchange Transport', N'MSExchangeTransport', N'Network Service', N'Provides SMTP server and transport stack. On Hub Transport servers, this service is dependent upon the Microsoft Exchange Active Directory Topology service. On Edge Transport servers, this service is dependent upon the Microsoft Exchange ADAM service.', N'Automatic', N'R', 0)
END
IF NOT EXISTS(select * from ServiceMaster where ServiceName = 'Microsoft Exchange Monitoring')
BEGIN
INSERT [dbo].[ServiceMaster] ([ServiceId], [ServiceName], [ServiceShortName], [SecurityContext], [ServiceDescription], [DefaultStartupType], [Required], [Custom]) VALUES (33, N'Microsoft Exchange Monitoring', N'MSExchangeMonitoring', N'Local System', N'Allows applications to call the Exchange diagnostic cmdlets. This service has no dependencies.', N'Manual', N'O', 0)
END
IF NOT EXISTS(select * from ServiceMaster where ServiceName = 'Microsoft Exchange Service Host')
BEGIN
INSERT [dbo].[ServiceMaster] ([ServiceId], [ServiceName], [ServiceShortName], [SecurityContext], [ServiceDescription], [DefaultStartupType], [Required], [Custom]) VALUES (34, N'Microsoft Exchange Service Host', N'MSExchangeServiceHost', N'Local System', N'Provides a host for several Exchange services. On internal server roles, this service is dependent upon the Microsoft Exchange Active Directory Topology service. On Edge Transport servers, this service is dependent upon the Microsoft Exchange ADAM service.', N'Automatic', N'R', 0)
END
IF NOT EXISTS(select * from ServiceMaster where ServiceName = 'Microsoft Exchange Speech Engine')
BEGIN
INSERT [dbo].[ServiceMaster] ([ServiceId], [ServiceName], [ServiceShortName], [SecurityContext], [ServiceDescription], [DefaultStartupType], [Required], [Custom]) VALUES (35, N'Microsoft Exchange Speech Engine', N'MSSpeechService', N'Network Service', N'Provides speech processing services for Unified Messaging. This service is dependent upon the Windows Management Instrumentation (WMI) service.', N'Automatic', N'R', 0)
END
IF NOT EXISTS(select * from ServiceMaster where ServiceName = 'Microsoft Exchange Anti-spam Update')
BEGIN
INSERT [dbo].[ServiceMaster] ([ServiceId], [ServiceName], [ServiceShortName], [SecurityContext], [ServiceDescription], [DefaultStartupType], [Required], [Custom]) VALUES (22, N'Microsoft Exchange Anti-spam Update', N'MSExchangeAntispamUpdate', N'Local System', N'Provides the Microsoft Forefront Protection 2010 for Exchange Server anti-spam update service. On Hub Transport servers, this service is dependent upon the Microsoft Exchange Active Directory Topology service. On Edge Transport servers, this service is dependent upon the Microsoft Exchange ADAM service.', N'Automatic', N'O', 0)
END
IF NOT EXISTS(select * from ServiceMaster where ServiceName = 'Microsoft Exchange EdgeSync')
BEGIN
INSERT [dbo].[ServiceMaster] ([ServiceId], [ServiceName], [ServiceShortName], [SecurityContext], [ServiceDescription], [DefaultStartupType], [Required], [Custom]) VALUES (27, N'Microsoft Exchange EdgeSync', N'MSExchangeEdgeSync', N'Local System', N'Connects to an AD LDS instance on subscribed Edge Transport servers over a secure LDAP channel to synchronize data between a Hub Transport server and an Edge Transport server. This service is dependent upon the Microsoft Exchange Active Directory Topology service. If Edge Subscription isn''t configured, this service can be disabled.', N'Hub', N'O', 0)
END
IF NOT EXISTS(select * from ServiceMaster where ServiceName = 'Microsoft Exchange Unified Messaging')
BEGIN
INSERT [dbo].[ServiceMaster] ([ServiceId], [ServiceName], [ServiceShortName], [SecurityContext], [ServiceDescription], [DefaultStartupType], [Required], [Custom]) VALUES (32, N'Microsoft Exchange Unified Messaging', N'MSExchangeUM', N'Local System', N'Enables Microsoft Exchange Unified Messaging features. This allows voice and fax messages to be stored in Exchange and gives users telephone access to e-mail, voice mail, calendar, contacts, or an auto attendant. If this service is stopped, Unified Messaging isn''t available. This service is dependent upon the Microsoft Exchange Active Directory Topology and the Microsoft Exchange Speech Engine service.', N'Automatic', N'R', 0)
END
SET IDENTITY_INSERT [dbo].[ServiceMaster] OFF

/****** Object:  Table [dbo].[ServerVersions]    Script Date: 03/15/2014 17:18:29 ******/

SET IDENTITY_INSERT [dbo].[ServerVersions] ON
IF NOT EXISTS(select * from ServerVersions where Id = 2)
BEGIN
INSERT [dbo].[ServerVersions] ([Id], [VersionNo]) VALUES (2, N'2007')
END
IF NOT EXISTS(select * from ServerVersions where Id = 3)
BEGIN
INSERT [dbo].[ServerVersions] ([Id], [VersionNo]) VALUES (3, N'2010')
END
IF NOT EXISTS(select * from ServerVersions where Id = 5)
BEGIN
INSERT [dbo].[ServerVersions] ([Id], [VersionNo]) VALUES (5, N'2013')
End
SET IDENTITY_INSERT [dbo].[ServerVersions] OFF

/****** Object:  Table [dbo].[RolesMaster]    Script Date: 03/15/2014 17:18:29 ******/
SET IDENTITY_INSERT [dbo].[RolesMaster] ON

IF NOT EXISTS(select * from RolesMaster where Id = 1)
BEGIN
INSERT [dbo].[RolesMaster] ([Id], [RoleName], [ServerTypeId]) VALUES (1, N'Mailbox', 5)
END
IF NOT EXISTS(select * from RolesMaster where Id = 2)
BEGIN
INSERT [dbo].[RolesMaster] ([Id], [RoleName], [ServerTypeId]) VALUES (2, N'CAS', 5)
END
IF NOT EXISTS(select * from RolesMaster where Id = 3)
BEGIN
INSERT [dbo].[RolesMaster] ([Id], [RoleName], [ServerTypeId]) VALUES (3, N'Edge', 5)
END
IF NOT EXISTS(select * from RolesMaster where Id = 4)
BEGIN
INSERT [dbo].[RolesMaster] ([Id], [RoleName], [ServerTypeId]) VALUES (4, N'Hub', 5)
END
IF NOT EXISTS(select * from RolesMaster where Id = 5)
BEGIN
INSERT [dbo].[RolesMaster] ([Id], [RoleName], [ServerTypeId]) VALUES (5, N'Unified Messaging', 5)
End
SET IDENTITY_INSERT [dbo].[RolesMaster] OFF


--Mukund 27Mar14
use [vitalsigns]
go
update [vitalsigns].[dbo].[MenuItems] set PageLink='/Configurator/PwrshellCredentials.aspx' where DisplayText='Microsoft Exchange Settings'
go
delete [vitalsigns].[dbo].[MenuItems]  where  DisplayText='Exchange Server Credentials'
go

IF EXISTS (select * from syscolumns where id = object_id('dbo.StatusDetail') and name= 'TestType')
BEGIN
	Alter TABLE [dbo].[StatusDetail] Drop Column TestType
END

use [VSS_Statistics]
go


--Mukund 27Mar14 Drop unused exchange tables

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_InputParameters_PowershellScripts]') AND parent_object_id = OBJECT_ID(N'[dbo].[InputParameters]'))
BEGIN
	ALTER TABLE [dbo].[InputParameters] DROP CONSTRAINT [FK_InputParameters_PowershellScripts]
END
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_OutputFields_InputParameters]') AND parent_object_id = OBJECT_ID(N'[dbo].[OutputFields]'))
BEGIN
	ALTER TABLE [dbo].[OutputFields] DROP CONSTRAINT [FK_OutputFields_InputParameters]
END
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_OutputTables_PowershellScripts]') AND parent_object_id = OBJECT_ID(N'[dbo].[OutputTables]'))
BEGIN
	ALTER TABLE [dbo].[OutputTables] DROP CONSTRAINT [FK_OutputTables_PowershellScripts]
END
GO

IF EXISTS(select * from syscolumns where id=Object_id('dbo.ExMailHealth'))
BEGIN	
	drop table dbo.ExMailHealth
END

IF EXISTS(select * from syscolumns where id=Object_id('dbo.ExchangeDAGHealthCopyStatus'))
BEGIN	
	drop table dbo.ExchangeDAGHealthCopyStatus
END

IF EXISTS(select * from syscolumns where id=Object_id('dbo.ExchangeDAGHealthCopySummary'))
BEGIN	
	drop table dbo.ExchangeDAGHealthCopySummary
END

IF EXISTS(select * from syscolumns where id=Object_id('dbo.ExchangeDAGHealthMemberReport'))
BEGIN	
	drop table dbo.ExchangeDAGHealthMemberReport
END

IF EXISTS(select * from syscolumns where id=Object_id('dbo.ExchangeMailBoxReport'))
BEGIN	
	drop table dbo.ExchangeMailBoxReport
END

IF EXISTS(select * from syscolumns where id=Object_id('dbo.ExchangeQueues'))
BEGIN	
	drop table dbo.ExchangeQueues
END

IF EXISTS(select * from syscolumns where id=Object_id('dbo.ExchangeServerHealth'))
BEGIN	
	drop table dbo.ExchangeServerHealth
END

IF EXISTS(select * from syscolumns where id=Object_id('dbo.InputParameters'))
BEGIN	
	drop table dbo.InputParameters
END

IF EXISTS(select * from syscolumns where id=Object_id('dbo.OutputFields'))
BEGIN	
	drop table dbo.OutputFields
END

IF EXISTS(select * from syscolumns where id=Object_id('dbo.OutputTables'))
BEGIN	
	drop table dbo.OutputTables
END

IF EXISTS(select * from syscolumns where id=Object_id('dbo.PowershellScripts'))
BEGIN	
	drop table dbo.PowershellScripts
END

IF EXISTS(select * from syscolumns where id=Object_id('dbo.PowershellScripts1'))
BEGIN	
	drop table dbo.PowershellScripts1
END

IF EXISTS (select * from syscolumns where id = object_id('dbo.EXGServiceMaster'))
BEGIN
drop table EXGServiceMaster
End

IF EXISTS (select * from syscolumns where id = object_id('dbo.EXGServiceSettings'))
BEGIN
drop table EXGServiceSettings
END

-- DRS: Traveler Issues @ McKinsey.
use [vitalsigns]
go

IF NOT EXISTS (select * from syscolumns where name= 'MoreDetailsURL' and id = object_id('dbo.Traveler_Devices'))
BEGIN
	ALTER TABLE [dbo].[Traveler_Devices] ADD MoreDetailsURL nvarchar(500) NULL
END
GO

IF NOT EXISTS (select * from syscolumns where name= 'IsMoreDetailsFetched' and id = object_id('dbo.Traveler_Devices'))
BEGIN
	ALTER TABLE [dbo].[Traveler_Devices] ADD IsMoreDetailsFetched bit NULL
END
GO

IF EXISTS (select * from syscolumns where name= 'DeviceId' and id = object_id('dbo.Traveler_Devices'))
BEGIN
	ALTER TABLE [dbo].[Traveler_Devices] ALTER COLUMN DeviceId NVARCHAR(150)
END
GO

IF  NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[TRAVELER_DEVICES]') AND name = N'IX_TD_DEVICE_NAME')
	CREATE INDEX IX_TD_DEVICE_NAME ON [dbo].[TRAVELER_DEVICES] ([DeviceName])
GO

IF  NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[TRAVELER_DEVICES]') AND name = N'IX_TD_OS_TYPE')
	CREATE INDEX IX_TD_OS_TYPE ON [dbo].[TRAVELER_DEVICES] ([OS_Type])
GO


IF  NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[TRAVELER_DEVICES]') AND name = N'IX_TD_SERVER_DEVICE')
	CREATE UNIQUE INDEX IX_TD_SERVER_DEVICE ON [dbo].[TRAVELER_DEVICES] ([DeviceId],[ServerName])
GO

--CY: Created table ServerRoles for server and role mapping

USE VitalSigns
Go

IF NOT  EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS WHERE CONSTRAINT_TYPE = 'PRIMARY KEY' AND TABLE_NAME = 'RolesMaster' AND TABLE_SCHEMA ='dbo' )
BEGIN
Alter table RolesMaster
Add constraint PK_Rolesmaster primary key(id)
END
GO

USE VitalSigns
Go
IF NOT EXISTS (select * from syscolumns where id = object_id('dbo.ServerRoles'))
BEGIN
Create Table ServerRoles
( ServerId int not null,
RoleId int not null,
Foreign Key (ServerId) references servers(id) ON DELETE CASCADE,
Foreign Key (RoleId) references RolesMaster(id) 
)
End
Go

PRINT N' Executing V1.3.1 upgrade scripts ......... ';
/* 4/8/2014 NS added for VSPLUS-537 */
USE [vitalsigns]
GO
DELETE FROM EventsMaster WHERE EventName='SH TA String'
GO

/* 4/8/2014 NS added for VSPLUS-519 */
IF NOT EXISTS (select * from syscolumns where name= 'EnablePersistentAlert' and id = object_id('dbo.AlertDetails'))
BEGIN
ALTER TABLE [dbo].[AlertDetails] ADD EnablePersistentAlert bit NULL DEFAULT 0
End
GO
/* 4/8/2014 NS added for VSPLUS-519 */
IF EXISTS(SELECT * FROM syscolumns where id=Object_id('AlertDetails'))
BEGIN
	UPDATE AlertDetails SET EnablePersistentAlert = 0
END
GO
/* 4/8/2014 NS added for VSPLUS-403 */
IF NOT EXISTS (select * from Settings where sname= 'AlertsWinLog' )
BEGIN	
	INSERT [dbo].[Settings] ([sname], [svalue], [stype]) VALUES (N'AlertsWinLog', N'false', N'System.String')
END
GO
/* 4/9/2014 NS added for VSPLUS-519 */
IF NOT EXISTS (select * from Settings where sname= 'PersistentAlertInterval' )
BEGIN	
	INSERT [dbo].[Settings] ([sname], [svalue], [stype]) VALUES (N'PersistentAlertInterval', N'15', N'System.String')
END
GO
/* 4/16/2014 NS added for VSPLUS-519 */
IF NOT EXISTS (select * from Settings where sname= 'PersistentAlertDuration' )
BEGIN	
	INSERT [dbo].[Settings] ([sname], [svalue], [stype]) VALUES (N'PersistentAlertDuration', N'1', N'System.String')
END
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


/*CY: Deleting if the required name exists and inserting a new record */
	DELETE from [dbo].Settings where sname = 'ConsecutiveTelnet'
	INSERT [dbo].[Settings] ([sname], [svalue], [stype]) VALUES (N'ConsecutiveTelnet', N'8', N'System.Int32')

/* 11/20/2015 NS modified for VSPLUS-2182 */
USE [vitalsigns]
GO
IF EXISTS(SELECT * from ServerTypes WHERE ServerType = N'Mobile Users')
BEGIN	
	SET IDENTITY_INSERT [dbo].[ServerTypes] ON
	DELETE FROM ServerTypes WHERE ServerType='Mobile Users'
	SET IDENTITY_INSERT [dbo].[ServerTypes] OFF
END
GO

USE [vitalsigns]
GO

/****** Object:  Table [dbo].[SelectedFeatures]    Script Date: 04/25/2014 11:51:33 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SelectedFeatures]') AND type in (N'U'))
CREATE TABLE [dbo].[SelectedFeatures](
	[FeatureID] [int] NULL
) ON [PRIMARY]

GO

/*13Oct14 Mukund added*/
USE [vitalsigns]
GO
IF NOT EXISTS (select * from syscolumns where name= 'FeatureId' and id = object_id('dbo.ServerTypes'))
BEGIN	
	ALTER TABLE dbo.ServerTypes ADD FeatureId [int] null
END
GO

SET IDENTITY_INSERT [dbo].[ServerTypes] ON
INSERT [dbo].[ServerTypes] ([ID], [ServerType], [ServerTypeTable], [FeatureId]) VALUES (11, N'Mobile Users','Traveler_Devices',9)
SET IDENTITY_INSERT [dbo].[ServerTypes] OFF

use [vitalsigns]
go
IF NOT EXISTS (select * from EventsMaster where EventName='Overdue Device Sync' )
BEGIN	
	SET IDENTITY_INSERT [dbo].[EventsMaster] ON
	INSERT [dbo].[EventsMaster] ([ID], [EventName], [ServerTypeID]) VALUES (59, N'Overdue Device Sync', 11)
	SET IDENTITY_INSERT [dbo].[EventsMaster] OFF
END
GO

IF EXISTS(SELECT * from ServerTypes WHERE ServerType = N'Domino Cluster database')
BEGIN	
	SET IDENTITY_INSERT [dbo].[ServerTypes] ON
	DELETE FROM ServerTypes WHERE ServerType='Domino Cluster database'	
	SET IDENTITY_INSERT [dbo].[ServerTypes] OFF
END
GO
SET IDENTITY_INSERT [dbo].[ServerTypes] ON
INSERT [dbo].[ServerTypes] ([ID], [ServerType], [ServerTypeTable], [FeatureId]) VALUES (12, N'Domino Cluster database','',1)
SET IDENTITY_INSERT [dbo].[ServerTypes] OFF

IF EXISTS(SELECT * from ServerTypes WHERE ServerType = N'NotesMail Probe')
BEGIN	
	SET IDENTITY_INSERT [dbo].[ServerTypes] ON
	DELETE FROM ServerTypes WHERE ServerType='NotesMail Probe'	
	SET IDENTITY_INSERT [dbo].[ServerTypes] OFF
END
GO
SET IDENTITY_INSERT [dbo].[ServerTypes] ON
INSERT [dbo].[ServerTypes] ([ID], [ServerType], [ServerTypeTable], [FeatureId]) VALUES (13, N'NotesMail Probe','',4)
SET IDENTITY_INSERT [dbo].[ServerTypes] OFF

IF EXISTS(SELECT * from ServerTypes WHERE ServerType = N'Exchange Mail Flow')
BEGIN	
	SET IDENTITY_INSERT [dbo].[ServerTypes] ON
	DELETE FROM ServerTypes WHERE ServerType='Exchange Mail Flow'	
	SET IDENTITY_INSERT [dbo].[ServerTypes] OFF
END
GO
SET IDENTITY_INSERT [dbo].[ServerTypes] ON
INSERT [dbo].[ServerTypes] ([ID], [ServerType], [ServerTypeTable], [FeatureId]) VALUES (14, N'Exchange Mail Flow','',2)
SET IDENTITY_INSERT [dbo].[ServerTypes] OFF

IF EXISTS(SELECT * from ServerTypes WHERE ServerType = N'Windows')
BEGIN	
	SET IDENTITY_INSERT [dbo].[ServerTypes] ON
	DELETE FROM ServerTypes WHERE ServerType='Windows'	
	SET IDENTITY_INSERT [dbo].[ServerTypes] OFF
END
GO
SET IDENTITY_INSERT [dbo].[ServerTypes] ON
INSERT [dbo].[ServerTypes] ([ID], [ServerType], [ServerTypeTable], [FeatureId]) VALUES (16, N'Windows','',14)
SET IDENTITY_INSERT [dbo].[ServerTypes] OFF

IF EXISTS(SELECT * from ServerTypes WHERE ServerType = N'Active Directory')
BEGIN	
	SET IDENTITY_INSERT [dbo].[ServerTypes] ON
	DELETE FROM ServerTypes WHERE ServerType='Active Directory'	
	SET IDENTITY_INSERT [dbo].[ServerTypes] OFF
END
GO
SET IDENTITY_INSERT [dbo].[ServerTypes] ON
INSERT [dbo].[ServerTypes] ([ID], [ServerType], [ServerTypeTable], [FeatureId]) VALUES (18, N'Active Directory','',16)
SET IDENTITY_INSERT [dbo].[ServerTypes] OFF

IF EXISTS(SELECT * from ServerTypes WHERE ServerType = N'Network Latency')
BEGIN	
	SET IDENTITY_INSERT [dbo].[ServerTypes] ON
	DELETE FROM ServerTypes WHERE ServerType='Network Latency'	
	SET IDENTITY_INSERT [dbo].[ServerTypes] OFF
END
GO
SET IDENTITY_INSERT [dbo].[ServerTypes] ON
INSERT [dbo].[ServerTypes] ([ID], [ServerType], [ServerTypeTable], [FeatureId]) VALUES (23, N'Network Latency','NetworkLatency',14)
SET IDENTITY_INSERT [dbo].[ServerTypes] OFF




IF EXISTS(SELECT * from EventsMaster WHERE EventName = N'Stat.Statistic')
BEGIN	
	-- Joe Thumma For Server Task Allert
	DELETE FROM [dbo].[EventsMaster] WHERE EventName = N'Stat.Statistic'	
END

IF NOT EXISTS(SELECT * from EventsMaster WHERE EventName = N'Server Task')
BEGIN	
	SET IDENTITY_INSERT [dbo].[EventsMaster] ON
	-- Joe Thumma For Server Task Allert
	INSERT [dbo].[EventsMaster] ([ID], [EventName], [ServerTypeID]) VALUES (60, N'Server Task', 1)
	SET IDENTITY_INSERT [dbo].[EventsMaster] OFF
END


IF NOT EXISTS(SELECT * from EventsMaster WHERE EventName = N'Domino Statistic')
BEGIN	
	SET IDENTITY_INSERT [dbo].[EventsMaster] ON
	-- Joe Thumma For Server Task Allert
	INSERT [dbo].[EventsMaster] ([ID], [EventName], [ServerTypeID]) VALUES (19, N'Domino Statistic', 1)
	SET IDENTITY_INSERT [dbo].[EventsMaster] OFF
END


IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[MobileUserThreshold]') AND type in (N'U'))
BEGIN
CREATE TABLE dbo.MobileUserThreshold(
	DeviceId varchar(200) NOT NULL,
	SyncTimeThreshold	int
)
END
GO

delete from [dbo].[MenuItems] where id = 64
IF OBJECTPROPERTY(OBJECT_ID('dbo.MenuItems'), 'TableHasIdentity') = 1 
BEGIN
	SET IDENTITY_INSERT [dbo].[MenuItems] ON
END 
GO
INSERT [dbo].[MenuItems] ([ID], [DisplayText], [OrderNum], [ParentID], [PageLink], [Level], [RefName], [ImageURL]) values (64,'Mobile Users',7,2,'/Configurator/MobileUsersGrid.aspx',2,'MobileUsers','~/images/icons/phone.png')
GO
IF OBJECTPROPERTY(OBJECT_ID('dbo.MenuItems'), 'TableHasIdentity') = 1 
BEGIN
	SET IDENTITY_INSERT [dbo].[MenuItems] OFF
END 
GO


/* 4/10/2014 NS added - the only reports currently available for scheduling are the ones below */
USE [vitalsigns]
GO

UPDATE [dbo].[ReportItems] SET MaySchedule = NULL
WHERE ID NOT IN (11,22,34)
GO

UPDATE [dbo].[ReportItems] SET MaySchedule = 1
WHERE ID IN (11,22,34)
GO


--CY: 4/11/2014
USE [vitalsigns]
GO
IF NOT EXISTS(SELECT * from ServerTypes WHERE ServerType = N'Domino Cluster database')
BEGIN	
	SET IDENTITY_INSERT [dbo].[ServerTypes] ON
	Insert into ServerTypes (ID,ServerType,ServerTypeTable) values(12,'Domino Cluster database','')
	SET IDENTITY_INSERT [dbo].[ServerTypes] OFF
END
GO
USE [vitalsigns]
GO
IF NOT EXISTS(SELECT * from ServerTypes WHERE ServerType = N'NotesMail Probe')
BEGIN	
	SET IDENTITY_INSERT [dbo].[ServerTypes] ON
	Insert into ServerTypes (ID,ServerType,ServerTypeTable) values(13,'NotesMail Probe','')
	SET IDENTITY_INSERT [dbo].[ServerTypes] OFF
END
GO

/* 9/12/2014 NS modified for VSPLUS-839; removed extra space from 'Cluster Replicator Delay ' */
USE [vitalsigns]
GO
DELETE FROM [dbo].[EventsMaster] where id in (61,62,63,64,65,66,67,68)
SET IDENTITY_INSERT [dbo].[EventsMaster] ON
INSERT [dbo].[EventsMaster] ([ID], [EventName], [ServerTypeID]) VALUES (61, N'Missing Replica', 12)
INSERT [dbo].[EventsMaster] ([ID], [EventName], [ServerTypeID]) VALUES (62, N'Cluster Replication', 12)
INSERT [dbo].[EventsMaster] ([ID], [EventName], [ServerTypeID]) VALUES (63, N'Slow', 13)
INSERT [dbo].[EventsMaster] ([ID], [EventName], [ServerTypeID]) VALUES (64, N'Failure', 13)
INSERT [dbo].[EventsMaster] ([ID], [EventName], [ServerTypeID]) VALUES (65, N'Cluster Replicator Delay', 1)
INSERT [dbo].[EventsMaster] ([ID], [EventName], [ServerTypeID]) VALUES (66, N'Resolve', 3)
INSERT [dbo].[EventsMaster] ([ID], [EventName], [ServerTypeID]) VALUES (67, N'Status Change', 3)
INSERT [dbo].[EventsMaster] ([ID], [EventName], [ServerTypeID]) VALUES (68, N'IM', 3)
SET IDENTITY_INSERT [dbo].[EventsMaster] OFF
GO

USE [vitalsigns]
GO
-- JT 4/15 Just a clean up before starting the services.
Delete from Status
GO

USE [vitalsigns]
GO

-- VSPLUS-555 The mobile devices traveler bar graph should be consolidated into general categories of devices
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DeviceTypeTranslation]') AND type in (N'U'))
BEGIN
	CREATE TABLE dbo.DeviceTypeTranslation(
	DeviceType varchar(100) NOT NULL,
	TranslatedValue	varchar(100) NOT NULL,
	OSName varchar(50) NOT NULL
	)

	CREATE UNIQUE INDEX UX_DEVICE_TRANSLATION ON dbo.DeviceTypeTranslation(DeviceType)
END
go


IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[OSTypeTranslation]') AND type in (N'U'))
BEGIN
	CREATE TABLE dbo.OSTypeTranslation(
	OSType varchar(100) NOT NULL,
	TranslatedValue	varchar(100) NOT NULL,
	OSName varchar(50) NOT NULL
	)
	CREATE UNIQUE INDEX UX_OS_TRANSLATION ON dbo.OSTypeTranslation(OSType)
END
go


--VSPLUS-707 Random numbers reported for device OS
IF NOT EXISTS (select * from syscolumns where name= 'OSName' and id = object_id('dbo.DeviceTypeTranslation'))
BEGIN
ALTER TABLE [dbo].[DeviceTypeTranslation] ADD OSName [nvarchar](50) NULL
End
GO

IF NOT EXISTS (select * from syscolumns where name= 'OSName' and id = object_id('dbo.OSTypeTranslation'))
BEGIN
ALTER TABLE [dbo].[OSTypeTranslation] ADD OSName [nvarchar](50) NULL
End
GO



-- VSPLUS-555 The mobile devices traveler bar graph should be consolidated into general categories of devices
DELETE FROM dbo.DeviceTypeTranslation
GO
INSERT [dbo].[DeviceTypeTranslation] ([DeviceType], [TranslatedValue], [OSName]) VALUES (N'5860E', N'Coolpad', N'Android')
INSERT [dbo].[DeviceTypeTranslation] ([DeviceType], [TranslatedValue], [OSName]) VALUES (N'ADR6300', N'HTC Droid', N'Android')
INSERT [dbo].[DeviceTypeTranslation] ([DeviceType], [TranslatedValue], [OSName]) VALUES (N'ADR6330VW', N'HTC Rhyme', N'Android')
INSERT [dbo].[DeviceTypeTranslation] ([DeviceType], [TranslatedValue], [OSName]) VALUES (N'ADR6350', N'HTC Droid', N'Android')
INSERT [dbo].[DeviceTypeTranslation] ([DeviceType], [TranslatedValue], [OSName]) VALUES (N'ADR6400L', N'HTC Thunderbolt', N'Android')
INSERT [dbo].[DeviceTypeTranslation] ([DeviceType], [TranslatedValue], [OSName]) VALUES (N'ADR6410LVW', N'HTC Droid', N'Android')
INSERT [dbo].[DeviceTypeTranslation] ([DeviceType], [TranslatedValue], [OSName]) VALUES (N'ADR6425LVW', N'HTC Rezound', N'Android')
INSERT [dbo].[DeviceTypeTranslation] ([DeviceType], [TranslatedValue], [OSName]) VALUES (N'ASUS_Transformer_Pad_TF300T', N'Asus', N'Android')
INSERT [dbo].[DeviceTypeTranslation] ([DeviceType], [TranslatedValue], [OSName]) VALUES (N'ASUS_Transformer_Pad_TF700T', N'Asus', N'Android')
INSERT [dbo].[DeviceTypeTranslation] ([DeviceType], [TranslatedValue], [OSName]) VALUES (N'C5155', N'Kyocera', N'Android')
INSERT [dbo].[DeviceTypeTranslation] ([DeviceType], [TranslatedValue], [OSName]) VALUES (N'C5170', N'Kyocera', N'Android')
INSERT [dbo].[DeviceTypeTranslation] ([DeviceType], [TranslatedValue], [OSName]) VALUES (N'C6603', N'Sony', N'Android')
INSERT [dbo].[DeviceTypeTranslation] ([DeviceType], [TranslatedValue], [OSName]) VALUES (N'C6606', N'Sony', N'Android')
INSERT [dbo].[DeviceTypeTranslation] ([DeviceType], [TranslatedValue], [OSName]) VALUES (N'C6903', N'Sony', N'Android')
INSERT [dbo].[DeviceTypeTranslation] ([DeviceType], [TranslatedValue], [OSName]) VALUES (N'Desire_HD', N'HTC Desire', N'Android')
INSERT [dbo].[DeviceTypeTranslation] ([DeviceType], [TranslatedValue], [OSName]) VALUES (N'DROID2_GLOBAL', N'Motorola Droid', N'Android')
INSERT [dbo].[DeviceTypeTranslation] ([DeviceType], [TranslatedValue], [OSName]) VALUES (N'DROID2', N'Motorola Droid', N'Android')
INSERT [dbo].[DeviceTypeTranslation] ([DeviceType], [TranslatedValue], [OSName]) VALUES (N'DROID3', N'Motorola Droid', N'Android')
INSERT [dbo].[DeviceTypeTranslation] ([DeviceType], [TranslatedValue], [OSName]) VALUES (N'DROID4', N'Motorola Droid', N'Android')
INSERT [dbo].[DeviceTypeTranslation] ([DeviceType], [TranslatedValue], [OSName]) VALUES (N'DROID_BIONIC', N'Motorola Droid', N'Android')
INSERT [dbo].[DeviceTypeTranslation] ([DeviceType], [TranslatedValue], [OSName]) VALUES (N'DROID', N'Motorola Droid', N'Android')
INSERT [dbo].[DeviceTypeTranslation] ([DeviceType], [TranslatedValue], [OSName]) VALUES (N'DROID_PRO', N'Motorola Droid', N'Android')
INSERT [dbo].[DeviceTypeTranslation] ([DeviceType], [TranslatedValue], [OSName]) VALUES (N'DROID_RAZR_HD', N'Motorola Droid', N'Android')
INSERT [dbo].[DeviceTypeTranslation] ([DeviceType], [TranslatedValue], [OSName]) VALUES (N'DROID_RAZR', N'Motorola Droid', N'Android')
INSERT [dbo].[DeviceTypeTranslation] ([DeviceType], [TranslatedValue], [OSName]) VALUES (N'DROID_X2', N'Motorola Droid', N'Android')
INSERT [dbo].[DeviceTypeTranslation] ([DeviceType], [TranslatedValue], [OSName]) VALUES (N'DROID_X', N'Motorola Droid', N'Android')
INSERT [dbo].[DeviceTypeTranslation] ([DeviceType], [TranslatedValue], [OSName]) VALUES (N'EVO', N'HTC Evo', N'Android')
INSERT [dbo].[DeviceTypeTranslation] ([DeviceType], [TranslatedValue], [OSName]) VALUES (N'Galaxy_Nexus', N'Samsung Galaxy Nexus', N'Android')
INSERT [dbo].[DeviceTypeTranslation] ([DeviceType], [TranslatedValue], [OSName]) VALUES (N'google_sdk', N'Android Emulator', N'Android')
INSERT [dbo].[DeviceTypeTranslation] ([DeviceType], [TranslatedValue], [OSName]) VALUES (N'I8160', N'Samsung Unknown', N'Android')
INSERT [dbo].[DeviceTypeTranslation] ([DeviceType], [TranslatedValue], [OSName]) VALUES (N'GT-I8190N', N'Samsung Galaxy S III Mini', N'Android')
INSERT [dbo].[DeviceTypeTranslation] ([DeviceType], [TranslatedValue], [OSName]) VALUES (N'GT-I8190', N'Samsung Galaxy S III Mini', N'Android')
INSERT [dbo].[DeviceTypeTranslation] ([DeviceType], [TranslatedValue], [OSName]) VALUES (N'GT-I9000', N'Samsung Galaxy S', N'Android')
INSERT [dbo].[DeviceTypeTranslation] ([DeviceType], [TranslatedValue], [OSName]) VALUES (N'GT-I9001', N'Samsung Unknown', N'Android')
INSERT [dbo].[DeviceTypeTranslation] ([DeviceType], [TranslatedValue], [OSName]) VALUES (N'GT-I9070', N'Samsung Unknown', N'Android')
INSERT [dbo].[DeviceTypeTranslation] ([DeviceType], [TranslatedValue], [OSName]) VALUES (N'GT-I9082', N'Samsung Unknown', N'Android')
INSERT [dbo].[DeviceTypeTranslation] ([DeviceType], [TranslatedValue], [OSName]) VALUES (N'GT-I9100M', N'Samsung Galaxy S II', N'Android')
INSERT [dbo].[DeviceTypeTranslation] ([DeviceType], [TranslatedValue], [OSName]) VALUES (N'GT-I9100P', N'Samsung Galaxy S II', N'Android')
INSERT [dbo].[DeviceTypeTranslation] ([DeviceType], [TranslatedValue], [OSName]) VALUES (N'GT-I9100', N'Samsung Galaxy S II', N'Android')
INSERT [dbo].[DeviceTypeTranslation] ([DeviceType], [TranslatedValue], [OSName]) VALUES (N'GT-I9100T', N'Samsung Galaxy S II', N'Android')
INSERT [dbo].[DeviceTypeTranslation] ([DeviceType], [TranslatedValue], [OSName]) VALUES (N'GT-I9195', N'Samsung Galaxy S4 Mini', N'Android')
INSERT [dbo].[DeviceTypeTranslation] ([DeviceType], [TranslatedValue], [OSName]) VALUES (N'GT-I9300', N'Samsung Galaxy S III', N'Android')
INSERT [dbo].[DeviceTypeTranslation] ([DeviceType], [TranslatedValue], [OSName]) VALUES (N'GT-I9300T', N'Samsung Galaxy S III', N'Android')
INSERT [dbo].[DeviceTypeTranslation] ([DeviceType], [TranslatedValue], [OSName]) VALUES (N'GT-I9305', N'Samsung Galaxy S III', N'Android')
INSERT [dbo].[DeviceTypeTranslation] ([DeviceType], [TranslatedValue], [OSName]) VALUES (N'GT-I9305T', N'Samsung Galaxy S III', N'Android')
INSERT [dbo].[DeviceTypeTranslation] ([DeviceType], [TranslatedValue], [OSName]) VALUES (N'GT-I95', N'Samsung Galaxy S4', N'Android')
INSERT [dbo].[DeviceTypeTranslation] ([DeviceType], [TranslatedValue], [OSName]) VALUES (N'GT-N', N'Samsung Galaxy Note', N'Android')
INSERT [dbo].[DeviceTypeTranslation] ([DeviceType], [TranslatedValue], [OSName]) VALUES (N'GT-P31', N'Samsung Galaxy Tab', N'Android')
INSERT [dbo].[DeviceTypeTranslation] ([DeviceType], [TranslatedValue], [OSName]) VALUES (N'GT-P51', N'Samsung Galaxy Tab', N'Android')
INSERT [dbo].[DeviceTypeTranslation] ([DeviceType], [TranslatedValue], [OSName]) VALUES (N'GT-P52', N'Samsung Galaxy Tab', N'Android')
INSERT [dbo].[DeviceTypeTranslation] ([DeviceType], [TranslatedValue], [OSName]) VALUES (N'GT-P7510', N'Samsung Galaxy Tab', N'Android')
INSERT [dbo].[DeviceTypeTranslation] ([DeviceType], [TranslatedValue], [OSName]) VALUES (N'GT-S5360', N'Samsung Unknown', N'Android')
INSERT [dbo].[DeviceTypeTranslation] ([DeviceType], [TranslatedValue], [OSName]) VALUES (N'GT-S5570', N'Samsung Unknown', N'Android')
INSERT [dbo].[DeviceTypeTranslation] ([DeviceType], [TranslatedValue], [OSName]) VALUES (N'GT-S5830i', N'Samsung Unknown', N'Android')
INSERT [dbo].[DeviceTypeTranslation] ([DeviceType], [TranslatedValue], [OSName]) VALUES (N'GT-S5830', N'Samsung Unknown', N'Android')
INSERT [dbo].[DeviceTypeTranslation] ([DeviceType], [TranslatedValue], [OSName]) VALUES (N'GT-S7562', N'Samsung Unknown', N'Android')
INSERT [dbo].[DeviceTypeTranslation] ([DeviceType], [TranslatedValue], [OSName]) VALUES (N'HTC6435LVW', N'HTC Droid', N'Android')
INSERT [dbo].[DeviceTypeTranslation] ([DeviceType], [TranslatedValue], [OSName]) VALUES (N'HTC6500LVW', N'HTC One', N'Android')
INSERT [dbo].[DeviceTypeTranslation] ([DeviceType], [TranslatedValue], [OSName]) VALUES (N'HTC_Amaze_4G', N'HTC Amaze', N'Android')
INSERT [dbo].[DeviceTypeTranslation] ([DeviceType], [TranslatedValue], [OSName]) VALUES (N'HTC_Desire', N'HTC Desire', N'Android')
INSERT [dbo].[DeviceTypeTranslation] ([DeviceType], [TranslatedValue], [OSName]) VALUES (N'HTC_Desire_C', N'HTC Desire', N'Android')
INSERT [dbo].[DeviceTypeTranslation] ([DeviceType], [TranslatedValue], [OSName]) VALUES (N'HTC_Desire_HD_A9191', N'HTC Desire', N'Android')
INSERT [dbo].[DeviceTypeTranslation] ([DeviceType], [TranslatedValue], [OSName]) VALUES (N'HTC_Desire_S', N'HTC Desire', N'Android')
INSERT [dbo].[DeviceTypeTranslation] ([DeviceType], [TranslatedValue], [OSName]) VALUES (N'HTCEVODesign4G', N'HTC Evo', N'Android')
INSERT [dbo].[DeviceTypeTranslation] ([DeviceType], [TranslatedValue], [OSName]) VALUES (N'HTCEVOV4G', N'HTC Evo', N'Android')
INSERT [dbo].[DeviceTypeTranslation] ([DeviceType], [TranslatedValue], [OSName]) VALUES (N'HTC_Glacier', N'HTC Glacier', N'Android')
INSERT [dbo].[DeviceTypeTranslation] ([DeviceType], [TranslatedValue], [OSName]) VALUES (N'HTC_Incredible_S', N'HTC Incredible', N'Android')
INSERT [dbo].[DeviceTypeTranslation] ([DeviceType], [TranslatedValue], [OSName]) VALUES (N'HTC_One', N'HTC One', N'Android')
INSERT [dbo].[DeviceTypeTranslation] ([DeviceType], [TranslatedValue], [OSName]) VALUES (N'HTCONE', N'HTC One', N'Android')
INSERT [dbo].[DeviceTypeTranslation] ([DeviceType], [TranslatedValue], [OSName]) VALUES (N'HTC_One_S', N'HTC One', N'Android')
INSERT [dbo].[DeviceTypeTranslation] ([DeviceType], [TranslatedValue], [OSName]) VALUES (N'HTC_One_V', N'HTC One', N'Android')
INSERT [dbo].[DeviceTypeTranslation] ([DeviceType], [TranslatedValue], [OSName]) VALUES (N'HTC_One_X', N'HTC One', N'Android')
INSERT [dbo].[DeviceTypeTranslation] ([DeviceType], [TranslatedValue], [OSName]) VALUES (N'HTC_One_X+', N'HTC One', N'Android')
INSERT [dbo].[DeviceTypeTranslation] ([DeviceType], [TranslatedValue], [OSName]) VALUES (N'HTC_PH39100', N'HTC Vivid', N'Android')
INSERT [dbo].[DeviceTypeTranslation] ([DeviceType], [TranslatedValue], [OSName]) VALUES (N'HTC_PN071', N'HTC One', N'Android')
INSERT [dbo].[DeviceTypeTranslation] ([DeviceType], [TranslatedValue], [OSName]) VALUES (N'HTC_Sensation_4G', N'HTC Sensation', N'Android')
INSERT [dbo].[DeviceTypeTranslation] ([DeviceType], [TranslatedValue], [OSName]) VALUES (N'HTC_Sensation_Z710e', N'HTC Sensation', N'Android')
INSERT [dbo].[DeviceTypeTranslation] ([DeviceType], [TranslatedValue], [OSName]) VALUES (N'HTC_VLE_U', N'HTC One', N'Android')
INSERT [dbo].[DeviceTypeTranslation] ([DeviceType], [TranslatedValue], [OSName]) VALUES (N'KFJWA', N'Kindle Fire', N'Android')
INSERT [dbo].[DeviceTypeTranslation] ([DeviceType], [TranslatedValue], [OSName]) VALUES (N'KFJWI', N'Kindle Fire', N'Android')
INSERT [dbo].[DeviceTypeTranslation] ([DeviceType], [TranslatedValue], [OSName]) VALUES (N'KFOT', N'Kindle Fire', N'Android')
INSERT [dbo].[DeviceTypeTranslation] ([DeviceType], [TranslatedValue], [OSName]) VALUES (N'KFTT', N'Kindle Fire', N'Android')
INSERT [dbo].[DeviceTypeTranslation] ([DeviceType], [TranslatedValue], [OSName]) VALUES (N'Kindle_Fire', N'Kindle Fire', N'Android')
INSERT [dbo].[DeviceTypeTranslation] ([DeviceType], [TranslatedValue], [OSName]) VALUES (N'LG-C800', N'LG myTouch', N'Android')
INSERT [dbo].[DeviceTypeTranslation] ([DeviceType], [TranslatedValue], [OSName]) VALUES (N'LG-D800', N'LG G2', N'Android')
INSERT [dbo].[DeviceTypeTranslation] ([DeviceType], [TranslatedValue], [OSName]) VALUES (N'LG-C801', N'LG G2', N'Android')
INSERT [dbo].[DeviceTypeTranslation] ([DeviceType], [TranslatedValue], [OSName]) VALUES (N'LG-C802', N'LG G2', N'Android')
INSERT [dbo].[DeviceTypeTranslation] ([DeviceType], [TranslatedValue], [OSName]) VALUES (N'LG-E739', N'LG MyTouch', N'Android')
INSERT [dbo].[DeviceTypeTranslation] ([DeviceType], [TranslatedValue], [OSName]) VALUES (N'LG-E970', N'LG Optimus', N'Android')
INSERT [dbo].[DeviceTypeTranslation] ([DeviceType], [TranslatedValue], [OSName]) VALUES (N'LG-E980', N'LG Optimus', N'Android')
INSERT [dbo].[DeviceTypeTranslation] ([DeviceType], [TranslatedValue], [OSName]) VALUES (N'LGL55C', N'LG LGL55C', N'Android')
INSERT [dbo].[DeviceTypeTranslation] ([DeviceType], [TranslatedValue], [OSName]) VALUES (N'LG-LS840', N'LG Viper', N'Android')
INSERT [dbo].[DeviceTypeTranslation] ([DeviceType], [TranslatedValue], [OSName]) VALUES (N'LG-LS970', N'LG Optimus', N'Android')
INSERT [dbo].[DeviceTypeTranslation] ([DeviceType], [TranslatedValue], [OSName]) VALUES (N'LG-LS980', N'LG G2', N'Android')
INSERT [dbo].[DeviceTypeTranslation] ([DeviceType], [TranslatedValue], [OSName]) VALUES (N'LGMS769', N'LG Optimus', N'Android')
INSERT [dbo].[DeviceTypeTranslation] ([DeviceType], [TranslatedValue], [OSName]) VALUES (N'LG-MS770', N'LG Motion', N'Android')
GO
print 'Processed 100 total records'
INSERT [dbo].[DeviceTypeTranslation] ([DeviceType], [TranslatedValue], [OSName]) VALUES (N'LG-MS910', N'LG Esteem', N'Android')
INSERT [dbo].[DeviceTypeTranslation] ([DeviceType], [TranslatedValue], [OSName]) VALUES (N'LG-P509', N'LG Optimus', N'Android')
INSERT [dbo].[DeviceTypeTranslation] ([DeviceType], [TranslatedValue], [OSName]) VALUES (N'LG-P769', N'LG Optimus', N'Android')
INSERT [dbo].[DeviceTypeTranslation] ([DeviceType], [TranslatedValue], [OSName]) VALUES (N'LG-P999', N'LG G2X', N'Android')
INSERT [dbo].[DeviceTypeTranslation] ([DeviceType], [TranslatedValue], [OSName]) VALUES (N'LG-VM696', N'LG Optimus', N'Android')
INSERT [dbo].[DeviceTypeTranslation] ([DeviceType], [TranslatedValue], [OSName]) VALUES (N'LS670', N'LG Optimus', N'Android')
INSERT [dbo].[DeviceTypeTranslation] ([DeviceType], [TranslatedValue], [OSName]) VALUES (N'LT26i', N'Sony', N'Android')
INSERT [dbo].[DeviceTypeTranslation] ([DeviceType], [TranslatedValue], [OSName]) VALUES (N'MB855', N'Motorola Photon', N'Android')
INSERT [dbo].[DeviceTypeTranslation] ([DeviceType], [TranslatedValue], [OSName]) VALUES (N'MB860', N'Motorola Atrix', N'Android')
INSERT [dbo].[DeviceTypeTranslation] ([DeviceType], [TranslatedValue], [OSName]) VALUES (N'MB865', N'Motorola Atrix', N'Android')
INSERT [dbo].[DeviceTypeTranslation] ([DeviceType], [TranslatedValue], [OSName]) VALUES (N'MB886', N'Motorola Atrix', N'Android')
INSERT [dbo].[DeviceTypeTranslation] ([DeviceType], [TranslatedValue], [OSName]) VALUES (N'Motorola_Electrify', N'Motorola Electrify', N'Android')
INSERT [dbo].[DeviceTypeTranslation] ([DeviceType], [TranslatedValue], [OSName]) VALUES (N'MOTWX435KT', N'Motorola Triumph', N'Android')
INSERT [dbo].[DeviceTypeTranslation] ([DeviceType], [TranslatedValue], [OSName]) VALUES (N'myTouch_4G_Slide', N'HTC myTouch', N'Android')
INSERT [dbo].[DeviceTypeTranslation] ([DeviceType], [TranslatedValue], [OSName]) VALUES (N'N860', N'ZTE Warp N860', N'Android')
INSERT [dbo].[DeviceTypeTranslation] ([DeviceType], [TranslatedValue], [OSName]) VALUES (N'Nexus_10', N'Google Nexus', N'Android')
INSERT [dbo].[DeviceTypeTranslation] ([DeviceType], [TranslatedValue], [OSName]) VALUES (N'Nexus_4', N'Google Nexus', N'Android')
INSERT [dbo].[DeviceTypeTranslation] ([DeviceType], [TranslatedValue], [OSName]) VALUES (N'Nexus_5', N'Google Nexus', N'Android')
INSERT [dbo].[DeviceTypeTranslation] ([DeviceType], [TranslatedValue], [OSName]) VALUES (N'Nexus_7', N'Google Nexus', N'Android')
INSERT [dbo].[DeviceTypeTranslation] ([DeviceType], [TranslatedValue], [OSName]) VALUES (N'Nexus_S_4G', N'Google Nexus', N'Android')
INSERT [dbo].[DeviceTypeTranslation] ([DeviceType], [TranslatedValue], [OSName]) VALUES (N'Nexus_S', N'Google Nexus', N'Android')
INSERT [dbo].[DeviceTypeTranslation] ([DeviceType], [TranslatedValue], [OSName]) VALUES (N'PantechP9070', N'Pantech Burst', N'Android')
INSERT [dbo].[DeviceTypeTranslation] ([DeviceType], [TranslatedValue], [OSName]) VALUES (N'PC36100', N'HTC Evo', N'Android')
INSERT [dbo].[DeviceTypeTranslation] ([DeviceType], [TranslatedValue], [OSName]) VALUES (N'PG06100', N'HTC EVO', N'Android')
INSERT [dbo].[DeviceTypeTranslation] ([DeviceType], [TranslatedValue], [OSName]) VALUES (N'PG86100', N'HTC Evo', N'Android')
INSERT [dbo].[DeviceTypeTranslation] ([DeviceType], [TranslatedValue], [OSName]) VALUES (N'PH44100', N'HTC Evo', N'Android')
INSERT [dbo].[DeviceTypeTranslation] ([DeviceType], [TranslatedValue], [OSName]) VALUES (N'SAMSUNG-SGH-I317', N'Samsung Galaxy Note', N'Android')
INSERT [dbo].[DeviceTypeTranslation] ([DeviceType], [TranslatedValue], [OSName]) VALUES (N'SAMSUNG-SGH-I337', N'Samsung Galaxy S4', N'Android')
INSERT [dbo].[DeviceTypeTranslation] ([DeviceType], [TranslatedValue], [OSName]) VALUES (N'SAMSUNG-SGH-I537', N'Samsung Galaxy S4', N'Android')
INSERT [dbo].[DeviceTypeTranslation] ([DeviceType], [TranslatedValue], [OSName]) VALUES (N'SAMSUNG-SGH-I717', N'Samsung Galaxy Note', N'Android')
INSERT [dbo].[DeviceTypeTranslation] ([DeviceType], [TranslatedValue], [OSName]) VALUES (N'SAMSUNG-SGH-I727', N'Samsung Skyrocket', N'Android')
INSERT [dbo].[DeviceTypeTranslation] ([DeviceType], [TranslatedValue], [OSName]) VALUES (N'SAMSUNG-SGH-I747', N'Samsung Galaxy S III', N'Android')
INSERT [dbo].[DeviceTypeTranslation] ([DeviceType], [TranslatedValue], [OSName]) VALUES (N'SAMSUNG-SGH-I777', N'Samsung Galaxy S II', N'Android')
INSERT [dbo].[DeviceTypeTranslation] ([DeviceType], [TranslatedValue], [OSName]) VALUES (N'SAMSUNG-SGH-I897', N'Samsung Unknown', N'Android')
INSERT [dbo].[DeviceTypeTranslation] ([DeviceType], [TranslatedValue], [OSName]) VALUES (N'SAMSUNG-SGH-I927', N'Samsung Unknown', N'Android')
INSERT [dbo].[DeviceTypeTranslation] ([DeviceType], [TranslatedValue], [OSName]) VALUES (N'SAMSUNG-SGH-I997', N'Samsung Unknown', N'Android')
INSERT [dbo].[DeviceTypeTranslation] ([DeviceType], [TranslatedValue], [OSName]) VALUES (N'SM-N9', N'Samsung Galaxy Note', N'Android')
INSERT [dbo].[DeviceTypeTranslation] ([DeviceType], [TranslatedValue], [OSName]) VALUES (N'SCH-I200', N'Samsung Unknown', N'Android')
INSERT [dbo].[DeviceTypeTranslation] ([DeviceType], [TranslatedValue], [OSName]) VALUES (N'SCH-I405', N'Samsung Unknown', N'Android')
INSERT [dbo].[DeviceTypeTranslation] ([DeviceType], [TranslatedValue], [OSName]) VALUES (N'SCH-I415', N'Samsung Unknown', N'Android')
INSERT [dbo].[DeviceTypeTranslation] ([DeviceType], [TranslatedValue], [OSName]) VALUES (N'SCH-I500', N'Samsung Unknown', N'Android')
INSERT [dbo].[DeviceTypeTranslation] ([DeviceType], [TranslatedValue], [OSName]) VALUES (N'SCH-I510', N'Samsung Unknown', N'Android')
INSERT [dbo].[DeviceTypeTranslation] ([DeviceType], [TranslatedValue], [OSName]) VALUES (N'SCH-I535', N'Samsung Galaxy S III', N'Android')
INSERT [dbo].[DeviceTypeTranslation] ([DeviceType], [TranslatedValue], [OSName]) VALUES (N'SCH-I545', N'Samsung Galaxy S4', N'Android')
INSERT [dbo].[DeviceTypeTranslation] ([DeviceType], [TranslatedValue], [OSName]) VALUES (N'SCH-I605', N'Samsung Galaxy Note', N'Android')
INSERT [dbo].[DeviceTypeTranslation] ([DeviceType], [TranslatedValue], [OSName]) VALUES (N'SCH-I800', N'Samsung Galaxy Tab', N'Android')
INSERT [dbo].[DeviceTypeTranslation] ([DeviceType], [TranslatedValue], [OSName]) VALUES (N'SCH-R530M', N'Samsung Galaxy S III', N'Android')
INSERT [dbo].[DeviceTypeTranslation] ([DeviceType], [TranslatedValue], [OSName]) VALUES (N'SCH-R530U', N'Samsung Galaxy S III', N'Android')
INSERT [dbo].[DeviceTypeTranslation] ([DeviceType], [TranslatedValue], [OSName]) VALUES (N'SCH-R720', N'Samsung Unknown', N'Android')
INSERT [dbo].[DeviceTypeTranslation] ([DeviceType], [TranslatedValue], [OSName]) VALUES (N'SCH-R720C', N'Samsung Unknown', N'Android')
INSERT [dbo].[DeviceTypeTranslation] ([DeviceType], [TranslatedValue], [OSName]) VALUES (N'SCH-R738C', N'Samsung Unknown', N'Android')
INSERT [dbo].[DeviceTypeTranslation] ([DeviceType], [TranslatedValue], [OSName]) VALUES (N'SGH-I317M', N'Samsung Galaxy Note', N'Android')
INSERT [dbo].[DeviceTypeTranslation] ([DeviceType], [TranslatedValue], [OSName]) VALUES (N'SGH-I337M', N'Samsung Galaxy S4', N'Android')
INSERT [dbo].[DeviceTypeTranslation] ([DeviceType], [TranslatedValue], [OSName]) VALUES (N'SGH-I727R', N'Samsung Galaxy S II', N'Android')
INSERT [dbo].[DeviceTypeTranslation] ([DeviceType], [TranslatedValue], [OSName]) VALUES (N'SGH-I747M', N'Samsung Galaxy S III', N'Android')
INSERT [dbo].[DeviceTypeTranslation] ([DeviceType], [TranslatedValue], [OSName]) VALUES (N'SGH-M919', N'Samsung Galaxy S4', N'Android')
INSERT [dbo].[DeviceTypeTranslation] ([DeviceType], [TranslatedValue], [OSName]) VALUES (N'SGH-T599N', N'Samsung Unknown', N'Android')
INSERT [dbo].[DeviceTypeTranslation] ([DeviceType], [TranslatedValue], [OSName]) VALUES (N'SGH-T679', N'Samsung Unknown', N'Android')
INSERT [dbo].[DeviceTypeTranslation] ([DeviceType], [TranslatedValue], [OSName]) VALUES (N'SGH-T769', N'Samsung Unknown', N'Android')
INSERT [dbo].[DeviceTypeTranslation] ([DeviceType], [TranslatedValue], [OSName]) VALUES (N'SGH-T889', N'Samsung Galaxy Note', N'Android')
INSERT [dbo].[DeviceTypeTranslation] ([DeviceType], [TranslatedValue], [OSName]) VALUES (N'SGH-T959', N'Samsung Unknown', N'Android')
INSERT [dbo].[DeviceTypeTranslation] ([DeviceType], [TranslatedValue], [OSName]) VALUES (N'SGH-T959V', N'Samsung Unknown', N'Android')
INSERT [dbo].[DeviceTypeTranslation] ([DeviceType], [TranslatedValue], [OSName]) VALUES (N'SGH-T989D', N'Samsung Galaxy S II', N'Android')
INSERT [dbo].[DeviceTypeTranslation] ([DeviceType], [TranslatedValue], [OSName]) VALUES (N'SGH-T989', N'Samsung Galaxy S II', N'Android')
INSERT [dbo].[DeviceTypeTranslation] ([DeviceType], [TranslatedValue], [OSName]) VALUES (N'SGH-T999L', N'Samsung Galaxy S III', N'Android')
INSERT [dbo].[DeviceTypeTranslation] ([DeviceType], [TranslatedValue], [OSName]) VALUES (N'SGH-T999', N'Samsung Galaxy S III', N'Android')
INSERT [dbo].[DeviceTypeTranslation] ([DeviceType], [TranslatedValue], [OSName]) VALUES (N'SGH-T999V', N'Samsung Galaxy S III', N'Android')
INSERT [dbo].[DeviceTypeTranslation] ([DeviceType], [TranslatedValue], [OSName]) VALUES (N'SHV-E210S', N'Samsung Galaxy S III', N'Android')
INSERT [dbo].[DeviceTypeTranslation] ([DeviceType], [TranslatedValue], [OSName]) VALUES (N'SM-T', N'Samsung Galaxy Tab', N'Android')
INSERT [dbo].[DeviceTypeTranslation] ([DeviceType], [TranslatedValue], [OSName]) VALUES (N'SPH-D600', N'Samsung Unknown', N'Android')
INSERT [dbo].[DeviceTypeTranslation] ([DeviceType], [TranslatedValue], [OSName]) VALUES (N'SPH-D700', N'Samsung Unknown', N'Android')
INSERT [dbo].[DeviceTypeTranslation] ([DeviceType], [TranslatedValue], [OSName]) VALUES (N'SPH-D710BST', N'Samsung Galaxy S II', N'Android')
INSERT [dbo].[DeviceTypeTranslation] ([DeviceType], [TranslatedValue], [OSName]) VALUES (N'SPH-D710', N'Samsung Unknown', N'Android')
INSERT [dbo].[DeviceTypeTranslation] ([DeviceType], [TranslatedValue], [OSName]) VALUES (N'SPH-D710VMUB', N'Samsung Galaxy S II', N'Android')
INSERT [dbo].[DeviceTypeTranslation] ([DeviceType], [TranslatedValue], [OSName]) VALUES (N'SPH-L300', N'Samsung Unknown', N'Android')
INSERT [dbo].[DeviceTypeTranslation] ([DeviceType], [TranslatedValue], [OSName]) VALUES (N'SPH-L710', N'Samsung Galaxy S III', N'Android')
INSERT [dbo].[DeviceTypeTranslation] ([DeviceType], [TranslatedValue], [OSName]) VALUES (N'SPH-L720', N'Samsung Galaxy S4', N'Android')
INSERT [dbo].[DeviceTypeTranslation] ([DeviceType], [TranslatedValue], [OSName]) VALUES (N'SPH-L900', N'Samsung Galaxy Note', N'Android')
INSERT [dbo].[DeviceTypeTranslation] ([DeviceType], [TranslatedValue], [OSName]) VALUES (N'SPH-M820-BST', N'Samsung Unknown', N'Android')
INSERT [dbo].[DeviceTypeTranslation] ([DeviceType], [TranslatedValue], [OSName]) VALUES (N'SPH-M830', N'Samsung Unknown', N'Android')
INSERT [dbo].[DeviceTypeTranslation] ([DeviceType], [TranslatedValue], [OSName]) VALUES (N'SPH-M930BST', N'Samsung Unknown', N'Android')
INSERT [dbo].[DeviceTypeTranslation] ([DeviceType], [TranslatedValue], [OSName]) VALUES (N'ST25i', N'Sony', N'Android')
INSERT [dbo].[DeviceTypeTranslation] ([DeviceType], [TranslatedValue], [OSName]) VALUES (N'T-Mobile_G2', N'T-Mobile G2', N'Android')
INSERT [dbo].[DeviceTypeTranslation] ([DeviceType], [TranslatedValue], [OSName]) VALUES (N'T-Mobile_myTouch_Q', N'T-Mobile MyTouch Q', N'Android')
INSERT [dbo].[DeviceTypeTranslation] ([DeviceType], [TranslatedValue], [OSName]) VALUES (N'Transformer_Prime_TF201', N'Asus', N'Android')
INSERT [dbo].[DeviceTypeTranslation] ([DeviceType], [TranslatedValue], [OSName]) VALUES (N'Transformer_TF101', N'Asus', N'Android')
INSERT [dbo].[DeviceTypeTranslation] ([DeviceType], [TranslatedValue], [OSName]) VALUES (N'VM670', N'LG Optimus V', N'Android')
INSERT [dbo].[DeviceTypeTranslation] ([DeviceType], [TranslatedValue], [OSName]) VALUES (N'VS840_4G', N'LG Lucid', N'Android')
INSERT [dbo].[DeviceTypeTranslation] ([DeviceType], [TranslatedValue], [OSName]) VALUES (N'VS910_4G', N'LG Revolution', N'Android')
INSERT [dbo].[DeviceTypeTranslation] ([DeviceType], [TranslatedValue], [OSName]) VALUES (N'VS920_4G', N'LG Spectrum', N'Android')
INSERT [dbo].[DeviceTypeTranslation] ([DeviceType], [TranslatedValue], [OSName]) VALUES (N'VS980_4G', N'LG G2', N'Android')
INSERT [dbo].[DeviceTypeTranslation] ([DeviceType], [TranslatedValue], [OSName]) VALUES (N'Xoom', N'Motorola Xoom', N'Android')
INSERT [dbo].[DeviceTypeTranslation] ([DeviceType], [TranslatedValue], [OSName]) VALUES (N'XT1030', N'Motorola Droid', N'Android')
INSERT [dbo].[DeviceTypeTranslation] ([DeviceType], [TranslatedValue], [OSName]) VALUES (N'XT1032', N'Motorola Moto', N'Android')
INSERT [dbo].[DeviceTypeTranslation] ([DeviceType], [TranslatedValue], [OSName]) VALUES (N'XT1058', N'Motorola Moto', N'Android')
GO
print 'Processed 200 total records'
INSERT [dbo].[DeviceTypeTranslation] ([DeviceType], [TranslatedValue], [OSName]) VALUES (N'XT1060', N'Motorola Moto', N'Android')
INSERT [dbo].[DeviceTypeTranslation] ([DeviceType], [TranslatedValue], [OSName]) VALUES (N'XT1080', N'Motorola Droid', N'Android')
INSERT [dbo].[DeviceTypeTranslation] ([DeviceType], [TranslatedValue], [OSName]) VALUES (N'XT897', N'Motorola Photo', N'Android')
INSERT [dbo].[DeviceTypeTranslation] ([DeviceType], [TranslatedValue], [OSName]) VALUES (N'XT907', N'Motorola Droid', N'Android')
INSERT [dbo].[DeviceTypeTranslation] ([DeviceType], [TranslatedValue], [OSName]) VALUES (N'SM-G900', N'Samsung Galaxy S5', N'Android')
INSERT [dbo].[DeviceTypeTranslation] ([DeviceType], [TranslatedValue], [OSName]) VALUES (N'LGL86C', N'LG Optimus', N'Android')

INSERT [dbo].[DeviceTypeTranslation] ([DeviceType], [TranslatedValue], [OSName]) VALUES (N'Apple-iPhone', N'iPhone', N'Apple')
INSERT [dbo].[DeviceTypeTranslation] ([DeviceType], [TranslatedValue], [OSName]) VALUES (N'Apple-iPhone1C2', N'iPhone 3G', N'Apple')
INSERT [dbo].[DeviceTypeTranslation] ([DeviceType], [TranslatedValue], [OSName]) VALUES (N'Apple-iPhone2C1', N'iPhone 3GS', N'Apple')
INSERT [dbo].[DeviceTypeTranslation] ([DeviceType], [TranslatedValue], [OSName]) VALUES (N'Apple-iPhone3C1', N'iPhone 4', N'Apple')
INSERT [dbo].[DeviceTypeTranslation] ([DeviceType], [TranslatedValue], [OSName]) VALUES (N'Apple-iPhone3C2', N'iPhone 4', N'Apple')
INSERT [dbo].[DeviceTypeTranslation] ([DeviceType], [TranslatedValue], [OSName]) VALUES (N'Apple-iPhone3C3', N'iPhone 4', N'Apple')
INSERT [dbo].[DeviceTypeTranslation] ([DeviceType], [TranslatedValue], [OSName]) VALUES (N'Apple-iPhone4C1', N'iPhone 4S', N'Apple')
INSERT [dbo].[DeviceTypeTranslation] ([DeviceType], [TranslatedValue], [OSName]) VALUES (N'Apple-iPhone5C1', N'iPhone 5', N'Apple')
INSERT [dbo].[DeviceTypeTranslation] ([DeviceType], [TranslatedValue], [OSName]) VALUES (N'Apple-iPhone5C2', N'iPhone 5', N'Apple')
INSERT [dbo].[DeviceTypeTranslation] ([DeviceType], [TranslatedValue], [OSName]) VALUES (N'Apple-iPhone5C3', N'iPhone 5C', N'Apple')
INSERT [dbo].[DeviceTypeTranslation] ([DeviceType], [TranslatedValue], [OSName]) VALUES (N'Apple-iPhone5C4', N'iPhone 5C', N'Apple')
INSERT [dbo].[DeviceTypeTranslation] ([DeviceType], [TranslatedValue], [OSName]) VALUES (N'Apple-iPhone6C1', N'iPhone 5S', N'Apple')
INSERT [dbo].[DeviceTypeTranslation] ([DeviceType], [TranslatedValue], [OSName]) VALUES (N'Apple-iPhone6C2', N'iPhone 5S', N'Apple')
INSERT [dbo].[DeviceTypeTranslation] ([DeviceType], [TranslatedValue], [OSName]) VALUES (N'Apple-iPhone7C1', N'iPhone 6+', N'Apple')
INSERT [dbo].[DeviceTypeTranslation] ([DeviceType], [TranslatedValue], [OSName]) VALUES (N'Apple-iPhone7C2', N'iPhone 6', N'Apple')
INSERT [dbo].[DeviceTypeTranslation] ([DeviceType], [TranslatedValue], [OSName]) VALUES (N'Apple-iPad5C4', N'iPad Air 2', N'Apple')
INSERT [dbo].[DeviceTypeTranslation] ([DeviceType], [TranslatedValue], [OSName]) VALUES (N'Apple-iPad', N'iPad', N'Apple')
INSERT [dbo].[DeviceTypeTranslation] ([DeviceType], [TranslatedValue], [OSName]) VALUES (N'Apple-iPad1C1', N'iPad', N'Apple')
INSERT [dbo].[DeviceTypeTranslation] ([DeviceType], [TranslatedValue], [OSName]) VALUES (N'Apple-iPad2C1', N'iPad 2', N'Apple')
INSERT [dbo].[DeviceTypeTranslation] ([DeviceType], [TranslatedValue], [OSName]) VALUES (N'Apple-iPad2C2', N'iPad 2', N'Apple')
INSERT [dbo].[DeviceTypeTranslation] ([DeviceType], [TranslatedValue], [OSName]) VALUES (N'Apple-iPad2C3', N'iPad 2', N'Apple')
INSERT [dbo].[DeviceTypeTranslation] ([DeviceType], [TranslatedValue], [OSName]) VALUES (N'Apple-iPad2C4', N'iPad Mini', N'Apple')
INSERT [dbo].[DeviceTypeTranslation] ([DeviceType], [TranslatedValue], [OSName]) VALUES (N'Apple-iPad2C5', N'iPad Mini', N'Apple')
INSERT [dbo].[DeviceTypeTranslation] ([DeviceType], [TranslatedValue], [OSName]) VALUES (N'Apple-iPad3C1', N'iPad 3', N'Apple')
INSERT [dbo].[DeviceTypeTranslation] ([DeviceType], [TranslatedValue], [OSName]) VALUES (N'Apple-iPad3C2', N'iPad 3', N'Apple')
INSERT [dbo].[DeviceTypeTranslation] ([DeviceType], [TranslatedValue], [OSName]) VALUES (N'Apple-iPad3C3', N'iPad 3', N'Apple')
INSERT [dbo].[DeviceTypeTranslation] ([DeviceType], [TranslatedValue], [OSName]) VALUES (N'Apple-iPad3C4', N'iPad 4', N'Apple')
INSERT [dbo].[DeviceTypeTranslation] ([DeviceType], [TranslatedValue], [OSName]) VALUES (N'Apple-iPad3C5', N'iPad 4', N'Apple')
INSERT [dbo].[DeviceTypeTranslation] ([DeviceType], [TranslatedValue], [OSName]) VALUES (N'Apple-iPad3C6', N'iPad 4', N'Apple')
INSERT [dbo].[DeviceTypeTranslation] ([DeviceType], [TranslatedValue], [OSName]) VALUES (N'Apple-iPad4C1', N'iPad Air', N'Apple')
INSERT [dbo].[DeviceTypeTranslation] ([DeviceType], [TranslatedValue], [OSName]) VALUES (N'Apple-iPad4C2', N'iPad Air', N'Apple')
INSERT [dbo].[DeviceTypeTranslation] ([DeviceType], [TranslatedValue], [OSName]) VALUES (N'Apple-iPad4C3', N'iPad Air', N'Apple')
INSERT [dbo].[DeviceTypeTranslation] ([DeviceType], [TranslatedValue], [OSName]) VALUES (N'Apple-iPad4C4', N'iPad Mini(Retina)', N'Apple')
INSERT [dbo].[DeviceTypeTranslation] ([DeviceType], [TranslatedValue], [OSName]) VALUES (N'Apple-iPad4C5', N'iPad Mini(Retina)', N'Apple')
INSERT [dbo].[DeviceTypeTranslation] ([DeviceType], [TranslatedValue], [OSName]) VALUES (N'Apple-iPod', N'iPod Touch', N'Apple')
INSERT [dbo].[DeviceTypeTranslation] ([DeviceType], [TranslatedValue], [OSName]) VALUES (N'Apple-iPod2C1', N'iPod Touch', N'Apple')
INSERT [dbo].[DeviceTypeTranslation] ([DeviceType], [TranslatedValue], [OSName]) VALUES (N'Apple-iPod3C1', N'iPod Touch', N'Apple')
INSERT [dbo].[DeviceTypeTranslation] ([DeviceType], [TranslatedValue], [OSName]) VALUES (N'Apple-iPod4C1', N'iPod Touch', N'Apple')
INSERT [dbo].[DeviceTypeTranslation] ([DeviceType], [TranslatedValue], [OSName]) VALUES (N'Apple-iPod5C1', N'iPod Touch', N'Apple')
INSERT [dbo].[DeviceTypeTranslation] ([DeviceType], [TranslatedValue], [OSName]) VALUES (N'Apple-iPad2C6', N'iPad 2', N'Apple')
INSERT [dbo].[DeviceTypeTranslation] ([DeviceType], [TranslatedValue], [OSName]) VALUES (N'Apple-iPad2C7', N'iPad 2', N'Apple')
INSERT [dbo].[DeviceTypeTranslation] ([DeviceType], [TranslatedValue], [OSName]) VALUES (N'GT-I9228', N'Samsung Galaxy Note', N'Android')
INSERT [dbo].[DeviceTypeTranslation] ([DeviceType], [TranslatedValue], [OSName]) VALUES (N'SHV-E250', N'Samsung Galaxy Note', N'Android')
INSERT [dbo].[DeviceTypeTranslation] ([DeviceType], [TranslatedValue], [OSName]) VALUES (N'SM-G3508', N'Samsung Unknown', N'Android')
INSERT [dbo].[DeviceTypeTranslation] ([DeviceType], [TranslatedValue], [OSName]) VALUES (N'SGH-I497', N'Samsung Galaxy Tab', N'Android')
INSERT [dbo].[DeviceTypeTranslation] ([DeviceType], [TranslatedValue], [OSName]) VALUES (N'GT-I8268', N'Samsung Galaxy S III', N'Android')
INSERT [dbo].[DeviceTypeTranslation] ([DeviceType], [TranslatedValue], [OSName]) VALUES (N'SM-G870A', N'Samsung Galaxy S5', N'Android')
INSERT [dbo].[DeviceTypeTranslation] ([DeviceType], [TranslatedValue], [OSName]) VALUES (N'SCH-N719', N'Samsung Galaxy Note', N'Android')
INSERT [dbo].[DeviceTypeTranslation] ([DeviceType], [TranslatedValue], [OSName]) VALUES (N'GT-I9308', N'Samsung Galaxy S III', N'Android')
INSERT [dbo].[DeviceTypeTranslation] ([DeviceType], [TranslatedValue], [OSName]) VALUES (N'SCH-R530X', N'Samsung Galaxy S III', N'Android')
INSERT [dbo].[DeviceTypeTranslation] ([DeviceType], [TranslatedValue], [OSName]) VALUES (N'SHW-M440S', N'Samsung Galaxy S III', N'Android')
INSERT [dbo].[DeviceTypeTranslation] ([DeviceType], [TranslatedValue], [OSName]) VALUES (N'SGH-I337', N'Samsung Galaxy S4', N'Android')
INSERT [dbo].[DeviceTypeTranslation] ([DeviceType], [TranslatedValue], [OSName]) VALUES (N'SCH-I435', N'Samsung Galaxy S4', N'Android')
INSERT [dbo].[DeviceTypeTranslation] ([DeviceType], [TranslatedValue], [OSName]) VALUES (N'SHV-E300S', N'Samsung Galaxy S4', N'Android')
INSERT [dbo].[DeviceTypeTranslation] ([DeviceType], [TranslatedValue], [OSName]) VALUES (N'SM-P600', N'Samsung Galaxy Tab', N'Android')
INSERT [dbo].[DeviceTypeTranslation] ([DeviceType], [TranslatedValue], [OSName]) VALUES (N'GT-P6800', N'Samsung Galaxy Tab', N'Android')
INSERT [dbo].[DeviceTypeTranslation] ([DeviceType], [TranslatedValue], [OSName]) VALUES (N'SGH-T859', N'Samsung Galaxy Tab', N'Android')
INSERT [dbo].[DeviceTypeTranslation] ([DeviceType], [TranslatedValue], [OSName]) VALUES (N'SCH-I925', N'Samsung Galaxy Tab', N'Android')
INSERT [dbo].[DeviceTypeTranslation] ([DeviceType], [TranslatedValue], [OSName]) VALUES (N'GT-I9220', N'Samsung Galaxy Note', N'Android')
INSERT [dbo].[DeviceTypeTranslation] ([DeviceType], [TranslatedValue], [OSName]) VALUES (N'SGH-I467M', N'Samsung Galaxy Note', N'Android')
INSERT [dbo].[DeviceTypeTranslation] ([DeviceType], [TranslatedValue], [OSName]) VALUES (N'SHV-E160S', N'Samsung Galaxy Note', N'Android')
INSERT [dbo].[DeviceTypeTranslation] ([DeviceType], [TranslatedValue], [OSName]) VALUES (N'SHW-M480W', N'Samsung Galaxy Note', N'Android')
INSERT [dbo].[DeviceTypeTranslation] ([DeviceType], [TranslatedValue], [OSName]) VALUES (N'SM-G3508J', N'Samsung Galaxy Note', N'Android')
INSERT [dbo].[DeviceTypeTranslation] ([DeviceType], [TranslatedValue], [OSName]) VALUES (N'GT-S7582', N'Samsung Galaxy Note', N'Android')
INSERT [dbo].[DeviceTypeTranslation] ([DeviceType], [TranslatedValue], [OSName]) VALUES (N'GT-I9268', N'Samsung Galaxy Note', N'Android')
INSERT [dbo].[DeviceTypeTranslation] ([DeviceType], [TranslatedValue], [OSName]) VALUES (N'GT-I8552', N'Samsung Galaxy Note', N'Android')
INSERT [dbo].[DeviceTypeTranslation] ([DeviceType], [TranslatedValue], [OSName]) VALUES (N'GT-I8730', N'Samsung Galaxy Note', N'Android')
INSERT [dbo].[DeviceTypeTranslation] ([DeviceType], [TranslatedValue], [OSName]) VALUES (N'GT-I9050', N'Samsung Galaxy Note', N'Android')
INSERT [dbo].[DeviceTypeTranslation] ([DeviceType], [TranslatedValue], [OSName]) VALUES (N'GT-I9168', N'Samsung Galaxy Note', N'Android')
INSERT [dbo].[DeviceTypeTranslation] ([DeviceType], [TranslatedValue], [OSName]) VALUES (N'GT-I9168I', N'Samsung Galaxy Note', N'Android')
INSERT [dbo].[DeviceTypeTranslation] ([DeviceType], [TranslatedValue], [OSName]) VALUES (N'SGH-T499', N'Samsung Galaxy Note', N'Android')
INSERT [dbo].[DeviceTypeTranslation] ([DeviceType], [TranslatedValue], [OSName]) VALUES (N'SCH-I779', N'Samsung Galaxy Note', N'Android')
INSERT [dbo].[DeviceTypeTranslation] ([DeviceType], [TranslatedValue], [OSName]) VALUES (N'SHV-E270S', N'Samsung Galaxy Note', N'Android')
INSERT [dbo].[DeviceTypeTranslation] ([DeviceType], [TranslatedValue], [OSName]) VALUES (N'SHV-E500S', N'Samsung Galaxy Note', N'Android')
INSERT [dbo].[DeviceTypeTranslation] ([DeviceType], [TranslatedValue], [OSName]) VALUES (N'Xiaomi', N'Xiaomi', N'Android')
INSERT [dbo].[DeviceTypeTranslation] ([DeviceType], [TranslatedValue], [OSName]) VALUES (N'Sony', N'Sony', N'Android')
INSERT [dbo].[DeviceTypeTranslation] ([DeviceType], [TranslatedValue], [OSName]) VALUES (N'Asus', N'Asus', N'Android')
INSERT [dbo].[DeviceTypeTranslation] ([DeviceType], [TranslatedValue], [OSName]) VALUES (N'Lumia', N'Nokia Lumia', N'Android')
INSERT [dbo].[DeviceTypeTranslation] ([DeviceType], [TranslatedValue], [OSName]) VALUES (N'Lenovo', N'Lenovo', N'Android')
INSERT [dbo].[DeviceTypeTranslation] ([DeviceType], [TranslatedValue], [OSName]) VALUES (N'LGE', N'LG Unknown', N'Android')
INSERT [dbo].[DeviceTypeTranslation] ([DeviceType], [TranslatedValue], [OSName]) VALUES (N'ZTE', N'ZTE', N'Android')
INSERT [dbo].[DeviceTypeTranslation] ([DeviceType], [TranslatedValue], [OSName]) VALUES (N'Unknown', N'Unknown', N'Android')
INSERT [dbo].[DeviceTypeTranslation] ([DeviceType], [TranslatedValue], [OSName]) VALUES (N'Toshiba', N'Toshiba', N'Android')
INSERT [dbo].[DeviceTypeTranslation] ([DeviceType], [TranslatedValue], [OSName]) VALUES (N'HUAWEI', N'HUAWEI', N'Android')
INSERT [dbo].[DeviceTypeTranslation] ([DeviceType], [TranslatedValue], [OSName]) VALUES (N'Philips', N'Philips', N'Android')
INSERT [dbo].[DeviceTypeTranslation] ([DeviceType], [TranslatedValue], [OSName]) VALUES (N'Acer', N'Acer', N'Android')



go

DELETE FROM dbo.OSTypeTranslation
GO
--OS TYPEINSERT [dbo].[OSTypeTranslation] ([OSType], [TranslatedValue], [OSName]) VALUES (N'1101.466', N'iOS 7.0.1', N'Apple')
INSERT [dbo].[OSTypeTranslation] ([OSType], [TranslatedValue], [OSName]) VALUES (N'1104.257', N'iOS 7.2', N'Apple')
INSERT [dbo].[OSTypeTranslation] ([OSType], [TranslatedValue], [OSName]) VALUES (N'1001.551', N'iOS 6.0', N'Apple')
INSERT [dbo].[OSTypeTranslation] ([OSType], [TranslatedValue], [OSName]) VALUES (N'1001.550', N'iOS 6.0', N'Apple')
INSERT [dbo].[OSTypeTranslation] ([OSType], [TranslatedValue], [OSName]) VALUES (N'Cupcake', N'Android 1.5', N'Android')
INSERT [dbo].[OSTypeTranslation] ([OSType], [TranslatedValue], [OSName]) VALUES (N'Doughnut', N'Android 1.6', N'Android')
INSERT [dbo].[OSTypeTranslation] ([OSType], [TranslatedValue], [OSName]) VALUES (N'Eclair', N'Android (2.0-2.1)', N'Android')
INSERT [dbo].[OSTypeTranslation] ([OSType], [TranslatedValue], [OSName]) VALUES (N'Froyo', N'Android 2.2', N'Android')
INSERT [dbo].[OSTypeTranslation] ([OSType], [TranslatedValue], [OSName]) VALUES (N'Gingerbread', N'Android 2.3', N'Android')
INSERT [dbo].[OSTypeTranslation] ([OSType], [TranslatedValue], [OSName]) VALUES (N'Honeycomb', N'Android (3.0-3.2)', N'Android')
INSERT [dbo].[OSTypeTranslation] ([OSType], [TranslatedValue], [OSName]) VALUES (N'Ice Cream Sandwich', N'Android 4.0', N'Android')
INSERT [dbo].[OSTypeTranslation] ([OSType], [TranslatedValue], [OSName]) VALUES (N'Jelly Bean', N'Android (4.1-4.3)', N'Android')
INSERT [dbo].[OSTypeTranslation] ([OSType], [TranslatedValue], [OSName]) VALUES (N'KitKat', N'Android 4.4', N'Android')
INSERT [dbo].[OSTypeTranslation] ([OSType], [TranslatedValue], [OSName]) VALUES (N'508.11', N'iOS 2.2.1', N'Apple')
INSERT [dbo].[OSTypeTranslation] ([OSType], [TranslatedValue], [OSName]) VALUES (N'701.341', N'iOS 3.0', N'Apple')
INSERT [dbo].[OSTypeTranslation] ([OSType], [TranslatedValue], [OSName]) VALUES (N'701.400', N'iOS 3.0.1', N'Apple')
INSERT [dbo].[OSTypeTranslation] ([OSType], [TranslatedValue], [OSName]) VALUES (N'702.367', N'iOS 3.2', N'Apple')
INSERT [dbo].[OSTypeTranslation] ([OSType], [TranslatedValue], [OSName]) VALUES (N'702.405', N'iOS 3.2.1', N'Apple')
INSERT [dbo].[OSTypeTranslation] ([OSType], [TranslatedValue], [OSName]) VALUES (N'702.500', N'iOS 3.2.2', N'Apple')
INSERT [dbo].[OSTypeTranslation] ([OSType], [TranslatedValue], [OSName]) VALUES (N'703.144', N'iOS 3.1', N'Apple')
INSERT [dbo].[OSTypeTranslation] ([OSType], [TranslatedValue], [OSName]) VALUES (N'704.11', N'iOS 3.1.2', N'Apple')
INSERT [dbo].[OSTypeTranslation] ([OSType], [TranslatedValue], [OSName]) VALUES (N'705.18', N'iOS 3.1.3', N'Apple')
INSERT [dbo].[OSTypeTranslation] ([OSType], [TranslatedValue], [OSName]) VALUES (N'801.293', N'iOS 4.0', N'Apple')
INSERT [dbo].[OSTypeTranslation] ([OSType], [TranslatedValue], [OSName]) VALUES (N'801.306', N'iOS 4.0.1', N'Apple')
INSERT [dbo].[OSTypeTranslation] ([OSType], [TranslatedValue], [OSName]) VALUES (N'801.400', N'iOS 4.0.2', N'Apple')
INSERT [dbo].[OSTypeTranslation] ([OSType], [TranslatedValue], [OSName]) VALUES (N'802.117', N'iOS 4.1', N'Apple')
INSERT [dbo].[OSTypeTranslation] ([OSType], [TranslatedValue], [OSName]) VALUES (N'802.118', N'iOS 4.1', N'Apple')
INSERT [dbo].[OSTypeTranslation] ([OSType], [TranslatedValue], [OSName]) VALUES (N'803.148', N'iOS 4.2.1', N'Apple')
INSERT [dbo].[OSTypeTranslation] ([OSType], [TranslatedValue], [OSName]) VALUES (N'803.14800001', N'iOS 4.2.1', N'Apple')
INSERT [dbo].[OSTypeTranslation] ([OSType], [TranslatedValue], [OSName]) VALUES (N'805.128', N'iOS 4.2.5', N'Apple')
INSERT [dbo].[OSTypeTranslation] ([OSType], [TranslatedValue], [OSName]) VALUES (N'805.200', N'iOS 4.2.6', N'Apple')
INSERT [dbo].[OSTypeTranslation] ([OSType], [TranslatedValue], [OSName]) VALUES (N'805.303', N'iOS 4.2.7', N'Apple')
INSERT [dbo].[OSTypeTranslation] ([OSType], [TranslatedValue], [OSName]) VALUES (N'805.401', N'iOS 4.2.8', N'Apple')
INSERT [dbo].[OSTypeTranslation] ([OSType], [TranslatedValue], [OSName]) VALUES (N'805.501', N'iOS 4.2.9', N'Apple')
INSERT [dbo].[OSTypeTranslation] ([OSType], [TranslatedValue], [OSName]) VALUES (N'805.600', N'iOS 4.2.10', N'Apple')
INSERT [dbo].[OSTypeTranslation] ([OSType], [TranslatedValue], [OSName]) VALUES (N'806.190', N'iOS 4.3', N'Apple')
INSERT [dbo].[OSTypeTranslation] ([OSType], [TranslatedValue], [OSName]) VALUES (N'806.191', N'iOS 4.3', N'Apple')
INSERT [dbo].[OSTypeTranslation] ([OSType], [TranslatedValue], [OSName]) VALUES (N'807.4', N'iOS 4.3.1', N'Apple')
INSERT [dbo].[OSTypeTranslation] ([OSType], [TranslatedValue], [OSName]) VALUES (N'808.7', N'iOS 4.3.2', N'Apple')
INSERT [dbo].[OSTypeTranslation] ([OSType], [TranslatedValue], [OSName]) VALUES (N'808.8', N'iOS 4.3.2', N'Apple')
INSERT [dbo].[OSTypeTranslation] ([OSType], [TranslatedValue], [OSName]) VALUES (N'810.2', N'iOS 4.3.3', N'Apple')
INSERT [dbo].[OSTypeTranslation] ([OSType], [TranslatedValue], [OSName]) VALUES (N'810.3', N'iOS 4.3.3', N'Apple')
INSERT [dbo].[OSTypeTranslation] ([OSType], [TranslatedValue], [OSName]) VALUES (N'811.2', N'iOS 4.3.4', N'Apple')
INSERT [dbo].[OSTypeTranslation] ([OSType], [TranslatedValue], [OSName]) VALUES (N'812.1', N'iOS 4.3.5', N'Apple')
INSERT [dbo].[OSTypeTranslation] ([OSType], [TranslatedValue], [OSName]) VALUES (N'901.334', N'iOS 5.0', N'Apple')
INSERT [dbo].[OSTypeTranslation] ([OSType], [TranslatedValue], [OSName]) VALUES (N'901.400', N'iOS 5.0.1', N'Apple')
INSERT [dbo].[OSTypeTranslation] ([OSType], [TranslatedValue], [OSName]) VALUES (N'901.401', N'iOS 5.0.1', N'Apple')
INSERT [dbo].[OSTypeTranslation] ([OSType], [TranslatedValue], [OSName]) VALUES (N'901.402', N'iOS 5.0.1', N'Apple')
INSERT [dbo].[OSTypeTranslation] ([OSType], [TranslatedValue], [OSName]) VALUES (N'901.403', N'iOS 5.0.1', N'Apple')
INSERT [dbo].[OSTypeTranslation] ([OSType], [TranslatedValue], [OSName]) VALUES (N'901.404', N'iOS 5.0.1', N'Apple')
INSERT [dbo].[OSTypeTranslation] ([OSType], [TranslatedValue], [OSName]) VALUES (N'901.405', N'iOS 5.0.1', N'Apple')
INSERT [dbo].[OSTypeTranslation] ([OSType], [TranslatedValue], [OSName]) VALUES (N'901.406', N'iOS 5.0.1', N'Apple')
INSERT [dbo].[OSTypeTranslation] ([OSType], [TranslatedValue], [OSName]) VALUES (N'901.407', N'iOS 5.0.1', N'Apple')
INSERT [dbo].[OSTypeTranslation] ([OSType], [TranslatedValue], [OSName]) VALUES (N'901.408', N'iOS 5.0.1', N'Apple')
INSERT [dbo].[OSTypeTranslation] ([OSType], [TranslatedValue], [OSName]) VALUES (N'901.409', N'iOS 5.0.1', N'Apple')
INSERT [dbo].[OSTypeTranslation] ([OSType], [TranslatedValue], [OSName]) VALUES (N'902.170', N'iOS 5.1', N'Apple')
INSERT [dbo].[OSTypeTranslation] ([OSType], [TranslatedValue], [OSName]) VALUES (N'902.171', N'iOS 5.1', N'Apple')
INSERT [dbo].[OSTypeTranslation] ([OSType], [TranslatedValue], [OSName]) VALUES (N'902.172', N'iOS 5.1', N'Apple')
INSERT [dbo].[OSTypeTranslation] ([OSType], [TranslatedValue], [OSName]) VALUES (N'902.173', N'iOS 5.1', N'Apple')
INSERT [dbo].[OSTypeTranslation] ([OSType], [TranslatedValue], [OSName]) VALUES (N'902.174', N'iOS 5.1', N'Apple')
INSERT [dbo].[OSTypeTranslation] ([OSType], [TranslatedValue], [OSName]) VALUES (N'902.175', N'iOS 5.1', N'Apple')
INSERT [dbo].[OSTypeTranslation] ([OSType], [TranslatedValue], [OSName]) VALUES (N'902.176', N'iOS 5.1', N'Apple')
INSERT [dbo].[OSTypeTranslation] ([OSType], [TranslatedValue], [OSName]) VALUES (N'902.177', N'iOS 5.1', N'Apple')
INSERT [dbo].[OSTypeTranslation] ([OSType], [TranslatedValue], [OSName]) VALUES (N'902.178', N'iOS 5.1', N'Apple')
INSERT [dbo].[OSTypeTranslation] ([OSType], [TranslatedValue], [OSName]) VALUES (N'902.179', N'iOS 5.1', N'Apple')
INSERT [dbo].[OSTypeTranslation] ([OSType], [TranslatedValue], [OSName]) VALUES (N'902.206', N'iOS 5.1.1', N'Apple')
INSERT [dbo].[OSTypeTranslation] ([OSType], [TranslatedValue], [OSName]) VALUES (N'1001.400', N'iOS 6.0', N'Apple')
INSERT [dbo].[OSTypeTranslation] ([OSType], [TranslatedValue], [OSName]) VALUES (N'1001.401', N'iOS 6.0', N'Apple')
INSERT [dbo].[OSTypeTranslation] ([OSType], [TranslatedValue], [OSName]) VALUES (N'1001.402', N'iOS 6.0', N'Apple')
INSERT [dbo].[OSTypeTranslation] ([OSType], [TranslatedValue], [OSName]) VALUES (N'1001.403', N'iOS 6.0', N'Apple')
INSERT [dbo].[OSTypeTranslation] ([OSType], [TranslatedValue], [OSName]) VALUES (N'1001.404', N'iOS 6.0', N'Apple')
INSERT [dbo].[OSTypeTranslation] ([OSType], [TranslatedValue], [OSName]) VALUES (N'1001.405', N'iOS 6.0', N'Apple')
INSERT [dbo].[OSTypeTranslation] ([OSType], [TranslatedValue], [OSName]) VALUES (N'1001.406', N'iOS 6.0', N'Apple')
INSERT [dbo].[OSTypeTranslation] ([OSType], [TranslatedValue], [OSName]) VALUES (N'1001.407', N'iOS 6.0', N'Apple')
INSERT [dbo].[OSTypeTranslation] ([OSType], [TranslatedValue], [OSName]) VALUES (N'1001.408', N'iOS 6.0', N'Apple')
INSERT [dbo].[OSTypeTranslation] ([OSType], [TranslatedValue], [OSName]) VALUES (N'1001.409', N'iOS 6.0', N'Apple')
INSERT [dbo].[OSTypeTranslation] ([OSType], [TranslatedValue], [OSName]) VALUES (N'1001.520', N'iOS 6.0.1', N'Apple')
INSERT [dbo].[OSTypeTranslation] ([OSType], [TranslatedValue], [OSName]) VALUES (N'1001.529', N'iOS 6.0.1', N'Apple')
INSERT [dbo].[OSTypeTranslation] ([OSType], [TranslatedValue], [OSName]) VALUES (N'1001.521', N'iOS 6.0.1', N'Apple')
INSERT [dbo].[OSTypeTranslation] ([OSType], [TranslatedValue], [OSName]) VALUES (N'1001.522', N'iOS 6.0.1', N'Apple')
INSERT [dbo].[OSTypeTranslation] ([OSType], [TranslatedValue], [OSName]) VALUES (N'1001.523', N'iOS 6.0.1', N'Apple')
INSERT [dbo].[OSTypeTranslation] ([OSType], [TranslatedValue], [OSName]) VALUES (N'1001.524', N'iOS 6.0.1', N'Apple')
INSERT [dbo].[OSTypeTranslation] ([OSType], [TranslatedValue], [OSName]) VALUES (N'1001.525', N'iOS 6.0.1', N'Apple')
INSERT [dbo].[OSTypeTranslation] ([OSType], [TranslatedValue], [OSName]) VALUES (N'1001.526', N'iOS 6.0.1', N'Apple')
INSERT [dbo].[OSTypeTranslation] ([OSType], [TranslatedValue], [OSName]) VALUES (N'1001.527', N'iOS 6.0.1', N'Apple')
INSERT [dbo].[OSTypeTranslation] ([OSType], [TranslatedValue], [OSName]) VALUES (N'1001.528', N'iOS 6.0.1', N'Apple')
INSERT [dbo].[OSTypeTranslation] ([OSType], [TranslatedValue], [OSName]) VALUES (N'1002.140', N'iOS 6.1', N'Apple')
INSERT [dbo].[OSTypeTranslation] ([OSType], [TranslatedValue], [OSName]) VALUES (N'1002.141', N'iOS 6.1', N'Apple')
INSERT [dbo].[OSTypeTranslation] ([OSType], [TranslatedValue], [OSName]) VALUES (N'1002.142', N'iOS 6.1', N'Apple')
INSERT [dbo].[OSTypeTranslation] ([OSType], [TranslatedValue], [OSName]) VALUES (N'1002.143', N'iOS 6.1', N'Apple')
INSERT [dbo].[OSTypeTranslation] ([OSType], [TranslatedValue], [OSName]) VALUES (N'1002.144', N'iOS 6.1', N'Apple')
INSERT [dbo].[OSTypeTranslation] ([OSType], [TranslatedValue], [OSName]) VALUES (N'1002.145', N'iOS 6.1', N'Apple')
INSERT [dbo].[OSTypeTranslation] ([OSType], [TranslatedValue], [OSName]) VALUES (N'1002.147', N'iOS 6.1', N'Apple')
INSERT [dbo].[OSTypeTranslation] ([OSType], [TranslatedValue], [OSName]) VALUES (N'1002.148', N'iOS 6.1', N'Apple')
INSERT [dbo].[OSTypeTranslation] ([OSType], [TranslatedValue], [OSName]) VALUES (N'1002.149', N'iOS 6.1', N'Apple')
INSERT [dbo].[OSTypeTranslation] ([OSType], [TranslatedValue], [OSName]) VALUES (N'1002.146', N'iOS 6.1.2', N'Apple')
INSERT [dbo].[OSTypeTranslation] ([OSType], [TranslatedValue], [OSName]) VALUES (N'1002.329', N'iOS 6.1.3', N'Apple')
INSERT [dbo].[OSTypeTranslation] ([OSType], [TranslatedValue], [OSName]) VALUES (N'1002.350', N'iOS 6.1.3', N'Apple')
INSERT [dbo].[OSTypeTranslation] ([OSType], [TranslatedValue], [OSName]) VALUES (N'1101.465', N'iOS 7.0', N'Apple')
INSERT [dbo].[OSTypeTranslation] ([OSType], [TranslatedValue], [OSName]) VALUES (N'1101.470', N'iOS 7.0.1', N'Apple')
INSERT [dbo].[OSTypeTranslation] ([OSType], [TranslatedValue], [OSName]) VALUES (N'1101.47000001', N'iOS 7.0.1', N'Apple')
GO
print 'Processed 100 total records'
INSERT [dbo].[OSTypeTranslation] ([OSType], [TranslatedValue], [OSName]) VALUES (N'1101.501', N'iOS 7.0.2', N'Apple')
INSERT [dbo].[OSTypeTranslation] ([OSType], [TranslatedValue], [OSName]) VALUES (N'1102.511', N'iOS 7.0.3 ', N'Apple')
INSERT [dbo].[OSTypeTranslation] ([OSType], [TranslatedValue], [OSName]) VALUES (N'1102.55400001', N'iOS 7.0.4', N'Apple')
INSERT [dbo].[OSTypeTranslation] ([OSType], [TranslatedValue], [OSName]) VALUES (N'1102.651', N'iOS 7.0.6', N'Apple')
INSERT [dbo].[OSTypeTranslation] ([OSType], [TranslatedValue], [OSName]) VALUES (N'1104.167', N'iOS 7.1', N'Apple')
INSERT [dbo].[OSTypeTranslation] ([OSType], [TranslatedValue], [OSName]) VALUES (N'1002.500', N'iOS 6.1.4', N'Apple')
INSERT [dbo].[OSTypeTranslation] ([OSType], [TranslatedValue], [OSName]) VALUES (N'1104.201', N'iOS 7.1.1', N'Apple')
INSERT [dbo].[OSTypeTranslation] ([OSType], [TranslatedValue], [OSName]) VALUES (N'1102.601', N'iOS 7.0.5', N'Apple')
INSERT [dbo].[OSTypeTranslation] ([OSType], [TranslatedValue], [OSName]) VALUES (N'1104.169', N'iOS 7.1', N'Apple')
INSERT [dbo].[OSTypeTranslation] ([OSType], [TranslatedValue], [OSName]) VALUES (N'1001.8500', N'iOS 6.0', N'Apple')
INSERT [dbo].[OSTypeTranslation] ([OSType], [TranslatedValue], [OSName]) VALUES (N'1001.8426', N'iOS 6.0', N'Apple')
INSERT [dbo].[OSTypeTranslation] ([OSType], [TranslatedValue], [OSName]) VALUES (N'1201.365', N'iOS 8.0', N'Apple')
INSERT [dbo].[OSTypeTranslation] ([OSType], [TranslatedValue], [OSName]) VALUES (N'1201.366', N'iOS 8.0.1', N'Apple')
INSERT [dbo].[OSTypeTranslation] ([OSType], [TranslatedValue], [OSName]) VALUES (N'1201.405', N'iOS 8.0.2', N'Apple')
INSERT [dbo].[OSTypeTranslation] ([OSType], [TranslatedValue], [OSName]) VALUES (N'1202.411', N'iOS 8.1', N'Apple')
INSERT [dbo].[OSTypeTranslation] ([OSType], [TranslatedValue], [OSName]) VALUES (N'1202.410', N'iOS 8.1', N'Apple')
INSERT [dbo].[OSTypeTranslation] ([OSType], [TranslatedValue], [OSName]) VALUES (N'1202.435', N'iOS 8.1.0', N'Apple')
INSERT [dbo].[OSTypeTranslation] ([OSType], [TranslatedValue], [OSName]) VALUES (N'1202.436', N'iOS 8.1.1', N'Apple')
INSERT [dbo].[OSTypeTranslation] ([OSType], [TranslatedValue], [OSName]) VALUES (N'1202.440', N'iOS 8.1.2', N'Apple')
INSERT [dbo].[OSTypeTranslation] ([OSType], [TranslatedValue], [OSName]) VALUES (N'1202.445', N'iOS 8.1.2', N'Apple')
INSERT [dbo].[OSTypeTranslation] ([OSType], [TranslatedValue], [OSName]) VALUES (N'1202.466', N'iOS 8.1.3', N'Apple')
INSERT [dbo].[OSTypeTranslation] ([OSType], [TranslatedValue], [OSName]) VALUES (N'1204.508', N'iOS 8.2', N'Apple')
INSERT [dbo].[OSTypeTranslation] ([OSType], [TranslatedValue], [OSName]) VALUES (N'1206.69', N'iOS 8.3', N'Apple')
INSERT [dbo].[OSTypeTranslation] ([OSType], [TranslatedValue], [OSName]) VALUES (N'1206.70', N'iOS 8.3', N'Apple')
INSERT [dbo].[OSTypeTranslation] ([OSType], [TranslatedValue], [OSName]) VALUES (N'1208.143', N'iOS 8.4', N'Apple')
INSERT [dbo].[OSTypeTranslation] ([OSType], [TranslatedValue], [OSName]) VALUES (N'1208.321', N'iOS 8.4.1', N'Apple')

GO

/* 4/17/2014 NS added for VSPLUS-573 */
USE [vitalsigns]
GO
DELETE FROM EventsMaster WHERE EventName='Painful Memory'
GO

/* 4/17/2014 NS added for VSPLUS-572 */
use vitalsigns
go
IF NOT EXISTS (select * from EventsMaster where EventName='Traveler Threads Warning' )
BEGIN	
	SET IDENTITY_INSERT [dbo].[EventsMaster] ON
	INSERT [dbo].[EventsMaster] ([ID], [EventName], [ServerTypeID]) VALUES (69, N'Traveler Threads Warning', 1)
	SET IDENTITY_INSERT [dbo].[EventsMaster] OFF
END
GO
IF NOT EXISTS (select * from EventsMaster where EventName='Traveler Insufficient Threads' )
BEGIN	
	SET IDENTITY_INSERT [dbo].[EventsMaster] ON
	INSERT [dbo].[EventsMaster] ([ID], [EventName], [ServerTypeID]) VALUES (70, N'Traveler Insufficient Threads', 1)
	SET IDENTITY_INSERT [dbo].[EventsMaster] OFF
END
GO

IF NOT EXISTS (select * from syscolumns where name= 'OS_Type_Min' and id = object_id('dbo.Traveler_Devices'))
BEGIN
ALTER TABLE [dbo].[Traveler_Devices] ADD OS_Type_Min [nvarchar](255) NULL
End
GO

--Mukund 25Apr14, VSPLUS-425: Menu items in dashboard should be database driven...

USE [vitalsigns]
GO

/****** Object:  Table [dbo].[Features]    Script Date: 04/25/2014 11:51:04 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Features]') AND type in (N'U'))
DROP TABLE [dbo].[Features]
GO

USE [vitalsigns]
GO

/****** Object:  Table [dbo].[Features]    Script Date: 04/25/2014 11:51:04 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

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
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[FeatureMenus]') AND type in (N'U'))
DROP TABLE [dbo].[FeatureMenus]
GO

USE [vitalsigns]
GO

/****** Object:  Table [dbo].[FeatureMenus]    Script Date: 04/25/2014 11:51:12 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[FeatureMenus](
	[FeatureID] [int] NULL,
	[MenuID] [int] NULL
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[SelectedFeatures]    Script Date: 04/25/2014 11:51:33 ******/
--IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SelectedFeatures]') AND type in (N'U'))
--DROP TABLE [dbo].[SelectedFeatures]
--GO


IF NOT EXISTS (select * from syscolumns where name= 'MenuArea' and id = object_id('dbo.MenuItems'))
BEGIN	
	ALTER TABLE dbo.MenuItems ADD MenuArea [nvarchar] (50) null
END
GO	
--VSPLUS-2474 DURGA
USE [vitalsigns]
GO

IF EXISTS (select * from syscolumns where name= 'SessionNames' and id = object_id('dbo.MenuItems'))
BEGIN	
	ALTER TABLE MenuItems ALTER COLUMN SessionNames nvarchar(MAX) null
END
GO
Delete from Features 
Go

/* 5/9/2014 NS added more features to the table; added inserts for SelectedFeatures */

/****** Object:  Table [dbo].[Features]    Script Date: 04/25/2014 15:05:39 ******/
SET IDENTITY_INSERT [dbo].[Features] ON
INSERT [dbo].[Features] ([ID], [Name]) VALUES (1, N'Domino')
INSERT [dbo].[Features] ([ID], [Name]) VALUES (2, N'Exchange')
INSERT [dbo].[Features] ([ID], [Name]) VALUES (3, N'BES')
INSERT [dbo].[Features] ([ID], [Name]) VALUES (4, N'NotesMail')
INSERT [dbo].[Features] ([ID], [Name]) VALUES (5, N'SharePoint')
INSERT [dbo].[Features] ([ID], [Name]) VALUES (6, N'Sametime')
INSERT [dbo].[Features] ([ID], [Name]) VALUES (8, N'URLs')
INSERT [dbo].[Features] ([ID], [Name]) VALUES (9, N'Mobile Users')
INSERT [dbo].[Features] ([ID], [Name]) VALUES (10, N'Network Devices')
INSERT [dbo].[Features] ([ID], [Name]) VALUES (11, N'Common Features')
INSERT [dbo].[Features] ([ID], [Name]) VALUES (12, N'Mail Services')
/* 8/4/2014 NS added */
INSERT [dbo].[Features] ([ID], [Name]) VALUES (13, N'Skype for Business')
/*29Sep14 Mukund added*/
INSERT [dbo].[Features] ([ID], [Name]) VALUES (14, N'Windows')
/*01Oct14 Mukund added*/
INSERT [dbo].[Features] ([ID], [Name]) VALUES (15, N'Cloud')
/*07Oct14 Mukund added*/
INSERT [dbo].[Features] ([ID], [Name]) VALUES (16, N'Active Directory')
/* 1/21/2015 NS added for VSPLUS-1378 */
/* 12/28/2015 NS commented out for VSPLUS-2045 */
/*
INSERT [dbo].[Features] ([ID], [Name]) VALUES (17, N'IBM Connections')
*/
--VSPLUS-2237 Durga
INSERT [dbo].[Features] ([ID], [Name]) VALUES (18, N'IBM WebSphere')
INSERT [dbo].[Features] ([ID], [Name]) VALUES (19, N'Office 365')
--3/11/2016 sowjanya Modified for VSPLUS-2650
INSERT [dbo].[Features] ([ID], [Name]) VALUES (20 , N'IBM Connections')


SET IDENTITY_INSERT [dbo].[Features] OFF


IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[MobileAppUsers]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[MobileAppUsers](
	[DeviceId] Varchar(255) NULL,
	[OSType] Varchar(100) NULL,
	[DeviceType] Varchar(100) NULL
) ON [PRIMARY]
END
GO

--5/5/2014: CY(VS-330) Added a column to URLs table
GO
IF NOT EXISTS (select * from syscolumns where name= 'AlertStringFound' and id = object_id('dbo.URLs'))
BEGIN	
	ALTER TABLE URLs 
	ADD AlertStringFound NVARCHAR(255) NULL
END

GO

/* 5/6/2014 NS modified for VSPLUS-602, ThresholdType - new column */
IF NOT EXISTS (select * from syscolumns where name= 'ThresholdType' and id = object_id('dbo.DominoDiskSettings'))
BEGIN	
	ALTER TABLE dbo.DominoDiskSettings ADD ThresholdType [varchar] (10) null
END
GO	

/*2/9/2016 Durga Added for VSPLUS-2174*/
USE [VSS_Statistics]
GO
IF  EXISTS (select * from syscolumns where id = object_id('dbo.TravelerDailySummaryStats'))
BEGIN
 exec sp_rename TravelerDailySummaryStats, TravelerSummaryStats
END
GO
/* 5/9/2014 NS added for VSPLUS-557*/
USE [VSS_Statistics]
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TravelerSummaryStats]') AND type in (N'U'))
BEGIN
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
END
GO


--Mukund 16Jul14, VSPLUS-741, VSPLUS-785 Disable/Enable Timer to update count in Header boxes

USE [vitalsigns]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PageSessions]') AND type in (N'U'))
BEGIN
Drop TABLE [dbo].[PageSessions]
END

IF NOT EXISTS (select * from syscolumns where name= 'SessionNames' and id = object_id('dbo.MenuItems'))
BEGIN	
	ALTER TABLE dbo.MenuItems ADD SessionNames [nvarchar] (200) null
END
GO
IF NOT EXISTS (select * from syscolumns where name= 'TimerEnable' and id = object_id('dbo.MenuItems'))
BEGIN	
	ALTER TABLE dbo.MenuItems ADD TimerEnable [bit] null
END
GO
--Durga VSPLUS 1631 
USE [VitalSigns]
GO	
IF NOT EXISTS (select * from syscolumns where name= 'OverrideSort' and id = object_id('dbo.MenuItems'))
BEGIN	
	ALTER TABLE dbo.MenuItems ADD OverrideSort int NULL
END
GO
/* 5/9/2014 NS added a new set of MenuItems, per Dhanraj's modifications */
/* 7/10/2014 NS modified for VSPLUS-318 */
--Mukund 16Jul14, VSPLUS-741, VSPLUS-785, To Disable/Enable Timer & Session variable names are now part of MenuItems, which were in PageSessions earlier(VSPLUS-138: Out Of Memory Issue). PageSessions table is dropped now. Only Server Settings Editor has TimerEnable as 0 (false)
/* 7/22/2014 NS added a new item for DAG Health */
/* 10/6/201 NS removed the item By Inbox Count from the Mail Files menu */
/* 4/17/2015 NS commented out VS Blog per discussion with Alan */
/* 6/25/2015 NS modified Cluster Health parent ID for VSPLUS-1909 */
--1/21/2016 Durga modified for VSPLUS-2474
/* 1/29/2016 NS modified for VSPLUS-2568 */
USE [vitalsigns]
GO
Delete from MenuItems
Go
SET IDENTITY_INSERT [dbo].[MenuItems] ON
INSERT [dbo].[MenuItems] ([ID], [DisplayText], [OrderNum], [ParentID], [PageLink], [Level], [RefName], [ImageURL], [MenuArea],[SessionNames] ,[TimerEnable]) VALUES (1, N'Home', 0, NULL, N'~/Configurator/Default.aspx', 1, N'Home', N'~/images/icons/house.png', N'Configurator', 'Submenu',0)
INSERT [dbo].[MenuItems] ([ID], [DisplayText], [OrderNum], [ParentID], [PageLink], [Level], [RefName], [ImageURL], [MenuArea],[SessionNames] ,[TimerEnable]) VALUES (2, N'Servers & Devices', 1, NULL, NULL, 1, N'ServersDevices', N'~/images/icons/server_key.png', N'Configurator', NULL,0)
INSERT [dbo].[MenuItems] ([ID], [DisplayText], [OrderNum], [ParentID], [PageLink], [Level], [RefName], [ImageURL], [MenuArea],[SessionNames] ,[TimerEnable]) VALUES (3, N'Alerts', 2, NULL, NULL, 1, N'Alerts', N'~/images/icons/error.png', N'Configurator', NULL,0)
INSERT [dbo].[MenuItems] ([ID], [DisplayText], [OrderNum], [ParentID], [PageLink], [Level], [RefName], [ImageURL], [MenuArea],[SessionNames] ,[TimerEnable]) VALUES (4, N'Hours & Maintenance', 3, NULL, NULL, 1, N'HoursMaintenance', N'~/images/icons/clock.png', N'Configurator', NULL,0)
INSERT [dbo].[MenuItems] ([ID], [DisplayText], [OrderNum], [ParentID], [PageLink], [Level], [RefName], [ImageURL], [MenuArea],[SessionNames] ,[TimerEnable]) VALUES (5, N'Stored Passwords & Options', 4, NULL, NULL, 1, N'Stored PasswordsOptions', N'~/images/icons/folder_key.png', N'Configurator', NULL,0)
INSERT [dbo].[MenuItems] ([ID], [DisplayText], [OrderNum], [ParentID], [PageLink], [Level], [RefName], [ImageURL], [MenuArea],[SessionNames] ,[TimerEnable]) VALUES (6, N'Service Controller', 5, NULL, N'~/Configurator/ServiceController.aspx', 1, N'ServiceController', N'~/images/icons/wrench_orange.png', N'Configurator', NULL,0)
INSERT [dbo].[MenuItems] ([ID], [DisplayText], [OrderNum], [ParentID], [PageLink], [Level], [RefName], [ImageURL], [MenuArea],[SessionNames] ,[TimerEnable]) VALUES (8, N'Reports', 7, NULL, NULL, 1, N'Reports', N'~/images/icons/report.png', N'Configurator', NULL,0)
/*INSERT [dbo].[MenuItems] ([ID], [DisplayText], [OrderNum], [ParentID], [PageLink], [Level], [RefName], [ImageURL], [MenuArea],[SessionNames] ,[TimerEnable]) VALUES (9, N'VitalSigns Blog', 8, NULL, N'/VSBlog.aspx', 1, N'VitalSigns Blog', N'~/images/icons/transmit_blue.png', N'Configurator', NULL,0)*/
INSERT [dbo].[MenuItems] ([ID], [DisplayText], [OrderNum], [ParentID], [PageLink], [Level], [RefName], [ImageURL], [MenuArea],[SessionNames] ,[TimerEnable]) VALUES (10, N'Setup & Security', 9, NULL, NULL, 1, N'Security', N'~/images/icons/key.png', N'Configurator', NULL,0)
INSERT [dbo].[MenuItems] ([ID], [DisplayText], [OrderNum], [ParentID], [PageLink], [Level], [RefName], [ImageURL], [MenuArea],[SessionNames] ,[TimerEnable]) VALUES (11, N'My Account', 11, NULL, N'~/Security/MyAccount.aspx', 1, N'MyAccount', N'~/images/icons/user.png', N'Configurator', 'UserLogin,UserFullName,UserEmail,UserSecurityQuestion1,UserSecurityQuestion1Answer,UserSecurityQuestion2,UserSecurityQuestion2Answer,Refreshtime,StartupURL,UserEmail,Isconfigurator,IsDashboard,Isconsolecomm,CustomBackground,UserID',0)
INSERT [dbo].[MenuItems] ([ID], [DisplayText], [OrderNum], [ParentID], [PageLink], [Level], [RefName], [ImageURL], [MenuArea],[SessionNames] ,[TimerEnable]) VALUES (12, N'IBM Domino', 3, 2, NULL, 2, N'LotusDomino', N'~/images/icons/dominoserver.gif', N'Configurator', NULL,0)
INSERT [dbo].[MenuItems] ([ID], [DisplayText], [OrderNum], [ParentID], [PageLink], [Level], [RefName], [ImageURL], [MenuArea],[SessionNames] ,[TimerEnable]) VALUES (13, N'IBM Sametime', 4, 2, N'~/Configurator/LotusSametimeGrid.aspx', 2, N'LotusSametimeServer', N'~/images/icons/sametime.gif', N'Configurator', 'UserPreferences,SametimeUpdateStatus,UserID,RestrictedServers,sametime',0)
INSERT [dbo].[MenuItems] ([ID], [DisplayText], [OrderNum], [ParentID], [PageLink], [Level], [RefName], [ImageURL], [MenuArea],[SessionNames] ,[TimerEnable]) VALUES (15, N'Mail', 5, 2, NULL, 2, N'Mail', N'~/images/icons/email.png', N'Configurator', NULL,0)
INSERT [dbo].[MenuItems] ([ID], [DisplayText], [OrderNum], [ParentID], [PageLink], [Level], [RefName], [ImageURL], [MenuArea],[SessionNames] ,[TimerEnable]) VALUES (16, N'BlackBerry Enterprise Servers', 1, 2, N'~/Configurator/BlackBerry.aspx', 2, N'BlackBerry', N'~/images/icons/BBDevice.gif', N'Configurator', 'UserPreferences,BlackberryUpdateStatus,UserID,RestrictedServers,BlackBerryServers',0)
INSERT [dbo].[MenuItems] ([ID], [DisplayText], [OrderNum], [ParentID], [PageLink], [Level], [RefName], [ImageURL], [MenuArea],[SessionNames] ,[TimerEnable]) VALUES (17, N'URLs', 13, 2, N'~/Configurator/URLsGrid.aspx', 2, N'URL', N'~/images/icons/url.gif', N'Configurator', 'UserPreferences,URLUpdateStatus,RestrictedServers,URLs,UserID',0)
INSERT [dbo].[MenuItems] ([ID], [DisplayText], [OrderNum], [ParentID], [PageLink], [Level], [RefName], [ImageURL], [MenuArea],[SessionNames] ,[TimerEnable]) VALUES (18, N'Network Devices', 12, 2, N'~/Configurator/NetworkDevicesGrid.aspx', 2, N'NetworkDevices', N'~/images/icons/network.gif', N'Configurator', 'UserPreferences,NetworkDeviceUpdateStatus,RestrictedServers,NetworkDevices,UserID',0)
INSERT [dbo].[MenuItems] ([ID], [DisplayText], [OrderNum], [ParentID], [PageLink], [Level], [RefName], [ImageURL], [MenuArea],[SessionNames] ,[TimerEnable]) VALUES (19, N'Alert Settings', 0, 3, N'~/Configurator/Alert_Settings.aspx', 2, N'AlertSettings', N'~/images/icons/sound.png', N'Configurator', 'AlertDataEvents,UserID,RestrictedServers,EmergencyAlerts',0)
INSERT [dbo].[MenuItems] ([ID], [DisplayText], [OrderNum], [ParentID], [PageLink], [Level], [RefName], [ImageURL], [MenuArea],[SessionNames] ,[TimerEnable]) VALUES (20, N'Alert Definitions', 1, 3, N'~/Configurator/AlertDefinitions_Grid.aspx', 2, N'AlertDefinitions', N'~/images/icons/transmit_blue.png', N'Configurator', NULL,0)
INSERT [dbo].[MenuItems] ([ID], [DisplayText], [OrderNum], [ParentID], [PageLink], [Level], [RefName], [ImageURL], [MenuArea],[SessionNames] ,[TimerEnable]) VALUES (23, N'IBM Domino Settings', 0, 5, N'~/Configurator/DominoSettings.aspx', 2, N'DominoSettings', N'~/images/icons/dominoserver.gif', N'Configurator', NULL,0)
-- INSERT [dbo].[MenuItems] ([ID], [DisplayText], [OrderNum], [ParentID], [PageLink], [Level], [RefName], [ImageURL], [MenuArea],[SessionNames] ,[TimerEnable]) VALUES (24, N'IBM Sametime Settings', 1, 5, N'~/Configurator/SametimeSettings.aspx', 2, N'SametimeSettings', N'~/images/icons/sametime.gif', N'Configurator', NULL,0)
INSERT [dbo].[MenuItems] ([ID], [DisplayText], [OrderNum], [ParentID], [PageLink], [Level], [RefName], [ImageURL], [MenuArea],[SessionNames] ,[TimerEnable]) VALUES (25, N'Server Credentials', 2, 5, N'~/Configurator/PwrshellCredentials.aspx', 2, N'ServersCredentials', N'~/images/icons/key.png', N'Configurator', 'UserPreferences,ErrorStatus,Alias,CredentialsUpdateStatus,password,UserID',0)
INSERT [dbo].[MenuItems] ([ID], [DisplayText], [OrderNum], [ParentID], [PageLink], [Level], [RefName], [ImageURL], [MenuArea],[SessionNames] ,[TimerEnable]) VALUES (26, N'License Information', 3, 5, N'~/Configurator/LicenseInformation.aspx', 2, N'LicenseInformation', N'~/images/icons/license.png', N'Configurator', 'Licence',0)
INSERT [dbo].[MenuItems] ([ID], [DisplayText], [OrderNum], [ParentID], [PageLink], [Level], [RefName], [ImageURL], [MenuArea],[SessionNames] ,[TimerEnable]) VALUES (27, N'Maintain Server Locations', 0, 10, N'~/Security/AdminTab.aspx', 2, N'MaintainServerLocations', N'~/images/icons/map.png', N'Configurator', 'UserPreferences,ErrorStatus,Locations,servertypes,UserID',0)
INSERT [dbo].[MenuItems] ([ID], [DisplayText], [OrderNum], [ParentID], [PageLink], [Level], [RefName], [ImageURL], [MenuArea],[SessionNames] ,[TimerEnable]) VALUES (28, N'Maintain Servers', 1, 10, N'~/Security/MaintainServers.aspx', 2, N'MaintainServers', N'~/images/icons/server_key.png', N'Configurator', 'UserPreferences,UserID,Servers',0)
INSERT [dbo].[MenuItems] ([ID], [DisplayText], [OrderNum], [ParentID], [PageLink], [Level], [RefName], [ImageURL], [MenuArea],[SessionNames] ,[TimerEnable]) VALUES (29, N'Maintain Users', 2, 10, N'~/Security/MaintainUsers.aspx', 2, N'MaintainUsers', N'~/images/icons/group.png', N'Configurator', 'Servers,UserID,UserPreferences',0)
INSERT [dbo].[MenuItems] ([ID], [DisplayText], [OrderNum], [ParentID], [PageLink], [Level], [RefName], [ImageURL], [MenuArea],[SessionNames] ,[TimerEnable]) VALUES (30, N'Assign Server Access', 3, 10, N'~/Security/AssignServerAccess.aspx', 2, N'AssignServerAccess', N'~/images/icons/sitemap_color.png', N'Configurator', 'visible,NotVisible,UserPreferences,ServerAccessGrid,locationnotdt,UserID',0)
INSERT [dbo].[MenuItems] ([ID], [DisplayText], [OrderNum], [ParentID], [PageLink], [Level], [RefName], [ImageURL], [MenuArea],[SessionNames] ,[TimerEnable]) VALUES (31, N'Assign Navigator Access', 4, 10, N'~/Security/AssignNavigator.aspx', 2, N'NavigatorAccess', N'~/images/icons/sitemap_color.png', N'Configurator', 'DataNotVisible,DataVisible,UserFullName,GroupIndex,ItemIndex,Submenu',0)
INSERT [dbo].[MenuItems] ([ID], [DisplayText], [OrderNum], [ParentID], [PageLink], [Level], [RefName], [ImageURL], [MenuArea],[SessionNames] ,[TimerEnable]) VALUES (51, N'Change Password', 12, NULL, N'~/Security/ChangePwd.aspx?M=C', 1, N'ChangePassword', N'~/images/icons/chngpwd.png', N'Configurator', 'UserPassword,UserID',0)
INSERT [dbo].[MenuItems] ([ID], [DisplayText], [OrderNum], [ParentID], [PageLink], [Level], [RefName], [ImageURL], [MenuArea],[SessionNames] ,[TimerEnable]) VALUES (52, N'Preferences', 4, 5, N'~/Configurator/UserPreferences.aspx', 2, N'Preferences', N'~/images/icons/information.png', N'Configurator', 'Profiles,ProfileId,UserID,DominoSession,SametimeSession,ExchangeSession,URLSession,ActiveDirectorySession,SessionName,UserLogin,UserFullName,UserEmail,UserSecurityQuestion1,UserSecurityQuestion1Answer,UserSecurityQuestion2,UserSecurityQuestion2Answer,Refreshtime,StartupURL
Isconfigurator,IsDashboard,Isconsolecomm',0)
--VSPLUS-2519 Sowjanya
INSERT [dbo].[MenuItems] ([ID], [DisplayText], [OrderNum], [ParentID], [PageLink], [Level], [RefName], [ImageURL], [MenuArea],[SessionNames] ,[TimerEnable]) VALUES (53, N'VitalSigns Dashboard', 14, NULL, N'~/Dashboard/OverallHealth1.aspx', 1, N'Dashboard', N'~/images/icons/dash2.png', N'Configurator', 'showsummary,UserLogin,CustomBackground,FilterByValue,ViewBy,FilterLabel,checkeddata,clouddata,Networkdata,MyServerTypes,MyServerLocations,MyCategory,UserPreferences,FilterBy,UserID,Isconfigurator,UserFullName',1)
INSERT [dbo].[MenuItems] ([ID], [DisplayText], [OrderNum], [ParentID], [PageLink], [Level], [RefName], [ImageURL], [MenuArea],[SessionNames] ,[TimerEnable]) VALUES (55, N'Business Hours', 1, 4, N'~/Configurator/BusinessHoursGrid.aspx', 2, N'BusinessHours', N'~/images/icons/bushours.png', N'Configurator', 'UserID',0)
INSERT [dbo].[MenuItems] ([ID], [DisplayText], [OrderNum], [ParentID], [PageLink], [Level], [RefName], [ImageURL], [MenuArea],[SessionNames] ,[TimerEnable]) VALUES (56, N'Maintenance', 0, 4, N'~/Configurator/MaintenanceWinList.aspx', 2, N'Maintenance', N'~/images/icons/maintenance.png', N'Configurator', 'ReturnUrl,UserPreferences,MaintServers,UserID',0)
INSERT [dbo].[MenuItems] ([ID], [DisplayText], [OrderNum], [ParentID], [PageLink], [Level], [RefName], [ImageURL], [MenuArea],[SessionNames] ,[TimerEnable]) VALUES (57, N'View Logs', 1, 126, N'~/Configurator/ReadVSLogs.aspx', 2, N'VSLogs', N'~/images/icons/zoom_in.png', N'Configurator','LogFiles',0)
INSERT [dbo].[MenuItems] ([ID], [DisplayText], [OrderNum], [ParentID], [PageLink], [Level], [RefName], [ImageURL], [MenuArea],[SessionNames] ,[TimerEnable]) VALUES (58, N'Import Domino Servers', 5, 10, N'~/Security/ImportServers.aspx', 2, N'ImportServers', N'~/images/icons/dominoserver.gif', N'Configurator', 'ImportedServers',0)
INSERT [dbo].[MenuItems] ([ID], [DisplayText], [OrderNum], [ParentID], [PageLink], [Level], [RefName], [ImageURL], [MenuArea],[SessionNames] ,[TimerEnable]) VALUES (59, N'Server Settings Editor', 0, 2, N'~/Security/ServerSettingsEditor.aspx', 2, N'ApplyServerSettings', N'~/images/icons/server_edit.png', N'Configurator', 'DataServers,Profiles,DominoServers,DominoTasks,ExchangeServers,WindowsServices,DiskSettingsDisks,DiskSettingsServers,ServerLocations,ServerCredentials,BusinessHours,Mode,ProfileId,UserPreferences,IntialAllservers,RestrictedServers,DominoTasks,Profiles,DataServers
UserID,WindowsServers,DiskSettingsDisks,ServerLocations',0)
INSERT [dbo].[MenuItems] ([ID], [DisplayText], [OrderNum], [ParentID], [PageLink], [Level], [RefName], [ImageURL], [MenuArea],[SessionNames] ,[TimerEnable]) VALUES (60, N'Notes Traveler Data Store', 1, 5, N'~/Configurator/TravelerDataStore.aspx', 2, N'TravelerDataStore', N'~/images/icons/traveler-icon.png', N'Configurator', 'UserPreferences,travelerDTVar,UserID',0)
INSERT [dbo].[MenuItems] ([ID], [DisplayText], [OrderNum], [ParentID], [PageLink], [Level], [RefName], [ImageURL], [MenuArea],[SessionNames] ,[TimerEnable]) VALUES (61, N'High Availability', 7, 10, N'~/Security/HighAvailability.aspx', 2, N'HighAvailability', NULL, N'Configurator', 'DataServers,ServerNodes,SelectedServers,UnSelectedServers',0)
INSERT [dbo].[MenuItems] ([ID], [DisplayText], [OrderNum], [ParentID], [PageLink], [Level], [RefName], [ImageURL], [MenuArea],[SessionNames] ,[TimerEnable]) VALUES (62, N'Microsoft Exchange', 7, 2, NULL, 2, N'MicrosoftExchangeServer', N'~/images/icons/exchange.jpg', N'Configurator', NULL,0)
INSERT [dbo].[MenuItems] ([ID], [DisplayText], [OrderNum], [ParentID], [PageLink], [Level], [RefName], [ImageURL], [MenuArea],[SessionNames] ,[TimerEnable]) VALUES (63, N'Mobile Users', 11, 2, N'~/Configurator/MobileUsersGrid.aspx', 2, N'MobileUsers', N'~/images/icons/phone.png', N'Configurator', 'UserPreferences,MobileDevicesDT,MobUserThGrid,UserID,DeviceID,MyDeviceId',0)
INSERT [dbo].[MenuItems] ([ID], [DisplayText], [OrderNum], [ParentID], [PageLink], [Level], [RefName], [ImageURL], [MenuArea],[SessionNames] ,[TimerEnable]) VALUES (73, N'Overall Health', 1, NULL, NULL, 1, N'Overall Health', NULL, N'Dashboard', NULL,1)
INSERT [dbo].[MenuItems] ([ID], [DisplayText], [OrderNum], [ParentID], [PageLink], [Level], [RefName], [ImageURL], [MenuArea],[SessionNames] ,[TimerEnable]) VALUES (74, N'Overview', 1, 73, N'~/Dashboard/OverallHealth1.aspx', 2, N'Overview', N'~/images/icons/firstaid2.png', N'Dashboard', 'showsummary,UserLogin,CustomBackground,FilterByValue,ViewBy,FilterLabel,checkeddata,clouddata,Networkdata,MyServerTypes,MyServerLocations,MyCategory,UserPreferences,FilterBy,UserID,Isconfigurator,UserFullName',1)
INSERT [dbo].[MenuItems] ([ID], [DisplayText], [OrderNum], [ParentID], [PageLink], [Level], [RefName], [ImageURL], [MenuArea],[SessionNames] ,[TimerEnable]) VALUES (75, N'Executive Summary', 2, 73, N'~/Dashboard/SummaryLandscape.aspx', 2, N'Executive Summary', N'~/images/icons/executive_icon.jpg', N'Dashboard', 'showsummary,UserLogin,CustomBackground,BackURL,UserFullName,Isconfigurator',1)
INSERT [dbo].[MenuItems] ([ID], [DisplayText], [OrderNum], [ParentID], [PageLink], [Level], [RefName], [ImageURL], [MenuArea],[SessionNames] ,[TimerEnable]) VALUES (76, N'EXJournal Summary', 3, 73, N'~/Dashboard/SummaryEXJournal.aspx', 2, N'EXJournal Summary', N'~/images/icons/databases.png', N'Dashboard', N'showsummary,UserLogin,UserPreferences,StatusTable,SummaryEXJournal,UserID',1)
INSERT [dbo].[MenuItems] ([ID], [DisplayText], [OrderNum], [ParentID], [PageLink], [Level], [RefName], [ImageURL], [MenuArea],[SessionNames] ,[TimerEnable]) VALUES (77, N'Mail Delivery Status', 4, 73, N'~/Dashboard/MailDeliveryStatus.aspx', 2, N'Mail Delivery Status', N'~/images/icons/maildelivery.png', N'Dashboard', N'StatusTable,showsummary,UserLogin,UserPreferences,UserID,ExStatusTable,UserID',1)
INSERT [dbo].[MenuItems] ([ID], [DisplayText], [OrderNum], [ParentID], [PageLink], [Level], [RefName], [ImageURL], [MenuArea],[SessionNames] ,[TimerEnable]) VALUES (78, N'Status List', 5, 73, N'~/Dashboard/DeviceTypeList.aspx', 2, N'Status List', N'~/images/icons/detail_list.png', N'Dashboard', N'myRow,StatusTable,showsummary,UserLogin,ViewBy,FilterByValue,Isconfigurator,Isconsolecomm,UserPreferences,Refreshtime,BackURL
,GroupIndex,ItemIndex,Submenu,SubmenuItem,RestrictedServers,srvrname,UserFullName',1)
INSERT [dbo].[MenuItems] ([ID], [DisplayText], [OrderNum], [ParentID], [PageLink], [Level], [RefName], [ImageURL], [MenuArea],[SessionNames] ,[TimerEnable]) VALUES (79, N'Server Health', 2, NULL, NULL, 1, N'Server Health', NULL, N'Dashboard', NULL,0)
INSERT [dbo].[MenuItems] ([ID], [DisplayText], [OrderNum], [ParentID], [PageLink], [Level], [RefName], [ImageURL], [MenuArea],[SessionNames] ,[TimerEnable]) VALUES (80, N'Domino Health', 4, 79, N'~/Dashboard/DominoServerHealthPage.aspx', 2, N'Domino Health', N'~/images/icons/dominoserver.gif', N'Dashboard', N'DominoServerList,TravelerServerList,SametimeServerList,DominoIssues,MailStatus,BackURL,UserPreferences,
MoniterdeServerTasks,UserID,SysInfo',0)
INSERT [dbo].[MenuItems] ([ID], [DisplayText], [OrderNum], [ParentID], [PageLink], [Level], [RefName], [ImageURL], [MenuArea],[SessionNames] ,[TimerEnable]) VALUES (81, N'Traveler Health', 10, 79, N'~/Dashboard/TravelerUsersDevicesOS.aspx', 2, N'Traveler Health', N'~/images/icons/traveler-icon.png', N'Dashboard', N'GridData,CurrentMailSrvInd,UserPreferences,GridData,CurrentMailSrvInd,UserID,UserPreferences,Submenu',0)
INSERT [dbo].[MenuItems] ([ID], [DisplayText], [OrderNum], [ParentID], [PageLink], [Level], [RefName], [ImageURL], [MenuArea],[SessionNames] ,[TimerEnable]) VALUES (82, N'Quickr Health', 7, 79, N'~/Dashboard/QuickrHealth.aspx', 2, N'Quickr Health', N'~/images/icons/quickr.gif', N'Dashboard', N'UserPreferences,BackURL,UserID,QuickrServersGridData,QuickrPlacesData',0)
INSERT [dbo].[MenuItems] ([ID], [DisplayText], [OrderNum], [ParentID], [PageLink], [Level], [RefName], [ImageURL], [MenuArea],[SessionNames] ,[TimerEnable]) VALUES (83, N'Sametime Health', 8, 79, N'~/Dashboard/SametimeHealth.aspx', 2, N'Sametime Server Health', N'~/images/icons/sametime.gif', N'Dashboard', N'dominosametimegrid,sametimeservergrid,Type,UserPreferences,Type,UserID,UserPreferences',0)
INSERT [dbo].[MenuItems] ([ID], [DisplayText], [OrderNum], [ParentID], [PageLink], [Level], [RefName], [ImageURL], [MenuArea],[SessionNames] ,[TimerEnable]) VALUES (84, N'Exchange Health', 5, 79, N'~/Dashboard/ExchangeHealth.aspx', 2, N'Exchange Server Health', N'~/images/icons/exchange.jpg', N'Dashboard', N'ExchangeServerList,ExchangeDAGStatusList,HubEdgeGrid,CASStatusGrid,MailBoxGridList,CASArrayGrid,BackURL,UserPreferences,UserFullName,UserID',0)
INSERT [dbo].[MenuItems] ([ID], [DisplayText], [OrderNum], [ParentID], [PageLink], [Level], [RefName], [ImageURL], [MenuArea],[SessionNames] ,[TimerEnable]) VALUES (85, N'Server Days Up', 9, 79, N'~/Dashboard/ServerDaysUp.aspx', 2, N'Server Days Up', N'~/images/icons/dominoserver.gif', N'Dashboard', NULL,0)
INSERT [dbo].[MenuItems] ([ID], [DisplayText], [OrderNum], [ParentID], [PageLink], [Level], [RefName], [ImageURL], [MenuArea],[SessionNames] ,[TimerEnable]) VALUES (86, N'Alerts History', 2, 79, N'~/Dashboard/OverallServerAlerts.aspx', 2, N'Alerts History', N'~/images/icons/error.png', N'Dashboard', 'UserPreferences,UserID',0)
INSERT [dbo].[MenuItems] ([ID], [DisplayText], [OrderNum], [ParentID], [PageLink], [Level], [RefName], [ImageURL], [MenuArea],[SessionNames] ,[TimerEnable]) VALUES (87, N'Key Metrics', 3, NULL, NULL, 1, N'Key Metrics', NULL, N'Dashboard', NULL,0)
INSERT [dbo].[MenuItems] ([ID], [DisplayText], [OrderNum], [ParentID], [PageLink], [Level], [RefName], [ImageURL], [MenuArea],[SessionNames] ,[TimerEnable]) VALUES (88, N'Mobile Users', 1, 87, N'~/Dashboard/MobileUsers.aspx', 2, N'Mobile Users', N'~/images/icons/group.png', N'Dashboard', N'UserPreferences,MenuItem,Fillgid,TellCommand,myUserName,myServerName,myDeviceName,TellCommand,UserID,MyDeviceId',0)
INSERT [dbo].[MenuItems] ([ID], [DisplayText], [OrderNum], [ParentID], [PageLink], [Level], [RefName], [ImageURL], [MenuArea],[SessionNames] ,[TimerEnable]) VALUES (89, N'Disk Health', 2, 87, N'~/Dashboard/DiskHealth.aspx', 2, N'Disk Health', N'~/images/icons/network.gif', N'Dashboard', N'UserPreferences,GridData,rowIndex,ServerName,UserID',0)
INSERT [dbo].[MenuItems] ([ID], [DisplayText], [OrderNum], [ParentID], [PageLink], [Level], [RefName], [ImageURL], [MenuArea],[SessionNames] ,[TimerEnable]) VALUES (90, N'Mail Health', 3, 87, N'~/Dashboard/MailHealth.aspx', 2, N'Mail Health', N'~/images/icons/email.png', N'Dashboard', N'MailTab,NotesMailTab,UserPreferences,BackURL,MailTab,NotesMailTab,UserID,UserPreferences',0)
INSERT [dbo].[MenuItems] ([ID], [DisplayText], [OrderNum], [ParentID], [PageLink], [Level], [RefName], [ImageURL], [MenuArea],[SessionNames] ,[TimerEnable]) VALUES (91, N'Cluster Health', 4, 79, N'~/Dashboard/ClusterHealth.aspx', 2, N'Cluster Health', N'~/images/icons/cluster.png', N'Dashboard', N'UserPreferences,ClusterHealth,UserID,Type',0)
INSERT [dbo].[MenuItems] ([ID], [DisplayText], [OrderNum], [ParentID], [PageLink], [Level], [RefName], [ImageURL], [MenuArea],[SessionNames] ,[TimerEnable]) VALUES (92, N'URL Health', 5, 87, N'~/Dashboard/URLHealth.aspx', 2, N'URL Health', N'~/images/icons/url.gif', N'Dashboard', N'URLGrid,Type,UserPreferences,UserID',0)
INSERT [dbo].[MenuItems] ([ID], [DisplayText], [OrderNum], [ParentID], [PageLink], [Level], [RefName], [ImageURL], [MenuArea],[SessionNames] ,[TimerEnable]) VALUES (93, N'User Counts', 6, 87, N'~/Dashboard/UserCount.aspx', 2, N'User Counts', N'~/images/icons/group.png', N'Dashboard', NULL,0)
INSERT [dbo].[MenuItems] ([ID], [DisplayText], [OrderNum], [ParentID], [PageLink], [Level], [RefName], [ImageURL], [MenuArea],[SessionNames] ,[TimerEnable]) VALUES (94, N'Response Time', 7, 87, N'~/Dashboard/ResponseTime.aspx', 2, N'Response Time', N'~/images/icons/chart_line.png', N'Dashboard', NULL,0)
INSERT [dbo].[MenuItems] ([ID], [DisplayText], [OrderNum], [ParentID], [PageLink], [Level], [RefName], [ImageURL], [MenuArea],[SessionNames] ,[TimerEnable]) VALUES (95, N'Maintenance Windows', 8, 87, N'~/Dashboard/ServerMaintenanceList.aspx', 2, N'Maintenance Windows', N'~/images/icons/information.png', N'Dashboard',N'maitservers,UserPreferences,UserID',0)
INSERT [dbo].[MenuItems] ([ID], [DisplayText], [OrderNum], [ParentID], [PageLink], [Level], [RefName], [ImageURL], [MenuArea],[SessionNames] ,[TimerEnable]) VALUES (96, N'Overall Domino Statistics', 9, 87, N'~/Dashboard/OverallServerStats.aspx', 2, N'Overall Domino Statistics', N'~/images/icons/chart_line.png', N'Dashboard', 'UserPreferences,UserID',0)
INSERT [dbo].[MenuItems] ([ID], [DisplayText], [OrderNum], [ParentID], [PageLink], [Level], [RefName], [ImageURL], [MenuArea],[SessionNames] ,[TimerEnable]) VALUES (97, N'Database List', 4, NULL, NULL, 1, N'Database List', NULL, N'Dashboard', NULL,0)
INSERT [dbo].[MenuItems] ([ID], [DisplayText], [OrderNum], [ParentID], [PageLink], [Level], [RefName], [ImageURL], [MenuArea],[SessionNames] ,[TimerEnable]) VALUES (98, N'All Databases', 1, 97, N'~/Dashboard/MonitoredDB.aspx', 2, N'All Databases', N'~/images/icons/detail_list.png', N'Dashboard', N'UserPreferences,Statustab,Dailytab,ActBtn,UserFullName,UserID',0)
INSERT [dbo].[MenuItems] ([ID], [DisplayText], [OrderNum], [ParentID], [PageLink], [Level], [RefName], [ImageURL], [MenuArea],[SessionNames] ,[TimerEnable]) VALUES (99, N'By Template', 2, 97, N'~/Dashboard/DBsByTemplate.aspx', 2, N'By Template', N'~/images/icons/detail_list.png', N'Dashboard', 'UserPreferences,DBsByTemplate,UserID,UserPreferences',0)
INSERT [dbo].[MenuItems] ([ID], [DisplayText], [OrderNum], [ParentID], [PageLink], [Level], [RefName], [ImageURL], [MenuArea],[SessionNames] ,[TimerEnable]) VALUES (100, N'Problems', 3, 97, N'~/Dashboard/Problems.aspx', 2, N'Problems', N'~/images/icons/exclamation.png', N'Dashboard', N'ProblemsFiles,UserPreferences,UserID',0)
INSERT [dbo].[MenuItems] ([ID], [DisplayText], [OrderNum], [ParentID], [PageLink], [Level], [RefName], [ImageURL], [MenuArea],[SessionNames] ,[TimerEnable]) VALUES (101, N'Mail', 5, NULL, NULL, 1, N'Mail Files', NULL, N'Dashboard', NULL,0)
INSERT [dbo].[MenuItems] ([ID], [DisplayText], [OrderNum], [ParentID], [PageLink], [Level], [RefName], [ImageURL], [MenuArea],[SessionNames] ,[TimerEnable]) VALUES (102, N'Alphabetical Order', 1, 101, N'~/Dashboard/MailFiles.aspx?MItem=1', 2, N'Alphabetical Order', N'~/images/icons/email.png', N'Dashboard', N'MailFiles,UserPreferences,UserID,UserPreferences',0)
INSERT [dbo].[MenuItems] ([ID], [DisplayText], [OrderNum], [ParentID], [PageLink], [Level], [RefName], [ImageURL], [MenuArea],[SessionNames] ,[TimerEnable]) VALUES (103, N'Biggest Mail Files', 2, 101, N'~/Dashboard/BiggestMailFiles.aspx', 2, N'Biggest Mail Files', N'~/images/icons/email.png', N'Dashboard', NULL,0)
/*INSERT [dbo].[MenuItems] ([ID], [DisplayText], [OrderNum], [ParentID], [PageLink], [Level], [RefName], [ImageURL], [MenuArea],[SessionNames] ,[TimerEnable]) VALUES (104, N'By Inbox Count', 3, 101, N'~/Dashboard/MailFiles.aspx?MItem=2', 2, N'By Inbox Count', N'images/icons/email.png', N'Dashboard', N'MailFiles',1)*/
INSERT [dbo].[MenuItems] ([ID], [DisplayText], [OrderNum], [ParentID], [PageLink], [Level], [RefName], [ImageURL], [MenuArea],[SessionNames] ,[TimerEnable]) VALUES (105, N'By Mail Template', 4, 101, N'~/Dashboard/MailFiles.aspx?MItem=4', 2, N'By Mail Template', N'~/images/icons/email.png', N'Dashboard',  N'MailFiles',0)
INSERT [dbo].[MenuItems] ([ID], [DisplayText], [OrderNum], [ParentID], [PageLink], [Level], [RefName], [ImageURL], [MenuArea],[SessionNames] ,[TimerEnable]) VALUES (106, N'By Percent of Quota', 5, 101, N'~/Dashboard/MailFiles.aspx?MItem=3', 2, N'By Percent of Quota', N'~/images/icons/exclamation.png', N'Dashboard', N'MailFiles,UserPreferences,UserID,UserPreferences',0)
INSERT [dbo].[MenuItems] ([ID], [DisplayText], [OrderNum], [ParentID], [PageLink], [Level], [RefName], [ImageURL], [MenuArea],[SessionNames] ,[TimerEnable]) VALUES (107, N'Overall Mail Statistics', 6, 101, N'~/Dashboard/OverallMailStats.aspx', 2, N'Overall Mail Statistics', N'~/images/icons/chart_line.png', N'Dashboard', 'UserPreferences,UserID',0)
INSERT [dbo].[MenuItems] ([ID], [DisplayText], [OrderNum], [ParentID], [PageLink], [Level], [RefName], [ImageURL], [MenuArea],[SessionNames] ,[TimerEnable]) VALUES (108, N'Reports', 6, NULL, N'~/DashboardReports/OverallSrvStatusHealthRpt.aspx?M=d', 1, N'Reports', NULL, N'Dashboard', NULL,0)
--Mukund 18Sep14 ?hidesubmenu=1
INSERT [dbo].[MenuItems] ([ID], [DisplayText], [OrderNum], [ParentID], [PageLink], [Level], [RefName], [ImageURL], [MenuArea],[SessionNames] ,[TimerEnable]) VALUES (109, N'Configurator', 7, NULL, N'~/Configurator/Default.aspx?hidesubmenu=1', 1, N'Configurator', NULL, N'Dashboard', 'Submenu',0)
INSERT [dbo].[MenuItems] ([ID], [DisplayText], [OrderNum], [ParentID], [PageLink], [Level], [RefName], [ImageURL], [MenuArea],[SessionNames] ,[TimerEnable]) VALUES (110, N'My Account', 8, NULL, NULL, 1, N'My Account', NULL, N'Dashboard', NULL,0)
INSERT [dbo].[MenuItems] ([ID], [DisplayText], [OrderNum], [ParentID], [PageLink], [Level], [RefName], [ImageURL], [MenuArea],[SessionNames] ,[TimerEnable]) VALUES (111, N'Account Details', 1, 110, N'~/Security/MyAccount.aspx?dboard=true', 2, N'Account Details', N'~/images/icons/user.png', N'Dashboard', 'UserLogin,UserFullName,UserEmail,UserSecurityQuestion1,UserSecurityQuestion1Answer,UserSecurityQuestion2,UserSecurityQuestion2Answer,Refreshtime,StartupURL,UserEmail,Isconfigurator,IsDashboard,Isconsolecomm,CustomBackground,UserID',0)
INSERT [dbo].[MenuItems] ([ID], [DisplayText], [OrderNum], [ParentID], [PageLink], [Level], [RefName], [ImageURL], [MenuArea],[SessionNames] ,[TimerEnable]) VALUES (112, N'My Custom Pages', 2, 110, N'~/Dashboard/MyCustomPages.aspx', 2, N'My Custom Pages', N'~/images/icons/page_white_stack.png', N'Dashboard', 'UserFullName,UserID',0)
INSERT [dbo].[MenuItems] ([ID], [DisplayText], [OrderNum], [ParentID], [PageLink], [Level], [RefName], [ImageURL], [MenuArea],[SessionNames] ,[TimerEnable]) VALUES (113, N'Logout', 4, 110, N'~/Login.aspx', 2, N'Logout', N'~/images/icons/logout.png', N'Dashboard', 'showsummary,BlackBerryServers,MaintServers,BlackBerryDevicePrbegrid,DominoCluster,DominoCustom,NotesDB,DominoServer,sametime,MailServices,MaintServers,NetworkDevices,NotesDatabase,NotesMailProbe,URLs,ServerVisibleDataGrid,visible,ServerNotVisibleDataGrid,NotVisible,Servers,Users,Locations,Attempts,UserLogin
,UserFullName,UserPassword,UserID,UserEmail,UserSecurityQuestion1,UserSecurityQuestion1Answer,UserSecurityQuestion2,UserSecurityQuestion2Answer,Isconfigurator,IsDashboard,Refreshtime,StartupURL,Isconsolecomm,CustomBackground,UserPreferences,ViewBy,FilterByValue,RestrictedServers,divControl',0)
INSERT [dbo].[MenuItems] ([ID], [DisplayText], [OrderNum], [ParentID], [PageLink], [Level], [RefName], [ImageURL], [MenuArea],[SessionNames] ,[TimerEnable]) VALUES (114, N'Servers', 1, 12, N'~/Configurator/LotusDominoServers.aspx', 3, N'LotusDominoServers', N'~/images/icons/servers.png', N'Configurator', 'DominoUpdateStatus,UserID,RestrictedServers,DominoServer,Key,MenuID',0)
/* 7/22/2015 NS modified for VSPLUS-1911 */
INSERT [dbo].[MenuItems] ([ID], [DisplayText], [OrderNum], [ParentID], [PageLink], [Level], [RefName], [ImageURL], [MenuArea],[SessionNames] ,[TimerEnable]) VALUES (115, N'Notes Database Replicas', 2, 12, N'~/Configurator/ClusterGrid.aspx', 3, N'ClusterGrid', N'~/images/icons/cluster.png', N'Configurator', 'UserPreferences,ClusterUpdateStatus,Status,DominoCluster,UserID',0)
INSERT [dbo].[MenuItems] ([ID], [DisplayText], [OrderNum], [ParentID], [PageLink], [Level], [RefName], [ImageURL], [MenuArea],[SessionNames] ,[TimerEnable]) VALUES (116, N'Notes Databases', 3, 12, N'~/Configurator/NotesDatabase.aspx', 3, N'NotesDatabase', N'~/images/icons/database.png', N'Configurator', 'UserPreferences,NotesDBUpdateStatus,NotesDatabase,UserID',0)
INSERT [dbo].[MenuItems] ([ID], [DisplayText], [OrderNum], [ParentID], [PageLink], [Level], [RefName], [ImageURL], [MenuArea],[SessionNames] ,[TimerEnable]) VALUES (117, N'Custom Statistics', 4, 12, N'~/Configurator/CustomStatistics.aspx', 3, N'CustomStatistics', N'~/images/icons/stats.png', N'Configurator', 'UserPreferences,DominoStatUpdateStatus,Submenu,DominoCustom,UserID',0)
INSERT [dbo].[MenuItems] ([ID], [DisplayText], [OrderNum], [ParentID], [PageLink], [Level], [RefName], [ImageURL], [MenuArea],[SessionNames] ,[TimerEnable]) VALUES (118, N'Server Task Definitions', 5, 12, N'~/Configurator/ServerTaskDefinitions.aspx', 3, N'ServerTaskDefinitions', N'~/images/icons/task.png', N'Configurator', 'UserPreferences,ServerTaskUpdateStatus,DominoCluster,UserID',0)
INSERT [dbo].[MenuItems] ([ID], [DisplayText], [OrderNum], [ParentID], [PageLink], [Level], [RefName], [ImageURL], [MenuArea],[SessionNames] ,[TimerEnable]) VALUES (119, N'Log File Scanning', 6, 12,N'~/Configurator/DominoELSDefinitions_Grid.aspx', 3, N'LogFileScanning', N'~/images/icons/logfile.png', N'Configurator', 'UserPreferences,DELSNameTable,UserID',0)
INSERT [dbo].[MenuItems] ([ID], [DisplayText], [OrderNum], [ParentID], [PageLink], [Level], [RefName], [ImageURL], [MenuArea],[SessionNames] ,[TimerEnable]) VALUES (120, N'BlackBerry Enterprise Servers', 1, 16, N'~/Configurator/BlackBerry.aspx', 3, N'BlackBerry', N'~/images/icons/BBDevice.gif', N'Configurator', 'UserPreferences,BlackberryUpdateStatus,UserID,RestrictedServers,BlackBerryServers',0)
INSERT [dbo].[MenuItems] ([ID], [DisplayText], [OrderNum], [ParentID], [PageLink], [Level], [RefName], [ImageURL], [MenuArea],[SessionNames] ,[TimerEnable]) VALUES (121, N'BlackBerry Device Probes', 2, 16, N'~/Configurator/BlackBerryDeviceProbesgrid.aspx', 3, N'BlackBerryDeviceProbesgrid', NULL, N'Configurator', 'UserPreferences,BlackberryDeviceUpdateStatus,BlackBerryDevicePrbegrid,UserID',0)
INSERT [dbo].[MenuItems] ([ID], [DisplayText], [OrderNum], [ParentID], [PageLink], [Level], [RefName], [ImageURL], [MenuArea],[SessionNames] ,[TimerEnable]) VALUES (122, N'NotesMail Probes', 1, 15, N'~/Configurator/NotesMailprobeGrid.aspx', 3, N'NotesMailProbes', N'~/images/icons/notesmail.png', N'Configurator', 'UserPreferences,NotesMailProbeUpdateStatus,RestrictedServers,NotesMailProbe,NotesMailProbeHistory,UserID',0)
INSERT [dbo].[MenuItems] ([ID], [DisplayText], [OrderNum], [ParentID], [PageLink], [Level], [RefName], [ImageURL], [MenuArea],[SessionNames] ,[TimerEnable]) VALUES (123, N'Mail Services', 3, 15, N'~/Configurator/MailServicesGrid.aspx', 3, N'MailServices', N'~/images/icons/mailservice.png', N'Configurator', 'UserPreferences,MailServiceUpdateStatus,MailServices,UserID',0)
-- INSERT [dbo].[MenuItems] ([ID], [DisplayText], [OrderNum], [ParentID], [PageLink], [Level], [RefName], [ImageURL], [MenuArea],[SessionNames] ,[TimerEnable]) VALUES (124, N'ExchangeMail Probe', 2, 15, N'~/Configurator/ExchangeMailProbe.aspx', 3, N'ExchangeMailProbes', N'~/images/icons/exchangemail.jpg', N'Configurator', NULL,0)
INSERT [dbo].[MenuItems] ([ID], [DisplayText], [OrderNum], [ParentID], [PageLink], [Level], [RefName], [ImageURL], [MenuArea],[SessionNames] ,[TimerEnable]) VALUES (125, N'Assign Features', 6, 10, N'~/Security/AssignFeatures.aspx', 2, N'AssignFeatures', N'~/images/icons/application_add.png', N'Configurator', 'Features',0)
INSERT [dbo].[MenuItems] ([ID], [DisplayText], [OrderNum], [ParentID], [PageLink], [Level], [RefName], [ImageURL], [MenuArea],[SessionNames] ,[TimerEnable]) VALUES (126, N'Logging & Tracing', 10, NULL, NULL, 1, N'LoggingAndTracing', N'~/images/icons/page_white_stack.png', N'Configurator', NULL,0)
INSERT [dbo].[MenuItems] ([ID], [DisplayText], [OrderNum], [ParentID], [PageLink], [Level], [RefName], [ImageURL], [MenuArea],[SessionNames] ,[TimerEnable]) VALUES (127, N'Log Settings', 2, 126, '~/Configurator/LogSettings.aspx', 2, N'LogSettings', N'~/images/icons/gear.png', N'Configurator', NULL,0)
INSERT [dbo].[MenuItems] ([ID], [DisplayText], [OrderNum], [ParentID], [PageLink], [Level], [RefName], [ImageURL], [MenuArea],[SessionNames] ,[TimerEnable]) VALUES (128, N'Overall Sametime Statistics', 10, 87, '~/Dashboard/OverallSametimeStats.aspx', 2, N'Overall Sametime Statistics', N'~/images/icons/chart_line.png', N'Dashboard', NULL,0)
--Mukund 18Jul14
--BELOW ARE INNER PAGE ITEMS. THESE ARE NOT DISPLAYED IN CONFIGURATOR/DASHBOARD MENU. ENTRY IS ONLY FOR TIMER DISABLE
INSERT [dbo].[MenuItems] ([ID], [DisplayText], [OrderNum], [ParentID], [PageLink], [Level], [RefName], [ImageURL], [MenuArea],[SessionNames] ,[TimerEnable]) VALUES (129, N'DominoProperties', 0, 0, '~/Configurator/DominoProperties.aspx', 0, N'DominoProperties', N'', N'InnerPage', 'Key,UserPreferences,DiskDataTable,STSettingsDataSet,ReturnUrl,DominoUpdateStatus,UserId,calanderdata,Submenu',0)
INSERT [dbo].[MenuItems] ([ID], [DisplayText], [OrderNum], [ParentID], [PageLink], [Level], [RefName], [ImageURL], [MenuArea],[SessionNames] ,[TimerEnable]) VALUES (130, N'AlertDef_Edit', 0, 0, '~/Configurator/AlertDef_Edit.aspx', 0, N'AlertDef_Edit', N'', N'InnerPage', 'DataEvents2,DataEvents3,DataServers,UserPreferences,SMSConfig,firsttempid,AlertDetails,profileid3,profilename2,lastid,profilename,profileid,Alerteventnames,SMSConfig,UserPreferences,UserID
,Ekey,EscalationDetails,SMSConfig,Duration,starttime,key,EscalationDetails,changeddata',0)
INSERT [dbo].[MenuItems] ([ID], [DisplayText], [OrderNum], [ParentID], [PageLink], [Level], [RefName], [ImageURL], [MenuArea],[SessionNames] ,[TimerEnable]) VALUES (131, N'MaintenanceWin', 0, 0, '~/Configurator/MaintenanceWin.aspx', 0, N'MaintenanceWin', N'', N'InnerPage', 'UserPreferences,KeyUsersGridView,ReturnUrl,DataServers,UserID',0)
INSERT [dbo].[MenuItems] ([ID], [DisplayText], [OrderNum], [ParentID], [PageLink], [Level], [RefName], [ImageURL], [MenuArea],[SessionNames] ,[TimerEnable]) VALUES (132, N'ExchangeServer', 0, 0, '~/Configurator/ExchangeServer.aspx', 0, N'ExchangeServer', N'', N'InnerPage', 'ExchangeUpdateStatus,DiskDataTable,WindowsServices,DatabaseDataTable,Submenu',0)
/* 7/22/2014 NS added */
INSERT [dbo].[MenuItems] ([ID], [DisplayText], [OrderNum], [ParentID], [PageLink], [Level], [RefName], [ImageURL], [MenuArea],[SessionNames] ,[TimerEnable]) VALUES (133, N'DAG Health', 3, 79, '~/Dashboard/DAGHealth.aspx', 2, N'DAG Health', N'~/images/icons/DAG-Health-Icon.png', N'Dashboard', 'DAGStatusList,UserPreferences,BackURL,DAGHealth,DAGMembers,DAGDB,DAGDBDetails,UserID,UserPreferences',0)
/* 7/24/2014 NS added */
-- INSERT [dbo].[MenuItems] ([ID], [DisplayText], [OrderNum], [ParentID], [PageLink], [Level], [RefName], [ImageURL], [MenuArea],[SessionNames] ,[TimerEnable]) VALUES (134, N'Overall Skype for Business Statistics', 11, 87, '~/Dashboard/OverallLyncStats.aspx', 2, N'Overall Lync Statistics', N'~/images/icons/lync-icon.gif', N'Dashboard', 'UserPreferences,UserID',0)
/* 8/4/2014 NS added */
INSERT [dbo].[MenuItems] ([ID], [DisplayText], [OrderNum], [ParentID], [PageLink], [Level], [RefName], [ImageURL], [MenuArea],[SessionNames] ,[TimerEnable]) VALUES (135, N'Microsoft Skype for Business', 8, 2, N'~/Configurator/LyncServersGrid.aspx', 2, N'MicrosoftLyncServer', N'~/images/icons/lync-icon.gif', N'Configurator', 'Submenu,UserPreferences,LyncUpdateStatus,UserID,RestrictedServers,LyncServer,UserID',0)
/* 8/14/2014 WS added */
INSERT [dbo].[MenuItems] ([ID], [DisplayText], [OrderNum], [ParentID], [PageLink], [Level], [RefName], [ImageURL], [MenuArea],[SessionNames] ,[TimerEnable]) VALUES (136, N'Send Log Files', 3, 126, N'~/Configurator/SendLogFiles.aspx', 2, N'SendLogFiles', N'~/images/icons/send.png', N'Configurator', 'LogFiles,UserLogin',0)
/* 3/11/2015 niranjan vsplus-1568*/
/*INSERT [dbo].[MenuItems] ([ID], [DisplayText], [OrderNum], [ParentID], [PageLink], [Level], [RefName], [ImageURL], [MenuArea],[SessionNames] ,[TimerEnable]) VALUES (137, N'Biggest Mail Files (Exchange )', 2, 101, N'~/Dashboard/BiggestExchangeMailFiles.aspx', 2, N'Biggest Ex Mail Files', N'~/images/icons/email.png', N'Dashboard', NULL,0)
INSERT [dbo].[MenuItems] ([ID], [DisplayText], [OrderNum], [ParentID], [PageLink], [Level], [RefName], [ImageURL], [MenuArea],[SessionNames] ,[TimerEnable]) VALUES (138, N'Alphabetical Order (Exchange)', 1, 101, N'~/Dashboard/ExchangeMailFiles.aspx', 2, N'Ex Mail Files', N'~/images/icons/email.png', N'Dashboard', NULL,0)*/
/* 5/9/2014 Mukund added Sharepoint pages */
INSERT [dbo].[MenuItems] ([ID], [DisplayText], [OrderNum], [ParentID], [PageLink], [Level], [RefName], [ImageURL], [MenuArea],[SessionNames] ,[TimerEnable]) VALUES (139, N'Microsoft SharePoint', 9, 2, NULL, 2, N'SharePoint', N'~/images/icons/SP16.png', N'Configurator', NULL,0)
/* 9/8/14 WS added Status History Page */
INSERT [dbo].[MenuItems] ([ID], [DisplayText], [OrderNum], [ParentID], [PageLink], [Level], [RefName], [ImageURL], [MenuArea], [SessionNames], [TimerEnable]) VALUES (140, N'Status Changes', 6, 73, N'~/Dashboard/StatusHistory.aspx', 2, N'Status History', N'~/images/icons/statuschange.png', N'Dashboard', N'UserPreferences,HistoryTable,HistoryTable,RestrictedServers,UserID', 1)
INSERT [dbo].[MenuItems] ([ID], [DisplayText], [OrderNum], [ParentID], [PageLink], [Level], [RefName], [ImageURL], [MenuArea], [SessionNames], [TimerEnable]) VALUES (141, N'Import Microsoft Servers', 6, 10, N'~/Security/ImportMicrosoftServers.aspx', 2, N'ImportExchangeServers', N'~/images/icons/Exchange.jpg', N'Configurator', N'importcredential,ImportedServers', 0)
/* 29Sep14 Mukund added */
INSERT [dbo].[MenuItems] ([ID], [DisplayText], [OrderNum], [ParentID], [PageLink], [Level], [RefName], [ImageURL], [MenuArea],[SessionNames] ,[TimerEnable])  values (142,N'Microsoft Windows', 10, 2,NULL,2,N'Windows',N'~/images/icons/windows.gif',N'Configurator',NULL,0)
/* 01Oct14 Mukund added */
INSERT [dbo].[MenuItems] ([ID], [DisplayText], [OrderNum], [ParentID], [PageLink], [Level], [RefName], [ImageURL], [MenuArea],[SessionNames] ,[TimerEnable])  values (143,N'Cloud Applications', 2, 2,N'~/Configurator/CloudApplicationsServerGrid.aspx',2,N'Cloud',N'~/images/icons/network-cloud-icon.png',N'Configurator','UserPreferences,CloudApplicationsServerUpdateStatus,RestrictedServers,CloudApplicationsServer,UserID',0)
/* 06Oct14 Mukund added */
INSERT [dbo].[MenuItems] ([ID], [DisplayText], [OrderNum], [ParentID], [PageLink], [Level], [RefName], [ImageURL], [MenuArea], [SessionNames], [TimerEnable]) VALUES (144, N'Servers', 1, 62, N'~/Configurator/MSServersGrid.aspx', 3, N'MicrosoftExchangeServer', N'~/images/icons/servers.png', N'Configurator', 'Submenu,UserPreferences,ExchangeUpdateStatus,UserID,RestrictedServers,ExchangeServer', 0)
INSERT [dbo].[MenuItems] ([ID], [DisplayText], [OrderNum], [ParentID], [PageLink], [Level], [RefName], [ImageURL], [MenuArea], [SessionNames], [TimerEnable]) VALUES (145, N'DAG', 2, 62, N'~/Configurator/DAGserverGrid.aspx', 3, N'MicrosoftExchangeDAG', N'~/images/icons/DAG-Health-Icon.png', N'Configurator', 'Submenu,UserPreferences,DAGUpdateStatus,RestrictedServers,ExchangeServer',0)
/* 07Oct14 Mukund added */
INSERT [dbo].[MenuItems] ([ID], [DisplayText], [OrderNum], [ParentID], [PageLink], [Level], [RefName], [ImageURL], [MenuArea], [SessionNames], [TimerEnable]) VALUES (146, N'Microsoft Active Directory', 6, 2, N'~/Configurator/ActiveDirectoryGrid.aspx', 2, N'Active Directory', N'~/images/icons/windows.gif', N'Configurator', 'Submenu,UserPreferences,WindowsUpdateStatus,UserID,RestrictedServers,WindowsServer', 0)
/* 13Oct14 Mukund added VSPlus-1004*/
/* 2/4/15 WS Commentted out since SNMP is not being used currently
INSERT [dbo].[MenuItems] ([ID], [DisplayText], [OrderNum], [ParentID], [PageLink], [Level], [RefName], [ImageURL], [MenuArea], [SessionNames], [TimerEnable]) VALUES (147, N'Network Devices', 1, 18, N'~/Configurator/NetworkDevicesGrid.aspx', 3, N'NetworkDevices', NULL, N'Configurator', NULL, 0)
INSERT [dbo].[MenuItems] ([ID], [DisplayText], [OrderNum], [ParentID], [PageLink], [Level], [RefName], [ImageURL], [MenuArea], [SessionNames], [TimerEnable]) VALUES (148, N'SNMP Devices', 2, 18, N'/Configurator/SNMPDevicesGrid.aspx', 3, N'SNMPDevices', NULL, N'Configurator', NULL, 0)
*/
INSERT [dbo].[MenuItems] ([ID], [DisplayText], [OrderNum], [ParentID], [PageLink], [Level], [RefName], [ImageURL], [MenuArea], [SessionNames], [TimerEnable]) VALUES (149, N'Active Directory Health', 1, 79, N'~/Dashboard/ActiveDirectoryHealth.aspx', 2, N'ActiveDirectory Server Health', N'~/images/icons/windows.gif', N'Dashboard', 'ADMembers', 1)
/* 04Nov14 Mouli Added, VSPlus-1141*/
INSERT [dbo].[MenuItems] ([ID], [DisplayText], [OrderNum], [ParentID], [PageLink], [Level], [RefName], [ImageURL], [MenuArea], [SessionNames], [TimerEnable]) VALUES (150, N'Change Passsword', 3, 110, N'~/Security/ChangePwd.aspx?M=d', 2, N'Change Password', NULL, N'Dashboard', 'UserPassword,UserID', 1)

/*somaraju 11/19/14*/
INSERT [dbo].[MenuItems] ([ID], [DisplayText], [OrderNum], [ParentID], [PageLink], [Level], [RefName], [ImageURL], [MenuArea],[SessionNames] ,[TimerEnable]) VALUES (151, N'SharePoint Server Health', 11, 79, N'~/Dashboard/SharepointServerHealth.aspx', 2, N'SharePoint Sever Health', N'~/images/icons/SP16.png', N'Dashboard', 'BackURL,UserPreferences,SharePointServerList,UserFullName,SharePointDatabaseList,SharePointFarmGrid,UserID,UserPreferences',1)
/* swathi VSPlus-1185 28/11/2014*/
INSERT [dbo].[MenuItems] ([ID], [DisplayText], [OrderNum], [ParentID], [PageLink], [Level], [RefName], [ImageURL], [MenuArea], [SessionNames], [TimerEnable]) VALUES (157, N'Exchange Heat Map', 10, 87, N'~/Dashboard/ExchangeHeatMap.aspx', 2, N'Exchange Heat Map', N'~/images/icons/exchange.jpg', N'Dashboard', 'sessiondata', 0)

/*somaraju,niranjan 2/12/14*/
INSERT [dbo].[MenuItems] ([ID], [DisplayText], [OrderNum], [ParentID], [PageLink], [Level], [RefName], [ImageURL], [MenuArea], [SessionNames], [TimerEnable]) VALUES (158, N'Message Latency Test', 4, 15, N'~/Configurator/MessageLatencyTest.aspx', 3, N'MessageLatencyTest', N'~/images/icons/latency.png', N'Configurator', 'Submenu,UserPreferences,ExchangeUpdateStatus,ExchangeServer', 0)
--Somaraju VSPLUS 2198
INSERT [dbo].[MenuItems] ([ID], [DisplayText], [OrderNum], [ParentID], [PageLink], [Level], [RefName], [ImageURL], [MenuArea],[SessionNames] ,[TimerEnable]) VALUES (159, N'Office 365 Health', 6, 79, N'~/Dashboard/Office365Health.aspx?Name=All', 2, N'Office365 Server Health', N'~/images/icons/O365.png', N'Dashboard', N'UserPreferences,0365Table,UserID,HealthAssessmentO365',0)
INSERT [dbo].[MenuItems] ([ID], [DisplayText], [OrderNum], [ParentID], [PageLink], [Level], [RefName], [ImageURL], [MenuArea],[SessionNames] ,[TimerEnable])  values (160,N'Microsoft Office 365', 8, 2,N'~/Configurator/O365ServerProperties.aspx',2,N'O365',N'~/images/icons/O365.png',N'Configurator','ID,UserPreferences,O365ServerUpdateStatus,NodeError,SeverName,Credentials,category,checked,Name,ScanInterval,ResponseThreshold,RetryInterval,SrvAtrFailBefAlertTextBox,OffScan,Uname,Pwd,ReturnUrl,UserID,UserPreferences
,Tests,Nodes,endindex',0)
/* 12/18/2014 NS added for VSPLUS-1229*/
INSERT [dbo].[MenuItems] ([ID], [DisplayText], [OrderNum], [ParentID], [PageLink], [Level], [RefName], [ImageURL], [MenuArea],[SessionNames] ,[TimerEnable])  values (161,N'Script Definitions', 2, 3,N'~/Configurator/ScriptDefGrid.aspx',2,N'ScriptDefGrid',N'~/images/icons/script.png',N'Configurator','CustomScriptsTable',0)
--WS Added
INSERT [dbo].[MenuItems] ([ID], [DisplayText], [OrderNum], [ParentID], [PageLink], [Level], [RefName], [ImageURL], [MenuArea], [SessionNames], [TimerEnable]) VALUES (163, N'Servers', 1, 139, N'~/Configurator/SharePointGrid.aspx', 3, N'SharePointServers', N'~/images/icons/SP16.png', N'Configurator', 'Submenu,UserPreferences,ExchangeUpdateStatus,UserID,RestrictedServers,ExchangeServer', 0)
INSERT [dbo].[MenuItems] ([ID], [DisplayText], [OrderNum], [ParentID], [PageLink], [Level], [RefName], [ImageURL], [MenuArea], [SessionNames], [TimerEnable]) VALUES (164, N'Farms', 2, 139, N'~/Configurator/SharePointFarmGrid.aspx', 3, N'SharePointServers', N'~/images/icons/SP16.png', N'Configurator', 'Submenu,ExchangeUpdateStatus,SPFarmGrid,UserID', 0)

INSERT [dbo].[MenuItems] ([ID], [DisplayText], [OrderNum], [ParentID], [PageLink], [Level], [RefName], [ImageURL], [MenuArea], [SessionNames], [TimerEnable]) VALUES (165, N'Manage Profile', 5, 5, N'~/Configurator/ManageProfiles.aspx', 2, N'Manage Profile', N'~/images/icons/profile.png', N'Configurator', 'Profiles,ProfileId,DominoSession,SametimeSession,ExchangeSession,URLSession,ActiveDirectorySession,UserPreferences,ProfileName,UserID,UserPreferences', 0)
/* 1/16/15 WS ADDED FOR MAP*/
INSERT [dbo].[MenuItems] ([ID], [DisplayText], [OrderNum], [ParentID], [PageLink], [Level], [RefName], [ImageURL], [MenuArea], [SessionNames], [TimerEnable]) VALUES (166, N'Health Map', 7, 73, N'~/Dashboard/MSMap.aspx', 2, N'Health Map', N'~/images/icons/map2.png', N'Dashboard', N'UserLogin', 1)
/* 1/21/2015 NS added for VSPLUS-1378 */
INSERT [dbo].[MenuItems] ([ID], [DisplayText], [OrderNum], [ParentID], [PageLink], [Level], [RefName], [ImageURL], [MenuArea], [SessionNames], [TimerEnable]) VALUES (167, N'IBM Connections Health', 12, 79, N'~/Dashboard/ConnectionsHealth.aspx', 2, N'ConnectionsHealth', N'~/images/icons/ibm.png', N'Dashboard', 'ConnectionsTests', 0)
/*1/23/15 WS ADDED */
INSERT [dbo].[MenuItems] ([ID], [DisplayText], [OrderNum], [ParentID], [PageLink], [Level], [RefName], [ImageURL], [MenuArea], [SessionNames], [TimerEnable]) VALUES (170, N'Network Latency Test', 13, 87, N'~/Dashboard/LatencyTest.aspx', 2, N'LatencyTest', N'~/images/icons/exchange.jpg', N'Dashboard', 'sessiondata,NetworkLatencyID', 0)

/*Mukund 24Jan15 added*/
INSERT [dbo].[MenuItems] ([ID], [DisplayText], [OrderNum], [ParentID], [PageLink], [Level], [RefName], [ImageURL], [MenuArea], [SessionNames], [TimerEnable]) VALUES (171, N'Servers', 1, 168, N'~/Configurator/WebSphereSeverGrid.aspx', 3, N'ManageWebsphereServers', N'~/images/icons/servers.png', N'Configurator', 'Submenu,UserPreferences,WebSphereUpdateStatus,UserID,RestrictedServers,WebSphereServer', 0)
/* swathi 1285*/
INSERT [dbo].[MenuItems] ([ID], [DisplayText], [OrderNum], [ParentID], [PageLink], [Level], [RefName], [ImageURL], [MenuArea], [SessionNames], [TimerEnable]) VALUES (172, N'Servers', 1, 142, N'~/Configurator/WindowsServerGrid.aspx', 3, N'Windows Servers', N'~/images/icons/servers.png', N'Configurator', 'Submenu,UserPreferences,WindowsUpdateStatus,UserID,RestrictedServers,WindowsServer', 0)
INSERT [dbo].[MenuItems] ([ID], [DisplayText], [OrderNum], [ParentID], [PageLink], [Level], [RefName], [ImageURL], [MenuArea], [SessionNames], [TimerEnable]) VALUES (173, N'Server Network Latency', 2, 142, N'~/Configurator/NetworkLatencyServers.aspx', 3, N'Windows Server Network Latency', N'~/images/icons/latency.png', N'Configurator', 'UserPreferences,nwlatencyUpdateStatus,networklatencytest,UserID,UserPreferences', 0)
INSERT [dbo].[MenuItems] ([ID], [DisplayText], [OrderNum], [ParentID], [PageLink], [Level], [RefName], [ImageURL], [MenuArea], [SessionNames], [TimerEnable]) VALUES (174, N'Applications', 4, 168, N'~/Security/Applications.aspx', 3, N'ManageWebsphereServers', N'~/images/icons/application_add.png', N'Configurator', 'UserPreferences,WindowsServices,servername,UserID', 0)
INSERT [dbo].[MenuItems] ([ID], [DisplayText], [OrderNum], [ParentID], [PageLink], [Level], [RefName], [ImageURL], [MenuArea], [SessionNames], [TimerEnable]) VALUES (175, N'Import WebSphere Servers', 7, 10, N'~/Security/WebsphereCellGrid.aspx', 2, N'Import WebSphere Servers', N'~/images/icons/ibm.png', N'Configurator', 'DataEvents123,UserPreferences,Treeviewvisibility,WebsphereCellNameTable,webcellid,DataEvents,NodesServers,UserID,UserPreferences,Treeviewvisibility', 0)
/* 3/30/2015 NS added for VSPLUS-1259 */
/*INSERT [dbo].[MenuItems] ([ID], [DisplayText], [OrderNum], [ParentID], [PageLink], [Level], [RefName], [ImageURL], [MenuArea], [SessionNames], [TimerEnable]) VALUES (177, N'Clusters', 1, 12, N'~/Configurator/ClusterGrid.aspx', 3, N'DominoClusters', NULL, N'Configurator', NULL, 0)
INSERT [dbo].[MenuItems] ([ID], [DisplayText], [OrderNum], [ParentID], [PageLink], [Level], [RefName], [ImageURL], [MenuArea], [SessionNames], [TimerEnable]) VALUES (178, N'Notes Databases', 2, 12, N'~/Configurator/NotesDatabase.aspx', 3, N'NotesDatabases', NULL, N'Configurator', NULL, 0)
INSERT [dbo].[MenuItems] ([ID], [DisplayText], [OrderNum], [ParentID], [PageLink], [Level], [RefName], [ImageURL], [MenuArea], [SessionNames], [TimerEnable]) VALUES (179, N'Custom Statistics', 3, 12, N'~/Configurator/CustomStatistics.aspx', 3, N'CustomStatistics', NULL, N'Configurator', NULL, 0)
INSERT [dbo].[MenuItems] ([ID], [DisplayText], [OrderNum], [ParentID], [PageLink], [Level], [RefName], [ImageURL], [MenuArea], [SessionNames], [TimerEnable]) VALUES (180, N'Server Task Definitions', 4, 12, N'~/Configurator/ServerTaskDefinitions.aspx', 3, N'ServerTaskDefinitions', NULL, N'Configurator', NULL, 0)
INSERT [dbo].[MenuItems] ([ID], [DisplayText], [OrderNum], [ParentID], [PageLink], [Level], [RefName], [ImageURL], [MenuArea], [SessionNames], [TimerEnable]) VALUES (181, N'Log File Scanning', 5, 12, N'~/Configurator/LogFileScanning.aspx', 3, N'LogFileScanning', NULL, N'Configurator', NULL, 0)
INSERT [dbo].[MenuItems] ([ID], [DisplayText], [OrderNum], [ParentID], [PageLink], [Level], [RefName], [ImageURL], [MenuArea], [SessionNames], [TimerEnable]) VALUES (182, N'Servers', 6, 12, N'~/Configurator/LotusDominoServers.aspx', 3, N'DominoServers', NULL, N'Configurator', NULL, 0)
INSERT [dbo].[MenuItems] ([ID], [DisplayText], [OrderNum], [ParentID], [PageLink], [Level], [RefName], [ImageURL], [MenuArea], [SessionNames], [TimerEnable]) VALUES (183, N'Manage Servers', 1, 168, N'~/Configurator/WebSphereSeverGrid.aspx', 2, N'WebsphereServers', NULL, N'Configurator', NULL, 0)
INSERT [dbo].[MenuItems] ([ID], [DisplayText], [OrderNum], [ParentID], [PageLink], [Level], [RefName], [ImageURL], [MenuArea], [SessionNames], [TimerEnable]) VALUES (184, N'Applications', 2, 168, N'~/Security/Applications.aspx', 2, N'WebsphereApplications', NULL, N'Configurator', NULL, 0)
INSERT [dbo].[MenuItems] ([ID], [DisplayText], [OrderNum], [ParentID], [PageLink], [Level], [RefName], [ImageURL], [MenuArea], [SessionNames], [TimerEnable]) VALUES (185, N'NotesMail Probes', 1, 15, N'~/Configurator/NotesMailprobeGrid.aspx', 2, N'NotesMailProbes', NULL, N'Configurator', NULL, 0)
INSERT [dbo].[MenuItems] ([ID], [DisplayText], [OrderNum], [ParentID], [PageLink], [Level], [RefName], [ImageURL], [MenuArea], [SessionNames], [TimerEnable]) VALUES (186, N'ExchangeMail Probes', 2, 15, N'~/Configurator/ExchangeMailProbe.aspx', 2, N'ExchangeMailProbes', N'~/images/icons/exchangemail.jpg', N'Configurator', NULL, 0)*/
/*
INSERT [dbo].[MenuItems] ([ID], [DisplayText], [OrderNum], [ParentID], [PageLink], [Level], [RefName], [ImageURL], [MenuArea], [SessionNames], [TimerEnable]) VALUES (187, N'Mail Services', 3, 15, N'~/Configurator/MailServicesGrid.aspx', 2, N'MailServices', NULL, N'Configurator', NULL, 0)
INSERT [dbo].[MenuItems] ([ID], [DisplayText], [OrderNum], [ParentID], [PageLink], [Level], [RefName], [ImageURL], [MenuArea], [SessionNames], [TimerEnable]) VALUES (189, N'Servers', 1, 139, N'~/Configurator/SharePointGrid.aspx', 2, N'SharepointServers', NULL, N'Configurator', NULL, 0)
INSERT [dbo].[MenuItems] ([ID], [DisplayText], [OrderNum], [ParentID], [PageLink], [Level], [RefName], [ImageURL], [MenuArea], [SessionNames], [TimerEnable]) VALUES (190, N'Farms', 2, 139, N'~/Configurator/SharePointFarmGrid.aspx', 2, N'SharepointFarms', NULL, N'Configurator', NULL, 0)
INSERT [dbo].[MenuItems] ([ID], [DisplayText], [OrderNum], [ParentID], [PageLink], [Level], [RefName], [ImageURL], [MenuArea], [SessionNames], [TimerEnable]) VALUES (191, N'Servers', 1, 142, N'~/Configurator/WindowsServerGrid.aspx', 2, N'WindowsServers', NULL, N'Configurator', NULL, 0)
INSERT [dbo].[MenuItems] ([ID], [DisplayText], [OrderNum], [ParentID], [PageLink], [Level], [RefName], [ImageURL], [MenuArea], [SessionNames], [TimerEnable]) VALUES (192, N'Server Network Latency', 2, 142, N'~/Configurator/NetworkLatencyServers.aspx', 2, N'WindowsServersNetworkLatency', NULL, N'Configurator', NULL, 0)*/
/* 4/17/2015 NS modified for VSPLUS-1650 */
/* 4/30/2015 Niranjan modified for VSPLUS-1711 */
INSERT [dbo].[MenuItems] ([ID], [DisplayText], [OrderNum], [ParentID], [PageLink], [Level], [RefName], [ImageURL], [MenuArea], [SessionNames], [TimerEnable]) VALUES (194, N'High Availability Settings/Status', 11, 10, N'~/Security/AssignServerToNode.aspx', 2, N'HighAvailability', N'~/images/icons/sitemap_color.png', N'Configurator', 'AssignNodes,AssignNodeServer,RestrictedServers,Servicesgrid,UserID', 0)
/*
INSERT [dbo].[MenuItems] ([ID], [DisplayText], [OrderNum], [ParentID], [PageLink], [Level], [RefName], [ImageURL], [MenuArea], [SessionNames], [TimerEnable]) VALUES (193, N'Assign Server to Node Access', 1, 194, N'~/Security/AssignServerToNode.aspx', 3, N'Assign Sever to Node Acess', NULL, N'Configurator', NULL, 0)
INSERT [dbo].[MenuItems] ([ID], [DisplayText], [OrderNum], [ParentID], [PageLink], [Level], [RefName], [ImageURL], [MenuArea], [SessionNames], [TimerEnable]) VALUES (195, N'Node Health', 2, 194, N'~/Configurator/NodeHealth.aspx', 3, N'Node Health', NULL, N'Configurator', NULL, 0)
*/
/* 5/7/2015 NS added for VSPLUS-1646 */
INSERT [dbo].[MenuItems] ([ID], [DisplayText], [OrderNum], [ParentID], [PageLink], [Level], [RefName], [ImageURL], [MenuArea], [SessionNames], [TimerEnable]) VALUES (196, N'Server Status Summary', 7, 73, N'~/Dashboard/ExecutiveSummaryStacked.aspx', 2, N'ServerStatusSummary', N'~/images/icons/table.png', N'Dashboard', NULL, 1)
--VSPLUS 1360 SWATHI
/* 1/7/2016 NS modified for VSPLUS-1271 */
INSERT [dbo].[MenuItems] ([ID], [DisplayText], [OrderNum], [ParentID], [PageLink], [Level], [RefName], [ImageURL], [MenuArea], [SessionNames], [TimerEnable]) VALUES (197, N'Overall Docking', 10, 73, N'~/Dashboard/OverallHealth3.aspx', 2, N'Overall Health Docking', N'~/images/icons/sitemap_color.png', N'Dashboard', 'UserLogin,CustomBackground,FilterByValue,ViewBy,FilterLabel,checkeddata,MyServerTypes,MyServerLocations,MyCategory
,UserID,UserPreferences,FilterBy,Isconfigurator,UserFullName,KeyMobileDevicesGrid,DockingLayout,GridData', 1)
/* 7/22/2015 NS added for VSPLUS-1911 */
INSERT [dbo].[MenuItems] ([ID], [DisplayText], [OrderNum], [ParentID], [PageLink], [Level], [RefName], [ImageURL], [MenuArea],[SessionNames] ,[TimerEnable]) VALUES (198, N'Database Replication Health', 4, 97, N'~/Dashboard/DBReplicationHealth.aspx', 2, N'Database Replication Health', N'~/images/icons/databases.png', N'Dashboard', 'UserPreferences,PotentialClusterIssues,UserID,UserPreferences',0)
--somaraju vsplus 1999
INSERT [dbo].[MenuItems] ([ID], [DisplayText], [OrderNum], [ParentID], [PageLink], [Level], [RefName], [ImageURL], [MenuArea], [SessionNames], [TimerEnable], [OverrideSort]) VALUES (199, N'View Alerts', 3, 3, N'~/Dashboard/OverallServerAlerts.aspx', 2, N'View Alerts', N'~/images/icons/script.png', N'Configurator', 'UserPreferences,UserID', 0, 9)
/* 8/13/2015 NS added for VSPLUS-2029 */
INSERT [dbo].[MenuItems] ([ID], [DisplayText], [OrderNum], [ParentID], [PageLink], [Level], [RefName], [ImageURL], [MenuArea],[SessionNames] ,[TimerEnable]) VALUES (200, N'Key User Devices', 1, 87, N'~/Dashboard/KeyMobileUsers.aspx', 2, N'Key Mobile Users', N'~/images/icons/group.png', N'Dashboard', 'UserPreferences,KeyMobileDevicesGrid',1)
--VSPLUS Somaraju 1669
INSERT [dbo].[MenuItems] ([ID], [DisplayText], [OrderNum], [ParentID], [PageLink], [Level], [RefName], [ImageURL], [MenuArea], [SessionNames], [TimerEnable], [OverrideSort]) VALUES (201, N'Event Templates', 4, 3, N'/Configurator/AlertEventTemplate.aspx', 2, N'Event Templates', N'/images/icons/transmit_blue.png', N'Configurator', 'eventtemplatename,Eventtemplate', 0, 9)
--VSPLUS-2187 SWATHI
INSERT [dbo].[MenuItems] ([ID], [DisplayText], [OrderNum], [ParentID], [PageLink], [Level], [RefName], [ImageURL], [MenuArea], [SessionNames], [TimerEnable], [OverrideSort]) VALUES (202, N'Cloud Health', 13, 79, N'~/Dashboard/CloudHealthPage.aspx', 2, N'Cloud  Health', N'~/images/icons/network-cloud-icon.png', N'Dashboard', N'cloudServerList,cloudIssues,MailStatus,BackURL,UserPreferences,UserID,UserPreferences', 0, 9)
INSERT [dbo].[MenuItems] ([ID], [DisplayText], [OrderNum], [ParentID], [PageLink], [Level], [RefName], [ImageURL], [MenuArea], [SessionNames], [TimerEnable]) VALUES (203, N'Event Log Scanning', 3, 142, N'~/Configurator/ELSDefinitions_Grid.aspx', 3, N'Event Log Scanning', N'~/images/icons/windows.gif', N'Configurator', 'UserPreferences,DELSNameTable,UserID', 0)

INSERT [dbo].[MenuItems] ([ID], [DisplayText], [OrderNum], [ParentID], [PageLink], [Level], [RefName], [ImageURL], [MenuArea],[SessionNames] ,[TimerEnable]) VALUES (204, N'Windows Event History', 14, 79, N'~/Dashboard/OverallServerEvents.aspx', 2, N'Windows Event History', N'~/images/icons/error.png', N'Dashboard', 'UserPreferences,UserID',0)
--VSPLUS 2488 Durga
INSERT [dbo].[MenuItems] ([ID], [DisplayText], [OrderNum], [ParentID], [PageLink], [Level], [RefName], [ImageURL], [MenuArea],[SessionNames] ,[TimerEnable]) VALUES (205, N'Top Mail Users', 2, 101, N'~/Dashboard/TopMailUsers.aspx', 2, N'Top Mail Users', N'~/images/icons/email.png', N'Dashboard', NULL,0)
--VSPLUS-2519 Sowjanya
INSERT [dbo].[MenuItems] ([ID], [DisplayText], [OrderNum], [ParentID], [PageLink], [Level], [RefName], [ImageURL], [MenuArea], [SessionNames], [TimerEnable], [OverrideSort]) VALUES (206, N'Help', 13, NULL, NULL, 1, N'Help', N'~\images\information.png', N'Configurator', NULL, 1, 9)
INSERT [dbo].[MenuItems] ([ID], [DisplayText], [OrderNum], [ParentID], [PageLink], [Level], [RefName], [ImageURL], [MenuArea], [SessionNames], [TimerEnable], [OverrideSort]) VALUES (207, N'Documentation', 0, 206, N'https://rprvitalsigns.atlassian.net/wiki/display/VUG/Installation+Guide', 2, N'Documentation', N'~/images\icons\logfile.png', N'Configurator', NULL, 1, 6)
INSERT [dbo].[MenuItems] ([ID], [DisplayText], [OrderNum], [ParentID], [PageLink], [Level], [RefName], [ImageURL], [MenuArea], [SessionNames], [TimerEnable], [OverrideSort]) VALUES (208, N'Support Portal', 1, 206, N'http://support.rprvitalsigns.com/hc/en-us', 2, N'Support Portal', N'~/images\icons\support.png', N'Configurator', NULL, 0, 7)
INSERT [dbo].[MenuItems] ([ID], [DisplayText], [OrderNum], [ParentID], [PageLink], [Level], [RefName], [ImageURL], [MenuArea], [SessionNames], [TimerEnable], [OverrideSort]) VALUES (209, N'Assembly Information', 2, 206, N'~/Configurator/GetAssemblyInfo.aspx', 2, N'Assembly Information', N'~/images\icons\wrench.png', N'Configurator', NULL, 0, 8)
INSERT [dbo].[MenuItems] ([ID], [DisplayText], [OrderNum], [ParentID], [PageLink], [Level], [RefName], [ImageURL], [MenuArea], [SessionNames], [TimerEnable], [OverrideSort]) VALUES (210, N'Credits', 3, 206, N'~/CreditsPage.aspx', 2, N'Credits', N'~/images\icons\profile.png', N'Configurator', NULL, 0, 9)
/* 2/15/2016 NS added for VSPLUS-2588*/
INSERT [dbo].[MenuItems] ([ID], [DisplayText], [OrderNum], [ParentID], [PageLink], [Level], [RefName], [ImageURL], [MenuArea], [SessionNames], [TimerEnable]) VALUES (211, N'All Reports', 1, 8, N'~/DashboardReports/OverallSrvStatusHealthRpt.aspx?M=d', 2, N'AllReports', N'~/images/icons/chart_pie.png', N'Configurator', NULL, 0)
INSERT [dbo].[MenuItems] ([ID], [DisplayText], [OrderNum], [ParentID], [PageLink], [Level], [RefName], [ImageURL], [MenuArea], [SessionNames], [TimerEnable]) VALUES (212, N'Scheduled Reports', 2, 8, N'~/Configurator/Reports.aspx', 2, N'ScheduledReports', N'~/images/icons/sched.gif', N'Configurator', NULL, 0)
--3/11/2016 sowjanya Modified for VSPLUS-2650
INSERT [dbo].[MenuItems] ([ID], [DisplayText], [OrderNum], [ParentID], [PageLink], [Level], [RefName], [ImageURL], [MenuArea], [SessionNames], [TimerEnable], [OverrideSort]) VALUES (213, N'IBM Connections', 3, 2, NULL, 2, N'LotusDomino', N'~/images/icons/dominoserver.gif', N'Configurator', NULL, 0, 9)
INSERT [dbo].[MenuItems] ([ID], [DisplayText], [OrderNum], [ParentID], [PageLink], [Level], [RefName], [ImageURL], [MenuArea], [SessionNames], [TimerEnable], [OverrideSort]) VALUES (214, N'Servers', 1, 213, N'~/Configurator/IBMConnectionsGrid.aspx', 3, N'IBMConnectionsServers', N'~/images/icons/servers.png', N'Configurator', N'', 0, 9)
--3/14/2016 Somaraju Addded for VSPLUS-2694,2697
/* 3/23/2016 NS modified for VSPLUS-2711 */
INSERT [dbo].[MenuItems] ([ID], [DisplayText], [OrderNum], [ParentID], [PageLink], [Level], [RefName], [ImageURL], [MenuArea], [SessionNames], [TimerEnable], [OverrideSort]) VALUES (215, N'Office 365 Groups', 10, 87, N'~/Dashboard/Office365Groups.aspx', 2, N'O365 Groups', N'~/images/icons/O365.png', N'Dashboard', N'UserPreferences,GridData,rowIndex,ServerName,UserID', 0, 9)
--Somaraju VSPLUS 2714
/* 3/23/2016 NS commented out - User Password Settings has been merged into the Office 365 health page */
/* INSERT [dbo].[MenuItems] ([ID], [DisplayText], [OrderNum], [ParentID], [PageLink], [Level], [RefName], [ImageURL], [MenuArea], [SessionNames], [TimerEnable], [OverrideSort]) VALUES (216, N'User Password Settings', 11, 87, N'~/Dashboard/UserPasswordSettings.aspx', 2, N'User Password Settings', N'~/images/icons/group.png', N'Dashboard', N'UserPreferences,GridData,rowIndex,ServerName,UserID', 0, 9) */
--Durga VSPLUS-2696
INSERT [dbo].[MenuItems] ([ID], [DisplayText], [OrderNum], [ParentID], [PageLink], [Level], [RefName], [ImageURL], [MenuArea],[SessionNames] ,[TimerEnable]) VALUES (217, N'Cost Per User Served', 3, 87, N'~/Dashboard/CostperUserserved.aspx', 2, N'Cost per User served', N'~/images/icons/money.png', N'Dashboard', N'',9)
--Durga VSPLUS-2866
INSERT [dbo].[MenuItems] ([ID], [DisplayText], [OrderNum], [ParentID], [PageLink], [Level], [RefName], [ImageURL], [MenuArea],[SessionNames] ,[TimerEnable]) VALUES (218, N'Financial Overview', 3, 87, N'~/Dashboard/FinancialOverview.aspx', 2, N'Financial Overview', N'~/images/icons/financial.png', N'Dashboard', N'',9)

--Somaraju     VSPLUS-2613
INSERT [dbo].[MenuItems] ([ID], [DisplayText], [OrderNum], [ParentID], [PageLink], [Level], [RefName], [ImageURL], [MenuArea],[SessionNames] ,[TimerEnable]) VALUES (219, N'Office 365 User Licenses with Services', 11, 87, N'/Dashboard/O365UserLicenseswithServices.aspx', 2, N'Office365UserLiceenses ', N'~/images/icons/O365.png', N'Dashboard', N'',9)
/* 5/31/2016 NS added for VSPLUS-2941 */
INSERT [dbo].[MenuItems] ([ID], [DisplayText], [OrderNum], [ParentID], [PageLink], [Level], [RefName], [ImageURL], [MenuArea],[SessionNames] ,[TimerEnable]) VALUES (220, N'CPU/Memory Health', 16, 87, N'~/Dashboard/CPUMemoryHealth.aspx', 2, N'CPU/Memory Health', N'~/images/icons/cpu.png', N'Dashboard', N'',9)
SET IDENTITY_INSERT [dbo].[MenuItems] OFF
GO



/* 5/9/2014 NS added */
/* 7/10/2014 NS modified for VSPLUS-318 */
USE [vitalsigns]
GO
DELETE FROM FeatureMenus
GO
INSERT [dbo].[FeatureMenus] ([FeatureID], [MenuID]) VALUES (1, 12)
INSERT [dbo].[FeatureMenus] ([FeatureID], [MenuID]) VALUES (1, 2)
INSERT [dbo].[FeatureMenus] ([FeatureID], [MenuID]) VALUES (1, 15)
INSERT [dbo].[FeatureMenus] ([FeatureID], [MenuID]) VALUES (1, 23)
INSERT [dbo].[FeatureMenus] ([FeatureID], [MenuID]) VALUES (1, 5)
INSERT [dbo].[FeatureMenus] ([FeatureID], [MenuID]) VALUES (1, 58)
INSERT [dbo].[FeatureMenus] ([FeatureID], [MenuID]) VALUES (1, 10)
INSERT [dbo].[FeatureMenus] ([FeatureID], [MenuID]) VALUES (1, 80)
INSERT [dbo].[FeatureMenus] ([FeatureID], [MenuID]) VALUES (1, 79)
INSERT [dbo].[FeatureMenus] ([FeatureID], [MenuID]) VALUES (1, 81)
INSERT [dbo].[FeatureMenus] ([FeatureID], [MenuID]) VALUES (1, 85)
INSERT [dbo].[FeatureMenus] ([FeatureID], [MenuID]) VALUES (1, 86)
/* 8/4/2014 NS modified - Disk Health is a common feature */
INSERT [dbo].[FeatureMenus] ([FeatureID], [MenuID]) VALUES (11, 89)
INSERT [dbo].[FeatureMenus] ([FeatureID], [MenuID]) VALUES (1, 87)
INSERT [dbo].[FeatureMenus] ([FeatureID], [MenuID]) VALUES (1, 91)
INSERT [dbo].[FeatureMenus] ([FeatureID], [MenuID]) VALUES (1, 97)
INSERT [dbo].[FeatureMenus] ([FeatureID], [MenuID]) VALUES (1, 98)
INSERT [dbo].[FeatureMenus] ([FeatureID], [MenuID]) VALUES (1, 99)
INSERT [dbo].[FeatureMenus] ([FeatureID], [MenuID]) VALUES (1, 100)
INSERT [dbo].[FeatureMenus] ([FeatureID], [MenuID]) VALUES (9, 63)
INSERT [dbo].[FeatureMenus] ([FeatureID], [MenuID]) VALUES (9, 2)
INSERT [dbo].[FeatureMenus] ([FeatureID], [MenuID]) VALUES (9, 60)
INSERT [dbo].[FeatureMenus] ([FeatureID], [MenuID]) VALUES (9, 5)
INSERT [dbo].[FeatureMenus] ([FeatureID], [MenuID]) VALUES (2, 62)
INSERT [dbo].[FeatureMenus] ([FeatureID], [MenuID]) VALUES (2, 2)
INSERT [dbo].[FeatureMenus] ([FeatureID], [MenuID]) VALUES (11, 25)
INSERT [dbo].[FeatureMenus] ([FeatureID], [MenuID]) VALUES (2, 5)
INSERT [dbo].[FeatureMenus] ([FeatureID], [MenuID]) VALUES (2, 84)
INSERT [dbo].[FeatureMenus] ([FeatureID], [MenuID]) VALUES (2, 79)
INSERT [dbo].[FeatureMenus] ([FeatureID], [MenuID]) VALUES (3, 16)
INSERT [dbo].[FeatureMenus] ([FeatureID], [MenuID]) VALUES (3, 2)
INSERT [dbo].[FeatureMenus] ([FeatureID], [MenuID]) VALUES (4, 12)
INSERT [dbo].[FeatureMenus] ([FeatureID], [MenuID]) VALUES (4, 2)
INSERT [dbo].[FeatureMenus] ([FeatureID], [MenuID]) VALUES (4, 15)
INSERT [dbo].[FeatureMenus] ([FeatureID], [MenuID]) VALUES (4, 23)
INSERT [dbo].[FeatureMenus] ([FeatureID], [MenuID]) VALUES (4, 5)
INSERT [dbo].[FeatureMenus] ([FeatureID], [MenuID]) VALUES (4, 60)
INSERT [dbo].[FeatureMenus] ([FeatureID], [MenuID]) VALUES (4, 101)
INSERT [dbo].[FeatureMenus] ([FeatureID], [MenuID]) VALUES (4, 102)
INSERT [dbo].[FeatureMenus] ([FeatureID], [MenuID]) VALUES (4, 103)
INSERT [dbo].[FeatureMenus] ([FeatureID], [MenuID]) VALUES (4, 104)
INSERT [dbo].[FeatureMenus] ([FeatureID], [MenuID]) VALUES (4, 105)
INSERT [dbo].[FeatureMenus] ([FeatureID], [MenuID]) VALUES (4, 106)
INSERT [dbo].[FeatureMenus] ([FeatureID], [MenuID]) VALUES (4, 107)
INSERT [dbo].[FeatureMenus] ([FeatureID], [MenuID]) VALUES (6, 13)
INSERT [dbo].[FeatureMenus] ([FeatureID], [MenuID]) VALUES (6, 2)
INSERT [dbo].[FeatureMenus] ([FeatureID], [MenuID]) VALUES (6, 24)
INSERT [dbo].[FeatureMenus] ([FeatureID], [MenuID]) VALUES (6, 5)
INSERT [dbo].[FeatureMenus] ([FeatureID], [MenuID]) VALUES (6, 83)
INSERT [dbo].[FeatureMenus] ([FeatureID], [MenuID]) VALUES (6, 79)
INSERT [dbo].[FeatureMenus] ([FeatureID], [MenuID]) VALUES (8, 17)
INSERT [dbo].[FeatureMenus] ([FeatureID], [MenuID]) VALUES (8, 2)
INSERT [dbo].[FeatureMenus] ([FeatureID], [MenuID]) VALUES (8, 92)
INSERT [dbo].[FeatureMenus] ([FeatureID], [MenuID]) VALUES (8, 87)
INSERT [dbo].[FeatureMenus] ([FeatureID], [MenuID]) VALUES (10, 18)
INSERT [dbo].[FeatureMenus] ([FeatureID], [MenuID]) VALUES (10, 2)
INSERT [dbo].[FeatureMenus] ([FeatureID], [MenuID]) VALUES (12, 15)
INSERT [dbo].[FeatureMenus] ([FeatureID], [MenuID]) VALUES (12, 2)
INSERT [dbo].[FeatureMenus] ([FeatureID], [MenuID]) VALUES (12, 90)
INSERT [dbo].[FeatureMenus] ([FeatureID], [MenuID]) VALUES (12, 87)
INSERT [dbo].[FeatureMenus] ([FeatureID], [MenuID]) VALUES (9, 88)
INSERT [dbo].[FeatureMenus] ([FeatureID], [MenuID]) VALUES (9, 87)
INSERT [dbo].[FeatureMenus] ([FeatureID], [MenuID]) VALUES (11, 1)
INSERT [dbo].[FeatureMenus] ([FeatureID], [MenuID]) VALUES (11, 59)
INSERT [dbo].[FeatureMenus] ([FeatureID], [MenuID]) VALUES (11, 2)
INSERT [dbo].[FeatureMenus] ([FeatureID], [MenuID]) VALUES (11, 3)
INSERT [dbo].[FeatureMenus] ([FeatureID], [MenuID]) VALUES (11, 19)
INSERT [dbo].[FeatureMenus] ([FeatureID], [MenuID]) VALUES (11, 20)
INSERT [dbo].[FeatureMenus] ([FeatureID], [MenuID]) VALUES (11, 4)
INSERT [dbo].[FeatureMenus] ([FeatureID], [MenuID]) VALUES (11, 56)
INSERT [dbo].[FeatureMenus] ([FeatureID], [MenuID]) VALUES (11, 55)
INSERT [dbo].[FeatureMenus] ([FeatureID], [MenuID]) VALUES (11, 26)
INSERT [dbo].[FeatureMenus] ([FeatureID], [MenuID]) VALUES (11, 5)
INSERT [dbo].[FeatureMenus] ([FeatureID], [MenuID]) VALUES (11, 52)
INSERT [dbo].[FeatureMenus] ([FeatureID], [MenuID]) VALUES (11, 6)
INSERT [dbo].[FeatureMenus] ([FeatureID], [MenuID]) VALUES (11, 8)
INSERT [dbo].[FeatureMenus] ([FeatureID], [MenuID]) VALUES (11, 9)
INSERT [dbo].[FeatureMenus] ([FeatureID], [MenuID]) VALUES (11, 27)
INSERT [dbo].[FeatureMenus] ([FeatureID], [MenuID]) VALUES (11, 10)
INSERT [dbo].[FeatureMenus] ([FeatureID], [MenuID]) VALUES (11, 28)
INSERT [dbo].[FeatureMenus] ([FeatureID], [MenuID]) VALUES (11, 29)
INSERT [dbo].[FeatureMenus] ([FeatureID], [MenuID]) VALUES (11, 30)
INSERT [dbo].[FeatureMenus] ([FeatureID], [MenuID]) VALUES (11, 31)
INSERT [dbo].[FeatureMenus] ([FeatureID], [MenuID]) VALUES (11, 57)
INSERT [dbo].[FeatureMenus] ([FeatureID], [MenuID]) VALUES (11, 125)
INSERT [dbo].[FeatureMenus] ([FeatureID], [MenuID]) VALUES (11, 11)
INSERT [dbo].[FeatureMenus] ([FeatureID], [MenuID]) VALUES (11, 51)
INSERT [dbo].[FeatureMenus] ([FeatureID], [MenuID]) VALUES (11, 53)
INSERT [dbo].[FeatureMenus] ([FeatureID], [MenuID]) VALUES (11, 73)
INSERT [dbo].[FeatureMenus] ([FeatureID], [MenuID]) VALUES (11, 74)
INSERT [dbo].[FeatureMenus] ([FeatureID], [MenuID]) VALUES (11, 75)
INSERT [dbo].[FeatureMenus] ([FeatureID], [MenuID]) VALUES (11, 76)
INSERT [dbo].[FeatureMenus] ([FeatureID], [MenuID]) VALUES (11, 77)
INSERT [dbo].[FeatureMenus] ([FeatureID], [MenuID]) VALUES (11, 78)
INSERT [dbo].[FeatureMenus] ([FeatureID], [MenuID]) VALUES (11, 86)
INSERT [dbo].[FeatureMenus] ([FeatureID], [MenuID]) VALUES (11, 79)
INSERT [dbo].[FeatureMenus] ([FeatureID], [MenuID]) VALUES (11, 90)
INSERT [dbo].[FeatureMenus] ([FeatureID], [MenuID]) VALUES (11, 87)
INSERT [dbo].[FeatureMenus] ([FeatureID], [MenuID]) VALUES (11, 93)
INSERT [dbo].[FeatureMenus] ([FeatureID], [MenuID]) VALUES (11, 94)
INSERT [dbo].[FeatureMenus] ([FeatureID], [MenuID]) VALUES (11, 95)
GO
print 'Processed 100 total records'
INSERT [dbo].[FeatureMenus] ([FeatureID], [MenuID]) VALUES (1, 96)
INSERT [dbo].[FeatureMenus] ([FeatureID], [MenuID]) VALUES (11, 108)
INSERT [dbo].[FeatureMenus] ([FeatureID], [MenuID]) VALUES (11, 109)
INSERT [dbo].[FeatureMenus] ([FeatureID], [MenuID]) VALUES (11, 110)
INSERT [dbo].[FeatureMenus] ([FeatureID], [MenuID]) VALUES (11, 111)
INSERT [dbo].[FeatureMenus] ([FeatureID], [MenuID]) VALUES (11, 112)
INSERT [dbo].[FeatureMenus] ([FeatureID], [MenuID]) VALUES (11, 113)
INSERT [dbo].[FeatureMenus] ([FeatureID], [MenuID]) VALUES (11, 126)
INSERT [dbo].[FeatureMenus] ([FeatureID], [MenuID]) VALUES (11, 127)
INSERT [dbo].[FeatureMenus] ([FeatureID], [MenuID]) VALUES (6, 128)
/* 7/22/2014 NS added DAG Health */
INSERT [dbo].[FeatureMenus] ([FeatureID], [MenuID]) VALUES (2, 133)
/* 7/24/2014 NS added Lync Stats */
INSERT [dbo].[FeatureMenus] ([FeatureID], [MenuID]) VALUES (13, 134)
/* 8/4/2014 NS added Lync pages */
INSERT [dbo].[FeatureMenus] ([FeatureID], [MenuID]) VALUES (13, 135)
/* 8/14/2014 WS added Log Sending pages */
INSERT [dbo].[FeatureMenus] ([FeatureID], [MenuID]) VALUES (11, 136)
INSERT [dbo].[FeatureMenus] ([FeatureID], [MenuID]) VALUES (2, 137)
INSERT [dbo].[FeatureMenus] ([FeatureID], [MenuID]) VALUES (2, 138)
/* 5/9/2014 Mukund added Sharepoint pages */
INSERT [dbo].[FeatureMenus] ([FeatureID], [MenuID]) VALUES (5, 139)
/* 9/8/14 WS added for Status History */
INSERT [dbo].[FeatureMenus] ([FeatureID], [MenuID]) VALUES (11, 140)
GO
INSERT [dbo].[FeatureMenus] ([FeatureID], [MenuID]) VALUES (2, 141)
GO
/*29Sep14 Mukund added*/
INSERT [dbo].[FeatureMenus] ([FeatureID], [MenuID]) VALUES (14, 142)
Go
/*01Oct14 Mukund added*/
INSERT [dbo].[FeatureMenus] ([FeatureID], [MenuID]) VALUES (15, 143)
Go
/*07Oct14 Mukund added*/
INSERT [dbo].[FeatureMenus] ([FeatureID], [MenuID]) VALUES (16, 146)
Go
--/*22Oct14 Mukund added*/
INSERT [dbo].[FeatureMenus] ([FeatureID], [MenuID]) VALUES (16, 149)
Go
/* 04Nov14 Mouli Added, VSPlus-1141*/
--INSERT [dbo].[FeatureMenus] ([FeatureID], [MenuID]) VALUES (11, 149)
/*19Nov14 Somaraju Added VSPLUS-1171*/
INSERT [dbo].[FeatureMenus] ([FeatureID], [MenuID]) VALUES (5, 151)
Go

/* 11/19/14 WS ADDED */
INSERT [dbo].[FeatureMenus] ([FeatureID], [MenuID]) VALUES (16, 141)
INSERT [dbo].[FeatureMenus] ([FeatureID], [MenuID]) VALUES (5, 141)
INSERT [dbo].[FeatureMenus] ([FeatureID], [MenuID]) VALUES (13, 141)
INSERT [dbo].[FeatureMenus] ([FeatureID], [MenuID]) VALUES (14, 141)
/*swathi vsplus-1185 */
INSERT [dbo].[FeatureMenus] ([FeatureID], [MenuID]) VALUES (2, 157)
INSERT [dbo].[FeatureMenus] ([FeatureID], [MenuID]) VALUES (2, 158)
INSERT [dbo].[FeatureMenus] ([FeatureID], [MenuID]) VALUES (19, 160)
/* 12/18/2014 NS added for VSPLUS-1229 */
INSERT [dbo].[FeatureMenus] ([FeatureID], [MenuID]) VALUES (11, 161)
INSERT [dbo].[FeatureMenus] ([FeatureID], [MenuID]) VALUES (11, 165)
/* 1/16/15 WS ADDED FOR MAP */
INSERT  [dbo].[FeatureMenus] ([FeatureID], [MenuID]) VALUES (11, 166)
/* 1/21/2015 NS added for VSPLUS-1378 */
INSERT  [dbo].[FeatureMenus] ([FeatureID], [MenuID]) VALUES (19, 159)
/* 12/28/2015 NS commented out for VSPLUS-2045 */
/*
INSERT  [dbo].[FeatureMenus] ([FeatureID], [MenuID]) VALUES (17, 167)
*/
/* 1/23/15 WS ADDED */
INSERT  [dbo].[FeatureMenus] ([FeatureID], [MenuID]) VALUES (11, 170)
--VSPLUS 2092 Durga
INSERT  [dbo].[FeatureMenus] ([FeatureID], [MenuID]) VALUES (18, 175)
/* 3/30/2015 NS added for VSPLUS-1259 */
INSERT [dbo].[FeatureMenus] ([FeatureID], [MenuID]) VALUES (18, 168)
INSERT [dbo].[FeatureMenus] ([FeatureID], [MenuID]) VALUES (5, 169)
INSERT [dbo].[FeatureMenus] ([FeatureID], [MenuID]) VALUES (1, 177)
INSERT [dbo].[FeatureMenus] ([FeatureID], [MenuID]) VALUES (1, 178)
INSERT [dbo].[FeatureMenus] ([FeatureID], [MenuID]) VALUES (1, 179)
INSERT [dbo].[FeatureMenus] ([FeatureID], [MenuID]) VALUES (1, 180)
INSERT [dbo].[FeatureMenus] ([FeatureID], [MenuID]) VALUES (1, 181)
INSERT [dbo].[FeatureMenus] ([FeatureID], [MenuID]) VALUES (1, 182)
INSERT [dbo].[FeatureMenus] ([FeatureID], [MenuID]) VALUES (18, 183)
INSERT [dbo].[FeatureMenus] ([FeatureID], [MenuID]) VALUES (18, 184)
INSERT [dbo].[FeatureMenus] ([FeatureID], [MenuID]) VALUES (12, 185)
INSERT [dbo].[FeatureMenus] ([FeatureID], [MenuID]) VALUES (12, 186)
INSERT [dbo].[FeatureMenus] ([FeatureID], [MenuID]) VALUES (12, 187)
INSERT [dbo].[FeatureMenus] ([FeatureID], [MenuID]) VALUES (12, 188)
INSERT [dbo].[FeatureMenus] ([FeatureID], [MenuID]) VALUES (5, 189)
INSERT [dbo].[FeatureMenus] ([FeatureID], [MenuID]) VALUES (5, 190)
INSERT [dbo].[FeatureMenus] ([FeatureID], [MenuID]) VALUES (14, 191)
INSERT [dbo].[FeatureMenus] ([FeatureID], [MenuID]) VALUES (14, 192)
INSERT [dbo].[FeatureMenus] ([FeatureID], [MenuID]) VALUES (2, 144)
INSERT [dbo].[FeatureMenus] ([FeatureID], [MenuID]) VALUES (2, 145)
/* 4/17/2015 NS modified for VSPLUS-1650 */
INSERT [dbo].[FeatureMenus] ([FeatureID], [MenuID]) VALUES (11, 194)
INSERT [dbo].[FeatureMenus] ([FeatureID], [MenuID]) VALUES (11, 193)
INSERT [dbo].[FeatureMenus] ([FeatureID], [MenuID]) VALUES (11, 195)
/* 5/5/15 WS Added for missing submenus for Domino, SharePoint and Microsoft Windows in Configurator */
INSERT [dbo].[FeatureMenus] ([FeatureID], [MenuID]) VALUES (5, 163)
INSERT [dbo].[FeatureMenus] ([FeatureID], [MenuID]) VALUES (5, 164)
INSERT [dbo].[FeatureMenus] ([FeatureID], [MenuID]) VALUES (14, 172)
INSERT [dbo].[FeatureMenus] ([FeatureID], [MenuID]) VALUES (14, 173)
INSERT [dbo].[FeatureMenus] ([FeatureID], [MenuID]) VALUES (1, 114)
INSERT [dbo].[FeatureMenus] ([FeatureID], [MenuID]) VALUES (1, 115)
INSERT [dbo].[FeatureMenus] ([FeatureID], [MenuID]) VALUES (1, 116)
INSERT [dbo].[FeatureMenus] ([FeatureID], [MenuID]) VALUES (1, 117)
INSERT [dbo].[FeatureMenus] ([FeatureID], [MenuID]) VALUES (1, 118)
INSERT [dbo].[FeatureMenus] ([FeatureID], [MenuID]) VALUES (1, 119)
INSERT [dbo].[FeatureMenus] ([FeatureID], [MenuID]) VALUES (4, 114)
INSERT [dbo].[FeatureMenus] ([FeatureID], [MenuID]) VALUES (4, 115)
INSERT [dbo].[FeatureMenus] ([FeatureID], [MenuID]) VALUES (4, 116)
INSERT [dbo].[FeatureMenus] ([FeatureID], [MenuID]) VALUES (4, 117)
INSERT [dbo].[FeatureMenus] ([FeatureID], [MenuID]) VALUES (4, 118)
INSERT [dbo].[FeatureMenus] ([FeatureID], [MenuID]) VALUES (4, 119)
/* 5/5/2015 NS added missing 3rd level menus */
INSERT [dbo].[FeatureMenus] ([FeatureID], [MenuID]) VALUES (18, 171)
INSERT [dbo].[FeatureMenus] ([FeatureID], [MenuID]) VALUES (18, 174)
INSERT [dbo].[FeatureMenus] ([FeatureID], [MenuID]) VALUES (4, 122)
INSERT [dbo].[FeatureMenus] ([FeatureID], [MenuID]) VALUES (12, 122)
INSERT [dbo].[FeatureMenus] ([FeatureID], [MenuID]) VALUES (12, 123)
INSERT [dbo].[FeatureMenus] ([FeatureID], [MenuID]) VALUES (12, 124)
/* 5/7/2015 NS added for VSPLUS-1646 */
INSERT [dbo].[FeatureMenus] ([FeatureID], [MenuID]) VALUES (11, 196)
--SWATHI VSPLUS 1360
INSERT [dbo].[FeatureMenus] ([FeatureID], [MenuID]) VALUES (11, 197)
/* 7/22/2015 NS added for VSPLUS-1911 */
INSERT [dbo].[FeatureMenus] ([FeatureID], [MenuID]) VALUES (1, 198)
--somaraju vsplus 1999
INSERT [dbo].[FeatureMenus] ([FeatureID], [MenuID]) VALUES (11, 199)
/* 8/13/2015 NS added for VSPLUS-2029 */
INSERT [dbo].[FeatureMenus] ([FeatureID], [MenuID]) VALUES (9, 200)
--VSPLUS 1669 somaraju
INSERT [dbo].[FeatureMenus] ([FeatureID], [MenuID]) VALUES (11, 201)
--VSPLUS 2187 SWATHI
INSERT [dbo].[FeatureMenus] ([FeatureID], [MenuID]) VALUES (15, 202)
INSERT [dbo].[FeatureMenus] ([FeatureID], [MenuID]) VALUES (14, 203)
INSERT [dbo].[FeatureMenus] ([FeatureID], [MenuID]) VALUES (14, 204)
--VSPLUS 2488 Durga
INSERT [dbo].[FeatureMenus] ([FeatureID], [MenuID]) VALUES (4, 205)
--VSPLUS-2519 Sowjanya
INSERT [dbo].[FeatureMenus] ([FeatureID], [MenuID]) VALUES (11, 206)
INSERT [dbo].[FeatureMenus] ([FeatureID], [MenuID]) VALUES (11, 207)
INSERT [dbo].[FeatureMenus] ([FeatureID], [MenuID]) VALUES (11, 208)
INSERT [dbo].[FeatureMenus] ([FeatureID], [MenuID]) VALUES (11, 209)
INSERT [dbo].[FeatureMenus] ([FeatureID], [MenuID]) VALUES (11, 210)
/* 2/15/2016 NS added for VSPLUS-2588*/
INSERT [dbo].[FeatureMenus] ([FeatureID], [MenuID]) VALUES (11, 211)
INSERT [dbo].[FeatureMenus] ([FeatureID], [MenuID]) VALUES (11, 212)
--3/11/2016 sowjanya Modified for VSPLUS-2650
INSERT [dbo].[FeatureMenus] ([FeatureID], [MenuID]) VALUES (20, 213)
INSERT [dbo].[FeatureMenus] ([FeatureID], [MenuID]) VALUES (20, 214)
--3/14/2016 Somaraju Addded for VSPLUS-2694,2697
INSERT [dbo].[FeatureMenus] ([FeatureID], [MenuID]) VALUES (19, 215)
/* 3/14/2016 NS added for VSPLUS-2651 */
INSERT  [dbo].[FeatureMenus] ([FeatureID], [MenuID]) VALUES (20, 167)
--Somaraju VSPLUS 2714
/* 3/23/2016 NS commented out - User Password Settings has been merged into the Office 365 health page */
/* INSERT  [dbo].[FeatureMenus] ([FeatureID], [MenuID]) VALUES (19, 216) */

--Durga VSPLUS 2656
INSERT  [dbo].[FeatureMenus] ([FeatureID], [MenuID]) VALUES (11, 217)
--20/4/2016 Durga Added for  VSPLUS-2866
INSERT  [dbo].[FeatureMenus] ([FeatureID], [MenuID]) VALUES (11, 218)
--Somaraju  VSPLUS-2613
INSERT  [dbo].[FeatureMenus] ([FeatureID], [MenuID]) VALUES (19, 219)
/* 5/31/2016 NS added for VSPLUS-2941 */
INSERT  [dbo].[FeatureMenus] ([FeatureID], [MenuID]) VALUES (1, 220)

GO





USE [vitalsigns]
GO
/* 1/21/2015 NS added all available features to be selected by default */
IF NOT EXISTS(select * from SelectedFeatures)
BEGIN
INSERT [dbo].[SelectedFeatures] ([FeatureID]) VALUES (1)
INSERT [dbo].[SelectedFeatures] ([FeatureID]) VALUES (2)
INSERT [dbo].[SelectedFeatures] ([FeatureID]) VALUES (3)
INSERT [dbo].[SelectedFeatures] ([FeatureID]) VALUES (4)
INSERT [dbo].[SelectedFeatures] ([FeatureID]) VALUES (5)
INSERT [dbo].[SelectedFeatures] ([FeatureID]) VALUES (6)
INSERT [dbo].[SelectedFeatures] ([FeatureID]) VALUES (8)
INSERT [dbo].[SelectedFeatures] ([FeatureID]) VALUES (9)
INSERT [dbo].[SelectedFeatures] ([FeatureID]) VALUES (10)
INSERT [dbo].[SelectedFeatures] ([FeatureID]) VALUES (11)
INSERT [dbo].[SelectedFeatures] ([FeatureID]) VALUES (12)
INSERT [dbo].[SelectedFeatures] ([FeatureID]) VALUES (13)
INSERT [dbo].[SelectedFeatures] ([FeatureID]) VALUES (14)
INSERT [dbo].[SelectedFeatures] ([FeatureID]) VALUES (15)
INSERT [dbo].[SelectedFeatures] ([FeatureID]) VALUES (16)
/* 12/28/2015 NS commented out for VSPLUS-2045 */
/*
INSERT [dbo].[SelectedFeatures] ([FeatureID]) VALUES (17)
*/
INSERT [dbo].[SelectedFeatures] ([FeatureID]) VALUES (18)
INSERT [dbo].[SelectedFeatures] ([FeatureID]) VALUES (19)
--3/11/2016 sowjanya Modified for VSPLUS-2650
INSERT [dbo].[SelectedFeatures] ([FeatureID]) VALUES (20)

END
GO
USE [vitalsigns]
GO
DELETE FROM ReportItems
/****** Object:  Table [dbo].[ReportItems]    Script Date: 09/06/2013 13:35:40 ******/
/* 4/10/2014 NS modified - set MaySchedule to 1 for the three reports currently available */
/* 7/10/2014 NS added for VSPLUS-618 */
/* 7/17/2014 NS added for VSPLUS-816*/
/* 8/5/2014 NS modified the Disk Space report names - removed Domino */
/* 8/21/2014 NS added a new Traveler Device Sync report for VSPLUS-886 */
/* 9/15/2014 NS added a new Potential Cluster Replication Problems report for VSPLUS-921 */
/* 9/17/2014 NS added a new Device Up Percent Daily report for VSPLUS-456 */
/* 10/2/2014 NS modified the report description for Server Disk Free Space for VE-36 */ 
/* 11/6/2014 NS added a new Any Statistic (ad hoc) report for VSPLUS-648 */
/* 1/28/2015 NS modified the category setting for Monthly Server Up Time and Down Time reports - now include Exchange servers*/
/* 2/2/2015 NS modified the category setting for Daily Memory Used - now include other server types */
/* 2/6/2015 NS modified the category setting for all Devices and Servers categories to Servers & Devices */
/* 2/12/2015 NS added a new report - Traveler CPU utilization */
/* 6/3/2015 NS modified - temporarily reset Assigned Maintenance Windows report status to IsWorking=False;
reset License Count report status to IsWorking=False */
/* 12/18/2015 NS modified for VSPLUS-2395 - enabled Disk Health (by location) and Domino Server Health
for scheduled reports */
/* 12/18/2015 NS modified for VSPLUS-2291 - renamed the Daily Mail Volume report to Daily/Monthly */
/* 1/29/2016 NS modified for VSPLUS-2533 - made Server Disk Free Space and Monthly Server Down Time available for scheduling */
SET IDENTITY_INSERT [dbo].[ReportItems] ON
INSERT [dbo].[ReportItems] ([ID], [Name], [Category], [Description], [PageURL], [ImageURL], [ConfiguratorOnly], [isworking], [MaySchedule]) VALUES (1, N'User Access List', N'Users', N'A list of authorized users and the servers they can administer', N'../ConfiguratorReports/ConfigUserListRpt.aspx', N'../images/imagesIcons/images1.jpg', 0, N'True', NULL)
INSERT [dbo].[ReportItems] ([ID], [Name], [Category], [Description], [PageURL], [ImageURL], [ConfiguratorOnly], [isworking], [MaySchedule]) VALUES (2, N'Log File Scanning', N'Domino', N'A list of all the keywords that are scanned for in the Domino log files', N'../ConfiguratorReports/LogFileScanRpt.aspx', N'../images/imagesIcons/images2.jpg', 0, N'True', NULL)
INSERT [dbo].[ReportItems] ([ID], [Name], [Category], [Description], [PageURL], [ImageURL], [ConfiguratorOnly], [isworking], [MaySchedule]) VALUES (3, N'Mail File Statistics', N'Mail', N'Statistics related to mail files', N'../ConfiguratorReports/MailFileRpt.aspx', N'../images/imagesIcons/images3.jpg', 0, N'True', NULL)
--2/22/2016 Sowjanya Modified for VSPLUS-2620
INSERT [dbo].[ReportItems] ([ID], [Name], [Category], [Description], [PageURL], [ImageURL], [ConfiguratorOnly], [isworking], [MaySchedule]) VALUES (4, N'Mail Thresholds', N'Domino', N'A list of Domino servers and the mail threshold values', N'../ConfiguratorReports/MailThresholdRpt.aspx', N'../images/imagesIcons/images4.jpg', 0, N'True', NULL)
INSERT [dbo].[ReportItems] ([ID], [Name], [Category], [Description], [PageURL], [ImageURL], [ConfiguratorOnly], [isworking], [MaySchedule]) VALUES (5, N'Maintenance Windows', N'Maintenance', N'A list of all configured maintenance windows', N'../ConfiguratorReports/MaintenanceWinRpt.aspx', N'../images/imagesIcons/images5.jpg', 0, N'True', NULL)
INSERT [dbo].[ReportItems] ([ID], [Name], [Category], [Description], [PageURL], [ImageURL], [ConfiguratorOnly], [isworking], [MaySchedule]) VALUES (6, N'Monitored Databases', N'Domino', N'Monitored Domino databases', N'../ConfiguratorReports/NotesDBsRpt.aspx', N'../images/imagesIcons/images11.jpg', 1, N'True', NULL)
INSERT [dbo].[ReportItems] ([ID], [Name], [Category], [Description], [PageURL], [ImageURL], [ConfiguratorOnly], [isworking], [MaySchedule]) VALUES (7, N'Server List by Type', N'Configuration', N'A list of servers by type', N'../ConfiguratorReports/ServerListTypeRpt.aspx', N'../images/imagesIcons/images3.jpg', 0, N'True', NULL)
INSERT [dbo].[ReportItems] ([ID], [Name], [Category], [Description], [PageURL], [ImageURL], [ConfiguratorOnly], [isworking], [MaySchedule]) VALUES (8, N'Monitored Domino Server Tasks', N'Domino', N'A list of all the Domino server tasks which are monitored for each server', N'../ConfiguratorReports/ServerTaskRpt.aspx', N'../images/imagesIcons/images5.jpg', 1, N'True', NULL)
INSERT [dbo].[ReportItems] ([ID], [Name], [Category], [Description], [PageURL], [ImageURL], [ConfiguratorOnly], [isworking], [MaySchedule]) VALUES (25, N'Daily Server Transactions per Minute', N'Domino', N'Domino server daily transactions per minute', N'../DashboardReports/SrvTransPerMinRpt.aspx', NULL, 0, N'True', NULL)
INSERT [dbo].[ReportItems] ([ID], [Name], [Category], [Description], [PageURL], [ImageURL], [ConfiguratorOnly], [isworking], [MaySchedule]) VALUES (10, N'Server List by Location', N'Configuration', N'All servers, listed by location', N'../ConfiguratorReports/ServerListLocRpt.aspx', N'../images/imagesIcons/images5.jpg', 0, N'True', NULL)
INSERT [dbo].[ReportItems] ([ID], [Name], [Category], [Description], [PageURL], [ImageURL], [ConfiguratorOnly], [isworking], [MaySchedule]) VALUES (11, N'Daily/Monthly Mail Volume', N'Mail', N'Daily Volume of Mail Delivered, Transferred, Routed for Domino Servers', N'../DashboardReports/DailyMailVolumeRpt.aspx', NULL, 0, N'True', 1)
INSERT [dbo].[ReportItems] ([ID], [Name], [Category], [Description], [PageURL], [ImageURL], [ConfiguratorOnly], [isworking], [MaySchedule]) VALUES (12, N'Daily Memory Percent Used', N'Servers & Devices', N'Daily memory percent used', N'../DashboardReports/DailyMemoryUsedRpt.aspx', NULL, 0, N'True', NULL)
INSERT [dbo].[ReportItems] ([ID], [Name], [Category], [Description], [PageURL], [ImageURL], [ConfiguratorOnly], [isworking], [MaySchedule]) VALUES (13, N'Device Daily Average On Target Percent', N'Servers & Devices', N'Daily average on target percent for devices', N'../DashboardReports/DeviceHourlyOnTargetPctRpt.aspx', NULL, 0, N'True', NULL)
INSERT [dbo].[ReportItems] ([ID], [Name], [Category], [Description], [PageURL], [ImageURL], [ConfiguratorOnly], [isworking], [MaySchedule]) VALUES (14, N'Device Up Percent Hourly', N'Servers & Devices', N'Percentage of time a device is up throughout a day', N'../DashboardReports/DeviceUptimeRpt.aspx', NULL, 0, N'True', NULL)
INSERT [dbo].[ReportItems] ([ID], [Name], [Category], [Description], [PageURL], [ImageURL], [ConfiguratorOnly], [isworking], [MaySchedule]) VALUES (15, N'Database Inventory', N'Domino', N'List of all Domino databases by server', N'../DashboardReports/DominoDBDailyStatsRpt.aspx', NULL, 0, N'True', NULL)
INSERT [dbo].[ReportItems] ([ID], [Name], [Category], [Description], [PageURL], [ImageURL], [ConfiguratorOnly], [isworking], [MaySchedule]) VALUES (16, N'Disk Health (by location)', N'Disk', N'Disk size and utilization by location', N'../DashboardReports/DominoDiskHealthLocRpt.aspx', NULL, 0, N'True', 1)
INSERT [dbo].[ReportItems] ([ID], [Name], [Category], [Description], [PageURL], [ImageURL], [ConfiguratorOnly], [isworking], [MaySchedule]) VALUES (17, N'Disk Health', N'Disk', N'Disk size and utilization by server', N'../DashboardReports/DominoDiskHealthRpt.aspx', NULL, 0, N'True', NULL)
INSERT [dbo].[ReportItems] ([ID], [Name], [Category], [Description], [PageURL], [ImageURL], [ConfiguratorOnly], [isworking], [MaySchedule]) VALUES (18, N'Domino Response Times', N'Domino', N'Daily Domino response times', N'../DashboardReports/DominoResponseTimesMonthlyRpt.aspx', NULL, 0, N'True', NULL)
INSERT [dbo].[ReportItems] ([ID], [Name], [Category], [Description], [PageURL], [ImageURL], [ConfiguratorOnly], [isworking], [MaySchedule]) VALUES (19, N'Domino Server Health', N'Domino', N'Domino server status and response time information', N'../DashboardReports/DominoServerHealthRpt.aspx', NULL, 0, N'True', 1)
INSERT [dbo].[ReportItems] ([ID], [Name], [Category], [Description], [PageURL], [ImageURL], [ConfiguratorOnly], [isworking], [MaySchedule]) VALUES (20, N'Domino Server CPU Utilization', N'Domino', N'Domino server CPU utilization hourly', N'../DashboardReports/DominoSrvCPUUtilRpt.aspx', NULL, 0, N'True', NULL)
INSERT [dbo].[ReportItems] ([ID], [Name], [Category], [Description], [PageURL], [ImageURL], [ConfiguratorOnly], [isworking], [MaySchedule]) VALUES (21, N'Overall Server Status Health', N'Servers & Devices', N'Server status, mail counts, and CPU information', N'../DashboardReports/OverallSrvStatusHealthRpt.aspx', NULL, 0, N'True', NULL)
INSERT [dbo].[ReportItems] ([ID], [Name], [Category], [Description], [PageURL], [ImageURL], [ConfiguratorOnly], [isworking], [MaySchedule]) VALUES (22, N'Response Times', N'Servers & Devices', N'Device response times', N'../DashboardReports/ResponseTimeRpt.aspx', NULL, 0, N'True', 1)
INSERT [dbo].[ReportItems] ([ID], [Name], [Category], [Description], [PageURL], [ImageURL], [ConfiguratorOnly], [isworking], [MaySchedule]) VALUES (23, N'Server Availability', N'Domino', N'Domino server daily availability index', N'../DashboardReports/SrvAvailabilityRpt.aspx', NULL, 0, N'False', NULL)
INSERT [dbo].[ReportItems] ([ID], [Name], [Category], [Description], [PageURL], [ImageURL], [ConfiguratorOnly], [isworking], [MaySchedule]) VALUES (24, N'Server Disk Free Space', N'Disk', N'Daily server disk free space', N'../DashboardReports/SrvDiskFreeSpaceTrendRpt.aspx', NULL, 0, N'True', 1)
INSERT [dbo].[ReportItems] ([ID], [Name], [Category], [Description], [PageURL], [ImageURL], [ConfiguratorOnly], [isworking], [MaySchedule]) VALUES (26, N'Cluster Seconds On Queue', N'Domino', N'Domino Cluster Seconds on Queue', N'../DashboardReports/ClusterSecRpt.aspx', NULL, 0, N'True', NULL)
INSERT [dbo].[ReportItems] ([ID], [Name], [Category], [Description], [PageURL], [ImageURL], [ConfiguratorOnly], [isworking], [MaySchedule]) VALUES (27, N'Average CPU utilization per day', N'Domino', N'Average CPU utilization per day', N'../DashboardReports/AVGCpuUtil.aspx', NULL, 0, N'True', NULL)
INSERT [dbo].[ReportItems] ([ID], [Name], [Category], [Description], [PageURL], [ImageURL], [ConfiguratorOnly], [isworking], [MaySchedule]) VALUES (28, N'Historical Response Times', N'Servers & Devices', N'Historical Response Times - graph over the course of a day, a week ', N'../DashboardReports/HistoricalResponseTime.aspx', NULL, 0, N'False', NULL)
INSERT [dbo].[ReportItems] ([ID], [Name], [Category], [Description], [PageURL], [ImageURL], [ConfiguratorOnly], [isworking], [MaySchedule]) VALUES (29, N'Average daily response times for a given time period ', N'Servers & Devices', N'Average daily response times for a given time period ', N'../DashboardReports/AvgResponseTimeDaily.aspx', NULL, 0, N'True', NULL)
INSERT [dbo].[ReportItems] ([ID], [Name], [Category], [Description], [PageURL], [ImageURL], [ConfiguratorOnly], [isworking], [MaySchedule]) VALUES (30, N'License Count report ', N'Servers & Devices', N'License Count report (Configurator) ', N'../ConfiguratorReports/LicenseCountRpt.aspx', NULL, 1, N'False', NULL)
/* 9/15/2015 NS commented out for VSPLUS-2025 */
/*INSERT [dbo].[ReportItems] ([ID], [Name], [Category], [Description], [PageURL], [ImageURL], [ConfiguratorOnly], [isworking], [MaySchedule]) VALUES (31, N'Mail File Open Times Delta', N'Traveler', N'Delta of the number of times a traveler server accesses user mail databases', N'../DashboardReports/TravelerStatsDeltaRpt.aspx', NULL, 0, N'True', NULL)*/
INSERT [dbo].[ReportItems] ([ID], [Name], [Category], [Description], [PageURL], [ImageURL], [ConfiguratorOnly], [isworking], [MaySchedule]) VALUES (32, N'Cumulative Mail File Open Times (by mail server)', N'Traveler', N'Number of times a traveler server accesses user mail databases on mail servers', N'../DashboardReports/TravelerStatsSrvRpt.aspx', NULL, 0, N'True', NULL)
INSERT [dbo].[ReportItems] ([ID], [Name], [Category], [Description], [PageURL], [ImageURL], [ConfiguratorOnly], [isworking], [MaySchedule]) VALUES (33, N'Cumulative Mail File Open Times (by interval)', N'Traveler', N'Number of times a traveler server accesses user mail databases by time interval', N'../DashboardReports/TravelerStatsRpt.aspx', NULL, 0, N'True', NULL)
INSERT [dbo].[ReportItems] ([ID], [Name], [Category], [Description], [PageURL], [ImageURL], [ConfiguratorOnly], [isworking], [MaySchedule]) VALUES (34, N'Disk Space Consumption', N'Disk', N'Average disk space consumption', N'../DashboardReports/DominoDiskTrendRpt.aspx', NULL, 0, N'True', 1)
INSERT [dbo].[ReportItems] ([ID], [Name], [Category], [Description], [PageURL], [ImageURL], [ConfiguratorOnly], [isworking], [MaySchedule]) VALUES (35, N'Assigned Maintenance Windows', N'Maintenance', N'A list of all configured maintenance windows by server assignment', N'../ConfiguratorReports/MaintenanceWinCatRpt.aspx', NULL, 1, N'False', NULL)
INSERT [dbo].[ReportItems] ([ID], [Name], [Category], [Description], [PageURL], [ImageURL], [ConfiguratorOnly], [isworking], [MaySchedule]) VALUES (36, N'Maximum CPU utilization per day', N'Domino', N'Maximum CPU utilization per day', N'../DashboardReports/MAXCpuUtilRpt.aspx', NULL, 0, N'True', NULL)
INSERT [dbo].[ReportItems] ([ID], [Name], [Category], [Description], [PageURL], [ImageURL], [ConfiguratorOnly], [isworking], [MaySchedule]) VALUES (37, N'Console Commands List', N'Domino', N'A list of submitted and processed Domino console commands by user', N'../DashboardReports/ConsoleCommandsRpt.aspx', NULL, 0, N'True', NULL)
INSERT [dbo].[ReportItems] ([ID], [Name], [Category], [Description], [PageURL], [ImageURL], [ConfiguratorOnly], [isworking], [MaySchedule]) VALUES (38, N'Disk Space Availability', N'Disk', N'Free disk space - annual trend', N'../DashboardReports/DominoDiskAnnualTrendRpt.aspx', NULL, 0, N'True', NULL)
INSERT [dbo].[ReportItems] ([ID], [Name], [Category], [Description], [PageURL], [ImageURL], [ConfiguratorOnly], [isworking], [MaySchedule]) VALUES (39, N'Disk Space Consumption Summary', N'Disk', N'Average disk space consumption combined for selected servers', N'../DashboardReports/DominoDiskTrendOverallRpt.aspx', NULL, 0, N'True', NULL)
INSERT [dbo].[ReportItems] ([ID], [Name], [Category], [Description], [PageURL], [ImageURL], [ConfiguratorOnly], [isworking], [MaySchedule]) VALUES (40, N'Monthly Server Down Time', N'Servers & Devices', N'Server down times in minutes monthly', N'../DashboardReports/ServerAvailabilityRpt.aspx', NULL, 0, N'True', 1)
INSERT [dbo].[ReportItems] ([ID], [Name], [Category], [Description], [PageURL], [ImageURL], [ConfiguratorOnly], [isworking], [MaySchedule]) VALUES (41,'Domino Server Configuration Settings', N'Configuration', N'List of All Domino Servers and its Configuration Settings', N'../Configurator/DominoServerThreshold.aspx', N'../images/imagesIcons/images5.jpg', 0, N'True', NULL)
INSERT [dbo].[ReportItems] ([ID], [Name], [Category], [Description], [PageURL], [ImageURL], [ConfiguratorOnly], [isworking], [MaySchedule]) VALUES (42, N'Monthly Server Up Time', N'Servers & Devices', N'Server up times in minutes monthly', N'../DashboardReports/ServerUpTimeRpt.aspx', NULL, 0, N'True', NULL)
INSERT [dbo].[ReportItems] ([ID], [Name], [Category], [Description], [PageURL], [ImageURL], [ConfiguratorOnly], [isworking], [MaySchedule]) VALUES (43, N'Server Disk Total Space', N'Disk', N'Total disk space by server (used and free)', N'../DashboardReports/DominoDiskSpaceRpt.aspx', NULL, 0, N'True', NULL)
INSERT [dbo].[ReportItems] ([ID], [Name], [Category], [Description], [PageURL], [ImageURL], [ConfiguratorOnly], [isworking], [MaySchedule]) VALUES (44, N'Device Sync Volume', N'Traveler', N'Total number of device syncs daily', N'../DashboardReports/TravelerDeviceSyncRpt.aspx', NULL, 0, N'True', NULL)
INSERT [dbo].[ReportItems] ([ID], [Name], [Category], [Description], [PageURL], [ImageURL], [ConfiguratorOnly], [isworking], [MaySchedule]) VALUES (45, N'Potential Cluster Replication Problems', N'Domino', N'Potential cluster replication problem databases', N'../DashboardReports/DBClusterRpt.aspx', NULL, 0, N'True', NULL)
--2/11/2016 Sowmya Modified for VSPLUS-2624
INSERT [dbo].[ReportItems] ([ID], [Name], [Category], [Description], [PageURL], [ImageURL], [ConfiguratorOnly], [isworking], [MaySchedule]) VALUES (46, N'Device Up Percent Daily', N'Servers & Devices', N'Percentage of time a device is up on a daily basis', N'../DashboardReports/DeviceUptimePctRpt.aspx', NULL, 0, N'True', N'True')
--2/11/2016 Sowjanya Modified for VSPLUS-2593
INSERT [dbo].[ReportItems] ([ID], [Name], [Category], [Description], [PageURL], [ImageURL], [ConfiguratorOnly], [isworking], [MaySchedule]) VALUES (47, N'Any Statistic', N'Servers & Devices', N'Weekly report of any statistic', N'../DashboardReports/AdHocRpt.aspx', NULL, 0, N'True', NULL)
INSERT [dbo].[ReportItems] ([ID], [Name], [Category], [Description], [PageURL], [ImageURL], [ConfiguratorOnly], [isworking], [MaySchedule]) VALUES (48, N'Sametime Statistics Grid', N'Sametime', N'Report grid of Sametime statistics', N'../DashboardReports/SametimeStatsRpt.aspx', NULL, 0, N'True', NULL)
INSERT [dbo].[ReportItems] ([ID], [Name], [Category], [Description], [PageURL], [ImageURL], [ConfiguratorOnly], [isworking], [MaySchedule]) VALUES (49, N'Sametime Statistics Chart', N'Sametime', N'Report chart of Sametime statistics', N'../DashboardReports/SametimeStatsChartRpt.aspx', NULL, 0, N'True', NULL)
INSERT [dbo].[ReportItems] ([ID], [Name], [Category], [Description], [PageURL], [ImageURL], [ConfiguratorOnly], [isworking], [MaySchedule]) VALUES (50, N'Traveler Server CPU utilization per day', N'Traveler', N'Traveler Server CPU utilization per day', N'../DashboardReports/AVGCpuUtil.aspx?M=d&ServerType=Traveler', NULL, 0, N'True', NULL)
INSERT [dbo].[ReportItems] ([ID],[Name],[Category],[Description],[PageURL],[ImageURL],[ConfiguratorOnly],[isworking],[MaySchedule]) VALUES(51,'Monitored Servers - Total licences used','Devices','Monitored Servers - Total licences used report (Configurator) ','../ConfiguratorReports/ServerMonitoringRpt.aspx',NULL,1,'False',NULL)
/* 4/17/2015 NS added a new report - Exchange Messages Sent, Response Time Trend for VSPLUS-1534 */
INSERT [dbo].[ReportItems] ([ID], [Name], [Category], [Description], [PageURL], [ImageURL], [ConfiguratorOnly], [isworking], [MaySchedule]) VALUES (52, N'Exchange Messages Sent', N'Exchange', N'Exchange messages sent', N'../DashboardReports/ExchangeMsgRpt.aspx', NULL, 0, N'True', NULL)
INSERT [dbo].[ReportItems] ([ID], [Name], [Category], [Description], [PageURL], [ImageURL], [ConfiguratorOnly], [isworking], [MaySchedule]) VALUES (53, N'Response Times Trend', N'Servers & Devices', N'Average response times trend', N'../DashboardReports/ResponseTimeTrendRpt.aspx', NULL, 0, N'True', NULL)
/* 6/19/2015 NS added a new User Count Trend report for VSPLUS-1841*/
INSERT [dbo].[ReportItems] ([ID], [Name], [Category], [Description], [PageURL], [ImageURL], [ConfiguratorOnly], [isworking], [MaySchedule]) VALUES (54, N'User Count Trend', N'Users', N'User count trend', N'../DashboardReports/UserCountTrendRpt.aspx', NULL, 0, N'True', NULL)
/* 12/8/2015 NS added for VSPLUS-2140 */
INSERT [dbo].[ReportItems] ([ID], [Name], [Category], [Description], [PageURL], [ImageURL], [ConfiguratorOnly], [isworking], [MaySchedule]) VALUES (55, N'Exchange Users', N'Users', N'Exchange users', N'../DashboardReports/ExchangeUserCountRpt.aspx', NULL, 0, N'True', NULL)
/* 1/5/2016 NS added added a new report - Exchange Mail Database Size Trend for VSPLUS-1534 */
INSERT [dbo].[ReportItems] ([ID], [Name], [Category], [Description], [PageURL], [ImageURL], [ConfiguratorOnly], [isworking], [MaySchedule]) VALUES (56, N'Exchange Mail Database Size Trend', N'Exchange', N'Exchange Mail Database Size Trend', N'../DashboardReports/ExchangeMailDBTrendRpt.aspx', NULL, 0, N'True', NULL)
/*2/9/2016 Durga Added for VSPLUS-2174*/
INSERT [dbo].[ReportItems] ([ID], [Name], [Category], [Description], [PageURL], [ImageURL], [ConfiguratorOnly], [isworking], [MaySchedule]) VALUES (57, N'Traveler Allocated Java memory', N'Traveler', N'Traveler Allocated Java memory', N'../DashboardReports/Javamemory.aspx?M=d&ServerType=Traveler', NULL, 0, N'True', NULL)
INSERT [dbo].[ReportItems] ([ID], [Name], [Category], [Description], [PageURL], [ImageURL], [ConfiguratorOnly], [isworking], [MaySchedule]) VALUES (58, N'Traveler Allocated C memory', N'Traveler', N'Traveler Allocated C memory', N'../DashboardReports/TravelerCmemory.aspx?M=d&ServerType=Traveler', NULL, 0, N'True', NULL)
INSERT [dbo].[ReportItems] ([ID], [Name], [Category], [Description], [PageURL], [ImageURL], [ConfiguratorOnly], [isworking], [MaySchedule]) VALUES (59, N'Traveler HTTP sessions', N'Traveler', N'Traveler HTTP sessions', N'../DashboardReports/TravelerHTTPsessions.aspx?M=d&ServerType=Traveler', NULL, 0, N'True', NULL)
/* 2/24/2016 NS added for VSPLUS-2641 */
/* 4/26/2016 Sowmya modified for VSPLUS-2881 */
INSERT [dbo].[ReportItems] ([ID], [Name], [Category], [Description], [PageURL], [ImageURL], [ConfiguratorOnly], [isworking], [MaySchedule]) VALUES (60, N'Server Availability Index', N'Domino', N'Server Availability Index', N'../DashboardReports/SrvAvailabilityIndexRpt.aspx', NULL, 0, N'True', NULL)
/* 3/9/2016 NS added for VSPLUS-2642 */
/* 4/26/2016 Sowmya modified for VSPLUS-2881 */
INSERT [dbo].[ReportItems] ([ID], [Name], [Category], [Description], [PageURL], [ImageURL], [ConfiguratorOnly], [isworking], [MaySchedule]) VALUES (61, N'Server Access via a Browser', N'Domino', N'Server Access via Browser', N'../DashboardReports/DominoAccessBrowserRpt.aspx', NULL, 0, N'True', NULL)
-- 3/18/2016 Durga Addded for VSPLUS-2702
/* 4/26/2016 Sowmya modified for VSPLUS-2881 */
INSERT [dbo].[ReportItems] ([ID], [Name], [Category], [Description], [PageURL], [ImageURL], [ConfiguratorOnly], [isworking], [MaySchedule]) VALUES (62, N'Stale Mailboxes', N'Exchange', N'Stale Mailboxes', N'../DashboardReports/Stalemailboxes.aspx', NULL, 0, N'True', NULL)
/* 3/21/2016 NS added for VSPLUS-2652 */
/* 4/26/2016 Sowmya modified for VSPLUS-2881 */
INSERT [dbo].[ReportItems] ([ID], [Name], [Category], [Description], [PageURL], [ImageURL], [ConfiguratorOnly], [isworking], [MaySchedule]) VALUES (63, N'Password Settings', N'Office 365', N'User Password Settings', N'../DashboardReports/O365PasswordSettingsRpt.aspx', NULL, 0, N'True', NULL)
--28/3/2016 Durga added for VSPLUS 2695 
INSERT [dbo].[ReportItems] ([ID], [Name], [Category], [Description], [PageURL], [ImageURL], [ConfiguratorOnly], [isworking], [MaySchedule]) VALUES (64, N'Cost Per User Served Grid', N'Financial', N'Cost Per User Served Grid', N'../DashboardReports/CostPerUserServed.aspx', NULL, 0, N'True', NULL)
INSERT [dbo].[ReportItems] ([ID], [Name], [Category], [Description], [PageURL], [ImageURL], [ConfiguratorOnly], [isworking], [MaySchedule]) VALUES (65, N'Cost Per User Served Chart', N'Financial', N'Cost Per User Served Chart', N'../DashboardReports/CostPerUserServedchart.aspx', NULL, 0, N'True', NULL)
--29/3/2016 Durga added for  VSPLUS-2698
INSERT [dbo].[ReportItems] ([ID], [Name], [Category], [Description], [PageURL], [ImageURL], [ConfiguratorOnly], [isworking], [MaySchedule]) VALUES (66, N'Server Utilization', N'Financial', N'Server Utilization', N'../DashboardReports/ServerUtilization.aspx', NULL, 0, N'True', NULL)
--12/4/2016 Durga added for VSPLUS-2829
INSERT [dbo].[ReportItems] ([ID], [Name], [Category], [Description], [PageURL], [ImageURL], [ConfiguratorOnly], [isworking], [MaySchedule]) VALUES (67, N'Wikis', N'Connections', N'Wikis', N'../DashboardReports/IBMConnectionsWikis.aspx', NULL, 0, N'True', NULL)
--12/4/2016 Sowjanya added for VSPLUS-2831
INSERT [dbo].[ReportItems] ([ID], [Name], [Category], [Description], [PageURL], [ImageURL], [ConfiguratorOnly], [isworking], [MaySchedule]) VALUES (68, N'Forums', N'Connections', N'Forums', N'../DashboardReports/IBMConnectionForumsRpt.aspx', NULL, 0, N'True', NULL)
--12/4/2016 Durga added for VSPLUS-2836
INSERT [dbo].[ReportItems] ([ID], [Name], [Category], [Description], [PageURL], [ImageURL], [ConfiguratorOnly], [isworking], [MaySchedule]) VALUES (69, N'Profiles', N'Connections', N'Profiles', N'../DashboardReports/IBMConnectionsProfiles.aspx', NULL, 0, N'True', NULL)
--12/4/2016 Sowmya added for VSPLUS-2830
INSERT [dbo].[ReportItems] ([ID], [Name], [Category], [Description], [PageURL], [ImageURL], [ConfiguratorOnly], [isworking], [MaySchedule]) VALUES (70, N'Files', N'Connections', N'Files', N'../DashboardReports/IBMConnectionFileRpt.aspx', NULL, 0, N'True', NULL)
--14/4/2016 Durga added for VSPLUS-2832
INSERT [dbo].[ReportItems] ([ID], [Name], [Category], [Description], [PageURL], [ImageURL], [ConfiguratorOnly], [isworking], [MaySchedule]) VALUES (71, N'Activities', N'Connections', N'Activities', N'../DashboardReports/IBMConnectionsActivity.aspx', NULL, 0, N'True', NULL)
--14/4/2016 Sowjanya added for VSPLUS-2833
INSERT [dbo].[ReportItems] ([ID], [Name], [Category], [Description], [PageURL], [ImageURL], [ConfiguratorOnly], [isworking], [MaySchedule]) VALUES (72, N'Bookmarks', N'Connections', N'Bookmarks', N'../DashboardReports/IBMConnectionBookmarksRpt.aspx', NULL, 0, N'True', NULL)
  --22/4/2016 Durga added for  VSPLUS-2806
INSERT [dbo].[ReportItems] ([ID], [Name], [Category], [Description], [PageURL], [ImageURL], [ConfiguratorOnly], [isworking], [MaySchedule]) VALUES (73, N'EXJournal Document Totals', N'Domino', N'EXJournal Document Totals', N'../DashboardReports/EXJournalDocumentTotals.aspx', NULL, 0, N'True', NULL)
/* 6/2/2016 NS added for VSPLUS-3021 */
INSERT [dbo].[ReportItems] ([ID], [Name], [Category], [Description], [PageURL], [ImageURL], [ConfiguratorOnly], [isworking], [MaySchedule]) VALUES (74, N'User Adoption', N'Connections', N'User Adoption', N'../DashboardReports/IBMConnectionsUserAdoptionRpt.aspx', NULL, 0, N'True', NULL)
/* 6/2/2016 NS added for VSPLUS-3019 */
INSERT [dbo].[ReportItems] ([ID], [Name], [Category], [Description], [PageURL], [ImageURL], [ConfiguratorOnly], [isworking], [MaySchedule]) VALUES (75, N'User Activity', N'Connections', N'User Activity', N'../DashboardReports/IBMConnectionsUserActivityRpt.aspx', NULL, 0, N'True', NULL)
--6/3/2016 Sowjanya added for VSPLUS-2895
INSERT [dbo].[ReportItems] ([ID], [Name], [Category], [Description], [PageURL], [ImageURL], [ConfiguratorOnly], [isworking], [MaySchedule]) VALUES (76, N'Tags', N'Connections', N'Tags', N'../DashboardReports/ConnectiontestRpt.aspx', NULL, 0, N'True', NULL)
--6/3/2016 Sowmya added for VSPLUS-2934
INSERT [dbo].[ReportItems] ([ID], [Name], [Category], [Description], [PageURL], [ImageURL], [ConfiguratorOnly], [isworking], [MaySchedule]) VALUES (77, N'Communities', N'Connections', N'Communities', N'../DashboardReports/IBMConnCommunityRpt.aspx', NULL, 0, N'True', NULL)
/* 6/3/2016 NS added for VSPLUS-3025 */
INSERT [dbo].[ReportItems] ([ID], [Name], [Category], [Description], [PageURL], [ImageURL], [ConfiguratorOnly], [isworking], [MaySchedule]) VALUES (78, N'Community Activity', N'Connections', N'Community Activity', N'../DashboardReports/IBMConnectionsCommunityActivityRpt.aspx', NULL, 0, N'True', NULL)
INSERT [dbo].[ReportItems] ([ID], [Name], [Category], [Description], [PageURL], [ImageURL], [ConfiguratorOnly], [isworking], [MaySchedule]) VALUES (79, N'Mailbox Storage Growth', N'Exchange', N'Exchange Mailbox storage growth', N'../DashboardReports/ExchangeMailboxstoragegrowth.aspx', NULL, 0, N'True', NULL)
INSERT [dbo].[ReportItems] ([ID], [Name], [Category], [Description], [PageURL], [ImageURL], [ConfiguratorOnly], [isworking], [MaySchedule]) VALUES (80, N'Office 365 Mailbox Storage Growth', N'Office 365', N'Office 365 Mailbox storage growth', N'../DashboardReports/O365Mailboxstoragegrowth.aspx', NULL, 0, N'True', NULL)
/* 6/9/2015 NS added for VSPLUS-3020 */
INSERT [dbo].[ReportItems] ([ID], [Name], [Category], [Description], [PageURL], [ImageURL], [ConfiguratorOnly], [isworking], [MaySchedule]) VALUES (81, N'User Adoption Metrics', N'Connections', N'User Adoption Metrics', N'../DashboardReports/IBMConnectionsUserAdoptionOverallRpt.aspx', NULL, 0, N'True', NULL)

SET IDENTITY_INSERT [dbo].[ReportItems] OFF

USE [VSS_Statistics]
GO
ALTER TABLE [dbo].[Daily] ALTER COLUMN [Title] [nvarchar](200) NULL
Go

USE [vitalsigns]
GO

/****** Object:  View [dbo].[V_DominoDiskSpace]    Script Date: 05/16/2014 00:30:21 ******/
IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[V_DominoDiskSpace]'))
DROP VIEW [dbo].[V_DominoDiskSpace]
GO

USE [vitalsigns]
GO

/****** Object:  View [dbo].[V_DominoDiskSpace]    Script Date: 05/16/2014 00:30:22 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW [dbo].[V_DominoDiskSpace]
AS
SELECT * FROM DominoDiskSpace WHERE DiskFree IS NOT NULL AND DiskSize IS NOT NULL
GO

IF NOT EXISTS(SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[MobileUserThreshold]') AND name = N'UX_DeviceId')
BEGIN
	CREATE UNIQUE INDEX UX_DeviceId ON [dbo].[MobileUserThreshold] ([DeviceId])
END

Go

/* 3/6/2014 Mukund added for VSPLUS-677*/

USE [vitalsigns]
GO
Delete from RPRAccessPages
Go
/****** Object:  Table [dbo].[RPRAccessPages]    Script Date: 06/03/2014 16:49:28 ******/
SET IDENTITY_INSERT [dbo].[RPRAccessPages] ON
INSERT [dbo].[RPRAccessPages] ([ID], [Name], [Category], [Description], [PageURL], [ImageURL]) VALUES (1, N'Maintain Menus', N'Menus', N'Add/ Edit/ Delete menu Items.', N'../Security/MaintainMenu.aspx', NULL)
INSERT [dbo].[RPRAccessPages] ([ID], [Name], [Category], [Description], [PageURL], [ImageURL]) VALUES (2, N'Maintain Features', N'Menus', N'Create Features List. ', N'../Security/MaintainFeatures.aspx', NULL)
INSERT [dbo].[RPRAccessPages] ([ID], [Name], [Category], [Description], [PageURL], [ImageURL]) VALUES (3, N'Assign Menus To Feature', N'Menus', N'Assign/Reassign Menus To Feature', N'../Security/AssignMenusToFeature.aspx', NULL)
INSERT [dbo].[RPRAccessPages] ([ID], [Name], [Category], [Description], [PageURL], [ImageURL]) VALUES (4, N'Manage Settings', N'Settings', N'Manage VitalSigns related Settings', N'../Security/ManageSettings.aspx', NULL)
SET IDENTITY_INSERT [dbo].[RPRAccessPages] OFF

USE [vitalsigns]
GO

IF NOT EXISTS (select * from Settings WHERE sname='Use_NotesAPI_Mutex')
BEGIN	
	INSERT [dbo].[Settings] ([sname],[svalue],[stype]) VALUES ('Use_NotesAPI_Mutex','False','System.String')
END
GO


--CY: 06/09/2014 updated script
USE [vitalsigns]
GO
Update ProfilesMaster SET UnitOfMeasurement ='string' where ID in (60,66)
GO

USE [vitalsigns]
GO

-- DRS: HTTPS for traveler servers
IF NOT EXISTS (select * from syscolumns where name= 'RequireSSL' and id = object_id('dbo.DominoServers'))
BEGIN	
	ALTER TABLE DominoServers ADD RequireSSL BIT NULL
END
GO

IF NOT EXISTS (select * from syscolumns where name= 'ExternalAlias' and id = object_id('dbo.DominoServers'))
BEGIN	
	ALTER TABLE DominoServers ADD ExternalAlias VARCHAR(100) NULL
END
GO
 
-- VSPLUS-685 Traveler devices table should store LastSync time as a native date
IF EXISTS (select * from syscolumns where name= 'LastSyncTime' and id = object_id('dbo.Traveler_Devices'))
BEGIN	
	ALTER TABLE dbo.Traveler_Devices ALTER COLUMN LastSyncTime DATETIME
END
GO
 
IF EXISTS (select * from syscolumns where name= 'AlertClearedDateTime' and id = object_id('dbo.AlertSentDetails'))
BEGIN	
	ALTER TABLE dbo.AlertSentDetails ALTER COLUMN AlertClearedDateTime DATETIME 
END
GO

IF EXISTS (select * from syscolumns where name= 'AlertCreatedDateTime' and id = object_id('dbo.AlertSentDetails'))
BEGIN	
	ALTER TABLE dbo.AlertSentDetails ALTER COLUMN AlertCreatedDateTime DATETIME 
END
GO

/* 24-Jun-2014 Mukund modified for VE-23, ThresholdType - new column */
USE [vitalsigns]
GO
IF NOT EXISTS (select * from syscolumns where name= 'ThresholdType' and id = object_id('dbo.DiskSettings'))
BEGIN	
	ALTER TABLE dbo.DiskSettings ADD ThresholdType [varchar] (10) null
END
GO

 
USE [vitalsigns]
GO


IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[V_DiskSpace]'))
DROP VIEW [dbo].[V_DiskSpace]
GO

USE [vitalsigns]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW [dbo].[V_DiskSpace]
AS
SELECT     ServerName, DiskName, DiskFree, DiskSize, PercentFree, PercentUtilization, AverageQueueLength, Updated, ID, Threshold
FROM         dbo.DiskSpace
WHERE     (DiskFree IS NOT NULL) AND (DiskSize IS NOT NULL)
GO

--Mukund 25Jun14, VSPLUS-759: Modify BES form in configurator to support HA
IF NOT EXISTS (select * from syscolumns where name= 'HAOption' and id = object_id('dbo.BlackBerryServers'))
BEGIN	
	ALTER TABLE dbo.BlackBerryServers ADD HAOption [nvarchar] (100) null
END
GO	

IF Exists(SELECT * FROM BlackBerryServers where HAOption= NULL)
BEGIN
	Update dbo.BlackBerryServers Set HAOption = 'Stand Alone Server'
END
GO

IF NOT EXISTS (select * from syscolumns where name= 'HAPartner' and id = object_id('dbo.BlackBerryServers'))
BEGIN	
	ALTER TABLE dbo.BlackBerryServers ADD HAPartner [nvarchar](100) null
END
GO

IF Exists(SELECT * FROM BlackBerryServers where HAPartner = NULL)
BEGIN
	Update dbo.BlackBerryServers Set HAPartner = 0
END
GO

USE [vitalsigns]
GO
IF EXISTS (select * from syscolumns where name= 'DateTimeSent' and id = object_id('dbo.AlertHistory'))
BEGIN	
	ALTER TABLE dbo.AlertHistory DROP COLUMN DateTimeSent
END
GO

USE [vitalsigns]
GO

IF NOT EXISTS (select * from Settings where sname= 'SuppressMultiThread')
BEGIN	
	INSERT [dbo].[Settings] ([sname], [svalue], [stype]) VALUES (N'SuppressMultiThread', N'False', N'System.String')
END 
GO

/* If Supress Multi Thread already exists and is 0 or 1 then set the boolean value */
If (select svalue from Settings where sname= 'SuppressMultiThread') = '0'
BEGIN 
	update Settings set svalue = 'False' where sname= 'SuppressMultiThread'
END 
If (select svalue from Settings where sname= 'SuppressMultiThread') = '1'
BEGIN 
	update Settings set svalue = 'True' where sname= 'SuppressMultiThread'
END 


USE [vitalsigns]
GO
IF NOT EXISTS (select * from syscolumns where name= 'AlertKey' and id = object_id('dbo.AlertSentDetails'))
BEGIN	
	ALTER TABLE dbo.AlertSentDetails ADD AlertKey INT NULL
END
GO

USE [vitalsigns]
GO

IF NOT EXISTS (select * from Settings where sname='Log Level VSAdapter')
BEGIN	
	INSERT [dbo].[Settings] ([sname], [svalue], [stype]) VALUES (N'Log Level VSAdapter', N'2', N'System.Int32')
END 
GO

USE [vitalsigns]
GO
print 'Dailytasks QueryType column updated'
IF NOT EXISTS (select * from syscolumns where name= 'QueryType' and id = object_id('dbo.DailyTasks'))
BEGIN	
	ALTER TABLE dbo.DailyTasks ADD QueryType int
END
GO

IF EXISTS(SELECT * FROM syscolumns where id=Object_id('dailytasks'))
BEGIN
	UPDATE DailyTasks SET QueryType=1
	UPDATE DailyTasks SET QueryType=2 where SourceTableName = 'DeviceUpTimeStats' 
	UPDATE DailyTasks SET QueryType=3 where SourceTableName = 'DeviceDailyStats'
END
GO

/* 7/10/2014 NS added for VSPLUS-806 */
USE [vitalsigns]
GO

IF NOT EXISTS (select * from syscolumns where name= 'DatabaseName' and id = object_id('dbo.Traveler_HA_Datastore'))
BEGIN	
	ALTER TABLE dbo.Traveler_HA_Datastore ADD DatabaseName varchar(150) DEFAULT 'Traveler'
END
GO


--Mukund 14Jul14, VSPLUS-480
USE [vitalsigns]
GO

IF NOT EXISTS (select * from syscolumns where name= 'LicensesUsed' and id = object_id('dbo.Status'))
BEGIN	
	ALTER TABLE dbo.Status ADD LicensesUsed nvarchar(100)
END
GO

IF NOT EXISTS (select * from syscolumns where name= 'SRPConnectionn' and id = object_id('dbo.Status'))
BEGIN	
	ALTER TABLE dbo.Status ADD SRPConnectionn nvarchar(100)
END
GO



/* 7/14/2014 NS added new event for BES - VSPLUS-801 */
use vitalsigns
go
IF NOT EXISTS (select * from EventsMaster where EventName='HA - Failover' )
BEGIN	
	SET IDENTITY_INSERT [dbo].[EventsMaster] ON
	INSERT [dbo].[EventsMaster] ([ID], [EventName], [ServerTypeID]) VALUES (71, N'HA - Failover', 2)
	SET IDENTITY_INSERT [dbo].[EventsMaster] OFF
END
GO

--Mukund 24Jul14, VE
USE [vitalsigns]
GO

IF NOT EXISTS (select * from syscolumns where name= 'ScanDAGHealth' and id = object_id('dbo.ServerAttributes'))
BEGIN	
	ALTER TABLE dbo.ServerAttributes ADD ScanDAGHealth bit
END
GO


if exists(select * from information_schema.columns where table_name='ServerAttributes' AND Column_Name='MemThreshold' AND Data_Type = 'int')
begin
alter table ServerAttributes alter column MemThreshold float
end
go

if exists(select * from information_schema.columns where table_name='ServerAttributes' AND Column_Name='CPU_Threshold' AND Data_Type = 'int')
begin
alter table ServerAttributes alter column CPU_Threshold float
end
go

IF NOT EXISTS(select * from syscolumns where name='DateStamp' and id=object_id('dbo.WindowsServices'))
begin
ALTER TABLE WindowsServices ADD DateStamp datetime
end
go


DELETE FROM [dbo].[EventsMaster] where id in (72,73,74,75,76,77,78,79,80,81)
begin
	SET IDENTITY_INSERT [dbo].[EventsMaster] ON
	INSERT [dbo].[EventsMaster] ([ID], [EventName], [ServerTypeID]) VALUES (72, N'SMTP', 5)
	-- INSERT [dbo].[EventsMaster] ([ID], [EventName], [ServerTypeID]) VALUES (73, N'POP', 5)
	INSERT [dbo].[EventsMaster] ([ID], [EventName], [ServerTypeID]) VALUES (74, N'IMAP', 5)
	INSERT [dbo].[EventsMaster] ([ID], [EventName], [ServerTypeID]) VALUES (75, N'MAPO', 5)
	INSERT [dbo].[EventsMaster] ([ID], [EventName], [ServerTypeID]) VALUES (76, N'Active Sync', 5)
	INSERT [dbo].[EventsMaster] ([ID], [EventName], [ServerTypeID]) VALUES (77, N'Active Sync Devices', 5)
	INSERT [dbo].[EventsMaster] ([ID], [EventName], [ServerTypeID]) VALUES (78, N'Free Space', 5)
	INSERT [dbo].[EventsMaster] ([ID], [EventName], [ServerTypeID]) VALUES (79, N'Submission', 5)
	INSERT [dbo].[EventsMaster] ([ID], [EventName], [ServerTypeID]) VALUES (80, N'Poison', 5)
	INSERT [dbo].[EventsMaster] ([ID], [EventName], [ServerTypeID]) VALUES (81, N'Unreachable', 5)
	SET IDENTITY_INSERT [dbo].[EventsMaster] OFF
	update vitalsigns.dbo.Eventsmaster set EventName='Memory' where servertypeid=5 and eventname='RAM'
end
go


IF NOT EXISTS(select * from syscolumns where name='IsActive' and id=object_id('dbo.DAGDatabase'))
begin
ALTER TABLE DAGDatabase ADD IsActive varchar(50) NULL
end
go

USE [vitalsigns]
GO

IF NOT EXISTS (select * from syscolumns where id = object_id('dbo.ExchangeMailProbe'))
BEGIN	
	CREATE TABLE [dbo].[ExchangeMailProbe](
		[Enabled] [bit] NULL,
		[Name] [nvarchar](50) NOT NULL,
		[ExchangeMailAddress] [nvarchar](255) NOT NULL,
		[Category] [nvarchar]  (255) NULL,
		[ScanInterval] [int] NULL,
		[OffHoursScanInterval] [int] NULL,
		[DeliveryThreshold] [int] NULL,
		[RetryInterval] [int] NULL,
		[DestinationServerID] [int] NULL,
		[DestinationDatabase] [nvarchar] (50) NULL,
		[SourceServer] [nvarchar] (50) NULL,
		[EchoService] [bit] NULL,
		[ReplyTo] [nvarchar] (150) NULL,
		[FileName] [nvarchar] (255) NULL,
		PRIMARY KEY (Name)
) ON [PRIMARY]
END
GO

IF NOT EXISTS (select * from syscolumns where id = object_id('dbo.ExchangeMailProbeHistory'))
BEGIN	
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
END
GO


USE [VSS_Statistics]
GO

IF NOT EXISTS (select * from syscolumns where id = object_id('dbo.ExchangeMailStats'))
BEGIN

	CREATE TABLE [dbo].[ExchangeMailStats](
		[ID] [int] IDENTITY(1,1) NOT NULL,
		[Name] [nvarchar](50) NULL,
		[Date] [datetime] NULL,
		[StatName] [nvarchar](50) NULL,
		[StatValue] [float] NULL,
 	CONSTRAINT [ExchangeMailStats$PrimaryKey] PRIMARY KEY CLUSTERED 
	(
	[ID] ASC
	)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
	) ON [PRIMARY]
END
GO

USE [vitalsigns]
go

/* 10/2/2015 NS modified for VSPLUS-2182 */
IF EXISTS(select * from ServerTypes where ServerType='Exchange Mail Flow')
BEGIN
	SET IDENTITY_INSERT [dbo].[ServerTypes] ON
	DELETE FROM ServerTypes WHERE ServerType='Exchange Mail Flow'
	INSERT [dbo].[ServerTypes] ([ID], [ServerType], [ServerTypeTable], [FeatureId]) VALUES (14, N'Exchange Mail Flow','',2)
	SET IDENTITY_INSERT [dbo].[ServerTypes] OFF
END
go

IF NOT EXISTS(select * from ServerTypes where ServerType='Exchange Mail Flow')
BEGIN
	SET IDENTITY_INSERT [dbo].[ServerTypes] ON
	INSERT [dbo].[ServerTypes] ([ID], [ServerType], [ServerTypeTable], [FeatureId]) VALUES (14, N'Exchange Mail Flow','',2)
	SET IDENTITY_INSERT [dbo].[ServerTypes] OFF
END
go

/* 8/1/2014 NS added for VSPLUS-846 */
USE [vitalsigns]
GO

IF NOT EXISTS (select * from syscolumns where id = object_id('dbo.LyncServers'))
BEGIN	
	SET ANSI_NULLS ON

	SET QUOTED_IDENTIFIER ON
	
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


	ALTER TABLE [dbo].[LyncServers]  WITH CHECK ADD  CONSTRAINT [FK_LyncServers_Credentials] FOREIGN KEY([CredentialsID])
	REFERENCES [dbo].[Credentials] ([ID])

	ALTER TABLE [dbo].[LyncServers] CHECK CONSTRAINT [FK_LyncServers_Credentials]

	ALTER TABLE [dbo].[LyncServers]  WITH CHECK ADD  CONSTRAINT [FK_LyncServers_Servers] FOREIGN KEY([ServerID])
	REFERENCES [dbo].[Servers] ([ID])
	ON UPDATE CASCADE
	ON DELETE CASCADE

	ALTER TABLE [dbo].[LyncServers] CHECK CONSTRAINT [FK_LyncServers_Servers]
END
GO


IF NOT EXISTS (select * from syscolumns where name= 'CredentialsId' and id = object_id('dbo.ServerAttributes'))
BEGIN	
	ALTER TABLE dbo.ServerAttributes ADD CredentialsId int
END
GO
USE [vitalsigns]
go

IF EXISTS(select * from ServerTypes where ServerType='Lync')
BEGIN
	DELETE FROM ServerTypes WHERE ServerType='Lync'
END
go

USE [vitalsigns]
go

/* 9/15/2015 NS modified for VSPLUS-2160 */
IF EXISTS(select * from ServerTypes where ServerType='Skype for Business')
BEGIN
	SET IDENTITY_INSERT [dbo].[ServerTypes] ON
	DELETE FROM ServerTypes WHERE ServerType='Skype for Business'
	insert into ServerTypes (ID, ServerType, ServerTypeTable, FeatureId) values (15, 'Skype for Business', '', 13)
	SET IDENTITY_INSERT [dbo].[ServerTypes] OFF
END
go

IF NOT EXISTS(select * from ServerTypes where ServerType='Skype for Business')
BEGIN
	SET IDENTITY_INSERT [dbo].[ServerTypes] ON
	insert into ServerTypes (ID, ServerType, ServerTypeTable, FeatureId) values (15, 'Skype for Business', '', 13)
	SET IDENTITY_INSERT [dbo].[ServerTypes] OFF
END
go



/* 8/4/2014 NS added */
IF EXISTS(SELECT * FROM syscolumns where id=Object_id('ReportItems'))
BEGIN
	UPDATE ReportItems SET Category='Disk' WHERE Name='Disk Space Consumption' OR 
	Name='Disk Space Consumption Summary' OR
	Name='Domino Disk Space' OR
	Name='Domino Disk Health' OR
	Name='Domino Disk Health (by location)' OR
	Name='Server Disk Free Space' OR
	Name='Disk Space Availability'
END
GO

/* 8/5/2014 NS modified the Disk Space report name - removed Domino */
IF NOt EXISTS(SELECT * FROM ReportItems WHERE name= 'Server Disk Total Space')
BEGIN
	UPDATE ReportItems SET Name='Server Disk Total Space', Description='Total disk space by server (used and free)'
	WHERE Name='Domino Disk Space'
END
GO
IF EXISTS(SELECT * FROM syscolumns where id=Object_id('ReportItems'))
BEGIN
	UPDATE ReportItems SET Name='Disk Health', Description='Disk size and utilization by server'
	WHERE Name='Domino Disk Health'
END
GO
IF EXISTS(SELECT * FROM syscolumns where id=Object_id('ReportItems'))
BEGIN
	UPDATE ReportItems SET Name='Disk Health (by location)', Description='Disk size and utilization by location'
	WHERE Name='Domino Disk Health (by location)'
END
GO



IF NOT EXISTS (select * from syscolumns where name= 'ActiveSyncCredentialsId' and id = object_id('dbo.ExchangeSettings'))
BEGIN	
	ALTER TABLE dbo.ExchangeSettings ADD ActiveSyncCredentialsId int
END
GO

/* 8/7/2014 NS added for VSPLUS-853 */
IF NOT EXISTS (select * from syscolumns where name= 'CredentialsID' and id = object_id('dbo.DominoServers'))
BEGIN	
	ALTER TABLE DominoServers ADD CredentialsID int NULL
END
GO

/* 8/18/2014 NS added the if block around the alter statement */
IF  NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_DominoServers_Credentials]') AND parent_object_id = OBJECT_ID(N'[dbo].[DominoServers]'))
BEGIN
	ALTER TABLE [dbo].[DominoServers]  WITH CHECK ADD  CONSTRAINT [FK_DominoServers_Credentials] FOREIGN KEY([CredentialsID])
	REFERENCES [dbo].[Credentials] ([ID])
	ALTER TABLE [dbo].[DominoServers] CHECK CONSTRAINT [FK_DominoServers_Credentials]
END
GO




IF NOT EXISTS (select * from syscolumns where name= 'DeliveryTimeInMinutes' and id = object_id('dbo.NotesMailProbeHistory'))
BEGIN	
	ALTER TABLE dbo.NotesMailProbeHistory add [DeliveryTimeInMinutes] [numeric](10,0) NULL
END
GO


USE [vitalsigns]
GO

IF EXISTS(SELECT * FROM ProfilesMaster WHERE ID=32 AND UnitOfMeasurement='minutes')
BEGIN
	UPDATE ProfilesMaster SET UnitOfMeasurement=N'Percentage Used (eg:- for 90% = 0.90)', DefaultValue=.9 WHERE ID=32
END
GO

IF EXISTS(SELECT * FROM ProfilesMaster WHERE ID=58 AND UnitOfMeasurement='minutes')
BEGIN
	UPDATE ProfilesMaster SET UnitOfMeasurement=N'Percentage Used (eg:- for 90% = 0.90)', DefaultValue=.9 WHERE ID=58
END
GO

IF NOT EXISTS(select * from syscolumns where name='ServerRequired' and id=object_id('dbo.WindowsServices'))
begin
ALTER TABLE WindowsServices ADD ServerRequired bit
end
go

/* 8/11/2014 NS added for VSPLUS-856 - the script below is to accommodate for the new field on the 
Domino Properties page that will store Traveler account credentials. 
The credentials will come from the Credentials table vs. the Settings table. 
The script will copy the values for Domino HTTP User and Password from the Settings table into the 
Credentials table and update the DominoServers table with the values for all NULLs in CredentialsID.
The script should only be used one time for the upgrade from 1.3.5 to the next version. */
USE [vitalsigns]
go
IF EXISTS(SELECT * FROM Settings WHERE sname='Domino HTTP Password')
BEGIN
	SET IDENTITY_INSERT [dbo].Credentials ON
	INSERT INTO Credentials(ID,UserID,Password,AliasName) VALUES(100,
	(SELECT svalue FROM Settings WHERE sname='Domino HTTP User'),
	(SELECT svalue FROM Settings WHERE sname='Domino HTTP Password'),'Traveler')
	SET IDENTITY_INSERT [dbo].Credentials OFF
	UPDATE DominoServers SET CredentialsID=100 WHERE CredentialsID IS NULL
	DELETE FROM Settings WHERE sname='Domino HTTP Password'
	DELETE FROM Settings WHERE sname='Domino HTTP User'
END
GO


USE [VSS_Statistics]
GO
IF NOT EXISTS (select * from syscolumns where id = object_id('dbo.ExchangeMailFiles'))
BEGIN

SET ANSI_NULLS ON

SET QUOTED_IDENTIFIER ON

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

END
GO



USE [vitalsigns]
GO

IF NOT EXISTS (select * from syscolumns where id = object_id('dbo.ExchangeMailboxOverview'))
BEGIN

SET ANSI_NULLS ON

SET QUOTED_IDENTIFIER ON

SET ANSI_PADDING ON

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


SET ANSI_PADDING OFF

END
GO

--Mukund 19Aug14 VSPLUS-878:ServerName & ServerId field removed from Mail Services

USE [vitalsigns]
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_MailServices_Servers]') AND parent_object_id = OBJECT_ID(N'[dbo].[MailServices]'))
BEGIN
	ALTER TABLE [dbo].[MailServices] DROP CONSTRAINT [FK_MailServices_Servers]
END
GO
IF EXISTS (select * from syscolumns where name= 'ServerID' and id = object_id('dbo.MailServices'))
BEGIN
	Alter TABLE [dbo].[MailServices] Drop Column ServerID
END
GO
IF EXISTS (select * from syscolumns where name= 'ServerName' and id = object_id('dbo.MailServices'))
BEGIN
	Alter TABLE [dbo].[MailServices] Drop Column ServerName
END
GO


IF EXISTS( select * from syscolumns where id=object_id('dbo.EventsMaster'))
BEGIN
	SET IDENTITY_INSERT [dbo].[EventsMaster] ON
	DELETE from [dbo].[EventsMaster] where EventName = 'Monitoring Delay'
	INSERT INTO [dbo].[EventsMaster] ([ID], [EventName], [ServerTypeID]) VALUES (82, N'Monitoring Delay', 10)
	SET IDENTITY_INSERT [dbo].[EventsMaster] OFF
END
GO


USE [vitalsigns]
GO

IF NOT EXISTS (select * from syscolumns where id = object_id('dbo.Status_History'))
BEGIN

CREATE TABLE [dbo].[Status_History](
	[TypeANDName] [nvarchar](255) NULL,
	[OldStatus] [nvarchar](255) NULL,
	[OldStatusCode] [nvarchar](255) NULL,
	[NewStatus] [nvarchar](255) NULL,
	[NewStatusCode] [nvarchar](255) NULL,
	[DateStatusUpdated] [datetime] NULL
) ON [PRIMARY]

END
GO


-- VE-68 Exchange queue thresholds should be merged in the Configurator

-- drop these columns

IF EXISTS (select * from syscolumns where name= 'HubSubQThreshold' and id = object_id('dbo.ExchangeSettings'))
BEGIN
	Alter TABLE [dbo].[ExchangeSettings] Drop Column HubSubQThreshold
END
GO

IF EXISTS (select * from syscolumns where name= 'HubPoisonQThreshold' and id = object_id('dbo.ExchangeSettings'))
BEGIN
	Alter TABLE [dbo].[ExchangeSettings] Drop Column HubPoisonQThreshold
END
GO

IF EXISTS (select * from syscolumns where name= 'HubUnReachableQThreshold' and id = object_id('dbo.ExchangeSettings'))
BEGIN
	Alter TABLE [dbo].[ExchangeSettings] Drop Column HubUnReachableQThreshold
END
GO

IF EXISTS (select * from syscolumns where name= 'HubTotalQThreshold' and id = object_id('dbo.ExchangeSettings'))
BEGIN
	Alter TABLE [dbo].[ExchangeSettings] Drop Column HubTotalQThreshold
END
GO

IF EXISTS (select * from syscolumns where name= 'EdgeSubQThreshold' and id = object_id('dbo.ExchangeSettings'))
BEGIN
	Alter TABLE [dbo].[ExchangeSettings] Drop Column EdgeSubQThreshold
END
GO

IF EXISTS (select * from syscolumns where name= 'EdgePosQThreshold' and id = object_id('dbo.ExchangeSettings'))
BEGIN
	Alter TABLE [dbo].[ExchangeSettings] Drop Column EdgePosQThreshold
END
GO

IF EXISTS (select * from syscolumns where name= 'EdgeUnReachableQThreshold' and id = object_id('dbo.ExchangeSettings'))
BEGIN
	Alter TABLE [dbo].[ExchangeSettings] Drop Column EdgeUnReachableQThreshold
END
GO

IF EXISTS (select * from syscolumns where name= 'EdgeTotalQThreshold' and id = object_id('dbo.ExchangeSettings'))
BEGIN
	Alter TABLE [dbo].[ExchangeSettings] Drop Column EdgeTotalQThreshold
END
GO

-- new these columns

IF NOT EXISTS (select * from syscolumns where name= 'SubQThreshold' and id = object_id('dbo.ExchangeSettings'))
BEGIN
	Alter TABLE [dbo].[ExchangeSettings] ADD SubQThreshold INT NULL
END
GO

IF NOT EXISTS (select * from syscolumns where name= 'PoisonQThreshold' and id = object_id('dbo.ExchangeSettings'))
BEGIN
	Alter TABLE [dbo].[ExchangeSettings] ADD PoisonQThreshold INT NULL
END
GO

IF NOT EXISTS (select * from syscolumns where name= 'UnReachableQThreshold' and id = object_id('dbo.ExchangeSettings'))
BEGIN
	Alter TABLE [dbo].[ExchangeSettings] ADD UnReachableQThreshold INT NULL
END
GO

IF NOT EXISTS (select * from syscolumns where name= 'TotalQThreshold' and id = object_id('dbo.ExchangeSettings'))
BEGIN
	Alter TABLE [dbo].[ExchangeSettings] ADD TotalQThreshold INT NULL
END
GO
/* Nov 25 Niranjan vsplus-1189*/
IF NOT EXISTS (select * from syscolumns where name= 'LatencyYellowThreshold' and id = object_id('dbo.ExchangeSettings'))
BEGIN
	Alter TABLE [dbo].[ExchangeSettings] ADD LatencyYellowThreshold INT NULL
END
GO
IF NOT EXISTS (select * from syscolumns where name= 'LatencyRedThreshold' and id = object_id('dbo.ExchangeSettings'))
BEGIN
	Alter TABLE [dbo].[ExchangeSettings] ADD LatencyRedThreshold INT NULL
END
GO



USE [VSS_Statistics]
GO

IF EXISTS(SELECT * from sys.columns WHERE Name = N'Hournumber' and Object_ID = Object_ID(N'ExchangeDailySummaryStats'))
BEGIN	
	
	ALTER TABLE ExchangeDailySummaryStats DROP COLUMN "Hournumber"
END
GO


/* 9/18/2014 NS added for VSPLUS-951 */
USE [vitalsigns]
GO
--IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__ClusterDa__Clust__531856C7]') AND parent_object_id = OBJECT_ID(N'[dbo].[ClusterDatabaseDetails]'))
--ALTER TABLE [dbo].[ClusterDatabaseDetails] DROP CONSTRAINT [FK__ClusterDa__Clust__531856C7]
--GO

--Mukund 10-Oct-14:: Dropping not known FK constraint name
--commented above and wrote the if exists FK logic
--==========================================
IF  EXISTS (SELECT name 
  FROM sys.foreign_keys where object_id in (
 SELECT constraint_object_id FROM sys.foreign_key_columns fk 
    INNER JOIN sys.columns pc ON pc.object_id = fk.parent_object_id AND pc.column_id = fk.parent_column_id 
    INNER JOIN sys.columns rc ON rc.object_id = fk.referenced_object_id AND rc.column_id = fk.referenced_column_id
    WHERE fk.parent_object_id = object_id('ClusterDatabaseDetails') AND pc.name = 'ClusterID'
    AND fk.referenced_object_id = object_id('DominoCluster') AND rc.NAME = 'ID'
    ))
    Begin
    Declare @constraint varchar(100)
	SELECT @constraint=name FROM sys.foreign_keys where object_id in (
	 SELECT constraint_object_id FROM sys.foreign_key_columns fk 
    INNER JOIN sys.columns pc ON pc.object_id = fk.parent_object_id AND pc.column_id = fk.parent_column_id 
    INNER JOIN sys.columns rc ON rc.object_id = fk.referenced_object_id AND rc.column_id = fk.referenced_column_id
    WHERE fk.parent_object_id = object_id('ClusterDatabaseDetails') AND pc.name = 'ClusterID'
    AND fk.referenced_object_id = object_id('DominoCluster') AND rc.NAME = 'ID'
    )
	exec ('ALTER TABLE [dbo].[ClusterDatabaseDetails] DROP CONSTRAINT ['+ @constraint +']')
    End   

USE [vitalsigns]
GO

IF NOT EXISTS ( SELECT * FROM sys.foreign_key_columns fk 
    INNER JOIN sys.columns pc ON pc.object_id = fk.parent_object_id AND pc.column_id = fk.parent_column_id 
    INNER JOIN sys.columns rc ON rc.object_id = fk.referenced_object_id AND rc.column_id = fk.referenced_column_id
    WHERE fk.parent_object_id = object_id('ClusterDatabaseDetails') AND pc.name = 'ClusterID'
    AND fk.referenced_object_id = object_id('DominoCluster') AND rc.NAME = 'ID'
)
ALTER TABLE [dbo].[ClusterDatabaseDetails]  WITH CHECK ADD FOREIGN KEY([ClusterID])
REFERENCES [dbo].[DominoCluster] ([ID])
ON DELETE CASCADE
GO
--==========================================
/*07Oct14 Mukund added*/
USE [vitalsigns]
GO
IF NOT EXISTS(SELECT * from ServerTypes WHERE ServerType = N'Database Availability Group')
BEGIN	
	SET IDENTITY_INSERT [dbo].[ServerTypes] ON
	Insert into ServerTypes (ID,ServerType,ServerTypeTable) values(19,'Database Availability Group','')
	SET IDENTITY_INSERT [dbo].[ServerTypes] OFF
END
GO
/* WS Added missing exchange alerts */
IF NOT EXISTS( select * from EventsMaster where EventName='Reboot Overdue' AND ServerTypeId = 5 )
BEGIN
	SET IDENTITY_INSERT [dbo].[EventsMaster] ON
	INSERT [dbo].[EventsMaster] ([ID], [EventName], [ServerTypeID]) VALUES (83, N'Reboot Overdue', 5)
	INSERT [dbo].[EventsMaster] ([ID], [EventName], [ServerTypeID]) VALUES (84, N'Not Responding', 5)
	INSERT [dbo].[EventsMaster] ([ID], [EventName], [ServerTypeID]) VALUES (85, N'DAG Member Health', 19)
	INSERT [dbo].[EventsMaster] ([ID], [EventName], [ServerTypeID]) VALUES (86, N'DAG Database Health', 19)
	INSERT [dbo].[EventsMaster] ([ID], [EventName], [ServerTypeID]) VALUES (87, N'DAG Activation Preference', 19)
	SET IDENTITY_INSERT [dbo].[EventsMaster] OFF
END
GO

/*29Sep14 Mukund added*/
USE [vitalsigns]
GO
IF NOT EXISTS(SELECT * from ServerTypes WHERE ServerType = N'Windows')
BEGIN	
	SET IDENTITY_INSERT [dbo].[ServerTypes] ON
	Insert into ServerTypes (ID,ServerType,ServerTypeTable) values(16,'Windows','')
	SET IDENTITY_INSERT [dbo].[ServerTypes] OFF
END
GO
USE [vitalsigns]
go

IF NOT EXISTS(select * from ServerTypes where ServerType='Cloud')
BEGIN
	SET IDENTITY_INSERT [dbo].[ServerTypes] ON
	INSERT [dbo].[ServerTypes] ([ID], [ServerType], [ServerTypeTable], [FeatureId]) VALUES (17, N'Cloud','',15)
	SET IDENTITY_INSERT [dbo].[ServerTypes] OFF
END
go

if NOT exists (select * from syscolumns where id = object_id('dbo.WindowsServers'))
Begin
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
End
GO


/* WS added for changes to exchg mailprobe table and for issue VE-111 */

USE [vitalsigns]
GO

IF EXISTS (SELECT * from sys.columns WHERE Name = N'DestinationServerID' and Object_ID = Object_ID(N'ExchangeMailProbe'))
BEGIN	
	

	/****** Object:  Table [dbo].[ExchangeMailProbe]    Script Date: 10/02/2014 17:58:15 ******/
	IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ExchangeMailProbe]') AND type in (N'U'))
	DROP TABLE [dbo].[ExchangeMailProbe]
	

	
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
	
end
go


/* 10/2/2014 NS added for VE-36 */
IF EXISTS (select * from syscolumns where id = object_id('dbo.[ReportItems]'))
BEGIN
	UPDATE [ReportItems] SET [Description] = 'Daily server disk free space'
	WHERE [Name] = 'Server Disk Free Space'
END


/*VE-123 06Oct14 Mukund added*/
if NOT exists (select * from syscolumns where id = object_id('dbo.DagSettings'))
Begin
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
End
GO

--VSPLUS-970 Mukund added 07Oct14
if NOT exists (select * from syscolumns where id = object_id('dbo.CloudMaster'))
Begin
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
End
Go
if NOT exists (select * from syscolumns where id = object_id('dbo.CloudDetails'))
Begin
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
End
Go

USE [vitalsigns]
GO
IF NOT EXISTS (select * from syscolumns where name= 'UpdateAlert' and id = object_id('dbo.EventsMaster'))
BEGIN	
	ALTER TABLE dbo.EventsMaster ADD [UpdateAlert] [bit] NULL
END
GO	

USE [vitalsigns]
go

IF NOT EXISTS(select * from ServerTypes where ServerType='Active Directory')
BEGIN
	SET IDENTITY_INSERT [dbo].[ServerTypes] ON
	insert into ServerTypes (ID, ServerType, ServerTypeTable) values (18, 'Active Directory', '')
	SET IDENTITY_INSERT [dbo].[ServerTypes] OFF
END
go


--10/8/2014 WS VE-107
USE [vitalsigns]
GO
if NOT exists (select * from syscolumns where id = object_id('dbo.ExchangeDatabaseSettings'))
begin
CREATE TABLE [dbo].[ExchangeDatabaseSettings](
	[ServerName] [nvarchar](50) NOT NULL,
	[ServerTypeId] [nvarchar](50) NOT NULL,
	[DatabaseName] [nvarchar](50) NOT NULL,
	[WhiteSpaceThreshold] [nvarchar](50) NOT NULL,
	[DatabaseSizeThreshold] [nvarchar](50) NOT NULL
) ON [PRIMARY]
end
GO

IF NOT EXISTS( select * from EventsMaster where EventName='Mailbox Send Prohibited' AND ServerTypeId = 5 )
BEGIN
	SET IDENTITY_INSERT [dbo].[EventsMaster] ON
	INSERT [dbo].[EventsMaster] ([ID], [EventName], [ServerTypeID]) VALUES (89, N'Mailbox Send Prohibited', 5)
	SET IDENTITY_INSERT [dbo].[EventsMaster] OFF
END
GO
IF NOT EXISTS( select * from EventsMaster where EventName='Mailbox Receive Prohibited' AND ServerTypeId = 5 )	
BEGIN
	SET IDENTITY_INSERT [dbo].[EventsMaster] ON
	INSERT [dbo].[EventsMaster] ([ID], [EventName], [ServerTypeID]) VALUES (90, N'Mailbox Receive Prohibited', 5)
	SET IDENTITY_INSERT [dbo].[EventsMaster] OFF
END
GO


--Mukund 10-Oct-14 Cloud Master Data : PRE-REQUISITE
USE [vitalsigns]
GO
DELETE FROM [CloudMaster]
GO
SET IDENTITY_INSERT [dbo].[CloudMaster] ON
--VSPLUS 2452 Sowjanya
INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (1, N'4 Marketing', N'~/images/Cloud_Apps/4demit.png', N'http://www.4marketing.it')
INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (2, N'Salesforce', N'~/images/Cloud_Apps/salesforce.png', N'http://www.salesforce.com')
INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (3, N'SugarCRM', N'~/images/Cloud_Apps/sugar_crm.png', N'http://www.sugarcrm.com')
INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (4, N'123 Contactform', N'~/images/Cloud_Apps/123contactform.png', N'http://www.123contactform.com')
INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (5, N'Activecampaign', N'~/images/Cloud_Apps/active_campaign.png', N'http://www.activecampaign.com')
INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (6, N'Activecollab', N'~/images/Cloud_Apps/activecollab.png', N'http://www.activecollab.com')
INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (7, N'Amazon Simple Storage', N'~/images/Cloud_Apps/amazon_simple_storage_service.png', N'http://aws.amazon.com')
INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (8, N'Acuity Scheduling', N'~/images/Cloud_Apps/acuity_scheduling.png', N'https://www.acuityscheduling.com')
INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (9, N'Airbrake', N'~/images/Cloud_Apps/airbrake.png', N'https://airbrake.io')
INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (10, N'Ambassador', N'~/images/Cloud_Apps/ambassador.png', N'https://getambassador.com')
INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (11, N'Agilecrm', N'~/images/Cloud_Apps/agile_crm.png', N'https://www.agilecrm.com')
INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (12, N'Agilezen', N'~/images/Cloud_Apps/agilezen.png', N'http://www.agilezen.com')
INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (13, N'Aim', N'~/images/Cloud_Apps/aim.png', N'http://www.aim.com')
INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (14, N'Analytic Call Tracking', N'~/images/Cloud_Apps/analytic_call_tracking.png', N'http://www.analyticcalltracking.com')
INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (15, N'Andriod', N'~/images/Cloud_Apps/android.png', N'http://www.android.com')
INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (16, N'AngelList', N'~/images/Cloud_Apps/angellist.png', N'https://angel.co')
INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (17, N'Appdotnet', N'~/images/Cloud_Apps/appdotnet.png', N'https://app.net')
INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (18, N'Appointlet', N'~/images/Cloud_Apps/appointlet.png', N'https://www.appointlet.com')
INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (19, N'Asana', N'~/images/Cloud_Apps/asana.png', N'https://asana.com')
INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (20, N'Authorize', N'~/images/Cloud_Apps/authorize-net.png', N'http://www.authorize.net')
INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (21, N'Aweber', N'~/images/Cloud_Apps/aweber.png', N'http://www.aweber.com')
INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (22, N'Balancedpayments', N'~/images/Cloud_Apps/balanced-payments.png', N'https://www.balancedpayments.com')
INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (23, N'Basecamp', N'~/images/Cloud_Apps/basecamp.png', N'https://basecamp.com')
INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (24, N'Base_crm', N'~/images/Cloud_Apps/base_crm.png', N'https://getbase.com')
INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (25, N'Batchbook', N'~/images/Cloud_Apps/batchbook.png', N'http://batchbook.com')
INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (26, N'Bigcommerce', N'~/images/Cloud_Apps/bigcommerce.png', N'https://www.bigcommerce.com')
INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (27, N'Beanstalk', N'~/images/Cloud_Apps/beanstalk.png', N'http://beanstalkapp.com')
INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (28, N'Bettervoice', N'~/images/Cloud_Apps/better_voicemail.png', N'https://www.bettervoice.com')
INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (29, N'Bitbucket', N'~/images/Cloud_Apps/bitbucket.png', N'https://bitbucket.org')
INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (30, N'Blogger', N'~/images/Cloud_Apps/blogger.png', N'https://www.blogger.com')
INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (31, N'Bombbombcom', N'~/images/Cloud_Apps/bombbombcom.png', N'http://www.bombbomb.com')
INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (32, N'Box', N'~/images/Cloud_Apps/box.png', N'https://www.box.com')
INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (33, N'Braintree', N'~/images/Cloud_Apps/braintree.png', N'https://www.braintreepayments.com')
INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (34, N'Buffer', N'~/images/Cloud_Apps/buffer.png', N'https://bufferapp.com')
INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (35, N'Bugherd', N'~/images/Cloud_Apps/bugherd.png', N'http://bugherd.com')
INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (36, N'Calldrip', N'~/images/Cloud_Apps/call_drip.png', N'http://www.calldrip.com')
INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (37, N'Call Tracking Metrics', N'~/images/Cloud_Apps/call_tracking_metrics.png', N'https://calltrackingmetrics.com')
INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (38, N'Callfire', N'~/images/Cloud_Apps/callfire.png', N'https://www.callfire.com')
INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (39, N'Callrail', N'~/images/Cloud_Apps/callrail.png', N'https://www.callrail.com')
INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (40, N'Capaignmonitor', N'~/images/Cloud_Apps/campaign_monitor.png', N'https://www.campaignmonitor.com')
INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (41, N'Campfire', N'~/images/Cloud_Apps/campfire.png', N'https://campfirenow.com')
INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (42, N'Capsule_crm', N'~/images/Cloud_Apps/capsule_crm.png', N'http://capsulecrm.com')
INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (43, N'Castingwords', N'~/images/Cloud_Apps/castingwords.png', N'https://castingwords.com')
INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (44, N'Catchwind_sms', N'~/images/Cloud_Apps/catchwind_sms.png', N'http://www.catchwind.com')
INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (45, N'Cerb', N'~/images/Cloud_Apps/cerb.png', N'http://www.cerberusweb.com')
INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (46, N'Chargebee', N'~/images/Cloud_Apps/chargebee.png', N'https://www.chargebee.com')
INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (47, N'Changeover', N'~/images/Cloud_Apps/chargeover.png', N'http://chargeover.com')
INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (48, N'Chargify', N'~/images/Cloud_Apps/chargify.png', N'http://chargify.com')
INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (49, N'Checkvist', N'~/images/Cloud_Apps/checkvist.png', N'https://checkvist.com')
INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (50, N'Cheddargetter', N'~/images/Cloud_Apps/cheddargetter.png', N'https://cheddargetter.com')
INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (51, N'Choir_io', N'~/images/Cloud_Apps/choir_io.png', N'https://choir.io')
INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (52, N'Clickatell', N'~/images/Cloud_Apps/clickatell.png', N'https://github.com/Clickatell')
INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (53, N'Clio', N'~/images/Cloud_Apps/clio.png', N'www.goclio.com')
INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (54, N'Close-io', N'~/images/Cloud_Apps/close_io.png', N'www.quora.com/Close-io')
INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (55, N'Codeplex', N'~/images/Cloud_Apps/codeplex.png', N'https://www.codeplex.com')
INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (56, N'Coinbase', N'~/images/Cloud_Apps/coinbase.png', N'https://zapier.com/zapbook/coinbase')
INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (57, N'Constant Contact', N'~/images/Cloud_Apps/constant_contact.png', N'www.constantcontact.com')
INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (58, N'Contactually', N'~/images/Cloud_Apps/contactually.png', N'https://www.contactually.com')
INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (59, N'Contentful', N'~/images/Cloud_Apps/contentful.png', N'https://www.contentful.com')
INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (60, N'Credit_repair_cloud', N'~/images/Cloud_Apps/credit_repair_cloud.png', N'https://www.creditrepaircloud.com')
INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (61, N'Crisply', N'~/images/Cloud_Apps/crisply.png', N'www.jobscore.com/jobs/.../bTADNsxOar4OP1iGakhP3Q')
INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (62, N'Crittercism', N'~/images/Cloud_Apps/crittercism.png', N'www.crittercism.com')
INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (63, N'Datadog', N'~/images/Cloud_Apps/datadog.png', N'https://www.datadoghq.com')
INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (64, N'Datasift', N'~/images/Cloud_Apps/datasift.png', N'datasift.com')
INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (65, N'Delicious', N'~/images/Cloud_Apps/delicious.png', N'https://delicious.com')
INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (66, N'Deputy', N'~/images/Cloud_Apps/deputy.png', N'www.deputy.com')
INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (67, N'Desk', N'~/images/Cloud_Apps/desk.png', N'www.desk.com')
INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (68, N'Diffbot', N'~/images/Cloud_Apps/diffbot.png', N'https://www.diffbot.com')
INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (69, N'Digital ocean', N'~/images/Cloud_Apps/digital-ocean.png', N'www.digitalocean.com')
INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (70, N'Discourse', N'~/images/Cloud_Apps/discourse.png', N'www.discourse.org')
INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (71, N'Displet-retsidx', N'~/images/Cloud_Apps/displet-retsidx.png', N'displet.com')
INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (72, N'Disqus', N'~/images/Cloud_Apps/Disqus.png', N'https://disqus.com')
INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (73, N'Docusign', N'~/images/Cloud_Apps/Disqus.png', N'https://www.docusign.com')
INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (74, N'Drip', N'~/images/Cloud_Apps/drip.png', N'https://www.getdrip.com')
INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (75, N'Dropbox', N'~/images/Cloud_Apps/dropbox.png', N'https://www.dropbox.com')
INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (76, N'Drupal', N'~/images/Cloud_Apps/drupal.png', N'https://www.drupal.org')
INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (77, N'Ducksboard', N'~/images/Cloud_Apps/ducksboard.png', N'https://ducksboard.com')
INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (78, N'Dwolla', N'~/images/Cloud_Apps/dwolla.png', N'https://www.dwolla.com')
INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (79, N'Dynamodb', N'~/images/Cloud_Apps/dynamodb.png', N'http://aws.amazon.com/dynamodb')
--VSPLUS 2357 somaraju 
INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (80, N'Ebay', N'~/images/Cloud_Apps/ebay.png', N'http://www.ebay.in')
INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (81, N'Echosign', N'~/images/Cloud_Apps/echosign.png', N'www.magimetrics.com/integration/echosign')
INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (82, N'Ecwid', N'~/images/Cloud_Apps/ecwid.png', N'www.ecwid.com')
INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (83, N'E-goi', N'~/images/Cloud_Apps/e-goi.png', N'www.e-goi.com')
INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (84, N'E-junkie', N'~/images/Cloud_Apps/e-junkie.png', N'www.e-junkie.com')
INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (85, N'Email', N'~/images/Cloud_Apps/email.png', N'www.gmail.com')
INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (86, N'Email-parser', N'~/images/Cloud_Apps/email_parser.png', N'http://www.email2db.com')
INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (87, N'Emaildirect', N'~/images/Cloud_Apps/emaildirect.png', N'www.emaildirect.com')
INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (88, N'Emma', N'~/images/Cloud_Apps/emma.png', N'http://myemma.com')
INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (89, N'Etsy', N'~/images/Cloud_Apps/etsy.png', N'https://www.etsy.com')
INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (90, N'Eve-online', N'~/images/Cloud_Apps/eve_online.png', N'www.eveonline.com')
INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (91, N'Eventbrite', N'~/images/Cloud_Apps/eventbrite.png', N'https://www.eventbrite.com')
INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (92, N'Ever_note', N'~/images/Cloud_Apps/ever_note.png', N'https://www.evernote.com')
INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (93, N'Evernote-business', N'~/images/Cloud_Apps/evernote_business.png', N'https://evernote.com/business/features')
INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (94, N'Exacttarget', N'~/images/Cloud_Apps/exacttarget.png', N'http://www.exacttarget.com')
INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (95, N'Exceptionless', N'~/images/Cloud_Apps/exceptionless.png', N'exceptionless.com')
INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (96, N'Exchange', N'~/images/Cloud_Apps/exchange.png', N'https://zapier.com/zapbook/exchange')
INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (97, N'Expensify', N'~/images/Cloud_Apps/expensify.png', N'https://www.expensify.com')
INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (98, N'Facebook', N'~/images/Cloud_Apps/facebook_page.png', N'https://www.facebook.com')
INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (99, N'Facebook Pages', N'~/images/Cloud_Apps/facebook-pages.png', N'https://www.facebook.com/about/pages')
INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (100, N'Feedly', N'~/images/Cloud_Apps/feedly.png', N'https://feedly.com')
GO
print 'Processed 100 Cloud Application records'
INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (101, N'Feng-Office', N'~/images/Cloud_Apps/feng-office.png', N'www.fengoffice.com')
INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (102, N'Findmyshift', N'~/images/Cloud_Apps/findmyshift.png', N'https://www.findmyshift.com')
INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (103, N'Firebase', N'~/images/Cloud_Apps/firebase.png', N'https://www.firebase.com')
INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (104, N'Flowdock', N'~/images/Cloud_Apps/flowdock.png', N'https://www.flowdock.com')
INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (105, N'Fluidsurveys', N'~/images/Cloud_Apps/fluidsurveys.png', N'https://zapier.com/zapbook/fluidsurveys')
INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (106, N'Fogbugz', N'~/images/Cloud_Apps/fogbugz.png', N'www.fogcreek.com/fogbugz/')
INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (107, N'Follow-up-boss', N'~/images/Cloud_Apps/follow-up-boss.png', N'www.followupboss.com')
INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (108, N'Followup', N'~/images/Cloud_Apps/follow_up_boss.png', N'https://followup.cc')
INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (109, N'Formdesk', N'~/images/Cloud_Apps/formdesk.png', N'https://en.formdesk.com')
INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (110, N'Formforall', N'~/images/Cloud_Apps/formforall.png', N'www.formforall.com')
INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (111, N'Formidable', N'~/images/Cloud_Apps/formidable.png', N'https://zapier.com/zapbook/formidable')
INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (112, N'Formitize', N'~/images/Cloud_Apps/formitize.png', N'formitize.com')
INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (113, N'Formstack', N'~/images/Cloud_Apps/formstack.png', N'https://www.formstack.com')
INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (114, N'Foursquare', N'~/images/Cloud_Apps/foursquare.png', N'https://zapier.com/zapbook/foursquare')
INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (115, N'Foxycart', N'~/images/Cloud_Apps/foxycart.png', N'www.foxycart.com')
INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (116, N'Freckle', N'~/images/Cloud_Apps/freckle.png', N'https://letsfreckle.com')
INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (117, N'Freeagent', N'~/images/Cloud_Apps/freeagent.png', N'www.freeagent.com')
INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (118, N'Freshbooks', N'~/images/Cloud_Apps/freshbooks.png', N'www.freshbooks.com')
INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (119, N'Freshdesk', N'~/images/Cloud_Apps/freshdesk.png', N'freshdesk.com')
INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (120, N'Fullcontact', N'~/images/Cloud_Apps/fullcontact.png', N'https://www.fullcontact.com')
INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (121, N'Geckoboard', N'~/images/Cloud_Apps/geckoboard.png', N'https://www.geckoboard.com')
INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (122, N'Genoo', N'~/images/Cloud_Apps/genoo.png', N'www.genoo.com')
INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (123, N'Getresponse', N'~/images/Cloud_Apps/getresponse.png', N'http://www.getresponse.com')
INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (124, N'Getsatisfaction', N'~/images/Cloud_Apps/get-satisfaction.png', N'http://www.getsatisfaction.com')
INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (125, N'Github', N'~/images/Cloud_Apps/github.png', N'https://github.com/moshen/Image-Term256Color')
INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (126, N'Gmail', N'~/images/Cloud_Apps/gmail.png', N'http://www.gmail.com')
INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (127, N'Google Calendar', N'~/images/Cloud_Apps/google_calendar.png', N'https://www.google.com/calendar')
INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (128, N'Google Docs', N'~/images/Cloud_Apps/google_docs.png', N'https://docs.google.com')
INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (129, N'Google Drive', N'~/images/Cloud_Apps/google_drive.png', N'https://drive.google.com')
INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (130, N'Google Glass', N'~/images/Cloud_Apps/google_glass.png', N'https://www.google.com/glass')
INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (131, N'Google Adwords', N'~/images/Cloud_Apps/google-adwords.png', N'https://www.google.com/adwords')
INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (132, N'Google Plus', N'~/images/Cloud_Apps/google-plus.png', N'https://plus.google.com')
INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (133, N'Google Sheets', N'~/images/Cloud_Apps/google-sheets.png', N'www.google.co.in/sheets')
INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (134, N'Esponce', N'~/images/Cloud_Apps/esponce.png', N'www.esponce.com')
INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (135, N'GoToMeeting', N'~/images/Cloud_Apps/gotomeeting.png', N'www.gotomeeting.in')
INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (136, N'GoToTraining', N'~/images/Cloud_Apps/gototraining.png', N'www.gototraining.com/fec/training/online_training_solutions')
INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (137, N'GoToWebinar', N'~/images/Cloud_Apps/gotowebinar.png', N'www.joinwebinar.com/fec/?locale=en_US&set=true')
INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (138, N'Gravityforms', N'~/images/Cloud_Apps/gravity_forms.png', N'www.gravityforms.com')
INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (139, N'Gtalk', N'~/images/Cloud_Apps/gtalk.png', N'www.google.com/talk/whatsnew_more.html')
INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (140, N'Gumroad', N'~/images/Cloud_Apps/gumroad.png', N'https://gumroad.com')
INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (141, N'Hall', N'~/images/Cloud_Apps/hall.png', N'https://hall.com')
INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (142, N'Happyfox', N'~/images/Cloud_Apps/happyfox.png', N'https://www.happyfox.com')
INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (143, N'Harvest', N'~/images/Cloud_Apps/harvest.png', N'https://www.getharvest.com')
INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (144, N'Help Scout', N'~/images/Cloud_Apps/help_scout.png', N'www.helpscout.net')
INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (145, N'Highrise', N'~/images/Cloud_Apps/highrise.png', N'https://highrisehq.com')
INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (146, N'Hipchat', N'~/images/Cloud_Apps/hipchat.png', N'https://www.hipchat.com')
INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (147, N'Hoiio', N'~/images/Cloud_Apps/hoiio.png', N'www.hoiio.com')
INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (148, N'Hootsuite', N'~/images/Cloud_Apps/hootsuite.png', N'https://hootsuite.com')
INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (149, N'Hotspotsystem', N'~/images/Cloud_Apps/hotspotsystem.png', N'www.hotspotsystem.com')
INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (150, N'Hubspot', N'~/images/Cloud_Apps/hubspot.png', N'www.hubspot.com')
INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (151, N'Hcontact', N'~/images/Cloud_Apps/icontact.png', N'www.icontact.com')
INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (152, N'Iformbuilder', N'~/images/Cloud_Apps/iformbuilder.png', N'https://www.iformbuilder.com')
INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (153, N'Inbound-now', N'~/images/Cloud_Apps/iformbuilder.png', N'http://www.inboundnow.com')
INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (154, N'Infusionsoft', N'~/images/Cloud_Apps/infusionsoft.png', N'www.infusionsoft.com')
INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (155, N'Ininbox', N'~/images/Cloud_Apps/ininbox.png', N'www.ininbox.com')
INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (156, N'Jira', N'~/images/Cloud_Apps/jira.png', N'www.jira.com')
/* vsplus 1236 Sowjanya*/
INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (158, N'Runscope', N'~/images/Cloud_Apps/runscope.png', N'https://www.runscope.com')
INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (159, N'Salesforce', N'~/images/Cloud_Apps/Salesforce.png', N'http://salesforce.com')
--INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (160, N'Samanage', N'~/images/Cloud_Apps/samanage.png', NULL)
--INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (161, N'Sazneo', N'~/images/Cloud_Apps/sazneo.png', NULL)
--INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (162, N'Schedule', N'~/images/Cloud_Apps/schedule.png', NULL)
--INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (163, N'Scout', N'~/images/Cloud_Apps/scout.png', NULL)
INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (164, N'Semantria', N'~/images/Cloud_Apps/semantria.png', N'https://semantria.com')
INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (165, N'Sendgrid', N'~/images/Cloud_Apps/sendgrid.png', N'https://sendgrid.com')
INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (166, N'Sendhub', N'~/images/Cloud_Apps/sendhub.png', N'https://www.sendhub.com')
--INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (167, N'Sendicate', N'~/images/Cloud_Apps/sendicate.png', NULL)
INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (168, N'Sendloop', N'~/images/Cloud_Apps/sendloop.png', N'https://sendloop.com')
--INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (169, N'Setmore-appointment', N'~/images/Cloud_Apps/setmore.png', NULL)
INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (170, N'Sharepoint', N'~/images/Cloud_Apps/Sharepoint.png', N'http://sharepoint.protiviti.com')
INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (171, N'Shipwire', N'~/images/Cloud_Apps/shipwire.png', N'http://www.shipwire.com')
INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (172, N'Shopify', N'~/images/Cloud_Apps/shopify.png', N'http://www.shopify.in')
--INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (173, N'Signals', N'~/images/Cloud_Apps/Signals.png', NULL)
INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (174, N'Sign-upto', N'~/images/Cloud_Apps/sign-upto.png', N'http://www.signupto.com')
INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (175, N'Silverpop', N'~/images/Cloud_Apps/silverpop.png', N'http://www.silverpop.com')
--INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (176, N'Sina', N'~/images/Cloud_Apps/sina.png', NULL)
--INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (177, N'Sirporly', N'~/images/Cloud_Apps/sirporly.png', NULL)
--INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (178, N'Site24', N'~/images/Cloud_Apps/site24.png', NULL)
INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (179, N'Siteleaf', N'~/images/Cloud_Apps/siteleaf.png', N'http://www.siteleaf.com')
--INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (180, N'Skyedrive', N'~/images/Cloud_Apps/skyedrivef.png', NULL)
INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (181, N'Slack', N'~/images/Cloud_Apps/slack.png', N'https://slack.com')
INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (182, N'Slideshare', N'~/images/Cloud_Apps/slideshare.png', N'http://www.slideshare.net')
INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (183, N'Smartsheet', N'~/images/Cloud_Apps/smartsheet.png', N'http://www.smartsheet.com')
--INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (184, N'Smtp', N'~/images/Cloud_Apps/smtp.png', NULL)
--INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (185, N'Snappy', N'~/images/Cloud_Apps/snappy.png', NULL)
INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (186, N'Solve360', N'~/images/Cloud_Apps/solve360.png', N'https://solve360.com')
INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (187, N'Soundcloud', N'~/images/Cloud_Apps/soundcloud.png', N'https://soundcloud.com')
INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (188, N'Sprintly', N'~/images/Cloud_Apps/sprintly.png', N'https://sprint.ly')
--INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (189, N'Sql_server', N'~/images/Cloud_Apps/sql_server.png', NULL)
INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (190, N'Squarespace', N'~/images/Cloud_Apps/squarespace.png', N'http://www.squarespace.com')
INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (191, N'Sqwiggle', N'~/images/Cloud_Apps/sqwiggle.png', N'https://www.sqwiggle.com')
INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (192, N'Stacklead', N'~/images/Cloud_Apps/stacklead.png', N'https://stacklead.com')
--INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (193, N'Statsmix', N'~/images/Cloud_Apps/Statsmix.png', NULL)
INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (194, N'Statuscake', N'~/images/Cloud_Apps/Statuscake.png', N'https://www.statuscake.com')
INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (195, N'Stocktwits', N'~/images/Cloud_Apps/stocktwits.png', N'http://stocktwits.com')
INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (196, N'Storenvy', N'~/images/Cloud_Apps/storenvy.png', N'http://www.storenvy.com')
INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (197, N'streak', N'~/images/Cloud_Apps/streak.png', N'https://www.streak.com')
--INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (198, N'Stride', N'~/images/Cloud_Apps/stride.png', NULL)
INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (199, N'stripe', N'~/images/Cloud_Apps/stripe.png', N'https://stripe.com')
--INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (200, N'sugarcrm', N'~/images/Cloud_Apps/sugarcrm.png', N'http://www.sugarcrm.com')
GO
--INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (201, NULL, NULL, N'http://www.sugarcrm.com')
INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (202, N'Strideapp', N'~/images/Cloud_Apps/strideapp.png', N'http://blog.strideapp.com')
--INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (203, N'Stripe', N'~/images/Cloud_Apps/stripe.png', N'https://stripe.com')
--INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (204, N'Sugarcrm', N'~/images/Cloud_Apps/sugarcrm.png', N'http://www.sugarcrm.com')
INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (205, N'Sugarsync', N'~/images/Cloud_Apps/sugarsync.png', N'https://www.sugarsync.com')
INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (206, N'Supersaas', N'~/images/Cloud_Apps/supersaas.png', N'http://www.supersaas.com')
--INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (207, N'Surveygizmo', N'~/images/Cloud_Apps/surveygizmo.png', NULL)
INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (208, N'Surveymethods', N'~/images/Cloud_Apps/surveymethods.png', N'https://www.surveymethods.com')
INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (209, N'Surveymonkey', N'~/images/Cloud_Apps/surveymonkey.png', N'https://www.surveymonkey.com')
INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (210, N'Surveypal', N'~/images/Cloud_Apps/Surveypal.png', N'http://www.surveypal.com')
INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (211, N'Talentlms', N'~/images/Cloud_Apps/talentlms.png', N'http://www.talentlms.com')
INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (212, N'Targetprocess', N'~/images/Cloud_Apps/targetprocess.png', N'http://www.targetprocess.com')
INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (213, N'Taskrabbit', N'~/images/Cloud_Apps/taskrabbit.png', N'https://www.taskrabbit.com')
INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (214, N'Totango', N'~/images/Cloud_Apps/Totango.png', N'http://www.totango.com')
--INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (215, N'Teambox', N'~/images/Cloud_Apps/Teambox.png', NULL)
INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (216, N'Teamwork', N'~/images/Cloud_Apps/teamwork_project_manager.png', N'https://www.teamwork.com')
INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (217, N'Tel_api', N'~/images/Cloud_Apps/tel_api.png', N'http://www.telapi.com')
INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (218, N'Tende-support', N'~/images/Cloud_Apps/tender-support.png', N'https://tenderapp.com')
INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (219, N'Tibbr', N'~/images/Cloud_Apps/Tibbr.png', N'http://www.tibbr.com')
INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (220, N'Todoist', N'~/images/Cloud_Apps/Todoist.png', N'http://todoist.com')
INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (221, N'Toggl', N'~/images/Cloud_Apps/Toggl.png', N'https://www.toggl.com')
INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (222, N'Toodledo', N'~/images/Cloud_Apps/toodledo.png', N'http://www.toodledo.com')
INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (223, N'Totango', N'~/images/Cloud_Apps/totango.png', N'http://www.totango.com')
INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (224, N'Trackvia', N'~/images/Cloud_Apps/Trackvia.png', N'https://www.trackvia.com')
INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (225, N'Trello', N'~/images/Cloud_Apps/trello.png', N'https://trello.com')
INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (226, N'Triggerapp', N'~/images/Cloud_Apps/triggerapp.png', N'https://www.triggerapp.com')
--INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (227, N'Tropical', N'~/images/Cloud_Apps/tropical.png', NULL)
INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (228, N'Tumblr', N'~/images/Cloud_Apps/tumblr.png', N'https://www.tumblr.com')
INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (229, N'Twilio', N'~/images/Cloud_Apps/twilio.png', N'https://www.twilio.com')
INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (230, N'Twitter', N'~/images/Cloud_Apps/twitter.png', N'https://about.twitter.com')
INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (231, N'typeform', N'~/images/Cloud_Apps/typeform.png', N'https://www.typeform.com')
INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (232, N'Unbounce', N'~/images/Cloud_Apps/unbounce.png', N'http://unbounce.com')
INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (233, N'Unison', N'~/images/Cloud_Apps/unison.png', N'https://www.unison.com')
--INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (234, N'Unleashed-software', N'~/images/Cloud_Apps/Unleashed-software.png', NULL)
INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (235, N'Uservoice', N'~/images/Cloud_Apps/uservoice.png', N'https://www.uservoice.com')
INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (236, N'Verticalresponse', N'~/images/Cloud_Apps/verticalresponse.png', N'http://www.verticalresponse.com')
INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (237, N'Vextras', N'~/images/Cloud_Apps/Vextras.png', N'https://www.vextras.com')
INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (238, N'Vimeo', N'~/images/Cloud_Apps/vimeo.png', N'https://vimeo.com')
INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (239, N'Vision6', N'~/images/Cloud_Apps/vision6.png', N'www.vision6.com')
INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (240, N'vision-helpdesk', N'~/images/Cloud_Apps/vision-helpdesk.png', N'https://www.visionhelpdesk.com')
INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (241, N'Visual_studio_online', N'~/images/Cloud_Apps/visual_studio_online.png', N'http://www.visualstudio.com')
INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (242, N'Visual-lease', N'~/images/Cloud_Apps/visual-lease.png', N'http://www.visuallease.com')
INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (243, N'Vouchfor', N'~/images/Cloud_Apps/vouchfor-refer-a-friend.png', N'https://vouchfor.com')
--INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (244, N'Vitiger', N'~/images/Cloud_Apps/vitigerr.png', NULL)
--INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (245, N'Webhooks', N'~/images/Cloud_Apps/webhooksr.png', NULL)
INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (246, N'Webmerge', N'~/images/Cloud_Apps/webmerge.png', N'https://www.webmerge.me')
INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (247, N'weebly', N'~/images/Cloud_Apps/weebly.png', N'http://www.weebly.com')
--INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (248, N'week_plan', N'~/images/Cloud_Apps/week_plan.png', NULL)
INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (249, N'Whmcs', N'~/images/Cloud_Apps/whmcs.png', N'http://whmcs.com')
INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (250, N'Windows', N'~/images/Cloud_Apps/windows-azure-web-sites.png', N'http://windows.microsoft.com')
INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (251, N'Wiredmarketing', N'~/images/Cloud_Apps/wired-marketing.png', N'http://www.wiredmarketing.co.uk')
INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (252, N'Wistia', N'~/images/Cloud_Apps/Wistia.png', N'http://wistia.com')
--INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (253, N'Woocommerse', N'~/images/Cloud_Apps/woocommerse.png', NULL)
INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (254, N'Wordpress', N'~/images/Cloud_Apps/wordpress.png', N'https://wordpress.org')
INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (255, N'Wp_remote', N'~/images/Cloud_Apps/wp-remote.png', N'https://wpremote.com/')
INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (256, N'Wrike', N'~/images/Cloud_Apps/wrike.png', N'https://www.wrike.com')
INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (257, N'Wufoo', N'~/images/Cloud_Apps/Wufoo.png', N'http://www.wufoo.com')
INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (258, N'Xero', N'~/images/Cloud_Apps/xero.png', N'https://www.xero.com')
INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (259, N'Yammer', N'~/images/Cloud_Apps/yammer.png', N'https://www.yammer.com')
INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (260, N'Youtube', N'~/images/Cloud_Apps/youtube.png', N'https://www.youtube.com')
INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (261, N'Zapier', N'~/images/Cloud_Apps/zapier.png', N'https://zapier.com')
INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (262, N'Zendesk', N'~/images/Cloud_Apps/zendesk.png', N'https://www.zendesk.com')
INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (263, N'Zillow', N'~/images/Cloud_Apps/zillow.png', N'http://zillow.mediaroom.com')
INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (264, N'Zoho', N'~/images/Cloud_Apps/zoho.png', N'https://www.zoho.com')
INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (265, N'Zoho_creator', N'~/images/Cloud_Apps/zoho_creator.png', N'https://www.zoho-creator.com')
INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (266, N'Zoho-invoices', N'~/images/Cloud_Apps/zoho_invoices.png', N'https://zapier.com/zapbook/zoho-invoices')
INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (267, N'Zuora', N'~/images/Cloud_Apps/zuora.png', N'https://www.zuora.com')







INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (268, N'Jive', N'~/images/Cloud_Apps/jive.png', N'https://www.jive.com')
INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (269, N'Joomla', N'~/images/Cloud_Apps/joomla.png', N'https://www.joomla.com')
INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (270, N'Jot_form', N'~/images/Cloud_Apps/jot_form.png', N'https://www.jotform.com')
INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (271, N'Jumplead', N'~/images/Cloud_Apps/jumplead.png', N'https://www.jumplead.com')
INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (272, N'Kanbanery', N'~/images/Cloud_Apps/kanbanery.png', N'https://www.kanbanery.com')
INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (273, N'Kanbantool', N'~/images/Cloud_Apps/kanbantool.png', N'https://www.kanbantool.com')
INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (274, N'Kato', N'~/images/Cloud_Apps/kato.png', N'https://www.kato.com')
INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (275, N'Keep_in_touch', N'~/images/Cloud_Apps/keep_in_touch.png', N'https://www.keepintouch.com')
INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (276, N'Kickofflabs', N'~/images/Cloud_Apps/kickofflabs.png', N'https://www.kickofflabs.com')
INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (277, N'Kippt', N'~/images/Cloud_Apps/kippt.png', N'https://www.kippt.com')
INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (278, N'Kissmetrics', N'~/images/Cloud_Apps/kissmetrics.png', N'https://www.kissmetrics.com')
INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (279, N'Knack', N'~/images/Cloud_Apps/knack.png', N'https://www.knack.com')
INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (280, N'Lander', N'~/images/Cloud_Apps/lander.png', N'https://www.lander.com')
INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (281, N'Leadersend', N'~/images/Cloud_Apps/leadersend.png', N'https://www.leadersend.com')
INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (282, N'Leadsimple', N'~/images/Cloud_Apps/leadsimple.png', N'https://www.leadsimple.com')
INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (283, N'Leankit', N'~/images/Cloud_Apps/leankit.png', N'https://www.leankit.com')
INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (284, N'Leftronic', N'~/images/Cloud_Apps/leftronic.png', N'https://www.leftronic.com')
INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (285, N'Less_accounting', N'~/images/Cloud_Apps/less_accounting.png', N'https://www.lessaccounting.com')
INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (286, N'Lighthouse', N'~/images/Cloud_Apps/lighthouse.png', N'https://www.lighthouse.com')
INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (287, N'Linkedin', N'~/images/Cloud_Apps/linkedin.png', N'https://www.linkedin.com')
INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (288, N'Liquid_planner', N'~/images/Cloud_Apps/liquid_planner.png', N'https://www.liquidplanner.com')
INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (289, N'Livechat', N'~/images/Cloud_Apps/livechat.png', N'https://www.livechat.com')
INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (290, N'Lockitron', N'~/images/Cloud_Apps/lockitron.png', N'https://www.lockitron.com')
INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (291, N'Mad_mimi', N'~/images/Cloud_Apps/mad_mimi.png', N'https://www.mad_mimi.com')
INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (292, N'Magento', N'~/images/Cloud_Apps/magento.png', N'https://www.magento.com')
INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (293, N'Magi_metrics', N'~/images/Cloud_Apps/magi_metrics.png', N'https://www.magimetrics.com')
INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (294, N'Mail_parser_io', N'~/images/Cloud_Apps/mail_parser_io.png', N'https://www.mailparserio.com')
INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (295, N'Mailchimp', N'~/images/Cloud_Apps/mailchimp.png', N'https://www.mailchimp.com')
INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (296, N'Mailgun', N'~/images/Cloud_Apps/mailgun.png', N'https://www.mailgun.com')
INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (297, N'Mailigen', N'~/images/Cloud_Apps/mailigen.png', N'https://www.mailigen.com')
INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (298, N'Mailup', N'~/images/Cloud_Apps/mailup.png', N'https://www.mailup.com')
INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (299, N'Mandrill', N'~/images/Cloud_Apps/mandrill.png', N'https://www.mandrill.com')
INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (300, N'Mavenlink', N'~/images/Cloud_Apps/mavenlink.png', N'https://www.mavenlink.com')
GO
INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (301, N'Meetup', N'~/images/Cloud_Apps/meetup.png', N'https://www.meetup.com')
INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (302, N'Mention', N'~/images/Cloud_Apps/mention.png', N'https://www.mention.com')
INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (303, N'Message-bus', N'~/images/Cloud_Apps/message-bus.png', N'https://www.messagebus.com')
INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (304, N'Microsoft-access', N'~/images/Cloud_Apps/microsoft-access.png', N'https://www.microsoftaccess.com')
INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (305, N'Microsoft-dynamics', N'~/images/Cloud_Apps/microsoft-dynamics.png', N'https://www.microsoftdynamics.com')
INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (306, N'Mindbody', N'~/images/Cloud_Apps/mindbody.png', N'https://www.mindbody.com')
INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (307, N'Mixpanel', N'~/images/Cloud_Apps/mixpanel.png', N'https://www.mixpanel.com')
INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (308, N'Mobileworks', N'~/images/Cloud_Apps/mobileworks.png', N'https://www.mobileworks.com')
--INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (309, N'Mobyt_sms', N'~/images/Cloud_Apps/mobyt_sms.png', N'')
INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (310, N'Mojo_helpdesk', N'~/images/Cloud_Apps/mojo_helpdesk.png', N'https://www.mojohelpdesk.com')
INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (311, N'Mongodb', N'~/images/Cloud_Apps/mongodb.png', N'https://www.mongodb.com')
INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (312, N'Monitis', N'~/images/Cloud_Apps/monitis.png', N'https://www.monitis.com')
INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (313, N'Myphoneroom', N'~/images/Cloud_Apps/myphoneroom.png', N'https://www.myphoneroom.com')
INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (314, N'Mysql', N'~/images/Cloud_Apps/mysql.png', N'https://www.mysql.com')
--INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (315, N'Netsuite', N'~/images/Cloud_Apps/netsuite.png', N'')
INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (316, N'New_relic', N'~/images/Cloud_Apps/new_relic.png', N'https://www.newrelic.com')
INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (317, N'New-relic-insights', N'~/images/Cloud_Apps/new-relic-insights.png', N'https://www.newrelicinsights.com')
INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (318, N'Nimble', N'~/images/Cloud_Apps/nimble.png', N'https://www.nimble.com')
INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (319, N'Ning', N'~/images/Cloud_Apps/ning.png', N'https://www.ning.com')
--INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (320, N'Noti', N'~/images/Cloud_Apps/noti.png', N'')
INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (321, N'Notify_my_android', N'~/images/Cloud_Apps/notify_my_android.png', N'https://www.notifymyandroid.com')
INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (322, N'Nozbe', N'~/images/Cloud_Apps/nozbe.png', N'https://www.nozbe.com')
INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (323, N'Numerous', N'~/images/Cloud_Apps/numerous.png', N'https://www.numerous.com')
INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (324, N'Nutshell-crm', N'~/images/Cloud_Apps/nutshell-crm.png', N'https://www.nutshell-crm.com')
INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (326, N'Obsurvey', N'~/images/Cloud_Apps/obsurvey.png', N'https://www.obsurvey.com')
INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (327, N'Odesk', N'~/images/Cloud_Apps/odesk.png', N'https://www.odesk.com')
INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (328, N'Odoo', N'~/images/Cloud_Apps/odoo.png', N'https://www.odoo.com')
INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (329, N'Office-auto-pilot', N'~/images/Cloud_Apps/office-auto-pilot.png', N'https://www.officeautopilot.com')
INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (330, N'Olark', N'~/images/Cloud_Apps/olark.png', N'https://www.olark.com')
INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (331, N'Onedrive', N'~/images/Cloud_Apps/onedrive.png', N'https://www.onedrive.com')
INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (332, N'Onenote', N'~/images/Cloud_Apps/onenote.png', N'https://www.onenote.com')
INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (333, N'Onepage_crm', N'~/images/Cloud_Apps/onepage_crm.png', N'https://www.onepagecrm.com')
INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (334, N'Openerp', N'~/images/Cloud_Apps/openerp.png', N'https://www.openerp.com')
INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (335, N'Opsgenie', N'~/images/Cloud_Apps/opsgenie.png', N'https://www.opsgenie.com')
INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (336, N'Orbtr', N'~/images/Cloud_Apps/orbtr.png', N'https://www.orbtr.com')
INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (337, N'Osmosis', N'~/images/Cloud_Apps/osmosis.png', N'https://www.osmosis.com')
INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (338, N'Outlook-com', N'~/images/Cloud_Apps/outlook-com.png', N'https://www.outlookcom.com')
INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (339, N'Pagerduty', N'~/images/Cloud_Apps/pagerduty.png', N'https://www.pagerduty.com')
INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (340, N'Papyrs', N'~/images/Cloud_Apps/papyrs.png', N'https://www.papyrs.com')
INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (341, N'Pardot', N'~/images/Cloud_Apps/pardot.png', N'https://www.pardot.com')
INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (342, N'Parse', N'~/images/Cloud_Apps/parse.png', N'https://www.parse.com')
INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (343, N'Paymill', N'~/images/Cloud_Apps/paymill.png', N'https://www.paymill.com')
INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (344, N'Paypal', N'~/images/Cloud_Apps/paypal.png', N'https://www.paypal.com')
INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (345, N'Phaxio', N'~/images/Cloud_Apps/phaxio.png', N'https://www.phaxio.com')
INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (346, N'Phone', N'~/images/Cloud_Apps/phone.png', N'https://www.phone.com')
INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (347, N'Pinboard', N'~/images/Cloud_Apps/pinboard.png', N'https://www.pinboard.com')
INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (348, N'Pingdom', N'~/images/Cloud_Apps/pingdom.png', N'https://www.pingdom.com')
INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (349, N'Pinterest', N'~/images/Cloud_Apps/pinterest.png', N'https://www.pinterest.com')
INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (350, N'Pipedrive', N'~/images/Cloud_Apps/pipedrive.png', N'https://www.pipedrive.com')
INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (351, N'Pipelinedeals', N'~/images/Cloud_Apps/pipelinedeals.png', N'https://www.pipelinedeals.com')
--INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (352, N'Pivotal-tracker', N'~/images/Cloud_Apps/pivotal-tracker.png', N'')
INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (353, N'Pocket', N'~/images/Cloud_Apps/pocket.png', N'https://www.pocket.com')
INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (354, N'Podio', N'~/images/Cloud_Apps/podio.png', N'https://www.podio.com')
INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (355, N'Positionly', N'~/images/Cloud_Apps/positionly.png', N'https://www.positionly.com')
INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (356, N'Postgresql', N'~/images/Cloud_Apps/postgresql.png', N'https://www.postgresql.com')
INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (357, N'Prefinery', N'~/images/Cloud_Apps/prefinery.png', N'https://www.prefinery.com')
INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (358, N'Prodpad', N'~/images/Cloud_Apps/prodpad.png', N'https://www.prodpad.com')
INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (359, N'Product_hunt', N'~/images/Cloud_Apps/product_hunt.png', N'https://www.producthunt.com')
INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (360, N'Producteev', N'~/images/Cloud_Apps/producteev.png', N'https://www.producteev.com')
--INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (361, N'Projectplace', N'~/images/Cloud_Apps/projectplace.png', N'')
INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (362, N'Pushbullet', N'~/images/Cloud_Apps/pushbullet.png', N'https://www.pushbullet.com')
INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (363, N'Pushover', N'~/images/Cloud_Apps/pushover.png', N'https://www.pushover.com')
INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (364, N'Pushwoosh', N'~/images/Cloud_Apps/pushwoosh.png', N'https://www.pushwoosh.com')
INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (365, N'Qortex', N'~/images/Cloud_Apps/qortex.png', N'https://www.qortex.com')
INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (366, N'Qualaroo', N'~/images/Cloud_Apps/qualaroo.png', N'https://www.qualaroo.com')
INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (367, N'Quickbase', N'~/images/Cloud_Apps/quickbase.png', N'https://www.quickbase.com')
INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (368, N'Quickbooks', N'~/images/Cloud_Apps/quickbooks.png', N'https://www.quickbooks.com')
INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (369, N'Quip', N'~/images/Cloud_Apps/quip.png', N'https://www.quip.com')
INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (370, N'Quote_roller', N'~/images/Cloud_Apps/quote_roller.png', N'https://www.quoteroller.com')
--INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (371, N'Rainforest-qa', N'~/images/Cloud_Apps/rainforest-qa.png', N'')
INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (372, N'Raven-tools', N'~/images/Cloud_Apps/raven-tools.png', N'https://www.raventools.com')
INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (373, N'reamaze', N'~/images/Cloud_Apps/reamaze.png', N'https://www.reamaze.com')
INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (374, N'Recurly', N'~/images/Cloud_Apps/recurly.png', N'https://www.recurly.com')
INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (375, N'Redbooth', N'~/images/Cloud_Apps/redbooth.png', N'https://www.redbooth.com')
INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (376, N'Reddit', N'~/images/Cloud_Apps/reddit.png', N'https://www.reddit.com')
INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (377, N'Redmine', N'~/images/Cloud_Apps/redmine.png', N'https://www.redmine.com')
INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (378, N'Relateiq', N'~/images/Cloud_Apps/relateiq.png', N'https://www.relateiq.com')
INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (379, N'Requirementone', N'~/images/Cloud_Apps/requirementone.png', N'https://www.requirementone.com')
INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (380, N'Rescuetime', N'~/images/Cloud_Apps/rescuetime.png', N'https://www.rescuetime.com')
--INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (381, N'Rethinkdb', N'~/images/Cloud_Apps/rethinkdb.png', N'')
INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (382, N'Rev', N'~/images/Cloud_Apps/rev.png', N'https://www.rev.com')
INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (383, N'Rezdy', N'~/images/Cloud_Apps/rezdy.png', N'https://www.rezdy.com')
INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (384, N'Ronin', N'~/images/Cloud_Apps/ronin.png', N'https://www.ronin.com')
INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (385, N'Roost', N'~/images/Cloud_Apps/roost.png', N'https://www.roost.com')
INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (386, N'Rss', N'~/images/Cloud_Apps/Rss.png', N'https://www.rss.com')
INSERT [dbo].[CloudMaster] ([ID], [Name], [Image], [Url]) VALUES (387, N'Run-my-accounts', N'~/images/Cloud_Apps/run-my-accounts.png', N'https://www.runmyaccounts.ch')

SET IDENTITY_INSERT [dbo].[CloudMaster] OFF


--WS MOVED DAG EVENTS TO DAG SERVER TYPE
USE [vitalsigns]
GO
IF NOT EXISTS( select * from EventsMaster where EventName='DAG Member Health' AND ServerTypeId = 5 )
BEGIN
  update EventsMaster set ServerTypeId = (select id from ServerTypes Where ServerType='Database Availability Group') where ID in (85, 86, 87)
END
GO

	
/* 1473 swathi*/
USE [vitalsigns]
GO
IF EXISTS (select * from syscolumns where name= 'LatencyMonitor' and id = object_id('dbo.ServerTypes'))
BEGIN	
	ALTER TABLE dbo.ServerTypes DROP COLUMN LatencyMonitor 
END
GO


USE [vitalsigns]
GO
IF NOT EXISTS(SELECT * from ServerTypes WHERE ServerType = N'SNMP Devices')
BEGIN	
	SET IDENTITY_INSERT [dbo].[ServerTypes] ON
	INSERT [dbo].[ServerTypes] ([ID], [ServerType], [ServerTypeTable], [FeatureId]) VALUES (20,N'SNMP Devices',N'SNMP',10)
	SET IDENTITY_INSERT [dbo].[ServerTypes] OFF
END
GO

IF EXISTS(select * from syscolumns where name= 'FeatureId' and id = object_id('dbo.ServerTypes'))
BEGIN	
	UPDATE ServerTypes SET FeatureId = 1 WHERE ServerType = 'Domino'
	UPDATE ServerTypes SET FeatureId = 1 WHERE ServerType = 'Notes Database'
	UPDATE ServerTypes SET FeatureId = 2 WHERE ServerType = 'Exchange'
	UPDATE ServerTypes SET FeatureId = 2 WHERE ServerType = 'Database Availability Group'
	UPDATE ServerTypes SET FeatureId = 3 WHERE ServerType = 'BES'
	UPDATE ServerTypes SET FeatureId = 4 WHERE ServerType = 'Mail'
	UPDATE ServerTypes SET FeatureId = 5 WHERE ServerType = 'SharePoint'
	UPDATE ServerTypes SET FeatureId = 6 WHERE ServerType = 'Sametime'
	UPDATE ServerTypes SET FeatureId = 8 WHERE ServerType = 'URL'
	UPDATE ServerTypes SET FeatureId = 10 WHERE ServerType = 'Network Device'
	UPDATE ServerTypes SET FeatureId = 11 WHERE ServerType = 'General'
	UPDATE ServerTypes SET FeatureId = 10 WHERE ServerType = 'SNMP'
END
GO

USE [vitalsigns]
GO
UPDATE EventsMaster SET ServerTypeID=19 WHERE EventName IN('DAG Member Health','DAG Database Health','DAG Activation Preference')
go


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



USE [vitalsigns]
GO

IF NOT EXISTS( select * from EventsMaster where EventName='DAG Copy Queue' AND ServerTypeId = 19 )
BEGIN
	SET IDENTITY_INSERT [dbo].[EventsMaster] ON
	INSERT [dbo].[EventsMaster] ([ID], [EventName], [ServerTypeID]) VALUES (91, N'DAG Copy Queue', 19)
	INSERT [dbo].[EventsMaster] ([ID], [EventName], [ServerTypeID]) VALUES (92, N'DAG Replay Queue', 19)
	SET IDENTITY_INSERT [dbo].[EventsMaster] OFF
END
GO

IF NOT EXISTS( select * from EventsMaster where EventName='Mailbox Database Size' AND ID = 88 )
BEGIN
	SET IDENTITY_INSERT [dbo].[EventsMaster] ON
	UPDATE [dbo].[EventsMaster] set EventName='Mailbox Database Size' where ID='88'
	SET IDENTITY_INSERT [dbo].[EventsMaster] OFF
END
GO

IF NOT EXISTS( select * from EventsMaster where EventName='Auto Discovery' AND ID = 93 )
BEGIN
	SET IDENTITY_INSERT [dbo].[EventsMaster] ON
	INSERT [dbo].[EventsMaster] ([ID], [EventName], [ServerTypeID]) VALUES (93, N'Auto Discovery', 19)
	UPDATE [dbo].[EventsMaster] set EventName='MAPI' WHERE ServerTypeID=5 AND EventName='MAPO'
	SET IDENTITY_INSERT [dbo].[EventsMaster] OFF
END
GO
IF NOT EXISTS (select * from syscolumns where name= 'DevicesAPIStatus' and id = object_id('dbo.TRAVELER_STATUS'))
	ALTER TABLE dbo.TRAVELER_STATUS ADD DevicesAPIStatus VARCHAR(25) NULL
GO

/******Mukund:21-Oct-14, Object:  Table [dbo].[ActiveDirectoryTest]    Script Date: 10/20/2014 17:50:55 ******/

USE [vitalsigns]
GO
if NOT exists (select * from syscolumns where id = object_id('dbo.ActiveDirectoryTest'))
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
	[QuorumGroup] [nvarchar](200) NULL,
	[FileShareQuorum] [nvarchar](200) NULL,
	[DBCopySuspend] [nvarchar](200) NULL,
	[DBDisconnected] [nvarchar](200) NULL,
	[DBLogCopyKeepingUP] [nvarchar](200) NULL,
	[DBLogReplayKeepingUP] [nvarchar](200) NULL,
 CONSTRAINT [PK_ActiveDirectoryTest] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO


--WS 10/21/14 VE-143
USE [vitalsigns]
GO
if NOT exists (select * from syscolumns where id = object_id('dbo.DAGQueueThresholds'))
BEGIN
CREATE TABLE [dbo].[DAGQueueThresholds](
	[DAGName] [nvarchar](50) NOT NULL,
	[ServerName] [nvarchar](50) NOT NULL,
	[ServerTypeId] [nvarchar](50) NOT NULL,
	[DatabaseName] [nvarchar](50) NOT NULL,
	[ReplayQueueThreshold] [nvarchar](50) NOT NULL,
	[CopyQueueThreshold] [nvarchar](50) NOT NULL
) ON [PRIMARY]
END
GO

/* 10/21/2014 NS added for VSPLUS-730 */
IF NOT EXISTS (select * from Settings where sname= 'AlertsRepeatOn' )
BEGIN	
	INSERT [dbo].[Settings] ([sname], [svalue], [stype]) VALUES (N'AlertsRepeatOn', N'false', N'System.String')
END
GO

IF NOT EXISTS (select * from Settings where sname= 'AlertsRepeatOccurrences' )
BEGIN	
	INSERT [dbo].[Settings] ([sname], [svalue], [stype]) VALUES (N'AlertsRepeatOccurrences', N'3', N'System.String')
END
GO

USE [vitalsigns]
GO
if NOT exists (select * from syscolumns where id = object_id('dbo.AlertRepeatOccurrence'))
BEGIN
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
END
GO


USE [vitalsigns]
GO
IF NOT EXISTS (select * from syscolumns where name= 'AlertOnRepeat' and id = object_id('dbo.EventsMaster'))
BEGIN	
	ALTER TABLE dbo.EventsMaster ADD [AlertOnRepeat] [bit] NULL
END
GO

USE [VSS_Statistics]
GO
if exists (select * from syscolumns where id = object_id('dbo.ExchangeDailyStats'))
BEGIN
	DROP TABLE dbo.ExchangeDailyStats
END
GO


USE [VSS_Statistics]
GO
if exists (select * from syscolumns where id = object_id('dbo.ExchangeSummaryStats'))
BEGIN
	DROP TABLE dbo.ExchangeSummaryStats
END
GO
	
USE [VSS_Statistics]
GO
if exists (select * from syscolumns where id = object_id('dbo.LyncDailyStats'))
BEGIN
	DROP TABLE dbo.LyncDailyStats
END
GO

USE [VSS_Statistics]
GO
if exists (select * from syscolumns where id = object_id('dbo.LyncSummaryStats'))
BEGIN
	DROP TABLE dbo.LyncSummaryStats
END
GO

USE [VSS_Statistics]
GO
if exists (select * from syscolumns where id = object_id('dbo.ActiveDirectorycDailyStats'))
BEGIN
	DROP TABLE dbo.ActiveDirectorycDailyStats
END
GO

USE [VSS_Statistics]
GO
if exists (select * from syscolumns where id = object_id('dbo.ActiveDirectorySummaryStats'))
BEGIN
	DROP TABLE dbo.ActiveDirectorySummaryStats
END
GO

USE [VSS_Statistics]
GO
if exists (select * from syscolumns where id = object_id('dbo.SharepointDailyStats'))
BEGIN
	DROP TABLE dbo.SharepointDailyStats
END
GO

USE [VSS_Statistics]
GO
if exists (select * from syscolumns where id = object_id('dbo.SharepointSummaryStats'))
BEGIN
	DROP TABLE dbo.SharepointSummaryStats
END
GO

USE [VSS_Statistics]
GO
if exists (select * from syscolumns where id = object_id('dbo.WindowsDailyStats'))
BEGIN
	DROP TABLE dbo.WindowsDailyStats
END
GO

USE [VSS_Statistics]
GO
if exists (select * from syscolumns where id = object_id('dbo.WindowsSummaryStats'))
BEGIN
	DROP TABLE dbo.WindowsSummaryStats
END
GO

USE [VSS_Statistics]
GO
if exists (select * from syscolumns where id = object_id('dbo.ExchangeDailySummaryStats'))
BEGIN
	DROP TABLE dbo.ExchangeDailySummaryStats
END
GO
--VSPLUS-913,Mukund 22Oct14, Stop counting mail, if over threshold
USE [vitalsigns]
GO
IF NOT EXISTS (select * from syscolumns where name= 'CheckMailThreshold' and id = object_id('dbo.DominoServers'))
BEGIN	
	ALTER TABLE dbo.DominoServers  ADD CheckMailThreshold int null
END
GO	


--WS ADDED 10/22/2014
USE [VSS_Statistics]
GO
IF NOT EXISTS (select * from syscolumns where name= 'ServerTypeId' and id = object_id('dbo.MicrosoftDailyStats'))
BEGIN	
	IF EXISTS (select * from syscolumns where id = object_id('dbo.MicrosoftDailyStats'))
	BEGIN
		DROP TABLE dbo.MicrosoftDailyStats
	END

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
END
GO

IF NOT EXISTS (select * from syscolumns where name= 'ServerTypeId' and id = object_id('dbo.MicrosoftSummaryStats'))
BEGIN	
	IF EXISTS (select * from syscolumns where id = object_id('dbo.MicrosoftSummaryStats'))
	BEGIN	
		DROP TABLE [dbo].[MicrosoftSummaryStats]
	END
	
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
END

GO
--Mukund:29Oct14 Below fields for VSPLUS-1124 - Allow log file scanning to be performed on selective servers
USE [vitalsigns]
GO
IF NOT EXISTS (select * from syscolumns where name= 'ServerTypeId' and id = object_id('dbo.WindowsServices'))
BEGIN	
	ALTER TABLE dbo.WindowsServices  ADD ServerTypeId int null
END
GO

IF NOT EXISTS (select * from syscolumns where name= 'FileWitnessServerStatus' and id = object_id('dbo.DagStatus'))
BEGIN	
	ALTER TABLE dbo.DagStatus  ADD FileWitnessServerStatus varchar(200) null
END
GO


if NOT exists (select * from syscolumns where id = object_id('dbo.DagDatabaseDetails'))
BEGIN
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
END
GO

--Mukund 29Oct14-Below fields for VSPLUS-1124 - Allow log file scanning to be performed on selective servers
IF NOT EXISTS(select * from syscolumns where name='scanlog' and id=object_id('dbo.DominoServers'))
begin
ALTER TABLE DominoServers ADD scanlog bit
end
go

IF  EXISTS(select * from syscolumns where name='scanantilog' and id=object_id('dbo.DominoServers'))
begin
ALTER TABLE DominoServers Drop Column scanantilog 
end
go

IF NOT EXISTS(select * from syscolumns where name='scanagentlog' and id=object_id('dbo.DominoServers'))
begin
ALTER TABLE DominoServers ADD scanagentlog bit
end
go

--Somaraj:VSPLUS-1028-CustomBackground
USE [vitalsigns]
GO
IF NOT EXISTS(select * from syscolumns where name='CustomBackground' and id=object_id('dbo.Users'))
begin
ALTER TABLE Users ADD CustomBackground varchar(200) NULL
end
go

USE [vitalsigns]
GO
IF NOT EXISTS(select * from syscolumns where name='LastScanDate' and id=object_id('dbo.ActiveDirectoryTest'))
begin
ALTER TABLE ActiveDirectoryTest ADD LastScanDate datetime NULL
end
go
--Durga:VSPLUS-1203-

USE [vitalsigns]
GO
IF NOT EXISTS(select * from syscolumns where name='ImageURL' and id=object_id('dbo.SNMPDevices'))
begin
ALTER TABLE SNMPDevices ADD ImageURL nvarchar(200) NULL
end 
IF NOT EXISTS(select * from syscolumns where name='IncludeOnDashBoard' and id=object_id('dbo.SNMPDevices'))
begin
ALTER TABLE SNMPDevices ADD IncludeOnDashBoard bit NULL
end 
GO
IF NOT EXISTS(select * from syscolumns where name='ImageURL' and id=object_id('dbo.[Network Devices]'))
begin
ALTER TABLE [Network Devices] ADD ImageURL nvarchar(200) NULL
end
IF NOT EXISTS(select * from syscolumns where name='IncludeOnDashBoard' and id=object_id('dbo.[Network Devices]'))
begin
ALTER TABLE [Network Devices] ADD IncludeOnDashBoard bit NULL
end

--Mukund:VSPLUS-78 added NotRequiredKeyword 03Nov14
USE [vitalsigns]
GO
IF NOT EXISTS(select * from syscolumns where name='NotRequiredKeyword' and id=object_id('dbo.LogFile'))
begin
ALTER TABLE LogFile ADD NotRequiredKeyword nvarchar(255) NULL
end
go


-- 11/12/2014 Mouli VSPLUS 1119--
USE [vitalsigns]
GO
/****** Object:  Table [dbo].[SharePointSettings]    Script Date: 11/12/2014 11:34:35 ******/
If NOT exists (select * from syscolumns where id = object_id('dbo.SharePointSettings'))
CREATE TABLE [dbo].[SharePointSettings](
	[ServerId] [int]  NOT NULL,
	[Sname] [nvarchar](250) NULL,
	[Svalue] [nvarchar](250) NULL
)
GO
/****** Object:  Table [dbo].[NetworkMaster]    Script Date: 11/25/2014 17:32:10 ******/

USE [vitalsigns]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
If NOT exists (select * from syscolumns where id = object_id('dbo.NetworkMaster'))
CREATE TABLE [dbo].[NetworkMaster](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](max) NULL,
	[Image] [nvarchar](100) NULL
) ON [PRIMARY]
GO
USE [vitalsigns]
GO
Delete from NetworkMaster
Go
--Sowjanya 1852
SET IDENTITY_INSERT [dbo].[NetworkMaster] ON
IF NOT EXISTS(select * from NetworkMaster where name='Device Network' and id=object_id('dbo.NetworkMaster'))
INSERT [dbo].[NetworkMaster] ([ID], [Name], [Image]) VALUES (1, N'Device Network', N'~/images/Network_Apps/device-network-icon.png')
IF NOT EXISTS(select * from NetworkMaster where name='Devices Network wireless Connected-100' and id=object_id('dbo.NetworkMaster'))

INSERT [dbo].[NetworkMaster] ([ID], [Name], [Image]) VALUES (2, N'Devices Network wireless Connected-100 ', N'~/images/Network_Apps/Devices-network-wireless-connected-100-icon.png')
IF NOT EXISTS(select * from NetworkMaster where name='Internet Device Vista' and id=object_id('dbo.NetworkMaster'))
INSERT [dbo].[NetworkMaster] ([ID], [Name], [Image]) VALUES (3, N'Internet Device Vista ', N'~/images/Network_Apps/Internet device Vista Icon.jpg')
IF NOT EXISTS(select * from NetworkMaster where name='Places Network Server Database' and id=object_id('dbo.NetworkMaster'))

INSERT [dbo].[NetworkMaster] ([ID], [Name], [Image]) VALUES (4, N'Places Network Server Database', N'~/images/Network_Apps/Places-network-server-database-icon.png')
IF NOT EXISTS(select * from NetworkMaster where name='Server Vista' and id=object_id('dbo.NetworkMaster'))

INSERT [dbo].[NetworkMaster] ([ID], [Name], [Image]) VALUES (5, N'Server Vista', N'~/images/Network_Apps/Server Vista Icon.jpg')
IF NOT EXISTS(select * from NetworkMaster where name='Signal Vista' and id=object_id('dbo.NetworkMaster'))

INSERT [dbo].[NetworkMaster] ([ID], [Name], [Image]) VALUES (6, N'Signal Vista ', N'~/images/Network_Apps/Signal Vista Icon.jpg')

IF NOT EXISTS(select * from NetworkMaster where name='Wifi Modem' and id=object_id('dbo.NetworkMaster'))
INSERT [dbo].[NetworkMaster] ([ID], [Name], [Image]) VALUES (7, N'Wifi Modem', N'~/images/Network_Apps/Wifi modem Icon.jpg')
IF NOT EXISTS(select * from NetworkMaster where name=' None' and id=object_id('dbo.NetworkMaster'))
INSERT [dbo].[NetworkMaster] ([ID], [Name], [Image]) VALUES (8, N'Default', N'~/images/Network_Apps/device-network-icon.png')
SET IDENTITY_INSERT [dbo].[NetworkMaster] OFF




--11/12/2014 Durga VSPLUS-1111

USE [vitalsigns]
GO

/****** Object:  Table [dbo].[WebSphereNodes]    Script Date: 11/10/2014 17:32:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
If NOT exists (select * from syscolumns where id = object_id('dbo.WebSphereNodes'))

CREATE TABLE [dbo].[WebSphereNodes](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[ServerID] [int] NULL,
	[Enabled] [bit] NULL,
	[Type] [nvarchar](50) NULL,
	[LoginName] [nvarchar](50) NULL,
	[Password] [nvarchar](50) NULL,
	[Connector] [nvarchar](50) NULL,
	[NodeName] [nvarchar](50) NULL,
	[Port] [int] NULL,
 CONSTRAINT [PK_WebSphereNodes] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

----Sharepoint Health-somaraju------------

USE [vitalsigns]
GO
IF NOT EXISTS(select * from syscolumns where name='ContentMemory' and id=object_id('dbo.Status'))
begin
ALTER TABLE [Status] ADD ContentMemory float NULL
end

--task 1111--
USE [vitalsigns]
GO
IF NOT EXISTS(select * from syscolumns where name='srvawareness' and id=object_id('dbo.SametimeServers'))
begin
ALTER TABLE SametimeServers ADD srvawareness bit NULL
end
IF NOT EXISTS(select * from syscolumns where name='srvdirectory' and id=object_id('dbo.SametimeServers'))
begin
ALTER TABLE SametimeServers ADD srvdirectory bit NULL
end
IF NOT EXISTS(select * from syscolumns where name='srvstorage' and id=object_id('dbo.SametimeServers'))
begin
ALTER TABLE SametimeServers ADD srvstorage bit NULL
end
IF NOT EXISTS(select * from syscolumns where name='srvbuddylist' and id=object_id('dbo.SametimeServers'))
begin
ALTER TABLE SametimeServers ADD srvbuddylist bit NULL
end
IF NOT EXISTS(select * from syscolumns where name='srvplace' and id=object_id('dbo.SametimeServers'))
begin
ALTER TABLE SametimeServers  ADD srvplace bit NULL
end
IF NOT EXISTS(select * from syscolumns where name='srvlookup' and id=object_id('dbo.SametimeServers'))
begin
ALTER TABLE SametimeServers ADD srvlookup bit NULL
end
IF NOT EXISTS(select * from syscolumns where name='srvtestchat' and id=object_id('dbo.SametimeServers'))
begin
ALTER TABLE SametimeServers ADD srvtestchat bit NULL
end
IF NOT EXISTS(select * from syscolumns where name='srvtestmeeting' and id=object_id('dbo.SametimeServers'))
begin
ALTER TABLE SametimeServers ADD srvtestmeeting bit NULL
end
IF NOT EXISTS(select * from syscolumns where name='generalport' and id=object_id('dbo.SametimeServers'))
begin
ALTER TABLE SametimeServers ADD generalport int NULL
end
IF NOT EXISTS(select * from syscolumns where name='proxytype' and id=object_id('dbo.SametimeServers'))
begin
ALTER TABLE SametimeServers ADD proxytype varchar(200) NULL
end
IF NOT EXISTS(select * from syscolumns where name='proxyprotocol' and id=object_id('dbo.SametimeServers'))
begin
ALTER TABLE SametimeServers ADD proxyprotocol varchar(200) NULL
end
IF NOT EXISTS(select * from syscolumns where name='db2hostname' and id=object_id('dbo.SametimeServers'))
begin
ALTER TABLE SametimeServers ADD db2hostname varchar(200) NULL
end
IF NOT EXISTS(select * from syscolumns where name='db2login' and id=object_id('dbo.SametimeServers'))
begin
ALTER TABLE SametimeServers ADD db2login varchar(200) NULL
end
IF NOT EXISTS(select * from syscolumns where name='db2password' and id=object_id('dbo.SametimeServers'))
begin
ALTER TABLE SametimeServers ADD db2password varchar(200) NULL
end
IF NOT EXISTS(select * from syscolumns where name='db2databasename' and id=object_id('dbo.SametimeServers'))
begin
ALTER TABLE SametimeServers ADD db2databasename varchar(200) NULL
end
IF NOT EXISTS(select * from syscolumns where name='enabledb2port' and id=object_id('dbo.SametimeServers'))
begin
ALTER TABLE SametimeServers ADD enabledb2port bit NULL
end
IF NOT EXISTS(select * from syscolumns where name='db2port' and id=object_id('dbo.SametimeServers'))
begin
ALTER TABLE SametimeServers ADD db2port varchar(200) NULL
end
IF NOT EXISTS(select * from syscolumns where name='db2downalert' and id=object_id('dbo.SametimeServers'))
begin
ALTER TABLE SametimeServers ADD db2downalert  varchar(200) NULL
end
IF NOT EXISTS(select * from syscolumns where name='db2backonlinealert' and id=object_id('dbo.SametimeServers'))
begin
ALTER TABLE SametimeServers ADD db2backonlinealert varchar(200) NULL
end
IF NOT EXISTS(select * from syscolumns where name='srvquery' and id=object_id('dbo.SametimeServers'))
begin
ALTER TABLE SametimeServers ADD srvquery bit NULL
end
go

--928 task sowjanya--
USE [vitalsigns]
GO
IF NOT EXISTS(select * from syscolumns where name='MinNoOfTasks' and id=object_id('dbo.ServerTaskSettings'))
begin
ALTER TABLE ServerTaskSettings ADD MinNoOfTasks int
end
go

IF NOT EXISTS(select * from syscolumns where name='IsMinTasksEnabled' and id=object_id('dbo.ServerTaskSettings'))
begin
ALTER TABLE ServerTaskSettings ADD IsMinTasksEnabled bit
end
go

--1131 BY NIRANJAN
USE [vitalsigns]
GO


IF NOT EXISTS(select * from syscolumns where name='CloudApplications' and id=object_id('dbo.Users'))
begin
ALTER TABLE Users ADD CloudApplications bit
end
go

IF NOT EXISTS(select * from syscolumns where name='OnPremisesApplications' and id=object_id('dbo.Users'))
begin
ALTER TABLE Users ADD OnPremisesApplications bit
end
go


IF NOT EXISTS(select * from syscolumns where name='NetworkInfrastucture' and id=object_id('dbo.Users'))
begin
ALTER TABLE Users ADD NetworkInfrastucture bit
end
go

IF NOT EXISTS(select * from syscolumns where name='DominoServerMetrics' and id=object_id('dbo.Users'))
begin
ALTER TABLE Users ADD DominoServerMetrics bit
end
go



----
IF NOT EXISTS(select * from syscolumns where name='SendMinTasksLoadCmd' and id=object_id('dbo.ServerTaskSettings'))
begin
ALTER TABLE ServerTaskSettings ADD SendMinTasksLoadCmd bit
end
go

--Dhiren VSPLUS-915
IF NOT EXISTS (select * from syscolumns where name= 'SendRouterRestart' and id = object_id('dbo.DominoServers'))
BEGIN	
	ALTER TABLE DominoServers ADD SendRouterRestart BIT NULL
END
GO


/* 11/21/2014 NS added for VSPLUS-1156 */
USE [VSS_Statistics]
UPDATE [DominoSummaryStats] SET WeekNumber = DATEPART( wk, Date)
GO
UPDATE [MicrosoftSummaryStats] SET WeekNumber = DATEPART( wk, Date)
GO
UPDATE [MicrosoftDailyStats] SET WeekNumber = DATEPART( wk, Date)
GO
UPDATE [BESDailySummaryStats] SET WeekNumber = DATEPART( wk, Date)
GO
UPDATE [DeviceDailySummaryStats ] SET WeekNumber = DATEPART( wk, Date)
GO
UPDATE [DeviceUpTimeSummaryStats] SET WeekNumber = DATEPART( wk, Date)
GO
UPDATE [BESDailyStats] SET WeekNumber = DATEPART( wk, Date)
GO
UPDATE [BlackBerrySummaryStats] SET WeekNumber = DATEPART( wk, Date)
GO
UPDATE [SametimeDailyStats] SET WeekNumber = DATEPART( wk, Date)
GO
UPDATE [SametimeSummaryStats] SET WeekNumber = DATEPART( wk, Date)
GO
UPDATE [DeviceDailyStats] SET WeekNumber = DATEPART( wk, Date)
GO
UPDATE [DeviceUpTimeStats] SET WeekNumber = DATEPART( wk, Date)
GO
UPDATE [DominoDailyStats] SET WeekNumber = DATEPART( wk, Date)
GO

--Latency Columns: Mukund
USE [vitalsigns]
GO
IF NOT EXISTS(select * from syscolumns where name='EnableLatencyTest' and id=object_id('dbo.ExchangeSettings'))
begin
ALTER TABLE ExchangeSettings ADD EnableLatencyTest bit NULL
end
go
IF NOT EXISTS(select * from syscolumns where name='LatencyYellowThreshold' and id=object_id('dbo.ExchangeSettings'))
begin
ALTER TABLE ExchangeSettings ADD LatencyYellowThreshold int NULL
end
go
IF NOT EXISTS(select * from syscolumns where name='LatencyRedThreshold' and id=object_id('dbo.ExchangeSettings'))
begin
ALTER TABLE ExchangeSettings ADD LatencyRedThreshold int NULL
end
go

USE [VSS_Statistics]
GO
IF NOT EXISTS (select * from syscolumns where id = object_id('dbo.MailLatencyStats'))
BEGIN
CREATE TABLE [dbo].[MailLatencyStats](
	[sourceserver] [nvarchar](100) NULL,
	[DestinationServer] [nvarchar](100) NULL,
	[Latency] [float] NULL,
	[Date] [datetime] NULL
) ON [PRIMARY]
End
Go
IF NOT EXISTS (select * from syscolumns where id = object_id('dbo.MailLatencyDailyStats'))
BEGIN
CREATE TABLE [dbo].[MailLatencyDailyStats](
	[sourceserver] [nvarchar](100) NULL,
	[DestinationServer] [nvarchar](100) NULL,
	[Latency] [float] NULL,
	[Date] [datetime] NULL
) ON [PRIMARY]
End
Go

use [vitalsigns]
GO

IF NOT EXISTS(select * from syscolumns where name='Replications' and id=object_id('dbo.ActiveDirectoryTest'))
begin

	IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ActiveDirectoryTest]') AND type in (N'U'))
	DROP TABLE [dbo].[ActiveDirectoryTest]
	

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

end
go

IF NOT EXISTS (select * from syscolumns where id = object_id('dbo.ActiveDirectoryReplicationSummary'))
begin
	CREATE TABLE [dbo].[ActiveDirectoryReplicationSummary](
		[ServerName] [nvarchar](50) NOT NULL,
		[SourceServer] [nvarchar](50) NOT NULL,
		[LargestDelta] [nvarchar](50) NOT NULL,
		[Fails] [nvarchar](50) NOT NULL,
		[DirectoryPartitions] [nvarchar](50) NOT NULL,
		[LastScanTime] [datetime] NOT NULL
	) ON [PRIMARY]
end
go

use [vitalsigns]
GO
IF NOT EXISTS (select * from syscolumns where id = object_id('dbo.DominoConsoleCommands'))
BEGIN	
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
	END
GO

--AF In case the user has the table already just add the new comments column--
use [vitalsigns]
GO
IF NOT EXISTS (select * from syscolumns where name= 'Comments' and id = object_id('dbo.DominoConsoleCommands'))
BEGIN	
	ALTER TABLE dbo.DominoConsoleCommands ADD Comments [nvarchar] (250) null
END

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
BEGIN
SET ANSI_NULLS ON

SET QUOTED_IDENTIFIER ON

CREATE TABLE [dbo].[ScanSettings](
	[sname] [nvarchar](250) NOT NULL,
	[svalue] [nvarchar](250) NULL,
	[stype] [nvarchar](250) NULL,
	[EnableReport] [bit] NULL,
	[ScanInterval] [int] NULL,
	[Priority] [int] NULL
) ON [PRIMARY]
END
GO

USE [vitalsigns]
go

IF NOT EXISTS (select * from syscolumns where id = object_id('dbo.Office365AccountStats'))
BEGIN

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


END
go

USE [VSS_Statistics]
go

IF NOT EXISTS (select * from syscolumns where id = object_id('dbo.O365AdditionalMailDetails'))
BEGIN
	CREATE TABLE [dbo].[O365AdditionalMailDetails](
	[Server] [varchar](100) NULL,
	[DisplayName] [varchar](200) NULL,
	[LastLogonTime] [datetime] NULL,
	[LastLoggOffTime] [datetime] NULL,
	[IsActive] [bit] NULL,
	[MailBoxType] [varchar](100) NULL
) ON [PRIMARY]
END
GO

USE [vitalsigns]
go
    IF EXISTS (select * from EventsMaster where EventName='Mail Database' AND ID=94)
    BEGIN	
	SET IDENTITY_INSERT [dbo].[EventsMaster] ON
	UPDATE [dbo].[EventsMaster] set EventName= N'Shadow', ServerTypeId = 5 where id=94
		
        SET IDENTITY_INSERT [dbo].[EventsMaster] OFF
    END
    IF NOT EXISTS (select * from EventsMaster where EventName='Shadow' AND ServerTypeID=5)
    BEGIN	
	SET IDENTITY_INSERT [dbo].[EventsMaster] ON
	INSERT [dbo].[EventsMaster] ([ID], [EventName], [ServerTypeID]) VALUES (94, N'Shadow', 5)
	
	SET IDENTITY_INSERT [dbo].[EventsMaster] OFF
	END
	 IF NOT EXISTS (select * from EventsMaster where EventName='Replication Summary' AND ServerTypeID=18)
    BEGIN	
	SET IDENTITY_INSERT [dbo].[EventsMaster] ON
	INSERT [dbo].[EventsMaster] ([ID], [EventName], [ServerTypeID]) VALUES (95, N'Replication Summary', 18)
	SET IDENTITY_INSERT [dbo].[EventsMaster] OFF
	END
	 IF NOT EXISTS (select * from EventsMaster where EventName='Advertising Test' AND ServerTypeID=18)
    BEGIN	
	SET IDENTITY_INSERT [dbo].[EventsMaster] ON
	INSERT [dbo].[EventsMaster] ([ID], [EventName], [ServerTypeID]) VALUES (96, N'Advertising Test', 18)
	SET IDENTITY_INSERT [dbo].[EventsMaster] OFF
	END
	 IF NOT EXISTS (select * from EventsMaster where EventName='FRS System Volume Test' AND ServerTypeID=18)
    BEGIN	
	SET IDENTITY_INSERT [dbo].[EventsMaster] ON
	INSERT [dbo].[EventsMaster] ([ID], [EventName], [ServerTypeID]) VALUES (97, N'FRS System Volume Test', 18)
	SET IDENTITY_INSERT [dbo].[EventsMaster] OFF
	END
	 IF NOT EXISTS (select * from EventsMaster where EventName='Replications Test' AND ServerTypeID=18)
    BEGIN	
	SET IDENTITY_INSERT [dbo].[EventsMaster] ON
	INSERT [dbo].[EventsMaster] ([ID], [EventName], [ServerTypeID]) VALUES (98, N'Replications Test', 18)
	SET IDENTITY_INSERT [dbo].[EventsMaster] OFF
	END
	 IF NOT EXISTS (select * from EventsMaster where EventName='Services Test' AND ServerTypeID=18)
    BEGIN	
	SET IDENTITY_INSERT [dbo].[EventsMaster] ON
	INSERT [dbo].[EventsMaster] ([ID], [EventName], [ServerTypeID]) VALUES (99, N'Services Test', 18)
	SET IDENTITY_INSERT [dbo].[EventsMaster] OFF
	END
	 IF NOT EXISTS (select * from EventsMaster where EventName='DNS Test' AND ServerTypeID=18)
    BEGIN	
	SET IDENTITY_INSERT [dbo].[EventsMaster] ON
	INSERT [dbo].[EventsMaster] ([ID], [EventName], [ServerTypeID]) VALUES (100, N'DNS Test', 18)
	SET IDENTITY_INSERT [dbo].[EventsMaster] OFF
	END
	 IF NOT EXISTS (select * from EventsMaster where EventName='FSMO Check Test' AND ServerTypeID=18)
    BEGIN	
	SET IDENTITY_INSERT [dbo].[EventsMaster] ON
	INSERT [dbo].[EventsMaster] ([ID], [EventName], [ServerTypeID]) VALUES (101, N'FSMO Check Test', 18)
	SET IDENTITY_INSERT [dbo].[EventsMaster] OFF
	END
	 IF NOT EXISTS (select * from EventsMaster where EventName='DNS Authentication Test' AND ServerTypeID=18)
    BEGIN	
	SET IDENTITY_INSERT [dbo].[EventsMaster] ON
	INSERT [dbo].[EventsMaster] ([ID], [EventName], [ServerTypeID]) VALUES (102, N'DNS Authentication Test', 18)
	SET IDENTITY_INSERT [dbo].[EventsMaster] OFF
	END
	 IF NOT EXISTS (select * from EventsMaster where EventName='DNS Basic Test' AND ServerTypeID=18)
    BEGIN	
	SET IDENTITY_INSERT [dbo].[EventsMaster] ON
	INSERT [dbo].[EventsMaster] ([ID], [EventName], [ServerTypeID]) VALUES (103, N'DNS Basic Test', 18)
	SET IDENTITY_INSERT [dbo].[EventsMaster] OFF
	END
	 IF NOT EXISTS (select * from EventsMaster where EventName='DNS Fowarders Test' AND ServerTypeID=18)
    BEGIN	
	SET IDENTITY_INSERT [dbo].[EventsMaster] ON
	INSERT [dbo].[EventsMaster] ([ID], [EventName], [ServerTypeID]) VALUES (104, N'DNS Fowarders Test', 18)
	SET IDENTITY_INSERT [dbo].[EventsMaster] OFF
	END
	IF NOT EXISTS (select * from EventsMaster where EventName='DNS Delegation Test' AND ServerTypeID=18)
    BEGIN	
	SET IDENTITY_INSERT [dbo].[EventsMaster] ON
	INSERT [dbo].[EventsMaster] ([ID], [EventName], [ServerTypeID]) VALUES (105, N'DNS Delegation Test', 18)
	SET IDENTITY_INSERT [dbo].[EventsMaster] OFF
	END
	IF NOT EXISTS (select * from EventsMaster where EventName='DNS Dynamic Update Test' AND ServerTypeID=18)
    BEGIN	
	SET IDENTITY_INSERT [dbo].[EventsMaster] ON
	INSERT [dbo].[EventsMaster] ([ID], [EventName], [ServerTypeID]) VALUES (106, N'DNS Dynamic Update Test', 18)
	SET IDENTITY_INSERT [dbo].[EventsMaster] OFF
	END
	IF NOT EXISTS (select * from EventsMaster where EventName='DNS Record Registration Test' AND ServerTypeID=18)
    BEGIN	
	SET IDENTITY_INSERT [dbo].[EventsMaster] ON
	INSERT [dbo].[EventsMaster] ([ID], [EventName], [ServerTypeID]) VALUES (107, N'DNS Record Registration Test', 18)
	SET IDENTITY_INSERT [dbo].[EventsMaster] OFF
	END
	IF NOT EXISTS (select * from EventsMaster where EventName='DNS External Name Test' AND ServerTypeID=18)
    BEGIN	
	SET IDENTITY_INSERT [dbo].[EventsMaster] ON
	INSERT [dbo].[EventsMaster] ([ID], [EventName], [ServerTypeID]) VALUES (108, N'DNS External Name Test', 18)
	SET IDENTITY_INSERT [dbo].[EventsMaster] OFF
END
GO

IF NOT EXISTS(select * from syscolumns where name='ShadowQThreshold' and id=object_id('dbo.ExchangeSettings'))
begin
ALTER TABLE ExchangeSettings ADD ShadowQThreshold int NULL
end
go

/* 1334 Durga */
--USE [vitalsigns]
--GO
--UPDATE Users SET CloudApplications = 'TRUE',OnPremisesApplications='TRUE',NetworkInfrastucture='TRUE',DominoServerMetrics='TRUE' WHERE 
--CloudApplications IS NULL AND OnPremisesApplications IS NULL AND NetworkInfrastucture IS NULL
--AND DominoServerMetrics IS NULL
--GO
/* TO acess Dashboard with out Errors */
USE [vitalsigns]
GO
Update Users SET CloudApplications='TRUE' where  CloudApplications IS NULL 
GO
Update Users SET OnPremisesApplications='TRUE' where  OnPremisesApplications IS NULL 
GO
Update Users SET NetworkInfrastucture='TRUE' where  NetworkInfrastucture IS NULL 
GO
Update Users SET DominoServerMetrics='TRUE' where  DominoServerMetrics IS NULL 
GO
Update [Network Devices] SET IncludeOnDashBoard='TRUE' where  IncludeOnDashBoard IS NULL 
GO
Update SNMPDevices SET IncludeOnDashBoard='TRUE' where  IncludeOnDashBoard IS NULL 
GO

USE [VitalSigns]
GO

IF NOT EXISTS (select * from syscolumns where id = object_id('dbo.O365Server'))
BEGIN
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
 CONSTRAINT [PK_O365Master] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO

USE [vitalsigns]
GO


IF NOT EXISTS (SELECT * FROM ServerTypes WHERE ID = 21)
BEGIN
	SET IDENTITY_INSERT [dbo].[ServerTypes] ON
	INSERT [dbo].[ServerTypes] ([ID], [ServerType], [ServerTypeTable], [FeatureId]) VALUES (21,N'Office365',N'',19)
	SET IDENTITY_INSERT [dbo].[ServerTypes] OFF
END
GO


/*WS ADDED FOR SHAREPOINT*/

IF NOT EXISTS (select * from syscolumns where id = object_id('dbo.SharePointFarms'))
BEGIN
	CREATE TABLE [dbo].[SharePointFarms](
		[ServerId] [int] NOT NULL,
		[Farm] [varchar](255) NOT NULL,
		[LastUpdated] [varchar](255) NOT NULL
	) ON [PRIMARY]
END
GO

IF NOT EXISTS (select * from syscolumns where id = object_id('dbo.SharePointDatabaseDetails'))
BEGIN
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
END
GO	
	
/* 12/18/2014 NS added for VSPLUS-1238 */
USE [vitalsigns]
GO

IF NOT EXISTS (select * from syscolumns where id = object_id('dbo.DeviceInventory'))
BEGIN	
	CREATE TABLE [dbo].[DeviceInventory](
		[ID] [int] IDENTITY(1,1) NOT NULL,
		[Name] [varchar](150) NOT NULL,
		[DeviceID] [int] NULL,
		[DeviceTypeID] [int] NOT NULL,
		[LocationID] [int] NULL,
	 CONSTRAINT [PK_DeviceInventory] PRIMARY KEY CLUSTERED 
	(
		[ID] ASC
	)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
	) ON [PRIMARY]
END
GO

/* 12/18/2014 NS added for VSPLUS-1229 */
IF NOT EXISTS (select * from syscolumns where id = object_id('dbo.CustomScripts'))
BEGIN	
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
END
GO

/* 12/18/2014 NS added for VSPLUS-1229 */
IF NOT EXISTS (select * from syscolumns where name= 'SMSTo' and id = object_id('AlertDetails'))
BEGIN	
	ALTER TABLE AlertDetails ADD SMSTo [varchar] (50) null
END
GO
IF NOT EXISTS(SELECT * from sys.columns WHERE Name = N'ScriptID' and Object_ID = Object_ID(N'AlertDetails'))
BEGIN	
	ALTER TABLE AlertDetails
	ADD [ScriptID] int NULL
	
	ALTER TABLE AlertDetails
	ADD FOREIGN KEY ([ScriptID])
	REFERENCES CustomScripts (ID)
	
END
GO
/* 12/18/2014 NS added for VSPLUS-1238 */
/* BEGIN - Populate table with data if empty */
IF NOT EXISTS(SELECT * from DeviceInventory WHERE DeviceTypeID=17)
BEGIN	
	INSERT INTO DeviceInventory(Name,DeviceID,DeviceTypeID,LocationID)
	SELECT Name, ID, 17, NULL
	FROM CloudDetails 
END
GO
IF NOT EXISTS(SELECT * from DeviceInventory WHERE DeviceTypeID=14)
BEGIN	
	INSERT INTO DeviceInventory(Name,DeviceID,DeviceTypeID,LocationID)
	SELECT Name, NULL, 14, NULL
	FROM ExchangeMailProbe  
END
GO
IF NOT EXISTS(SELECT * from DeviceInventory WHERE DeviceTypeID=6)
BEGIN	
	INSERT INTO DeviceInventory(Name,DeviceID,DeviceTypeID,LocationID)
	SELECT Name, [key], 6, NULL
	FROM MailServices
END
GO
IF NOT EXISTS(SELECT * from DeviceInventory WHERE DeviceTypeID=11)
BEGIN	
	INSERT INTO DeviceInventory(Name,DeviceID,DeviceTypeID,LocationID)
	SELECT DeviceId, NULL, 11, NULL
	FROM MobileUserThreshold
END
GO
IF NOT EXISTS(SELECT * from DeviceInventory WHERE DeviceTypeID=8)
BEGIN	
	INSERT INTO DeviceInventory(Name,DeviceID,DeviceTypeID,LocationID)
	SELECT Name, ID, 8, NULL
	FROM [Network Devices] 
END
GO
IF NOT EXISTS(SELECT * from DeviceInventory WHERE DeviceTypeID=9)
BEGIN	
	INSERT INTO DeviceInventory(Name,DeviceID,DeviceTypeID,LocationID)
	SELECT Name, ID, 9, NULL
	FROM NotesDatabases
END
GO
IF NOT EXISTS(SELECT * from DeviceInventory WHERE DeviceTypeID=13)
BEGIN	
	INSERT INTO DeviceInventory(Name,DeviceID,DeviceTypeID,LocationID)
	SELECT Name, NULL, 13, NULL
	FROM NotesMailProbe 
END
GO

/* 4/21/2015 NS modified - LocationID is not NULL for URLs */
IF NOT EXISTS(SELECT * from DeviceInventory WHERE DeviceTypeID=7)
BEGIN	
	INSERT INTO DeviceInventory(Name,DeviceID,DeviceTypeID,LocationID)
	SELECT Name, ID, 7, LocationID
	FROM URLs  
END
GO
IF NOT EXISTS(SELECT * from DeviceInventory WHERE DeviceTypeID IN (SELECT DISTINCT ServerTypeID FROM Servers))
BEGIN	
	INSERT INTO DeviceInventory(Name,DeviceID,DeviceTypeID,LocationID)
	SELECT ServerName, ID, ServerTypeID, LocationID
	FROM Servers
END
GO

/* END - Populate table with data if empty */

/* 12/18/2014 NS added for VSPLUS-1238 */
/* BEGIN - check if triggers don't exist, create */
IF EXISTS(SELECT * FROM sys.triggers WHERE name = 'tr_DeleteDeviceInventoryCloudDetails')
DROP TRIGGER [dbo].[tr_DeleteDeviceInventoryCloudDetails]
GO
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

IF EXISTS(SELECT * FROM sys.triggers WHERE name = 'tr_InsertDeviceInventoryCloudDetails')
DROP TRIGGER [dbo].[tr_InsertDeviceInventoryCloudDetails]
GO
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

/* 4/27/2015 NS modified for VSPLUS-1686 */
IF EXISTS(SELECT * FROM sys.triggers WHERE name = 'tr_UpdateDeviceInventoryCloudDetails')
DROP TRIGGER [dbo].[tr_UpdateDeviceInventoryCloudDetails]
GO
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

IF EXISTS(SELECT * FROM sys.triggers WHERE name = 'tr_DeleteDeviceInventoryExchangeMailProbe')
DROP TRIGGER [dbo].[tr_DeleteDeviceInventoryExchangeMailProbe]
GO
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

IF EXISTS(SELECT * FROM sys.triggers WHERE name = 'tr_InsertDeviceInventoryExchangeMailProbe')
DROP TRIGGER [dbo].[tr_InsertDeviceInventoryExchangeMailProbe]
GO
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

/* 4/27/2015 NS modified for VSPLUS-1686 */
IF EXISTS(SELECT * FROM sys.triggers WHERE name = 'tr_UpdateDeviceInventoryExchangeMailProbe')
DROP TRIGGER [dbo].[tr_UpdateDeviceInventoryExchangeMailProbe]
GO
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

IF EXISTS(SELECT * FROM sys.triggers WHERE name = 'tr_DeleteDeviceInventoryMailServices')
DROP TRIGGER [dbo].[tr_DeleteDeviceInventoryMailServices]
GO
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

IF EXISTS(SELECT * FROM sys.triggers WHERE name = 'tr_InsertDeviceInventoryMailServices')
DROP TRIGGER [dbo].[tr_InsertDeviceInventoryMailServices]
GO
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

/* 4/27/2015 NS modified for VSPLUS-1686 */
IF EXISTS(SELECT * FROM sys.triggers WHERE name = 'tr_UpdateDeviceInventoryMailServices')
DROP TRIGGER [dbo].[tr_UpdateDeviceInventoryMailServices]
GO
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

IF EXISTS(SELECT * FROM sys.triggers WHERE name = 'tr_DeleteDeviceInventoryMobileUserThreshold')
DROP TRIGGER [dbo].[tr_DeleteDeviceInventoryMobileUserThreshold]
GO
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

IF EXISTS(SELECT * FROM sys.triggers WHERE name = 'tr_InsertDeviceInventoryMobileUserThreshold')
DROP TRIGGER [dbo].[tr_InsertDeviceInventoryMobileUserThreshold]
GO
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

IF EXISTS(SELECT * FROM sys.triggers WHERE name = 'tr_UpdateDeviceInventoryMobileUserThreshold')
DROP TRIGGER [dbo].[tr_UpdateDeviceInventoryMobileUserThreshold]
GO
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

IF EXISTS(SELECT * FROM sys.triggers WHERE name = 'tr_DeleteDeviceInventoryNetworkDevices')
DROP TRIGGER [dbo].[tr_DeleteDeviceInventoryNetworkDevices]
GO
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

IF EXISTS(SELECT * FROM sys.triggers WHERE name = 'tr_InsertDeviceInventoryNetworkDevices')
DROP TRIGGER [dbo].[tr_InsertDeviceInventoryNetworkDevices]
GO
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

/* 4/27/2015 NS modified for VSPLUS-1686 */
IF EXISTS(SELECT * FROM sys.triggers WHERE name = 'tr_UpdateDeviceInventoryNetworkDevices')
DROP TRIGGER [dbo].[tr_UpdateDeviceInventoryNetworkDevices]
GO
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

IF EXISTS(SELECT * FROM sys.triggers WHERE name = 'tr_DeleteDeviceInventoryNotesDatabases')
DROP TRIGGER [dbo].[tr_DeleteDeviceInventoryNotesDatabases]
GO
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

IF EXISTS(SELECT * FROM sys.triggers WHERE name = 'tr_InsertDeviceInventoryNotesDatabases')
DROP TRIGGER [dbo].[tr_InsertDeviceInventoryNotesDatabases]
GO
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

/* 4/27/2015 NS modified for VSPLUS-1686 */
IF EXISTS(SELECT * FROM sys.triggers WHERE name = 'tr_UpdateDeviceInventoryNotesDatabases')
DROP TRIGGER [dbo].[tr_UpdateDeviceInventoryNotesDatabases]
GO
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

IF EXISTS(SELECT * FROM sys.triggers WHERE name = 'tr_DeleteDeviceInventoryNotesMailProbe')
DROP TRIGGER [dbo].[tr_DeleteDeviceInventoryNotesMailProbe]
GO
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

IF EXISTS(SELECT * FROM sys.triggers WHERE name = 'tr_InsertDeviceInventoryNotesMailProbe')
DROP TRIGGER [dbo].[tr_InsertDeviceInventoryNotesMailProbe]
GO
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

/* 4/27/2015 NS modified for VSPLUS-1686 */
IF EXISTS(SELECT * FROM sys.triggers WHERE name = 'tr_UpdateDeviceInventoryNotesMailProbe')
DROP TRIGGER [dbo].[tr_UpdateDeviceInventoryNotesMailProbe]
GO
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

IF EXISTS(SELECT * FROM sys.triggers WHERE name = 'tr_DeleteDeviceInventoryServers')
DROP TRIGGER [dbo].[tr_DeleteDeviceInventoryServers]
GO
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

IF EXISTS(SELECT * FROM sys.triggers WHERE name = 'tr_InsertDeviceInventoryServers')
DROP TRIGGER [dbo].[tr_InsertDeviceInventoryServers]
GO
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

IF EXISTS(SELECT * FROM sys.triggers WHERE name = 'tr_UpdateDeviceInventoryServers')
DROP TRIGGER [dbo].[tr_UpdateDeviceInventoryServers]
GO
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

IF EXISTS(SELECT * FROM sys.triggers WHERE name = 'tr_DeleteDeviceInventoryURLs')
DROP TRIGGER [dbo].[tr_DeleteDeviceInventoryURLs]
GO
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

/* 4/21/2015 NS modified - LocationID is not NULL for URLs */
IF EXISTS(SELECT * FROM sys.triggers WHERE name = 'tr_InsertDeviceInventoryURLs')
DROP TRIGGER [dbo].[tr_InsertDeviceInventoryURLs]
GO
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

/* 4/21/2015 NS modified - LocationID is not NULL for URLs */
/* 4/27/2015 NS modified for VSPUS-1686 */
IF EXISTS(SELECT * FROM sys.triggers WHERE name = 'tr_UpdateDeviceInventoryURLs')
DROP TRIGGER [dbo].[tr_UpdateDeviceInventoryURLs]
GO
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

/* END - check if triggers don't exist, create */


USE [VitalSigns]
GO

IF NOT EXISTS (select * from syscolumns where id = object_id('dbo.O365LYNCP2PSessionReport'))
BEGIN
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
END
GO

IF NOT EXISTS (select * from syscolumns where id = object_id('dbo.O365LYNCPAVTimeReport'))
BEGIN
CREATE TABLE [dbo].[O365LYNCPAVTimeReport](
	[ServerId] [int] NOT NULL,
	[AccountName] [varchar](254) NULL,
	[TotalAudioMinutes] [int] NULL,
	[TotalVideoMinutes] [int] NULL
) ON [PRIMARY]
END
GO

IF NOT EXISTS (select * from syscolumns where id = object_id('dbo.O365LYNCConferenceReport'))
BEGIN
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
END
GO

IF NOT EXISTS (select * from syscolumns where id = object_id('dbo.O365LYNCStats'))
BEGIN
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
END
GO

IF NOT EXISTS (select * from syscolumns where id = object_id('dbo.O365LYNCDevices'))
BEGIN
CREATE TABLE [dbo].[O365LYNCDevices](
	[ServerId] [int] NOT NULL,
	[AccountName] [varchar](254) NULL,
	[WindowsUsers] [int] NULL,
	[WindowsPhoneUsers] [int] NULL,
	[AndroidUsers] [int] NULL,
	[iPhoneUsers] [int] NULL,
	[iPadUsers] [int] NULL
) ON [PRIMARY]
END
GO

/* 12/19/2014 NS added - the event Free Space is not used anymore */
IF EXISTS(SELECT * from EventsMaster WHERE EventName = N'Free Space' AND ServerTypeID = 5)
BEGIN	
	DELETE FROM [dbo].[EventsMaster] WHERE EventName = N'Free Space' AND ServerTypeID = 5	
END
/* 12/19/2014 NS added new events */
IF EXISTS(SELECT * from EventsMaster WHERE ID >= 109 and ID <= 141)
BEGIN	
	DELETE FROM [dbo].[EventsMaster] WHERE ID >= 109 and ID <= 141
END
IF NOT EXISTS( select * from EventsMaster where ID >= 109 and ID <= 141 )
BEGIN
	SET IDENTITY_INSERT [dbo].[EventsMaster] ON
	INSERT [dbo].[EventsMaster] ([ID], [EventName], [ServerTypeID]) VALUES (109, N'Availability', 18)
	INSERT [dbo].[EventsMaster] ([ID], [EventName], [ServerTypeID]) VALUES (110, N'Query Latency', 18)
	INSERT [dbo].[EventsMaster] ([ID], [EventName], [ServerTypeID]) VALUES (111, N'Logon Test', 18)
	INSERT [dbo].[EventsMaster] ([ID], [EventName], [ServerTypeID]) VALUES (112, N'Port Test', 18)
	INSERT [dbo].[EventsMaster] ([ID], [EventName], [ServerTypeID]) VALUES (113, N'Web Count', 4)
	INSERT [dbo].[EventsMaster] ([ID], [EventName], [ServerTypeID]) VALUES (114, N'Database Site Warning Count', 4)
	INSERT [dbo].[EventsMaster] ([ID], [EventName], [ServerTypeID]) VALUES (115, N'Database Site Max Count', 4)
	INSERT [dbo].[EventsMaster] ([ID], [EventName], [ServerTypeID]) VALUES (116, N'Site Health Check', 4)
	INSERT [dbo].[EventsMaster] ([ID], [EventName], [ServerTypeID]) VALUES (117, N'Network Latency Test', 4)
	INSERT [dbo].[EventsMaster] ([ID], [EventName], [ServerTypeID]) VALUES (118, N'Mail flow', 14)
	INSERT [dbo].[EventsMaster] ([ID], [EventName], [ServerTypeID]) VALUES (119, N'RPC', 5)
	INSERT [dbo].[EventsMaster] ([ID], [EventName], [ServerTypeID]) VALUES (120, N'POP3', 5)
	INSERT [dbo].[EventsMaster] ([ID], [EventName], [ServerTypeID]) VALUES (121, N'Auto Discovery', 5)
	INSERT [dbo].[EventsMaster] ([ID], [EventName], [ServerTypeID]) VALUES (122, N'Services', 5)
	INSERT [dbo].[EventsMaster] ([ID], [EventName], [ServerTypeID]) VALUES (123, N'Services', 18)
	INSERT [dbo].[EventsMaster] ([ID], [EventName], [ServerTypeID]) VALUES (124, N'Memory', 18)
	INSERT [dbo].[EventsMaster] ([ID], [EventName], [ServerTypeID]) VALUES (125, N'CPU', 18)
	INSERT [dbo].[EventsMaster] ([ID], [EventName], [ServerTypeID]) VALUES (126, N'Disk Space', 18)
	INSERT [dbo].[EventsMaster] ([ID], [EventName], [ServerTypeID]) VALUES (127, N'Reboot Overdue', 18)
	INSERT [dbo].[EventsMaster] ([ID], [EventName], [ServerTypeID]) VALUES (128, N'Not Responding', 18)
	INSERT [dbo].[EventsMaster] ([ID], [EventName], [ServerTypeID]) VALUES (129, N'Services', 4)
	INSERT [dbo].[EventsMaster] ([ID], [EventName], [ServerTypeID]) VALUES (130, N'Memory', 4)
	INSERT [dbo].[EventsMaster] ([ID], [EventName], [ServerTypeID]) VALUES (131, N'CPU', 4)
	INSERT [dbo].[EventsMaster] ([ID], [EventName], [ServerTypeID]) VALUES (132, N'Disk Space', 4)
	INSERT [dbo].[EventsMaster] ([ID], [EventName], [ServerTypeID]) VALUES (133, N'Reboot Overdue', 4)
	INSERT [dbo].[EventsMaster] ([ID], [EventName], [ServerTypeID]) VALUES (134, N'Not Responding', 4)
	INSERT [dbo].[EventsMaster] ([ID], [EventName], [ServerTypeID]) VALUES (135, N'Disk Space', 5)
	INSERT [dbo].[EventsMaster] ([ID], [EventName], [ServerTypeID]) VALUES (136, N'Services', 15)
	INSERT [dbo].[EventsMaster] ([ID], [EventName], [ServerTypeID]) VALUES (137, N'Memory', 15)
	INSERT [dbo].[EventsMaster] ([ID], [EventName], [ServerTypeID]) VALUES (138, N'CPU', 15)
	INSERT [dbo].[EventsMaster] ([ID], [EventName], [ServerTypeID]) VALUES (139, N'Disk Space', 15)
	INSERT [dbo].[EventsMaster] ([ID], [EventName], [ServerTypeID]) VALUES (140, N'Reboot Overdue', 15)
	INSERT [dbo].[EventsMaster] ([ID], [EventName], [ServerTypeID]) VALUES (141, N'Not Responding', 15)
	SET IDENTITY_INSERT [dbo].[EventsMaster] OFF
END

--WS SharePoint Health page SQL changes

IF NOT EXISTS (select * from syscolumns where id = object_id('dbo.SharePointFarmHealth'))
BEGIN
CREATE TABLE [dbo].[SharePointFarmHealth](
	[FarmName] [varchar](250) NOT NULL,
	[LogOnTest] [bit] NULL,
	[UploadTest] [bit] NULL,
	[SiteCollectionTest] [bit] NULL
) ON [PRIMARY]
END
GO

IF NOT EXISTS (select * from syscolumns where id = object_id('dbo.SharePointFarmSettings'))
BEGIN
CREATE TABLE [dbo].[SharePointFarmSettings](
	[FarmName] [varchar](250) NOT NULL,
	[LogOnTest] [bit] NULL,
	[SiteCreationTest] [bit] NULL,
	[FileUploadTest] [bit] NULL,
	[TestApplicationURL] [varchar](250) NULL
) ON [PRIMARY]
END
GO

IF NOT EXISTS( select * from RolesMaster where ID >= 7 and ID <= 11 )
BEGIN
	SET IDENTITY_INSERT [dbo].[RolesMaster] ON
	INSERT [dbo].[RolesMaster] ([Id], [RoleName], [ServerTypeId]) VALUES (7, N'WebFrontEnd', 4)
	INSERT [dbo].[RolesMaster] ([Id], [RoleName], [ServerTypeId]) VALUES (8, N'Database', 4)
	INSERT [dbo].[RolesMaster] ([Id], [RoleName], [ServerTypeId]) VALUES (9, N'Application', 4)
	INSERT [dbo].[RolesMaster] ([Id], [RoleName], [ServerTypeId]) VALUES (10, N'SingleServer', 4)
	INSERT [dbo].[RolesMaster] ([Id], [RoleName], [ServerTypeId]) VALUES (11, N'Invalid', 4)
	SET IDENTITY_INSERT [dbo].[RolesMaster] OFF
END
GO

IF NOT EXISTS(select * from syscolumns where name='City' and id=object_id('dbo.Locations'))
BEGIN
	ALTER TABLE Locations ADD City varchar(255) NULL
END
GO

IF NOT EXISTS(select * from syscolumns where name='State' and id=object_id('dbo.Locations'))
BEGIN
	ALTER TABLE Locations ADD State varchar(255) NULL
END
GO

IF NOT EXISTS(select * from syscolumns where name='Country' and id=object_id('dbo.Locations'))
BEGIN
	ALTER TABLE Locations ADD Country varchar(255) NULL
END
GO
--VSPLUS 2226 SWATHI
/* Table  [dbo].[TestsMaster]*/
USE [vitalsigns]
GO
IF NOT EXISTS (select * from syscolumns where id = object_id('dbo.TestsMaster'))
BEGIN

CREATE TABLE [dbo].[TestsMaster](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ServerType] [int] NULL,
	[Tests] [nvarchar](50) NULL,
	[Type] [nvarchar](50) NULL
) ON [PRIMARY]


END
GO
/* Sowjanya 1311,1314 */
/* Table  [dbo].[Office365Tests]*/
USE [vitalsigns]
GO
IF NOT EXISTS (select * from syscolumns where id = object_id('dbo.Office365Tests'))
BEGIN


CREATE TABLE [dbo].[Office365Tests](
	[Id] [int] NOT NULL,
	[ServerId] [int] NULL,
	[EnableSimulationTests] [bit] NULL,
	[ResponseThreshold] [int] NULL,
	[Type] [varchar](254) NULL,
	[Tests] [varchar](254) NULL
) ON [PRIMARY]
END
GO
--VSPLUS 2607 Somaraju
USE [vitalsigns]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__Office365Tes__TestsMaster]') AND parent_object_id = OBJECT_ID(N'[dbo].[Office365Tests]'))
ALTER TABLE [dbo].[Office365Tests] DROP CONSTRAINT [FK__Office365Tes__TestsMaster]
GO
USE [vitalsigns]
GO

/****** Object:  Index [PK_Office365Tests]    Script Date: 03/14/2016 03:39:25 ******/
IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[Office365Tests]') AND name in(SELECT name
FROM   sys.key_constraints
WHERE  [type] = 'PK'
       AND [parent_object_id] = Object_id('dbo.Office365Tests')))
  DECLARE @table NVARCHAR(512), @sql NVARCHAR(MAX);

SELECT @table = N'dbo.Office365Tests';

SELECT @sql = 'ALTER TABLE ' + @table 
    + ' DROP CONSTRAINT ' + name + ';'
    FROM sys.key_constraints
    WHERE [type] = 'PK'
    AND [parent_object_id] = OBJECT_ID(@table);

EXEC sp_executeSQL @sql;
GO
IF EXISTS (select * from syscolumns where name= 'Id' and id = object_id('dbo.[Office365Tests]'))
BEGIN
	Alter TABLE [dbo].[Office365Tests] Drop Column Id
END
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS WHERE CONSTRAINT_TYPE = 'PRIMARY KEY' AND TABLE_NAME = 'Office365Tests')
BEGIN
	Alter TABLE [dbo].[Office365Tests]
 
add  [id] [int] IDENTITY(1,1) NOT NULL
	ALTER TABLE [dbo].[Office365Tests] ADD  CONSTRAINT [PK_Office365Tests] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]


END
GO
--VSPLUS 2226 SWATHI
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS WHERE CONSTRAINT_TYPE = 'PRIMARY KEY' AND TABLE_NAME = 'TestsMaster')
BEGIN
	ALTER TABLE TestsMaster ADD PRIMARY KEY(Id)
END
GO

/* Sowjanya 1311,1314 */
USE [vitalsigns]
GO
-- Delete from TestsMaster
-- Go
SET IDENTITY_INSERT [dbo].[TestsMaster] ON 

IF NOT EXISTS(SELECT * FROM TESTSMASTER WHERE TESTS='Mail Flow')
	INSERT [dbo].[TestsMaster] ([Id], [ServerType], [Tests], [Type]) VALUES (1, NULL, N'Mail Flow', N'User Scenario Tests')
GO
IF NOT EXISTS(SELECT * FROM TESTSMASTER WHERE TESTS='Inbox Test')
	INSERT [dbo].[TestsMaster] ([Id], [ServerType], [Tests], [Type]) VALUES (2, NULL, N'Inbox Test', N'User Scenario Tests')
GO
IF EXISTS(SELECT * FROM TESTSMASTER WHERE TESTS='Compose Email')
	Update dbo.TestsMaster set TESTS='OWA' where TESTS='Compose Email'
GO

IF NOT EXISTS(SELECT * FROM TESTSMASTER WHERE TESTS='OWA')
	INSERT [dbo].[TestsMaster] ([Id], [ServerType], [Tests], [Type]) VALUES (3, NULL, N'OWA', N'User Scenario Tests')
GO
IF NOT EXISTS(SELECT * FROM TESTSMASTER WHERE TESTS='Create Task')
	INSERT [dbo].[TestsMaster] ([Id], [ServerType], [Tests], [Type]) VALUES (4, NULL, N'Create Task', N'User Scenario Tests')
GO
IF NOT EXISTS(SELECT * FROM TESTSMASTER WHERE TESTS='Create Folder')
	INSERT [dbo].[TestsMaster] ([Id], [ServerType], [Tests], [Type]) VALUES (5, NULL, N'Create Folder', N'User Scenario Tests')
GO
IF NOT EXISTS(SELECT * FROM TESTSMASTER WHERE TESTS='OneDrive Upload Document')
	INSERT [dbo].[TestsMaster] ([Id], [ServerType], [Tests], [Type]) VALUES (6, NULL, N'OneDrive Upload Document', N'User Scenario Tests')
GO
IF NOT EXISTS(SELECT * FROM TESTSMASTER WHERE TESTS='OneDrive Download Document')
	INSERT [dbo].[TestsMaster] ([Id], [ServerType], [Tests], [Type]) VALUES (7, NULL, N'OneDrive Download Document', N'User Scenario Tests')
GO
IF NOT EXISTS(SELECT * FROM TESTSMASTER WHERE TESTS='OneDrive Search')
	INSERT [dbo].[TestsMaster] ([Id], [ServerType], [Tests], [Type]) VALUES (8, NULL, N'OneDrive Search', N'User Scenario Tests')
GO
IF NOT EXISTS(SELECT * FROM TESTSMASTER WHERE TESTS='Create Site')
	INSERT [dbo].[TestsMaster] ([Id], [ServerType], [Tests], [Type]) VALUES (9, NULL, N'Create Site', N'User Scenario Tests')
GO
IF NOT EXISTS(SELECT * FROM TESTSMASTER WHERE TESTS='Create Calendar Entry')
	INSERT [dbo].[TestsMaster] ([Id], [ServerType], [Tests], [Type]) VALUES (10, NULL, N'Create Calendar Entry', N'User Scenario Tests')
GO
IF NOT EXISTS(SELECT * FROM TESTSMASTER WHERE TESTS='Resolve User')
	INSERT [dbo].[TestsMaster] ([Id], [ServerType], [Tests], [Type]) VALUES (11, NULL, N'Resolve User', N'User Scenario Tests')
GO
IF NOT EXISTS(SELECT * FROM TESTSMASTER WHERE TESTS='POP Test')
	INSERT [dbo].[TestsMaster] ([Id], [ServerType], [Tests], [Type]) VALUES (12, NULL, N'POP Test', N'Service Availability Tests ')
GO
IF NOT EXISTS(SELECT * FROM TESTSMASTER WHERE TESTS='IMAP Test')
	INSERT [dbo].[TestsMaster] ([Id], [ServerType], [Tests], [Type]) VALUES (13, NULL, N'IMAP Test', N'Service Availability Tests ')
GO
IF NOT EXISTS(SELECT * FROM TESTSMASTER WHERE TESTS='SMTP Test')
	INSERT [dbo].[TestsMaster] ([Id], [ServerType], [Tests], [Type]) VALUES (14, NULL, N'SMTP Test', N'Service Availability Tests ')
GO

IF NOT EXISTS(SELECT * FROM TESTSMASTER WHERE TESTS='ActiveSync Verification')
	INSERT [dbo].[TestsMaster] ([Id], [ServerType], [Tests], [Type]) VALUES (17, NULL, N'ActiveSync Verification', N'Service Availability Tests ')
GO
IF NOT EXISTS(SELECT * FROM TESTSMASTER WHERE TESTS='Admin Portal Test')
	INSERT [dbo].[TestsMaster] ([Id], [ServerType], [Tests], [Type]) VALUES (18, NULL, N'Admin Portal Test', N'Service Availability Tests ')
GO


--VSPLUS 2204  SWATHI 
IF NOT EXISTS(SELECT * FROM TESTSMASTER WHERE TESTS='Dir Sync Export')
	INSERT [dbo].[TestsMaster] ([Id], [ServerType], [Tests], [Type]) VALUES (20, NULL, N'Dir Sync Export', N'User Scenario Tests')
GO
IF NOT EXISTS(SELECT * FROM TESTSMASTER WHERE TESTS='Dir Sync Import')
	INSERT [dbo].[TestsMaster] ([Id], [ServerType], [Tests], [Type]) VALUES (21, NULL, N'Dir Sync Import', N'User Scenario Tests')
GO

SET IDENTITY_INSERT [dbo].[TestsMaster] OFF

USE [vitalsigns]
GO
IF EXISTS(SELECT * FROM TESTSMASTER WHERE TESTS='Compose Email')
	Update dbo.TestsMaster set TESTS='OWA' where TESTS='Compose Email'
GO
IF EXISTS(SELECT * FROM TESTSMASTER WHERE TESTS='PowerShell connectivity test')
	Delete from dbo.TestsMaster where TESTS='PowerShell connectivity test'
GO
IF EXISTS(SELECT * FROM TESTSMASTER WHERE TESTS='OWA Test')
	Delete from dbo.TestsMaster where TESTS='OWA Test'
GO
IF EXISTS(SELECT * FROM TESTSMASTER WHERE TESTS='Outlook Anywhere Test')
	Delete from dbo.TestsMaster where TESTS='Outlook Anywhere Test'
GO

IF EXISTS(SELECT * FROM Office365Tests WHERE TESTS='Compose Email')
	Update dbo.Office365Tests set Tests='OWA' where Tests='Compose Email'
GO

IF EXISTS(SELECT * FROM Office365Tests WHERE TESTS='OWA Test')
	Delete from dbo.Office365Tests WHERE TESTS='OWA Test'
GO

IF EXISTS(SELECT * FROM Office365Tests WHERE TESTS='Outlook Anywhere Test')
	Delete from dbo.Office365Tests WHERE TESTS='Outlook Anywhere Test'
GO

IF EXISTS(SELECT * FROM Office365Tests WHERE TESTS='PowerShell connectivity test')
	Delete from dbo.Office365Tests WHERE TESTS='PowerShell connectivity test'
GO

/*1322-Swathi*/
USE [vitalsigns]
GO
IF NOT EXISTS (select * from syscolumns where id = object_id('dbo.WebsphereCell'))
BEGIN

/****** Object:  Table [dbo].[WebsphereCell]    Script Date: 01/22/2015 19:16:33 ******/

CREATE TABLE [dbo].[WebsphereCell](
	[CellID] [int] IDENTITY(1,1) NOT NULL,
	[CellName] [nvarchar](200) NULL,
	[HostName] [nvarchar](200) NULL,
	[ConnectionType] [nvarchar](50) NULL,
	[PortNo] [int] NULL,
	[GlobalSecurity] [bit] NULL,
	[UserID] [nvarchar](50) NULL,
	[Password] [nvarchar](50) NULL,
	[Realm] [nvarchar](max) NULL,
	[Certification] [nvarchar](max) NULL,
	[IBMConnectionSID] int NULL
 CONSTRAINT [PK_WebsphereCell] PRIMARY KEY CLUSTERED 
(
	[CellID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

END
GO
USE [vitalsigns]
GO
IF NOT EXISTS (select * from syscolumns where id = object_id('dbo.WebsphereNode'))
BEGIN


CREATE TABLE [dbo].[WebsphereNode](
	[NodeID] [int] IDENTITY(1,1) NOT NULL,
	[NodeName] [nvarchar](200) NULL,
	[CellID] [int] NULL,
	[JVMs] [nvarchar](200) NULL,
	[Status] [nvarchar](200) NULL,
 CONSTRAINT [PK_WebsphereNode] PRIMARY KEY CLUSTERED 
(
	[NodeID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

/****** Object:  ForeignKey [FK__Websphere__CellI__6B3AC472]    Script Date: 01/22/2015 19:16:33 ******/
ALTER TABLE [dbo].[WebsphereNode]  WITH CHECK ADD FOREIGN KEY([CellID])
REFERENCES [dbo].[WebsphereCell] ([CellID])

END
GO
USE [vitalsigns]
GO
IF NOT EXISTS (select * from syscolumns where id = object_id('dbo.WebsphereCellStats'))
BEGIN

/****** Object:  Table [dbo].[WebsphereCellStats]    Script Date: 01/22/2015 19:16:33 ******/

CREATE TABLE [dbo].[WebsphereCellStats](
	[CellID] [int] NULL,
	[CellName] [nvarchar](200) NULL,
	[Status] [nvarchar](200) NULL,
	[LastScan] [datetime] NULL,
	[TotalJVM] [int] NULL,
	[MonitoredJVMs] [int] NULL
) ON [PRIMARY]

/****** Object:  ForeignKey [FK__Websphere__CellI__74C42EAC]    Script Date: 01/22/2015 19:16:33 ******/
ALTER TABLE [dbo].[WebsphereCellStats]  WITH CHECK ADD FOREIGN KEY([CellID])
REFERENCES [dbo].[WebsphereCell] ([CellID])

END
GO
USE [vitalsigns]
GO
IF NOT EXISTS (select * from syscolumns where id = object_id('dbo.WebsphereServer'))
BEGIN


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
	[Enabled] [bit] NULL
) ON [PRIMARY]

/****** Object:  ForeignKey [FK__Websphere__CellI__70F39DC8]    Script Date: 01/22/2015 19:16:33 ******/
ALTER TABLE [dbo].[WebsphereServer]  WITH CHECK ADD FOREIGN KEY([CellID])
REFERENCES [dbo].[WebsphereCell] ([CellID])

/****** Object:  ForeignKey [FK__Websphere__NodeI__6F0B5556]    Script Date: 01/22/2015 19:16:33 ******/
ALTER TABLE [dbo].[WebsphereServer]  WITH CHECK ADD FOREIGN KEY([NodeID])
REFERENCES [dbo].[WebsphereNode] ([NodeID])

/****** Object:  ForeignKey [FK__Websphere__Serve__6E17311D]    Script Date: 01/22/2015 19:16:33 ******/
ALTER TABLE [dbo].[WebsphereServer]  WITH CHECK ADD FOREIGN KEY([ServerID])
REFERENCES [dbo].[Servers] ([ID])



ALTER TABLE [dbo].[WebsphereServer]  WITH CHECK ADD FOREIGN KEY([ServerID])
REFERENCES [dbo].[Servers] ([ID])
ON DELETE CASCADE


END
GO



/* WebSphere Insertion statements*/
USE [vitalsigns]
GO
IF NOT EXISTS (select * from ServerTypes where ID=22)
BEGIN
SET IDENTITY_INSERT [dbo].[ServerTypes] ON
INSERT [dbo].[ServerTypes] ([ID], [ServerType], [ServerTypeTable], [FeatureId]) VALUES (22, N'Websphere Server', N'Websphereserver', 11)
SET IDENTITY_INSERT [dbo].[ServerTypes] OFF
END
GO
USE [vitalsigns]
GO
Delete from MenuItems where ID=168
Go
Delete from MenuItems where ID=169
Go
SET IDENTITY_INSERT [dbo].[MenuItems] ON
INSERT [dbo].[MenuItems] ([ID], [DisplayText], [OrderNum], [ParentID], [PageLink], [Level], [RefName], [ImageURL], [MenuArea], [SessionNames], [TimerEnable]) VALUES (168, N'IBM WebSphere', 14, 2, NULL, 2, N'WebSpher', N'~/images/icons/ibm.png', N'Configurator', NULL, 0)
--VSPLUS-2237 Durga
INSERT [dbo].[MenuItems] ([ID], [DisplayText], [OrderNum], [ParentID], [PageLink], [Level], [RefName], [ImageURL], [MenuArea], [SessionNames], [TimerEnable]) VALUES (169, N'WebSphere Server Health', 12, 79,N'~/Dashboard/WebsphereServerHealth.aspx', 2, N'Websphere Server Health', N'~/images/icons/ibm.png', N'Dashboard', NULL, 1)
SET IDENTITY_INSERT [dbo].[MenuItems] OFF
GO
/* Niranjan 1510 */
USE [vitalsigns]
GO
Delete from FeatureMenus where MenuID=168
Go
INSERT  [dbo].[FeatureMenus] ([FeatureID], [MenuID]) VALUES (18, 168)
Go
Delete from FeatureMenus where MenuID=169
Go
-- VSPLUS-2092 DURGA
INSERT [dbo].[FeatureMenus] ([FeatureID], [MenuID]) VALUES (18, 169)
Go


USE [vitalsigns]
GO

/****** Object:  Table [dbo].[NetworkLatency]    Script Date: 1/23/2015 5:48:22 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS (select * from syscolumns where id = object_id('dbo.NetworkLatency'))
BEGIN
CREATE TABLE [dbo].[NetworkLatency](
	[NetworkLatencyId] [int] IDENTITY(1,1) NOT NULL,
	[TestName] [nvarchar](50) NULL,
	[ScanInterval] [int] NULL,
	[TestDuration] [int] NULL,
	[ServerType] [nvarchar](50) NULL,
	[Enable] [bit] NULL,
 CONSTRAINT [PK_NetworkLatency_1] PRIMARY KEY CLUSTERED 
(
	[NetworkLatencyId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

END
GO

USE [vitalsigns]
GO
IF NOT EXISTS (select * from syscolumns where id = object_id('dbo.NetworkLatencyServers'))
BEGIN
CREATE TABLE [dbo].[NetworkLatencyServers](
	[ServerID] [int] NULL,
	[NetworkLatencyId] [int] NULL,
	[LatencyYellowThreshold] [int] NULL,
	[LatencyRedThreshold] [int] NULL,
	[Enabled] [bit] NULL
) ON [PRIMARY]

END
GO







/****** Object:  Table [dbo].[NetworkLatencyStats]    Script Date: 1/23/2015 5:48:22 AM ******/
USE [VSS_Statistics]
GO
IF NOT EXISTS (select * from syscolumns where id = object_id('dbo.NetworkLatencyStats'))
BEGIN
CREATE TABLE [dbo].[NetworkLatencyStats](
[NetworkLatencyId] [int],
	[sourceserver] [nvarchar](100) NULL,
	[DestinationServer] [nvarchar](100) NULL,
	[Latency] [float] NULL,
	[Date] [datetime] NULL
) ON [PRIMARY]

End
GO
/*Swathi 1285*/
USE [vitalsigns]
GO

IF EXISTS (select * from syscolumns where name= 'ServerType' and id = object_id('dbo.NetworkLatency'))
BEGIN
	Alter TABLE [dbo].[NetworkLatency] Drop Column ServerType
END
GO
USE [vitalsigns]
GO

IF EXISTS(select * from syscolumns where id=Object_id('dbo.NetworkLatencyServers'))
BEGIN	
	drop table dbo.[NetworkLatencyServers]

END
GO
USE [vitalsigns]
GO

if NOT exists (select * from syscolumns where id = object_id('dbo.NetworkLatencyServers'))
BEGIN

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



ALTER TABLE [dbo].[NetworkLatencyServers]  WITH CHECK ADD FOREIGN KEY([ServerID])
REFERENCES [dbo].[Servers] ([ID])


ALTER TABLE [dbo].[NetworkLatencyServers]  WITH CHECK ADD FOREIGN KEY([NetworkLatencyId])
REFERENCES [dbo].[NetworkLatency] ([NetworkLatencyId])
END
GO



/* 1/28/2015 NS modified the category setting for Monthly Server Up Time and Down Time reports - now include Exchange servers*/
USE [vitalsigns]
GO

IF EXISTS (select * from ReportItems where Name='Monthly Server Down Time')
BEGIN	
	UPDATE [dbo].[ReportItems] SET [Category]='Servers', [Description]='Server down times in minutes monthly'
	WHERE Name='Monthly Server Down Time'
END 
GO
IF EXISTS (select * from ReportItems where Name='Monthly Server Up Time')
BEGIN	
	UPDATE [dbo].[ReportItems] SET [Category]='Servers', [Description]='Server up times in minutes monthly'
	WHERE Name='Monthly Server Up Time'
END 
GO

/* 2/2/2015 NS modified the category setting for Daily Memory Used - now include other server types */
USE [vitalsigns]
GO

IF EXISTS (select * from ReportItems where Name='Daily Memory Percent Used')
BEGIN	
	UPDATE [dbo].[ReportItems] SET [Category]='Servers'
	WHERE Name='Daily Memory Percent Used'
END 
GO



-- +++++++++++ Updated the MAX Log file sizes as this was causing the disk to be filled up +++++++++++
-- +++++++++++ Sart of Ticket # VS1401 - Updated by Joe Thumma  +++++++++++


USE VSS_Statistics
GO
/* Setting the Recovery level to Simple */
ALTER DATABASE VSS_Statistics SET RECOVERY SIMPLE
GO
/* Shrink the database to 5MB or to a min size that was setup earlier */
DBCC SHRINKFILE (VSS_Statistics_log, 5) WITH NO_INFOMSGS;
GO

/* Changing the Max Log File size to be 2GB instead of the old size of 2048GB by mistake... */

USE master;
GO
ALTER DATABASE VSS_Statistics 
MODIFY FILE
    (NAME = VSS_Statistics_log,
    MAXSIZE = 2GB);
GO

/* Set Vitalsigns DB Now to max of 2GB file size. */

USE Vitalsigns
GO
/* Setting the Recovery level to Simple */
ALTER DATABASE Vitalsigns SET RECOVERY SIMPLE
GO
/* Shrink the database to 5MB or to a min size that was setup earlier */
DBCC SHRINKFILE (vitalsigns_log, 5) WITH NO_INFOMSGS;
GO

/* Changing the Max Log File size to be 2GB instead of the old size of 2048GB by mistake... */

USE master;
GO
ALTER DATABASE Vitalsigns 
MODIFY FILE
    (NAME = Vitalsigns_Log,
    MAXSIZE = 2GB);
GO

-- +++++++++++ End of Ticket # VS1401 - Updated by Joe Thumma  +++++++++++
USE [vitalsigns]
GO

IF EXISTS(SELECT * from EventsMaster WHERE ID >= 142 and ID <= 155)
BEGIN	
	DELETE FROM [dbo].[EventsMaster] WHERE ID >= 142 and ID <= 155
END
GO
IF NOT EXISTS( select * from EventsMaster where ID >= 142 and ID <= 155 )
BEGIN
	SET IDENTITY_INSERT [dbo].[EventsMaster] ON
	INSERT [dbo].[EventsMaster] ([ID], [EventName], [ServerTypeID]) VALUES (142, N'Create Site', 21)
	INSERT [dbo].[EventsMaster] ([ID], [EventName], [ServerTypeID]) VALUES (143, N'Delete Site', 21)
	INSERT [dbo].[EventsMaster] ([ID], [EventName], [ServerTypeID]) VALUES (144, N'Auto Discovery', 21)
	INSERT [dbo].[EventsMaster] ([ID], [EventName], [ServerTypeID]) VALUES (145, N'IMAP', 21)
	INSERT [dbo].[EventsMaster] ([ID], [EventName], [ServerTypeID]) VALUES (146, N'SMTP', 21)
	INSERT [dbo].[EventsMaster] ([ID], [EventName], [ServerTypeID]) VALUES (147, N'POP3', 21)
	INSERT [dbo].[EventsMaster] ([ID], [EventName], [ServerTypeID]) VALUES (148, N'MAPI Connectivity', 21)
	INSERT [dbo].[EventsMaster] ([ID], [EventName], [ServerTypeID]) VALUES (149, N'Create Calendar Entry', 21)
	INSERT [dbo].[EventsMaster] ([ID], [EventName], [ServerTypeID]) VALUES (150, N'Delete Calendar Entry', 21)
	INSERT [dbo].[EventsMaster] ([ID], [EventName], [ServerTypeID]) VALUES (151, N'OWA', 21)
	INSERT [dbo].[EventsMaster] ([ID], [EventName], [ServerTypeID]) VALUES (152, N'Mail flow', 21)
	INSERT [dbo].[EventsMaster] ([ID], [EventName], [ServerTypeID]) VALUES (153, N'Inbox', 21)
	INSERT [dbo].[EventsMaster] ([ID], [EventName], [ServerTypeID]) VALUES (154, N'OneDrive Upload Document', 21)
	INSERT [dbo].[EventsMaster] ([ID], [EventName], [ServerTypeID]) VALUES (155, N'OneDrive Download Document', 21)
	SET IDENTITY_INSERT [dbo].[EventsMaster] OFF
END
GO

USE [vitalsigns]
GO
IF NOT EXISTS (select * from syscolumns where name= 'HAPoolName' and id = object_id('dbo.Traveler_Devices'))
BEGIN	
	ALTER TABLE dbo.Traveler_Devices ADD HAPoolName [varchar] (150) null
END
GO

USE [vitalsigns]
GO
IF NOT EXISTS (select * from syscolumns where id = object_id('dbo.NetworkDevicesDetails'))
BEGIN
CREATE TABLE [dbo].[NetworkDevicesDetails](
	[NetworkID] [int] NULL,
	[StatName] [nvarchar](50) NULL,
	[StatValue] [nvarchar](50) NULL
) ON [PRIMARY]

END

GO

IF NOT EXISTS (select * from syscolumns where name= 'NetworkType' and id = object_id('dbo.Network Devices'))
BEGIN
	ALTER TABLE [Network Devices] ADD NetworkType varchar(50) NULL
END
GO

IF EXISTS (Select * from NetworkMaster where image is null and id=8)
BEGIN
	UPDATE NetworkMaster SET Image='' Where ID=8
END
GO

/* 2/6/2015 NS added - update Devices and Servers category values */
USE [vitalsigns]
GO

IF EXISTS (select * from ReportItems where Name='Average CPU utilization per day')
BEGIN	
	UPDATE ReportItems SET Category='Domino' WHERE Name='Average CPU utilization per day'
END 
GO

IF EXISTS (select * from ReportItems where Name='Maximum CPU utilization per day')
BEGIN	
	UPDATE ReportItems SET Category='Domino' WHERE Name='Maximum CPU utilization per day'
END 
GO

IF EXISTS (select * from ReportItems where Name='Cluster Seconds On Queue')
BEGIN	
	UPDATE ReportItems SET Category='Domino' WHERE Name='Cluster Seconds On Queue'
END 
GO

IF EXISTS(SELECT * FROM syscolumns where id=Object_id('ReportItems'))
BEGIN
	UPDATE ReportItems SET Category='Servers & Devices' WHERE Category='Devices'
	OR Category='Servers'
END
GO
/* [dbo].[SametimeServers] VSPLUS 1321 Durga */
USE [vitalsigns]
GO

IF EXISTS (select * from syscolumns where id = object_id('dbo.SametimeServers') and name= 'db2login')
BEGIN
	Alter TABLE [dbo].[SametimeServers] Drop Column db2login
END

IF EXISTS (select * from syscolumns where id = object_id('dbo.SametimeServers') and name= 'db2password')
BEGIN
	Alter TABLE [dbo].[SametimeServers] Drop Column db2password
END
USE [vitalsigns]
GO
IF NOT EXISTS (select * from syscolumns where name= 'CredentialID' and id = object_id('dbo.SametimeServers'))
BEGIN	
	ALTER TABLE dbo.SametimeServers ADD CredentialID int null
END
GO

USE [vitalsigns]
GO
IF NOT EXISTS (select * from syscolumns where name= 'Platform' and id = object_id('dbo.SametimeServers'))
BEGIN	
	ALTER TABLE dbo.SametimeServers ADD Platform [nvarchar] (50) null 
END
GO

IF EXISTS (select * from syscolumns where id = object_id('dbo.SametimeServers') and name= 'db2downalert')
BEGIN
	Alter TABLE [dbo].[SametimeServers] Drop Column db2downalert
END

IF EXISTS (select * from syscolumns where id = object_id('dbo.SametimeServers') and name= 'db2backonlinealert')
BEGIN
	Alter TABLE [dbo].[SametimeServers] Drop Column db2backonlinealert
END

IF NOT EXISTS (select * from syscolumns where name= 'CredentialsID' and id = object_id('dbo.WebsphereCell'))
BEGIN	
	ALTER TABLE dbo.WebsphereCell ADD CredentialsID int null 
END
GO

IF NOT EXISTS (select * from syscolumns where name= 'SametimeId' and id = object_id('dbo.WebsphereCell'))
BEGIN	
	ALTER TABLE dbo.WebsphereCell ADD SametimeId [int]  null 
END
GO
USE [vitalsigns]
GO
IF EXISTS (select * from syscolumns where id = object_id('dbo.WebsphereCell') and name= 'UserID')
BEGIN
	Alter TABLE [dbo].[WebsphereCell] Drop Column UserID
END
GO

IF EXISTS (select * from syscolumns where id = object_id('dbo.WebsphereCell') and name= 'Password')
BEGIN
	Alter TABLE [dbo].[WebsphereCell] Drop Column Password
END
GO

USE [vitalsigns]
GO
IF NOT EXISTS (select * from syscolumns where name= 'ServerTypeID' and id = object_id('dbo.Credentials'))
BEGIN	
	ALTER TABLE dbo.Credentials ADD ServerTypeID int null
END
GO
IF EXISTS(select * from syscolumns where id=Object_id('dbo.WebSphereNodes'))
BEGIN	
	drop table dbo.[WebSphereNodes]
END
/* VSPLUS 1320,1321 */
USE [vitalsigns]
GO
Update SametimeServers SET CredentialID=0 where  CredentialID IS NULL 
Update SametimeServers SET Platform='Domino' where  Platform IS NULL 

Update WebsphereCell SET CredentialsID=0 where  CredentialsID IS NULL

IF NOT EXISTS (select * from Credentials where ID=0)
BEGIN	
SET IDENTITY_INSERT [dbo].[Credentials] ON 
INSERT [dbo].[Credentials] ([ID], [AliasName], [UserID], [Password], [ServerTypeID]) VALUES (0, N'Default', N'Default', N'189, 107, 237, 20, 101, 142, 23, 176', 0)
SET IDENTITY_INSERT [dbo].[Credentials] OFF
END
GO


--2/17/15 WS added VSPLUS 1247

USE [vitalsigns]
GO
IF NOT EXISTS (select * from syscolumns where id = object_id('dbo.SharePointSiteCollections'))
BEGIN

CREATE TABLE [dbo].[SharePointSiteCollections](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[SiteCollection] [nvarchar](255) NULL,
	[SizeMB] [nvarchar](50) NULL,
	[Date] [datetime] NULL,
	[FarmName] [nvarchar](255) NULL
) ON [PRIMARY]

END

GO
/* sowjanya-1468 adding columns in logfile*/
use [vitalsigns]
GO
IF NOT EXISTS (select * from syscolumns where name= 'Log' and id = object_id('dbo.LogFile'))
BEGIN
ALTER TABLE dbo.LogFile ADD [Log] [bit] null
END
GO
IF NOT EXISTS (select * from syscolumns where name= 'AgentLog' and id = object_id('dbo.LogFile'))
BEGIN
ALTER TABLE dbo.LogFile ADD AgentLog [bit] null
END
GO

/*1321 Durga */
USE [vitalsigns]
GO
IF NOT EXISTS (select * from syscolumns where name= 'HostName' and id = object_id('dbo.WebsphereServer'))
BEGIN	
	ALTER TABLE dbo.WebsphereServer ADD HostName [nvarchar] (MAX) null
END
GO
/* Swathi */
USE [vitalsigns]
GO
IF EXISTS (select * from syscolumns where id = object_id('dbo.WebsphereCell') and name= 'Certification')
BEGIN
	Alter TABLE [dbo].[WebsphereCell] Drop Column Certification
END
GO
/* 1424 Sowjanya */
use [vitalsigns]
go
IF NOT EXISTS (select * from syscolumns where name= 'EnableTravelerBackend' and id = object_id('dbo.DominoServers'))
BEGIN	
Alter table [dbo].[DominoServers] ADD EnableTravelerBackend [bit] NULL
End
Go
/* Durga 1493 */
USE [vitalsigns]
GO
IF NOT EXISTS (select * from Settings where sname= 'Core Service Start' )
BEGIN	
	INSERT [dbo].[Settings] ([sname],[stype]) VALUES (N'Core Service Start', N'System.String')
END
GO
/* Swathi 1509 */
USE [vitalsigns]
GO
ALTER TABLE [dbo].[Users] ALTER COLUMN [CustomBackground] [varchar](200) NULL
GO
/* Durga 1555*/
USE [vitalsigns]
GO
IF NOT EXISTS (select * from syscolumns where id = object_id('dbo.Servertypeexcludelist'))
BEGIN
CREATE TABLE [dbo].[Servertypeexcludelist](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Page] [nvarchar](50) NULL,
	[Control] [nvarchar](50) NULL,
	[ServertypeID] [int] NULL
) ON [PRIMARY]

END
GO
/* Durga 1555*/
USE [vitalsigns]
GO
DELETE FROM  [dbo].[Servertypeexcludelist]
SET IDENTITY_INSERT [dbo].[Servertypeexcludelist] ON 
INSERT [dbo].[Servertypeexcludelist] ([ID], [Page], [Control], [ServertypeID]) VALUES (1, N'ServerSettingsEditor.aspx', N'DiskSettingsTreeList', 7)
INSERT [dbo].[Servertypeexcludelist] ([ID], [Page], [Control], [ServertypeID]) VALUES (2, N'BiggestMailFiles.aspx', N'TypeComboBox', 1)
INSERT [dbo].[Servertypeexcludelist] ([ID], [Page], [Control], [ServertypeID]) VALUES (3, N'BiggestMailFiles.aspx', N'TypeComboBox', 5)
INSERT [dbo].[Servertypeexcludelist] ([ID], [Page], [Control], [ServertypeID]) VALUES (4, N'MailFiles.aspx?MItem=1', N'TypeComboBox', 1)
INSERT [dbo].[Servertypeexcludelist] ([ID], [Page], [Control], [ServertypeID]) VALUES (5, N'MailFiles.aspx?MItem=1', N'TypeComboBox', 5)
INSERT [dbo].[Servertypeexcludelist] ([ID], [Page], [Control], [ServertypeID]) VALUES (6, N'BiggestExchangeMailFiles.aspx', N'TypeComboBox', 1)
INSERT [dbo].[Servertypeexcludelist] ([ID], [Page], [Control], [ServertypeID]) VALUES (7, N'BiggestExchangeMailFiles.aspx', N'TypeComboBox', 5)
INSERT [dbo].[Servertypeexcludelist] ([ID], [Page], [Control], [ServertypeID]) VALUES (8, N'ExchangeMailFiles.aspx', N'TypeComboBox', 1)
INSERT [dbo].[Servertypeexcludelist] ([ID], [Page], [Control], [ServertypeID]) VALUES (9, N'ExchangeMailFiles.aspx', N'TypeComboBox', 5)
INSERT [dbo].[Servertypeexcludelist] ([ID], [Page], [Control], [ServertypeID]) VALUES (10, N'NetworkLatencyTestServers.aspx', N'NetworkLatencyTestgrd', 4)
INSERT [dbo].[Servertypeexcludelist] ([ID], [Page], [Control], [ServertypeID]) VALUES (11, N'NetworkLatencyTestServers.aspx', N'NetworkLatencyTestgrd', 5)
INSERT [dbo].[Servertypeexcludelist] ([ID], [Page], [Control], [ServertypeID]) VALUES (12, N'NetworkLatencyTestServers.aspx', N'NetworkLatencyTestgrd', 15)
INSERT [dbo].[Servertypeexcludelist] ([ID], [Page], [Control], [ServertypeID]) VALUES (13, N'NetworkLatencyTestServers.aspx', N'NetworkLatencyTestgrd', 16)
INSERT [dbo].[Servertypeexcludelist] ([ID], [Page], [Control], [ServertypeID]) VALUES (14, N'NetworkLatencyTestServers.aspx', N'NetworkLatencyTestgrd', 18)
INSERT [dbo].[Servertypeexcludelist] ([ID], [Page], [Control], [ServertypeID]) VALUES (15, N'NetworkLatencyTestServers.aspx', N'NetworkLatencyTestgrd', 21)
--VSPLUS-1768 Durga
INSERT [dbo].[Servertypeexcludelist] ([ID], [Page], [Control], [ServertypeID]) VALUES (16, N'ServerSettingsEditor.aspx', N'BusinessHoursTreeList', 7)
  --2/25/2016 Durga Modified for  VSPLUS-2611
INSERT [dbo].[Servertypeexcludelist] ([ID], [Page], [Control], [ServertypeID]) VALUES (17, N'O365MailFiles.aspx', N'TypeComboBox', 21)
INSERT [dbo].[Servertypeexcludelist] ([ID], [Page], [Control], [ServertypeID]) VALUES (18, N'O365MailFiles.aspx', N'TypeComboBox', 1)
INSERT [dbo].[Servertypeexcludelist] ([ID], [Page], [Control], [ServertypeID]) VALUES (19, N'O365MailFiles.aspx', N'TypeComboBox', 5)
INSERT [dbo].[Servertypeexcludelist] ([ID], [Page], [Control], [ServertypeID]) VALUES (20, N'ExchangeMailFiles.aspx', N'TypeComboBox', 21)
INSERT [dbo].[Servertypeexcludelist] ([ID], [Page], [Control], [ServertypeID]) VALUES (21, N'MailFiles.aspx?MItem=1', N'TypeComboBox', 21)
--2/29/2016 Durga Modified for  VSPLUS-2688
 INSERT [dbo].[Servertypeexcludelist] ([ID], [Page], [Control], [ServertypeID]) VALUES (22, N'ServerSettingsEditor.aspx', N'ServerTypeComboBox', 21)
SET IDENTITY_INSERT [dbo].[Servertypeexcludelist] OFF

/* 3/9/2015 NS modified the link for the Reports menu item */
/* 2/18/2016 NS commented out for VSPLUS-2588 */
/*
IF EXISTS (select * from MenuItems WHERE DisplayText='Reports')
BEGIN
	UPDATE [MenuItems] SET [PageLink] = '~/DashboardReports/OverallSrvStatusHealthRpt.aspx?M=c'
	WHERE [DisplayText] = 'Reports'
END
GO
*/




/* 3/9/15 WS ADDED FOR HA SUPPORT */

USE [vitalsigns]
GO
IF NOT EXISTS (select * from syscolumns where id = object_id('dbo.NodeDetails'))
BEGIN
	CREATE TABLE [dbo].[NodeDetails](
		[ID] [int] IDENTITY(1,1) NOT NULL,
		[NodeId] [int] NULL,
		[Name] [nvarchar](255) NULL,
		[Value] [nvarchar](255) NULL
	) ON [PRIMARY]

END
GO

IF NOT EXISTS (select * from syscolumns where id = object_id('dbo.Nodes'))
BEGIN

	CREATE TABLE [dbo].[Nodes](
		[ID] [int] IDENTITY(1,1) NOT NULL,
		[Name] [nvarchar](255) NULL,
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
		[IsConfiguredPrimaryNode] [bit] NULL
	) ON [PRIMARY]

END
GO


IF NOT EXISTS (select * from syscolumns where id = object_id('dbo.MonitoringTablesToCollections'))
BEGIN


	CREATE TABLE [dbo].[MonitoringTablesToCollections](
		[ID] [int] IDENTITY(1,1) NOT NULL,
		[TableName] [nvarchar](255) NULL,
		[ServiceCollectionType] [nvarchar](255) NULL
	) ON [PRIMARY]

END
GO

DELETE FROM [dbo].[MonitoringTablesToCollections]

GO

SET IDENTITY_INSERT [dbo].[MonitoringTablesToCollections] ON
INSERT [dbo].[MonitoringTablesToCollections] ([ID], [TableName], [ServiceCollectionType]) VALUES (1, N'DominoServers', N'Domino,ExJournal')
INSERT [dbo].[MonitoringTablesToCollections] ([ID], [TableName], [ServiceCollectionType]) VALUES (2, N'URLs', N'URL')
INSERT [dbo].[MonitoringTablesToCollections] ([ID], [TableName], [ServiceCollectionType]) VALUES (3, N'SametimeServers', N'Sametime')
INSERT [dbo].[MonitoringTablesToCollections] ([ID], [TableName], [ServiceCollectionType]) VALUES (4, N'DominoCluster', N'Domino Cluster')
INSERT [dbo].[MonitoringTablesToCollections] ([ID], [TableName], [ServiceCollectionType]) VALUES (6, N'NotesMailProbe', N'Notes Mail Probe')
INSERT [dbo].[MonitoringTablesToCollections] ([ID], [TableName], [ServiceCollectionType]) VALUES (7, N'Traveler_HA_Datastore', N'Traveler')
INSERT [dbo].[MonitoringTablesToCollections] ([ID], [TableName], [ServiceCollectionType]) VALUES (8, N'LogFile', N'Key Words')
INSERT [dbo].[MonitoringTablesToCollections] ([ID], [TableName], [ServiceCollectionType]) VALUES (9, N'Network Devices', N'Network Devices')
INSERT [dbo].[MonitoringTablesToCollections] ([ID], [TableName], [ServiceCollectionType]) VALUES (10, N'BlackBerryServers', N'Black Berry Servers')
INSERT [dbo].[MonitoringTablesToCollections] ([ID], [TableName], [ServiceCollectionType]) VALUES (11, N'MailServices', N'Mail')
INSERT [dbo].[MonitoringTablesToCollections] ([ID], [TableName], [ServiceCollectionType]) VALUES (12, N'ServerAttributes', N'Exchange,Sharepoint,Active Directory,Windows,Lync,Database Availability Group,WebSphere,IBM Connections')
INSERT [dbo].[MonitoringTablesToCollections] ([ID], [TableName], [ServiceCollectionType]) VALUES (13, N'ExchangeSettings', N'Exchange')
INSERT [dbo].[MonitoringTablesToCollections] ([ID], [TableName], [ServiceCollectionType]) VALUES (14, N'LyncServers', N'Lync')
INSERT [dbo].[MonitoringTablesToCollections] ([ID], [TableName], [ServiceCollectionType]) VALUES (15, N'DagSettings', N'Database Availability Group')
INSERT [dbo].[MonitoringTablesToCollections] ([ID], [TableName], [ServiceCollectionType]) VALUES (16, N'NetworkLatency', N'Network Latency')
INSERT [dbo].[MonitoringTablesToCollections] ([ID], [TableName], [ServiceCollectionType]) VALUES (17, N'NetworkLatencyServers', N'Network Latency')
INSERT [dbo].[MonitoringTablesToCollections] ([ID], [TableName], [ServiceCollectionType]) VALUES (18, N'O365Server', N'Office 365')
INSERT [dbo].[MonitoringTablesToCollections] ([ID], [TableName], [ServiceCollectionType]) VALUES (19, N'Office365Tests', N'Office 365')
INSERT [dbo].[MonitoringTablesToCollections] ([ID], [TableName], [ServiceCollectionType]) VALUES (20, N'NotesDatabases', N'Notes Databases')
INSERT [dbo].[MonitoringTablesToCollections] ([ID], [TableName], [ServiceCollectionType]) VALUES (21, N'WebSphereServer', N'WebSphere')
INSERT [dbo].[MonitoringTablesToCollections] ([ID], [TableName], [ServiceCollectionType]) VALUES (22, N'WebSphereCell', N'WebSphere')
INSERT [dbo].[MonitoringTablesToCollections] ([ID], [TableName], [ServiceCollectionType]) VALUES (23, N'CloudDetails', N'Cloud')
INSERT [dbo].[MonitoringTablesToCollections] ([ID], [TableName], [ServiceCollectionType]) VALUES (24, N'IBMConnectionsServers', N'IBM Connections')
INSERT [dbo].[MonitoringTablesToCollections] ([ID], [TableName], [ServiceCollectionType]) VALUES (25, N'IBMConnectionsTests', N'IBM Connections')
INSERT [dbo].[MonitoringTablesToCollections] ([ID], [TableName], [ServiceCollectionType]) VALUES (26, N'ClusterDatabaseDetails', N'Domino Cluster')
SET IDENTITY_INSERT [dbo].[MonitoringTablesToCollections] OFF

GO

IF NOT EXISTS (select * from syscolumns where name= 'AssignedNodeId' and id = object_id('dbo.DeviceInventory'))
BEGIN	
	ALTER TABLE DeviceInventory ADD [AssignedNodeId] [int] NULL
END

GO

IF NOT EXISTS (select * from syscolumns where name= 'CurrentNodeId' and id = object_id('dbo.DeviceInventory'))
BEGIN	
	ALTER TABLE DeviceInventory ADD [CurrentNodeId] [int] NULL
END

GO

IF NOT EXISTS (select * from syscolumns where name= 'ServerId' and id = object_id('dbo.DagStatus'))
BEGIN	
	ALTER TABLE DagStatus ADD [ServerId] [int] NULL
END

GO

IF NOT EXISTS (select * from sys.Foreign_keys where parent_object_id = OBJECT_ID(N'[dbo].[DagStatus]') and delete_referential_action_desc = 'CASCADE')
BEGIN	
	ALTER TABLE DagStatus ADD FOREIGN KEY (ServerId) REFERENCES Servers(ID) ON DELETE CASCADE
END

GO

IF NOT EXISTS (select * from sys.Foreign_keys where parent_object_id = OBJECT_ID(N'[dbo].[DagMembers]') and delete_referential_action_desc = 'CASCADE')
BEGIN	
	ALTER TABLE DagMembers DROP CONSTRAINT FK_DAGMembers_DAGStatus
	ALTER TABLE DagMembers ADD FOREIGN KEY (DAGID) REFERENCES DagStatus(ID) ON DELETE CASCADE
END

GO

IF NOT EXISTS (select * from sys.Foreign_keys where parent_object_id = OBJECT_ID(N'[dbo].[DagDatabase]') and delete_referential_action_desc = 'CASCADE')
BEGIN	
	ALTER TABLE DagDatabase DROP CONSTRAINT FK_DAGDatabase_DAGMembers
	ALTER TABLE DagDatabase ADD FOREIGN KEY (DagMemberId) REFERENCES DagMembers(ID) ON DELETE CASCADE
END

GO

IF NOT EXISTS (select * from sys.Foreign_keys where parent_object_id = OBJECT_ID(N'[dbo].[DagDatabaseDetails]') and delete_referential_action_desc = 'CASCADE')
BEGIN	
	ALTER TABLE DagDatabaseDetails ADD FOREIGN KEY (DAGID) REFERENCES DagStatus(ID) ON DELETE CASCADE
END

GO

IF NOT EXISTS (select * from syscolumns where name= 'DominoServerId' and id = object_id('dbo.TRAVELER_STATUS'))
BEGIN
	ALTER TABLE dbo.TRAVELER_STATUS ADD DominoServerId INT NULL
	
	ALTER TABLE TRAVELER_STATUS
	ADD FOREIGN KEY ([DominoServerId])
	REFERENCES Servers (ID)
	ON DELETE CASCADE
END
GO

/* 10/2/2015 NS modified for VSPLUS-2182 */
IF EXISTS(select * from ServerTypes where ServerType='Network Latency')
BEGIN
	SET IDENTITY_INSERT [dbo].[ServerTypes] ON
	DELETE FROM ServerTypes WHERE ServerType='Network Latency'
	INSERT [dbo].[ServerTypes] ([ID], [ServerType], [ServerTypeTable], [FeatureId]) VALUES (23,N'Network Latency',N'NetworkLatency',14)
	SET IDENTITY_INSERT [dbo].[ServerTypes] OFF
END
go
IF NOT EXISTS (SELECT * FROM ServerTypes WHERE ID = 23)
BEGIN
	SET IDENTITY_INSERT [dbo].[ServerTypes] ON
	INSERT [dbo].[ServerTypes] ([ID], [ServerType], [ServerTypeTable], [FeatureId]) VALUES (23,N'Network Latency',N'NetworkLatency',14)
	SET IDENTITY_INSERT [dbo].[ServerTypes] OFF
END
GO

IF NOT EXISTS (SELECT * FROM ServerTypes WHERE ID = 24)
BEGIN
	SET IDENTITY_INSERT [dbo].[ServerTypes] ON
	INSERT [dbo].[ServerTypes] ([ID], [ServerType], [ServerTypeTable], [FeatureId]) VALUES (24,N'Notes Traveler',N'Traveler_HA_Datastore',1)
	SET IDENTITY_INSERT [dbo].[ServerTypes] OFF
END
GO

IF NOT EXISTS (SELECT * FROM ServerTypes WHERE ID = 25)
BEGIN
	SET IDENTITY_INSERT [dbo].[ServerTypes] ON
	INSERT [dbo].[ServerTypes] ([ID], [ServerType], [ServerTypeTable], [FeatureId]) VALUES (25,N'Domino Cluster',N'DominoCluster',1)
	SET IDENTITY_INSERT [dbo].[ServerTypes] OFF
END
GO
--3/11/2016 sowjanya Modified for VSPLUS-2650
IF NOT EXISTS(select * from ServerTypes where ServerType='IBM Connections')
BEGIN
	SET IDENTITY_INSERT [dbo].[ServerTypes] ON
	insert into ServerTypes (ID, ServerType, ServerTypeTable, FeatureId) values (27, 'IBM Connections', '', 20)
	SET IDENTITY_INSERT [dbo].[ServerTypes] OFF
END
go


IF NOT EXISTS(SELECT * from DeviceInventory WHERE DeviceTypeID = (Select ID From ServerTypes where ServerType = 'Domino Cluster'))
BEGIN	
	INSERT INTO DeviceInventory(Name,DeviceID,DeviceTypeID,LocationID)
	SELECT Name, ID, (Select ID From ServerTypes where ServerType = 'Domino Cluster'), NULL
	FROM DominoCluster
END
GO


IF NOT EXISTS(SELECT * from DeviceInventory WHERE DeviceTypeID = (Select ID From ServerTypes where ServerType = 'Network Latency'))
BEGIN	
	INSERT INTO DeviceInventory(Name,DeviceID,DeviceTypeID,LocationID)
	SELECT TestName, NetworkLatencyId, (Select ID From ServerTypes where ServerType = 'Network Latency'), NULL
	FROM NetworkLatency
END


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

/* VSPLUS 596 Durga */

USE [vitalsigns]
GO
IF NOT EXISTS (select * from syscolumns where name= 'Starttime' and id = object_id('dbo.HoursIndicator'))
BEGIN	
	ALTER TABLE dbo.HoursIndicator ADD Starttime [nvarchar] (50) NULL
END
GO	
IF NOT EXISTS (select * from syscolumns where name= 'Duration' and id = object_id('dbo.HoursIndicator'))
BEGIN	
	ALTER TABLE dbo.HoursIndicator ADD Duration int NULL
END
GO

IF NOT EXISTS (select * from syscolumns where name= 'Issunday' and id = object_id('dbo.HoursIndicator'))
BEGIN	
	ALTER TABLE dbo.HoursIndicator ADD Issunday bit NULL
END
GO

IF NOT EXISTS (select * from syscolumns where name= 'IsMonday' and id = object_id('dbo.HoursIndicator'))
BEGIN	
	ALTER TABLE dbo.HoursIndicator ADD IsMonday bit null
END
GO

IF NOT EXISTS (select * from syscolumns where name= 'IsTuesday' and id = object_id('dbo.HoursIndicator'))
BEGIN	
	ALTER TABLE dbo.HoursIndicator ADD IsTuesday  bit NULL
END
GO

IF NOT EXISTS (select * from syscolumns where name= 'IsWednesday' and id = object_id('dbo.HoursIndicator'))
BEGIN	
	ALTER TABLE dbo.HoursIndicator ADD IsWednesday  bit NULL
END
GO

IF NOT EXISTS (select * from syscolumns where name= 'IsThursday' and id = object_id('dbo.HoursIndicator'))
BEGIN	
	ALTER TABLE dbo.HoursIndicator ADD IsThursday  bit NULL
END
GO

IF NOT EXISTS (select * from syscolumns where name= 'IsFriday' and id = object_id('dbo.HoursIndicator'))
BEGIN	
	ALTER TABLE dbo.HoursIndicator ADD IsFriday bit NULL
END
GO


IF NOT EXISTS (select * from syscolumns where name= 'Issaturday' and id = object_id('dbo.HoursIndicator'))
BEGIN	
	ALTER TABLE dbo.HoursIndicator ADD Issaturday bit NULL
END
GO

/* 4/23/2015 NS added for VSPLUS-1297*/
IF NOT EXISTS (select * from syscolumns where name= 'UseType' and id = object_id('dbo.HoursIndicator'))
BEGIN	
	ALTER TABLE dbo.HoursIndicator ADD UseType int NULL
END
GO


USE [vitalsigns]
GO
IF NOT EXISTS (select * from syscolumns where name= 'BusinesshoursID' and id = object_id('dbo.Servers'))
BEGIN	
	ALTER TABLE dbo.Servers ADD BusinesshoursID INT NULL
END
GO

USE [vitalsigns]
GO
IF EXISTS (select * from syscolumns where name= 'EndTime' and id = object_id('dbo.AlertDetails'))
BEGIN	
	ALTER TABLE dbo.AlertDetails DROP COLUMN EndTime 
END
GO

--   Sowjanya 1564 ticket  -----
USE [vitalsigns]
GO
IF NOT EXISTS (select * from syscolumns where id = object_id('Traveler_Devices_temp'))
BEGIN
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
	[HAPoolName] [varchar](150) NULL
) ON [PRIMARY]
END
GO	


/* VSPLUS 1299 Durga */
/* 4/23/2015 NS modified for VSPLUS-1297 - UseType=2 for Business Hours, UseType=0 for All and Specific Hours */
  USE [vitalsigns]
  GO
  
  /*5/27/15 WS Modified to not overwrite hours each update */
  IF EXISTS(SELECT * FROM HoursIndicator WHERE StartTime is null AND Type='Business Hours')
  BEGIN
	  declare 
	  @bsstart as nvarchar(50),
	  @issun as bit,@ismon as bit,@istue as bit,@iswen as bit,@isthu as bit,
	  @isfri as bit,@issat as bit,@bsend as nvarchar(50),@duration as int,@starttime as nvarchar(50)
	  select @issun=svalue from Settings where sname='BusinessHoursSunday'
	  select @ismon=svalue from Settings where sname='BusinessHoursMonday'
	  select @istue=svalue from Settings where sname='BusinessHoursTuesday'
	  select @iswen=svalue from Settings where sname='BusinessHoursWednesday'
	  select @isthu=svalue from Settings where sname='BusinessHoursThursday'
	  select @isfri=svalue from Settings where sname='BusinessHoursFriday'
	  select @issat=svalue from Settings where sname='BusinessHoursSaturday'
	  select @bsstart=svalue from Settings where sname='BusinessHoursStart'
	  select @bsend=svalue from Settings where sname='BusinessHoursEnd'
	  set @starttime=@bsstart
	  set @bsstart=convert(datetime ,@bsstart)
	  set @bsend=convert(datetime ,@bsend)
	  set @duration= DATEDIFF(hh, @bsstart, @bsend)*60
	  update HoursIndicator set Starttime=@starttime,Issunday=@issun,IsMonday=@ismon,IsTuesday=@istue,IsWednesday=@iswen,IsThursday=@isthu,IsFriday=@isfri,Issaturday=@issat,
	  Duration=@duration,UseType=2 where ID=0
  END
  GO
  update HoursIndicator set UseType=0 where ID=2 OR ID=3
  GO
  UPDATE AlertDetails set HoursIndicator=0 where HoursIndicator is null
  GO
  UPDATE servers set BusinesshoursID=0 where BusinesshoursID is null
  GO	
  	

IF NOT EXISTS (select * from syscolumns where name= 'IsActive' and id = object_id('dbo.Traveler_Devices'))
BEGIN	
	ALTER TABLE dbo.Traveler_Devices ADD IsActive bit NULL
END
GO


/* 3/18/15 WS Removed triggers on Traveler DB for DeviceInventory Table */

IF EXISTS(SELECT * FROM sys.triggers WHERE name = 'tr_UpdateDeviceInventoryNotesTraveler')
DROP TRIGGER [dbo].[tr_UpdateDeviceInventoryNotesTraveler]
GO

IF EXISTS(SELECT * FROM sys.triggers WHERE name = 'tr_InsertDeviceInventoryNetworkLatency')
DROP TRIGGER [dbo].[tr_InsertDeviceInventoryNetworkLatency]
GO

IF EXISTS(SELECT * FROM sys.triggers WHERE name = 'tr_DeleteDeviceInventoryNetworkLatency')
DROP TRIGGER [dbo].[tr_DeleteDeviceInventoryNetworkLatency]
GO

/*VSPLUS-1589 Durga*/
USE [vitalsigns]
GO
IF EXISTS (select * from syscolumns where name= 'ServerType' and id = object_id('dbo.Credentials'))
BEGIN
	Alter TABLE [dbo].[Credentials] Drop Column ServerType
END
GO



IF NOT EXISTS (select * from syscolumns where name= 'HostName' and id = object_id('dbo.WebsphereNode'))
BEGIN	
	ALTER TABLE dbo.WebsphereNode ADD HostName [nvarchar](200) NULL
END
GO

--- DRS: Licensing Start
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

IF EXISTS (select * from syscolumns where id = object_id('dbo.ServerTypeLicenses'))
BEGIN
delete from dbo.ServerTypeLicenses 

INSERT INTO dbo.ServerTypeLicenses(ServerTypeId,UnitCost) values(1,1)
INSERT INTO dbo.ServerTypeLicenses(ServerTypeId,UnitCost) values(2,0)
INSERT INTO dbo.ServerTypeLicenses(ServerTypeId,UnitCost) values(3,0)
INSERT INTO dbo.ServerTypeLicenses(ServerTypeId,UnitCost) values(4,.5)
INSERT INTO dbo.ServerTypeLicenses(ServerTypeId,UnitCost) values(5,.33)
INSERT INTO dbo.ServerTypeLicenses(ServerTypeId,UnitCost) values(6,0)
INSERT INTO dbo.ServerTypeLicenses(ServerTypeId,UnitCost) values(7,0)
INSERT INTO dbo.ServerTypeLicenses(ServerTypeId,UnitCost) values(8,0)
INSERT INTO dbo.ServerTypeLicenses(ServerTypeId,UnitCost) values(9,0)
INSERT INTO dbo.ServerTypeLicenses(ServerTypeId,UnitCost) values(10,0)
INSERT INTO dbo.ServerTypeLicenses(ServerTypeId,UnitCost) values(11,0)
INSERT INTO dbo.ServerTypeLicenses(ServerTypeId,UnitCost) values(12,0)
INSERT INTO dbo.ServerTypeLicenses(ServerTypeId,UnitCost) values(13,0)
INSERT INTO dbo.ServerTypeLicenses(ServerTypeId,UnitCost) values(14,0)
INSERT INTO dbo.ServerTypeLicenses(ServerTypeId,UnitCost) values(15,.33)
INSERT INTO dbo.ServerTypeLicenses(ServerTypeId,UnitCost) values(16,.33)
INSERT INTO dbo.ServerTypeLicenses(ServerTypeId,UnitCost) values(17,.16)
INSERT INTO dbo.ServerTypeLicenses(ServerTypeId,UnitCost) values(18,.16)
INSERT INTO dbo.ServerTypeLicenses(ServerTypeId,UnitCost) values(19,0)
INSERT INTO dbo.ServerTypeLicenses(ServerTypeId,UnitCost) values(20,.13)
INSERT INTO dbo.ServerTypeLicenses(ServerTypeId,UnitCost) values(21,0)
INSERT INTO dbo.ServerTypeLicenses(ServerTypeId,UnitCost) values(22,1.33)
INSERT INTO dbo.ServerTypeLicenses(ServerTypeId,UnitCost) values(23,0)
INSERT INTO dbo.ServerTypeLicenses(ServerTypeId,UnitCost) values(24,0)
INSERT INTO dbo.ServerTypeLicenses(ServerTypeId,UnitCost) values(25,0)
INSERT INTO dbo.ServerTypeLicenses(ServerTypeId,UnitCost) values(26,0)

END
GO

if exists(select	*
	from	sys.certificates
		inner join	sys.key_encryptions
				on	sys.key_encryptions.thumbprint = sys.certificates.thumbprint
	where	(sys.certificates.[name] = 'EncryptLicenseKey'))
BEGIN
OPEN MASTER KEY DECRYPTION BY PASSWORD = 'VSLicense1234!@#$'
OPEN SYMMETRIC KEY VSLicenseKey DECRYPTION
BY CERTIFICATE EncryptLicenseKey

UPDATE ServerTypeLicenses SET EncUnitCost = ENCRYPTBYKEY(KEY_GUID('VSLicenseKey'),convert(varchar,UnitCost) )


Close SYMMETRIC KEY VSLicenseKey
CLOSE MASTER KEY
END
GO
--- DRS: Licensing End
/* vsplus 1599,1600 */
USE [VitalSigns]
GO
IF NOT EXISTS (select * from syscolumns where name= 'HostName' and id = object_id('dbo.WebsphereCell'))
BEGIN	
	ALTER TABLE dbo.WebsphereCell ADD HostName [nvarchar](200) NULL
END
GO


-- 3/31/15 WS ADDED VSPLUS-1497

IF NOT EXISTS (select * from syscolumns where name= 'ScanServlet' and id = object_id('dbo.DominoServers'))
BEGIN	
	ALTER TABLE dbo.DominoServers ADD ScanServlet bit NULL
END
GO


if NOT EXISTS(select * from sys.all_columns c join sys.tables t on t.object_id = c.object_id
    join sys.schemas s on s.schema_id = t.schema_id join sys.default_constraints d on c.default_object_id = d.object_id
    where t.name = 'Nodes' and c.name = 'HostName' and d.name = 'DF_HostName'
)
BEGIN
	ALTER TABLE [dbo].[Nodes] ADD  CONSTRAINT [DF_HostName]  DEFAULT (N'') FOR [HostName]
END

GO

if NOT EXISTS(select * from sys.all_columns c join sys.tables t on t.object_id = c.object_id
    join sys.schemas s on s.schema_id = t.schema_id join sys.default_constraints d on c.default_object_id = d.object_id
    where t.name = 'Nodes' and c.name = 'Alive' and d.name = 'DF_Alive'
)
BEGIN
	ALTER TABLE [dbo].[Nodes] ADD  CONSTRAINT [DF_Alive]  DEFAULT (N'0') FOR [Alive]
END

GO

if NOT EXISTS(select * from sys.all_columns c join sys.tables t on t.object_id = c.object_id
    join sys.schemas s on s.schema_id = t.schema_id join sys.default_constraints d on c.default_object_id = d.object_id
    where t.name = 'Nodes' and c.name = 'Version' and d.name = 'DF_Version'
)
BEGIN
	ALTER TABLE [dbo].[Nodes] ADD  CONSTRAINT [DF_Version]  DEFAULT (N'1.0.0.0') FOR [Version]
END

GO

if NOT EXISTS(select * from sys.all_columns c join sys.tables t on t.object_id = c.object_id
    join sys.schemas s on s.schema_id = t.schema_id join sys.default_constraints d on c.default_object_id = d.object_id
    where t.name = 'Nodes' and c.name = 'NodeType' and d.name = 'DF_NodeType'
)
BEGIN
	ALTER TABLE [dbo].[Nodes] ADD  CONSTRAINT [DF_NodeType]  DEFAULT (N'') FOR [NodeType]
END

GO

if NOT EXISTS(select * from sys.all_columns c join sys.tables t on t.object_id = c.object_id
    join sys.schemas s on s.schema_id = t.schema_id join sys.default_constraints d on c.default_object_id = d.object_id
    where t.name = 'Nodes' and c.name = 'LoadFactor' and d.name = 'DF_LoadFactor'
)
BEGIN
	ALTER TABLE [dbo].[Nodes] ADD  CONSTRAINT [DF_LoadFactor]  DEFAULT (N'10') FOR [LoadFactor]
END

GO

if NOT EXISTS(select * from sys.all_columns c join sys.tables t on t.object_id = c.object_id
    join sys.schemas s on s.schema_id = t.schema_id join sys.default_constraints d on c.default_object_id = d.object_id
    where t.name = 'Nodes' and c.name = 'IsPrimaryNode' and d.name = 'DF_IsPrimaryNode'
)
BEGIN
	ALTER TABLE [dbo].[Nodes] ADD  CONSTRAINT [DF_IsPrimaryNode]  DEFAULT (N'0') FOR [IsPrimaryNode]
END

GO

/* 4/7/15 WS ADDED FOR VSPLUS 1626 */
IF NOT EXISTS (select * from syscolumns where name= 'LocationID' and id = object_id('dbo.Nodes'))
BEGIN	
	ALTER TABLE dbo.Nodes ADD LocationID int NULL
END
GO

update menuitems set OverrideSort=9 where OverrideSort is null
GO
--DURGA VSPLUS 1581
update menuitems set OverrideSort=1 where DisplayText='Server Settings Editor'
GO

/* 4/14/2015 NS added for VSPLUS-1414 */
USE [vitalsigns]
IF NOT EXISTS (select * from syscolumns where id = object_id('dbo.SystemMessages'))
BEGIN
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
END
GO

USE [vitalsigns]
IF NOT EXISTS (select * from syscolumns where id = object_id('dbo.UserSystemMessages'))
BEGIN
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
END
GO

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_UserSystemMessages_SystemMessages]') AND parent_object_id = OBJECT_ID(N'[dbo].[UserSystemMessages]'))
ALTER TABLE [dbo].[UserSystemMessages]  WITH CHECK ADD  CONSTRAINT [FK_UserSystemMessages_SystemMessages] FOREIGN KEY([SysMsgID])
REFERENCES [dbo].[SystemMessages] ([ID])
ON DELETE CASCADE
GO

IF EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_UserSystemMessages_SystemMessages]') AND parent_object_id = OBJECT_ID(N'[dbo].[UserSystemMessages]'))
ALTER TABLE [dbo].[UserSystemMessages] CHECK CONSTRAINT [FK_UserSystemMessages_SystemMessages]
GO

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_UserSystemMessages_Users]') AND parent_object_id = OBJECT_ID(N'[dbo].[UserSystemMessages]'))
ALTER TABLE [dbo].[UserSystemMessages]  WITH CHECK ADD  CONSTRAINT [FK_UserSystemMessages_Users] FOREIGN KEY([UserID])
REFERENCES [dbo].[Users] ([ID])
ON DELETE CASCADE
GO

IF EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_UserSystemMessages_Users]') AND parent_object_id = OBJECT_ID(N'[dbo].[UserSystemMessages]'))
ALTER TABLE [dbo].[UserSystemMessages] CHECK CONSTRAINT [FK_UserSystemMessages_Users]
GO
/* 1684 swathi*/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Servers_WebsphereServer]') AND parent_object_id = OBJECT_ID(N'[dbo].[Servers]'))
BEGIN
ALTER TABLE [dbo].[WebsphereServer] DROP CONSTRAINT [FK_Servers_WebsphereServer] 
ALTER TABLE [dbo].[WebsphereServer]  WITH CHECK ADD  CONSTRAINT [FK_Servers_WebsphereServer] FOREIGN KEY([ServerID])
REFERENCES [dbo].[Servers] ([ID])
ON DELETE CASCADE
END 
GO

/* 4/14/2015 NS added for VSPLUS-219 */
USE [vitalsigns]
IF NOT EXISTS (select * from syscolumns where id = object_id('dbo.EscalationDetails'))
BEGIN
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
END
GO

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_EscalationDetails_AlertNames]') AND parent_object_id = OBJECT_ID(N'[dbo].[EscalationDetails]'))
ALTER TABLE [dbo].[EscalationDetails]  WITH CHECK ADD  CONSTRAINT [FK_EscalationDetails_AlertNames] FOREIGN KEY([AlertKey])
REFERENCES [dbo].[AlertNames] ([AlertKey])
GO

IF EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_EscalationDetails_AlertNames]') AND parent_object_id = OBJECT_ID(N'[dbo].[EscalationDetails]'))
ALTER TABLE [dbo].[EscalationDetails] CHECK CONSTRAINT [FK_EscalationDetails_AlertNames]
GO

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_EscalationDetails_CustomScripts]') AND parent_object_id = OBJECT_ID(N'[dbo].[EscalationDetails]'))
ALTER TABLE [dbo].[EscalationDetails]  WITH CHECK ADD  CONSTRAINT [FK_EscalationDetails_CustomScripts] FOREIGN KEY([ScriptID])
REFERENCES [dbo].[CustomScripts] ([ID])
GO

IF EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_EscalationDetails_CustomScripts]') AND parent_object_id = OBJECT_ID(N'[dbo].[EscalationDetails]'))
ALTER TABLE [dbo].[EscalationDetails] CHECK CONSTRAINT [FK_EscalationDetails_CustomScripts]
GO

USE [vitalsigns]
IF NOT EXISTS (select * from syscolumns where id = object_id('dbo.EscalationSentDetails'))
BEGIN
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
END
GO

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_EscalationSentDetails_AlertHistory]') AND parent_object_id = OBJECT_ID(N'[dbo].[EscalationSentDetails]'))
ALTER TABLE [dbo].[EscalationSentDetails]  WITH CHECK ADD  CONSTRAINT [FK_EscalationSentDetails_AlertHistory] FOREIGN KEY([AlertHistoryID])
REFERENCES [dbo].[AlertHistory] ([ID])
GO

IF EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_EscalationSentDetails_AlertHistory]') AND parent_object_id = OBJECT_ID(N'[dbo].[EscalationSentDetails]'))
ALTER TABLE [dbo].[EscalationSentDetails] CHECK CONSTRAINT [FK_EscalationSentDetails_AlertHistory]
GO

/* 4/16/2015 NS added for VSPLUS-1659 */
USE [vitalsigns]
IF NOT EXISTS( select * from EventsMaster where EventName='Traveler Data Store' AND ServerTypeId = 1 )
BEGIN
	SET IDENTITY_INSERT [dbo].[EventsMaster] ON
	INSERT [dbo].[EventsMaster] ([ID], [EventName], [ServerTypeID]) VALUES (156, N'Traveler Data Store', 1)
	SET IDENTITY_INSERT [dbo].[EventsMaster] OFF
END
GO

--VSPLUS 1655 Sowjanya
USE [vitalsigns]
GO
IF NOT EXISTS (select * from syscolumns where id = object_id('dbo.O365Nodes'))
BEGIN	
	CREATE TABLE [dbo].[O365Nodes](
	[O365ServerID] [int] NOT NULL,
	[NodeID] [int] NOT NULL)
END
GO

USE [vitalsigns]
GO
SET IDENTITY_INSERT [dbo].[DominoServerTasks] ON
IF EXISTS (select * from dbo.DominoServerTasks where TaskID = 25)
BEGIN
	delete from dbo.DominoServerTasks where TaskID = 25
	INSERT [dbo].[DominoServerTasks] ([TaskID], [TaskName], [ConsoleString], [RetryCount], [FreezeDetect], [MaxBusyTime], [IdleString], [LoadString]) VALUES (25, N'Traveler 9.0+', N'Notes Traveler', 2, 0, 30, N'Running', N'lo traveler')
END
ELSE 
BEGIN
	INSERT [dbo].[DominoServerTasks] ([TaskID], [TaskName], [ConsoleString], [RetryCount], [FreezeDetect], [MaxBusyTime], [IdleString], [LoadString]) VALUES (25, N'Traveler 9.0+', N'Notes Traveler', 2, 0, 30, N'Running', N'lo traveler')
END 

IF NOT EXISTS (select * from dbo.DominoServerTasks where TaskID = 39)
BEGIN 
	INSERT [dbo].[DominoServerTasks] ([TaskID], [TaskName], [ConsoleString], [RetryCount], [FreezeDetect], [MaxBusyTime], [IdleString], [LoadString]) VALUES (39, N'Traveler 9.0.1.3+', N'Traveler', 2, 0, 30, N'Running', N'lo traveler')
END

IF EXISTS (select * from dbo.DominoServerTasks where TaskID = 18)
BEGIN 
	delete from dbo.DominoServerTasks where TaskID = 18
	INSERT [dbo].[DominoServerTasks] ([TaskID], [TaskName], [ConsoleString], [RetryCount], [FreezeDetect], [MaxBusyTime], [IdleString], [LoadString]) VALUES (18, N'Update', N'Indexer', 2, 0, 30, N'Idle', N'lo update')
END

SET IDENTITY_INSERT [dbo].[DominoServerTasks] OFF

GO
--Durga  VSPLUS-1577
USE [vitalsigns]
GO
update menuitems set OverrideSort=1 where DisplayText='Overview'
GO

--4/27/15 WS Added
IF NOT EXISTS (select * from syscolumns where name= 'IsConfiguredPrimaryNode' and id = object_id('dbo.Nodes'))
BEGIN	
	ALTER TABLE dbo.Nodes ADD IsConfiguredPrimaryNode bit NULL
END
GO

--4/27/15 Dhiren: Delete Default Location from previous Install VSPlus 1548
IF EXISTS (select * from Locations where Location = 'Location1')
BEGIN
Delete Locations where Location = 'Location1'
END
GO

/* 4/27/2015 NS added for VSPLUS-1686 */
USE [vitalsigns]
GO

IF EXISTS(SELECT * FROM sys.triggers WHERE name = 'tr_UpdateDeviceInventoryDominoServers')
DROP TRIGGER [dbo].[tr_UpdateDeviceInventoryDominoServers]
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

IF EXISTS(SELECT * FROM sys.triggers WHERE name = 'tr_UpdateDeviceInventorySametimeServers')
DROP TRIGGER [dbo].[tr_UpdateDeviceInventorySametimeServers]
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

IF EXISTS(SELECT * FROM sys.triggers WHERE name = 'tr_UpdateDeviceInventoryServerAttributes')
DROP TRIGGER [dbo].[tr_UpdateDeviceInventoryServerAttributes]
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

USE [vitalsigns]
GO

IF EXISTS(SELECT * FROM sys.triggers WHERE name = 'tr_UpdateDeviceInventoryO365Server')
DROP TRIGGER [dbo].[tr_UpdateDeviceInventoryO365Server]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
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

USE [vitalsigns]
GO

IF EXISTS(SELECT * FROM sys.triggers WHERE name = 'tr_InsertDeviceInventoryO365Server')
DROP TRIGGER [dbo].[tr_InsertDeviceInventoryO365Server]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
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

--Remove The dummy data of WebSphere VSPLUS-1705 Durga
USE [vitalsigns]
GO
delete from WebsphereServer where ServerName in('WS01/APPNode/server','WS02/APPNode1/server1','WS02/APPNode1/server2','WS02/APPNode2/server1','WS02/APPNode2/server2','WS03/APPNode1/server1','WS03/APPNode2/server1','WS03/APPNode2/server2', 'WS03/APPNode3/server1','WS03/APPNode3/server2','WS03/APPNode3/server3')
delete from WebsphereNode where NodeName in('WS01/node','WS02/APPNode1','WS02/APPNode2','WS03/APPNode1','WS03/APPNode2','WS03/APPNode3')
delete from WebsphereCellStats where CellName in('WS01','WS02','WS03')
delete from WebsphereCell where CellName in('WS01','WS02','WS03')
delete from Servers where ServerName in ('WS01', 'WS02', 'WS03', 'WS04', 'WS05', 'WS06', 'WS07', 'WS08', 'WS09', 'WS010', 'WS011')
GO

--VSPLUS 1603 SWATHI
USE [vitalsigns]
GO
Update [dbo].[Network Devices] set ImageURL='~/images/Network_Apps/device-network-icon.png' where  ImageURL='' or ImageURL is null 
GO



/* Sowjanya 1704 */
USE vitalsigns
GO
DELETE from dbo.Settings where sname = 'VSWebPath'
go
INSERT [dbo].[Settings] ([sname],[stype]) VALUES (N'VSWebPath', N'System.String')
go


USE [vitalsigns]
GO

IF EXISTS(SELECT * FROM sys.triggers WHERE name = 'tr_UpdateDeviceInventoryWindowsServers')
DROP TRIGGER [dbo].[tr_UpdateDeviceInventoryWindowsServers]
GO

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

/* 5/1/2015 NS added for VSPLUS-1715*/
USE [vitalsigns]
GO
UPDATE LogFile SET [Log]=1, [AgentLog]=0
WHERE [Log] IS NULL OR [AgentLog] IS NULL
--VSPLUS 1704 Sowjanya
USE [vitalsigns]
GO

IF NOT EXISTS (select * from syscolumns where id = object_id('dbo.VS_AssemblyVersionInfo'))
BEGIN	
	CREATE TABLE [dbo].[VS_AssemblyVersionInfo](
	[AssemblyName] [varchar](50) NULL,
	[AssemblyVersion] [nvarchar](50) NULL,
	[ProductVersion] [nvarchar](50) NULL,
	[BuildDate] [datetime] NULL,
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[FileArea] [nvarchar](50) NULL,
	[NodeName] [nvarchar](100) NULL
) ON [PRIMARY]


END
GO
USE [vitalsigns]
GO
delete from VS_AssemblyVersionInfo where NodeName is null
GO
--5/5/15 WS Added for Node Disabling
IF NOT EXISTS (select * from syscolumns where name= 'isDisabled' and id = object_id('dbo.Nodes'))
BEGIN	
	ALTER TABLE dbo.Nodes ADD isDisabled bit NULL
END
GO

/* 5/7/2015 NS added */
update menuitems set OverrideSort=1 where DisplayText='Servers' and PageLink='~/Configurator/LotusDominoServers.aspx'
GO
if not exists(select * from dbo.Settings where sname='TravelerPageMultiplier')
	insert into dbo.Settings (sname,svalue,stype) values('TravelerPageMultiplier','100','System.Int32')
GO
--VSPLUS 1746 Sowjanya
USE [VitalSigns]
GO

IF NOT EXISTS (select * from syscolumns where id = object_id('dbo.TravelerStatusReasons'))
BEGIN
create table TravelerStatusReasons(
[ID] int Identity(1,1) not null primary key,
[ServerName] [nvarchar](150) NOT NULL,
[Details] [varchar](500) NULL,
[LastUpdate] [datetime] NULL
)
END
GO

/* 5/15/2015 NS added for VSPLUS-1754 */
USE [vitalsigns]
GO
IF NOT EXISTS (select * from syscolumns where name= 'ResourceConstraint' and id = object_id('dbo.Traveler_Status'))
BEGIN	
	ALTER TABLE dbo.Traveler_Status ADD ResourceConstraint [varchar] (50) null
END
GO	


/* 5/15/2015 NS added new event for Traveler - VSPLUS-1754 */
use vitalsigns
go
IF NOT EXISTS (select * from EventsMaster where EventName='Traveler Resource Constraint' )
BEGIN	
	SET IDENTITY_INSERT [dbo].[EventsMaster] ON
	INSERT [dbo].[EventsMaster] ([ID], [EventName], [ServerTypeID]) VALUES (157, N'Traveler Resource Constraint', 1)
	SET IDENTITY_INSERT [dbo].[EventsMaster] OFF
END
GO

/* 5/15/15 WS Added - VSPLUS-1766 */
iF NOT EXISTS(select * from sys.Foreign_keys where parent_object_id = OBJECT_ID(N'[dbo].[Servers]') and referenced_object_id = OBJECT_ID(N'[dbo].[HoursIndicator]'))
BEGIN
	
	Update Servers set BusinessHoursId = (select min(id) from HoursIndicator) where BusinessHoursID not in (Select ID from HoursIndicator)

	ALTER TABLE [dbo].[Servers]  WITH CHECK ADD FOREIGN KEY([BusinesshoursID])
	REFERENCES [dbo].[HoursIndicator] ([ID])
END
GO

USE [vitalsigns]
GO
IF NOT EXISTS (select * from syscolumns where name= 'ScanTravelerServer' and id = object_id('dbo.DominoServers'))
BEGIN	
	ALTER TABLE dbo.DominoServers ADD ScanTravelerServer bit NOT NULL default(1)
END
GO

/* 5/21/15 WS added for VSPLUS-1786 */

IF NOT EXISTS (select * from sys.Foreign_keys where parent_object_id = OBJECT_ID(N'[dbo].[NetworkLatencyServers]') and referenced_object_id=OBJECT_ID(N'[dbo].[Servers]') and delete_referential_action_desc = 'CASCADE')
BEGIN	
	declare @cmd varchar(255)
	select @cmd = 'ALTER TABLE NetworkLatencyServers DROP CONSTRAINT ' + Name from sys.Foreign_keys where parent_object_id = OBJECT_ID(N'[dbo].[NetworkLatencyServers]') and referenced_object_id=OBJECT_ID(N'[dbo].[Servers]')
	execute (@cmd)	
	ALTER TABLE NetworkLatencyServers WITH CHECK ADD CONSTRAINT [FK_NetworkLatencyServers_Servers] FOREIGN KEY (ServerID) REFERENCES Servers(ID) ON DELETE CASCADE
END
GO
--VSPLUS-1784 Durga
USE [vitalsigns]
GO
update servers set ProfileName='0' where ProfileName='' or ProfileName  is null
GO
update servers set ProfileName='0' where ProfileName='Default'
GO
update servers set servers.ProfileName=pn.id  from servers  srv inner join ProfileNames pn on srv.ProfileName=pn.ProfileName
Go


--VSPLUS 1784 WS

--IF EXISTS(SELECT * FROM ProfileNames WHERE ID <> 0 AND ProfileName='Default')
--BEGIN
--	UPDATE ProfilesMaster SET ProfileID=0 WHERE ProfileID=(Select ID FROM ProfileNames WHERE ProfileName='Default')
--	UPDATE Servers SET ProfileName=0 WHERE ProfileName=(Select ID FROM ProfileNames WHERE ProfileName='Default')
--	SET IDENTITY_INSERT [dbo].[ProfileNames] ON
--	DELETE FROM ProfileNames WHERE ProfileName='Default'
--	INSERT [dbo].[ProfileNames]([ID], [ProfileName]) VALUES (0, 'Default')
--	SET IDENTITY_INSERT [dbo].[ProfileNames] OFF
--END
--GO

/* 6/2/2015 NS added for VSPLUS-1661 */
USE [vitalsigns]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[FeatureReports]') AND type in (N'U'))
DROP TABLE [dbo].[FeatureReports]
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

USE [vitalsigns]
GO
DELETE FROM FeatureReports
GO
/* Domino and Traveler Reports */
INSERT [dbo].[FeatureReports] ([FeatureID], [ReportID]) VALUES (1, 36)
INSERT [dbo].[FeatureReports] ([FeatureID], [ReportID]) VALUES (1, 37)
INSERT [dbo].[FeatureReports] ([FeatureID], [ReportID]) VALUES (1, 15)
INSERT [dbo].[FeatureReports] ([FeatureID], [ReportID]) VALUES (1, 23)
INSERT [dbo].[FeatureReports] ([FeatureID], [ReportID]) VALUES (1, 25)
INSERT [dbo].[FeatureReports] ([FeatureID], [ReportID]) VALUES (1, 26)
INSERT [dbo].[FeatureReports] ([FeatureID], [ReportID]) VALUES (1, 27)
INSERT [dbo].[FeatureReports] ([FeatureID], [ReportID]) VALUES (1, 18)
INSERT [dbo].[FeatureReports] ([FeatureID], [ReportID]) VALUES (1, 19)
INSERT [dbo].[FeatureReports] ([FeatureID], [ReportID]) VALUES (1, 20)
INSERT [dbo].[FeatureReports] ([FeatureID], [ReportID]) VALUES (1, 8)
INSERT [dbo].[FeatureReports] ([FeatureID], [ReportID]) VALUES (1, 6)
INSERT [dbo].[FeatureReports] ([FeatureID], [ReportID]) VALUES (1, 2)
INSERT [dbo].[FeatureReports] ([FeatureID], [ReportID]) VALUES (1, 45)
INSERT [dbo].[FeatureReports] ([FeatureID], [ReportID]) VALUES (1, 31)
INSERT [dbo].[FeatureReports] ([FeatureID], [ReportID]) VALUES (1, 32)
INSERT [dbo].[FeatureReports] ([FeatureID], [ReportID]) VALUES (1, 33)
INSERT [dbo].[FeatureReports] ([FeatureID], [ReportID]) VALUES (1, 44)
INSERT [dbo].[FeatureReports] ([FeatureID], [ReportID]) VALUES (1, 50)
/* Exchange Reports */
INSERT [dbo].[FeatureReports] ([FeatureID], [ReportID]) VALUES (2, 52)
/* 1/5/2016 NS added for VSPLUS-1534 */
INSERT [dbo].[FeatureReports] ([FeatureID], [ReportID]) VALUES (2, 56)
/* NotesMail Reports */
--2/22/2016 Sowjanya Modified for VSPLUS-2620
INSERT [dbo].[FeatureReports] ([FeatureID], [ReportID]) VALUES (1, 4)
INSERT [dbo].[FeatureReports] ([FeatureID], [ReportID]) VALUES (4, 11)
/* Mail Reports */
INSERT [dbo].[FeatureReports] ([FeatureID], [ReportID]) VALUES (12, 3)
/* Sametime Reports */
INSERT [dbo].[FeatureReports] ([FeatureID], [ReportID]) VALUES (6, 48)
INSERT [dbo].[FeatureReports] ([FeatureID], [ReportID]) VALUES (6, 49)
/* Common Reports */
INSERT [dbo].[FeatureReports] ([FeatureID], [ReportID]) VALUES (11, 7)
INSERT [dbo].[FeatureReports] ([FeatureID], [ReportID]) VALUES (11, 10)
INSERT [dbo].[FeatureReports] ([FeatureID], [ReportID]) VALUES (11, 41)
INSERT [dbo].[FeatureReports] ([FeatureID], [ReportID]) VALUES (11, 51)
INSERT [dbo].[FeatureReports] ([FeatureID], [ReportID]) VALUES (11, 43)
INSERT [dbo].[FeatureReports] ([FeatureID], [ReportID]) VALUES (11, 16)
INSERT [dbo].[FeatureReports] ([FeatureID], [ReportID]) VALUES (11, 17)
INSERT [dbo].[FeatureReports] ([FeatureID], [ReportID]) VALUES (11, 24)
INSERT [dbo].[FeatureReports] ([FeatureID], [ReportID]) VALUES (11, 34)
INSERT [dbo].[FeatureReports] ([FeatureID], [ReportID]) VALUES (11, 38)
INSERT [dbo].[FeatureReports] ([FeatureID], [ReportID]) VALUES (11, 39)
INSERT [dbo].[FeatureReports] ([FeatureID], [ReportID]) VALUES (11, 5)
INSERT [dbo].[FeatureReports] ([FeatureID], [ReportID]) VALUES (11, 35)
INSERT [dbo].[FeatureReports] ([FeatureID], [ReportID]) VALUES (11, 46)
--2/11/2016 Sowjanya Modified for VSPLUS-2593
INSERT [dbo].[FeatureReports] ([FeatureID], [ReportID]) VALUES (11, 47)
INSERT [dbo].[FeatureReports] ([FeatureID], [ReportID]) VALUES (11, 42)
INSERT [dbo].[FeatureReports] ([FeatureID], [ReportID]) VALUES (11, 53)
INSERT [dbo].[FeatureReports] ([FeatureID], [ReportID]) VALUES (11, 40)
INSERT [dbo].[FeatureReports] ([FeatureID], [ReportID]) VALUES (11, 28)
INSERT [dbo].[FeatureReports] ([FeatureID], [ReportID]) VALUES (11, 29)
INSERT [dbo].[FeatureReports] ([FeatureID], [ReportID]) VALUES (11, 30)
INSERT [dbo].[FeatureReports] ([FeatureID], [ReportID]) VALUES (11, 12)
INSERT [dbo].[FeatureReports] ([FeatureID], [ReportID]) VALUES (11, 13)
INSERT [dbo].[FeatureReports] ([FeatureID], [ReportID]) VALUES (11, 14)
INSERT [dbo].[FeatureReports] ([FeatureID], [ReportID]) VALUES (11, 21)
INSERT [dbo].[FeatureReports] ([FeatureID], [ReportID]) VALUES (11, 22)
INSERT [dbo].[FeatureReports] ([FeatureID], [ReportID]) VALUES (11, 1)
/* 6/19/2015 NS added for VSPLUS-1841 */
INSERT [dbo].[FeatureReports] ([FeatureID], [ReportID]) VALUES (11, 54)
/* 12/8/2015 NS added for VSPLUS-2140 */
INSERT [dbo].[FeatureReports] ([FeatureID], [ReportID]) VALUES (5, 55)
/*2/9/2016 Durga Added for VSPLUS-2174*/
INSERT [dbo].[FeatureReports] ([FeatureID], [ReportID]) VALUES (1, 57)
INSERT [dbo].[FeatureReports] ([FeatureID], [ReportID]) VALUES (1, 58)
INSERT [dbo].[FeatureReports] ([FeatureID], [ReportID]) VALUES (1, 59)
/* 2/24/2016 NS added for VSPLUS-2641 */
INSERT [dbo].[FeatureReports] ([FeatureID], [ReportID]) VALUES (1, 60)
/* 3/9/2016 NS added for VSPLUS-2642 */
INSERT [dbo].[FeatureReports] ([FeatureID], [ReportID]) VALUES (1, 61)
--3/18/2016 Durga Addded for VSPLUS-2702
INSERT [dbo].[FeatureReports] ([FeatureID], [ReportID]) VALUES (5, 62)
/* 3/21/2016 NS added for VSPLUS-2652 */
INSERT [dbo].[FeatureReports] ([FeatureID], [ReportID]) VALUES (19, 63)
--28/3/2016 Durga added for VSPLUS 2695 
INSERT [dbo].[FeatureReports] ([FeatureID], [ReportID]) VALUES (11, 64)
INSERT [dbo].[FeatureReports] ([FeatureID], [ReportID]) VALUES (11, 65)
--28/3/2016 Durga added for VSPLUS 2698 
INSERT [dbo].[FeatureReports] ([FeatureID], [ReportID]) VALUES (11, 66)
--12-04-2016 Durga Modified for VSPLUS-2829
INSERT [dbo].[FeatureReports] ([FeatureID], [ReportID]) VALUES (20, 67)
--12-04-2016 Sowjanya Modified for VSPLUS-2831
INSERT [dbo].[FeatureReports] ([FeatureID], [ReportID]) VALUES (20, 68)
--12-04-2016 Durga Modified for VSPLUS-2836
INSERT [dbo].[FeatureReports] ([FeatureID], [ReportID]) VALUES (20, 69)
--12-04-2016 Sowmya Modified for VSPLUS-2830
INSERT [dbo].[FeatureReports] ([FeatureID], [ReportID]) VALUES (20, 70)
--14-04-2016 Durga Modified for VSPLUS-2832
INSERT [dbo].[FeatureReports] ([FeatureID], [ReportID]) VALUES (20, 71)
--14-04-2016 Sowjanya Modified for VSPLUS-2833
INSERT [dbo].[FeatureReports] ([FeatureID], [ReportID]) VALUES (20, 72)
--22/4/2016 Durga added for  VSPLUS-2806
INSERT [dbo].[FeatureReports] ([FeatureID], [ReportID]) VALUES (1, 73)
/* 6/2/2016 NS added for VSPLUS-3021 */
INSERT [dbo].[FeatureReports] ([FeatureID], [ReportID]) VALUES (20, 74)
/* 6/2/2016 NS added for VSPLUS-3019 */
INSERT [dbo].[FeatureReports] ([FeatureID], [ReportID]) VALUES (20, 75)
--6/3/2016 Sowjanya added for VSPLUS-2895
INSERT [dbo].[FeatureReports] ([FeatureID], [ReportID]) VALUES (20, 76)
--6/3/2016 Sowmya added for VSPLUS-2934
INSERT [dbo].[FeatureReports] ([FeatureID], [ReportID]) VALUES (20, 77)
/* 6/2/2016 NS added for VSPLUS-3025 */
INSERT [dbo].[FeatureReports] ([FeatureID], [ReportID]) VALUES (20, 78)
/* 6/2/2016 NS added for VSPLUS-3025 */
INSERT [dbo].[FeatureReports] ([FeatureID], [ReportID]) VALUES (2, 79)
INSERT [dbo].[FeatureReports] ([FeatureID], [ReportID]) VALUES (19, 80)
/* 6/9/2016 NS added for VSPLUS-3020 */
INSERT [dbo].[FeatureReports] ([FeatureID], [ReportID]) VALUES (20, 81)
GO

GO






-- VSPLUS-1578 Durga
USE [vitalsigns]
GO
IF NOT EXISTS (select * from syscolumns where name= 'DiskInfo' and id = object_id('dbo.DominoDiskSettings'))
BEGIN	
	ALTER TABLE dbo.DominoDiskSettings ADD DiskInfo [nvarchar] (MAX) null
END
GO	

--VSPLUS 1811 Sowjanya
USE [vitalsigns]
GO
IF NOT EXISTS (select * from syscolumns where name= 'IsFirstTimeLogin' and id = object_id('dbo.Users'))
BEGIN	
ALTER TABLE [dbo].[Users] ADD IsFirstTimeLogin bit
End
GO

USE [vitalsigns]
GO
IF Exists(select * from Users where IsFirstTimeLogin is null)
BEGIN
Update Users SET IsFirstTimeLogin='False'  
END 
GO

IF NOT EXISTS (select * from syscolumns where name= 'WsScanMeetingServer' and id = object_id('dbo.SametimeServers'))
	ALTER TABLE dbo.SametimeServers ADD WsScanMeetingServer BIT
go

IF NOT EXISTS (select * from syscolumns where name= 'WsMeetingHost' and id = object_id('dbo.SametimeServers'))
	ALTER TABLE dbo.SametimeServers ADD WsMeetingHost NVARCHAR(255)
go

IF NOT EXISTS (select * from syscolumns where name= 'WsMeetingRequireSSL' and id = object_id('dbo.SametimeServers'))
	ALTER TABLE dbo.SametimeServers ADD WsMeetingRequireSSL BIT
go

IF NOT EXISTS (select * from syscolumns where name= 'WsMeetingPort' and id = object_id('dbo.SametimeServers'))
	ALTER TABLE dbo.SametimeServers ADD WsMeetingPort INT
go

IF NOT EXISTS (select * from syscolumns where name= 'WsScanMediaServer' and id = object_id('dbo.SametimeServers'))
	ALTER TABLE dbo.SametimeServers ADD WsScanMediaServer BIT
go

IF NOT EXISTS (select * from syscolumns where name= 'WsMediaHost' and id = object_id('dbo.SametimeServers'))
	ALTER TABLE dbo.SametimeServers ADD WsMediaHost NVARCHAR(255)
go

IF NOT EXISTS (select * from syscolumns where name= 'WsMediaRequireSSL' and id = object_id('dbo.SametimeServers'))
	ALTER TABLE dbo.SametimeServers ADD WsMediaRequireSSL BIT
go

IF NOT EXISTS (select * from syscolumns where name= 'WsMediaPort' and id = object_id('dbo.SametimeServers'))
	ALTER TABLE dbo.SametimeServers ADD WsMediaPort INT
go

IF NOT EXISTS (select * from syscolumns where name= 'TestChatSimulation' and id = object_id('dbo.SametimeServers'))
BEGIN
	ALTER TABLE dbo.SametimeServers ADD TestChatSimulation BIT
	EXEC('UPDATE dbo.SameTimeServers SET TestChatSimulation = 1')
END
go


IF NOT EXISTS (select * from syscolumns where id = object_id('dbo.SystemMessagesTemp'))
BEGIN

Create table dbo.SystemMessagesTemp(
ID int IDENTITY(1,1),
Details VARCHAR(250),
MessageType bit,
DateCreated datetime
)

END
go

USE [vitalsigns]
GO
IF NOT EXISTS(select * from Settings where sname = 'Log File Size')
BEGIN
INSERT [dbo].[Settings] ([sname], [svalue], [stype]) VALUES (N'Log File Size', N'10', N'System.Int32')
END
GO

IF NOT EXISTS(select * from Settings where sname = 'Log File Rotation')
BEGIN
INSERT [dbo].[Settings] ([sname], [svalue], [stype]) VALUES (N'Log File Rotation', N'10', N'System.Int32')
END
GO

IF NOT EXISTS (select * from syscolumns where name= 'NodeName' and id = object_id('dbo.VS_AssemblyVersionInfo'))
BEGIN	
	ALTER TABLE dbo.VS_AssemblyVersionInfo ADD NodeName nvarchar(100) NULL
END
GO
--VSPLUS SWATHI 1507

USE [vitalsigns]
GO
IF NOT EXISTS(select * from syscolumns where name='cloudindex' and id=object_id('dbo.Users'))
BEGIN
ALTER TABLE Users ADD cloudindex int NULL
END
GO
IF NOT EXISTS(select * from syscolumns where name='premisesindex' and id=object_id('dbo.Users'))
BEGIN
ALTER TABLE Users ADD premisesindex int NULL
END
GO
IF NOT EXISTS(select * from syscolumns where name='networkindex' and id=object_id('dbo.Users'))
BEGIN
ALTER TABLE Users ADD networkindex int NULL
END
GO
IF NOT EXISTS(select * from syscolumns where name='dockindex' and id=object_id('dbo.Users'))
BEGIN
ALTER TABLE Users ADD dockindex int NULL
END
GO

IF NOT EXISTS(select * from syscolumns where name='cloudZone' and id=object_id('dbo.Users'))
BEGIN
ALTER TABLE Users ADD cloudZone  varchar(50) NULL
END
GO
IF NOT EXISTS(select * from syscolumns where name='premisesZone' and id=object_id('dbo.Users'))
BEGIN
ALTER TABLE Users ADD premisesZone varchar(50) NULL
END
GO
IF NOT EXISTS(select * from syscolumns where name='networkZone' and id=object_id('dbo.Users'))
BEGIN
ALTER TABLE Users ADD networkZone  varchar(50) NULL
END
GO
IF NOT EXISTS(select * from syscolumns where name='DockZone' and id=object_id('dbo.Users'))
BEGIN 
ALTER TABLE Users ADD DockZone varchar(50) NULL
END
GO

IF NOT EXISTS (select * from syscolumns where name= 'ChatUser1CredentialsId' and id = object_id('dbo.SametimeServers'))
	Alter Table dbo.Sametimeservers Add ChatUser1CredentialsId INT
go
IF NOT EXISTS (select * from syscolumns where name= 'ChatUser2CredentialsId' and id = object_id('dbo.SametimeServers'))
	Alter Table dbo.Sametimeservers Add ChatUser2CredentialsId INT
go

IF NOT EXISTS (select * from syscolumns where name= 'STExtendedStatsPort' and id = object_id('dbo.SametimeServers'))
	ALTER TABLE dbo.SametimeServers ADD STExtendedStatsPort INT
go
IF NOT EXISTS (select * from syscolumns where name= 'STScanExtendedStats' and id = object_id('dbo.SametimeServers'))
BEGIN
	ALTER TABLE dbo.SametimeServers ADD STScanExtendedStats BIT
	EXEC('UPDATE dbo.SameTimeServers SET STScanExtendedStats = 1')
END
go

/* 6/17/2015 WS added for VSPLUS-1759 */
USE [vitalsigns]
go
IF NOT EXISTS (select * from EventsMaster where ID=158 )
BEGIN	
	SET IDENTITY_INSERT [dbo].[EventsMaster] ON
	INSERT [dbo].[EventsMaster] ([ID], [EventName], [ServerTypeID]) VALUES (158, N'Health Check', 5)
	SET IDENTITY_INSERT [dbo].[EventsMaster] OFF
END
GO
/* Dhiren added for VSPLUS-863 */
USE [vitalsigns]
GO
IF NOT EXISTS(select * from syscolumns where name='Load_Cluster_Rep_Delays_Threshold' and id=object_id('dbo.DominoServers'))
BEGIN
ALTER TABLE DominoServers ADD Load_Cluster_Rep_Delays_Threshold float
END
GO
/* 6/17/2015 WS added for VSPLUS-1872 */

IF NOT EXISTS (select * from sys.Foreign_keys where parent_object_id = OBJECT_ID(N'[dbo].[VS_AssemblyVersionInfo]') and referenced_object_id=OBJECT_ID(N'[dbo].[Nodes]'))
BEGIN
	--Cleans up any entries that do not exist in the Nodes table already
	DELETE FROM VS_AssemblyVersionInfo WHERE NodeName NOT IN (SELECT Name FROM Nodes)

	--Make the Name in the Nodes table unique
	ALTER TABLE Nodes
	ADD Constraint UQ_Nodes_Name UNIQUE (Name)
	
	--Changes the type and size of the NodeName to match that of the Name column
	ALTER TABLE VS_AssemblyVersionInfo
	ALTER COLUMN NodeName NVARCHAR(255)

	--Adds the foreign key
	ALTER TABLE VS_AssemblyVersionInfo
	ADD CONSTRAINT fk_VS_AssemblyVersionInfo_NodeName_Node_Name
	FOREIGN KEY (NodeName)
	REFERENCES Nodes(Name) 
	ON DELETE CASCADE

END




/* 6/23/15 WS Added */
DELETE FROM ServerTypes WHERE ServerType='Notes Traveler'
GO

/* 6/25/2015 NS added for VSPLUS-1802 */
USE vitalsigns
GO
IF NOT EXISTS (select * from syscolumns where name= 'EXJStartTime' and id = object_id('dbo.DominoServers'))
BEGIN	
	ALTER TABLE DominoServers ADD EXJStartTime nvarchar(50) NULL
END
GO
IF NOT EXISTS (select * from syscolumns where name= 'EXJDuration' and id = object_id('dbo.DominoServers'))
BEGIN	
	ALTER TABLE DominoServers ADD EXJDuration int NULL
END
GO
IF NOT EXISTS (select * from syscolumns where name= 'EXJLookBackDuration' and id = object_id('dbo.DominoServers'))
BEGIN	
	ALTER TABLE DominoServers ADD EXJLookBackDuration int NULL
END
GO
IF NOT EXISTS (select * from syscolumns where name= 'EXJEnabled' and id = object_id('dbo.DominoServers'))
BEGIN	
	ALTER TABLE DominoServers ADD EXJEnabled bit NULL
END
GO
IF NOT EXISTS(select * from sys.all_columns c join sys.tables t on t.object_id = c.object_id
    join sys.schemas s on s.schema_id = t.schema_id join sys.default_constraints d on c.default_object_id = d.object_id
    where t.name = 'DominoServers' and c.name = 'EXJEnabled' and d.name = 'DF_DominoServers_EXJEnabled')
BEGIN
	ALTER TABLE [dbo].[DominoServers] ADD  CONSTRAINT [DF_DominoServers_EXJEnabled]  DEFAULT ((0)) FOR [EXJEnabled]
END
GO
UPDATE DominoServers SET EXJEnabled=0 WHERE EXJEnabled IS NULL
GO
IF (SELECT IS_NULLABLE from INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME ='DominoServers' AND COLUMN_NAME ='EXJEnabled') != 'NO'
BEGIN
	ALTER TABLE DominoServers ALTER COLUMN EXJEnabled bit NOT NULL
END
GO

/* 6/25/2015 NS added for VSPLUS-1802 */
USE [vitalsigns]
GO
IF NOT EXISTS (select * from syscolumns where id = object_id('dbo.EXJournalStats'))
BEGIN	
	CREATE TABLE [dbo].[EXJournalStats](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[EXJournalDB] [varchar](50) NOT NULL,
	[Delta] [int] NOT NULL,
	[DocCount] [int] NOT NULL,
	[DateUpdated] [datetime] NOT NULL,
 CONSTRAINT [PK_EXJournalStats] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
--Swathi VSPLUS 1651
IF NOT EXISTS (select * from Settings where sname= 'WebSphereAppClientPath' )
BEGIN	
	INSERT [dbo].[Settings] ([sname], [svalue], [stype]) VALUES (N'WebSphereAppClientPath', N'C:\Program Files (x86)\IBM\WebSphere\AppClient\', N'System.String')
END
GO

/* 6/26/2015 NS added for VSPLUS-1802*/
USE [vitalsigns]
GO
IF NOT EXISTS (select * from syscolumns where id = object_id('dbo.DailyCleanup'))
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

IF NOT EXISTS (select * from DailyCleanup where DBName='vitalsigns' AND TableName='EXJournalStats' AND Parameter='DateUpdated')
BEGIN	
	SET IDENTITY_INSERT [dbo].[DailyCleanup] ON
	INSERT [dbo].[DailyCleanup] ([ID], [DBName], [TableName], [ParameterType], [Parameter], [Condition], [Value]) VALUES (1, N'vitalsigns', N'EXJournalStats', N'DateTime', N'DateUpdated', N'<', N'GETDATE()')
	SET IDENTITY_INSERT [dbo].[DailyCleanup] OFF
END
GO
--VSPLUS SWATHI 1507
--2/16/2016 Durga Modified for VSPLUS 2595

USE [vitalsigns]
GO
IF Exists(select * from Users where cloudindex is null)
BEGIN
Update Users SET [cloudindex]=3 where  [cloudindex] IS NULL 
END 
GO
USE [vitalsigns]
GO
IF Exists(select * from Users where premisesindex is null)
BEGIN
Update Users SET [premisesindex]=0 where  [premisesindex] IS NULL
END 
GO
USE [vitalsigns]
GO
IF Exists(select * from Users where networkindex is null)
BEGIN
Update Users SET [networkindex]=1 where  [networkindex] IS NULL 
END 
GO
USE [vitalsigns]
GO
IF Exists(select * from Users where dockindex is null)
BEGIN
Update Users SET [dockindex]=0 where  [dockindex] IS NULL
END 
GO
USE [vitalsigns]
GO
IF Exists(select * from Users where premisesZone is null)
BEGIN
Update Users SET [premisesZone]='LeftZone' where  [premisesZone] IS NULL
END 
GO
USE [vitalsigns]
GO
IF Exists(select * from Users where networkZone is null)
BEGIN
Update Users SET [networkZone]='FarRightZone' where  [networkZone] IS NULL
END 
GO
USE [vitalsigns]
GO
IF Exists(select * from Users where DockZone is null)
BEGIN
Update Users SET [DockZone]='RightZone' where  [DockZone] IS NULL
END 
GO
USE [vitalsigns]
GO
IF Exists(select * from Users where cloudZone is null)
BEGIN
Update Users SET [cloudZone]='FarRightZone' where  [cloudZone] IS NULL
END 
GO
--VSPLUS 1840 DURGA
USE [vitalsigns]
GO
IF NOT EXISTS (select * from ServerTypes where ID=26)
BEGIN
SET IDENTITY_INSERT [dbo].[ServerTypes] ON
INSERT [dbo].[ServerTypes] ([ID], [ServerType], [ServerTypeTable], [FeatureId]) VALUES (26, N'ExchangeMail Probe', N'', 2)
SET IDENTITY_INSERT [dbo].[ServerTypes] OFF
END
GO

/*7/2/15 WS ADDED FOR 1557 */
IF NOT EXISTS (SELECT * from INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME ='Status_History' AND COLUMN_NAME ='ID')
BEGIN
	alter table Status_History Add ID int Identity(1,1) 
END
GO

USE [vitalsigns]
GO
-- convert sametime users from settings to credentials mode

declare
@stUser1 varchar(100),
@stPwd varchar(255),
@stUser2 varchar(100),
@stPwd2 varchar(255),
@stID1 int,
@stID2 int

select @stUser1=svalue from settings where sname='Sametime User 1'
select @stUser2=svalue from settings where sname='Sametime User 2'
select @stPwd=svalue from settings where sname='Sametime Password 1'
select @stPwd2=svalue from settings where sname='Sametime Password 2'
if @stUser1 is not null and @stUser2 is not null and @stPwd is not null and @stPwd2 is not null
begin
if not exists(select * from dbo.Credentials where AliasName='ST User1')
	begin
	insert into dbo.Credentials(AliasName,UserID,Password,ServerTypeID)
	values( 'ST User1',@stUser1,@stPwd,3)
	set @stID1=@@IDENTITY 
	update dbo.SametimeServers set ChatUser1CredentialsId=@stID1 where (ChatUser1CredentialsId IS NULL OR ChatUser1CredentialsId =0)
	end
if not exists(select * from dbo.Credentials where AliasName='ST User2')
	begin
	insert into dbo.Credentials(AliasName,UserID,Password,ServerTypeID)
	values( 'ST User2',@stUser2,@stPwd2,3)
	set @stID2=@@IDENTITY 
	update dbo.SametimeServers set ChatUser2CredentialsId=@stID2 where (ChatUser2CredentialsId IS NULL OR ChatUser2CredentialsId =0)
	end

end

--Websphere changes Sowjanya
USE [VSS_Statistics]
GO
/****** Object:  Table [dbo].[WebSphereDailyStats]    Script Date: 07/08/2015 17:26:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (select * from syscolumns where id = object_id('dbo.WebSphereDailyStats'))
BEGIN
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
END
GO

/* 7/8/15 WS Added for 1875 */
USE [vitalsigns]
GO 
IF NOT EXISTS(SELECT * FROM Settings WHERE sname = 'Domino Thread Killed Counter Max')
BEGIN

	INSERT [dbo].[Settings] ([sname], [svalue], [stype]) VALUES (N'Domino Thread Killed Counter Max', N'5', N'System.Int')

END
GO

/* 7/10/2015 NS added for VSPLUS-1926 */
USE [vitalsigns]
GO
IF  NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_AlertDetails_HoursIndicator]') AND parent_object_id = OBJECT_ID(N'[dbo].[AlertDetails]'))
BEGIN
	ALTER TABLE [dbo].[AlertDetails]  WITH CHECK ADD  CONSTRAINT [FK_AlertDetails_HoursIndicator] FOREIGN KEY([HoursIndicator])
	REFERENCES [dbo].[HoursIndicator] ([ID])
	ALTER TABLE [dbo].[AlertDetails] CHECK CONSTRAINT [FK_AlertDetails_HoursIndicator]
END
GO

/* 7/20/15 WS Added for VSPLUS-1906 */
USE [vitalsigns]
GO
IF NOT EXISTS (select * from syscolumns where name= 'AuthenticationType' and id = object_id('dbo.ExchangeSettings'))
BEGIN
	 ALTER TABLE dbo.ExchangeSettings ADD [AuthenticationType] [varchar](50) NULL
END
GO

/* 7/22/15 WS Added for 1875 */
USE [vitalsigns]
GO
IF NOT EXISTS(SELECT * FROM Settings WHERE sname = 'Domino Thread Killed Counter Max')
BEGIN

	INSERT [dbo].[Settings] ([sname], [svalue], [stype]) VALUES (N'Domino Thread Killed Counter Max', N'5', N'System.Int')

END
GO

/*7/22/15 WS ADDED FOR 1557 */
IF NOT EXISTS (SELECT * from INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME ='Status_History' AND COLUMN_NAME ='ID')
BEGIN
	alter table Status_History Add ID int Identity(1,1) 
END
GO

/*7/22/15 WS Added */
--Removes identity on CellID. No data should be in the table unless manually added
IF EXISTS (select * from syscolumns where name='CellID' and id = object_id('dbo.WebSphereCellStats') and colstat=1)
BEGIN
	--drop the foreign key
	DECLARE @WebSphereCellStatsForeignKeyName varchar(255) 
	SELECT @WebSphereCellStatsForeignKeyName=name FROM sys.foreign_keys  WHERE parent_object_id = OBJECT_ID(N'[dbo].[WebSphereCellStats]') and referenced_object_id=OBJECT_ID(N'[dbo].[WebSphereCell]')
	EXEC('ALTER TABLE dbo.WebSphereCellStats DROP CONSTRAINT ' + @WebSphereCellStatsForeignKeyName)

	--drop the column
	ALTER TABLE dbo.WebSphereCellStats DROP COLUMN CellID
	
	--add the column back
	ALTER TABLE dbo.WebSphereCellStats ADD [CellID] [int] NULL
	
	--add the foreign key
	EXEC('ALTER TABLE [dbo].[WebsphereCellStats] ADD CONSTRAINT WebSphereCellStats_CellID_WebSphereCell_CellID FOREIGN KEY([CellID]) REFERENCES [dbo].[WebsphereCell] ([CellID])')
END
go


/* 7/22/2015 NS added for VSPLUS-1562 */
USE [vitalsigns]
GO
IF NOT EXISTS (select * from syscolumns where id = object_id('dbo.AlertEmergencyContacts'))
CREATE TABLE [dbo].[AlertEmergencyContacts](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Email] [nvarchar](150) NOT NULL,
 CONSTRAINT [PK_AlertEmergencyContacts] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

/* 7/24/2015 NS added for VSPLUS-1226 */
IF NOT EXISTS (select * from Settings where sname='CleanUpTablesDate')
BEGIN	
	SET IDENTITY_INSERT [dbo].[DailyCleanup] ON
	INSERT [dbo].[Settings] ([sname], [svalue], [stype]) VALUES (N'CleanUpTablesDate', NULL, N'System.String')
	SET IDENTITY_INSERT [dbo].[DailyCleanup] OFF
END
GO


--VSPLUS-1858 DURGA
USE [vitalsigns]
GO

if exists(SELECT name FROM dbo.sysobjects WHERE name = 'tr_RefreshCollection' AND type = 'TR')
	ALTER TABLE dbo.DeviceInventory DISABLE TRIGGER tr_RefreshCollection
go
if exists(select * from Traveler_Devices)
begin

print('update device inventory')
delete from DeviceInventory where DeviceTypeID =11

insert into DeviceInventory(Name,DeviceTypeID)
select distinct TD.UserName+'-' +TD.DeviceID,11  from MobileUserThreshold MUT,Traveler_Devices TD where MUT.DeviceId =TD.DeviceID 
end
else if exists(select * from Traveler_Devices_temp)
begin
print('update device inventory from temp')
delete from DeviceInventory where DeviceTypeID =11

insert into DeviceInventory(Name,DeviceTypeID)
select distinct TD.UserName+'-' +TD.DeviceID,11  from MobileUserThreshold MUT,Traveler_Devices_temp  TD where MUT.DeviceId =TD.DeviceID 
end
GO

if exists(SELECT name FROM dbo.sysobjects WHERE name = 'tr_RefreshCollection' AND type = 'TR')
	ALTER TABLE dbo.DeviceInventory ENABLE TRIGGER tr_RefreshCollection
go

/* 7/2/2015 NS modified for VSPLUS-1925 */
/* DELETE FROM [Traveler_Devices] */
/* Moved the code to this location because to populate the DeviceInventory table first and then delete records */ 
IF Exists(select * from syscolumns where id = object_id('dbo.Traveler_Devices_temp'))
DELETE FROM [Traveler_Devices_temp]
GO


/* 7/31/15 WS Added for VSPLUS-2033 */
USE [VitalSigns]
GO

IF EXISTS (SELECT * FROM MailServices WHERE LocationID NOT IN (SELECT ID FROM Locations))
BEGIN
	DECLARE @currentMailServiceKey INT
	SELECT @currentMailServiceKey=min([key]) from MailServices
	while @currentMailServiceKey <= (select max([key]) from MailServices WHERE LocationID NOT IN (SELECT ID FROM Locations))
	begin
		update MailServices set LocationID = (select min(id) from Locations) where LocationID not in (select ID from Locations) and [key] =  @currentMailServiceKey
		select @currentMailServiceKey = min([key]) from MailServices where [key] > @currentMailServiceKey and LocationID NOT IN (SELECT ID FROM Locations)
	end
END
GO

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[fk_MailServices_LocationID]') AND parent_object_id = OBJECT_ID(N'[dbo].[MailServices]'))
BEGIN
	ALTER TABLE [dbo].[MailServices] ADD CONSTRAINT fk_MailServices_LocationID
	FOREIGN KEY([LocationID]) REFERENCES [dbo].[Locations] ([ID])
END
GO

IF NOT EXISTS (SELECT * from INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME ='Traveler_Devices_Temp' AND COLUMN_NAME ='IsActive')
BEGIN
	Alter Table Traveler_Devices_Temp Add IsActive bit
END
GO


/* 8/7/15 WS added for VSPLUS 1701 */
use [VitalSigns]

go

if exists(select * from DominoCluster where ServerID_A not in (select ID from servers) or ServerID_B not in (select ID from servers) or ServerID_C not in (select ID from servers))
begin
	/*Removes entries that the server for was deleted to allow a safe Foreign Key */
	DECLARE @currentDominoClusterID int
	select @currentDominoClusterID=min(id) from DominoCluster
	while @currentDominoClusterID <= (select max(id) from DominoCluster)
	begin
		update DominoCluster set ServerID_A = 0, Enabled = 0 where ServerID_A not in (select ID from servers) and ID =  @currentDominoClusterID
		update DominoCluster set ServerID_B = 0, Enabled = 0 where ServerID_B not in (select ID from servers) and ID =  @currentDominoClusterID
		update DominoCluster set ServerID_C = 0 where ServerID_C not in (select ID from servers) and ID =  @currentDominoClusterID
		select @currentDominoClusterID = min(id) from DominoCluster where id > @currentDominoClusterID
	end
end

GO

IF EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[fk_DominoCluster_Servers_A]') AND parent_object_id = OBJECT_ID(N'[dbo].[DominoCluster]'))
BEGIN
	ALTER TABLE DominoCluster DROP CONSTRAINT fk_DominoCluster_Servers_A
END

GO

IF EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[fk_DominoCluster_Servers_B]') AND parent_object_id = OBJECT_ID(N'[dbo].[DominoCluster]'))
BEGIN
	ALTER TABLE DominoCluster DROP CONSTRAINT fk_DominoCluster_Servers_B
END

GO

IF EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[fk_DominoCluster_Servers_C]') AND parent_object_id = OBJECT_ID(N'[dbo].[DominoCluster]'))
BEGIN
	ALTER TABLE DominoCluster DROP CONSTRAINT fk_DominoCluster_Servers_C
END

GO

/*Deletes old entries in the DominoClusterHealth table.  The DominoClusterHealth management is handled in the web when a server is deleted now */
DELETE FROM [dbo].[DominoClusterHealth] WHERE [ServerName] NOT IN (SELECT [ServerName] FROM [dbo].[Servers] WHERE [ServerTypeId] = '1')

GO
USE [vitalsigns]
GO
IF NOT EXISTS (select * from syscolumns where name= 'Name' and id = object_id('dbo.WebsphereCell'))
BEGIN	
	ALTER TABLE dbo.WebsphereCell ADD Name [nvarchar] (MAX) null
END
GO	

/* 8/13/2015 WS added for VSPLUS-2003 */
IF NOT EXISTS (select * from Settings where sname='Traveler Status Send Tell Command')
BEGIN	
	INSERT [dbo].[Settings] ([sname], [svalue], [stype]) VALUES (N'Traveler Status Send Tell Command', '', N'System.String')
END
GO
/* VSPLUS-2079-SWATHI*/
IF NOT EXISTS (select * from Settings where sname='Show Dashboard only/Exec Summary Buttons')
BEGIN	
	INSERT [dbo].[Settings] ([sname], [svalue], [stype]) VALUES (N'Show Dashboard only/Exec Summary Buttons', N'True', N'System.String')
END
GO
--VSPLUS Somaraju 1669
USE [vitalsigns]
GO
IF NOT EXISTS (select * from syscolumns where name= 'Templateid' and id = object_id('dbo.AlertNames'))
BEGIN	
	ALTER TABLE dbo.AlertNames ADD Templateid int null
END
GO	
USE [vitalsigns]
GO

IF NOT EXISTS (select * from syscolumns where id = object_id('dbo.EventsTemplate'))
BEGIN	
	CREATE TABLE [dbo].[EventsTemplate](
 [ID] [int] IDENTITY(1,1) NOT NULL,
 [Name] [nvarchar](max) NULL,
 [EventID] [nvarchar](max) NULL
) ON [PRIMARY]

END
GO
--VSPLUS 2129 SWATHI
USE [vitalsigns]
GO

IF EXISTS (select * from syscolumns where name= 'Imageurl' and id = object_id('dbo.CloudDetails'))
BEGIN
ALTER TABLE [dbo].[CloudDetails] ALTER COLUMN [Imageurl] [nvarchar](100) NULL
END
GO


IF  NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[Servers]') AND name = N'IX_SERVERS_NAME_TYPE')
	CREATE UNIQUE INDEX IX_SERVERS_NAME_TYPE ON [dbo].[Servers] ([ServerName],[ServerTypeID])
GO

IF  NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[DeviceInventory]') AND name = N'IX_DI_NAME_ID_TYPE')
	CREATE UNIQUE INDEX IX_DI_NAME_ID_TYPE ON [dbo].[DeviceInventory] ([Name],[DeviceId],[DeviceTypeID])
GO


/*9/30/15 WS altered for VSPLUS-2193 */

USE [vitalsigns]
GO

IF NOT EXISTS (select * from syscolumns where id = object_id('dbo.WebsphereServerDetails'))
BEGIN
	CREATE TABLE [dbo].[WebsphereServerDetails](
		[ID] [int] IDENTITY(1,1) NOT NULL,
		[ServerID] [int] NOT NULL,
		[ProcessID] [int] NOT NULL,
		[UpTimeSeconds] [int] NOT NULL
	)
	
	ALTER TABLE [dbo].[WebsphereServerDetails]  WITH CHECK ADD  CONSTRAINT [FK_Servers_WebsphereServerDetails] FOREIGN KEY([ServerID])
	REFERENCES [dbo].[Servers] ([ID])
	ON DELETE CASCADE

	ALTER TABLE [dbo].[WebsphereServerDetails] CHECK CONSTRAINT [FK_Servers_WebsphereServerDetails]
END

GO


USE [vitalsigns]
GO
IF NOT EXISTS (select * from syscolumns where name= 'Mode' and id = object_id('dbo.O365Server'))
BEGIN	
	ALTER TABLE dbo.O365Server ADD Mode [nvarchar] (50) null 
END
GO


USE [vitalsigns]
GO
IF NOT EXISTS (select * from syscolumns where name= 'ServerName' and id = object_id('dbo.O365Server'))
BEGIN	
	ALTER TABLE dbo.O365Server ADD ServerName [nvarchar] (100) null 
END
GO

USE [vitalsigns]
GO
IF NOT EXISTS (select * from syscolumns where name= 'CredentialsId' and id = object_id('dbo.O365Server'))
BEGIN	
	ALTER TABLE dbo.O365Server ADD CredentialsId  int null 
END
GO

/* 10/1/2015 NS added for VSPLUS-2150 */
USE [vitalsigns]
GO
IF NOT EXISTS (select * from syscolumns where name= 'ReplicaID' and id = object_id('dbo.ClusterDatabaseDetails'))
BEGIN	
	ALTER TABLE dbo.ClusterDatabaseDetails ADD [ReplicaID] [varchar](50) NOT NULL DEFAULT ''
END
GO	

/*10/6/15 WS Modified for VSPLUS-2249 */
UPDATE ServerTypes SET ServerType = 'WebSphere' WHERE servertype='WebSphere Server'

GO
--VSPLUS 2245 SOwjanya
USE [vitalsigns]
GO
DELETE FROM [dbo].[UserSecurityQuestions] where id in (4,5)
SET IDENTITY_INSERT [dbo].[UserSecurityQuestions] ON
INSERT [dbo].[UserSecurityQuestions] ([ID], [SecurityQuestion]) VALUES (4, N'What was the name of the first school you attended?')
INSERT [dbo].[UserSecurityQuestions] ([ID], [SecurityQuestion]) VALUES (5, N'What was the name of the first street you lived on?')
SET IDENTITY_INSERT [dbo].[UserSecurityQuestions] OFF
GO


/* 10/9/2015 NS added for VSPLUS-2196 */
IF NOT EXISTS (select * from syscolumns where name= 'ServerName' and id = object_id('dbo.EXJournalStats'))
BEGIN
ALTER TABLE EXJournalStats ADD ServerName nvarchar(255) NOT NULL DEFAULT ''
END

/* 10/9/2015 NS added for VSPLUS-2252 */
/* 10/14/2015 NS corrected the syntax below */
USE [vitalsigns]
GO
IF NOT EXISTS (select * from syscolumns where id = object_id('dbo.DominoServerDetails'))
BEGIN
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


ALTER TABLE [dbo].[DominoServerDetails]  WITH CHECK ADD  CONSTRAINT [FK_DominoServerDetails_Servers] FOREIGN KEY([ServerID])
REFERENCES [dbo].[Servers] ([ID])

ALTER TABLE [dbo].[DominoServerDetails] CHECK CONSTRAINT [FK_DominoServerDetails_Servers]
END

GO

/* 10/26/15 WS Added for VSPLUS-1347 */
USE [VSS_Statistics]
GO

IF NOT EXISTS (select * from syscolumns where id = object_id('dbo.SharePointSiteRelativeUrl'))
BEGIN
	CREATE TABLE [dbo].[SharePointSiteRelativeUrl](
		[ID] [int] IDENTITY(1,1) NOT NULL,
		[ServerRelativeUrl] [nvarchar](250) NOT NULL,
	 CONSTRAINT [PK_SharePointSiteRelativeUrl] PRIMARY KEY CLUSTERED 
	(
		[ID] ASC
	)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
	) ON [PRIMARY]
END
GO

IF NOT EXISTS (select * from syscolumns where id = object_id('dbo.SharePointUsers'))
BEGIN
	CREATE TABLE [dbo].[SharePointUsers](
		[ID] [int] IDENTITY(1,1) NOT NULL,
		[UserName] [nvarchar](250) NOT NULL,
	 CONSTRAINT [PK_SharePointUsers] PRIMARY KEY CLUSTERED 
	(
		[ID] ASC
	)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
	) ON [PRIMARY]
END
GO

IF NOT EXISTS (select * from syscolumns where id = object_id('dbo.SharePointWebTrafficDailyStats'))
BEGIN

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

	ALTER TABLE [dbo].[SharePointWebTrafficDailyStats]  WITH CHECK ADD  CONSTRAINT [FK_UserID_SharePointSiteRelativeUrl_ID] FOREIGN KEY([UrlId])
	REFERENCES [dbo].[SharePointSiteRelativeUrl] ([ID])

	ALTER TABLE [dbo].[SharePointWebTrafficDailyStats] CHECK CONSTRAINT [FK_UserID_SharePointSiteRelativeUrl_ID]

	ALTER TABLE [dbo].[SharePointWebTrafficDailyStats]  WITH CHECK ADD  CONSTRAINT [FK_UserID_SharePointUsers_ID] FOREIGN KEY([UserId])
	REFERENCES [dbo].[SharePointUsers] ([ID])

	ALTER TABLE [dbo].[SharePointWebTrafficDailyStats] CHECK CONSTRAINT [FK_UserID_SharePointUsers_ID]
END

GO

/*10/26/15 WS Added for VSPLUS-1249*/

USE [VitalSigns]
GO

IF NOT EXISTS (select * from syscolumns where id = object_id('dbo.SharePointTimerJobs'))
BEGIN
	CREATE TABLE [dbo].[SharePointTimerJobs](
		[ID] [int] IDENTITY(1,1) NOT NULL,
		[JobName] [varchar](250) NOT NULL,
		[ServerName] [nvarchar](250) NULL,
		[WebApplicationName] [nvarchar](250) NULL,
		[Status] [nvarchar](250) NULL,
		[StartTime] [datetime] NULL,
		[EndTime] [datetime] NULL,
		[DataBaseName] [varchar](250) NULL,
		[ErrorMessage] [varchar](250) NULL
	) ON [PRIMARY]
END
GO
--VSPLUS 2305 SWATHI
IF EXISTS(SELECT * FROM syscolumns where id=Object_id('ServerTypes'))
BEGIN	
	UPDATE ServerTypes SET FeatureId =1 WHERE ServerType = 'Domino Cluster database'
	UPDATE ServerTypes SET FeatureId =14 WHERE ServerType = 'Windows'
	UPDATE ServerTypes SET FeatureId =4 WHERE ServerType = 'Notesmailprobe'
	UPDATE ServerTypes SET FeatureId =16 WHERE ServerType = 'Active Directory'
	END
   GO
   --VSPLUS VSPLUS-2303 DURGA
USE [vitalsigns]
GO
DELETE FROM [dbo].[EventsMaster] where id in (159,160,161)
SET IDENTITY_INSERT [dbo].[EventsMaster] ON
INSERT [dbo].[EventsMaster] ([ID], [EventName], [ServerTypeID]) VALUES (159, N'Process(JVM) CPU Utilization', 22)
INSERT [dbo].[EventsMaster] ([ID], [EventName], [ServerTypeID]) VALUES (160, N'Heap', 22)
INSERT [dbo].[EventsMaster] ([ID], [EventName], [ServerTypeID]) VALUES (161, N'Thread Pool Count', 22)
SET IDENTITY_INSERT [dbo].[EventsMaster] OFF

-- Windows Event Log Scanning 
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

IF EXISTS ( SELECT * FROM ServerTaskSettings WHERE TaskId IS NULL )
BEGIN
	Delete from dbo.ServerTaskSettings where TaskId IS NULL
END
GO

IF EXISTS (select * from syscolumns where name= 'TaskId' and id = object_id('dbo.ServerTaskSettings'))
BEGIN
Alter table dbo.ServerTaskSettings Alter column TaskId int Not Null
END
GO

/* 11/4/2015 NS added for VSPLUS-2023 */
/* 11/10/2015 NS modified for VSPLUS-2023*/
USE [vitalsigns]
GO

IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[ConfigUsersList]'))
DROP VIEW [dbo].[ConfigUsersList]
GO

USE [vitalsigns]
GO

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

/* 11/12/2015 NS added for VSPLUS-2331 */
USE [VSS_Statistics]
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name='DominoSummaryStats_DateIndex' AND object_id = OBJECT_ID('DominoSummaryStats'))
BEGIN
	CREATE NONCLUSTERED INDEX [DominoSummaryStats_DateIndex] ON [dbo].[DominoSummaryStats] 
(
	[Date] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
END
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name='SametimeSummaryStats_DateIndex' AND object_id = OBJECT_ID('SametimeSummaryStats'))
BEGIN
	CREATE NONCLUSTERED INDEX [SametimeSummaryStats_DateIndex] ON [dbo].[SametimeSummaryStats] 
(
	[Date] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
END
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name='DeviceUpTimeSummaryStats_DateIndex' AND object_id = OBJECT_ID('DeviceUpTimeSummaryStats'))
BEGIN
	CREATE NONCLUSTERED INDEX [DeviceUpTimeSummaryStats_DateIndex] ON [dbo].[DeviceUpTimeSummaryStats] 
(
	[Date] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
END
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name='DeviceDailySummaryStats_DateIndex' AND object_id = OBJECT_ID('DeviceDailySummaryStats '))
BEGIN
	CREATE NONCLUSTERED INDEX [DeviceDailySummaryStats_DateIndex] ON [dbo].[DeviceDailySummaryStats ] 
(
	[Date] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
END
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name='MicrosoftSummaryStats_DateIndex' AND object_id = OBJECT_ID('MicrosoftSummaryStats'))
BEGIN
	CREATE NONCLUSTERED INDEX [MicrosoftSummaryStats_DateIndex] ON [dbo].[MicrosoftSummaryStats] 
(
	[Date] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
END
GO
--Durga VSPLUS 2281
USE [vitalsigns]
GO
DELETE from [vitalsigns].[dbo].[Settings] where sname = 'CleanupMonth' 
INSERT [vitalsigns].[dbo].[Settings] ([sname],[svalue],[stype]) VALUES (N'CleanupMonth',6,'System.Int32')
GO
--Somaraju VSPLUS 2284
USE [vitalsigns]
GO
DELETE from [vitalsigns].[dbo].[Settings] where sname = 'DiskYellowThreshold' 
INSERT [vitalsigns].[dbo].[Settings] ([sname],[svalue],[stype]) VALUES (N'DiskYellowThreshold',20,'System.Int32')
GO
--SOMARAJU VSPLUS 2284
USE [vitalsigns]
GO
IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[viewdiskspace]'))
DROP VIEW [dbo].[viewdiskspace]
GO
--sowmya
/****** Object:  View [dbo].[viewdiskspace]    Script Date: 11/26/2015 13:51:36 ******/
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

/****** Object:  View [dbo].[diskhealthstatusnew]    Script Date: 11/18/2015 08:33:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[diskhealthstatusnew]'))
EXEC dbo.sp_executesql @statement = N'Create view [dbo].[diskhealthstatusnew]
as
Select servername ,diskname,DiskSize,diskfree,ThresholdType,
CASE WHEN ThresholdType = ''Percent''  THEN Threshold else ''0.0'' END AS RedthresholdPercent,
CASE WHEN ThresholdType = ''Percent''  THEN DiskSize*(Threshold/100) when ThresholdType = ''GB'' then Threshold else ''0.0'' END AS RedThresholdValue
,
--CASE WHEN ThresholdType = ''GB''  THEN Threshold else ''0.0'' END AS RedthresholdGB,
--CASE WHEN ThresholdType = ''Percent'' THEN PercentFree else ''0.0'' END AS ValPercentFree,
( select svalue from settings where sname=''DiskYellowThreshold'')
AS yellowthreshold,
Case WHEN ThresholdType = ''Percent'' then (DiskSize*(Threshold/100)*(select svalue from settings where sname=''DiskYellowThreshold'')/100) when ThresholdType=''GB'' then ((DiskSize*(Threshold)*(select svalue from settings where sname=''DiskYellowThreshold'')/100))else ''0.0'' end as yellowthresholdvaluebytype from 
[viewdiskspace]

'
GO
--Somaraju VSPLUS 2284
/****** Object:  View [dbo].[diskheastatusnew]    Script Date: 11/18/2015 08:33:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[diskheastatusnew]'))
EXEC dbo.sp_executesql @statement = N'CREATE view [dbo].[diskheastatusnew]
as
Select servername ,diskname,DiskSize,diskfree,ThresholdType,ddsdiskname,
CASE WHEN ThresholdType = ''Percent''  THEN Threshold else ''0.0'' END AS RedthresholdPercent,
CASE WHEN ThresholdType = ''Percent''  THEN DiskSize*(Threshold/100) when ThresholdType = ''GB'' then Threshold else ''0.0'' END AS RedThresholdValue
,
--CASE WHEN ThresholdType = ''GB''  THEN Threshold else ''0.0'' END AS RedthresholdGB,
--CASE WHEN ThresholdType = ''Percent'' THEN PercentFree else ''0.0'' END AS ValPercentFree,
( select svalue from settings where sname=''DiskYellowThreshold'')
AS yellowthreshold,
Case WHEN ThresholdType = ''Percent'' then (DiskSize*(Threshold/100)*(select svalue from settings where sname=''DiskYellowThreshold'')/100) when ThresholdType=''GB'' then ((DiskSize*(Threshold)*(select svalue from settings where sname=''DiskYellowThreshold'')/100))else ''0.0'' end as yellowthresholdvaluebytype from 
[viewdiskspace]


'
GO

IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[diskheastatusnew]'))
DROP VIEW [dbo].[diskheastatusnew]
GO
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
--CASE WHEN ThresholdType = ''GB''  THEN Threshold else ''0.0'' END AS RedthresholdGB,
--CASE WHEN ThresholdType = ''Percent'' THEN PercentFree else ''0.0'' END AS ValPercentFree,
( select svalue from settings where sname='DiskYellowThreshold')
AS yellowthreshold,
Case WHEN ThresholdType = 'Percent' then (DiskSize*(Threshold/100)*(select svalue from settings where sname='DiskYellowThreshold')/100) when ThresholdType='GB' then ((DiskSize*(Threshold)*(select svalue from settings where sname='DiskYellowThreshold')/100))else '0.0' end as yellowthresholdvaluebytype from 
[viewdiskspace]
GO

USE [vitalsigns]
GO
IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[diskyellowvalue]'))
DROP VIEW [dbo].[diskyellowvalue]
GO

/****** Object:  View [dbo].[diskyellowvalue]    Script Date: 11/20/2015 08:38:46 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


create view [dbo].[diskyellowvalue]
AS
select *,  (yellowthresholdvaluebytype+RedThresholdValue)YellowThresholdvalue
from diskheastatusnew 



GO

/* 11/23/2015 NS added for VSPLUS-2356 */
IF NOT EXISTS(SELECT * from EventsMaster WHERE EventName = N'CPU' AND ServerTypeId=16)
BEGIN	
	SET IDENTITY_INSERT [dbo].[EventsMaster] ON
	INSERT [dbo].[EventsMaster] ([ID], [EventName], [ServerTypeID]) VALUES (162, N'CPU', 16)
	SET IDENTITY_INSERT [dbo].[EventsMaster] OFF
END
IF NOT EXISTS(SELECT * from EventsMaster WHERE EventName = N'Services' AND ServerTypeId=16)
BEGIN	
	SET IDENTITY_INSERT [dbo].[EventsMaster] ON
	INSERT [dbo].[EventsMaster] ([ID], [EventName], [ServerTypeID]) VALUES (163, N'Services', 16)
	SET IDENTITY_INSERT [dbo].[EventsMaster] OFF
END
IF NOT EXISTS(SELECT * from EventsMaster WHERE EventName = N'Memory' AND ServerTypeId=16)
BEGIN	
	SET IDENTITY_INSERT [dbo].[EventsMaster] ON
	INSERT [dbo].[EventsMaster] ([ID], [EventName], [ServerTypeID]) VALUES (164, N'Memory', 16)
	SET IDENTITY_INSERT [dbo].[EventsMaster] OFF
END
IF NOT EXISTS(SELECT * from EventsMaster WHERE EventName = N'Reboot Overdue' AND ServerTypeId=16)
BEGIN	
	SET IDENTITY_INSERT [dbo].[EventsMaster] ON
	INSERT [dbo].[EventsMaster] ([ID], [EventName], [ServerTypeID]) VALUES (165, N'Reboot Overdue', 16)
	SET IDENTITY_INSERT [dbo].[EventsMaster] OFF
END
IF NOT EXISTS(SELECT * from EventsMaster WHERE EventName = N'Disk Space' AND ServerTypeId=16)
BEGIN	
	SET IDENTITY_INSERT [dbo].[EventsMaster] ON
	INSERT [dbo].[EventsMaster] ([ID], [EventName], [ServerTypeID]) VALUES (166, N'Disk Space', 16)
	SET IDENTITY_INSERT [dbo].[EventsMaster] OFF
END
IF NOT EXISTS(SELECT * from EventsMaster WHERE EventName = N'Response Time' AND ServerTypeId=16)
BEGIN	
	SET IDENTITY_INSERT [dbo].[EventsMaster] ON
	INSERT [dbo].[EventsMaster] ([ID], [EventName], [ServerTypeID]) VALUES (167, N'Response Time', 16)
	SET IDENTITY_INSERT [dbo].[EventsMaster] OFF
END


GO

--Durga VSPLUS 2418
USE [vitalsigns]
GO
IF EXISTS(SELECT * FROM syscolumns where id=Object_id('ServerTypes'))
BEGIN	
UPDATE ServerTypes SET FeatureId = 18 WHERE servertype='WebSphere'
END
GO

/* 12/8/2015 NS added for VSPLUS-2227 */
USE [vitalsigns]
GO
IF NOT EXISTS (select * from syscolumns where name= 'DeviceID' and id = object_id('dbo.ServerMaintenance'))
BEGIN	
	ALTER TABLE dbo.ServerMaintenance ADD DeviceID [nvarchar] (250) null
END
GO
--VSPLUS 2300 Sowjanya
USE [vitalsigns]
GO

IF NOT EXISTS (select * from syscolumns where id = object_id('dbo.[DominoEventLogServers]'))
BEGIN	

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
END
GO
USE [vitalsigns]
GO
IF NOT EXISTS (select * from syscolumns where id = object_id('dbo.[DominoEventLog]'))
BEGIN	

CREATE TABLE [dbo].[DominoEventLog](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](250) NULL,
 CONSTRAINT [PK_EventsNames] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
USE [vitalsigns]
GO
IF NOT EXISTS (select * from syscolumns where name= 'DominoEventLogId' and id = object_id('dbo.[LogFile]'))
BEGIN	
	ALTER TABLE dbo.[LogFile] ADD DominoEventLogId [int] Not Null DEFAULT 0
END
GO

USE [vitalsigns]
GO
	
if exists(select * from MenuItems where DisplayText ='Microsoft Office 365')
BEGIN
update MenuItems set PageLink ='~/Configurator/O365ServerGrid.aspx' where DisplayText ='Microsoft Office 365'
END
GO

IF NOT EXISTS (select * from Settings WHERE sname='Office365URL') 
BEGIN
	INSERT [dbo].[Settings] ([sname], [svalue], [stype]) VALUES (N'Office365URL', N'https://outlook.office365.com', N'System.String')
END
go


--2/11/2016 Durga Modified for VSPLUS 2595
USE [vitalsigns]
GO
IF NOT EXISTS(select * from syscolumns where name='TravelerIndex' and id=object_id('dbo.Users'))
BEGIN
ALTER TABLE Users ADD TravelerIndex int NULL
END
GO
IF NOT EXISTS(select * from syscolumns where name='KeyUserDevicesIndex' and id=object_id('dbo.Users'))
BEGIN
ALTER TABLE Users ADD KeyUserDevicesIndex int NULL
END
GO
IF NOT EXISTS(select * from syscolumns where name='StatusIndex' and id=object_id('dbo.Users'))
BEGIN
ALTER TABLE Users ADD StatusIndex int NULL
END
GO

IF NOT EXISTS(select * from syscolumns where name='TravelerZone' and id=object_id('dbo.Users'))
BEGIN
ALTER TABLE Users ADD TravelerZone  nvarchar(MAX) NULL
END
GO
IF NOT EXISTS(select * from syscolumns where name='KeyUserDevicesZone' and id=object_id('dbo.Users'))
BEGIN
ALTER TABLE Users ADD KeyUserDevicesZone nvarchar(MAX) NULL
END
GO
IF NOT EXISTS(select * from syscolumns where name='StatusZone' and id=object_id('dbo.Users'))
BEGIN
ALTER TABLE Users ADD StatusZone  nvarchar(MAX) NULL
END
GO

USE [vitalsigns]
GO
IF Exists(select * from Users where TravelerIndex is null)
BEGIN
Update Users SET [TravelerIndex]=0 where  [TravelerIndex] IS NULL 
END 
GO
USE [vitalsigns]
GO
IF Exists(select * from Users where KeyUserDevicesIndex is null)
BEGIN
Update Users SET [KeyUserDevicesIndex]=2 where  [KeyUserDevicesIndex] IS NULL
END 
GO
USE [vitalsigns]
GO
IF Exists(select * from Users where StatusIndex is null)
BEGIN
Update Users SET [StatusIndex]=1 where  [StatusIndex] IS NULL 
END 
GO
USE [vitalsigns]
GO
IF Exists(select * from Users where TravelerZone is null)
BEGIN
Update Users SET [TravelerZone]='FarRightZone' where  [TravelerZone] IS NULL
END 
GO
USE [vitalsigns]
GO
IF Exists(select * from Users where KeyUserDevicesZone is null)
BEGIN
Update Users SET [KeyUserDevicesZone]='FarRightZone' where  [KeyUserDevicesZone] IS NULL
END 
GO
USE [vitalsigns]
GO
IF Exists(select * from Users where StatusZone is null)
BEGIN
Update Users SET [StatusZone]='RightZone' where  [StatusZone] IS NULL
END 
GO
/*2/16/2016 Sowmya Modified for VSPLUS 2455*/
USE [vitalsigns]
GO
IF NOT EXISTS (select * from syscolumns where name= 'ClusterScan' and id = object_id('dbo.DominoCluster'))
BEGIN	
	ALTER TABLE dbo.DominoCluster ADD ClusterScan [nvarchar] (50) null
END
GO

IF NOT EXISTS(SELECT * from EventsMaster WHERE EventName = N'Authentication' AND ServerTypeId=21)
BEGIN	
	SET IDENTITY_INSERT [dbo].[EventsMaster] ON
	INSERT [dbo].[EventsMaster] ([ID], [EventName], [ServerTypeID]) VALUES (168, N'Authentication', 21)
	SET IDENTITY_INSERT [dbo].[EventsMaster] OFF
END

IF NOT EXISTS(SELECT * from EventsMaster WHERE EventName = N'Not Responding' AND ServerTypeId=21)
BEGIN	
	SET IDENTITY_INSERT [dbo].[EventsMaster] ON
	INSERT [dbo].[EventsMaster] ([ID], [EventName], [ServerTypeID]) VALUES (169, N'Not Responding', 21)
	SET IDENTITY_INSERT [dbo].[EventsMaster] OFF
END	


/* WS Added for IBM Connections */

IF EXISTS(select * from TestsMaster where ServerType is null and ID <= 21)
BEGIN
	UPDATE TestsMaster SET ServerType = 21 WHERE ID <= 21
END
GO

IF NOT EXISTS(select * from TestsMaster where ID = 22)
BEGIN
	SET IDENTITY_INSERT [dbo].[TestsMaster] ON
	INSERT [dbo].[TestsMaster] ([Id], [ServerType], [Tests], [Type]) VALUES (22, 27, N'Create Activity', N'User Scenario Tests')
	SET IDENTITY_INSERT [dbo].[TestsMaster] OFF
END
GO

IF NOT EXISTS(select * from TestsMaster where ID = 23)
BEGIN
	SET IDENTITY_INSERT [dbo].[TestsMaster] ON
	INSERT [dbo].[TestsMaster] ([Id], [ServerType], [Tests], [Type]) VALUES (23, 27, N'Create Blog', N'User Scenario Tests')
	SET IDENTITY_INSERT [dbo].[TestsMaster] OFF
END
GO

IF NOT EXISTS(select * from TestsMaster where ID = 24)
BEGIN
	SET IDENTITY_INSERT [dbo].[TestsMaster] ON
	INSERT [dbo].[TestsMaster] ([Id], [ServerType], [Tests], [Type]) VALUES (24, 27, N'Create Bookmark', N'User Scenario Tests')
	SET IDENTITY_INSERT [dbo].[TestsMaster] OFF
END
GO


IF NOT EXISTS(select * from TestsMaster where ID = 25)
BEGIN
	SET IDENTITY_INSERT [dbo].[TestsMaster] ON
	INSERT [dbo].[TestsMaster] ([Id], [ServerType], [Tests], [Type]) VALUES (25, 27, N'Create Community', N'User Scenario Tests')
	SET IDENTITY_INSERT [dbo].[TestsMaster] OFF
END
GO


IF NOT EXISTS(select * from TestsMaster where ID = 26)
BEGIN
	SET IDENTITY_INSERT [dbo].[TestsMaster] ON
	INSERT [dbo].[TestsMaster] ([Id], [ServerType], [Tests], [Type]) VALUES (26, 27, N'Create File', N'User Scenario Tests')
	SET IDENTITY_INSERT [dbo].[TestsMaster] OFF
END
GO


IF NOT EXISTS(select * from TestsMaster where ID = 27)
BEGIN
	SET IDENTITY_INSERT [dbo].[TestsMaster] ON
	INSERT [dbo].[TestsMaster] ([Id], [ServerType], [Tests], [Type]) VALUES (27, 27, N'Create Wiki', N'User Scenario Tests')
	SET IDENTITY_INSERT [dbo].[TestsMaster] OFF
END
GO


IF NOT EXISTS(select * from TestsMaster where ID = 28)
BEGIN
	SET IDENTITY_INSERT [dbo].[TestsMaster] ON
	INSERT [dbo].[TestsMaster] ([Id], [ServerType], [Tests], [Type]) VALUES (28, 27, N'Search Profile', N'User Scenario Tests')
	SET IDENTITY_INSERT [dbo].[TestsMaster] OFF
END
GO

IF NOT EXISTS (select * from syscolumns where id = object_id('dbo.[IBMConnectionsServers]'))
BEGIN

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

	ALTER TABLE [dbo].[IBMConnectionsServers]  WITH CHECK ADD  CONSTRAINT [FK_IBMConnectionsServers_Servers] FOREIGN KEY([ServerID])
	REFERENCES [dbo].[Servers] ([ID])
	ON DELETE CASCADE
	
	ALTER TABLE [dbo].[IBMConnectionsServers] CHECK CONSTRAINT [FK_IBMConnectionsServers_Servers]

	ALTER TABLE [dbo].[IBMConnectionsServers]  WITH CHECK ADD  CONSTRAINT [FK_IBMConnectionsServers_WebsphereCell] FOREIGN KEY([WSCellID])
	REFERENCES [dbo].[WebsphereCell] ([CellID])

	ALTER TABLE [dbo].[IBMConnectionsServers] CHECK CONSTRAINT [FK_IBMConnectionsServers_WebsphereCell]

END

GO

IF NOT EXISTS (select * from syscolumns where id = object_id('dbo.[IBMConnectionsTests]'))
BEGIN

	CREATE TABLE [dbo].[IBMConnectionsTests](
		[Id] [int] NOT NULL,
		[ServerId] [int] NULL,
		[EnableSimulationTests] [bit] NULL,
		[ResponseThreshold] [int] NULL
	) ON [PRIMARY]

	ALTER TABLE [dbo].[IBMConnectionsTests]  WITH CHECK ADD  CONSTRAINT [FK__IBMConnectionsTests__TestsMaster] FOREIGN KEY([Id])
	REFERENCES [dbo].[TestsMaster] ([Id])

	ALTER TABLE [dbo].[IBMConnectionsTests] CHECK CONSTRAINT [FK__IBMConnectionsTests__TestsMaster]

END

GO

/* 2/22/2016 NS added for VSPLUS-2641 */
USE vitalsigns
GO
IF NOT EXISTS (select * from syscolumns where name= 'AvailabilityIndexThreshold' and id = object_id('dbo.DominoServers'))
BEGIN	
	ALTER TABLE DominoServers ADD AvailabilityIndexThreshold int NULL
END
GO
--Durga Modified on 3/14/2016 
IF NOT EXISTS(SELECT * from EventsMaster WHERE EventName = N'Availability Index' AND ServerTypeId=1)
BEGIN	
	SET IDENTITY_INSERT [dbo].[EventsMaster] ON
	INSERT [dbo].[EventsMaster] ([ID], [EventName], [ServerTypeID]) VALUES (170, N'Availability Index', 1)
	SET IDENTITY_INSERT [dbo].[EventsMaster] OFF
END
GO
/*2/29/2016 Durga Added for VSPLUS-2611*/
USE [vitalsigns]
GO
IF EXISTS(SELECT * FROM syscolumns where id=Object_id('ServerTypes'))
BEGIN	
	UPDATE ServerTypes SET FeatureId =19 WHERE ServerType = 'Office 365'
END
GO

USE [vitalsigns]
GO
IF NOT EXISTS (select * from syscolumns where id = object_id('dbo.[O365Groups]'))
BEGIN

CREATE TABLE [dbo].[O365Groups](
	[ServerId] [int] NOT NULL,
	[GroupId] [varchar](254) NOT NULL,
	[GroupName] [varchar](254) NOT NULL,
	[GroupType] [varchar](254) NOT NULL,
	[GroupDescription] [varchar](1000) NULL
) ON [PRIMARY]
END
GO

IF NOT EXISTS (select * from syscolumns where id = object_id('dbo.[O365MSOLUsers]'))
BEGIN
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
END
GO

IF NOT EXISTS (select * from syscolumns where id = object_id('dbo.[O365UserGroups]'))
BEGIN
CREATE TABLE [dbo].[O365UserGroups](
	[GroupId] [varchar](254) NULL,
	[UserPrincipalName] [varchar](1000) NULL
) ON [PRIMARY]
END
GO

USE [vitalsigns]
GO
IF NOT EXISTS (select * from syscolumns where name= 'License' and id = object_id('dbo.O365MSOLUsers'))
BEGIN	
	ALTER TABLE dbo.O365MSOLUsers ADD License varchar(1000) NULL
END
GO

USE [VSS_Statistics]
GO
IF NOT EXISTS (select * from syscolumns where name= 'InActiveDaysCount' and id = object_id('dbo.O365AdditionalMailDetails'))
BEGIN	
	ALTER TABLE dbo.O365AdditionalMailDetails ADD InActiveDaysCount int NULL
END
GO
--3/11/2016 sowjanya Modified for VSPLUS-2650
USE [vitalsigns]
GO
IF NOT EXISTS (select * from syscolumns where name= 'Category' and id = object_id('dbo.[IBMConnectionsServers]'))
BEGIN	
	ALTER TABLE [IBMConnectionsServers] ADD Category nvarchar(255) NULL
END
GO
USE [vitalsigns]
GO
IF NOT EXISTS (select * from syscolumns where name= 'ScanInterval' and id = object_id('dbo.[IBMConnectionsServers]'))
BEGIN	
	ALTER TABLE [IBMConnectionsServers] ADD ScanInterval int NULL
END
GO
USE [vitalsigns]
GO
IF NOT EXISTS (select * from syscolumns where name= 'OffHoursScanInterval' and id = object_id('dbo.[IBMConnectionsServers]'))
BEGIN	
	ALTER TABLE [IBMConnectionsServers] ADD OffHoursScanInterval int NULL
END
GO
USE [vitalsigns]
GO
IF NOT EXISTS (select * from syscolumns where name= 'RetryInterval' and id = object_id('dbo.[IBMConnectionsServers]'))
BEGIN	
	ALTER TABLE [IBMConnectionsServers] ADD RetryInterval int NULL
END
GO
USE [vitalsigns]
GO
IF NOT EXISTS (select * from syscolumns where name= 'ResponseThreshold' and id = object_id('dbo.[IBMConnectionsServers]'))
BEGIN	
	ALTER TABLE [IBMConnectionsServers] ADD ResponseThreshold int NULL
END
GO
USE [vitalsigns]
GO
IF NOT EXISTS (select * from syscolumns where name= 'Enabled' and id = object_id('dbo.[IBMConnectionsServers]'))
BEGIN	
	ALTER TABLE [IBMConnectionsServers] ADD Enabled bit NULL
END
GO
USE [vitalsigns]
GO
IF NOT EXISTS (select * from syscolumns where name= 'FailureThreshold' and id = object_id('dbo.[IBMConnectionsServers]'))
BEGIN	
	ALTER TABLE [IBMConnectionsServers] ADD FailureThreshold int NULL
END
GO
USE [vitalsigns]
GO
IF NOT EXISTS (select * from syscolumns where name= 'CredentialID' and id = object_id('dbo.[IBMConnectionsServers]'))
BEGIN	
	ALTER TABLE [IBMConnectionsServers] ADD CredentialID int NULL
END
GO
USE [vitalsigns]
GO
IF NOT EXISTS (select * from syscolumns where name= 'ChatUser1CredentialsId' and id = object_id('dbo.[IBMConnectionsServers]'))
BEGIN	
	ALTER TABLE [IBMConnectionsServers] ADD ChatUser1CredentialsId int NULL
END
GO
USE [vitalsigns]
GO
IF NOT EXISTS (select * from syscolumns where name= 'ChatUser2CredentialsId' and id = object_id('dbo.[IBMConnectionsServers]'))
BEGIN	
	ALTER TABLE [IBMConnectionsServers] ADD ChatUser2CredentialsId int NULL
END
GO
USE [vitalsigns]
GO
IF NOT EXISTS (select * from syscolumns where name= 'TestChatSimulation' and id = object_id('dbo.[IBMConnectionsServers]'))
BEGIN	
	ALTER TABLE [IBMConnectionsServers] ADD TestChatSimulation bit NULL
END
GO
USE [vitalsigns]
GO
IF NOT EXISTS (select * from syscolumns where name= 'SSL' and id = object_id('dbo.[IBMConnectionsServers]'))
BEGIN	
	ALTER TABLE [IBMConnectionsServers] ADD SSL bit NULL
END
GO
USE [vitalsigns]
GO
IF NOT EXISTS (select * from syscolumns where name= 'EnableDB2port' and id = object_id('dbo.[IBMConnectionsServers]'))
BEGIN	
	ALTER TABLE [IBMConnectionsServers] ADD EnableDB2port bit NULL
END

GO
USE [vitalsigns]
GO
IF NOT EXISTS (select * from syscolumns where name= 'URL' and id = object_id('dbo.[IBMConnectionsServers]'))
BEGIN	
	ALTER TABLE [IBMConnectionsServers] ADD URL nvarchar(max) NULL
END
GO
USE [vitalsigns]
GO
IF NOT EXISTS (select * from syscolumns where name= 'PortNumber' and id = object_id('dbo.[IBMConnectionsServers]'))
BEGIN	
	ALTER TABLE [IBMConnectionsServers] ADD PortNumber int NULL
END
GO
USE [vitalsigns]
GO
IF NOT EXISTS (select * from syscolumns where name= 'IBMConnectionSID' and id = object_id('dbo.[WebsphereCell]'))
BEGIN	
	ALTER TABLE WebsphereCell ADD IBMConnectionSID int NULL
END
GO
--3/14/2016 Somaraju Addded for VSPLUS-2694,2697
USE [vitalsigns]
GO
IF NOT EXISTS (select * from syscolumns where name= 'MonthlyOperatingCost' and id = object_id('dbo.[Servers]'))
BEGIN	
	ALTER TABLE Servers ADD MonthlyOperatingCost  decimal(18, 0) NULL
END
GO
USE [vitalsigns]
GO
IF NOT EXISTS (select * from syscolumns where name= 'IdealUserCount' and id = object_id('dbo.[Servers]'))
BEGIN	
	ALTER TABLE Servers ADD IdealUserCount int NULL
END
GO



/*WS For IBM COnnections*/
USE [vitalsigns]
GO
IF NOT EXISTS (select * from syscolumns where id = object_id('dbo.[IBMConnectionsTests]'))
BEGIN
	CREATE TABLE [dbo].[IBMConnectionsTests](
		[Id] [int] NOT NULL,
		[ServerId] [int] NULL,
		[EnableSimulationTests] [bit] NULL,
		[ResponseThreshold] [int] NULL
	) ON [PRIMARY]

	ALTER TABLE [dbo].[IBMConnectionsTests]  WITH CHECK ADD  CONSTRAINT [FK__IBMConnectionsTests__TestsMaster] FOREIGN KEY([Id])
		REFERENCES [dbo].[TestsMaster] ([Id])
	ALTER TABLE [dbo].[IBMConnectionsTests] CHECK CONSTRAINT [FK__IBMConnectionsTests__TestsMaster]

END
GO

IF NOT EXISTS (select * from syscolumns where id = object_id('dbo.[IBMConnectionsServers]'))
BEGIN
	CREATE TABLE [dbo].[IBMConnectionsServers](
		[ServerID] [int] NOT NULL,
		[ProxyServerType] [varchar](250) NULL,
		[ProxyServerProtocall] [varchar](250) NULL,
		[DBHostName] [varchar](250) NULL,
		[DBName] [varchar](250) NULL,
		[DBPort] [int] NULL,
		[DBCredentialsID] [int] NULL,
		[WSCellID] [int] NULL
	) ON [PRIMARY]

	ALTER TABLE [dbo].[IBMConnectionsServers]  WITH CHECK ADD  CONSTRAINT [FK_IBMConnectionsServers_Servers] FOREIGN KEY([ServerID])
		REFERENCES [dbo].[Servers] ([ID])
		ON DELETE CASCADE
	ALTER TABLE [dbo].[IBMConnectionsServers] CHECK CONSTRAINT [FK_IBMConnectionsServers_Servers]
	ALTER TABLE [dbo].[IBMConnectionsServers]  WITH CHECK ADD  CONSTRAINT [FK_IBMConnectionsServers_WebsphereCell] FOREIGN KEY([WSCellID])
		REFERENCES [dbo].[WebsphereCell] ([CellID])
	ALTER TABLE [dbo].[IBMConnectionsServers] CHECK CONSTRAINT [FK_IBMConnectionsServers_WebsphereCell]
END

GO

USE [VSS_Statistics]
GO

IF NOT EXISTS (select * from syscolumns where id = object_id('dbo.[IbmConnectionsDailyStats]'))
BEGIN
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
END

GO

IF NOT EXISTS (select * from syscolumns where id = object_id('dbo.[IbmConnectionsSummaryStats]'))
BEGIN
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
END

GO

IF NOT EXISTS (select * from syscolumns where id = object_id('dbo.[IbmConnectionsTopStats]'))
BEGIN
	CREATE TABLE [dbo].[IbmConnectionsTopStats](
		[ServerId] [int] NULL,
		[ServerName] [nvarchar](50) NULL,
		[Ranking] [int] NULL,
		[Name] [nvarchar](200) NULL,
		[UsageCount] [float] NULL,
		[Type] [nvarchar](200) NULL,
		[DateTime] [datetime] NULL
	) ON [PRIMARY]
END

GO

--Somaraju VSPLUS 2714
USE [vitalsigns]
GO
IF NOT EXISTS (select * from syscolumns where name= 'StrongPasswordRequired' and id = object_id('dbo.[O365MSOLUsers]'))
BEGIN
ALTER TABLE [dbo].[O365MSOLUsers] ADD StrongPasswordRequired bit NULL
End
GO
IF NOT EXISTS (select * from syscolumns where name= 'PasswordNeverExpires' and id = object_id('dbo.[O365MSOLUsers]'))
BEGIN
ALTER TABLE [dbo].[O365MSOLUsers] ADD PasswordNeverExpires bit NULL
End
GO

/* 3/28 WS Added for VSPLUS-2764/2765 */

IF NOT EXISTS(SELECT * from EventsMaster WHERE EventName = N'MailBox Assistants Service Test' AND ServerTypeID = 5)
BEGIN	
	SET IDENTITY_INSERT [dbo].[EventsMaster] ON
	INSERT [dbo].[EventsMaster] ([ID], [EventName], [ServerTypeID]) VALUES (179, N'MailBox Assistants Service Test', 5)
	SET IDENTITY_INSERT [dbo].[EventsMaster] OFF
END

GO

IF NOT EXISTS(SELECT * from EventsMaster WHERE EventName = N'Mailbox Replication Service Test' AND ServerTypeID = 5)
BEGIN	
	SET IDENTITY_INSERT [dbo].[EventsMaster] ON
	INSERT [dbo].[EventsMaster] ([ID], [EventName], [ServerTypeID]) VALUES (180, N'Mailbox Replication Service Test', 5)
	SET IDENTITY_INSERT [dbo].[EventsMaster] OFF
END

GO

/* 3/29 WS Added for VSPLUS-1249 */
IF NOT EXISTS (select * from syscolumns where name= 'Farm' and id = object_id('dbo.[SharePointTimerJobs]'))
BEGIN
	ALTER TABLE [dbo].[SharePointTimerJobs] ADD Farm varchar(250) NULL
End
GO
--3/31/2016 Somaraju Modified for VSPLUS-2528 
USE [vitalsigns]
GO
IF NOT EXISTS (select * from syscolumns where id = object_id('dbo.[O365ServiceDetails]'))
BEGIN
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
END

GO


/* 4/1 WS Added for VSPLUS-2760 */
IF NOT EXISTS(SELECT * from EventsMaster WHERE EventName = N'Not Responding' AND ServerTypeID = 19)
BEGIN	
	SET IDENTITY_INSERT [dbo].[EventsMaster] ON
	INSERT [dbo].[EventsMaster] ([ID], [EventName], [ServerTypeID]) VALUES (181, N'Not Responding', 19)
	SET IDENTITY_INSERT [dbo].[EventsMaster] OFF
END

GO

/* 4/5 WS Added for VSPLUS-1347 */
IF NOT EXISTS (select * from syscolumns where name= 'SiteCount' and id = object_id('dbo.[SharePointSiteCollections]'))
BEGIN
	ALTER TABLE [dbo].[SharePointSiteCollections] ADD SiteCount int NULL
End
GO

IF NOT EXISTS (select * from syscolumns where name= 'Owner' and id = object_id('dbo.[SharePointSiteCollections]'))
BEGIN
	ALTER TABLE [dbo].[SharePointSiteCollections] ADD Owner varchar(250) NULL
End
GO

/* 4/6/16 WS Added for VSPLUS-1249 */

IF NOT EXISTS (select * from syscolumns where name= 'Schedule' and id = object_id('dbo.[SharePointTimerJobs]'))
BEGIN
	ALTER TABLE [dbo].[SharePointTimerJobs] ADD Schedule varchar(250) NULL
End
GO

/* 4/14/16  WS added for VSPLUS-2846 */ 

if exists( select column_name, data_type, character_maximum_length    
	from information_schema.columns  
	where table_name = 'StatusDetail' and COLUMN_NAME = 'Details' and CHARACTER_MAXIMUM_LENGTH = 100)
BEGIN
	alter table StatusDetail alter column Details varchar(500) 
END

GO
--20/4/2016 Somaraju Modified for VSPLUS-2613
USE [vitalsigns]
GO
IF NOT EXISTS (select * from syscolumns where id = object_id('dbo.[O365UserswithLicensesandServices]'))
BEGIN
	CREATE TABLE [dbo].[O365UserswithLicensesandServices](
 [DisplayName] [nvarchar](150) NULL,
 [XMLConfiguration] [nvarchar](max) NULL,
 [ServerID] [int] NULL
) ON [PRIMARY]


END

GO
/* 4/1 WS Added for VSPLUS-1327 */
IF NOT EXISTS(SELECT * from EventsMaster WHERE EventName = N'Database Corruption' AND ServerTypeID = 5)
BEGIN	
	SET IDENTITY_INSERT [dbo].[EventsMaster] ON
	INSERT [dbo].[EventsMaster] ([ID], [EventName], [ServerTypeID]) VALUES (182, N'Database Corruption', 5)
	SET IDENTITY_INSERT [dbo].[EventsMaster] OFF
END

GO

USE [VSS_Statistics]
GO

IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[GetHourlydataforEXJournalDocCountTotal]'))
DROP VIEW [dbo].[GetHourlydataforEXJournalDocCountTotal]
GO

USE [VSS_Statistics]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

Create view [dbo].[GetHourlydataforEXJournalDocCountTotal]
AS
    
      select datepart(hour, date) as Hour,  avg(StatValue) as StatValue,ServerName from [DominoDailyStats]  WHERE StatName='EXJournal.DocCount.Total'

       and CAST(date AS DATE) = CAST(Getdate() AS DATE) group by  datepart(hour, date),ServerName


GO
--10/5/2016 SWATHI VSPLUS 2479 
USE [vitalsigns]
GO

IF NOT EXISTS (select * from syscolumns where name= 'IsPrereqsDone' and id = object_id('dbo.ServerAttributes'))
BEGIN 
 ALTER TABLE dbo.ServerAttributes ADD IsPrereqsDone bit  null
END
GO

--5/31/15 WS Added for 2890

USE [vitalsigns]
GO

--IBM Connections Objects

IF NOT EXISTS (select * from syscolumns where id = object_id('dbo.[IbmConnectionsObjects]'))
BEGIN

	CREATE TABLE [dbo].[IbmConnectionsObjects](
		[ID] [int] IDENTITY(1,1) NOT NULL,
		[Name] [varchar](255) NOT NULL,
		[Type] [varchar](255) NOT NULL,
		[DateCreated] [datetime] NOT NULL,
		[DateLastModified] [datetime] NOT NULL,
		[ServerID] [int] NOT NULL,
		[OwnerId] [int] NULL,
		[ParentObjectId] [int] NULL,
	 CONSTRAINT [PK_IbmConnectionsObjects] PRIMARY KEY CLUSTERED 
	(
		[ID] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]

	ALTER TABLE [dbo].[IbmConnectionsObjects]  WITH CHECK ADD  CONSTRAINT [IbmConnectionsObjects_ServerID_Servers_ID] FOREIGN KEY([ServerID])
	REFERENCES [dbo].[Servers] ([ID])
	ON DELETE CASCADE

	ALTER TABLE [dbo].[IbmConnectionsObjects] CHECK CONSTRAINT [IbmConnectionsObjects_ServerID_Servers_ID]
END
GO

--IBM Connections Tags

IF NOT EXISTS (select * from syscolumns where id = object_id('dbo.[IbmConnectionsTags]'))
BEGIN

	CREATE TABLE [dbo].[IbmConnectionsTags](
		[ID] [int] IDENTITY(1,1) NOT NULL,
		[Tag] [varchar](255) NOT NULL,
	 CONSTRAINT [PK_IbmConnectionsTags] PRIMARY KEY CLUSTERED 
	(
		[ID] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
END

GO

--IBM Connections Users
IF NOT EXISTS (select * from syscolumns where id = object_id('dbo.[IbmConnectionsUsers]'))
BEGIN

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

	ALTER TABLE [dbo].[IbmConnectionsUsers]  WITH CHECK ADD  CONSTRAINT [IbmConnectionsUsers_ServerID_Servers_ID] FOREIGN KEY([ServerID])
	REFERENCES [dbo].[Servers] ([ID])
	ON DELETE CASCADE

	ALTER TABLE [dbo].[IbmConnectionsUsers] CHECK CONSTRAINT [IbmConnectionsUsers_ServerID_Servers_ID]

END

--IBM Connections Users Objects
IF NOT EXISTS (select * from syscolumns where id = object_id('dbo.[IbmConnectionsObjectUsers]'))
BEGIN

	CREATE TABLE [dbo].[IbmConnectionsObjectUsers](
		[ObjectId] [int] NOT NULL,
		[UserId] [int] NOT NULL
	) ON [PRIMARY]

	ALTER TABLE [dbo].[IbmConnectionsObjectUsers]  WITH CHECK ADD  CONSTRAINT [IbmConnectionsObjectUsers_ObjectId_IbmConnectionsObjects_ID] FOREIGN KEY([ObjectId])
	REFERENCES [dbo].[IbmConnectionsObjects] ([ID])
	ON DELETE CASCADE

	ALTER TABLE [dbo].[IbmConnectionsObjectUsers] CHECK CONSTRAINT [IbmConnectionsObjectUsers_ObjectId_IbmConnectionsObjects_ID]

	ALTER TABLE [dbo].[IbmConnectionsObjectUsers]  WITH CHECK ADD  CONSTRAINT [IbmConnectionsObjectUsers_UserId_IbmConnectionsUsers_ID] FOREIGN KEY([UserId])
	REFERENCES [dbo].[IbmConnectionsUsers] ([ID])

	ALTER TABLE [dbo].[IbmConnectionsObjectUsers] CHECK CONSTRAINT [IbmConnectionsObjectUsers_UserId_IbmConnectionsUsers_ID]

END

--IBM Connections Object Tags

IF NOT EXISTS (select * from syscolumns where id = object_id('dbo.[IbmConnectionsObjectTags]'))
BEGIN

	CREATE TABLE [dbo].[IbmConnectionsObjectTags](
		[ObjectId] [int] NOT NULL,
		[TagId] [int] NOT NULL
	) ON [PRIMARY]

	ALTER TABLE [dbo].[IbmConnectionsObjectTags]  WITH CHECK ADD  CONSTRAINT [IbmConnectionsObjectTags_ObjectId_IbmConnectionsObjects_ID] FOREIGN KEY([ObjectId])
	REFERENCES [dbo].[IbmConnectionsObjects] ([ID])
	ON DELETE CASCADE

	ALTER TABLE [dbo].[IbmConnectionsObjectTags] CHECK CONSTRAINT [IbmConnectionsObjectTags_ObjectId_IbmConnectionsObjects_ID]

	ALTER TABLE [dbo].[IbmConnectionsObjectTags]  WITH CHECK ADD  CONSTRAINT [IbmConnectionsObjectTags_TagId_IbmConnectionsTags_ID] FOREIGN KEY([TagId])
	REFERENCES [dbo].[IbmConnectionsTags] ([ID])
	ON DELETE CASCADE

	ALTER TABLE [dbo].[IbmConnectionsObjectTags] CHECK CONSTRAINT [IbmConnectionsObjectTags_TagId_IbmConnectionsTags_ID]

END
GO

--IBM Connections Community

IF NOT EXISTS (select * from syscolumns where id = object_id('dbo.[IbmConnectionsCommunity]'))
BEGIN

	CREATE TABLE [dbo].[IbmConnectionsCommunity](
	[ObjectID] [int] NOT NULL,
	[CommunityType] [nvarchar](255) NOT NULL,
	CONSTRAINT [PK_IbmConnectionsCommunity] PRIMARY KEY CLUSTERED 
	(
		[ObjectID] ASC
	)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
	) ON [PRIMARY]

	ALTER TABLE [dbo].[IbmConnectionsCommunity]  WITH CHECK ADD  CONSTRAINT [IbmConnectionsCommunity_ObjectID_IbmConnectionsObjects_ID] FOREIGN KEY([ObjectID])
	REFERENCES [dbo].[IbmConnectionsObjects] ([ID])
	ON DELETE CASCADE

	ALTER TABLE [dbo].[IbmConnectionsCommunity] CHECK CONSTRAINT [IbmConnectionsCommunity_ObjectID_IbmConnectionsObjects_ID]

END
GO
--Sowjanya VSPLUS VSPLUS-3000
USE [vitalsigns]
GO
IF NOT EXISTS (select * from syscolumns where name= 'Costperuser' and id = object_id('dbo.O365Server'))
BEGIN	
	ALTER TABLE dbo.O365Server ADD Costperuser int  null 
END
GO

IF NOT EXISTS (select * from syscolumns where name= 'GUID' and id = object_id('dbo.IbmConnectionsObjects'))
BEGIN 
 ALTER TABLE dbo.IbmConnectionsObjects ADD GUID varchar(255)  null
END
GO
/* 14/06/2016 Swathi Added for VSPLUS-1250 */
IF NOT EXISTS(SELECT * from EventsMaster WHERE EventName = N'Search Crawl' AND ServerTypeID = 4)
BEGIN	
	SET IDENTITY_INSERT [dbo].[EventsMaster] ON
	INSERT [dbo].[EventsMaster] ([ID], [EventName], [ServerTypeID]) VALUES (183, N'Search Crawl',  4 )
	SET IDENTITY_INSERT [dbo].[EventsMaster] OFF
END

GO

--6/20/16 WS Added for VSPLUS-3008
IF NOT EXISTS (select * from syscolumns where name= 'IsActive' and id = object_id('dbo.IbmConnectionsUsers'))
BEGIN 
	ALTER TABLE dbo.IbmConnectionsUsers ADD IsActive BIT null
END
GO


IF NOT EXISTS (select * from syscolumns where name= 'IsInternal' and id = object_id('dbo.IbmConnectionsUsers'))
BEGIN 
	ALTER TABLE dbo.IbmConnectionsUsers ADD IsInternal BIT null
END
GO

--6/21/16 WS Added for VSPLUS-3035
USE [vitalsigns]
GO

IF EXISTS(SELECT * FROM sys.triggers WHERE name = 'tr_UpdateDeviceInventoryIbmConnectionsServers')
DROP TRIGGER [dbo].[tr_UpdateDeviceInventoryIbmConnectionsServers]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TRIGGER [dbo].[tr_UpdateDeviceInventoryIbmConnectionsServers]
   ON  [dbo].[IbmConnectionsServers]
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
				s.ServerName, ServerID, 27, s.LocationID
				FROM inserted i INNER JOIN Servers s ON i.ServerID=s.ID
				WHERE i.[Enabled] = 1
		END
		ELSE
		 IF (SELECT [Enabled] FROM Inserted) = 0 AND (SELECT [Enabled] FROM Deleted) = 1
		 BEGIN
			DELETE d FROM DeviceInventory AS d INNER JOIN Inserted AS i ON d.DeviceID = i.ServerID
			AND  d.DeviceTypeID=27 AND i.[Enabled] = 0
		 END
	END
END

GO


-- WS Added for 3098 --
USE [vitalsigns]
GO

IF NOT EXISTS (select * from syscolumns where id = object_id('dbo.[SharePointServerSettings]'))
BEGIN

	CREATE TABLE [dbo].[SharePointServerSettings](
		[ServerID] [int] NOT NULL,
		[ConflictingContentType] [bit] NULL,
		[CustomizedFiles] [bit] NULL,
		[MissingGalleries] [bit] NULL,
		[MissingParentContentTypes] [bit] NULL,
		[MissingSiteTemplates] [bit] NULL,
		[UnsupportedLanguagePack] [bit] NULL,
		[UnsupportedMUI] [bit] NULL
	) ON [PRIMARY]

END
	
GO
--Sowjanya VSPLUS -3106
USE [vitalsigns]
GO
IF EXISTS (select * from syscolumns where name= 'Costperuser' and id = object_id('dbo.O365Server'))
BEGIN
	ALTER TABLE O365Server 
ALTER COLUMN Costperuser float  null
END
GO

USE [vitalsigns]
GO

IF NOT EXISTS (select * from syscolumns where id = object_id('dbo.CASServerTests'))
BEGIN	
	CREATE TABLE [dbo].[CASServerTests](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[ServerId] [int] NULL,
	[TestId] [int] NULL,
	[URLs] [nvarchar](max) NULL,
	[CredentialsId] [int] NULL
) 
END
GO
USE [vitalsigns]
GO

IF NOT EXISTS (select * from syscolumns where id = object_id('dbo.ExchangeTestNames'))
BEGIN	
	CREATE TABLE [dbo].[ExchangeTestNames](
	[TestId] [int] IDENTITY(1,1) NOT NULL,
	[TestName] [nvarchar](50) NULL
) 

END
GO
USE [vitalsigns]
GO
DELETE FROM [ExchangeTestNames]
--14/07/2016 sowmya added for VSPLUS-3097
/****** Object:  Table [dbo].[ExchangeTestNames]    Script Date: 07/15/2016 12:39:59 ******/
SET IDENTITY_INSERT [dbo].[ExchangeTestNames] ON
INSERT [dbo].[ExchangeTestNames] ([TestId], [TestName]) VALUES (1, N'SMTP')
INSERT [dbo].[ExchangeTestNames] ([TestId], [TestName]) VALUES (2, N'POP3')
INSERT [dbo].[ExchangeTestNames] ([TestId], [TestName]) VALUES (3, N'IMAP')
INSERT [dbo].[ExchangeTestNames] ([TestId], [TestName]) VALUES (4, N'Outlook Anywhere')
INSERT [dbo].[ExchangeTestNames] ([TestId], [TestName]) VALUES (5, N'Auto Discovery')
INSERT [dbo].[ExchangeTestNames] ([TestId], [TestName]) VALUES (6, N'Active Sync')
INSERT [dbo].[ExchangeTestNames] ([TestId], [TestName]) VALUES (7, N'OWA (Outlook Web App)')
INSERT [dbo].[ExchangeTestNames] ([TestId], [TestName]) VALUES (8, N'Outlook Native RPC')
SET IDENTITY_INSERT [dbo].[ExchangeTestNames] OFF

IF EXISTS( select * from syscolumns where id=object_id('dbo.EventsMaster'))
BEGIN
	SET IDENTITY_INSERT [dbo].[EventsMaster] ON
	DELETE from [dbo].[EventsMaster] where EventName = 'Create Mail Folder'
	INSERT INTO [dbo].[EventsMaster] ([ID], [EventName], [ServerTypeID]) VALUES (186, N'Create Mail Folder', 21)
	SET IDENTITY_INSERT [dbo].[EventsMaster] OFF
END
GO


IF EXISTS( select * from syscolumns where id=object_id('dbo.EventsMaster'))
BEGIN
	SET IDENTITY_INSERT [dbo].[EventsMaster] ON
	DELETE from [dbo].[EventsMaster] where EventName = 'URL'
	INSERT INTO [dbo].[EventsMaster] ([ID], [EventName], [ServerTypeID]) VALUES (187, N'URL', 21)
	SET IDENTITY_INSERT [dbo].[EventsMaster] OFF
END
GO

IF EXISTS( select * from syscolumns where id=object_id('dbo.EventsMaster'))
BEGIN
	SET IDENTITY_INSERT [dbo].[EventsMaster] ON
	DELETE from [dbo].[EventsMaster] where EventName = 'DirSync Import'
	INSERT INTO [dbo].[EventsMaster] ([ID], [EventName], [ServerTypeID]) VALUES (188, N'DirSync Import', 21)
	SET IDENTITY_INSERT [dbo].[EventsMaster] OFF
END
GO


IF EXISTS( select * from syscolumns where id=object_id('dbo.EventsMaster'))
BEGIN
	SET IDENTITY_INSERT [dbo].[EventsMaster] ON
	DELETE from [dbo].[EventsMaster] where EventName = 'DirSync Export'
	INSERT INTO [dbo].[EventsMaster] ([ID], [EventName], [ServerTypeID]) VALUES (189, N'DirSync Export', 21)
	SET IDENTITY_INSERT [dbo].[EventsMaster] OFF
END
GO

/* 9/2/2016 NS modified for VSPLUS-3192 */
USE [vitalsigns]
GO
IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
            WHERE TABLE_NAME = 'AlertHistory' 
           AND  COLUMN_NAME = 'DeviceName')
BEGIN
	ALTER TABLE AlertHistory
	ALTER COLUMN [DeviceName] [varchar](150) NULL
END


-- +++++++++++ NO CHANGES BELOW THIS POINT +++++++++++
USE [vitalsigns]
GO

BEGIN TRANSACTION
go
/*swathi 1473*/

DECLARE @VS_MANAGEMENT_VSBUILD varchar(50)
SET @VS_MANAGEMENT_VSBUILD = '3.5.0_DAILY_09'
IF EXISTS(SELECT * FROM dbo.VS_MANAGEMENT WHERE CATEGORY='VS_BUILD')
	UPDATE dbo.VS_MANAGEMENT SET VALUE=@VS_MANAGEMENT_VSBUILD, LAST_UPDATE = getdate( ),UPDATE_BY=CURRENT_USER WHERE CATEGORY = 'VS_BUILD'
ELSE
	INSERT INTO dbo.VS_MANAGEMENT(CATEGORY,VALUE,LAST_UPDATE,DESCRIPTION,UPDATE_BY) VALUES ('VS_BUILD',@VS_MANAGEMENT_VSBUILD,getdate(),'This indicates the current build that was installed.',CURRENT_USER)
go

IF EXISTS(SELECT * FROM dbo.VS_MANAGEMENT WHERE CATEGORY='VS_Update')
	-- this setting is not to be confused with VS_VERSION category, VS_Update category is used to track when and if the update script was run
	UPDATE dbo.VS_MANAGEMENT SET VALUE=dbo.fn_GetVSVersion( ), LAST_UPDATE = getdate( ),UPDATE_BY=CURRENT_USER WHERE CATEGORY = 'VS_Update'
ELSE
	INSERT INTO dbo.VS_MANAGEMENT(CATEGORY,VALUE,LAST_UPDATE,DESCRIPTION,UPDATE_BY) VALUES ('VS_Update',dbo.fn_GetVSVersion( ),getdate(),'This indicates the version and LAST run date of the UPDATE script.',CURRENT_USER)
go

IF EXISTS(SELECT * FROM dbo.VS_MANAGEMENT WHERE CATEGORY='VS_VERSION')
	-- this setting is not to be confused with VS_VERSION category, VS_Update category is used to track when and if the update script was run
	UPDATE dbo.VS_MANAGEMENT SET VALUE=dbo.fn_GetVSVersion( ), LAST_UPDATE = getdate( ) WHERE CATEGORY = 'VS_VERSION'
ELSE
	INSERT INTO dbo.VS_MANAGEMENT(CATEGORY,VALUE,LAST_UPDATE,DESCRIPTION,UPDATE_BY) VALUES ('VS_VERSION',dbo.fn_GetVSVersion( ),getdate(),'This value indicates the fundamental VS database version, which should match the base application version.',CURRENT_USER)
go
COMMIT TRANSACTION
go
