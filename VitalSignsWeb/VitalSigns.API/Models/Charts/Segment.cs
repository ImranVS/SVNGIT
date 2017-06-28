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

        [JsonProperty("label", NullValueHandling = NullValueHandling.Ignore)]
        [BsonElement("label")]
        public string Label { get; set; }

        [JsonProperty("label2", NullValueHandling = NullValueHandling.Ignore)]
        [BsonElement("label2")]
        public string Label2 { get; set; }

        [JsonProperty("value", NullValueHandling = NullValueHandling.Ignore)]
        [BsonElement("value")]
        public double? Value { get; set; }

        [JsonProperty("value1", NullValueHandling = NullValueHandling.Ignore)]
        [BsonElement("value1")]
        public double? Value1 { get; set; }

        [JsonProperty("value2", NullValueHandling = NullValueHandling.Ignore)]
        [BsonElement("value2")]
        public double? Value2 { get; set; }

        [JsonProperty("color", NullValueHandling = NullValueHandling.Ignore)]
        [BsonElement("color")]
        public string Color { get; set; }

        [JsonProperty("drilldownname", NullValueHandling = NullValueHandling.Ignore)]
        [BsonElement("drilldownname")]
        public string DrillDownName { get; set; }
    }
}
