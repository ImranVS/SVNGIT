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
    [CollectionName("location")]
    public class Location : Entity
    {
        public Location()
        {
            System.Reflection.PropertyInfo[] props = this.GetType().GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.DeclaredOnly);
            foreach (var property in props)
            {
                property.SetValue(this, null);
            }

        }

        [DataMember]
        [BsonElement("location_name")]
        [BsonIgnoreIfNull]
        public string LocationName { get; set; }

        [DataMember]
        [BsonElement("city")]
        [BsonIgnoreIfNull]
        public string City { get; set; }

        [DataMember]
        [BsonElement("region")]
        [BsonIgnoreIfNull]
        public string Region { get; set; }

        [DataMember]
        [BsonElement("country")]
        [BsonIgnoreIfNull]
        public string Country { get; set; }

        [DataMember]
        [BsonElement("office_365_id")]
        [BsonIgnoreIfNull]
        public string O365Id { get; set; }
        
        [DataMember]
        [BsonElement("office_365_optimal_data_center")]
        [BsonIgnoreIfNull]
        public string OptimalDataCenter { get; set; }

        [DataMember]
        [BsonElement("office_365_active_data_center")]
        [BsonIgnoreIfNull]
        public string ActiveDataCenter { get; set; }
    }
}
