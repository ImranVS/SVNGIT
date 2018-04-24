using System;
using System.Runtime.Serialization;
using VSNext.Mongo.Repository;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace VSNext.Mongo.Entities
{
    [DataContract]
    [Serializable]
    [CollectionName("daily_task")]
    public class DailyTasks:Entity
    {
        [DataMember]
        [BsonElement("aggregation_type")]
        public string AggregationType { get; set; }

        [DataMember]
        [BsonElement("device_type")]
        [BsonIgnoreIfNull]
        public string DeviceType { get; set; }

        [DataMember]
        [BsonElement("statName")]
        public string StatName { get; set; }

    }
}
