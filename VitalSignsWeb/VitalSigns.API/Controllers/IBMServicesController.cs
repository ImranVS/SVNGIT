using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using VitalSigns.API.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using VitalSigns.API.Models.Charts;
using MongoDB.Bson.Serialization;
using VSNext.Mongo.Repository;
using VSNext.Mongo.Entities;
using System.Globalization;
using System.Dynamic;
using Microsoft.AspNet.Authorization;

namespace VitalSigns.API.Controllers
{
    [Authorize("Bearer")]
    [Route("services")]
    public class IBMServicesController : BaseController
    {
        private IRepository<Status> statusRepository;
        private IRepository<Server> serverRepository;
        private IRepository<DailyStatistics> dailyRepository;

        [HttpGet("websphere_devices")]
        public APIResponse GetWebsphereDevices(string parentid, string devicetype)
        {
            Expression<Func<Status, bool>> expressionStatus;
            statusRepository = new Repository<Status>(ConnectionString);
            serverRepository = new Repository<Server>(ConnectionString);
            dailyRepository = new Repository<DailyStatistics>(ConnectionString);
            List<dynamic> result = new List<dynamic>();
            List<WebSphereNode> nodes = new List<WebSphereNode>();
            List<WebSphereServer> servers = new List<WebSphereServer>();
            BsonDocument bsondoc = new BsonDocument();

            try
            {
                Expression<Func<Server, bool>> expression = (p => p.DeviceType == "WebSphereCell" && p.Id == parentid);
                var cells = serverRepository.Find(expression).AsQueryable().OrderBy(x => x.DeviceName).FirstOrDefault();
                if (cells.Nodes.Count > 0)
                {
                    nodes = cells.Nodes.ToList();
                    foreach (WebSphereNode node in nodes)
                    {
                        if (devicetype == "WebSphere")
                        {
                            if (node.WebSphereServers.Count > 0)
                            {
                                servers = node.WebSphereServers.ToList();
                                List<String> ids = node.WebSphereServers.Select(x => x.ServerId).ToList();
                                FilterDefinition<Status> filterdefStatus = statusRepository.Filter.And(statusRepository.Filter.Eq(x => x.DeviceType, devicetype),
                                    statusRepository.Filter.In(x => x.DeviceId, ids));
                                FilterDefinition<DailyStatistics> filterdefStats = dailyRepository.Filter.And(dailyRepository.Filter.In(x => x.DeviceId, ids),
                                    dailyRepository.Filter.Gte(x => x.CreatedOn, DateTime.Now.Date));
                                var statuslist = statusRepository.Find(filterdefStatus).AsQueryable().OrderBy(x => x.DeviceName);
                                var statsList = dailyRepository.Collection.Aggregate()
                                        .Match(filterdefStats)
                                        .Group(r => new { statName = r.StatName, deviceId = r.DeviceId }, g =>
                                            new {
                                                Key = g.Key,
                                                avgValue = g.Average(x => x.StatValue)
                                            })
                                        .Project(r => new StatsData()
                                        {
                                            DeviceId = r.Key.deviceId,
                                            StatName = r.Key.statName,
                                            StatValue = r.avgValue
                                        }).ToList();
                                foreach (Status status in statuslist)
                                {
                                    bsondoc = status.ToBsonDocument();
                                    var x = new ExpandoObject() as IDictionary<string, Object>;
                                    foreach (var field in bsondoc)
                                    {
                                        x.Add(field.Name, field.Value.ToString());
                                    }
                                    foreach (StatsData stats in statsList)
                                    {
                                        if (stats.DeviceId == status.DeviceId)
                                        {
                                            var bson2 = stats.ToBsonDocument();
                                            var statname = bson2["StatName"].ToString();
                                            var statvalue = bson2["StatValue"].ToString();
                                            x.Add(statname, statvalue);
                                        }
                                    }
                                    result.Add(x);
                                }
                            }
                        }
                        else
                        {
                            expressionStatus = (p => p.DeviceType == devicetype && p.DeviceId == node.NodeId);
                            var statuslist = statusRepository.Find(expressionStatus).AsQueryable().OrderBy(x => x.DeviceName).ToList();
                            foreach (Status status in statuslist)
                            {
                                var x = new ExpandoObject() as IDictionary<string, Object>;
                                foreach (var field in status.ToBsonDocument())
                                {
                                    x.Add(field.Name, field.Value.ToString());
                                }
                                result.Add(x);
                            }
                        }
                    }
                }
                Response = Common.CreateResponse(result);
                return Response;
            }
            catch (Exception exception)
            {
                Response = Common.CreateResponse(null, "Error", exception.Message);

                return Response;
            }
        }
        
    }
}
