﻿using System;
using System.Runtime.Serialization;
using VSNext.Mongo.Repository;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace VSNext.Mongo.Entities
{
    [DataContract]
    [Serializable]
    [CollectionName("summary_statistics")]
    public class SummaryStatistics : Entity
    {

        [DataMember]
        [BsonElement("device_id")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string DeviceId { get; set; }

        [DataMember]
        [BsonElement("stat_name")]
        public string StatName { get; set; }

        [DataMember]
        [BsonElement("stat_value")]
        public double StatValue { get; set; }

        [DataMember]
        [BsonElement("device_name")]
        public string DeviceName { get; set; }

    }
}