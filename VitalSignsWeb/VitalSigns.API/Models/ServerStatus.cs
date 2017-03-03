using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using MongoDB.Bson;

namespace VitalSigns.API.Models
{
    public class ServerStatus
    {

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("device_id")]
        public string DeviceId { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("status_code")]
        public string StatusCode { get; set; }

        [JsonProperty("icon")]
        public string Icon { get; set; }

        [JsonProperty("country")]
        public string Country { get; set; }

        [JsonProperty("version")]
        public string Version { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }


        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("details")]
        public string Details { get; set; }

        [JsonProperty("cpu")]
        public double? CPU { get; set; }

        [JsonProperty("user_count")]
        public int? UserCount { get; set; }

        [JsonProperty("last_updated")]
        public DateTime? LastUpdated { get; set; }

        [JsonProperty("next_scan")]
        public DateTime? NextScan { get; set; }

        [JsonProperty("secondary_role")]
        public string SecondaryRole { get; set; }

        [JsonProperty("location")]
        public string Location { get; set; }

        [JsonProperty("is_enabled")]
        public bool? IsEnabled { get; set; }

        [JsonProperty("failure_threshold")]
        public int? FailureThreshold { get; set; }

        [JsonProperty("tabs")]
        public List<Tab> Tabs { get; set; }

    }
}
