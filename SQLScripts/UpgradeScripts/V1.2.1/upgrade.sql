/************** PLACE HOLDER TO PUT UPGRADE SCRIPTS IF ANY FOR V 1.2.1 ************/ 

/**** Resolve the Consolidation issues for Mail Transferred **************/
USE [vitalsigns]
GO
SET IDENTITY_INSERT [dbo].[DailyTasks] ON
delete from [dbo].[DailyTasks] where SourceStatName=N'Mail.Transferred'
INSERT [dbo].[DailyTasks] ([ID], [SourceTableName], [SourceAggregation], [SourceStatName], [DestinationTableName], [DestinationStatName]) VALUES (16, N'DominoDailyStats', N'SUM', N'Mail.Transferred', N'DominoSummaryStats', N'Mail.Transferred')

/**** I saw this missing in couple of upgrades */ 
USE [vitalsigns]
GO
Delete from [dbo].[Settings] where sname = 'AlertsOn'
INSERT [dbo].[Settings] ([sname], [svalue], [stype]) VALUES (N'AlertsOn', N'True', N'System.String')
GO 

SET IDENTITY_INSERT [dbo].[DailyTasks] OFF
GO
/* 1/10/2014 NS added set columns to not NULL before setting the PK */
ALTER TABLE DailyTasks ALTER COLUMN SourceTableName VARCHAR(150) NOT NULL
ALTER TABLE DailyTasks ALTER COLUMN SourceAggregation VARCHAR(150) NOT NULL
ALTER TABLE DailyTasks ALTER COLUMN SourceStatName VARCHAR(150) NOT NULL
GO

/* 1/9/2014 NS added PK for DailyTasks */
ALTER TABLE DailyTasks ADD PRIMARY KEY (SourceTableName,SourceAggregation,SourceStatName)
GO
/**********************************************************************/
USE [vitalsigns]
GO
drop table dbo.[AlertDefinitions]
--drop table dbo.[AlertDeviceTypes]
drop table dbo.[AlertExceptions]
--drop table dbo.[AlertLocations]
drop table dbo.[Appointments]
drop table dbo.[BBHistory]
drop table dbo.[BlackBerry Users]
drop table dbo.[DNS_Servers]
drop table dbo.[Resources]
GO