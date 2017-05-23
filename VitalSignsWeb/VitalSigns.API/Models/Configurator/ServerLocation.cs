using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;

namespace VitalSigns.API.Models
{
    public class ServerLocation
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("location_name")]
        public string LocationName { get; set; }

        [JsonProperty("device_name")]
        public string DeviceName { get; set; }

        [JsonProperty("current_node")]
        public string CurrentNode { get; set; }

        [JsonProperty("assigned_node")]
        public string AssignedNode { get; set; }

        [JsonProperty("device_type")]
        public string DeviceType { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("is_selected")]
        public bool IsSelected { get; set; }

        [JsonProperty("category")]
        public string Category { get; set; }

        [JsonProperty("credentials")]
        public string Credentials { get; set; }
    }
}
