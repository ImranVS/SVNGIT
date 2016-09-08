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
    [CollectionName("notes_mail_probe_history")]
    public class NotesMailProbeHistory : Entity
    {

        [DataMember]
        [BsonElement("sent_date_time")]
        [BsonIgnoreIfNull]
        public DateTime? SentDateTime { get; set; }

        [DataMember]
        [BsonElement("sent_to")]
        [BsonIgnoreIfNull]
        public string SentTo { get; set; }

        [DataMember]
        [BsonElement("delivery_threshold_minutes")]
        [BsonIgnoreIfNull]
        public double? DeliveryThresholdMinutes { get; set; }

        [DataMember]
        [BsonElement("delivery_time_minutes")]
        [BsonIgnoreIfNull]
        public double? DeliveryTimeMinutes { get; set; }

        [DataMember]
        [BsonElement("subject_key")]
        [BsonIgnoreIfNull]
        public string SubjectKey { get; set; }

        [DataMember]
        [BsonElement("arrival_at_mailbox")]
        [BsonIgnoreIfNull]
        public DateTime? ArrivalAtMailbox { get; set; }

        [DataMember]
        [BsonElement("status")]
        [BsonIgnoreIfNull]
        public string Status { get; set; }

        [DataMember]
        [BsonElement("details")]
        [BsonIgnoreIfNull]
        public string Details { get; set; }

        [DataMember]
        [BsonElement("device_id")]
        [BsonIgnoreIfNull]
        [BsonRepresentation(BsonType.ObjectId)]
        public string DeviceID { get; set; }

        [DataMember]
        [BsonElement("device_name")]
        [BsonIgnoreIfNull]
        public string DeviceName { get; set; }

        [DataMember]
        [BsonElement("target_server")]
        [BsonIgnoreIfNull]
        public string TargetServer { get; set; }

        [DataMember]
        [BsonElement("target_database")]
        [BsonIgnoreIfNull]
        public string TargetDatabase { get; set; }


    }

}