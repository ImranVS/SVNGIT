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
    [CollectionName("server_type")]
    public class ServerType:Entity
    {
        [DataMember]
        [BsonElement("server_type__id")]
        [BsonIgnoreIfNull]
        public int ServerTypeId { get; set; }

        [DataMember]
        [BsonElement("name")]
        [BsonIgnoreIfNull]
        public string Name { get; set; }

        [DataMember]
        [BsonElement("server_type_table")]
        [BsonIgnoreIfNull]
        public string ServerTypeTable { get; set; }

        [DataMember]
        [BsonElement("tabs")]
        [BsonIgnoreIfNull]
        public int[] Tabs { get; set; }
        
    }

    [DataContract]
    [Serializable]
    [CollectionName("tab")]
    public class Tab:Entity
    {
        [DataMember]
        [BsonElement("tab_id")]
        [BsonIgnoreIfNull]
        public int TabId { get; set; }

        [DataMember]
        [BsonElement("title")]
        [BsonIgnoreIfNull]
        public string Title { get; set; }

        [DataMember]
        [BsonElement("component")]
        [BsonIgnoreIfNull]
        public string Component { get; set; }

        [DataMember]
        [BsonElement("path")]
        [BsonIgnoreIfNull]
        public string Path { get; set; }
    }

    [DataContract]
    [Serializable]
    [CollectionName("feature")]
    public class Feature:Entity
    {
        [DataMember]
        [BsonElement("feature_id")]
        [BsonIgnoreIfNull]
        public int FeatureId { get; set; }

        [DataMember]
        [BsonElement("name")]
        [BsonIgnoreIfNull]
        public string Name { get; set; }

        [DataMember]
        [BsonElement("server_types")]
        [BsonIgnoreIfNull]
        public int[] ServerTypes { get; set; }
    }
}
