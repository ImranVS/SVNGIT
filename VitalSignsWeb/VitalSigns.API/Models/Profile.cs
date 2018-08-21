using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VitalSigns.API.Models
{

    [BsonIgnoreExtraElements]
    public class Profile
    {

        [JsonProperty("tenant_id")]
        [BsonElement("tenant_id")]
        public int TenantId { get; set; }
        
        [JsonProperty("email")]
        [BsonElement("email")]
        public string Email { get; set; }

        [JsonProperty("full_name")]
        [BsonElement("full_name")]
        public string FullName { get; set; }

        [JsonProperty("roles")]
        [BsonElement("roles")]
        public List<string> Roles { get; set; }

        [JsonIgnore]
        [BsonElement("hash")]
        public string Hash { get; set; }

        [JsonProperty("created_on")]
        [BsonElement("created_on")]
        public DateTime Created { get; set; }

        [JsonProperty("modified_on")]
        [BsonElement("modified_on")]
        public DateTime Modified { get; set; }

        [JsonProperty("ad_user")]
        [BsonElement("ad_user")]
        public bool Aduser { get; set; }
        
    }
}