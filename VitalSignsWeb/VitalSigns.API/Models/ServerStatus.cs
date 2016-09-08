﻿using System;
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
        [JsonProperty("tabs")]
        public List<Tab> Tabs { get; set; }
    }
}
