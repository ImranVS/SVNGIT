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
        public const string DATABASE_NAME = "vitalsigns_reference";
        public const string SITE_MAP_COLLECTION_NAME = "sitemap";

        // TODO: put this into an IoC container
        private static readonly IMongoClient _client;
        private static readonly IMongoDatabase _database;

        static DataContext()
        {
            _client = new MongoClient(Startup.ConnectionString);
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
            get { return _database.GetCollection<SiteMap>(SITE_MAP_COLLECTION_NAME); }
        }
    }
}
