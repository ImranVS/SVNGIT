using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using VitalSigns.API.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using VitalSigns.API.Models.Charts;
using VSNext.Mongo.Repository;
using VSNext.Mongo.Entities;
using System.Linq.Expressions;
using MongoDB.Bson;
using System.Globalization;
using System.Dynamic;
using Microsoft.AspNet.Authorization;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace VitalSigns.API.Controllers
{
    [Route("[controller]")]
    [Authorize("Bearer")]
    public class DashBoardController : BaseController
    {
        private IRepository<MobileDevices> mobileDevicesRepository;
        private IRepository<DiskHealth> diskHealthRepository;
        private IRepository<StatusDetails> statusdetailsRepository;

        private IRepository<Status> statusRepository;
        private IRepository<TravelerStats> travelerRepository;
        private IRepository<Server> serverRepository;
        private IRepository<Database> databaseRepository;
        private IRepository<Outages> outagesRepository;
        private IRepository<EventsDetected> eventsRepository;
        private IRepository<TravelerStatusSummary> travelerStatsRepository;
        private IRepository<IbmConnectionsObjects> connectionsObjectsRepository;
        private IRepository<DailyStatistics> dailyStatisticsRepository;
        private IRepository<SummaryStatistics> summaryStatisticsRepository;
        private IRepository<Maintenance> maintenanceRepository;
        private IRepository<Location> locationRepository;
        private IRepository<NameValue> namevalueRepository;
        private IRepository<IbmConnectionsObjects> connectionsRepository;

        private string DateFormat = "yyyy-MM-ddTHH:mm:ss.fffK";
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}/overall/memory-usage")]
        public Chart MemoryUsage(int id)
        {
            var dataContext = new DataContext();

            var aggregate = dataContext.DailyStatistics.Aggregate()
                    .Match(new BsonDocument {
                        { "device_id", id },
                        { "stat_name", "Mem.PercentUsed" }
                    })
                    .Group(new BsonDocument {
                    { "_id",
                            new BsonDocument {
                                { "hour", new BsonDocument("$hour", "$created_on") }
                            }
                        },
                    { "created_on", new BsonDocument("$min", "$created_on") },
                    { "value", new BsonDocument("$avg", "$stat_value") }
                    })
                    .Project<Segment>(new BsonDocument {
                        { "_id", 0 },
                        { "label",
                            new BsonDocument {
                                { "$dateToString",
                                       new BsonDocument {
                                            { "format", "%H" },
                                            { "date", "$created_on" }
                                        }
                                }
                            }
                        },
                        { "value", 1 }
                    })
                    .Sort(new BsonDocument("label", 1));

            Serie serie = new Serie();
            serie.Title = "Memory Usage";
            serie.Segments = aggregate.ToList();

            List<Serie> series = new List<Serie>();
            series.Add(serie);

            Chart chart = new Chart();
            chart.Title = "Memory Usage";
            chart.Series = series;

            return chart;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}/overall/cpu-usage")]
        public Chart CPUUsage(int id)
        {
            var dataContext = new DataContext();

            var aggregate = dataContext.DailyStatistics.Aggregate()
                    .Match(new BsonDocument {
                        { "device_id", id },
                        { "stat_name", "Platform.System.PctCombinedCpuUtil" }
                    })
                    .Group(new BsonDocument {
                    { "_id",
                            new BsonDocument {
                                { "hour", new BsonDocument("$hour", "$created_on") }
                            }
                        },
                    { "created_on", new BsonDocument("$min", "$created_on") },
                    { "value", new BsonDocument("$avg", "$stat_value") }
                    })
                    .Project<Segment>(new BsonDocument {
                        { "_id", 0 },
                        { "label",
                            new BsonDocument {
                                { "$dateToString",
                                       new BsonDocument {
                                            { "format", "%H" },
                                            { "date", "$created_on" }
                                        }
                                }
                            }
                        },
                        { "value", 1 }
                    })
                    .Sort(new BsonDocument("label", 1));

            Serie serie = new Serie();
            serie.Title = "CPU Usage";
            serie.Segments = aggregate.ToList();

            List<Serie> series = new List<Serie>();
            series.Add(serie);

            Chart chart = new Chart();
            chart.Title = "CPU Usage";
            chart.Series = series;

            return chart;
        }


        ///<Author>Kiran Dadireddy</Author>
        /// <summary>
        /// Returns all mobile user devices
        /// </summary>
        /// <returns> </returns>
        [HttpGet("mobile_user_devices")]
        public APIResponse GetAllMobileUserDevices(bool isKey = false)
        {
            List<string> keyUsersList = new List<string>();
            mobileDevicesRepository = new Repository<MobileDevices>(ConnectionString);
            List<MobileUserDevice> result = null;
            if (!isKey)
            {
                result = mobileDevicesRepository.Collection
                                 .AsQueryable()
                                 .Select(x => new MobileUserDevice
                                 {
                                     Id = x.Id,
                                     UserName = x.UserName,
                                     Device = x.DeviceName,
                                     Notification = x.NotificationType,
                                     OperatingSystem = x.OSType,
                                     LastSyncTime = x.LastSyncTime,
                                     Access = x.Access,
                                     DeviceId = x.DeviceID,


                                     //IsSelected = false
                                 }).ToList();
            }
            else
            {
                FilterDefinition<MobileDevices> filterDef = mobileDevicesRepository.Filter.Exists(x => x.ThresholdSyncTime, true);
                result = mobileDevicesRepository.Find(filterDef)
                                 .Select(x => new MobileUserDevice
                                 {
                                     Id = x.Id,
                                     UserName = x.UserName,
                                     Device = x.DeviceName,
                                     Notification = x.NotificationType,
                                     OperatingSystem = x.OSType,
                                     LastSyncTime = x.LastSyncTime,
                                     Access = x.Access,
                                     DeviceId = x.DeviceID
                                 }).ToList();
            }


            maintenanceRepository = new Repository<Maintenance>(ConnectionString);
            var maintainWindows = maintenanceRepository.All().Select(x => new MaintenanceModel
            {
                Id = x.Id,


            }).ToList();

            mobileDevicesRepository = new Repository<MobileDevices>(ConnectionString);
            var keyUsers = mobileDevicesRepository.Collection.AsQueryable().Select(x => new { KeyuserID = x.Id, MaintenanceWindows = x.MaintenanceWindows });
            foreach (var maintenaceWindow in maintainWindows)
            {
                keyUsersList = new List<string>();
                var innerkeyUsers = keyUsers.Where(x => x.MaintenanceWindows.Contains(maintenaceWindow.Id)).ToList();
                foreach (var keyUser in innerkeyUsers)
                {
                    keyUsersList.Add(keyUser.KeyuserID);
                }
                maintenaceWindow.KeyUsers = keyUsersList;
            }

            Response = Common.CreateResponse(result.OrderBy(x => x.UserName));
            return Response;
        }

        ///<Author>Sowmya Pathuri</Author>
        /// <summary>
        /// Returns the mobile user devices count by type
        /// </summary>
        /// <returns>Chart</returns>
        [HttpGet("mobile_user_devices/count_by_type")]
        public APIResponse CountUserDevicesByType()
        {
            mobileDevicesRepository = new Repository<MobileDevices>(ConnectionString);
            var result = mobileDevicesRepository.Collection.Aggregate()
                                             .Group(x => x.DeviceType, g => new { label = g.Key, value = g.Count() })
                                             .Project(x => new Segment
                                             {
                                                 Label = x.label,
                                                 Value = x.value
                                             });
            Serie serie = new Serie();
            serie.Title = "Mobile Devices";
            serie.Segments = result.ToList();

            List<Serie> series = new List<Serie>();
            series.Add(serie);

            Chart chart = new Chart();
            chart.Title = "Mobile Devices";
            chart.Series = series;
            Response = Common.CreateResponse(chart);
            return Response;
        }

        ///<Author>Swathi Dongari</Author>
        /// <summary>
        /// Returns all mobile user devices counts per User
        /// </summary>
        /// <returns>Chart </returns>
        [HttpGet("mobile_user_devices/count_per_user")]
        public APIResponse GroupByDevicesCountForUser()
        {
            mobileDevicesRepository = new Repository<MobileDevices>(ConnectionString);
            var result = mobileDevicesRepository.Collection.Aggregate()
                                                .Group(x => x.UserName, g => new { label = g.Key, value = g.Count() })
                                                .Project(x => new Segment
                                                {
                                                    Label = x.label,
                                                    Value = x.value
                                                }).ToList();
            List<double?> deviceCount = result.Select(x => x.Value).Distinct().ToList();
            List<Segment> segments = new List<Segment>();
            foreach (double value in deviceCount)
            {
                Segment segment = new Segment();
                segment.Label = string.Format("Users with {0} device(s)", value);
                segment.Value = result.Where(x => x.Value == value).Count();
                segments.Add(segment);

            }

            Serie serie = new Serie();
            serie.Title = "Device count / user";
            serie.Segments = segments;
            List<Serie> series = new List<Serie>();
            series.Add(serie);
            Chart chart = new Chart();
            chart.Title = "Device count / user";
            chart.Series = series;
            Response = Common.CreateResponse(chart);
            return Response;
        }

        ///<Author>Durga</Author>
        /// <summary>
        /// Returns Mobile User Devices Sync Interval 
        /// </summary>
        /// <returns>Chart </returns>
        [HttpGet("mobile_user_devices/group_by_sync_interval")]
        public APIResponse GroupBySyncInterval()
        {
            mobileDevicesRepository = new Repository<MobileDevices>(ConnectionString);

            var result = mobileDevicesRepository.Collection.AsQueryable()
                                              .Select(x => new
                                              {
                                                  LastSyncTime = x.LastSyncTime


                                              }).ToList();


            List<Segment> segments = new List<Segment>();
            double deviceSyncLast15Min = result.Where(x => x.LastSyncTime > (DateTime.Now.ToUniversalTime().AddMinutes(-15)) && x.LastSyncTime <= DateTime.Now.ToUniversalTime()).Count();
            double deviceSyncBetween15to30 = result.Where(x => x.LastSyncTime < DateTime.Now.ToUniversalTime().AddMinutes(-15) && x.LastSyncTime >= DateTime.Now.ToUniversalTime().AddMinutes(-30)).Count();
            double deviceSyncBetween30to60 = result.Where(x => x.LastSyncTime < DateTime.Now.ToUniversalTime().AddMinutes(-30) && x.LastSyncTime >= DateTime.Now.ToUniversalTime().AddMinutes(-60)).Count();
            double deviceSyncBetween60to120 = result.Where(x => x.LastSyncTime < DateTime.Now.ToUniversalTime().AddMinutes(-60) && x.LastSyncTime >= DateTime.Now.ToUniversalTime().AddMinutes(-120)).Count();
            double deviceSyncGreater120 = result.Where(x => x.LastSyncTime < DateTime.Now.ToUniversalTime().AddMinutes(-120)).Count();
            if (deviceSyncLast15Min > 0)
            {
                segments.Add(new Segment { Label = "Within 15 mins.", Value = deviceSyncLast15Min });
            }

            if (deviceSyncBetween15to30 > 0)
            {
                segments.Add(new Segment { Label = "Between 15-30 mins.", Value = deviceSyncBetween15to30 });
            }
            if (deviceSyncBetween30to60 > 0)
            {
                segments.Add(new Segment { Label = "Between 30-60 mins.", Value = deviceSyncBetween30to60 });
            }
            if (deviceSyncBetween60to120 > 0)
            {
                segments.Add(new Segment { Label = "Between 60-120 mins.", Value = deviceSyncBetween60to120 });
            }

            if (deviceSyncGreater120 > 0)
            {
                segments.Add(new Segment { Label = "Greater than 120 mins.", Value = deviceSyncGreater120 });
            }

            Serie serie = new Serie();
            serie.Title = "Sync times";
            serie.Segments = segments.ToList();

            List<Serie> series = new List<Serie>();
            series.Add(serie);

            Chart chart = new Chart();
            chart.Title = "Sync times";
            chart.Series = series;
            Response = Common.CreateResponse(chart);
            return Response;
        }

        ///<Author>Sowjanya Korumilli</Author>
        /// <summary>
        /// Returns userdevices count by os
        /// </summary>
        /// <returns>Chart</returns>
        [HttpGet("mobile_user_devices/count_by_os")]
        public APIResponse CountUserDevicesByOS()
        {
            mobileDevicesRepository = new Repository<MobileDevices>(ConnectionString);
            var result = mobileDevicesRepository.Collection.Aggregate()
                                           .Group(x => x.OSType, g => new { label = g.Key, value = g.Count() })
                                           .Project(x => new Segment
                                           {
                                               Label = x.label,
                                               Value = x.value
                                           });
            Serie serie = new Serie();
            serie.Title = "Mobile devices OS for all Servers";
            serie.Segments = result.ToList();

            List<Serie> series = new List<Serie>();
            series.Add(serie);

            Chart chart = new Chart();
            chart.Title = "Mobile devices OS for all Servers";
            chart.Series = series;
            Response = Common.CreateResponse(chart);
            return Response;
        }

        [HttpGet("overall/disk-space")]

        public APIResponse GetStatusOfServerDiskDrives(string id = "")
        {
            //List<dynamic> disksizes = new List<dynamic>();
            DateTime maxDt = new DateTime();
            double valueDiff = 0;
            double prevValue = 0;
            double currValue = 0;
            int count = 0;
            double avgDailyGrowth = 0;
            
            try
            {
                ServerDiskStatus serverDiskStatus = new ServerDiskStatus();
                List<ServerDiskStatus> serverDiskStatusList = new List<ServerDiskStatus>();
                summaryStatisticsRepository = new Repository<SummaryStatistics>(ConnectionString);
                statusRepository = new Repository<Status>(ConnectionString);

                if (id != "")
                {
                    Expression<Func<Status, bool>> expression = (p => p.Disks != null && p.DeviceId == id);
                    var results = statusRepository.Find(expression).AsQueryable().ToList();
                    if (results.Count > 0)
                    {
                        foreach(var result in results)
                        {
                            serverDiskStatus = new ServerDiskStatus();
                            serverDiskStatus.Id = result.Id;
                            serverDiskStatus.Name = result.DeviceName;
                            foreach (DiskStatus drive in result.Disks)
                            {
                                avgDailyGrowth = 0;
                                var disksizes = summaryStatisticsRepository.Collection.AsQueryable()
                                    .Where(x => x.StatName == drive.DiskName + ".Free" && x.DeviceId == id)
                                    .OrderByDescending(x => x.StatDate).ToList();
                                if (disksizes.Count > 0)
                                {
                                    if (maxDt == DateTime.MinValue)
                                    {
                                        maxDt = Convert.ToDateTime(disksizes[0].StatDate);
                                    }
                                    prevValue = disksizes[0].StatValue / 1024 / 1024;
                                    currValue = disksizes[0].StatValue / 1024 / 1024;
                                    foreach (var disksize in disksizes)
                                    {
                                        if (disksize.StatDate >= maxDt.AddDays(-30))
                                        {
                                            valueDiff += prevValue - currValue;
                                            prevValue = currValue;
                                            currValue = disksize.StatValue /1024/1024;
                                            count += 1;
                                        }
                                        else
                                        {
                                            break;
                                        }
                                    }
                                    avgDailyGrowth = Math.Round(valueDiff / (count - 1), 1);
                                }
                                serverDiskStatus.Drives.Add(new DiskDriveStatus
                                {
                                    DiskFree = Math.Round(Convert.ToDouble(drive.DiskFree),1),
                                    DiskSize = Math.Round(Convert.ToDouble(drive.DiskSize),1),
                                    DiskName = drive.DiskName,
                                    DiskUsed = Math.Round(Convert.ToDouble(drive.DiskSize) - Convert.ToDouble(drive.DiskFree),1),
                                    PercentFree = Math.Round(Convert.ToDouble(drive.PercentFree*100),1),
                                    Threshold = drive.Threshold,
                                    AvgDailyGrowth = avgDailyGrowth
                                });
                            }
                            serverDiskStatusList.Add(serverDiskStatus);
                        }
                        
                    }
                }
                else
                {
                    Expression<Func<Status, bool>> expression = (p => p.Disks != null);
                    var results = statusRepository.Find(expression).AsQueryable().ToList();
                    if (results.Count > 0)
                    {
                        foreach (var result in results)
                        {
                            serverDiskStatus = new ServerDiskStatus();
                            serverDiskStatus.Id = result.Id;
                            serverDiskStatus.Name = result.DeviceName;
                            foreach (DiskStatus drive in result.Disks)
                            {
                                avgDailyGrowth = 0;
                                var disksizes = summaryStatisticsRepository.Collection.AsQueryable()
                                    .Where(x => x.StatName == drive.DiskName + ".Free" && x.DeviceId == result.DeviceId)
                                    .OrderByDescending(x => x.StatDate).ToList();
                                if (disksizes.Count > 0)
                                {
                                    if (maxDt == DateTime.MinValue)
                                    {
                                        maxDt = Convert.ToDateTime(disksizes[0].StatDate);
                                    }
                                    prevValue = disksizes[0].StatValue / 1024 / 1024;
                                    currValue = disksizes[0].StatValue / 1024 / 1024;
                                    foreach (var disksize in disksizes)
                                    {
                                        if (disksize.StatDate != null)
                                        {
                                            if (disksize.StatDate >= maxDt.AddDays(-30))
                                            {
                                                valueDiff += prevValue - currValue;
                                                prevValue = currValue;
                                                currValue = disksize.StatValue / 1024 / 1024;
                                                count += 1;
                                            }
                                            else
                                            {
                                                break;
                                            }
                                        }
                                        else
                                        {
                                            break;
                                        }
                                    }
                                    avgDailyGrowth = Math.Round(valueDiff / (count - 1), 1);
                                }
                                serverDiskStatus.Drives.Add(new DiskDriveStatus
                                {
                                    DiskFree = Math.Round(Convert.ToDouble(drive.DiskFree), 1),
                                    DiskSize = Math.Round(Convert.ToDouble(drive.DiskSize), 1),
                                    DiskName = drive.DiskName,
                                    DiskUsed = Math.Round(Convert.ToDouble(drive.DiskSize) - Convert.ToDouble(drive.DiskFree), 1),
                                    PercentFree = Math.Round(Convert.ToDouble(drive.PercentFree), 1),
                                    Threshold = drive.Threshold,
                                    AvgDailyGrowth = avgDailyGrowth
                                });
                            }
                            serverDiskStatusList.Add(serverDiskStatus);
                        }

                    }
                }

                Response = Common.CreateResponse(serverDiskStatusList);
                return Response;
            }
            catch (Exception exception)
            {
                Response = Common.CreateResponse(null, "Error", exception.Message);

                return Response;
            }
            
        }

        [HttpGet("{device_id}/health-assessment")]
        public APIResponse GetHealthAssessment(string device_id)
        {
            statusdetailsRepository = new Repository<StatusDetails>(ConnectionString);
            try
            {

                Expression<Func<StatusDetails, bool>> expression = (p => p.DeviceId == device_id);
                var result = statusdetailsRepository.Find(expression).Select(x => new HealthAssessment
                {
                    DeviceId = x.DeviceId,
                    Type = x.Type,
                    Category = x.category,
                    LastScan = Convert.ToString(x.LastUpdate.Value.ToString(DateFormat)),
                    TestName = x.TestName,
                    Result = x.Result,
                    Details = x.Details
                });
                Response = Common.CreateResponse(result);
            }
            catch (Exception exception)
            {
                Response = Common.CreateResponse(null, Common.ResponseStatus.Error.ToDescription(), exception.Message);

            }
            return Response;
        }


        /// <summary>
        /// Get traveler health data
        /// </summary>
        /// <author>Sowjanya</author>
        /// <returns>List of traveler health  details</returns>
        [HttpGet("traveler-health")]
        public APIResponse GetTravelerHealth(string deviceid = "")
        {
            List<TravelerHealth> result = null;
            try
            {
                statusRepository = new Repository<Status>(ConnectionString);
                if (string.IsNullOrEmpty(deviceid))
                {
                    FilterDefinition<Status> filterDef = statusRepository.Filter.Exists(p => p.TravelerStatus);
                    result = statusRepository.Find(filterDef).Select(x => new TravelerHealth
                    {
                        DeviceId = x.DeviceId,
                        DeviceName = x.DeviceName,
                        TravelerStatus = x.TravelerStatus,
                        ResourceConstraint = x.ResourceConstraint,
                        TravelerDetails = x.TravelerDetails,
                        TravelerHeartBeat = x.TravelerHeartBeat,
                        TravelerServlet = x.TravelerServlet,
                        TravelerUsers = x.TravelerUsers,
                        TravelerHA = x.TravelerHA,
                        TravelerIncrementalSyncs = x.TravelerIncrementalSyncs,
                        HttpStatus = x.HttpStatus,
                        TravelerDevicesAPIStatus = x.TravelerDevicesAPIStatus,
                    }).ToList();
                }
                else
                {
                    Expression<Func<Status, bool>> expression = (p => p.DeviceId == deviceid);
                    result = statusRepository.Find(expression).Select(x => new TravelerHealth
                    {
                        DeviceId = x.DeviceId,
                        ResourceConstraint = x.ResourceConstraint,
                        TravelerDetails = x.TravelerDetails,
                        TravelerHeartBeat = x.TravelerHeartBeat,
                        TravelerServlet = x.TravelerServlet,
                        TravelerUsers = x.TravelerUsers,
                        TravelerHA = x.TravelerHA,
                        TravelerIncrementalSyncs = x.TravelerIncrementalSyncs,
                        HttpStatus = x.HttpStatus,
                        TravelerDevicesAPIStatus = x.TravelerDevicesAPIStatus,
                    }).ToList();
                }
                Response = Common.CreateResponse(result);
                return Response;
            }

            catch (Exception exception)
            {
                Response = Common.CreateResponse(null, Common.ResponseStatus.Error.ToDescription(), exception.Message);

                return Response;
            }
        }

        [HttpGet("{id}/traveler-mailservers")]
        public IEnumerable<TravelerHealth> GetTravelerHealthGrid(string id)
        {
            travelerRepository = new Repository<TravelerStats>(ConnectionString);

            Expression<Func<TravelerStats, bool>> expression = (p => p.Id == id);
            var result = travelerRepository.Find(expression).Select(x => new TravelerHealth
            {
                // ID = x.ID,
                MailServerName = x.MailServerName,
                DateUpdated = Convert.ToString(x.DateUpdated)


            });
            return result.ToList();
        }

        [HttpGet("database")]
        public APIResponse GetDatabase(string filter_by, string filter_value, string order_by, string order_type, string group_by, string top_x, bool get_chart, string exceptions, string device_id = "")
        {
            List<ServerDatabase> data = null;
            List<Segment> segments = new List<Segment>();
            List<BsonDocument> bsonDocs;
            int charLimit = 50;

            try
            {
                var builder = Builders<Database>.Filter;
                databaseRepository = new Repository<Database>(ConnectionString);
                // filter_by is not specified - getting all documents in the collection
                if (string.IsNullOrEmpty(filter_by))
                {
                    // group_by is not specified - getting a flat list of entries, no grouping
                    if (string.IsNullOrEmpty(group_by))
                    {
                        // exceptions is not requested - getting all entries, no exceptions
                        if (string.IsNullOrEmpty(exceptions))
                        {
                            if (string.IsNullOrEmpty(device_id))
                            {
                                var res = databaseRepository.Collection.Aggregate()
                                    .Match(_ => true).ToList();
                                data = res.Select(x => new ServerDatabase
                                {
                                    DeviceId = x.DeviceId,
                                    Title = x.Title.Length > charLimit ? x.Title.Substring(0, charLimit) + "..." : x.Title,
                                    DeviceName = x.DeviceName,
                                    Status = x.Status,
                                    Folder = x.Folder,
                                    FolderCount = x.FolderCount,
                                    Details = x.Details,
                                    FileName = x.FileName,
                                    DesignTemplateName = Convert.ToString(x.DesignTemplateName == null || x.DesignTemplateName == "" ? "" : x.DesignTemplateName),
                                    FileSize = x.FileSize,
                                    Quota = x.Quota,
                                    InboxDocCount = x.InboxDocCount,
                                    ScanDateTime = Convert.ToString(x.ScanDateTime.Value.ToString(DateFormat)),
                                    ReplicaId = x.ReplicaId,
                                    DocumentCount = x.DocumentCount,
                                    Categories = x.Categories,
                                    PercentQuota = Convert.ToDouble(x.Quota > 0 ? Math.Round(Convert.ToDouble(Convert.ToDouble(x.FileSize) / Convert.ToDouble(x.Quota) * 100), 1) : 0.0)
                                }).ToList();
                            }
                            else
                            {
                                var res = databaseRepository.Collection.Aggregate()
                                    .Match(y => y.DeviceId == device_id).ToList();
                                data = res.Select(x => new ServerDatabase
                                {
                                    DeviceId = x.DeviceId,
                                    Title = x.Title.Length > charLimit ? x.Title.Substring(0, charLimit) + "..." : x.Title,
                                    DeviceName = x.DeviceName,
                                    Status = x.Status,
                                    Folder = x.Folder,
                                    FolderCount = x.FolderCount,
                                    Details = x.Details,
                                    FileName = x.FileName,
                                    DesignTemplateName = Convert.ToString(x.DesignTemplateName == null || x.DesignTemplateName == "" ? "" : x.DesignTemplateName),
                                    FileSize = x.FileSize,
                                    Quota = x.Quota,
                                    InboxDocCount = x.InboxDocCount,
                                    ScanDateTime = Convert.ToString(x.ScanDateTime.Value.ToString(DateFormat)),
                                    ReplicaId = x.ReplicaId,
                                    DocumentCount = x.DocumentCount,
                                    Categories = x.Categories,
                                    PercentQuota = Convert.ToDouble(x.Quota > 0 ? Math.Round(Convert.ToDouble(Convert.ToDouble(x.FileSize) / Convert.ToDouble(x.Quota) * 100), 1) : 0.0)
                                }).ToList();
                            }
                        }
                        else
                        {
                            var filterDef = databaseRepository.Filter.Ne(i => i.Status, "OK");
                            var res = databaseRepository.Collection.Aggregate()
                                    .Match(filterDef).ToList();

                            data = res.Select(x => new ServerDatabase
                            {
                                DeviceId = x.DeviceId,
                                Title = x.Title.Length > charLimit ? x.Title.Substring(0, charLimit) + "..." : x.Title,
                                DeviceName = x.DeviceName,
                                Status = x.Status,
                                Folder = x.Folder,
                                FolderCount = x.FolderCount,
                                Details = x.Details,
                                FileName = x.FileName,
                                DesignTemplateName = x.DesignTemplateName,
                                FileSize = x.FileSize,
                                Quota = x.Quota,
                                InboxDocCount = x.InboxDocCount,
                                ScanDateTime = Convert.ToString(x.ScanDateTime.Value.ToString(DateFormat)),
                                ReplicaId = x.ReplicaId,
                                DocumentCount = x.DocumentCount,
                                Categories = x.Categories,
                                PercentQuota = Convert.ToDouble(x.Quota > 0 ? Math.Round(Convert.ToDouble(Convert.ToDouble(x.FileSize) / Convert.ToDouble(x.Quota) * 100), 1) : 0.0)
                            }).ToList();
                        }
                        // order_by is specified - sorting resulting data by th field specified in order_by in order specified by order_type (asc/desc)
                        if (!string.IsNullOrEmpty(order_by) && !string.IsNullOrEmpty(order_type))
                        {
                            var propertyInfo = typeof(ServerDatabase).GetProperty(order_by);
                            data = order_type == "asc" ? data.OrderBy(x => propertyInfo.GetValue(x, null)).ToList() : data.OrderByDescending(x => propertyInfo.GetValue(x, null)).ToList();
                        }
                        if (!string.IsNullOrEmpty(top_x))
                        {
                            data = data.Take(Convert.ToInt32(top_x)).ToList();
                        }
                    }
                    // group_by is specified - grouping data by the field specified by the group_by parameter
                    else
                    {
                        bsonDocs = databaseRepository.Collection.Aggregate()
                                    .Group(new BsonDocument { { "_id", "$" + group_by }, { "count", new BsonDocument("$sum", 1) } }).ToList();
                        if (!string.IsNullOrEmpty(order_by) && !string.IsNullOrEmpty(order_type))
                        {
                            var propertyInfo = typeof(ServerDatabase).GetProperty(order_by);
                            bsonDocs = order_type == "asc" ? bsonDocs.OrderBy(x => propertyInfo.GetValue(x, null)).ToList() : bsonDocs.OrderByDescending(x => propertyInfo.GetValue(x, null)).ToList();
                        }
                        foreach (BsonDocument doc in bsonDocs)
                        {
                            if (!doc["_id"].IsBsonNull)
                            {
                                Segment segment = new Segment()
                                {
                                    //Might have to add additional types for support.  Format is IfThis ? DoThis : Else
                                    Label = doc["_id"].IsString ? doc["_id"].AsString :
                                    (doc["_id"].IsInt32 ? Convert.ToString(doc["_id"].AsInt32) :
                                    (doc["_id"].IsBoolean ? Convert.ToString(doc["_id"].AsBoolean) : Convert.ToString(doc["_id"].AsBsonValue))),
                                    Value = doc["count"].AsInt32
                                };
                                segments.Add(segment);
                            }
                        }
                    }
                }
                // filter_by is specified - getting a subset of documents using the value specified in the filter_value parameter
                else
                {
                    // group_by is not specified - getting a flat list of entries, no grouping
                    if (string.IsNullOrEmpty(group_by))
                    {
                        bool flag;
                        if (Boolean.TryParse(filter_value, out flag))
                        {
                            data = databaseRepository.Find(builder.Eq(filter_by, flag))
                                    .Select(x => new ServerDatabase
                                    {
                                        DeviceId = x.DeviceId,
                                        Title = x.Title.Length > charLimit ? x.Title.Substring(0, charLimit) + "..." : x.Title,
                                        DeviceName = x.DeviceName,
                                        Status = x.Status,
                                        Folder = x.Folder,
                                        FolderCount = x.FolderCount,
                                        Details = x.Details,
                                        FileName = x.FileName,
                                        DesignTemplateName = x.DesignTemplateName,
                                        FileSize = x.FileSize,
                                        Quota = x.Quota,
                                        InboxDocCount = x.InboxDocCount,
                                        ScanDateTime = Convert.ToString(x.ScanDateTime.Value.ToString(DateFormat)),
                                        ReplicaId = x.ReplicaId,
                                        DocumentCount = x.DocumentCount,
                                        Categories = x.Categories,
                                        PercentQuota = Convert.ToDouble(x.Quota > 0 ? Math.Round(Convert.ToDouble(Convert.ToDouble(x.FileSize) / Convert.ToDouble(x.Quota) * 100), 1) : 0.0)
                                    }).ToList();
                        }
                        else
                        {
                            data = databaseRepository.Find(builder.Eq(filter_by, filter_value))
                                    .Select(x => new ServerDatabase
                                    {
                                        DeviceId = x.DeviceId,
                                        Title = x.Title.Length > charLimit ? x.Title.Substring(0, charLimit) + "..." : x.Title,
                                        DeviceName = x.DeviceName,
                                        Status = x.Status,
                                        Folder = x.Folder,
                                        FolderCount = x.FolderCount,
                                        Details = x.Details,
                                        FileName = x.FileName,
                                        DesignTemplateName = x.DesignTemplateName,
                                        FileSize = x.FileSize,
                                        Quota = x.Quota,
                                        InboxDocCount = x.InboxDocCount,
                                        ScanDateTime = Convert.ToString(x.ScanDateTime.Value.ToString(DateFormat)),
                                        ReplicaId = x.ReplicaId,
                                        DocumentCount = x.DocumentCount,
                                        Categories = x.Categories,
                                        PercentQuota = Convert.ToDouble(x.Quota > 0 ? Math.Round(Convert.ToDouble(Convert.ToDouble(x.FileSize) / Convert.ToDouble(x.Quota) * 100), 1) : 0.0)
                                    }).ToList();
                        }
                        // order_by is specified - sorting resulting data by th field specified in order_by in order specified by order_type (asc/desc)
                        if (!string.IsNullOrEmpty(order_by) && !string.IsNullOrEmpty(order_type))
                        {
                            var propertyInfo = typeof(ServerDatabase).GetProperty("DeviceName");
                            data = data.OrderBy(x => propertyInfo.GetValue(x, null)).ToList();
                            propertyInfo = typeof(ServerDatabase).GetProperty(order_by);
                            data = order_type == "asc" ? data.OrderBy(x => propertyInfo.GetValue(x, null)).ToList() : data.OrderByDescending(x => propertyInfo.GetValue(x, null)).ToList();
                        }
                        if (!string.IsNullOrEmpty(top_x))
                        {
                            data = data.Take(Convert.ToInt32(top_x)).ToList();
                        }
                    }
                    // group_by is specified - grouping data by the field specified by the group_by parameter
                    else
                    {
                        bool flag;
                        if (Boolean.TryParse(filter_value, out flag))
                        {
                            bsonDocs = databaseRepository.Collection.Aggregate()
                               .Match(builder.Eq(filter_by, flag))
                               .Group(new BsonDocument { { "_id", "$" + group_by }, { "count", new BsonDocument("$sum", 1) } }).ToList();
                        }
                        else
                        {
                            bsonDocs = databaseRepository.Collection.Aggregate()
                               .Match(builder.Eq(filter_by, filter_value))
                               .Group(new BsonDocument { { "_id", "$" + group_by }, { "count", new BsonDocument("$sum", 1) } }).ToList();
                        }
                        if (!string.IsNullOrEmpty(order_by) && !string.IsNullOrEmpty(order_type))
                        {
                            var propertyInfo = typeof(ServerDatabase).GetProperty(order_by);
                            bsonDocs = order_type == "asc" ? bsonDocs.OrderBy(x => propertyInfo.GetValue(x, null)).ToList() : bsonDocs.OrderByDescending(x => propertyInfo.GetValue(x, null)).ToList();
                        }
                        foreach (BsonDocument doc in bsonDocs)
                        {
                            if (!doc["_id"].IsBsonNull)
                            {
                                if (!doc["_id"].IsBsonNull)
                                {
                                    var labelVal = "";
                                    labelVal = doc["_id"].IsString ? doc["_id"].AsString :
                                        (doc["_id"].IsInt32 ? Convert.ToString(doc["_id"].AsInt32) :
                                        (doc["_id"].IsBoolean ? Convert.ToString(doc["_id"].AsBoolean) : Convert.ToString(doc["_id"].AsBsonValue)));
                                    if (doc["_id"].AsString == "")
                                    {
                                        labelVal = "Not Assigned";
                                    }
                                    Segment segment = new Segment()
                                    {
                                        Label = labelVal,
                                        Value = doc["count"].AsInt32
                                    };
                                    segments.Add(segment);
                                }
                            }
                        }
                    }
                }

                // If a call was made without the group_by parameter, return data set as is,
                // otherwise, return data as a chart
                if (!string.IsNullOrEmpty(get_chart.ToString()))
                {
                    if (!get_chart)
                    {
                        Response = Common.CreateResponse(data);
                    }
                    else
                    {
                        if (segments.Count == 0)
                        {
                            foreach (ServerDatabase doc in data)
                            {
                                var bsondoc = doc.ToBsonDocument();
                                var propertyInfo = typeof(ServerDatabase).GetProperty(order_by);
                                Segment segment = new Segment()
                                {
                                    Label = doc.Title + " (" + doc.DeviceName + ")",
                                    Value = Convert.ToDouble(propertyInfo.GetValue(doc))
                                };
                                segments.Add(segment);
                            }
                        }
                        List<Serie> dbseries = new List<Serie>();
                        Serie dbserie = new Serie();
                        segments.RemoveAll(item => item.Label == null);
                        segments.RemoveAll(item => item.Label == "");
                        dbserie.Title = "Data";
                        dbserie.Segments = segments;
                        dbseries.Add(dbserie);
                        Chart chart = new Chart();
                        chart.Title = "Database";
                        chart.Series = dbseries;
                        Response = Common.CreateResponse(chart);
                    }
                }
                else
                {
                    Response = Common.CreateResponse(data);
                }
            }
            catch (Exception exception)
            {
                Response = Common.CreateResponse(null, "Error", exception.Message);
            }
            return Response;
        }

        [HttpGet("{device_id}/notifications")]
        public APIResponse GetNotifications(string device_id)
        {
            List<dynamic> result = new List<dynamic>();
            eventsRepository = new Repository<EventsDetected>(ConnectionString);
            try
            {
                Expression<Func<EventsDetected, bool>> expression = (p => p.DeviceId == device_id && (p.EventDetected<=DateTime.Now && p.EventDetected>=DateTime.Now.AddDays(-7)) );
                var events1 = eventsRepository.Find(expression).AsQueryable().ToList().OrderByDescending(p=>p.EventDetected);
                foreach (EventsDetected event1 in events1)
                {
                    var x = new ExpandoObject() as IDictionary<string, Object>;
                    foreach (var field in event1.ToBsonDocument())
                    {
                        x.Add(field.Name, field.Value.ToString());
                    }
                    result.Add(x);
                }

                Response = Common.CreateResponse(result);
            }
            catch (Exception exception)
            {
                Response = Common.CreateResponse(null, "Error", exception.Message);
            }
            return Response;
        }

        [HttpGet("{device_id}/outages")]
        public APIResponse GetOutages(string device_id)
        {
            outagesRepository = new Repository<Outages>(ConnectionString);
            try
            {
                Expression<Func<Outages, bool>> expression = (p => p.DeviceId == device_id);
                var result = outagesRepository.Find(expression).Select(x => new Outage
                {
                    DeviceId = x.DeviceId,
                    DeviceName = x.DeviceName,
                    DateTimeDown = Convert.ToString(x.DateTimeDown.ToString(DateFormat)),
                    DateTimeUp = Convert.ToString(x.DateTimeUp.Value.ToString(DateFormat)),
                }).ToList().OrderBy(x=>x.DeviceName);
                Response = Common.CreateResponse(result);
            }
            catch (Exception exception)
            {
                Response = Common.CreateResponse(null, "Error", exception.Message);
            }
            return Response;
        }


        /// <summary>
        /// Get all monitored tasks data
        /// </summary>
        /// <author>Sowjanya</author>
        /// <returns>List of monitored tasks details</returns>
        [HttpGet("{deviceid}/monitoredtasks")]
        public APIResponse GetMonitoredTasks(string deviceid, bool is_monitored)
        {

            statusRepository = new Repository<Status>(ConnectionString);
            try
            {

                Expression<Func<Status, bool>> expression = (p => p.DeviceId == deviceid);
                var result = statusRepository.Find(expression).FirstOrDefault();

                List<MonitoredTasks> monitoredTasks = new List<MonitoredTasks>();
                // dominoservertaskStatus.Id = result.DeviceId;
                foreach (DominoServerTask monitored in result.DominoServerTasks)
                {

                    monitoredTasks.Add(new MonitoredTasks
                    {
                        // DeviceId = monitored.
                        TaskName = monitored.TaskName,
                        Monitored = monitored.Monitored,
                        PrimaryStatus = monitored.PrimaryStatus,
                        SecondaryStatus = monitored.SecondaryStatus,
                        StatusSummary = monitored.StatusSummary,
                        LastUpdated = Convert.ToString(monitored.LastUpdated.Value.ToString(DateFormat))
                        

                    });

                }

                var monitoredData = monitoredTasks.Where(x => x.Monitored == is_monitored);

                Response = Common.CreateResponse(monitoredData);
            }
            catch (Exception exception)
            {
                Response = Common.CreateResponse(null, Common.ResponseStatus.Error.ToDescription(), exception.Message);

            }
            return Response;
        }

        /// <summary>
        /// Get traveler mails tats data
        /// </summary>
        /// <author>Sowjanya</author>
        /// <returns>List of traveler mail stats details</returns>

        [HttpGet("{deviceid}/traveler_mailstats")]
        public APIResponse Travelerstats(string deviceid)
        {
            travelerStatsRepository = new Repository<TravelerStatusSummary>(ConnectionString);
            try
            {


                Expression<Func<TravelerStatusSummary, bool>> expression = (p => p.DeviceId == deviceid);
                var result = travelerStatsRepository.Find(expression).Select(x => new TravelerHealth
                {
                    DeviceId = x.DeviceId,
                    MailServerName = x.MailServerName,
                    c_000_001 = x.c_000_001,
                    c_001_002 = x.c_001_002,
                    c_002_005 = x.c_002_005,
                    c_005_010 = x.c_005_010,
                    c_010_030 = x.c_010_030,
                    c_030_060 = x.c_030_060,
                    c_060_120 = x.c_060_120,
                    c_120_INF = x.c_120_INF,
                    DateUpdated = Convert.ToString(x.DateUpdated)


                }).ToList();

                Response = Common.CreateResponse(result);

            }

            catch (Exception exception)
            {
                Response = Common.CreateResponse(null, Common.ResponseStatus.Error.ToDescription(), exception.Message);
            }

            return Response;


        }

        [HttpGet("{device_id}/clusterhealth")]
        public APIResponse GetClusterHealth(string device_id)
        {
            statusRepository = new Repository<Status>(ConnectionString);
            try
            {
                var clusterData = statusRepository.Find(x => x.DeviceId == device_id).FirstOrDefault();
                if (clusterData != null)
                {
                    string clusterName = clusterData.ClusterName;
                    Expression<Func<Status, bool>> expression = (p => p.ClusterName == clusterName);
                    var result = statusRepository.Find(expression).Select(x => new StatusBox
                    {
                        DeviceId = x.DeviceId,
                        DeviceName = x.DeviceName,
                        ClusterName = x.ClusterName,
                        ClusterSecondsOnQueue = x.ClusterSecondsOnQueue,
                        ClusterSecondsOnQueueAverage = x.ClusterSecondsOnQueueAverage,
                        ClusterWorkQueueDepth = x.ClusterWorkQueueDepth,
                        ClusterWorkQueueDepthAverage = x.ClusterWorkQueueDepthAverage,
                        ClusterAvailability = x.ClusterAvailability,
                        ClusterAvailabilityThreshold = x.ClusterAvailabilityThreshold,
                        LastUpdated = Convert.ToString(x.LastUpdated.Value),
                        ClusterAnalysis = x.ClusterAnalysis

                    }).ToList();
                    Response = Common.CreateResponse(result);
                }
                else
                {
                    Response = Common.CreateResponse(null, "Data Error", "Cluster data not found");
                }
            }
            catch (Exception exception)
            {
                Response = Common.CreateResponse(null, "Error", exception.Message);

            }
            return Response;
        }

        [HttpGet("mail_health")]
        public APIResponse GetMailHealth(string month)
        {
            try
            {
                string[] statNames = { "Mail.TotalRouted", "Mail.Delivered", "Mail.TransferFailures", "Mail.TotalPending",
                    "Mail.AverageDeliverTime", "Mail.AverageServerHops", "Mail.AverageSizeDelivered", "SMTP.MessagesProcessed" };
                string DateFormat = "yyyy-MM";
                DateTime date = DateTime.ParseExact(month, DateFormat, CultureInfo.InvariantCulture);
                DateTime firstDayOfMonth = new DateTime(date.Year, date.Month, 1);
                DateTime nextMonth = firstDayOfMonth.AddMonths(1);

                summaryStatisticsRepository = new Repository<SummaryStatistics>(ConnectionString);
                FilterDefinition<SummaryStatistics> filterDef = summaryStatisticsRepository.Filter.Gte(x => x.CreatedOn, firstDayOfMonth) &
                    summaryStatisticsRepository.Filter.Lt(x => x.CreatedOn, nextMonth) &
                    summaryStatisticsRepository.Filter.In(x => x.StatName, statNames);

                var summaryStats = summaryStatisticsRepository.Find(filterDef).ToList();
                List<Object> result = new List<object>();
                foreach (string deviceName in summaryStats.Select(i => i.DeviceName).Distinct())
                {
                    var summaryStatsTemp = summaryStats.Where(i => i.DeviceName == deviceName).ToList();
                    result.Add(new
                    {
                        DeviceName = deviceName,
                        TotalRouted = Convert.ToInt32(summaryStatsTemp.Where(i => i.StatName == "Mail.TotalRouted").Select(i => i.StatValue).DefaultIfEmpty(0).Sum()),
                        Delivered = Convert.ToInt32(summaryStatsTemp.Where(i => i.StatName == "Mail.Delivered").Select(i => i.StatValue).DefaultIfEmpty(0).Sum()),
                        TransferFailures = Convert.ToInt32(summaryStatsTemp.Where(i => i.StatName == "Mail.TransferFailures").Select(i => i.StatValue).DefaultIfEmpty(0).Sum()),
                        TotalPending = Math.Round(summaryStatsTemp.Where(i => i.StatName == "Mail.TotalPending").Select(i => i.StatValue).DefaultIfEmpty(0).Average(), 1),
                        AvgDeliveryTimeInSeconds = Math.Round(summaryStatsTemp.Where(i => i.StatName == "Mail.AverageDeliverTime").Select(i => i.StatValue).DefaultIfEmpty(0).Average(), 1),
                        AvgServerHops = Math.Round(summaryStatsTemp.Where(i => i.StatName == "Mail.AverageServerHops").Select(i => i.StatValue).DefaultIfEmpty(0).Average(), 1),
                        AvgSizeDelivered = Math.Round(summaryStatsTemp.Where(i => i.StatName == "Mail.AverageSizeDelivered").Select(i => i.StatValue).DefaultIfEmpty(0).Average(), 1),
                        SmtpMessagesProcessed = Convert.ToInt32(summaryStatsTemp.Where(i => i.StatName == "SMTP.MessagesProcessed").Select(i => i.StatValue).DefaultIfEmpty(0).Sum())
                    });
                }

                var param = "DeviceName";
                var propertyInfo = result[0].GetType().GetProperty(param);
                Response = Common.CreateResponse(result.OrderBy(i => propertyInfo.GetValue(i, null)));
                return Response;
            }

            catch (Exception exception)
            {
                Response = Common.CreateResponse(null, "Error", exception.Message);

                return Response;
            }
        }

        [HttpGet("connections/top_communities")]
        public APIResponse ConnectionsMostActive(string deviceid, string count = "5")
        {
            try
            {

                connectionsObjectsRepository = new Repository<IbmConnectionsObjects>(ConnectionString);

                var listOfCommunity = connectionsObjectsRepository.Find(i => i.Type == "Community" && i.DeviceId == deviceid).ToList();

                var filterDef = connectionsObjectsRepository.Filter.In(i => i.ParentGUID, listOfCommunity.Select(i => i.Id).ToList()) &
                    connectionsObjectsRepository.Filter.Eq(i => i.DeviceId, deviceid);


                var result = connectionsObjectsRepository.Collection.Aggregate()
                    .Match(filterDef)
                    .Group(i => new { ParentGuid = i.ParentGUID, Type = i.Type }, g => new { Key = g.Key, Count = g.Count() })
                    .ToList()
                    .OrderByDescending(i => i.Count)
                    .ToList();

                var topParents = result.GroupBy(i => i.Key.ParentGuid)
                    .Select(group => new
                    {
                        ParentGUID = group.Key,
                        Total = group.Sum(x => x.Count)
                    })
                    .OrderByDescending(group => group.Total)
                    .Select(i => i.ParentGUID)
                    .Take(Convert.ToInt32(count));

                var topLists = result.Where(i => topParents.Contains(i.Key.ParentGuid)).ToList();

                List<Serie> listOfSeries = new List<Serie>();

                if (count == "1")
                {
                    var segments = new List<Segment>();

                    foreach (var type in topLists.Select(i => i.Key.Type).Distinct())
                    {
                        segments.Add(new Segment()
                        {
                            Label = type,
                            Value = topLists.Where(i => i.Key.Type == type).FirstOrDefault().Count
                        });
                    }

                    Serie serie = new Serie();
                    serie.Title = listOfCommunity.Where(i => i.Id == topLists.FirstOrDefault().Key.ParentGuid).FirstOrDefault().Name;
                    serie.Segments = segments;

                    listOfSeries.Add(serie);
                }
                else
                {
                    foreach (var currType in topLists.Select(i => i.Key.Type).Distinct())
                    {
                        List<Segment> listOfSegments = new List<Segment>();
                        var resultByType = topLists.Where(i => i.Key.Type == currType).ToList();
                        foreach (var currObj in topParents)
                        {
                            Segment segment = new Segment()
                            {
                                Label = listOfCommunity.Where(i => i.Id == currObj).FirstOrDefault().Name,
                                Value = resultByType.Where(i => i.Key.ParentGuid == currObj).Select(i => i.Count).FirstOrDefault()
                            };
                            listOfSegments.Add(segment);
                        }

                        listOfSeries.Add(new Serie() { Segments = listOfSegments, Title = currType });

                    }
                }
                Chart chart = new Chart()
                {
                    Series = listOfSeries,
                    Title = "Top " + count + " Communities"
                };


                Response = Common.CreateResponse(chart);
                return Response;
            }

            catch (Exception exception)
            {
                Response = Common.CreateResponse(null, "Error", exception.Message);

                return Response;
            }
        }

        [HttpGet("connections/top_tags")]
        public APIResponse ConnectionsTopTags(string type, string deviceid, string count = "5")
        {
            try
            {

                connectionsObjectsRepository = new Repository<IbmConnectionsObjects>(ConnectionString);

                var result = connectionsObjectsRepository.Collection.Aggregate()
                                               .Match(x => x.Type == type && x.DeviceId == deviceid)
                                               .Unwind(x => x.tags)
                                               .Group(new BsonDocument
                                               {
                                                   {
                                                       "_id", "$tags"
                                                   },
                                                   {
                                                       "count", new BsonDocument("$sum", 1 )
                                                   }
                                               })
                                               .Sort(new BsonDocument("count", -1))
                                               .Limit(Convert.ToInt32(count))
                                               .ToList();



                List<Segment> segmentList = new List<Segment>();

                foreach (var doc in result)
                {
                    Segment segment = new Segment()
                    {
                        Label = doc["_id"].AsString,
                        Value = doc["count"].AsInt32
                    };
                    segmentList.Add(segment);
                }

                Serie serie = new Serie();
                serie.Title = "total";
                serie.Segments = segmentList;


                List<Serie> series = new List<Serie>();
                series.Add(serie);

                Chart chart = new Chart();

                chart.Title = "Top 5 tags";
                chart.Series = series;

                Response = Common.CreateResponse(chart);

                return Response;
            }

            catch (Exception exception)
            {
                Response = Common.CreateResponse(null, "Error", exception.Message);

                return Response;
            }
        }

        [HttpGet("connections/most_active_object")]
        public APIResponse ConnectionsMostActiveObject(string type, string deviceid, string count = "5")
        {
            try
            {
                //returns the top N communities (name and count) that have the most of type
                connectionsObjectsRepository = new Repository<IbmConnectionsObjects>(ConnectionString);

                var result = connectionsObjectsRepository.Collection.Aggregate()
                    .Match(i => i.Type == type && i.ParentGUID != null && i.DeviceId == deviceid)
                    .Group(i => new { ParentGuid = i.ParentGUID }, g => new { Key = g.Key, Count = g.Count() })
                    .ToList()
                    .OrderByDescending(i => i.Count)
                    .Take(Convert.ToInt32(count))
                    .ToList();

                var filterDef = connectionsObjectsRepository.Filter.In(i => i.Id, result.Select(i => i.Key.ParentGuid).ToList()) &
                    connectionsObjectsRepository.Filter.Eq(i => i.DeviceId, deviceid);

                var listOfParents = connectionsObjectsRepository.Find(filterDef).ToList();

                List<Segment> segmentList = new List<Segment>();

                foreach (var doc in result)
                {
                    Segment segment = new Segment()
                    {
                        Label = listOfParents.Where(i => i.Id.Equals(doc.Key.ParentGuid.ToString())).FirstOrDefault().Name,
                        Value = doc.Count
                    };
                    segmentList.Add(segment);
                }

                Serie serie = new Serie();
                serie.Title = "total";
                serie.Segments = segmentList;


                List<Serie> series = new List<Serie>();
                series.Add(serie);

                Chart chart = new Chart();

                chart.Title = "Top 5 tags";
                chart.Series = series;

                Response = Common.CreateResponse(chart);
                return Response;
            }

            catch (Exception exception)
            {
                Response = Common.CreateResponse(null, "Error", exception.Message);

                return Response;
            }
        }

        [HttpGet("connections/users")]
        public APIResponse ConnectionsUsers(string deviceid)
        {
            List<UserList> result = new List<UserList>();

            try
            {
                connectionsRepository = new Repository<IbmConnectionsObjects>(ConnectionString);
                FilterDefinition<IbmConnectionsObjects> filterDef = connectionsRepository.Filter.And(connectionsRepository.Filter.Eq(x => x.Type, "Users"),
                    connectionsRepository.Filter.Eq(x => x.DeviceId, deviceid));
                result = connectionsRepository.Find(filterDef)
                    .Select(x => new UserList
                    {
                        Id = x.Id,
                        Name = x.Name
                    }).OrderBy(x => x.Name).ToList();
                Response = Common.CreateResponse(result);
                return Response;
            }
            catch (Exception ex)
            {
                Response = Common.CreateResponse(null, "Error", ex.Message);

                return Response;
            }
        }

        [HttpGet("connections/community_user")]
        public APIResponse ConnectionsCommunityUser(string deviceid)
        {
            List<string> users = new List<string>();
            List<CommunityUserList> result = new List<CommunityUserList>();
            FilterDefinition<IbmConnectionsObjects> filterDef;
            try
            {
                connectionsRepository = new Repository<IbmConnectionsObjects>(ConnectionString);
                filterDef = connectionsRepository.Filter.And(connectionsRepository.Filter.Eq(x => x.Type, "Community"),
                    connectionsRepository.Filter.Eq(x => x.DeviceId, deviceid));
                var result1 = connectionsRepository.Find(filterDef).OrderBy(x => x.Name).ToList();
                foreach (var community in result1)
                {
                    if (community.users != null)
                    {
                        filterDef = connectionsRepository.Filter.And(connectionsRepository.Filter.Eq(x => x.Type, "Users"),
                            connectionsRepository.Filter.In(x => x.Id, community.users));
                        users = connectionsRepository.Find(filterDef).OrderBy(x => x.Name).Select(x => x.Name).ToList();
                        if (users != null)
                        {
                            foreach (var user in users)
                            {
                                result.Add(new CommunityUserList{
                                    Id = community.Id,
                                    Community = community.Name,
                                    Name = user
                                });
                            }
                        }
                    }
                }
                Response = Common.CreateResponse(result);
                return Response;
            }
            catch (Exception ex)
            {
                Response = Common.CreateResponse(null, "Error", ex.Message);

                return Response;
            }
        }

        [HttpGet("connections/compare_users")]
        public APIResponse ConnectionsCompareUsers(string deviceid, string user1 = "", string user2 = "", string username1 = "", string username2 = "")
        {
            List<UserComparison> result1 = new List<UserComparison>();
            List<UserComparison> result2 = new List<UserComparison>();
            List<UserComparison> result3 = new List<UserComparison>();
            List<UserComparison> result = null;
            FilterDefinition<IbmConnectionsObjects> filterDef;

            try
            {
                if (user1 != "" && user2 != "" && username1 != "" && username2 != "")
                {
                    connectionsRepository = new Repository<IbmConnectionsObjects>(ConnectionString);
                    //Get communities in common for both users
                    filterDef = connectionsRepository.Filter.And(connectionsRepository.Filter.Eq(x => x.Type, "Community"),
                        connectionsRepository.Filter.Eq(x => x.DeviceId, deviceid),
                        connectionsRepository.Filter.AnyEq(x => x.users, user1),
                        connectionsRepository.Filter.AnyEq(x => x.users, user2));
                    result1 = connectionsRepository.Find(filterDef)
                        .Select(x => new UserComparison
                        {
                            Id = x.Id,
                            Name = x.Name,
                            Category = "Communities in common"
                        }).OrderBy(x => x.Name).ToList();

                    //Get communities to which only user1 belongs
                    filterDef = connectionsRepository.Filter.And(connectionsRepository.Filter.Eq(x => x.Type, "Community"),
                        connectionsRepository.Filter.Eq(x => x.DeviceId, deviceid),
                        connectionsRepository.Filter.AnyEq(x => x.users, user1),
                        connectionsRepository.Filter.AnyNe(x => x.users, user2));
                    result2 = connectionsRepository.Find(filterDef)
                        .Select(x => new UserComparison
                        {
                            Id = x.Id,
                            Name = x.Name,
                            Category = "Communities of which only " + username1 + " is a member"
                        }).OrderBy(x => x.Name).ToList();

                    //Get communities to which only user2 belongs
                    filterDef = connectionsRepository.Filter.And(connectionsRepository.Filter.Eq(x => x.Type, "Community"),
                        connectionsRepository.Filter.Eq(x => x.DeviceId, deviceid),
                        connectionsRepository.Filter.AnyNe(x => x.users, user1),
                        connectionsRepository.Filter.AnyEq(x => x.users, user2));
                    result3 = connectionsRepository.Find(filterDef)
                        .Select(x => new UserComparison
                        {
                            Id = x.Id,
                            Name = x.Name,
                            Category = "Communities of which only " + username2 + " is a member"
                        }).OrderBy(x => x.Name).ToList();
                    result = new List<UserComparison>();
                    result.AddRange(result1);
                    result.AddRange(result2);
                    result.AddRange(result3);
                }
                Response = Common.CreateResponse(result);
                return Response;
            }
            catch (Exception ex)
            {
                Response = Common.CreateResponse(null, "Error", ex.Message);

                return Response;
            }
        }

        [HttpGet("users/cost_per_user")]
        public APIResponse GetCostPerUser(string deviceId, string sortby, string startDate = "", string endDate = "", bool isChart = false, int top_x = 0)
        {
            Repository<Server> serverRepository = new Repository<Server>(ConnectionString);
            Repository<SummaryStatistics> summaryRepository = new Repository<SummaryStatistics>(ConnectionString);
            List<dynamic> result = new List<dynamic>();
            List<Server> serverlist = null;
            string fieldName = "monthly_operating_cost";

            if (startDate == "")
                startDate = DateTime.Now.AddDays(-7).ToString(DateFormat);

            if (endDate == "")
                endDate = DateTime.Today.ToString(DateFormat);


            //1 day is added to the end so we include that days data
            DateTime dtStart = DateTime.ParseExact(startDate, DateFormat, CultureInfo.InvariantCulture);
            DateTime dtEnd = DateTime.ParseExact(endDate, DateFormat, CultureInfo.InvariantCulture).AddDays(1);

            dtStart = DateTime.SpecifyKind(dtStart, DateTimeKind.Utc);
            dtEnd = DateTime.SpecifyKind(dtEnd, DateTimeKind.Utc);
            try
            {
                FilterDefinition<SummaryStatistics> filterDefTemp;
                FilterDefinition<SummaryStatistics> filterDef = summaryRepository.Filter.Gte(p => p.CreatedOn, dtStart) &
                    summaryRepository.Filter.Lte(p => p.CreatedOn, dtEnd);
                if (string.IsNullOrEmpty(deviceId))
                {
                    filterDefTemp = filterDef &
                    summaryRepository.Filter.And(summaryRepository.Filter.Ne(p => p.DeviceName, null),
                                                summaryRepository.Filter.Eq(p => p.StatName, "Server.Users"));
                }
                else
                {
                    filterDefTemp = filterDef &
                    summaryRepository.Filter.And(summaryRepository.Filter.Ne(p => p.DeviceName, null),
                                                summaryRepository.Filter.Eq(p => p.StatName, "Server.Users"),
                                                 summaryRepository.Filter.Eq(p => p.DeviceId, deviceId));

                }
                var summarytemp = summaryRepository.Find(filterDefTemp).OrderBy(p => p.DeviceName);
                var summarylist = summarytemp.GroupBy(row => new
                {
                    row.DeviceId,
                    row.DeviceName
                })
                    .Select(grp => new
                    {
                        Id = grp.Key.DeviceId,
                        Label = grp.Key.DeviceName,
                        Value = grp.Average(x => x.StatValue)
                    }).ToList(); ;
                serverlist = serverRepository.Collection.Aggregate().ToList();
                foreach (var stats in summarylist)
                {
                    bool found = false;
                    var x = new ExpandoObject() as IDictionary<string, Object>;
                    x.Add("device_name", stats.Label);
                    foreach (Server server in serverlist)
                    {
                        if (stats.Id == server.Id)
                        {
                            found = true;
                            var bson2 = server.ToBsonDocument();
                            var fieldvalue = 0.0;
                            if (bson2[fieldName] != null)
                            {
                                fieldvalue = bson2[fieldName].ToDouble();
                            }
                            if (stats.Value != 0)
                            {
                                x.Add("cost_per_user", Math.Round(fieldvalue / stats.Value, 2));
                            }
                            else
                            {
                                x.Add("cost_per_user", 0.0);
                            }
                        }
                    }
                    if (!found)
                    {
                        x.Add("cost_per_user", 0.0);
                    }
                    result.Add(x);
                }
                if (isChart)
                {
                    List<Segment> segmentList = new List<Segment>();
                    if (string.IsNullOrEmpty(sortby))
                    {
                        result = result.OrderBy(x => x.device_name).ToList();
                    }
                    else
                    {
                        result = result.OrderByDescending(x => x.cost_per_user).ToList();
                    }
                    if (top_x > 0)
                    {
                        result = result.Take(top_x).ToList();
                    }
                    foreach (var doc in result)
                    {
                        if (doc.cost_per_user != null)
                        {
                            Segment segment = new Segment()
                            {
                                Label = doc.device_name,
                                Value = Convert.ToDouble(doc.cost_per_user)
                            };
                            segmentList.Add(segment);
                        }
                        else
                        {
                            Segment segment = new Segment()
                            {
                                Label = doc.device_name,
                                Value = 0.0
                            };
                            segmentList.Add(segment);
                        }
                        
                    }

                    Serie serie = new Serie();
                    serie.Title = "";
                    serie.Segments = segmentList;


                    List<Serie> series = new List<Serie>();
                    series.Add(serie);

                    Chart chart = new Chart();

                    chart.Title = "";
                    chart.Series = series;

                    Response = Common.CreateResponse(chart);
                }
                else
                {
                    Response = Common.CreateResponse(result);
                }
            }
            catch (Exception exception)
            {
                Response = Common.CreateResponse(null, "Error", exception.Message);

            }
            return Response;
        }

        [HttpGet("cpu_memory_health")]
        public APIResponse GetCPUMemory()
        {
            List<dynamic> result = new List<dynamic>();
            FilterDefinition<Server> filterDefServer;
            try
            {
                serverRepository = new Repository<Server>(ConnectionString);
                dailyStatisticsRepository = new Repository<DailyStatistics>(ConnectionString);
                FilterDefinition<DailyStatistics> filterDef = dailyStatisticsRepository.Filter.Eq(x => x.StatName, "Mem.PercentUsed");
                var dailyStatsMem = dailyStatisticsRepository.Find(filterDef);
                var result1 = dailyStatsMem
                    .GroupBy(row => new
                    {
                        row.DeviceName,
                        row.StatName
                    })
                    .Select(row => new
                    {
                        DeviceName = row.Key.DeviceName,
                        AvgMem = Math.Round(row.Average(x => x.StatValue), 2),
                        MaxMem = row.Max(x => x.StatValue)
                    }).OrderBy(x => x.DeviceName).ToList();
                filterDef = dailyStatisticsRepository.Filter.Eq(x => x.StatName, "Platform.System.PctCombinedCpuUtil");
                var dailyStatsCPU = dailyStatisticsRepository.Find(filterDef);
                var result2 = dailyStatsCPU
                    .GroupBy(row => new
                    {
                        row.DeviceName,
                        row.StatName
                    })
                    .Select(row => new
                    {
                        DeviceName = row.Key.DeviceName,
                        AvgCPU = Math.Round(row.Average(x => x.StatValue), 2),
                        MaxCPU = row.Max(x => x.StatValue)
                    }).ToList();
                statusRepository = new Repository<Status>(ConnectionString);
                FilterDefinition<Status> filterDefStatus = statusRepository.Filter.And(statusRepository.Filter.In(i => i.DeviceName, result1.Select(i => i.DeviceName).ToList()),
                    statusRepository.Filter.Exists(i => i.Memory, true), statusRepository.Filter.Exists(i => i.CPU, true));
                var result3 = statusRepository.Find(filterDefStatus).ToList();

                foreach (var stat1 in result1)
                {
                    var x = new ExpandoObject() as IDictionary<string, Object>;
                    foreach (var field in stat1.ToBsonDocument())
                    {
                        x.Add(field.Name, field.Value.ToString());
                    }
                    foreach (var stat2 in result2)
                    {
                        if (stat1.DeviceName == stat2.DeviceName)
                        {
                            foreach (var field in stat2.ToBsonDocument())
                            {
                                if (field.Name != "DeviceName")
                                {
                                    x.Add(field.Name, field.Value.ToString());
                                }
                            }
                        }
                    }
                    foreach (var stat3 in result3)
                    {
                        if (stat1.DeviceName == stat3.DeviceName)
                        {
                            var doc = stat3.ToBsonDocument();
                            x.Add("Memory", Math.Round(Convert.ToDouble(doc["memory"].ToString()) * 100, 2).ToString());
                            x.Add("CPU", Math.Round(Convert.ToDouble(doc["cpu"].ToString()) * 100, 2).ToString());
                            filterDefServer = serverRepository.Filter.Eq(i => i.Id, stat3.DeviceId);
                            var advsettings = serverRepository.Find(filterDefServer).ToList();
                            if (advsettings.Count > 0)
                            {
                                var memT = advsettings[0].MemoryThreshold != null ? Math.Round(Convert.ToDouble(advsettings[0].MemoryThreshold) * 100, 2) : 0.0;
                                var cpuT = advsettings[0].CpuThreshold != null ? Math.Round(Convert.ToDouble(advsettings[0].CpuThreshold) * 100, 2) : 0.0;
                                x.Add("MemoryThreshold", memT);
                                x.Add("CPUThreshold", cpuT);
                            }
                        }
                    }
                    result.Add(x);
                }
                Response = Common.CreateResponse(result);
            }
            catch (Exception exception)
            {
                Response = Common.CreateResponse(null, "Error", exception.Message);

            }
            return Response;
        }

        [HttpGet("disk_health")]
        public APIResponse GetDisk()
        {
            List<dynamic> result = new List<dynamic>();
            List<DiskStatus> disks = new List<DiskStatus>();
            try
            {
                statusRepository = new Repository<Status>(ConnectionString);
                FilterDefinition<Status> filterDefStatus = statusRepository.Filter.Exists(x => x.Disks, true);
                List<Status> result1 = statusRepository.Find(filterDefStatus).AsQueryable().OrderBy(x => x.DeviceName).ToList();
                foreach (Status status in result1)
                {
                    disks = status.Disks;
                    foreach (DiskStatus drive in disks)
                    {
                        result.Add(new DiskDriveStatus
                        {
                            DeviceName = status.DeviceName,
                            DiskFree = drive.DiskFree == null ? 0 : drive.DiskFree,
                            DiskSize = drive.DiskSize == null ? 0 : drive.DiskSize,
                            DiskName = drive.DiskName,
                            DiskUsed = drive.DiskFree == null || drive.DiskSize == null ? 0 : drive.DiskSize - drive.DiskFree,
                            PercentFree = drive.PercentFree == null ? 0 : Math.Round(Convert.ToDouble(drive.PercentFree) * 100, 1),
                            Threshold = drive.Threshold == null ? 0 : drive.Threshold,
                            ThresholdType = drive.ThresholdType,
                            LastUpdated = status.LastUpdated,
                            Location = status.Location
                        });

                    }
                }
                Response = Common.CreateResponse(result);
            }
            catch (Exception exception)
            {
                Response = Common.CreateResponse(null, "Error", exception.Message);

            }
            return Response;
        }


        /// <summary>
        /// Get mail delivery status data
        /// </summary>
        /// <author>Sowjanya</author>
        /// <returns>List of mail delivery status details</returns>
        [HttpGet("get_mail_delivery_status/{deviceType}")]
        public APIResponse GetMailDeliveryStatusData(string deviceType)
        {
            try
            {
                statusRepository = new Repository<Status>(ConnectionString);
                Expression<Func<Status, bool>> expression = (p => p.DeviceType == deviceType);
                var result = statusRepository.Find(expression).Select(x => new MailDeliveryStatusModel
                {
                    DeviceName = x.DeviceName,
                    Category = x.Category,
                    LastUpdated = x.LastUpdated.Value.ToString(DateFormat),
                    PendingMail = x.PendingMail == null ? 0 : x.PendingMail,
                    DeadMail = x.DeadMail == null ? 0 : x.DeadMail,
                    HeldMail = x.HeldMail == null ? 0 : x.HeldMail,
                    Location = x.Location,
                    StatusCode = x.StatusCode,
                    PendingThreshold = x.PendingThreshold == null ? 0 : x.PendingThreshold,
                    DeadThreshold = x.DeadThreshold == null ? 0 : x.DeadThreshold,
                    HeldThreshold = x.HeldThreshold == null ? 0 : x.HeldThreshold

                }).ToList().OrderBy(x => x.DeviceName).OrderByDescending(x => x.PendingMail == null ? 0 : x.PendingMail);
                Response = Common.CreateResponse(result);
            }

            catch (Exception exception)
            {
                Response = Common.CreateResponse(null, Common.ResponseStatus.Error.ToDescription(), "Get mail delivery status failed .\n Error Message :" + exception.Message);
            }
            return Response;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="statdate"></param>
        /// <author>sowmya</author>
        /// <returns></returns>
        [HttpGet("get_domino_statistics")]
        public APIResponse GetOverallDominoStatistics(DateTime statdate)
        {
            try
            {
                List<DominoStatisticsModel> dominoStatisticsData = new List<DominoStatisticsModel>();
                summaryStatisticsRepository = new Repository<SummaryStatistics>(ConnectionString);
                serverRepository = new Repository<Server>(ConnectionString);
                locationRepository = new Repository<Location>(ConnectionString);
                List<SummaryDataModel> summaryStats = new List<SummaryDataModel>();
                if (statdate == DateTime.MinValue || statdate.Date == DateTime.Now.Date)
                {
                    statdate = DateTime.Now;
                    var result = summaryStatisticsRepository.All().Where(x => x.DeviceType == "Domino" && x.StatName != null && x.StatDate.HasValue && x.StatDate.Value.Month == statdate.Month && x.StatDate.Value.Year == statdate.Year).ToList();                   
                    summaryStats = result.GroupBy(x => new { x.DeviceId, x.DeviceName, x.StatName })
                                         .Select(x => new SummaryDataModel
                                         {                                     
                                             DeviceID = x.Key.DeviceId,
                                             DeviceName = x.Key.DeviceName,
                                             StatName = x.Key.StatName,
                                             SumValue = x.Sum(y => y.StatValue),
                                             AvgValue = x.Average(y => y.StatValue)                                        
                                         }).ToList();                                     
                }
                else
                {
                    var result = summaryStatisticsRepository.All().Where(x => x.DeviceType == "Domino" && x.StatName != null && x.StatDate.HasValue && x.StatDate.Value.Month == statdate.Month && x.StatDate.Value.Year == statdate.Year).ToList();
                    summaryStats = result.GroupBy(x => new { x.DeviceId, x.DeviceName, x.StatName })
                                         .Select(x => new SummaryDataModel
                                         {
                                             DeviceID = x.Key.DeviceId,
                                             DeviceName = x.Key.DeviceName,
                                             StatName = x.Key.StatName,
                                             SumValue = x.Sum(y => y.StatValue),
                                             AvgValue = x.Average(y => y.StatValue)                                         
                                         }).ToList();

                }
                var distinctData = summaryStats.Select(x => new { DeviceId = x.DeviceID, DeviceName = x.DeviceName }).Distinct().OrderBy(x => x.DeviceName).ToList();
                foreach (var item in distinctData)
                {
                    if (item != null)
                    {
                        var server = serverRepository.Collection.AsQueryable().FirstOrDefault(x => x.Id == item.DeviceId);
                       
                        if (server != null && server.IsEnabled.HasValue && server.IsEnabled.Value)
                        {
                            var locationname = string.Empty;
                            if (server.LocationId != null)
                            {
                                var location = locationRepository.All().FirstOrDefault(x => x.Id == server.LocationId);
                                if(location != null)
                                      locationname = location.LocationName;                               
                            }                        
                            DominoStatisticsModel dominoStats = new DominoStatisticsModel();
                            dominoStats.DeviceName = item.DeviceName;
                            dominoStats.LocationName = locationname;

                            var mailDelivered = summaryStats.FirstOrDefault(x => x.DeviceID == item.DeviceId && x.StatName == "Mail.Delivered");
                            if (mailDelivered != null)
                                dominoStats.TotalMailDelivered = mailDelivered.SumValue;
                            else
                                dominoStats.TotalMailDelivered = null;

                            var mailaveragedelivertime = summaryStats.FirstOrDefault(x => x.DeviceID == item.DeviceId && x.StatName == "Mail.AverageDeliverTime");

                            if (mailaveragedelivertime != null)
                                dominoStats.AvgMailDelivery = mailaveragedelivertime.AvgValue;
                            else
                                dominoStats.AvgMailDelivery = null;

                            var serveravailabilityindex = summaryStats.FirstOrDefault(x => x.DeviceID == item.DeviceId && x.StatName == "Server.AvailabilityIndex");
                            if (serveravailabilityindex != null)
                                dominoStats.AvgServerAvailabilityIndex = serveravailabilityindex.AvgValue;
                            else
                                dominoStats.AvgServerAvailabilityIndex = null;

                            var hourlydowntimemin = summaryStats.FirstOrDefault(x => x.DeviceID == item.DeviceId && x.StatName == "HourlyDownTimeMinutes");
                            if (hourlydowntimemin != null)
                                dominoStats.DownTime = hourlydowntimemin.SumValue;
                            else
                                dominoStats.DownTime = null;

                            var mempercentavilable = summaryStats.FirstOrDefault(x => x.DeviceID == item.DeviceId && x.StatName == "Mem.PercentAvailable");
                            if (mempercentavilable != null)
                                dominoStats.AvgMemory = mempercentavilable.AvgValue;
                            else
                                dominoStats.AvgMemory = null;

                            var dominocommandopendocument = summaryStats.FirstOrDefault(x => x.DeviceID == item.DeviceId && x.StatName == "Domino.Command.OpenDocument");
                            if (dominocommandopendocument != null)
                                dominoStats.WebDocumentsOpened = dominocommandopendocument.SumValue;
                            else
                                dominoStats.WebDocumentsOpened = null;

                            var dominocommandcreatedocument = summaryStats.FirstOrDefault(x => x.DeviceID == item.DeviceId && x.StatName == "Domino.Command.CreateDocument");
                            if (dominocommandcreatedocument != null)
                                dominoStats.WebDocumentsCreated = dominocommandcreatedocument.SumValue;
                            else
                                dominoStats.WebDocumentsCreated = null;

                            var dominocommandopendb = summaryStats.FirstOrDefault(x => x.DeviceID == item.DeviceId && x.StatName == "Domino.Command.OpenDatabase");
                            if (dominocommandopendb != null)
                                dominoStats.WebDatabaseOpened = dominocommandopendb.SumValue;
                            else
                                dominoStats.WebDatabaseOpened = null;

                            var dominocommandopenview = summaryStats.FirstOrDefault(x => x.DeviceID == item.DeviceId && x.StatName == "Domino.Command.OpenView");
                            if (dominocommandopenview != null)
                                dominoStats.WebViewsOpened = dominocommandopenview.SumValue;
                            else
                                dominoStats.WebViewsOpened = null;

                            var dominocommandtotal = summaryStats.FirstOrDefault(x => x.DeviceID == item.DeviceId && x.StatName == "Domino.Command.Total");
                            if (dominocommandtotal != null)
                                dominoStats.WebCommandsTotal = dominocommandtotal.SumValue;
                            else
                                dominoStats.WebCommandsTotal = null;

                            var httpsession = summaryStats.FirstOrDefault(x => x.DeviceID == item.DeviceId && x.StatName == "Http.CurrentConnections");
                            if (httpsession != null)
                                dominoStats.HttpSession = httpsession.SumValue;
                            else
                                dominoStats.HttpSession = null;

                            dominoStatisticsData.Add(dominoStats);
                        }
                    }
                }

                Response = Common.CreateResponse(dominoStatisticsData);
            }

            catch (Exception exception)
            {
                Response = Common.CreateResponse(null, Common.ResponseStatus.Error.ToDescription(), "Get Overall Domino Statistics falied .\n Error Message :" + exception.Message);
            }
            return Response;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="statdate"></param>
        /// <author>sowmya</author>
        /// <returns></returns>
        [HttpGet("get_sametime_statistics")]
        public APIResponse GetSametimeStatistics(DateTime statdate)
        {
            try
            {
                List<SametimeStatisticsModel> sametimeStatisticsData = new List<SametimeStatisticsModel>();
                summaryStatisticsRepository = new Repository<SummaryStatistics>(ConnectionString);
                serverRepository = new Repository<Server>(ConnectionString);
                List<SummaryDataModel> summaryStats = new List<SummaryDataModel>();
                if (statdate == DateTime.MinValue || statdate.Date == DateTime.Now.Date)
                {
                    statdate = DateTime.Now;
                    var result = summaryStatisticsRepository.All().Where(x => x.DeviceType == "Sametime" && x.StatName != null && x.StatDate.HasValue && x.StatDate.Value.Date == statdate.Date).ToList();

                    summaryStats = result.GroupBy(x => new { x.DeviceId, x.DeviceName, x.StatName })
                                         .Select(x => new SummaryDataModel
                                         {
                                             DeviceID = x.Key.DeviceId,
                                             DeviceName = x.Key.DeviceName,
                                             StatName = x.Key.StatName,
                                             Value = x.Sum(y => y.StatValue)
                                         }).ToList();
                }
                else
                {
                    var result = summaryStatisticsRepository.All().Where(x => x.DeviceType == "Sametime" && x.StatName != null && x.StatDate.HasValue && x.StatDate.Value.Month == statdate.Month && x.StatDate.Value.Year == statdate.Year).ToList();

                    summaryStats = result.GroupBy(x => new { x.DeviceId, x.DeviceName, x.StatName })
                                         .Select(x => new SummaryDataModel
                                         {
                                             DeviceID = x.Key.DeviceId,
                                             DeviceName = x.Key.DeviceName,
                                             StatName = x.Key.StatName,
                                             Value = x.Sum(y => y.StatValue)
                                         }).ToList();

                }
                var distinctData = summaryStats.Select(x => new { DeviceId = x.DeviceID, DeviceName = x.DeviceName }).Distinct().OrderBy(x => x.DeviceName).ToList();

                foreach (var item in distinctData)
                {
                    if (item != null)
                    {

                        SametimeStatisticsModel MaxConcurrentLogins = new SametimeStatisticsModel();
                        MaxConcurrentLogins.DeviceName = item.DeviceName;
                        MaxConcurrentLogins.StatName = "Peak Logins";
                        var peaklogin = summaryStats.FirstOrDefault(x => x.DeviceID == item.DeviceId && (x.StatName == "MaxConcurrentLogins" || x.StatName == "PeakLogins"));
                        if (peaklogin != null)
                        {
                            MaxConcurrentLogins.StatValue = peaklogin.Value;
                        }
                        else
                        {
                            MaxConcurrentLogins.StatValue = null;
                        }
                        SametimeStatisticsModel TotalTwoWayChats = new SametimeStatisticsModel();
                        TotalTwoWayChats.DeviceName = item.DeviceName;
                        TotalTwoWayChats.StatName = "Total 2-Way Chats";
                        var totaltwoway = summaryStats.FirstOrDefault(x => x.DeviceID == item.DeviceId && (x.StatName == "TotalTwoWayChats" || x.StatName == "Total2WayChats"));
                        if (totaltwoway != null)
                        {
                            TotalTwoWayChats.StatValue = totaltwoway.Value;
                        }
                        else
                        {
                            TotalTwoWayChats.StatValue = null;
                        }
                        SametimeStatisticsModel TotalNWChats = new SametimeStatisticsModel();
                        TotalNWChats.DeviceName = item.DeviceName;
                        TotalNWChats.StatName = "Total n-Way Chats";
                        var totalnchat = summaryStats.FirstOrDefault(x => x.DeviceID == item.DeviceId && (x.StatName == "TotalNWChats" || x.StatName == "TotalnWayChats"));
                        if (totalnchat != null)
                        {
                            TotalNWChats.StatValue = totalnchat.Value;
                        }
                        else
                        {
                            TotalNWChats.StatValue = null;
                        }
                        if (MaxConcurrentLogins.StatValue != null)
                        {
                            sametimeStatisticsData.Add(MaxConcurrentLogins);
                        }
                        if (TotalNWChats.StatValue != null)
                        {
                            sametimeStatisticsData.Add(TotalNWChats);
                        }
                        if (TotalTwoWayChats.StatValue != null)
                        {
                            sametimeStatisticsData.Add(TotalTwoWayChats);
                        }

                    }
                }
                Response = Common.CreateResponse(sametimeStatisticsData);
            }
            catch (Exception exception)
            {
                Response = Common.CreateResponse(null, Common.ResponseStatus.Error.ToDescription(), "Get sametime statistics falied .\n Error Message :" + exception.Message);
            }
            return Response;
        }

        [HttpGet("monthly_expenditure")]
        public APIResponse GetMonthlyExpenditure(int count = 5, string group_by = "")
        {
            List<dynamic> result = new List<dynamic>();
            List<Segment> segmentList = new List<Segment>();

            try
            {
                namevalueRepository = new Repository<NameValue>(ConnectionString);
                var currencySymbol = namevalueRepository.Collection.Aggregate()
                    .Match(y => y.Name == "Currency Symbol")
                    .ToList();

                locationRepository = new Repository<Location>(ConnectionString);
                serverRepository = new Repository<Server>(ConnectionString);
                var temp = serverRepository.Collection.Aggregate().ToList();
                if (group_by == "")
                {
                    var serverdata = temp.GroupBy(row => new
                    {
                        row.DeviceType
                    })
                    .Select(grp => new
                    {
                        Label = grp.Key.DeviceType,
                        Value = grp.Sum(y => y.MonthlyOperatingCost)
                    }).ToList();
                    foreach (var val in serverdata)
                    {
                        var x = new ExpandoObject() as IDictionary<string, Object>;
                        x.Add("device_type", val.Label);
                        x.Add("monthly_cost", val.Value);
                        result.Add(x);
                    }
                    result = result.OrderByDescending(x => x.monthly_cost).Take(count).ToList();
                    foreach (var doc in result)
                    {
                        Segment segment = new Segment()
                        {
                            Label = doc.device_type,
                            Value = Convert.ToDouble(doc.monthly_cost)
                        };
                        segmentList.Add(segment);
                    }
                }
                else if (group_by == "Location")
                {
                    var serverdata = temp.GroupBy(row => new
                    {
                        row.LocationId
                    })
                    .Select(grp => new
                    {
                        Label = grp.Key.LocationId,
                        Value = grp.Sum(y => y.MonthlyOperatingCost)
                    }).ToList();
                    foreach (var val in serverdata)
                    {
                        var x = new ExpandoObject() as IDictionary<string, Object>;
                        var locationData = locationRepository.Collection.Aggregate()
                                    .Match(y => y.Id == val.Label).ToList();
                        foreach (var value in locationData)
                        {
                            x.Add("location", value.LocationName);
                            x.Add("monthly_cost", val.Value);
                            result.Add(x);
                        }
                    }
                    result = result.OrderByDescending(x => x.monthly_cost).Take(count).ToList();
                    foreach (var doc in result)
                    {
                        Segment segment = new Segment()
                        {
                            Label = doc.location,
                            Value = Convert.ToDouble(doc.monthly_cost)
                        };
                        segmentList.Add(segment);
                    }
                }
                else if (group_by == "Category")
                {
                    var serverdata = temp.GroupBy(row => new
                    {
                        row.Category
                    })
                    .Select(grp => new
                    {
                        Label = grp.Key.Category,
                        Value = grp.Sum(y => y.MonthlyOperatingCost)
                    }).ToList();
                    foreach (var val in serverdata)
                    {
                        var x = new ExpandoObject() as IDictionary<string, Object>;
                        x.Add("category", val.Label);
                        x.Add("monthly_cost", val.Value);
                        result.Add(x);
                    }
                    result = result.OrderByDescending(x => x.monthly_cost).Take(count).ToList();
                    foreach (var doc in result)
                    {
                        Segment segment = new Segment()
                        {
                            Label = doc.category,
                            Value = Convert.ToDouble(doc.monthly_cost)
                        };
                        segmentList.Add(segment);
                    }
                }

                Serie serie = new Serie();
                serie.Title = "Monthly Expenditure";
                serie.Segments = segmentList;


                List<Serie> series = new List<Serie>();
                series.Add(serie);

                Chart chart = new Chart();
                chart.Title = "";
                chart.Series = series;
                if (currencySymbol.Count > 0)
                {
                    chart.YAxisTitle = currencySymbol[0].Value;
                }

                Response = Common.CreateResponse(chart);
            }
            catch (Exception exception)
            {
                Response = Common.CreateResponse(null, "Error", "Get monthly expenditure falied .\n Error Message :" + exception.Message);
            }
            return Response;
        }

        [HttpGet("device_utilization")]
        public APIResponse GetDeviceUtilization(string sort_type = "asc", int top_x = 0)
        {
            List<dynamic> result = new List<dynamic>();
            List<SummaryStatistics> summaryData = new List<SummaryStatistics>(); 
            List<Segment> segmentList = new List<Segment>();

            try
            {
                summaryStatisticsRepository = new Repository<SummaryStatistics>(ConnectionString);
                serverRepository = new Repository<Server>(ConnectionString);
                summaryData = summaryStatisticsRepository.Collection.Aggregate()
                    .Match(y => y.StatName == "Server.Users")
                    .ToList();
                var summary = summaryData.GroupBy(row => new
                {
                    row.DeviceId,
                    row.DeviceName
                })
                .Select(row => new {
                    DeviceId = row.Key.DeviceId,
                    DeviceName = row.Key.DeviceName,
                    StatValue = row.Average(x => x.StatValue)
                }).ToList();
                
                var serverData = serverRepository.Collection.Aggregate().ToList();
                foreach (var val in summary)
                {
                    var devices = serverData.Where(y => y.Id == val.DeviceId && y.DeviceName == val.DeviceName)
                        .Select(y => new
                        {
                            DeviceName = y.DeviceName,
                            UserCount = y.IdealUserCount == null ? 0 : y.IdealUserCount
                        }).ToList();
                    if (devices.Count > 0)
                    {
                        var x = new ExpandoObject() as IDictionary<string, Object>;
                        x.Add("device_name", val.DeviceName);
                        if (devices[0].UserCount != 0)
                        {
                            double usercount = Convert.ToDouble(devices[0].UserCount);
                            x.Add("device_expenditure", Math.Round(val.StatValue / usercount * 100, 2));
                        }
                        else
                        {
                            x.Add("device_expenditure", 0.0);
                        }
                        result.Add(x);
                    }     
                }

                if (sort_type == "asc")
                {
                    //var propertyInfo = result[0].GetType().GetProperty("device_expenditure");
                    result = result.OrderBy(x => x.device_expenditure).ToList();
                }
                else
                {
                    //var propertyInfo = result[0].GetType().GetProperty("device_expenditure");
                    result = result.OrderByDescending(x => x.device_expenditure).ToList();
                }
                if (top_x > 0)
                {
                    result = result.Take(top_x).ToList();
                }
                foreach (var doc in result)
                {
                    Segment segment = new Segment()
                    {
                        Label = doc.device_name,
                        Value = Convert.ToDouble(doc.device_expenditure)
                    };
                    segmentList.Add(segment);
                }
                Serie serie = new Serie();
                serie.Title = "Percent Utilization";
                serie.Segments = segmentList;


                List<Serie> series = new List<Serie>();
                series.Add(serie);
                
                Chart chart = new Chart();
                chart.Title = "";
                chart.Series = series;

                Response = Common.CreateResponse(chart);
            }
            catch (Exception exception)
            {
                Response = Common.CreateResponse(null, "Error", "Get device utilization falied .\n Error Message :" + exception.Message);
            }
            return Response;
        }
    }
}

        