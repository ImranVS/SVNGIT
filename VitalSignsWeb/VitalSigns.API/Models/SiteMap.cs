using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VitalSigns.API.Models
{

    [BsonIgnoreExtraElements]
    public class SiteMap
    {

        [JsonProperty("id")]
        [BsonId]
        public string Id { get; set; }

        [JsonProperty("title")]
        [BsonElement("title")]
        public string Title { get; set; }
        
        [JsonProperty("nodes")]
        [BsonElement("nodes"), BsonIgnoreIfNull, BsonIgnoreIfDefault]
        public List<SiteMapNode> Nodes { get; set; }

    }
}
