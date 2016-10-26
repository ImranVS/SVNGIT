using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Runtime.Serialization;
using VSNext.Mongo.Repository;

namespace VSNext.Mongo.Entities
{
    [DataContract]
    [Serializable]
    [CollectionName("device_attributes")]
    public class DeviceAttributes : Entity
    {
        [DataMember]
        [BsonElement("attribute_name")]
        public string AttributeName { get; set; }

        [DataMember]
        [BsonElement("device_type")]
        public string DeviceType { get; set; }

        [DataMember]
        [BsonElement("unitofmeasurement")]
        public string Unitofmeasurement { get; set; }

        [DataMember]
        [BsonElement("default_value")]
        public string DefaultValue { get; set; }

        [DataMember]
        [BsonElement("field_name")]
        public string FieldName { get; set; }

        [DataMember]
        [BsonElement("category")]
        public string Category { get; set; }

        [DataMember]
        [BsonElement("type")]
        public string Type { get; set; }

    }
}
