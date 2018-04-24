USE [vitalsigns]
GO

Create Table DailyTasks
(
ID INT NOT NULL IDENTITY(1,1),
SourceTableName Varchar(150),
SourceAggregation Varchar(150),
SourceStatName Varchar(150),
DestinationTableName Varchar(150),
DestinationStatName Varchar(150)
)

USE [vitalsigns]
GO
/****** Object:  Table [dbo].[DailyTasks]    Script Date: 12/05/2013 18:44:56 ******/
SET IDENTITY_INSERT [dbo].[DailyTasks] ON
INSERT [dbo].[DailyTasks] ([ID], [SourceTableName], [SourceAggregation], [SourceStatName], [DestinationTableName], [DestinationStatName]) VALUES (9, N'DeviceDailyStats', N'AVG', N'DeviceType', N'DeviceDailyStats', N'DailyResponseAverage')
INSERT [dbo].[DailyTasks] ([ID], [SourceTableName], [SourceAggregation], [SourceStatName], [DestinationTableName], [DestinationStatName]) VALUES (11, N'DominoDailyStats', N'AVG', N'Mail.AverageServerHops', N'DominoSummaryStats', N'Mail.AverageServerHops')
INSERT [dbo].[DailyTasks] ([ID], [SourceTableName], [SourceAggregation], [SourceStatName], [DestinationTableName], [DestinationStatName]) VALUES (12, N'DominoDailyStats', N'AVG', N'Mail.AverageDeliverTime', N'DominoSummaryStats', N'Mail.AverageDeliverTime')
INSERT [dbo].[DailyTasks] ([ID], [SourceTableName], [SourceAggregation], [SourceStatName], [DestinationTableName], [DestinationStatName]) VALUES (13, N'DominoDailyStats', N'AVG', N'Mail.AverageSizeDelivered', N'DominoSummaryStats', N'Mail.AverageSizeDelivered')
INSERT [dbo].[DailyTasks] ([ID], [SourceTableName], [SourceAggregation], [SourceStatName], [DestinationTableName], [DestinationStatName]) VALUES (14, N'DominoDailyStats', N'SUM', N'Mail.Delivered', N'DominoSummaryStats', N'Mail.Delivered')
INSERT [dbo].[DailyTasks] ([ID], [SourceTableName], [SourceAggregation], [SourceStatName], [DestinationTableName], [DestinationStatName]) VALUES (15, N'DominoDailyStats', N'SUM', N'Mail.TotalRouted', N'DominoSummaryStats', N'Mail.TotalRouted')
INSERT [dbo].[DailyTasks] ([ID], [SourceTableName], [SourceAggregation], [SourceStatName], [DestinationTableName], [DestinationStatName]) VALUES (16, N'DominoDailyStats', N'AVG', N'Mail.Transferred', N'DominoSummaryStats', N'Mail.Transferred')
INSERT [dbo].[DailyTasks] ([ID], [SourceTableName], [SourceAggregation], [SourceStatName], [DestinationTableName], [DestinationStatName]) VALUES (17, N'DominoDailyStats', N'Sum', N'SMTP.MessagesProcessed', N'DominoSummaryStats', N'SMTP.MessagesProcessed')
INSERT [dbo].[DailyTasks] ([ID], [SourceTableName], [SourceAggregation], [SourceStatName], [DestinationTableName], [DestinationStatName]) VALUES (18, N'DominoDailyStats', N'Sum', N'Mail.TransferFailures', N'DominoSummaryStats', N'Mail.TransferFailures')
INSERT [dbo].[DailyTasks] ([ID], [SourceTableName], [SourceAggregation], [SourceStatName], [DestinationTableName], [DestinationStatName]) VALUES (19, N'DominoDailyStats', N'AVG', N'Mail.TotalPending', N'DominoSummaryStats', N'Mail.TotalPending')
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
INSERT [dbo].[DailyTasks] ([ID], [SourceTableName], [SourceAggregation], [SourceStatName], [DestinationTableName], [DestinationStatName]) VALUES (31, N'DominoDailyStats', N'AVG', N'Mem.PercentUsed', N'DominoSummaryStats', N'Mem.PercentUsed')
INSERT [dbo].[DailyTasks] ([ID], [SourceTableName], [SourceAggregation], [SourceStatName], [DestinationTableName], [DestinationStatName]) VALUES (32, N'DominoDailyStats', N'AVG', N'Platform.System.PctCombinedCpuUtil', N'DominoSummaryStats', N'Platform.System.PctCombinedCpuUtil')
INSERT [dbo].[DailyTasks] ([ID], [SourceTableName], [SourceAggregation], [SourceStatName], [DestinationTableName], [DestinationStatName]) VALUES (33, N'DominoDailyStats', N'AVG', N'Platform.Memory.RAM.TotalMBytes', N'DominoSummaryStats', N'Platform.Memory.RAM.TotalMBytes')
INSERT [dbo].[DailyTasks] ([ID], [SourceTableName], [SourceAggregation], [SourceStatName], [DestinationTableName], [DestinationStatName]) VALUES (34, N'DominoDailyStats', N'AVG', N'Platform.Memory.RAM.AvailMBytes', N'DominoSummaryStats', N'Platform.Memory.RAM.AvailMBytes')
INSERT [dbo].[DailyTasks] ([ID], [SourceTableName], [SourceAggregation], [SourceStatName], [DestinationTableName], [DestinationStatName]) VALUES (35, N'DominoDailyStats', N'AVG', N'Platform.Memory.KBFree', N'DominoSummaryStats', N'Platform.Memory.KBFree')
INSERT [dbo].[DailyTasks] ([ID], [SourceTableName], [SourceAggregation], [SourceStatName], [DestinationTableName], [DestinationStatName]) VALUES (36, N'DominoDailyStats', N'AVG', N'Replica.Cluster.WorkQueueDepth', N'DominoSummaryStats', N'Replica.Cluster.WorkQueueDepth')
INSERT [dbo].[DailyTasks] ([ID], [SourceTableName], [SourceAggregation], [SourceStatName], [DestinationTableName], [DestinationStatName]) VALUES (37, N'DominoDailyStats', N'AVG', N'Replica.Cluster.Failed', N'DominoSummaryStats', N'Replica.Cluster.Failed')
INSERT [dbo].[DailyTasks] ([ID], [SourceTableName], [SourceAggregation], [SourceStatName], [DestinationTableName], [DestinationStatName]) VALUES (38, N'DominoDailyStats', N'AVG', N'Replica.Cluster.SecondsOnQueue', N'DominoSummaryStats', N'Replica.Cluster.SecondsOnQueue')
INSERT [dbo].[DailyTasks] ([ID], [SourceTableName], [SourceAggregation], [SourceStatName], [DestinationTableName], [DestinationStatName]) VALUES (39, N'DominoDailyStats', N'AVG', N'Replica.Cluster.RetryWaiting', N'DominoSummaryStats', N'Replica.Cluster.RetryWaiting')
INSERT [dbo].[DailyTasks] ([ID], [SourceTableName], [SourceAggregation], [SourceStatName], [DestinationTableName], [DestinationStatName]) VALUES (40, N'DominoDailyStats', N'Sum', N'Server.Cluster.OpenRedirects.Failover.Successful', N'DominoSummaryStats', N'Server.Cluster.OpenRedirects.Failover.Successful')
INSERT [dbo].[DailyTasks] ([ID], [SourceTableName], [SourceAggregation], [SourceStatName], [DestinationTableName], [DestinationStatName]) VALUES (41, N'DominoDailyStats', N'Sum', N'Server.Cluster.OpenRedirects.Failover.Unsuccessful', N'DominoSummaryStats', N'Server.Cluster.OpenRedirects.Failover.Unsuccessful')
INSERT [dbo].[DailyTasks] ([ID], [SourceTableName], [SourceAggregation], [SourceStatName], [DestinationTableName], [DestinationStatName]) VALUES (42, N'DominoDailyStats', N'Sum', N'Server.Cluster.OpenRedirects.LoadBalance.Successful', N'DominoSummaryStats', N'Server.Cluster.OpenRedirects.LoadBalance.Successful')
INSERT [dbo].[DailyTasks] ([ID], [SourceTableName], [SourceAggregation], [SourceStatName], [DestinationTableName], [DestinationStatName]) VALUES (43, N'DominoDailyStats', N'Sum', N'Server.Cluster.OpenRedirects.LoadBalance.Unsuccessful', N'DominoSummaryStats', N'Server.Cluster.OpenRedirects.LoadBalance.Unsuccessful')
INSERT [dbo].[DailyTasks] ([ID], [SourceTableName], [SourceAggregation], [SourceStatName], [DestinationTableName], [DestinationStatName]) VALUES (44, N'DominoDailyStats', N'Sum', N'Domino.Command.OpenDocument', N'DominoSummaryStats', N'Domino.Command.OpenDocument')
INSERT [dbo].[DailyTasks] ([ID], [SourceTableName], [SourceAggregation], [SourceStatName], [DestinationTableName], [DestinationStatName]) VALUES (45, N'DominoDailyStats', N'Sum', N'Domino.Command.CreateDocument', N'DominoSummaryStats', N'Domino.Command.CreateDocument')
INSERT [dbo].[DailyTasks] ([ID], [SourceTableName], [SourceAggregation], [SourceStatName], [DestinationTableName], [DestinationStatName]) VALUES (46, N'DominoDailyStats', N'Sum', N'Domino.Command.DeleteDocument', N'DominoSummaryStats', N'Domino.Command.DeleteDocument')
INSERT [dbo].[DailyTasks] ([ID], [SourceTableName], [SourceAggregation], [SourceStatName], [DestinationTableName], [DestinationStatName]) VALUES (47, N'DominoDailyStats', N'Sum', N'Domino.Command.OpenDatabase', N'DominoSummaryStats', N'Domino.Command.OpenDatabase')
INSERT [dbo].[DailyTasks] ([ID], [SourceTableName], [SourceAggregation], [SourceStatName], [DestinationTableName], [DestinationStatName]) VALUES (48, N'DominoDailyStats', N'Sum', N'Domino.Command.OpenView', N'DominoSummaryStats', N'Domino.Command.OpenView')
INSERT [dbo].[DailyTasks] ([ID], [SourceTableName], [SourceAggregation], [SourceStatName], [DestinationTableName], [DestinationStatName]) VALUES (49, N'DominoDailyStats', N'Sum', N'Domino.Command.Total', N'DominoSummaryStats', N'Domino.Command.Total')
INSERT [dbo].[DailyTasks] ([ID], [SourceTableName], [SourceAggregation], [SourceStatName], [DestinationTableName], [DestinationStatName]) VALUES (50, N'BlackBerryProbeStats', N'AVG', N'DeliveryTime.Seconds', N'DeviceDailyStats', N'DailyDeliveryTimeAverage')
INSERT [dbo].[DailyTasks] ([ID], [SourceTableName], [SourceAggregation], [SourceStatName], [DestinationTableName], [DestinationStatName]) VALUES (51, N'NotesMailStats', N'AVG', N'DeliveryTime.Seconds', N'DeviceDailyStats', N'DailyDeliveryTimeAverage')
INSERT [dbo].[DailyTasks] ([ID], [SourceTableName], [SourceAggregation], [SourceStatName], [DestinationTableName], [DestinationStatName]) VALUES (52, N'DeviceDailyStats', N'AVG', N'ResponseTime', N'DeviceDailyStats', N'DailyResponseAverage')
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
INSERT [dbo].[DailyTasks] ([ID], [SourceTableName], [SourceAggregation], [SourceStatName], [DestinationTableName], [DestinationStatName]) VALUES (89, N'SametimeDailyStats', N'AVG', N'MaxConcurrentLoggedInUsers', N'SametimeSummaryStats', N'MaxConcurrentLoggedInUsers')
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
INSERT [dbo].[DailyTasks] ([ID], [SourceTableName], [SourceAggregation], [SourceStatName], [DestinationTableName], [DestinationStatName]) VALUES (180, N'DominoDailyStats', N'Max', N'Platform.System.PctCombinedCpuUtil', N'DominoSummaryStats', N'Platform.System.PctCombinedCpuUtil.Max')
INSERT [dbo].[DailyTasks] ([ID], [SourceTableName], [SourceAggregation], [SourceStatName], [DestinationTableName], [DestinationStatName]) VALUES (181, N'DeviceUpTimeStats', N'AVG', N'HourlyUpPercent', N'DeviceUpTimeStats', N'DailyUpTimePercent')
INSERT [dbo].[DailyTasks] ([ID], [SourceTableName], [SourceAggregation], [SourceStatName], [DestinationTableName], [DestinationStatName]) VALUES (182, N'DeviceUpTimeStats', N'Sum', N'HourlyDownTimeMinutes', N'DeviceUpTimeStats', N'DailyDownTimeMinutes')
INSERT [dbo].[DailyTasks] ([ID], [SourceTableName], [SourceAggregation], [SourceStatName], [DestinationTableName], [DestinationStatName]) VALUES (183, N'DeviceUpTimeStats', N'AVG', N'HourlyOnTargetPercent', N'DeviceUpTimeStats', N'DailyOnTargetPercent')
SET IDENTITY_INSERT [dbo].[DailyTasks] OFF



