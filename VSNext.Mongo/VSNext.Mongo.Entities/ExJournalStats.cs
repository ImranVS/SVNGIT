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
    [CollectionName("exjournal_stats")]
    public class ExJournalStats : Entity
    {
        [DataMember]
        [BsonElement("device_name")]
        public string DeviceName { get; set; }

        [DataMember]
        [BsonElement("exjournal_database")]
        public string ExJournalDatabase { get; set; }

        [DataMember]
        [BsonElement("delta")]
        public double? Delta { get; set; }

        [DataMember]
        [BsonElement("document_count")]
        public int? DocumentCount { get; set; }


    }
}
