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
    [CollectionName("windows_events")]
    public class WindowsEvents : Entity
    {

        [DataMember]
        [BsonElement("name")]
        [BsonIgnoreIfNull]
        public string Name { get; set; }

       

        //Windows Log File Scanning

        [DataMember]
        [BsonElement("event_keywords")]
        [BsonIgnoreIfNull]
        public List<EventKeyword> EventKeywords { get; set; }

        [DataMember]
        [BsonElement("device_ids")]
        [BsonIgnoreIfNullAttribute]
        [BsonRepresentation(BsonType.ObjectId)]
        public List<string> DeviceIds { get; set; }


        public class EventKeyword
        {
            [DataMember]
            [BsonIgnoreIfNull]
            [BsonElement("alias_name")]
            public String AliasName { get; set; }

            [DataMember]
            [BsonIgnoreIfNull]
            [BsonElement("message")]
            public String Message { get; set; }

            [DataMember]
            [BsonIgnoreIfNull]
            [BsonElement("event_id")]
            public int? EventId { get; set; }

            [DataMember]
            [BsonIgnoreIfNull]
            [BsonElement("source")]
            public String Source { get; set; }

            [DataMember]
            [BsonIgnoreIfNull]
            [BsonElement("event_name")]
            public String EventName { get; set; }

            [DataMember]
            [BsonIgnoreIfNull]
            [BsonElement("event_level")]
            public String EventLevel { get; set; }
        }
    }


    [DataContract]
    [Serializable]
    [CollectionName("windows_events_history")]
    public class WindowsEventsHistory : Entity
    {

        [DataMember]
        [BsonElement("alias_name")]
        [BsonIgnoreIfNull]
        public string AliasName { get; set; }

        [DataMember]
        [BsonElement("log_name")]
        [BsonIgnoreIfNull]
        public string LogName { get; set; }

        [DataMember]
        [BsonElement("index")]
        [BsonIgnoreIfNull]
        public string Index { get; set; }

        [DataMember]
        [BsonElement("event_time")]
        [BsonIgnoreIfNull]
        public DateTime? EventTime { get; set; }

        [DataMember]
        [BsonElement("entry_type")]
        [BsonIgnoreIfNull]
        public string EntryType { get; set; }

        [DataMember]
        [BsonElement("source")]
        [BsonIgnoreIfNull]
        public string Source { get; set; }

        [DataMember]
        [BsonElement("instance_id")]
        [BsonIgnoreIfNull]
        public string InstanceId { get; set; }

        [DataMember]
        [BsonElement("message")]
        [BsonIgnoreIfNull]
        public string Message { get; set; }

        [DataMember]
        [BsonElement("device_id")]
        [BsonIgnoreIfNull]
        [BsonRepresentation(BsonType.ObjectId)]
        public string DeviceId { get; set; }

        [DataMember]
        [BsonElement("device_name")]
        [BsonIgnoreIfNull]
        public string DeviceName { get; set; }

        [DataMember]
        [BsonElement("windows_event_id")]
        [BsonIgnoreIfNull]
        [BsonRepresentation(BsonType.ObjectId)]
        public string WindowsEventId { get; set; }
    }
}