--createing ApplServerSettings menu item
--12/12/2013 NS added a new menu item to set server settings in bulk
USE [vitalsigns]
GO
INSERT [dbo].[MenuItems] ([ID], [DisplayText], [OrderNum], [ParentID], [PageLink], [Level], [RefName], [ImageURL]) VALUES (59, N'Server Settings Editor', 7, 10, N'/Security/ApplyServerSettings.aspx', 2, N'ApplyServerSettings', N'')
--INSERT INTO MenuItems VALUES(59,'Apply Server Settings',7,10,'/Security/ApplyServerSettings.aspx',2,'ApplyServerSettings','');



USE [vitalsigns]
GO
CREATE TABLE [dbo].[ProfilesMaster](
 [ID] [int] IDENTITY(1,1) NOT NULL,
 [ServerTypeId] [int] NULL,
 [AttributeName] [nvarchar](100) NULL,
 [DefaultValue] [nvarchar](100) NULL,
 [UnitOfMeasurement] [nvarchar](100) NULL,
 [RelatedTable] [nvarchar](100) NULL,
 [RelatedField] [nvarchar](100) NULL
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
/****** Object:  Table [dbo].[ProfilesMaster]    Script Date: 12/05/2013 22:00:49 ******/
SET IDENTITY_INSERT [dbo].[ProfilesMaster] ON
INSERT [dbo].[ProfilesMaster] ([ID], [ServerTypeId], [AttributeName], [DefaultValue], [UnitOfMeasurement], [RelatedTable], [RelatedField]) VALUES (7, 1, N'Off Hours Scan Interval', N'10', N'minutes', N'DominoServers', N'OffHoursScanInterval')
INSERT [dbo].[ProfilesMaster] ([ID], [ServerTypeId], [AttributeName], [DefaultValue], [UnitOfMeasurement], [RelatedTable], [RelatedField]) VALUES (8, 1, N'Scan Interval', N'8', N'Minutes', N'DominoServers', N'Scan Interval')
INSERT [dbo].[ProfilesMaster] ([ID], [ServerTypeId], [AttributeName], [DefaultValue], [UnitOfMeasurement], [RelatedTable], [RelatedField]) VALUES (9, 1, N'Response Time Threshold', N'2000', N'milliseconds', N'DominoServers', N'ResponseThreshold')
INSERT [dbo].[ProfilesMaster] ([ID], [ServerTypeId], [AttributeName], [DefaultValue], [UnitOfMeasurement], [RelatedTable], [RelatedField]) VALUES (11, 1, N'Pending Mail Threshold', N'100', N'Count', N'DominoServers', N'PendingThreshold')
INSERT [dbo].[ProfilesMaster] ([ID], [ServerTypeId], [AttributeName], [DefaultValue], [UnitOfMeasurement], [RelatedTable], [RelatedField]) VALUES (12, 1, N'Dead Mail Threshold', N'100', N'Count', N'DominoServers', N'DeadThreshold')
INSERT [dbo].[ProfilesMaster] ([ID], [ServerTypeId], [AttributeName], [DefaultValue], [UnitOfMeasurement], [RelatedTable], [RelatedField]) VALUES (13, 1, N'Enable for Scan', N'0', N'True/False (1- True, 0-False)', N'DominoServers', N'Enabled')
INSERT [dbo].[ProfilesMaster] ([ID], [ServerTypeId], [AttributeName], [DefaultValue], [UnitOfMeasurement], [RelatedTable], [RelatedField]) VALUES (14, 1, N'Retry Interval', N'2', N'Minutes', N'DominoServers', N'RetryInterval')
INSERT [dbo].[ProfilesMaster] ([ID], [ServerTypeId], [AttributeName], [DefaultValue], [UnitOfMeasurement], [RelatedTable], [RelatedField]) VALUES (15, 1, N'Failure Threshold', N'2', N'No of failures', N'DominoServers', N'FailureThreshold')
INSERT [dbo].[ProfilesMaster] ([ID], [ServerTypeId], [AttributeName], [DefaultValue], [UnitOfMeasurement], [RelatedTable], [RelatedField]) VALUES (16, 1, N'Disk Space Threshold', N'10', N'Percentage free', N'DominoServers', N'DiskSpaceThreshold')
INSERT [dbo].[ProfilesMaster] ([ID], [ServerTypeId], [AttributeName], [DefaultValue], [UnitOfMeasurement], [RelatedTable], [RelatedField]) VALUES (17, 1, N'Dead Mail Delete Threshold', N'100', N'Count', N'DominoServers', N'DeadMailDeleteThreshold')
INSERT [dbo].[ProfilesMaster] ([ID], [ServerTypeId], [AttributeName], [DefaultValue], [UnitOfMeasurement], [RelatedTable], [RelatedField]) VALUES (18, 1, N'Held Mail Threshold', N'100', N'Count', N'DominoServers', N'HeldThreshold')
INSERT [dbo].[ProfilesMaster] ([ID], [ServerTypeId], [AttributeName], [DefaultValue], [UnitOfMeasurement], [RelatedTable], [RelatedField]) VALUES (19, 1, N'Memory Threshold', N'20', N'Percentage', N'DominoServers', N'Memory_Threshold')
INSERT [dbo].[ProfilesMaster] ([ID], [ServerTypeId], [AttributeName], [DefaultValue], [UnitOfMeasurement], [RelatedTable], [RelatedField]) VALUES (20, 1, N'CPU Threshold', N'40', N'Percentage', N'DominoServers', N'CPU_Threshold')
INSERT [dbo].[ProfilesMaster] ([ID], [ServerTypeId], [AttributeName], [DefaultValue], [UnitOfMeasurement], [RelatedTable], [RelatedField]) VALUES (21, 1, N'Cluster Replication Delays Threshold', N'10', N'Minutes', N'DominoServers', N'Cluster_Rep_Delays_Threshold')
INSERT [dbo].[ProfilesMaster] ([ID], [ServerTypeId], [AttributeName], [DefaultValue], [UnitOfMeasurement], [RelatedTable], [RelatedField]) VALUES (22, 3, N'Enabled', N'0', N'True/False (1- True, 0-False)', N'SametimeServers', N'Enabled')
INSERT [dbo].[ProfilesMaster] ([ID], [ServerTypeId], [AttributeName], [DefaultValue], [UnitOfMeasurement], [RelatedTable], [RelatedField]) VALUES (23, 3, N'Scan Interval', N'8', N'Minutes', N'SametimeServers', N'ScanInterval')
SET IDENTITY_INSERT [dbo].[ProfilesMaster] OFF
GO

-- =============================================
-- Author:		Natallya Shkarayeva	
-- Create date: 12/3/2013 
-- Description:	[GetAlertHistory]
--
-- This stored procedure returns all records from the AlertHistory table
-- where DateTimeAlertCleared value is Null which means the alert is active.
-- =============================================
CREATE PROCEDURE [dbo].[GetAlertHistory]
	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    SELECT ID, DeviceName, DeviceType, AlertType, Location, Details
    FROM AlertHistory WHERE DateTimeAlertCleared IS NULL
END

GO

/* 12/6/2013 NS added in case ServerTypeID does not exist in the AlertServers table */
/* 12/16/2013 NS moved the table update prior to the stored procedure creation, otherwise, 
the ServerTypeID column is not being found.
*/
IF NOT EXISTS(SELECT * from sys.columns 
            WHERE Name = N'ServerTypeID' and Object_ID = Object_ID(N'AlertServers'))
begin
    ALTER TABLE AlertServers ADD ServerTypeID int NULL
end
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
-- =============================================
CREATE PROCEDURE [dbo].[GetAlertsForAllServersByLocation]
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
		inner join Locations t6 on t4.LocationID=t6.ID and t6.Location ='URL'
		where t4.ServerID = 0
		union
		-- Select rows to cover cases when both Location and Event categories are selected as a whole,
		-- rows come from the Servers table
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
		union
		-- Select rows to cover cases when both Location and Event categories are selected as a whole,
		-- rows come from the URL table
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
		union
		-- Select rows to cover cases when ALL Locations have been selected,
		-- rows come from the Servers table
		select distinct t1.AlertKey AlertKey,t1.HoursIndicator HoursIndicator,t1.SendTo SendTo,t1.CopyTo CopyTo,
		t1.BlindCopyTo BlindCopyTo,t1.StartTime StartTime,t1.EndTime EndTime,t1.DAY Day,t1.Duration Duration,
		t1.SendSNMPTrap SendSNMPTrap,
		t2.AlertName AlertName,t8.EventName EventName,t3.ServerType ServerType,t5.ServerName ServerName
		from AlertDetails t1 inner join AlertNames t2 on t1.AlertKey=t2.AlertKey
		inner join AlertEvents t7 on t1.AlertKey = t7.AlertKey 
		inner join AlertServers t4 on t2.AlertKey=t4.AlertKey,
		EventsMaster t8, Servers t5, ServerTypes t3
		where t4.ServerID=0 and t7.EventID = 0 and t4.ServerTypeID=0 and t8.ServerTypeID=t5.ServerTypeID and t3.ID=t5.ServerTypeID
		union
		-- Select rows to cover cases when ALL Locations have been selected,
		-- rows come from the URLs table
		select distinct t1.AlertKey AlertKey,t1.HoursIndicator HoursIndicator,t1.SendTo SendTo,t1.CopyTo CopyTo,
		t1.BlindCopyTo BlindCopyTo,t1.StartTime StartTime,t1.EndTime EndTime,t1.DAY Day,t1.Duration Duration,
		t1.SendSNMPTrap SendSNMPTrap,
		t2.AlertName AlertName,t8.EventName EventName,t3.ServerType ServerType,t5.Name ServerName
		from AlertDetails t1 inner join AlertNames t2 on t1.AlertKey=t2.AlertKey
		inner join AlertEvents t7 on t1.AlertKey = t7.AlertKey 
		inner join AlertServers t4 on t2.AlertKey=t4.AlertKey,
		EventsMaster t8, URLs t5, ServerTypes t3
		where t4.ServerID=0 and t7.EventID = 0 and t4.ServerTypeID=0 and t8.ServerTypeID=t5.ServerTypeID and t3.ID=t5.ServerTypeID
	) as tmp
	order by AlertKey
