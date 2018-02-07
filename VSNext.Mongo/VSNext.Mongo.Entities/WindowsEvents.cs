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
            public int EventId { get; set; }

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

            [DataMember]
            [BsonIgnoreIfNull]
            [BsonElement("task_category")]
            public String TaskCategory { get; set; }
        }
    }
}
