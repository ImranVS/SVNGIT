using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using VitalSigns.API.Models;
using VitalSigns.API.Models.Charts;
using VSNext.Mongo.Repository;
using VSNext.Mongo.Entities;
using MongoDB.Driver;
using System.Linq.Expressions;
using System.Globalization;
using MongoDB.Bson;
using System.Dynamic;
using Microsoft.AspNet.Authorization;
using System.IdentityModel.Tokens.Jwt;

namespace VitalSigns.API.Controllers
{

    [Authorize("Bearer")]
    [Route("[controller]")]
    public class ServicesController : BaseController
    {

        private IRepository<Server> serverRepository;
        private IRepository<ServerOther> serverOtherRepository;
        private IRepository<Status> statusRepository;
        private IRepository<DailyStatistics> dailyRepository;
        private IRepository<SummaryStatistics> summaryRepository;
        private IRepository<NameValue> nameValueRepository;
        private IRepository<EventsDetected> eventsDetectedRepository;
        private IRepository<StatusDetails> statusDetailsRepository;
        private IRepository<Credentials> credentialsRepository;
        // private IRepository<DominoSettingsModel> dominoSettingsRepository;

        private string DateFormat = "yyyy-MM-ddTHH:mm:ss.fffK";

        //private IRepository<IbmConnectionsTopStats> ibmRepository;




        [HttpGet("dashboard_summary")]
        public APIResponse ServersStatusSummary()
        {
            try
            {
                List<string> statusCode = new List<string>();
                serverRepository = new Repository<Server>(ConnectionString);
                serverOtherRepository = new Repository<ServerOther>(ConnectionString);
                statusRepository = new Repository<Status>(ConnectionString);
                eventsDetectedRepository= new Repository<EventsDetected>(ConnectionString);
                var servers = serverRepository.Collection.AsQueryable().Where(x => x.IsEnabled == true)
                                                        .Select(x => new ServerStatus
                                                        {
                                                            Id = x.Id,
                                                            IsEnabled = x.IsEnabled,
                                                            Type = x.DeviceType,
                                                            Name = x.DeviceName,
                                                        }).OrderBy(x => x.Name).ToList();

                //same filter restrictions are in GetStatusSummaryByType, GetAllServerServices and ServerStatusSummary
                servers.AddRange(serverOtherRepository.Find(x => x.IsEnabled == true && x.Type == Enums.ServerType.NotesDatabase.ToDescription()).ToList()
                                                        .Select(x => new ServerStatus
                                                        {
                                                            Id = x.Id,
                                                            IsEnabled = x.IsEnabled,
                                                            Type = x.Type,
                                                            Name = x.Name
                                                        }));
                foreach (var server in servers)
                {
                    var serverStatus = statusRepository.Collection.AsQueryable().FirstOrDefault(x => x.DeviceId == server.Id);
                    if (serverStatus != null)
                    {
                        statusCode.Add(serverStatus.StatusCode);
                    }
                }
                var issue = statusCode.Where(item => item == "Issue").Count();
                var ok = statusCode.Where(item => item == "OK").Count();
                var notResponding = statusCode.Where(item => item == "Not Responding").Count();
                var maintenance = statusCode.Where(item => item == "Maintenance").Count();
                var systemMessages = eventsDetectedRepository.Collection.AsQueryable().Where(x => x.IsSystemMessage == true && x.EventDismissed==null && (x.IsSystemMessageDismissed == false || x.IsSystemMessageDismissed == null)).ToList(); ;

                Response = Common.CreateResponse(new { issue = issue, ok = ok, notResponding = notResponding, maintenance = maintenance, systemMessages= systemMessages==null?0: systemMessages.Count });
            }
            catch (Exception exception)
            {
                Response = Common.CreateResponse(null, "Error", exception.Message);

            }
            return Response;
        }


        [HttpGet("get_system_messages")]
        public APIResponse GetSystemMessages()
        {
            try
            {
                eventsDetectedRepository = new Repository<EventsDetected>(ConnectionString);
                var systemMessages = eventsDetectedRepository.Collection.AsQueryable().Where(x => x.IsSystemMessage == true && x.EventDismissed == null && (x.IsSystemMessageDismissed == false || x.IsSystemMessageDismissed == null))
                                                                                       .Select(x => new
                                                                                       {
                                                                                           CreatedDate = x.CreatedOn,
                                                                                           Details = x.Details
                                                                                       }).ToList();
                Response = Common.CreateResponse(systemMessages);

            }
            catch (Exception exception)
            {
                Response = Common.CreateResponse(null, "Error", exception.Message);

            }
            return Response;
        }