END


GO

-- =============================================
-- Author:		Natallya Shkarayeva
-- Create date: 12/3/2013
-- Description:	
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
-- =============================================
CREATE PROCEDURE [dbo].[GetAlertsForSelectedEventsServers]
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    select distinct AlertKey,HoursIndicator,SendTo,CopyTo,BlindCopyTo,ISNULL(StartTime,'') StartTime,
    ISNULL(EndTime,'') Endtime,Day,Duration,SendSNMPTrap,AlertName,EventName,ServerType,ServerName from
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
		inner join Servers t5 on t5.ID=t4.ServerID and t8.ServerTypeID=t5.ServerTypeID and t4.ServerTypeID=t5.ServerTypeID
		inner join Locations t6 on t4.LocationID=t6.ID
		where t7.EventID = t8.ID
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
		inner join URLs t5 on t5.ID=t4.ServerID and t8.ServerTypeID=t5.ServerTypeID and t4.ServerTypeID=t5.ServerTypeID
		inner join Locations t6 on t4.LocationID=t6.ID 
		where t7.EventID = t8.ID
		union
		select t1.AlertKey AlertKey,t1.HoursIndicator HoursIndicator,t1.SendTo SendTo,t1.CopyTo CopyTo,
		t1.BlindCopyTo BlindCopyTo,t1.StartTime StartTime,t1.EndTime EndTime,t1.DAY Day,t1.Duration Duration,
		t1.SendSNMPTrap SendSNMPTrap,
		t2.AlertName AlertName,t4.EventName EventName,t5.ServerType ServerType,ISNULL(t6.ServerName,'') as ServerName
		from AlertDetails t1 inner join AlertNames t2 on t1.AlertKey=t2.AlertKey
		inner join AlertEvents t3 on t2.AlertKey=t3.AlertKey 
		inner join EventsMaster t4 on t3.EventID=t4.ID
		inner join ServerTypes t5 on t3.ServerTypeID=t5.ID 
		left outer join Servers t6 on t6.ServerTypeID = t5.ID
		where t6.ServerName is null and t5.ServerType != 'URL'
	) as tmp
	order by AlertKey
