using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace VitalSigns.API.Models
{
    public class TravelerHealth
    {

        [JsonProperty("device_id")]
        public string DeviceId { get; set; }

        [JsonProperty("device_name")]
        public string DeviceName { get; set; }

        [JsonProperty("traveler_status")]
        public string TravelerStatus { get; set; }

        [JsonProperty("resource_constraint")]
        public string ResourceConstraint { get; set; }

        [JsonProperty("traveler_details")]
        public string TravelerDetails { get; set; }

        [JsonProperty("traveler_heartbeat")]
        public string TravelerHeartBeat { get; set; }

        [JsonProperty("traveler_servlet")]
        public string TravelerServlet { get; set; }


        [JsonProperty("traveler_users")]
        public int? TravelerUsers { get; set; }

        [JsonProperty("traveler_ha")]
        public Boolean? TravelerHA { get; set; }

        [JsonProperty("traveler_incremental_syncs")]
        public int? TravelerIncrementalSyncs { get; set; }


        [JsonProperty("http_status")]
        public string HttpStatus { get; set; }

        [JsonProperty("traveler_devices_api_status")]
        public string TravelerDevicesAPIStatus { get; set; }

        [JsonProperty("mail_server_name")]
        public string MailServerName { get; set; }

        [JsonProperty("traveler_server_name")]
        public string traveler_server_name { get; set; }

        [JsonProperty("date_updated")]
        public string DateUpdated { get; set; }

        [JsonProperty("[000-001]")]
        public int? c_000_001 { get; set; }

        [JsonProperty("[001-002]")]
        public int? c_001_002 { get; set; }

        [JsonProperty("[002-005]")]
        public int? c_002_005 { get; set; }

        [JsonProperty("[005-010]")]
        public int? c_005_010 { get; set; }

        [JsonProperty("[010-030]")]
        public int? c_010_030 { get; set; }

        [JsonProperty("[030-060]")]
        public int? c_030_060 { get; set; }

        [JsonProperty("[060-120]")]
        public int? c_060_120 { get; set; }

        [JsonProperty("[120-INF]")]
        public int? c_120_INF { get; set; }

     
    }
}

