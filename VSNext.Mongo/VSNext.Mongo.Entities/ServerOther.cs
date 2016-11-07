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
        [DataMember]
        [BsonElement("name")]
        [BsonIgnoreIfNull]
        public string Name { get; set; }

        [DataMember]
        [BsonElement("domino_type")]
        [BsonIgnoreIfNull]
        public string DominoType { get; set; }

        [DataMember]
        [BsonElement("scan_interval")]
        [BsonIgnoreIfNull]
        public int? ScanInterval { get; set; }

      

        [DataMember]
        [BsonElement("off_hours_scan_interval")]
        [BsonIgnoreIfNull]
        public int? OffHoursScanInterval { get; set; }


        [DataMember]
        [BsonElement("is_enabled")]
        [BsonIgnoreIfNull]
        public Boolean? IsEnabled { get; set; }

        //Note Database
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
        [BsonElement("retry_interval")]
        [BsonIgnoreIfNull]
        public int? RetryInterval { get; set; }






        //Note replica      


        [DataMember]
        [BsonElement("category")]
        [BsonIgnoreIfNull]
        public string Category { get; set; }

        [DataMember]
        [BsonElement("difference_threshold")]
        [BsonIgnoreIfNull]
        public int? DifferenceThreshold { get; set; }

        [DataMember]
        [BsonElement("domino_server_a")]
        [BsonIgnoreIfNull]
        public string DominoServerA { get; set; }

        [DataMember]
        [BsonElement("domino_server_a_file_mask")]
        [BsonIgnoreIfNull]
        public string DominoServerAFileMask { get; set; }

        [DataMember]
        [BsonElement("domino_server_a_exclude_folders")]
        [BsonIgnoreIfNull]
        public string DominoServerAExcludeFolders { get; set; }

        [DataMember]
        [BsonElement("domino_server_b")]
        [BsonIgnoreIfNull]
        public string DominoServerB { get; set; }

        [DataMember]
        [BsonElement("domino_server_b_file_mask")]
        [BsonIgnoreIfNull]
        public string DominoServerBFileMask { get; set; }

        [DataMember]
        [BsonElement("domino_server_b_exclude_folders")]
        [BsonIgnoreIfNull]
        public string DominoServerBExcludeFolders { get; set; }

        [DataMember]
        [BsonElement("domino_server_c")]
        [BsonIgnoreIfNull]
        public string DominoServerC { get; set; }

        [DataMember]
        [BsonElement("domino_server_c_file_mask")]
        [BsonIgnoreIfNull]
        public string DominoServerCFileMask { get; set; }

        [DataMember]
        [BsonElement("domino_server_c_exclude_folders")]
        [BsonIgnoreIfNull]
        public string DominoServerCExcludeFolders { get; set; }

        [DataMember]
        [BsonElement("first_alert_threshold")]
        [BsonIgnoreIfNull]
        public int? FirstAlertThreshold { get; set; }

        //Custom stats


        [DataMember]
        [BsonIgnoreIfNull]
        [BsonElement("stat_name")]
        public String StatName { get; set; }

        [DataMember]
        [BsonIgnoreIfNull]
        [BsonElement("yellow_threshold")]
        public String ThresholdValue { get; set; }

        [DataMember]
        [BsonIgnoreIfNull]
        [BsonElement("greater_than_or_less_than")]
        public String GreaterThanOrLessThan { get; set; }

        [DataMember]
        [BsonIgnoreIfNull]
        [BsonElement("times_in_a_row")]
        public int? TimesInARow { get; set; }

        [DataMember]
        [BsonIgnoreIfNull]
        [BsonElement("console_command")]
        public String ConsoleCommand { get; set; }

        [DataMember]
        [BsonIgnoreIfNull]
        [BsonElement("domino_servers")]
        public List<String> DominoServers { get; set; }

    }
}
