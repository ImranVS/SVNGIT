using System;
using System.Runtime.Serialization;
using VSNext.Mongo.Repository;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace VSNext.Mongo.Entities
{
    [DataContract]
    [Serializable]
    [CollectionName("daily_statistics")]
    public class DailyStatistics : Entity
    {
        
        [DataMember]
        [BsonElement("device_id")]
        public int  DeviceId { get; set; }
        [DataMember]
        [BsonElement("stat_name")]
        public string StatName { get; set; }
        [DataMember]
        [BsonElement("stat_value")]
        public double StatValue { get; set; }

    }

    [DataContract]
    [Serializable]
    [CollectionName("share_point_web_traffic_daily_statistics")]
    public class SharePointWebTrafficDailyStatistics : Entity
    {

        [DataMember]
        [BsonElement("device_id")]
        public int DeviceId { get; set; }
        [DataMember]
        [BsonElement("url_id")]
        public int? UrlId { get; set; }
        [DataMember]
        [BsonElement("user_id")]
        public int? UserId { get; set; }
        [DataMember]
        [BsonElement("stat_value")]
        public double StatValue { get; set; }

    }
}