        [HttpGet("dismiss_system_messages")]
        public APIResponse DismissSystemMessages()
        {
            try
            {
                eventsDetectedRepository = new Repository<EventsDetected>(ConnectionString);
                FilterDefinition<EventsDetected> filterDefination = Builders<EventsDetected>.Filter.Where(x => x.IsSystemMessage == true && x.EventDismissed == null && (x.IsSystemMessageDismissed == false || x.IsSystemMessageDismissed == null));
                var updateDefination = eventsDetectedRepository.Updater.Set(p => p.IsSystemMessageDismissed, true);
                var result = eventsDetectedRepository.Update(filterDefination, updateDefination);
                Response = Common.CreateResponse(result);
            }
            catch (Exception exception)
            {
                Response = Common.CreateResponse(null, "Error", exception.Message);

            }
            return Response;
        }
        [HttpGet("status_summary_by_type")]
        public APIResponse GetStatusSummaryByType()
        {
            try
            {
                List<ServerStatus> statusData = new List<ServerStatus>();
                List<string> typeList = new List<string>();
            
                serverRepository = new Repository<Server>(ConnectionString);
                serverOtherRepository = new Repository<ServerOther>(ConnectionString);
                statusRepository = new Repository<Status>(ConnectionString);
                var deviceIds = serverRepository.Collection.AsQueryable().Where(x => x.IsEnabled == true)
                                                        .Select(x => x.Id).ToList();

                //same filter restrictions are in GetStatusSummaryByType, GetAllServerServices and ServerStatusSummary
                deviceIds.AddRange(serverOtherRepository.Find(x => x.IsEnabled == true && x.Type == Enums.ServerType.NotesDatabase.ToDescription()).Select(x => x.Id).ToList());

                foreach (var server in deviceIds)
                {
                    var serverStatus = statusRepository.Collection.AsQueryable().FirstOrDefault(x => x.DeviceId == server);
                    if (serverStatus != null)
                    {
                        statusData.Add(new ServerStatus { Type = serverStatus.DeviceType, StatusCode = serverStatus.StatusCode });
                        if(!typeList.Contains(serverStatus.DeviceType))
                             typeList.Add(serverStatus.DeviceType);
                    }
                }
              
                List<StatusSummary> summaryList = new List<StatusSummary>();
                foreach (string type in typeList)
                {
                    summaryList.Add(new StatusSummary
                    {
                        Type = type,
                        Ok = statusData.Where(x =>  x.Type == type && x.StatusCode == "OK").Count(),
                        NotResponding = statusData.Where(x => x.Type == type && x.StatusCode == "Not Responding").Count(),
                        Issue = statusData.Where(x => x.Type == type && x.StatusCode == "Issue").Count(),
                        Maintenance = statusData.Where(x => x.Type == type && x.StatusCode == "Maintenance").Count()
                    });
                }
                summaryList = summaryList.OrderBy(x => x.Type).ToList();
                Response = Common.CreateResponse(summaryList.Where(x => x.Type != null && x.Type != "Domino Cluster").ToList());
            }
            catch (Exception exception)
            {
                Response = Common.CreateResponse(null, "Error", exception.Message);

            }
            return Response;
        }
        [HttpGet("dashboard_stats")]
        public APIResponse GetDashboardStats()
        {
            statusRepository = new Repository<Status>(ConnectionString);
            var result = statusRepository.All()
                                        .Select(x => new
                                        {
                                            UserCount = x.UserCount,
                                            ResponseTime = x.ResponseTime,
                                            DownMinutes = x.DownMinutes,
                                            PendingMail = x.PendingMail,
                                            DeadMail = x.DeadMail,
                                            HeldMail = x.HeldMail

                                        }).ToList();
            int? userCount = result.Sum(x => x.UserCount);
            double? responseTime = result.Average(x => x.ResponseTime);
            double? downMinutes = result.Sum(x => x.DownMinutes);
            int? pendingMail = result.Sum(x => x.PendingMail);
            int? deadMail = result.Sum(x => x.DeadMail);
            int? heldMail = result.Sum(x => x.HeldMail);




            return Common.CreateResponse(new { user_count = userCount, response_time = responseTime, downMinutes = downMinutes, pendingMail = pendingMail, deadMail = deadMail, heldMail = heldMail });
        }
        /// <summary>
        /// Returns all servers details
        /// </summary>
        /// <author>Kiran Dadireddy</author>
        /// <returns>List of servers details</returns>
        [HttpGet("device_list")]
        public APIResponse GetAllServerServices(string module, string deviceType)
        {
            statusRepository = new Repository<Status>(ConnectionString);
            serverRepository = new Repository<Server>(ConnectionString);
            serverOtherRepository = new Repository<ServerOther>(ConnectionString);
            
            try
            {
                var serverFilterDef = serverRepository.Filter.Empty;
                var serverOtherFilterDef = serverOtherRepository.Filter.Empty;
                if (!string.IsNullOrWhiteSpace(deviceType))
                {
                    serverFilterDef = serverFilterDef & serverRepository.Filter.Eq(x => x.DeviceType, deviceType);
                    serverOtherFilterDef = serverOtherFilterDef & serverOtherRepository.Filter.Eq(x => x.Type, deviceType);
                }
                var serviceIcons = Common.GetServerTypeIcons();
                var servers = serverRepository.Find(serverFilterDef & serverRepository.Filter.Ne(x => x.DeviceType, Enums.ServerType.Office365.ToDescription())).AsQueryable()
                    .Select(x => new ServerStatus
                    {
                        Id = x.Id,
                        IsEnabled = x.IsEnabled,
                        Type = x.DeviceType,
                        Name = x.DeviceName,
                    }).OrderBy(x=>x.Name).ToList();

                if (module == "configurator")
                {
                    servers.AddRange(serverRepository.Find(serverFilterDef & serverRepository.Filter.Eq(x => x.DeviceType, Enums.ServerType.Office365.ToDescription())).AsQueryable()
                    .Select(x => new ServerStatus
                    {
                        Id = x.Id,
                        IsEnabled = x.IsEnabled,
                        Type = x.DeviceType,
                        Name = x.DeviceName,
                    }).OrderBy(x => x.Name).ToList());
                }
                else
                {
                    servers.AddRange(
                        serverRepository.Collection.Aggregate()
                            .Match(serverFilterDef & serverRepository.Filter.Eq(x => x.DeviceType, Enums.ServerType.Office365.ToDescription()))
                            .Unwind(x => x.NodeIds)
                            .Lookup("nodes", "node_ids", "_id", "location")
                            .ToList()
                            .Select(x => new ServerStatus
                            {
                                Id = x["_id"].ToString(),
                                IsEnabled = x["is_enabled"].AsBoolean,
                                Type = x["device_type"].ToString(),
                                Name = x["device_name"].ToString() + "-" + (x["location"][0].ToBsonDocument().Names.Contains("location") ? x["location"][0]["location"].ToString() : x["location"][0]["name"].ToString())
                            })
                            );
                }
                //l[0]["location"][0]["location_name"].ToString()


                //same filter restrictions are in GetStatusSummaryByType, GetAllServerServices and ServerStatusSummary
                var serverOthers = serverOtherRepository
                    .Find(serverOtherFilterDef & serverOtherRepository.Filter.Eq(x => x.Type, Enums.ServerType.NotesDatabase.ToDescription()))
                    .ToList()
                    .Select(x => new ServerStatus
                    {
                        Id = x.Id,
                        IsEnabled = x.IsEnabled,
                        Type = x.Type,
                        Name = x.Name,
                        ServerOther = true
                    });
                servers.AddRange(serverOthers);

                servers = servers.OrderBy(x => x.Name).ToList();

                foreach (var server in servers)
                {
                    var statusFilterDef = statusRepository.Filter.Eq(x => x.DeviceId, server.Id);
                    if (module == "dashboard")
                        statusFilterDef = statusFilterDef & statusRepository.Filter.Eq(x => x.DeviceName, server.Name);
                    var serverStatus = statusRepository.Find(statusFilterDef).AsQueryable().FirstOrDefault();
                    if (serverStatus != null)
                    {
                        server.Category = serverStatus.Category;
                        server.Country = serverStatus.Location;
                        server.Version = serverStatus.SoftwareVersion;
                        server.LastUpdated = serverStatus.LastUpdated;
                        server.Status = serverStatus.StatusCode;// Holds the formated status code for displaying colors in UI
                        server.StatusCode = serverStatus.StatusCode;//Holds actual server code data
                        server.StatusString = serverStatus.CurrentStatus;
                        server.Location = serverStatus.Location;
                        server.Details = serverStatus.Details;
                        if (server.LastUpdated.HasValue)
                            server.Description = "Last Updated: " + server.LastUpdated.Value.ToShortDateString();
                        else
                            server.Description = "Device Status not updated";

                    }
                    else
                    {
                        server.Description = "Device Status not updated";
                    }
                    
                    if (server.Type != null)
                    {
                        if (serviceIcons.ContainsKey(server.Type))
                            server.Icon = serviceIcons[server.Type];
                        else
                            server.Icon = @"/img/servers/Paintbrush.svg";
                    }
                    if (!string.IsNullOrEmpty(server.Status))
                        server.Status = server.Status.ToLower().Replace(" ", "");
                    else
                        server.Status = "notset";
                    if (string.IsNullOrEmpty(server.Type))
                        server.Type = string.Empty;
                    if (string.IsNullOrEmpty(server.StatusCode))
                        server.StatusCode = string.Empty;

                    if (string.IsNullOrEmpty(server.Location))
                        server.Location = string.Empty;

                }
                
                Response = Common.CreateResponse(servers.OrderBy(x=>x.Name));
            }
            catch (Exception exception)
            {
                Response = Common.CreateResponse(null, "Error", exception.Message);

            }
            return Response;
        }
        /// <summary>
        /// Returns server details by id
        /// </summary>
        /// <author>Kiran Dadireddy</author>
        /// <param name="id"></param>
        /// <returns>Server details</returns>
        [HttpGet("device_details")]
        public APIResponse GetServerDetails(string device_id, string destination, string secondaryRole, string deviceType)
        {

            statusRepository = new Repository<Status>(ConnectionString);
            ServerStatus serverStatus = new ServerStatus();
            serverRepository = new Repository<Server>(ConnectionString);
            serverOtherRepository = new Repository<ServerOther>(ConnectionString);

            try
            {
                
                if (!string.IsNullOrEmpty(device_id))
                {
                    string nodeName = null;
                    if (device_id.Contains(";"))
                    {
                        nodeName = device_id.Substring(device_id.IndexOf(';') + 1);
                        device_id = device_id.Substring(0, device_id.IndexOf(';'));
                    }
                    //Expression<Func<Status, bool>> expression = (p => p.DeviceId == device_id);
                    //var result = (statusRepository.Find(expression)
                    //                     .Select(x => new ServerStatus
                    //                     {
                    //                         Id = x.Id,
                    //                         Type = x.DeviceType,
                    //                         Country = x.Location,
                    //                         Name = x.DeviceName,
                    //                         Version = x.SoftwareVersion,
                    //                         LastUpdated = x.LastUpdated,
                    //                         Description = x.Description,
                    //                         Status = x.StatusCode,
                    //                         DeviceId = x.DeviceId,
                    //                         SecondaryRole = x.SecondaryRole
                    //                     })).FirstOrDefault();
                    var serviceIcons = Common.GetServerTypeIcons();
                    Server server = null;
                    try
                    {
                        server = serverRepository.Find(serverRepository.Filter.Eq(x => x.Id, device_id)).First();
                    }
                    catch (Exception ex)
                    {
                        //string s = (serverRepository.Filter.Eq(x => x.Id, device_id) & serverRepository.Filter.Regex(x => x.DeviceName, new BsonRegularExpression("-" + deviceLocation + "&"))).Render(serverRepository.Collection.DocumentSerializer, serverRepository.Collection.Settings.SerializerRegistry).ToString();
                    }

                    if (server == null)
                    {
                        try
                        {
                            //belongs to the server_other collection
                            var serverOtherInstance = (serverOtherRepository.Get(device_id));
                            server = new Server()
                            {
                                Id = serverOtherInstance.Id,
                                IsEnabled = serverOtherInstance.IsEnabled,
                                DeviceType = serverOtherInstance.Type,
                                DeviceName = serverOtherInstance.Name
                            };
                        }
                        catch (Exception ex)
                        {

                        }
                    }

                    if (server != null)
                    {
                        serverStatus.Id = server.Id;
                        serverStatus.IsEnabled = server.IsEnabled;
                        serverStatus.Type = server.DeviceType;
                        serverStatus.Name = server.DeviceName;

                        // string s = (statusRepository.Filter.Eq(x => x.DeviceId, device_id) &
                        //     ((nodeName != null) ? statusRepository.Filter.Eq(x => x.Category, nodeName) : null)).Render(statusRepository.Collection.DocumentSerializer, statusRepository.Collection.Settings.SerializerRegistry).ToString();

                        Status status = null;
                        try
                        {
                            status = statusRepository.Find(
                                statusRepository.Filter.Eq(x => x.DeviceId, device_id) &
                                ((nodeName != null) ? statusRepository.Filter.Eq(x => x.Category, nodeName) : statusRepository.Filter.Empty)).First();
                        }
                        catch (Exception ex)
                        { }

                        //var status = statusRepository.Collection.AsQueryable().FirstOrDefault(x => x.DeviceId == server.Id);
                        if (status != null)
                        {
                            serverStatus.Category = status.Category;
                            serverStatus.Country = status.Location;
                            serverStatus.Version = status.SoftwareVersion;
                            serverStatus.LastUpdated = status.LastUpdated;
                            serverStatus.Status = status.StatusCode;// Holds the formated status code for displaying colors in UI
                            serverStatus.StatusCode = status.StatusCode;//Holds actual server code data
                            serverStatus.Location = status.Location;
                            if (serverStatus.LastUpdated.HasValue)
                                serverStatus.Description = "Last Updated: " + serverStatus.LastUpdated.Value.ToShortDateString();
                            else
                                serverStatus.Description = "Device Status not updated";

                            serverStatus.Details = status.Details;
                            if (!string.IsNullOrEmpty(status.SecondaryRole))
                            {
                                serverStatus.SecondaryRole = status.SecondaryRole;
                            }
                        }
                        else
                        {
                            serverStatus.Description = "Device Status not updated";
                        }

                        if (serverStatus.Type != null)
                        {
                            if (serviceIcons.ContainsKey(serverStatus.Type))
                                serverStatus.Icon = serviceIcons[serverStatus.Type];
                            else
                                serverStatus.Icon = @"/img/servers/Paintbrush.svg";
                        }
                        if (!string.IsNullOrEmpty(serverStatus.Status))
                            serverStatus.Status = serverStatus.Status.ToLower().Replace(" ", "");
                        else
                            serverStatus.Status = string.Empty;
                        if (string.IsNullOrEmpty(serverStatus.Type))
                            serverStatus.Type = string.Empty;
                        if (string.IsNullOrEmpty(serverStatus.StatusCode))
                            serverStatus.StatusCode = string.Empty;

                        if (string.IsNullOrEmpty(serverStatus.Location))
                            serverStatus.Location = string.Empty;
                    }



                    Models.ServerTypeModel serverType = Common.GetServerTypeTabs(serverStatus.Type);

                    if (string.IsNullOrEmpty(serverStatus.SecondaryRole))
                        serverStatus.Tabs = serverType.Tabs.Where(x => x.Type.ToUpper() == destination.ToUpper() && x.SecondaryRole == null).ToList();
                    else
                    {
                        var secondaryRoles = serverStatus.SecondaryRole.Split(';').Select(x => x.Trim());
                        serverStatus.Tabs = serverType.Tabs.Where(x => x.Type.ToUpper() == destination.ToUpper() && (x.SecondaryRole == null || secondaryRoles.Contains(x.SecondaryRole))).ToList();
                    }



                    if (!string.IsNullOrEmpty(serverStatus.Status))
                        serverStatus.Status = serverStatus.Status.ToLower().Replace(" ", "");
                    Response = Common.CreateResponse(serverStatus);
                }
                else
                {
                    if (!string.IsNullOrEmpty(deviceType))
                    {
                        var result = (statusRepository.Find(x => x.DeviceType == deviceType).AsQueryable()
                                             .Select(x => new ServerStatus
                                             {
                                                 Id = x.Id,
                                                 Type = x.DeviceType,
                                                 Country = x.Location,
                                                 Name = x.DeviceName,
                                                 Version = x.SoftwareVersion,
                                                 LastUpdated = x.LastUpdated,
                                                 Description = x.Description,
                                                 Status = x.StatusCode,
                                                 DeviceId = x.DeviceId,
                                                 SecondaryRole = x.SecondaryRole,
                                                 Details = x.Details
                                             })).FirstOrDefault();
                        if (result == null)
                            result = new ServerStatus();
                        Models.ServerTypeModel serverType = Common.GetServerTypeTabs(deviceType);
                        result.Tabs = serverType.Tabs.Where(x => x.Type.ToUpper() == destination.ToUpper() && x.SecondaryRole == null).ToList();
                        Response = Common.CreateResponse(result);

                    }
                }
            }
            catch (Exception exception)
            {
                Response = Common.CreateResponse(null, "Error", exception.Message);

            }
            return Response;
        }


