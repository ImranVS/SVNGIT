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
        

        [JsonProperty("disk_free")]
        [BsonElement("disk_free")]
        public List<double> DiskFree { get; set; }

        [JsonProperty("disk_size")]
        [BsonElement("disk_size")]
        public List<double> DiskSize { get; set; }
    }
}
