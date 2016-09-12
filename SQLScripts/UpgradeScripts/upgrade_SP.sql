USE [vitalsigns]
GO

Print N'Executing V1.1 upgrade stored procedures .........';
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetAlertHistory]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetAlertHistory]
GO

/****** Object:  StoredProcedure [dbo].[GetAlertHistory]    Script Date: 2/5/2014 9:45:24 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
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


USE [vitalsigns]
GO

/****** Object:  StoredProcedure [dbo].[GetAlertsForAllServersByLocation]    Script Date: 2/5/2014 9:45:56 AM ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetAlertsForAllServersByLocation]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetAlertsForAllServersByLocation]
GO

/****** Object:  StoredProcedure [dbo].[GetAlertsForAllServersByLocation]    Script Date: 2/5/2014 9:45:56 AM ******/
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
-- The stored procedure will also cover the case where ALL event types and ALL locations have been selected.
-- Modified date: 3/6/2014 for VSPLUS-454
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

    select distinct AlertKey,HoursIndicator,SendTo,CopyTo,BlindCopyTo,ISNULL(StartTime,'') StartTime
  ,Day,Duration,SendSNMPTrap,AlertName,EventName,ServerType,ServerName,
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

USE [vitalsigns]
GO

/****** Object:  StoredProcedure [dbo].[GetAlertsForSelectedEventsServers]    Script Date: 2/5/2014 9:46:29 AM ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetAlertsForSelectedEventsServers]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetAlertsForSelectedEventsServers]
GO

/****** Object:  StoredProcedure [dbo].[GetAlertsForSelectedEventsServers]    Script Date: 2/5/2014 9:46:29 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
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
-- Modified date: 3/6/2014 for VSPLUS-454
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

USE [vitalsigns]
GO

/****** Object:  StoredProcedure [dbo].[GetAlertsWithAllEventsSelected]    Script Date: 2/5/2014 9:46:51 AM ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetAlertsWithAllEventsSelected]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetAlertsWithAllEventsSelected]
GO

/****** Object:  StoredProcedure [dbo].[GetAlertsWithAllEventsSelected]    Script Date: 2/5/2014 9:46:51 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
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
-- Modified date: 3/6/2014 for VSPLUS-454
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
USE [vitalsigns]
GO

/****** Object:  StoredProcedure [dbo].[ShouldAlertGoOutNow]    Script Date: 2/5/2014 9:47:15 AM ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ShouldAlertGoOutNow]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[ShouldAlertGoOutNow]
GO

/****** Object:  StoredProcedure [dbo].[ShouldAlertGoOutNow]    Script Date: 2/5/2014 9:47:15 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
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





-----------------------------
Print N'Executing V1.1.1 upgrade stored procedures .........';
USE [VSS_Statistics]
GO

/****** Object:  StoredProcedure [dbo].[GetDeviceHourlyVals]    Script Date: 2/5/2014 10:00:19 AM ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetDeviceHourlyVals]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetDeviceHourlyVals]
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

Print N'Executing V1.2.3 upgrade stored procedures .........';

USE [VSS_Statistics]
GO

/****** Object:  StoredProcedure [dbo].[GetExchangeHourlyVals]    Script Date: 2/10/2014 8:24:58 AM ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetExchangeHourlyVals]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].[GetExchangeHourlyVals]
GO

USE [VitalSigns]
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
--22/7/2016 Durga Modified for VSPLUS-3125

USE [vitalsigns]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ServerLocations]') AND type in (N'P', N'PC'))
/****** Object:  StoredProcedure [dbo].[ServerLocations]    Script Date: 2/10/2014 10:15:16 AM ******/
DROP PROCEDURE [dbo].[ServerLocations]
GO
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
select sr.ID,sr.name as ServerName ,ln.ID as LocationID,ST.ID as ServerTypeId,ST.ServerType,sr.Category as description
FROM   [vitalsigns].[dbo].Status s,[vitalsigns].[dbo].Nodes n,[vitalsigns].[dbo].O365Nodes o365,
[vitalsigns].[dbo].Locations ln,[vitalsigns].[dbo].O365Server sr, ServerTypes ST where s.Name=sr.Name and
 sr.ID=o365.O365ServerID and o365.NodeID=n.ID and n.LocationID=ln.ID and s.Location=ln.Location and sr.ServerTypeid = st.ID
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
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ServerTypeEvents]') AND type in (N'P', N'PC'))
/******Mukund:VSPlus-984:  Object:  StoredProcedure [dbo].[ServerTypeEvents]    Script Date: 10/11/2014 10:21:29  ******/
DROP PROCEDURE [dbo].[ServerTypeEvents]
GO
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

