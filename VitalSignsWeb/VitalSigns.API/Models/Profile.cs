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

        [JsonProperty("email")]
        [BsonId]
        public string Email { get; set; }

        [JsonProperty("name")]
        [BsonElement("name")]
        public string Name { get; set; }
        
    }
}