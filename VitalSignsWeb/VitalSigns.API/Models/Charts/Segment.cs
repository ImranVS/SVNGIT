using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VitalSigns.API.Models.Charts
{

    [BsonIgnoreExtraElements]
    public class Segment
    {

        [JsonProperty("label")]
        [BsonElement("label")]
        public string Label { get; set; }

        [JsonProperty("label2")]
        [BsonElement("label2")]
        public string Label2 { get; set; }

        [JsonProperty("value")]
        [BsonElement("value")]
        public double? Value { get; set; }

        [JsonProperty("value1")]
        [BsonElement("value1")]
        public double? Value1 { get; set; }

        [JsonProperty("value2")]
        [BsonElement("value2")]
        public double? Value2 { get; set; }

        [JsonProperty("color", NullValueHandling = NullValueHandling.Ignore)]
        [BsonElement("color")]
        public string Color { get; set; }

        [JsonProperty("drilldownname")]
        [BsonElement("drilldownname")]
        public string DrillDownName { get; set; }
    }
}