-- DRS: we do not need this view anymore.
USE VitalSigns
Go

IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[MonitoringTables]'))
	DROP VIEW [dbo].[MonitoringTables]
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
Exec RestructureTables
Go

USE [VSS_Statistics]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetNotesHourlyVals]') AND type in (N'P', N'PC'))
/****** Object:  StoredProcedure [dbo].[GetNotesHourlyVals]    Script Date: 20 Feb 2014 ******/
DROP PROCEDURE [dbo].[GetNotesHourlyVals]
GO

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

/* 2/20/2014 NS added for VSPLUS-36 */
USE [vitalsigns]
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ShouldScheduledReportsBeSent]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[ShouldScheduledReportsBeSent]
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
/******   ***/
---Mukund 12/06/2014 :: HeatMap stored procedure

USE [VSS_Statistics]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetExchangeHeatmap]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetExchangeHeatmap]
GO

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

--Mukund 11Apr14
USE [vitalsigns]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[fnSplitString]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[fnSplitString]
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

GO
--VSPLUS-1864 Sowjanya
--Somaraju VSPLUS 2336
/* 3/29/2016 NS modified to represent Office 365 in a correct format */
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[StatusByType]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[StatusByType]
GO
/****** Object:  StoredProcedure [dbo].[StatusByType]    Script Date: 08/19/2014 11:55:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
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
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[StatusByCategory]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[StatusByCategory]
GO
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
    and name in (SELECT ServerName FROM Servers union Select name from URLs where Enabled=1  union Select Name from O365Server where Enabled=1 union Select Name from CloudDetails where Enabled=1  union Select Name from MailServices where Enabled=1 union Select Name from NotesMailProbe where Enabled=1 union Select Name from [Network Devices] where Enabled=1 union Select Name from ExchangeMailProbe where Enabled=1 union Select Name from NotesDatabases where Enabled=1)     
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
Go

/* 5/9/2014 NS added for VSPLUS-557 */
USE [VSS_Statistics]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PopulateTravelerSummaryStats]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[PopulateTravelerSummaryStats]
GO

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
-- =============================================
--2/11/2016 Durga Modified for VSPLUS 2174
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
--VSPLUS-1864 Sowjanya
--Somaraju VSPLUS 2336
USE [vitalsigns]
GO
/****** Object:  StoredProcedure [dbo].[StatusByLocation]    Script Date: 06/20/2014 18:06:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
Alter procedure [dbo].[StatusByLocation](@type varchar(8000)) as 
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



--dhiren
USE [VSS_Statistics]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetExchHourlyVals]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetExchHourlyVals]
GO

/* 8/8/2014 NS added */
USE [VSS_Statistics]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CalcAvgDiskConsumtpion]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[CalcAvgDiskConsumtpion]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Natallya Shkarayeva
-- Create date: 8/11/2014 - added Exchange
-- Description:	<Description,,>
-- =============================================
/* 2/6/2015 NS modified */
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

/* 8/11/2014 NS added */
USE [VSS_Statistics]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CalcAvgDiskConsumptionTotal]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[CalcAvgDiskConsumptionTotal]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

/* 2/6/2015 NS modified */
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

USE [vitalsigns]
GO

/****** Object:  StoredProcedure [dbo].[ServerCredentials]    Script Date: 09/11/2014 14:40:09 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ServerCredentials]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[ServerCredentials]
GO

USE [vitalsigns]
GO

/****** Object:  StoredProcedure [dbo].[ServerCredentials]    Script Date: 09/11/2014 14:40:09 ******/
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


--Mukund 26-Aug-14
USE [VSS_Statistics]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetLyncHourlyVals]') AND type in (N'P', N'PC'))
/****** Object:  StoredProcedure [dbo].[GetLyncHourlyVals]    Script Date: 26-Aug-14 ******/
DROP PROCEDURE [dbo].[GetLyncHourlyVals]

GO

USE [vitalsigns]
GO

/****** Object:  Trigger [StatusBackupUpdate]    Script Date: 09/08/2014 16:59:46 ******/
IF  EXISTS (SELECT * FROM sys.triggers WHERE object_id = OBJECT_ID(N'[dbo].[StatusBackupUpdate]'))
DROP TRIGGER [dbo].[StatusBackupUpdate]
GO