        /// <summary>
        /// Returns daily stats data by deviceid
        /// </summary>
        /// <author>Swathi Dongari</author>
        /// <param name="id"></param>
        /// <returns> daily stats data </returns>
        //[HttpGet("statistics")]
        //public APIResponse GetDailyStat(string deviceId, string statName, string operation)
        //{

        //    dailyRepository = new Repository<DailyStatistics>(ConnectionString);

        //    try
        //    {
        //        var statNames = statName.Replace("[", "").Replace("]", "").Replace(" ", "").Split(',');
        //        if (string.IsNullOrEmpty(deviceId) && !string.IsNullOrEmpty(statName))
        //        {


        //            Expression<Func<DailyStatistics, bool>> expression = (p => statNames.Contains(p.StatName));
        //            var result = dailyRepository.Find(expression).Select(x => new StatsData
        //            {
        //                DeviceId = x.DeviceId,
        //                StatName = x.StatName,
        //                StatValue = x.StatValue

        //            }).OrderBy(x => x.StatName).ToList();


        //            Response = Common.CreateResponse(result);

        //        }
        //        else
        //        {
        //            Expression<Func<DailyStatistics, bool>> expression = (p => statNames.Contains(p.StatName) && p.DeviceId == deviceId);

        //            if (string.IsNullOrEmpty(operation) && !string.IsNullOrEmpty(statName))
        //            {



        //                var result = dailyRepository.Find(expression).Select(x => new StatsData
        //                {
        //                    DeviceId = x.DeviceId,
        //                    StatName = x.StatName,
        //                    StatValue = x.StatValue

        //                }).OrderBy(x => x.StatName).ToList();

        //                Response = Common.CreateResponse(result);

        //                return Response;
        //            }
        //            else if (!string.IsNullOrEmpty(operation) && !string.IsNullOrEmpty(statName))
        //            {

        //                // StatsData statsData;


        //                switch (operation.ToUpper())
        //                {
        //                    case "SUM":

        //                        var statsSum = dailyRepository.Find(expression);

        //                        var statsSumData = statsSum
        //                                      .GroupBy(row => row.StatName)
        //                                      .Select(grp => new
        //                                      {
        //                                          StatName = grp.Key,
        //                                          Value = grp.Sum(x => x.StatValue),
        //                                          DeviceId = deviceId
        //                                      }).ToList();

        //                        // statsData = new StatsData { StatName = statName, StatValue = statsSum, DeviceId = deviceId };
        //                        Response = Common.CreateResponse(statsSumData);

        //                        break;

        //                    case "AVG":

        //                        var statsAvg = dailyRepository.Find(expression);

        //                        var statsAvgData = statsAvg
        //                                  .GroupBy(row => row.StatName)
        //                                  .Select(grp => new
        //                                  {
        //                                      StatName = grp.Key,
        //                                      Value = grp.Average(x => x.StatValue),
        //                                      DeviceId = deviceId
        //                                  }).ToList();
        //                        Response = Common.CreateResponse(statsAvgData);
        //                        break;
        //                    case "COUNT":
        //                        var statsCount = dailyRepository.Find(expression);

        //                        var statsCountData = statsCount
        //                               .GroupBy(row => row.StatName)
        //                               .Select(grp => new
        //                               {
        //                                   StatName = grp.Key,
        //                                   Value = grp.Count(),
        //                                   DeviceId = deviceId
        //                               }).ToList();
        //                        Response = Common.CreateResponse(statsCountData);
        //                        break;
        //                    case "HOURLY":
        //                        var statsHourly = dailyRepository.Find(expression);


        //                        var result = statsHourly
        //                               .GroupBy(row => new
        //                               {
        //                                   row.CreatedOn.Hour,
        //                                   row.StatName

        //                               })
        //                               .Select(row => new
        //                               {
        //                                   Hour = row.Key.Hour,
        //                                   Value = Math.Round(row.Average(x => x.StatValue), 2),
        //                                   StatName = row.Key.StatName

        //                               }).ToList();






        //                        // DateTime moment = DateTime.Now.Hour;
        //                        // int onhour = moment.Hour;
        //                        List<Serie> series = new List<Serie>();

        //                        foreach (var name in statNames)
        //                        {

        //                            List<Segment> segments = new List<Segment>();
        //                            Serie serie = new Serie();
        //                            for (int hour = 1; hour <= 24; hour++)
        //                            {
        //                                // To do
        //                                // string hourString =hour<12?hour.ToString()+ " A.M " 

        //                                var item = result.Where(x => x.Hour == hour).FirstOrDefault();
        //                                var output = result.Where(x => x.Hour == hour && x.StatName == name).ToList();
        //                                DateTime time = new DateTime();
        //                                time = DateTime.Now.AddHours(-hour);
        //                                time = time.AddMinutes(-1 * time.Minute);
        //                                string displayTime = "";
        //                                displayTime = time.ToString("hh:mm tt");


        //                                if (item != null && statNames.Length == 1)
        //                                {

        //                                    segments.Add(new Segment { Label = displayTime.ToString(), Value = item.Value });
        //                                    serie.Title = name;
        //                                    serie.Segments = segments;
        //                                }
        //                                else if (item != null && output != null && statNames.Length > 1)
        //                                {
        //                                    foreach (var statvalue in output)
        //                                    {
        //                                        segments.Add(new Segment { Label = displayTime, Value = statvalue.Value });
        //                                        serie.Title = name;
        //                                        serie.Segments = segments;
        //                                    }

        //                                }
        //                                else
        //                                {

        //                                    // TimeSpan timespan = new TimeSpan(hour);

        //                                    segments.Add(new Segment { Label = displayTime.ToString(), Value = 0 });
        //                                    serie.Title = name;
        //                                    serie.Segments = segments;

        //                                }





        //                            }
        //                            series.Add(serie);
        //                        }

        //                        Chart chart = new Chart();
        //                        chart.Title = statName;
        //                        chart.Series = series;



        //                        Response = Common.CreateResponse(chart);
        //                        break;
        //                }


        //            }
        //        }
        //        return Response;



        //    }
        //    catch (Exception exception)
        //    {
        //        Response = Common.CreateResponse(null, "Error", exception.Message);

        //        return Response;
        //    }
        //}



