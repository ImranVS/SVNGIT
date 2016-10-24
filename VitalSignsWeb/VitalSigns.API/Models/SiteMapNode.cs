using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VitalSigns.API.Models
{

    [BsonIgnoreExtraElements]
    public class SiteMapNode
    {

        [JsonProperty("title")]
        [BsonElement("title")]
        public string Title { get; set; }

        [JsonProperty("url")]
        [BsonElement("url"), BsonIgnoreIfNull, BsonIgnoreIfDefault]
        public string Url { get; set; }

        [JsonProperty("icon")]
        [BsonElement("icon"), BsonIgnoreIfNull, BsonIgnoreIfDefault]
        public string Icon { get; set; }

        [JsonProperty("disabled")]
        [BsonElement("disabled"), BsonIgnoreIfNull, BsonIgnoreIfDefault]
        public bool Disabled { get; set; }

        [JsonProperty("nodes")]
        [BsonElement("nodes"), BsonIgnoreIfNull, BsonIgnoreIfDefault]
        public List<SiteMapNode> Nodes { get; set; }

    }
}