USE [vitalsigns]
GO

/****** Object:  Trigger [dbo].[StatusBackupUpdate]    Script Date: 09/08/2014 16:59:46 ******/
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

/****** Object:  StoredProcedure [dbo].[StatusChanges]    Script Date: 09/08/2014 17:28:58 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[StatusChanges]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[StatusChanges]
GO

USE [vitalsigns]
GO

/****** Object:  StoredProcedure [dbo].[StatusChanges]    Script Date: 09/08/2014 17:28:58 ******/
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


--Mukund 15Sep14, Sharepoint, BES related
Use VSS_Statistics
Go
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetSharepointHourlyVals]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetSharepointHourlyVals]

Go

--Mukund 15Sep14, Sharepoint, BES related
Use VSS_Statistics
Go
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetBESHourlyVals]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetBESHourlyVals]
GO
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

--Mukund 30Sep14, Generic Windows related
Use VSS_Statistics
Go
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetWindowsHourlyVals]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetWindowsHourlyVals]


GO


/****** Object:  StoredProcedure [dbo].[GetActiveDirectoryHourlyVals]    Script Date: 10/09/2014 19:16:02 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Mukund Dunakhe
-- Description:	Get the data for Dashboard graphs for AD
-- =============================================
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetActiveDirectoryHourlyVals]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetActiveDirectoryHourlyVals]
GO


--WS ADDED 5/25/14 Added all microsoft related tests to one table
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetMicrosoftHourlyVals]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetMicrosoftHourlyVals]
GO
/****** Object:  StoredProcedure [dbo].[GetMicrosoftHourlyVals]    Script Date: 2/10/2014 8:24:58 AM ******/
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

--Sowjanya 28Oct14, Sametime related
Use VSS_Statistics
Go
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetSametimeHourlyVals]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetSametimeHourlyVals]
Go
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


-- Dhiren

USE [VitalSigns]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SharePointServerLocations]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[SharePointServerLocations]
GO

/****** Object:  StoredProcedure [dbo].[SharePointServerLocations] **/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


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

--Somaraju--
USE [VSS_Statistics]

GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetMailLatencyDailyStats]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetMailLatencyDailyStats]
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

/*1322-Swathi*/
--Websphere changes Sowjanya
USE [VSS_Statistics]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetWebSphereHourlyVals]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetWebSphereHourlyVals]
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

--Mukund 24Jan15
USE [vitalsigns]
GO

/****** Object:  StoredProcedure [dbo].[WSNodes]    Script Date: 24-Jan-15 5:52:38 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[WSNodes]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[WSNodes]
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

USE [VSS_Statistics]
GO

/****** Object:  StoredProcedure [dbo].[GetNetworkDeviceHourlyVals]    Script Date: 02/03/2015 16:22:59 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetNetworkDeviceHourlyVals]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetNetworkDeviceHourlyVals]
GO

USE [VSS_Statistics]
GO

/****** Object:  StoredProcedure [dbo].[GetNetworkDeviceHourlyVals]    Script Date: 02/03/2015 16:22:59 ******/
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
/* 1321 Durga */
--Webspherechnges-SWATHI
USE [vitalsigns]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[WSsametimeNodes]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[WSsametimeNodes]
GO
USE [vitalsigns]
GO
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
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[WSsametimeservers]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[WSsametimeservers]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
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

/* 2/12/2015 NS modified for VSPLUS-1429 */
USE [VSS_Statistics]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CalcNumDaysinMonth]'))
DROP FUNCTION [dbo].[CalcNumDaysinMonth]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO
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




--2/18/15 WS ADDED

USE [VSS_Statistics]
GO

/****** Object:  StoredProcedure [dbo].[GetNetworkLatencyHeatmap]    Script Date: 02/18/2015 18:12:39 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetNetworkLatencyHeatmap]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetNetworkLatencyHeatmap]
GO

USE [VSS_Statistics]
GO

/****** Object:  StoredProcedure [dbo].[GetNetworkLatencyHeatmap]    Script Date: 02/18/2015 18:12:39 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE proc [dbo].[GetNetworkLatencyHeatmap](
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
--vsplus 1552 Niranjan 
--VSPLU 1631 DURGA
--1/21/2016 Durga modified for VSPLUS-2474
USE [vitalsigns]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_MenuSorting]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[sp_MenuSorting]
GO

/****** Object:  StoredProcedure [dbo].[sp_MenuSorting]    Script Date: 03/05/2015 15:50:43 ******/
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
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SpecificServerLocations]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[SpecificServerLocations]
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