        [HttpGet("statistics")]
        public APIResponse GetDailyStat(string deviceId, string statName, string operation, bool isChart = false, bool getNode = false)
        {
            UtilsController uc = new UtilsController();
            dailyRepository = new Repository<DailyStatistics>(ConnectionString);
            statusRepository = new Repository<Status>(ConnectionString);
            DateTime startDate;
            DateTime endDate;
            try
            {
                endDate = DateTime.UtcNow.Date.AddHours(DateTime.UtcNow.Hour).ToUniversalTime();
                startDate = endDate.AddDays(-1).ToUniversalTime();

                var statNames = statName.Replace("[", "").Replace("]", "").Replace(" ", "").Split(',');
                if (!string.IsNullOrEmpty(statName) && getNode)
                {
                    Expression<Func<Status, bool>> expressionstatus = (p => p.DeviceId == deviceId);
                    var resultstatus = statusRepository.Find(expressionstatus).ToList();
                    if (resultstatus.Count > 0)
                    {
                        statNames = statName.Replace("[", "").Replace("]", "").Replace(" ", "").Replace("null", resultstatus[0].Category).Split(',');
                    }
                }
                if (string.IsNullOrEmpty(deviceId) && !string.IsNullOrEmpty(statName))
                {
                    Expression<Func<DailyStatistics, bool>> expression = (p => statNames.Contains(p.StatName) && p.CreatedOn >= startDate);

                    if (string.IsNullOrEmpty(operation))
                    {
                        var result = dailyRepository.Find(expression).Select(x => new StatsData
                        {
                            DeviceId = x.DeviceId,
                            StatName = x.StatName,
                            StatValue = x.StatValue

                        }).OrderBy(x => x.StatName).ToList();


                        Response = Common.CreateResponse(result);
                    }
                    else
                    {
                        List<Serie> series = new List<Serie>();
                        List<Segment> segments = new List<Segment>();
                        Serie serie = new Serie();
                        Chart chart = new Chart();
                        switch (operation.ToUpper())
                        {
                            case "SUM":

                                var statsSum = dailyRepository.Find(expression).OrderBy(i => i.CreatedOn);
                                var statsSumData = statsSum
                                              .GroupBy(row => row.StatName)
                                              .Select(grp => new
                                              {
                                                  StatName = uc.GetUserFriendlyStatName(grp.Key),
                                                  Value = grp.Sum(x => x.StatValue),
                                                  DeviceId = deviceId
                                              }).ToList();
                                Response = Common.CreateResponse(statsSumData);
                                break;

                            case "AVG":

                                var statsAvg = dailyRepository.Find(expression).OrderBy(i => i.CreatedOn);

                                if (isChart)
                                {
                                    var statsAvgData = statsAvg
                                          .GroupBy(row => row.DeviceName)
                                          .Select(grp => new
                                          {
                                              Label = uc.GetUserFriendlyStatName(grp.Key),
                                              Value = Math.Round(grp.Average(x => x.StatValue), 1)
                                          }).ToList();
                                    foreach (var item in statsAvgData)
                                    {
                                        segments.Add(new Segment()
                                        {
                                            Label = item.Label,
                                            Value = item.Value
                                        });
                                    }

                                    serie = new Serie();
                                    serie.Title = "test";
                                    serie.Segments = segments;
                                    series.Add(serie);

                                    chart = new Chart();
                                    chart.Title = "";
                                    chart.Series = series;
                                    Response = Common.CreateResponse(chart);
                                }
                                else
                                {
                                    var statsAvgData = statsAvg
                                          .GroupBy(row => row.DeviceName)
                                          .Select(grp => new
                                          {
                                              StatName = uc.GetUserFriendlyStatName(grp.Key),
                                              Value = grp.Sum(x => x.StatValue),
                                              DeviceId = deviceId
                                          }).ToList();
                                    Response = Common.CreateResponse(statsAvgData);
                                }
                                break;

                            case "COUNT":

                                var statsCount = dailyRepository.Find(expression).OrderBy(i => i.CreatedOn);
                                var statsCountData = statsCount
                                       .GroupBy(row => row.StatName)
                                       .Select(grp => new
                                       {
                                           StatName = uc.GetUserFriendlyStatName(grp.Key),
                                           Value = grp.Count(),
                                           DeviceId = deviceId
                                       }).ToList();
                                Response = Common.CreateResponse(statsCountData);
                                break;

                            case "HOURLY":

                                var statsHourly = dailyRepository.Find(expression).OrderBy(i => i.CreatedOn);
                                var result = statsHourly
                                       .GroupBy(row => new
                                       {
                                           row.CreatedOn.Hour,
                                           row.CreatedOn.Year,
                                           row.CreatedOn.Month,
                                           row.CreatedOn.Day,
                                           row.StatName
                                       })
                                       .Select(row => new
                                       {
                                           Hour = row.Key.Hour,
                                           Year = row.Key.Year,
                                           Month = row.Key.Month,
                                           Day = row.Key.Day,
                                           Value = Math.Round(row.Average(x => x.StatValue), 2),
                                           StatName = row.Key.StatName

                                       }).ToList();

                                series = new List<Serie>();
                                DateTime time = new DateTime();
                                foreach (var name in statNames)
                                {
                                    serie = new Serie();
                                    serie.Title = uc.GetUserFriendlyStatName(name);
                                    serie.Segments = new List<Segment>();

                                    for (DateTime date = startDate; date < endDate; date = date.AddHours(1))
                                    {
                                        var item = result.Where(x => x.Year == date.Year && x.Month == date.Month && x.Day == date.Day && x.Hour == date.Hour && x.StatName == name).ToList();
                                        time = item.Count > 0 ? new DateTime(item[0].Year, item[0].Month, item[0].Day, item[0].Hour, 0, 0, DateTimeKind.Utc) :
                                            new DateTime(date.Year, date.Month, date.Day, date.Hour, 0, 0, DateTimeKind.Utc);
                                        serie.Segments.Add(new Segment() { Label = time.ToString(DateFormat), Value = item.Count > 0 ? (double?)Math.Round(item[0].Value, 2) : 0 });
                                    }
                                    series.Add(serie);
                                }
                                chart = new Chart();
                                chart.Title = uc.GetUserFriendlyStatName(statName);
                                chart.Series = series;
                                Response = Common.CreateResponse(chart);
                                break;
                        }
                    }
                }
                else
                {
                    Expression<Func<DailyStatistics, bool>> expression = (p => statNames.Contains(p.StatName) && p.DeviceId == deviceId && p.CreatedOn >= startDate);

                    if (string.IsNullOrEmpty(operation) && !string.IsNullOrEmpty(statName))
                    {

                        var result = dailyRepository.Find(expression).OrderBy(i => i.CreatedOn)
                            .Select(x => new StatsData
                            {
                                DeviceId = x.DeviceId,
                                StatName = x.StatName,
                                StatValue = x.StatValue

                            }).OrderBy(x => x.StatName).ToList();

                        Response = Common.CreateResponse(result);

                        return Response;
                    }
                    else if (!string.IsNullOrEmpty(operation) && !string.IsNullOrEmpty(statName))
                    {
                        // StatsData statsData;
                        switch (operation.ToUpper())
                        {
                            case "SUM":

                                var statsSum = dailyRepository.Find(expression).OrderBy(i => i.CreatedOn);

                                var statsSumData = statsSum
                                              .GroupBy(row => row.StatName)
                                              .Select(grp => new
                                              {
                                                  StatName = uc.GetUserFriendlyStatName(grp.Key),
                                                  Value = grp.Sum(x => x.StatValue),
                                                  DeviceId = deviceId
                                              }).ToList();

                                // statsData = new StatsData { StatName = statName, StatValue = statsSum, DeviceId = deviceId };
                                Response = Common.CreateResponse(statsSumData);

                                break;

                            case "AVG":

                                var statsAvg = dailyRepository.Find(expression).OrderBy(i => i.CreatedOn);

                                var statsAvgData = statsAvg
                                          .GroupBy(row => row.StatName)
                                          .Select(grp => new
                                          {
                                              StatName = uc.GetUserFriendlyStatName(grp.Key),
                                              Value = Math.Round(grp.Average(x => x.StatValue), 1),
                                              DeviceId = deviceId
                                          }).ToList();
                                Response = Common.CreateResponse(statsAvgData);
                                break;
                            case "COUNT":
                                var statsCount = dailyRepository.Find(expression).OrderBy(i => i.CreatedOn);

                                var statsCountData = statsCount
                                       .GroupBy(row => row.StatName)
                                       .Select(grp => new
                                       {
                                           StatName = uc.GetUserFriendlyStatName(grp.Key),
                                           Value = grp.Count(),
                                           DeviceId = deviceId
                                       }).ToList();
                                Response = Common.CreateResponse(statsCountData);
                                break;
                            case "HOURLY":
                                var statsHourly = dailyRepository.Find(expression).OrderBy(i => i.CreatedOn);
                                var result = statsHourly
                                       .GroupBy(row => new
                                       {
                                           row.CreatedOn.Hour,
                                           row.CreatedOn.Year,
                                           row.CreatedOn.Month,
                                           row.CreatedOn.Day,
                                           row.StatName

                                       })
                                       .Select(row => new
                                       {
                                           Hour = row.Key.Hour,
                                           Year = row.Key.Year,
                                           Month = row.Key.Month,
                                           Day = row.Key.Day,
                                           Value = Math.Round(row.Average(x => x.StatValue), 2),
                                           StatName = row.Key.StatName
                                       }).ToList();

                                List<Serie> series = new List<Serie>();
                                DateTime time = new DateTime();
                                foreach (var name in statNames)
                                {
                                    Serie serie = new Serie();
                                    serie.Title = uc.GetUserFriendlyStatName(name);
                                    serie.Segments = new List<Segment>();

                                    for (DateTime date = startDate; date < endDate; date = date.AddHours(1))
                                    {
                                        var item = result.Where(x => x.Year == date.Year && x.Month == date.Month && x.Day == date.Day && x.Hour == date.Hour && x.StatName == name).ToList();
                                        time = item.Count > 0 ? new DateTime(item[0].Year, item[0].Month, item[0].Day, item[0].Hour, 0, 0, DateTimeKind.Utc) :
                                            new DateTime(date.Year, date.Month, date.Day, date.Hour, 0, 0, DateTimeKind.Utc);
                                        serie.Segments.Add(new Segment() { Label = time.ToString(DateFormat), Value = item.Count > 0 ? (double?)Math.Round(item[0].Value, 2) : 0 });
                                    }
                                    series.Add(serie);
                                }
                                Chart chart = new Chart();
                                chart.Title = uc.GetUserFriendlyStatName(statName);
                                chart.Series = series;
                                Response = Common.CreateResponse(chart);
                                break;
                        }
                    }
                }
                return Response;
            }
            catch (Exception exception)
            {
                Response = Common.CreateResponse(null, "Error", exception.Message);

                return Response;
            }
        }

