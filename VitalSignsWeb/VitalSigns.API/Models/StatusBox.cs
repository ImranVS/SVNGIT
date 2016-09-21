using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;


namespace VitalSigns.API.Models
{
    ///<Author>Sowmya Pathuri</Author>
    /// <summary>
    /// Status Model
    /// </summary>
    public class StatusBox
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("last_scan")]
        public DateTime? LastScan { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("icon")]
        public string Icon { get; set; }

        [JsonProperty("device_id")]
        public string DeviceId { get; set; }

        [JsonProperty("device_name")]
        public string DeviceName { get; set; }

        [JsonProperty("cluster_name")]
        public string ClusterName { get; set; }

        [JsonProperty("cluster_seconds_on_queue")]
        public int? ClusterSecondsOnQueue { get; set; }

        [JsonProperty("cluster_seconds_on_queue_average")]
        public double? ClusterSecondsOnQueueAverage { get; set; }

        [JsonProperty("cluster_work_queue_depth")]
        public int? ClusterWorkQueueDepth { get; set; }

        [JsonProperty("cluster_work_queue_depth_average")]
        public double? ClusterWorkQueueDepthAverage { get; set; }

        [JsonProperty("cluster_availability")]
        public double? ClusterAvailability { get; set; }

        [JsonProperty("cluster_availability_threshold")]
        public double? ClusterAvailabilityThreshold { get; set; }

        [JsonProperty("last_updated")]
        public string LastUpdated { get; set; }

        [JsonProperty("cluster_analysis")]
        public string ClusterAnalysis { get; set; }


    }
}