exec dbo.PR_EncDecLicense 0

--VSPLUS-1730 Durga
-- Insert existing Key in Lincese
USE [VitalSigns]
GO	
declare
@licExpDate varchar(50),
@licCount int,
@licType varchar(100),
@licKey varchar(400)
if not exists(select * from License)
begin
	if exists (select * from dbo.Settings where sname='License Type') and exists(select * from dbo.Settings where sname='License_Count') and exists(select * from dbo.Settings where sname='License Key') and exists(select * from dbo.Settings where sname='License Expiration')
	begin
 	select @licExpDate=svalue from Settings where sname='License Expiration'
 	select @licType=svalue from Settings where sname='License Type'
 	select @licCount=svalue from Settings where sname='License_Count'
 	select @licKey=svalue from Settings where sname='License Key'
	if ISNULL(@licExpDate,'') <> '' and ISNULL(@licType,'') <> '' and ISNULL(@licCount,'') <> '' and ISNULL(@licKey,'') <> ''
	begin
 		INSERT INTO dbo.License(LicenseKey,Units,InstallType,CompanyName,LicenseType,ExpirationDate) values(@licKey,@licCount,@licType,'VitalSigns Plus',@licType,@licExpDate)
	 	exec dbo.PR_EncDecLicense 0
	end
	end
end
go


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

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[tr_RefreshCollection]') and OBJECTPROPERTY(id, N'IsTrigger') = 1)
	drop trigger [dbo].[tr_RefreshCollection]
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

/* 3/18/15 WS Added for Node Flages */

USE [vitalsigns]
GO

/****** Object:  Trigger [NodeInsertedInits]    Script Date: 03/18/2015 10:19:34 ******/
IF  EXISTS (SELECT * FROM sys.triggers WHERE object_id = OBJECT_ID(N'[dbo].[NodeInsertedInits]'))
DROP TRIGGER [dbo].[NodeInsertedInits]
GO

USE [vitalsigns]
GO

/****** Object:  Trigger [dbo].[NodeInsertedInits]    Script Date: 03/18/2015 10:19:35 ******/
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

--VSPLUS 596 DURGA Adding a tab Business Hours In Server Settings Editor
--VSPLUS 1768 Durga
USE [vitalsigns]
GO
/****** Object:  StoredProcedure [dbo].[BusinessHours]    Script Date: 4/22/2015 6:51:00 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[BusinessHours]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[BusinessHours]
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
/* 5/7/2015 NS added for VSPLUS-1622 */
USE [vitalsigns]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetAlertHistoryByAlertKey]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetAlertHistoryByAlertKey]
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


/* 5/29/15 WS Added - VSPLUS-1789 */

USE [vitalsigns]
GO

/****** Object:  StoredProcedure [dbo].[GetMaxDominoScanIntervalByHours]    Script Date: 05/29/2015 13:56:32 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetMaxDominoScanIntervalByHours]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetMaxDominoScanIntervalByHours]
GO

USE [vitalsigns]
GO

/****** Object:  StoredProcedure [dbo].[GetMaxDominoScanIntervalByHours]    Script Date: 05/29/2015 13:56:32 ******/
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

/* 6/8/2015 WS added for VSPLUS 1816 */

USE [vitalsigns]
GO

/****** Object:  StoredProcedure [dbo].[InBusinessHoursByServer]    Script Date: 06/08/2015 17:13:23 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[InBusinessHoursByServer]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[InBusinessHoursByServer]
GO

USE [vitalsigns]
GO

/****** Object:  StoredProcedure [dbo].[InBusinessHoursByServer]    Script Date: 06/08/2015 17:13:23 ******/
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



/*8/21/15 WS modified for Traveler Devices Loop */

USE [vitalsigns]
GO