        /// <summary>
        /// Returns summary stats data by deviceid
        /// </summary>
        /// <author>Swathi Dongari</author>
        /// <param name="id"></param>
        /// <returns> summary stats data </returns>
        [HttpGet("summarystats")]
        public APIResponse GetSummaryStat(string deviceId, string statName, string seriesTitle = "", string startDate = "", string endDate = "", bool isChart = true, string regex = "", bool getNode = false, bool includeLastDay = true)
        {
            statusRepository = new Repository<Status>(ConnectionString);
            UtilsController uc = new UtilsController();
            //DateFormat is YYYY-MM-DD
            if (startDate == "")
                startDate = DateTime.Now.Date.ToUniversalTime().AddDays(-7).ToString(DateFormat);

            if (endDate == "")
                endDate = DateTime.Now.Date.ToUniversalTime().ToString(DateFormat);

            //1 day is added to the end so we include that days data
            //NS - removed adding one day since the summary collection is always 1 day behind
            DateTime dtStart = DateTime.ParseExact(startDate, DateFormat, CultureInfo.InvariantCulture).ToUniversalTime();
            DateTime dtEnd = DateTime.ParseExact(endDate, DateFormat, CultureInfo.InvariantCulture).ToUniversalTime();
            //if (dtStart.CompareTo(dtEnd) == 0)
                dtEnd = dtEnd.AddDays(1);

            summaryRepository = new Repository<SummaryStatistics>(ConnectionString);
            var statNames = statName.Replace("[", "").Replace("]", "").Replace(" ", "").Split(',');
            if (!string.IsNullOrEmpty(statName) && getNode)
            {
                Expression<Func<Status, bool>> expressionstatus = (p => p.DeviceId == deviceId);
                var resultstatus = statusRepository.Find(expressionstatus).ToList();
                if (resultstatus.Count > 0)
                {
                    statNames = statName.Replace("[", "").Replace("]", "").Replace(" ", "").Replace("null", resultstatus[0].Category).Split(',');
                }
            }
            try
            {
                FilterDefinition<SummaryStatistics> filterDefTemp;
                FilterDefinition<SummaryStatistics> filterDef = summaryRepository.Filter.Gte(p => p.StatDate, dtStart) &
                    summaryRepository.Filter.Lte(p => p.StatDate, dtEnd);
                if (regex != "")
                    filterDef = filterDef & summaryRepository.Filter.Regex(x => x.StatName, new BsonRegularExpression(regex, "i"));

                if (!string.IsNullOrEmpty(statName) && isChart == false)
                {
                    if (string.IsNullOrEmpty(deviceId))
                    {
                        filterDefTemp = filterDef &
                        summaryRepository.Filter.And(summaryRepository.Filter.In(p => p.StatName, statNames.Where(i => !(i.Contains("*")))),
                                                     summaryRepository.Filter.Ne(p => p.DeviceName, null));
                    }
                    else
                    {
                        filterDefTemp = filterDef &
                        summaryRepository.Filter.And(summaryRepository.Filter.In(p => p.StatName, statNames.Where(i => !(i.Contains("*")))),
                                                     summaryRepository.Filter.Ne(p => p.DeviceName, null),
                                                     summaryRepository.Filter.Eq(p => p.DeviceId, deviceId));

                    }

                    var result = summaryRepository.Find(filterDefTemp).Select(x => new StatsData
                    {
                        DeviceId = x.DeviceId,
                        DeviceName = x.DeviceName,
                        StatName = uc.GetUserFriendlyStatName(x.StatName),
                        StatValue = x.StatValue

                    }).OrderBy(x => x.StatName).ToList();             //.Take(500).ToList(); - commented out by NS

                    foreach (string currString in statNames.Where(i => i.Contains("*")))
                    {
                        filterDefTemp = filterDef &
                            summaryRepository.Filter.Regex(p => p.StatName, new BsonRegularExpression(currString.Replace("*", ".*"), "i"));

                        if (!string.IsNullOrEmpty(deviceId))
                        {
                            filterDefTemp = filterDefTemp & summaryRepository.Filter.Eq(p => p.DeviceId, deviceId);
                        }


                        result.AddRange(
                            summaryRepository.Find(filterDefTemp).Select(x => new StatsData
                            {
                                DeviceId = x.DeviceId,
                                DeviceName = x.DeviceName,
                                StatName = uc.GetUserFriendlyStatName(x.StatName),
                                StatValue = x.StatValue

                            }).OrderBy(x => x.StatName).ToList()                 //.Take(500).ToList() - commented out by NS
                        );

                    }
                    Response = Common.CreateResponse(result);

                }
                else if (!string.IsNullOrEmpty(statName))
                {
                    if (!string.IsNullOrEmpty(deviceId))
                    {
                        filterDef = filterDef &
                        summaryRepository.Filter.Eq(p => p.DeviceId, deviceId);
                    }
                    filterDefTemp = filterDef &
                        summaryRepository.Filter.In(p => p.StatName, statNames.Where(i => !(i.Contains("*"))));


                    var statsHourly = summaryRepository.Find(filterDefTemp);

                    var result = statsHourly
                                       .GroupBy(row => new
                                       {
                                           row.StatDate.Value.Date,
                                           row.StatName,
                                           row.DeviceName

                                       })
                                       .Select(row => new
                                       {
                                           Date = row.Key.Date,
                                           Value = Math.Round(row.Average(x => x.StatValue), 2),
                                           StatName = row.Key.StatName,
                                           DeviceName = row.Key.DeviceName

                                       }).ToList();

                    foreach (string currString in statNames.Where(i => i.Contains("*")))
                    {
                        filterDefTemp = filterDef & 
                            summaryRepository.Filter.Regex(p => p.StatName, new BsonRegularExpression(currString.Replace("*", ".*"), "i"));

                        result.AddRange(
                            summaryRepository.Find(filterDefTemp)
                            .GroupBy(row => new
                            {
                                row.StatDate.Value.Date,
                                row.StatName,
                                row.DeviceName
                            })
                            .Select(row => new
                            {
                                Date = row.Key.Date,
                                Value = Math.Round(row.Average(x => x.StatValue), 2),
                                StatName = row.Key.StatName,
                                DeviceName = row.Key.DeviceName

                            }).ToList()                               //.Take(500).ToList() - commented out by NS
                        );
                    }

                    List<Serie> series = new List<Serie>();

                    if ((dtEnd - dtStart).TotalDays <= 1)
                    {
                        var segments = new List<Segment>();

                        foreach (var item in result)
                        {
                            segments.Add(new Segment()
                            {
                                Label = uc.GetUserFriendlyStatName(item.StatName),
                                Value = item.Value
                            });
                        }

                        Serie serie = new Serie();
                        serie.Title = dtStart.ToString(DateFormat);
                        serie.Segments = segments;

                        series.Add(serie);
                        
                    }
                    else
                    {
                        foreach (var name in result.Select(i => i.StatName).Distinct())
                        {

                            List<Segment> segments = new List<Segment>();
                            Serie serie = new Serie();
                            var devicename = result.Where(x => x.StatName == name.ToString()).ToList();

                            //WS changed to just less then end date due to the end date being the next day to include all of the previous day values.
                            for (DateTime date = dtStart; date <= dtEnd.AddDays(-1); date = date.AddDays(1))
                            {
                                if (!includeLastDay && date.AddDays(1) > dtEnd.AddDays(-1))
                                    continue;

                                var item = result.Where(x => x.Date.Date == date.Date && x.StatName == name.ToString()).FirstOrDefault();
                                var output = result.Where(x => x.Date.Date == date.Date && x.StatName == name.ToString()).ToList();
                                
                                string statdate = date.ToString(DateFormat);
                                if (item != null && statNames.Length == 1)
                                {
                                    segments.Add(new Segment { Label = statdate.ToString(), Value = item.Value });
                                    if (string.IsNullOrEmpty(seriesTitle))
                                    {
                                        serie.Title = uc.GetUserFriendlyStatName(name.ToString()); 
                                    }
                                    else
                                    {
                                        serie.Title = devicename[0].DeviceName;
                                    }
                                    serie.Segments = segments;
                                }
                                else if (item != null && output != null && statNames.Length > 1)
                                {
                                    foreach (var statvalue in output)
                                    {
                                        segments.Add(new Segment { Label = statdate, Value = statvalue.Value });
                                        if (string.IsNullOrEmpty(seriesTitle))
                                        {
                                            serie.Title = uc.GetUserFriendlyStatName(name.ToString());
                                        }
                                        else
                                        {
                                            serie.Title = statvalue.DeviceName;
                                        }
                                        serie.Segments = segments;
                                    }
                                }
                                else
                                {
                                    segments.Add(new Segment { Label = statdate.ToString(), Value = 0 });
                                    if (string.IsNullOrEmpty(seriesTitle))
                                    {
                                        serie.Title = uc.GetUserFriendlyStatName(name.ToString());
                                    }
                                    else
                                    {
                                        serie.Title = devicename[0].DeviceName;
                                    }
                                    serie.Segments = segments;
                                }
                                
                            }
                            series.Add(serie);

                        }
                    }


                    Chart chart = new Chart();
                    chart.Title = uc.GetUserFriendlyStatName(statName);
                    chart.Series = series;
                    Response = Common.CreateResponse(chart);

                }
                return Response;

            }
            catch (Exception exception)
            {
                Response = Common.CreateResponse(null, "Error", exception.Message);

                return Response;
            }
        }

        //[HttpGet("top_tags")]
        //public APIResponse GetIbmConnection(string deviceId)
        //{
        //    ibmRepository = new Repository<IbmConnectionsTopStats>(ConnectionString);
        //    try
        //    {
        //        if (!string.IsNullOrEmpty(deviceId))
        //        {

        //            Expression<Func<IbmConnectionsTopStats, bool>> expression = (p => p.DeviceId == deviceId);
        //            var topTags = ibmRepository.Find(expression);
        //            var result = topTags
        //                              .GroupBy(row => row.Name)
        //                              .Select(grp => new Segment
        //                              {
        //                                  Label = grp.Key,
        //                                  Value = grp.Count(),

        //                              }).ToList();
        //            List<Segment> segments = new List<Segment>();
        //            Serie serie = new Serie();
        //            //serie.Title = name;
        //            serie.Segments = result.OrderBy(x => x.Value).Take(5).ToList();

        //            List<Serie> series = new List<Serie>();
        //            series.Add(serie);

        //            Chart chart = new Chart();
        //            // chart.Title = name;
        //            chart.Series = series;
        //            Response = Common.CreateResponse(chart);

        //        }
        //        return Response;
        //    }
        //    catch (Exception exception)
        //    {
        //        Response = Common.CreateResponse(null, "Error", exception.Message);

        //        return Response;
        //    }
        //}


