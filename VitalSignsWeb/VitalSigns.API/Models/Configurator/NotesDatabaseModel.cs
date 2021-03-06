﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace VitalSigns.API.Models
{
    public class NotesDatabaseModel
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("domino_server_name")]
        public string DominoServerName { get; set; }

        [JsonProperty("database_file_name")]
        public string DatabaseFileName { get; set; }
        

         [JsonProperty("is_enabled")]
        public Boolean? IsEnabled { get; set; }


        [JsonProperty("scan_interval")]
        public int? ScanInterval { get; set; }

        [JsonProperty("retry_interval")]
        public int? RetryInterval { get; set; }

        [JsonProperty("off_hours_scan_interval")]
        public int? OffHoursScanInterval { get; set; }

        [JsonProperty("category")]
        public string Category { get; set; }

        [JsonProperty("trigger_type")]
        public string TriggerType { get; set; }

        [JsonProperty("trigger_value")]
        public int? TriggerValue { get; set; }
        
        [JsonProperty("initiate_replication")]
        public bool? InitiateReplication { get; set; }

        [JsonProperty("replication_destination")]
        public List<string> ReplicationDestination { get; set; }

       [JsonProperty("domino_type")]
        public string DominoType { get; set; }


        [JsonProperty("domino_server_id")]
        public string DominoServerId { get; set; }

    }
}
