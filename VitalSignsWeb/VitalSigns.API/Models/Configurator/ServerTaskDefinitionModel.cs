using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace VitalSigns.API.Models
{
    public class ServerTaskDefinitionModel
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("task_name")]
        public string TaskName { get; set; }

        [JsonProperty("load_string")]
        public string LoadString { get; set; }

        [JsonProperty("console_string")]
        public string ConsoleString { get; set; }

        [JsonProperty("freeze_detect")]
        public bool FreezeDetect { get; set; }

        [JsonProperty("idle_string")]
        public string IdleString { get; set; }

        [JsonProperty("max_busy_time")]
        public int MaxBusyTime { get; set; }

        [JsonProperty("retry_count")]
        public int RetryCount { get; set; }
    }
}
