using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace VitalSigns.API.Models
{

    public class ServerDiskStatus
    {
        public ServerDiskStatus()
        {
            Drives = new List<DiskDriveStatus>();
        }
       public string Id { get; set; }
        [JsonProperty("server_name")]
        public string Name { get; set; }
        [JsonProperty("drives")]
        public List<DiskDriveStatus> Drives { get; set; }
    }
    public class DiskDriveStatus
    {
        [JsonProperty("device_name")]
        public string DeviceName { get; set; }

        [JsonProperty("disk_name")]
        public string DiskName { get; set; }

        [JsonProperty("disk_size")]
        public double? DiskSize { get; set; }

        [JsonProperty("disk_free")]
        public double? DiskFree { get; set; }

        [JsonProperty("disk_used")]
        public double? DiskUsed { get; set; }

        [JsonProperty("percent_free")]
        public double? PercentFree { get; set; }

        [JsonProperty("IsMarkedForMonitor")]
        public bool? IsMarkedForMonitor { get; set; }

        [JsonProperty("threshold")]
        public double? Threshold { get; set; }

        [JsonProperty("threshold_type")]
        public string ThresholdType { get; set; }

        [JsonProperty("last_updated")]
        public DateTime? LastUpdated { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("location")]
        public string Location { get; set; }

        [JsonProperty("avg_daily_growth")]
        public double? AvgDailyGrowth { get; set; }
    }
}