        [HttpGet("status_list")]
        public APIResponse GetStatusList(string type, string docfield = "", string sortby = "", bool isChart = false,string deviceId="")
        {
            statusRepository = new Repository<Status>(ConnectionString);
            List<Status> statslist = null;
            Expression<Func<Status, bool>> expression;
            List<dynamic> result = new List<dynamic>();
            List<Segment> segments = new List<Segment>();
            List<Status> list = null;
            List<StatusDetails> statusdetails = null;
            List<dynamic> tests = new List<dynamic>();
            List<string> headerList = new List<string>();
            List<string> dataList = new List<string>();
            Segment segment = new Segment();
            FilterDefinition<Status> FilterdefStatus = statusRepository.Filter.Where(x => true);
            if (deviceId != "")
            {
                FilterdefStatus = statusRepository.Filter.Eq(x => x.DeviceId,deviceId);
            }
            try
            {
                if (string.IsNullOrEmpty(type))
                {
                    list = statusRepository.Find(FilterdefStatus).AsQueryable().OrderBy(x => x.DeviceName).ToList();

                    foreach (Status status in list)
                    {
                        var x = new ExpandoObject() as IDictionary<string, Object>;
                        foreach (var field in status.ToBsonDocument())
                        {
                            x.Add(field.Name, field.Value.ToString());
                        }
                        result.Add(x);
                    }
                    Response = Common.CreateResponse(result);
                }
                else if (!string.IsNullOrEmpty(type))
                {
                    if (string.IsNullOrEmpty(docfield))
                    {
                        if (type == "Traveler")
                        {
                            FilterDefinition<Status> filterDef = statusRepository.Filter.Exists(p => p.TravelerStatus);
                            list = statusRepository.Find(filterDef & FilterdefStatus).AsQueryable().OrderBy(x => x.DeviceName).ToList();
                        }
                        else
                        {
                            expression = (p => p.DeviceType == type);
                            list = statusRepository.Find(expression & FilterdefStatus).AsQueryable().OrderBy(x => x.DeviceName).ToList();
                        }
                        
                        // Get test information from the status_details collection
                        if (type == "IBM Connections" || type == "Office365")
                        {
                           
                            statusDetailsRepository = new Repository<StatusDetails>(ConnectionString);
                            FilterDefinition<StatusDetails> filterDefDetails = statusDetailsRepository.Filter.Eq(p => p.Type, type);
                            if (deviceId != "")
                            {
                                filterDefDetails = filterDefDetails & statusDetailsRepository.Filter.Eq(x => x.DeviceId, deviceId);
                            }
                            statusdetails = statusDetailsRepository.Find(filterDefDetails).ToList();

                        }

                        if (type != "IBM Connections" && type != "Office365")
                        {
                            foreach (Status status in list)
                            { 
                                var x = new ExpandoObject() as IDictionary<string, Object>;
                                foreach (var field in status.ToBsonDocument())
                                {
                                    x.Add(field.Name, field.Value.ToString());
                                }
                                result.Add(x);
                            }
                        }
                        else
                        {
                            foreach (Status status in list)
                            {
                                var x = new ExpandoObject() as IDictionary<string, Object>;
                                foreach (var field in status.ToBsonDocument())
                                {
                                    x.Add(field.Name, field.Value.ToString());
                                }
                                headerList = statusdetails.Where(n => n.DeviceId == status.DeviceId && (type == "Office365" && n.NodeName == status.Category))
                                    .Select(p => p.TestName).ToList();
                                if (headerList.Count > 0)
                                {
                                    x.Add("headerText", headerList);
                                }
                                dataList = statusdetails.Where(n => n.DeviceId == status.DeviceId && (type == "Office365" && n.NodeName == status.Category))
                                    .Select(p => p.Result).ToList();
                                if (dataList.Count > 0)
                                {
                                    x.Add("dataKey", dataList);
                                }
                                result.Add(x);
                            }
                            
                        }

                        Response = Common.CreateResponse(result);
                    }
                    else
                    {
                        if (type == "Traveler")
                        {
                            FilterDefinition<Status> filterDef = statusRepository.Filter.Exists(p => p.TravelerStatus);
                            statslist = statusRepository.Find(filterDef & FilterdefStatus).AsQueryable().OrderBy(x => x.DeviceName).ToList();
                        }
                        else
                        {
                            FilterDefinition<Status> filterdef = statusRepository.Filter.Eq(x => x.DeviceType, type);
                            statslist = statusRepository.Collection.Find(filterdef & FilterdefStatus).ToList();
                        }
                        
                        if (!string.IsNullOrEmpty(sortby))
                        {
                            var propertyInfo = typeof(Status).GetProperty(sortby);
                            statslist = statslist.OrderByDescending(x => propertyInfo.GetValue(x, null)).ToList();
                        }
                        else
                        {
                            var propertyInfo = typeof(Status).GetProperty("DeviceName");
                            statslist = statslist.OrderBy(x => propertyInfo.GetValue(x, null)).ToList();
                        }
                        foreach (Status status in statslist)
                        {
                            var x = new ExpandoObject() as IDictionary<string, Object>;
                            var bson2 = status.ToBsonDocument();

                            if (bson2.Contains(docfield) && bson2.Contains("device_name"))
                            {
                                var statname = bson2["device_name"].ToString();
                                var statvalue =Convert.ToInt32(bson2[docfield]).ToString();
                                
                                if (!isChart)
                                {
                                    x.Add(statname, statvalue);
                                    result.Add(x);
                                }
                                else
                                {
                                    segment = new Segment();
                                    segment.Label = bson2["device_name"].ToString();
                                    segment.Value = Convert.ToInt32(bson2[docfield].ToDouble());
                                    segments.Add(segment);
                                }
                            }
                        }
                        if (!isChart)
                        {
                            Response = Common.CreateResponse(result);
                        }
                        else
                        {
                            Serie serie = new Serie();
                            serie.Title = "";
                            serie.Segments = segments;
                            List<Serie> series = new List<Serie>();
                            series.Add(serie);
                            Chart chart = new Chart();
                            chart.Series = series;
                            chart.Title = "";
                            Response = Common.CreateResponse(chart);
                        }
                    }
                }
                return Response;
            }
            catch (Exception exception)
            {
                Response = Common.CreateResponse(null, "Error", exception.Message);

                return Response;
            }
        }