/****** Object:  StoredProcedure [dbo].[UpdateTravelerTempTableTVP]    Script Date: 08/21/2015 11:33:52 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UpdateTravelerTempTableTVP]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[UpdateTravelerTempTableTVP]
GO

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
USE [vitalsigns]
GO

IF EXISTS(SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.SplitDelimiterString') AND xtype IN (N'FN', N'IF', N'TF'))
	DROP FUNCTION [dbo].[SplitDelimiterString]
go

USE [vitalsigns]
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
USE [vitalsigns]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ServerTypeEventsbytemplate]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[ServerTypeEventsbytemplate]
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


USE [vitalsigns]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Getselectedeventbytemplate]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[Getselectedeventbytemplate]
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


USE [vitalsigns]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetselectedeventsbyID]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetselectedeventsbyID]
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


/* 9/30/15 WS Added for VSPLUS-2193 */

USE [VSS_Statistics]
GO

/****** Object:  StoredProcedure [dbo].[GetWebSphereScanVals]    Script Date: 09/28/2015 10:35:40 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetWebSphereScanVals]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetWebSphereScanVals]
GO

USE [VSS_Statistics]
GO

/****** Object:  StoredProcedure [dbo].[GetWebSphereScanVals]    Script Date: 09/28/2015 10:35:40 ******/
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


/* 10/6/2015 NS added for VSPLUS-2170 */
USE [vitalsigns]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ConcatUserRestrictions]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[ConcatUserRestrictions]
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

--Durga vsplus 2281
--Durga VSPLUS-2410
--VSPLUS-2281  Kiran Dadireddy
USE [VSS_Statistics]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DominoDailyCleanup]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[DominoDailyCleanup]
GO

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
--VSPLUS 2300 Sowjanya

USE [vitalsigns]
GO

