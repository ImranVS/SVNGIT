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

namespace MigrateVitalSignsData.MigrateData
{
    public static class ServerData
    {
       static Repository<Location> locationRepository = new Repository<Location>(MappingHelper.MongoConnectionString);
        static Repository<BusinessHours> businessHoursRepository = new Repository<BusinessHours>(MappingHelper.MongoConnectionString);
        static Repository<Server> serverRepository = new Repository<Server>(MappingHelper.MongoConnectionString);
        static Repository<Credentials> credentialsRepository = new Repository<Credentials>(MappingHelper.MongoConnectionString);
        static List<Location> locations;
        static List<BusinessHours> businessHours;
        static List<Credentials> credentials;
        static List<DiskSetting> disksettings;

        public static void MigrateServerData()
        {
            locations = locationRepository.Collection.AsQueryable().ToList();
             businessHours = businessHoursRepository.Collection.AsQueryable().ToList();
             credentials = credentialsRepository.Collection.AsQueryable().ToList();
           //  disks = serverRepository.Collection.AsQueryable().ToList();
            //Damino Servers
            var dominoServers = new Mapper<Server>("DominoServer.json").Map();
            var disksettings = new Mapper<DiskSetting>("DiskSetings.json").Map();

            foreach (var server in dominoServers)
            {
                server.DeviceType = Enums.ServerType.Domino.ToDescription();
                BuildServerData(server);
            }
            upsertData(dominoServers);
            //if (dominoServers.Count > 0)
            //    serverRepository.Insert(dominoServers);


            //IBM WebSphere

            var websphereCell = new Mapper<Server>("WebSphereCell.json").Map();
            foreach (var server in websphereCell)
            {
                server.Id= ObjectId.GenerateNewId().ToString();
                server.DeviceType = Enums.ServerType.WebSphereCell.ToDescription();
                BuildServerData(server);
            }
           
            var websphereNode = new Mapper<Server>("WebSphereNode.json").Map();
            foreach (var server in websphereNode)
            {
                server.Id = ObjectId.GenerateNewId().ToString();
                server.DeviceType = Enums.ServerType.WebSphereNode.ToDescription();
            }
            var websphereServer = new Mapper<Server>("WebSphere.json").Map();
            foreach (var server in websphereServer)
            {
                server.Id = ObjectId.GenerateNewId().ToString();
                server.DeviceType = Enums.ServerType.WebSphere.ToDescription();
                BuildServerData(server);
            }
            foreach (Server node in websphereNode)
            {
                List<String> ListOfIds = new List<String>();
                String CellName = "";
                foreach (Server server in websphereServer.Where(i => i.DeviceName.Contains("~" + node.DeviceName + "]")).ToList())
                {
                    ListOfIds.Add(server.Id);
                    server.NodeId = node.Id;
                    CellName = server.DeviceName.Substring(server.DeviceName.IndexOf("[") + 1, server.DeviceName.IndexOf("~") - server.DeviceName.IndexOf("[") - 1);
                    server.CellId = websphereCell.FirstOrDefault(i => i.DeviceName.Equals(CellName)).Id;
                }
                node.ServerId = ListOfIds;
                node.CellId = websphereCell.FirstOrDefault(i => i.DeviceName.Equals(CellName)).Id;
            }

            foreach (Server cell in websphereCell)
            {
                List<WebSphereNode> nodes = new List<WebSphereNode>();
                foreach (Server node in websphereNode.Where(i => i.CellId == cell.Id))
                {
                    WebSphereNode wsNode = new WebSphereNode();
                    wsNode.HostName = node.IPAddress;
                    wsNode.NodeId = node.Id;
                    wsNode.NodeName = node.DeviceName;
                    List<WebSphereServer> wsServer = new List<WebSphereServer>();
                    foreach (Server server in websphereServer.Where(i => i.NodeId == node.Id))
                        wsServer.Add(new WebSphereServer() { ServerId = server.Id, ServerName = server.DeviceName });
                    wsNode.WebSphereServers = wsServer;
                    nodes.Add(wsNode);
                }
                cell.Nodes = nodes;
            }

            upsertData(websphereServer);
            upsertData(websphereCell);
            upsertData(websphereNode);


            //if (websphereServer.Count > 0)
            //    serverRepository.Insert(websphereServer);
            //if (websphereCell.Count > 0)
            //    serverRepository.Insert(websphereCell);
            //if (websphereNode.Count > 0)
            //    serverRepository.Insert(websphereNode);
          

            //IBM Connections
            var ConnectionServers = new Mapper<Server>("IBMConnections.json").Map();

            string query = "SELECT S.ServerName,IT.EnableSimulationTests,IT.ResponseThreshold,TM.Tests from [vitalsigns].[dbo].[Servers] S inner join [vitalsigns].[dbo].[IBMConnectionsTests] IT ON IT.ServerId = S.Id Inner JOIN[vitalsigns].[dbo].[TestsMaster] TM ON IT.Id = TM.Id";
            var testsTable = (MappingHelper.ExecuteQuery(query)).AsEnumerable().Select(x => new
            {
                ServerName = x.Field<string>(0).Trim(),
                EnableSimulationTests = x.Field<bool>(1),
                ResponseThreshold = x.Field<int?>(2),
                Test = x.Field<string>(3)
            }).ToList();
            foreach (var server in ConnectionServers)
            {
                server.DeviceType = Enums.ServerType.IBMConnections.ToDescription();
                var testRows = testsTable.Where(x => x.ServerName == server.DeviceName).ToList();
                List<NameValuePair> simulationTests = new List<NameValuePair>();
                List<Tests> tests = new List<Tests>();
                foreach (var row in testRows)
                {
                    if (row.EnableSimulationTests)
                    {
                        NameValuePair stest = new NameValuePair();
                        stest.Name = row.Test;
                        stest.Value = row.ResponseThreshold.ToString();
                        simulationTests.Add(stest);

                        Tests test = new Tests();
                        test.TestName = row.Test;
                        test.Threshold = row.ResponseThreshold;
                        tests.Add(test);
                    }                 

                }
                if (simulationTests.Count() > 0)
                    server.SimulationTests = simulationTests;                
                BuildServerData(server);
            }

            upsertData(ConnectionServers);

            //if (ConnectionServers.Count > 0)
            //    serverRepository.Insert(ConnectionServers);


            //sametime server
            var sameTimeServers = new Mapper<Server>("Sametime.json").Map();
            query = "SELECT ID,AliasName From [vitalsigns].[dbo].[Credentials]";
            var credentialsTable = (MappingHelper.ExecuteQuery(query)).AsEnumerable().Select(x => new
            {
                Id = x.Field<int?>(0),
                AliasName = x.Field<string>(1)
            }).ToList();
            foreach (var server in sameTimeServers)
            {
                server.DeviceType = Enums.ServerType.Sametime.ToDescription();
                BuildServerData(server);
                if (!string.IsNullOrEmpty(server.User1CredentialsId))
                {
                    int credId = Convert.ToInt32(server.User1CredentialsId);
                    var credential = credentialsTable.FirstOrDefault(x => x.Id == credId);
                    if (credential != null)
                        server.User1CredentialsId = SetCredentials(credential.AliasName);
                    else
                        server.User1CredentialsId = null;
                }
                else
                    server.User1CredentialsId = null;
                if (!string.IsNullOrEmpty(server.User2CredentialsId))
                {
                    int credId = Convert.ToInt32(server.User2CredentialsId);
                    var credential = credentialsTable.FirstOrDefault(x => x.Id == credId);
                    if (credential != null)
                        server.User2CredentialsId = SetCredentials(credential.AliasName);
                    else
                        server.User2CredentialsId = null;
                }
                else
                    server.User2CredentialsId = null;
            }

            upsertData(sameTimeServers);

            //if (sameTimeServers.Count > 0)
            //    serverRepository.Insert(sameTimeServers);

        }
        private static void BuildServerData(Server server)
        {
            var location = locations.FirstOrDefault(x => x.LocationName == server.LocationId);
            var businesshour = businessHours.FirstOrDefault(x => x.Name == server.BusinessHoursId);
            if (disksettings != null)
            {
                var diskinfo = disksettings.Where(x => x.Name == server.DeviceName).ToList();

                if (diskinfo.Count > 0)
                {
                    foreach (var disk in diskinfo)
                    {
                        disk.Name = null;
                    }
                    server.DiskInfo = diskinfo;


                }
            }
            if (location != null)
            {
                server.LocationId = location.Id;
            }
            else
            {
                server.LocationId = null;
            }
            if (businesshour != null)
            {
                server.BusinessHoursId = businesshour.Id;
            }
            else
            {
                server.BusinessHoursId = null;
            }
            var credential = credentials.FirstOrDefault(x => x.Alias == server.CredentialsId);

            if (credential != null)
            {
                server.CredentialsId = credential.Id;
            }
            else
            {
                server.CredentialsId = null;
            }
        }
        private static string SetCredentials(string credentialName)
        {
            var credential = credentials.FirstOrDefault(x => x.Alias == credentialName);

            if (credential != null)
            {
                return credential.Id;
            }
            else
            {
               return null;
            }
        }

        private static void upsertData(List<Server> servers)
        {
            foreach (var entry in servers)
                serverRepository.Replace(entry, new UpdateOptions() { IsUpsert = true }, serverRepository.Filter.Eq(x => x.DeviceName, entry.DeviceName) & serverRepository.Filter.Eq(x => x.DeviceType, entry.DeviceType));
        }
    }
}
