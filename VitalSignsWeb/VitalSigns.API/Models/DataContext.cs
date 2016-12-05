using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VitalSigns.API.Models
{
    public class DataContext
    {
        public const string DAILY_STATISTICS_COLLECTION_NAME = "daily_statistics";
        public const string PROFILES_COLLECTION_NAME = "users";
        public const string SITEMAP_COLLECTION_NAME = "sitemap";

        // TODO: put this into an IoC container
        private static readonly IMongoClient _client;
        private static readonly IMongoDatabase _database;

        static DataContext()
        {
            _client = new MongoClient(Startup.ConnectionString);
            _database = _client.GetDatabase(Startup.DataBaseName);
        }

        public IMongoClient Client
        {
            get { return _client; }
        }

        public IMongoDatabase Database
        {
            get { return _database; }
        }

        public IMongoCollection<BsonDocument> DailyStatistics
        {
            get { return _database.GetCollection<BsonDocument>(DAILY_STATISTICS_COLLECTION_NAME); }
        }

        public IMongoCollection<Profile> Profiles
        {
            get { return _database.GetCollection<Profile>(PROFILES_COLLECTION_NAME); }
        }

        public IMongoCollection<SiteMap> SiteMaps
        {
            get { return _database.GetCollection<SiteMap>(SITEMAP_COLLECTION_NAME); }
        }
    }
}
