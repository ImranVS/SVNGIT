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
}
