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
    [CollectionName("traveler_stats")]
    public class TravelerStats : Entity
    {

        public TravelerStats()
        {
            System.Reflection.PropertyInfo[] props = this.GetType().GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.DeclaredOnly);
            foreach (var property in props)
            {
                property.SetValue(this, null);
            }

        }

        [DataMember]
        [BsonElement("device_id")]
        [BsonRepresentation(BsonType.ObjectId)]
        [BsonIgnoreIfNullAttribute]
        public string DeviceId { get; set; }

        [DataMember]
        [BsonElement("traveler_server_name")]
        [BsonIgnoreIfNullAttribute]
        public string TravelerServerName { get; set; }

        [DataMember]
        [BsonElement("mail_server_name")]
        [BsonIgnoreIfNullAttribute]
        public string MailServerName { get; set; }

        [DataMember]
        [BsonElement("interval")]
        [BsonIgnoreIfNullAttribute]
        public string Interval { get; set; }

        [DataMember]
        [BsonElement("delta")]
        [BsonIgnoreIfNullAttribute]
        public int? Delta { get; set; }

        [DataMember]
        [BsonElement("open_times")]
        [BsonIgnoreIfNullAttribute]
        public int? OpenTimes { get; set; }

        [DataMember]
        [BsonElement("date_updated")]
        [BsonIgnoreIfNullAttribute]
        public DateTime? DateUpdated { get; set; }

    }

    [CollectionName("traveler_summarystats")]
    public class TravelerStatusSummary : Entity
    {
        // [JsonProperty("[000-001]")]
        [DataMember]
        [BsonElement("[000-001]")]
        [BsonIgnoreIfNull]
        public int? c_000_001 { get; set; }

        [DataMember]
        [BsonElement("[001-002]")]
        [BsonIgnoreIfNull]
        public int? c_001_002 { get; set; }

        [DataMember]
        [BsonElement("[002-005]")]
        [BsonIgnoreIfNull]
        public int? c_002_005 { get; set; }

        [DataMember]
        [BsonElement("[005-010]")]
        [BsonIgnoreIfNull]
        public int? c_005_010 { get; set; }

        [DataMember]
        [BsonElement("[010-030]")]
        [BsonIgnoreIfNull]
        public int? c_010_030 { get; set; }
        [DataMember]
        [BsonElement("[030-060]")]
        [BsonIgnoreIfNull]
        public int? c_030_060 { get; set; }
        [DataMember]
        [BsonElement("[060-120]")]
        [BsonIgnoreIfNull]
        public int? c_060_120 { get; set; }
        [DataMember]
        [BsonElement("[120-INF]")]
        [BsonIgnoreIfNull]
        public int? c_120_INF { get; set; }


        [DataMember]
        [BsonElement("traveler_server_name")]
        [BsonIgnoreIfNullAttribute]
        public string TravelerServerName { get; set; }

        [DataMember]
        [BsonElement("mail_server_name")]
        [BsonIgnoreIfNullAttribute]
        public string MailServerName { get; set; }

        [DataMember]
        [BsonElement("interval")]
        [BsonIgnoreIfNullAttribute]
        public string Interval { get; set; }

        [DataMember]
        [BsonElement("delta")]
        [BsonIgnoreIfNullAttribute]
        public int? Delta { get; set; }

        [DataMember]
        [BsonElement("open_times")]
        [BsonIgnoreIfNullAttribute]
        public int? OpenTimes { get; set; }

        [DataMember]
        [BsonElement("date_updated")]
        [BsonIgnoreIfNullAttribute]
        public DateTime? DateUpdated { get; set; }


        [DataMember]
        [BsonElement("device_id")]
        [BsonRepresentation(BsonType.ObjectId)]
        [BsonIgnoreIfNullAttribute]
        public string DeviceId { get; set; }


        [DataMember]
        [BsonElement("stat_name")]
        [BsonIgnoreIfNullAttribute]
        public string StatName { get; set; }

    }

}
