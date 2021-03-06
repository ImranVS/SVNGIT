﻿using System;
using System.Runtime.Serialization;
using VSNext.Mongo.Repository;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;

namespace VSNext.Mongo.Entities
{

    [DataContract]
    [Serializable]
    [CollectionName("disk_health")]
    public class DiskHealth :Entity
    {
        [DataMember]
        [BsonElement("device_name")]
        public string DeviceName { get; set; }

        [DataMember]
        [BsonElement("device_type")]
        public string DeviceType { get; set; }

        [DataMember]
        [BsonElement("drives")]
        public List<Drive> Drives { get; set; }

       
    }
    [DataContract]
    [Serializable]
    [CollectionName("disk_status")]
    public class Drive : Entity
    {
        [DataMember]
        [BsonElement("disk_name")]
        public string DiskName { get; set; }

        [DataMember]
        [BsonElement("disk_size")]
        public double? DiskSize { get; set; }

        [DataMember]
        [BsonElement("disk_free")]
        public double? DiskFree
        {
            get; set;
        }
        [DataMember]
        [BsonElement("percent_free")]
        public Double? PercentFree { get; set; }

        [DataMember]
        [BsonElement("is_marked_for_monitor")]
        public bool? IsMarkedForMonitor { get; set; }

        [DataMember]
        [BsonElement("threshold")]
        public double? Threshold
        {
            get; set;
        }
        [DataMember]
        [BsonElement("unit")]
        public string Unit { get; set; }

        [DataMember]
        [BsonElement("last_updated")]
        public DateTime? LastUpdated { get; set; }

        [DataMember]
        [BsonElement("status")]
        public string Status { get; set; }
    }
}
