using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Runtime.Serialization;
using VSNext.Mongo.Repository;

namespace VSNext.Mongo.Entities
{
    [DataContract]
    [Serializable]
    [CollectionName("traveler_data_store")]
    public class TravelerDataStore : Entity
    {
        [DataMember]
        [BsonElement("traveler_service_pool_name")]
        [BsonIgnoreIfNull]
        public string TravelerServicePoolName { get; set; }

        [DataMember]
        [BsonElement("device_name")]
        [BsonIgnoreIfNull]
        public string DeviceName { get; set; }

        [DataMember]
        [BsonElement("data_store")]
        [BsonIgnoreIfNull]
        public string DataStore { get; set; }

        [DataMember]
        [BsonElement("database_name")]
        [BsonIgnoreIfNull]
        public string DatabaseName { get; set; }

        [DataMember]
        [BsonElement("port")]
        [BsonIgnoreIfNull]
        public int Port { get; set; }

        [DataMember]
        [BsonElement("user_name")]
        [BsonIgnoreIfNull]
        public string UserName { get; set; }

        [DataMember]
        [BsonElement("password")]
        [BsonIgnoreIfNull]
        public string Password { get; set; }

        [DataMember]
        [BsonElement("integrated_security")]
        [BsonIgnoreIfNull]
        public bool IntegratedSecurity { get; set; }

        [DataMember]
        [BsonElement("test_scan_server")]
        [BsonIgnoreIfNull]
        public string TestScanServer { get; set; }

        [DataMember]
        [BsonElement("used_by_servers")]
        [BsonIgnoreIfNull]
        public string UsedByServers { get; set; }
    }
}
