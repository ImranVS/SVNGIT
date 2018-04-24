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
    [CollectionName("valid_location")]
    public class ValidLocation:Entity
    {
        [DataMember]
        [BsonElement("country")]
        [BsonIgnoreIfNull]
        public string Country { get; set; }

        [DataMember]
        [BsonElement("states")]
        [BsonIgnoreIfNull]
        public List<string> States { get; set; }

    }
}
