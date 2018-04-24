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
    [CollectionName("events_master")]

    public class EventsReport : Entity
    {
        [DataMember]
        [BsonElement("report_title")]
        [BsonIgnoreIfNullAttribute]
        public string ReportTitle { get; set; }

        [DataMember]
        [BsonElement("report_type")]
        [BsonIgnoreIfNullAttribute]
        public string ReportType { get; set; }

    }

}
