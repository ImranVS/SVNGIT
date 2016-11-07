using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VSNext.Mongo.Repository;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Runtime.Serialization;

namespace VSNext.Mongo.Entities
{
    [DataContract]
    [Serializable]
    [CollectionName("server_other")]
  public  class ServerOther : Entity
    {

        //Note Database

        [DataMember]
        [BsonElement("domino_type")]
        [BsonIgnoreIfNull]       
        public string DominoType { get; set; }

        [DataMember]
        [BsonElement("domino_server_id")]
        [BsonIgnoreIfNull]
        [BsonRepresentation(BsonType.ObjectId)]
        public string DominoServerId { get; set; }

        [DataMember]
        [BsonElement("domino_server_name")]
        [BsonIgnoreIfNull]
        public string DominoServerName { get; set; }

        [DataMember]
        [BsonElement("database_file_name")]
        [BsonIgnoreIfNull]
        public string DatabaseFileName { get; set; }

        [DataMember]
        [BsonElement("trigger_type")]
        [BsonIgnoreIfNull]
        public string TriggerType { get; set; }

        [DataMember]
        [BsonElement("trigger_value")]
        [BsonIgnoreIfNull]
        public int? TriggerValue { get; set; }

        [DataMember]
        [BsonElement("initiate_replication")]
        [BsonIgnoreIfNull]
        public bool? InitiateReplication { get; set; }

        [DataMember]
        [BsonElement("replication_destination")]
        [BsonIgnoreIfNull]
        public List<String> ReplicationDestination { get; set; }



        [DataMember]
        [BsonElement("is_enabled")]
        [BsonIgnoreIfNull]
        public Boolean? IsEnabled { get; set; }


        [DataMember]
        [BsonElement("scan_interval")]
        [BsonIgnoreIfNull]
        public int? ScanInterval { get; set; }

        [DataMember]
        [BsonElement("retry_interval")]
        [BsonIgnoreIfNull]
        public int? RetryInterval { get; set; }

        [DataMember]
        [BsonElement("off_hours_scan_interval")]
        [BsonIgnoreIfNull]
        public int? OffHoursScanInterval { get; set; }

    }
}
