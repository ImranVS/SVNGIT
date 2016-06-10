using System;
using System.Runtime.Serialization;
using VSNext.Mongo.Repository;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;

namespace VSNext.Mongo.Entities
{
    [DataContract]
    [Serializable]
    [CollectionName("server")]
    public class Server: Entity
    {
        
        [DataMember]
        [BsonElement("server_name")]     
        public string ServerName { get; set; }

        [DataMember]
        [BsonElement("server_type")]
        public string ServerType { get; set; }

        [DataMember]
        [BsonElement("location")]
        public string Location { get ; set; }

        [DataMember]
        [BsonElement("scan_interval")]
        public int ScanInterval { get; set; }

        [DataMember]
        [BsonIgnoreIfNull]
        [BsonElement("windows_services")]
        public List<WindowServices> WindowServices { get; set; }
        
    }

    public class WindowServices : Entity
    {

        [DataMember]
        [BsonElement("server_required")]
        [BsonIgnoreIfNullAttribute]
        public bool ServerRequired { get; set; }


        [DataMember]
        [BsonElement("service_name")]
        [BsonIgnoreIfNullAttribute]
        public string ServiceName { get; set; }

        [DataMember]
        [BsonElement("display_name")]
        [BsonIgnoreIfNullAttribute]
        public string DisplayName { get; set; }

        [DataMember]
        [BsonElement("startup_mode")]
        [BsonIgnoreIfNullAttribute]
        public string StartupMode { get; set; }

        [DataMember]
        [BsonElement("monitored")]
        [BsonIgnoreIfNullAttribute]
        public bool Monitored { get; set; }

        [DataMember]
        [BsonElement("status")]
        [BsonIgnoreIfNullAttribute]
        public string Status { get; set; }
    }

}
