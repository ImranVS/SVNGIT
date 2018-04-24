/************** PLACE HOLDER TO PUT UPGRADE SCRIPTS IF ANY FOR V 1.2 ************/ 
/* 12/23/2013 NS added - delete FK from the AlertServers and AlertEvents tables */
USE [vitalsigns]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_AlertEvents_EventsMaster]') AND parent_object_id = OBJECT_ID(N'[dbo].[AlertServers]'))
ALTER TABLE [dbo].[AlertServers] DROP CONSTRAINT [FK_AlertEvents_EventsMaster]
GO

USE [vitalsigns]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_AlertServers_Servers]') AND parent_object_id = OBJECT_ID(N'[dbo].[AlertServers]'))
ALTER TABLE [dbo].[AlertServers] DROP CONSTRAINT [FK_AlertServers_Servers]
GO