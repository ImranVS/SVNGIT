using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VitalSigns.API.Models.Configurator
{
    public class LogFileScanning
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("device_name")]
        public string DeviceName { get; set; }

        [JsonProperty("log_file")]
        public List<LogFile> LogFile { get; set; }

        [JsonProperty("log_servers")]
        public List<string> LogServers { get; set; }

    }
    public class LogFile
    {
          
         
        [JsonProperty("keyword")]
        public string Keyword { get; set; }          
         
        [JsonProperty("exclude")]
        public string Exclude { get; set; }
     
         
        [JsonProperty("one_alert_per_day")]
        public Boolean? OneAlertPerDay { get; set; }        
         
        [JsonProperty("scan_log")]
        public Boolean? ScanLog { get; set; }      
         
        [JsonProperty("scan_agent_log")]
        public Boolean? ScanAgentLog { get; set; }

      
        [JsonProperty("event_id")]
        public string EventId { get; set; }
    }
}
