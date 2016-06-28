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
        public SharePointWebTrafficDailyStatistics()
        {
            System.Reflection.PropertyInfo[] props = this.GetType().GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.DeclaredOnly);
            foreach (var property in props)
            {
                property.SetValue(this, null);
            }
        }

        [DataMember]
        [BsonElement("server_name")]
        public string ServerName { get; set; }
        [DataMember]
        [BsonElement("relative_url")]
        public string RelativeUrl { get; set; }
        [DataMember]
        [BsonElement("user_name")]
        public string UserName { get; set; }
        [DataMember]
        [BsonElement("stat_value")]
        public double? StatValue { get; set; }

    }
}
