using System;
using System.Runtime.Serialization;
using VSNext.Mongo.Repository;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace VSNext.Mongo.Entities
{
    [DataContract]
    [Serializable]
    [CollectionName("outages")]
    public class Outages : Entity 
    {
        [DataMember]
        [BsonElement("device_name")]
        public string DeviceName { get; set; }
        [DataMember]
        [BsonElement("device_type")]
        public string DeviceType { get; set; }
        [DataMember]
        [BsonElement("date_time_down")]
        public DateTime DateTimeDown { get; set; }
        [DataMember]
        [BsonElement("date_time_up")]
        public DateTime? DateTimeUp { get; set; }
        [DataMember]
        [BsonElement("description")]
        public string Description { get; set; }

    }
}
