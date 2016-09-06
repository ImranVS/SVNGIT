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
    [CollectionName("status_history")]
    public class StatusHistory : Entity
    {

        public StatusHistory()
        {
            System.Reflection.PropertyInfo[] props = this.GetType().GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.DeclaredOnly);
            foreach (var property in props)
            {
                property.SetValue(this, null);
            }

        }

        [DataMember]
        [BsonElement("type")]
        [BsonIgnoreIfNullAttribute]
        public string Type { get; set; }

        [DataMember]
        [BsonElement("name")]
        [BsonIgnoreIfNullAttribute]
        public string Name { get; set; }

        [DataMember]
        [BsonElement("old_status")]
        [BsonIgnoreIfNullAttribute]
        public string OldStatus { get; set; }

        [DataMember]
        [BsonElement("old_status_code")]
        [BsonIgnoreIfNullAttribute]
        public string OldStatusCode { get; set; }

        [DataMember]
        [BsonElement("new_status")]
        [BsonIgnoreIfNullAttribute]
        public string NewStatus { get; set; }

        [DataMember]
        [BsonElement("new_status_code")]
        [BsonIgnoreIfNullAttribute]
        public string NewStatusCode { get; set; }

        [DataMember]
        [BsonElement("date_time_status_updated")]
        [BsonIgnoreIfNullAttribute]
        public DateTime? DateTimeStatusUpdated { get; set; }
        
    }
}
