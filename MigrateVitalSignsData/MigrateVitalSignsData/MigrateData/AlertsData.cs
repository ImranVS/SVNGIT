using MigrateVitalSignsData.Mappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VSNext.Mongo.Entities;
using VSNext.Mongo.Repository;
using MongoDB.Driver;
using System.Data;
using MongoDB.Bson;
namespace MigrateVitalSignsData
{
    public static class AlertsData
    {
        static Repository<Notifications> notificationsRepository = new Repository<Notifications>(MappingHelper.MongoConnectionString);
        static Repository<EventsMaster> eventsmasterRepository = new Repository<EventsMaster>(MappingHelper.MongoConnectionString);
        static Repository<BusinessHours> businessHoursRepository = new Repository<BusinessHours>(MappingHelper.MongoConnectionString);
        static Repository<NotificationDestinations> notificationdesRepository = new Repository<NotificationDestinations>(MappingHelper.MongoConnectionString);
        static Repository<Server> serverRepository = new Repository<Server>(MappingHelper.MongoConnectionString);
        static Repository<Location> locationRepository = new Repository<Location>(MappingHelper.MongoConnectionString);

        public static void MigrateAlertsData()
        {
            var businessHours = businessHoursRepository.Collection.AsQueryable().ToList();
            //Event Master
            var eventsMaster = new Mapper<EventsMaster>("EventsMaster.json").Map();

            //eventsmasterRepository.Insert(eventsMaster);

            //Notifications
            var notifications = new Mapper<Notifications>("Notification.json").Map();
            if (notifications.Count() > 0)
            {

                notificationsRepository.Insert(notifications);
            }
            //Update EventMaster with Notifications

            string query = @"SELECT AN.AlertName, EM.EventName,ST.ServerType
                              FROM vitalsigns.dbo.AlertEvents AE
                              Inner join vitalsigns.dbo.AlertNames AN on AN.AlertKey = AE.AlertKey
                              Inner Join vitalsigns.dbo.EventsMaster EM on EM.ID = AE.EventID
                              Inner join vitalsigns.dbo.ServerTypes ST on ST.ID = AE.ServerTypeID  ";
            var AlertEventsTable = (MappingHelper.ExecuteQuery(query)).AsEnumerable().Select(x => new
            {
                AlertName = x.Field<string>(0).Trim(),
                EventName = x.Field<string>(1),
                ServerType = x.Field<string>(2)
            }).ToList();
            eventsMaster = eventsmasterRepository.Collection.AsQueryable().ToList();
            notifications = notificationsRepository.Collection.AsQueryable().ToList();
            foreach (EventsMaster eventMaster in eventsMaster)
            {
                var alertNames = AlertEventsTable.Where(x => x.EventName == eventMaster.EventType).Select(x => x.AlertName);
                var notificationList = notifications.Where(x => alertNames.Contains(x.NotificationName)).Select(x => x.Id);
                if (notificationList.Count() > 0)
                {
                    var updateDefination = eventsmasterRepository.Updater.Set(p => p.NotificationList, notificationList);
                    var result = eventsmasterRepository.Update(eventMaster, updateDefination);
                }
            }

            //Notification Destination
            query = @"SELECT hi.Type,
	                           AN.AlertName,
                               ad.SMSTo,
                               ad.SendTo,
                               ad.CopyTo,
                               ad.BlindCopyTo,
                               cs.ScriptCommand,
                               cs.ScriptLocation,
                               ad.EnablePersistentAlert,
                               ad.Duration
                        FROM [vitalsigns].[dbo].[AlertDetails] AD
                        Inner join vitalsigns.dbo.AlertNames AN on AN.AlertKey=AD.AlertKey
                        LEFT JOIN [vitalsigns].[dbo].[HoursIndicator] hi  ON hi.ID=ad.HoursIndicator
                        LEFT JOIN [vitalsigns].[dbo].[CustomScripts] cs ON cs.ID= ad.ScriptID ";
            var AlertDetailsTable = (MappingHelper.ExecuteQuery(query)).AsEnumerable().Select(x => new
            {
                BusinessHour = x.Field<string>(0).Trim(),
                AlertName = x.Field<string>(1).Trim(),
                SMSTo = x.Field<string>(2),
                SendTo = x.Field<string>(3),
                CopyTo = x.Field<string>(4),
                BCC = x.Field<string>(5),
                ScriptCommand = x.Field<string>(6),
                ScriptLocation = x.Field<string>(7),
                EnablePersistentAlert = x.Field<bool?>(8),
                Duration= x.Field<int?>(9)
            }).ToList();
            Dictionary<string, List<string>> notifAndNotifDest = new Dictionary<string, List<string>>();
            List<NotificationDestinations> notifDestinations = new List<NotificationDestinations>();
            foreach (var alertDetails in AlertDetailsTable)
            {
                NotificationDestinations notifDestination = new NotificationDestinations();
                var businessHour = businessHours.FirstOrDefault(x => x.Name == alertDetails.BusinessHour);
                if (businessHour != null)
                    notifDestination.BusinessHoursId = businessHour.Id;
                if (!string.IsNullOrEmpty(alertDetails.SMSTo))
                {
                    notifDestination.SendVia = "SMS";
                    notifDestination.SendTo = alertDetails.SendTo;

                }
                else if (!string.IsNullOrEmpty(alertDetails.SendTo))
                {
                    notifDestination.SendVia = "E-mail";
                    notifDestination.SendTo = alertDetails.SendTo;
                    notifDestination.CopyTo = alertDetails.CopyTo;
                    notifDestination.BlindCopyTo = alertDetails.BCC;
                }

                notifDestination.ScriptCommand = alertDetails.ScriptCommand;
                notifDestination.ScriptLocation = alertDetails.ScriptLocation;
                notifDestination.PersistentNotification = alertDetails.EnablePersistentAlert;
                // notifDestinations.Add(notifDestination);
                notifDestination.Interval = alertDetails.Duration == 0 ? null : alertDetails.Duration;
                notificationdesRepository.Insert(notifDestination);
                if (notifAndNotifDest.ContainsKey(alertDetails.AlertName))
                {
                    List<string> notifDest = notifAndNotifDest[alertDetails.AlertName];
                    notifDest.Add(notifDestination.Id);
                }else
                {
                    List<string> notifDest = new List<string>();
                    notifDest.Add(notifDestination.Id);
                    notifAndNotifDest.Add(alertDetails.AlertName, notifDest);
                }
            }
            
            //Update Notification with notification Destinations
           // var notifResult = notificationsRepository.All();
            foreach( Notifications notif in notifications)
            {
                if (notifAndNotifDest.ContainsKey(notif.NotificationName))
                {
                    List<string> notifDest = notifAndNotifDest[notif.NotificationName];
                    if(notifDest.Count>0)
                    {
                        var updateDefination = notificationsRepository.Updater.Set(p => p.SendList, notifDest);
                        notificationsRepository.Update(notif, updateDefination);
                    }
                }
            }


            //Update servers notifications

            query = @"select AN.AlertName, asvr.ServerId, asvr.LocationID, asvr.ServerTypeID
                        From vitalsigns.dbo.AlertNames AN
                        Inner join vitalsigns.dbo.AlertServers asvr on AN.AlertKey = asvr.AlertKey";
            var AlertServers = (MappingHelper.ExecuteQuery(query)).AsEnumerable().Select(x => new AlertServersClass
            {
                AlertName = x.Field<string>(0).Trim(),
                ServerId = x.Field<int>(1).ToString(),
                LocationId = x.Field<int>(2).ToString(),
                ServerTypeId = x.Field<int>(3).ToString(),

            }).ToList();

            query = @"SELECT LocationId, id, ServerTypeId, ServerName from vitalsigns.dbo.Servers";
            var Servers = (MappingHelper.ExecuteQuery(query)).AsEnumerable().Select(x => new ServersClass
            {
                LocationId = x.Field<int>(0).ToString(),
                ServerId = x.Field<int>(1).ToString(),
                ServerTypeId = x.Field<int>(2).ToString(),
                ServerName = x.Field<string>(3),

            }).ToList();

            query = @"SELECT Location, id from vitalsigns.dbo.Locations";
            var Locations = (MappingHelper.ExecuteQuery(query)).AsEnumerable().Select(x => new LocationsClass
            {
                Location = x.Field<string>(0),
                LocationId = x.Field<int>(1).ToString()
            }).ToList();

            query = @"SELECT ID, ServerType from vitalsigns.dbo.ServerTypes";
            var ServerTypes = (MappingHelper.ExecuteQuery(query)).AsEnumerable().Select(x => new ServerTypesClass
            {
                ServerTypeId = x.Field<int>(0).ToString(),
                ServerType = x.Field<string>(1)
            }).ToList();

            var locationsMongo = locationRepository.Find(x => true).ToList();

            foreach (var entry in AlertServers)
            {
                try
                {
                    List<LocationsClass> locations;
                    if (entry.LocationId == "0")
                        locations = Locations.ToList();
                    else
                        locations = Locations.Where(x => x.LocationId == entry.LocationId).ToList();

                    List<ServerTypesClass> serverTypes;
                    if (entry.ServerTypeId == "0")
                        serverTypes = ServerTypes.ToList();
                    else
                        serverTypes = ServerTypes.Where(x => x.ServerTypeId == entry.ServerTypeId).ToList();

                    List<ServersClass> servers;
                    if (entry.ServerId == "0")
                        servers = Servers.ToList();
                    else
                        servers = Servers.Where(x => x.ServerId == entry.ServerId).ToList();

                    var notification = notifications.Where(x => x.NotificationName == entry.AlertName).First();

                    serverRepository.Update((
                        serverRepository.Filter.In(x => x.DeviceName, servers.Select(x => x.ServerName)) &
                        serverRepository.Filter.In(x => x.LocationId, locationsMongo.Where(y => locations.Select(z => z.Location).Contains(y.LocationName)).Select(y => y.Id))) &
                        serverRepository.Filter.In(x => x.DeviceName, servers.Select(x => x.ServerName)),
                        serverRepository.Updater.AddToSet(x => x.NotificationList, notification.Id));

                }
                catch(Exception ex)
                {

                }
            }


            //Events Detected



        }

        private class AlertServersClass
        {
            public String AlertName;
            public String ServerId;
            public String LocationId;
            public String ServerTypeId;
        }

        private class LocationsClass
        {
            public String LocationId;
            public String Location;
        }

        private class ServerTypesClass
        {
            public String ServerTypeId;
            public String ServerType;
        }

        private class ServersClass
        {
            public String ServerTypeId;
            public String ServerName;
            public String LocationId;
            public String ServerId;
        }
    }
}