END

GO

-- =============================================
-- Author:		Natallya Shkarayeva
-- Create date: 12/3/2013
-- Description:	[GetAlertsWithAllEventsSelected]
--
-- This stored procedure gets event/server information based on the user selections
-- on the Alert Definition page. The query returns selections that cover all event types 
-- for a specific category, i.e., 
-- all Domino events have been selected from the events grid and some servers have been selected
-- from the servers grid.
-- =============================================
CREATE PROCEDURE [dbo].[GetAlertsWithAllEventsSelected]
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
		inner join Locations t6 on t4.LocationID=t6.ID and t6.Location ='URL'
		where t7.EventID = 0
		union
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
	) as tmp
	order by AlertKey
END

GO

-- =============================================
-- Author:		Natallya Shkarayeva
-- Create date: 12/3/2013
-- Description:	[ShouldAlertGoOutNow]
--
-- This stored procedure evaluates the alert parameters, i.e., StartTime, duration, type of alert, days to
-- notify, etc. and returns 1 if the entered parameters indicate that an alert should be sent now
-- or 0 if the time is outside of the currently active time frame. 
-- Business Hours information is based on the Settings table values.
-- =============================================
CREATE PROCEDURE [dbo].[ShouldAlertGoOutNow](@StartTime as varchar(50), @Duration as int, @DaysStr as varchar(200), @IntType as int)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	Declare @GreaterThanStart as int
	Declare @LessThanEnd as int
	Declare @LessThanStart as int
	Declare @GreaterThanEnd as int
	Declare @IsBusinessDay as int
	Declare @IsDayIncluded as int
    Declare @IsSpecific as int
    Declare @StartTimeDT as datetime
    
    Select @StartTimeDT = CONVERT(time,@StartTime)
    Select @GreaterThanStart = case when convert(time,GETDATE()) >= convert(time,svalue) then 1 else 0 end 
	from Settings where sname ='BusinessHoursStart'
	Select @LessThanEnd = case when convert(time,GETDATE()) <= convert(time,svalue) then 1 else 0 end 
	from Settings where sname = 'BusinessHoursEnd'
	Select @LessThanStart = case when convert(time,GETDATE()) < convert(time,svalue) then 1 else 0 end 
	from Settings where sname ='BusinessHoursStart'
	Select @GreaterThanEnd = case when convert(time,GETDATE()) > convert(time,svalue) then 1 else 0 end 
	from Settings where sname = 'BusinessHoursEnd'
	Select @IsBusinessDay = case when svalue='1' then 1 else 0 end 
	from settings where sname='BusinessHours' + datename(dw,GETDATE())
	if @IntType = 2
		begin
			Select @IsSpecific = case when (convert(time,GETDATE()) >= convert(time,@StartTimeDT) and 
			convert(time,GETDATE()) <= convert(time,dateadd(minute,@Duration,@StartTimeDT))) then 1 else 0 end
		end
	else
		Select @IsSpecific = 0
    Select @IsDayIncluded = case when CHARINDEX(datename(dw,GETDATE()),@DaysStr) > 0 then 1 else 0 end
    
    if @IntType = 0
		Return @GreaterThanStart & @LessThanEnd & @IsBusinessDay 
	else
		if @IntType = 1
			Return @LessThanStart | @GreaterThanEnd
		else 
			if @IntType = 2
				Return @IsSpecific & @IsDayIncluded
			else
				Return 1
END

GO

--create data base tables
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

/****** Object:  Table [dbo].[BESDailyStats]    Script Date: 12/06/2013 22:17:55 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ExchangeDailyStats](
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

/****** Object:  Table [dbo].[BESDailyStats]    Script Date: 12/06/2013 22:17:55 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ExchangeDailySummaryStats](
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


USE [vitalsigns]
GO
UPDATE dailytasks SET destinationtablename = 'DeviceDailySummaryStats' WHERE destinationtablename ='DeviceDailyStats'

UPDATE dailytasks SET destinationtablename = 'DeviceUpTimeSummaryStats' WHERE destinationtablename ='DeviceUpTimeStats' 
GO

USE [VSS_Statistics]
GO

CREATE TABLE [dbo].[ScanResults](
[ID] [int] IDENTITY(1,1) NOT NULL,
[ScanDate] [datetime] NULL,
[ServerName] [nvarchar](250) NULL,
[ScanCount] [int] NULL,
[DatabaseCount] [int] NULL)

Alter TABLE [dbo].[Daily] Add Temp Bit NULL
