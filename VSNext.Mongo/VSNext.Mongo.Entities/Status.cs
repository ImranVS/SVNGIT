using System;
using System.Runtime.Serialization;
using VSNext.Mongo.Repository;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace VSNext.Mongo.Entities
{

    [DataContract]
    [Serializable]
    [CollectionName("status")]
    public class Status: Entity
    {
        [DataMember]
        [BsonElement("type")]
        public string Type { get; set; }

        [DataMember]
        [BsonElement("secondary_role")]
        public string SecondaryRole { get; set; }

        [DataMember]
        [BsonElement("name")]
        public string Name { get; set; }
        //"last_update" : "2015-08-05T08:40:51.620Z",
        [DataMember]
        [BsonElement("next_Scan")]
        public DateTime NextScan { get; set; }

        [DataMember]
        [BsonElement("location")]
        public string Location { get; set; }

        [DataMember]
        [BsonElement("category")]
        public string Category { get; set; }

        [DataMember]
        [BsonElement("status")]
        public string CurrentStatus { get; set; }

        [DataMember]
        [BsonElement("status_code")]
        public string StatusCode { get; set; }

        [DataMember]
        [BsonElement("details")]
        public string Details { get; set; }

        [DataMember]
        [BsonElement("description")]
        public string Description { get; set; }

        [DataMember]
        [BsonElement("pending_mail")]
        public int? PendingMail { get; set; }

        [DataMember]
        [BsonElement("dead_mail")]
        public int? DeadMail { get; set; }

        [DataMember]
        [BsonElement("mail_details")]
        public string MailDetails { get; set; }

        [DataMember]
        [BsonElement("up_count")]
        public int? UpCount { get; set; }

        [DataMember]
        [BsonElement("down_count")]
        public int? DownCount { get; set; }

        [DataMember]
        [BsonElement("up_percent")]
        public int? UpPercent { get; set; }

        [DataMember]
        [BsonElement("response_time")]
        public int? ResponseTime { get; set; }

        [DataMember]
        [BsonElement("response_threshold")]
        public int? ResponseThreshold { get; set; }

        [DataMember]
        [BsonElement("pending_threshold")]
        public int? PendingThreshold { get; set; }

        [DataMember]
        [BsonElement("held_threshold")]
        public int? HeldThreshold { get; set; }


        [DataMember]
        [BsonElement("dead_threshold")]
        public int? DeadThreshold { get; set; }

        [DataMember]
        [BsonElement("user_count")]
        public int? UserCount { get; set; }

        [DataMember]
        [BsonElement("domino_server_tasks")]
        public string DominoServerTasks { get; set; }

        [DataMember]
        [BsonElement("type_and_name")]
        public string TypeAndName { get; set; }

        [DataMember]
        [BsonElement("operating_system")]
        public string OperatingSystem { get; set; }
        [DataMember]
        [BsonElement("version")]
        public string version { get; set; }

        [DataMember]
        [BsonElement("cpu")]
        public double CPU { get; set; }

        [DataMember]
        [BsonElement("Cpu_threshold")]
        public double CPUthreshold { get; set; }

        [DataMember]
        [BsonElement("memory")]
        public double Memory { get; set; }

        [DataMember]
        [BsonElement("elapsed_days")]
        public int? ElapsedDays { get; set; }

        [DataMember]
        [BsonElement("exjournal")]
        public int? Exjournal { get; set; }

        [DataMember]
        [BsonElement("exjournal1")]
        public int? Exjournal1 { get; set; }

        [DataMember]
        [BsonElement("exjournal2")]
        public int? Exjournal2 { get; set; }

        [DataMember]
        [BsonElement("exjournal_date")]
        public DateTime ExjournalDate { get; set; }
      }


    [DataContract]
    [Serializable]
    [CollectionName("status_details")]
    public class StatusDetails :Entity
    {

        [DataMember]
        [BsonElement("device_id")]
        public int? DeviceId { get; set; }

        [DataMember]
        [BsonElement("type")]
        public string Type { get; set; }

        [DataMember]
        [BsonElement("category")]
        public string category { get; set; }

        [DataMember]
        [BsonElement("test_name")]
        public string TestName { get; set; }

        [DataMember]
        [BsonElement("result")]
        public string Result { get; set; }

        [DataMember]
        [BsonElement("details")]
        public string Details { get; set; }

    }

}
