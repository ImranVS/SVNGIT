using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace VitalSigns.API.Models
{

    public class WebsphereThresholdList
    {
        [JsonProperty("device_name")]
        public string ServerName { get; set; }

        [JsonProperty("response_time")]
        public int? ResponseTime { get; set; }

        [JsonProperty("process_cpu")]
        public int? CPU { get; set; }

        [JsonProperty("active_thread_count")]
        public int? ActiveThreadCount { get; set; }

        [JsonProperty("hung_thread_count")]
        public int? HungThreadCount { get; set; }

        [JsonProperty("memory_used")]
        public int? MemoryUsed { get; set; }

        [JsonProperty("heap_current")]
        public int? HeapCurrent { get; set; }

        [JsonProperty("maximum_heap")]
        public int? HeapMaximum { get; set; }

        [JsonProperty("average_thread_pool")]
        public int? AverageThreadPoolCount { get; set; }

        [JsonProperty("scan_interval")]
        public int? ScanInterval { get; set; }

    }

}
