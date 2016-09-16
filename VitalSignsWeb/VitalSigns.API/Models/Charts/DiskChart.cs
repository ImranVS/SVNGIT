using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VitalSigns.API.Models.Charts
{
    [BsonIgnoreExtraElements]
    public class DiskChart
    {
        [JsonProperty("disk_name")]
        [BsonElement("disk_name")]
        public string DiskName { get; set; }

        [JsonProperty("disk_free")]
        [BsonElement("disk_free")]
        public double DiskFree { get; set; }

        [JsonProperty("disk_size")]
        [BsonElement("disk_size")]
        public double DiskSize { get; set; }
    }
}
