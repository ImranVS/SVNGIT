using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VitalSigns.Web.Models;
using VitalSigns.Web.Redesign;

namespace VitalSigns.API.Models
{
    public class DataContext
    {
        private static string DATABASE_NAME = Startup .DatabaseName;
        public const string SITE_MAP_COLLECTION_NAME = "sitemap";
        public const string SERVER_COLLECTION_NAME = "server";
        public const string SERVER_OTHER_COLLECTION_NAME = "server_other";

        // TODO: put this into an IoC container
        private static readonly IMongoClient _client;
        private static readonly IMongoDatabase _database;

        static DataContext()
        {
            _client = new MongoClient(Startup.ConnectionString.EndsWith("/") ? Startup.ConnectionString + Startup.DatabaseName : Startup.ConnectionString + "/" + Startup.DatabaseName);
            _database = _client.GetDatabase(DATABASE_NAME);
        }

        public IMongoClient Client
        {
            get { return _client; }
        }

        public IMongoDatabase Database
        {
            get { return _database; }
        }

        public IMongoCollection<SiteMap> SiteMap
        {
            get {
                
                return _database.GetCollection<SiteMap>(SITE_MAP_COLLECTION_NAME);
            }
        }

        public List<String> ServerTypesUsed
        {
            get
            {
                if (Startup.ServerTypes != null)
                    return Startup.ServerTypes;
                List<string> serverTypes = _database.GetCollection<BsonDocument>(SERVER_COLLECTION_NAME).Find(x => true).ToList().Select(x => x["device_type"].AsString).ToList();
                serverTypes.AddRange(_database.GetCollection<BsonDocument>(SERVER_OTHER_COLLECTION_NAME).Find(x => true).ToList().Select(x => x["type"].AsString).ToList());
                return serverTypes.Distinct().ToList();
            }
        }
    }
}
