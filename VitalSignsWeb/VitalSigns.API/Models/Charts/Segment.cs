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

        [JsonProperty("value")]
        [BsonElement("value")]
        public double? Value { get; set; }

        [JsonProperty("color", NullValueHandling = NullValueHandling.Ignore)]
        [BsonElement("color")]
        public string Color { get; set; }


    }
}