        [HttpGet("status_count")]
        public APIResponse GetStatusCount(string type, string docfield)
        {

            statusRepository = new Repository<Status>(ConnectionString);

            try
            {
                string color = "";
                var types = type.Replace("[", "").Replace("]", "").Replace(" ", "").Split(',');
                
                Expression<Func<Status, bool>> expression = (p => types.Contains(p.DeviceType));
                var bsonDocs = statusRepository.Collection.Aggregate()
                                    .Match(expression)
                                    .Group(new BsonDocument { { "_id", "$" + docfield }, { "count", new BsonDocument("$sum", 1) } }).ToList();
                List<Segment> result = new List<Segment>();
                foreach (BsonDocument doc in bsonDocs)
                {
                    if (!doc["_id"].IsBsonNull)
                    {
                        if (doc["_id"].ToString() == "Not Responding" || doc["_id"].ToString() == "Red" || doc["_id"].ToString() == "Fail")
                        {
                            color = "rgba(239, 58, 36, 1)";
                        }
                        else if (doc["_id"].ToString() == "OK" || doc["_id"].ToString() == "Green")
                        {
                            color = "rgba(95, 190, 127, 1)";
                        }
                        else if (doc["_id"].ToString() == "Issue" || doc["_id"].ToString() == "Yellow")
                        {
                            //color = "rgba(255, 195, 0, 1)";
                            color = "rgba(249, 156, 28, 1)";
                        }
                        else if (doc["_id"].ToString() == "Maintenance")
                        {
                            color = "rgba(119 , 119, 119, 1)";
                        }
                        Segment segment = new Segment()
                        {
                            //Might have to add additional types for support.  Format is IfThis ? DoThis : Else
                            Label = doc["_id"].ToString(),
                            Value = doc["count"].AsInt32,
                            Color = color
                        };
                        result.Add(segment);
                    }
                }
                

                result.RemoveAll(item => item.Label == null);
                result.RemoveAll(item => item.Label == "");
                Serie serie = new Serie();
                serie.Title = docfield;
                serie.Segments = result;


                List<Serie> series = new List<Serie>();
                series.Add(serie);

                Chart chart = new Chart();
                
                chart.Title = docfield;
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

        [HttpGet("server_count")]
        public APIResponse GetServerCount(string type, string docfield)
        {

            serverRepository = new Repository<Server>(ConnectionString);

            try
            {
                string color = "";
                var types = type.Replace("[", "").Replace("]", "").Replace(" ", "").Split(',');

                Expression<Func<Server, bool>> expression = (p => types.Contains(p.DeviceType));
                var bsonDocs = serverRepository.Collection.Aggregate()
                                    .Match(expression)
                                    .Unwind(new MongoDB.Driver.StringFieldDefinition<Server>(docfield))
                                    .Group(new BsonDocument { { "_id", "$" + docfield }, { "count", new BsonDocument("$sum", 1) } }).ToList();
                List<Segment> result = new List<Segment>();
                foreach (BsonDocument doc in bsonDocs)
                {
                    if (!doc["_id"].IsBsonNull)
                    {
                        if (doc["_id"].ToString() == "Not Responding" || doc["_id"].ToString() == "Red" || doc["_id"].ToString() == "Fail")
                        {
                            color = "rgba(239, 58, 36, 1)";
                        }
                        else if (doc["_id"].ToString() == "OK" || doc["_id"].ToString() == "Green")
                        {
                            color = "rgba(95, 190, 127, 1)";
                        }
                        else if (doc["_id"].ToString() == "Issue" || doc["_id"].ToString() == "Yellow")
                        {
                            //color = "rgba(255, 195, 0, 1)";
                            color = "rgba(249, 156, 28, 1)";
                        }
                        else if (doc["_id"].ToString() == "Maintenance")
                        {
                            color = "rgba(119 , 119, 119, 1)";
                        }
                        Segment segment = new Segment()
                        {
                            //Might have to add additional types for support.  Format is IfThis ? DoThis : Else
                            Label = doc["_id"].ToString(),
                            Value = doc["count"].AsInt32,
                            Color = color
                        };
                        result.Add(segment);
                    }
                }
                result.RemoveAll(item => item.Label == null);
                result.RemoveAll(item => item.Label == "");
                Serie serie = new Serie();
                serie.Title = docfield;
                serie.Segments = result;
                List<Serie> series = new List<Serie>();
                series.Add(serie);
                Chart chart = new Chart();
                chart.Title = docfield;
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

        [HttpGet("disk_space")]
        public APIResponse GetDiskSpace(string deviceId = "",string ismonitored ="")
        {
            FilterDefinition<Status> filterDefStatus;
            List<dynamic> result = new List<dynamic>();
            List<DiskStatus> disks = new List<DiskStatus>();
            //statusRepository = new Repository<Status>(ConnectionString);
            //serverRepository = new Repository<Server>(ConnectionString);
            //var servers = serverRepository.Find(x => true).ToList();
            try
            {
                statusRepository = new Repository<Status>(ConnectionString);
                serverRepository = new Repository<Server>(ConnectionString);
                var servers = serverRepository.Find(x => true).ToList();
   
                if (!string.IsNullOrEmpty(deviceId))
                {
                    List<string> listofdevices = deviceId.Replace("[", "").Replace("]", "").Replace(" ", "").Split(',').ToList();
                    filterDefStatus = statusRepository.Filter.And(statusRepository.Filter.Exists(x => x.Disks, true),
                        statusRepository.Filter.In(x => x.DeviceId, listofdevices));
                }
                else
                {
                    filterDefStatus = statusRepository.Filter.Exists(x => x.Disks, true);
                }
                var result1 = statusRepository.Find(filterDefStatus).AsQueryable().OrderBy(x => x.DeviceName).ToList();
                List<Serie> diskserie = new List<Serie>();
                Serie diskFreeSerie = new Serie();
                diskFreeSerie.Title = "Available";
                Serie diskUsedSerie = new Serie();
                diskUsedSerie.Title = "Used";

                List<Segment> diskfreesegments = new List<Segment>();
                List<Segment> diskusedsegments = new List<Segment>();
                   foreach (Status status in result1)
                    {

                        disks = status.Disks;
                        disks.RemoveAll(item => item.DiskFree == null || item.DiskFree == 0.0);
                        disks.RemoveAll(item => item.DiskSize - item.DiskFree == null || item.DiskFree == 0.0);
                        if (ismonitored == "true" && (servers.Where(x => x.Id == status.DeviceId).Count() > 0))
                        {
                            Server server = servers.Where(x => x.Id == status.DeviceId).First();
                            if (server.DiskInfo != null)
                            {
                                List<DiskSetting> diskSetting = server.DiskInfo;
                                List<String> diskNames = diskSetting.Select(x => x.DiskName.ToLower()).ToList();
                                disks = disks.Where(x => diskNames.Contains(x.DiskName.ToLower()) || diskNames.Contains("AllDisks")).ToList();
                            }
                        }
                        var data = disks.Select(x => new
                        {
                            Name = x.DiskName,
                            Free = x.DiskFree,
                            Used = x.DiskSize - x.DiskFree
                        });

                        //diskFreeSerie.Segments.Add(data.Select(x => new Segment { Label = status.DeviceName + " - " + x.Name, Value = x.Free.Value, Color = "rgba(95, 190, 127, 1)" }).ToList());
                        foreach (var item in data)
                        {
                            diskfreesegments.Add(new Segment()
                            {
                              
                                Label = string.IsNullOrEmpty(deviceId) ? status.DeviceName + " - " + item.Name : item.Name,
                                Value = item.Free.Value,
                                Color = "rgba(95, 190, 127, 1)"
                            });
                            diskusedsegments.Add(new Segment()
                            {
                             
                                Label = string.IsNullOrEmpty(deviceId) ? status.DeviceName + " - " + item.Name : item.Name,
                                Value = item.Used.Value,
                                Color = "rgba(239, 58, 36, 1)"
                            });
                        }

                        //diskUsedSerie.Segments = data.Select(x => new Segment { Label = status.DeviceName + " - " + x.Name, Value = x.Used.Value, Color = "rgba(239, 58, 36, 1)" }).ToList();

                    }
              
                diskFreeSerie.Segments = diskfreesegments;
                diskUsedSerie.Segments = diskusedsegments;
                diskserie.Add(diskFreeSerie);
                diskserie.Add(diskUsedSerie);
                Chart chart = new Chart();

                chart.Title = "Disk Space";
                chart.Series = diskserie;
                Response = Common.CreateResponse(chart);
                return Response;
            }

            catch (Exception exception)
            {
                Response = Common.CreateResponse(null, "Error", exception.Message);

                return Response;
            }
        }

        [HttpGet("server_list_selectlist_data")]
        public APIResponse GetDeviceListDropDownData()
        {
            try
            {
                statusRepository = new Repository<Status>(ConnectionString);
                var deviceTypeData = statusRepository.All().Where(x=>x.DeviceType!=null).Select(x => x.DeviceType).Distinct().OrderBy(x=>x).ToList();
                var deviceStatusData = statusRepository.All().Where(x => x.StatusCode != null).Select(x => x.StatusCode).Distinct().OrderBy(x => x).ToList();
                var deviceLocationData = statusRepository.All().Where(x => x.Location != null).Select(x => x.Location).Distinct().OrderBy(x => x).ToList();
                deviceTypeData.Insert(0, "-All-");
                deviceStatusData.Insert(0, "-All-");
                deviceLocationData.Insert(0, "-All-");

                Response = Common.CreateResponse(new { deviceTypeData = deviceTypeData, deviceStatusData = deviceStatusData , deviceLocationData = deviceLocationData });
                return Response;
            }
            catch (Exception exception)
            {
                Response = Common.CreateResponse(null, "Error", exception.Message);

                return Response;
            }
        }

        [HttpGet("get-name-values")]
        public APIResponse GetNameVlaues(string category, string name)
        {
            try
            {

                nameValueRepository = new Repository<NameValue>(ConnectionString);
                if (string.IsNullOrEmpty(category) && string.IsNullOrEmpty(name))
                {
                    var result = nameValueRepository.All().ToList();
                    Response = Common.CreateResponse(result);

    }
                else if (!string.IsNullOrEmpty(category))
                {
                    Expression<Func<NameValue, bool>> expression = (p => p.Category == category);
                    var result = nameValueRepository.Find(expression)
                        .Select(x => new NameValueModel

                        { Name = x.Name, Id = x.Id, Category = x.Category, Value = x.Value }).ToList();
                    Response = Common.CreateResponse(result);
}
                else if (!string.IsNullOrEmpty(name))
                {
                    var names = name.Replace("[", "").Replace("]", "").Split(',');
                    Expression<Func<NameValue, bool>> expression = (p => names.Contains(p.Name));
                    var result = nameValueRepository.Find(expression).Select(x => new NameValueModel

                    { Name = x.Name, Id = x.Id, Category = x.Category, Value = x.Value }).ToList();
                    Response = Common.CreateResponse(result);

                }

               


            }
            catch (Exception exception)
            {
                Response = Common.CreateResponse(null, "Error", exception.Message);

                return Response;
            }

            return Response;
        }

        [HttpGet("server_list_dropdown")]
        public APIResponse GetServersDropDown(string type = "")
        {

            try
            {
               serverRepository = new Repository<Server>(ConnectionString);
                Func<Server, bool> expression;
                if (type != "")
                {
                    expression = x => x.DeviceType == type;
                }
                else
                {
                    expression = x => true;    
                }
                //var deviceNameData = serverRepository.All().Where(x => x.DeviceType == type).Select(x => x.DeviceName).Distinct().OrderBy(x => x).ToList();
                var result = serverRepository.All().Where(expression).Select(x => new NameValueModel

                { Name = x.DeviceName, Id = x.Id }).ToList().OrderBy(x => x.Name).ToList();
                Response = Common.CreateResponse(result);

                result.Insert(0, new NameValueModel { Name="All", Id="" });

                Response = Common.CreateResponse(new { deviceNameData = result });
                return Response;
            }
            catch (Exception exception)
            {
                Response = Common.CreateResponse(null, "Error", exception.Message);

                return Response;
            }
        }

        [HttpGet("stats_list_dropdown")]
        public APIResponse GetStatsDropDown(string type,string statType)
        {

            try
            {
                summaryRepository = new Repository<SummaryStatistics>(ConnectionString);
                var filterDef = summaryRepository.Filter.And(summaryRepository.Filter.Eq(p => p.DeviceType, type),
                    summaryRepository.Filter.Regex(p => p.StatName, new BsonRegularExpression("/" + statType +"/i")));
                var result = summaryRepository.Collection.Distinct(x => x.StatName, filterDef).ToList();
                Response = Common.CreateResponse(result);
                return Response;
            }
            catch (Exception exception)
            {
                Response = Common.CreateResponse(null, "Error", exception.Message);

                return Response;
            }
        }


        [HttpGet("get_name_value")]
        public APIResponse GetNameValueAPI(string name)
        { 
            try
            {
                var nameValue = Common.GetNameValue(name);
                Response = Common.CreateResponse(new NameValueModel() { Name = nameValue.Name, Value = nameValue.Value });

            }
            catch (Exception exception)
            {
                Response = Common.CreateResponse(null, "Error", exception.Message);

            }
            return Response;
        }

        [HttpPut("set_name_value")]
        public APIResponse SaveNameValueAPI([FromBody]NameValueModel nameValue)
        {
            try
            {
                Common.SaveNameValue(new NameValue() { Name = nameValue.Name, Value = nameValue.Value });
                Response = Common.CreateResponse(true, Common.ResponseStatus.Success.ToDescription(), "Value updated successfully");
                
            }
            catch (Exception exception)
            {
                Response = Common.CreateResponse(null, Common.ResponseStatus.Error.ToDescription(), "Value update has failed.\n Error Message :" + exception.Message);
            }
            return Response;
        }

        #region PowerShellScriptsOnDemand

        [HttpGet("get_powershell_scripts")]
        public APIResponse GetPowerShellScripts(string deviceType = "")
        {
            try
            {
                var results = new List<PowerShellScriptModel>();
                var powershellFiles = new List<String>();
                if (String.IsNullOrWhiteSpace(deviceType))
                    powershellFiles = System.IO.Directory.EnumerateFiles(Startup.wwwrootPath, "*.*", System.IO.SearchOption.AllDirectories).ToList();
                else
                    powershellFiles = System.IO.Directory.EnumerateFiles(Startup.wwwrootPath + "\\" + deviceType, "*.*", System.IO.SearchOption.AllDirectories).ToList();

                foreach (string filePath in powershellFiles)
                {
                    string currDeviceType = filePath.Replace(Startup.wwwrootPath, "");
                    currDeviceType = currDeviceType.Substring(0, currDeviceType.IndexOf("\\")).Replace("\\", "");
                    string script = System.IO.File.ReadAllText(filePath);
                    string parameters;
                    System.Management.Automation.Language.Token[] tokens;
                    System.Management.Automation.Language.ParseError[] errors;
                    var ast = System.Management.Automation.Language.Parser.ParseInput(script, out tokens, out errors);

                    var parameterList = ast.ParamBlock != null ? ast.ParamBlock.Parameters.Select(p => p.Name.ToString().Replace("$", "")).ToList() : new List<String>();
                    string description = null;
                    try
                    {
                        System.Text.RegularExpressions.Regex regex = new System.Text.RegularExpressions.Regex("(?<=\\.DESCRIPTION\r\n).*\r\n");

                        description = regex.Match(script).Value;
                    }
                    catch(Exception ex)
                    {

                    }

                    List<string> subTypes = null;
                    try
                    {
                        System.Text.RegularExpressions.Regex regex = new System.Text.RegularExpressions.Regex("(?<=\\.SUBTYPES\r\n).*\r\n");

                        subTypes = regex.Match(script).Value.Replace("[", "").Replace("]", "").Trim().Split(',').ToList();
                    }
                    catch (Exception ex)
                    {

                    }

                    results.Add(new PowerShellScriptModel {
                        Name = filePath.Substring(filePath.LastIndexOf("\\")).Replace("\\", ""),
                        ParametersList = parameterList.Select(x => new PowerShellScriptModel.Parameters() { Name = x }).ToList(),
                        Path = filePath,
                        DeviceType = currDeviceType,
                        Description = description,
                        SubTypes = subTypes
                    });
                }
                serverRepository = new Repository<Server>(ConnectionString);
                List<ServersModel> deviceList = serverRepository.Find(serverRepository.Filter.In(x => x.DeviceType, results.Select(y => y.DeviceType))).ToList().Select(x => new ServersModel() { DeviceId = x.Id, DeviceName = x.DeviceName, DeviceType = x.DeviceType }).ToList() ;

                Response = Common.CreateResponse(new { scripts = results, devices = deviceList });
            }
            catch (Exception exception)
            {
                Response = Common.CreateResponse(null, "Error", exception.Message);

            }
            return Response;
        }
        
        [HttpPut("execute_powershell_script")]
        public APIResponse ExecutePowerShellScript([FromBody]PowerShellScriptModel obj)
        {
            System.Management.Automation.PowerShell ps = null;
            try
            {

                if (string.IsNullOrWhiteSpace(obj.Path) || (obj.ParametersList == null || obj.ParametersList.Count() == 0) || String.IsNullOrWhiteSpace(obj.DeviceId))
                    Response = Common.CreateResponse(null, "Error", "Please ensure all fields are filled out.");
                serverRepository = new Repository<Server>(ConnectionString);
                credentialsRepository = new Repository<Credentials>(ConnectionString);

                //Gets server info
                FilterDefinition<Server> filterDefServer = serverRepository.Filter.Eq(x => x.Id, obj.DeviceId);
                List<Server> listOfServers = serverRepository.Find(filterDefServer).ToList();
                if (listOfServers.Count() == 0) {
                    Response = Common.CreateResponse(null, "Error", "Could not find the server in the database.");
                    return Response;
                }
                Server server = listOfServers.First();

                //Gets credential Info
                FilterDefinition<Credentials> filterDefCredentials = credentialsRepository.Filter.Eq(x => x.Id, server.CredentialsId);
                List<Credentials> listOfCredentials = credentialsRepository.Find(filterDefCredentials).ToList();
                if (listOfServers.Count() == 0) {
                    Response = Common.CreateResponse(null, "Error", "Could not find the credentials in the database.");
                    return Response;
                }
                Credentials creds = listOfCredentials.First();

                //Calls the connect to server
                VSFramework.TripleDES tripleDes = new VSFramework.TripleDES();
                
                if (server.DeviceType == Enums.ServerType.Exchange.ToDescription().ToString())
                {
                    ps = MicrosoftConnections.ConnectToExchange(server.DeviceName, creds.UserId, tripleDes.Decrypt(creds.Password), server.IPAddress, server.AuthenticationType);
                }
                else if (server.DeviceType == Enums.ServerType.SharePoint.ToDescription().ToString())
                {
                    ps = MicrosoftConnections.ConnectToSharePoint(server.DeviceName, creds.UserId, tripleDes.Decrypt(creds.Password), server.IPAddress);
                }
                else if (server.DeviceType == Enums.ServerType.Office365.ToDescription().ToString())
                {
                    ps = MicrosoftConnections.ConnectToOffice365(server.DeviceName, creds.UserId, tripleDes.Decrypt(creds.Password), server.IPAddress);
                }
                else if (server.DeviceType == Enums.ServerType.ActiveDirectory.ToDescription().ToString())
                {
                    ps = MicrosoftConnections.ConnectToActiveDirectory(server.DeviceName, creds.UserId, tripleDes.Decrypt(creds.Password), server.IPAddress);
                }
                else
                {
                    throw new Exception("Device Type is not supported");
                }

                ps.Commands.Clear();
                ps.AddCommand(obj.Path);
                foreach (PowerShellScriptModel.Parameters parameter in obj.ParametersList)
                    ps.AddParameter(parameter.Name, parameter.Value);

                string response = "Output from PowerShell:\n";

                System.Collections.ObjectModel.Collection<System.Management.Automation.PSObject> psOutput = ps.Invoke();



                

                foreach (System.Management.Automation.PSObject psObject in psOutput)
                {
                    response += "\n\n";
                    foreach (System.Management.Automation.PSPropertyInfo psPropertyInfo in psObject.Properties)
                    {
                        response += psPropertyInfo.Name + " : " + psPropertyInfo.Value + "\n";
                    }
                }

                if (ps.Streams.Error.Count > 0)
                {
                    response += "Erros:\n";
                    foreach (System.Management.Automation.ErrorRecord error in ps.Streams.Error)
                        response += error.ToString() + "\n";
                }


                string authHeader = Request.Headers["Authorization"].First();
                JwtSecurityTokenHandler jwt = new JwtSecurityTokenHandler();
                JwtSecurityToken token = jwt.ReadJwtToken(authHeader.Substring(authHeader.IndexOf(' ')).Trim());
                string email = token.Claims.Where(x => x.Type == "email").First().Value;
                Repository<Users> usersRepo = new Repository<Users>(ConnectionString);
                Users user = usersRepo.Find(x => x.Email == email).First();


                Repository<PowerScriptsLog> powerScriptsLogRepo = new Repository<PowerScriptsLog>(ConnectionString);
                PowerScriptsLog psLogEntry = new PowerScriptsLog()
                {
                    TargetDeviceId = obj.DeviceId,
                    TargetDeviceName = server.DeviceName,
                    DeviceType = server.DeviceType,
                    Parameters = obj.ParametersList.Select(x => new NameValuePair() { Name = x.Name, Value = x.Value }).ToList(),
                    ScriptName = obj.Name,
                    ScriptPath = obj.Path,
                    Response = response,
                    UserId = user.Id,
                    UserName = user.FullName
                };
                powerScriptsLogRepo.Insert(psLogEntry);


                

                response = response.Replace("\n", "<br />");
                Response = Common.CreateResponse(response);
                

            }
            catch (Exception exception)
            {
                Response = Common.CreateResponse(null, "Error", exception.Message);

            }
            finally
            {
                MicrosoftConnections.ClosePowerShell( ref ps);
                    
            }
            return Response;
        }

        [HttpGet("get_powerscripts_audit_log")]
        public APIResponse GetPowerScriptsAuditLog()
        {
            try
            {
                Repository<PowerScriptsLog> repo = new Repository<PowerScriptsLog>(ConnectionString);
                List<PowerScriptsAuditModel> powerScriptsLogList = repo.Find(x => true).ToList()
                    .Select(x => new PowerScriptsAuditModel()
                    {
                        DeviceId = x.TargetDeviceId,
                        DeviceName = x.TargetDeviceName,
                        DeviceType = x.DeviceType,
                        ParametersList = x.Parameters.Select(y => new PowerScriptsAuditModel.Parameters() { Name = y.Name, Value = y.Value }).ToList(),
                        Response = x.Response != null ? x.Response.Replace("\n", "<br />") : x.Response,
                        ScriptName = x.ScriptName,
                        ScriptPath = x.ScriptPath,
                        UserId = x.UserId,
                        UserName = x.UserName,
                        DateTimeExecuted = x.CreatedOn.ToUniversalTime()
                    }).ToList();

                Response = Common.CreateResponse(powerScriptsLogList);
            }
            catch (Exception exception)
            {
                Response = Common.CreateResponse(null, "Error", exception.Message);
            }
            return Response;

        }

        #endregion


    }

}




