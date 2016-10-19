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

namespace VitalSigns.API.Controllers
{

    [Route("services")]
    public class IBMServicesController : BaseController
    {
        private IRepository<Status> statusRepository;
        private IRepository<Server> serverRepository;

        [HttpGet("websphere_devices")]
        public APIResponse GetWebsphereDevices(string parentid, string devicename, string devicetype)
        {
            Expression<Func<Status, bool>> expression2;
            statusRepository = new Repository<Status>(ConnectionString);
            serverRepository = new Repository<Server>(ConnectionString);
            List<dynamic> result = new List<dynamic>();
            List<WebSphereNode> nodes = new List<WebSphereNode>();
            List<WebSphereServer> servers = new List<WebSphereServer>();

            try
            {
                Expression<Func<Server, bool>> expression = (p => p.DeviceType == "WebSphereCell" && p.DeviceName == devicename);
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
                                foreach (WebSphereServer server in servers)
                                {
                                    expression2 = (p => p.DeviceType == devicetype && p.DeviceName == server.ServerName);
                                    var statuslist = statusRepository.Find(expression2).AsQueryable().OrderBy(x => x.DeviceName).ToList();
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
                        else
                        {
                            expression2 = (p => p.DeviceType == devicetype && p.DeviceName == node.NodeName);
                            var statuslist = statusRepository.Find(expression2).AsQueryable().OrderBy(x => x.DeviceName).ToList();
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