/****** Object:  StoredProcedure [dbo].[GetAlertsWithAllEventsSelected]    Script Date: 2/5/2014 9:46:51 AM ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ServerLocationsDS]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[ServerLocationsDS]
GO
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

--VitalSignsDailytasks Cleanup # kiran #
USE [VSS_Statistics]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CleanUpObsoleteData]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[CleanUpObsoleteData]
GO

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
				 (SELECT top(50000)* FROM '+@SourceTableName+' WHERE date<= (GETDATE()-2))
				 DELETE from CTE; 	
				 SELECT @pCount=count(*) from '+@SourceTableName+' WHERE date<= (GETDATE()-2);'
				EXECUTE sp_executesql @sql_string,	N'@pCount int output',@pCount = @count output 				
				COMMIT TRANSACTION DELETEDOMINODAILYSTATS
			 END
		DELETE @Temp WHERE SourceTableName = @SourceTableName
	END
	DELETE FROM NotesMailStats where date <(GETDATE()+1)

END
GO

USE [Vitalsigns]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CleanUpData]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[CleanUpData]
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
GO

-- VSPLUS-2210 Somaraju
USE [VSS_Statistics]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetMicrosoftOffice365HourlyVals]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetMicrosoftOffice365HourlyVals]
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

USE [vitalsigns]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[WSIBMConnectionsNodes]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[WSIBMConnectionsNodes]
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
Go

USE [vitalsigns]
GO


IF  EXISTS (SELECT * FROM sys.triggers WHERE object_id = OBJECT_ID(N'[dbo].[TR_DelTraveler_Devices]'))
DROP TRIGGER [dbo].[TR_DelTraveler_Devices]
GO


USE [VitalSigns]
GO

/****** Object:  StoredProcedure [dbo].[AddReseedIdentityTrigger]    Script Date: 05/02/2016 11:28:03 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[AddReseedIdentityTrigger]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[AddReseedIdentityTrigger]
GO

USE [VitalSigns]
GO

/****** Object:  StoredProcedure [dbo].[AddReseedIdentityTrigger]    Script Date: 05/02/2016 11:28:03 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[AddReseedIdentityTrigger](
	@sTableName 	VARCHAR (250),
	@sColumnName   VARCHAR (250)
)  
AS BEGIN

   DECLARE @sLastCommand VARCHAR (8000)
  -- drop an exiting audit trail trigger
  Set @sLastCommand =
  'IF EXISTS (SELECT name FROM dbo.sysobjects 
  WHERE name = ''tr_ReseedIdentity_' + replace(@sTableName,' ','_') + ''' AND type = ''TR'')		
  DROP TRIGGER dbo.tr_ReseedIdentity_' + replace(@sTableName,' ','_') 
  EXEC (@sLastCommand)
print(@sLastCommand)
  -- create the new one 
  Set @sLastCommand = 
  'CREATE TRIGGER dbo.tr_ReseedIdentity_' + replace(@sTableName,' ','_') + ' ON [' + @sTableName + '] FOR DELETE AS 
  DECLARE @id int
  BEGIN
  select @id=' +@sColumnName +' from deleted  
if @id > 2147483600
DBCC CHECKIDENT (''' + @sTableName +''' , RESEED, 0)
 end '
 
  EXEC (@sLastCommand)
  print(@sLastCommand)
END
GO


USE [VitalSigns]
GO

/****** Object:  StoredProcedure [dbo].[AddAllReseedIdentityTriggers]    Script Date: 05/02/2016 11:26:54 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[AddAllReseedIdentityTriggers]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[AddAllReseedIdentityTriggers]
GO

USE [VitalSigns]
GO

/****** Object:  StoredProcedure [dbo].[AddAllReseedIdentityTriggers]    Script Date: 05/02/2016 11:26:54 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[AddAllReseedIdentityTriggers]   
AS BEGIN

  DECLARE @sLastCommand VARCHAR (2000)
    DECLARE @sTableName VARCHAR (250)
    DECLARE @sServiceName VARCHAR (250)
    DECLARE VSTableNames CURSOR FOR select TABLE_NAME,COLUMN_NAME
      from INFORMATION_SCHEMA.COLUMNS
       where TABLE_SCHEMA = 'dbo' and DATA_TYPE ='int'
       and COLUMNPROPERTY(object_id(TABLE_NAME), COLUMN_NAME, 'IsIdentity') = 1
       order by TABLE_NAME
    DECLARE VSOldTables CURSOR FOR SELECT name FROM dbo.sysobjects WHERE name like 'tr_ReseedIdentity_%' 
  
    Open VSOldTables
    FETCH NEXT FROM VSOldTables into @sTableName
  
    WHILE @@FETCH_STATUS = 0 
  	BEGIN
  	print 'Dropping old triggers...'
      -- drop an exiting audit trail trigger
      print 'Drop Reseed Identity Trigger on: ' + @sTableName
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
      WHERE name = ''tr_ReseedIdentity_' + replace(@sTableName,' ','_') + ''' AND type = ''TR'')		
      DROP TRIGGER dbo.tr_ReseedIdentity_' + replace(@sTableName,' ','_') 
      EXEC (@sLastCommand)
     print(@sLastCommand)
          print 'CREATE TRIGGER on: ' + @sTableName
          SET @sLastCommand =
  		'AddReseedIdentityTrigger ''' + @sTableName + ''',''' + @sServiceName + ''''
          EXEC( @sLastCommand )
        print @sLastCommand
      FETCH NEXT FROM VSTableNames into @sTableName,@sServiceName
    END
    CLOSE VSTableNames
  DEALLOCATE VSTableNames
END



GO


USE [VSS_STATISTICS]
GO

/****** Object:  StoredProcedure [dbo].[AddReseedIdentityTrigger]    Script Date: 05/02/2016 11:28:03 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[AddReseedIdentityTrigger]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[AddReseedIdentityTrigger]
GO

USE [VSS_STATISTICS]
GO

/****** Object:  StoredProcedure [dbo].[AddReseedIdentityTrigger]    Script Date: 05/02/2016 11:28:03 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[AddReseedIdentityTrigger](
	@sTableName 	VARCHAR (250),
	@sColumnName   VARCHAR (250)
)  
AS BEGIN

   DECLARE @sLastCommand VARCHAR (8000)
  -- drop an exiting audit trail trigger
  Set @sLastCommand =
  'IF EXISTS (SELECT name FROM dbo.sysobjects 
  WHERE name = ''tr_ReseedIdentity_' + replace(@sTableName,' ','_') + ''' AND type = ''TR'')		
  DROP TRIGGER dbo.tr_ReseedIdentity_' + replace(@sTableName,' ','_') 
  EXEC (@sLastCommand)
print(@sLastCommand)
  -- create the new one 
  Set @sLastCommand = 
  'CREATE TRIGGER dbo.tr_ReseedIdentity_' + replace(@sTableName,' ','_') + ' ON [' + @sTableName + '] FOR DELETE AS 
  DECLARE @id int
  BEGIN
  select @id=' +@sColumnName +' from deleted  
if @id > 2147483600
DBCC CHECKIDENT (''' + @sTableName +''' , RESEED, 0)
 end '
 
  EXEC (@sLastCommand)
  print(@sLastCommand)
END
GO

USE [VSS_STATISTICS]
GO

/****** Object:  StoredProcedure [dbo].[AddAllReseedIdentityTriggers]    Script Date: 05/02/2016 11:26:54 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[AddAllReseedIdentityTriggers]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[AddAllReseedIdentityTriggers]
GO

USE [VSS_STATISTICS]
GO

/****** Object:  StoredProcedure [dbo].[AddAllReseedIdentityTriggers]    Script Date: 05/02/2016 11:26:54 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[AddAllReseedIdentityTriggers]   
AS BEGIN

  DECLARE @sLastCommand VARCHAR (2000)
    DECLARE @sTableName VARCHAR (250)
    DECLARE @sServiceName VARCHAR (250)
    DECLARE VSTableNames CURSOR FOR select TABLE_NAME,COLUMN_NAME
      from INFORMATION_SCHEMA.COLUMNS
       where TABLE_SCHEMA = 'dbo' and DATA_TYPE ='int'
       and COLUMNPROPERTY(object_id(TABLE_NAME), COLUMN_NAME, 'IsIdentity') = 1
       order by TABLE_NAME
    DECLARE VSOldTables CURSOR FOR SELECT name FROM dbo.sysobjects WHERE name like 'tr_ReseedIdentity_%' 
  
    Open VSOldTables
    FETCH NEXT FROM VSOldTables into @sTableName
  
    WHILE @@FETCH_STATUS = 0 
  	BEGIN
  	print 'Dropping old triggers...'
      -- drop an exiting audit trail trigger
      print 'Drop Reseed Identity Trigger on: ' + @sTableName
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
      WHERE name = ''tr_ReseedIdentity_' + replace(@sTableName,' ','_') + ''' AND type = ''TR'')		
      DROP TRIGGER dbo.tr_ReseedIdentity_' + replace(@sTableName,' ','_') 
      EXEC (@sLastCommand)
     print(@sLastCommand)
          print 'CREATE TRIGGER on: ' + @sTableName
          SET @sLastCommand =
  		'AddReseedIdentityTrigger ''' + @sTableName + ''',''' + @sServiceName + ''''
          EXEC( @sLastCommand )
        print @sLastCommand
      FETCH NEXT FROM VSTableNames into @sTableName,@sServiceName
    END
    CLOSE VSTableNames
  DEALLOCATE VSTableNames
END



GO

USE [vitalsigns]
GO


IF  EXISTS (SELECT * FROM sys.triggers WHERE object_id = OBJECT_ID(N'[dbo].[tr_DeleteDeviceInventoryO365Server]'))
DROP TRIGGER [dbo].[tr_DeleteDeviceInventoryO365Server]
GO
CREATE TRIGGER [dbo].[tr_DeleteDeviceInventoryO365Server]
ON [dbo].[O365Server]
FOR DELETE AS

BEGIN
delete from dbo.DeviceInventory where DeviceTypeID =21  AND DeviceID=(SELECT ID  FROM DELETED) 
END
GO

-- +++++++++++ NO CHANGES BELOW THIS POINT +++++++++++
USE [vitalsigns]
GO

IF exists (select * from dbo.sysobjects where id = object_id('dbo.AddAllAuditTrailTriggers'))
	BEGIN
	-- exec stored proc on every run to create ALL audit trail triggers of new tables 
	EXECUTE dbo.AddAllAuditTrailTriggers 
	END
go

--bring the nodes down for upgrade
UPDATE dbo.NODES SET ALIVE=0, PULSE=NULL
GO

/* 5/19/15 WS added to reassign the servers to nodes */
exec PR_RefreshServerCollection 1

exec AddAllReseedIdentityTriggers


USE [VSS_STATISTICS]
GO

exec AddAllReseedIdentityTriggers

USE [vitalsigns]
GO

if NOT exists ( select * from dbo.VS_MANAGEMENT where CATEGORY = 'Upgrade_Sp' )
	
	INSERT INTO dbo.VS_MANAGEMENT ( CATEGORY, VALUE, LAST_UPDATE,DESCRIPTION,UPDATE_BY ) VALUES ( 'Upgrade_Sp', dbo.fn_GetVSVersion( ), getdate( ),'This indicates the version and LAST run date of the Upgrade SP script.',CURRENT_USER )
go

UPDATE dbo.VS_MANAGEMENT SET VALUE=dbo.fn_GetVSVersion( ), LAST_UPDATE = getdate( ) WHERE CATEGORY = 'Upgrade_Sp'
go

